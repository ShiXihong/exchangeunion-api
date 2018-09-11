using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class Transparency
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Time")]
        public DateTime Time { get; set; }

        [Display(Name = "Amount")]
        public Int32 Amount { get; set; }

        [Display(Name = "Reason")]
        public Int32 Reason { get; set; }

        [Display(Name = "Tx Hash")]
        public Int32 TxHash { get; set; }

        [Display(Name = "Active")]
        public string Active { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

    }
}
