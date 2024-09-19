using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("Form")]
    public class Form
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string FormId { get; set; }
        
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]

        public string UserId { get; set; }
        public virtual User? User { get; set; }

        public DateTime Date { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Category")]
        public string? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Project")]
        public string? ProjectId { get; set; }
        public virtual Project? Project { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength]
        public string Content { get; set; }

        public DateTime ExpectedFinishedDay { get; set; }
       

        [Column(TypeName = "VARCHAR")]
        [MaxLength(10)]
        public string Tel { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string ProcessNodeId { get; set; }

        public virtual ICollection<ProcessNode>  ProcessNode { get; set; }

        public virtual ICollection<FormRecord> FormRecord { get; set; }

        public bool FormIsActive { get; set; }

        public int? ManDay { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }
    }
}
