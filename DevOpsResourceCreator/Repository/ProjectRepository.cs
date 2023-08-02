using DevOpsResourceCreator.Model;
using DevOpsResourceCreator.Repository;
using Microsoft.Extensions.Configuration;
using System;

namespace DevOpsResourceCreator
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string baseUri;
        private readonly IDevOpsRestApi devOpsRestApi;
        public ProjectRepository(IConfiguration configuration, IDevOpsRestApi devOpsRestApi)
        {
            baseUri = configuration.GetSection("DevOps").GetSection("TargetBaseAddress").Value;
            this.devOpsRestApi = devOpsRestApi;
        }

        public string Create(DevOpsEntity model)
        {
            string uri = string.Join("?", baseUri + model.Organization + "/_apis/projects", Constants.API);
            var content = Helper.GetFromResources(@"Templates.CreateProject.json");
            content = content.Replace("#Name#", model.ProjectName);
            content = content.Replace("#Description#", model.ProjectDescription);
            return devOpsRestApi.Create(uri, content, model.Token);
        }

        public string Delete(DevOpsEntity model)
        {
            string uri = "https://dev.azure.com/{organization}/_apis/projects/" + model.ProjectId + "?api-version=6.0";
            return devOpsRestApi.Delete(uri, model.Token);
        }

        public string GetAll(DevOpsEntity model)
        {
            
            string uri =  String.Join("?", baseUri + model.Organization + "/_apis/projects", Constants.API);
            var result = devOpsRestApi.GetAll(uri, model.Token);

            return result;
        }

       public string GetServiceEndPoints(DevOpsEntity model)
        {
            //GET https://dev.azure.com/{organization}/{project}/_apis/serviceendpoint/endpoints?api-version=6.0-preview.4
            string uri = string.Join("/", baseUri, model.Organization, model.ProjectName, "_apis/serviceendpoint/endpoints?api-version=6.0-preview.4");
            var result = devOpsRestApi.GetAll(uri, model.Token);

            return result;
        }
    }
}