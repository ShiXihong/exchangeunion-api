using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class MailChimpConfig
    {
        public MailChimpConfig()
        {
        }

        public string APIKey { get; set; }
        public string XUCListID { get; set; }
        public string DFGListID { get; set; }
    }
}
