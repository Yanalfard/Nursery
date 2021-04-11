using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblRegex
    {
        public TblRegex()
        {
            TblFieldRegexRel = new HashSet<TblFieldRegexRel>();
        }

        [Key]
        public int RegexId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Regex { get; set; }
        [StringLength(500)]
        public string ValidationMessage { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("Regex")]
        public virtual ICollection<TblFieldRegexRel> TblFieldRegexRel { get; set; }
    }
}
