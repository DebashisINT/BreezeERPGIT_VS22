using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
//////using DevExpress.Web;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System.Web.Services;
////using DevExpress.Web.ASPxTabControl;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee_EmployeeCTC : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataSet ds = new DataSet();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        void Gridbind()
        {
            string[] InputName = new string[1];
            string[] InputType = new string[1];
            string[] InputValue = new string[1];

            InputName[0] = "cnt_internalId";
            InputType[0] = "V";
            InputValue[0] = Session["KeyVal_InternalID"].ToString();
            ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[EmplyeeCTCSelect]", InputName, InputType, InputValue);
            EmployeeCTC.DataSource = ds.Tables[0];
            EmployeeCTC.DataBind();



        }
        protected void Page_Load(object sender, EventArgs e)
        
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
           
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/employee.aspx");
                Organization.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
               Responsibility.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                Designation.ConnectionString =Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                EmployeeType.ConnectionString =Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
               Department.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                LeaveScheme.ConnectionString =Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                ReportTo_Collegue_EmployeeDeputy.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                workingHr.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                Branch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //EployeeCTC.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    EployeeCTC.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //EployeeCTC.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    EployeeCTC.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
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


                if (Request.QueryString["id"] != null)
                {
                    //if (HttpContext.Current.Session["KeyVal_InternalID"] == null)
                    //{
                    Session["KeyVal_InternalID"] = Request.QueryString["id"].ToString();

                    //----Making TABs Disable------//
                    TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("General");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("DP Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Family Members");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Registration");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Group Member");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Employee");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Remarks");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Education");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Subscription");
                    page.Visible = false;


                    //}
                }
                // Gridbind();
                Session["mode"] = null;
                string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }

                DataTable DT_empId = oDBEngine.GetDataTable(" tbl_master_contact ", " cnt_id ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                if (DT_empId.Rows.Count > 0)
                {
                    string id= Convert.ToString(DT_empId.Rows[0]["cnt_id"]);
                    hdKeyVal.Value = id;
                }
                   


            }
        }

        protected void EmployeeCTC_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string data = e.GetValue("EffectiveUntil").ToString();
                if (data != "")
                {

                    int commandColumnIndex = -1;
                    for (int i = 0; i < EmployeeCTC.Columns.Count; i++)
                        if (EmployeeCTC.Columns[i] is GridViewCommandColumn)
                        {
                            commandColumnIndex = i;
                            break;
                        }
                    if (commandColumnIndex == -1)
                        return;
                    //____One colum has been hided so index of command column will be leass by 1 
                    commandColumnIndex = commandColumnIndex;// -1;

                    DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                    for (int i = 0; i < cell.Controls.Count; i++)
                    {
                        DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                        if (button == null) continue;

                        DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;
                        //hyperlink.Enabled = Convert.ToInt32(e.KeyValue) % 2 == 0;
                        hyperlink.Enabled = false;
                    }
                }



            }
        }

        protected void EmployeeCTC_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            DataTable DT_empCTC = oDBEngine.GetDataTable(" tbl_trans_employeeCTC ", " emp_id ", " emp_cntId='" + Session["KeyVal_InternalID"] + "'");
            if (DT_empCTC.Rows.Count > 0)
                e.NewValues["emp_dateofJoining"] = oDBEngine.GetDate();
            else
            {
                DataTable dt = oDBEngine.GetDataTable(" tbl_master_employee", "emp_dateofJoining", " (emp_contactId = '" + Session["KeyVal_InternalID"].ToString() + "')");
                e.NewValues["emp_dateofJoining"] = dt.Rows[0][0];

            }
        }

        protected void EmployeeCTC_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (!EmployeeCTC.IsNewRowEditing)
            {
                if (e.Column.FieldName == "emp_dateofJoining")
                    e.Editor.ReadOnly = false;

            }
            if (EmployeeCTC.IsNewRowEditing)
            {
                if (e.Column.FieldName == "emp_dateofJoining")
                {
                    DataTable dt = oDBEngine.GetDataTable(" tbl_master_employee", "emp_dateofJoining", " (emp_contactId = '" + Session["KeyVal_InternalID"].ToString() + "')");
                    ASPxDateEdit Date = (ASPxDateEdit)e.Editor;
                    if (dt.Rows[0][0].ToString() != "n")
                        Date.MinDate = Convert.ToDateTime(dt.Rows[0][0].ToString());
                }
            }
        }
        protected void EmployeeCTC_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!EmployeeCTC.IsNewRowEditing)
            {
                string data = Session["PageAccess"].ToString();
                ASPxGridViewTemplateReplacement RT = EmployeeCTC.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Edit" || Session["PageAccess"].ToString().Trim() == "All")
                {
                    RT.Visible = true;
                }
                else
                    RT.Visible = false;
            }
        }
        protected void EmployeeCTC_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            EmployeeCTC.JSProperties["cpmsg"] = null;
            if (e.Parameters != "")
            {
                string tranid = e.Parameters.ToString();
                string[] mainid = tranid.Split('~');
                if (mainid[0].ToString() == "Delete")
                {
                    DataTable dtemployee = new DataTable();
                    Employee_BL objEmployee = new Employee_BL();
                    dtemployee = objEmployee.NumberOfEmployeeCTC(Convert.ToInt32(mainid[1]));
                    if (Convert.ToInt32(dtemployee.Rows[0]["totalno"]) > 1)
                    {
                        DataTable DT_empCTC = oDBEngine.GetDataTable(" tbl_trans_employeeCTC ", " emp_effectiveuntil ", " EMP_ID='" + mainid[1].ToString() + "'");
                        if (DT_empCTC.Rows[0][0].ToString().Length > 0)
                        {
                            EmployeeCTC.JSProperties["cpmsg"] = "You can not delete this record.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "jAlert('You can not delete this record.');", true);
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>alert('You can not delete this record.');</script>");

                        }
                        else
                        {
                            //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                            String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                            SqlConnection lcon = new SqlConnection(con);
                            lcon.Open();
                            SqlCommand lcmdEmplInsert = new SqlCommand("employeeCTCdelete", lcon);
                            lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                            lcmdEmplInsert.Parameters.AddWithValue("@Id", mainid[1].ToString());
                            lcmdEmplInsert.ExecuteNonQuery();
                            // Mantis Issue 24802
                            if (lcon.State == ConnectionState.Open)
                            {
                                lcon.Close();
                            }
                            // End of Mantis Issue 24802
                            EmployeeCTC.DataBind();
                        }
                    }
                    else
                    {
                        EmployeeCTC.JSProperties["cpmsg"] = "You can not delete this record.";
                    }

                }
            }
        }

        //[WebMethod]
        //public static string IsUserLoggedIn()
        //{
        //    string EmployeeId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
        //    string[] LoggedInStatus = oDBEngine.GetFieldValue1("tbl_master_user", "user_status", " user_Contactid='" + EmployeeId + "'", 1);

        //    //if (LoggedInStatus[0] == "1")
        //    //{
        //    //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Associated user already logged in. To do any change user need to sign out from the system.')</script>", false);

        //    //}



        //    CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        //    int ispermission = 0;
        //    ispermission = objCRMSalesDtlBL.QuotationEditablePermission(Convert.ToInt32(ActiveUser));

            
        //    return Convert.ToString(ispermission);

        //}
    
    }
}