using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public partial class TblKid
    {
        [Key]
        public int KidId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Nickname { get; set; }
    }
}
