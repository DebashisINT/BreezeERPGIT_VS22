using DataAccessLayer;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class AttendanceChartReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMonthDropDown();
            }
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            LoadBranchWiseSale();
        }
        public void LoadBranchWiseSale()
        {
            string id = Convert.ToString(cmbMonthYear.Value);


            ProcedureExecute proc = new ProcedureExecute("Prc_AttendanceSystem");
            proc.AddVarcharPara("@Action", 100, "GetEmploggedByMonth");
            proc.AddVarcharPara("@EmpId", 20, EmpId.Value);
            proc.AddVarcharPara("@startDate", 10, id.Split('-')[1] + "-" + id.Split('-')[0] + "-01");
            System.Data.DataTable salesTable = proc.GetTable();
            Session["chartData"] = salesTable;
            foreach (DataRow Dr in salesTable.Rows)
            {
                
                Series series1 = new Series(Convert.ToString( Convert.ToDateTime(Dr["AttDate"]).ToString("dd-MM-yyyy")), ViewType.Bar);
                DevExpress.XtraCharts.SeriesPoint SpObj; 
                SpObj = new SeriesPoint(Convert.ToDateTime(Dr["AttDate"]), Convert.ToDecimal(Dr["WorkingHour"]));
                SpObj.ToolTipHint = "Custom Text1";
                series1.Points.Add(SpObj);
                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Tag =Convert.ToString( Dr["keyString"]);

                series1.CrosshairLabelPattern = "{S} - " + String.Format("{0:00}", Convert.ToDecimal(Dr["WorkingHour"])) + ":" + String.Format("{0:00}", Convert.ToDecimal(Dr["WorkingMin"])); 


                WebChartControl1.Series.Add(series1);
            }
            WebChartControl1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            WebChartControl1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            WebChartControl1.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;

            XYDiagram diagram = (XYDiagram)WebChartControl1.Diagram;
            diagram.AxisY.WholeRange.Auto = false;
            diagram.AxisY.WholeRange.SetMinMaxValues(1, 16);

            
        }
        public void LoadMonthDropDown()
        {

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
            while (startMonth < EndMonth)
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
        }

        protected void WebChartControl1_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            DataTable chartDt = (DataTable)Session["chartData"];
            DataRow[] darray = chartDt.Select("keyString='" + e.Series.Tag + "'");
            e.LabelText =String.Format("{0:00}", Convert.ToDecimal(darray[0]["WorkingHour"])) + ":" + String.Format("{0:00}", Convert.ToDecimal(darray[0]["WorkingMin"]));
        }
    }
}