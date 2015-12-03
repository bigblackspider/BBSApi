using System;
using BBSApi.Core.Models.Types;

namespace BBSApi.Core.Models.General
{
    public class History
    {
        public long HistoryId { get; set; }
        public THistoryType HistoryType { get; set; } = THistoryType.General;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string Key { get; set; }
        public long LinkId { get; set; }
        public string Description { get; set; }
    }
}