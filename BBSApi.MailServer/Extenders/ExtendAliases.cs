using System.Collections.Generic;
using System.Linq;
using hMailServer;

namespace BBSApi.MailServer.Extenders
{
    public static class ExtendAliases
    {
        public delegate bool Filter(Alias domain);

        public static IEnumerable<Alias> ToEnumerable(this Aliases domains)
        {
            var index = 0;
            while (index < domains.Count)
            {
                yield return domains[index++];
            }
        }

        public static IEnumerable<Alias> ToEnumerable(this Aliases domains, Filter filter)
        {
            return domains.ToEnumerable().Where(domain => filter(domain));
        }

        public static bool Exists(this Aliases domains, string name)
        {
            try
            {
                return (domains.ItemByName[name] != null);
            }
            catch
            {
                return false;
            }
        }
    }
}