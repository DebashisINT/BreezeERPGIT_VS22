using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Models
{
    public class PClassGenerationEngine
    {
        string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PClassName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PClassId { get; set; }
        public DateTime? _PeriodFrm { get; set; }


        public DateTime? _PeriodTo { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PUnit { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PGeneration { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PBranchUnit { get; set; }
        public string _PHoliday { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<SelectListItem> _PClassUnit { get; set; }
        public List<SelectListItem> _PClassGen { get; set; }

        public List<SelectListItem> _PClassBranchUnit { get; set; }
        public List<SelectListItem> _PHoliDayBind { get; set; }
        public List<SelectListItem> PopulateClassUnit()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='PU' AND Active='Y' ");
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
        public List<SelectListItem> PopulateClassGen()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_main_master ", " CODE,[DESC] ", " RID='PG' AND Active='Y' ");
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

        public List<SelectListItem> PopulateBranchUnit()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable("Select branch_id as CODE,branch_description as [DESC] from tbl_master_Branch");
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

        public List<SelectListItem> PopulateHoliDay()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);

            DataTable DT = objEngine.GetDataTable("select HOLIDAYID as HId,HOLIDAY_DESC as HDesc from ERP_HOLIDAYMAIN");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["HDesc"].ToString(),
                        Value = row["HId"].ToString()
                    });
                }
            }

            return items;
        }

    }
}