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
                if (_db.User.Any(i => i.TellNo == user.TellNo))
                {
                    ModelState.AddModelError("TellNo", "شماره تلفن تکراریست");
                }
                else if (_db.User.Any(i => i.IdentificationNo == user.IdentificationNo))
                {
                    ModelState.AddModelError("IdentificationNo", "کدملی تکراریست");
                }
                else
                {
                    TblUser addUser = new TblUser();
                    addUser.Name = user.Name;
                    addUser.TellNo = user.TellNo.Replace(" ", "");
                    addUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
                    addUser.IsDeleted = false;
                    addUser.IdentificationNo = user.IdentificationNo;
                    addUser.DateCreated = DateTime.Now;
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
                    return await Task.FromResult(Redirect("/Admin/User/List?addUser=true"));
                }
            }
            return await Task.FromResult(View(user));
        }
        public async Task<IActionResult> Edit(int id)
        {
            TblUser selectedUser = _db.User.GetById(id);
            EditUserVm user = new EditUserVm()
            {
                Name = selectedUser.Name,
                ImageUrl = selectedUser.ImageUrl,
                IdentificationNo = selectedUser.IdentificationNo,
                TellNo = selectedUser.TellNo,
                UserId = selectedUser.UserId,
            };
            return await Task.FromResult(View(user));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserVm user, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (_db.User.Any(i => i.UserId != user.UserId && i.TellNo == user.TellNo))
                {
                    ModelState.AddModelError("TellNo", "شماره تلفن تکراریست");
                }
                else if (_db.User.Any(i => i.UserId != user.UserId && i.IdentificationNo == user.IdentificationNo))
                {
                    ModelState.AddModelError("IdentificationNo", "کدملی تکراریست");
                }
                else
                {
                    TblUser addUser = _db.User.GetById(user.UserId);
                    addUser.Name = user.Name;
                    addUser.TellNo = user.TellNo.Replace(" ", "");
                    addUser.IdentificationNo = user.IdentificationNo;
                    #region addImage
                    if (imageUrl != null && imageUrl.IsImage() && imageUrl.Length < 20485760)
                    {
                        if (user.ImageUrl != null)
                        {
                            var imagePathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User/", user.ImageUrl);
                            if (System.IO.File.Exists(imagePathDelete))
                            {
                                System.IO.File.Delete(imagePathDelete);
                            }
                        }
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
                    _db.User.Update(addUser);
                    _db.Save();
                    #endregion addToDataBase
                    return await Task.FromResult(Redirect("/Admin/User/List?editUser=true"));
                }
            }
            return await Task.FromResult(View(user));
        }
        public async Task<IActionResult> List()
        {
            return await Task.FromResult(View(_db.User.Get(i => i.IsDeleted == false, j => j.OrderByDescending(k => k.UserId))));
        }

    }
}
