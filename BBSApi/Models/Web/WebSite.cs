using System;
using System.Collections.Generic;
using BBSApi.Models.Types;

namespace BBSApi.Models.Web
{
    public class WebSite
    {
        public int SiteId { get; set; }
        public Guid SiteGuid { get; } = new Guid();
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; } = DateTime.UtcNow;
        public TSiteStatus Status { get; set; } = TSiteStatus.Created;
        public Dictionary<string, string> Details { get; } = new Dictionary<string, string>();
        public Dictionary<string, object> Images { get; } = new Dictionary<string, object>();
    }
}