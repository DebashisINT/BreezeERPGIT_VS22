using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{
    public class topbranchbonthsalesoutput
    {
        public string unit { get; set; }
        public string branch { get; set; }
        public string year { get; set; }
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<Topsalesmonthbranch> data { get; set; }

    }
    public class Topsalesmonthbranch
    {
        public string month { get; set; }
        public decimal amount { get; set; }

    }

    public class SalesTopMonthwiseInput
    {
        public string branch { get; set; }
        public string year { get; set; }
        public string token { get; set; }

    }

}