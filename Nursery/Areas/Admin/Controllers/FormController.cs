using DataLayer.Models;
using DataLayer.ViewModels;
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
    [PermissionChecker("admin")]
    public class FormController : Controller
    {
        private Core _db = new Core();

        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }

        List<DRegexVm> options = new List<DRegexVm>();

        public IActionResult Index(int? id = 0)
        {
            List<TblRegex> list = _db.Regex.Get(i => i.IsDeleted == false).ToList();
            foreach (var item in list)
                options.Add(new DRegexVm(item));

            ViewData["Options"] = options.ToList();
            ViewData["pageId"] = id;
            return View();
        }



        //[HttpGet]
        //public IActionResult GetSelectOptions()
        //{
        //    return Json(options);
        //}

        [HttpPost]
        public IActionResult Create([FromBody] DFormVm dform,int pageId)
        {
            TblForm form = new TblForm();
            form.Name = dform.Name;
            form.Body = dform.Body;
            form.DateCreated = DateTime.Now;
            form.IsDeleted = false;

            //form -> db
            // formid
            _db.Form.Add(form);
            _db.Save();
            dform.Fields.ForEach(dfield =>
            {
                TblField field = new TblField();
                field.Label = dfield.Label;
                field.Options = dfield.Options;
                field.Placeholder = dfield.Placeholder;
                field.Tooltip = dfield.Tooltip;
                field.Type = nameof(dfield.Type).ToLower();
                field.IsDeleted = false;



                //field -> db
                _db.Field.Add(field);
                _db.Save();


                TblFormFieldRel relFormFieldRel = new TblFormFieldRel();
                relFormFieldRel.FieldId = field.FieldId;
                relFormFieldRel.FormId = form.FormId;
                _db.FormFieldRel.Add(relFormFieldRel);
                _db.Save();

                dfield.Validations.ForEach(dvalidation =>
                {

                    //TblRegex regex = new TblRegex();
                    //regex.Name = dvalidation.Name;
                    //regex.IsDeleted = false;
                    //regex.Regex = dvalidation.Regex;
                    //regex.ValidationMessage = dvalidation.ValidationMessage;

                    //regex -> db
                    //_db.Regex.Add(regex);
                    //_db.Save();

                    TblFieldRegexRel rel = new TblFieldRegexRel();
                    rel.FieldId = field.FieldId;
                    rel.RegexId = dvalidation.RegexId;

                    //regexFieldRel -> db
                    _db.FieldRegexRel.Add(rel);
                    _db.Save();

                });

            });

            return Ok("LOL");
        }

    }
}
