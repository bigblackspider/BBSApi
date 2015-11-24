using System;
using System.Collections.Generic;
using System.Linq;
using BBSApi.MailServer.Extenders;
using hMailServer;
using hMailDomain = hMailServer.Domain;
using hMailAccount = hMailServer.Account;
using hMailAlias = hMailServer.Alias;
using Settings = BBSApi.MailServer.Properties.Settings;

namespace BBSApi.MailServer
{
    public class MailEngine
    {
        private const string ERR_DOMAIN_MISSING = "Domain '{0}' does not exist.";
        private const string ERR_DOMAIN_EXISTS = "Domain '{0}' already exists.";
        private const string ERR_ACCOUNT_MISSING = "Account '{0}' does not exist in domain '{1}'.";
        private const string ERR_ACCOUNT_EXISTS = "Account '{0}' already exists in domain '{1}'.";
        private const string ERR_ALIAS_MISSING = "Alias '{0}' does not exist in domain '{1}'.";
        private const string ERR_ALIAS_EXISTS = "Alias '{0}' already exists in domain '{1}'.";
        private static readonly IInterfaceApplication App = new Application();

        static MailEngine()
        {
            App.Authenticate(Settings.Default.MailAdminUser, Settings.Default.MailAdminPassword);
        }

        public static IEnumerable<hMailDomain> GetDomains()
        {
            return App.Domains.ToEnumerable();
        }

        public static IEnumerable<hMailAccount> GetAccounts(string domainName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();

            //********** Check if domain exists
            if (DoesDomainExist(domainName))
                throw new Exception(string.Format(ERR_DOMAIN_MISSING, domainName));

            //********** Get Accounts 
            return
                from d in App.Domains.ToEnumerable()
                where d.Name == domainName
                from a in d.Accounts.ToEnumerable()
                select a;
        }

        public static IEnumerable<hMailAlias> GetAliases(string domainName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();

            //********** Check if domain exists
            if (DoesDomainExist(domainName))
                throw new Exception(string.Format(ERR_DOMAIN_MISSING, domainName));

            //********** Get Alaises 
            return
                from d in App.Domains.ToEnumerable()
                where d.Name == domainName
                from a in d.Aliases.ToEnumerable()
                select a;
        }

        public static bool DoesDomainExist(string domainName)
        {
            try
            {
                //********** Init
                domainName = domainName.Trim().ToLower();

                //********** Test
                if (App.Domains.Count > 0)
                    return App.Domains.ItemByName[domainName] != null;
            }
            catch (Exception)
            {
                // ignored
            }
            return false;
        }

