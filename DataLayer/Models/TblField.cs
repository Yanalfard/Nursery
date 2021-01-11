using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblField
    {
        public TblField()
        {
            TblFormFieldRel = new HashSet<TblFormFieldRel>();
            TblRegex = new HashSet<TblRegex>();
        }

        [Key]
        public int FieldId { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(150)]
        public string Lable { get; set; }
        [StringLength(500)]
        public string Tooltip { get; set; }
        public string Options { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("Field")]
        public virtual ICollection<TblFormFieldRel> TblFormFieldRel { get; set; }
        [InverseProperty("Field")]
        public virtual ICollection<TblRegex> TblRegex { get; set; }
    }
}
