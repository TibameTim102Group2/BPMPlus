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
        public virtual List<Project> Projects { get; set; } = [];
        public virtual  List<PermissionGroup> PermissionGroups { get; set; } = [];
        public virtual  List<Meeting> Meetings { get; } = [];
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Department")]
        [Required]
        public string DepartmentId { get; set; }
        public virtual Department Department { get; set; }


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
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string TEL { get; set; }
        public long? ModifyPasswordTime { get; set; }
		public string? SessionToken { get; set; }
		public bool PermittedTo(string functionId)
        {
            foreach (var permissionGroup in PermissionGroups)
            {
                foreach(var UserActivity in permissionGroup.UserActivities)
                {
                    if (UserActivity.UserActivityId == functionId) return true;
                }
            }
            return false;
        }
    }
}
