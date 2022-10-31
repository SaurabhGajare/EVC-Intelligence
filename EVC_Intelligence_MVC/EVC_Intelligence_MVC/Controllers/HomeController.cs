using EVC_Intelligence_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EVC_Intelligence_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Checkslot for Selecting Station and Date
        [Authorize(Roles = ("3"), AuthenticationSchemes = "UserAuth")]
        public IActionResult Checkslot(int id)
        {
            if(User.Identity.Name == id.ToString())
            {
                // API Call to get the avaialble cities
                HttpClient httpclient = new HttpClient();
                Task<HttpResponseMessage> message = httpclient.GetAsync("https://localhost:44339/api/GetChargingStation/SearchCity");
                var strdata = message.Result.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(strdata);
                ViewBag.Cities = json;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            
        }

        // Update User Details - phone and password
        [Authorize(Roles = ("3"), AuthenticationSchemes = "UserAuth")]
        public IActionResult UpdateUserDetails(int id)
        {
            if(User.Identity.Name == id.ToString())
            {
                // API Call to get correct user details from DB
                HttpClient httpclient = new HttpClient();
                Task<HttpResponseMessage> message = httpclient.GetAsync("https://localhost:44339/api/Login/GetUserDetails/" + id);
                var strdata = message.Result.Content.ReadAsStringAsync().Result;

                JObject data = JObject.Parse(strdata);

                ViewBag.data = data;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        // Action method for User SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        // User Login
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // User Dashborad
        [Authorize(Roles = ("3"), AuthenticationSchemes = "UserAuth")]
        public IActionResult Dashboard(int id)
        {
            if(User.Identity.Name == id.ToString())
            {
                // API Call to get user's booking data
                HttpClient httpclient = new HttpClient();
                Task<HttpResponseMessage> message = httpclient.GetAsync("https://localhost:44339/api/Dashboard/AllDetails/" + id);
                var strdata = message.Result.Content.ReadAsStringAsync().Result;

                JObject data = JObject.Parse(strdata);

                ViewBag.id = id;
                ViewBag.data = data;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            
        }

        // Select Time Slot, Counter for Booking and Pay
        [Authorize(Roles = ("3"), AuthenticationSchemes = "UserAuth")]
        public IActionResult Pay(int StationId, string date, int UserId)
        {
            if(User.Identity.Name == UserId.ToString())
            {
                HttpClient httpclient = new HttpClient();
                // API call to pay and book the slot
                Task<HttpResponseMessage> message = httpclient.GetAsync("https://localhost:44339/api/Pay?StationId=" + StationId + "&date=" + date);
                // API call to get the station name of the booked slot
                Task<HttpResponseMessage> messageStationName = httpclient.GetAsync("https://localhost:44339/api/Pay/GetStationDetail/" + StationId);
                var strdata = message.Result.Content.ReadAsStringAsync().Result;
                var strStationData = messageStationName.Result.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(strdata);
                ViewBag.SlotDetails = json;
                ViewBag.Date = date;
                ViewBag.UserId = UserId;
                ViewBag.StationName = strStationData;
                ViewBag.StationId = StationId;
                ViewBag.Counters = 4;
                ViewBag.StartTime = 9;
                ViewBag.EndTime = 18;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // Logout for User
        [Authorize(Roles = ("3"), AuthenticationSchemes = "UserAuth")]
        public async Task<IActionResult> Logout(int id)
        {
            if(User.Identity.Name == id.ToString())
            {
                await HttpContext.SignOutAsync("UserAuth");
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
