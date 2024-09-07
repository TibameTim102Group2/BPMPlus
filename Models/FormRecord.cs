using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("FormRecord")]
    public class FormRecord
    {
        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string ProcessingRecordId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(300)]
        public string? Remark { get; set; }
  
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        [ForeignKey("Form")]
        public string? FormId { get; set; }
        public virtual Form? Form { get; set; }


        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]

        public string DepartmentId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("User")]

        public string UserId { get; set; }

        public virtual User? User { get; set; }


        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        [ForeignKey("Result")]
        public string? ResultId { get; set; }
        public virtual Result? Result { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("UserActivity")]
        public string? UserActivityId { get; set; }
        public virtual UserActivity? UserActivity { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Grade")]
        public string? GradeId { get; set; }
        public virtual Grade? Grade { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }
    }
}
