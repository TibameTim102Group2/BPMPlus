using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPMPlus.Models
{
    [Table("Project")]
    public class Project
    {
        public List<User> Users{ get; } = [];
        [Key]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string ProjectId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(64)]
        public string ProjectName { get; set; }

        [Column(TypeName = "NCHAR")]
        [MaxLength(500)]
        public string Summary { get; set; }

        public DateTime DeadLine { get; set; }

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
