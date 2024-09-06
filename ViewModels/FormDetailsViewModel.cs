using BPMPlus.Models;

namespace BPMPlus.ViewModels
{
    public class FormDetailsViewModel: FormBaseViewModel
    {

        public string ProcessNodeId { get; set; }

        public IList<FormDetailsProcessNodeViewModel> FormDetailsProcessNodes {  get; set; } 

        public IList<FormDetailsFormProcessViewModel> FormDetailsFormProcesses {  get; set; }

        
    }
}
