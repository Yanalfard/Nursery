using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblFormFieldRel
    {
        public TblFormFieldRel()
        {
            TblValue = new HashSet<TblValue>();
        }

        [Key]
        public int FormFieldId { get; set; }
        public int FormId { get; set; }
        public int FieldId { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(FieldId))]
        [InverseProperty(nameof(TblField.TblFormFieldRel))]
        public virtual TblField Field { get; set; }
        [ForeignKey(nameof(FormId))]
        [InverseProperty(nameof(TblForm.TblFormFieldRel))]
        public virtual TblForm Form { get; set; }
        [InverseProperty("FormField")]
        public virtual ICollection<TblValue> TblValue { get; set; }
    }
}
