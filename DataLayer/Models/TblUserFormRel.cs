using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblUserFormRel
    {
        [Key]
        public int UserFormId { get; set; }
        public int FormId { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }
        public int DoneCount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateSubmited { get; set; }

        [ForeignKey(nameof(FormId))]
        [InverseProperty(nameof(TblForm.TblUserFormRel))]
        public virtual TblForm Form { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.TblUserFormRel))]
        public virtual TblUser User { get; set; }
    }
}
