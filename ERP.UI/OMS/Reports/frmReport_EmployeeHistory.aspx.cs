using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_EmployeeHistory : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        SqlDataAdapter commanAdapter;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtName_hidden.Style["Display"] = "none";
            if (!IsPostBack)
            {
                string[,] branches = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id,(branch_description+'['+branch_code+']') as branch ", " branch_id in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", 2, " branch_description ");
                clsdrp.AddDataToDropDownList(branches, cmbBranch, "All");
                cmbBranch.SelectedIndex = 0;
                string[,] companies = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_id,cmp_name ", null, 2);
                clsdrp.AddDataToDropDownList(companies, cmbCompany, "All");
                cmbCompany.SelectedIndex = 0;
            }
            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            //______________________________End Script____________________________//
            txtName.Attributes.Add("onkeyup", "CallAjax(this,'employeeBranchHrchy',event)");
            if (IsCallback)
                populateEmployeeDetails();

        }
        protected void populateEmployeeDetails()
        {
            DataTable dtBasic = new DataTable();
            SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            if (txtName_hidden.Text.Trim() != "")
            {
                string lsSql = "select cnt_shortName code,(ISNULL(tms.sal_name,'') + '. ' +ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'')) as fullname,emp_contactId,CONVERT(VARCHAR(12), emp_dateofJoining, 113) DOJ,CONVERT(VARCHAR(12), emp_dateofLeaving, 113) DOL from tbl_master_employee emp,tbl_master_contact cnt,tbl_master_salutation tms where cnt.cnt_internalId=emp.emp_contactId and tms.sal_id=cnt.cnt_salutation " +
                                " and emp.emp_contactId='" + txtName_hidden.Text.Trim() + "'" +
                                " order by emp.emp_dateofJoining desc ";
                commanAdapter = new SqlDataAdapter(lsSql, lcon);
                commanAdapter.Fill(dtBasic);
                commanAdapter = null;
                if (dtBasic.Rows.Count > 0)
                {
                    //BindGridDetails(dtBasic.Rows[0]["emp_cntId"].ToString(), commanAdapter, lcon);

                    lblEmpName.Text = dtBasic.Rows[0]["fullname"].ToString();
                    lblCode.Text = dtBasic.Rows[0]["code"].ToString();
                    lblDOJ.Text = dtBasic.Rows[0]["DOJ"].ToString();
                    lblDOL.Text = dtBasic.Rows[0]["DOL"].ToString();
                }

            }
            BindGridDetails(txtName_hidden.Text.Trim(), commanAdapter, lcon);
            lcon.Close();
            lcon.Dispose();
            dtBasic.Dispose();
            /*--------End if clause-------------*/

        }
        protected void BindGridDetails(string empid, SqlDataAdapter da, SqlConnection con)
        {
            DataTable dtgrid = new DataTable();
            string sql = "select emp_cntId,(select wor_scheduleName from tbl_master_workingHours where wor_id=emp_workinghours) workingHr,ISNULL(emp_currentCTC,'') ctc,(select ISNULL(deg_designation,'') from tbl_master_designation where deg_id=emp_Designation) deg,emp_uniqueCode Code,ISNULL(cmp_Name,'') cmp,ISNULL(branch_description,'') branch,CONVERT(VARCHAR(12), emp_effectiveDate, 113) joindate,  CONVERT(VARCHAR(12), emp_effectiveuntil, 113) enddate,(select (ISNULL(rptcnt.cnt_firstName,'')+' '+ISNULL(rptcnt.cnt_middleName,'')+' '+ISNULL(rptcnt.cnt_lastName,'')) from  tbl_master_contact rptcnt,tbl_master_employee emp  where emp.emp_contactId=rptcnt.cnt_internalId and emp.emp_id=emp_reportTo) reportHead  " +
                        " from tbl_trans_employeeCTC inner join  tbl_master_contact on tbl_master_contact.cnt_internalId=tbl_trans_employeeCTC.emp_cntId  " +
                        " inner join tbl_master_company on tbl_master_company.cmp_id=tbl_trans_employeeCTC.emp_Organization  " +
                        " inner join tbl_master_branch on tbl_master_branch.branch_id=tbl_trans_employeeCTC.emp_branch  " +
                        " inner join tbl_master_employee on tbl_master_employee.emp_contactId=tbl_trans_employeeCTC.emp_cntId  " +
                        " where emp_contactId='" + empid.Trim() + "'" +
                        " order by emp_effectiveuntil DESC ";
            da = new SqlDataAdapter(sql, con);
            da.Fill(dtgrid);
            gridHistory.DataSource = dtgrid;
            gridHistory.DataBind();
            dtgrid.Dispose();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            populateEmployeeDetails();
        }
    }
}