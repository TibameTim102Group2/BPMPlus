using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string CategoryId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(50)]
        public string CategoryDescription { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
