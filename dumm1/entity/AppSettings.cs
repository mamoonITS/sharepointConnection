namespace dumm1.entity
{
    public class AppSettings
    {
        public AzureAdConfig AzureAd { get; set; }
        public SharePointConfig SharePoint { get; set; }

        public class AzureAdConfig
        {
            public string Instance { get; set; }
            public string ClientId { get; set; }
            public string TenantId { get; set; }
            public string Scopes { get; set; }
        }

        public class SharePointConfig
        {
            public string SiteId { get; set; }
            public string ListId { get; set; }
        }
    }
}
