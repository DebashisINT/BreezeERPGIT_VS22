using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class VehiclelogSubmit
    {

            //public RegisterShopInputData()
            //{
            //    data = new RegisterShopInput();
            //}


            public string data { get; set; }

            public HttpPostedFileBase vehicle_image { get; set; }
        }
        public class VehicleInput
        {
            //[Required]
            public string session_token { get; set; }
           
            public string user_id { get; set; }

           
            public string log_type { get; set; }
          
            public string log_text { get; set; }
   
            public string current_address { get; set; }
          
            public string current_location { get; set; }

             
            public string current_lat { get; set; }

         
            public string current_long { get; set; }
          
            public string date_time { get; set; }
          

        }





        public class VehiclelogShopOutput
        {
            public string status { get; set; }
            public string message { get; set; }
          
         
        }


   

   
    }
