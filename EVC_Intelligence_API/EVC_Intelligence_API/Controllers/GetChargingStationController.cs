using EVC_Intelligence_API.Models;
using EVC_Intelligence_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetChargingStationController : ControllerBase
    {
        private readonly IBookingDetail _bookingdetailrepository;
        private readonly IChargingStation _chargingstationrepository;

        public GetChargingStationController(IBookingDetail bookingdetailrepository, IChargingStation chargingstationrepository)
        {
            _bookingdetailrepository = bookingdetailrepository;
            _chargingstationrepository = chargingstationrepository;
        }

        // Get Charging Station by PinCode or CityName
        [HttpGet]
        [Route("Get/{pid}/{date}/{boolean}")]
        public async Task<ActionResult> GetAllChargingStations(string pid, DateTime date, int boolean)
        {
            List<ChargingStation> chargingStationPinCode;
            if (boolean == 0)                               //Search By Pin Code
            {
                int id = int.Parse(pid);
                chargingStationPinCode = await _chargingstationrepository.GetAllByPincode(id);

            }
            else
            {                                           
                //Search By city
                chargingStationPinCode = await _chargingstationrepository.GetCities(pid);
            }

            List<ChargingStation> chargingStation = await _chargingstationrepository.GetAllStations();
            List<BookingDetail> bookingDetails = await _bookingdetailrepository.GetBookingDetail();

            List<BookingData> bookingData = new List<BookingData>();
            int[] array = new int[chargingStationPinCode.Count()];
            int[] arrayg = new int[chargingStationPinCode.Count()];
            string[] arr = new string[chargingStationPinCode.Count()];
            int i = 0;
            int p = 0;
            int g = 0;
            DateTime Time = DateTime.Now;
            TimeSpan currentTime = Time.TimeOfDay;
            foreach (var item in chargingStationPinCode)
            {
                array[i] = item.StationId;
                arr[p] = item.StationName;
                int hours = (int)item.TotalSlotsPerCounter;           //Slots Per Counter for any other day
                if (date.Date.CompareTo(Time.Date) == 0)              //Slots Per Counter for same day
                {
                    TimeSpan slots = (TimeSpan)item.ClosingTime;
                    TimeSpan difference = slots - currentTime;
                    hours = difference.Hours;
                    if (hours > ((TimeSpan)(item.ClosingTime - item.OpeningTime)).Hours)
                    {

                        hours = ((TimeSpan)(item.ClosingTime - item.OpeningTime)).Hours;
                    }
                }
                arrayg[g] = (int)(hours * item.TotalCounters);          //Total Slots
                i++;
                p++;
                g++;
            }
            int j = 0;
            int[] array2 = new int[chargingStationPinCode.Count()];     //Array to store booked Slots

            if (date.Date.CompareTo(Time.Date) == 0)               //To Show booked Slots for the same day
            {
                foreach (var itr in array)
                {
                    var count = 0;
                    foreach (var comb in bookingDetails)

                    {
                        DateTime dt = (DateTime)(comb.Date);
                        if (itr == comb.StationId && ((DateTime.Compare(dt, date)) == 0) && (((TimeSpan)comb.SlotTime) > currentTime))
                            count++;
                    }
                    array2[j] = count;
                    j++;
                }

            }
            else                                          //To Show Booked Slot for any other Day                             
            {
                foreach (var itr in array)
                {
                    var count = 0;
                    foreach (var comb in bookingDetails)

                    {
                        DateTime dt = (DateTime)(comb.Date);
                        if (itr == comb.StationId && ((DateTime.Compare(dt, date)) == 0))
                            count++;
                    }
                    array2[j] = count;
                    j++;
                }
            }
            var bookingobjtotal = new List<dynamic>();
            for (int s = 0; s < chargingStationPinCode.Count(); s++)
            {

                var DateCurrent = date.Date;
                var stringDate = DateCurrent.ToString("dd/MM/yyyy");
                DateTime newDate = DateTime.Now;
                

                if (((newDate.TimeOfDay.Hours) > ((TimeSpan)chargingStationPinCode[0].ClosingTime).Hours) && (DateTime.Compare(newDate.Date, DateCurrent.Date)) == 0)  //To Show Slots After Closing Time
                {
                    var bookingobj1 = new
                    {
                        StationId = array[s],
                        Stations = arr[s],
                        Slots = arrayg[s],
                        BookedSlots = array2[s],
                        EmptySlots = 0,
                        Date = stringDate
                    };
                    bookingobjtotal.Add(bookingobj1);
                }
                else
                {
                    var bookingobj2 = new
                    {
                        StationId = array[s],
                        Stations = arr[s],
                        Slots = arrayg[s],
                        BookedSlots = array2[s],
                        EmptySlots = (arrayg[s] - array2[s]),
                        Date = stringDate
                    };
                    bookingobjtotal.Add(bookingobj2);
                }
                
            }

            return Ok(bookingobjtotal);
        }

        // Get all cities
        [HttpGet]
        [Route("SearchCity")]
        public async Task<List<string>> GetCities()
        {

            List<City> cities = await _chargingstationrepository.GetCityName();
            List<string> cityNames = new List<string>();
            foreach (var item in cities)
            {

                cityNames.Add(item.CityName);

            }
            return cityNames;

        }

    }
}
