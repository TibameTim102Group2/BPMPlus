namespace BPMPlus.ViewModels
{
    public class BookingMeetingRoomVM
    {
        public string Room { get; set; }
        public string BookingDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Note { get; set; }
        public List<string> Members { get; set; }
    }
}
