using System.Collections.Generic;

namespace DevOpsResourceCreator.Model
{
    public class DevOpsEntity
    {
        public string Organization { get; set; }
        public string OrganizationId { get; set; }//Remove this if it's not required
        public string Project { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string Repository { get; set; }
        public string RepositoryId { get; set; }
        public string SourceRepUrl { get; set; }
        public string Pipeline { get; set; }
        public string PipelineId { get; set; }
        public string Release { get; set; }
        public string ProjectDescription { get; set; }
        public string ServiceEndPointName { get; set; }
        public string Token { get; set; }
        public string MemberId { get; set; }

        public string RestApiUrl { get; set; }
        public bool CreateNewProject { get; set; }
        public bool CreatePipeline { get; set; }

        public bool CreateWorkItems { get; set; }
        public string PAT { get; set; }
        public string ServiceConnectionName { get; set; }
        public string PublisherName { get; set; }
        public string PublisherEmail { get; set; }
        public List<string> SelectedPolicies { get; set; }
        public bool DeploytoUAT { get; set; }
        public bool DeploytoProduction { get; set; }
        public bool DeploytoDev { get; set; }
    }
}
