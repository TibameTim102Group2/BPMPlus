using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class Group
    {
        public List<Function> Functions { get; } = [];
        public List<User> Users{ get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string GroupId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(20)]
        public string GroupName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
