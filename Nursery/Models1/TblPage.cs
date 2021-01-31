using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursery.Models1
{
    public partial class TblPage
    {
        public TblPage()
        {
            TblPageFormRel = new HashSet<TblPageFormRel>();
            TblRolePageRel = new HashSet<TblRolePageRel>();
        }

        [Key]
        public int PageId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string ControllerName { get; set; }
        [StringLength(50)]
        public string ActionName { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("Page")]
        public virtual ICollection<TblPageFormRel> TblPageFormRel { get; set; }
        [InverseProperty("Page")]
        public virtual ICollection<TblRolePageRel> TblRolePageRel { get; set; }
    }
}
