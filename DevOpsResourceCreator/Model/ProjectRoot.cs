using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsResourceCreator.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
   

    public class ProjectRoot
    {
        public int count { get; set; }
        public List<Project> value { get; set; }
    }


}
