using System;
using System.Linq;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;
using NUnit.Framework;

namespace BBSApi.Data.Tests
{
    [TestFixture]
    public class RedisTests
    {
        private static WebSite TestSite(long siteId,string name)
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
            var ds = new DataServerRedis();

            //********** Get All
            var lis = ds.GetAll<WebSite>();

            //********** Create Test Data
            for (var i = 0; i < 100; i++)
                ds.Create(TestSite(ds.NextId<WebSite>(), $"WebSite{i:0000}"));

            //********** Get All
            lis = ds.GetAll<WebSite>();

            //********** Delete 
            var site = ds.GetAll<WebSite>().FirstOrDefault(o => o.DomainName.StartsWith("WebSite006"));
            site.Status = TSiteStatus.Closed;
            ds.Update(o => o.SiteId == site.SiteId, site);

            //********** Delete 
            site = ds.GetAll<WebSite>().FirstOrDefault(o => o.Status == TSiteStatus.Closed);
            ds.Delete(o => o.SiteId == site.SiteId, site);

            //********** Get All
            lis = ds.GetAll<WebSite>();

            //********** Delete All
            ds.DeleteAll<WebSite>();

            //********** Get All
            lis = ds.GetAll<WebSite>();



        }



       /* [Test]
        public void Test2()
        {
            var r = new RedisMemoryProvider<User>();

            // We do not touch sequence, by running example we can see that sequence will give Users new unique Id.

            // Empty data store.
            Console.WriteLine("Our User Data store should be empty.");
            Console.WriteLine("Users In \"Database\" : {0}\n", r.GetAll<User>().Count);

            // Add imaginary users.
            Console.WriteLine("Adding 30 imaginairy users.");
            for (var i = 0; i < 30; i++)
                r.Create(new User {Id = r.Next<User>(), Name = "Joachim Nordvik"});

            // We should have 30 users in data store.
            Console.WriteLine("Users In \"Database\" : {0}\n", r.GetAll<User>().Count);

            // Lets print 10 users from data store.
            Console.WriteLine("Order by Id, Take (10) and print users.");
            foreach (var u in r.GetAll<User>().OrderBy(z => z.Id).Take(10))
            {
                Console.WriteLine("ID:{0}, Name: {1}", u.Id, u.Name);

                // Lets update an entity.
                u.Name = "My new Name";
                r.Update(x => x.Id == u.Id, u);
            }

            // Lets print 20 users from data store, we already edited 10 users.
            Console.WriteLine(
                "\nOrder by Id, Take (20) and print users, we previously edited the users that we printed lets see if it worked.");
            foreach (var u in r.GetAll<User>().OrderBy(z => z.Id).Take(20))
            {
                Console.WriteLine("ID:{0}, Name: {1}", u.Id, u.Name);
            }

            // Clean up data store.
            Console.WriteLine("\nCleaning up Data Store.\n");
            foreach (var u in r.GetAll<User>())
                r.Delete(u);

            // Confirm that we no longer have any users.
            Console.WriteLine("Confirm that we no longer have User entities in Data Store.");
            Console.WriteLine("Users In \"Database\" : {0}\n\n", r.GetAll<User>().Count);

            //Do some misc additional test 
            r.Set("Dog", "Gomle");
            var dog = r.Get("Dog");
            Console.WriteLine("Key: Dog, Value: " + dog);
            r.ExpireAt("Dog", 11);
            var ttlDog = r.GetTtl("Dog");
            Console.WriteLine("Key: Dog, Expiration: " + ttlDog);

            var info = r.GetInfo();

            Console.WriteLine("INFO:");
            foreach (var x in info)
            {
                Console.WriteLine(x.Key + ": " + x.Value);
            }

            Console.WriteLine("Hit return to exit!");
            Console.Read();
        }*/

       /* [Test]
        public void WebSiteTest()
        {
            using (IDataServer ds = new DataServerRedis())
            {
                ds.Reset();
                for (var i = 0; i < 10000; i++)
                    ds.Add(TestSite($"WebSite{i:0000}"));
                var lis = ds.WebSites;
            }
        }*/
    }


    /*public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }*/
}