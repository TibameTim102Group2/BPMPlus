using BPMPlus.Models;
using System.Collections.Generic;

namespace BPMPlus.ViewModels
{
    public class FormReviewViewModel
    {

        public string UserId { get; set; }
        public string FormId { get; set; }
        public DateTime Date { get; set; }
        public string CategoryId { get; set; }
        public string CurrentResults { get; set; }
        public string DepartmentId { get; set; }

        public string AssginEmployee { get; set; }
        public string NeedEmployees { get; set; }
        public DateTime HopeFinishDate { get; set; }
        public string BelongProjects { get; set; }
        public int EstimatedTime { get; set; }
        public string Content { get; set; }
        public string? Remark { get; set; }
        public string? ResultId { get; set; }

        public string UserActivityId { get; set; }

        public string ProcessNodeId { get; set; }
        public string UserName { get; internal set; }
        public List<FormReviewFormProcessViewModel> FormRecordList { get; set; }
        public List<FormReviewProcessFlowViewModel> FormProcessFlow { get; set; }

        public DateTime Enddate { get; set; } = DateTime.Now;

        public string Id { get; set; }

        public List<IFormFile> Files { get; set; } = null;

        public string DepartmentName { get; set; }

        public string CategoryDescription { get; set; }

        public string CurrentResultsDescription { get; set; }


    }
}
