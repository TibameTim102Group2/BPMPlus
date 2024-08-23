using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class Form
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string FormId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string UserId { get; set; }

        public DateTime Date { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string CategoryId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string ProjectId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string DepartmentId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(200)]
        public string Content { get; set; }

        public DateTime ExpectedFinishedDay { get; set; }


        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string HandleDepartmentId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(10)]
        public string Tel { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string ProcessNodeId { get; set; }

        public bool FormIsActive { get; set; }

        public int man_day { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
