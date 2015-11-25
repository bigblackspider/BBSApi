using System;
using System.Collections.Generic;
using BBSApi.Core.Models.Types;

namespace BBSApi.Core.Models.Web
{
    public class WebSite
    {
        public int SiteId { get; set; }
        public Guid Token { get; } = new Guid();
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public TSiteStatus Status { get; set; } = TSiteStatus.Created;
        public string MailDomainName { get; set; } = "none";
        public Dictionary<string, string> Details { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, object> Images { get; private set; } = new Dictionary<string, object>();

        public void Update(WebSite details)
        {
            Name = details.Name;
            Description = details.Description;
            LastUpdated = DateTime.UtcNow;
            Status = details.Status;
            MailDomainName = details.MailDomainName;
            Details = details.Details;
            Images = details.Images;
        }
    }
}