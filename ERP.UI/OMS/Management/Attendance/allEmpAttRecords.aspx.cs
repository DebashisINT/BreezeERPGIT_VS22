using DataAccessLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class allEmpAttRecords : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            GridFullYear.JSProperties["cpProcExecuted"] = null;
            if (!IsPostBack)
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/attendance/AttendanceMonthwise.aspx");
                 
            }

            if (IsPostBack) 
            {
                DateTime FromDate = Convert.ToDateTime(HdFromDate.Value);
                DateTime ToDate = Convert.ToDateTime(HdToDate.Value);
                string ColumnName = "";
                foreach (GridViewBandColumn col in GridFullYear.Columns)
                {
                   
                    ColumnName = col.Name;
                    if (col.Name.StartsWith("Band"))
                    {
                        col.Caption = col.Caption + "-" + FromDate.ToString("MM-yyyy");
                        int Colid = Convert.ToInt32(ColumnName.Replace("Band", ""));
                        if (!(Colid >= FromDate.Day && Colid <= ToDate.Day)) 
                        {
                            col.Visible = false;
                        }
                    }
                }
            
            }


            if (HdBarcnhName.Value == "-All-")
                HdBarcnhName.Value = "All Unit(s)";
            if (HdBarcnhName.Value != "")
                GridFullYear.Columns["MainBandedHeader"].Caption = "Employee Details [" + HdBarcnhName.Value+"]";
        }

        protected void GridFullYear_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        { 

            ProcedureExecute proc = new ProcedureExecute("prc_callEmpAttendanceRpt");
            proc.AddVarcharPara("@UserId", 10,Convert.ToString(Session["userid"]));
            proc.AddVarcharPara("@BranchHierchy", -1, Convert.ToString(Session["userbranchHierarchy"]).Trim());
            ////proc.AddVarcharPara("@startDate", 10, MonthYear.Split('-')[1] + "-" + MonthYear.Split('-')[0]+"-01");
            proc.AddVarcharPara("@startDate", 10, HdFromDate.Value);
            proc.AddVarcharPara("@EndDate", 10, HdToDate.Value); 
            proc.AddVarcharPara("@EmpId", -1, hdEmpId.Value);
            proc.AddVarcharPara("@branchId", 10, hdBranchId.Value);
            proc.AddVarcharPara("@ShowInactive", 10, hdShowInactive.Value);

            proc.AddPara("@considerPayBranch",ConsiderPayBranch.Value);

            proc.RunActionQuery();
            GridFullYear.JSProperties["cpProcExecuted"] = "1";
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            if (IsPostBack)
            {
                e.KeyExpression = "EmpId";
              //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.tbl_EmpAttendanceRecord_reports
                        where d.UserId == Convert.ToInt64(Session["userid"]) 
                        orderby d.EmpName descending
                        select d;
                e.QueryableSource = q;
            }
            else {
                e.KeyExpression = "EmpId";

              //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.tbl_EmpAttendanceRecord_reports
                        where d.UserId == 99999
                        orderby d.EmpName descending
                        select d;
                e.QueryableSource = q;
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
            string filename = "Employeewise Attendance";
            exporter.FileName = filename; 
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