using EVC_Intelligence_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EVC_Intelligence_MVC.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HandleLoginController : ControllerBase
    {
        // Log in the user to the system
        [HttpPost]
        public async Task<IActionResult> PostLogin(LoginData loginData)
        {
            var content = JsonConvert.SerializeObject(loginData);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // API Call to fetch the correct login details of the user based on the email specified
            HttpClient httpclient = new HttpClient();
            Task<HttpResponseMessage> message = httpclient.PostAsync("https://localhost:44339/api/Login/", byteContent);
            var strdata = message.Result.Content.ReadAsStringAsync().Result;

            dynamic data = JsonConvert.DeserializeObject(strdata);
            var claims = new List<Claim>();
            // Setting claims for user
            claims.Add(new Claim("email", data.email.ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, data.email.ToString()));
            claims.Add(new Claim("username", data.name.ToString()));
            claims.Add(new Claim("name", data.email.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, data.id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, data.roleid.ToString()));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //await HttpContext.SignInAsync(claimsPrincipal);

            // Sign In user based on the role
            if (data.roleid == "3")
            {
                await HttpContext.SignInAsync("UserAuth", claimsPrincipal);
            }
            else if (data.roleid == "2")
            {
                await HttpContext.SignInAsync("AdminAuth", claimsPrincipal);
            }
            else if (data.roleid == "1")
            {
                await HttpContext.SignInAsync("SuperAdminAuth", claimsPrincipal);
            }
            return Ok(new { id = data.id.ToString(), returnurl = data.returnUrl.ToString() });
        }
    }
}
