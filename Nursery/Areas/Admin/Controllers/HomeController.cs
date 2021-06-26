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
    public class HomeController : Controller
    {
        private Core db = new Core();

        TimelineVm[] times =
        {
            new TimelineVm(),
            new TimelineVm(),
            new TimelineVm(),
            new TimelineVm(),
            new TimelineVm()
        };

        public IActionResult Index()
        {
            List<TblValue> selectedListFormFieldRel = db.Value.Get(i => i.IsDeleted == false
       && i.FormField.IsDeleted == false
       && i.FormField.Form.IsDeleted == false, orderBy: i => i.OrderByDescending(i => i.ValueId)).ToList();
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


            DashbordInfoVm countDashbord = new DashbordInfoVm();
            countDashbord.FormCount = db.Form.Get(i => i.IsDeleted == false).ToList().Count();
            countDashbord.RoleCount = db.Role.Get(i => i.IsDeleted == false).ToList().Count();
            countDashbord.PageCount = db.Page.Get(i => i.IsDeleted == false).ToList().Count();
            countDashbord.KidCount = db.Kid.Get(i => i.IsDeleted == false).ToList().Count();
            countDashbord.UserCount = db.User.Get(i => i.IsDeleted == false).ToList().Count();
            countDashbord.ValuesCount = list.Count();
            countDashbord.ValusAcceptCount = list.Count(i => i.IsAccepted == true);

            return View(countDashbord);
        }

        [Obsolete]
        public IActionResult TimelineData()
        {
            return Ok(times);
        }

        public IActionResult Config()
        {
            return View();
        }

    }
}
