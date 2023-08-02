using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsResourceCreator.Model
{
        public class Creator
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class HRef
        {
            public string name { get; set; }
            public string objectId { get; set; }
            public Creator creator { get; set; }
            public string url { get; set; }
        }

        public class HRefRoot
    {
            public List<HRef> value { get; set; }
            public int count { get; set; }
        }

}
