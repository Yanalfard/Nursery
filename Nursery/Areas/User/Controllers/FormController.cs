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

namespace Nursery.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class FormController : Controller
    {
        private Core _db = new Core();
        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }
        public async Task<IActionResult> Index(int pageId = 1,
            string nameForm = null,
            string nickname = null,
            string startDate = null,
            string endDate = null
            )
        {
            ViewBag.nameForm = nameForm;
            ViewBag.nickname = nickname;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            List<TblValue> selectedListFormFieldRel = _db.Value.Get(i => i.IsDeleted == false
            && i.FormField.IsDeleted == false
            && i.FormField.Form.IsDeleted == false && i.UserId == SelectUser().UserId
            , orderBy: i => i.OrderByDescending(i => i.ValueId)).ToList();
            List<ValueListVm> list = new List<ValueListVm>();
            foreach (var item in selectedListFormFieldRel)
            {
                if (!list.Any(i => i.IndexN == item.IndexNo))
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
                    val.IsAccepted = item.IsAccepted;
                    val.IsDeleted = item.IsDeleted;
                    list.Add(val);
                }
            }

            if (nameForm != null)
            {
                list = list.Where(i => i.Form.Name.Contains(nameForm)).ToList();
            }
            if (nickname != null)
            {
                list = list.Where(i => i.Kid.Name.Contains(nickname)).ToList();
            }
            if (startDate != null)
            {
                PersianCalendar pc = new PersianCalendar();
                string[] Start = startDate.Split('/');
                DateTime startTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                list = list.Where(i => i.DateCreated.Date >= startTime.Date).ToList();
            }
            if (endDate != null)
            {
                PersianCalendar pc = new PersianCalendar();
                string[] Start = endDate.Split('/');
                DateTime endTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                list = list.Where(i => i.DateCreated.Date <= endTime.Date).ToList();
            }

            list = list.Distinct().ToList();
            //Pagging
            int take = 10;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
            ViewBag.PageShow = pageId;
            ViewBag.skip = skip;
            ViewBag.UserRoleRel = _db.Role.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.RoleId)).ToList();
            ViewBag.RolePageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View(list.Skip(skip).Take(take)));
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

        public async Task<IActionResult> ToUsers(int IndexN)
        {
            ViewBag.IndexN = IndexN;
            ViewBag.FormName = _db.Value.Get(i=>i.IndexNo==IndexN).FirstOrDefault().FormField.Form.Name;
            ViewBag.UsersId = _db.User.Get(i => i.UserId != SelectUser().UserId);
            return await Task.FromResult(View());
        }
        [HttpPost]
        public async Task<IActionResult> ToUsers(int IndexN, string comment, List<int> usersId)
        {
            ViewBag.IndexN = IndexN;
            int selecteUserId = SelectUser().UserId;
            foreach (var item in usersId)
            {
                TblRefrence refrence = new TblRefrence();
                refrence.FromId = selecteUserId;
                refrence.ToId = item;
                refrence.DateSubmited = DateTime.Now;
                refrence.IndexNo = IndexN;
                refrence.Comment = comment;
                _db.Refrence.Add(refrence);
                _db.Save();
            }
            return await Task.FromResult(Redirect("/User/Form/Index"));
        }
        public async Task<IActionResult> Refrence()
        {
            List<TblRefrence> refrence = _db.Refrence.Get(i => i.ToId == SelectUser().UserId, orderBy: i => i.OrderByDescending(i => i.RefrenceId)).ToList();
            List<RefrenceVm> refrences = new List<RefrenceVm>();
            foreach (var item in refrence)
            {
                if (!refrences.Any(i => i.IndexNo == item.IndexNo))
                {
                    TblValue value = _db.Value.Get(i => i.IndexNo == item.IndexNo).FirstOrDefault();
                    if (value.UserId != SelectUser().UserId)
                    {
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
                    if (value.UserId == SelectUser().UserId)
                    {
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
            }
            return await Task.FromResult(View(refrences));
        }
    }
}
