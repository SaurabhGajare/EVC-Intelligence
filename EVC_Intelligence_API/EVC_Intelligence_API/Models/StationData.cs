using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Models
{
    public class StationData
    {
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string StationName { get; set; }
        public bool? OpenClose { get; set; }
        public string Address { get; set; }
        public int? Pincode { get; set; }
    }
}
