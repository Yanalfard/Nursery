using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.ViewModels
{
    public class ValueListVm
    {
        public int ValueId { get; set; }
        public int FormFieldId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool? IsAccepted { get; set; }
        public bool? IsDeleted { get; set; }
        public int? IndexN { get; set; }

        public TblValue Value { get; set; }
        public TblKid Kid { get; set; }
        public TblUser User { get; set; }
        public TblRole Role { get; set; }
        public TblPage Page { get; set; }
        public TblForm Form { get; set; }
    }
}
