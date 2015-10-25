using System.Collections.Generic;
using System.Linq;
using hMailServer;
using Newtonsoft.Json;

namespace BBSApi.MailServer.Extenders
{
    public static class ExtendDomains
    {
        public delegate bool Filter(Domain domain);

        public static IEnumerable<Domain> ToEnumerable(this Domains domains)
        {
            var index = 0;
            while (index < domains.Count)
            {
                yield return domains[index++];
            }
        }

      public static IEnumerable<Domain> ToEnumerable(this Domains domains, Filter filter)
        {
            return domains.ToEnumerable().Where(domain => filter(domain));
        }
        

       public static void Update(this Domain domain, string changes)
        {
            //********** Get changes
            var delta = JsonConvert.DeserializeObject<Dictionary<string, object>>(changes);

            //********** Update
            foreach (var change in delta)
            {
                if (change.Key == "Active")
                    domain.Active = (bool) change.Value;
                if (change.Key == "Postmaster")
                    domain.Postmaster = change.Value.ToString();
                if (change.Key == "MaxSize")
                    domain.MaxSize = int.Parse(change.Value.ToString());
                if (change.Key == "MaxMessageSize")
                    domain.MaxMessageSize = int.Parse(change.Value.ToString());
                if (change.Key == "MaxAccountSize")
                    domain.MaxAccountSize = int.Parse(change.Value.ToString());
                if (change.Key == "MaxNumberOfAccounts")
                    domain.MaxNumberOfAccounts = int.Parse(change.Value.ToString());
                if (change.Key == "MaxNumberOfAliases")
                    domain.MaxNumberOfAliases = int.Parse(change.Value.ToString());
                if (change.Key == "MaxNumberOfDistributionLists")
                    domain.MaxNumberOfDistributionLists = int.Parse(change.Value.ToString());
            }
        }
    }
}