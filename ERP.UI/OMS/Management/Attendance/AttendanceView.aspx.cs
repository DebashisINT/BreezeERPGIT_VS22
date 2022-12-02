using DataAccessLayer;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class AttendanceView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                LoadMonthDropDown();
            }



            gridAttendance.JSProperties["cpReturnMsg"] = null;
        }

        public void LoadMonthDropDown() {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_AttendanceSystem");
            proc.AddVarcharPara("@Action", 100, "GetFinacialYearBasedQouteDate");
            proc.AddVarcharPara("@FinYear", 15, Convert.ToString(Session["LastFinYear"]).Trim());
            dt = proc.GetTable();  

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();

            DataTable Month = new DataTable();
            Month.Columns.Add("MonthId", typeof(System.String));
            Month.Columns.Add("MonthName", typeof(System.String));
            DataRow MonthRow = Month.NewRow();
            DateTime startMonth = Convert.ToDateTime(dt.Rows[0]["finYearStartDate"]);
            DateTime EndMonth = Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]);
            while(startMonth<EndMonth)
            {
                string strMonthName = mfi.GetMonthName(startMonth.Month).ToString();
                MonthRow = Month.NewRow();
                MonthRow["MonthId"] = startMonth.Month + "-" + startMonth.Year.ToString();
                MonthRow["MonthName"] = strMonthName + " - " + startMonth.Year.ToString();
                Month.Rows.Add(MonthRow);
                startMonth = startMonth.AddMonths(1);
            }

            cmbMonthYear.DataSource = Month;
            cmbMonthYear.TextField = "MonthName";
            cmbMonthYear.ValueField = "MonthId";
            cmbMonthYear.DataBind();
            cmbMonthYear.SelectedIndex = 0;




            ProcedureExecute proc1 = new ProcedureExecute("Prc_AttendanceSystem");
            proc1.AddVarcharPara("@Action", 100, "GetLeaveType"); 
            DataTable LeaveType = proc1.GetTable();
            EmpStatus.DataSource = LeaveType;
            EmpStatus.TextField = "Name";
            EmpStatus.ValueField = "Code";
            EmpStatus.DataBind();
            EmpStatus.SelectedIndex = 0;

        }

        protected void gridAttendance_DataBinding(object sender, EventArgs e)
        {
            string id = Convert.ToString(cmbMonthYear.Value);


             ProcedureExecute proc = new ProcedureExecute("Prc_AttendanceSystem");
                proc.AddVarcharPara("@Action", 100, "GetEmpAttendanceByMonth");
                proc.AddVarcharPara("@EmpId", 20, EmpId.Value);
                proc.AddVarcharPara("@startDate", 10, id.Split('-')[1] + "-" + id.Split('-')[0]+"-01"); 
                proc.AddPara("@User",Convert.ToInt32(Session["userid"]));
                System.Data.DataTable AttTable = proc.GetTable();

                gridAttendance.DataSource = AttTable;
        }

        protected void gridAttendance_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "Update") {

                string AttDate = hdAttDate.Value;

                if (IntimeEdit.DateTime.Year == 100)
                    IntimeEdit.DateTime = IntimeEdit.DateTime.AddMinutes(1);

                DateTime InTime = IntimeEdit.DateTime;

                DateTime NewInTime = new DateTime(Convert.ToInt32(AttDate.Split('-')[2]), Convert.ToInt32(AttDate.Split('-')[1]), Convert.ToInt32(AttDate.Split('-')[0]),
                    InTime.Hour, InTime.Minute, InTime.Second);



                if (OutTimeEdit.DateTime.Year == 100)
                    OutTimeEdit.DateTime = OutTimeEdit.DateTime.AddMinutes(1);
                DateTime outTime = OutTimeEdit.DateTime;
               

                DateTime NewOutTime = new DateTime(Convert.ToInt32(AttDate.Split('-')[2]), Convert.ToInt32(AttDate.Split('-')[1]), Convert.ToInt32(AttDate.Split('-')[0]),
                    outTime.Hour, outTime.Minute, outTime.Second);
               



                if (NewInTime <= NewOutTime)
                {
                    ProcedureExecute proc = new ProcedureExecute("Prc_AttendanceSystem");
                    proc.AddVarcharPara("@Action", 100, "updateAttendance");
                    proc.AddVarcharPara("@EmpId", 20, EmpId.Value);
                    proc.AddVarcharPara("@Emp_status", 20, EmpStatus.Value.ToString());
                    proc.AddDateTimePara("@Att_Date", new DateTime(Convert.ToInt32(AttDate.Split('-')[2]), Convert.ToInt32(AttDate.Split('-')[1]), Convert.ToInt32(AttDate.Split('-')[0])));
                    if (IntimeEdit.Text.Trim() != "")
                        proc.AddDateTimePara("@In_Time", NewInTime);
                    if (OutTimeEdit.Text.Trim() != "")
                        proc.AddDateTimePara("@Out_Time", NewOutTime);
                    proc.AddIntegerPara("@User", Convert.ToInt32(Session["userid"]));
                    proc.AddVarcharPara("@Remarks", 1000, txtRewmarks.Text); 
                    proc.RunActionQuery();
                    gridAttendance.DataBind();
                    gridAttendance.JSProperties["cpReturnMsg"] = "Update~Updated Successfully.";
                }
                else {
                    gridAttendance.JSProperties["cpReturnMsg"] = "Update~MustBeGreater.";
                }
            
            }
        }


        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                 
                    bindexport(Filter);
                 
            }
        }

        public void bindexport(int Filter)
        {

            string filename = "Employee Attendance Details";
                exporter.FileName = filename;

                exporter.PageHeader.Left = "Attendance Details (" + empButtonEdit.Text+")";
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
            
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
    
    
    }
}