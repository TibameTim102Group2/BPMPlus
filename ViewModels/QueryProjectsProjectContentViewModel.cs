using System.ComponentModel.DataAnnotations;
namespace BPMPlus.ViewModels

{
    public class QueryProjectsProjectContentViewModel
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string Summary { get; set; }

        public string DeadLine { get; set; }

        public string ProjectManager { get; set; }
    }
}
