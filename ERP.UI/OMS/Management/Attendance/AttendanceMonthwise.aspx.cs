using DataAccessLayer;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class AttendanceMonthwise : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMonthDropDown();
            }
        }

        public void LoadMonthDropDown()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_AttendanceSystem");
            proc.AddVarcharPara("@Action", 100, "GetFinacialYearBasedQouteDate");
            proc.AddVarcharPara("@FinYear", 15, Convert.ToString(Session["LastFinYear"]).Trim());
            dt = proc.GetTable();

            FormDate.MaxDate = Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]);
            ToDate.MaxDate = Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]);

            FormDate.MinDate = Convert.ToDateTime(dt.Rows[0]["finYearStartDate"]);
            ToDate.MinDate = Convert.ToDateTime(dt.Rows[0]["finYearStartDate"]);

            if (DateTime.Now > Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]))
            {
                FormDate.Date = Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]);
            }
            else {
                FormDate.Date = DateTime.Now;
                ToDate.Date = DateTime.Now;
            }



            //System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();

            //DataTable Month = new DataTable();
            //Month.Columns.Add("MonthId", typeof(System.String));
            //Month.Columns.Add("MonthName", typeof(System.String));
            //DataRow MonthRow = Month.NewRow();
            //DateTime startMonth = Convert.ToDateTime(dt.Rows[0]["finYearStartDate"]);
            //DateTime EndMonth = Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]);
            //while (startMonth < EndMonth)
            //{
            //    string strMonthName = mfi.GetMonthName(startMonth.Month).ToString();
            //    MonthRow = Month.NewRow();
            //    MonthRow["MonthId"] = startMonth.Month + "-" + startMonth.Year.ToString();
            //    MonthRow["MonthName"] = strMonthName + " - " + startMonth.Year.ToString();
            //    Month.Rows.Add(MonthRow);
            //    startMonth = startMonth.AddMonths(1);
            //}

            //cmbMonthYear.DataSource = Month;
            //cmbMonthYear.TextField = "MonthName";
            //cmbMonthYear.ValueField = "MonthId";
            //cmbMonthYear.DataBind();
            //cmbMonthYear.SelectedIndex = 0;


             proc = new ProcedureExecute("Prc_AttendanceSystem");
            proc.AddVarcharPara("@Action", 100, "GetBranchList");
            proc.AddVarcharPara("@BranchHierchy", -1, Convert.ToString(Session["userbranchHierarchy"]).Trim());
            dt = proc.GetTable();

            cmbBranch.DataSource = dt;
            cmbBranch.TextField = "branch_description";
            cmbBranch.ValueField = "branch_id";
            cmbBranch.DataBind();
            cmbBranch.SelectedIndex = 0;

        }
          

         

    }
}