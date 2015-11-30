using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBSApi.Core.Models.Web;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BBSApi.Data.Tests
{
    [TestFixture]
    public class RedisTests
    {
        [Test]
        public void WebSiteTest()
        {
            using (IDataServer ds = new DataServerRedis())
            {
                ds.Reset();
                for (var i = 0; i < 10000; i++)
                    ds.Add(TestSite($"WebSite{i:0000}"));
                //ds.Commit();
            }

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
            s.Details.Add("$MAIN-TEXT$", $"This is a unit test website for '<b>{name}</b>'.");
            s.Details.Add("$ABOUT-HEADING$", $"All About {name}'");
            s.Details.Add("$ABOUT-TEXT$",
                $"Some text that describes the about section for domain '<b><i>{s.DomainName}</i></b>'.");
            return s;
        }
    }
}
