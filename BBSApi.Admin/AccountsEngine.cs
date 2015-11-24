using System;
using System.Collections.Generic;
using BBSApi.Core.Models.Customer;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Mail;
using BBSApi.Core.Models.Web;

namespace BBSApi.AdminServer.Accounts
{
    public class AccountsEngine
    {
        private static IEnumerable<CustomerAccount> _Accounts { get; } = new List<CustomerAccount>();


        public static IEnumerable<CustomerAccount> GetAccounts()
        {
            return _Accounts;
        }


        public static CustomerAccount GetCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public static CustomerAccount CreateCustomer(CustomerAccount customerAccount)
        {
            throw new NotImplementedException();
        }

        public static CustomerAccount UpdateCustomer(int customerId, CustomerAccount customerAccount)
        {
            throw new NotImplementedException();
        }

        public static void DeleteCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<History> GetCustomerHistory(int customerId)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<WebSite> GetCustomerWebSites(int customerId)
        {
            throw new NotImplementedException();
        }

        public static CustomerAccount AttachCustomerWebsite(int customerId, int siteId)
        {
            throw new NotImplementedException();
        }

        public static CustomerAccount DetachCustomerWebsite(int customerId, int siteId)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Domain> GetCustomerMailDomains(int customerId)
        {
            throw new NotImplementedException();
        }

        public static CustomerAccount AttachCustomerMailDomain(int customerId, int domainId)
        {
            throw new NotImplementedException();
        }

        public static CustomerAccount DetachCustomerMailDomain(int customerId, int domainId)
        {
            throw new NotImplementedException();
        }
    }
}