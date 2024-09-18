namespace BPMPlus.ViewModels
{
    public class QueryProjectsViewModel
    {


        public QueryProjectsSearchInputViewModel QueryProjectsSearchInput { get; set; }

        public QueryProjectsSearchListViewModel QueryProjectsSearchList { get; set; }

        public IList<QueryProjectsProjectContentViewModel> QueryProjectsProjectContents { get; set; }
    }
}
