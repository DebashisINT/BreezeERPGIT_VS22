using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class RolesMasterViewModel
    {
        public string RoleID { get; set; }
         public string RoleName { get; set; }
        public string Description { get; set; }
        public string BillingType { get; set; }
        public string Unit { get; set; }
        public string SkillCategory { get; set; }
        public string SkillSet { get; set; }
        public List<BillingTypes> BillingTypeList { get; set; }
        public List<Units> UnitList { get; set; }
        public List<SkillCategories> SkillCategoryList { get; set; }
        public List<SkillSets> SkillSetList { get; set; }
    }

    public class SkillSets
    {
        public string SkillSetID { get; set; }
        public string SkillSetName { get; set; }
    }

    public class RolesMasterDetailsView
    {
        public long ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string BILLING_NAME { get; set; }
        public string UnitName { get; set; }
        public string SkillCategoryName { get; set; }
        public string SKILL_SET { get; set; }
        public string CREATE_NAME { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string UPDATE_NAME { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
    public class BillingTypes
    {
        public string BillingID { get; set; }
        public string BillingName { get; set; }
    }
    public class Units
    {
        public string branch_id { get; set; }
        public string branch_description { get; set; }
    }
    public class SkillCategories
    {
        public string SkillCategoryID { get; set; }
        public string SkillCategoryName { get; set; }
    }
}