using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;
using BBSApi.Data.Extenders;
using NUnit.Framework;

namespace BBSApi.Data.Tests
{
    [TestFixture]
    public class RedisTests
    {
        private static WebSite TestSite(long siteId, string name)
        {
            var s = new WebSite
            {
                SiteId = siteId,
                DomainName = name + ".dev.bigblackspider.com",
                Description = $"Unit test website for '{name}'.",
                MailDomainName = name + ".mail.bigblackspider.com"
            };
            s.Details.Add("$SHORT-DESCRIPTION$", "Test web site.");
            s.Details.Add("$MAIN-HEADING$", $"{name} Unit Test Web Site");
            s.Details.Add("$MAIN-TEXT$", $"This is a unit test website for '<b>{name}</b>,'.");
            s.Details.Add("$ABOUT-HEADING$", $"All About {name}'");
            s.Details.Add("$ABOUT-TEXT$",
                $"Some text that describes the about section for domain '<b><i>{s.DomainName}</i></b>'.");
            return s;
        }

        [Test]
        public void WebSitesTst()
        {
            //********** Init
            var lis = new List<WebSite>();
            lis.RedisClear();

            //********** Create Test Data
            for (var i = 0; i < 100; i++)
                lis.RedisAdd(TestSite(lis.RedisNextId(), $"WebSite{i:000}"));
            var tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(100, tst.Count);

            //********** Update 
            foreach (var site in lis.Where(o => o.DomainName.Contains("5")))
                site.Status = TSiteStatus.Closed;
            lis.RedisUpdate();
            tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);


            //********** Delete 
            foreach (var site in lis.Where(o => o.Status == TSiteStatus.Closed).ToArray())
                lis.RedisRemove(site);
            tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(81, tst.Count);

            //********** Clear
            lis.RedisClear();
            tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(0, tst.Count);
        }
    }
}