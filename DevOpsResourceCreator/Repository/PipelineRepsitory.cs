using DevOpsResourceCreator.Model;
using DevOpsResourceCreator.Repository;
using Microsoft.Extensions.Configuration;
using System;

namespace DevOpsResourceCreator
{
    public class PipelineRepsitory : IPipelineRepository
    {
        private readonly string baseUri;
        private readonly IDevOpsRestApi devOpsRestApi;
        public PipelineRepsitory(IConfiguration configuration, IDevOpsRestApi devOpsRestApi)
        {
            baseUri = configuration.GetSection("DevOps").GetSection("TargetBaseAddress").Value;
            this.devOpsRestApi = devOpsRestApi;
        }
        public string Create(DevOpsEntity model)
        {
            
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);
            string uri = String.Join("?", url + "/_apis/pipelines", Constants.API_Preview1);
            var json = Helper.GetFromResources(@"Templates.CreatePipeline.json");
            json = json.Replace("#Name#", model.Pipeline);
            json = json.Replace("#RepositoryID#", model.RepositoryId);
            json = json.Replace("#RepoName#", model.Repository);
            string result = devOpsRestApi.Create(uri, json, model.Token);
            return result;

            
        }

        public string Delete(DevOpsEntity model)
        {
            return string.Empty;
        }

        public string GetAll(DevOpsEntity model)
        {
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);
            //ToDo: Clean up this method
            string uri = String.Join("?", url + "_apis/git/repositories", Constants.API);
            return devOpsRestApi.GetAll(uri, model.Token);
        }

       
    }
}