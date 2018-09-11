using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Blog
    {
        public Int32 Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public string Author { get; set; }

        public string Photo { get; set; }

        public string AuthorLink { get; set; }

        public string PostDate { get; set; }

        public string CreateDate { get; set; }
    }
}
