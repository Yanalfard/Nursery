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

        [InverseProperty("Kid")]
        public virtual ICollection<TblValue> TblValue { get; set; }
    }
}
