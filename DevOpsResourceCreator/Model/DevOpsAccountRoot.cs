using System.Collections.Generic;

namespace DevOpsResourceCreator.Model
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Properties
    {
    }

    public class DevOpsAccount
    {
        public string accountId { get; set; }
        public string accountUri { get; set; }
        public string accountName { get; set; }
        public Properties properties { get; set; }
    }

    public class DevOpsAccountRoot
    {
        public int count { get; set; }
        public List<DevOpsAccount> value { get; set; }
    }


}