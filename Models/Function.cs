using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    [Table("Function")]
    public class Function
    {
        public List<Group> Groups{ get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string FunctionId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string FunctionDescription { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
