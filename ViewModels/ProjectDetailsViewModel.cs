namespace BPMPlus.ViewModels
{
    public class ProjectFormsViewModels
    {
        public string FormId {  get; set; }
        public string Department {  get; set; }
        public string UserId {  get; set; }
        public string UserName { get; set; }
        public string Category {  get; set; }
        public string Status {  get; set; }
        public ProjectFormsViewModels(
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
    public class ProjectUsersViewModels
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Department { get; set; }
        public string Grade { get; set; }
        public string IsPm {  get; set; }
        
        public ProjectUsersViewModels(
            string userName,
            string userId,
            string department,
            string grade,
            string isPm
        )
        {
            this.UserName = userName;
            this.Department = department;
            this.UserId = userId;
            this.Grade = grade;
            this.IsPm = isPm;
        }

    }
    public class ProjectDetailsViewModel
    {
        public List<ProjectUsersViewModels> projectUsersViewModels { get; set; }
        public List<ProjectFormsViewModels> projectFormsViewModels { get; set; }
        public ProjectDetailsViewModel(List<ProjectUsersViewModels> pu, List<ProjectFormsViewModels> pf) { 
            this.projectUsersViewModels = pu;
            this.projectFormsViewModels = pf;
        }
    }

    public class ProjectChartViewModel
    {

    }
}
