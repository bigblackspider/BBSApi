using System;
using BBSApi.Core.Models.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BBSApi.WebServer.Tests
{
    [TestClass]
    public class WebTest
    {
        [TestMethod]
        public void CreateWebSite()
        {
            //********** Create Test Website Entry
            var site = new WebSite
            {
                DomainName = "test01.com.au",
                Description = "A test web site.",
                MailDomainName = "test01.com.au"
            };
            site.Details.Add("$MAIN-HEADING$","Website Test 1");
            site.Details.Add("$MAIN-TEXT$", "Website Test 1 text will go here.");
            site.Details.Add("$ABOUT-HEADINGS$", "About Us");
            site.Details.Add("$ABOUT-TEXT$", "All abut us and this test web site.");
            site = WebEngine.CreateSite(site);

            //********** Build Web Site 
            WebEngine.BuildSite(site.SiteId, "professional-web");
        }
    }
}
