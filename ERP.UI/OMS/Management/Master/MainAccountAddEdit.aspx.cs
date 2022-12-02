using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ERP.OMS.Management.Master
{
    public partial class MainAccountAddEdit : System.Web.UI.Page
    {
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            hdMainActId.Value = "";
            CommonBL ComBL = new CommonBL();
            string SubledgerCashBankTypeCheck = ComBL.GetSystemSettingsResult("SubledgerCashBankType");

            if (!String.IsNullOrEmpty(SubledgerCashBankTypeCheck))
            {
                if (SubledgerCashBankTypeCheck == "Yes")
                {                   
                    hdnSubledgerCashBankType.Value="1";
                }
                else if (SubledgerCashBankTypeCheck.ToUpper().Trim() == "NO")
                {
                    hdnSubledgerCashBankType.Value = "0";
                }
            }

            if (!Page.IsPostBack)
            {
                Session["EditId"] = null;
                PopulateMainAccountDtls();
                //Rev work start 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
                //if (Request.QueryString["id"] != "ADD")
                if (Request.QueryString["id"] != "ADD" && Request.QueryString["key"] != "Copy")
                {
                //Rev work close 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
                    hdMainActId.Value = Request.QueryString["id"];
                    Session["EditId"] = Request.QueryString["id"];
                    PopulateMailAccountDetailsById(Request.QueryString["id"]);
                    Keyval_internalId.Value = "MainAccountHead" + Request.QueryString["id"];
                    hdnModuleMAPID.Value = Request.QueryString["id"];
                }
                //Rev work start 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
                else if(Request.QueryString["id"] != "ADD" && Request.QueryString["key"] == "Copy")
                {                    
                    Keyval_internalId.Value = "Add";

                    hdMainActId.Value = Request.QueryString["id"];
                    Session["EditId"] = Request.QueryString["id"];
                    PopulateMailAccountDetailsById(Request.QueryString["id"]);
                    Keyval_internalId.Value = "MainAccountHead" + Request.QueryString["id"];
                    hdnModuleMAPID.Value = Request.QueryString["id"];
                }
                //Rev work close 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
                else
                {
                    hdMainActId.Value = "0";
                   // BranchdataSource.SelectCommand = "select branch_id,branch_code,branch_description from tbl_master_branch";
                    Keyval_internalId.Value = "Add";
                }
                  
            }
           
        }
        protected void drp_acnt_grp_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {

            GetAccountGroup();
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {

            Save();
        }
        #endregion

        #region Custom Methods
        private void PopulateMainAccountDtls()
        {
            try
            {

                DataSet ds = new DataSet();
                MainAccountBal obj = new MainAccountBal();
                string return_msg = "";
                ds = obj.PopulateMainAccountDtls(ref return_msg, Convert.ToInt32(Session["userid"].ToString()));
                if (ds == null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" +return_msg.Replace("'", "") + "');</script>");
                }
                else
                {
                    drp_acnt_type.DataSource = ds.Tables[0];
                    drp_acnt_type.TextField = "text";
                    drp_acnt_type.ValueField = "value";
                    drp_acnt_type.DataBind();
                    drp_acnt_type.SelectedIndex = 0;


                    drp_acnt_grp.DataSource = ds.Tables[1];
                    drp_acnt_grp.TextField = "CategoryArrange";
                    drp_acnt_grp.ValueField = "AccountGroup_ReferenceID";
                    drp_acnt_grp.DataBind();
                    drp_acnt_grp.SelectedIndex = 0;

                    drp_cmp_nm.DataSource = ds.Tables[2];
                    drp_cmp_nm.TextField = "cmp_name";
                    drp_cmp_nm.ValueField = "cmp_internalId";
                    drp_cmp_nm.DataBind();
                    drp_cmp_nm.SelectedIndex = 0;

                    drp_tds_section.DataSource = ds.Tables[3];
                    drp_tds_section.TextField = "tdsdescription";
                    drp_tds_section.ValueField = "tdscode";
                    drp_tds_section.DataBind();

                }
            }

            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + ex.Message.Replace("'", "") + "');</script>");
            }

        }

        private void GetAccountGroup()
        {
            try
            {
                DataTable dt = new DataTable();
                MainAccountBal obj = new MainAccountBal();
                string return_msg = "";
                dt = obj.GetAccountGroup(ref return_msg, drp_acnt_type.Text);
                if (dt == null)
                {
                    drp_acnt_grp.JSProperties["cperrormsg"] = return_msg;
                }
                else
                {


                    drp_acnt_grp.DataSource = dt;
                    drp_acnt_grp.TextField = "CategoryArrange";
                    drp_acnt_grp.ValueField = "AccountGroup_ReferenceID";
                    drp_acnt_grp.DataBind();
                    drp_acnt_grp.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + ex.Message.Replace("'", "") + "');</script>");
            }
        }
        private void Save()
        {
            try
            {

                string _ModuleSet = hdnModuleSet.Value;
                string ddlModule = "";
                if (_ModuleSet != "")
                {
                    ddlModule = String.Join(",", _ModuleSet);                   
                }

                var _dtTable = new DataTable();
                _dtTable.Columns.Add("branch_id", typeof(int));
                List<object> branchList = BranchGridLookup.GridView.GetSelectedFieldValues("branch_id");



                if (branchList.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('Select atleast one branch');</script>");
                    return;
                }

                foreach (var branch in branchList)
                {
                    DataRow dr = _dtTable.NewRow();

                    dr["branch_id"] = branch;



                    _dtTable.Rows.Add(dr);
                }

                MainAccountBal obj = new MainAccountBal();
                string output = string.Empty;
                long acnt_id = 0;
                string action = "main_acnt_save", ReturnId = "0";
                //Rev work start 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
                //if (Request.QueryString["id"] != "ADD")
                if (Request.QueryString["id"] != "ADD" && Request.QueryString["key"] != "Copy")
                //Rev work close 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
                {
                    acnt_id = Convert.ToInt64(Request.QueryString["id"]);
                    action = "modify";

                }

                string Subledger="None";
                if (hdnSubledgerCashBankType.Value=="1")
                {
                    //Subledger = (((drp_acnt_type.Value.ToString().Trim() == "Asset") || (drp_acnt_type.Value.ToString().Trim() == "Liability")) ? (((drp_asset_type.Value.ToString().Trim() == "Bank") || (drp_asset_type.Value.ToString().Trim() == "Cash")) ? drp_sub_ledger_type.Value.ToString().Trim() : "None") : "None");
                    Subledger = drp_sub_ledger_type.Value.ToString().Trim();
                }
                else
                {
                    Subledger = (drp_acnt_type.Value.ToString().Trim() != "Asset" ? drp_sub_ledger_type.Value.ToString().Trim() : (Convert.ToString(drp_asset_type.Value).Trim() == "Fixed Asset" || Convert.ToString(drp_asset_type.Value).Trim() == "Other") ? drp_sub_ledger_type.Value.ToString().Trim() : "None");
                }


                output = obj.MainAccountSave(acnt_id, action, drp_acnt_type.Value.ToString().Trim(), drp_cmp_nm.Value.ToString().Trim(), //4
                    drp_acnt_type.Value.ToString().Trim() == "Asset" || drp_acnt_type.Value.ToString().Trim() == "Liability" ? Convert.ToString(drp_asset_type.Value).Trim() : "", //1
                    txt_short_nm.Text.ToString().Trim(), drp_acnt_grp.Value.ToString().Trim(), txt_acnt_nm.Text.ToString().Trim(),  //3
                    ((drp_acnt_type.Value.ToString().Trim() == "Asset" || drp_acnt_type.Value.ToString().Trim() == "Liability" ) && Convert.ToString(drp_asset_type.Value).Trim() == "Bank") ? txt_bnk_acnt_nmbr.Text.ToString().Trim() : "", //1
                   // drp_acnt_type.Value.ToString().Trim() != "Asset" ? drp_sub_ledger_type.Value.ToString().Trim() : (Convert.ToString(drp_asset_type.Value).Trim() == "Fixed Asset" || Convert.ToString(drp_asset_type.Value).Trim() == "Other") ? drp_sub_ledger_type.Value.ToString().Trim() : "None",
                   Subledger.Trim(),
                    drp_acnt_type.Value.ToString().Trim() != "Asset" ? Convert.ToString(drp_tds_section.Value).Trim() : (Convert.ToString(drp_asset_type.Value).Trim() == "Other" ? Convert.ToString(drp_tds_section.Value).Trim() : ""), Convert.ToString(drp_asset_type.Value).Trim() == "Fixed Asset" ? Convert.ToDecimal(txtDepreciation.Text.ToString().Trim()) : 0,
                    Convert.ToString(HttpContext.Current.Session["userid"]), cPaymenttype.Value.ToString().Trim(), drp_old_unit_ledger.Value.ToString().Trim(),
                    drp_revrs_applicabl.Value.ToString().Trim(), _dtTable, Convert.ToString(HttpContext.Current.Session["userid"]), ref ReturnId, txtBalanceLimit.Text.ToString().Trim(),
                    Convert.ToString(cmbNegativeStk.Value), txtDailyLimit.Text.ToString().Trim(), Convert.ToString(cmbDailyLimitExceed.Value)
                    , Convert.ToBoolean(Isparty.Value), Convert.ToString(cmbDeducteestat.Value), Convert.ToString(cmbTaxdeducteedType.Value)

                    ,Convert.ToString(ddlModule)

                    );


                DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                if (udfTable != null)
                {
                    Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("AH", "MainAccountHead" + ReturnId, udfTable, Convert.ToString(Session["userid"]));
                }


                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + output.Replace("'","") + "');</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + ex.Message.Replace("'", "") + "');</script>");
            }

        }
        [WebMethod]
        public static object PopulateAllModule(string ModuleMAPID)
        {            
            string actionqry = "PopulateAllModule";

            List<ModuleDetails> GrpDet = new List<ModuleDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
                proc.AddVarcharPara("@action", 100, actionqry);
                proc.AddVarcharPara("@ModuleMAPID", 100, ModuleMAPID);
                
                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new ModuleDetails
                          {
                              Module_Name = Convert.ToString(dr["Test"]),
                              Module_Value = Convert.ToString(dr["Value"]),
                              IsChecked = Convert.ToString(dr["IsChecked"])

                          }).ToList();

            }
            return GrpDet;
        }
        
        public class ModuleDetails
        {

            public string Module_Name { get; set; }
            public string Module_Value { get; set; }

            public string IsChecked { get; set; }
        }
        private void PopulateMailAccountDetailsById(string acnt_reference_id)
        {
            try
            {

                DataSet ds = new DataSet();
                MainAccountBal obj = new MainAccountBal();
                string return_msg = "",Trans_msg="";
                ds = obj.PopulateMainAccountDtlsById(ref return_msg, acnt_reference_id);
                DataTable dtex = new DataTable();
                dtex = obj.TransactionCheck(ref Trans_msg, acnt_reference_id);

                if (Trans_msg=="1")
                {
                    drp_acnt_type.ClientEnabled = false;
                }
                else if (Trans_msg == "0")
                {
                    drp_acnt_type.ClientEnabled = true;
                }

                if (ds == null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + return_msg.Replace("'", "") + "');</script>");
                }
                else
                {
                    if (return_msg == "true")
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //Rev work start 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
                            //txt_short_nm.ClientEnabled = false;                           
                            
                            //txt_acnt_nm.Text = ds.Tables[0].Rows[0]["AccountName"].ToString().Trim();
                            //txt_short_nm.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString().Trim();
                            if (Request.QueryString["key"] == "Copy")
                            {
                                txt_short_nm.ClientEnabled = true;
                                txt_acnt_nm.Text = "";
                                txt_short_nm.Text = "";
                            }
                            else
                            {
                                txt_short_nm.ClientEnabled = false;
                                txt_acnt_nm.Text = ds.Tables[0].Rows[0]["AccountName"].ToString().Trim();
                                txt_short_nm.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString().Trim();
                            }
                            drp_acnt_type.Value = ds.Tables[0].Rows[0]["AccountType"].ToString().Trim();
                            drp_acnt_grp.DataSource = ds.Tables[2];
                            drp_acnt_grp.TextField = "CategoryArrange";
                            drp_acnt_grp.ValueField = "AccountGroup_ReferenceID";
                            drp_acnt_grp.DataBind();
                            drp_acnt_grp.Value = ds.Tables[0].Rows[0]["AccountGroup"].ToString().Trim();
                            drp_asset_type.Value = ds.Tables[0].Rows[0]["BankCashType"].ToString().Trim();
                            txt_bnk_acnt_nmbr.Text = ds.Tables[0].Rows[0]["BankAccountNo"].ToString().Trim();

                            txtBalanceLimit.Text = ds.Tables[0].Rows[0]["Cash_Bank_BalanceLimit"].ToString().Trim();
                            cmbNegativeStk.Value = ds.Tables[0].Rows[0]["NegativeStock"].ToString().Trim();

                            txtDailyLimit.Text = ds.Tables[0].Rows[0]["DailtLimit"].ToString().Trim();
                            cmbDailyLimitExceed.Value = ds.Tables[0].Rows[0]["DailyLimitExceed"].ToString().Trim();

                            Isparty.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISPARTY"].ToString().Trim());

                            cmbDeducteestat.Value = Convert.ToString(ds.Tables[0].Rows[0]["MainAccount_DeducteeStatus"]);

                            cmbTaxdeducteedType.Value = Convert.ToString(ds.Tables[0].Rows[0]["MainAccount_TaxEntityType"]);

                            if (ds.Tables[0].Rows[0]["BankCompany"].ToString().Trim() == "")
                            {
                                drp_cmp_nm.SelectedIndex = 0;
                            }
                            else
                            {
                                drp_cmp_nm.Value = ds.Tables[0].Rows[0]["BankCompany"].ToString().Trim();
                            }



                            if (ds.Tables[0].Rows[0]["AccountType"].ToString().Trim() == "Asset" && ds.Tables[0].Rows[0]["BankCashType"].ToString().Trim() == "Bank")
                            {
                                cPaymenttype.Items.Clear();
                                cPaymenttype.Items.Add("None", "None");
                                cPaymenttype.Items.Add("Card", "Card");
                                cPaymenttype.Items.Add("Coupon", "Coupon");
                                cPaymenttype.Items.Add("Etransfer", "Etransfer");
                            }

                            else
                            {
                                cPaymenttype.Items.Clear();
                                cPaymenttype.Items.Add("None", "None");
                                cPaymenttype.Items.Add("Ledger for Interstate Stk-Out", "LedgOut");
                                cPaymenttype.Items.Add("Ledger for Interstate Stk-In", "LedgIn");
                                cPaymenttype.Items.Add("Finance Processing Fee", "PrcFee");
                                cPaymenttype.Items.Add("Finance Other Charges Emi", "EmiCharge");
                                cPaymenttype.Items.Add("Goods in Transit", "TrnstGoods");
                            }

                            cPaymenttype.Value = ds.Tables[0].Rows[0]["MainAccount_PaymentType"].ToString().Trim();
                            drp_sub_ledger_type.Value = ds.Tables[0].Rows[0]["SubLedgerType"].ToString().Trim();
                            txtDepreciation.Text = ds.Tables[0].Rows[0]["Depreciation"].ToString().Trim();
                            drp_tds_section.Value = ds.Tables[0].Rows[0]["TDSRate"].ToString().Trim();
                            drp_old_unit_ledger.Value = ds.Tables[0].Rows[0]["MainAccount_OldUnitLedger"].ToString().Trim();
                            drp_revrs_applicabl.Value = ds.Tables[0].Rows[0]["MainAccount_ReverseApplicable"].ToString().Trim();

                            BranchdataSource.SelectCommand = @"select br.branch_id,branch_code,branch_description from tbl_master_branch br 
                            inner join tbl_master_ledgerBranch_map map on map.branch_id = br.branch_id
                            where MainAccount_id=" + Request.QueryString["id"] + @" union all  
                            select branch_id,branch_code,branch_description from tbl_master_branch br where branch_id not in
                            (select branch_id from tbl_master_ledgerBranch_map where MainAccount_id='" + Request.QueryString["id"] + "')";
                            BranchGridLookup.DataBind();
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {

                                BranchGridLookup.GridView.Selection.SelectRowByKey(ds.Tables[1].Rows[i]["branch_id"]);

                            }


                        }
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            hdnModuleSet.Value = ds.Tables[3].Rows[0]["ModuleName"].ToString().Trim();
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + ex.Message.Replace("'", "") + "');</script>");
            }
        }

        #endregion




    }
}