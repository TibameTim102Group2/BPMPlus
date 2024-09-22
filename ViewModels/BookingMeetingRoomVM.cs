namespace BPMPlus.ViewModels
{
    public class BookingMeetingRoomVM
    {
        public string Room { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Note { get; set; }
        public string MeetingHost { get; set; }
        public List<string> Members { get; set; }
    }
}
