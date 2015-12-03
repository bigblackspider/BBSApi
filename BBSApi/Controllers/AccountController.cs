using System;
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
        [Route("customers/{customerId:int}")]
        public IHttpActionResult GetAccount(long customerId)
        {
            try
            {
                var cust = AccountsEngine.CustomerAccounts.FirstOrDefault(o => o.CustomerId == customerId);
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
                if (AccountsEngine.CustomerAccounts.Any(o => o.Email == customerAccount.Email))
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
        public IHttpActionResult UpdateCustomer(long customerId, [FromBody] CustomerAccount customerAccount)
        {
            try
            {
                if (AccountsEngine.CustomerAccounts.All(o => o.CustomerId != customerId))
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
        public IHttpActionResult DeleteCustomer(long customerId)
        {
            try
            {
                if (AccountsEngine.CustomerAccounts.All(o => o.CustomerId != customerId))
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
        public IHttpActionResult GetAccountHistory(long customerId)
        {
            try
            {
                if (AccountsEngine.CustomerAccounts.All(o => o.CustomerId != customerId))
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
        public IHttpActionResult GetAccountWebSites(long customerId)
        {
            try
            {
                if (AccountsEngine.CustomerAccounts.All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.GetCustomerWebSites(customerId).Select(id => Request.RequestUri + "/" + id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        [HttpGet]
        [Route("customers/{accountId:int}/maildomains")]
        public IHttpActionResult GetAccountMailDomains(long customerId)
        {
            try
            {
                if (AccountsEngine.CustomerAccounts.All(o => o.CustomerId != customerId))
                    return NotFound();
                return Ok(AccountsEngine.GetCustomerMailDomains(customerId).Select(id => Request.RequestUri + "/" + id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
    }
}