namespace BPMPlus.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public string FormId {  get; set; }
        public string Department {  get; set; }
        public string UserId {  get; set; }
        public string UserName { get; set; }
        public string Category {  get; set; }
        public string Status {  get; set; }
        public ProjectDetailsViewModel(
            string formId,
            string department,
            string userId,
            string userName,
            string category,
            string status
        )
        {
            this.FormId = formId;
            this.Department = department;
            this.UserId = userId;
            this.UserName = userName;
            this.Category = category;
            this.Status = status;
        }

    }
}
