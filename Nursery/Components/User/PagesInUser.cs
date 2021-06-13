using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
namespace Nursery.Components.User
{
    public class PagesInUser : ViewComponent
    {
        private Core _db = new Core();

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            List<TblPage> list = new List<TblPage>();
            TblUser selectedUser = _db.User.GetById(id);
            foreach (var item in selectedUser.TblUserRoleRel.Where(i => i.IsShiftPreminent || i.ShiftDate.Value.TimeOfDay == DateTime.Now.TimeOfDay))
            {
                foreach (var j in item.Role.TblRolePageRel)
                {
                    list.Add(j.Page);
                }
            }
            return await Task.FromResult(View("~/Areas/User/Views/Shared/Components/PagesInUser.cshtml", list));
        }
    }
}
