using DevOpsResourceCreator.Model;
using DevOpsResourceCreator.Repository;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DevOpsResourceCreator
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string baseUri;
        private readonly IDevOpsRestApi devOpsRestApi;
        public AccountRepository(IConfiguration configuration, IDevOpsRestApi devOpsRestApi)
        {
            baseUri = configuration.GetSection("DevOps").GetSection("TargetBaseAddress").Value;
            this.devOpsRestApi = devOpsRestApi;
        }

        public DevOpsAccountRoot GetAll(DevOpsEntity model)
        {

            string uri = "https://app.vssps.visualstudio.com/_apis/accounts?memberId=" + model.MemberId + "&api-version=6.0";
            var result = devOpsRestApi.GetAll(uri, model.Token);

            var repoResponse =
                JsonSerializer.Deserialize<DevOpsAccountRoot>(result);

            return repoResponse;
        }       
    }
}