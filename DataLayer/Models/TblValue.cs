using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblValue
    {
        [Key]
        public int ValueId { get; set; }
        [Required]
        [StringLength(500)]
        public string Value { get; set; }
        public int FormFieldId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public bool? IsAccepted { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(FormFieldId))]
        [InverseProperty(nameof(TblFormFieldRel.TblValue))]
        public virtual TblFormFieldRel FormField { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.TblValue))]
        public virtual TblUser User { get; set; }
    }
}
