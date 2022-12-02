using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreezeERPAPI.Models
{
    public class MISUserLogin
    {
        public string token { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
    }

    public class MISUserLoginOutput
    {

        public string UserName { get; set; }
        public string UserId { get; set; }
        public string user_name { get; set; }
        public string Branch { get; set; }
        public string session_token { get; set; }

        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
    }

}