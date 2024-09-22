namespace BPMPlus.ViewModels.Project
{
    public class ProjectFormsViewModels
    {
        public string FormId { get; set; }
        public string Department { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public ProjectFormsViewModels(
            string formId,
            string department,
            string userId,
            string userName,
            string category,
            string status
        )
        {
            FormId = formId;
            Department = department;
            UserId = userId;
            UserName = userName;
            Category = category;
            Status = status;
        }

    }
    public class ProjectUsersViewModels
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Department { get; set; }
        public string Grade { get; set; }
        public string IsPm { get; set; }

        public ProjectUsersViewModels(
            string userName,
            string userId,
            string department,
            string grade,
            string isPm
        )
        {
            UserName = userName;
            Department = department;
            UserId = userId;
            Grade = grade;
            IsPm = isPm;
        }

    }
    public class GanttData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartTime {  get; set; }
        public string EndTime { get; set; }
        public int Progress {  get; set; }
        public string ?Dependencies {  get; set; }
        public GanttData(string id, string name, string startTime, string endTime, int progess, string? dependence)
        {
            this.Id = id;
            this.Name = name;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Progress = progess;
            this.Dependencies = (dependence==null? dependence : null);
        }
    }
    public class ProjectChartViewModel
    {
        public List<GanttData> ProjectGanttDataList { get; set; }
        public List<GanttData> FormGanttDataList { get; set; }
        public ProjectChartViewModel(List<GanttData> pgd, List<GanttData> fgd)
        { 
            this.ProjectGanttDataList = pgd;
            this.FormGanttDataList = fgd;
        }
    }
    public class ProjectDetailsViewModel
    {
        public List<ProjectUsersViewModels> ProjectUsersViewModels { get; set; }
        public List<ProjectFormsViewModels> ProjectFormsViewModels { get; set; }
        public ProjectChartViewModel ProjectChartViewModel { get; set; }
        public ProjectDetailsViewModel(List<ProjectUsersViewModels> pu, List<ProjectFormsViewModels> pf, ProjectChartViewModel pc)
        {
            ProjectUsersViewModels = pu;
            ProjectFormsViewModels = pf;
            ProjectChartViewModel = pc;
        }
    }


}
