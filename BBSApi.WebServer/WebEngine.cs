using System;
using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Extenders;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;

namespace BBSApi.WebServer
{
    public class WebEngine
    {
        private const string ERR_MISSING_SITE = "Web site with an Id of '{0}' does not exist.";


        private static List<WebSite> _WebSites { get; } = new List<WebSite>();


        public static IEnumerable<WebSite> GetSites()
        {
            return _WebSites;
        }

        public static IEnumerable<WebSite> GetSites(TSiteStatus status)
        {
            return _WebSites.Where(o => o.Status == status).ToList();
        }

        public static List<WebSite> GetSites(DateRange range)
        {
            return _WebSites.Where(o => (o.DateCreated >= range.FromDate) && (o.DateCreated <= range.ToDate)).ToList();
        }

        

        public static WebSite CreateSite(WebSite webSite)
        {
            if (_WebSites.Any(o => o.DomainName == webSite.DomainName))
                throw new Exception($"Web site with domain of '{webSite.DomainName}' create already exists.");
            _WebSites.Add(webSite);
            return webSite;
        }

        public static WebSite UpdateSite(int siteId, WebSite webSite)
        {
            var site = _WebSites.FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));
            site.Update(webSite);
            return site;
        }

        public static void DeleteSite(int siteId)
        {
            var site = _WebSites.FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));
            _WebSites.Remove(site);
        }

        
    }
}