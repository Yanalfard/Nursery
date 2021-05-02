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

namespace Nursery.Areas.User.Controllers
{
    [Area("User")]
    [PermissionChecker("user")]
    public class PageController : Controller
    {
        private Core _db = new Core();

        TblUser SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblUser selectUser = _db.User.GetById(userId);
            return selectUser;
        }
        [Route("/User/Page/{pageId}")] // یا نام پیج هرطور راحت هستید
        public IActionResult Index(int? pageId)
        {
            return View(_db.Page.GetById(pageId));
        }
        [Route("/User/Page/Form/{formID}")]
        public IActionResult Form(int? formId)
        {
            TblForm selectedForm = _db.Form.GetById(formId);

            List<TblFormFieldRel> listFild = _db.FormFieldRel.Get(i => i.FormId == selectedForm.FormId).ToList();
            List<DFieldVm> fields = new List<DFieldVm>();
            foreach (var item in listFild)
            {
                List<TblRegex> listTblRegex = _db.Regex.Get(i => i.IsDeleted == false).ToList();

                List<DRegexVm> validations = new List<DRegexVm>();
                foreach (var j in listTblRegex)
                {
                    validations.Add(new DRegexVm()
                    {
                        Name = j.Name,
                        Regex = j.Regex,
                        RegexId = j.RegexId,
                        ValidationMessage = j.ValidationMessage
                    });
                }
                // convert string to enum

                Enum.TryParse(item.Field.Type, out DFieldType myStatus);

                fields.Add(new DFieldVm
                {
                    FieldId = item.FieldId,
                    FormId = selectedForm.FormId,
                    IsRequired = (bool)item.Field.IsRequired,
                    Label = item.Field.Label,
                    Options = item.Field.Options,
                    Placeholder = item.Field.Placeholder,
                    Tooltip = item.Field.Tooltip,
                    Type = myStatus,
                    Validations = validations,
                });
            }
            //List<DFieldVm> fields = new List<DFieldVm>()
            //{
            //    new DFieldVm
            //    (1,0,"USERNAME",DFieldType.Text,true,"OPTION1,OPTION2,OPTION3","Enter your username","THIS IS A TOOLTIP",validations),
            //    new DFieldVm
            //    (1,0,"PASSWORD",DFieldType.Combo,true,"MOZ,Khiar,Holo,Badimjan","Enter your username","THIS IS A TOOLTIP",validations),
            //    new DFieldVm
            //    (1,0,"USERNAME",DFieldType.Range,true,"OPTION1,OPTION2,OPTION3","Enter your username","THIS IS A TOOLTIP",validations)
            //};

            //DFormVm formVm = new DFormVm
            //    (0, "FORM TITLE", "FORM SUBTITLE", DateTime.Now, fields);

            DFormVm form = new DFormVm();
            form.FormId = selectedForm.FormId;
            form.Body = selectedForm.Body;
            form.DateCreated = (DateTime)selectedForm.DateCreated;
            form.Name = selectedForm.Name;
            form.Fields = fields;


            // MEHDIIIIIIIIIIIIIIIIIIII <- 
            ViewData["Data"] = JsonConvert.SerializeObject(form);

            return View();

        }

        public IActionResult SubmitForm()
        {
            Console.Beep();
            return Ok();
        }
    }
}
