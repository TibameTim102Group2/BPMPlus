namespace BPMPlus.ViewModels
{
	public class bKUserEditViewModel
	{
		public string UserId { get; set; }

		public string UserName { get; set; }

		public string DepartmentId { get; set; }


		public string GradeId { get; set; }


		public string Email { get; set; }

		public string Tel { get; set; }

		public string UserIsActive { get; set; }
		public List<string> Permissions { get; set; }
	}
}
