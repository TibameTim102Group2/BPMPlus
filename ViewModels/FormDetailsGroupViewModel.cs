namespace BPMPlus.ViewModels
{
    public class FormDetailsGroupViewModel
    {
        public FormDetailsViewModel FormDetails { get; set; }

        public IList<FormDetailsProcessFlowViewModel> FormDetailsProcesseFlows { get; set; }
    }
}
