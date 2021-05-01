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


            //List<TblRegex> validations = new List<TblRegex> {
            //    new TblRegex() {
            //        FieldId = 1 ,IsDeleted = false ,
            //        Regex = "" , RegexId = 1 , ValidationMessage = "REGEX FAILED"
            //    },
            //};

            //List<TblField> fields = new List<TblField>()
            //{
            //    new TblField() {
            //        FieldId = 1 , IsDeleted = false, IsRequired = true, Lable ="LABEL", Options="O<O<O<O", PlcaeHolder = "PLACEHOLDER" , Tooltip = "TOOLTIP"
            //    , Type = Enum.GetName(typeof(DFieldType),DFieldType.Text)
            //    }
            //};

            //TblForm formVm = new TblForm()
            //{
            //    Body = "FORM BODY",
            //    DateCreated = DateTime.Now,
            //    FormId = 1,
            //    IsDeleted = false,
            //    Name = "FORM TITLE"
            //};

            // MEHDIIIIIIIIIIIIIIIIIIII <- 
            ViewData["Data"] = JsonConvert.SerializeObject(formVm);

            return View();

        }

        public IActionResult SubmitForm()
        {
            Console.Beep();
            return Ok();
        }
    }
}
