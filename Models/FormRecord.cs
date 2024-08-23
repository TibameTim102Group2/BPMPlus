using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class FormRecord
    {
        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string ProcessingRecordId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(300)]
        public string Remark { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string FormId { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string DepartmentId { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string UserId { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string ResultId { get; set; }

        [Column(TypeName = "int")]
        public string FunctionId { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string GradeId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }
    }
}
