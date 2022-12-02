using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{
    public class Topsalesbranchoutput
    {
        public string unit { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public List<Topsalesbranch> data { get; set; }

    }
    public class Topsalesbranch
    {
        public string title { get; set; }
        public decimal amount { get; set; }
    }

    public class SalesTopbranchwiseInput
    {
        public string month { get; set; }
        public string year { get; set; }

        public string token { get; set; }
    }
}