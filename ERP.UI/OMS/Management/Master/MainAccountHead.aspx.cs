using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections;
using DataAccessLayer;
using System.Web.UI.WebControls;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Linq;
using System.Text.RegularExpressions;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_MainAccountHead : ERP.OMS.ViewState_class.VSPage
    {
        string Error = "a";
        string Error1 = "a";
        string EditCompanyCode = String.Empty;
        string EditBranchCode = String.Empty;

        public static string AccountGp = "";
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        int NoofRowsAffected = 0;
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                //string sPath = HttpContext.Current.Request.Url.ToString();
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);

                // .............................Code Above Commented and Added by Sam on 06122016...................................... 

                oDBEngine.Call_CheckPageaccessebility(sPath);
                Session["exportval"] = null;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            MainAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AllAccountGroup.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            branchdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSegment.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            tdstcs.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //hdnAssetType.Value = "";
            MainAccountGrid.JSProperties["cpDelete"] = null;
            MainAccountGrid.JSProperties["cpValidating"] = null;
            MainAccountGrid.JSProperties["cpinsert"] = null;
            MainAccountGrid.JSProperties["cpUpdate"] = null;
            Session["requesttype"] = "Account Heads";
            Session["ContactType"] = "Account Heads";
            Session["KeyVal_InternalID"] = "";
            Session["Name"] = "";

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/MainAccountHead.aspx");

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            GetGridDetails();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            // this.Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>PageLoad();</script>");

            
            if (!IsPostBack)
            {
                
                chkSysAccount.Attributes.Add("onclick", "checkChange(this.checked)");

                //// Added By : Samrat Roy , Date : 19/06/2017 
                //// Purpose : To bind HSN/SAC list for Mapping with Ledger

                MainAccountHead_BL mahbl = new MainAccountHead_BL();
                DataSet ds = mahbl.GetHSNSACNoList();

                Session.Add("sessHsnLookUp", ds.Tables[0]);
                HsnLookUp.DataSource = ds.Tables[0];
                HsnLookUp.DataBind();

                Session.Add("sessScaLookUp", ds.Tables[1]);
                ScaLookUp.DataSource = ds.Tables[1];
                ScaLookUp.DataBind();

                // Mantis Issue 24953
                BindPostingTypes();
                // End of Mantis Issue 24953

                //Session["AssetType"] = null;

            }
        }


        protected void HsnLookUp_DataBinding(object sender, EventArgs e)
        {
            if (Session["sessHsnLookUp"] != null)
            {
                HsnLookUp.DataSource = (DataTable)Session["sessHsnLookUp"];
            }
        }
        protected void ScaLookUp_DataBinding(object sender, EventArgs e)
        {
            if (Session["sessScaLookUp"] != null)
            {
                ScaLookUp.DataSource = (DataTable)Session["sessScaLookUp"];
            }
        }


        Dictionary<string, object> values = new Dictionary<string, object>();
        protected void btnSearch(object sender, EventArgs e)
        {
            MainAccountGrid.Settings.ShowFilterRow = true;

        }

        public void bindexport(int Filter)
        {
            MainAccountGrid.Columns[16].Visible = false;
            MainAccountGrid.Columns[17].Visible = false;
            MainAccountGrid.Columns[18].Visible = false;
            MainAccountGrid.Columns[19].Visible = false;
            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = "Account Head";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Account Head";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

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
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        protected void ASPxComboBox4_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpInsertError1"] = Error1;
        }
        protected void ASPxComboBox4_Callback(object source, CallbackEventArgsBase e)
        {
            MainAccountHead_BL mahbl = new MainAccountHead_BL();
            //using (SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                //string id = e.Parameter.ToString();
                //string[] idlist = id.Split('~');
                //string AccountCode = idlist[0].ToString();
                //string SubLedgerType = idlist[1].ToString();

                string id = Convert.ToString(e.Parameter);
                string[] idlist = id.Split('~');
                string AccountCode = Convert.ToString(idlist[0]);
                string SubLedgerType = Convert.ToString(idlist[1]);

                // .............................Code Above Commented and Added by Sam on 06122016...................................... 

                if (SubLedgerType == "Custom")
                {


                    //using (SqlCommand lcmd = new SqlCommand("DeleteCustomMain", lcon))
                    //{
                    //    lcmd.CommandType = CommandType.StoredProcedure;
                    //    lcmd.Parameters.Add("@mainAc", SqlDbType.VarChar, 15).Value = AccountCode;
                    //    lcon.Open();
                    //   NoofRowsAffected = lcmd.ExecuteNonQuery();
                    //    lcon.Close();
                    //    if (NoofRowsAffected <= 0)
                    //    {
                    //        Error1 = "b";
                    //    }
                    //}
                    NoofRowsAffected = mahbl.MainAccountGridDeleteCustomMain(AccountCode);
                    if (NoofRowsAffected <= 0)
                    {
                        Error1 = "b";
                    }
                }
                else
                {
                    //using (SqlCommand lcmd = new SqlCommand("DeleteMainAccountHead", lcon))
                    //{
                    //    lcmd.CommandType = CommandType.StoredProcedure;
                    //    lcmd.Parameters.Add("@AcountCode", SqlDbType.VarChar, 15).Value = AccountCode;
                    //    lcon.Open();
                    //   NoofRowsAffected = lcmd.ExecuteNonQuery();
                    //    lcon.Close();
                    //    if (NoofRowsAffected <= 0)
                    //    {
                    //        Error1 = "b";
                    //    }
                    //}
                    NoofRowsAffected = mahbl.MainAccountGridDeleteMainAccountHead(AccountCode);
                    if (NoofRowsAffected <= 0)
                    {
                        Error1 = "b";
                    }
                }

            }
        }
        protected void comboSegment_Callback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");
            ASPxComboBox comboSegment = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboSegment");
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

            //string CompID = comboCompanyName.SelectedItem.Value.ToString();
            string CompID = Convert.ToString(comboCompanyName.SelectedItem.Value);

            // .............................Code Above Commented and Added by Sam on 06122016...................................... 

            SqlSegment.SelectCommand = " select exch_internalId,isnull(((select top 1 exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId),exch_membershipType) as Exchange from tbl_master_companyExchange where exch_compId='" + CompID + "'";
            comboSegment.DataBind();
        }
        private void GetGridDetails()
        {
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            string Segment = "";
            if (Session["userlastsegment"] != null)
            {
                //string Segment = Session["userlastsegment"].ToString();
                Segment = Convert.ToString(Session["userlastsegment"]);
            }
            // .............................Code Above Commented and Added by Sam on 06122016...................................... 


            if (chkSysAccount.Checked == true)
            {
                

                MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable,  [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%' AND(MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";
                 
            }
            else
                
                MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where (MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";

          
            if (Segment == "5")
            {
                MainAccountGrid.Columns[20].Visible = true;
                MainAccountGrid.Columns[19].Visible = true;
            }
            else
            {
                //MainAccountGrid.Columns[20].Visible = false;
                //MainAccountGrid.Columns[19].Visible = false;
            }
        }

        // Mantis Issue 24953
        private void BindPostingTypes()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
            proc.AddVarcharPara("@Action", 20, "GetPostingType");
            ds = proc.GetDataSet();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                cmbPostingType.ValueField = "ID";
                cmbPostingType.TextField = "POSTING_TYPE";
                cmbPostingType.DataSource = ds.Tables[0];
                cmbPostingType.DataBind();
            }
        }

        [WebMethod]
        public static string SavePostingType(int AccountTypeID, int PostingTypeID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
                proc.AddVarcharPara("@Action", 20, "SavePostingType");
                proc.AddBigIntegerPara("@AccountTypeID", Convert.ToInt64(AccountTypeID));
                proc.AddBigIntegerPara("@PostingTypeID", Convert.ToInt64(PostingTypeID));
                proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                NoOfRowEffected = proc.RunActionQuery();
                output = Convert.ToString(proc.GetParaValue("@is_success"));
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            return output;
        }

        [WebMethod]
        public static int GetSelectedPostingType(int AccountTypeID){
            
            int PostingTypeId = 0;

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
            proc.AddVarcharPara("@Action", 20, "GetPostingID");
            proc.AddIntegerPara("@AccountTypeID", AccountTypeID);
            //ds = proc.GetDataSet();

            PostingTypeId = Convert.ToInt32(proc.GetScalar());
                       

            return PostingTypeId;
        }
        // End of Mantis Issue 24953


        #region GridView Event
        protected void MainAccountGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            //e.CommandArgs.CommandName
            if (e.CommandArgs.CommandName == "AddToCart")
            {

            }
        }

        protected void MainAccountGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            //if (!rights.CanDelete)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Delete)
            //    {
            //        e.Visible = false;
            //    }
            //}


            //if (!rights.CanEdit)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Edit)
            //    {
            //        e.Visible = false;
            //    }
            //}
        }

        protected void MainAccountGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            MainAccountGrid.JSProperties["cpDelete"] = null;
            MainAccountHead_BL mahbl = new MainAccountHead_BL();
            //using (SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {

                // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                string AccountCode = Convert.ToString(e.Values["AccountCode"]);
                string SubLedgerType = Convert.ToString(e.Values["SubLedgerType"]);
                bool b = AccountCode.ToLower().Contains("systm");
                // .............................Code Above Commented and Added by Sam on 06122016...................................... 

                if (b)
                { MainAccountGrid.JSProperties["cpDelete"] = "syscode"; }
                else
                {
                    //if (SubLedgerType == "Custom")
                    //{


                    //    NoofRowsAffected = mahbl.MainAccountGridDeleteCustomMain(AccountCode,);
                    //    if (NoofRowsAffected <= 0)
                    //    {
                    //        MainAccountGrid.JSProperties["cpDelete"] = "f";
                    //    }
                    //    else
                    //    {
                    //        MainAccountGrid.JSProperties["cpDelete"] = "s";
                    //        GetGridDetails();
                    //    }
                    //}
                    //else
                    //{

                    NoofRowsAffected = mahbl.MainAccountGridDeleteMainAccountHead(AccountCode);
                    if (NoofRowsAffected <= 0)
                    {
                        MainAccountGrid.JSProperties["cpDelete"] = "f";
                    }
                    else
                    {
                        MainAccountGrid.JSProperties["cpDelete"] = "s";
                        GetGridDetails();
                    }
                    //}
                }
            }
            e.Cancel = true;

        }

        protected void MainAccountGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            values.Clear();

            ASPxCheckBox chkBox1 = (ASPxCheckBox)MainAccountGrid.FindEditFormTemplateControl("FBTApplicable");
            values.Add("FBTApplicable", chkBox1.Checked);
            if (MainAccountGrid.IsNewRowEditing)
            {
                ASPxTextBox AccountNo = (ASPxTextBox)MainAccountGrid.FindEditFormTemplateControl("txtAccountNo");
                AccountNo.Focus();


            }
            else
            {
                //Debjyoti make enable false of shortname in edit mode 
                //ASPxTextBox shortName = (ASPxTextBox)MainAccountGrid.FindEditFormTemplateControl("txtAccountCode");
                //shortName.Enabled = false;


            }

            ASPxComboBox cmbPayemetType = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("cPaymenttype");
            cmbPayemetType.Value = "None";

            //DataTable DTCOM = oDBEngine.GetDataTable(" MASTER_USERCOMPANY ", "USERCOMPANY_COMPANYID", "USERCOMPANY_USERID='" + Session["userid"].ToString() + "'");
            DataTable DTCOM = oDBEngine.GetDataTable(" MASTER_USERCOMPANY ", "USERCOMPANY_COMPANYID", "USERCOMPANY_USERID='" + Convert.ToString(Session["userid"]) + "'");
            if (DTCOM.Rows.Count > 0)
            {

                ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");
                if (string.IsNullOrEmpty(EditCompanyCode))
                {
                    //select 'ALL' as cmp_internalId,'ALL' as cmp_name union all
                    //SqlCompany.SelectCommand = "select 'ALL' as cmp_internalId,'ALL' as cmp_name union all select cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(SELECT DISTINCT USERCOMPANY_COMPANYID FROM MASTER_USERCOMPANY  WHERE USERCOMPANY_USERID='" + Session["userid"].ToString() + "')";


                    //SqlCompany.SelectCommand = " select cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(SELECT DISTINCT USERCOMPANY_COMPANYID FROM MASTER_USERCOMPANY  WHERE USERCOMPANY_USERID='" + Convert.ToString(Session["userid"]) + "')";
                    // rev 1.0.1 discuss with jitendra and pijush all option again included, by sandip on 01.02.2017
                    SqlCompany.SelectCommand = "select '' as cmp_internalId,'ALL' as cmp_name union all select cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(SELECT DISTINCT USERCOMPANY_COMPANYID FROM MASTER_USERCOMPANY  WHERE USERCOMPANY_USERID='" + Convert.ToString(Session["userid"]) + "')";
                    comboCompanyName.DataBind();
                    //comboCompanyName.DataBind();
                    comboCompanyName.SelectedIndex = 0;
                }
                else
                {
                    //select 'ALL' as cmp_internalId,'ALL' as cmp_name union all
                    //SqlCompany.SelectCommand = "select cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(SELECT DISTINCT USERCOMPANY_COMPANYID FROM MASTER_USERCOMPANY  WHERE USERCOMPANY_USERID='" + Session["userid"].ToString() + "')";
                    SqlCompany.SelectCommand = "select '' as cmp_internalId,'ALL' as cmp_name union all select  cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(SELECT DISTINCT USERCOMPANY_COMPANYID FROM MASTER_USERCOMPANY  WHERE USERCOMPANY_USERID='" + Convert.ToString(Session["userid"]) + "')";

                    comboCompanyName.DataBind();
                    //if (comboCompanyName.Items.Count>0)
                    //{
                    //    foreach(var li in comboCompanyName.Items)
                    //    {
                    //        if(li)
                    //    }
                    //}
                    comboCompanyName.Items.FindByValue(EditCompanyCode.Trim()).Selected = true;
                }
                ASPxComboBox CmbBranch = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("CmbBranch");
                if (string.IsNullOrEmpty(EditBranchCode))
                {
                    //branchdtl.SelectCommand = "select branch_id,branch_description from tbl_master_branch order by branch_description";
                    branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";
                    CmbBranch.DataBind();
                    //comboCompanyName.DataBind();
                    //CmbBranch.SelectedIndex = 0;
                    CmbBranch.Value = "0";
                }
                else
                {
                    //branchdtl.SelectCommand = "select branch_id,branch_description from tbl_master_branch order by branch_description";
                    branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";
                    CmbBranch.DataBind();
                    CmbBranch.Items.FindByValue(EditBranchCode.Trim()).Selected = true;
                }


                //comboCompanyName.Items.Insert(0, new ListEditItem("ALL", DBNull.Value));
                //if (comboCompanyName.SelectedIndex <= 0)
                //{
                //    comboCompanyName.SelectedIndex = 0;
                //}
            }
            else
            {
                ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");
                if (string.IsNullOrEmpty(EditCompanyCode.Trim()))
                {
                    //select 'ALL' as cmp_internalId,'ALL' as cmp_name union all
                    SqlCompany.SelectCommand = "select '' as cmp_internalId,'ALL' as cmp_name union all select cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(select distinct exch_compId from tbl_master_companyExchange)";
                    comboCompanyName.DataBind();
                    comboCompanyName.SelectedIndex = 0;
                }
                else
                {
                    SqlCompany.SelectCommand = "select '' as cmp_internalId,'ALL' as cmp_name union all select cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(select distinct exch_compId from tbl_master_companyExchange)";
                    comboCompanyName.DataBind();
                    comboCompanyName.Items.FindByValue(EditCompanyCode.Trim()).Selected = true;
                }

                ASPxComboBox CmbBranch = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("CmbBranch");
                if (string.IsNullOrEmpty(EditBranchCode))
                {
                    //branchdtl.SelectCommand = "select branch_id,branch_description from tbl_master_branch order by branch_description";
                    branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";
                    CmbBranch.DataBind();
                    //comboCompanyName.DataBind();
                    //CmbBranch.SelectedIndex = 0;
                    CmbBranch.Value = "0";
                }
                else
                {
                    //branchdtl.SelectCommand = "select branch_id,branch_description from tbl_master_branch order by branch_description";
                    branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";

                    CmbBranch.DataBind();
                    CmbBranch.Items.FindByValue(EditBranchCode).Selected = true;
                }
                //comboCompanyName.Items.Insert(0, new ListEditItem("ALL", DBNull.Value));
            }
            // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring()...................................... 

        }
        protected void MainAccountGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            MainAccountGrid.SettingsText.PopupEditFormCaption = "Modify Main Account";
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            //string Keyvalue = e.EditingKeyValue.ToString();
            string Keyvalue = Convert.ToString(e.EditingKeyValue);
            MainAccountGrid.JSProperties["cpUDFKey"] = "MainAccountHead" + Keyvalue;

            int id = Convert.ToInt32(e.EditingKeyValue.ToString());
            Session["id"] = id;
            SetBranchRecordToSessionTable(Keyvalue);
            // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            DataTable dt = oDBEngine.GetDataTable("tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId  inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId", "[MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,'' as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid    LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment", " MainAccount_ReferenceID='" + Keyvalue + "'");
            if (dt.Rows.Count > 0)
            {
                ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");
                ASPxComboBox comboBranchName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("CmbBranch");


                // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

                comboCompanyName.Value = Convert.ToString(dt.Rows[0][17]);
                comboBranchName.SelectedItem = comboBranchName.Items.FindByValue(Convert.ToString(dt.Rows[0][18]));
                //comboBranchName.Value = Convert.ToString(dt.Rows[0][18]);
                //comboCompanyName.Value = dt.Rows[0][17].ToString();
                string aType = Convert.ToString(dt.Rows[0][7]);
                //string aType = dt.Rows[0][7].ToString();
                // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

                if (aType == "Asset")
                {
                    aType = "0";
                }
                else
                {
                    aType = "1";
                }
                ASPxComboBox ASPxComboBox2 = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("ASPxComboBox2");
                ASPxComboBox2.Value = Convert.ToString(dt.Rows[0][6]);
                // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                string bkType = Convert.ToString(dt.Rows[0][6]);
                //string bkType = dt.Rows[0][6].ToString();
                // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                if (bkType == "Bank")
                {
                    bkType = "0";
                    //  ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");
                    ASPxComboBox comboSegment = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboSegment");
                    if (comboCompanyName.SelectedItem != null)
                    {
                        // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                        string CompID = Convert.ToString(comboCompanyName.SelectedItem.Value);
                        //string CompID = comboCompanyName.SelectedItem.Value.ToString();

                        //SqlSegment.SelectCommand = "select exch_internalId,isnull(((select top 1 exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId),exch_membershipType) as Exchange from tbl_master_companyExchange where exch_compId='" + CompID + "'";
                        //comboSegment.TextField = "Exchange";
                        //comboSegment.ValueField = "exch_internalId";
                        //comboSegment.DataBind();
                        // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ...................
                    }
                }
                else if (bkType == "Cash")
                {
                    bkType = "1";
                }
                else if (bkType == "Fixed Asset")
                {
                    bkType = "2";
                }
                else
                {
                    bkType = "3";
                }
                hdnAssetType.Value = bkType;
                hdneditassettype.Value = bkType;
                Session["AssetType"] = bkType;
                // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                string bAType = Convert.ToString(dt.Rows[0][8]);
                //string bAType = dt.Rows[0][8].ToString();
                // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                if (bAType == "Clearing")
                {
                    bAType = "0";
                }
                else if (bAType == "")
                {
                    bAType = "A";
                }
                else
                {
                    bAType = "1";
                }
                // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                Session["Code"] = Convert.ToString(dt.Rows[0][1]);
                //Session["Code"] = dt.Rows[0][1].ToString();

                string Code = "";
                //if (dt.Rows[0][1].ToString().Length > 5)
                if (Convert.ToString(dt.Rows[0][1]).Length > 5)
                {
                    Code = Convert.ToString(dt.Rows[0][1]).Substring(0, 5);
                    //Code = dt.Rows[0][1].ToString().Substring(0, 5);

                }
                else
                {
                    Code = Convert.ToString(dt.Rows[0][1]);
                    //Code = dt.Rows[0][1].ToString();
                }
                string Segment = "";
                //if (Convert.ToString(dt.Rows[0][9])!= "")
                //    Segment = Convert.ToString(dt.Rows[0][9]);
                //if (dt.Rows[0][9].ToString() != "")
                //    Segment = dt.Rows[0][9].ToString();
                Error = aType + "~" + bkType + "~" + bAType + "~" + Code + "~" + Segment;
                EditCompanyCode = Convert.ToString(dt.Rows[0]["BankCompany"]).Trim();
                EditBranchCode = Convert.ToString(dt.Rows[0][18]);
                // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

            }

        }


        protected void MainAccountGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            try
            {

                if (e.RowType == GridViewRowType.Data)
                {
                    string Acccode1 = null;
                    string data = Convert.ToString(e.GetValue("SubLedgerType"));
                    string accountType = Convert.ToString(e.GetValue("AccountType"));
                    string BankCashType = Convert.ToString(e.GetValue("BankCashType"));
                    string AccountCode = Convert.ToString(e.GetValue("AccountCode"));
                    //string data = e.GetValue("SubLedgerType").ToString();
                    //string accountType = e.GetValue("AccountType").ToString();
                    //string BankCashType = e.GetValue("BankCashType").ToString();
                    //string AccountCode = e.GetValue("AccountCode").ToString();

                    if (AccountCode.Length > 6)
                    {
                        Acccode1 = AccountCode.Substring(0, 5);
                    }

                    if (data == "None")
                    {
                        e.Row.Cells[6].Style.Value = "cursor:default;text-align:center;";
                        //debjyoti
                        // e.Row.Cells[7].Attributes.Add("onclick", "Show('" + e.KeyValue.ToString() + "')");
                        // e.Row.Cells[7].Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                        //End here

                        //e.Row.Cells[5].Style.Value = "cursor:default;text-align:center;";
                        //e.Row.Cells[6].Attributes.Add("onclick", "Show('" + e.KeyValue.ToString() + "')");
                        //e.Row.Cells[6].Style.Value = "cursor:pointer;color: #000099;text-align:center;";


                        ASPxHyperLink hlinknew2 = (ASPxHyperLink)MainAccountGrid.FindRowTemplateControlByKey(e.KeyValue.ToString(), "hlink2");
                        ASPxHyperLink hlinknew3 = (ASPxHyperLink)MainAccountGrid.FindRowCellTemplateControlByKey(e.KeyValue.ToString(), null, "hlink2");
                        if (e.Row.Cells.Count > 8)
                        {
                            //Debjyoti -1 cell
                            e.Row.Cells[8].CssClass = "gridcellright";
                            e.Row.Cells[8].Style.Add("cursor", "pointer");
                            e.Row.Cells[8].Attributes.Add("onclick", "javascript:showhistory('" + Convert.ToString(e.KeyValue) + "^" + Convert.ToString(AccountCode) + "');");
                            //End here

                            //e.Row.Cells[8].CssClass = "gridcellright";                             
                            //e.Row.Cells[8].Style.Add("cursor", "pointer");                             
                            //e.Row.Cells[8].Attributes.Add("onclick", "javascript:showhistory('" + Convert.ToString(e.KeyValue) + "^" + Convert.ToString(AccountCode) + "');");

                        }

                    }
                    else
                    {
                        string kv = Convert.ToString(e.KeyValue);
                        e.Row.Cells[6].Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                        e.Row.Cells[6].Attributes.Add("onclick", "LoadSubledger('" + kv + "','" + data + "','" + accountType + "','" + AccountCode + "')");
                        //Debjyoti
                        // e.Row.Cells[7].Style.Value = "cursor:default;text-align:center;";
                        e.Row.Cells[8].Style.Value = "cursor:default;text-align:center;";
                        //End here
                        //e.Row.Cells[5].Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                        //e.Row.Cells[5].Attributes.Add("onclick", "LoadSubledger('" + kv + "','" + data + "','" + accountType + "','" + AccountCode + "')");
                        //e.Row.Cells[6].Style.Value = "cursor:default;text-align:center;"; 
                        //e.Row.Cells[8].Style.Value = "cursor:default;text-align:center;";

                    }

                    if (data == "None" && BankCashType == "Fixed Asset" && accountType == "Asset")
                    {
                        if (e.Row.Cells.Count > 7)
                        {
                            //Debjyoti -1 cell 
                            e.Row.Cells[7].CssClass = "gridcellright";
                            e.Row.Cells[7].Style.Add("cursor", "pointer");
                            e.Row.Cells[7].Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                            e.Row.Cells[7].Attributes.Add("onclick", "ShowAssetDetail('" + Convert.ToString(e.KeyValue) + "','" + AccountCode + "')");
                            //End here

                            //e.Row.Cells[7].CssClass = "gridcellright";
                            //e.Row.Cells[7].Style.Add("cursor", "pointer");                             
                            //e.Row.Cells[7].Style.Value = "cursor:pointer;color: #000099;text-align:center;";
                            //e.Row.Cells[7].Attributes.Add("onclick", "ShowAssetDetail('" + Convert.ToString(e.KeyValue) + "','" + AccountCode + "')");

                        }
                    }
                    else
                    {
                        //Debjyoti -1
                        e.Row.Cells[7].Style.Value = "cursor:default;text-align:center;";
                        //End here
                    }

                }
            }
            catch
            {
            }
            // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

        }
        protected void MainAccountGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {

            e.Properties["cpInsertError"] = Error;

        }

        protected void MainAccountGrid_OnRowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            int mainac = Convert.ToInt32(e.Keys["MainAccount_ReferenceID"]);
            MainAccountGrid.JSProperties["cpValidating"] = null;
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            if (!MainAccountGrid.IsNewRowEditing)
            {

                //foreach (GridViewColumn column in MainAccountGrid.Columns)
                //{
                //    GridViewDataColumn dataColumn = column as GridViewDataColumn;
                //    if (dataColumn == null) continue;
                //    if (e.NewValues[dataColumn.FieldName] == null)
                //    {
                //        e.Errors[dataColumn] = "Value can't be null.";
                //    }
                //}



                string strAccountCode = "";

                if (e.NewValues["AccountCode"] != null)
                {
                    //strAccountCode = e.NewValues["AccountCode"].ToString();
                    strAccountCode = Convert.ToString(e.NewValues["AccountCode"]);
                    //}
                    //if (Session["Code"] != null)
                    //{
                    //if (Convert.ToString(Session["Code"]) == strAccountCode)
                    //{
                    //    return;
                    //}
                    //}
                    //if (Session["Code"].ToString() == strAccountCode)
                    //{
                    //    return;
                    //}

                    //Disable Payment Type
                    DataTable existingTable = oDBEngine.GetDataTable("Trans_AccountsLedger", "AccountsLedger_id", "AccountsLedger_MainAccountID='" + strAccountCode + "' and AccountsLedger_MainAccountID Not Like 'SYSTM%'");

                    if (existingTable.Rows.Count > 0)
                    {
                        if (Convert.ToString(e.NewValues["MainAccount_PaymentType"]).Trim() != Convert.ToString(e.OldValues["MainAccount_PaymentType"]))
                        {
                            e.RowError = "Transaction exists.Cannot Modify the Payment Type";
                            MainAccountGrid.JSProperties["cpValidating"] = "Transaction exists.Cannot Modify the Payment Type";
                            return;
                        }

                    }
                    string strOldUnit = "";
                    if (Convert.ToString(e.NewValues["MainAccount_OldUnitLedger"]) == "No")
                    {
                        strOldUnit = "0";
                    }
                    else if (Convert.ToString(e.NewValues["MainAccount_OldUnitLedger"]) == "0")
                    {
                        strOldUnit = "0";
                    }
                    else
                    {
                        strOldUnit = "1";
                    }
                    if (strOldUnit != "0")
                    {
                        if (oDBEngine.getCount("Master_MainAccount", "MainAccount_OldUnitLedger=1 and MainAccount_ReferenceID<>" + mainac + "") > 0)
                        {
                            e.RowError = " ";
                            Error = "Old Unit Ledger Already Exists.";
                            MainAccountGrid.JSProperties["cpValidating"] = "Old Unit Ledger already exists.";
                            return;
                        }
                    }
                    if (Convert.ToString(e.NewValues["AccountType"]) != null)
                    {
                        if (Convert.ToString(e.NewValues["AccountType"]) == "Asset")
                        {
                            if (Convert.ToString(e.NewValues["BankCashType"]) != null)
                            {
                                if (Convert.ToString(e.NewValues["BankCashType"]) == "Bank")
                                {

                                    if (Convert.ToString(e.NewValues["MainAccount_PaymentType"]) == "None" && Convert.ToString(e.NewValues["BankAccountNo"]).Trim() == "")
                                    {
                                        e.RowError = "Please enter Bank Account Number";
                                        MainAccountGrid.JSProperties["cpValidating"] = "Please enter Bank Account Number";
                                        return;
                                        //e.Errors[BankAccountNo,"BankAccountNo"] = "Please enter Bank Account Number";
                                    }
                                    else
                                    {
                                        if (Convert.ToString(e.NewValues["BankAccountNo"]).Trim() != "")
                                        {
                                            if (oDBEngine.getCount("Master_MainAccount", "LTRIM(Rtrim(MainAccount_BankAcNumber))='"
                                        + Convert.ToString(e.NewValues["BankAccountNo"]).Trim() + "' and MainAccount_ReferenceID!=" + mainac + "") > 0)
                                            {
                                                e.RowError = " ";
                                                Error = "Bank Account Number Already Exists.";
                                                MainAccountGrid.JSProperties["cpValidating"] = "Bank Account Number already exists.";
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                e.RowError = "Please select asset type.";
                                //Error = "Bank Account Number Already Exists.";
                                MainAccountGrid.JSProperties["cpValidating"] = "Please select asset type.";
                                //return;
                            }
                        }
                    }
                    else
                    {
                        e.RowError = "Please select account type.";
                        Error = "Bank Account Number Already Exists.";
                        MainAccountGrid.JSProperties["cpValidating"] = "Please select asset type.";
                        return;
                    }
                }

                if (e.NewValues["SubLedgerType"] != null)
                {
                    string AccountType = Convert.ToString(e.NewValues["AccountType"]);
                    string AssetType = Convert.ToString(e.NewValues["BankCashType"]);
                    string SubLedgerType = Convert.ToString(e.NewValues["SubLedgerType"]);
                    if (AccountType == "Asset")
                    {

                        //string AccountType = e.NewValues["AccountType"].ToString();
                        //string AssetType = e.NewValues["BankCashType"].ToString();
                        //string SubLedgerType = e.NewValues["SubLedgerType"].ToString();
                        if (AccountType != "Asset" && AssetType != "Other")
                        {
                            if (SubLedgerType != "Custom" && SubLedgerType != "None")
                            {
                                //e.RowError = "SubLedger Type Can Only Custom or None";
                                Error = "SubLedger Type Can Only Custom or None";
                                e.RowError = "SubLedger Type Can Only Custom or None";
                                MainAccountGrid.JSProperties["cpValidating"] = "SubLedger Type Can Only Custom or None";
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('SubLedger Type Can Only Custom or None');", true);
                                //return;

                                //return;
                            }
                        }
                    }
                }
                //ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");

                //BusinessLogicLayer.DBEngine gg = new BusinessLogicLayer.DBEngine("");
                //SqlDataReader GetReader1 = gg.GetReader("SELECT  MainAccount_AccountCode as AccountCode FROM Master_MainAccount where MainAccount_AccountCode='" + strAccountCode + "'");
                //if (GetReader1.HasRows == true)
                //{

                //    //e.RowError = "This AccountCode Already Exists";
                //    Error = "This AccountCode Already Exists";
                //    return;
                //}
                //gg.CloseConnection();
            }
            else
            {

                //if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "AH"))
                //{
                //    e.RowError = "UDF is set as Mandatory. Please enter values.";
                //    MainAccountGrid.JSProperties["cpUDFVali"] = "UDFManddratory";
                //    return;

                //}
                string strOldUnit = "";
                if (Convert.ToString(e.NewValues["MainAccount_OldUnitLedger"]) == "No")
                {
                    strOldUnit = "0";
                }
                else if (Convert.ToString(e.NewValues["MainAccount_OldUnitLedger"]) == "0")
                {
                    strOldUnit = "0";
                }
                else
                {
                    strOldUnit = "1";
                }
                if (strOldUnit != "0")
                {
                    if (oDBEngine.getCount("Master_MainAccount", "MainAccount_OldUnitLedger=1 and MainAccount_ReferenceID<>" + mainac + "") > 0)
                    {
                        e.RowError = " ";
                        Error = "Old Unit Ledger Already Exists.";
                        MainAccountGrid.JSProperties["cpValidating"] = "Old Unit Ledger already exists.";
                        return;
                    }
                }
                if (Convert.ToString(e.NewValues["AccountType"]) != null)
                {
                    if (Convert.ToString(e.NewValues["AccountType"]) == "Asset")
                    {
                        if (Convert.ToString(e.NewValues["BankCashType"]) != null)
                        {
                            if (Convert.ToString(e.NewValues["BankCashType"]) == "Bank")
                            {
                                if (Convert.ToString(e.NewValues["BankAccountNo"]).Trim() == "" && Convert.ToString(e.NewValues["MainAccount_PaymentType"]) == "None")
                                {
                                    e.RowError = "Please enter Bank Account Number";
                                    MainAccountGrid.JSProperties["cpValidating"] = "Please enter Bank Account Number";

                                }
                                else if (Convert.ToString(e.NewValues["BankAccountNo"]) != null && Convert.ToString(e.NewValues["BankAccountNo"]).Trim() != "")
                                {

                                    if (oDBEngine.getCount("Master_MainAccount", "LTRIM(Rtrim(MainAccount_BankAcNumber))='"
                                        + Convert.ToString(e.NewValues["BankAccountNo"]).Trim() + "'") > 0)
                                    {
                                        e.RowError = " ";
                                        Error = "Bank Account Number Already Exists.";
                                        MainAccountGrid.JSProperties["cpValidating"] = "Bank Account Number already exists.";

                                    }
                                }
                            }
                        }
                        else
                        {
                            e.RowError = "Please select asset type.";
                            Error = "Bank Account Number Already Exists.";
                            MainAccountGrid.JSProperties["cpValidating"] = "Please select asset type.";
                            //return;
                        }
                    }
                }
                else
                {
                    e.RowError = "Please select account type.";
                    Error = "Bank Account Number Already Exists.";
                    MainAccountGrid.JSProperties["cpValidating"] = "Please select account type.";
                    //return;
                }


                if (e.NewValues["SubLedgerType"] != null)
                {
                    string AccountType = Convert.ToString(e.NewValues["AccountType"]);
                    if (AccountType == "Asset")
                    {
                        string AssetType = Convert.ToString(e.NewValues["BankCashType"]);
                        string SubLedgerType = Convert.ToString(e.NewValues["SubLedgerType"]);
                        //string AssetType = e.NewValues["BankCashType"].ToString();
                        //string SubLedgerType = e.NewValues["SubLedgerType"].ToString();
                        if (SubLedgerType == "")
                        {
                            // .............................Code Commented and Added by Sam on 06122016. .....................................
                            //e.RowError = "Sub Ledger Type Can Not Be Blank";
                            e.RowError = "Sub Ledger Type Can Not Be Blank";
                            // .............................Code Above Commented and Added by Sam on 06122016...................................... 
                            Error = "Sub Ledger Type Can Not Be Blank";
                            MainAccountGrid.JSProperties["cpValidating"] = "Sub Ledger Type Can Not Be Blank";
                            //return;
                        }
                        if (AccountType != "Asset" && AssetType != "Other")
                        {

                            if (SubLedgerType != "Custom" && SubLedgerType != "None")
                            {
                                // .............................Code Commented and Added by Sam on 06122016. .....................................
                                //e.RowError = "SubLedger Type Can Only Custom or None";
                                e.RowError = "SubLedger Type Can Only Custom or None";
                                // .............................Code Above Commented and Added by Sam on 06122016...................................... 
                                Error = "SubLedger Type Can Only Custom or None";
                                MainAccountGrid.JSProperties["cpValidating"] = "SubLedger Type Can Only Custom or None";
                                //return;
                            }
                        }
                    }
                }

            }
        }


        protected void MainAccountGrid_OnHtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

            if (e.DataColumn.FieldName == "AccountGroup")
            {
                if (Convert.ToString(e.CellValue).ToLower() != "")
                {
                    AccountGp = Convert.ToString(e.CellValue);
                }
                //if (e.CellValue.ToString().ToLower() != "")
                //{
                //    AccountGp = e.CellValue.ToString();
                //}

            }
            // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 


        }
        protected void MainAccountGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            if (e.Parameters == "T")
            {
                //MainAccount.SelectCommand = "i";
                //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany  from [Master_MainAccount]   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%'  AND (MAINACCOUNT_BANKCOMPANY='" + Session["LastCompany"].ToString() + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,branch_description  branchname  from [Master_MainAccount] join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%'  AND (MAINACCOUNT_BANKCOMPANY='" + Convert.ToString(Session["LastCompany"]) + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable,'' as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%' AND(MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";
                MainAccountGrid.DataBind();
            }
            if (e.Parameters == "F")
            {
                //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany  from [Master_MainAccount]   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  WHERE (MAINACCOUNT_BANKCOMPANY='" + Session["LastCompany"].ToString() + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany ,branch_id,branch_description  branchname from [Master_MainAccount] join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid  LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  WHERE (MAINACCOUNT_BANKCOMPANY='" + Convert.ToString(Session["LastCompany"]) + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable,'' as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where (MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";
                MainAccountGrid.DataBind();
            }
            if (e.Parameters.Split('~')[0] == "s")
            {
                if (e.Parameters.Split('~')[1] == "T")
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany  from [Master_MainAccount]   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%'  AND (MAINACCOUNT_BANKCOMPANY='" + Session["LastCompany"].ToString() + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany ,branch_id,branch_description  branchname from [Master_MainAccount] join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%'  AND (MAINACCOUNT_BANKCOMPANY='" + Convert.ToString(Session["LastCompany"]) + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                    MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable,'' as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%' AND(MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";
                else
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany  from [Master_MainAccount]   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  WHERE (MAINACCOUNT_BANKCOMPANY='" + Session["LastCompany"].ToString() + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany ,branch_id,branch_description  branchname from [Master_MainAccount] join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid  LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  WHERE (MAINACCOUNT_BANKCOMPANY='" + Convert.ToString(Session["LastCompany"]) + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                    MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable,'' as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where (MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";
                MainAccountGrid.DataBind();
                MainAccountGrid.Settings.ShowFilterRow = true;
            }

            if (e.Parameters.Split('~')[0] == "All")
            {
                if (e.Parameters.Split('~')[1] == "T")
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany  from [Master_MainAccount]   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%' AND (MAINACCOUNT_BANKCOMPANY='" + Session["LastCompany"].ToString() + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')   order by MainAccount_ReferenceID  desc";
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany ,branch_id,branch_description  branchname from [Master_MainAccount] join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid  LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%' AND (MAINACCOUNT_BANKCOMPANY='" + Convert.ToString(Session["LastCompany"]) + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')   order by MainAccount_ReferenceID  desc";
                    MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable,'' as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where MainAccount_AccountCode not like 'SYS%' AND(MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";
                else
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany  from [Master_MainAccount]   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  WHERE (MAINACCOUNT_BANKCOMPANY='" + Session["LastCompany"].ToString() + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                    //MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,[MainAccount_BankAccountType] as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany ,branch_id,branch_description  branchname from [Master_MainAccount] join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid  LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  WHERE (MAINACCOUNT_BANKCOMPANY='" + Session["LastCompany"].ToString() + "'  OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 OR  MAINACCOUNT_BANKCOMPANY='')  order by MainAccount_ReferenceID  desc";
                    MainAccount.SelectCommand = "SELECT  [MainAccount_ReferenceID],[MainAccount_AccountCode] as AccountCode ,[MainAccount_AccountGroup] as AccountGroup ,[MainAccount_Name] as AccountName,[MainAccount_BankAcNumber] as BankAccountNo,'' as openingBalance, [MainAccount_SubLedgerType] as SubLedgerType ,[MainAccount_BankCashType] as BankCashType ,[MainAccount_AccountType] as AccountType,MainAccount_OldUnitLedger,MainAccount_ReverseApplicable,'' as BankAccountType, [MainAccount_ExchangeSegment] as ExchengSegment1,ExchengSegment,MainAccount_PaymentType,(select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' from master_tdstcs where ltrim(rtrim(tdstcs_code))=ltrim(rtrim(Master_MainAccount.MainAccount_TDSRate))) as TDSApplicable,isnull([MainAccount_IsFBT],0) as FBTApplicable, [MainAccount_TDSRate] as TDSRate,[MainAccount_FBTRate] as FBTRate ,[MainAccount_RateOfInterest] as RateOfIntrest,mainaccount_Depreciation as Depreciation,[MainAccount_BankCompany] as BankCompany,branch_id,Isnull(branch_description,'ALL')  branchname  from [Master_MainAccount] left join tbl_master_branch  on  tbl_master_branch.branch_id= [Master_MainAccount].MainAccount_branchid   LEFT OUTER JOIN (select exch_internalId,exch_compId,exch_exchId,(cmp_name + '--' + exh_shortname +'--' + exch_segmentId)  as ExchengSegment from tbl_master_companyExchange inner join tbl_master_company on tbl_master_company.cmp_internalid=tbl_master_companyExchange.exch_compId inner join tbl_master_exchange on tbl_master_exchange.exh_cntid=tbl_master_companyExchange.exch_exchId ) AS a ON  [Master_MainAccount].[MainAccount_ExchangeSegment]= a.exch_internalId  where (MAINACCOUNT_BANKCOMPANY in (" + Convert.ToString(HttpContext.Current.Session["CompanyHierarchy"]) + ")" + " OR MAINACCOUNT_BANKCOMPANY IS NULL or MAINACCOUNT_BANKCOMPANY='ALL' OR LEN(MAINACCOUNT_BANKCOMPANY)=0 or   MAINACCOUNT_BANKCOMPANY='') order by MainAccount_ReferenceID  desc";
                MainAccountGrid.DataBind();
                MainAccountGrid.FilterExpression = string.Empty;
            }
            // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void MainAccountGrid_OnRowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
            MainAccountHead_BL mahbl = new MainAccountHead_BL();

            MainAccountGrid.JSProperties["cpUpdate"] = null;
            //e.NewValues["TDSApplicable"] = values["TDSApplicable"];
            e.NewValues["FBTApplicable"] = values["FBTApplicable"];
            //using (SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))
            //{
            //    lcon.Open();
            //    using (SqlCommand lcmd = new SqlCommand("UpdateMainAccountHead", lcon))
            //    {
            // lcmd.CommandType = CommandType.StoredProcedure;
            // lcmd.Parameters.Add("@MainAccount_ReferenceID", SqlDbType.Int).Value = e.Keys[0].ToString();
            //if (e.NewValues["AccountType"] != null)
            //{
            //   lcmd.Parameters.Add("@AccountType", SqlDbType.VarChar).Value = e.NewValues["AccountType"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@AccountType", SqlDbType.VarChar).Value = "";
            //--new added

            //  ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");

            //if (comboCompanyName.SelectedItem != null)
            //    lcmd.Parameters.Add("@BankCompany", SqlDbType.VarChar).Value = comboCompanyName.SelectedItem.Value;
            //else
            //    lcmd.Parameters.Add("@BankCompany", SqlDbType.VarChar).Value = DBNull.Value;

            //if (e.NewValues["BankCashType"] != null)
            //{
            //    lcmd.Parameters.Add("@BankCashType", SqlDbType.VarChar).Value = e.NewValues["BankCashType"].ToString();
            //}
            //else
            //{
            //    lcmd.Parameters.Add("@BankCashType", SqlDbType.VarChar).Value = "";
            //}
            //if (e.NewValues["BankAccountType"] != null)
            //    lcmd.Parameters.Add("@BankAccountType", SqlDbType.VarChar).Value = e.NewValues["BankAccountType"].ToString();
            //else
            //    lcmd.Parameters.Add("@BankAccountType", SqlDbType.VarChar).Value = "";
            // ASPxRadioButtonList redSegment = (ASPxRadioButtonList)MainAccountGrid.FindEditFormTemplateControl("rbSegment");
            //if (redSegment.SelectedItem.Value.ToString() != "A")
            //{
            //    if (hdSegment.Value != null)
            //    {
            //        lcmd.Parameters.Add("@ExchengSegment", SqlDbType.VarChar).Value = hdSegment.Value;
            //    }
            //    else
            //        lcmd.Parameters.Add("@ExchengSegment", SqlDbType.VarChar).Value = "0";
            //}
            //else
            //    lcmd.Parameters.Add("@ExchengSegment", SqlDbType.VarChar).Value = "0";

            //  lcmd.Parameters.Add("@AccountCode", SqlDbType.VarChar).Value = e.NewValues["AccountCode"].ToString();

            //ASPxComboBox combAccountGroup = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("combAccountGroup");
            //if (combAccountGroup.SelectedItem != null)
            //{
            //    e.NewValues["AccountGroup"] = combAccountGroup.SelectedItem.Value.ToString().Trim();
            //}
            //else
            //    e.NewValues["AccountGroup"] = "";


            //if (aaaa.SelectedItem.Value != null)
            //{
            //    e.NewValues["AccountGroup"] = aaaa.SelectedItem.Value.ToString().Trim();
            //}
            //else
            //    e.NewValues["AccountGroup"] = "";

            // lcmd.Parameters.Add("@AccountGroup", SqlDbType.VarChar).Value = e.NewValues["AccountGroup"].ToString();
            // lcmd.Parameters.Add("@AccountName", SqlDbType.VarChar).Value = e.NewValues["AccountName"].ToString();
            // lcmd.Parameters.Add("@BankAccountNo", SqlDbType.VarChar).Value = e.NewValues["BankAccountNo"].ToString();
            //if (e.NewValues["SubLedgerType"] != null)
            //    lcmd.Parameters.Add("@SubLedgerType", SqlDbType.VarChar).Value = e.NewValues["SubLedgerType"].ToString();
            //else
            //    lcmd.Parameters.Add("@SubLedgerType", SqlDbType.VarChar).Value = "None";

            //lcmd.Parameters.Add("@TDSApplicable", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["TDSApplicable"]);
            //if (e.NewValues["TDSRate"] != null)
            //    lcmd.Parameters.Add("@TDSRate", SqlDbType.VarChar).Value = e.NewValues["TDSRate"].ToString();
            //else
            //    lcmd.Parameters.Add("@TDSRate", SqlDbType.VarChar).Value = "";

            //lcmd.Parameters.Add("@FBTApplicable", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["FBTApplicable"]);
            //if (e.NewValues["FBTRate"] != null)
            //    lcmd.Parameters.Add("@FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["FBTRate"].ToString());
            //else
            //    lcmd.Parameters.Add("@FBTRate", SqlDbType.Decimal).Value = 0.00;

            //if (e.NewValues["RateOfIntrest"] != null)
            //    lcmd.Parameters.Add("@RateOfIntrest", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString());
            //else
            //    lcmd.Parameters.Add("@RateOfIntrest", SqlDbType.Decimal).Value = 0.00;

            //if (e.NewValues["Depreciation"] != null)
            //    lcmd.Parameters.Add("@Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["Depreciation"].ToString());
            //else
            //    lcmd.Parameters.Add("@Depreciation", SqlDbType.Decimal).Value = 0.00;

            // lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //        int NoOfRowEffected2 = lcmd.ExecuteNonQuery();
            //    }
            //}
            string ExchengSegment;
            string AccountGroup;
            string BankCompany;
            string branch;
            ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");

            ASPxRadioButtonList redSegment = (ASPxRadioButtonList)MainAccountGrid.FindEditFormTemplateControl("rbSegment");


            ASPxComboBox paymentType = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("cPaymenttype");

            string paymentTypevalue = Convert.ToString(paymentType.Value);


            ASPxComboBox cmbOldUnitLedger = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("cmbOldUnitLedger");
            string strEditcmbOldUnitLedger = Convert.ToString(cmbOldUnitLedger.Value);

            ASPxComboBox cmbReverseApplicable = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("cmbReverseApplicable");
            string strEditcmbReverseApplicable = Convert.ToString(cmbReverseApplicable.Value);

            // .............................Code Commented and Added by Sam on 15122016. to get the segment from login session instead of from page segment ..................................... 
            if (HttpContext.Current.Session["userlastsegment"] != null)
            {
                ExchengSegment = Convert.ToString(Session["userlastsegment"]);
            }
            else
            {
                ExchengSegment = "0";
            }
            //if (redSegment.SelectedItem.Value.ToString() != "A")
            //{
            //    if (hdSegment.Value != null)
            //    {
            //        ExchengSegment = hdSegment.Value;
            //    }
            //    else
            //        ExchengSegment = "0";
            //}
            //else
            //    ExchengSegment = "0";
            // .............................Code Above Commented and Added by Sam on 15122016...................................... 
            ASPxComboBox combAccountGroup = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("combAccountGroup");
            if (combAccountGroup.SelectedItem != null)
            {
                AccountGroup = combAccountGroup.SelectedItem.Value.ToString().Trim();
            }
            else
                AccountGroup = "";
            e.NewValues["AccountGroup"] = AccountGroup;

            ASPxComboBox CmbBranch = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("CmbBranch");
            if (CmbBranch.Value != null)
                branch = Convert.ToString(CmbBranch.Value);
            //BankCompany = comboCompanyName.SelectedItem.Value.ToString();
            else
                branch = Convert.ToString(DBNull.Value);

            // .............................Code Commented and Added by Sam on 19122016. .....................................
            if (comboCompanyName.SelectedItem != null)
                BankCompany = Convert.ToString(comboCompanyName.Value);
            //BankCompany = comboCompanyName.SelectedItem.Value.ToString();
            else
                BankCompany = Convert.ToString(DBNull.Value);

            string BranchList = GetBranchList();


            int NoOfRowEffected = 0;
            if (e.NewValues["AccountType"] != null)
            {
                if (Convert.ToString(e.NewValues["AccountType"]) == "Asset")
                {
                    #region Asset
                    if (e.NewValues["BankCashType"] != null)
                    {
                        if (Convert.ToString(e.NewValues["BankCashType"]) == "Bank")
                        {
                            NoOfRowEffected = mahbl.MainAccountGridUpdating(Convert.ToString(e.Keys[0]), e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                            e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                            ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                           e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", e.NewValues["BankAccountNo"] != null ? e.NewValues["BankAccountNo"].ToString() : "",
                            "None", "", Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                             0, 0, Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, BranchList, strEditcmbOldUnitLedger, strEditcmbReverseApplicable);
                        }
                        else if (Convert.ToString(e.NewValues["BankCashType"]) == "Cash")
                        {
                            NoOfRowEffected = mahbl.MainAccountGridUpdating(Convert.ToString(e.Keys[0]), e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                            e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                            ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                            e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                             "None", "", Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                             0, 0, Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, BranchList, strEditcmbOldUnitLedger, strEditcmbReverseApplicable);
                        }
                        else if (Convert.ToString(e.NewValues["BankCashType"]) == "Fixed Asset")
                        {
                            NoOfRowEffected = mahbl.MainAccountGridUpdating(Convert.ToString(e.Keys[0]), e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                            e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                            ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                            e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                            e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", "",
                            Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                             0, e.NewValues["Depreciation"] != null ? Convert.ToDecimal(e.NewValues["Depreciation"].ToString()) : 0,
                             Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, BranchList, strEditcmbOldUnitLedger, strEditcmbReverseApplicable);
                        }
                        else if (Convert.ToString(e.NewValues["BankCashType"]) == "Other")
                        {
                            NoOfRowEffected = mahbl.MainAccountGridUpdating(Convert.ToString(e.Keys[0]), e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                            e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                            ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                            e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                            e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", e.NewValues["TDSRate"] != null ? Convert.ToString(e.NewValues["TDSRate"]) : "",
                            Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                            e.NewValues["RateOfIntrest"] != null ? Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString()) : 0, 0,
                             Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, BranchList, strEditcmbOldUnitLedger, strEditcmbReverseApplicable);
                        }


                    }
                    #endregion Asset

                }
                else if (Convert.ToString(e.NewValues["AccountType"]) != "Asset")
                {
                    NoOfRowEffected = mahbl.MainAccountGridUpdating(Convert.ToString(e.Keys[0]), e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                       "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                       ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                       e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                       e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", e.NewValues["TDSRate"] != null ? Convert.ToString(e.NewValues["TDSRate"]) : "",
                       Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                       e.NewValues["RateOfIntrest"] != null ? Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString()) : 0, 0,
                        Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, BranchList, strEditcmbOldUnitLedger, strEditcmbReverseApplicable);
                }
            }
            if (NoOfRowEffected > 0)
            {
                MainAccountGrid.JSProperties["cpinsert"] = "Saved successfully";
            }
            else
            {
                MainAccountGrid.JSProperties["cpinsert"] = "Saved unsuccessful.";
            }



            // .............................Code Above Commented and Added by Sam on 19122016...................................... 


            //int NoOfRowEffected = mahbl.MainAccountGridUpdating(Convert.ToString(e.Keys[0]),
            //    (Convert.ToString(e.NewValues["AccountType"]) != null ? Convert.ToString(e.NewValues["AccountType"]) : ""),
            //    comboCompanyName.SelectedItem != null ? Convert.ToString(comboCompanyName.SelectedItem.Value) : Convert.ToString(DBNull.Value),
            //    e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "",
            //    e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
            //    ExchengSegment, Convert.ToString(e.NewValues["AccountCode"]), AccountGroup, Convert.ToString(e.NewValues["AccountName"]), Convert.ToString(e.NewValues["BankAccountNo"]),
            //    e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", e.NewValues["TDSRate"] != null ? Convert.ToString(e.NewValues["TDSRate"]) : "",
            //    Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,

            //    e.NewValues["RateOfIntrest"] != null ? Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString()) : 0,
            //    e.NewValues["Depreciation"] != null ? Convert.ToDecimal(e.NewValues["Depreciation"].ToString()) : 0,
            //    Convert.ToString(HttpContext.Current.Session["userid"]));





            if (NoOfRowEffected > 0)
            {
                DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                if (udfTable != null)
                {
                    Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("AH", "MainAccountHead" + Convert.ToString(NoOfRowEffected), udfTable, Convert.ToString(Session["userid"]));
                }
                MainAccountGrid.JSProperties["cpUpdate"] = "Saved successfully";
            }
            else
            {
                MainAccountGrid.JSProperties["cpUpdate"] = "Saved unsuccessful.";
            }



            //int NoOfRowEffected = mahbl.MainAccountGridUpdating(e.Keys[0].ToString(),
            //    (e.NewValues["AccountType"].ToString() != null ? e.NewValues["AccountType"].ToString() : ""),
            //    comboCompanyName.SelectedItem != null ? comboCompanyName.SelectedItem.Value.ToString() : DBNull.Value.ToString(),
            //    e.NewValues["BankCashType"] != null ? e.NewValues["BankCashType"].ToString() : "",
            //    e.NewValues["BankAccountType"] != null ? e.NewValues["BankAccountType"].ToString() : "Own",
            //    ExchengSegment, e.NewValues["AccountCode"].ToString(), AccountGroup, e.NewValues["AccountName"].ToString(), e.NewValues["BankAccountNo"].ToString(),
            //    e.NewValues["SubLedgerType"] != null ? e.NewValues["SubLedgerType"].ToString() : "None", e.NewValues["TDSRate"] != null ? e.NewValues["TDSRate"].ToString() : "",
            //    Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,

            //    e.NewValues["RateOfIntrest"] != null ? Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString()) : 0,
            //    e.NewValues["Depreciation"] != null ? Convert.ToDecimal(e.NewValues["Depreciation"].ToString()) : 0,
            //    HttpContext.Current.Session["userid"].ToString());

            e.Cancel = true;
            MainAccountGrid.CancelEdit();
            // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

        }
        protected void MainAccountGrid_OnRowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

            if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "AH"))
            {
                //e.RowError = "UDF is set as Mandatory. Please enter values.";
                //MainAccountGrid.JSProperties["cpUDFVali"] = "UDFManddratory";
                MainAccountGrid.JSProperties["cpUDF"] = "Validation Required";
                //  MainAccountGrid.CancelEdit();
                e.Cancel = true;
                // MainAccountGrid.CancelEdit();
                return;

            }
            MainAccountGrid.JSProperties["cpinsert"] = null;
            MainAccountHead_BL mahbl = new MainAccountHead_BL();
            //e.NewValues["TDSApplicable"] = values["TDSApplicable"];
            e.NewValues["FBTApplicable"] = values["FBTApplicable"];
            #region
            //using (SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))
            //{
            //    lcon.Open();
            //    using (SqlCommand lcmd = new SqlCommand("MainAccountInsert", lcon))
            //    {
            //        lcmd.CommandType = CommandType.StoredProcedure;
            //if (e.NewValues["AccountType"] != null)
            //{
            //    lcmd.Parameters.Add("@AccountType", SqlDbType.VarChar).Value = Convert.ToString(e.NewValues["AccountType"]);
            //}
            //else
            //    lcmd.Parameters.Add("@AccountType", SqlDbType.VarChar).Value = "";

            //if(e.NewValues["BankCompany"]!= null)
            //     lcmd.Parameters.Add("@BankCompany", SqlDbType.VarChar).Value = e.NewValues["BankCompany"].ToString();
            //else
            //     lcmd.Parameters.Add("@BankCompany", SqlDbType.VarChar).Value = DBNull.Value;


            //ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");

            //if (comboCompanyName.SelectedItem != null)
            //    lcmd.Parameters.Add("@BankCompany", SqlDbType.VarChar).Value = comboCompanyName.SelectedItem.Value;
            //else
            //    lcmd.Parameters.Add("@BankCompany", SqlDbType.VarChar).Value = DBNull.Value;



            //if (e.NewValues["BankCashType"] != null)
            //{

            //    lcmd.Parameters.Add("@BankCashType", SqlDbType.VarChar).Value = e.NewValues["BankCashType"].ToString();
            //}
            //else
            //{
            //    lcmd.Parameters.Add("@BankCashType", SqlDbType.VarChar).Value = "";

            //}
            //if (e.NewValues["BankAccountType"] != null)
            //{
            //    lcmd.Parameters.Add("@BankAccountType", SqlDbType.VarChar).Value = e.NewValues["BankAccountType"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@BankAccountType", SqlDbType.VarChar).Value = "";

            //if (e.NewValues["AccountCode"] != null)
            //{
            //    lcmd.Parameters.Add("@AccountCode", SqlDbType.VarChar).Value = e.NewValues["AccountCode"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@AccountCode", SqlDbType.VarChar).Value = "";

            //ASPxComboBox AccountG = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("combAccountGroup");
            //if (AccountG.Value != null)
            //{
            //    e.NewValues["AccountGroup"] = AccountG.Value.ToString();
            //}
            //if (e.NewValues["AccountGroup"] != null)
            //{
            //    lcmd.Parameters.Add("@AccountGroup", SqlDbType.VarChar).Value = e.NewValues["AccountGroup"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@AccountGroup", SqlDbType.VarChar).Value = "";

            //if (e.NewValues["AccountName"] != null)
            //{
            //    lcmd.Parameters.Add("@AccountName", SqlDbType.VarChar).Value = e.NewValues["AccountName"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@AccountName", SqlDbType.VarChar).Value = "";
            //if (e.NewValues["BankAccountNo"] != null)
            //{

            //    lcmd.Parameters.Add("@BankAccountNo", SqlDbType.VarChar).Value = e.NewValues["BankAccountNo"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@BankAccountNo", SqlDbType.VarChar).Value = "";


            //ASPxComboBox Subledertype = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("CmbSubLedgerType");

            //e.NewValues["SubLedgerType"] = Subledertype.SelectedItem.Value;


            //if (e.NewValues["SubLedgerType"] != null)
            //{
            //    lcmd.Parameters.Add("@SubLedgerType", SqlDbType.VarChar).Value = e.NewValues["SubLedgerType"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@SubLedgerType", SqlDbType.VarChar).Value = "None";

            //lcmd.Parameters.Add("@TDSApplicable", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["TDSApplicable"]);
            // lcmd.Parameters.Add("@FBTApplicable", SqlDbType.Bit).Value = Convert.ToBoolean(e.NewValues["FBTApplicable"]);
            //if (e.NewValues["TDSRate"] != null)
            //{
            //    lcmd.Parameters.Add("@TDSRate", SqlDbType.VarChar).Value = e.NewValues["TDSRate"].ToString();
            //}
            //else
            //    lcmd.Parameters.Add("@TDSRate", SqlDbType.VarChar).Value = "";


            //if (e.NewValues["FBTRate"] != null)
            //{
            //    lcmd.Parameters.Add("@FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["FBTRate"].ToString());
            //}
            //else
            //    lcmd.Parameters.Add("@FBTRate", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");

            //if (e.NewValues["RateOfIntrest"] != null)
            //{
            //    lcmd.Parameters.Add("@RateOfIntrest", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString());
            //}
            //else
            //    lcmd.Parameters.Add("@RateOfIntrest", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            //if (e.NewValues["Depreciation"] != null)
            //{
            //    lcmd.Parameters.Add("@Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal(e.NewValues["Depreciation"].ToString());
            //}
            //else
            //    lcmd.Parameters.Add("@Depreciation", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");

            //ASPxRadioButtonList redSegment = (ASPxRadioButtonList)MainAccountGrid.FindEditFormTemplateControl("rbSegment");
            //if (redSegment.SelectedItem.Value.ToString() != "A")
            //{
            //    //TextBox specific = (TextBox)MainAccountGrid.FindEditFormTemplateControl("txtSpefificExchange");
            //    //HiddenField Specific = (HiddenField)MainAccountGrid.FindEditFormTemplateControl("txtSpefificExchange_hidden");
            //    ASPxComboBox comboSegment = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboSegment");
            //    if (comboSegment.Value != null)
            //    {
            //        lcmd.Parameters.Add("@ExchengSegment", SqlDbType.VarChar).Value = comboSegment.Value.ToString(); ;
            //    }
            //    else
            //        lcmd.Parameters.Add("@ExchengSegment", SqlDbType.VarChar).Value = "0";
            //}
            //else
            //    lcmd.Parameters.Add("@ExchengSegment", SqlDbType.VarChar).Value = "0";
            // lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //try
            //{
            //    int NoOfRowEffected = lcmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    this.Error = ex.Message;
            //    e.Cancel = true;

            //}
            //    }
            //}
            #endregion
            try
            {
                string BankCompany;
                string branch;
                string ExchengSegment;
                // .............................Code Commented and Added by Sam on 15122016. to get the segment from login session instead of from page segment ..................................... 
                if (HttpContext.Current.Session["userlastsegment"] != null)
                {
                    ExchengSegment = Convert.ToString(Session["userlastsegment"]);
                }
                else
                {
                    ExchengSegment = "0";
                }
                // .............................Code Above Commented and Added by Sam on 15122016...................................... 
                string AccountGroup;
                ASPxComboBox comboCompanyName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboCompanyName");

                ASPxComboBox paymentType = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("cPaymenttype");

                string paymentTypevalue = Convert.ToString(paymentType.Value);
                //...................Old Unit Ledger.............
                ASPxComboBox OldUnitLedger = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("cmbOldUnitLedger");
                string strOldUnit = "";
                if (Convert.ToString(OldUnitLedger.Value) == "No")
                {
                    strOldUnit = "0";
                }
                else if (Convert.ToString(OldUnitLedger.Value) == "0")
                {
                    strOldUnit = "0";
                }
                else
                {
                    strOldUnit = "1";
                }
                string strOldUnitLedger = Convert.ToString(strOldUnit);
                //...................end Old Unit Ledger.............

                //...................Reverse Applicable.............
                ASPxComboBox ReverseApplicable = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("cmbReverseApplicable");
                string strReverseAppilcble = "";
                if (Convert.ToString(ReverseApplicable.Value) == "No")
                {
                    strReverseAppilcble = "0";
                }
                else if (Convert.ToString(ReverseApplicable.Value) == "0")
                {
                    strReverseAppilcble = "0";
                }
                else
                {
                    strReverseAppilcble = "1";
                }
                //string strReverseAppilcble = Convert.ToString(strReverseAppilcble);
                //...................end Old Unit Ledger.............


                if (comboCompanyName.SelectedItem != null)
                {
                    BankCompany = Convert.ToString(comboCompanyName.SelectedItem.Value);
                }
                //BankCompany = comboCompanyName.SelectedItem.Value.ToString();
                else
                {
                    BankCompany = Convert.ToString(DBNull.Value);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Please Select a Branch');", true);
                    return;

                }

                String LedgerBranchList = GetBranchList();

                ASPxComboBox CmbBranch = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("CmbBranch");
                if (CmbBranch.SelectedItem != null)
                {
                    branch = Convert.ToString(CmbBranch.SelectedItem.Value);
                }
                //BankCompany = comboCompanyName.SelectedItem.Value.ToString();
                else
                {
                    branch = Convert.ToString(DBNull.Value);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Please Select a Branch');", true);
                    return;

                }
                //BankCompany = DBNull.Value.ToString();
                // .............................Code Commented and Added by Sam on 15122016. to get the segment from login session instead of from page segment .............
                //ASPxRadioButtonList redSegment = (ASPxRadioButtonList)MainAccountGrid.FindEditFormTemplateControl("rbSegment");
                //if (Convert.ToString(redSegment.SelectedItem.Value) != "A")
                ////if (redSegment.SelectedItem.Value.ToString() != "A")
                //{
                //    ASPxComboBox comboSegment = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("comboSegment");
                //    if (comboSegment.Value != null)
                //    {
                //        //ExchengSegment = comboSegment.Value.ToString(); 
                //        ExchengSegment = Convert.ToString(comboSegment.Value); 
                //    }
                //    else
                //        ExchengSegment = "0";
                //}
                //else
                //    ExchengSegment = "0";
                // .............................Code Above Commented and Added by Sam on 15122016...................................... 
                ASPxComboBox AccountG = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("combAccountGroup");
                if (AccountG.Value != null)
                {
                    //e.NewValues["AccountGroup"] = AccountG.Value.ToString();
                    e.NewValues["AccountGroup"] = Convert.ToString(AccountG.Value);
                }
                if (e.NewValues["AccountGroup"] != null)
                {
                    //AccountGroup = e.NewValues["AccountGroup"].ToString();
                    AccountGroup = Convert.ToString(e.NewValues["AccountGroup"]);
                }
                else
                    AccountGroup = "";

                // .............................Code Commented and Added by Sam on 19122016. to insert filter data instead of all data ..................................... 
                int NoOfRowEffected = 0;
                if (e.NewValues["AccountType"] != null)
                {
                    if (Convert.ToString(e.NewValues["AccountType"]) == "Asset")
                    {
                        #region Asset
                        if (e.NewValues["BankCashType"] != null)
                        {
                            if (Convert.ToString(e.NewValues["BankCashType"]) == "Bank")
                            {
                                NoOfRowEffected = mahbl.MainAccountGridInserting(e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                                e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                                ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                               e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", e.NewValues["BankAccountNo"] != null ? e.NewValues["BankAccountNo"].ToString() : "",
                                "None", "", Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                                 0, 0, Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, LedgerBranchList, strOldUnitLedger, strReverseAppilcble);
                            }
                            else if (Convert.ToString(e.NewValues["BankCashType"]) == "Cash")
                            {
                                NoOfRowEffected = mahbl.MainAccountGridInserting(e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                                e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                                ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                                e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                                 "None", "", Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                                 0, 0, Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, LedgerBranchList, strOldUnitLedger, strReverseAppilcble);
                            }
                            else if (Convert.ToString(e.NewValues["BankCashType"]) == "Fixed Asset")
                            {
                                NoOfRowEffected = mahbl.MainAccountGridInserting(e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                                e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                                ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                                e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                                e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", "",
                                Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                                 0, e.NewValues["Depreciation"] != null ? Convert.ToDecimal(e.NewValues["Depreciation"].ToString()) : 0,
                                 Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, LedgerBranchList, strOldUnitLedger, strReverseAppilcble);
                            }
                            else if (Convert.ToString(e.NewValues["BankCashType"]) == "Other")
                            {
                                NoOfRowEffected = mahbl.MainAccountGridInserting(e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                                e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                                ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                                e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                                e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", e.NewValues["TDSRate"] != null ? Convert.ToString(e.NewValues["TDSRate"]) : "",
                                Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                                e.NewValues["RateOfIntrest"] != null ? Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString()) : 0, 0,
                                 Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, LedgerBranchList, strOldUnitLedger, strReverseAppilcble);
                            }


                        }
                        #endregion Asset

                    }
                    else if (Convert.ToString(e.NewValues["AccountType"]) != "Asset")
                    {
                        NoOfRowEffected = mahbl.MainAccountGridInserting(e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                           "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                           ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                           e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", "",
                           e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", e.NewValues["TDSRate"] != null ? Convert.ToString(e.NewValues["TDSRate"]) : "",
                           Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                           e.NewValues["RateOfIntrest"] != null ? Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString()) : 0, 0,
                            Convert.ToString(HttpContext.Current.Session["userid"]), branch, paymentTypevalue, LedgerBranchList, strOldUnitLedger, strReverseAppilcble);
                    }
                    //else
                    //{
                    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Please Select a Asset Type');", true);
                    //    MainAccountGrid.JSProperties["cpinsert"] = "Please Select a Asset Type";
                    //    MainAccountGrid.CancelEdit();
                    //    return;
                    //}
                }
                //else
                //{
                //    //AccountGroup = Convert.ToString(DBNull.Value);
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Please Select a Account Type');", true);
                //    MainAccountGrid.CancelEdit();
                //    //MainAccountGrid.JSProperties["cpinsert"] = "Please Select a Account Type";
                //    return;
                //}

                //NoOfRowEffected = mahbl.MainAccountGridInserting(e.NewValues["AccountType"] != null ? Convert.ToString(e.NewValues["AccountType"]) : "", BankCompany,
                //   e.NewValues["BankCashType"] != null ? Convert.ToString(e.NewValues["BankCashType"]) : "", e.NewValues["BankAccountType"] != null ? Convert.ToString(e.NewValues["BankAccountType"]) : "Own",
                //   ExchengSegment, e.NewValues["AccountCode"] != null ? Convert.ToString(e.NewValues["AccountCode"]) : "", AccountGroup,
                //   e.NewValues["AccountName"] != null ? e.NewValues["AccountName"].ToString() : "", e.NewValues["BankAccountNo"] != null ? e.NewValues["BankAccountNo"].ToString() : "",
                //   e.NewValues["SubLedgerType"] != null ? Convert.ToString(e.NewValues["SubLedgerType"]) : "None", e.NewValues["TDSRate"] != null ? Convert.ToString(e.NewValues["TDSRate"]) : "",
                //   Convert.ToBoolean(e.NewValues["FBTApplicable"]), e.NewValues["FBTRate"] != null ? Convert.ToDecimal(e.NewValues["FBTRate"].ToString()) : 0,
                //   e.NewValues["RateOfIntrest"] != null ? Convert.ToDecimal(e.NewValues["RateOfIntrest"].ToString()) : 0,
                //    e.NewValues["Depreciation"] != null ? Convert.ToDecimal(e.NewValues["Depreciation"].ToString()) : 0,
                //    Convert.ToString(HttpContext.Current.Session["userid"]));
                if (NoOfRowEffected > 0)
                {
                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                    {
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("AH", "MainAccountHead" + Convert.ToString(NoOfRowEffected), udfTable, Convert.ToString(Session["userid"]));
                    }
                    MainAccountGrid.JSProperties["cpinsert"] = "Saved successfully";
                }
                else
                {
                    MainAccountGrid.JSProperties["cpinsert"] = "Saved unsuccessful.";
                }

                // .............................Code Above Commented and Added by Sam on 19122016......................................  
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                e.Cancel = true;

            }
            e.Cancel = true;
            MainAccountGrid.CancelEdit();

            // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 

        }

        #endregion GridView Event

        protected void MainAccountGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            //if (Session["userbranchID"] != null)
            //{
            //    ASPxComboBox comboBranchName = (ASPxComboBox)MainAccountGrid.FindEditFormTemplateControl("CmbBranch");
            //    comboBranchName.SelectedItem = comboBranchName.Items.FindByValue(Convert.ToString(Session["userbranchID"]));

            //}
            e.NewValues["AccountType"] = "Asset";
            e.NewValues["BankCashType"] = "Bank";



        }

        [WebMethod]
        public static bool CheckUniqueCode(string strAccountCode, string Accountid)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {

                DataTable dt = new DataTable();
                if (Accountid == "0")
                {
                    dt = oGenericMethod.GetDataTable("SELECT  * FROM Master_MainAccount where MainAccount_AccountCode='" + strAccountCode + "'");
                }
                else
                {
                    dt = oGenericMethod.GetDataTable("SELECT  * FROM Master_MainAccount where MainAccount_AccountCode = '" + strAccountCode + "' and MainAccount_ReferenceID<>'" + Accountid + "'");
                }

                int cnt = dt.Rows.Count;
                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

        protected void combAccountGroup_Callback(object sender, CallbackEventArgsBase e)
        {
            var p = e.Parameter;
            if (p == "0")
            {
                AllAccountGroup.SelectCommand = "Select AccountGroup_Name as Name, cast([AccountGroup_ReferenceID]  as varchar(100)) as ID ,AccountGroup_Name as AccountGroup  from Master_AccountGroup where AccountGroup_Type='Asset'";
                ((ASPxComboBox)sender as ASPxComboBox).DataBind();

            }
            else if (p == "1")
            {
                AllAccountGroup.SelectCommand = "Select AccountGroup_Name as Name, cast([AccountGroup_ReferenceID]  as varchar(100)) as ID ,AccountGroup_Name as AccountGroup  from Master_AccountGroup where AccountGroup_Type='Liability'";
                ((ASPxComboBox)sender as ASPxComboBox).DataBind();
            }
            else if (p == "2")
            {
                AllAccountGroup.SelectCommand = "Select AccountGroup_Name as Name, cast([AccountGroup_ReferenceID]  as varchar(100)) as ID ,AccountGroup_Name as AccountGroup  from Master_AccountGroup where AccountGroup_Type='Income'";
                ((ASPxComboBox)sender as ASPxComboBox).DataBind();
            }

            else
            {
                AllAccountGroup.SelectCommand = "Select AccountGroup_Name as Name, cast([AccountGroup_ReferenceID]  as varchar(100)) as ID ,AccountGroup_Name as AccountGroup  from Master_AccountGroup where AccountGroup_Type='Expenses'";
                ((ASPxComboBox)sender as ASPxComboBox).DataBind();
            }

        }

        protected void branchGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receviedString = e.Parameters;
            branchGrid.JSProperties["cpReceviedString"] = receviedString;
            if (receviedString == "SetAllRecordToDataTable")
            {
                List<object> branchList = branchGrid.GetSelectedFieldValues("branch_id");
                CreateBranchTable();
                DataTable branchListtable = (DataTable)Session["BranchListTableForMainAccount"];
                foreach (object obj in branchList)
                {
                    branchListtable.Rows.Add(Convert.ToInt32(obj));

                }
                Session["BranchListTableForMainAccount"] = branchListtable;
            }
            else if (receviedString == "SetAllSelectedRecord")
            {
                DataTable branchListtable = (DataTable)Session["BranchListTableForMainAccount"];
                branchGrid.Selection.UnselectAll();
                if (branchListtable != null)
                {
                    foreach (DataRow dr in branchListtable.Rows)
                    {
                        branchGrid.Selection.SelectRowByKey(dr["Branch_id"]);
                    }
                }
            }
            else {
                DataTable dt = new DataTable();
            // receviedString   this is main account id
               dt= oDBEngine.GetDataTable("select branch_id from  tbl_master_ledgerBranch_map where MainAccount_id='" + receviedString+"'");
               branchGrid.Selection.UnselectAll();
               for (int i = 0; i < dt.Rows.Count; i++)
               {

                   branchGrid.Selection.SelectRowByKey(dt.Rows[i]["branch_id"]);

               }
            
            
            }


        }

        public void CreateBranchTable()
        {
            DataTable branchListtable = new DataTable();
            branchListtable.Columns.Add("Branch_id", typeof(System.Int32));
            Session["BranchListTableForMainAccount"] = branchListtable;
        }

        private void SetBranchRecordToSessionTable(string Keyvalue)
        {
            DataTable branchListtable = oDBEngine.GetDataTable("select branch_id Branch_id from tbl_master_ledgerBranch_map where mainAccount_id=" + Keyvalue);
            Session["BranchListTableForMainAccount"] = branchListtable;
        }

        public string GetBranchList()
        {
            DataTable branchListtable = (DataTable)Session["BranchListTableForMainAccount"];
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
        //Rev Rajdip
        public class ClassDeducteeType
        {
            public string ID { get; set; }
            //public string Name { get; set; }
            public string Type_Name { get; set; }
        }
        [WebMethod]
        public static List<ClassDeducteeType> Binddeducteetype()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtdeducteetype = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlDataAdapter dadeducteetype = new SqlDataAdapter("select ID,TYPE_NAME from TDSRATE_TYPE",con);
            dadeducteetype.Fill(dtdeducteetype);
            con.Dispose();
            List<ClassDeducteeType> ClassDeducteeType = new List<ClassDeducteeType>();
            ClassDeducteeType = (from DataRow dr in dtdeducteetype.Rows
                                  select new ClassDeducteeType()
                                  {
                                      ID = dr["ID"].ToString(),
                                      Type_Name = dr["Type_Name"].ToString(),
     
                                  }).ToList();

            return ClassDeducteeType;

        }

        //End Rev Rajdip
        #region ###### Mapping Section #############
        [WebMethod]
        //Rev Rajdip
        //public static bool MapLedgerToHSNSCA(string LedgerID, string HSNSCACode, string HSNSCAType, bool FOBFlag, string GSTIN)
        public static bool MapLedgerToHSNSCA(string LedgerID, string HSNSCACode, string HSNSCAType, bool FOBFlag, string GSTIN, string PAN, string Deductee_Type, string NameAsPerPan)
        //End Rev Rajdip
        {
            bool flag = false;
            MainAccountHead_BL mahbl = new MainAccountHead_BL();
            try
            {
                //int NoofRowsAffected = mahbl.MappedHSNSACToLedger(LedgerID, HSNSCACode, HSNSCAType, FOBFlag);
                //Rev Rajdip
                //int NoofRowsAffected = mahbl.MappedHSNSACToLedger(LedgerID, HSNSCACode, HSNSCAType, FOBFlag, GSTIN);
                int NoofRowsAffected = mahbl.MappedHSNSACToLedger(LedgerID, HSNSCACode, HSNSCAType, FOBFlag, GSTIN, PAN, Deductee_Type, NameAsPerPan);
                //End Rev Rajdip
                if (NoofRowsAffected > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }
        
        [WebMethod]
        public static ArrayList GetMappedHSNSCAData(string LedgerID)
        {
            DataSet ds = new DataSet();
            ArrayList arrList = new ArrayList();
            MainAccountHead_BL mahbl = new MainAccountHead_BL();
            try
            {
                ds = mahbl.GetMappedHSNSACNoList(LedgerID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    arrList.Add(ds.Tables[0].Rows[0]["HSN_SAC_id"]);
                    arrList.Add(ds.Tables[0].Rows[0]["HSN_SAC_Type"]);
                    arrList.Add(ds.Tables[0].Rows[0]["Furtherance_Of_Busines_Flag"]);
                    arrList.Add(ds.Tables[0].Rows[0]["HSN_SAC_Code"]);
                    // Code Added By Sam on 05082017 to add GSTIN field 
                    arrList.Add(ds.Tables[0].Rows[0]["GSTIN"]);
                    //Rev Rajdip
                    arrList.Add(ds.Tables[0].Rows[0]["LEDGER_PAN"]);
                    arrList.Add(ds.Tables[0].Rows[0]["Deductee_Type"]);
                    arrList.Add(ds.Tables[0].Rows[0]["MainAccount_NameFromPAN"]);
                    //END REV RAJDIP
                    // Code Added By Sam on 05082017 to add GSTIN field 
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return arrList;
        
        }

        [WebMethod]
        public static string DeleteMainAccount(string acnt_id)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
                proc.AddVarcharPara("@Action", 20, "delete");
                proc.AddBigIntegerPara("@main_acnt_id", Convert.ToInt64(acnt_id));
                proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                NoOfRowEffected = proc.RunActionQuery();
                output = Convert.ToString(proc.GetParaValue("@is_success"));
            }
            catch(Exception ex)
            {
                output = ex.Message.ToString();
            }


            return output;
            
        }

        #endregion

        protected void branchGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
            {
                e.Enabled = false;
            }
        }


    }
}