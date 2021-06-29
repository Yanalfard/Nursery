using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.ViewModels
{
    public class ToUsersVm
    {
        public int IndexN { get; set; }
        public string Comment { get; set; }
        public List<int> UsersId { get; set; }
    }
}
