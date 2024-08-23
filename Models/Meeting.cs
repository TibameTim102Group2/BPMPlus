using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Azure;

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
        public string ?MeetingRoom { get; set; }
        public virtual MeetingRooms? MeetingRooms { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}