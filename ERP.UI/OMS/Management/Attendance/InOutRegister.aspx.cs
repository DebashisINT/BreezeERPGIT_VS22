using DataAccessLayer;
using DevExpress.Export;
using DevExpress.XtraPrinting;
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
    public partial class InOutRegister : System.Web.UI.Page
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

            FormDate.MinDate = Convert.ToDateTime(dt.Rows[0]["finYearStartDate"]);

            if (DateTime.Now > Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]))
            {
                FormDate.Date = Convert.ToDateTime(dt.Rows[0]["finYearEndDate"]);
            }
            else
            {
                FormDate.Date = DateTime.Now;
            }



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

        protected void GridDetail_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string branchList="";

            if (cmbBranch.Value.ToString() == "0")
            {

                for (int i = 0; i < cmbBranch.Items.Count; i++)
                {
                    branchList = branchList + cmbBranch.Items[i].Value.ToString() + ",";
                }
                branchList = branchList.TrimEnd(',');

            }
            else
                branchList = cmbBranch.Value.ToString();


            ProcedureExecute proc = new ProcedureExecute("Prc_callInOutRegister");
            proc.AddPara("@branch", branchList);
            proc.AddPara("@AllEmp", chkAllEmp.Checked);
            proc.AddPara("@EmpList", EmpId.Value);
            proc.AddPara("@Date", FormDate.Date.ToString("yyyy/MM/dd"));
            proc.AddPara("@ShowInactive", chkInactive.Checked);
            proc.AddPara("@Userid", Session["userid"].ToString());
            proc.AddPara("@considerPayBranch",chkPayrollBranch.Checked);

            proc.RunActionQuery();



            GridDetail.DataBind();
        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "internalId";
            if (IsPostBack)
            {
               // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                e.QueryableSource = from d in dc.tbl_report_inOutRegisters
                                    where d.userId == Convert.ToInt64(Session["userid"])
                                    select d;
            }
            else {

                //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                e.QueryableSource = from d in dc.tbl_report_inOutRegisters
                                    where d.userId == 999999
                                    select d;
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