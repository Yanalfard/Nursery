using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        public async Task<IActionResult> Kids(int formId = 1, int pagesId = 1, int pageId = 1, string name = null, string nickname = null)
        {
            ViewBag.FormId = formId;
            ViewBag.PagesId = pagesId;
            List<int> pageIds = _db.Form.GetById(formId).TblPageFormRel?.Select(i => i.PageId).ToList();
            ViewBag.name = name;
            ViewBag.nickname = nickname;
            List<TblKid> list = new List<TblKid>();
            foreach (var item in pageIds)
            {
                list.AddRange(_db.Kid.Get(j => j.PageId == item && j.IsDeleted == false, orderBy: j => j.OrderByDescending(k => k.KidId)).ToList());
            }
            if (name != null)
            {
                list = list.Where(i => i.Name.Contains(name)).ToList();
            }
            if (nickname != null)
            {
                list = list.Where(i => i.Nickname.Contains(nickname)).ToList();
            }
            //Pagging
            int take = 10;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
            ViewBag.PageShow = pageId;
            ViewBag.skip = skip;
            return await Task.FromResult(View(list.Skip(skip).Take(take)));
        }

    }
}
