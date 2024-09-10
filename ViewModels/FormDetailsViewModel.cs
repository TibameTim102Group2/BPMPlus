using BPMPlus.Models;

namespace BPMPlus.ViewModels
{
    public class FormDetailsViewModel: FormBaseViewModel
    {

        public string ProcessNodeId { get; set; }

        public string UserId { get; set; }

        public bool IsUser { get; set; }

        public string UserActivityId { get; set; }

        public bool IsApply { get; set; }

        public IList<FormDetailsProcessNodeViewModel> FormDetailsProcessNodes {  get; set; } 

        public IList<FormDetailsFormProcessViewModel> FormDetailsFormProcesses {  get; set; }

        
    }
}
