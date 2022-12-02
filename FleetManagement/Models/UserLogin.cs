using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class UserLogin
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string login_time { get; set; }
        public string Imei { get; set; }

    }
    public class ClassLoginOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public string session_token { get; set; }
        public UserClass user_details { get; set; }
    }


    public class UserClass
    {

        public string user_id { get; set; }
        public string name { get; set; }

        public string email { get; set; }
        public string phone_number { get; set; }

        public string imeino { get; set; }
        public string success { get; set; }
        public string version_name { get; set; }
        public string address { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string profile_image { get; set; }
        public DateTime? lastlogin { get; set; }

    }

}