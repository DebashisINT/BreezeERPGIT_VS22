using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class WorkCenterViewModel
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

    public class Country
    {
        public String Name { get; set; }

        public String Code { get; set; }
    }

    public class Branch
    {
        public String branch_id { get; set; }

        public String branch_description { get; set; }
    }

    public class InsertUpdate
    {
        public bool Success { get; set; }

        public String Message { get; set; }
    }

    public class AddressDetails
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int PinId { get; set; }
        public string PinCode { get; set; }
    }
}