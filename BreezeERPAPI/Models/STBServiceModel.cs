using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{
    public class STBServiceModel
    {

    }

    public class ServiceDetailsInput
    {
        [Required]
        public string login_id { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string entity_code { get; set; }
        [Required]
        public string from_date { get; set; }
        [Required]
        public string to_date { get; set; }
    }

    public class ServiceDetailsOutPut
    {
        public string message { get; set; }
        public string error { get; set; }
        public List<ServiceDetails> data { get; set; }
    }

    public class ServiceDetails
    {
        public string transaction_id { get; set; }
        public string stb_no { get; set; }
        public string date { get; set; }
        public string issue { get; set; }
        public string service_by { get; set; }
        public string service_remarks { get; set; }
    }

    public class STBProcurementDetailsOutPut
    {
        public string message { get; set; }
        public string error { get; set; }
        public List<ProcurementDetails> data { get; set; }
    }

    public class ProcurementDetails
    {
        public string transaction_id { get; set; }
        public string stb_model { get; set; }
        public string date { get; set; }
        public string count { get; set; }
        public string approve_by { get; set; }
    }

    public class STBServiceSumOutPut
    {
        public string message { get; set; }
        public string error { get; set; }
        public IDictionary<string, int> data { get; set; }
    }
}