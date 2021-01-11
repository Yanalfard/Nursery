using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public partial class TblRolePageRel
    {
        [Key]
        public int RolePageRelId { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(PageId))]
        [InverseProperty(nameof(TblPage.TblRolePageRel))]
        public virtual TblPage Page { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(TblRole.TblRolePageRel))]
        public virtual TblRole Role { get; set; }
    }
}
