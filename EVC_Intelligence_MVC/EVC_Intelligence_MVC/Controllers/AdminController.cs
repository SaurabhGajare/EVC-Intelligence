using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EVC_Intelligence_MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Admin Login
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();        
        }

        // Admin Dashboard
        [Authorize(Roles = ("2"), AuthenticationSchemes = "AdminAuth")]
        public IActionResult Dashboard(int id)
        {
            if(User.Identity.Name == id.ToString())
            {
                // API call to get station details
                HttpClient httpclient = new HttpClient();
                Task<HttpResponseMessage> message = httpclient.GetAsync("https://localhost:44339/api/AdminDashboard/AdminDetails/" + id);
                var strdata = message.Result.Content.ReadAsStringAsync().Result;

                dynamic data = JsonConvert.DeserializeObject(strdata);

                ViewBag.id = id;
                ViewBag.data = data;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Admin");
            }
        }

        //Action to add station
        [Authorize(Roles = ("2"), AuthenticationSchemes = "AdminAuth")]
        public IActionResult AddStation(int id)
        {
            if (User.Identity.Name == id.ToString())
            {
                // API call to Add Station into DB
                HttpClient httpclient = new HttpClient();
                Task<HttpResponseMessage> message = httpclient.GetAsync("https://localhost:44339/api/AddStation");
                var strdata = message.Result.Content.ReadAsStringAsync().Result;

                ViewBag.data = strdata;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Admin");
            }
        }

        // update phone and password for admin
        [Authorize(Roles = ("2"), AuthenticationSchemes = "AdminAuth")]
        public IActionResult UpdateUserDetails(int id)
        {
            if (User.Identity.Name == id.ToString())
            {
                // API Call to get current details of user
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

        // Logout for admin
        [Authorize(Roles = ("2"), AuthenticationSchemes = "AdminAuth")]
        public async Task<IActionResult> Logout(int id)
        {
            if (User.Identity.Name == id.ToString())
            {
                await HttpContext.SignOutAsync("AdminAuth");
                return Redirect("/Admin/Login");
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
    }
}
