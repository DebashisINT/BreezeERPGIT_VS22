using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class RequirementViewModel
    {
        public string ReqID { get; set; }
        public string ReqName { get; set; }
        public string ReqStatus { get; set; }
        public string Branch { get; set; }
        public string Create_By { get; set; }
        public string Create_Date { get; set; }
        public string Update_BY { get; set; }
        public string Update_Date { get; set; }
        public List<Units> BranchList { get; set; }
        public List<reqStatus> ReqStatusList { get; set; }
    }

    public class  RequirementList
    {
        public long ReqID { get; set; }
        public string ReqName { get; set; }
        public string ReqStatus { get; set; }
        public long Branch { get; set; }
        public long Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public long Update_BY { get; set; }
        public DateTime? Update_Date { get; set; }
        public string BRANCH_NAME { get; set; }
        public string CREATE_NAME { get; set; }
        public string UPDATE_NAME { get; set; }
        public string TypeNAME { get; set; }
    }

    public class reqStatus
    {
        public string TypeID { get; set; }
        public string TypeNAME { get; set; }
    }
}