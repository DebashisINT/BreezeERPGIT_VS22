using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{

    public class NvendorsInput
    {
        public string month { get; set; }
        public string year { get; set; }
        public string token { get; set; }
    }

    public class Nvendorsoutput
    {

        public string unit { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<Nsalesmonthbranch> data { get; set; }

    }
    public class Nsalesmonthbranch
    {
        public string vendor { get; set; }
        public decimal amount { get; set; }

    }
}