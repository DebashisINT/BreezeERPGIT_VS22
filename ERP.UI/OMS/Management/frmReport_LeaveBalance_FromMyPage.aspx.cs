using ClsDropDownlistNameSpace;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Management
{
    public partial class Reports_frmReport_LeaveBalance_FromMyPage : System.Web.UI.Page
    {
        DataTable DT_Main = new DataTable();

        clsDropDownList oclsDropDownList = new clsDropDownList();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        //   DBEngine oDBEngine = new DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {

                string Contactid = oDBEngine.GetFieldValue("tbl_master_user", "user_contactid", "user_id='" + Session["userid"].ToString() + "'", 1)[0, 0];
                string[,] branches = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id,(branch_description+'['+ltrim(rtrim(branch_code))+']') as branch ", " branch_id in (select cnt_branchid from tbl_master_contact where cnt_internalid='" + Contactid.ToString().Trim() + "')", 2, " branch_description ");
                oclsDropDownList.AddDataToDropDownList(branches, cmbBranch, "All");
                cmbBranch.SelectedIndex = 1;
                string[,] companies = oDBEngine.GetFieldValue(" (select emp_organization from tbl_trans_employeectc where emp_cntid='" + Contactid.ToString() + "' and (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000')) as a inner join tbl_master_company as com on com.cmp_id=a.emp_organization ", " com.cmp_id,com.cmp_name ", null, 2);
                oclsDropDownList.AddDataToDropDownList(companies, cmbCompany, "All");
                cmbCompany.SelectedIndex = 1;
                cmbBranch.Enabled = false;
                cmbCompany.Enabled = false;
                cmbDate.MaxDate = oDBEngine.GetDate();
                cmbDate.Value = oDBEngine.GetDate();
                //string[,] branches = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id,(branch_description+'['+branch_code+']') as branch ", " branch_id in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", 2, " branch_description ");
                //oDBEngine.AddDataToDropDownList(branches, cmbBranch, "All");
                //cmbBranch.SelectedIndex = 0;
                //string[,] companies = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_id,cmp_name ", null, 2);
                //oDBEngine.AddDataToDropDownList(companies, cmbCompany, "All");
                //cmbCompany.SelectedIndex = 0;
                //txtName.Attributes.Add("onkeyup", "CallAjax(this,'employeeBranchHrchy',event)");
                txtName_hidden.Text = Contactid.ToString().Trim();
                txtName.Text = oDBEngine.GetFieldValue("tbl_master_contact", " isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') as name", "cnt_internalid='" + Contactid.ToString().Trim() + "'", 1)[0, 0];
                txtName.Enabled = false;
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                { }
                    //ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                    //______________________________End Script____________________________//
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }

            if (IsPostBack)
            {
                populateTreeList();

            }
        }


        protected void populateTreeList()
        {
            DT_Main = new DataTable();

            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            string lsSql = " select compName,code,branch,EmpName,Designation,(select top 1 (cast(lab_PLCurrentYear as decimal(18,2))) from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) TotalPL, " +
                        " (select top 1 (cast(lab_PLtotalAvailedThisYear as decimal(18,2)))  from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) PLAvailed, " +
                       " (select top 1 (cast(lab_PLCurrentYear as decimal(18,2))) - (cast(lab_PLtotalAvailedThisYear as decimal(18,2)))  from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) PLAveliable, " +

                    " (select top 1 (cast(lab_CLCurrentYear as decimal(18,2))) from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) TotalCL, " +
                    " (select top 1 (cast(lab_CLtotalAvailedThisYear as decimal(18,2)))  from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) CLAvailed, " +
                    " (select top 1 (cast(lab_CLCurrentYear as decimal(18,2))) - (cast(lab_CLtotalAvailedThisYear as decimal(18,2)))  from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) CLAveliable, " +

                    " (select top 1 (cast(lab_SLCurrentYear as decimal(18,2))) from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) TotalSL, " +
                    " (select top 1 (cast(lab_SLtotalAvailedThisYear as decimal(18,2)))  from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) SLAvailed, " +
                    " (select top 1 (cast(lab_SLCurrentYear as decimal(18,2))) - (cast(lab_SLtotalAvailedThisYear as decimal(18,2)))  from tbl_trans_LeaveAccountBalance where lab_contactId=ID  order by lab_effectiveDate DESC ) SLAveliable  " +
                    " from (select D.ID " +
                    ",(select top 1 cmp_Name from tbl_master_company where cmp_id=(select top 1 emp_Organization from tbl_trans_employeeCTC where emp_cntId=D.ID order by emp_effectiveDate desc)) compName" +
                    ",(select isnull(cnt_firstName,'')+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'') from tbl_master_contact where tbl_master_contact.cnt_internalId=D.ID) EmpName" +
                    ",(select isnull(cnt_shortName,'') from tbl_master_contact where tbl_master_contact.cnt_internalId=D.ID) code" +
                    ",  (select top 1 branch_description from tbl_master_branch where branch_id=(select top 1 emp_branch from tbl_trans_employeeCTC where emp_cntId=D.ID order by emp_effectiveDate desc)) branch " +
                    ",(select top 1 deg_designation from tbl_master_designation where deg_id=(select top 1 emp_Designation from tbl_trans_employeeCTC where emp_cntId=D.ID order by emp_effectiveDate desc)) Designation" +
                    ",(select top 1 CreateDate   from tbl_trans_employeeCTC where emp_cntId=D.ID order by emp_effectiveDate desc)  as CreateDate" +
                    " from (select  ctc.emp_cntID as ID from tbl_trans_employeeCTC ctc,tbl_master_employee emp where emp.emp_contactId=ctc.emp_cntID and (emp.emp_dateofLeaving is null or emp.emp_dateofLeaving='1/1/1900 12:00:00 AM' or emp.emp_dateofLeaving>cast('" + cmbDate.Value + "' as datetime)) and" +
                    " (ctc.emp_effectiveuntil>cast('" + cmbDate.Value + "' as datetime) or ctc.emp_effectiveuntil is null)";

            if (cmbCompany.SelectedItem.Value.ToString() != "All")
                lsSql += " and emp_branch in (" + cmbBranch.Value + ")";
            else
                lsSql += " and emp_branch in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";

            if (cmbCompany.Value.ToString().Trim() != "All")
                lsSql += " and emp_Organization=" + cmbCompany.Value;
            if (rbUser.SelectedItem.Value.ToString() != "A")
                lsSql += " and emp_cntID='" + txtName_hidden.Text + "'";
            lsSql += " group by emp_cntID ) as D";
            lsSql += ") LM where compName is not null and branch is not null and EmpName is not null order by CreateDate DESC";
            //if (rbUser.SelectedItem.Value.ToString() == "A")
            //{
            //    lsSql = lsSql + "(select isnull(cnt_firstName,'')+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+isnull(cnt_shortName,'')+']' from tbl_master_contact where tbl_master_contact.cnt_internalId=emp_cntId) EmpName, ";

            //}
            //else
            //{
            //    if (txtName_hidden.Text.Trim() != "")
            //lsSql = lsSql + ",(select isnull(cnt_firstName,'')+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+isnull(cnt_shortName,'')+']' from tbl_master_contact where tbl_master_contact.cnt_internalId=D.ID) EmpName";
            //}

            ///lsSql = lsSql + " (select top 1 branch_description from tbl_master_branch where branch_id=emp_branch) branch,(select top 1 deg_designation from tbl_master_designation where deg_id=emp_Designation) Designation,CreateDate " +
            // " from tbl_master_employee,tbl_trans_employeeCTC where (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 ectc.emp_effectiveuntil from tbl_trans_employeeCTC ectc  where ectc.emp_cntId=tbl_trans_employeeCTC.emp_cntId order by createdate desc)) )";

            //if (cmbBranch.SelectedItem.Value.ToString() != "All")
            //{
            //    lsSql = lsSql + " and emp_branch=" + cmbBranch.SelectedItem.Value.ToString();
            //}
            //else
            //{
            //    lsSql += " and emp_branch in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ") ";
            //}
            //lsSql += " and emp_cntId in (select distinct lab_contactId from tbl_trans_LeaveAccountBalance where lab_effectiveDate <= '" + cmbDate.Date.ToString() + "')";
            //lsSql = lsSql + ") LM where compName is not null and branch is not null and EmpName is not null order by CreateDate DESC";

            lcon.Open();

            SqlDataAdapter lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Main);

            GridLeaveBalenece.DataSource = DT_Main.DefaultView;
            GridLeaveBalenece.DataBind();


            lcon.Close();
            lcon.Dispose();
            DT_Main.Dispose();

        }

        protected void GridLeaveBalenece_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

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