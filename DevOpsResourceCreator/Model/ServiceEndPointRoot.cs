using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsResourceCreator.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Data
    {
        public string environment { get; set; }
        public string scopeLevel { get; set; }
        public string subscriptionId { get; set; }
        public string subscriptionName { get; set; }
        public string creationMode { get; set; }
    }


    public class Avatar
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Avatar avatar { get; set; }
    }

    public class CreatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }
    public class Parameters
    {
        public string serviceprincipalid { get; set; }
        public string authenticationType { get; set; }
        public string tenantid { get; set; }
        public string username { get; set; }
        public object password { get; set; }
    }

    public class Authorization
    {
        public Parameters parameters { get; set; }
        public string scheme { get; set; }
    }

    public class ProjectReference
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ServiceEndpointProjectReference
    {
        public ProjectReference projectReference { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class ServiceEndPointRoot
    {
        public Data data { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public CreatedBy createdBy { get; set; }
        public Authorization authorization { get; set; }
        public bool isShared { get; set; }
        public bool isReady { get; set; }
        public string owner { get; set; }
        public List<ServiceEndpointProjectReference> serviceEndpointProjectReferences { get; set; }
    }


}
