using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CaseManagementViewModel
    {
        public String CaseID { get; set; }

        public String CaseName { get; set; }

        public String CaseDescription { get; set; }

        public String Customer { get; set; }

        public String CreatedBy { get; set; }

        public String CreatedDate { get; set; }

        public String ModifyBy { get; set; }

        public String ModifyDate { get; set; }
    }
}