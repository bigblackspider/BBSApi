using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BBSApi.Core.Models.Mail;
using BBSApi.MailServer;
using BBSApi.MailServer.Extenders;
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
        public IEnumerable<Core.Models.Mail.MailDomain> GetDomains()
        {
            return MailEngine.GetDomains().Select(mailDomain => new Core.Models.Mail.MailDomain
            {
                DomainName = mailDomain.Name,
                Active = mailDomain.Active,
                Postmaster = mailDomain.Postmaster
            });
        }

        [HttpPut]
        [Route("domains/{domainName}")]
        public Core.Models.Mail.MailDomain CreateDomain(string domainName)
        {
            var mailDomain = MailEngine.CreateDomain(domainName);
            return new Core.Models.Mail.MailDomain
            {
                DomainName = mailDomain.Name,
                Active = mailDomain.Active,
                Postmaster = mailDomain.Postmaster
            };
        }


        [HttpGet]
        [Route("domains/{domainName}")]
        public Core.Models.Mail.MailDomain GetDomain(string domainName)
        {
            var mailDomain = MailEngine.GetDomain(domainName);
            return new Core.Models.Mail.MailDomain
            {
                DomainId = mailDomain.ID,
                DomainName = mailDomain.Name,
                Active = mailDomain.Active,
                Postmaster = mailDomain.Postmaster
            };
        }

        [HttpGet]
        [Route("domains/{domainName}/accounts")]
        public IEnumerable<Core.Models.Mail.MailAccount> GetAccounts(string domainName)
        {
            var mailDomain = MailEngine.GetDomain(domainName);
            return mailDomain.Accounts.ToEnumerable().Select(mailAccount => new Core.Models.Mail.MailAccount
            {
                AccountId = mailAccount.ID,
                Address = mailAccount.Address,
                Active = mailAccount.Active,
                PersonFirstName = mailAccount.PersonFirstName,
                PersonLastName = mailAccount.PersonLastName,
                MaxSize = mailAccount.MaxSize
            });
        }

        [HttpGet]
        [Route("domains/{domainName}/accounts/{address}")]
        public Core.Models.Mail.MailAccount GetAccount(string domainName, string address)
        {
            if (!address.Contains("@"))
                address = address + "@" + domainName;

            return (from mailAccount in MailEngine.GetDomain(domainName).Accounts.ToEnumerable()
                where mailAccount.Address == address
                select new Core.Models.Mail.MailAccount
                {
                    AccountId = mailAccount.ID,
                    Address = mailAccount.Address,
                    Active = mailAccount.Active,
                    PersonFirstName = mailAccount.PersonFirstName,
                    PersonLastName = mailAccount.PersonLastName,
                    MaxSize = mailAccount.MaxSize
                }).FirstOrDefault();
        }

        [HttpGet]
        [Route("domains/{domainName}/aliases")]
        public IEnumerable<Core.Models.Mail.MailAlias> GetAliases(string domainName)
        {
            var mailDomain = MailEngine.GetDomain(domainName);
            return mailDomain.Aliases.ToEnumerable().Select(mailAlias => new Core.Models.Mail.MailAlias
            {
                AliasId = mailAlias.ID,
                AliasName = mailAlias.Name,
                RedirectName = mailAlias.Value
            });
        }

        [HttpGet]
        [Route("domains/{domainName}/aliases/{aliasName}")]
        public Core.Models.Mail.MailAlias GetAlias(string domainName, string aliasName)
        {
            if (!aliasName.Contains("@"))
                aliasName = aliasName + "@" + domainName;

            return (from mailAlias in MailEngine.GetDomain(domainName).Aliases.ToEnumerable()
                where mailAlias.Name == aliasName
                select new Core.Models.Mail.MailAlias
                {
                    AliasId = mailAlias.ID,
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