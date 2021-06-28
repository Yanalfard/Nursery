using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblRefrenceFrom = new HashSet<TblRefrence>();
            TblRefrenceTo = new HashSet<TblRefrence>();
            TblUserFormRelAdmin = new HashSet<TblUserFormRel>();
            TblUserFormRelUser = new HashSet<TblUserFormRel>();
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
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        public bool IsAdmin { get; set; }

        [InverseProperty(nameof(TblRefrence.From))]
        public virtual ICollection<TblRefrence> TblRefrenceFrom { get; set; }
        [InverseProperty(nameof(TblRefrence.To))]
        public virtual ICollection<TblRefrence> TblRefrenceTo { get; set; }
        [InverseProperty(nameof(TblUserFormRel.Admin))]
        public virtual ICollection<TblUserFormRel> TblUserFormRelAdmin { get; set; }
        [InverseProperty(nameof(TblUserFormRel.User))]
        public virtual ICollection<TblUserFormRel> TblUserFormRelUser { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblUserLog> TblUserLog { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblUserRoleRel> TblUserRoleRel { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblValue> TblValue { get; set; }
    }
}
