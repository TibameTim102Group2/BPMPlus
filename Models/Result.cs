using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class Result
    {

        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string ResultId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string ResultDescription { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }
    }
}
