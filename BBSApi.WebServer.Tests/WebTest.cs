using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;
using BBSApi.Data.Extenders;
using NUnit.Framework;

namespace BBSApi.WebServer.Tests
{
    [TestFixture]
    public class WebTest
    {
        public void WebBuildTst()
        {
        }


        private static WebSite TestSite(string name)
        {
            var s = new WebSite
            {
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

        private static void Reset()
        {
            var lis = new List<WebSite>();
            lis.RedisClear();
        }

        private static List<WebSite> DataStoreLoad()
        {
            var lis = new List<WebSite>();
            lis.RedisGetAll();
            return lis;
        }

        [Test]
        public void WebStorageTst()
        {
            //********** Init
            Reset();

            //********** Rollback
            for (var i = 0; i < 100; i++)
                WebEngine.CreateSite(TestSite($"WebSite{i:000}"));
            var lis = WebEngine.WebSites;
            var tst = DataStoreLoad();
            Assert.AreEqual(100, lis.Count);
            Assert.AreEqual(0, tst.Count);
            WebEngine.Rollback();
            lis = WebEngine.WebSites;
            Assert.AreEqual(0, lis.Count);

            //********** Commit
            for (var i = 0; i < 100; i++)
                WebEngine.CreateSite(TestSite($"WebSite{i:000}"));
            WebEngine.Commit();
            lis = WebEngine.WebSites;
            tst = DataStoreLoad();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(100, tst.Count);

            //********** Update 
            foreach (var site in lis.Where(o => o.DomainName.Contains("5")))
            {
                var updSite = new WebSite
                {
                    Status = TSiteStatus.Closed
                };
                updSite.Details.Add("$SHORT-DESCRIPTION$", "Test to update an existing parameter value.");
                updSite.Details.Add("$TESTUPDATE01$", "This is a test update 1.");
                updSite.Details.Add("$TESTUPDATE02$", "This is a test update 2.");
                updSite.Details.Add("$TESTUPDATE03$", "This is a test update 3.");
                updSite.Details.Add("$TESTUPDATE04$", "This is a test update 4.");
                updSite.Details.Add("$TESTUPDATE05$", "This is a test update 5.");
                WebEngine.UpdateSite(site.SiteId, updSite);
            }
            WebEngine.Commit();
            tst = DataStoreLoad();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(100, tst.Count);

            //********** Delete
            foreach (var site in lis.Where(o => o.Status == TSiteStatus.Closed).ToArray())
                WebEngine.DeleteSite(site.SiteId);
            tst = DataStoreLoad();
            Assert.AreEqual(81, lis.Count);
            Assert.AreEqual(100, tst.Count);
            WebEngine.Commit();
            tst = DataStoreLoad();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(81, tst.Count);

        }
    }
}