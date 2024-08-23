using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Models
{
    public class MeetingRooms
    {

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [Key]
        public string MeetingRoom { get; set; }
        public int Accomadation { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }


    }
}
