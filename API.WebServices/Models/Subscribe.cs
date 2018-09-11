using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Subscribe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Subscribe Date")]
        public DateTime SubscribeDate { get; set; }

        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "IP")]
        public string IP { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }
        
    }
}
