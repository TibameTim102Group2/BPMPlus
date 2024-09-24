namespace BPMPlus.ViewModels.Project
{
    public class GradePart
    {
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public GradePart(string gId, string gN)
        {
            this.GradeId = gId;
            this.GradeName = gN;
        }
    }
    public class DepartmentPart
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DepartmentPart(string dId, string dN)
        {
            this.DepartmentId = dId;
            this.DepartmentName = dN;
        }
    }
    public class RenderUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DepartmentPart Department {  get; set; }
        public GradePart Grade { get; set; }
        public RenderUser(string userId, string userName, DepartmentPart department, GradePart grade)
        {
            UserId = userId;
            UserName = userName;
            Department = department;
            Grade = grade;
        }
    }
    public class UserPart
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public UserPart(string uI, string uN)
        {
            this.UserId = uI;
            this.UserName = uN;
        }
    }
    public class CategoryPart
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public CategoryPart(string cI, string cN)
        {
            this.CategoryName = cI;
            this.CategoryId = cN;
        }
    }
    public class StatusPart
    {
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public StatusPart(string sI, string sN)
        {
            this.StatusId = sI;
            this.StatusName = sN;
        }
    }
    public class RenderForm
    {
        public string FormId { get; set; }
        public string DepartmentName {get;set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public RenderForm(string fI, string dN, string uI, string uN, string cT, string sT) { 
            this.FormId = fI;
            this.DepartmentName = dN;
            this.UserId = uI;
            this.UserName = uN;
            this.Category = cT;
            this.Status = sT;
        }
    }

    public class ModifyProjectViewModels
    {
        public string ProjectId {  get; set; }
        public string Deadline {  get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription {  get; set; }
        public List<string> UserIds { get; set; }
        public List<string> FormIds { get; set; }
        public ModifyProjectViewModels(
            string projectId,
            string deadline,
            string projectName,
            string projectDescription,
            List<string> userIds,
            List<string> formIds
        ) 
        {
            this.ProjectId = projectId;
            this.Deadline = deadline;
            this.ProjectName = projectName;
            this.ProjectDescription = projectDescription;
            this.UserIds = userIds;
            this.FormIds = formIds;

        }

    }
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
        public string AllowModify {  get; set; }
        public List<ProjectUsersViewModels> ProjectUsersViewModels { get; set; }
        public List<ProjectFormsViewModels> ProjectFormsViewModels { get; set; }
        public ProjectChartViewModel ProjectChartViewModel { get; set; }
        public ProjectDetailsViewModel(string am, List<ProjectUsersViewModels> pu, List<ProjectFormsViewModels> pf, ProjectChartViewModel pc)
        {
            this.AllowModify = am;
            ProjectUsersViewModels = pu;
            ProjectFormsViewModels = pf;
            ProjectChartViewModel = pc;
        }
    }


}
