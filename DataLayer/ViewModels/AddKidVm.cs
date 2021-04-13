using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.ViewModels
{
    public class AddKidVm
    {
        public int KidId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [StringLength(128)]
        public string Name { get; set; }
        [Display(Name = "نام مستعار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [StringLength(128)]
        public string Nickname { get; set; }
    }
}
