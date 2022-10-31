using EVC_Intelligence_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace EVC_Intelligence_API.Repositories
{
    public class ChargingStationRepo : IChargingStation
    {
        private readonly EVC_IntelligenceContext _context;

        public ChargingStationRepo(EVC_IntelligenceContext context)
        {
            _context = context;
        }

        //Get List Of All Charging Stations
        public async Task<List<ChargingStation>> GetAllStations()
        {
            return await _context.ChargingStations.ToListAsync();

        }

        // Get station details by StationId
        public async Task<ChargingStation> Get(int StationId)
        {
            var station =  await _context.ChargingStations.ToListAsync();
            return station.Find(x => x.StationId == StationId);
        }

        //Get List Of All Charging Stations By pincode
        public async Task<List<ChargingStation>> GetAllByPincode(int pincode)
        {
            var obj = await _context.ChargingStations.ToListAsync();
            return obj.FindAll(e => e.Pincode == pincode && e.OpenClose == true).ToList();
        }

        //Get Station Name by Id
        public async Task<string> GetStationName(int Id)
        {
            var station = await _context.ChargingStations.FindAsync(Id);
            return station.StationName;
        }

        //Get List of All City Names
        public async Task<List<City>> GetCityName()
        {
            var cities = await _context.Cities.ToListAsync();
            return cities;
        }

        // Get List of States
        public async Task<List<State>> GetStateName()
        {
            var states = await _context.States.ToListAsync();
            return states;
        }

        //Get List of charging stations using City Name 
        public async Task<List<ChargingStation>> GetCities(string cityName)
        {
            var cities = await _context.ChargingStations.ToListAsync();
            var city = (await _context.Cities.ToListAsync()).FindAll(e => e.CityName == cityName);
            List<int> cityById = new List<int>();
            List<ChargingStation> chargingStations = new List<ChargingStation>();
            foreach (var itm in city)
            {
                cityById.Add(itm.CityId);

            }

            foreach (var id in cityById)
            {
                foreach (var cs in cities)
                {

                    if (cs.CityId == id && cs.OpenClose == true)
                    {

                        chargingStations.Add(cs);
                    }
                }
            }
            return chargingStations;
        }

        // Modify open-close status of charging station, by using station id
        public async Task<ChargingStation> Modify_Charging_Station(int stnid)
        {
            var obj = (from p in _context.ChargingStations where p.StationId == stnid select p).FirstOrDefault();

            obj.OpenClose = !obj.OpenClose;

            await _context.SaveChangesAsync();

            return obj;
        }

        // Deleting Station detailsn by using station id
        public async Task Delete_Station_Details(int stnid)
        {
            var Booking = _context.BookingDetails.ToList().FindAll(x => x.StationId == stnid);
            var CS = _context.ChargingStations.ToList().Find(x => x.StationId == stnid);

            foreach (var j in Booking)
            {
                _context.BookingDetails.Remove(j);
            }
            _context.ChargingStations.Remove(CS);

            await _context.SaveChangesAsync();
        }

        //Get list of all cities by city name
        public async Task<List<City>> GetCity(string cityname)
        {
            var obj = await _context.Cities.ToListAsync();
            return obj.FindAll(e => e.CityName == cityname).ToList();
        }

        //Get state by statename
        public async Task<State> GetState(string statename)
        {
            var obj = await _context.States.ToListAsync();
            return obj.Find(e => e.StateName == statename);
        }

        //Add charging station into db
        public async Task<ChargingStation> Create(ChargingStation station)
        {
            _context.ChargingStations.Add(station);
            await _context.SaveChangesAsync();

            return station;
        }
    }
}
