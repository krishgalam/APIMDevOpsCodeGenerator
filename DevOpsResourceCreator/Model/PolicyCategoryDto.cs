using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsResourceCreator.Model
{
    /// <summary>
    /// Hold Policy Category Info
    /// </summary>
    public class PolicyCategoryDto
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
