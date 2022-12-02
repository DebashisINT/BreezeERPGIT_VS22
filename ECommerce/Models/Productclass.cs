using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
  
    public class Productclasslistoutput
    {
        public string success { get; set; }

        public string message { get; set; }
        public List<Productclasslist> class_details { get; set; }

    }
    public class Productclasslist
    {

        public long class_id { get; set; }
        public string class_name { get; set; }

     

    }
}