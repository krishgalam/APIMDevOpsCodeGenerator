using DevOpsResourceCreator;
using DevOpsResourceCreator.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace APIMDevOpsCodeGenerator.Web.Controllers
{

    [ApiController]
    public class DevopsProjectController : ControllerBase
    {
        private readonly IProjectRepository repository;
        public DevopsProjectController(IProjectRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        [Route("api/[controller]/Create")]
        public async Task<string> Create()
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
                    Organization = Constants.ORG

                };
                var result = repository.Create(devOpsModel);
                return result;
            }
            return null;
        }

        [Authorize]
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<List<Project>> GetAll()
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


                var projectRoot =
                   JsonSerializer.Deserialize<ProjectRoot>(result);


                return projectRoot.value;
            }
            return null;
        }
    }
}
