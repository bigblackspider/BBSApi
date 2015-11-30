using System;
using System.Collections.Generic;
using BBSApi.Core.Extenders;
using BBSApi.Core.Models.Types;

namespace BBSApi.Core.Models.Web
{
    public class WebSite
    {
        private string _domainName;
        public long SiteId { get; set; } = new Guid().GetHashCode();

        public string DomainName
        {
            get { return _domainName; }
            set
            {
                if (!value.IsValidDomainName())
                    throw new Exception($"Domain name '{value}' is invalid.");
                _domainName = value;
            }
        }

        public string Description { get; set; }
        public DateTime DateCreated { get; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public TSiteStatus Status { get; set; } = TSiteStatus.Created;
        public string MailDomainName { get; set; } = "none";
        public Dictionary<string, string> Details { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, object> Images { get; private set; } = new Dictionary<string, object>();

        public void Update(WebSite details)
        {
            DomainName = details.DomainName;
            Description = details.Description;
            LastUpdated = DateTime.UtcNow;
            Status = details.Status;
            MailDomainName = details.MailDomainName;
            Details = details.Details;
            Images = details.Images;
        }
    }
}