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

        public async Task<IActionResult> Index(int formFieldId)
        {
            //List<>
            return await Task.FromResult(View());
        }
        public async Task<IActionResult> List()
        {


            List<TblFormFieldRel> selectedListFormFieldRel = _db.FormFieldRel.Get(i => i.TblValue.Any()).ToList();
            List<ValueListVm> list = new List<ValueListVm>();
            foreach (var item in selectedListFormFieldRel)
            {
                foreach (TblValue j in item.TblValue)
                {
                    //if (list.Any(i => i.DateCreated != j.DateCreated && i.User.UserId != j.UserId))
                    //{
                        ValueListVm val = new ValueListVm();
                        val.FormFieldId = item.FormFieldId;
                        val.Value = j;
                        val.DateCreated = j.DateCreated;
                        val.User = j.User;
                        val.Kid = j.Kid;
                        val.Form = item.Form;
                        val.Page = val.Form?.TblPageFormRel?.Single().Page;
                        val.Role = val.Page?.TblRolePageRel?.Single().Role;
                        list.Add(val);
                  //  }
                }
            }
            return await Task.FromResult(View(list));
        }
    }
}
