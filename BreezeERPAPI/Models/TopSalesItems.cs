using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{
    public class TopSalesItemsoutput
    {
        public string unit { get; set; }
        public string month { get; set; }
        public string year { get; set; }

        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public List<Items> data { get; set; }

    }



    public class SalesItemInput
    {
        public string month { get; set; }
        public string year { get; set; }
        public string token { get; set; }
    }

    public class Items
    {
        public string title { get; set; }
        public decimal amount { get; set; }
        public decimal quantity { get; set; }

    }
}