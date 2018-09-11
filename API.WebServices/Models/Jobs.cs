using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Jobs
    {
        public Int32 Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string ApplyLink { get; set; }

        public string CreateDate { get; set; }
    }
}
