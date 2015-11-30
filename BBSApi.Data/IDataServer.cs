using System;
using System.Collections.Generic;
using BBSApi.Core.Models.Customer;
using BBSApi.Core.Models.Mail;
using BBSApi.Core.Models.Web;
using StackExchange.Redis;

namespace BBSApi.Data
{
    public interface IDataServer : IDisposable
    {
        ConnectionMultiplexer Redis { get; }
        List<WebSite> WebSites { get; }
        IEnumerable<CustomerAccount> CustomerAccounts { get; }
        IEnumerable<Address> Addresses { get; }
        IEnumerable<MailDomain> MailDomains { get; }
        IEnumerable<MailAccount> MailAccounts { get; }
        IEnumerable<MailAlias> MailAliases { get; }
        void Refresh();
        new void Dispose();
        void Reset();
        void Add<T>(T data);
        void Commit ();
    }
}