﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPMPlus.Models
{
    [Table("ProcessNode")]
    public class ProcessNode
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string ProcessNodeId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("UserActivity")]
        public string? UserActivityId { get; set; }
        public virtual UserActivity? UserActivity{ get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(450)]
        public string UserId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Department")]
        public string? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

       

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
