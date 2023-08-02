using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsResourceCreator.Model
{
    public class ServiceEndPointsRoot
    {
        public int count { get; set; }
        public List<ServiceEndPointRoot> value { get; set; }
    }
}
