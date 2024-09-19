using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string DepartmentId { get; set; }

        public virtual List<User> Users { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string DepartmentName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }

    }
}
