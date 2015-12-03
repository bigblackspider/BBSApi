using System;
using System.Collections.Generic;
using System.Linq;
using BBS.RedisExtenders.Extenders;
using BBSApi.Core.Extenders;
using BBSApi.Core.Models.Customer;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Mail;
using BBSApi.Core.Models.Types;
using BBSApi.Core.Models.Web;

namespace BBSApi.AccountsServer
{
    public class AccountsEngine
    {
        private const string ERR_MISSING_CUSTOMER = "Customer with an Id of '{0}' does not exist.";
 
        private static List<CustomerAccount> _customerAccounts;

        public static List<CustomerAccount> CustomerAccounts
        {
            get
            {
                if (_customerAccounts == null)
                {
                    _customerAccounts = new List<CustomerAccount>();
                    _customerAccounts.RedisGetAll();
                }
                return _customerAccounts;
            }
        }

        public static CustomerAccount CreateCustomer(CustomerAccount customerAccount)
        {
            if (CustomerAccounts.Any(o => o.Email == customerAccount.Email))
                throw new Exception($"Customer with a Email address of '{customerAccount.Email}' already exists.");
            customerAccount.DateCreated = DateTime.UtcNow;
            CustomerAccounts.Add(customerAccount);
            return customerAccount;
        }

        public static CustomerAccount UpdateCustomer(long customerId, CustomerAccount customerAccount)
        {
            if (CustomerAccounts.All(o => o.CustomerId != customerId))
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            return CustomerAccounts.First(o => o.CustomerId == customerId).Update(customerAccount);
        }

        public static void DeleteCustomer(long customerId)
        {
            var cust = CustomerAccounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            CustomerAccounts.Remove(cust);
        }

        public static IEnumerable<History> GetCustomerHistory(long customerId)
        {
            var cust = CustomerAccounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            var history = new List<History>();
            history.RedisFind(o => o.HistoryType == THistoryType.Account && o.LinkId == customerId);
            return history;
        }

        public static IEnumerable<WebSite> GetCustomerWebSites(long customerId)
        {
            var cust = CustomerAccounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            var webSites = new List<WebSite>();
            webSites.RedisFind(o => o.CustomerId == customerId);
            return webSites;
        }

        public static IEnumerable<MailDomain> GetCustomerMailDomains(long customerId)
        {
            var cust = CustomerAccounts.FirstOrDefault(o => o.CustomerId == customerId);
            if (cust == null)
                throw new Exception(ERR_MISSING_CUSTOMER.Fmt(customerId));
            var mailDomains = new List<MailDomain>();
            mailDomains.RedisFind(o => o.CustomerId == customerId);
            return mailDomains;
        }
    }
}