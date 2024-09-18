using BPMPlus.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.ViewModels
{
    public class CategoryNode
    {
        public string UserActivityId { get; set; }
        public string DepartmentId { get; set; }

        public CategoryNode() { }
        public CategoryNode(string UserActivityId, string DepartmentId)
        {
            this.UserActivityId = UserActivityId;
            this.DepartmentId = DepartmentId;
        }
    }
    public class CreateCategory
    {
        public string CategoryName {  get; set; }
        public List<CategoryNode> Nodes { get; set; }
    }
    public class GetTemplateOfNodeTemplates
    {
        
        public List<CategoryNode> Nodes { get; set; }
        public GetTemplateOfNodeTemplates(List<CategoryNode> ctList)
        {
            this.Nodes = ctList;
        }
    }
    public class GetDataForCategoryCreate
    {
        public GetTemplateOfNodeTemplates TemplateOfNodeTemplates { get; set; }
        public Dictionary<string, string> UserActivityDict { get; set; }
        public Dictionary<string, string> DepartmentDict { get; set; }
        public GetDataForCategoryCreate(GetTemplateOfNodeTemplates gT, Dictionary<string, string> uAD, Dictionary<string, string> dD)
        {
            this.TemplateOfNodeTemplates = gT;
            this.UserActivityDict = uAD;
            this.DepartmentDict = dD;
        }
        
    }

}
