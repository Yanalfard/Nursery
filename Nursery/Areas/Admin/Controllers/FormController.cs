using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

        public async Task<IActionResult> Index(int id = 0, string name = null)
        {
            try
            {
                ViewData["name"] = name;
                return await Task.FromResult(View(_db.Form.GetById(id)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }


        public async Task<IActionResult> Add(int id, string name = null)
        {
            try
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
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }


        //[HttpGet]
        //public IActionResult GetSelectOptions()
        //{
        //    return Json(options);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DFormVm dform)
        {
            try
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


                if (pageId > 0)
                {
                    TblPageFormRel addPage = new TblPageFormRel();
                    addPage.PageId = pageId;
                    addPage.FormId = form.FormId;
                    addPage.IndexNo = 1;
                    _db.PageFormRel.Add(addPage);
                    _db.Save();
                }
                dform.Fields.ForEach(dfield =>
                {
                    TblField field = new TblField();
                    field.Label = dfield.Label;
                    field.Options = dfield.Options;
                    field.Placeholder = dfield.Placeholder;
                    field.Tooltip = dfield.Tooltip;
                    field.Type = dfield.Type.ToString();
                    field.IsDeleted = false;
                    field.IsRequired = dfield.IsRequired;

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
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

            // return await Task.FromResult(Redirect("/Admin/Page/Index/" + pageId + "&name=" + name));

        }


        public async Task<IActionResult> List(int pageId = 1, int id = 0, string description = null, string name = null, string checkedDelete = null)
        {
            try
            {
                //var claimIdentity = this.User.Identity as ClaimsIdentity;
                //IEnumerable<Claim> claims = claimIdentity.Claims;

                //var userId = Convert.ToInt32(User.Claims.First().Value);
                //var names = User.Identities.First().Name;
                //var currentUserID = claimIdentity.FindFirst(ClaimTypes.Email).Value;
                ViewBag.name = name;
                ViewBag.id = id;
                ViewBag.description = description;
                ViewBag.checkedDelete = checkedDelete == "on" ? true : false;
                List<TblForm> list = _db.Form.Get(orderBy: j => j.OrderByDescending(k => k.FormId)).ToList();
                if (name != null)
                {
                    list = list.Where(i => i.Name.Contains(name)).ToList();
                }
                if (description != null)
                {
                    list = list.Where(i => i.Body.Contains(description)).ToList();
                }
                if (id != 0)
                {
                    list = list.Where(i => i.FormId == id).ToList();
                }
                list = list.Where(i => i.IsDeleted == ViewBag.checkedDelete).ToList();
                //Pagging
                int take = 10;
                int skip = (pageId - 1) * take;
                ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
                ViewBag.PageShow = pageId;
                return await Task.FromResult(PartialView(list.Skip(skip).Take(take)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return await Task.FromResult(View(_db.Form.GetById(id)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblForm form)
        {
            if (ModelState.IsValid)
            {
                //TblForm selectedForm = _db.Form.GetById(id);
                _db.Form.Update(form);
                _db.Save();
                return await Task.FromResult(Redirect("/Admin/Form/List?editForm=true"));
            }
            return await Task.FromResult(View(form));
        }

        public async Task<string> Delete(int id)
        {
            TblForm selectedForm = _db.Form.GetById(id);
            if (selectedForm != null)
            {
                selectedForm.IsDeleted = true;
                _db.Form.Update(selectedForm);
                _db.Save();
                #region Add Log
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.DeleteForm(SelectUser().IdentificationNo, selectedForm.Name),
                    UserId = SelectUser().UserId,
                    Type = 2,
                    DateCreated = DateTime.Now
                });
                _db.Save();
                #endregion
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");
        }
        public async Task<IActionResult> AddPage(int id, string name = null)
        {
            ViewBag.name = name;
            ViewBag.FormPageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View(new TblPageFormRel()
            {
                FormId = id
            }));
        }
        [HttpPost]
        public async Task<IActionResult> AddPageAsync(TblPageFormRel addPage, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                if (_db.PageFormRel.Any(i => i.FormId == addPage.FormId && i.PageId == addPage.PageId && i.IsDeleted == false))
                {
                    ModelState.AddModelError("PageId", "بخش مورد نظر قبلا به این فرم اضافه شده است");
                }
                else
                {
                    _db.PageFormRel.Add(addPage);
                    _db.Save();
                    #region Add Log
                    TblForm selectedFormEdit = _db.Form.GetById(addPage.FormId);
                    TblPage selectedPageEdit = _db.Page.GetById(addPage.PageId);
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.AddFormPageRel(SelectUser().IdentificationNo, selectedFormEdit.Name.ToString(), selectedPageEdit.Name.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 1,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Form/Index?id=" + addPage.FormId + "&name=" + name));
                }


            }
            ViewBag.FormPageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View(addPage));
        }


        public async Task<IActionResult> EditPage(int id, string name = null)
        {
            ViewBag.name = name;
            ViewBag.FormPageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            TblPageFormRel selectedForm = _db.PageFormRel.GetById(id);
            return await Task.FromResult(View(selectedForm));
        }
        [HttpPost]
        public async Task<IActionResult> EditPageAsync(TblPageFormRel editPage, string name = null)
        {
            ViewBag.name = name;
            if (ModelState.IsValid)
            {
                if (_db.PageFormRel.Any(i => i.PageFormRelId != editPage.PageFormRelId && i.FormId == editPage.FormId && i.PageId == editPage.PageId && i.IsDeleted == false))
                {
                    ModelState.AddModelError("PageId", "بخش مورد نظر قبلا به این فرم اضافه شده است");
                }
                else
                {
                    _db.PageFormRel.Update(editPage);
                    _db.Save();
                    #region Add Log
                    TblForm selectedFormEdit = _db.Form.GetById(editPage.FormId);
                    TblPage selectedPageEdit = _db.Page.GetById(editPage.PageId);
                    _db.UserLog.Add(new TblUserLog()
                    {
                        Text = LogRepo.EditFormPageRel(SelectUser().IdentificationNo, selectedFormEdit.Name.ToString(), selectedPageEdit.Name.ToString()),
                        UserId = SelectUser().UserId,
                        Type = 3,
                        DateCreated = DateTime.Now
                    });
                    _db.Save();
                    #endregion
                    return await Task.FromResult(Redirect("/Admin/Form/Index?id=" + editPage.FormId + "&name=" + selectedFormEdit.Name));

                }

            }
            ViewBag.FormPageRel = _db.Page.Get(i => i.IsDeleted == false, orderBy: i => i.OrderByDescending(k => k.PageId)).ToList();
            return await Task.FromResult(View(editPage));
        }


        public async Task<string> DeleteFormPageRelId(int id)
        {
            TblPageFormRel selectedUser = _db.PageFormRel.GetById(id);
            if (selectedUser != null)
            {
                selectedUser.IsDeleted = true;
                _db.PageFormRel.Update(selectedUser);
                TblForm selectedFormEdit = _db.Form.GetById(selectedUser.FormId);
                TblPage selectedPageEdit = _db.Page.GetById(selectedUser.PageId);
                _db.UserLog.Add(new TblUserLog()
                {
                    Text = LogRepo.DeleteFormPageRel(SelectUser().IdentificationNo, selectedFormEdit.Name.ToString(), selectedPageEdit.Name.ToString()),
                    UserId = SelectUser().UserId,
                    Type = 2,
                    DateCreated = DateTime.Now
                });
                _db.Save();
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

        }
    }
}
