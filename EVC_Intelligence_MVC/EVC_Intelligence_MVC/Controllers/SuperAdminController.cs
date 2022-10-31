using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EVC_Intelligence_MVC.Controllers
{
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Action method for Super Admin Login
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        // Action method for super admin dashboard
        [Authorize(Roles = ("1"), AuthenticationSchemes = "SuperAdminAuth")]
        public IActionResult Dashboard()
        {
            // API call to get all admin details
            HttpClient httpclient = new HttpClient();
            Task<HttpResponseMessage> message = httpclient.GetAsync("https://localhost:44339/api/SuperAdminDashboard/SuperAdmin/");
            var strdata = message.Result.Content.ReadAsStringAsync().Result;

            dynamic data = JsonConvert.DeserializeObject(strdata);

            ViewBag.data = data;

            return View();
        }

        // function to generate random password for admin
        private static string RandomPassword(int len)
        {
            string searchFrom = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?";

            char[] password = new char[len];
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                password[i] = searchFrom[random.Next(0, searchFrom.Length)];
            }
            return new string(password);
        }

        // add admin page
        [Authorize(Roles = ("1"), AuthenticationSchemes = "SuperAdminAuth")]
        public IActionResult AddAdmin()
        {
            string password = RandomPassword(8);
            ViewBag.Password = password;
            return View();
        }

        // Logout for SuperAdmin
        [Authorize(Roles = ("1"), AuthenticationSchemes = "SuperAdminAuth")]
        public async Task<IActionResult> Logout(int id)
        {
            if (User.Identity.Name == id.ToString())
            {
                await HttpContext.SignOutAsync("SuperAdminAuth");
                return Redirect("/SuperAdmin/Login");
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }


    }
}
