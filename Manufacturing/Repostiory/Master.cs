using Manufacturing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Repostiory
{
    public class Master:IMaster
    {
        public IOrderedQueryable<v_CustomerList> GetCustomerQueryable(string FilterData)
        {

            if (string.IsNullOrEmpty(FilterData))
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[HttpContext.Current.Session["ErpConnection"].ToString().Trim()].ConnectionString;
                Manufacturing.Models.customerDataContext dc = new Manufacturing.Models.customerDataContext(connectionString);
                var q = from d in dc.v_CustomerLists
                        orderby d.cnt_id descending
                        select d;
                return q;
            }
            else {

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[HttpContext.Current.Session["ErpConnection"].ToString().Trim()].ConnectionString;
                Manufacturing.Models.customerDataContext dc = new Manufacturing.Models.customerDataContext(connectionString);
                var q = from d in dc.v_CustomerLists
                        where d.Name.Contains(FilterData)
                        orderby d.cnt_id descending
                        select d;
                return q;
            }
            
        }
      
    }
}