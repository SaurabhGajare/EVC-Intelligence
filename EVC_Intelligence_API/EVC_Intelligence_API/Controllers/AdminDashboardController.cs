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
    public class AdminDashboardController : ControllerBase
    {
        private readonly IUserDetail _userdetailrepository;
        private readonly IBookingDetail _bookingdetailrepository;
        private readonly IChargingStation _chargingstationrepository;

        public AdminDashboardController(IUserDetail userdetailrepository, IBookingDetail bookingdetailrepository, IChargingStation chargingstationrepository)
        {
            _userdetailrepository = userdetailrepository;
            _bookingdetailrepository = bookingdetailrepository;
            _chargingstationrepository = chargingstationrepository;
        }

        // API to get all charging station details for admin dashboard
        [HttpGet]
        [Route("AdminDetails/{id}")]
        public async Task<List<object>> GetAdminDetails(int id)
        {
            List<object> Details = new List<object>();
            // var AdminDetail = await _dashboard.User_Detail(id);
            var AllStations = await _chargingstationrepository.GetAllStations();

            foreach (var j in AllStations)
            {
                var obj = new
                {
                    address = j.Address,
                    stnid = j.StationId,
                    stnname = j.StationName,
                    status = j.OpenClose
                };
                Details.Add(obj);
            }

            return Details;
        }

        // API to update the open close status of charging station
        [HttpPost]
        [Route("StationStatus")]
        public async Task<ActionResult> Poststationstatus([FromBody] int stnid)
        {
            var chargingstationdetails = await _chargingstationrepository.Modify_Charging_Station(stnid);

            return Ok("Updated Successfully");
        }

        // API to delete the charging staion details from admin dashboard
        [HttpDelete]
        [Route("RemoveStation")]
        public async Task<ActionResult> DeleteStationDetails([FromBody] int stnid)
        {
            await _chargingstationrepository.Delete_Station_Details(stnid);

            return Ok("Deleted Successfully");
        }
    }
}
