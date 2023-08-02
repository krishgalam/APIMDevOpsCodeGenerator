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
    public class DevopsVarGroupController : ControllerBase
    {
        private readonly IVarGroupRepository repository;
        public DevopsVarGroupController(IVarGroupRepository repository)
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
                    ProjectName = "APIMDemoGenerator",
                    ProjectId="600cdf00-50fd-453d-aa93-6b128aec9445",
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
                    ProjectName = "APIMDemoGenerator",
                    ProjectId="600cdf00-50fd-453d-aa93-6b128aec9445",
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
