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
        [StringLength(100)]
        public string Name { get; set; }
        [Display(Name = "نام مستعار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [StringLength(100)]
        public string Nickname { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        public int PageId { get; set; }
    }
}
