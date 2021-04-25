using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.ViewModels
{
    public class AddUserVm
    {
        public int UserId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [StringLength(128)]
        public string Name { get; set; }
        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(11, ErrorMessage = "تعداد کاراکتر کم است")]
        [MaxLength(11, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[0]{1}[9]{1}[0-9]{9}", ErrorMessage = "شماره تلفن وارد شده معتبر نمی باشد")]
        [StringLength(11)]
        public string TellNo { get; set; }
        [Display(Name = "پسورد ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(64, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [MinLength(5, ErrorMessage = "تعداد کاراکتر کم است")]
        [StringLength(64)]
        public string Password { get; set; }
        [Display(Name="کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [CodeMelli("لطفا کد ملی را بدرستی وارد کنید")]
        [MaxLength(10, ErrorMessage = "تعداد کاراکتر بیشتر است")]
        [MinLength(10, ErrorMessage = "تعداد کاراکتر کم است")]
        [StringLength(10)]
        public string IdentificationNo { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsAdmin { get; set; }
    }
}
