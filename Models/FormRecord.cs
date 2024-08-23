using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(450)]

        public string UserId { get; set; }


        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        [ForeignKey("Result")]
        public string? ResultId { get; set; }
        public virtual Result? Result { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Function")]
        public string? FunctionId { get; set; }
        public virtual Function? Function { get; set; }
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
        public DateTime UpdateTime { get; set; }
    }
}
