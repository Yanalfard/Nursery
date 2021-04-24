﻿using DataLayer.Models;
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

        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }
        public async Task<IActionResult> Index(int id = 0, string name = null)
        {
            ViewData["name"] = name;
            return await Task.FromResult(View(_db.User.GetById(id)));
        }

        public async Task<IActionResult> Add()
        {
            return await Task.FromResult(View());
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
        public async Task<IActionResult> List(int pageId = 1, string name = null, string tell = null, string identificationNo = null, string checkedDelete = null)
        {
           // var userId = Convert.ToInt32(User.Claims.First().Value);
            ViewBag.name = name;
            ViewBag.tell = tell;
            ViewBag.identificationNo = identificationNo;
            ViewBag.checkedDelete = checkedDelete == "on" ? true : false;
            List<TblUser> list = _db.User.Get(orderBy: j => j.OrderByDescending(k => k.UserId)).ToList();
            if (name != null)
            {
                list = list.Where(i => i.Name.Contains(name)).ToList();
            }
            if (tell != null)
            {
                list = list.Where(i => i.TellNo.Contains(tell)).ToList();
            }
            if (identificationNo != null)
            {
                list = list.Where(i => i.IdentificationNo.Contains(identificationNo)).ToList();
            }

            list = list.Where(i => i.IsDeleted == ViewBag.checkedDelete).ToList();
            //Pagging
            int take = 10;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
            ViewBag.PageShow = pageId;
            return await Task.FromResult(PartialView(list.Skip(skip).Take(take)));
        }

        public async Task<IActionResult> ChangePassword(int id, string name = null)
        {
            ViewData["name"] = name;
            return await Task.FromResult(View(new ChangePasswordVm() { UserId = id }));
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm change)
        {
            if (ModelState.IsValid)
            {
                TblUser selectedUserById = _db.User.GetById(change.UserId);
                selectedUserById.Password = PasswordHelper.EncodePasswordMd5(change.Password);
                _db.User.Update(selectedUserById);
                _db.Save();
                return await Task.FromResult(Redirect("/Admin/User/Index?id=" + change.UserId + "&changePassword=true"));
            }
            return await Task.FromResult(View(change));
        }





        public async Task<string> Delete(int id)
        {
            TblUser selectedUser = _db.User.GetById(id);
            if (selectedUser != null)
            {
                selectedUser.IsDeleted = true;
                _db.User.Update(selectedUser);
                _db.Save();
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }

        public async Task<string> DeleteUserRoleId(int id)
        {
            TblUserRoleRel selectedUser = _db.UserRoleRel.GetById(id);
            if (selectedUser != null)
            {
                selectedUser.IsDeleted = true;
                _db.UserRoleRel.Update(selectedUser);
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.DeleteUserRole("", selectedUser.UserRoleId.ToString(), selectedUser.User.IdentificationNo),
                    UserId = SelectUser().UserId,
                    Type=2,
                    DateCreated=DateTime.Now
                });
                _db.Save();
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }


        public async Task<IActionResult> AddRole(int id, string name = null)
        {
            ViewBag.name = name;
            ViewBag.UserRoleRel = _db.Role.Get(orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            return await Task.FromResult(View(new TblUserRoleRel()
            {
                UserId = id
            }));
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(TblUserRoleRel addRole, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                _db.UserRoleRel.Add(addRole);
                _db.Save();
                return await Task.FromResult(Redirect("/Admin/User/Index/" + addRole.UserId + "?name=" + name));

            }
            ViewBag.UserRoleRel = _db.Role.Get(orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            return await Task.FromResult(View());
        }


        public async Task<IActionResult> EditRole(int id, string name = null)
        {
            ViewBag.name = name;
            ViewBag.UserRoleRel = _db.Role.Get(orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            return await Task.FromResult(View(_db.UserRoleRel.GetById(id)));
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(TblUserRoleRel addRole, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                _db.UserRoleRel.Update(addRole);
                _db.Save();
                return await Task.FromResult(Redirect("/Admin/User/Index/" + addRole.UserId + "?name=" + name));
            }
            ViewBag.UserRoleRel = _db.Role.Get(orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            return await Task.FromResult(View());
        }
    }
}
