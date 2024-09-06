namespace BPMPlus.ViewModels
{
    public class FormDetailsGroupViewModel
    {
        public FormDetailsViewModel FormDetails { get; set; }

        public IList<FormDetailsUserActivityViewModel> FormDetailsUserActivities { get; set; }
    }
}
