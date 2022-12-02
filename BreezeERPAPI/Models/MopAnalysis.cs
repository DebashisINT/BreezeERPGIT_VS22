using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{

    public class MopAnalysisoutput
    {
        public string unit { get; set; }
        public string branch { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<MobAnalysis> data { get; set; }
    }



    public class MobAnalysis
    {
        public string title { get; set; }
        public double cash { get; set; }
        public double finance { get; set; }
        public double credit { get; set; }
        public double total { get; set; }
    }

    public class MopAnalysisInput
    {
        public string branch { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string token { get; set; }
    }

}