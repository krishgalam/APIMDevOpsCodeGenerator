using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace APIMDevOpsCodeGenerator.Web.Models
{
    public class ExtensionModel
    {
        public string ExtensionName { get; set; }
        [DisplayName("License Terms")]
        public string LicenceUrl { get; set; }
    }
}
