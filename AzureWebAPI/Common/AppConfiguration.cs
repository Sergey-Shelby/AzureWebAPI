namespace AzureWebAPI.Common
{
    public class AppConfiguration
    {
        public AzureSettings AzureActiveDirectory { get; set; }
        
        public MicrosoftGraphSettings MicrosoftGraph { get; set; }
    }

    public class AzureSettings
    {
        public string Instance { get; set; }
        
        public string Domain { get; set; }
        
        public string TenantId { get; set; }
        
        public string ClientId { get; set; }
        
        public string ClientSecret { get; set; }
        
        public string CallbackPath { get; set; }
        
        public string Scheme { get; set; }
        
        public string[] Scopes { get; set; }
        
        public string AuthorizationUrl { get; set; }
        
        public string TokenUrl { get; set; }
    }

    public class MicrosoftGraphSettings
    {
        public string BaseUrl { get; set; }
        
        public string[] Scopes { get; set; }
    }
}
