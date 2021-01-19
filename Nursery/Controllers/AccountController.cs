using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Controllers
{
    public class AccountController : Controller
    {
        // Login
        public IActionResult Index()
        {
            return View();
        }

    }
}
