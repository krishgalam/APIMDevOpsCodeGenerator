using System.ComponentModel;

namespace APIMDevOpsCodeGenerator.Web.Models
{
    public class DevOpsAccountModel
    {
        [DisplayName("Account ID")]
        public string AccountId { get; set; }
        [DisplayName("Account Uri")]
        public string AccountUri { get; set; }
        [DisplayName("Account Name")]
        public string AccountName { get; set; }
    }
}