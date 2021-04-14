using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.User.Controllers
{
    public class PageController : Controller    
    {
        [Area("User")]
        [Route("/User/Page/{pageId}")] // یا نام پیج هرطور راحت هستید
        public IActionResult Index(int? pageId)
        {
            return View();
        }
    }
}
