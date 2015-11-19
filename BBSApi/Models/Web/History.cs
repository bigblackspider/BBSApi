using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BBSApi.Models.Types;

namespace BBSApi.Models.Web
{
    public class History
    {
        public int HistoryId { get; set; }
        public THistoryType HistoryType { get; set; } = THistoryType.General;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string Key { get; set; }
        public int LinkId { get; set; }
        public string Description { get; set; }
    }
}