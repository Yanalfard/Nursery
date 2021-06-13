using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblForm
    {
        public TblForm()
        {
            TblFormFieldRel = new HashSet<TblFormFieldRel>();
            TblPageFormRel = new HashSet<TblPageFormRel>();
            TblUserFormRel = new HashSet<TblUserFormRel>();
        }

        [Key]
        public int FormId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public string Body { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("Form")]
        public virtual ICollection<TblFormFieldRel> TblFormFieldRel { get; set; }
        [InverseProperty("Form")]
        public virtual ICollection<TblPageFormRel> TblPageFormRel { get; set; }
        [InverseProperty("Form")]
        public virtual ICollection<TblUserFormRel> TblUserFormRel { get; set; }
    }
}
