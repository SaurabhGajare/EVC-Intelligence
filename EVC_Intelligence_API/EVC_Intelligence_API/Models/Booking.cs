using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Models
{
    public class Booking
    {
        public int StationId { get; set; }
        public int UserId { get; set; }
        public int CounterId { get; set; }
        public int SlotId { get; set; }
        public string SlotTime { get; set; }
        public string Date { get; set; }
        public string VehicleType { get; set; }
    }
}
