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
    public class PayController : ControllerBase
    {
        private readonly IBookingDetail _bookingdetailrepository;
        private readonly IChargingStation _chargingstationrepository;

        public PayController(IBookingDetail bookingdetailrepository, IChargingStation chargingstationrepository)
        {
            _bookingdetailrepository = bookingdetailrepository;
            _chargingstationrepository = chargingstationrepository;
        }

        // Book the slot of the User
        [HttpPost]
        [Route("Pay")]
        public async Task<ActionResult> PostBookingDetail([FromBody] Booking booking)
        {
            BookingDetail bookingdetail = new BookingDetail();
            bookingdetail.Date = Convert.ToDateTime(booking.Date);
            bookingdetail.CounterId = booking.CounterId;
            bookingdetail.SlotTime = TimeSpan.Parse(booking.SlotTime);
            bookingdetail.UserId = booking.UserId;
            bookingdetail.StationId = booking.StationId;
            bookingdetail.SlotId = booking.SlotId;
            bookingdetail.VehicleType = booking.VehicleType;
            var bookingDetail = await _bookingdetailrepository.Create(bookingdetail);


            return Ok("Booking Successful");
        }

        // Get details of slot of paticulat station on particular date
        [HttpGet]
        public async Task<List<BookingDetail>> GetSlotDetail(int StationId, string date)
        {
            DateTime Date = Convert.ToDateTime(date.ToString());

            List<BookingDetail> bookingDetail = await _bookingdetailrepository.GetBookingDetail();

            var slotDetails = bookingDetail.FindAll(x => x.StationId == StationId && x.Date == Date);

            return slotDetails;

        }

        // Get Station name based on Station Id
        [HttpGet]
        [Route("GetStationDetail/{id}")]
        public async Task<string> GetStationName(int id)
        {

            string stationName = await _chargingstationrepository.GetStationName(id);
            return stationName;

        }
    }
}
