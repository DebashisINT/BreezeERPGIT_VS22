using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCompany.Models
{
    public class ProductSyncClass
    {
        public List<Master_Company> Company_list { get; set; }
        public List<Products> product_List { get; set; }

    }


    public class TaxSyncClass
    {
        public List<Master_Company> Company_list { get; set; }
    }
    public class Master_Company
    {
        public string DbName { get; set; }
        public string Company_Name { get; set; }
    }

    public class Products
    {
        public int Id { get; set; }
        public string product_Name { get; set; }
    }

    public class SyncInput
    {
        public string Product { get; set; }
        public string Company { get; set; }

    }

}