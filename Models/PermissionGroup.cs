using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    [Table("PermissionGroup")]
    public class PermissionGroup
    {
        public List<UserActivity> UserActivities { get; } = [];
        public List<User> Users { get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string PermissionGroupId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(20)]
        public string PermissionGroupName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
