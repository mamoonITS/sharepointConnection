using dumm1.auth;
using dumm1.entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace dumm1.Controllers
{
    public class SharePointController : ControllerBase
    {
        private readonly GraphService _graphService;
        private readonly AppSettings _config;

        public SharePointController(
            GraphService graphService,
            IOptions<AppSettings> config)
        {
            _graphService = graphService;
            _config = config.Value;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var items = await _graphService.GetSharePointListItems();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Failed to retrieve items",
                    Details = ex.Message,
                    SiteId = _config.SharePoint.SiteId,
                    ListId = _config.SharePoint.ListId
                });
            }
        }
    }
    }
