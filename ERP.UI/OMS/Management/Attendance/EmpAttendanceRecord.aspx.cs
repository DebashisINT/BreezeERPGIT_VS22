using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class EmpAttendanceRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridFullYear.JSProperties["cpHoliday"] = null;
        }

        protected void GridMonth_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string eparam = e.Parameters;
            string Year = e.Parameters.Split('~')[0];

            ProcedureExecute proc = new ProcedureExecute("prc_EmpAttrecords");
            proc.AddVarcharPara("@startDate", 10, ddlYear.Text + "-01-01");
            proc.AddVarcharPara("@Empid", 15, EmpId.Value);
            proc.AddVarcharPara("@year", 10, ddlYear.Text);
            DataSet ds = proc.GetDataSet();
            ds = UpdateSource(ds);
            Session["prc_EmpAttrecords"] = ds;
            GridFullYear.DataBind();

            DataRow holiday = ds.Tables[2].Rows[0];


            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            GridFullYear.JSProperties["cpHoliday"] = oSerializer.Serialize(GetLeaveCount(holiday));
        }

        public List<LeaveCount> GetLeaveCount(DataRow HeaderRow)
        {
            List<LeaveCount> leaveCnt = new List<LeaveCount>();
            LeaveCount lc = new LeaveCount();
            lc.Neme = "Co";
            lc.ThisYear = HeaderRow["CO"].ToString();
            lc.lastYear = HeaderRow["lastCO"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "Workingdays";
            lc.ThisYear = HeaderRow["Workingdays"].ToString();
            lc.lastYear = "0";
            leaveCnt.Add(lc);


            lc = new LeaveCount();
            lc.Neme = "Co";
            lc.ThisYear = HeaderRow["CO"].ToString();
            lc.lastYear = HeaderRow["lastCO"].ToString();
            leaveCnt.Add(lc);


            lc = new LeaveCount();
            lc.Neme = "HC";
            lc.ThisYear = HeaderRow["HC"].ToString();
            lc.lastYear = HeaderRow["lastHC"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "HS";
            lc.ThisYear = HeaderRow["HS"].ToString();
            lc.lastYear = HeaderRow["lastHS"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "OV";
            lc.ThisYear = HeaderRow["OV"].ToString();
            lc.lastYear = HeaderRow["lastOV"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "PD";
            lc.ThisYear = HeaderRow["PD"].ToString();
            lc.lastYear = HeaderRow["lastPD"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "PH";
            lc.ThisYear = HeaderRow["PH"].ToString();
            lc.lastYear = HeaderRow["lastPH"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "PL";
            lc.ThisYear = HeaderRow["PL"].ToString();
            lc.lastYear = HeaderRow["lastPL"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "SL";
            lc.ThisYear = HeaderRow["SL"].ToString();
            lc.lastYear = HeaderRow["lastSL"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "WO";
            lc.ThisYear = HeaderRow["WO"].ToString();
            lc.lastYear = HeaderRow["lastWO"].ToString();
            leaveCnt.Add(lc);

            lc = new LeaveCount();
            lc.Neme = "OVOT";
            lc.ThisYear = HeaderRow["OVOT"].ToString();
            lc.lastYear = HeaderRow["lastOVOT"].ToString();
            leaveCnt.Add(lc);
            return leaveCnt;
        }


        public DataSet UpdateSource(DataSet ds)
        {
            #region updateMonth
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Convert.ToString(dr["YearMonth"]) == "1")
                    dr["YearMonthName"] = "Jan";
                else if (Convert.ToString(dr["YearMonth"]) == "2")
                    dr["YearMonthName"] = "Feb";
                else if (Convert.ToString(dr["YearMonth"]) == "3")
                    dr["YearMonthName"] = "Mar";
                else if (Convert.ToString(dr["YearMonth"]) == "4")
                    dr["YearMonthName"] = "Apr";
                else if (Convert.ToString(dr["YearMonth"]) == "5")
                    dr["YearMonthName"] = "May";
                else if (Convert.ToString(dr["YearMonth"]) == "6")
                    dr["YearMonthName"] = "Jun";
                else if (Convert.ToString(dr["YearMonth"]) == "7")
                    dr["YearMonthName"] = "Jul";
                else if (Convert.ToString(dr["YearMonth"]) == "8")
                    dr["YearMonthName"] = "Aug";
                else if (Convert.ToString(dr["YearMonth"]) == "9")
                    dr["YearMonthName"] = "Sep";
                else if (Convert.ToString(dr["YearMonth"]) == "10")
                    dr["YearMonthName"] = "Oct";
                else if (Convert.ToString(dr["YearMonth"]) == "11")
                    dr["YearMonthName"] = "Nov";
                else if (Convert.ToString(dr["YearMonth"]) == "12")
                    dr["YearMonthName"] = "Dec";

            }
            ds.Tables[0].AcceptChanges();
            #endregion

            return ds;
        }


        protected void GridFullYear_DataBinding(object sender, EventArgs e)
        {
            GridFullYear.DataSource = ((DataSet)Session["prc_EmpAttrecords"]).Tables[0];
        }

        protected void GridFullYear_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            DataSet fDs = (DataSet)Session["prc_EmpAttrecords"];

            if (!string.IsNullOrEmpty(e.CellValue.ToString()) && e.DataColumn.FieldName != "YearMonthName")
            {
                string Month = String.Format("{0:00}", e.KeyValue);
                string day = String.Format("{0:00}", Convert.ToDecimal(e.CellValue));

                DataRow[] weekFlter = fDs.Tables[3].Select("weekdayOn='" + day + Month + ddlYear.Text + "'");
                if (weekFlter.Length > 0)
                {
                    e.Cell.BackColor = Color.FromArgb(0xCC, 0x66, 0x33);
                }
                else
                {
                    if (DateTime.Now > new DateTime(Convert.ToInt32(ddlYear.Text), Convert.ToInt32(Month), Convert.ToInt32(day), 0, 0, 0, 0))
                    {
                        e.Cell.BackColor = Color.Red;
                    }
                }

                DataRow[] FinDr = fDs.Tables[1].Select("ddmmyy='" + day + Month + ddlYear.Text + "'");
                if (FinDr.Length > 0)
                {
                    if (Convert.ToString(FinDr[0]["Emp_status"]) == "P")
                    {
                        e.Cell.BackColor = Color.FromArgb(0x66, 0xCC, 0x66);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "AB")
                    {
                        e.Cell.BackColor = Color.Red;
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "SL")
                    {
                        e.Cell.BackColor = Color.FromArgb(0x8A, 0x61, 0x0A);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "PL")
                    {
                        e.Cell.BackColor = Color.FromArgb(0x5C, 0x0F, 0x00);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "CO")
                    {
                        e.Cell.BackColor = Color.FromArgb(0xFF, 0x36, 0x00);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "OVOT")
                    {
                        e.Cell.BackColor = Color.FromArgb(0x26, 0x7D, 0x90);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "HC")
                    {
                        e.Cell.BackColor = Color.FromArgb(0xFF, 0x90, 0x00);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "HS")
                    {
                        e.Cell.BackColor = Color.FromArgb(0x00, 0xBA, 0xFF);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "OV")
                    {
                        e.Cell.BackColor = Color.FromArgb(0xB1, 0x31, 0x73);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "PD")
                    {
                        e.Cell.BackColor = Color.FromArgb(0x23, 0x4B, 0x0D);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "PH")
                    {
                        e.Cell.BackColor = Color.FromArgb(0x8A, 0x34, 0x0A);
                    }
                    else if (Convert.ToString(FinDr[0]["Emp_status"]) == "WO")
                    {
                        e.Cell.BackColor = Color.FromArgb(0xCC, 0x66, 0x33);
                    }
                    else
                    {
                        e.Cell.BackColor = Color.Yellow;
                    }
                }
               
            }
        }


        public class LeaveCount
        {
            public string Neme { get; set; }
            public string ThisYear { get; set; }
            public string lastYear { get; set; }
        }


    }
}