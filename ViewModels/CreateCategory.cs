using BPMPlus.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.ViewModels
{
    public class CategoryNode
    {
        public string UserActivityId { get; set; }
        public string DepartmentId { get; set; }
    }
    public class CreateCategory
    {
        public string CategoryName {  get; set; }
        List<CategoryNode> Nodes { get; set; }
    }
}
