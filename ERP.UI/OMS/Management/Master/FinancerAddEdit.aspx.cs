using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxClasses;
//using DevExpress.Web.ASPxEditors;
//using DevExpress.Web.ASPxGridView;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;
using System.Text;
using DataAccessLayer;

namespace ERP.OMS.Managemnent.Master
{
    public partial class management_Master_FinancerAddEdit : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
        FinancerExecutiveBL finEx = new FinancerExecutiveBL();
        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        clsDropDownList clsdropdown = new clsDropDownList();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Code  Added and Commented By Priti on 21122016 to add Convert.ToString instead of ToString()
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/BranchAddEdit.aspx");

                if (!IsPostBack)
                {
                    loadBranch();
                    ShowForm();
                    Session["ContactType"] = "Financer";
                    IsUdfpresent.Value = Convert.ToString(getUdfCount());
                }
                hdnstorequrystring.Value = Convert.ToString(Request.QueryString["id"]);

            }
            catch { }
        }


        protected void loadBranch()
        {
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_branch ", "branch_id as branchId, branch_description as branchDescription", null, 2, "branch_description");
            if (Data[0, 0] != "n")
            {
                clsdropdown.AddDataToDropDownList(Data, cmbBranch);
                cmbBranch.Items.Insert(0, new ListItem("None", "0"));
            }
            else
            {
                cmbBranch.Items.Insert(0, new ListItem("None", "0"));
            }
        }

        protected void ShowForm()
        {
            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                TabPage page = PageControl1.TabPages.FindByName("Correspondence");
                page.Enabled = false;
                page = PageControl1.TabPages.FindByName("Documents");
                page.Enabled = false;
                btnUdf.Enabled = false;
                hiddenedit.Value = "";
            }
            else
            {
                string FinId = Convert.ToString(Request.QueryString["id"]);
                string[,] ContactData;
                string InternalId = "";
                HttpContext.Current.Session["KeyVal"] = FinId;
                hiddenedit.Value = FinId;

                ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId", "cnt_id=" + FinId, 1);
                if (ContactData[0, 0] != "n")
                {
                    InternalId = ContactData[0, 0];
                    Session["KeyVal_InternalID"] = ContactData[0, 0];
                }
                ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_firstName,cnt_branchId,Is_Active,cnt_mainAccount", " cnt_internalId='" + InternalId + "'", 5);
                SetfieldData(ContactData);

                hdbBranch.Value = Convert.ToString(ContactData[0, 2]);
                // BindExecutiveDropDown(Convert.ToString(ContactData[0,2]));
                //DataTable ExFin = oDBEngine.GetDataTable("select ExecutiveName,ExecutiveuserId,ExecutivePassword,ExecutiveIsActive  from tbl_master_FinancerExecutive where Fin_InternalId='" + InternalId + "'");
                //executiveName_hidden.Value = "";
                //    foreach (DataRow dr in ExFin.Rows)
                //    {
                //        executiveName_hidden.Value = executiveName_hidden.Value + "~" + Convert.ToString(dr["ExecutiveName"]) + "|" + Convert.ToString(dr["ExecutiveuserId"]) + "|" + Convert.ToString(dr["ExecutivePassword"]) + "|" + (Convert.ToBoolean(dr["ExecutiveIsActive"])== true?"1":"0");
                //    }
                //    executiveName_hidden.Value = Convert.ToString(executiveName_hidden.Value).TrimStart('~');
                DataTable ExFin = oDBEngine.GetDataTable("select executive_id  from tbl_master_FinancerExecutive where Fin_InternalId='" + InternalId + "'");
                hdnEditAssignTo.Value = "";
                foreach (DataRow dr in ExFin.Rows)
                {

                    if (hdnEditAssignTo.Value == "")
                    {
                        hdnEditAssignTo.Value = Convert.ToString(dr["executive_id"]);
                    }
                    else
                    {
                        hdnEditAssignTo.Value = hdnEditAssignTo.Value + "," + Convert.ToString(dr["executive_id"]);
                    }
                }
                hdnEditAssignTo.Value = Convert.ToString(hdnEditAssignTo.Value).TrimStart('~');
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string webLogin = "";
                string Password = "";
                DateTime defaultDt = Convert.ToDateTime("01-01-1900");
                if (Convert.ToString(Request.QueryString["id"]) == "ADD")
                {
                    if (txtFinancerId.Text != "")
                    {
                        webLogin = "Yes";
                        Password = txtFinancerId.Text;
                    }
                    else
                    {
                        webLogin = "No";
                        Password = "";
                    }

                    bool isActive = false;
                    if (chkActive.Checked)
                        isActive = true;

                    //if (CheckUnique(Convert.ToString(executiveName_hidden.Value).TrimStart('~'),null) )
                    //{
                    string InternalId = finEx.InsertFinancerMaster("Financer", txtFinancerId.Text.Trim(), txtFinancerName.Text.Trim(), Convert.ToInt32(cmbBranch.SelectedItem.Value), "", isActive, Convert.ToString(HttpContext.Current.Session["userid"]), "FI", "ADD");

                    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;


                    //Insert into tbl_master_FinancerExecutive
                    //finEx.InsertFinancerExecutive(InternalId, Convert.ToString(executiveName_hidden.Value).TrimStart('~'));
                    finEx.InsertFinancerExecutive(InternalId, Convert.ToString(hdnAssign.Value).Trim());
                    //End here
                    Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "'", " cnt_internalId='" + InternalId + "'");

                    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalId + "'", 1);
                    if (Convert.ToString(cnt_id[0, 0]) != "n")
                    {
                        //Response.Redirect("FinancerAddEdit.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>popUpRedirect('FinancerAddEdit.aspx?id=" + Convert.ToString(cnt_id[0, 0]) + "');</script>");
                    }
                    //}
                }
                else
                {
                    string isActive = "0";
                    if (chkActive.Checked)
                        isActive = "1";
                    String value = "";
                    value = "cnt_branchid=" + cmbBranch.SelectedItem.Value + ",cnt_UCC='" + txtFinancerId.Text.Trim() + "',cnt_firstName='" + txtFinancerName.Text.Trim() + "',cnt_shortName='" + txtFinancerId.Text.Trim() + "',Is_Active=" + isActive + ",cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "'";

                    //if (CheckUnique(Convert.ToString(executiveName_hidden.Value).TrimStart('~'), Convert.ToString(Session["KeyVal_InternalID"])))
                    //{
                    oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                    //update into tbl_master_FinancerExecutive 
                    finEx.InsertFinancerExecutive(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToString(hdnAssign.Value).TrimStart('~'));
                    //End here
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>popUpRedirect('FinancerAddEdit.aspx?id=" + Convert.ToString(Request.QueryString["id"]) + "');</script>");
                    //}
                    ShowForm();
                }
            }
            catch (Exception ex)
            {

            }


        }

        public bool CheckUnique(string ExecutiveList, string IntId)
        {
            Boolean retValue = true;
            DataTable dtUser = oDBEngine.GetDataTable("select user_loginId loginId from tbl_master_user");
            DataTable dtexecutive;
            if (IntId == null)
                dtexecutive = oDBEngine.GetDataTable("select ExecutiveuserId loginId from tbl_master_FinancerExecutive");
            else
                dtexecutive = oDBEngine.GetDataTable("select ExecutiveuserId loginId from tbl_master_FinancerExecutive where Fin_InternalId='" + IntId + "'");
            DataRow[] loginId;
            String Expression = "";
            string[] eachItem = ExecutiveList.Split('~');
            ErrorExecutive.Value = "";

            foreach (string obj in eachItem)
            {

                Expression = "loginId ='" + obj.Split('|')[1] + "'";
                loginId = dtUser.Select(Expression);
                if (loginId.Length > 0)
                {
                    ErrorExecutive.Value = ErrorExecutive.Value + "~" + obj.Split('|')[1];
                    retValue = false;
                }
                else
                {
                    loginId = dtexecutive.Select(Expression);
                    if (loginId.Length > 0)
                    {
                        ErrorExecutive.Value = ErrorExecutive.Value + "~" + obj.Split('|')[1];
                        retValue = false;
                    }
                }
            }
            ErrorExecutive.Value = Convert.ToString(ErrorExecutive.Value).TrimStart('~');
            return retValue;

        }

        protected void SetfieldData(string[,] ContactData)
        {
            if (ContactData[0, 0] != "n")
            {
                txtFinancerId.Text = ContactData[0, 0];
                txtFinancerName.Text = ContactData[0, 1];
                cmbBranch.SelectedValue = ContactData[0, 2];
                chkActive.Checked = Convert.ToBoolean(ContactData[0, 3]);
                hndTaxRates_MainAccount_hidden.Value = ContactData[0, 4];
                string[] getData = oDBEngine.GetFieldValue1("Trans_AccountsLedger", "COUNT(*)", "AccountsLedger_MainAccountID='" + ContactData[0, 4] + "' and  AccountsLedger_MainAccountID<>''", 1);
                if (getData[0] == "0")
                    hdIsMainAccountInUse.Value = "notInUse";
                else
                    hdIsMainAccountInUse.Value = "IsInUse";
            }

        }
        protected void ExecutiveCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string data = Convert.ToString(e.Parameter);
            saveUpdateData(data);
        }

        protected void saveUpdateData(string mode)
        {

            //string webLogin = "";
            //string Password = "";
            //DateTime defaultDt = Convert.ToDateTime("01-01-1900");
            //if (mode == "SAVE")
            //{
            //    if (txtFinancerId.Text != "")
            //    {
            //        webLogin = "Yes";
            //        Password = txtFinancerId.Text;
            //    }
            //    else
            //    {
            //        webLogin = "No";
            //        Password = "";
            //    }

            //    string InternalId = oContactGeneralBL.Insert_ContactGeneral("Financer", txtFinancerId.Text.Trim(), "0",
            //                                         txtFinancerName.Text.Trim(), "", "",
            //                                         txtFinancerId.Text.Trim(), cmbBranch.SelectedItem.Value, "0", "0"
            //                                        , defaultDt, defaultDt, null,
            //                                        null, null, null,
            //                                        null, null, null,
            //                                         null, null, null, "FI",
            //                                         "5", defaultDt, "0", "",
            //                                         null, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), "",
            //                                         defaultDt, "", "0", false, 0, 0
            //                                          );
            //    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;

            //    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalId + "'", 1);
            //    if (Convert.ToString(cnt_id[0, 0]) != "n")
            //    {
            //        Response.Redirect("FinancerAddEdit.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
            //    }

            //}
            //else
            //{
            //    String value = "";
            //    value = "cnt_branchid=" + cmbBranch.SelectedItem.Value + ",cnt_UCC='" + txtFinancerId.Text.Trim() + "',cnt_firstName='" + txtFinancerName.Text.Trim() + "',cnt_shortName='" + txtFinancerId.Text.Trim() + "'";
            //    oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
            //}

        }


        [WebMethod]
        public static bool CheckUniqueName(string ShortName, string FinCode)
        {
            string ShortCode = "0";

            if (HttpContext.Current.Session["KeyVal_InternalID"] != null)
            {
                ShortCode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            }
            if (FinCode == "0")
            {
                ShortCode = "0";
            }

            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (ShortCode != "" && Convert.ToString(ShortName).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(ShortName, ShortCode, "Financer");
            }
            return status;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Financer.aspx");
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='FI'   and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        [WebMethod]
        public static List<string> GetMainAccountList(string reqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            // DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");

            //  DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name, MainAccount_AccountCode ", " MainAccount_AccountCode not like 'SYS%'");

            ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            proc.AddVarcharPara("@action", 20, "GetMainAccount");
            DT = proc.GetTable();


            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["MainAccount_Name"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
            }
            return obj;
        }


        public void BindExecutiveDropDown(string branchid)
        {
            DataTable dtbl = new DataTable();
            dtbl = oDBEngine.GetDataTable("select  distinct(c.cnt_firstName+c.cnt_middleName+c.cnt_lastName) as name,c.cnt_id as id from tbl_master_contact c inner join tbl_trans_employeeCTC e on c.cnt_internalId=e.emp_cntId inner join tbl_master_user TBU  on c.cnt_internalId=TBU.user_contactId   where e.emp_type in(select emptpy_id from tbl_master_employeeType where emptpy_id=21)  and c.cnt_branchid=" + branchid + "");


        }
        [WebMethod]
        public static List<string> GetAllUserListBeforeSelect(string id, string branchid)
        {
            StringBuilder filter = new StringBuilder();
            Employee_BL objemployeebal = new Employee_BL();
            string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
            DataTable dtbl = new DataTable();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtExists = new DataTable();
            if (id != "ADD")
            {
                // dtExists = oDBEngine.GetDataTable("select executive_id= STUFF((select ',' + Cast(executive_id as varchar(50)) from tbl_master_FinancerExecutive  where Fin_InternalId not in(select cnt_internalId from  tbl_master_contact where cnt_id ='" + id + "') and executive_id<>0  For XML PATH('')),1,1,'')");


                //   dtbl = oDBEngine.GetDataTable("select distinct(c.cnt_firstName+c.cnt_middleName+c.cnt_lastName) as name,c.cnt_id as id from tbl_master_contact c inner join tbl_trans_employeeCTC e on c.cnt_internalId=e.emp_cntId where e.emp_type in(select emptpy_id from tbl_master_employeeType where emptpy_id=21) and c.cnt_id not in(" + dtExists.Rows[0][0].ToString() + ")");

                dtbl = oDBEngine.GetDataTable("select  distinct(c.cnt_firstName+c.cnt_middleName+c.cnt_lastName) as name,c.cnt_id as id from tbl_master_contact c inner join tbl_trans_employeeCTC e on c.cnt_internalId=e.emp_cntId inner join tbl_master_user TBU  on c.cnt_internalId=TBU.user_contactId   where e.emp_type in(select emptpy_id from tbl_master_employeeType where emptpy_id=21)  and c.cnt_branchid=" + branchid + "  and c.cnt_id  not in (select executive_id from tbl_master_FinancerExecutive  where Fin_InternalId not in (select cnt_internalId  from tbl_master_contact where cnt_id='" + id + "'))");

            }
            else
            {

                dtbl = oDBEngine.GetDataTable("select  distinct(c.cnt_firstName+c.cnt_middleName+c.cnt_lastName) as name,c.cnt_id as id from tbl_master_contact c inner join tbl_trans_employeeCTC e on c.cnt_internalId=e.emp_cntId inner join tbl_master_user TBU  on c.cnt_internalId=TBU.user_contactId   where e.emp_type in(select emptpy_id from tbl_master_employeeType where emptpy_id=21)  and c.cnt_branchid=" + branchid + " and c.cnt_id  not in (select executive_id from tbl_master_FinancerExecutive) ");

            }

            List<string> obj = new List<string>();
            if (dtbl != null && dtbl.Rows.Count > 0)
            {
                foreach (DataRow dr in dtbl.Rows)
                {

                    //obj.Add(Convert.ToString(dr["emptpy_type"]) + "|" + Convert.ToString(dr["emptpy_id"]));
                    obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["id"]));
                }

            }

            return obj;
        }
    }
}
