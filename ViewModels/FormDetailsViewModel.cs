using BPMPlus.Models;

namespace BPMPlus.ViewModels
{
    public class FormDetailsViewModel
    {
        public string FormId { get; set; }

        public string ProjectName { get; set; }

        public string CategoryDescription { get; set; }

        public string Tel { get; set; }

        public DateTime ExpectedFinishedDay { get; set; }

        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public string DepartmentName { get; set; }

        public string EmployeeName { get; set; }

        
        public IList<FormDetailsProcessNodeViewModel> FormDetailsProcessNodes {  get; set; } 

        public IList<FormDetailsFormProcessViewModel> FormDetailsFormProcesses {  get; set; }
    }
}
