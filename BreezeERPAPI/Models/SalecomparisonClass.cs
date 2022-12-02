using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{
    public class SalecomparisonClassOutput
    {

        public string unit { get; set; }
        public List<Salecomparisonclass> data { get; set; }
    }

    public class Salecomparisonclassoutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<Salecomparisonclass> slecomclass { get; set; }

    }
    public class Salecomparisonclass
    {
        public string title { get; set; }
        public decimal amountcm { get; set; }
        public decimal quantitycm { get; set; }
        public decimal amountlm { get; set; }
        public decimal quantitylm { get; set; }
    }

}