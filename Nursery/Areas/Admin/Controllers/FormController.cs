using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
