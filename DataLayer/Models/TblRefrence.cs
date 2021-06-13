using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblRefrence
    {
        [Key]
        public int RefrenceId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public bool IsEnded { get; set; }
        public string Comment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateSubmited { get; set; }
        public int? IndexNo { get; set; }

        [ForeignKey(nameof(FromId))]
        [InverseProperty(nameof(TblUser.TblRefrenceFrom))]
        public virtual TblUser From { get; set; }
        [ForeignKey(nameof(ToId))]
        [InverseProperty(nameof(TblUser.TblRefrenceTo))]
        public virtual TblUser To { get; set; }
    }
}
