using DevOpsResourceCreator.Model;
using DevOpsResourceCreator.Repository;
using Microsoft.Extensions.Configuration;
using System;

namespace DevOpsResourceCreator
{
    public class VarGroupRepsitory : IVarGroupRepository
    {
        private readonly string baseUri;
        private readonly IDevOpsRestApi devOpsRestApi;
        public VarGroupRepsitory(IConfiguration configuration, IDevOpsRestApi devOpsRestApi)
        {
            baseUri = configuration.GetSection("DevOps").GetSection("TargetBaseAddress").Value;
            this.devOpsRestApi = devOpsRestApi;
        }
        public string Create(DevOpsEntity model)
        {
            var url = string.Join("/", baseUri, model.Organization);
            var uri = String.Join("?", url + "/_apis/distributedtask/variablegroups", Constants.API_Preview2);
            var json = Helper.GetFromResources(@"Templates.CreateVarGroup.json");
            json = json.Replace("#ProjectName#", model.ProjectName);
            json = json.Replace("#ProjectId#", model.ProjectId);
            string result = devOpsRestApi.Create(uri, json, model.PAT);
            return result;
        }

        //ToDo: Clean this method up later
        public string Delete(DevOpsEntity model)
        {
            string uri = "https://dev.azure.com/{organization}/_apis/repositories/" + model.RepositoryId + "?api-version=6.0";

            return devOpsRestApi.Delete(uri, model.Token);
        }

        public string GetAll(DevOpsEntity model)
        {
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);
            var uri = String.Join("?", url + "/_apis/distributedtask/variablegroups", Constants.API_Preview2);
            var result = devOpsRestApi.GetAll(uri, model.Token);

            return result;
        }
    }
}