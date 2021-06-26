using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.ViewModels
{
    public class RefrenceVm
    {
        public int IndexNo { get; set; }
        public TblForm Form { get; set; }
        public TblRefrence Refrence { get; set; }
        public TblUser User { get; set; }
        public TblKid  Kid{ get; set; }
        public string PageName { get; set; }
        public bool IsEnded{ get; set; }
    }
}
