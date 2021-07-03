using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nursery.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<IActionResult> AddFormInUser(int id = 0, string name = null)
        {
            try
            {
                ViewData["name"] = name;
                ViewBag.id = id;
                List<TblUserFormRel> list = _db.UserFormRel.Get(i => i.FormId == id).ToList();
                var allUser = _db.User.Get(orderBy: i => i.OrderByDescending(i => i.UserId));
                List<TblUser> listUser = new List<TblUser>();
                foreach (var item in allUser)
                {
                    if (!list.Any(i => i.UserId == item.UserId))
                    {
                        listUser.Add(item);
                    }
                }
                ViewBag.User = listUser;
                return await Task.FromResult(View(list));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddFormInUser(int id, string name, int doneCount, List<int> usersId)
        {
            ViewBag.id = id;
            int selecteUserId = SelectUser().UserId;
            foreach (var item in usersId)
            {
                TblUserFormRel formRel = new TblUserFormRel();
                formRel.FormId = id;
                formRel.AdminId = selecteUserId;
                formRel.UserId = item;
                formRel.DateSubmited = DateTime.Now;
                formRel.DoneCount = doneCount;
                _db.UserFormRel.Add(formRel);
                _db.Save();
            }
            return await Task.FromResult(Redirect("/Admin/Form/AddFormInUser/" + id + "?name=" + name));
        }

        public async Task<string> DeleteFormInUser(int id)
        {
            TblUserFormRel selectedUserFormRel = _db.UserFormRel.GetById(id);
            if (selectedUserFormRel != null)
            {

                _db.UserFormRel.Delete(selectedUserFormRel);
                _db.Save();
                #region Add Log
                //_db.UserLog.Add(new TblUserLog()
                //{
                //    Text = LogRepo.DeleteUser(SelectUser().IdentificationNo, selectedUser.IdentificationNo.ToString()),
                //    UserId = SelectUser().UserId,
                //    Type = 2,
                //    DateCreated = DateTime.Now
                //});
                //_db.Save();
                #endregion
                return await Task.FromResult("true");
            }
            return await Task.FromResult("false");

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
                form.Priority = dform.Priority;
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
                ViewBag.skip = skip;
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
        public async Task<string> AddPriorityInUser(int id)
        {
            TblForm selectedForm = _db.Form.GetById(id);
            if (selectedForm != null)
            {
                selectedForm.Priority = 0;
                selectedForm.IsRegister = true;
                _db.Form.Update(selectedForm);
                _db.Save();
                TblForm selected = _db.Form.Get(i => i.Priority == 0 && i.FormId != id).FirstOrDefault();
                if (selected != null)
                {
                    selected.Priority = 1;
                    _db.Form.Update(selected);
                    _db.Save();
                }
                //#region Add Log
                //_db.UserLog.Add(new TblUserLog()
                //{
                //    Text = LogRepo.DeleteForm(SelectUser().IdentificationNo, selectedForm.Name),
                //    UserId = SelectUser().UserId,
                //    Type = 2,
                //    DateCreated = DateTime.Now
                //});
                //_db.Save();
                //#endregion
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

        public async Task<IActionResult> PriorityFormInKids(int pageId = 1, int kidId = 0, int id = 0, string description = null, string name = null, string checkedDelete = null)
        {
            try
            {
                ViewBag.name = name;
                ViewBag.kidId = kidId;
                ViewBag.id = id;
                ViewBag.description = description;
                ViewBag.checkedDelete = checkedDelete == "on" ? true : false;
                List<TblForm> list = _db.Form.Get(i => i.IsRegister, orderBy: j => j.OrderByDescending(k => k.FormId)).ToList();
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
                List<TblKidFormVm> listForm = new List<TblKidFormVm>();
                TblKid checkKid = _db.Kid.GetById(kidId);
                if (checkKid != null)
                {
                    ViewBag.KidName = checkKid.Nickname;
                    foreach (var item in list)
                    {
                        TblKidFormVm addForm = new TblKidFormVm();
                        addForm.KidId = checkKid.KidId;
                        addForm.Name = item.Name;
                        addForm.PageId = checkKid.PageId;
                        addForm.PageName = checkKid.Page.Name;
                        addForm.Priority = item.Priority;
                        addForm.FormId = item.FormId;
                        addForm.DateCreated = item.DateCreated;
                        addForm.Body = item.Body;
                        addForm.IsDeleted = item.IsDeleted;
                        TblValue value = _db.Value.Get(i => i.FormField.FormId == item.FormId
                        && i.KidId == kidId).FirstOrDefault();
                        if (value != null)
                        {
                            addForm.IsFull = true;
                            addForm.TblValue = value;
                        }
                        listForm.Add(addForm);
                    }
                }
                //Pagging
                int take = 10;
                int skip = (pageId - 1) * take;
                ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)listForm.Count() / take));
                ViewBag.PageShow = pageId;
                ViewBag.skip = skip;
                return await Task.FromResult(PartialView(listForm.Skip(skip).Take(take)));
            }
            catch
            {
                return await Task.FromResult(Redirect("404.html"));
            }

        }
        public async Task<IActionResult> FormValueInKid(int pageId = 1, int kidId = 0,
            string nameForm = null,
            string nameUser = null,
            string identificationNo = null,
            string nickname = null,
            int roleName = 0,
            int pageName = 0,
            string checkedDelete = null,
            string checkedOffAccepted = null,
            string checkedOnAccepted = null,
            string startDate = null,
            string endDate = null
            )
        {
            ViewBag.nameForm = nameForm;
            ViewBag.kidId = kidId;
            ViewBag.nameUser = nameUser;
            ViewBag.identificationNo = identificationNo;
            ViewBag.checkedDelete = checkedDelete == "on" ? true : false;
            ViewBag.checkedOnAccepted = checkedOnAccepted == "on" ? true : false;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            List<TblValue> selectedListFormFieldRel = _db.Value.Get(i => i.KidId == kidId && i.IsDeleted == false
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

            if (nameForm != null)
            {
                list = list.Where(i => i.Form.Name.Contains(nameForm)).ToList();
            }
            if (nameUser != null)
            {
                list = list.Where(i => i.User.Name.Contains(nameUser)).ToList();
            }
            if (identificationNo != null)
            {
                list = list.Where(i => i.User.IdentificationNo.Contains(identificationNo)).ToList();
            }
            if (nickname != null)
            {
                list = list.Where(i => i.Kid.Name.Contains(nickname)).ToList();
            }
            if (roleName != 0)
            {
                list = list.Where(i => i.Role.RoleId == roleName).ToList();
            }
            if (pageName != 0)
            {
                list = list.Where(i => i.Page.PageId == pageName).ToList();
            }
            if (startDate != null)
            {
                PersianCalendar pc = new PersianCalendar();
                string[] Start = startDate.Split('/');
                DateTime startTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                list = list.Where(i => i.DateCreated.Date >= startTime.Date).ToList();
            }
            if (endDate != null)
            {
                PersianCalendar pc = new PersianCalendar();
                string[] Start = endDate.Split('/');
                DateTime endTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                list = list.Where(i => i.DateCreated.Date <= endTime.Date).ToList();
            }

            list = list.Where(i => i.IsDeleted == ViewBag.checkedDelete).ToList();
            list = list.Where(i => i.IsAccepted == ViewBag.checkedOnAccepted).ToList();
            list = list.Distinct().ToList();
            //Pagging
            int take = 10;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = Convert.ToInt32(Math.Ceiling((double)list.Count() / take));
            ViewBag.PageShow = pageId;
            ViewBag.skip = skip;
            return await Task.FromResult(View(list.Skip(skip).Take(take)));
        }


    }
}
