using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class ManufacturingProcessMasterViewModel
    {
        public Int64 WorkCenterID { get; set; }

        public String WorkCenterCode { get; set; }

        public String WorkCenterDescription { get; set; }

        public String Remarks { get; set; }

        public String WorkCenterAddress1 { get; set; }

        public String WorkCenterAddress2 { get; set; }

        public String WorkCenterAddress3 { get; set; }

        public String WorkCenterLandmark { get; set; }

        public String WorkCenterCountry { get; set; }

        public String WorkCenterState { get; set; }

        public String WorkCenterCity { get; set; }

        public String WorkCenterPin { get; set; }

        public Int16 WorkCenterBranch { get; set; }

        public Int64 UserID { get; set; }

        public String BranchName { get; set; }

        public string pin_code { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string country { get; set; }
    }
}