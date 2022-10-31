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
    public class UpdateUserDetailsController : ControllerBase
    {
        private readonly IUserDetail _userdetailrepository;
        private readonly ILoginData _logindatarepository;

        public UpdateUserDetailsController(IUserDetail userdetailrepository, ILoginData logindatarepository)
        {
            _userdetailrepository = userdetailrepository;
            _logindatarepository = logindatarepository;
        }

        // To update only the phone number
        [HttpPut]
        public async Task<ActionResult> UpdatePhone(UpdateUserDetail userdetail)
        {
            try
            {
                var user = await _userdetailrepository.UpdatePhone(userdetail.phone, userdetail.userid);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // To update only the phone number with password
        [HttpPut]
        [Route("UpdateWithPassword")]
        public async Task<ActionResult> UpdateWithPassword(UpdateUserDetail userdetail)
        {
            var olddetails = await _logindatarepository.Get(userdetail.userid);
            //var verified = BCrypt.Net.BCrypt.Verify(userdetail.password, olddetails.Password);
            var passwordHash = olddetails.Password;
            var userWithPhone = await _userdetailrepository.UpdatePhone(userdetail.phone, userdetail.userid);
            var userWithPass = await _logindatarepository.UpdatePassword(userdetail.userid, userdetail.password, userdetail.currentPassword, passwordHash);

            if (userWithPass)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Current Password is Incorrect!");
            }
            
        }
    }
}
