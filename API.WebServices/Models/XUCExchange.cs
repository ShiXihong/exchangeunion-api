using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Models
{
    public class XUCExchange
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Display(Name = "XUC/BTC")]
        public bool XUC_BTC { get; set; }

        [Display(Name = "XUC/BTC Link")]
        public string XUC_BTC_Link { get; set; }

        [Display(Name = "XUC/ETH")]
        public bool XUC_ETH { get; set; }

        [Display(Name = "XUC/ETH Link")]
        public string XUC_ETH_Link { get; set; }

        [Display(Name = "XUC/USDT")]
        public bool XUC_USDT { get; set; }

        [Display(Name = "XUC/BTC Link")]
        public string XUC_USDT_Link { get; set; }

        [Display(Name = "XUC/FIAT")]
        public bool XUC_FIAT { get; set; }

        [Display(Name = "XUC/FIAT Link")]
        public string XUC_FIAT_Link { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Order")]
        public Int32 Order { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }
    }
}
