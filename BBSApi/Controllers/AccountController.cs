using System.Collections.Generic;
using System.Web.Http;
using BBSApi.AdminServer.Accounts;
using BBSApi.Core.Models.Customer;
using BBSApi.Core.Models.General;
using BBSApi.Core.Models.Mail;
using BBSApi.Core.Models.Web;

namespace BBSApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        [HttpGet]
        [Route("customers")]
        public IEnumerable<CustomerAccount> GetAccounts()
        {
            return AccountsEngine.GetAccounts();
        }

        [HttpGet]
        [Route("customers/{accountId}")]
        public CustomerAccount GetAccount(int customerId)
        {
            return AccountsEngine.GetCustomer(customerId);
        }

        [HttpPut]
        [Route("customers")]
        public CustomerAccount CreateCustomer([FromBody] CustomerAccount customerAccount)
        {
            return AccountsEngine.CreateCustomer(customerAccount);
        }

        [HttpPost]
        [Route("customers/{customerId}")]
        public CustomerAccount UpdateCustomer(int customerId, [FromBody] CustomerAccount customerAccount)
        {
            return AccountsEngine.UpdateCustomer(customerId, customerAccount);
        }

        [HttpDelete]
        [Route("customers/{customerId}")]
        public void DeleteCustomer(int customerId)
        {
            AccountsEngine.DeleteCustomer(customerId);
        }

        [HttpGet]
        [Route("customers/{accountId}/history")]
        public IEnumerable<History> GetAccountHistory(int customerId)
        {
            return AccountsEngine.GetCustomerHistory(customerId);
        }

        [HttpGet]
        [Route("customers/{accountId}/websites")]
        public IEnumerable<WebSite> GetAccountWebSites(int customerId)
        {
            return AccountsEngine.GetCustomerWebSites(customerId);
        }

        [HttpPut]
        [Route("customers/{customerId}/websites/{siteId}")]
        public CustomerAccount AttachCustomerWebsite(int customerId, int siteId)
        {
            return AccountsEngine.AttachCustomerWebsite(customerId, siteId);
        }

        [HttpDelete]
        [Route("customers/{customerId}/websites/{siteId}")]
        public CustomerAccount DetachCustomerWebsite(int customerId, int siteId)
        {
            return AccountsEngine.DetachCustomerWebsite(customerId, siteId);
        }

        [HttpGet]
        [Route("customers/{accountId}/maildomains")]
        public IEnumerable<Domain> GetAccountMailDomains(int customerId)
        {
            return AccountsEngine.GetCustomerMailDomains(customerId);
        }

        [HttpPut]
        [Route("customers/{customerId}/maildomains/{domainId}")]
        public CustomerAccount AttachCustomeMailDomain(int customerId, int domainId)
        {
            return AccountsEngine.AttachCustomerMailDomain(customerId, domainId);
        }

        [HttpDelete]
        [Route("customers/{customerId}/maildomains/{domainId}")]
        public CustomerAccount DetachCustomerMailDomain(int customerId, int domainId)
        {
            return AccountsEngine.DetachCustomerMailDomain(customerId, domainId);
        }
    }
}