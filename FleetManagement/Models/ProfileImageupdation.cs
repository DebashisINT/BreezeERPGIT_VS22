using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class ProfileImageupdation
    {

        [Required]
        public string session_token { get; set; }
        [Required]
        public string user_id { get; set; }

        [Required]
        public string user_name { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string latitude { get; set; }
        [Required]
        public string longitude { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string pincode { get; set; }
    }

    public class ProfileImageupdationInputData
    {
        //public RegisterShopInputData()
        public string data { get; set; }

        public HttpPostedFileBase profile_image { get; set; }
    }

    public class ProfileImageuShopOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public string session_token { get; set; }
    
    }

}