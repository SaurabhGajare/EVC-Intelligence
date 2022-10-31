using EVC_Intelligence_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Repositories
{
    public interface IChargingStation
    {
        Task<List<ChargingStation>> GetAllStations();
        Task<ChargingStation> Get(int StationId);
        Task<List<ChargingStation>> GetAllByPincode(int id);
        Task<string> GetStationName(int Id);
        Task<List<City>> GetCityName();
        Task<List<State>> GetStateName();
        Task<List<ChargingStation>> GetCities(string cityName);
        Task<ChargingStation> Modify_Charging_Station(int stnid);
        Task Delete_Station_Details(int stnid);
        Task<List<City>> GetCity(string cityname);
        Task<State> GetState(string statename);
        Task<ChargingStation> Create(ChargingStation station);
    }
}
