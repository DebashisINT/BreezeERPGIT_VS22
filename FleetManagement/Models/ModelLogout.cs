using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class ModelLogout
    {
    }

    public class Model_Logout
    {

        public string session_token { get; set; }
        public string user_id { get; set; }
        //public string [] user_location { get; set; }

        public string latitude { get; set; }
        public string longitude { get; set; }
        public string logout_time { get; set; }
        public string Autologout { get; set; }

    }

    public class Model_LogoutOutput
    {
        public string status { get; set; }
        public string message { get; set; }

    }
}