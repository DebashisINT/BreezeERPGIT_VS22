using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NewCompany.Models
{
    public class ErpSettingList
    {
        public List<ErpSettProp> ErpSettProp { get; set; }
    }


    public class ErpSettProp
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class ReturnData
    {
        public Boolean Success { get; set; }

        public String Message { get; set; }
    }

    
}