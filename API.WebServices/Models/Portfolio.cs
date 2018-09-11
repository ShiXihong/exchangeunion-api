using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Portfolio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Required]
        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Show Home")]
        public Boolean ShowHome { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }
    }
}
