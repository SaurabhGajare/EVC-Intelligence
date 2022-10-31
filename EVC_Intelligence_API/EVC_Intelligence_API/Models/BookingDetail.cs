using System;
using System.Collections.Generic;

#nullable disable

namespace EVC_Intelligence_API.Models
{
    public partial class BookingDetail
    {
        public int BookingId { get; set; }
        public int? StationId { get; set; }
        public int? UserId { get; set; }
        public int? CounterId { get; set; }
        public int? SlotId { get; set; }
        public TimeSpan? SlotTime { get; set; }
        public DateTime? Date { get; set; }
        public string VehicleType { get; set; }

        public virtual ChargingStation Station { get; set; }
        public virtual LoginDatum User { get; set; }
    }
}