        public static hMailDomain CreateDomain(string domainName)
        {
            try
            {
                //********** Init
                domainName = domainName.Trim().ToLower();

                //********** Check if domain already exists
                if (DoesDomainExist(domainName))
                    throw new Exception(string.Format(ERR_DOMAIN_EXISTS, domainName));

                //********** Create domain
                var domain = App.Domains.Add();
                domain.Name = domainName;
                domain.Active = true;
                domain.Postmaster = "postmaster@" + domainName;
                domain.Save();

                //********** Create standard accounts and aliases
                CreateAccount(domainName, "admin", "Mail", "Administrator");
                CreateAccount(domainName, "postmaster", "Mail", "Postmaster");
                CreateAlias(domainName, "sales", "admin@" + domainName);
                CreateAlias(domainName, "info", "admin@" + domainName);
                CreateAlias(domainName, "abuse", "admin@" + domainName);
                CreateAccount(domainName, "noreply", "Mail", "NoReply");

                //********** Final
                return domain;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public static hMailDomain GetDomain(string domainName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();

            //********** Check if domain exists
            if (!DoesDomainExist(domainName))
                throw new Exception(string.Format(ERR_DOMAIN_MISSING, domainName));

            //********** Get Domain
            return App.Domains.ItemByName[domainName];
        }

        public static void UpdateDomain(string domainName, string changes)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();

            //********** Check if domain exists
            if (!DoesDomainExist(domainName))
                throw new Exception(string.Format(ERR_DOMAIN_MISSING, domainName));

            //********** Update Domain
            var domain = App.Domains.ItemByName[domainName];
            domain.Update(changes);
            domain.Save();
        }

        public static void DeleteDomain(string domainName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();

            //********** Check if domain exists
            if (!DoesDomainExist(domainName))
                throw new Exception(string.Format(ERR_DOMAIN_MISSING, domainName));

            //********** Delete Domain
            var domain = App.Domains.ItemByName[domainName];
            App.Domains.DeleteByDBID(domain.ID);
        }

        public static bool DoesAccountExist(string domainName, string accountName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();
            accountName = accountName.Trim().ToLower();

            return
                App.Domains.ToEnumerable()
                    .Where(d => d.Name == domainName)
                    .SelectMany(d => d.Accounts.ToEnumerable())
                    .Any(account => account.Address == accountName + "@" + domainName);
        }

        public static hMailAccount CreateAccount(string domainName, string accountName, string firstName, string lastName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();
            accountName = accountName.Trim().ToLower();

            //********** Check if account exists
            if (DoesAccountExist(domainName, accountName))
                throw new Exception(string.Format(ERR_ACCOUNT_EXISTS, domainName, accountName));

            //********** Create Account
            var domain = App.Domains.ItemByName[domainName];
            var acc = domain.Accounts.Add();
            acc.Address = accountName + "@" + domainName;
            acc.Password = "secret";
            acc.PersonFirstName = firstName;
            acc.PersonLastName = lastName;
            acc.MaxSize = 100;
            acc.Active = true;
            acc.Save();

            //********** Final
            return acc;
        }

        public static void DeleteAccount(string domainName, string accountName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();
            accountName = accountName.Trim().ToLower();

            //********** Check if account exists
            if (!DoesAccountExist(domainName, accountName))
                throw new Exception(string.Format(ERR_ACCOUNT_MISSING, domainName, accountName));

            //********** Delete account
            var addr = accountName + "@" + domainName;
            var domain = App.Domains.ItemByName[domainName];
            var account = domain.Accounts.ItemByAddress[addr];
            domain.Accounts.DeleteByDBID(account.ID);
            domain.Save();
        }

        public static bool DoesAliasExist(string domainName, string aliasName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();
            aliasName = aliasName.Trim().ToLower();
            return
                App.Domains.ToEnumerable()
                    .Where(d => d.Name == domainName)
                    .SelectMany(d => d.Aliases.ToEnumerable())
                    .Any(alias => alias.Name.ToLower() == aliasName);
        }

        public static hMailAlias CreateAlias(string domainName, string aliasName, string accountName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();
            aliasName = aliasName.Trim().ToLower();


            //********** Check if alias exists
            if (DoesAliasExist(domainName, aliasName))
                throw new Exception(string.Format(ERR_ALIAS_EXISTS, domainName, aliasName));

            //********** Create Alias
            var domain = App.Domains.ItemByName[domainName];
            var alias = domain.Aliases.Add();
            alias.Name = aliasName + "@" + domainName;
            alias.Value = accountName;
            alias.Active = true;
            alias.Save();

            //********** Final
            return alias;
        }

        public static void DeleteAlias(string domainName, string aliasName)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();
            aliasName = aliasName.Trim().ToLower();

            //********** Check if alias exists
            if (!DoesAliasExist(domainName, aliasName))
                throw new Exception(string.Format(ERR_ALIAS_MISSING, domainName, aliasName));

            //********** Delete alias
            var domain = App.Domains.ItemByName[domainName];
            var alias = domain.Aliases.ItemByName[aliasName];
            domain.Accounts.DeleteByDBID(alias.ID);
            domain.Save();
        }

        public static void ChangePassword(string domainName, string accountName, string newPassword)
        {
            //********** Init
            domainName = domainName.Trim().ToLower();
            accountName = accountName.Trim().ToLower();

            //********** Check if account exists
            if (!DoesAccountExist(domainName, accountName))
                throw new Exception(string.Format(ERR_ACCOUNT_MISSING, domainName, accountName));

            //********** Change Password
            var addr = accountName + "@" + domainName;
            var domain = App.Domains.ItemByName[domainName];
            var account = domain.Accounts.ItemByAddress[addr];
            account.Password = newPassword;
            account.Save();
        }
    }
}