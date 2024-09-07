using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("PermissionGroup")]
    public class PermissionGroup
    {
        public virtual List<UserActivity> UserActivities { get; } = [];
        public virtual List<User> Users { get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string PermissionGroupId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(20)]
        public string PermissionGroupName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }
    }
}
