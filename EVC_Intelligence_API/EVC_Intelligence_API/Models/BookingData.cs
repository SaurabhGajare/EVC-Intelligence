using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Models
{
    public class BookingData
    {
        public int StationID;
        public string Stations;
        public int Slots;
        public int BookedSlots;
        public int EmptySlots;
        public DateTime Date;

        public BookingData(int StationID, string Stations, int Slots, int BookedSlots, int EmptySlots, DateTime Date)
        {
            this.Stations = Stations;
            this.Slots = Slots;
            this.BookedSlots = BookedSlots;
            this.EmptySlots = EmptySlots;
            this.StationID = StationID;
            this.Date = Date;

        }
        public BookingData()
        {

        }
    }
}
