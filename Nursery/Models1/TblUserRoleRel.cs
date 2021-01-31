using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursery.Models1
{
    public partial class TblUserRoleRel
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ShiftStart { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SiftEnd { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(TblRole.TblUserRoleRel))]
        public virtual TblRole Role { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.TblUserRoleRel))]
        public virtual TblUser User { get; set; }
    }
}
