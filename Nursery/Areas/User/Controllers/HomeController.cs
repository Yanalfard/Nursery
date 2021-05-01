using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Nursery.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.User.Controllers
{
    [Area("User")]
    [PermissionChecker("user")]
    public class HomeController : Controller
    {
        private Core _db = new Core();

        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View(SelectUser()));
        }

        public async Task<IActionResult> Profile()
        {
            return await Task.FromResult(View(SelectUser()));
        }
        public async Task<IActionResult> ChangePassword()
        {
            return await Task.FromResult(View());
        }    
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetChangePasswordVm change)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TblUser updateUser = _db.User.GetById(SelectUser().UserId);
                    string pass = PasswordHelper.EncodePasswordMd5(change.OldPassword);
                    if (pass != updateUser.Password)
                    {
                        ModelState.AddModelError("OldPassword", "رمز قدیمی اشتباه است");
                    }
                    else
                    {
                        updateUser.Password = PasswordHelper.EncodePasswordMd5(change.Password);
                        _db.User.Update(updateUser);
                        _db.Save();
                        return await Task.FromResult(Redirect("/User/Home/Profile/Index?ResetPass=true"));
                    }
                }
                return await Task.FromResult(View(change));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }
    }
}
