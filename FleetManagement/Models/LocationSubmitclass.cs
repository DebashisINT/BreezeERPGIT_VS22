using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class LocationSubmitclass
    {
        public string user_id { get; set; }

        public List<Locationupdate> location_details { get; set; }

    }


    public class Locationupdate
    {

        public string location_name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string distance_covered { get; set; }

        public string last_update_time { get; set; }
        public string date { get; set; }
        public string order_delivered { get; set; }

    }

    public class LocationupdateOutput
    {
        public string status { get; set; }
        public string message { get; set; }

    }

}