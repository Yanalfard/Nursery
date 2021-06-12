using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblField
    {
        public TblField()
        {
            TblFieldRegexRel = new HashSet<TblFieldRegexRel>();
            TblFormFieldRel = new HashSet<TblFormFieldRel>();
        }

        [Key]
        public int FieldId { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(150)]
        public string Label { get; set; }
        [StringLength(500)]
        public string Tooltip { get; set; }
        public string Options { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsDeleted { get; set; }
        [StringLength(150)]
        public string Placeholder { get; set; }

        [InverseProperty("Field")]
        public virtual ICollection<TblFieldRegexRel> TblFieldRegexRel { get; set; }
        [InverseProperty("Field")]
        public virtual ICollection<TblFormFieldRel> TblFormFieldRel { get; set; }
    }
}
