using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPMPlus.Models
{
    [Table("Project")]
    public class Project
    {
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

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
