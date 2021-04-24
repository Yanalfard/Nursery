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
                        TblUser user = db.User.Get().FirstOrDefault(i => i.TellNo == login.IdentificationNo);
                        var claims = new List<Claim>()
                    {
                              new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.Name)
                        //new Claim(ClaimTypes.NameIdentifier,user.IdentificationNo.ToString()),
                        //new Claim(ClaimTypes.Name,user.TellNo),
                        ////new Claim(ClaimTypes.Role,db.Role.GetById(user.RoleId).Name.Trim()),
                        //new Claim(ClaimTypes.Role,db.UserRoleRel.GetById(user.UserId).Role.Name.Trim()),
                        //new Claim(ClaimTypes.Role,db.RolePageRel.GetById(user.UserId).Role.Name.Trim()),
                    };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        };
                        await HttpContext.SignInAsync(principal, properties);
                        ViewBag.IsSuccess = true;
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("TellNo", "شماره تلفن  یا رمز اشتباه است");
                    }
                }
                return await Task.FromResult(View(login));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }
    }
}
