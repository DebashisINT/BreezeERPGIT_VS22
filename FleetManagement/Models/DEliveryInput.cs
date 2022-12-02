using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManagement.Models
{
    public class DEliveryInput
    {
        public string session_token { get; set; }
        public string user_id { get; set; }
        public DeliveryDetailsInput delivery_details { get; set; }
       
    }
    public class DeliveryDetailsInput
    {
        public string delivery_id { get; set; }
        public string address { get; set; }
        public string pin_code { get; set; }
        public string lat { get; set; }

        public string longt { get; set; }
        public string owner_name { get; set; }
        public string owner_contact_no { get; set; }
        public string owner_email { get; set; }
        public string order_id { get; set; }
        public string delivery_date { get; set; }
        public string delivered_on { get; set; }
        public string deliver_quantity { get; set; }
        public string deliver_status { get; set; }
        public string delivery_status_id { get; set; }
        public string customerid { get; set; }
        public List<DeliveryInputproduct> delivery_details_list { get; set; }
        public List<ExchangeInputdetails> exchange_old_item_list { get; set; }
        public string delivery_note { get; set; }
        public string exchange_note { get; set; }
      
    }


    public class DeliveryInputproduct
    {
        public string delivery_details_id { get; set; }
        public string status { get; set; }
        public string delivery_product_name { get; set; }
        public string delivery_product_id { get; set; }
        public string delivery_qty { get; set; }

    }
    public class ExchangeInputdetails
    {
        public string exchange_id { get; set; }
        public string receive_status { get; set; }
        public string product_name { get; set; }
        public string product_id { get; set; }
        public string qty { get; set; }
    }


    public class DeliveryDetailsInputDattabase
    {
        public string delivery_id { get; set; }
        public string address { get; set; }
        public string pin_code { get; set; }
        public string lat { get; set; }

        public string longt { get; set; }
        public string owner_name { get; set; }
        public string owner_contact_no { get; set; }
        public string owner_email { get; set; }
        public string order_id { get; set; }
        public string delivery_date { get; set; }
        public string delivered_on { get; set; }
        public string deliver_quantity { get; set; }
        public string deliver_status { get; set; }
        public string delivery_status_id { get; set; }

        public string delivery_note { get; set; }
        public string exchange_note { get; set; }


        public string delivery_details_id { get; set; }
        public string status { get; set; }
        public string delivery_product_name { get; set; }
        public string delivery_product_id { get; set; }
        public string delivery_qty { get; set; }
        public string customerid { get; set; }
    }

    public class DeliveryExchangeInputDattabase
    {
        public string delivery_id { get; set; }
    
        public string exchange_note { get; set; }
        public string customerid { get; set; }
        public string exchange_id { get; set; }
        public string receive_status { get; set; }
        public string product_name { get; set; }
        public string product_id { get; set; }
        public string qty { get; set; }
        public string delivery_date { get; set; }
        public string order_id { get; set; }
    }

}