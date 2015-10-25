using System.Collections.Generic;
using System.Linq;
using hMailServer;
using Newtonsoft.Json;

namespace BBSApi.MailServer.Extenders
{
    public static class ExtendAccounts
    {
        public delegate bool Filter(Account account);

        public static IEnumerable<Account> ToEnumerable(this Accounts accounts)
        {
            var index = 0;
            while (index < accounts.Count)
            {
                yield return accounts[index++];
            }
        }

       public static IEnumerable<Account> ToEnumerable(this Accounts accounts, Filter filter)
        {
            return accounts.ToEnumerable().Where(domain => filter(domain));
        }

        public static void Update(this Account account, string changes)
        {
            //********** Get changes
            var delta = JsonConvert.DeserializeObject<Dictionary<string, object>>(changes);

            //********** Update
            foreach (var change in delta)
            {
                if (change.Key == "Active")
                   account.Active = (bool)change.Value;
                if (change.Key == "PersonFirstName")
                    account.PersonFirstName = change.Value.ToString();
                if (change.Key == "PersonLastName")
                    account.PersonLastName = change.Value.ToString();
                if (change.Key == "MaxSize")
                    account.MaxSize = int.Parse(change.Value.ToString());
                /*if (change.Key == "MaxMessageSize")
                    account.MaxMessageSize = int.Parse(change.Value.ToString());
                if (change.Key == "MaxAccountSize")
                    account.MaxAccountSize = int.Parse(change.Value.ToString());
                if (change.Key == "MaxNumberOfAccounts")
                    account.MaxNumberOfAccounts = int.Parse(change.Value.ToString());
                if (change.Key == "MaxNumberOfAliases")
                    account.MaxNumberOfAliases = int.Parse(change.Value.ToString());
                if (change.Key == "MaxNumberOfDistributionLists")
                    account.MaxNumberOfDistributionLists = int.Parse(change.Value.ToString());*/
            }
        }
    }
}