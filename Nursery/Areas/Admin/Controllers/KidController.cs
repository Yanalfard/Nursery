﻿using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class KidController : Controller
    {
        private Core _db = new Core();

        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }
        public IActionResult Index()
        {
            var a = _db.Kid.Get((i) => i.KidId == 0);

            return View();
        }

        public async Task<IActionResult> List(int pageId = 1, string name = null, string nickname = null, string checkedDelete = null)
        {
            ViewBag.name = name;
            ViewBag.nickname = nickname;
            ViewBag.checkedDelete = checkedDelete == "on" ? true : false;
            List<TblKid> list = _db.Kid.Get(orderBy: j => j.OrderByDescending(k => k.KidId)).ToList();
            if (name != null)
            {
                list = list.Where(i => i.Name.Contains(name)).ToList();
            }
            if (nickname != null)
            {
                list = list.Where(i => i.Nickname.Contains(nickname)).ToList();
            }
            list = list.Where(i => i.IsDeleted == ViewBag.checkedDelete).ToList();
            ViewBag.IsRegister = _db.Form.Any(i => i.Priority == 0);
            //List<TblKidFormVm> listKids = new List<TblKidFormVm>();
            //TblForm checkForm = _db.Form.Get(i => i.Priority == 0).FirstOrDefault();
            //foreach (var item in list)
            //{
            //    TblKidFormVm addKid = new TblKidFormVm();
            //    addKid.ImageUrl = item.ImageUrl;
            //    addKid.KidId = item.KidId;
            //    addKid.Name = item.Name;
            //    addKid.Nickname = item.Nickname;
            //    addKid.PageId = item.PageId;
            //    addKid.PageName = item.Page.Name;
            //    //addKid.FormId = checkForm != null ? checkForm.FormId : 0;
            //    //addKid.FormName = checkForm != null ? checkForm.Name : "";
            //    addKid.IsRegister = _db.Form.Any(i => i.Priority == 0);
            //    //TblValue value = _db.Value.Get(i => i.FormField.FormId == checkForm.FormId
            //    //&& i.KidId == item.KidId).FirstOrDefault();
            //    //if (value != null && checkForm != null)
            //    //{
            //    //    addKid.IsFull = true;
            //    //    addKid.TblValue = value;
            //    //}
            //    listKids.Add(addKid);
            //}
            //Pagging
            int take = 10;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
            ViewBag.PageShow = pageId;
            ViewBag.skip = skip;
            return await Task.FromResult(View(list.Skip(skip).Take(take)));
        }


        public async Task<IActionResult> Add()
        {
            ViewBag.RolePageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();

            return await Task.FromResult(View());
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddKidVm kid, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (_db.Kid.Any(i => i.Nickname == kid.Nickname && i.IsDeleted == false))
                {

                    ModelState.AddModelError("Nickname", "نام مستعار تکراریست");
                }
                else
                {
                    #region addImage
                    if (ImageUrl != null && ImageUrl.IsImage() && ImageUrl.Length < 20485760)
                    {
                        kid.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Kid");
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), savePath, kid.ImageUrl);
                        if (!Directory.Exists(savePath))
                        {
                            Directory.CreateDirectory(savePath);
                        }
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await ImageUrl.CopyToAsync(stream);
                        }
                    }
                    #endregion addImage
                    TblKid addKid = new TblKid()
                    {
                        Name = kid.Name,
                        Nickname = kid.Nickname,
                        IsDeleted = false,
                        ImageUrl = kid.ImageUrl == null ? "" : kid.ImageUrl,
                        PageId = kid.PageId,
                    };
                    _db.Kid.Add(addKid);
                    _db.Save();
                    #region Add Log
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.AddKid(SelectUser().IdentificationNo, addKid.Name.ToString(), addKid.Nickname.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 1,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Kid/List?addKid=true"));
                }

            }
            return await Task.FromResult(View(kid));
        }
        public async Task<IActionResult> Edit(int id, string name)
        {
            TblKid selectedKid = _db.Kid.GetById(id);
            AddKidVm kid = new AddKidVm()
            {
                KidId = selectedKid.KidId,
                Name = selectedKid.Name,
                Nickname = selectedKid.Nickname,
                ImageUrl = selectedKid.ImageUrl,
                PageId = (int)selectedKid.PageId,
            };
            ViewBag.RolePageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();

            return await Task.FromResult(View(kid));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AddKidVm kid, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (_db.Kid.Any(i => i.KidId != kid.KidId && i.Nickname == kid.Nickname && i.IsDeleted == false))
                {
                    ModelState.AddModelError("Nickname", "نام مستعار تکراریست");
                }
                else
                {
                    TblKid updateKid = _db.Kid.GetById(kid.KidId);
                    updateKid.Name = kid.Name;
                    updateKid.Nickname = kid.Nickname;
                    updateKid.PageId = kid.PageId;
                    #region addImage
                    if (ImageUrl != null && ImageUrl.IsImage() && ImageUrl.Length < 20485760)
                    {
                        if (updateKid.ImageUrl != null)
                        {
                            var imagePathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Kid/", updateKid.ImageUrl);
                            if (System.IO.File.Exists(imagePathDelete))
                            {
                                System.IO.File.Delete(imagePathDelete);
                            }
                        }
                        kid.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Kid");
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), savePath, kid.ImageUrl);
                        if (!Directory.Exists(savePath))
                        {
                            Directory.CreateDirectory(savePath);
                        }
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await ImageUrl.CopyToAsync(stream);
                        }
                        updateKid.ImageUrl = kid.ImageUrl;
                    }
                    #endregion addImage
                    _db.Kid.Update(updateKid);
                    _db.Save();
                    #region Add Log
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.EditKid(SelectUser().IdentificationNo, updateKid.Name.ToString(), updateKid.Nickname.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 3,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Kid/List?editKid=true"));
                }
            }
            return await Task.FromResult(View(kid));
        }
        public async Task<string> Delete(int id)
        {
            TblKid selectedKid = _db.Kid.GetById(id);
            if (selectedKid != null)
            {
                selectedKid.IsDeleted = true;
                //var imagePathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Kid/", selectedKid.ImageUrl);
                //if (System.IO.File.Exists(imagePathDelete))
                //{
                //    System.IO.File.Delete(imagePathDelete);
                //}
                _db.Kid.Update(selectedKid);
                _db.Save();
                #region Add Log
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.DeleteKid(SelectUser().IdentificationNo, selectedKid.Name.ToString(), selectedKid.Nickname.ToString()),
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
    }
}
