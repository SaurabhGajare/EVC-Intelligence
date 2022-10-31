using EVC_Intelligence_API.Models;
using EVC_Intelligence_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddAdminController : ControllerBase
    {

        private readonly IUserDetail _userdetailrepository;
        private readonly ILoginData _logindatarepository;
        private readonly IConfiguration _iConfig;

        public AddAdminController(IUserDetail userdetailrepository, ILoginData logindatarepository, IConfiguration iConfig)
        {
            _userdetailrepository = userdetailrepository;
            _logindatarepository = logindatarepository;
            _iConfig = iConfig;
        }

        // Add new admin into the DB
        [HttpPost]
        public async Task<ActionResult> PostNotifyAdmin(int id, [FromBody] Logininfo _notifyinfo)
        {
            var userExist = await _userdetailrepository.Get(_notifyinfo.email);
            if (userExist == null)
            {
                LoginDatum loginuser = new LoginDatum
                {
                    Password = BCrypt.Net.BCrypt.HashPassword(_notifyinfo.password),
                    SecurityQuestion = "What is your job role ?",

                    Answer = "Admin",
                    RoleId = 2
                };

                var newloginuser = await _logindatarepository.Create(loginuser);
                var userId = newloginuser.UserId;


                UserDetail detailuser = new UserDetail
                {
                    UserId = userId,
                    Name = _notifyinfo.name,
                    Email = _notifyinfo.email,
                    PhoneNumber = 1234567890,
                    Coins = 0

                };
                var newuserdetail = await _userdetailrepository.Create(detailuser);
                string body1 = "<p>Welcome To EVC Intelligence</p><br><p> You have been added as Admin in our System </p><p> Your OTP is :";
                string body2 = "</p>";
                bool result = sendEmail(_notifyinfo.email, "Credentials for Admin", body1 + _notifyinfo.password + body2);
                return Ok("Admin Created");
            }
            else
            {
                return BadRequest("Admin already exist");
            }
        }

        // function to send email to new admin with the login details(password)
        public bool sendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                string senderEmail = _iConfig.GetSection("MySettings").GetSection("EmailId").Value;
                string senderPassword = _iConfig.GetSection("MySettings").GetSection("Password").Value;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

    }
}
