using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Azure;
using System.ComponentModel;

namespace BPMPlus.Models
{
    public class Meeting
    {
        public List<User> Users{ get; } = [];
        [Key]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        public string MeetingId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]
        [ForeignKey("MeetingRooms")]
        public string ?MeetingRoomId { get; set; }
        public virtual MeetingRooms? MeetingRooms { get; set; }
        public string MeetingHost {  get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedTime { get; set; }
    }
}