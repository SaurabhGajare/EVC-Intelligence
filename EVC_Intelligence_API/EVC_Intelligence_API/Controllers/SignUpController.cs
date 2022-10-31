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
    public class SignUpController : ControllerBase
    {
        private readonly IUserDetail _userdetailrepository;
        private readonly ILoginData _logindatarepository;

        public SignUpController(IUserDetail userdetailrepository, ILoginData logindatarepository)
        {
            _userdetailrepository = userdetailrepository;
            _logindatarepository = logindatarepository;
        }

        // Create New User in the DB
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] SignUpData data)
        {

            var userExist = await _userdetailrepository.Get(data.email);
            if (userExist == null)
            {
                LoginDatum loginuser = new LoginDatum
                {
                    Password = BCrypt.Net.BCrypt.HashPassword(data.password),
                    SecurityQuestion = data.question,
                    Answer = data.answer,
                    RoleId = 3
                };

                var newloginuser = await _logindatarepository.Create(loginuser);
                var userId = newloginuser.UserId;


                UserDetail detailuser = new UserDetail
                {
                    UserId = userId,
                    Name = data.name,
                    Email = data.email,
                    PhoneNumber = data.phone,
                    Coins = 0
                };

                var newuserdetail = await _userdetailrepository.Create(detailuser);
                
                return Ok("User Created");
            }
            else
            {
                return BadRequest("User already exist");
            }
            
        }

    }
}
