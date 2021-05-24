using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nursery.Utilities;
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
    public class FormKidController : Controller
    {
        private Core _db = new Core();
        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }

        public async Task<IActionResult> Index(int indexN)
        {
            //List<>
            List<TblValue> selectedListFormFieldRel = _db.Value.Get(i => i.IndexN == indexN).ToList();
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
                val.IndexN = item.IndexN;
                list.Add(val);
            }
            return await Task.FromResult(View(list));
        }

        private bool isDatesEqualinMinute(DateTime dt1, DateTime dt2)
        {
            return new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, 0) == new DateTime(dt2.Year, dt2.Month, dt2.Day, dt2.Hour, dt2.Minute, 0);
        }

        public async Task<IActionResult> List(int pageId = 1,
            string nameForm = null,
            string nameUser = null,
            string identificationNo = null,
            string nickname = null,
            int roleName = 0,
            int pageName = 0,
            string checkedDelete = null,
            string checkedOffAccepted = null,
            string checkedOnAccepted = null,
            string startDate = null,
            string endDate = null
            )
        {
            ViewBag.nameForm = nameForm;
            ViewBag.nameUser = nameUser;
            ViewBag.identificationNo = identificationNo;
            ViewBag.nickname = nickname;
            ViewBag.roleName = roleName;
            ViewBag.pageName = pageName;
            ViewBag.checkedDelete = checkedDelete == "on" ? true : false;
            ViewBag.checkedOnAccepted = checkedOnAccepted == "on" ? true : false;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            List<TblValue> selectedListFormFieldRel = _db.Value.Get(i => i.IsDeleted == false
            && i.FormField.IsDeleted == false
            && i.FormField.Form.IsDeleted == false, orderBy: i => i.OrderByDescending(i => i.ValueId)).ToList();
            List<ValueListVm> list = new List<ValueListVm>();
            foreach (var item in selectedListFormFieldRel)
            {
                if (!list.Any(i => i.IndexN == item.IndexN))
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
                    val.IndexN = item.IndexN;
                    val.IsAccepted = item.IsAccepted;
                    val.IsDeleted = item.IsDeleted;
                    list.Add(val);
                }
            }

            if (nameForm != null)
            {
                list = list.Where(i => i.Form.Name.Contains(nameForm)).ToList();
            }
            if (nameUser != null)
            {
                list = list.Where(i => i.User.Name.Contains(nameUser)).ToList();
            }
            if (identificationNo != null)
            {
                list = list.Where(i => i.User.IdentificationNo.Contains(identificationNo)).ToList();
            }
            if (nickname != null)
            {
                list = list.Where(i => i.Kid.Name.Contains(nickname)).ToList();
            }
            if (roleName != 0)
            {
                list = list.Where(i => i.Role.RoleId == roleName).ToList();
            }
            if (pageName != 0)
            {
                list = list.Where(i => i.Page.PageId == pageName).ToList();
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

            list = list.Where(i => i.IsDeleted == ViewBag.checkedDelete).ToList();
            list = list.Where(i => i.IsAccepted == ViewBag.checkedOnAccepted).ToList();
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


        public async Task<string> Delete(int id)
        {
            List<TblValue> selectedUser = _db.Value.Get(i => i.IndexN == id).ToList();
            if (selectedUser != null)
            {
                string kidName = "";
                string userName = "";
                foreach (var item in selectedUser)
                {
                    userName = item.User.Name;
                    kidName = item.Kid.Nickname;
                    item.IsDeleted = true;
                    _db.Value.Update(item);
                }
                _db.Save();
                #region Add Log
                var addLog = new TblUserLog();
                addLog.UserId = SelectUser().UserId;
                addLog.Type = 2;
                addLog.DateCreated = DateTime.Now;
                addLog.Text = LogRepo.DeleteFormKid(SelectUser().IdentificationNo, kidName, userName);
                _db.UserLog.Add(addLog);
                //_db.UserLog.Add(new TblUserLog()
                //{
                //    Text = LogRepo.DeleteFormKid(SelectUser().IdentificationNo, selectedUser.SingleOrDefault().Kid.Nickname, selectedUser.SingleOrDefault().User.Name),
                //    UserId = SelectUser().UserId,
                //    Type = 2,
                //    DateCreated = DateTime.Now
                //});
                _db.Save();
                #endregion
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }

        public async Task<IActionResult> IsAccepted(int id, int value)
        {
            List<TblValue> selectedUser = _db.Value.Get(i => i.IndexN == id).ToList();
            if (selectedUser != null)
            {
                if (value == 1)
                {
                    string kidName = "";
                    string userName = "";
                    foreach (var item in selectedUser)
                    {
                        userName = item.User.Name;
                        kidName = item.Kid.Nickname;
                        item.IsAccepted = true;
                        _db.Value.Update(item);
                    }
                    _db.Save();
                    #region Add Log
                    var addLog = new TblUserLog();
                    addLog.UserId = SelectUser().UserId;
                    addLog.Type = 3;
                    addLog.DateCreated = DateTime.Now;
                    addLog.Text = LogRepo.UpdateIsAcceptedFormKid(SelectUser().IdentificationNo, kidName, userName);
                    _db.UserLog.Add(addLog);
                    _db.Save();
                    #endregion
                }
                else if (value == 0)
                {
                    string kidName = "";
                    string userName = "";
                    foreach (var item in selectedUser)
                    {
                        userName = item.User.Name;
                        kidName = item.Kid.Nickname;
                        item.IsAccepted = false;
                        _db.Value.Update(item);
                    }
                    _db.Save();
                    #region Add Log
                    var addLog = new TblUserLog();
                    addLog.UserId = SelectUser().UserId;
                    addLog.Type = 3;
                    addLog.DateCreated = DateTime.Now;
                    addLog.Text = LogRepo.UpdateOffAcceptedFormKid(SelectUser().IdentificationNo, kidName, userName);
                    _db.UserLog.Add(addLog);
                    _db.Save();
                    #endregion
                }

            }
            return await Task.FromResult(Redirect("/Admin/FormKid/Index?indexN=" + id));

        }

    }
}
