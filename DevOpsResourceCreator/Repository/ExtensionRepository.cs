using DevOpsResourceCreator.Model;
using DevOpsResourceCreator.Repository;
using System;

namespace DevOpsResourceCreator
{
    public class ExtensionRepository : IExtensionRepository
    {
        private readonly IDevOpsRestApi devOpsRestApi;
        public ExtensionRepository(IDevOpsRestApi devOpsRestApi)
        {
            this.devOpsRestApi = devOpsRestApi;
        }
        public string Create(DevOpsEntity model)
        {
            string uri = String.Join("?", "/_apis/distributedtask/variablegroups", Constants.API_Preview2);

            var json = Helper.GetFromResources(@"Templates.CreateVarGroup.json");
            json = json.Replace("#ProjectName#", model.ProjectName);
            json = json.Replace("#ProjectId#", model.ProjectId);
            string result = devOpsRestApi.Create(uri, json, model.Token);
            return result;
        }

        //ToDo: Clean this method up later
        public string Delete(DevOpsEntity model)
        {
            string uri = "https://dev.azure.com/{organization}/_apis/repositories/" + model.RepositoryId + "?api-version=6.0";

            return devOpsRestApi.Delete(uri, model.Token);
        }
        //ToDo: Clean this method up later
        public string GetAll(DevOpsEntity model)
        {
            string uri = "https://extmgmt.dev.azure.com/" + model.Organization + "/_apis/extensionmanagement/installedextensions?api-version=6.0-preview.1";
            return devOpsRestApi.GetAll(uri, model.Token);
        }
    }
}