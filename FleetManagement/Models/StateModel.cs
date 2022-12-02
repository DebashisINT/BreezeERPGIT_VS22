using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class StateListoutput
    {

        public string status { get; set; }
        public string message { get; set; }
        public List<StateModel> state_list { get; set; }

    }
    public class StateModel
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

    }
}