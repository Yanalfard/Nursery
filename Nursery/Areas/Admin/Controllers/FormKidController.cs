using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nursery.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nursery.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
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
                if (!list.Any(i => i.IndexN == item.IndexN))
                {
                    ValueListVm val = new ValueListVm();
                    val.FormFieldId = item.FormFieldId;
                    val.Value = item;
                    val.DateCreated = item.DateCreated;
                    val.User = item.User;
                    val.Kid = item.Kid;
                    val.Form = item?.FormField?.Form;
                    val.Page = val.Form?.TblPageFormRel?.Single().Page;
                    val.Role = val.Page?.TblRolePageRel?.Single().Role;
                    val.IndexN = item.IndexN;
                    list.Add(val);
                }
            }
            return await Task.FromResult(View(list));
        }

        private bool isDatesEqualinMinute(DateTime dt1, DateTime dt2)
        {
            return new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, 0) == new DateTime(dt2.Year, dt2.Month, dt2.Day, dt2.Hour, dt2.Minute, 0);
        }

        public async Task<IActionResult> List()
        {


            List<TblValue> selectedListFormFieldRel = _db.Value.Get().ToList();
            List<ValueListVm> list = new List<ValueListVm>();
            foreach (var item in selectedListFormFieldRel)
            {
                if (!list.Any(i => i.IndexN == item.IndexN))
                {
                    ValueListVm val = new ValueListVm();
                    val.FormFieldId = item.FormFieldId;
                    val.Value = item;
                    val.DateCreated = item.DateCreated;
                    val.User = item.User;
                    val.Kid = item.Kid;
                    val.Form = item?.FormField?.Form;
                    val.Page = val.Form?.TblPageFormRel?.Single().Page;
                    val.Role = val.Page?.TblRolePageRel?.Single().Role;
                    val.IndexN = item.IndexN;
                    list.Add(val);
                }
            }
            return await Task.FromResult(View(list));
        }
    }
}
