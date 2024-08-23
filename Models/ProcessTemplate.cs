using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class ProcessTemplate
    {
        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string ProcessNodeId { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string FunctionId { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string CategoryId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }
    }
}
