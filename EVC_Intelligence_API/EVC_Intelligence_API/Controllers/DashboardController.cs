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
    public class DashboardController : ControllerBase
    {
        private readonly IUserDetail _userdetailrepository;
        private readonly IBookingDetail _bookingdetailrepository;
        private readonly IChargingStation _chargingstationrepository;

        public DashboardController(IUserDetail userdetailrepository, IBookingDetail bookingdetailrepository, IChargingStation chargingstationrepository)
        {
            _userdetailrepository = userdetailrepository;
            _bookingdetailrepository = bookingdetailrepository;
            _chargingstationrepository = chargingstationrepository;
        }

        // get booking details and coins of the User
        [HttpGet]
        [Route("AllDetails/{id}")]
        public async Task<object> GetAllDetails(int id)
        {
            List<object> Details = new List<object>();
            var UserDetail = await _userdetailrepository.GetUserById(id);
            var BookingDetail = await _bookingdetailrepository.GetBookingByUser(id);

            if (BookingDetail == null)
            {
                var notobj = new
                {
                    username = UserDetail.Name,
                    stationname = "Not booked",
                    coins = UserDetail.Coins
                };

                return notobj;
            }

            int stationid = int.Parse(BookingDetail.StationId.ToString());
            var StationDetail = await _chargingstationrepository.Get(stationid);


            var obj = new
            {
                username = UserDetail.Name,
                stationname = StationDetail.StationName,
                vehicletype = BookingDetail.VehicleType,
                date = BookingDetail.Date,
                counter = BookingDetail.CounterId,
                timeslot = BookingDetail.SlotTime.Value.Hours,
                coins = UserDetail.Coins
            };

            return obj;
        }

        // Delete Booking of the User
        [HttpDelete]
        [Route("CancelBooking/{userid}")]
        public async Task<ActionResult> CancelBooking(int userid)
        {
            await _bookingdetailrepository.CancelBooking(userid);
            return Ok("Booking Deleted");
        }
    }
}
