using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblRolePageRel = new HashSet<TblRolePageRel>();
            TblUserRoleRel = new HashSet<TblUserRoleRel>();
        }

        [Key]
        public int RoleId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<TblRolePageRel> TblRolePageRel { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<TblUserRoleRel> TblUserRoleRel { get; set; }
    }
}
