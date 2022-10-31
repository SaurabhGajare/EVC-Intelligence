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
    public class SuperAdminDashboardController : ControllerBase
    {
        private readonly IUserDetail _userdetailrepository;
        private readonly IBookingDetail _bookingdetailrepository;
        private readonly IChargingStation _chargingstationrepository;

        public SuperAdminDashboardController(IUserDetail userdetailrepository, IBookingDetail bookingdetailrepository, IChargingStation chargingstationrepository)
        {
            _userdetailrepository = userdetailrepository;
            _bookingdetailrepository = bookingdetailrepository;
            _chargingstationrepository = chargingstationrepository;
        }

        // API to get all admin details for super admin dashboard
        [HttpGet]
        [Route("SuperAdmin")]
        public async Task<List<object>> GetAllAdminDetails()
        {
            var Details = new List<object>();
            var AdminDetails = await _userdetailrepository.Login_Data_Details();

            foreach (var j in AdminDetails)
            {
                var obj = new
                {
                    AdminId = j.UserId,
                    AdminName = j.Name,
                    email = j.Email
                };
                Details.Add(obj);
            }

            return Details;
        }

        // API to delete the admin details from super admin dashboard
        [HttpDelete]
        [Route("RemoveAdmin")]
        public async Task<ActionResult> DeleteAdminDetails([FromBody] int id)
        {
            await _userdetailrepository.Delete_Admin_Details(id);

            return Ok("Deleted Successfully");
        }

        
    }
}
