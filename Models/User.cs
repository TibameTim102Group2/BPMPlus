using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        public List<Project> Projects { get; } = [];
        public List<Group> Groups { get; } = [];
        public List<Meeting> Meetings{ get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Department")]
        public string? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Grade")]
        public string? GradeId { get; set; }
        public virtual Grade? Grade { get; set; }
        public bool UserIsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string TEL { get; set; }
    }
}
