﻿namespace BBSApi.Core.Models.Customer
{
    public class Address
    {
        public long AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
    }
}