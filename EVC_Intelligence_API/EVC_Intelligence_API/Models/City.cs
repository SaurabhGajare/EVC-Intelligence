using System;
using System.Collections.Generic;

#nullable disable

namespace EVC_Intelligence_API.Models
{
    public partial class City
    {
        public City()
        {
            ChargingStations = new HashSet<ChargingStation>();
        }

        public int CityId { get; set; }
        public int? StateId { get; set; }
        public string CityName { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<ChargingStation> ChargingStations { get; set; }
    }
}
