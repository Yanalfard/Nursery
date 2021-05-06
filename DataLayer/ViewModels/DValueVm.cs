using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.ViewModels
{
    public class DValueVm
    {
        public string Value { get; set; }
        public int FormFieldId { get; set; }
        public int FormId { get; set; }
        public int UserId { get; set; }

        public DValueVm()
        {

        }

        public DValueVm(string value, int formFieldId, int formId, int userId)
        {
            Value = value;
            FormFieldId = formFieldId;
            FormId = formId;
            UserId = userId;
        }
    }
}
