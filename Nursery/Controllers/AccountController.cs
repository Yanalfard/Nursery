using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using Services.Services;
using DataLayer.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Nursery.Utilities;

namespace Nursery.Controllers
{
    public class AccountController : Controller
    {
        private Core db = new Core();
        // Login
        public IActionResult Index()
        {
            return View();
        }
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/");
            //}
            if (User.Identity.IsAuthenticated)
            {
                if (bool.Parse(User.FindFirstValue("IsAdmin")))
                    return Redirect("/Admin");
                return Redirect("/User");
            }
            return await Task.FromResult(View());
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm login, string ReturnUrl = "/")
        {
            try
            {

                //if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
                //{
                //    ModelState.AddModelError("TellNo", "ورود غیر مجاز");
                //    return View(login);
                //}
                if (ModelState.IsValid)
                {
                    string password = PasswordHelper.EncodePasswordMd5(login.Password);
                    if (db.User.Get().Any(i => i.IdentificationNo == login.IdentificationNo && i.Password == password))
                    {
                        var user = db.User.Get(i => i.IdentificationNo == login.IdentificationNo).FirstOrDefault();
                        if (user.IsDeleted == false)
                        {
                            var claims = new List<Claim>()
                    {
                        //new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        //new Claim(ClaimTypes.Name,user.IdentificationNo)
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.IdentificationNo),
                        //new Claim(ClaimTypes.Role,db.Role.GetById(user.RoleId).Name.Trim()),
                        new Claim(ClaimTypes.Email,user.IdentificationNo.Trim()),
                        new Claim("CodeMeli", user.IdentificationNo),
                        new Claim("IsAdmin", user.IsAdmin.ToString()),

                    };
                            if (user.ImageUrl != null)
                            {
                                claims.Add(new Claim("ImageUser", user.ImageUrl));
                            }
                            else
                            {
                                claims.Add(new Claim("ImageUser", ""));
                            }
                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            var properties = new AuthenticationProperties
                            {
                                IsPersistent = login.RememberMe
                            };
                            await HttpContext.SignInAsync(principal, properties);
                            ViewBag.IsSuccess = true;
                            #region Add Log
                            db.UserLog.Add(new TblUserLog()
                            {
                                Text = LogRepo.UserLogin(user.IdentificationNo),
                                UserId = user.UserId,
                                Type = 1,
                                DateCreated = DateTime.Now
                            });
                            db.Save();
                            if (user.IsAdmin)
                                return Redirect("/Admin");
                            #endregion
                            return Redirect("/User");
                        }
                        else
                        {
                            ModelState.AddModelError("IdentificationNo", "حساب شما حذف شده است");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("IdentificationNo", "کد ملی  یا رمز اشتباه است");
                    }
                }
                return await Task.FromResult(View(login));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }

        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return await Task.FromResult(Redirect("/"));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }
    }
}
