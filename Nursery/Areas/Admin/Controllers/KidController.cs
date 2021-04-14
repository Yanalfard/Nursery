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
    public class KidController : Controller
    {
        private Core _db = new Core();
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
            //Pagging
            int take = 10;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
            ViewBag.PageShow = pageId;
            return await Task.FromResult(View(list.Skip(skip).Take(take)));
        }


        public async Task<IActionResult> Add()
        {
            return await Task.FromResult(View());
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddKidVm kid)
        {
            if (ModelState.IsValid)
            {
                TblKid addKid = new TblKid()
                {
                    Name = kid.Name,
                    Nickname = kid.Nickname,

                };
                _db.Kid.Add(addKid);
                _db.Save();
                return await Task.FromResult(Redirect("/Admin/Kid/List?addKid=true"));
            }
            return await Task.FromResult(View(kid));
        }
        public async Task<IActionResult> Edit(int id)
        {
            TblKid selectedKid = _db.Kid.GetById(id);
            AddKidVm kid = new AddKidVm()
            {
                KidId = selectedKid.KidId,
                Name = selectedKid.Name,
                Nickname = selectedKid.Nickname,
            };
            return await Task.FromResult(View(kid));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AddKidVm kid)
        {
            if (ModelState.IsValid)
            {
                TblKid updateKid = _db.Kid.GetById(kid.KidId);
                updateKid.Name = kid.Name;
                updateKid.Nickname = kid.Nickname;
                _db.Kid.Update(updateKid);
                _db.Save();
                return await Task.FromResult(Redirect("/Admin/Kid/List?editKid=true"));
            }
            return await Task.FromResult(View(kid));
        }
        public async Task<string> Delete(int id)
        {
            TblKid selectedKid = _db.Kid.GetById(id);
            if (selectedKid != null)
            {
                selectedKid.IsDeleted = true;
                _db.Kid.Update(selectedKid);
                _db.Save();
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }
    }
}
