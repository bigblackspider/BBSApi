using System;
using System.Collections.Generic;
using System.Linq;
using BBSApi.Models.Types;
using BBSApi.Models.Web;

namespace BBSApi.WebServer
{
    public class WebEngine
    {
        private static IEnumerable<WebSite> _WebSites { get; } = new List<WebSite>();



        public static IEnumerable<WebSite> GetSites()
        {
            return _WebSites;
        }

        public static IEnumerable<WebSite> GetSites(TSiteStatus status)
        {
            var lis = new List<WebSite>();
            foreach (var o in _WebSites)
            {
                if(o.Status == status)
                    lis.Add(o);
            }
            return lis;
        }

        public static IEnumerable<WebSite> GetSites(DateTime startDate, DateTime endDate)
        {
            return _WebSites.Where(o => (o.DateCreated >= startDate) && (o.DateCreated <= endDate)).ToList();
        }

        public static WebSite GetSite(int siteId)
        {
            return _WebSites.FirstOrDefault(o => o.SiteId == siteId);
        }
        public static WebSite GetSite(Guid siteGuid)
        {
            return _WebSites.FirstOrDefault(o => o.SiteGuid == siteGuid);
        }
        public static WebSite GetSite(string name)
        {
            return _WebSites.FirstOrDefault(o => o.Name == name);
        }
    }
}