using System;
using System.Collections.Generic;
using System.Web.Http;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Web;
using BBSApi.WebServer;

namespace BBSApi.Controllers
{
    [RoutePrefix("api/web")]
    public class WebController : ApiController
    {
        [HttpGet]
        [Route("sites")]
        public IEnumerable<WebSite> GetSites()
        {
            return WebEngine.GetSites();
        }

        [HttpPost]
        [Route("sites")]
        public IEnumerable<WebSite> GetSites([FromBody] DateRange range)
        {
            return WebEngine.GetSites(range);
        }

        [HttpPut]
        [Route("sites/{siteName}")]
        public WebSite CreateSite(string siteName, [FromBody] WebSite site)
        {
            return WebEngine.CreateSite(siteName, site);
        }

        [HttpPost]
        [Route("sites/{token}")]
        public WebSite UpdateSite(Guid token, [FromBody] WebSite site)
        {
            return WebEngine.UpdateSite(token, site);
        }

        [HttpDelete]
        [Route("sites/{token}")]
        public void DeleteSite(Guid token)
        {
            WebEngine.DeleteSite(token);
        }

        [HttpGet]
        [Route("sites/{siteName}")]
        public WebSite GetSite(string siteName)
        {
            return WebEngine.GetSite(siteName);
        }

        [HttpGet]
        [Route("sites/{status}")]
        public WebSite GetSite(int status)
        {
            return WebEngine.GetSite(status);
        }

        [HttpGet]
        [Route("sites/{token}")]
        public WebSite GetSite(Guid token)
        {
            return WebEngine.GetSite(token);
        }
    }
}