using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsResourceCreator.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    
    public class ServiceConnection
    {
        public Data data { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public CreatedBy createdBy { get; set; }
        public string description { get; set; }
        public Authorization authorization { get; set; }
        public bool isShared { get; set; }
        public bool isReady { get; set; }
        public string owner { get; set; }
        public List<ServiceEndpointProjectReference> serviceEndpointProjectReferences { get; set; }
    }

    public class ServiceConnectionRoot
    {
        public int count { get; set; }
        public List<ServiceConnection> value { get; set; }
    }
}
