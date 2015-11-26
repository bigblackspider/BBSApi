using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BBSApi.Core.Extenders;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;
using BBSApi.WebServer.Properties;

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

        public static void BuildSite(int siteId, string templateName)
        {
            //********** Get Site Details
            var site = _WebSites.FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));

            //********** Copy Template Folder
            var src = Settings.Default.TemplateFolder + "/" + templateName;
            var target = Settings.Default.WebsiteFolder + "/" + site.DomainName;
            foreach (string dirPath in Directory.GetDirectories(src, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(src, target));
            foreach (string newPath in Directory.GetFiles(src, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(src, target), true);

            //********** Update Place Holders
            foreach (var filePath in Directory.GetFiles(target, "*.*",
                SearchOption.AllDirectories))
            {
                var dets = File.ReadAllText(filePath);
                foreach (var key in site.Details.Keys)
                {
                    dets = dets.Replace(key, site.Details[key]);
                }
                File.WriteAllText(filePath,dets);
            }
        }
        
    }
}