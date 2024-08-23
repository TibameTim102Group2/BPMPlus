using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class Department
    {
        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string DepartmentId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string DepartmentName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }

    }
}
