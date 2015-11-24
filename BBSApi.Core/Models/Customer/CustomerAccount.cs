using System.Collections.Generic;
using BBSApi.Core.Models.General;

namespace BBSApi.Core.Models.Customer
{
    public class CustomerAccount
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleNames { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Address Contact { get; } = new Address();
        public List<History> History { get; } = new List<History>();
    }
}