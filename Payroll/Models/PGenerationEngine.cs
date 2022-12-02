using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Models
{
    public class PGenerationEngine
    {

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PClassId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]

        public string NumberingSchemsId { get; set; }
        
        public string _PeriodName { get; set; }
        public List<SelectListItem> _PClassName { get; set; }
        public List<SelectListItem> _PPeriodName { get; set; }
        public List<SelectListItem> NumberingSchemas { get; set; }

        public List<HierarchyList> Hierarchy_List { get; set; }
        public List<SelectListItem> PopulateClassName()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable(" proll_PayrollClass_Master ", " PayrollClassID,PayrollClassName ", "IsDeleted='N'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["PayrollClassName"].ToString(),
                        Value = row["PayrollClassID"].ToString()

                    });
                }
            }
            return items;

        }

        public List<SelectListItem> PopulateNumberingSchema()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, "numberingschema");
            proc.AddVarcharPara("@userbranchHierarchy", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            DT = proc.GetTable();

            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["ID"].ToString(),
                        Value = row["SchemaName"].ToString()

                    });
                }
            }
            return items;

        }

        public String Hierarchy { get; set; }

    }
    public class HierarchyList
    {
        public string Hierarchy_id { get; set; }
        public string Hierarchy_Name { get; set; }
    }
    public class ReturnData
    {
        public Boolean Success { get; set; }

        public String Message { get; set; }
    }
}