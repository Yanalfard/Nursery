using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace Nursery.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RefrenceController : Controller
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
            return View();
        }
        public async Task<IActionResult> ShowForm(int indexN)
        {
            //List<>
            List<TblValue> selectedListFormFieldRel = _db.Value.Get(i => i.IndexNo == indexN).ToList();
            List<ValueListVm> list = new List<ValueListVm>();
            foreach (var item in selectedListFormFieldRel)
            {
                ValueListVm val = new ValueListVm();
                val.FormFieldId = item.FormFieldId;
                val.Value = item;
                val.DateCreated = (DateTime)item.DateCreated;
                val.User = item.User;
                val.Kid = item.Kid;
                val.Form = item?.FormField?.Form;
                val.Page = val.Form?.TblPageFormRel?.FirstOrDefault()?.Page;
                val.Role = val.Page?.TblRolePageRel?.FirstOrDefault()?.Role;
                val.IndexN = item.IndexNo;
                list.Add(val);
            }
            return await Task.FromResult(View(list));
        }
        public async Task<IActionResult> Refrence()
        {
            List<TblRefrence> refrence = _db.Refrence.Get(orderBy: i => i.OrderByDescending(i => i.RefrenceId)).ToList();
            List<RefrenceVm> refrences = new List<RefrenceVm>();
            foreach (var item in refrence)
            {
                if (!refrences.Any(i => i.IndexNo == item.IndexNo))
                {
                    TblValue value = _db.Value.Get(i => i.IndexNo == item.IndexNo).FirstOrDefault();
                    RefrenceVm addRefrence = new RefrenceVm();
                    addRefrence.Form = value.FormField?.Form;
                    addRefrence.Refrence = item;
                    addRefrence.User = item.From;
                    addRefrence.Kid = value.Kid;
                    addRefrence.IndexNo = (int)item.IndexNo;
                    addRefrence.PageName = value.Kid.Page.Name;
                    addRefrence.IsEnded = _db.Refrence.Get(i => i.IndexNo == item.IndexNo).Any(i => i.IsEnded);
                    refrences.Add(addRefrence);
                }
            }
            return await Task.FromResult(View(refrences));
        }
        public async Task<IActionResult> ShowRefrence(int indexN)
        {

            RefrenceAndValueVm refrenceAndValueVm = new RefrenceAndValueVm();
            List<TblRefrence> refrence = _db.Refrence.Get(i => i.IndexNo == indexN, orderBy: i => i.OrderByDescending(i => i.RefrenceId)).ToList();
            ViewBag.IsEnded = refrence.Any(i => i.IsEnded);
            //////////////////
            ///
            List<TblValue> selectedListFormFieldRel = _db.Value.Get(i => i.IndexNo == indexN).ToList();
            List<ValueListVm> list = new List<ValueListVm>();
            foreach (var item in selectedListFormFieldRel)
            {
                ValueListVm val = new ValueListVm();
                val.FormFieldId = item.FormFieldId;
                val.Value = item;
                val.DateCreated = (DateTime)item.DateCreated;
                val.User = item.User;
                val.Kid = item.Kid;
                val.Form = item?.FormField?.Form;
                val.Page = val.Form?.TblPageFormRel?.FirstOrDefault()?.Page;
                val.Role = val.Page?.TblRolePageRel?.FirstOrDefault()?.Role;
                val.IndexN = item.IndexNo;
                list.Add(val);
            }

            refrenceAndValueVm.TblRefrence = refrence;
            refrenceAndValueVm.ValueListVm = list;
            return await Task.FromResult(View(refrenceAndValueVm));

        }

        public async Task<IActionResult> MyRefrence()
        {
            List<TblRefrence> refrence = _db.Refrence.Get(i => i.FromId == SelectUser().UserId || i.ToId == SelectUser().UserId, orderBy: i => i.OrderByDescending(i => i.RefrenceId)).ToList();
            List<RefrenceVm> refrences = new List<RefrenceVm>();
            foreach (var item in refrence)
            {
                if (!refrences.Any(i => i.IndexNo == item.IndexNo))
                {
                    TblValue value = _db.Value.Get(i => i.IndexNo == item.IndexNo).FirstOrDefault();
                    RefrenceVm addRefrence = new RefrenceVm();
                    addRefrence.Form = value.FormField?.Form;
                    addRefrence.Refrence = item;
                    addRefrence.User = item.From;
                    addRefrence.Kid = value.Kid;
                    addRefrence.IndexNo = (int)item.IndexNo;
                    addRefrence.PageName = value.Kid.Page.Name;
                    addRefrence.IsEnded = _db.Refrence.Get(i => i.IndexNo == item.IndexNo).Any(i => i.IsEnded);
                    refrences.Add(addRefrence);
                }
            }
            return await Task.FromResult(View(refrences));
        }

    }
}
