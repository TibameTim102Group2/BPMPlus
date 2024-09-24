namespace BPMPlus.ViewModels
{
	public class BookingMeetingRoomEditVM
	{
		public string Date { get; set; }
		public string MeetingRoom { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string Note { get; set; }
		public string MeetingId { get; set; }
		public List<MeetingMemberVM> MeetingMembers { get; set; }
	}
	public class MeetingMemberVM
	{
		public string DepartmentId { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string DepartmentName { get; set; }  
	}
}
