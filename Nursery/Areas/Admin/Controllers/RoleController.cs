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
    public class RoleController : Controller
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
                return await Task.FromResult(View(_db.Role.GetById(id)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }

        public async Task<IActionResult> List(int pageId = 1, string name = null, string titles = null)
        {
            try
            {
                ViewBag.name = name;
                ViewBag.titles = titles;
                List<TblRole> list = _db.Role.Get(orderBy: j => j.OrderByDescending(k => k.RoleId)).ToList();
                if (titles != null)
                {
                    list = list.Where(i => i.Title.Contains(titles)).ToList();
                }
                //Pagging
                int take = 10;
                int skip = (pageId - 1) * take;
                ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
                ViewBag.PageShow = pageId;
                ViewBag.skip = skip;
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
        public async Task<IActionResult> AddAsync(AddRoleVm role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Role.Any(i => i.Title == role.Title && i.IsDeleted == false))
                    {
                        ModelState.AddModelError("Title", "تیتر شیفت تکراری می باشد");
                    }
                    else
                    {
                        TblRole addRole = new TblRole()
                        {
                            Title = role.Title,
                        };
                        _db.Role.Add(addRole);
                        _db.Save();
                        #region Add Log
                        _db.UserLog.Add(new TblUserLog()
                        {
                            Text = LogRepo.AddRole(SelectUser().IdentificationNo, addRole.Title.ToString()),
                            UserId = SelectUser().UserId,
                            Type = 1,
                            DateCreated = DateTime.Now
                        });
                        _db.Save();
                        #endregion
                        return await Task.FromResult(Redirect("/Admin/Role/List?addRole=true"));
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
                TblRole selectedRole = _db.Role.GetById(id);
                AddRoleVm edit = new AddRoleVm()
                {
                    Title = selectedRole.Title,
                    RoleId = selectedRole.RoleId,
                };
                return await Task.FromResult(View(edit));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(AddRoleVm role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Role.Any(i => i.RoleId != role.RoleId && i.Title == role.Title && i.IsDeleted == false))
                    {
                        ModelState.AddModelError("Title", "تیتر شیفت تکراری می باشد");
                    }
                    else
                    {
                        TblRole selectedRole = _db.Role.GetById(role.RoleId);
                        if (selectedRole != null)
                        {
                            selectedRole.Title = role.Title.Trim();
                            _db.Role.Update(selectedRole);
                            _db.Save();
                            #region Add Log
                            _db.UserLog.Add(new TblUserLog()
                            {
                                Text = LogRepo.EditRole(SelectUser().IdentificationNo, selectedRole.Title.ToString()),
                                UserId = SelectUser().UserId,
                                Type = 3,
                                DateCreated = DateTime.Now
                            });
                            _db.Save();
                            #endregion
                            return await Task.FromResult(Redirect("/Admin/Role/List?editRole=true"));
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


        public async Task<IActionResult> AddPage(int id, string name = null)
        {
            ViewBag.name = name;
            ViewBag.RolePageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View(new TblRolePageRel()
            {
                RoleId = id
            }));
        }
        [HttpPost]
        public async Task<IActionResult> AddPageAsync(TblRolePageRel addPage, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                if (_db.RolePageRel.Any(i => i.RoleId == addPage.RoleId && i.PageId == addPage.PageId && i.IsDeleted == false))
                {
                    ModelState.AddModelError("PageId", "بخش مورد نظر به این شیفت اضافه شده است");
                }
                else
                {
                    _db.RolePageRel.Add(addPage);
                    _db.Save();
                    #region Add Log
                    TblRole selectedRoleEdit = _db.Role.GetById(addPage.RoleId);
                    TblPage selectedPageEdit = _db.Page.GetById(addPage.PageId);
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.AddRolePageRel(SelectUser().IdentificationNo, selectedRoleEdit.Title.ToString(), selectedPageEdit.Name.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 1,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Role/Index?id=" + addPage.RoleId + "&name=" + name));
                }

            }
            ViewBag.RolePageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View());
        }


        public async Task<IActionResult> EditPage(int id, string name = null)
        {
            ViewBag.name = name;
            ViewBag.RolePageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View(_db.RolePageRel.GetById(id)));
        }
        [HttpPost]
        public async Task<IActionResult> EditPageAsync(TblRolePageRel editPage, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                if (_db.RolePageRel.Any(i => i.RolePageRelId != editPage.RolePageRelId && i.RoleId == editPage.RoleId && i.PageId == editPage.PageId && i.IsDeleted == false))
                {
                    ModelState.AddModelError("PageId", "بخش مورد نظر به این شیفت اضافه شده است");
                }
                else
                {

                    _db.RolePageRel.Update(editPage);
                    _db.Save();
                    #region Add Log
                    TblRole selectedRoleEdit = _db.Role.GetById(editPage.RoleId);
                    TblPage selectedPageEdit = _db.Page.GetById(editPage.PageId);
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.EditRolePageRel(SelectUser().IdentificationNo, selectedRoleEdit.Title.ToString(), selectedPageEdit.Name.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 3,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Role/Index?id=" + editPage.RoleId + "&name=" + name));

                }
            }
            ViewBag.RolePageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View());
        }


        public async Task<string> DeleteRolePageRelId(int id)
        {
            TblRolePageRel selectedUser = _db.RolePageRel.GetById(id);
            if (selectedUser != null)
            {
                selectedUser.IsDeleted = true;
                _db.RolePageRel.Update(selectedUser);
                TblRole selectedRoleEdit = _db.Role.GetById(selectedUser.RoleId);
                TblPage selectedPageEdit = _db.Page.GetById(selectedUser.PageId);
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.DeleteRolePageRel(SelectUser().IdentificationNo, selectedRoleEdit.Title.ToString(), selectedPageEdit.Name.ToString()),
                    UserId = SelectUser().UserId,
                    Type = 2,
                    DateCreated = DateTime.Now
                });
                _db.Save();
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }
    }
}
