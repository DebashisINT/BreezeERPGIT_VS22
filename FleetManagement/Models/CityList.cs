using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{


    public class CityListoutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<CityList> city_list { get; set; }
    }


    public class CityList
    {

        public string state_id { get; set; }
        public string city_id { get; set; }
        public string city_name { get; set; }
    }
}