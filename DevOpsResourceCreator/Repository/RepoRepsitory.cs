using DevOpsResourceCreator.Model;
using DevOpsResourceCreator.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DevOpsResourceCreator
{
    public class RepoRepsitory : IRepoRepository
    {
        private readonly string baseUri;
        private readonly IDevOpsRestApi devOpsRestApi;
        private readonly IConfiguration configuration;
        public RepoRepsitory(IConfiguration configuration, IDevOpsRestApi devOpsRestApi)
        {
            baseUri = configuration.GetSection("DevOps").GetSection("TargetBaseAddress").Value;
            this.devOpsRestApi = devOpsRestApi;
            this.configuration = configuration;
        }
        public string Create(DevOpsEntity model, string repositoryName)
        {
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);
            string uri = string.Join("?", url + "/_apis/git/repositories", Constants.API);

            var json = Helper.GetFromResources(@"Templates.CreateRepo.json");
            json = json.Replace("#Name#", repositoryName);
            json = json.Replace("#ProjectId#", model.ProjectId);
            string result = devOpsRestApi.Create(uri, json, model.Token);
            return result;
        }

        public string Delete(DevOpsEntity model)
        {
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName, model.RepositoryId);
            string uri = url + "?api-version=6.0";

            return devOpsRestApi.Delete(uri, model.Token);
        }

        public string GetAll(DevOpsEntity model)
        {
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);
            var uri = String.Join("?", url + "/_apis/git/repositories", Constants.API);
            return devOpsRestApi.GetAll(uri, model.Token);
        }

        public void Push(DevOpsEntity model)
        {
            PushPipeline(model, @"Templates.InfraProvision_Container_Reg.yml", "InfraProvisionContainerReg");

            PushPipeline(model, @"Templates.InfraProvision_Container_App.yml", "InfraProvisionContainerApp");
        }

        public string PushPipeline(DevOpsEntity model, string templateName, string pipeline)
        {
            var getUrl = baseUri + model.Organization;
            getUrl = getUrl + "/_apis/git/repositories/" + model.RepositoryId + "/refs?api-version=5.1";
            var objectId = string.Empty;
            var flag = true;
            while (flag)
            {
                string getresult = devOpsRestApi.Get(getUrl, model.Token);
                var getResponse = JsonSerializer.Deserialize<HRefRoot>(getresult);
                if (getResponse.count > 0)
                {
                    objectId = getResponse.value.Where(a => a.name.Equals("refs/heads/main")).FirstOrDefault().objectId;
                    flag = false;
                }
            }

            var ymlContent = Helper.GetFromResources(templateName);
            ymlContent = ymlContent.Replace("#ServiceConnectionName#", model.ServiceConnectionName);
            var ymlEnvironment = string.Empty;
            if (model.DeploytoUAT)
            {
                ymlEnvironment = "- uat";
            }
            
            if(model.DeploytoUAT && model.DeploytoProduction)
            {
                ymlEnvironment += Environment.NewLine;
            }

            if (model.DeploytoProduction)
            {
                ymlEnvironment += "- prod";
            }
            
            ymlContent = ymlContent.Replace("#environments#", ymlEnvironment);
            
            var pipelineName = configuration.GetSection("DevOps").GetSection(pipeline).Value;
            var json = Helper.GetFromResources(@"Templates.PushToBranch.json");
            json = json.Replace("#OldObjectID#", objectId);
            json = json.Replace("#Content#", ymlContent);
            json = json.Replace("#PipelineName#", pipelineName);

            var url = string.Join("/", baseUri, model.Organization);
            string uri = url + "/_apis/git/repositories/" + model.RepositoryId + "/pushes?api-version=5.1";

            string result = devOpsRestApi.Create(uri, json, model.Token);
            return result;
        }

        public string PushPolicy(DevOpsEntity model)
        {
            var getUrl = baseUri + model.Organization;
            getUrl = getUrl + "/_apis/git/repositories/" + model.RepositoryId + "/refs?api-version=5.1";
            var objectId = string.Empty;
            var flag = true;
            while (flag)
            {
                string getresult = devOpsRestApi.Get(getUrl, model.Token);
                var getResponse = JsonSerializer.Deserialize<HRefRoot>(getresult);
                if (getResponse.count > 0)
                {
                    objectId = getResponse.value.Where(a => a.name.Equals("refs/heads/main")).FirstOrDefault().objectId;
                    flag = false;
                }
            }

            var content = ProcessPolicies(model.SelectedPolicies);
            var url = string.Join("/", baseUri, model.Organization);
            string uri = url + "/_apis/git/repositories/" + model.RepositoryId + "/pushes?api-version=5.1";

            var json = Helper.GetFromResources(@"Templates.PushToBranchPolicy.json");
            json = json.Replace("#OldObjectID#", objectId);                        
            json = json.Replace("#Content#", content);

            string result = devOpsRestApi.Create(uri, json, model.Token);
            return result;
        }

        private string ProcessPolicies(List<string> selectedPolicies)
        {
            try
            {
                var content = Helper.GetFromResources(@"Templates.Policies.Policies.json");
                var policies = JsonSerializer.Deserialize<List<PolicyCategoryDto>>(content);

                var selected = new List<Policy>();
                foreach (var policy in policies)
                {
                    var result = policy.Policies.Where(a => selectedPolicies.Any(b => b == a.TemplateName));
                    selected.AddRange(result);
                }

                var finalPolicyContent = Helper.GetFromResources(@"Templates.Policies.FinalPolicy.txt");
                foreach (var policy in selected)
                {
                    var replaceKey = policy.TemplateName;
                    replaceKey = "#" + replaceKey + "#";
                    var templateName = "Templates.Policies." + policy.TemplateName + ".txt";
                    var policyContent = Helper.GetFromResources(templateName);
                    finalPolicyContent = finalPolicyContent.Replace(replaceKey, policyContent);
                }
                foreach (var item in policies)
                {
                    foreach (var policy in item.Policies)
                    {
                        var replaceKey = policy.TemplateName;
                        replaceKey = "#" + replaceKey + "#";
                        finalPolicyContent = finalPolicyContent.Replace(replaceKey, string.Empty);
                    }
                }
                return finalPolicyContent;
            }
            catch(Exception ex)
            {
                var daa = ex.Message;
                throw;
            }
        }
    }
}