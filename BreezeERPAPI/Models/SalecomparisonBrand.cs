using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{

    public class SalecomparisonBrand
    {
        public string unit { get; set; }
        public List<SaleBrands> data { get; set; }
    }


    public class SalecomparisonBrandoutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<SaleBrands> slecombrand { get; set; }

    }
    public class SaleBrands
    {
        public string title { get; set; }
        public decimal amountcm { get; set; }
        public decimal quantitycm { get; set; }
        public decimal amountlm { get; set; }
        public decimal quantitylm { get; set; }
    }


}