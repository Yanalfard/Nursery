using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblKid
    {
        public TblKid()
        {
            TblValue = new HashSet<TblValue>();
            TblValueFormRel = new HashSet<TblValueFormRel>();
        }

        [Key]
        public int KidId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Nickname { get; set; }
        public bool IsDeleted { get; set; }
        public int? PageId { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(PageId))]
        [InverseProperty(nameof(TblPage.TblKid))]
        public virtual TblPage Page { get; set; }
        [InverseProperty("Kid")]
        public virtual ICollection<TblValue> TblValue { get; set; }
        [InverseProperty("Kid")]
        public virtual ICollection<TblValueFormRel> TblValueFormRel { get; set; }
    }
}
