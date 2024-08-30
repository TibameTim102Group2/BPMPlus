using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("Result")]
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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getutcdate()")]
        public DateTime UpdatedTime { get; set; }
    }
}
