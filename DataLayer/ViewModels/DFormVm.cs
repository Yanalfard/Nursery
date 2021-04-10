using DataLayer.Models;
using System;
using System.Collections.Generic;
namespace DataLayer.ViewModels
{
    public class DFormVm
    {
        public int FormId { get; set; }
        /// <summary>
        /// Form Title
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Text to show under the Title
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// The date the form was created
        /// </summary>
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The Forms, field hierarchy -with their regex validations attached
        /// </summary>
        public List<DFieldVm> Fields { get; set; }

        public DFormVm()
        {

        }

        public DFormVm(int formId, string name, string body, DateTime dateCreated, List<DFieldVm> fields, bool isDeleted = false)
        {
            FormId = formId;
            Name = name;
            Body = body;
            DateCreated = dateCreated;
            IsDeleted = isDeleted;
            Fields = fields;
        }

        public DFormVm(TblForm tblForm, List<DFieldVm> fields)
        {
            FormId = tblForm.FormId;
            Name = tblForm.Name;
            Body = tblForm.Body;
            DateCreated = tblForm.DateCreated.Value;
            IsDeleted = tblForm.IsDeleted;
            Fields = fields;
        }
    }
}
