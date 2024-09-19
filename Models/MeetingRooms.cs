using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BPMPlus.Models
{
    [Table("MeetingRooms")]
    public class MeetingRooms
    {

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string MeetingRoomId { get; set; }
        public int Accommodation { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }


    }
}
