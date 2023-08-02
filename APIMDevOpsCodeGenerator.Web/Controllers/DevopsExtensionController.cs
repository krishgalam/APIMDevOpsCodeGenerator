using DevOpsResourceCreator;
using DevOpsResourceCreator.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Buffers;
using System.Buffers.Text;
namespace APIMDevOpsCodeGenerator.Web.Controllers
{
    [ApiController]
    public class DevopsExtensionController : ControllerBase
    {
        private readonly IExtensionRepository repository;
        public DevopsExtensionController(IExtensionRepository repository)
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
                   // Repository = "TestRep",
                    Token = accessToken,
                    // ProjectName = "APIMDemoGenerator",
                    // ProjectId="600cdf00-50fd-453d-aa93-6b128aec9445",
                    // ProjectDescription = "TestProject Description",
                    // Organization = "cgfsdigitalassets"
                };
                var result = repository.GetAll(devOpsModel);
                Root myDeserializedClass = JsonSerializer.Deserialize<Root>(result);
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

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Validation
    {
        public bool isRequired { get; set; }
        public string dataType { get; set; }
    }

    public class Values
    {
        public string inputId { get; set; }
        public string defaultValue { get; set; }
        public bool isDisabled { get; set; }
    }

    public class InputDescriptor
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string inputMode { get; set; }
        public bool isConfidential { get; set; }
        public Validation validation { get; set; }
        public Values values { get; set; }
    }

    public class AuthenticationScheme
    {
        public string type { get; set; }
        public List<InputDescriptor> inputDescriptors { get; set; }
    }

    public class SupportedSize
    {
        public int rowSpan { get; set; }
        public int columnSpan { get; set; }
    }

    public class Content
    {
        public List<string> require { get; set; }
        public string initialize { get; set; }
    }

    public class Defaults
    {
        public string controller { get; set; }
        public string action { get; set; }
    }

    public class Constraints
    {
        public string parameters { get; set; }
        public string name { get; set; }
        public Properties properties { get; set; }
        public bool? inverse { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string url { get; set; }
        public List<object> inputDescriptors { get; set; }
        public List<AuthenticationScheme> authenticationSchemes { get; set; }

        // [JsonProperty("::Attributes")]
        public int Attributes { get; set; }

        // [JsonProperty("::Version")]
        public string Version { get; set; }
        public string description { get; set; }
        public string previewImageUrl { get; set; }
        public string catalogIconUrl { get; set; }
        public string loadingImageUrl { get; set; }
        public string uri { get; set; }
        public bool? isVisibleFromCatalog { get; set; }
        public bool? isNameConfigurable { get; set; }
        public bool? configurationRequired { get; set; }
        public List<SupportedSize> supportedSizes { get; set; }
        public List<string> supportedScopes { get; set; }

        // [JsonProperty("::ServiceInstanceType")]
        public string ServiceInstanceType { get; set; }
        public int? order { get; set; }
        public string serviceInstanceType { get; set; }
        public string resolution { get; set; }
        public string icon { get; set; }
        public string defaultRoute { get; set; }
        public Content content { get; set; }
        public string hostType { get; set; }
        public List<string> routeTemplates { get; set; }
        public Defaults defaults { get; set; }
        public Constraints constraints { get; set; }
        public string action { get; set; }
        public string artifactType { get; set; }
        public string pluralName { get; set; }
        public string message { get; set; }
        public string level { get; set; }
        public Filters filters { get; set; }
        public string featureName { get; set; }
    }

    public class Filters
    {
        public string type { get; set; }
    }

    public class Contribution
    {
        public string id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public List<string> targets { get; set; }
        public Properties properties { get; set; }
        public List<Constraints> constraints { get; set; }
        public List<string> includes { get; set; }
    }

    public class InstallState
    {
        public string flags { get; set; }
        public DateTime lastUpdated { get; set; }
    }

    public class Uri
    {
        public object name { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
    }

    public class ContributionType
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Properties properties { get; set; }
    }

    public class Value
    {
        public string extensionId { get; set; }
        public string extensionName { get; set; }
        public string publisherId { get; set; }
        public string publisherName { get; set; }
        public string version { get; set; }
        public string registrationId { get; set; }
        [JsonConverter(typeof(IntToStringConverter))]
        public int manifestVersion { get; set; }
        public string baseUri { get; set; }
        public List<string> scopes { get; set; }
        public List<Contribution> contributions { get; set; }
        public InstallState installState { get; set; }
        public List<ContributionType> contributionTypes { get; set; }
        public DateTime lastPublished { get; set; }
        public List<object> files { get; set; }
        public string flags { get; set; }
        public string serviceInstanceType { get; set; }
    }

    public class Root
    {
        public int count { get; set; }
        public List<Value> value { get; set; }
    }

public class IntToStringConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                if (int.TryParse(reader.GetString(), out number))
                {
                    return number;
                }
            }

            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }