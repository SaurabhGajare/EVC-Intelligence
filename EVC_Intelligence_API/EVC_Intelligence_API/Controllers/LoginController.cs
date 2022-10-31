using EVC_Intelligence_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVC_Intelligence_API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace EVC_Intelligence_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserDetail _userdetailrepository;
        private readonly ILoginData _logindatarepository;

        public LoginController(IUserDetail userdetailrepository, ILoginData logindatarepository)
        {
            _userdetailrepository = userdetailrepository;
            _logindatarepository = logindatarepository;
        }

        public object ViewData { get; private set; }

        // Verify Login Details of the user
        [HttpPost]
        public async Task<ActionResult> PostLogin( Logininfo _logininfo)
        {
              
            var data = await _userdetailrepository.Get(_logininfo.email);
            var passmatchinfo = await _logindatarepository.Get(data.UserId);
            var roleId = passmatchinfo.RoleId.ToString();
            var verified = BCrypt.Net.BCrypt.Verify(_logininfo.password, passmatchinfo.Password);
            if (verified)
            {
                var userobj = new
                {
                    id = data.UserId,
                    returnUrl = _logininfo.returnurl,
                    roleid = roleId,
                    name = data.Name,
                    email = _logininfo.email
                };
                return Ok(userobj);

            }
            else
            {
                return BadRequest();
            }
            
        }

        // Get User Details based on userId
        [HttpGet]
        [Route("GetUserDetails/{userid}")]
        public async Task<object> GetUserDetails(int userid)
        {
            var UserDetails = await _userdetailrepository.GetUserById(userid);
            var LoginDetails = await _logindatarepository.Get(userid);

            var updateUserDetailsObj = new
            {
                id = userid,
                name = UserDetails.Name,
                email = UserDetails.Email,
                phone = UserDetails.PhoneNumber,
                security_question = LoginDetails.SecurityQuestion,
                security_question_answer = LoginDetails.Answer,
                password_hash = LoginDetails.Password
            };

            return updateUserDetailsObj;
        }

        private ActionResult RedirectToActionResult(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
