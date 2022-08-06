using AzureWebAPI.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;

namespace AzureWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            var config = Configuration.GetSection(nameof(AzureConfig)).Get<AzureConfig>();

            services.AddHttpClient();

            services.AddControllers();

            services.AddResponseCompression();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection(nameof(AzureConfig)));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AllAuthenticated",
                    policy => policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).
                        RequireAuthenticatedUser());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.API", Version = "v1" });
                c.AddSecurityDefinition(config.Scheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri(config.AuthorizationUrl),
                            TokenUrl = new Uri(config.TokenUrl),
                            Scopes = { { config.Scope, string.Empty } }
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
                                Id = config.Scheme
                            },
                            Scheme = config.Scheme,
                            Name = config.Scheme,
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                    }
                });
            });

            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var config = Configuration.GetSection(nameof(AzureConfig)).Get<AzureConfig>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.API v1");
                c.OAuthClientId(config.ClientId);
                c.OAuthClientSecret(config.ClientSecret);
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder.WithOrigins(Configuration["FrontendEndpoints"].Split(',')).AllowAnyHeader()
                .AllowAnyMethod().AllowCredentials());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
