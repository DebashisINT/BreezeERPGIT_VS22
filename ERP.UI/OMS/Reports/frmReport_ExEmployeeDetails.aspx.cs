using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_ExEmployeeDetails : System.Web.UI.Page
    {
        #region GlobalVariable
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataTable dt = new DataTable();
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        public string pageAccess = "";
        public string PageSize = "10";
        #endregion

        #region Page Property
        public string PageNum
        {
            get { return (string)Session["PageNum"]; }
            set { Session["PageNum"] = value; }
        }
        #endregion

        #region PageClass
        private void bindCombo()
        {
            string[,] data = oDBEngine.GetFieldValue(" tbl_master_company ", " [cmp_id], [cmp_Name]", null, 2, " cmp_Name");
            clsdrp.AddDataToDropDownList(data, cmbCompany, "All");
            data = oDBEngine.GetFieldValue(" tbl_master_branch ", " [branch_id], [branch_description] ", null, 2, " branch_description");
            clsdrp.AddDataToDropDownList(data, cmbBranch, "All");
            data = oDBEngine.GetFieldValue(" tbl_master_costCenter ", " [cost_id],[cost_description] ", " cost_costCenterType = 'department' ", 2, " cost_description");
            clsdrp.AddDataToDropDownList(data, cmbDepartment, "All");
            //data = oDBEngine.GetFieldValue(" tbl_master_contact ", " [cnt_internalID],(cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+' ['+isnull(cnt_shortName,'')+']') as name ", " cnt_internalID like 'EM%' ", 2, " cnt_firstName ");

            //oDBEngine.AddDataToDropDownList(data, cmbReplacement, "no");

        }

        protected void populateExEmployeeGrid(string PageNum, string PageSize)
        {
            string sqlQuery;
            string maintable = @" (select (select top 1 cnt_id from tbl_master_contact  where cnt_internalid=emp_contactID) as emp_id, emp_contactID,emp_dateofjoining as joindate,
                         emp_dateofleaving Leavedate,(select cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'') from tbl_master_contact where cnt_internalID=emp_contactID) as Name,
                        (select isnull(cnt_shortName,'') from tbl_master_contact where cnt_internalID=emp_contactID) as code,
                        (select deg_designation from tbl_master_designation where deg_id in(select top 1 emp_designation from tbl_trans_employeectc where emp_cntID=emp_contactID order by emp_effectiveDate desc)) as designation," +
                            " (select cmp_name from tbl_master_company where cmp_id in(select top 1 emp_organization from tbl_trans_employeectc where emp_cntID=emp_contactID order by emp_effectiveDate desc)) as company," +
                            " (select branch_description from tbl_master_branch where branch_id in(select top 1 emp_branch from tbl_trans_employeectc where emp_cntID=emp_contactID order by emp_effectiveDate desc)) as branch," +
                            " (Select cost_description from tbl_master_costCenter where cost_costCenterType = 'department' and  cost_id in(select top 1 emp_Department from tbl_trans_employeectc where emp_cntID=emp_contactID order by emp_effectiveDate desc)) as department," +
                            " (select cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+' ['+isnull(cnt_shortName,'')+']' from tbl_master_contact where cnt_internalID=(select D.emp_contactID from tbl_master_employee D where D.emp_id=(select top 1 emp_reportTo from tbl_trans_employeectc where emp_cntID=tbl_master_employee.emp_contactID order by emp_effectiveDate desc))) as reportTo," +
                            " (select top 1 emp_Department from tbl_trans_employeectc where emp_cntID=emp_contactID order by emp_effectiveDate desc) as departmentid," +
                            " (select top 1 emp_branch from tbl_trans_employeectc where emp_cntID=emp_contactID order by emp_effectiveDate desc) as branchid," +
                            " (select top 1 emp_organization from tbl_trans_employeectc where emp_cntID=emp_contactID order by emp_effectiveDate desc) as companyid" +
                            " from tbl_master_employee where emp_dateofleaving between '" + fromDate.Value + "' and '" + toDate.Value + "') as D";

            string wherecondition = "";

            if (cmbCompany.SelectedItem.Value.ToString() != "All")
            {
                wherecondition += " companyid = '" + cmbCompany.SelectedItem.Value + "'";
            }
            if (cmbBranch.SelectedItem.Value.ToString() != "All")
            {
                if (wherecondition == "")
                    wherecondition += " branchid = " + cmbBranch.SelectedItem.Value;
                else
                    wherecondition += " and branchid = " + cmbBranch.SelectedItem.Value;
            }
            if (cmbDepartment.SelectedItem.Value.ToString() != "All")
            {
                if (wherecondition == "")
                    wherecondition += " departmentid = '" + cmbDepartment.SelectedItem.Value + "'";
                else
                    wherecondition += " and departmentid = '" + cmbDepartment.SelectedItem.Value + "'";
            }


            if (wherecondition != "")
            {
                sqlQuery = "(Select  ROW_NUMBER()  OVER (ORDER BY  emp_contactID) As [Srl. No], * from " + maintable + " where" + wherecondition + ")as N";
            }
            else
            {
                sqlQuery = "(Select  ROW_NUMBER()  OVER (ORDER BY  emp_contactID) As [Srl. No], * from " + maintable + ")as N";
            }

            DataTable dtGrid = new DataTable();
            DataTable DtCount = new DataTable();
            string whereconditionNavigation = "[Srl. No] BETWEEN (" + PageNum + "- 1) * " + PageSize + @" + 1 AND " + PageNum + "* " + PageSize;
            dtGrid = oDBEngine.GetDataTable(sqlQuery, " * ", whereconditionNavigation);
            DtCount = oDBEngine.GetDataTable(sqlQuery, " COUNT(emp_contactID) TotalRow ", null);
            aspxExEmpGrid.DataSource = dtGrid;
            aspxExEmpGrid.DataBind();

            if (DtCount.Rows.Count > 0)
            {
                int TotalItems = Convert.ToInt32(DtCount.Rows[0]["TotalRow"].ToString());
                int TotalPage = TotalItems % Convert.ToInt32(PageSize) == 0 ? (TotalItems / Convert.ToInt32(PageSize)) : (TotalItems / Convert.ToInt32(PageSize)) + 1;
                //string strSplitSearchBy = Session["strSearchBy"] != null ? Session["strSearchBy"].ToString() : String.Empty;
                aspxExEmpGrid.JSProperties["cpIsEmptyDsSearch"] = "No~" + PageNum + '~' + TotalPage + "~" + TotalItems;
                //+ '~' + strSplitSearchBy.Substring(0, strSplitSearchBy.LastIndexOf(','));
            }
            else
            {
                aspxExEmpGrid.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
            }
        }

        #endregion

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
            Department.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlExEmployeeDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            // cmbReplacement.Attributes.Add("onkeyup", "CallList(this,'SearchByEmpCont',event)");
            // txtReportTo.Attributes.Add("onkeyup", "CallList(this,'SearchByEmpCont',event)");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            Page.Title = "InFlux CRM Ex-Employee's Detail Report";
            if (!IsPostBack)
            {

                fromDate.EditFormatString = Oconverter.GetDateFormat("Date");
                fromDate.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                toDate.EditFormatString = Oconverter.GetDateFormat("Date");
                toDate.Value = oDBEngine.GetDate();

                //cmbRejoinDate.EditFormatString = Oconverter.GetDateFormat("Date");
                //fromDate.EditFormatString = Oconverter.GetDateFormat("Date");
                //toDate.EditFormatString = Oconverter.GetDateFormat("Date");
                PageNum = "1";
                bindCombo();
                cmbCompany.SelectedIndex = 0;
                cmbBranch.SelectedIndex = 0;
                cmbDepartment.SelectedIndex = 0;
                Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>pageload();</script>");
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
            if (IsCallback)
            {
                populateExEmployeeGrid(PageNum, PageSize);
            }
        }



        protected void aspxExEmpGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            aspxExEmpGrid.JSProperties["cpIsEmptyDsSearch"] = "undefined";
            string command = e.Parameters.Split('~')[0];

            if (e.Parameters.Split('~')[2] == "s")
                aspxExEmpGrid.Settings.ShowFilterRow = true;
            if (e.Parameters.Split('~')[2] == "All")
            {
                aspxExEmpGrid.FilterExpression = string.Empty;
            }

            if (command == "SearchByNavigation")
            {
                PageNum = e.Parameters.Split('~')[1];

                populateExEmployeeGrid(PageNum, PageSize);
            }

        }

        //protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    //if (cmbReplacement.Value != null)
        //    //{
        //    //    string[,] emplIds = oDBEngine.GetFieldValue(" tbl_master_employee ", " Distinct emp_id ", " emp_contactId in (select C.emp_cntId from tbl_trans_employeeCTC C where (C.emp_reportTo = (SELECT EMP_ID FROM tbl_master_employee where emp_contactId='" + cmbReplacement_hidden.Value + "')) and (C.emp_effectiveuntil is null OR C.emp_effectiveuntil='1/1/1900 12:00:00 AM'))", 1);
        //    //    if (emplIds[0, 0] == "n")
        //    //        childemployees.Value = "n";
        //    //    else
        //    //    {
        //    //        string empIdsG = "";
        //    //        for (int i = 0; i < emplIds.Length; i++)
        //    //        {
        //    //            if (empIdsG == "")
        //    //                empIdsG = emplIds[i, 0];
        //    //            else
        //    //                empIdsG += "," + emplIds[i, 0];
        //    //        }
        //    //        childemployees.Value = empIdsG;
        //    //    }
        //    //}
        //    //else
        //     if(txtReportTo_hidden.Value !="")
        //     {
        //       childemployees.Value = "n";

        //       DataTable dtCmp = oDBEngine.GetDataTable("tbl_master_employee ", " * ", "emp_contactid='" + txtReportTo_hidden.Value + "'");
        //    SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //    lcon.Open();

        //    SqlCommand lcmd = new SqlCommand("Employee_Rejoin", lcon);
        //    lcmd.CommandType = CommandType.StoredProcedure;
        //    lcmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = e.Parameter;
        //    lcmd.Parameters.Add("@DOJ", SqlDbType.DateTime).Value = Convert.ToDateTime(cmbRejoinDate.Value.ToString());
        //    lcmd.Parameters.Add("@Users", SqlDbType.VarChar).Value = childemployees.Value;
        //    lcmd.Parameters.Add("@modifyUser", SqlDbType.Int).Value = int.Parse(HttpContext.Current.Session["userid"].ToString());
        //    lcmd.Parameters.Add("@ReportTo", SqlDbType.Int).Value = int.Parse(HttpContext.Current.Session["userid"].ToString());

        //    lcmd.ExecuteNonQuery();

        //    lcmd.Dispose();
        //    lcon.Close();
        //    lcon.Dispose();

        //    }
        //}


    }
}