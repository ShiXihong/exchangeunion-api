using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{

    public class EmailConfigList
    {
        public EmailConfigList()
        {
        }

        public EmailConfig XUCEmailConfig { get; set; }

        public EmailConfig DFGEmailConfig { get; set; }
    }

    public class EmailConfig
    {
        public EmailConfig()
        {
        }

        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Form { get; set; }
        public string DisplayName { get; set; }
        public string NewsletterSubject { get; set; }
        public string ContactSubject { get; set; }
    }
}
