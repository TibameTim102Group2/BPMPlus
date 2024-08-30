using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.ComponentModel;

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

        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime UpdatedTime { get; set; }
    }
}
