using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Models.Web;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;

namespace BBSApi.Tests
{
    [TestFixture]
    public class WebApiTests
    {

        private static WebSite TestSite(string name)
        {
            var db = Redis.GetDatabase();


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


        //    [TestMethod]
        /* public void CreateSite()
    {
        var ctrl = new WebController();

        var resp = ctrl.CreateSite(TestSite("Test01")) as OkNegotiatedContentResult<WebSite>;

        // Assert
        Assert.IsNotNull(resp, "Returned a non OK response");
        Assert.IsNotNull(resp.Content, "Didnot return any content.");

    }

   



        [Test]
        public void Test2()
        {
           
           
            

            var db = Redis.GetDatabase();
            


            var tst1 = db.StringGet("Test01");
            db.StringSet("Test01", JsonConvert.SerializeObject(TestSite("Test01")));
            db.StringSet("Test02", JsonConvert.SerializeObject(TestSite("Test02")));
            db.StringSet("Test03", JsonConvert.SerializeObject(TestSite("Test03")));
            db.StringSet("Test04", JsonConvert.SerializeObject(TestSite("Test04")));
            var tst = JsonConvert.DeserializeObject<WebSite>(db.StringGet("Test01"));
            foreach (var o in db.SetMembers("WebSites"))
                db.SetRemove("WebSites", o);
            for (var i = 0; i < 10000; i++)
            {
                var o = TestSite($"WebSite{i:0000}");
                o.SiteId = db.HashIncrement("SiteId", 1);
                db.SetAdd("WebSites", JsonConvert.SerializeObject(o));
            }

                       var lis1 = db.SetMembers("WebSites");
            var lis2 = db.SetMembers("WebSites").Select(o => JsonConvert.DeserializeObject<WebSite>(o)).ToList();
        }
    }
    */
    }
}