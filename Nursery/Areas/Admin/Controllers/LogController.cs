using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nursery.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LogController : Controller
    {
        private Core _db = new Core();
        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> List(int pageId = 1, string name = null)
        {
            ViewBag.name = name;
            List<TblUserLog> list = _db.UserLog.Get(orderBy: j => j.OrderByDescending(k => k.UserLogId)).ToList();
            if (name != null)
            {
                list = list.Where(i => i.Text.ToLower().Contains(name.ToLower()) || i.User.Name.ToLower().Contains(name.ToLower()) || i.User.IdentificationNo.ToLower().Contains(name.ToLower()) || i.User.TellNo.ToLower().Contains(name.ToLower())).ToList();
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
