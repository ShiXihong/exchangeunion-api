using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Email
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Send Date")]
        public DateTime SendDate { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }
    }

    public class EmailResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
