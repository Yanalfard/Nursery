using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.ViewModels
{
    public class LoginVm
    {
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(10, ErrorMessage = "تعداد کاراکتر کم است")]
        [MaxLength(10, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [CodeMelli("لطفا کد ملی را بدرستی وارد کنید")]
        [StringLength(10)]
        public string IdentificationNo { get; set; }
        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
