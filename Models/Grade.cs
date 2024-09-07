using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("Grade")]
    public class Grade
    {
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string GradeId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string GradeName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }

    }
}
