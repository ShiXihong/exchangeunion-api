using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Image Url")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Link")]
        public string Link { get; set; }

        [Required]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "Notes")]
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
