using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class TranscationCategoryViewModel
    {
        public string Trans_id { get; set; }
        public string TransName { get; set; }
        public string Branch { get; set; }
        public string BillingType { get; set; }
        public List<Units> BranchList { get; set; }
        public List<BillingTypes> BillingTypeList { get; set; }
    }


    public class TranscationCategoryList
    {
        public long Trans_id { get; set; }
        public string TransName { get; set; }
        public string Branch { get; set; }
        public string BillingType { get; set; }
        public string CREATE_NAME { get; set; }
        public string UPDATE_NAME { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
}