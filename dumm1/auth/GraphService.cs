using Azure.Core;
using dumm1.entity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace dumm1.auth
{
    public class GraphService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly AppSettings.SharePointConfig _spConfig;

        public GraphService(
            GraphServiceClient graphClient,
            IOptions<AppSettings> config)
        {
            _graphClient = graphClient;
            _spConfig = config.Value.SharePoint;
        }

        public async Task<List<ListItem>> GetSharePointListItems()
        {
            try
            {
                var result = await _graphClient.Sites[_spConfig.SiteId]
                    .Lists[_spConfig.ListId]
                    .Items
                    .GetAsync(requestConfig =>
                    {
                        requestConfig.QueryParameters.Expand = ["fields"];
                    });

                return result?.Value?.ToList() ?? new List<ListItem>();
            }
            catch (ServiceException ex)
            {
                throw new ApplicationException(
                    $"Failed to get items from SharePoint list {_spConfig.ListId}", ex);
            }
        }

        public async Task<ListItem> CreateListItem(Dictionary<string, object> fields)
        {
            var newItem = new ListItem
            {
                Fields = new FieldValueSet
                {
                    AdditionalData = fields
                }
            };

            return await _graphClient.Sites[_spConfig.SiteId]
                .Lists[_spConfig.ListId]
                .Items
                .PostAsync(newItem);
        }
    }
}
