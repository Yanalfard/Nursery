using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PageController : Controller
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
                return await Task.FromResult(View(_db.Page.GetById(id)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }

        public async Task<IActionResult> List(int pageId = 1, string name = null)
        {
            try
            {
                ViewBag.name = name;
                List<TblPage> list = _db.Page.Get(orderBy: j => j.OrderByDescending(k => k.PageId)).ToList();
                if (name != null)
                {
                    list = list.Where(i => i.Name.Contains(name)).ToList();
                }
                //Pagging
                int take = 10;
                int skip = (pageId - 1) * take;
                ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
                ViewBag.PageShow = pageId;
                return await Task.FromResult(View(list.Skip(skip).Take(take)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }

        public async Task<IActionResult> Add()
        {
            try
            {
                return await Task.FromResult(View());
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(AddPageVm role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Page.Any(i => i.Name == role.Name.Trim().ToLower() && i.IsDeleted == false))
                    {
                        ModelState.AddModelError("Name", "نام بخش تکراری می باشد");
                    }
                    else
                    {
                        TblPage addPage = new TblPage()
                        {
                            Name = role.Name.Trim(),
                        };
                        _db.Page.Add(addPage);
                        _db.Save();
                        #region Add Log
                        _db.UserLog.Add(new TblUserLog()
                        {
                            Text = LogRepo.AddPage(SelectUser().IdentificationNo, role.Name),
                            UserId = SelectUser().UserId,
                            Type = 1,
                            DateCreated = DateTime.Now
                        });
                        _db.Save();
                        #endregion
                        return await Task.FromResult(Redirect("/Admin/Page/List?addPage=true"));
                    }
                }
                return await Task.FromResult(View(role));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                TblPage selectedRole = _db.Page.GetById(id);
                AddPageVm edit = new AddPageVm()
                {
                    Name = selectedRole.Name.Trim(),
                    PageId = selectedRole.PageId,
                };
                return await Task.FromResult(View(edit));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(AddPageVm role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Page.Any(i => i.PageId != role.PageId && i.Name == role.Name.Trim().ToLower() && i.IsDeleted == false))
                    {
                        ModelState.AddModelError("Name", "نام بخش تکراری می باشد");
                    }
                    else
                    {
                        TblPage selectedPage = _db.Page.GetById(role.PageId);
                        if (selectedPage != null)
                        {
                            selectedPage.Name = role.Name.Trim();
                            _db.Page.Update(selectedPage);
                            _db.Save();
                            #region Add Log
                            _db.UserLog.Add(new TblUserLog()
                            {
                                Text = LogRepo.EditPage(SelectUser().IdentificationNo, role.Name),
                                UserId = SelectUser().UserId,
                                Type = 3,
                                DateCreated = DateTime.Now
                            });
                            _db.Save();
                            #endregion
                            return await Task.FromResult(Redirect("/Admin/Page/List?editPage=true"));
                        }
                    }
                }
                return await Task.FromResult(View(role));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }


        public async Task<string> DeletePageFormRel(int id)
        {
            TblPageFormRel selectedPage = _db.PageFormRel.GetById(id);
            if (selectedPage != null)
            {
                selectedPage.IsDeleted = true;
                _db.PageFormRel.Update(selectedPage);
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.DeletePageFormRel(SelectUser().IdentificationNo, selectedPage.Form.Name.ToString(), selectedPage.Page.Name),
                    UserId = SelectUser().UserId,
                    Type = 2,
                    DateCreated = DateTime.Now
                });
                _db.Save();
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }



        public async Task<IActionResult> AddForm(int PageId, string name = null)
        {
            ViewBag.name = name;
            ViewBag.PageFormRel = _db.Form.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.FormId)).ToList();
            return await Task.FromResult(View(new TblPageFormRel()
            {
                PageId = PageId
            }));
        }
        [HttpPost]
        public async Task<IActionResult> AddFormAsync(TblPageFormRel addPage, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                if (_db.PageFormRel.Any(i => i.FormId == addPage.FormId && i.PageId == addPage.PageId && i.IsDeleted == false))
                {
                    ModelState.AddModelError("FormId", "فرم مورد نظر قبلا به این بخش اضافه شده است");
                }
                else
                {
                    _db.PageFormRel.Add(addPage);
                    _db.Save();
                    #region Add Log
                    TblForm selectedFormEdit = _db.Form.GetById(addPage.FormId);
                    TblPage selectedPageEdit = _db.Page.GetById(addPage.PageId);
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.AddFormPageRel(SelectUser().IdentificationNo, selectedFormEdit.Name.ToString(), selectedPageEdit.Name.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 1,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Page/Index?id=" + addPage.PageId + "&name=" + name));
                }
            }
            ViewBag.PageFormRel = _db.Form.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.FormId)).ToList();
            return await Task.FromResult(View(addPage));
        }


        public async Task<IActionResult> EditForm(int pageId, string name = null)
        {
            ViewBag.name = name;
            ViewBag.PageFormRel = _db.Form.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.FormId)).ToList();
            TblPageFormRel selected = _db.PageFormRel.GetById(pageId);
            return await Task.FromResult(View(selected));
        }
        [HttpPost]
        public async Task<IActionResult> EditFormAsync(TblPageFormRel editPage, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                if (_db.PageFormRel.Any(i => i.PageFormRelId != editPage.PageFormRelId && i.FormId == editPage.FormId && i.PageId == editPage.PageId && i.IsDeleted == false))
                {
                    ModelState.AddModelError("FormId", "فرم مورد نظر قبلا به این بخش اضافه شده است");
                }
                else
                {

                    _db.PageFormRel.Update(editPage);
                    _db.Save();
                    #region Add Log
                    TblForm selectedFormEdit = _db.Form.GetById(editPage.FormId);
                    TblPage selectedPageEdit = _db.Page.GetById(editPage.PageId);
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.EditFormPageRel(SelectUser().IdentificationNo, selectedFormEdit.Name.ToString(), selectedPageEdit.Name.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 3,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Page/Index?id=" + editPage.PageId + "&name=" + name));

                }
            }
            ViewBag.PageFormRel = _db.Form.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.FormId)).ToList();
            return await Task.FromResult(View(editPage));
        }

    }
}
