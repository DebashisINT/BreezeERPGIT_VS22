using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_NewEmployeeDetails : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        DataTable dt = new DataTable();
        public string pageAccess = "";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SqlEmployeeDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fromDate.Date = oDBEngine.GetDate();
                toDate.Date = oDBEngine.GetDate();
                populateCombobox();
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                //______________________________End Script____________________________//
            }
            populateEmployeeGrid();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        protected void populateCombobox()
        {
            int rowNo;
            //BindComapny Details
            dt = null;
            dt = oDBEngine.GetDataTable("tbl_master_company", "cmp_internalid,cmp_Name", null);
            rowNo = dt.Rows.Count;
            cmbCompany.Items.Add("All", 0);
            for (int i = 0; i < rowNo; i++)
            {
                cmbCompany.Items.Add(dt.Rows[i]["cmp_Name"].ToString(), dt.Rows[i]["cmp_internalid"].ToString());
            }
            cmbCompany.SelectedIndex = 0;

            //BindBranch Details
            dt = null;
            dt = oDBEngine.GetDataTable("tbl_master_branch", "branch_id,branch_internalId,branch_description", null);
            rowNo = dt.Rows.Count;
            cmbBranch.Items.Add("All", 0);
            for (int i = 0; i < rowNo; i++)
            {
                cmbBranch.Items.Add(dt.Rows[i]["branch_description"].ToString(), dt.Rows[i]["branch_id"].ToString());
            }
            cmbBranch.SelectedIndex = 0;

            //BindDepartments Details
            dt = null;
            dt = oDBEngine.GetDataTable("tbl_master_costCenter", "cost_id,cost_description", " cost_costCenterType = 'Department'");
            rowNo = dt.Rows.Count;
            cmbDepartment.Items.Add("All", 0);
            for (int i = 0; i < rowNo; i++)
            {
                cmbDepartment.Items.Add(dt.Rows[i]["cost_description"].ToString(), dt.Rows[i]["cost_id"].ToString());
            }
            cmbDepartment.SelectedIndex = 0;

        }

        protected void populateEmployeeGrid()
        {
            string appendCon = "";
            if (fromDate != null)
            {
                appendCon = " and tbl_master_employee.emp_dateofJoining >= '" + fromDate.Value + "'";
                if (toDate != null)
                {
                    appendCon = appendCon + " and tbl_master_employee.emp_dateofJoining <= '" + toDate.Value + "'";
                }
            }
            if (cmbCompany.SelectedItem.Text != "All")
            {
                appendCon = appendCon + " and tbl_master_company.cmp_Name = '" + cmbCompany.SelectedItem.Text.Trim() + "'";
            }
            if (cmbBranch.SelectedItem.Text != "All")
            {
                appendCon = appendCon + " and tbl_master_contact.cnt_branchid = " + cmbBranch.SelectedItem.Value.ToString();
            }
            if (cmbDepartment.SelectedItem.Text != "All")
            {
                appendCon = appendCon + " and tbl_master_costCenter.cost_description = '" + cmbDepartment.SelectedItem.Text.Trim() + "'";
            }
            if (fromDate.Text != "")
            {
                SqlEmployeeDetails.SelectCommand = " select distinct CONVERT(CHAR(11),tbl_master_employee.emp_dateofJoining,113) as joindate,tbl_master_employee.emp_dateofJoining,emp_contactId,ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '')+'['+ISNull(tbl_master_contact.cnt_shortName,'')+']' AS Name, " +
                                                    " tbl_master_contact.cnt_branchid,b.branch_description branch,tbl_trans_employeeCTC.emp_Organization,tbl_master_company.cmp_Name company,emp_Designation,tbl_master_designation.deg_designation designation,emp_Department,tbl_master_costCenter.cost_description department,tblReportTO.reportTo " +
                                                    " from tbl_master_contact,tbl_master_employee,tbl_trans_employeeCTC,tbl_master_company,tbl_master_branch as b,tbl_master_designation,tbl_master_costCenter, " +
                                                    " (select distinct emp_cntId,ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '')+'['+ISNull(tbl_master_contact.cnt_shortName,'')+']' reportTo from tbl_trans_employeeCTC left outer join tbl_master_employee on tbl_master_employee.emp_id = tbl_trans_employeeCTC.emp_reportTo inner join tbl_master_contact on tbl_master_contact.cnt_internalId = tbl_master_employee.emp_contactId where tbl_trans_employeeCTC.emp_effectiveuntil is null) as tblReportTO " +
                                                    " where tbl_master_employee.emp_contactId=tbl_master_contact.cnt_internalId " +
                                                    " and tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId " +
                                                    " and tbl_master_company.cmp_id = tbl_trans_employeeCTC.emp_Organization " +
                                                    " and b.branch_id = tbl_master_contact.cnt_branchid " +
                                                    " and tbl_master_company.cmp_id = tbl_trans_employeeCTC.emp_Organization " +
                                                    " and tbl_master_designation.deg_id = tbl_trans_employeeCTC.emp_Designation " +
                                                    " and tbl_master_costCenter.cost_id = tbl_trans_employeeCTC.emp_Department " +
                                                    " and tblReportTO.emp_cntId = tbl_master_contact.cnt_internalId " +
                                                    " and tbl_trans_employeeCTC.emp_effectiveuntil is null" +
                                                    appendCon +
                                                    " order by tbl_master_employee.emp_dateofJoining  desc ";
            }
            aspxEmpGrid.DataBind();
        }
        protected void aspxEmpGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] strSplit = e.Parameters.Split('~');
            string WhichCall = strSplit[0];
            aspxEmpGrid.ClearSort();
            if (WhichCall == "BindGrid")
            {
                populateEmployeeGrid();
            }
            //aspxEmpGrid.DataBind();


        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
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