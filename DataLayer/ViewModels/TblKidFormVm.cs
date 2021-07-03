using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.ViewModels
{
    public class TblKidFormVm
    {

        [Key]
        public int FormId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public string Body { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public bool? IsDeleted { get; set; }
        public short Priority { get; set; }
        public bool IsRegister { get; set; }
        [Key]
        public int KidId { get; set; }
        public int? PageId { get; set; }
        public TblValue TblValue { get; set; }
        public string PageName { get; set; }
        public bool IsFull { get; set; }
    }
}
