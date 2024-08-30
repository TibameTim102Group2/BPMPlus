﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    public class ProcessTemplate
    {
        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string ProcessNodeId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("UserActivity")]
        public string? UserActivityId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]

        public string DepartmentId { get; set; }
        public virtual UserActivity? UserActivity { get; set; }

        
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("Category")]
        public string? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime UpdatedTime { get; set; }
    }
}
