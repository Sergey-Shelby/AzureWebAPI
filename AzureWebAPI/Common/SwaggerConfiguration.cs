using Microsoft.OpenApi.Models;

namespace AzureWebAPI.Common
{

    public static class SwaggerConfiguration
    {
        internal static void AddSwagger(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AzureWebAPI.API", Version = "v1" });
                c.AddSecurityDefinition(configuration.AzureActiveDirectory.Scheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri(configuration.AzureActiveDirectory.AuthorizationUrl),
                            TokenUrl = new Uri(configuration.AzureActiveDirectory.TokenUrl),
                            Scopes = configuration.AzureActiveDirectory.Scopes.ToDictionary(key => key, value => string.Empty)
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = configuration.AzureActiveDirectory.Scheme
                            },
                            Scheme = configuration.AzureActiveDirectory.Scheme,
                            Name = configuration.AzureActiveDirectory.Scheme,
                            In = ParameterLocation.Header
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        internal static void UseSwagger(this IApplicationBuilder app, AppConfiguration configuration)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Benedict.API v1");
                c.OAuthClientId(configuration.AzureActiveDirectory.ClientId);
                c.OAuthClientSecret(configuration.AzureActiveDirectory.ClientSecret);
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            }
            );
        }
    }
}
