using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPMPlus.Models
{
    public class ProcessNode
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string ProcessNodeId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string FunctionId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string UserId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(20)]
        public string Department { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string FormId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
