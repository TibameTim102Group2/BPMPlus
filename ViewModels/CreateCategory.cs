using BPMPlus.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.ViewModels
{
    public class CategoryNode
    {
        public string UserActivityId { get; set; }
        public string DepartmentId { get; set; }
        public string CategoryId { get; set; }
    }
    public class CreateCategory
    {
        List<CategoryNode> Nodes { get; set; }
    }
}
