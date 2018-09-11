using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class TokenHolders
    {
        public string Rand { get; set; }

        public string Address { get; set; }

        public string Quantity { get; set; }

        public string Percentage { get; set; }
    }
}
