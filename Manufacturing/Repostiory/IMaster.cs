using Manufacturing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Repostiory
{
    public interface IMaster
    {
        IOrderedQueryable<v_CustomerList> GetCustomerQueryable( string FilterData);
       

    }
}