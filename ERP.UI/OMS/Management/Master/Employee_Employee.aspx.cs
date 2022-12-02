using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using DevExpress.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee_Employee : ERP.OMS.ViewState_class.VSPage
    {
        public string[] InputName = new string[5];
        public string[] InputType = new string[5];
        public string[] InputValue = new string[5];
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public  EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        #region AjaxList
        void CallReplacementEmployee()
        {
            string strQuery_Table = @"(Select  ISNULL(Ltrim(Rtrim(cnt_firstName)), '') + ' ' + ISNULL(Ltrim(Rtrim(cnt_middleName)), '') + ' ' + ISNULL(Ltrim(Rtrim(cnt_lastName)), '')+ ' [' + ISNULL(Ltrim(Rtrim(cnt_shortName)), '')+']' AS Name,cnt_shortName UniqueCode,emp_id as ID from tbl_master_employee,tbl_master_contact Where emp_contactId=cnt_internalId and isnull(emp_dateofLeaving,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000') T1";
            string strQuery_FieldName = "Top 10 Name,ID,UniqueCode";
            string strQuery_WhereClause = "Name Like '%RequestLetter%' Or UniqueCode Like '%RequestLetter%'";
            string strQuery_OrderBy = "";
            string strQuery_GroupBy = "";
            string CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;
            CombinedQuery = CombinedQuery.Replace("'", "\\'");
            txtReplacement.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedQuery + "')");
        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            Sqljobresponsibility.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlDept.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Sqldesignation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                #region Check Payroll Active Or Not

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable ConfigDT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='IsPayrollActive' AND IsActive=1");
                if (ConfigDT != null && ConfigDT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(ConfigDT.Rows[0]["Variable_Value"]).Trim();
                    if (IsMandatory == "Yes")
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                    else
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                }
                else
                {
                    ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                }
                #endregion
            }

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            
           rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/employee.aspx");
           

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //sqlHistory.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    sqlHistory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //sqlHistory.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    sqlHistory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            chkReplacement.Attributes.Add("onclick", "ShowReplacementNew(this.checked)");
            if (!IsPostBack)
            {
                string Header1 = "";
                string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    Header1 = EmployeeNameID[0, 0].ToUpper();
                }

                DataTable DT_empId = oDBEngine.GetDataTable(" tbl_master_contact ", " cnt_id ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                if (DT_empId.Rows.Count > 0)
                {
                    string id = Convert.ToString(DT_empId.Rows[0]["cnt_id"]);
                    hdKeyVal.Value = id;
                }
                string[,] EmployeeData = oDBEngine.GetFieldValue("tbl_master_employee", " Top 1 emp_contactId,case when emp_dateofJoining='1/1/1900 12:00:00 AM' then null else emp_dateofJoining end as emp_dateofJoining,case when emp_dateofLeaving='1/1/1900 12:00:00 AM' then null else emp_dateofLeaving end as emp_dateofLeaving,emp_ReasonLeaving,emp_NextEmployer,emp_AddNextEmployer,emp_previousEmployer1,emp_previousCTC1,case when emp_joinpreviousEmployer1='1/1/1900 12:00:00 AM' then null else  emp_joinpreviousEmployer1 end as emp_joinpreviousEmployer1,case when emp_toPreviousEmployer1='1/1/1900 12:00:00 AM' then null else emp_toPreviousEmployer1 end as emp_toPreviousEmployer1,emp_PreviousDesignation1,emp_JobResponsibility1,emp_AddressPrevEmployer1,emp_PANNoPrevEmployer1,emp_TDSPrevEmployer1,emp_PreviousTaxableIncome1, emp_previousEmployer2,emp_previousCTC2,case when emp_joinpreviousEmployer2='1/1/1900 12:00:00 AM' then null else emp_joinpreviousEmployer2 end as emp_joinpreviousEmployer2,case when emp_toPreviousEmployer2='1/1/1900 12:00:00 AM' then null else emp_toPreviousEmployer2 end as emp_toPreviousEmployer2,emp_PreviousDesignation2,emp_JobResponsibility2,emp_AddPrevEmployer2,emp_PANNoPrevEmployer2,emp_TDSPrevEmployer2,emp_PreviousTaxableIncome2,emp_id,emp_uniqueCode,emp_din,emp_ReplacingTO", "emp_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 30);// ='" +   "' order by emp_dateofJoining desc " , 26);
                //string[,] EmployeeData = oDBEngine.GetFieldValue("tbl_master_employee", " Top 1 emp_contactId,emp_dateofJoining,emp_dateofLeaving,emp_ReasonLeaving,emp_NextEmployer,emp_AddNextEmployer,emp_previousEmployer1,emp_previousCTC1,emp_joinpreviousEmployer1,emp_toPreviousEmployer1,emp_PreviousDesignation1,emp_JobResponsibility1,emp_AddressPrevEmployer1,emp_PANNoPrevEmployer1,emp_TDSPrevEmployer1,emp_PreviousTaxableIncome1, emp_previousEmployer2,emp_previousCTC2,emp_joinpreviousEmployer2,emp_toPreviousEmployer2,emp_PreviousDesignation2,emp_JobResponsibility2,emp_AddPrevEmployer2,emp_PANNoPrevEmployer2,emp_TDSPrevEmployer2,emp_PreviousTaxableIncome2", "emp_contactId='EMP0000029' ", 26);
                if (EmployeeData[0, 0] != "n")
                {
                    lblHeader.Text = Header1;
                    //txtEmployeeID.Text = EmployeeData[0, 27];
                    txtDIN.Text = EmployeeData[0, 28];
                    txtEmployeeID.ReadOnly = true;
                    if (EmployeeData[0, 1] == "")
                        cmbDOJ.Value = null;
                    else
                        cmbDOJ.Value = Convert.ToDateTime(EmployeeData[0, 1]);
                    if (EmployeeData[0, 2] == "")
                        cmbDOL.Value = null;
                    else
                        cmbDOL.Value = Convert.ToDateTime(EmployeeData[0, 2]);
                    txtROLeaving.Text = EmployeeData[0, 3];
                    txtNextEmployeerName.Text = EmployeeData[0, 4];
                    txtNextEmployeerAddress.Text = EmployeeData[0, 5];


                    emp_id.Text = EmployeeData[0, 26];
                    if (EmployeeData[0, 29].ToString().Trim() != "")
                    {
                        chkReplacement.Checked = true;
                        txtReplacement.Text = EmployeeData[0, 29].ToString().Trim();
                        txtReplacement_hidden.Value = EmployeeData[0, 29].ToString().Trim();
                        Page.ClientScript.RegisterStartupScript(GetType(), "chk", "<script language='javascript'>ShowReplacementNew(true);</script>");
                    }
                }
                else
                {

                    txtEmployeeID.Text = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
                }

            }

            ///////////Call Ajax Method
            CallReplacementEmployee();


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            ClearArray();
            DateTime today = oDBEngine.GetDate();
            string[,] UCode = oDBEngine.GetFieldValue("tbl_master_employee", "emp_id,emp_din", " emp_din='" + txtDIN.Text + "'", 2);
            if (UCode[0, 0] != "n")
            {
                if (UCode[0, 1] != "")
                {
                    if ((UCode[0, 0] != emp_id.Text) && (txtDIN.Text != " "))
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('This DIN is already Exists !')</script>");
                        return;
                    }
                }
            }
            if (cmbDOL.Value == null)
            {
                String value = "";
                if (chkReplacement.Checked == true)
                {
                    value = "emp_uniqueCode ='" + txtEmployeeID.Text + "',emp_din='" + txtDIN.Text + "', emp_dateofJoining ='" + cmbDOJ.Value + "', emp_dateofLeaving ='" + cmbDOL.Value + "',emp_ReasonLeaving  ='" + txtROLeaving.Text + "', emp_NextEmployer ='" + txtNextEmployeerName.Text + "', emp_AddNextEmployer  ='" + txtNextEmployeerAddress.Text + "',emp_ReplacingTO='" + txtReplacement_hidden.Value + "'";
                    string[,] data = oDBEngine.GetFieldValue("  (select *,(select C.emp_ReportTo from tbl_trans_employeeCTC C where C.emp_effectiveuntil=D.emp_effectiveuntil and C.emp_cntId=D.emp_cntId) as reportTo from (" +
                                " select max(emp_effectiveuntil) as emp_effectiveuntil,emp_cntId from tbl_trans_employeeCTC CTC,tbl_master_employee emp where emp.emp_contactId=CTC.emp_cntId and (emp.emp_DateOfLeaving is null or emp.emp_DateOfLeaving='1/1/1900 12:00:00 AM') and  CTC.emp_effectiveuntil is not null " +
                                " group by CTC.emp_cntId) as D) as E,tbl_master_employee ee",
                                " ee.emp_id ",
                                " ee.emp_contactId=E.emp_cntId and E.reportTo=" + txtReplacement_hidden.Value.ToString().Trim(), 1);
                    if (data[0, 0] != "n")
                    {
                        string empIdsG = "";
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (empIdsG == "")
                                empIdsG = data[i, 0];
                            else
                                empIdsG += "," + data[i, 0];
                        }
                        string[,] empID = oDBEngine.GetFieldValue(" tbl_master_employee ", " emp_id ", " emp_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString().Trim() + "'", 1);
                        //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                        SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        lcon.Open();

                        SqlCommand lcmd = new SqlCommand("Employee_Replacement", lcon);
                        lcmd.CommandType = CommandType.StoredProcedure;
                        lcmd.Parameters.Add("@ReplacementId", SqlDbType.VarChar).Value = empID[0, 0].ToString().Trim();
                        lcmd.Parameters.Add("@Users", SqlDbType.VarChar).Value = empIdsG;
                        lcmd.Parameters.Add("@modifyUser", SqlDbType.Int).Value = int.Parse(HttpContext.Current.Session["userid"].ToString());
                        lcmd.ExecuteNonQuery();

                        lcmd.Dispose();
                        lcon.Close();
                        lcon.Dispose();
                    }
                }
                else
                {
                    value = "emp_din='" + txtDIN.Text + "', emp_dateofJoining ='" + cmbDOJ.Value + "', emp_dateofLeaving ='" + cmbDOL.Value + "',emp_ReasonLeaving  ='" + txtROLeaving.Text + "', emp_NextEmployer ='" + txtNextEmployeerName.Text + "', emp_AddNextEmployer  ='" + txtNextEmployeerAddress.Text + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'";
                    Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_employee", value, " emp_id =" + emp_id.Text);
                    Int32 RowsEffect = oDBEngine.SetFieldValue("tbl_trans_employeeCTC", "emp_effectiveuntil='" + cmbDOL.Value + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " emp_cntId='" + Session["KeyVal_InternalID"].ToString() + "'");
                }

            }
            else
            {
                String value = "emp_din='" + txtDIN.Text + "', emp_dateofJoining ='" + cmbDOJ.Value + "', emp_dateofLeaving ='" + cmbDOL.Value + "',emp_ReasonLeaving  ='" + txtROLeaving.Text + "', emp_NextEmployer ='" + txtNextEmployeerName.Text + "', emp_AddNextEmployer  ='" + txtNextEmployeerAddress.Text + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'";
                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_employee", value, " emp_id =" + emp_id.Text);
                Int32 RowsEffect = oDBEngine.SetFieldValue("tbl_trans_employeeCTC", "emp_effectiveuntil='" + cmbDOL.Value + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " emp_cntId='" + Session["KeyVal_InternalID"].ToString() + "' and (emp_effectiveuntil is null or emp_effectiveuntil='1/1/1900 12:00:00 AM')");
            }


        }

        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }

        protected void gridHistory_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            gridHistory.SettingsText.PopupEditFormCaption = "Modify Employment Details";
        }

        //protected void gridHistory_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        //{
        //    gridHistory.SettingsText.PopupEditFormCaption = "Add Employment Details";
        //}

        protected void gridHistory_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }

    }
}