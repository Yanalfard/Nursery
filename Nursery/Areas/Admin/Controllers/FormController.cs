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

        public IActionResult Index()
        {
            List<DRegexVm> validations = new List<DRegexVm>() {
                new DRegexVm
                (0,"REGEX NAME","NO","REGEX FAILED"),
            };

            List<DFieldVm> fields = new List<DFieldVm>()
            {
                new DFieldVm
                (1,0,"USERNAME",DFieldType.Text,true,"OPTION1,OPTION2,OPTION3","Enter your username","THIS IS A TOOLTIP",validations),
                new DFieldVm
                (1,0,"PASSWORD",DFieldType.Combo,true,"MOZ,Khiar,Holo,Badimjan","Enter your username","THIS IS A TOOLTIP",validations),
                new DFieldVm
                (1,0,"USERNAME",DFieldType.Range,true,"OPTION1,OPTION2,OPTION3","Enter your username","THIS IS A TOOLTIP",validations)
            };

            DFormVm formVm = new DFormVm
                (0, "FORM TITLE", "FORM SUBTITLE", DateTime.Now, fields);

            DFormVm form = new DFormVm();

            // MEHDIIIIIIIIIIIIIIIIIIII <- 
            ViewData["Data"] = JsonConvert.SerializeObject(formVm);

            return View();
        }


        public IActionResult Add(int id, string name = null)
        {
            List<TblRegex> list = _db.Regex.Get(i => i.IsDeleted == false).ToList();
            foreach (var item in list)
                options.Add(new DRegexVm(item));

            ViewData["Options"] = options.ToList();
            HttpContext.Session.SetComplexData("pageId", id);
            HttpContext.Session.SetComplexData("name", name);
            ViewBag.name = name;
            ViewBag.pageId = id;
            return View();
        }


        //[HttpGet]
        //public IActionResult GetSelectOptions()
        //{
        //    return Json(options);
        //}

        [HttpPost]
        public IActionResult Create([FromBody] DFormVm dform)
        {

            int pageId = HttpContext.Session.GetComplexData<int>("pageId");
            string name = HttpContext.Session.GetComplexData<string>("name");
            TblForm form = new TblForm();
            form.Name = dform.Name;
            form.Body = dform.Body;
            form.DateCreated = DateTime.Now;
            form.IsDeleted = false;

            //form -> db
            // formid
            _db.Form.Add(form);
            _db.Save();


            TblPageFormRel addPage = new TblPageFormRel();
            addPage.PageId = pageId;
            addPage.FormId = form.FormId;
            addPage.IndexNo = 1;
            _db.PageFormRel.Add(addPage);
            _db.Save();
            dform.Fields.ForEach(dfield =>
            {
                TblField field = new TblField();
                field.Label = dfield.Label;
                field.Options = dfield.Options;
                field.Placeholder = dfield.Placeholder;
                field.Tooltip = dfield.Tooltip;
                field.Type = dfield.Type.ToString();
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

            _db.UserLog.Add(new TblUserLog()
            {
                Text = LogRepo.AddPageFormRel(SelectUser().IdentificationNo, form.Name.ToString(), name),
                UserId = SelectUser().UserId,
                Type = 1,
                DateCreated = DateTime.Now
            });
            _db.Save();
            return Ok("LOL");

            // return await Task.FromResult(Redirect("/Admin/Page/Index/" + pageId + "?name=" + name));

        }


        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
