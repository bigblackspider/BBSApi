﻿using System;
using System.Collections.Generic;
using BBSApi.Core.Extenders;
using BBSApi.MailServer.Extenders;
using hMailServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BBSApi.MailServer.Tests
{
    [TestClass]
    public class Mailtest
    {
        [TestMethod]
        public void CreateDomain()
        {
            //********** Init
            DeleteAllDomains();

            //********** Create Domain
            var tstDomainName = "Test01.com";
            MailEngine.CreateDomain(tstDomainName);
            Assert.IsTrue(MailEngine.DoesDomainExist(tstDomainName), $"Mail domain '{tstDomainName}' create failed.");

            //********** Check Domain Default Accounts
            var tstAccountName = "admin";
            Assert.IsTrue(MailEngine.DoesAccountExist(tstDomainName, tstAccountName),
                $"Mail domain '{tstDomainName}' Account '{tstAccountName}' missing.");
            tstAccountName = "postmaster";
            Assert.IsTrue(MailEngine.DoesAccountExist(tstDomainName, tstAccountName),
                $"Mail domain '{tstDomainName}' Account '{tstAccountName}' missing.");

            //********** Check Domain Default Aliases
            var tstAliasName = "sales";
            Assert.IsTrue(MailEngine.DoesAliasExist(tstDomainName, tstAliasName),
                $"Mail domain '{tstDomainName}' Alias '{tstAliasName}' missing.");
            tstAliasName = "info";
            Assert.IsTrue(MailEngine.DoesAliasExist(tstDomainName, tstAliasName),
                $"Mail domain '{tstDomainName}' Alias '{tstAliasName}' missing.");
            tstAliasName = "abuse";
            Assert.IsTrue(MailEngine.DoesAliasExist(tstDomainName, tstAliasName),
                $"Mail domain '{tstDomainName}' Alias '{tstAliasName}' missing.");

            //********** Try create a duplicate Domain
            Domain domain = null;
            try
            {
                domain = MailEngine.CreateDomain(tstDomainName);
            }
            catch (Exception)
            {
                // ignored
            }
            Assert.IsNull(domain, "Managed to create a duplicate domain '{0}'.", tstDomainName);

            //********** Try create an invalid Domain
            tstDomainName = "xxxx";
            try
            {
                domain = MailEngine.CreateDomain(tstDomainName);
            }
            catch (Exception)
            {
                // ignored
            }
            Assert.IsNull(domain, "Managed to create a invalid domain '{0}'.", tstDomainName);
        }

        [TestMethod]
        public void UpdateDomain()
        {
            //********** Init
            DeleteAllDomains();

            //********** Create Domain
            var tstDomainName = "Test01.com";
            MailEngine.CreateDomain(tstDomainName);

            //********** Update All Details
            var json = JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                {"Active", false},
                {"Postmaster", "xxxx@" + tstDomainName},
                {"MaxSize", 1000},
                {"MaxMessageSize", 100},
                {"MaxAccountSize", 101},
                {"MaxNumberOfAccounts", 15},
                {"MaxNumberOfAliases", 16},
                {"MaxNumberOfDistributionLists", 17}
            });
            MailEngine.UpdateDomain(tstDomainName, json);

            //********** Check Details
            var domain = MailEngine.GetDomain(tstDomainName);
            Assert.IsFalse(domain.Active, $"Mail domain '{tstDomainName}' failed to update 'Active'.");
            Assert.AreEqual("xxxx@" + tstDomainName, domain.Postmaster,
                "Mail domain '{0}' failed to update 'Postmaster'.".Fmt(tstDomainName));
            Assert.AreEqual(1000, domain.MaxSize, "Mail domain '{0}' failed to update 'MaxSize'.".Fmt(tstDomainName));
            Assert.AreEqual(100, domain.MaxMessageSize,
                "Mail domain '{0}' failed to update 'MaxMessageSize'.".Fmt(tstDomainName));
            Assert.AreEqual(101, domain.MaxAccountSize,
                "Mail domain '{0}' failed to update 'MaxAccountSize'.".Fmt(tstDomainName));
            Assert.AreEqual(15, domain.MaxNumberOfAccounts,
                "Mail domain '{0}' failed to update 'MaxNumberOfAccounts'.".Fmt(tstDomainName));
            Assert.AreEqual(16, domain.MaxNumberOfAliases,
                "Mail domain '{0}' failed to update 'MMaxNumberOfAliases'.".Fmt(tstDomainName));
            Assert.AreEqual(17, domain.MaxNumberOfDistributionLists,
                "Mail domain '{0}' failed to update 'MaxNumberOfDistributionLists'.".Fmt(tstDomainName));

            //********** Update selected Details
            json = JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                {"Active", true},
                {"MaxSize", 2000}
            });
            MailEngine.UpdateDomain(tstDomainName, json);

            //********** Check Details 2
            domain = MailEngine.GetDomain(tstDomainName);
            Assert.IsTrue(domain.Active, $"Mail domain '{tstDomainName}' failed to update 'Active' (Check Details 2).");
            Assert.AreEqual("xxxx@" + tstDomainName, domain.Postmaster,
                "Mail domain '{0}' failed to update 'Postmaster' (Check Details 2).".Fmt(tstDomainName));
            Assert.AreEqual(2000, domain.MaxSize,
                "Mail domain '{0}' failed to update 'MaxSize' (Check Details 2).".Fmt(tstDomainName));
            Assert.AreEqual(100, domain.MaxMessageSize,
                "Mail domain '{0}' failed to update 'MaxMessageSize' (Check Details 2).".Fmt(tstDomainName));
            Assert.AreEqual(101, domain.MaxAccountSize,
                "Mail domain '{0}' failed to update 'MaxAccountSize' (Check Details 2).".Fmt(tstDomainName));
            Assert.AreEqual(15, domain.MaxNumberOfAccounts,
                "Mail domain '{0}' failed to update 'MaxNumberOfAccounts' (Check Details 2).".Fmt(tstDomainName));
            Assert.AreEqual(16, domain.MaxNumberOfAliases,
                "Mail domain '{0}' failed to update 'MMaxNumberOfAliases' (Check Details 2).".Fmt(tstDomainName));
            Assert.AreEqual(17, domain.MaxNumberOfDistributionLists,
                "Mail domain '{0}' failed to update 'MaxNumberOfDistributionLists' (Check Details 2).".Fmt(tstDomainName));

            //********** Try Add Invalid Update Values
            json = JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                {"Active", "ABCD"},
                {"Postmaster", 92},
                {"MaxSize", "#$%^&"},
                {"MaxMessageSize", "#$%^&"},
                {"MaxAccountSize", "#$%^&"},
                {"MaxNumberOfAccounts", "#$%^&"},
                {"MaxNumberOfAliases", "#$%^&"},
                {"MaxNumberOfDistributionLists", "#$%^&"}
            });
            var domainUpdated = false;
            try
            {
                MailEngine.UpdateDomain(tstDomainName, json);
                domainUpdated = true;
            }
            catch (Exception)
            {
                // ignored
            }
            Assert.IsFalse(domainUpdated, "Managed to update domain '{0}' with invalid values.", tstDomainName);
        }

        [TestMethod]
        public void CreateAccount()
        {
            //********** Init
            DeleteAllDomains();

            //********** Create Domain
            var tstDomainName = "Test01.com";
            MailEngine.CreateDomain(tstDomainName);

            //********** Create Account
            MailEngine.CreateAccount(tstDomainName, "testaccount01", "Test", "Account");

            //********** Check Details
        }

        private static void DeleteAllDomains()
        {
            foreach (var domain in MailEngine.GetDomains())
                MailEngine.DeleteDomain(domain.Name);
        }
    }
}