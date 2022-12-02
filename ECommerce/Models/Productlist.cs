using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Productlistinput
    {
        public string pageno { get; set; }
        public string rowcount { get; set; }

    }
    public class Productlistoutput
    {
        public string success { get; set; }

        public string message { get; set; }
        public List<Productlist> product_details { get; set; }

    }
    public class Productlist
    {

        public long product_id { get; set; }
        public string product_name { get; set; }

        public decimal product_price { get; set; }
        public string product_small_description { get; set; }
        public string product_full_desc { get; set; }
        public string product_service_desc { get; set; }
        public string product_brand_name { get; set; }

        public string product_category_name { get; set; }
        public int product_brand_id { get; set; }
        public int product_category_id { get; set; }

        public decimal product_min_price { get; set; }

        public string product_image { get; set; }

    }
}