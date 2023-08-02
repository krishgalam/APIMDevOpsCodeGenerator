using System.Collections.Generic;

namespace APIMDevOpsCodeGenerator.Web.Models
{
    public class PolicyCategoryModel
    {
        public string Name { get; set; }
        public int Sort { get; set; }
        public List<Policy> Policies { get; set; }
        
    }
    public class Policy
    {
        public string Name { get; set; }
        public int PolicyId { get; set; }
        public string TemplateName { get; set; }
        public int Sort { get; set; }
        public string Type { get; set; }
        public bool IsChecked { get; set; }
    }
}