using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.ViewModels;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using CORS.ViewModels;
using API.Models;

namespace CORS.Controllers
{
    public class AccountsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44303/api/")
        };
        public IActionResult Index()
        {
            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.IsAvailable)
            {
                if (HttpContext.Session.GetString("Id") != null)
                {
                    return Redirect("~/Views/Shared/_Login.cshtml");
                }
            }
            return View();
        }
        [Route("validate")]
        public IActionResult Validate(LoginViewModel userVM)
        {
            var jsonUserVM = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonUserVM);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var resTask = client.PostAsync("Account/login/", byteContent);
            resTask.Wait();
            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                if (data != "")
                {
                    var handler = new JwtSecurityTokenHandler().ReadJwtToken(data);
                    var account = new LoginVM
                    {
                        Id = handler.Claims.Where(p => p.Type == "id").Select(s => s.Value).FirstOrDefault(),
                        Username = handler.Claims.Where(p => p.Type == "uname").Select(s => s.Value).FirstOrDefault(),
                        Email = handler.Claims.Where(p => p.Type == "email").Select(s => s.Value).FirstOrDefault(),
                        Phone = handler.Claims.Where(p => p.Type == "phone").Select(s => s.Value).FirstOrDefault(),
                        RoleName = handler.Claims.Where(p => p.Type == "lvl").Select(s => s.Value).FirstOrDefault()

                    };

                    if (account.RoleName == "Admin" || account.RoleName == "Customer Service" || account.RoleName == "IT Support" || account.RoleName == "Software Developer" || account.RoleName == "Client")
                    {
                        HttpContext.Session.SetString("id", account.Id);
                        HttpContext.Session.SetString("phone", account.Phone);
                        HttpContext.Session.SetString("uname", account.Username);
                        HttpContext.Session.SetString("email", account.Email);
                        HttpContext.Session.SetString("lvl", account.RoleName);
                        HttpContext.Session.SetString("JWToken", "Bearer " + data);
                        if (account.RoleName == "Admin")
                        {
                            return Json(new { status = true, msg = "Login Successfully !" });
                        }
                        else
                        {
                            return Json(new { status = true, msg = "Login Successfully !" });
                        }
                    }
                    else
                    {
                        return Json(new { status = false, msg = "Please registration first." });
                    }
                }
                else
                {
                    return Json(new { status = false, msg = "The data must be filled" });
                }
            }
            return Redirect("/login");

        }

        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("regisvalidate")]
        public IActionResult Validate(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Username != null)
            {
                var json = JsonConvert.SerializeObject(registerViewModel);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = client.PostAsync("Account/register/", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { status = true, code = result, msg = "Registration success" });
                }
                else
                {
                    return Json(new { status = false, msg = "Something went wrong. Please try again" });
                }
            }
            return Redirect("/register");
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("verify")]
        public IActionResult Verify()
        {
            return View();
        }
        [Route("verif")]
        [HttpPost]
        public IActionResult Verify(User model)
        {
            if (model.SecurityStamp != null)
            {
                var jsonUserVM = JsonConvert.SerializeObject(model);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonUserVM);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var resTask = client.PostAsync("account/verify/", byteContent);
                resTask.Wait();
                var result = resTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    if (data != "")
                    {
                        var json = JsonConvert.DeserializeObject(data).ToString();
                        var account = JsonConvert.DeserializeObject<VerifyViewModel>(json);
                        if (account.RoleName == "Admin" || account.RoleName == "Customer Service" || account.RoleName == "IT Support" || account.RoleName == "Software Developer" || account.RoleName == "Client")
                        {
                            HttpContext.Session.SetString("id", account.Id);
                            HttpContext.Session.SetString("phone", account.Phone);
                            HttpContext.Session.SetString("uname", account.Username);
                            HttpContext.Session.SetString("email", account.Email);
                            HttpContext.Session.SetString("lvl", account.RoleName);
                            if (account.RoleName == "Admin")
                            {
                                return Json(new { status = true, msg = "Well done. Your account has been verified" });
                            }
                            else
                            {
                                return Json(new { status = true, msg = "Well done. Your account has been verified" });
                            }
                        }
                        else
                        {
                            return Json(new { status = false, msg = "Please registration first." });
                        }
                    }
                    else
                    {
                        return Json(new { status = false, msg = "The data must be filled" });
                    }
                }
                else
                {
                    return Json(new { status = false, msg = "Something went wrong" });
                }
            }
            return Redirect("/verify");
        }
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
