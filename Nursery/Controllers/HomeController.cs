using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            List<DRegexVm> validations = new List<DRegexVm>() {
                new DRegexVm
                (0,"NO","REGEX FAILED"),
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

            ViewData["Data"] = JsonConvert.SerializeObject(formVm);

            return View();
        }

        public IActionResult Demo()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Config()
        {
            return View();
        }


    }
}
