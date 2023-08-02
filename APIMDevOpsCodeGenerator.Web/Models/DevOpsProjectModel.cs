using System;
using System.ComponentModel;

namespace APIMDevOpsCodeGenerator.Web.Models
{
    public class DevOpsProjectModel
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Url")]
        public string Url { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Visibility")]
        public string Visibility { get; set; }
    }
}