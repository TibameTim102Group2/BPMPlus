using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    [Table("MeetingRooms")]
    public class MeetingRooms
    {

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string MeetingRoomId { get; set; }
        public int Accomodation { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }


    }
}
