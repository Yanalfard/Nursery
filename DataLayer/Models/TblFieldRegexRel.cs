using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblFieldRegexRel
    {
        [Key]
        public int FieldRegexRelId { get; set; }
        public int FieldId { get; set; }
        public int RegexId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(FieldId))]
        [InverseProperty(nameof(TblField.TblFieldRegexRel))]
        public virtual TblField Field { get; set; }
        [ForeignKey(nameof(RegexId))]
        [InverseProperty(nameof(TblRegex.TblFieldRegexRel))]
        public virtual TblRegex Regex { get; set; }
    }
}
