using System;
using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;

namespace BBSApi.WebServer
{
    public class WebEngine
    {
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

        public static WebSite GetSite(int siteId)
        {
            return _WebSites.FirstOrDefault(o => o.SiteId == siteId);
        }

        public static WebSite GetSite(Guid token)
        {
            return _WebSites.FirstOrDefault(o => o.Token == token);
        }

        public static WebSite GetSite(string name)
        {
            return _WebSites.FirstOrDefault(o => o.Name == name);
        }


        public static WebSite CreateSite(string siteName, WebSite site)
        {
            if (_WebSites.Any(o => o.Name == siteName))
                throw new Exception($"Web site '{siteName}' create already exists.");
            _WebSites.Add(site);
            return site;
        }

        public static WebSite UpdateSite(Guid token, WebSite site)
        {
            var webSite = GetSite(token);
            if (webSite == null)
                throw new Exception($"Web site with token of '{token}' does not exist.");
            webSite.Change(site);
            return webSite;
        }

        public static void DeleteSite(Guid token)
        {
            if (_WebSites.Any(o => o.Token != token))
                throw new Exception($"Web site with token of '{token}' does not exist.");
            _WebSites.Remove(_WebSites.FirstOrDefault(o => o.Token == token));
        }
    }
}