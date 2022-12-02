using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Models
{
    public class SettingEssOutput
    {
        public string status { get; set; }

        public string message { get; set; }

        public SettingEssInputs EssInfo { get; set; }

    }



    public class SettingEssInputs
    {
        public string ID { get; set; }
        public string delete_days { get; set; }
        public string edit_days { get; set; }

    }

    public class ESSInputs
    {
        public string ID { get; set; }
        public string delete_days { get; set; }
        public string edit_days { get; set; }
    }
}