using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class DeliverystatusModel
    {
        public string user_id { get; set; }
        //public string date { get; set; }
        public string session_token { get; set; }
    }
    public class DeliverystatusModeloutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<deliverystatus> delivary_list { get; set; }
    }
    public class deliverystatus
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
        public int count { get; set; }
    }

}