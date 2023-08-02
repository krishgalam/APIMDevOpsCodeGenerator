using DevOpsResourceCreator.Model;
using DevOpsResourceCreator.Repository;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DevOpsResourceCreator
{
    public class DevOpsRepository : IDevOpsRepository
    {
        private readonly IDevOpsRestApi devOpsRestApi;
        private readonly IRepoRepository repoRepository;
        private readonly IPipelineRepository pipelineRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IVarGroupRepository varGroupRepository;
        private readonly IConfiguration configuration;
        private readonly string baseUri;
        public DevOpsRepository(IConfiguration configuration, IDevOpsRestApi devOpsRestApi, IRepoRepository repoRepository, IPipelineRepository pipelineRepository, IProjectRepository projectRepository, IVarGroupRepository varGroupRepository)
        {
            baseUri = configuration.GetSection("DevOps").GetSection("TargetBaseAddress").Value;
            this.devOpsRestApi = devOpsRestApi;
            this.repoRepository = repoRepository;
            this.pipelineRepository = pipelineRepository;
            this.projectRepository = projectRepository;
            this.varGroupRepository = varGroupRepository;
            this.configuration = configuration;

        }
        public void Create(DevOpsEntity model)
        {
            var projectResponse = projectRepository.GetAll(model);
            var projectResult = JsonSerializer.Deserialize<ProjectRoot>(projectResponse);
            model.ProjectName = projectResult.value.Where(a => a.id == model.ProjectId).FirstOrDefault().name;
            var SourceIaCRepoUrl = configuration.GetSection("DevOps").GetSection("SourceIaCRepoUrl").Value;
            var repositoryId = CreateRepository(model, model.Repository, SourceIaCRepoUrl);
            model.RepositoryId = repositoryId;
           
            model.Pipeline = configuration.GetSection("DevOps").GetSection("InfraProvisionContainerReg").Value;
            model.RepositoryId = repositoryId;
            pipelineRepository.Create(model);

            model.Pipeline = configuration.GetSection("DevOps").GetSection("InfraProvisionContainerApp").Value;
            model.RepositoryId = repositoryId;
            pipelineRepository.Create(model);

            // Commit Piple to the repository (add new pipeline yml file)
            repoRepository.Push(model);            
            
           
           try
           {
                       
                // Create Azure Function Repository
                var functionRepository = configuration.GetSection("DevOps").GetSection("AzureFunctionReposirotyName").Value;
                var functionRepositoryUrl = configuration.GetSection("DevOps").GetSection("AzureFunctionRepoUrl").Value;
                var functionrepositoryId = CreateRepository(model, functionRepository, functionRepositoryUrl);
                model.Repository = configuration.GetSection("DevOps").GetSection("AzureFunctionReposirotyName").Value; 
                model.Pipeline = configuration.GetSection("DevOps").GetSection("PipelineforDeployAzureFunctionImage").Value;
                model.RepositoryId = functionrepositoryId;
                pipelineRepository.Create(model);
                // Commit Piple to the repository (update yml file)
                repoRepository.PushPipeline(model, @"Templates.DeployFunctionImage.yml", "PipelineforDeployAzureFunctionImage");
           }
           catch (System.Exception exp)
           {                        
           }

            try
            {                           
                // Create Api1 Repository
                var api1Repository = configuration.GetSection("DevOps").GetSection("Api1RepositoryName").Value;
                var api1RepositoryUrl = configuration.GetSection("DevOps").GetSection("SourceApi1RepoUrl").Value;
                var repositoryIdApi1 = CreateRepository(model, api1Repository, api1RepositoryUrl);
                model.Repository = configuration.GetSection("DevOps").GetSection("Api1RepositoryName").Value; 
                model.Pipeline = configuration.GetSection("DevOps").GetSection("PipelineForDeployApiImage").Value;
                model.RepositoryId = repositoryIdApi1;
                pipelineRepository.Create(model);
                // Commit Piple to the repository (update yml file)
                repoRepository.PushPipeline(model, @"Templates.DeployApiImage.yml", "PipelineForDeployApiImage");                       
            }
            catch (System.Exception exp)
            {                                
            }

             try
            {                           
                // Create Web UI Repository
                var webUiRepository = configuration.GetSection("DevOps").GetSection("WebUIRepositoryName").Value;
                var webUIRepositoryUrl = configuration.GetSection("DevOps").GetSection("SourecWebUIRepoUrl").Value;
                var repositoryIdWebUI = CreateRepository(model, webUiRepository, webUIRepositoryUrl);
                model.Repository = configuration.GetSection("DevOps").GetSection("WebUIRepositoryName").Value; 
                model.Pipeline = configuration.GetSection("DevOps").GetSection("PipelineForDeployWebUIImage").Value;
                model.RepositoryId = repositoryIdWebUI;
                pipelineRepository.Create(model);
                // Commit Piple to the repository (update yml file)
                repoRepository.PushPipeline(model, @"Templates.DeployWebUIImage.yml", "PipelineForDeployWebUIImage");                       
            }
            catch (System.Exception exp)
            {                                
            }
        }

        private string CreateRepository(DevOpsEntity model, string repositoryName, string sourceRepositoryUrl)
        {
            // Create Targe Repository
            var repoListResponse = repoRepository.GetAll(model);

            var repoList = JsonSerializer.Deserialize<RepositoryRoot>(repoListResponse);
            if (repoList.value.Any(a => a.name.Equals(repositoryName, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                return repoList.value.FirstOrDefault(a => a.name.Equals(repositoryName, System.StringComparison.InvariantCultureIgnoreCase)).id;
            }

            var repo = repoRepository.Create(model, repositoryName);
            var repoResponse = JsonSerializer.Deserialize<Model.Repository>(repo);
            model.ProjectName = repoResponse.project.name;

            // Get Service End point
            var serviceEndpointId = GetServiceEndPoint(model, repositoryName, sourceRepositoryUrl);

            // Clone Repository
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);
            string repoUri = url + "/_apis/git/repositories/" + repositoryName + "/importRequests?api-version=6.0-preview.1";

            var importRepjson = Helper.GetFromResources(@"Templates.ImportRepo.json");
            importRepjson = importRepjson.Replace("#SourceCloneUrl#", sourceRepositoryUrl);
            importRepjson = importRepjson.Replace("#serviceEndpointId#", serviceEndpointId);
            devOpsRestApi.Create(repoUri, importRepjson, model.Token);
            return repoResponse.id;
        }

        private string GetServiceEndPoint(DevOpsEntity model, string repositoryName, string repositoryUrl)
        {

            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);
            string uri = url + "/_apis/serviceendpoint/endpoints?api-version=6.0-preview.4";

            var serviceEndPoints = devOpsRestApi.GetAll(uri, model.Token);
            var response = JsonSerializer.Deserialize<ServiceEndPointsRoot>(serviceEndPoints);

            if (response.value.Any(a => a.name.Equals(repositoryName)))
            {
                var data = response.value.FirstOrDefault(a => a.name.Equals(repositoryName)).id;
                return data;
            }
            else
            {
                var json = Helper.GetFromResources(@"Templates.CreateAndCloneRepo.json");
                json = json.Replace("#PAT#", Constants.PAT);
                json = json.Replace("#ProjectId#", model.ProjectId);
                json = json.Replace("#ProjectName#", model.ProjectName);
                json = json.Replace("#ServiceEndPointName#", repositoryName);
                json = json.Replace("#SourceUrl#", repositoryUrl);
                var result = devOpsRestApi.Create(uri, json, model.Token);

                var serviceEndPointResponse =
                       JsonSerializer.Deserialize<ServiceEndPointRoot>(result);
                return serviceEndPointResponse.id;
            }
            
        }


        public string Delete(DevOpsEntity model)
        {
            var url = string.Join("/", baseUri, model.Organization, model.ProjectName);

            string uri = url + "/_apis/serviceendpoint/endpoints?api-version=6.0-preview.4";
            return devOpsRestApi.Delete(uri, model.Token);
        }

        public string GetAll(DevOpsEntity model)
        {

            return string.Empty;
        }
    }
}