using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("Users")]
    public class User
    {
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string UserId { get; set; }
        
        [MaxLength(256)]
        public string UserName {  get; set; }
        public List<Project> Projects { get; } = [];
        public List<PermissionGroup> PermissionGroups { get; } = [];
        public List<Meeting> Meetings { get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Department")]
        public string? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }


        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Grade")]
        public string? GradeId { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength]
        public string Password { get; set; }
        public virtual Grade? Grade { get; set; }
        public bool UserIsActive { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime UpdatedTime { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string TEL { get; set; }
    }
}
