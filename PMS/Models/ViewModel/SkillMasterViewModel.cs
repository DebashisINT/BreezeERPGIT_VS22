using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class SkillMasterViewModel
    {
        public string skill_id { get; set; }
        public string SkillName { get; set; }
        public string Description { get; set; }
        public string Charecteristic_Type { get; set; }
        public string Branch { get; set; }
        public List<Units> BranchList { get; set; }
        public List<SkillSet> SkillList { get; set; }
        public string skillDetails { get; set; }
        public UserRightsForPage UserRightsForPage { get; set; }
    }

    public class SkillSet
    {
        public string SkillsName { get; set; }
    }
    public class SkillMasterDetailsView
    {
        public long SKILL_ID { get; set; }
        public string SkillName { get; set; }
        public string DESCRIPTION { get; set; }
        public string Charecteristic_Type { get; set; }
        public string Branch { get; set; }
        public string SKILL_SET { get; set; }
        public string CREATE_NAME { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string UPDATE_NAME { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
}