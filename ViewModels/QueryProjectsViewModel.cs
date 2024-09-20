namespace BPMPlus.ViewModels
{
    public class QueryProjectsViewModel
    {


        public QueryProjectsSearchInputViewModel QueryProjectsSearchInput { get; set; }

        public IList<QueryProjectsSearchDepartmentViewModel> QueryProjectsSearchDepartments { get; set; }

        public IList<QueryProjectsSearchEmployeeViewModel> QueryProjectsSearchEmployees { get; set; }

        public IList<QueryProjectsProjectContentViewModel> QueryProjectsProjectContents { get; set; }
    }
}
