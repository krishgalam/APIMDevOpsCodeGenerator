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
    public class DevopsRepositoryController : ControllerBase
    {
        private readonly IRepoRepository repository;
        public DevopsRepositoryController(IRepoRepository repository)
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
                    Token = accessToken,
                    ProjectName = "APIMDemoGenerator",
                    Organization = "cgfsdigitalassets"
                };
                var result = repository.GetAll(devOpsModel);
                return result;
            }

            return "No repositories found";
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
                    Repository = "TestRep",
                    Token = accessToken,
                    Organization = Constants.ORG,
                    SourceRepUrl = "",

                };
                var result = repository.Create(devOpsModel, "");
                return result;
            }
            return null;
        }

        [Route("api/[controller]/Delete")]
        public async Task<string> Delete(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                var devOpsModel = new DevOpsEntity
                {
                    Repository = id,
                    Token = accessToken,
                    Organization = Constants.ORG,
                    SourceRepUrl = "",

                };
                var result = repository.Delete(devOpsModel);
                return result;
            }
            return null;
        }
    }
}
