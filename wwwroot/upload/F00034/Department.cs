﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string DepartmentId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string DepartmentName { get; set; }

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
