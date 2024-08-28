using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BPMPlus.Models
{
    public class UserActivity
    {
        public List<PermissionGroup> PermissionGroups { get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string UserActivityId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string UserActivityIdDescription { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
