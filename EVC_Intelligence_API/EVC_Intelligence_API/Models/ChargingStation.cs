using System;
using System.Collections.Generic;

#nullable disable

namespace EVC_Intelligence_API.Models
{
    public partial class ChargingStation
    {
        public ChargingStation()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public int StationId { get; set; }
        public int? CityId { get; set; }
        public string StationName { get; set; }
        public bool? OpenClose { get; set; }
        public string Address { get; set; }
        public int? Pincode { get; set; }
        public int? TotalCounters { get; set; }
        public int? TotalSlotsPerCounter { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
