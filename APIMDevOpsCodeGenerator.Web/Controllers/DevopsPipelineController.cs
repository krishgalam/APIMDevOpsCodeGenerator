using DevOpsResourceCreator;
using DevOpsResourceCreator.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APIMDevOpsCodeGenerator.Web.Controllers
{
    [ApiController]
    public class DevopsPipelineController : ControllerBase
    {
        private readonly IPipelineRepository repository;
        public DevopsPipelineController(IPipelineRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<string> GetAll()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                var devOpsModel = new DevOpsEntity
                {
                    Repository = "TestRep",
                    Token = accessToken,
                    Project = "TestProj001",
                    ProjectDescription = "TestProject Description",
                    Organization = "cgfsdigitalassets"
                };
                var result = repository.GetAll(devOpsModel);
                return result;
            }
            return null;
        }

        [Route("api/[controller]/Create")]
        [Authorize]
        public async Task<string> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                var devOpsModel = new DevOpsEntity
                {
                    //ToDo: All The below values should come dynamically
                    Project = "APIMDemoGenerator",
                    Pipeline = "APIM-as-a-Service-new",
                    Repository = "TestRep100",
                    RepositoryId = "1a7bf244-9653-4fb1-84a9-49cb023c591f", 
                    Token = accessToken,
                    Organization = Constants.ORG,
                };
                var result = repository.Create(devOpsModel);
                return result;
            }
            return null;
        }
    }
}
