using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Nursery.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class HomeController : Controller
    {
        private Core db = new Core();

        TimelineVm[] times =
        {
            new TimelineVm(),
            new TimelineVm(),
            new TimelineVm(),
            new TimelineVm(),
            new TimelineVm()
        };

        public IActionResult Index()
        {
            return View();
        }

        [Obsolete]
        public IActionResult TimelineData()
        {
            return Ok(times);
        }

        public IActionResult Config()
        {
            return View();
        }

    }
}
