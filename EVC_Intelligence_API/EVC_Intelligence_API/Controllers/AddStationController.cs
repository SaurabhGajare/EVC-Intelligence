using EVC_Intelligence_API.Models;
using EVC_Intelligence_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddStationController : ControllerBase
    {
        private readonly IChargingStation _chargingstationrepository;

        public AddStationController(IChargingStation chargingstationrepository)
        {
            _chargingstationrepository = chargingstationrepository;
        }

        //Post method to send station data
        [HttpPost]
        public async Task<ActionResult> PostStation([FromBody] StationData data)
        {
            int cityId = 0;
            int stateid = 0;

            ChargingStation station = new ChargingStation();
            List<City> cityName = await _chargingstationrepository.GetCity(data.CityName);

            var statename = await _chargingstationrepository.GetState(data.StateName);
            if (statename != null) { stateid = statename.StateId; }                //Get state id of user entered state

            foreach (var item in cityName)
            {
                if (item.StateId == stateid) //Match user entered state id with city's state id
                {
                    cityId = item.CityId;
                }
            }

            station.CityId = cityId;
            station.StationName = data.StationName;
            station.OpenClose = true;
            station.Address = data.Address;
            station.Pincode = data.Pincode;
            station.TotalCounters = 4;
            station.TotalSlotsPerCounter = 8;
            station.OpeningTime = TimeSpan.Parse("10:00:00");
            station.ClosingTime = TimeSpan.Parse("18:00:00");

            var newstation = await _chargingstationrepository.Create(station);

            return Ok("Station Added");
        }
        
        // function to map cities to states
        [HttpGet]
        public async Task<Dictionary<string, List<dynamic>>> GetStatesAndCities()
        {
            var states = await _chargingstationrepository.GetStateName();
            var cities = await _chargingstationrepository.GetCityName();


            Dictionary<string, List<dynamic>> dictobj = new Dictionary<string, List<dynamic>>();
            foreach (var state in states)
            {
                var obj = new List<dynamic>();
                foreach (var city in cities)
                {
                    if (city.StateId == state.StateId)
                    {
                        obj.Add(city.CityName);
                    }

                }

                dictobj.Add(state.StateName, obj);
            }

            return dictobj;
        }
    }
}
