using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.ViewModels
{
    public class AddRoleVm
    {
        public int RoleId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage ="فقط مجاز به تایپ انگلیسی هستید")]
        [StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "تیتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [StringLength(50)]
        public string Title { get; set; }
    }
}
