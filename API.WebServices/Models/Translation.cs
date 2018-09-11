using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Translation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Key")]
        public string Key { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Source")]
        public string Source { get; set; }
    }
}
