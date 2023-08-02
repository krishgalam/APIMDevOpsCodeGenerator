using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Linq;
using System.Security.Claims;

namespace APIMDevOpsCodeGenerator.Web
{
    public static class Helper
    {
        public static void Track(ClaimsPrincipal user)
        {
            var client = new TelemetryClient();
            client.InstrumentationKey = "8100bc39-9b96-48c4-9e2c-80715c95445e";

            var item = new EventTelemetry("Logged in user information");
            var name = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var org = email.Split('@')[1];
            var organization = org.Split('.')[0];
            item.Properties.Add("UserName", name);
            item.Properties.Add("Organization", organization);

            client.TrackEvent(item);
        }
    }
}
