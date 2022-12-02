using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCompany.Models
{
    public class CompanyCreationClass
    {
        public List<Company_type> Company_type { get; set; }
        public List<Company_List> Company_List { get; set; }

        public List<Griddata> grd { get; set; }
    }
    public class Company_type
    {
        public string ID { get; set; }
        public string Level_Name { get; set; }
    }
    public class Company_List
    {
        public string ID { get; set; }
        public string Company_Name { get; set; }
    }

    public class Griddata
    {
        public string Company_Name { get; set; }
        public string Dbname { get; set; }
        public string Parent_name { get; set; }
        public string URL { get; set; }
    }
}