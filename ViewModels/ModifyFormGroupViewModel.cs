namespace BPMPlus.ViewModels
{
    public class ModifyFormGroupViewModel
    {

        public ModifyFormViewModel ModifyFrom { get; set; }
        public IList<ModifyFormCategoryViewModel> Categories { get; set; }
        public IList<ModifyFormProjectViewModel> Projects { get; set; }
    }
}
