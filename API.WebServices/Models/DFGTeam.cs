using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class DFGTeam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Chinese Name")]
        public string ZHName { get; set; }

        [Required]
        [Display(Name = "Enlgish Name")]
        public string ENName { get; set; }

        [Display(Name = "Chinese Position Title")]
        public string ZHPosition { get; set; }

        [Display(Name = "Enlgish Position Title")]
        public string ENPosition { get; set; }

        [Required]
        [Display(Name = "Chinese Description")]
        public string ZHDescription { get; set; }

        [Required]
        [Display(Name = "Enlgish Description")]
        public string ENDescription { get; set; }

        [Required]
        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "LinkedIn Link")]
        public string LinkedIn { get; set; }

        [Display(Name = "Twitter Link")]
        public string Twitter { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Order")]
        public int Order { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }
    }
}
