namespace BPMPlus.ViewModels
{
    public class CreateFormViewModel
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
    public class NewFormViewModel
    {
        public string CategoryId { get; set; }
        public string? ProjectId { get; set; }
        public string DepartmentId { get; set; }
        public string ExpectedFinishedDay { get; set; }
        public string TEL { get; set; }
        public string Content {  get; set; }
    }
    public class GetFormForReferenceFormViewModel
    {
        public string FormId { get; set; }
        public string UserId { get; set; }
        public string CategoryId {  get; set; }
        public string Content {  get; set; }
        public string CategoryDescription {  get; set; }
    }
}
