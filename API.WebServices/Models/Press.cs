using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Press
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }
    }
}
