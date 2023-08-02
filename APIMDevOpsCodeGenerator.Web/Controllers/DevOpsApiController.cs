using DevOpsResourceCreator;
using DevOpsResourceCreator.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIMDevOpsCodeGenerator.Web.Controllers
{
    [ApiController]
    public class DevOpsApiController : ControllerBase
    {
        private readonly IDevOpsRepository repository;
        public DevOpsApiController(IDevOpsRepository repository)
        {
            this.repository = repository;
        }


        [Route("api/[controller]")]
        public string Get()
        {

            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;

            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var devOpsModel = new DevOpsEntity
            {
                MemberId = userId,
            };
            return repository.GetAll(devOpsModel);
        }


        [Route("api/[controller]/Create")]
        public string Create()
        {
            
            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;

            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var devOpsModel = new DevOpsEntity
            {
                MemberId = userId,
                Repository = "TestRep006",
                Token = accessToken,
                Project = "TestProj001",
                ProjectDescription = "TestProject Description",
                Organization = Constants.ORG,
                ProjectId = "600cdf00-50fd-453d-aa93-6b128aec9445",
                ProjectName = "APIMDemoGenerator",
                ServiceEndPointName = "ManualServiceEndPoint",
                SourceRepUrl = "https://cgfsdigitalassets@dev.azure.com/cgfsdigitalassets/APIM-as-a-Service/_git/APIMDevOpsCodeGenerator",
            };
            repository.Create(devOpsModel);
            return "";
        }
    }
}
