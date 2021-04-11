using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblUserLog
    {
        [Key]
        public int UserLogId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public string Text { get; set; }
        public int? UserId { get; set; }
        public int? Type { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.TblUserLog))]
        public virtual TblUser User { get; set; }
    }
}
