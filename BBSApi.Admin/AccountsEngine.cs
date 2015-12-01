using System;
using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Extenders;
using BBSApi.Core.Models.Customer;
using BBSApi.Core.Models.General;
using BBSApi.WebServer;

namespace BBSApi.AccountsServer
{
    public class AccountsEngine
    {
        private const string ERR_MISSING_CUSTOMER = "Customer with an Id of '{0}' does not exist.";
        private const string ERR_MISSING_SITE = "Web site with an Id of '{0}' for customer '{1}' does not exist.";
        private const string ERR_ATTATCH_SITE = "Web site with an Id of '{0}' for customer '{1}' is already attached.";
        private const string ERR_DETATCH_SITE = "Web site with an Id of '{0}' for customer '{1}' is not attached.";
        private const string ERR_MISSING_DOMAIN = "Mail domain with an Id of '{0}' for customer '{1}' does not exist.";

        private const string ERR_ATTATCH_DOMAIN =
            "Mail domain with an Id of '{0}' for customer '{1}' is already attached.";

        private const string ERR_DETATCH_DOMAIN = "Mail domain with an Id of '{0}' for customer '{1}' is not attached.";


        private static List<CustomerAccount> _Accounts { get; } = new List<CustomerAccount>();


        public static IEnumerable<CustomerAccount> GetAccounts()
        {
            return _Accounts;
        }

        public static CustomerAccount CreateCustomer(CustomerAccount customerAccount)
        {
            if (_Accounts.Any(o => o.Email == customerAccount.Email))
                throw new Exception($"Customer with a Email address of '{customerAccount.Email}' already exists.");
            customerAccount.DateCreated = DateTime.UtcNow;
            _Accounts.Add(customerAccount);
            return customerAccount;
        }

        public static CustomerAccount UpdateCustomer(int customerId, CustomerAccount customerAccount)
        {
            if (_Accounts.All(o => o.CustomerId != customerId))
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            return _Accounts.First(o => o.CustomerId == customerId).Update(customerAccount);
        }

        public static void DeleteCustomer(int customerId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            _Accounts.Remove(cust);
        }

        public static IEnumerable<History> GetCustomerHistory(int customerId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            return cust.History;
        }

        public static IEnumerable<int> GetCustomerWebSites(int customerId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            return cust.SiteIdList;
        }

        public static CustomerAccount AttatchCustomerWebsite(int customerId, int siteId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            if (WebEngine.WebSites.All(o => o.SiteId != siteId))
                throw new Exception(ERR_MISSING_SITE.Fmt(siteId, customerId));
            if (cust.SiteIdList.Contains(siteId))
                throw new Exception(ERR_ATTATCH_SITE.Fmt(siteId, customerId));
            cust.SiteIdList.Add(siteId);
            return cust;
        }

        public static CustomerAccount DetatchCustomerWebsite(int customerId, int siteId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            if (!cust.SiteIdList.Contains(siteId))
                throw new Exception(ERR_DETATCH_SITE.Fmt(siteId, customerId));
            cust.SiteIdList.Remove(siteId);
            return cust;
        }

        public static IEnumerable<int> GetCustomerMailDomains(int customerId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            return cust.DomainIdList;
        }

        public static CustomerAccount AttatchCustomerMailDomain(int customerId, int domainId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            /*if (MailServer.MailEngine.GetDomains().All(o => o.DomainId != domainId))
                throw new Exception(ERR_MISSING_DOMAIN.Fmt(domainId, customerId));*/
            if (cust.SiteIdList.Contains(domainId))
                throw new Exception(ERR_ATTATCH_DOMAIN.Fmt(domainId, customerId));
            cust.DomainIdList.Add(domainId);
            return cust;
        }

        public static CustomerAccount DetatchCustomerMailDomain(int customerId, int domainId)
        {
            var cust = _Accounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            if (!cust.SiteIdList.Contains(domainId))
                throw new Exception(ERR_DETATCH_DOMAIN.Fmt(domainId, customerId));
            cust.DomainIdList.Remove(domainId);
            return cust;
        }
    }
}