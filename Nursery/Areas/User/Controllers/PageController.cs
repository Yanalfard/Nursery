using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Nursery.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
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
        [Route("/User/Page/Form/{formID?}/{kidId?}")]
        public IActionResult Form(int? formId, int? kidId)
        {
            if (formId == null || kidId == null)
            {
                return Redirect("/User/Home/Index");
            }
            ViewBag.kidId = kidId;
            TblForm selectedForm = _db.Form.GetById(formId);
            ViewBag.name = selectedForm.Name;
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
                    FieldId = item.FormFieldId,
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

        [HttpPost]
        [Route("/User/Page/Form/SubmitForm")]
        public IActionResult SubmitForm([FromBody] ICollection<DValueVm> values, int kidId)
        {
            //Get user Id User Id
            int userId = SelectUser().UserId;
            var SelectedIndexN = _db.Value.Get(orderBy: i => i.OrderByDescending(i => i.ValueId)).FirstOrDefault();
            int IndexN = 1;
            if (SelectedIndexN != null)
            {
                IndexN = (int)SelectedIndexN.IndexNo + 1;
            }

            List<TblValue> addValue = new List<TblValue>();
            int selectformId = 0;
            foreach (var val in values)
            {
                TblValue addVal = new TblValue();
                addVal.UserId = userId;
                addVal.DateCreated = DateTime.Now;
                addVal.FormFieldId = val.FormFieldId;
                addVal.Value = val.Value;
                addVal.KidId = kidId;
                addVal.IsAccepted = false;
                addVal.IsDeleted = false;
                addVal.IndexNo = IndexN;
                _db.Value.Add(addVal);
                selectformId = val.FormId;
            }
            _db.Save();

            #region Add Log
            string name = _db.Kid.GetById(kidId).Nickname;
            string formName = _db.Form.GetById(selectformId).Name;
            _db.UserLog.Add(new TblUserLog()
            {
                Text = LogRepo.AddFormKid(SelectUser().IdentificationNo, formName, name),
                UserId = SelectUser().UserId,
                Type = 1,
                DateCreated = DateTime.Now
            });
            _db.Save();
            #endregion
            return Ok();
        }

        [HttpPost]
        [Route("/User/Page/OnPostUploadAsync")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            var http = HttpContext;
            long size = files.Sum(f => f.Length);
            List<TblValue> vals = new List<TblValue>();

            foreach (var formFile in files)
            {
                string nameImage = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FilesUploads/");
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), savePath, nameImage);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                vals.Add(new TblValue() { Value = nameImage });
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(vals);
        }

    }
}
