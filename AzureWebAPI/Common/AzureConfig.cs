namespace AzureWebAPI.Common
{
    public class AzureConfig
    { 
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
        public string Scheme { get; set; }
        public string Scope { get; set; }
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
    }
}
