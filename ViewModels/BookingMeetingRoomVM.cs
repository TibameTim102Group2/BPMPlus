namespace BPMPlus.ViewModels
{
    public class BookingMeetingRoomVM
    {
        public string Room { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Note { get; set; }
        public string MeetingHost { get; set; }
        public List<string> Members { get; set; }
    }
}
