using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BBSApi.Core.Extenders;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;
using BBSApi.Data;
using BBSApi.WebServer.Properties;

namespace BBSApi.WebServer
{
    public class WebEngine
    {
        private const string ERR_MISSING_SITE = "Web site with an Id of '{0}' does not exist.";
        private static readonly IDataServer ds = new DataServerRedis();


        public static IEnumerable<WebSite> GetSites()
        {
            return ds.GetAll<WebSite>();
            ;
        }

        public static IEnumerable<WebSite> GetSites(TSiteStatus status)
        {
            return GetSites().Where(o => o.Status == status).ToList();
        }

        public static List<WebSite> GetSites(DateRange range)
        {
            return GetSites().Where(o => (o.DateCreated >= range.FromDate) && (o.DateCreated <= range.ToDate)).ToList();
        }


        public static WebSite CreateSite(WebSite webSite)
        {
            if (GetSites().Any(o => o.DomainName == webSite.DomainName))
                throw new Exception($"Web site with domain of '{webSite.DomainName}' create already exists.");
            webSite.SiteId = ds.NextId<WebSite>();
            ds.Create(webSite);
            return webSite;
        }

        public static WebSite UpdateSite(int siteId, WebSite webSite)
        {
            var site = GetSites().FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));
            site.Update(webSite);
            ds.Update(o => o.SiteId == siteId, site);
            return site;
        }

        public static void DeleteSite(int siteId)
        {
            var site = GetSites().FirstOrDefault(o => o.SiteId == siteId);
            if (site == null)
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId));
            ds.Delete(o => o.SiteId == siteId, site);
        }

        public static void BuildSite(long siteId, string templateName)
        {
            //********** Get Site Details
            var site = GetSites().FirstOrDefault(o => o.SiteId == siteId);
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
    }
}