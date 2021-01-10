using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblRegex
    {
        [Key]
        public int RegexId { get; set; }
        [Required]
        [StringLength(500)]
        public string Regex { get; set; }
        [StringLength(500)]
        public string ValidationMessage { get; set; }
        public int FieldId { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(FieldId))]
        [InverseProperty(nameof(TblField.TblRegex))]
        public virtual TblField Field { get; set; }
    }
}
