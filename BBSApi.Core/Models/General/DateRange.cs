using System;

namespace BBSApi.Core.Models.General
{
    public class DateRange
    {
        public DateTime FromDate { get; set; } = DateTime.UtcNow;
        public DateTime ToDate { get; set; } = DateTime.UtcNow.AddDays(1);
    }
}