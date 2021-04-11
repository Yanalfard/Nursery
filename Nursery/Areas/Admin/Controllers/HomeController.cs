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
    public class HomeController : Controller
    {
        private Core db = new Core();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Config()
        {
            return View();
        }
    }
}
