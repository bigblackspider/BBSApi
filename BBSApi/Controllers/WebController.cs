using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BBSApi.Core.Models.Web;
using BBSApi.WebServer;

namespace BBSApi.Controllers
{
    [RoutePrefix("api/web")]
    public class WebController : ApiController
    {
        private IEnumerable<WebSite> _webSites => WebEngine.GetSites();

        [HttpGet]
        [Route("sites")]
        public IHttpActionResult GetSites()
        {
            try
            {
                return Ok(_webSites);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("sites/{siteId:int}")]
        public IHttpActionResult GetSite(int siteId)
        {
            try
            {
                var site = _webSites.FirstOrDefault(o => o.SiteId == siteId);
                if (site != null)
                    return Ok(site);
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Route("sites")]
        public IHttpActionResult CreateSite([FromBody] WebSite webSite)
        {
            try
            {
                if (_webSites.Any(o => o.DomainName == webSite.DomainName))
                    return Conflict();
                var site = WebEngine.CreateSite(webSite);
                var location = Request.RequestUri + "/" + site.SiteId;
                return Created(location, site);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("sites/{siteId:int}")]
        public IHttpActionResult UpdateSite(int siteId, [FromBody] WebSite webSite)
        {
            try
            {
                if (_webSites.Any(o => o.SiteId != webSite.SiteId))
                    return NotFound();
                return Ok(WebEngine.UpdateSite(siteId, webSite));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("sites/{siteId:int}")]
        public IHttpActionResult DeleteSite(int siteId)
        {
            try
            {
                if (_webSites.Any(o => o.SiteId != siteId))
                    return NotFound();
                WebEngine.DeleteSite(siteId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("sites/{siteId:int}/build/{templateName}")]
        public IHttpActionResult BuildSite(int siteId,string templateName)
        {
            try
            {
                var site = _webSites.FirstOrDefault(o => o.SiteId == siteId);
                if (site == null)
                    return NotFound();
                WebEngine.BuildSite(siteId,templateName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}