using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.ViewModels
{
    public class RefrenceAndValueVm
    {
        public List<TblRefrence>  TblRefrence{ get; set; }
        public List<ValueListVm> ValueListVm { get; set; }
    }
}
