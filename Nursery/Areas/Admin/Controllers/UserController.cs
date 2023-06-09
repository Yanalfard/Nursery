﻿using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nursery.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nursery.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
            try
            {
                ViewData["name"] = name;
                return await Task.FromResult(View(_db.User.GetById(id)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

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
                if (_db.User.Any(i => i.TellNo == user.TellNo && i.IsDeleted == false))
                {
                    ModelState.AddModelError("TellNo", "شماره تلفن تکراریست");
                }
                else if (_db.User.Any(i => i.IdentificationNo == user.IdentificationNo && i.IsDeleted == false))
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
                    addUser.IsAdmin = user.IsAdmin;
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


                    #region Add Log
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.AddUser(SelectUser().IdentificationNo, addUser.IdentificationNo.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 1,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion

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
                IsAdmin = selectedUser.IsAdmin
            };
            return await Task.FromResult(View(user));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserVm user, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (_db.User.Any(i => i.UserId != user.UserId && i.TellNo == user.TellNo && i.IsDeleted == false))
                {
                    ModelState.AddModelError("TellNo", "شماره تلفن تکراریست");
                }
                else if (_db.User.Any(i => i.UserId != user.UserId && i.IdentificationNo == user.IdentificationNo && i.IsDeleted == false))
                {
                    ModelState.AddModelError("IdentificationNo", "کدملی تکراریست");
                }
                else
                {
                    TblUser addUser = _db.User.GetById(user.UserId);
                    addUser.Name = user.Name;
                    addUser.TellNo = user.TellNo.Replace(" ", "");
                    addUser.IdentificationNo = user.IdentificationNo;
                    addUser.IsAdmin = user.IsAdmin;
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


                    #region Add Log
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.EditUser(SelectUser().IdentificationNo, addUser.IdentificationNo.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 3,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion

                    return await Task.FromResult(Redirect("/Admin/User/List?editUser=true"));
                }
            }
            return await Task.FromResult(View(user));
        }
        public async Task<IActionResult> List(int pageId = 1, string name = null, string tell = null, string identificationNo = null, string checkedDelete = null)
        {
            //var claimIdentity = this.User.Identity as ClaimsIdentity;
            //IEnumerable<Claim> claims = claimIdentity.Claims;

            //var userId = Convert.ToInt32(User.Claims.First().Value);
            //var names = User.Identities.First().Name;
            //var currentUserID = claimIdentity.FindFirst(ClaimTypes.Email).Value;
            ViewBag.name = name;
            ViewBag.tell = tell;
            ViewBag.identificationNo = identificationNo;
            ViewBag.checkedDelete = checkedDelete == "on" ? true : false;
            // List<TblUser> list = _db.User.Get(j => j.IsAdmin == false, orderBy: j => j.OrderByDescending(k => k.UserId)).ToList();
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
            ViewBag.skip = skip;
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
                #region Add Log
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.EditUserPassword(SelectUser().IdentificationNo, selectedUserById.IdentificationNo.ToString()),
                    UserId = SelectUser().UserId,
                    Type = 3,
                    DateCreated = DateTime.Now
                });
                _db.Save();
                #endregion
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
                #region Add Log
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.DeleteUser(SelectUser().IdentificationNo, selectedUser.IdentificationNo.ToString()),
                    UserId = SelectUser().UserId,
                    Type = 2,
                    DateCreated = DateTime.Now
                });
                _db.Save();
                #endregion
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
                    Text = LogRepo.DeleteUserRole(SelectUser().IdentificationNo, selectedUser.UserRoleId.ToString(), selectedUser.User.IdentificationNo),
                    UserId = SelectUser().UserId,
                    Type = 2,
                    DateCreated = DateTime.Now
                });
                _db.Save();
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }


        public async Task<IActionResult> AddRole(int id, string name = null)
        {
            ViewBag.name = name;
            ViewBag.UserRoleRel = _db.Role.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            return await Task.FromResult(View(new TblUserRoleRel()
            {
                UserId = id
            }));
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(TblUserRoleRel addRole, List<DateTime> shiftDatas, string name = null)
        {
            ViewBag.name = name;
            bool checkedIsDate = !addRole.IsShiftPreminent;

            if (_db.UserRoleRel.Any(i => i.RoleId == addRole.RoleId && !checkedIsDate && i.IsShiftPreminent && i.UserId == addRole.UserId && i.IsDeleted == false))
            {
                ModelState.AddModelError("RoleId", "این شیفت به این کاربر  قبلا اضافه شده است");
            }
            else
            {
                shiftDatas = shiftDatas.Where(i => !string.IsNullOrEmpty(i.ToString())).ToList();
                if (addRole.IsShiftPreminent == false && !shiftDatas.Any())
                {
                    checkedIsDate = true;
                    ModelState.AddModelError("IsShiftPreminent", " تاریخ وارد نشده است");
                }
                else
                {
                    if (addRole.IsShiftPreminent == false && shiftDatas.Any())
                    {
                        foreach (var item in shiftDatas)
                        {
                            PersianCalendar pc = new PersianCalendar();
                            string[] Start = Convert.ToString(item).Split(' ')[0].Split('/');
                            DateTime startTime = pc.ToDateTime(Convert.ToInt32(Start[2]), Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), 0, 0, 0, 0).Date;
                            if (startTime.Date >= DateTime.Now.Date)
                            {
                                if (!_db.UserRoleRel.Any(i => i.ShiftDate == startTime.Date
                                && i.RoleId == addRole.RoleId
                                && i.UserId == addRole.UserId 
                                && i.IsDeleted == false))
                                {
                                    addRole.ShiftDate = startTime.Date;
                                    TblUserRoleRel addTblUserRoleRel = new TblUserRoleRel();
                                    addTblUserRoleRel.RoleId = addRole.RoleId;
                                    addTblUserRoleRel.UserId = addRole.UserId;
                                    addTblUserRoleRel.ShiftDate = addRole.ShiftDate;
                                    _db.UserRoleRel.Add(addTblUserRoleRel);
                                    _db.Save();
                                }
                            }
                        }
                    }
                    else
                    {
                        _db.UserRoleRel.Add(addRole);
                        _db.Save();
                    }
                    //if (addRole.ShiftDate != null)
                    //{
                    //    PersianCalendar pc = new PersianCalendar();
                    //    string[] Start = Convert.ToString(addRole.ShiftDate).Split(' ')[0].Split('/');
                    //    DateTime startTime = pc.ToDateTime(Convert.ToInt32(Start[2]), Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), 0, 0, 0, 0).Date;
                    //    addRole.ShiftDate = startTime.Date;
                    //}

                    #region Add Log
                    TblRole selectedRoleEdit = _db.Role.GetById(addRole.RoleId);
                    TblUser selectedUserEdit = _db.User.GetById(addRole.UserId);
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.AddUserRoleRel(SelectUser().IdentificationNo, selectedRoleEdit.Title.ToString(), selectedUserEdit.IdentificationNo.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 1,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/User/Index?id=" + addRole.UserId + "&name=" + name + "&addRoleInUser=true"));

                }
            }
            ViewBag.checkedIsDate = checkedIsDate;
            ViewBag.UserRoleRel = _db.Role.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            return await Task.FromResult(View(addRole));
        }


        public async Task<IActionResult> EditRole(int id, string name = null, string nameUser = null)
        {
            ViewBag.name = name;
            ViewBag.nameUser = nameUser;
            ViewBag.UserRoleRel = _db.Role.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            ViewBag.checkedIsDate = false;
            TblUserRoleRel role = _db.UserRoleRel.GetById(id);
            if (!role.IsShiftPreminent)
            {
                ViewBag.checkedIsDate = true;
            }
            return await Task.FromResult(View(role));
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(TblUserRoleRel addRole, string name = null)
        {
            ViewBag.name = name;
            ViewBag.checkedIsDate = false;
            if (!addRole.IsShiftPreminent)
            {
                ViewBag.checkedIsDate = true;
            }
            if (ModelState.IsValid)
            {
                if (_db.UserRoleRel.Any(i => i.UserRoleId != addRole.UserRoleId && i.UserId == addRole.UserId && i.RoleId == addRole.RoleId && i.IsDeleted == false))
                {
                    ModelState.AddModelError("RoleId", "این شیفت به این کاربر  قبلا اضافه شده است");
                }
                else
                {
                    if (addRole.IsShiftPreminent == false && addRole.ShiftDate == null)
                    {
                        ViewBag.checkedIsDate = true;
                        ModelState.AddModelError("IsShiftPreminent", " تاریخ وارد نشده است");
                    }
                    else
                    {
                        if (addRole.ShiftDate != null)
                        {
                            PersianCalendar pc = new PersianCalendar();
                            string[] Start = Convert.ToString(addRole.ShiftDate).Split(' ')[0].Split('/');
                            DateTime startTime = pc.ToDateTime(Convert.ToInt32(Start[2]), Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), 0, 0, 0, 0).Date;
                            addRole.ShiftDate = startTime.Date;
                        }
                        _db.UserRoleRel.Update(addRole);
                        _db.Save();
                        #region Add Log
                        TblRole selectedRoleEdit = _db.Role.GetById(addRole.RoleId);
                        TblUser selectedUserEdit = _db.User.GetById(addRole.UserId);
                        _db.UserLog.Add(new TblUserLog()
                        {
                            Text = LogRepo.EditUserRoleRel(SelectUser().IdentificationNo, selectedRoleEdit.Title.ToString(), selectedUserEdit.IdentificationNo.ToString()),
                            UserId = SelectUser().UserId,
                            Type = 3,
                            DateCreated = DateTime.Now
                        });
                        _db.Save();
                        #endregion
                        return await Task.FromResult(Redirect("/Admin/User/Index?id=" + addRole.UserId + "&name=" + name + "&editRoleInUser=true"));

                    }

                }
            }
            ViewBag.UserRoleRel = _db.Role.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            return await Task.FromResult(View(addRole));
        }
    }
}
