using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblUserLog = new HashSet<TblUserLog>();
            TblUserRoleRel = new HashSet<TblUserRoleRel>();
            TblValue = new HashSet<TblValue>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        [StringLength(12)]
        public string TellNo { get; set; }
        [Required]
        [StringLength(64)]
        public string Password { get; set; }
        [StringLength(10)]
        public string IdentificationNo { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<TblUserLog> TblUserLog { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblUserRoleRel> TblUserRoleRel { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblValue> TblValue { get; set; }
    }
}
