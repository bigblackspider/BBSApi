﻿using System;
using BBSApi.Core.Extenders;

namespace BBSApi.Core.Models.Customer
{
    public class CustomerAccount
    {
        private string _email;
        private string _mobile;
        private string _phone;

        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleNames { get; set; }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (!value.IsValidPhone())
                    throw new Exception($"Phone number '{value}' is not a valid.");
                _phone = value;
            }
        }

        public string Mobile
        {
            get { return _mobile; }
            set
            {
                if (!value.IsValidPhone())
                    throw new Exception($"Mobile phone number '{value}' is not a valid.");
                _mobile = value;
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (!value.IsValidEmail())
                    throw new Exception($"Email Address '{value}' is not a valid.");
                _email = value;
            }
        }

        public long AddrtessId { get; set; } = -1;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


        public CustomerAccount Update(CustomerAccount cust)
        {
            FirstName = cust.FirstName;
            LastName = cust.LastName;
            MiddleNames = cust.MiddleNames;
            Phone = cust.Phone;
            Mobile = cust.Mobile;
            Email = cust.Email;
            DateUpdated = DateTime.UtcNow;
            return this;
        }
    }
}