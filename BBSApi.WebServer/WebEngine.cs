using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BBSApi.Core.Extenders;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Web;
using BBSApi.Data.Extenders;
using BBSApi.WebServer.Properties;

namespace BBSApi.WebServer
{
    public class WebEngine
    {
        private const string ERR_MISSING_SITE = "Web site with an Id of '{0}' does not exist.";

        private static List<WebSite> _webSites;

        public static List<WebSite> WebSites
        {
            get
            {
                if (_webSites == null)
                {
                    _webSites = new List<WebSite>();
                    _webSites.RedisGetAll();
                }
                return _webSites;
            }
        }

        public static WebSite CreateSite(WebSite webSite)
        {
            if (WebSites.Any(o => o.DomainName == webSite.DomainName))
                throw new Exception($"Web site with domain of '{webSite.DomainName}' create already exists.");
            webSite.SiteId = WebSites.RedisNextId();
            WebSites.Add(webSite);
            return webSite;
        }

        public static WebSite UpdateSite(long siteId, WebSite webSite)
        {
            var site = WebSites.FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));
            site.Update(webSite);
            return site;
        }

        public static void DeleteSite(long siteId)
        {
            var site = WebSites.FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));
            WebSites.Remove(site);
        }

        public static void BuildSite(long siteId, string templateName)
        {
            //********** Get Site Details
            var site = WebSites.FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));

            //********** Copy Template Folder
            var src = Settings.Default.TemplateFolder + "/" + templateName;
            var target = Settings.Default.WebsiteFolder + "/" + site.DomainName;
            foreach (var dirPath in Directory.GetDirectories(src, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(src, target));
            foreach (var newPath in Directory.GetFiles(src, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(src, target), true);

            //********** Update Place Holders
            foreach (var filePath in Directory.GetFiles(target, "*.*", SearchOption.AllDirectories))
            {
                var dets = File.ReadAllText(filePath);
                dets = site.Details.Keys.Aggregate(dets, (current, key) => current.Replace(key, site.Details[key]));
                File.WriteAllText(filePath, dets);
            }
        }

        public static void Commit()
        {
           WebSites.RedisUpdate();
        }
        public static void Rollback()
        {
            _webSites = new List<WebSite>();
            _webSites.RedisGetAll();
        }
    }
}