using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblValueFormRel
    {
        [Key]
        public int ValueFormId { get; set; }
        public int FormId { get; set; }
        public int KidId { get; set; }
        public int? IndexNo { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(FormId))]
        [InverseProperty(nameof(TblField.TblValueFormRel))]
        public virtual TblField Form { get; set; }
        [ForeignKey(nameof(KidId))]
        [InverseProperty(nameof(TblKid.TblValueFormRel))]
        public virtual TblKid Kid { get; set; }
    }
}
