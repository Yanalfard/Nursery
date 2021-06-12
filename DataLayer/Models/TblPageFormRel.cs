using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblPageFormRel
    {
        [Key]
        public int PageFormRelId { get; set; }
        public int PageId { get; set; }
        public int FormId { get; set; }
        public int IndexNo { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(FormId))]
        [InverseProperty(nameof(TblForm.TblPageFormRel))]
        public virtual TblForm Form { get; set; }
        [ForeignKey(nameof(PageId))]
        [InverseProperty(nameof(TblPage.TblPageFormRel))]
        public virtual TblPage Page { get; set; }
    }
}
