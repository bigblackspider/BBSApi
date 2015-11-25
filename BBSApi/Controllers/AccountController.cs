using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BBSApi.AccountsServer;
using BBSApi.Core.Models.Customer;

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
        [Route("customers/{customerId:int}")]
        public IHttpActionResult GetAccount(int customerId)
        {
            try
            {
                var cust = GetAccounts().FirstOrDefault(o => o.CustomerId == customerId);
                if (cust != null)
                    return Ok(cust);
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("customers")]
        public IHttpActionResult CreateCustomer([FromBody] CustomerAccount customerAccount)
        {
            try
            {
                if (GetAccounts().Any(o => o.Email == customerAccount.Email))
                    return Conflict();
                var cust = AccountsEngine.CreateCustomer(customerAccount);
                var location = Request.RequestUri + "/" + cust.CustomerId;
                return Created(location, cust);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("customers/{customerId:int}")]
        public IHttpActionResult UpdateCustomer(int customerId, [FromBody] CustomerAccount customerAccount)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.UpdateCustomer(customerId, customerAccount));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("customers/{customerId:int}")]
        public IHttpActionResult DeleteCustomer(int customerId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                AccountsEngine.DeleteCustomer(customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("customers/{accountId:int}/history")]
        public IHttpActionResult GetAccountHistory(int customerId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.GetCustomerHistory(customerId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("customers/{accountId:int}/websites")]
        public IHttpActionResult GetAccountWebSites(int customerId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.GetCustomerWebSites(customerId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("customers/{customerId:int}/websites/{siteId:int}")]
        public IHttpActionResult AttachCustomerWebsite(int customerId, int siteId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.AttatchCustomerWebsite(customerId, siteId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("customers/{customerId:int}/websites/{siteId:int}")]
        public IHttpActionResult DetachCustomerWebsite(int customerId, int siteId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.DetatchCustomerWebsite(customerId, siteId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("customers/{accountId:int}/maildomains")]
        public IHttpActionResult GetAccountMailDomains(int customerId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.GetCustomerMailDomains(customerId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("customers/{customerId:int}/maildomains/{domainId:int}")]
        public IHttpActionResult AttachCustomeMailDomain(int customerId, int domainId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.AttatchCustomerMailDomain(customerId, domainId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("customers/{customerId:int}/maildomains/{domainId:int}")]
        public IHttpActionResult DetachCustomerMailDomain(int customerId, int domainId)
        {
            try
            {
                if (GetAccounts().All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.DetatchCustomerMailDomain(customerId, domainId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}