using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class User : IdentityUser
    {
        public List<Meeting> Meetings{ get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string DepartmentId { get; set; }

        [MaxLength(20)]
        public string GradeId { get; set; }
        public bool UserIsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string TEL { get; set; }
    }
}
