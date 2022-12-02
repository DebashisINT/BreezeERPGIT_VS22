using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Models
{
    public class PayStructureEngine
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ActionType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureHeaderName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayHeadID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayHeadName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayHeadShortName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CalculationType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RoundOffType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Cal_CalculateFormula { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Cal_DisplayFormula { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Cal_PayHeadID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> PayHeadTypeList { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> CalculationTypeList { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> RoundOffTypeList { get; set; }


        public class PayHeadsDetails
        {
            public string PayHead { get; set; }
            public string ShortName { get; set; }
            public string PayType { get; set; }
            public string CalculationType { get; set; }
            public string RoundOffType { get; set; }
        }
        public class PayHeadIDList
        {
            public string PayHeadID { get; set; }
            public string PayHead { get; set; }
        }


        public List<PayHeadsDetails> AllowanceDetails { get; set; }
        public List<PayHeadIDList> PayHeadList { get; set; }

        public List<SelectListItem> PopulatePayHeadType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='PT'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulateCalculationType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='CT'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }
        public List<SelectListItem> PopulateRoundOffType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='RO'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }
    }
}