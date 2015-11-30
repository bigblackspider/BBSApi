using System;
using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Models.Customer;
using BBSApi.Core.Models.Mail;
using BBSApi.Core.Models.Web;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BBSApi.Data
{
    public class DataServerRedis : IDataServer
    {
        private static ConnectionMultiplexer _redis;
        private IEnumerable<WebSite> _webSites;
        private IEnumerable<CustomerAccount> _customerAccounts;
        private IEnumerable<Address> _addresses;
        private IEnumerable<MailAccount> _mailAccounts;
        private IEnumerable<MailAlias> _mailAliases;
        private IEnumerable<MailDomain> _mailDomains;

        public ConnectionMultiplexer Redis => _redis ?? (_redis = ConnectionMultiplexer.Connect("localhost"));

        public List<WebSite> WebSites
        {
            get
            {
                if (_webSites == null)
                {
                    var db = Redis.GetDatabase();
                    _webSites = db.SetMembers("WebSites").Select(o => JsonConvert.DeserializeObject<WebSite>(o));
                }
                if (_webSites == null)
                    _webSites = new List<WebSite>();
                    return _webSites.ToList();
            }
       }
        public IEnumerable<CustomerAccount> CustomerAccounts
        {
            get
            {
                if (_customerAccounts == null)
                {
                    var db = Redis.GetDatabase();
                    _customerAccounts = db.SetMembers("CustomerAccounts").Select(o => JsonConvert.DeserializeObject<CustomerAccount>(o));
                }
                return (List<CustomerAccount>) _customerAccounts;
            }
        }
        public IEnumerable<Address> Addresses
        {
            get
            {
                if (_addresses == null)
                {
                    var db = Redis.GetDatabase();
                    _addresses = db.SetMembers("Addresses").Select(o => JsonConvert.DeserializeObject<Address>(o));
                }
                return (List<Address>) _addresses;
            }
        }

        public IEnumerable<MailDomain> MailDomains
        {
            get
            {
                if (_mailDomains == null)
                {
                    var db = Redis.GetDatabase();
                    _mailDomains = db.SetMembers("MailDomains").Select(o => JsonConvert.DeserializeObject<MailDomain>(o));
                }
                return (List<MailDomain>) _mailDomains;
            }
        }

        public IEnumerable<MailAccount> MailAccounts
        {
            get
            {
                if (_mailAccounts == null)
                {
                    var db = Redis.GetDatabase();
                    _mailAccounts = db.SetMembers("MailAccounts").Select(o => JsonConvert.DeserializeObject<MailAccount>(o));
                }
                return (List<MailAccount>) _mailAccounts;
            }
        }

        public IEnumerable<MailAlias> MailAliases
        {
            get
            {
                if (_mailAccounts == null)
                {
                    var db = Redis.GetDatabase();
                    _mailAliases = db.SetMembers("MailAliases").Select(o => JsonConvert.DeserializeObject<MailAlias>(o));
                }
                return (List<MailAlias>) _mailAliases;
            }
        }
        public void Refresh()
        {
            _webSites = null;
            _customerAccounts = null;
            _addresses = null;
            _mailDomains = null;
            _mailAccounts = null;
            _mailAliases = null;
        }

        public void Dispose()
        {
            Redis.Close();
            Redis.Dispose();
        }

        public void Reset()
        {
            Refresh();
            Reset("Addresses");
            Reset("CustomerAccounts");
            Reset("MailAccounts");
            Reset("MailAliases");
            Reset("MailDomains");
            Reset("WebSites");
        }

        public void Add<T>(T data)
        {
            if (data.GetType() == typeof (WebSite))
            {

                var tst = Convert.ChangeType(data, typeof(T));
               WebSites.Add((WebSite) tst);

            }
                 
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        private void Reset(string setName)
        {
            var db = Redis.GetDatabase();
            foreach (var o in db.SetMembers(setName))
                db.SetRemove(setName, o);
            
        }
    }
}