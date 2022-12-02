using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.OMS.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class SrvMastSMO : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
        FinancerExecutiveBL finEx = new FinancerExecutiveBL();
        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        clsDropDownList clsdropdown = new clsDropDownList();

        Srv_MastSMOBL MSOBL = new Srv_MastSMOBL();

        protected void Page_Init(object sender, EventArgs e)
        {
            branchdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                    Session["BranchListTableForMSO"] = null;
                    if (Request.QueryString["Type"] != null)
                    {
                        if (Request.QueryString["Type"] == "view")
                        {
                            btnSave.Style.Add("display", "none");
                            Session["ViewMode"] = "view";
                        }
                        else
                        {
                            Session["ViewMode"] = "";
                        }
                    }

                    if (Session["ViewMode"] != null)
                    {
                        if (Session["ViewMode"].ToString() == "view")
                        {
                            btnSave.Style.Add("display", "none");
                            headingName.InnerText = "View MSO";
                        }
                    }
                    ShowForm();
                    Session["ContactType"] = "MSO";
                    IsUdfpresent.Value = Convert.ToString(getUdfCount());
                }
                hdnstorequrystring.Value = Convert.ToString(Request.QueryString["id"]);
            }
            catch { }
        }

        protected void loadBranch()
        {
            branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";
            cmbMultiBranches.DataBind();
            cmbMultiBranches.Value = "0";

            //DataTable dt = MSOBL.GetBranch();
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    //clsdropdown.AddDataToDropDownList(Data, cmbBranch);
            //    //cmbBranch.DataSource = dt;

            //    //cmbBranch.DataTextField = "branchDescription";
            //    //cmbBranch.DataValueField = "branchId";
            //    //cmbBranch.DataBind();
            //    //cmbBranch.Items.Insert(0, new ListItem("All", "0"));
            //}
            //else
            //{
            //    //cmbBranch.Items.Insert(0, new ListItem("All", "0"));
            //}
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
                //  ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_firstName,cnt_branchId,Is_Active,cnt_mainAccount", " cnt_internalId='" + InternalId + "'", 5);
                DataTable dt = MSOBL.GetMSO(InternalId);
                SetfieldData(dt);

                SetBranchRecordToSessionTable(Convert.ToString(InternalId));
                ShowBranchName(Convert.ToString(InternalId));

                hdbBranch.Value = Convert.ToString(dt.Rows[0]["cnt_branchid"]);

                //DataTable ExFin = oDBEngine.GetDataTable("select executive_id  from tbl_master_FinancerExecutive where Fin_InternalId='" + InternalId + "'");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                string branch = string.Empty;
                branch = "0";
                string webLogin = "";
                string Password = "";
                DateTime defaultDt = Convert.ToDateTime("01-01-1900");
                if (Convert.ToString(Request.QueryString["id"]) == "ADD")
                {
                    DataTable dtMSOCode = oDBEngine.GetDataTable("select TOP(1)cnt_ucc from tbl_master_contact where cnt_contactType='MS' and cnt_ucc='" + txtMSOCode.Text.Trim() + "'");
                    if (dtMSOCode != null && dtMSOCode.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>ErrorpopUpRedirect();</script>");
                    }
                    else
                    {
                        if (txtMSOCode.Text != "")
                        {
                            webLogin = "Yes";
                            Password = txtMSOCode.Text;
                        }
                        else
                        {
                            webLogin = "No";
                            Password = "";
                        }

                        bool isActive = false;
                        if (chkActive.Checked)
                            isActive = true;
                        //cmbBranch.SelectedItem.Value
                        string InternalId = MSOBL.InsertMSOMaster("MSO", txtMSOCode.Text.Trim(), txtName.Text.Trim(), Convert.ToInt32(0), txtContactNo.Text.Trim(), isActive, Convert.ToString(HttpContext.Current.Session["userid"]), "MS", "ADD", hndTaxRates_MainAccount_hidden.Value, txtContactPerson.Text.Trim());

                        HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;
                        // Add branches for MSO
                        string BranchList = GetBranchList();
                        int brmap = MSOBL.insertMSOBranchMap(BranchList, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), Convert.ToInt16(branch));


                        DataTable dtcnt = MSOBL.GetMSO(InternalId);
                        if (dtcnt != null && dtcnt.Rows.Count > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>popUpRedirect('SrvMastSMO.aspx?id=" + Convert.ToString(dtcnt.Rows[0]["cnt_id"]) + "');</script>");
                        }
                    }
                }
                else
                {
                    bool isActive = false;
                    if (chkActive.Checked)
                        isActive = true;
                    String value = "";
                    //cmbBranch.SelectedItem.Value
                    int returns = MSOBL.UpdateMSOMaster(txtMSOCode.Text.Trim(), txtName.Text.Trim(), Convert.ToInt32(0), txtContactNo.Text.Trim(), isActive, Convert.ToString(HttpContext.Current.Session["userid"]), "EDIT", hndTaxRates_MainAccount_hidden.Value, Convert.ToInt32(HttpContext.Current.Session["KeyVal"]), txtContactPerson.Text.Trim());

                    // Add branches for MSO
                    string BranchList = GetBranchList();
                    int brmap = MSOBL.insertMSOBranchMap(BranchList, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), Convert.ToInt16(branch));


                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>popUpRedirect('SrvMastSMO.aspx?id=" + Convert.ToString(Request.QueryString["id"]) + "');</script>");
                    ShowForm();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string GetBranchList()
        {
            DataTable branchListtable = (DataTable)Session["BranchListTableForMSO"];
            string branchlist = "";
            if (branchListtable != null)
            {
                foreach (DataRow dr in branchListtable.Rows)
                {
                    branchlist = branchlist + "," + Convert.ToString(dr["Branch_id"]);
                }
            }

            branchlist = branchlist.TrimStart(',');
            return branchlist;
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
            //ErrorExecutive.Value = "";

            foreach (string obj in eachItem)
            {
                Expression = "loginId ='" + obj.Split('|')[1] + "'";
                loginId = dtUser.Select(Expression);
                if (loginId.Length > 0)
                {
                    retValue = false;
                }
                else
                {
                    loginId = dtexecutive.Select(Expression);
                    if (loginId.Length > 0)
                    {
                        retValue = false;
                    }
                }
            }
            return retValue;
        }

        protected void SetfieldData(DataTable ContactData)
        {
            if (ContactData != null && ContactData.Rows.Count > 0)
            {
                txtMSOCode.Text = ContactData.Rows[0]["cnt_UCC"].ToString();
                txtName.Text = ContactData.Rows[0]["cnt_firstName"].ToString();
                chkActive.Checked = Convert.ToBoolean(ContactData.Rows[0]["Is_Active"]);
                hndTaxRates_MainAccount_hidden.Value = ContactData.Rows[0]["cnt_mainAccount"].ToString();
                txtContactNo.Text = ContactData.Rows[0]["cnt_ContactNo"].ToString();
                txtContactPerson.Text = ContactData.Rows[0]["cnt_ContactPerson"].ToString();
            }
        }

        protected void ExecutiveCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string data = Convert.ToString(e.Parameter);
            saveUpdateData(data);
        }

        protected void saveUpdateData(string mode)
        {

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
                status = objMShortNameCheckingBL.CheckUnique(ShortName, ShortCode, "MS");
            }
            return status;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("SrvMastMSOList.aspx");
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='FI'   and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        [WebMethod]
        public static List<string> GetEmployeeList(string reqStr)
        {
            Srv_MastSMOBL MSOBL = new Srv_MastSMOBL();
            DataTable DT = MSOBL.GetEmployee();
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {
                obj.Add(Convert.ToString(dr["EMP_NAME"]) + "|" + Convert.ToString(dr["EMP_CODE"]));
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
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtExists = new DataTable();
            if (id != "ADD")
            {
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
                    obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["id"]));
                }
            }
            return obj;
        }

        protected void branchGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receviedString = e.Parameters;
            branchGrid.JSProperties["cpReceviedString"] = receviedString;

            if (receviedString == "SelectAllBranchesFromList")
            {
                branchGrid.Selection.SelectAll();
            }

            if (receviedString == "ClearSelectedBranch")
            {
                branchGrid.Selection.UnselectAll();
            }

            else if (receviedString == "SetAllRecordToDataTable")
            {
                List<object> branchList = branchGrid.GetSelectedFieldValues("branch_id");
                CreateBranchTable();
                DataTable branchListtable = (DataTable)Session["BranchListTableForMSO"];
                foreach (object obj in branchList)
                {
                    if (Convert.ToInt32(obj) != 0)
                        branchListtable.Rows.Add(Convert.ToInt32(obj));
                }
                if (hdnBranchAllSelected.Value == "0")
                {
                    if (branchListtable.Rows.Count > 0)
                    {
                        branchGrid.JSProperties["cpBrselected"] = 1;
                    }
                    else
                    {
                        branchGrid.JSProperties["cpBrselected"] = 0;
                    }
                }
                Session["BranchListTableForMSO"] = branchListtable;
            }
            else if (receviedString == "SetAllSelectedRecord")
            {
                DataTable branchListtable = (DataTable)Session["BranchListTableForMSO"];
                branchGrid.Selection.UnselectAll();
                if (branchListtable != null)
                {
                    foreach (DataRow dr in branchListtable.Rows)
                    {
                        branchGrid.Selection.SelectRowByKey(dr["Branch_id"]);
                        if (Convert.ToString(dr["Branch_id"]) == "0")
                        {
                            branchGrid.JSProperties["cpBrChecked"] = 1;
                        }
                    }
                }
            }
        }

        private void SetBranchRecordToSessionTable(string Keyvalue)
        {
            DataTable branchListtable = oDBEngine.GetDataTable("select branch_id Branch_id from Srv_master_TechnicianBranch_map where Tech_InternalId='" + Keyvalue + "'");
            if (branchListtable != null)
            {
                if (branchListtable.Rows[0]["branch_id"].ToString() == "0")
                {
                    branchListtable = oDBEngine.GetDataTable("select branch_id Branch_id from tbl_master_branch");
                }
            }
            Session["BranchListTableForMSO"] = branchListtable;
        }

        private void ShowBranchName(string Keyvalue)
        {
            string SelectedBranch = string.Empty;
            DataTable branchListtable = oDBEngine.GetDataTable("select m.branch_id Branch_id,b.branch_description  from Srv_master_TechnicianBranch_map m left join tbl_master_branch b on m.branch_id=b.branch_id where m.Tech_InternalId='" + Keyvalue + "'");

            if (branchListtable != null && branchListtable.Rows.Count > 0 && Convert.ToString(branchListtable.Rows[0]["Branch_Id"]) == "0")
            { lblSelectedBranch.Text = "All Branch"; }
            else
            {
                if (branchListtable != null)
                {
                    foreach (DataRow dr in branchListtable.Rows)
                    {
                        SelectedBranch = SelectedBranch + ", " + dr["branch_description"];
                    }
                }
                if (SelectedBranch.Length > 1)
                {
                    lblSelectedBranch.Text = SelectedBranch.Substring(1, SelectedBranch.Length - 1);
                }
                else
                {
                    lblSelectedBranch.Text = "";
                }
            }
        }

        public void CreateBranchTable()
        {
            DataTable branchListtable = new DataTable();
            branchListtable.Columns.Add("Branch_id", typeof(System.Int32));
            Session["BranchListTableForMSO"] = branchListtable;
        }
    }
}