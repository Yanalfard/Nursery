using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private Core _db = new Core();
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
                if (name != null)
                {
                    list = list.Where(i => i.Name.Contains(name)).ToList();
                }
                if (titles != null)
                {
                    list = list.Where(i => i.Title.Contains(titles)).ToList();
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
        public async Task<IActionResult> AddAsync(AddRoleVm role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_db.Role.Any(i => i.Name == role.Name.Trim()))
                    {
                        ModelState.AddModelError("Name", "نام شیفت تکراری می باشد");
                    }
                    else
                    {
                        TblRole addRole = new TblRole()
                        {
                            Name = role.Name.Trim(),
                            Title = role.Title.Trim(),
                        };
                        _db.Role.Add(addRole);
                        _db.Save();
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
                    Name = selectedRole.Name.Trim(),
                    Title = selectedRole.Title.Trim(),
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
                    if (_db.Role.Any(i => i.Name == role.Name.Trim() && i.RoleId != role.RoleId))
                    {
                        ModelState.AddModelError("Name", "نام شیفت تکراری می باشد");
                    }
                    else
                    {
                        TblRole selectedRole = _db.Role.GetById(role.RoleId);
                        if (selectedRole != null)
                        {
                            selectedRole.Name = role.Name.Trim();
                            selectedRole.Title = role.Title.Trim();
                            _db.Role.Update(selectedRole);
                            _db.Save();
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
    }
}
