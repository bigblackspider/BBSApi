using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BBSApi.MailServer;
using BBSApi.MailServer.Extenders;
using BBSApi.Models.Mail;
using MailDomain = hMailServer.Domain;
using MailAccount = hMailServer.Account;
using MailAlias = hMailServer.Alias;

namespace BBSApi.Controllers
{
    [RoutePrefix("api/mail")]
    public class MailController : ApiController
    {
        [HttpGet]
        [Route("domains")]
        public IEnumerable<Domain> GetDomains()
        {
            return MailEngine.GetDomains().Select(mailDomain => new Domain
            {
                DomainName = mailDomain.Name,
                Active = mailDomain.Active,
                Postmaster = mailDomain.Postmaster
            });
        }

        [HttpPut]
        [Route("domains/{domainName}")]
        public Domain CreateDomain(string domainName)
        {
            var mailDomain = MailEngine.CreateDomain(domainName);
            return new Domain
            {
                DomainName = mailDomain.Name,
                Active = mailDomain.Active,
                Postmaster = mailDomain.Postmaster
            };
        }


        [HttpGet]
        [Route("domains/{domainName}")]
        public Domain GetDomain(string domainName)
        {
            var mailDomain = MailEngine.GetDomain(domainName);
            return new Domain
            {
                ID = mailDomain.ID,
                DomainName = mailDomain.Name,
                Active = mailDomain.Active,
                Postmaster = mailDomain.Postmaster
            };
        }

        [HttpGet]
        [Route("domains/{domainName}/accounts")]
        public IEnumerable<Account> GetAccounts(string domainName)
        {
            var mailDomain = MailEngine.GetDomain(domainName);
            return mailDomain.Accounts.ToEnumerable().Select(mailAccount => new Account
            {
                ID = mailAccount.ID,
                Address = mailAccount.Address,
                Active = mailAccount.Active,
                PersonFirstName = mailAccount.PersonFirstName,
                PersonLastName = mailAccount.PersonLastName,
                MaxSize = mailAccount.MaxSize
            });
        }

        [HttpGet]
        [Route("domains/{domainName}/accounts/{address}")]
        public Account GetAccount(string domainName, string address)
        {
            if (!address.Contains("@"))
                address = address + "@" + domainName;

            return (from mailAccount in MailEngine.GetDomain(domainName).Accounts.ToEnumerable()
                where mailAccount.Address == address
                select new Account
                {
                    ID = mailAccount.ID,
                    Address = mailAccount.Address,
                    Active = mailAccount.Active,
                    PersonFirstName = mailAccount.PersonFirstName,
                    PersonLastName = mailAccount.PersonLastName,
                    MaxSize = mailAccount.MaxSize
                }).FirstOrDefault();
        }

        [HttpGet]
        [Route("domains/{domainName}/aliases")]
        public IEnumerable<Alias> GetAliases(string domainName)
        {
            var mailDomain = MailEngine.GetDomain(domainName);
            return mailDomain.Aliases.ToEnumerable().Select(mailAlias => new Alias
            {
                ID = mailAlias.ID,
                AliasName = mailAlias.Name,
                RedirectName = mailAlias.Value
            });
        }

        [HttpGet]
        [Route("domains/{domainName}/aliases/{aliasName}")]
        public Alias GetAlias(string domainName, string aliasName)
        {
            if (!aliasName.Contains("@"))
                aliasName = aliasName + "@" + domainName;

            return (from mailAlias in MailEngine.GetDomain(domainName).Aliases.ToEnumerable()
                where mailAlias.Name == aliasName
                select new Alias
                {
                    ID = mailAlias.ID,
                    AliasName = mailAlias.Name,
                    RedirectName = mailAlias.Value
                }).FirstOrDefault();
        }

        // POST: api/Mail
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Mail/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Mail/5
        public void Delete(int id)
        {
        }
    }
}