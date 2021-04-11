using DataLayer.Models;
using DataLayer.ViewModels;
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
    public class UserController : Controller
    {
        private Core _db = new Core();
        public IActionResult Index(int id = 0)
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddUserVm user, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                TblUser addUser = new TblUser();
                addUser.Name = user.Name;
                addUser.TellNo = user.TellNo.Replace(" ", "");
                addUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
                addUser.IsDeleted = false;
                addUser.IdentificationNo = user.IdentificationNo;
                #region addImage
                if (imageUrl != null && imageUrl.IsImage() && imageUrl.Length < 20485760)
                {
                    addUser.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(imageUrl.FileName);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User");
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), savePath, addUser.ImageUrl);
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageUrl.CopyToAsync(stream);
                    }
                }
                #endregion addImage
                #region addToDataBase
                _db.User.Add(addUser);
                _db.Save();
                #endregion addToDataBase
                return await Task.FromResult(RedirectToAction("List?addUser=true"));
            }
            return await Task.FromResult(View(user));
        }
        public IActionResult Edit()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            return await Task.FromResult(View(_db.User.Get()));
        }

    }
}
