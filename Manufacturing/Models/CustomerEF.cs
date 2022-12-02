using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class CustomerEF
    {
        public IOrderedQueryable<v_CustomerList> GetCustomerQueryable() 
        {

            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            Manufacturing.Models.customerDataContext dc = new Manufacturing.Models.customerDataContext(HttpContext.Current.Session["CurConString"].ToString());
            var q = from d in dc.v_CustomerLists
                    orderby d.cnt_id descending
                    select d;
            return q;
        }


    }
}