using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIMDevOpsCodeGenerator.Web.Models
{
    public class DevOpsModel
    {
        public DevOpsModel()
        {
            Organizations = new List<SelectListItem>();
            RequiredExtenions = new List<ExtensionModel>();
            PolicyCategories = new List<PolicyCategoryModel>();
            
        }
        [DisplayName("Organization Name*")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "Please enter an alphanumeric characters and - Symbol"), StringLength(60)]
        public string OrganizationName { get; set; }

        public List<SelectListItem> Organizations { get; set; }

        [DisplayName("Project Name*")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "Please enter an alphanumeric characters and - Symbol"), StringLength(60)]
        public string ProjectName { get; set; }

        public List<SelectListItem> Projects { get; set; }

        [DisplayName("Create New Project*")]
        [Required]
        public bool CreateNewProject { get; set; }

        [DisplayName("Project Description")]
        public string ProjectDescription { get; set; }

        [Required]
        [DisplayName("Repository Name*")]
        public string RepoName { get; set; }

        [Required]
        [DisplayName("Service Connection*")]
        [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "Please enter an alphanumeric characters and - Symbol"), StringLength(60)]
        public string ServiceConnection { get; set; }

        [Required]
        [DisplayName("Location*")]
        public string Location { get; set; }

        [Required]
        [DisplayName("Source Repository Url*")]
        public string SourceRepositoryUrl { get; set; }

        [DisplayName("Create Pipeline")]
        public bool CreatePipeline { get; set; }

        [DisplayName("UAT")]
        public bool DeploytoUAT { get; set; }

        [DisplayName("Production")]
        public bool DeploytoProduction { get; set; }

        [DisplayName("Dev")]
        public bool DeploytoDev { get; set; }

        [DisplayName("Create WorkItems")]
        public bool CreateWorkItems { get; set; }
        public string ResponseMessage { get; set; }

        [DisplayName("Accept Licence")]
        public bool CreateExtensions { get; set; }
        public List<ExtensionModel> RequiredExtenions { get; set; }

        public List<PolicyCategoryModel> PolicyCategories { get; set; }

        public string[] SelectedPolicies { get; set; }
    }
}