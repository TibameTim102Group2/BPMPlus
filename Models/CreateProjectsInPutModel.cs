namespace BPMPlus.Models
{
    public class CreateProjectsInPutModel
    {
        public string ProjectName { get; set; } 

        public string Summary { get; set; }

        public DateTime DeadLine { get; set; }
    }
}