using DataLayer.Models;
using DataLayer.ViewModels;
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

        DRegexVm[] options =
        {
            new DRegexVm(1,"Just numbers","[0-9]*4","This was Wrong"),
            new DRegexVm(2,"Just Characters","[a-z]","Character"),
            new DRegexVm(3,"Hello","[ha-ea-sd]","Bye"),
        };

        [HttpGet]
        public IActionResult GetSelectOptions()
        {
            return Json(options);
        }

    }
}
