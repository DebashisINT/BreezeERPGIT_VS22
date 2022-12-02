using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.Services;
using BusinessLogicLayer;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using EntityLayer.CommonELS;
using DataAccessLayer;
using ERP.Models;
using System.Linq;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_TdsTcsPopUp : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string RetVal = "";
        string RVal = "I~0";


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {

                    //SqlTscTcsRate.ConnectionString = Convert.ToString(ConfigurationSettings.AppSettings["DBReadOnlyConnection"]); MULTI
                    SqlTscTcsRate.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //SqlTscTcsRate.ConnectionString = Convert.ToString(ConfigurationSettings.AppSettings["DBReadOnlyConnection"]); MULTI
                    SqlTscTcsRate.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
            }
            BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Master/frm_TdsTcsPopUp.aspx");
            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            //Start Rev set TDS type OLD/NEW Tanmoy 30-07-2019
            TdsTcsBL objTdsTcsBL = new TdsTcsBL();
            DataTable dt = objTdsTcsBL.GetTDSSettings();
            if (dt != null && dt.Rows.Count > 0)
            {
                hdnOldTDS.Value = dt.Rows[0]["Variable_Value"].ToString();
            }
            if (hdnOldTDS.Value == "No")
            {
                divOldTds.Attributes["class"] = "hidden";


                //System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("divOldTds");
                //dynDiv.Attributes.Add("Style", "Display:none");
            }
            else
            {
                divNewTDS.Attributes["class"] = "hidden";
            }

            if (!IsPostBack)
            {
                Session["exportval"] = null;
                LoadTdsDD("TDS");
                BindApplicablefor("TDS");
                LoadTCSPayable();
                hddnVal.Value = "";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "JScript", "<script language='JavaScript'>Page_Load();</script>");
                //txtMainAccount.Attributes.Add("onkeyup", "call_ajax(this,'MainAccountForTDS',event)");
                PopulateMainAccountDropDownForTDSTCS();
                //txtSubAccount.Attributes.Add("onkeyup", "call_ajax1(this,'SubAccount',event)");
                if (Request.QueryString["id"] != null)
                {
                    if (Convert.ToString(Request.QueryString["id"]) != "ADD")
                    {
                        BindData();
                    }
                }
                // .............................Code Commented and Added by Sam on 20122016. to replace Autocomple and use of chosen drop down.....................................

                // .............................Code Above Commented and Added by Sam on 20122016...................................... 


            }
            Session["KeyVal"] = txtCode.Text;
        }

        public void LoadTCSPayable()
        {
            string branch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            TdsTcsBL objTdsTcsBL = new TdsTcsBL();
            DataTable dt = new DataTable();
            dt = objTdsTcsBL.PopulateSubAccountDropDownForTDSTCS("0", branch);

            lstTCSPayable.DataTextField = "Contact_Name";
            lstTCSPayable.DataValueField = "cnt_internalID";
            lstTCSPayable.DataSource = dt;
            lstTCSPayable.DataBind();
        }

        public void BindApplicablefor(String Type)
        {
            //VendorTDSBl tdsdetails = new VendorTDSBl();
            //DataTable tdsMaster = tdsdetails.GetTDSMASTERLIST();

            ProcedureExecute proc = new ProcedureExecute("PRC_TDSMASTERLIST");
            proc.AddPara("@TYPE", Type);
            // Rev Mantis Issue 24468
            proc.AddPara("@Action", "EXCLUDE_NOTAPPLICABLE");
            // End of Mantis Issue 24468
            DataTable tdsMaster = proc.GetTable();
            if (tdsMaster != null && tdsMaster.Rows.Count > 0)
            {
                aspxDeducteesNew.TextField = "TYPE_NAME";
                aspxDeducteesNew.ValueField = "ID";
                aspxDeducteesNew.DataSource = tdsMaster;
                aspxDeducteesNew.DataBind();
            }
        }

        public void LoadTdsDD(String Type)
        {
            ProcedureExecute proc = new ProcedureExecute("GetTdsDetails");
            proc.AddPara("@TYPE", Type);
            DataTable TdsTable = proc.GetTable();
            TdsSection.DataSource = TdsTable;
            TdsSection.TextField = "Section_Description";
            TdsSection.ValueField = "Section_Code";
            TdsSection.DataBind();
        }

        // .............................Code Commented and Added by Sam on 20122016. to replace Autocomple and use of chosen drop down.....................................
        public void PopulateMainAccountDropDownForTDSTCS()
        {
            TdsTcsBL objTdsTcsBL = new TdsTcsBL();
            DataTable dt = objTdsTcsBL.PopulateMainAccountDropDownForTDSTCS();
            lstMainAccount.DataTextField = "AccountTextDtl";
            lstMainAccount.DataValueField = "AccountValueDtl";
            lstMainAccount.DataSource = dt;
            lstMainAccount.DataBind();
            lstMainAccount.Items.Insert(0, new ListItem("Select", "0"));
            lstMainAccount.SelectedIndex = 0;


            lstPurchase.DataTextField = "AccountTextDtl";
            lstPurchase.DataValueField = "AccountValueDtl";
            lstPurchase.DataSource = dt;
            lstPurchase.DataBind();
            lstPurchase.Items.Insert(0, new ListItem("Select", "0"));
            lstPurchase.SelectedIndex = 0;


            lstInterestLedger.DataTextField = "AccountTextDtl";
            lstInterestLedger.DataValueField = "AccountValueDtl";
            lstInterestLedger.DataSource = dt;
            lstInterestLedger.DataBind();
            lstInterestLedger.Items.Insert(0, new ListItem("Select", "0"));
            lstInterestLedger.SelectedIndex = 0;


            lstLateFeeLedger.DataTextField = "AccountTextDtl";
            lstLateFeeLedger.DataValueField = "AccountValueDtl";
            lstLateFeeLedger.DataSource = dt;
            lstLateFeeLedger.DataBind();
            lstLateFeeLedger.Items.Insert(0, new ListItem("Select", "0"));
            lstLateFeeLedger.SelectedIndex = 0;


            lstOthersLedger.DataTextField = "AccountTextDtl";
            lstOthersLedger.DataValueField = "AccountValueDtl";
            lstOthersLedger.DataSource = dt;
            lstOthersLedger.DataBind();
            lstOthersLedger.Items.Insert(0, new ListItem("Select", "0"));
            lstOthersLedger.SelectedIndex = 0;


            lstSales.DataTextField = "AccountTextDtl";
            lstSales.DataValueField = "AccountValueDtl";
            lstSales.DataSource = dt;
            lstSales.DataBind();
            lstSales.Items.Insert(0, new ListItem("Select", "0"));
            lstSales.SelectedIndex = 0;

            // Rev Mantis Issue 24161
            lstAdvanceLedger.DataTextField = "AccountTextDtl";
            lstAdvanceLedger.DataValueField = "AccountValueDtl";
            lstAdvanceLedger.DataSource = dt;
            lstAdvanceLedger.DataBind();
            lstAdvanceLedger.Items.Insert(0, new ListItem("Select", "0"));
            lstAdvanceLedger.SelectedIndex = 0;
            // End of Rev Mantis Issue 24161
        }

        // .............................Code Above Commented and Added by Sam on 20122016...................................... 


        [System.Web.Services.WebMethod]

        protected void ASPxComboBox1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string MainAccountID = "";

            string InterestLedgerID = "";
            string LateFeeLedgerID = "";
            string OthersLedgerID = "";
            string purchaseLedger = "";

            string salesLedger = "";
            // Rev Mantis Issue 24161
            string AdvanceLedgerID = "";
            // End of Rev Mantis Issue 24161

            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                try
                {

                    // .............................Code Commented and Added by Sam on 21122016. .....................................  

                    //string[] MainID = Convert.ToString(txtMainAccount_hidden.Value).Split('~');
                    string[] MainID = Convert.ToString(lstMainAccount.SelectedValue).Split('~');
                    if (MainID.Length > 1)
                    {
                        MainAccountID = Convert.ToString(MainID[0]);
                    }
                    else
                    {
                        MainAccountID = txtMainAccount_hidden.Value;
                    }

                    string[] InterestID = Convert.ToString(lstInterestLedger.SelectedValue).Split('~');
                    if (InterestID.Length > 1)
                    {
                        InterestLedgerID = Convert.ToString(InterestID[0]);
                    }
                    else
                    {
                        InterestLedgerID = hdnInterestLedgerID.Value;
                    }

                    string[] PurchaseID = Convert.ToString(lstPurchase.SelectedValue).Split('~');
                    if (PurchaseID.Length > 1)
                    {
                        purchaseLedger = Convert.ToString(PurchaseID[0]);
                    }
                    else
                    {
                        purchaseLedger = ltsPurchase_code.Value;
                    }

                    //Sales Account ledger
                    string[] SalesID = Convert.ToString(lstSales.SelectedValue).Split('~');
                    if (SalesID.Length > 1)
                    {
                        salesLedger = Convert.ToString(SalesID[0]);
                    }
                    else
                    {
                        salesLedger = ltsSales_code.Value;
                    }
                    //Sales Acount ldger



                    string[] LateFeeID = Convert.ToString(lstLateFeeLedger.SelectedValue).Split('~');
                    if (LateFeeID.Length > 1)
                    {
                        LateFeeLedgerID = Convert.ToString(LateFeeID[0]);
                    }
                    else
                    {
                        LateFeeLedgerID = hdnLateFeeLedgerID.Value;
                    }

                    string[] OthersID = Convert.ToString(lstOthersLedger.SelectedValue).Split('~');
                    if (OthersID.Length > 1)
                    {
                        OthersLedgerID = Convert.ToString(OthersID[0]);
                    }
                    else
                    {
                        OthersLedgerID = hdnOthersLedgerID.Value;
                    }

                    // Rev Mantis Issue 24161
                    string[] AdvanceID = Convert.ToString(lstAdvanceLedger.SelectedValue).Split('~');
                    if (AdvanceID.Length > 1)
                    {
                        AdvanceLedgerID = Convert.ToString(AdvanceID[0]);
                    }
                    else
                    {
                        AdvanceLedgerID = hdnAdvanceLedgerID.Value;
                    }
                    // End of Rev Mantis Issue 24161
                    
                    // .............................Code Above Commented and Added by Sam on 21122016...................................... 
                    int NoofAffect = 0;


                    //string SubMainID = Convert.ToString(lstSubAccount.SelectedValue);
                    //if (MainID.Length > 1)
                    //{
                    //    MainAccountID = Convert.ToString(MainID[0]);
                    //}
                    //else
                    //{
                    //    MainAccountID = txtMainAccount_hidden.Value;
                    //}


                    String con = "";

                    //------- For Read Only User in SQL Datasource Connection String   Start-----------------

                    if (HttpContext.Current.Session["EntryProfileType"] != null)
                    {
                        if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                        {
                            //con = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                            con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        }
                        else
                        {
                            //con = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                            con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        }
                    }

                    //------- For Read Only User in SQL Datasource Connection String   End-----------------



                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    //SqlCommand lcmdBrkgInsert = new SqlCommand("InsertTDSTCS", lcon); prc_TDSTCS
                    SqlCommand lcmdBrkgInsert = new SqlCommand("prc_TDSTCS", lcon);
                    lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = new SqlParameter("@ResultId", SqlDbType.BigInt);
                    parameter.Direction = ParameterDirection.Output;
                    lcmdBrkgInsert.Parameters.AddWithValue("@action", "InsertTDSTCS");
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_Type", ddlType.SelectedItem.Text.Trim());
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_Code", txtCode.Text.Trim());
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_Description", txtDescription.Text.Trim());
                    lcmdBrkgInsert.Parameters.AddWithValue("@purchaseLedger", purchaseLedger);

                    lcmdBrkgInsert.Parameters.AddWithValue("@SalesLedger", salesLedger);

                    if (ddlType.SelectedItem.Text.Trim() == "TCS")
                    {
                        lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_SubAccountCode", MainAccountID);
                        if (ddlType.SelectedValue != "0")
                        {
                            if (hdnSubACCode.Value != "" && hdnSubACCode.Value != null)
                            {
                                lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_MainAccountCode", hdnSubACCode.Value);
                            }
                        }
                    }
                    else
                    {
                        lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_MainAccountCode", MainAccountID);
                        //lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_MainAccountCode", txtMainAccount.Text);
                        if (ddlType.SelectedValue != "0")
                        {
                            if (hdnSubACCode.Value != "" && hdnSubACCode.Value != null)
                            {
                                lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_SubAccountCode", hdnSubACCode.Value);
                            }
                            //lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_SubAccountCode", txtSubAccount_hidden.Value);
                        }
                        lcmdBrkgInsert.Parameters.AddWithValue("@Interest_Ledger", InterestLedgerID);
                        lcmdBrkgInsert.Parameters.AddWithValue("@Late_Fee_Ledger", LateFeeLedgerID);
                        lcmdBrkgInsert.Parameters.AddWithValue("@Others_Ledger", OthersLedgerID);
                        // Rev Mantis Issue 24161
                        lcmdBrkgInsert.Parameters.AddWithValue("@Advance_Ledger", AdvanceLedgerID);
                        // End of Rev Mantis Issue 24161
                    }
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_CreateUser", Session["userid"]);
                    lcmdBrkgInsert.Parameters.AddWithValue("@CalculationBasedOn", ddlCalculationBasedOn.SelectedValue.ToString());
                    lcmdBrkgInsert.Parameters.Add(parameter);
                    lcmdBrkgInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802

                    hddnVal.Value = Convert.ToString(lcmdBrkgInsert.Parameters["@ResultId"].Value);
                    //hddnVal.Value = Convert.ToString(parameter.Value);
                    NoofAffect = Convert.ToInt32(hddnVal.Value);
                    if (NoofAffect > 0)
                    {
                        RetVal = "Insert~" + hddnVal.Value;
                        Session["KeyVal"] = txtCode.Text.Trim();
                    }
                    else
                    {
                        RetVal = "Error~0";
                    }
                    //RetVal = "Insert~" + hddnVal.Value;

                }
                catch
                {
                    RetVal = "Error~0";
                }
            }
            else
            {
                try
                {
                    String con = "";
                    // .............................Code Commented and Added by Sam on 21122016. .....................................  
                    if (HttpContext.Current.Session["EntryProfileType"] != null)
                    {
                        if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                        {
                            //con = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                            con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        }
                        else
                        {
                            //con = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                            con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        }
                    }

                    string[] MainID = Convert.ToString(lstMainAccount.SelectedValue).Split('~');
                    String MainAcType = "";
                    //string[] MainID = Convert.ToString(txtMainAccount_hidden.Value).Split('~');
                    if (MainID.Length > 1)
                    {
                        MainAccountID = Convert.ToString(MainID[0]);
                        MainAcType = Convert.ToString(MainID[1]);
                    }
                    else
                    {
                        MainAccountID = txtMainAccount_hidden.Value;
                    }

                    string[] InterestID = Convert.ToString(lstInterestLedger.SelectedValue).Split('~');
                    if (InterestID.Length > 1)
                    {
                        InterestLedgerID = Convert.ToString(InterestID[0]);
                    }
                    else
                    {
                        InterestLedgerID = hdnInterestLedgerID.Value;
                    }

                    string[] LateFeeID = Convert.ToString(lstLateFeeLedger.SelectedValue).Split('~');
                    if (LateFeeID.Length > 1)
                    {
                        LateFeeLedgerID = Convert.ToString(LateFeeID[0]);
                    }
                    else
                    {
                        LateFeeLedgerID = hdnLateFeeLedgerID.Value;
                    }

                    string[] OthersID = Convert.ToString(lstOthersLedger.SelectedValue).Split('~');
                    if (OthersID.Length > 1)
                    {
                        OthersLedgerID = Convert.ToString(OthersID[0]);
                    }
                    else
                    {
                        OthersLedgerID = hdnOthersLedgerID.Value;
                    }

                    string[] PurchaseID = Convert.ToString(lstPurchase.SelectedValue).Split('~');
                    if (PurchaseID.Length > 1)
                    {
                        purchaseLedger = Convert.ToString(PurchaseID[0]);
                    }
                    else
                    {
                        purchaseLedger = ltsPurchase_code.Value;
                    }


                    //Sales Account ledger
                    string[] SalesID = Convert.ToString(lstSales.SelectedValue).Split('~');
                    if (SalesID.Length > 1)
                    {
                        salesLedger = Convert.ToString(SalesID[0]);
                    }
                    else
                    {
                        salesLedger = ltsSales_code.Value;
                    }
                    //Sales Acount ldger

                    // Rev Mantis Issue 24161
                    string[] AdvanceID = Convert.ToString(lstAdvanceLedger.SelectedValue).Split('~');
                    if (AdvanceID.Length > 1)
                    {
                        AdvanceLedgerID = Convert.ToString(AdvanceID[0]);
                    }
                    else
                    {
                        AdvanceLedgerID = hdnAdvanceLedgerID.Value;
                    }
                    // End of Rev Mantis Issue 24161

                    //  string SubMainID = Convert.ToString(lstSubAccount.SelectedValue);

                    string SubMainID = Convert.ToString(lstTCSPayable.SelectedValue);

                    Int32 NoofAffect = 0;
                    // .............................Code Above Commented and Added by Sam on 21122016...................................... 

                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    //SqlCommand lcmdBrkgInsert = new SqlCommand("InsertTDSTCS", lcon); prc_TDSTCS
                    SqlCommand lcmdBrkgInsert = new SqlCommand("prc_TDSTCS", lcon);
                    lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = new SqlParameter("@ResultId", SqlDbType.BigInt);
                    parameter.Direction = ParameterDirection.Output;
                    lcmdBrkgInsert.Parameters.AddWithValue("@action", "UpdateTDSTCS");
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_ID", Convert.ToString(Request.QueryString["id"]));
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_Type", ddlType.SelectedItem.Text.Trim());
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_Code", txtCode.Text.Trim());
                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_Description", txtDescription.Text.Trim());
                    //lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_Description", txtDescription.Text.Trim());
                    lcmdBrkgInsert.Parameters.AddWithValue("@purchaseLedger", purchaseLedger);

                    lcmdBrkgInsert.Parameters.AddWithValue("@SalesLedger", salesLedger);
                    
                    if (ddlType.SelectedItem.Text.Trim() == "TCS")
                    {
                        lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_SubAccountCode", MainAccountID);
                        //lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_MainAccountCode", txtMainAccount.Text);
                        if (ddlType.SelectedValue != "0")
                        {
                            //if (MainAcType != "None")
                            //{
                                if (hdnSubACCode.Value != "" && hdnSubACCode.Value != null)
                                {
                                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_MainAccountCode", hdnSubACCode.Value);
                                }
                            //}
                            //lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_SubAccountCode", txtSubAccount_hidden.Value);
                        }
                    }
                    else
                    {
                        lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_MainAccountCode", MainAccountID);
                        if (ddlType.SelectedValue != "0")
                        {
                            if (MainAcType != "None")
                            {
                                if (hdnSubACCode.Value != "" && hdnSubACCode.Value != null)
                                {
                                    lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_SubAccountCode", hdnSubACCode.Value);
                                }
                            }
                        }
                        lcmdBrkgInsert.Parameters.AddWithValue("@Interest_Ledger", InterestLedgerID);
                        lcmdBrkgInsert.Parameters.AddWithValue("@Late_Fee_Ledger", LateFeeLedgerID);
                        lcmdBrkgInsert.Parameters.AddWithValue("@Others_Ledger", OthersLedgerID);
                        // Rev Mantis Issue 24161
                        lcmdBrkgInsert.Parameters.AddWithValue("@Advance_Ledger", AdvanceLedgerID);
                        // End of Rev Mantis Issue 24161
                    }
                    if (Session["userid"] != null)
                    {
                        lcmdBrkgInsert.Parameters.AddWithValue("@TDSTCS_ModifyUser", Convert.ToInt32(Session["userid"]));
                    }

                    lcmdBrkgInsert.Parameters.AddWithValue("@CalculationBasedOn", ddlCalculationBasedOn.SelectedValue.ToString());

                    lcmdBrkgInsert.Parameters.Add(parameter);
                    lcmdBrkgInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802
                    //hddnVal.Value = Convert.ToString(parameter.Value);
                    //if (ddlType.SelectedValue == "0")
                    //{
                    //      NoofAffect = oDBEngine.SetFieldValue("Master_TDSTCS", " TDSTCS_Type='" + ddlType.SelectedItem.Value + "',TDSTCS_Code='" + txtCode.Text + "',TDSTCS_Description='" + txtDescription.Text + "',TDSTCS_MainAccountCode='" + MainAccountID + "',TDSTCS_SubAccountCode='',TDSTCS_ModifyUser='" + Convert.ToString(Session["userid"]) + "',TDSTCS_ModifyDateTime='" + Convert.ToString(oDBEngine.GetDate()) + "'", " TDSTCS_ID=" + Convert.ToString(Request.QueryString["id"]) + "");
                    //}
                    //else
                    //{
                    //      NoofAffect = oDBEngine.SetFieldValue("Master_TDSTCS", " TDSTCS_Type='" + ddlType.SelectedItem.Value + "',TDSTCS_Code='" + txtCode.Text + "',TDSTCS_Description='" + txtDescription.Text + "',TDSTCS_MainAccountCode='" + MainAccountID + "',TDSTCS_SubAccountCode='" + txtSubAccount_hidden.Value + "',TDSTCS_ModifyUser='" + Convert.ToString(Session["userid"]) + "',TDSTCS_ModifyDateTime='" + Convert.ToString(oDBEngine.GetDate()) + "'", " TDSTCS_ID=" + Convert.ToString(Request.QueryString["id"]) + "");
                    //}
                    ////Int32 NoofAffect = oDBEngine.SetFieldValue("Master_TDSTCS", " TDSTCS_Type='" + ddlType.SelectedItem.Value + "',TDSTCS_Code='" + txtCode.Text + "',TDSTCS_Description='" + txtDescription.Text + "',TDSTCS_MainAccountCode='" + MainAccountID + "',TDSTCS_SubAccountCode='" + txtSubAccount_hidden.Value + "',TDSTCS_ModifyUser='" + Convert.ToString(Session["userid"]) + "',TDSTCS_ModifyDateTime='" + Convert.ToString(oDBEngine.GetDate()) + "'", " TDSTCS_ID=" + Convert.ToString(Request.QueryString["id"]) + "");
                    NoofAffect = Convert.ToInt32(parameter.Value);
                    if (NoofAffect > 0)
                    {
                        RetVal = "Update~0";
                        Session["KeyVal"] = txtCode.Text.Trim();

                    }
                    else
                    {
                        RetVal = "Error~0";
                    }
                }
                catch (Exception ex)
                {
                    RetVal = "Error~0";
                }
            }
        }
        protected void ASPxComboBox1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpInsert"] = RetVal;
        }
        public void BindData()
        {
            TdsTcsBL objTdsTcsBL = new TdsTcsBL();

            DataTable dtEdit = objTdsTcsBL.PopulateTDSTCSInEditMode(Convert.ToInt32(Request.QueryString["id"]));

            // .............................Code Above Commented and Added by Sam on 20122016 to avoid unused data of NSDL and CSDL Table ...................................... 
            //DataTable dtEdit = oDBEngine.GetDataTable("Master_TDSTCS", " TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,TDSTCS_MainAccountCode,TDSTCS_SubAccountCode,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as MainAccountName,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode)))))) as SubAccountName ", " TDSTCS_ID=" + Request.QueryString["id"].ToString() + "");
            //DataTable dtEdit = oDBEngine.GetDataTable("Master_TDSTCS", " TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,TDSTCS_MainAccountCode,TDSTCS_SubAccountCode,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as  MainAccountName,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')  from tbl_master_contact where cnt_internalid=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode), isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode) ,'')))) as SubAccountName  ", " TDSTCS_ID=" + Convert.ToString(Request.QueryString["id"]) + "");

            // .............................Code Above Commented and Added by Sam on 20122016...................................... 

            if (dtEdit != null && dtEdit.Rows.Count > 0)
            {
                string ddltype = Convert.ToString(dtEdit.Rows[0][0]).Trim().ToUpper();
                if (ddltype == "TDS")
                {
                    ddlType.SelectedValue = "0";
                    LoadTdsDD("TDS");
                    BindApplicablefor("TDS");
                }
                else
                {
                    ddlType.SelectedValue = "1";
                    LoadTdsDD("TCS");
                    BindApplicablefor("TCS");

                    divOldTds.Attributes["class"] = "hidden";
                    divNewTDS.Attributes["class"] = "show";
                }

                txtCode.Text = Convert.ToString(dtEdit.Rows[0][1]).Trim();
                TdsSection.Value = Convert.ToString(dtEdit.Rows[0][1]).Trim();
                // txtCode.Enabled = false;
                txtDescription.Text = Convert.ToString(dtEdit.Rows[0][2]).Trim();
                hddnVal.Value = Convert.ToString(Request.QueryString["id"]).Trim();
                ddlCalculationBasedOn.SelectedValue = Convert.ToString(dtEdit.Rows[0]["TCS_Calculation_Based_On_ID"]).Trim();

                hdnInterestLedgerID.Value = Convert.ToString(dtEdit.Rows[0]["Interest_Ledger"]).Trim();
                hdnLateFeeLedgerID.Value = Convert.ToString(dtEdit.Rows[0]["Late_Fee_Ledger"]).Trim();
                hdnOthersLedgerID.Value = Convert.ToString(dtEdit.Rows[0]["Others_Ledger"]).Trim();
                // Rev Mantis Issue 24161
                hdnAdvanceLedgerID.Value = Convert.ToString(dtEdit.Rows[0]["Advance_Ledger"]).Trim();
                // End of Rev Mantis Issue 24161

                ltsPurchase_code.Value = Convert.ToString(dtEdit.Rows[0]["Purchase_Ledger"]).Trim();

                ltsSales_code.Value = Convert.ToString(dtEdit.Rows[0]["Sales_Ledger"]).Trim();

                if (ddltype == "TDS")
                {
                    txtMainAccount_hidden.Value = Convert.ToString(dtEdit.Rows[0][3]).Trim();
                    txtSubAccount_hidden.Value = Convert.ToString(dtEdit.Rows[0][4]).Trim();



                    if (lstMainAccount.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstMainAccount.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0][3]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstMainAccount.SelectedIndex = indexer;
                    }

                    // Rev Mantis Issue 24161
                    if (lstAdvanceLedger.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstAdvanceLedger.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0]["Advance_Ledger"]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstAdvanceLedger.SelectedIndex = indexer;
                    }
                    // End of Rev Mantis Issue 24161

                    if (lstLateFeeLedger.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstLateFeeLedger.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0]["Late_Fee_Ledger"]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstLateFeeLedger.SelectedIndex = indexer;
                    }
                    /*Rev work start 12.05.2022 0024885: The Interest Ledger in TDS/TCS Master is not visible in the Edit Mode although it is updated with message*/
                    if (lstInterestLedger.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstInterestLedger.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0]["Interest_Ledger"]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstInterestLedger.SelectedIndex = indexer;
                    }
                    /*Rev work close 12.05.2022 0024885: The Interest Ledger in TDS/TCS Master is not visible in the Edit Mode although it is updated with message*/
                    if (lstOthersLedger.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstOthersLedger.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0]["Others_Ledger"]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstOthersLedger.SelectedIndex = indexer;
                    }


                    if (lstSales.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstSales.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0]["Sales_Ledger"]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstSales.SelectedIndex = indexer;
                    }


                    hdnSubACCode.Value = Convert.ToString(dtEdit.Rows[0][4]).Trim();
                }
                else
                {
                    if (lstPurchase.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstPurchase.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0]["Purchase_Ledger"]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstPurchase.SelectedIndex = indexer;
                    }


                    txtMainAccount_hidden.Value = Convert.ToString(dtEdit.Rows[0][4]).Trim();
                    txtSubAccount_hidden.Value = Convert.ToString(dtEdit.Rows[0][3]).Trim();

                    if (lstMainAccount.Items.Count > 0)
                    {
                        int indexer = 0;
                        foreach (ListItem li in lstMainAccount.Items)
                        {
                            string[] accountcode = (li.Value.Split('~'));
                            if (Convert.ToString(accountcode[0]) == Convert.ToString(dtEdit.Rows[0][4]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexer = indexer + 1;
                            }
                        }
                        lstMainAccount.SelectedIndex = indexer;
                    }

                    hdnSubACCode.Value = Convert.ToString(dtEdit.Rows[0][3]).Trim();
                }
            }
            string branch = "";
            //if (HttpContext.Current.Session["userbranchHierarchy"] != null)
            //{
            branch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //}
            int indexsubac = 0;
            if (Convert.ToString(dtEdit.Rows[0][7]).Trim() != "" && Convert.ToString(dtEdit.Rows[0][7]) != null)
            {
                DataTable dt = new DataTable();
                dt = objTdsTcsBL.PopulateSubAccountDropDownForTDSTCS(Convert.ToString(dtEdit.Rows[0][7]).Trim(), branch);
                lstSubAccount.DataTextField = "Contact_Name";
                lstSubAccount.DataValueField = "cnt_internalID";
                lstSubAccount.DataSource = dt;
                lstSubAccount.DataBind();


                lstTCSPayable.DataTextField = "Contact_Name";
                lstTCSPayable.DataValueField = "cnt_internalID";
                lstTCSPayable.DataSource = dt;
                lstTCSPayable.DataBind();


                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dtEdit.Rows[0][0]).Trim().ToUpper() == "TDS")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Convert.ToString(dr["cnt_internalID"]) == Convert.ToString(dtEdit.Rows[0][4]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexsubac = indexsubac + 1;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Convert.ToString(dr["cnt_internalID"]) == Convert.ToString(dtEdit.Rows[0][3]).Trim())
                            {
                                break;
                            }
                            else
                            {
                                indexsubac = indexsubac + 1;
                            }
                        }
                    }

                    //if (lstSubAccount.Items.Count > 0)
                    //{
                    //    lstSubAccount.SelectedIndex = indexsubac;
                    //}

                    if (lstTCSPayable.Items.Count > 0)
                    {
                        lstTCSPayable.SelectedIndex = indexsubac;
                    }

                }
            }

            //@MainAccountID
            if (ddlType.SelectedValue == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "JScript1", "<script language='JavaScript'>ForEdit();</script>");
            }
        }
        protected void gridTdsTcs_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["TDSTCSRates_Code"] = txtCode.Text;
        }

        protected void gridTdsTcs_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            RVal = "I~1";
        }
        protected void gridTdsTcs_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpInsertEna"] = RVal;
        }
        protected void gridTdsTcs_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {


            RVal = "I~2";
        }



        // .............................Code Commented and Added by Sam on 21122016 to use webmethod to check unique name. ..................................... 



        [WebMethod]
        public static bool CheckUniqueName(string TDSTCSId, string TDSTCSCode)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            //bool IsPresent = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (Convert.ToString(TDSTCSId) != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(TDSTCSCode, TDSTCSId, "MasterTDSTCS");

            }
            return status;
        }
        //dt = oGeneric.GetDataTable("SELECT COUNT(TDSTCS_Code) AS TDSTCS_Name FROM Master_TDSTCS WHERE sProducts_Code = '" + ProductName + "'");
        //else
        //{
        //    dt = oGeneric.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "' and sProducts_ID<>" + procode + "");
        //}
        //DataTable dt = oGeneric.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "'");

        //if (dt.Rows.Count > 0)
        //{
        //    if (Convert.ToInt32(dt.Rows[0]["sProducts_Name"]) > 0)
        //    {
        //        IsPresent = true;
        //    }
        //}



        // .............................Code Above Commented and Added by Sam on 21122016......................................

        [WebMethod]
        public static List<string> GetSubAccountList(string CombinedQuery)
        {
            string branch = "0";
            List<string> obj = new List<string>();
            DataTable DT = new DataTable();
            if (HttpContext.Current.Session["userbranchHierarchy"] != null)
            {
                branch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            TdsTcsBL objTdsTcsBL = new TdsTcsBL();



            string[] param = CombinedQuery.Split('$');
            string ProcedureName = Convert.ToString(param[0]);
            string InputName = Convert.ToString(param[1]);
            string InputType = Convert.ToString(param[2]);
            string[] InputValue = param[3].Split('~');
            string mainaccode = Convert.ToString(InputValue[2]);
            if (ProcedureName.Trim() != String.Empty && (mainaccode != ""))
            {
                DT = objTdsTcsBL.PopulateSubAccountDropDownForTDSTCS(mainaccode, branch);
                //DT = objEngine.SelectProcedureArr(ProcedureName, InputName, InputType, InputValue);
                if (DT.Rows.Count != 0)
                {
                    foreach (DataRow dr in DT.Rows)
                    {

                        obj.Add(Convert.ToString(dr["Contact_Name"]) + "|" + Convert.ToString(dr["cnt_internalID"]));
                    }
                }

            }


            return obj;
        }


        [WebMethod]
        public static object GetTdsDetailsList(string Type)
        {
            List<ddlContactPerson> listCotact = new List<ddlContactPerson>();
            ProcedureExecute proc = new ProcedureExecute("GetTdsDetails");
            proc.AddPara("@TYPE", Type);
            DataTable TdsTable = proc.GetTable();
            if (TdsTable != null && TdsTable.Rows.Count > 0)
            {
                DataView dvData = new DataView(TdsTable);
                listCotact = (from DataRow dr in dvData.ToTable().Rows
                              select new ddlContactPerson()
                              {
                                  Id = dr["Section_Code"].ToString(),
                                  Name = dr["Section_Description"].ToString(),
                              }).ToList();
            }

            return listCotact;
        }

        public class ddlContactPerson
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        protected void gridTdsTcs_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (gridTdsTcs.IsNewRowEditing)
            {
                if (e.Column.FieldName == "TDSTCSRates_DateFrom")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "TDSTCSRates_Rate")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "TDSTCSRates_Surcharge")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "TDSTCSRates_EduCess")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "TDSTCSRates_HgrEduCess")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "TDSTCSRates_ApplicableAmount")
                {
                    e.Editor.ReadOnly = false;
                }


            }

        }

        #region Export event

        public void bindexport(int Filter)
        {
            gridTdsTcs.Columns[9].Visible = false;
            //SchemaGrid.Columns[11].Visible = false;
            //SchemaGrid.Columns[12].Visible = false;
            string filename = "TDS/TCS Rates";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "TDS/TCS Rates";
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
        #endregion


        #region trash Code

        //GetSubAccountList
        ////Newly added for TDS/TCS sub account on 19.08.2015

        //    if (Request.QueryString["SubAccount"] == "1")
        //    {
        //        DataSet DS = new DataSet();
        //        string reqStr = Request.QueryString["letters"].ToString();
        //        string param = Request.QueryString["search_param"].ToString();
        //        string[] param1 = param.Split('~');
        //        if (Session["ExchangeSegmentID"] == null)
        //            Session["ExchangeSegmentID"] = "0";
        //        string Branch = null;
        //        //if (param1[1].ToString() == "N")
        //        //{
        //        //    Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();
        //        //}
        //        //else
        //        //    Branch = param1[1].ToString();

        //        Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();

        //        SqlCommand cmd = new SqlCommand("SubAccountSelect", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@CashBank_MainAccountID", param1[0].ToString());
        //        cmd.Parameters.AddWithValue("@clause", reqStr);
        //        cmd.Parameters.AddWithValue("@branch", Branch);
        //        cmd.Parameters.AddWithValue("@exchSegment", Session["ExchangeSegmentID"].ToString());
        //        cmd.Parameters.AddWithValue("@SegmentN", "");//"'" + SegmentName.ToString() + "'"
        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter Adap = new SqlDataAdapter();
        //        Adap.SelectCommand = cmd;
        //        Adap.Fill(DS);
        //        cmd.Dispose();
        //        GC.Collect();

        //        DT = DS.Tables[0];
        //        //string[,] Data1 = { { "@CashBank_MainAccountID", SqlDbType.VarChar.ToString(), param1[0].ToString() }, { "@clause", SqlDbType.VarChar.ToString(), reqStr }, { "@branch", SqlDbType.VarChar.ToString(), Branch }, { "@exchSegment", SqlDbType.VarChar.ToString(), Session["ExchangeSegmentID"].ToString() } };
        //        //DT = oDBEngine.GetDatatable_StoredProcedure("SubAccountSelect", Data1);
        //        if (DT.Rows.Count != 0)
        //        {
        //            for (int i = 0; i < DT.Rows.Count; i++)
        //            {
        //                Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
        //            }
        //        }
        //        else
        //        {
        //            if (DS.Tables[1].Rows.Count > 0)
        //                if (DS.Tables[1].Rows[0][0] == "1")
        //                    Response.Write("Suspended Client###Suspended Client|");
        //                else
        //                    Response.Write("No Record Found###No Record Found|");
        //            else
        //                Response.Write("No Record Found###No Record Found|");
        //        }
        //    }

        #endregion trash Code

        [WebMethod]
        public static string save(String Rates_Code, String Rates_DateFrom, String Rates_Rate, String Rates_Surcharge, String Rates_EduCess, String Rates_HgrEduCess, String Rates_ApplicableAmount, String Rates_ApplicableFor,
            String Rates_Rouding, String Rates_ID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                ProcedureExecute proc = new ProcedureExecute("MultiTdsTcsRate_Insert");
                proc.AddVarcharPara("@TDSTCSRates_Code", 80, Convert.ToString(Rates_Code));
                proc.AddVarcharPara("@TDSTCSRates_DateFrom", 10, Convert.ToString(Rates_DateFrom));
                proc.AddVarcharPara("@TDSTCSRates_Rate", 10, Convert.ToString(Rates_Rate));
                proc.AddVarcharPara("@TDSTCSRates_Surcharge", 10, Convert.ToString(Rates_Surcharge));
                proc.AddVarcharPara("@TDSTCSRates_EduCess", 10, Convert.ToString(Rates_EduCess));
                proc.AddVarcharPara("@TDSTCSRates_HgrEduCess", 10, Convert.ToString(Rates_HgrEduCess));
                proc.AddVarcharPara("@TDSTCSRates_Rouding", 10, Convert.ToString(Rates_Rouding));
                proc.AddVarcharPara("@TDSTCSRates_ApplicableAmount", 10, Convert.ToString(Rates_ApplicableAmount));
                proc.AddIntegerPara("@TDSTCSRates_CreateUser", user_id);
                proc.AddVarcharPara("@TDSTCSRates_ApplicableFor", 30, Convert.ToString(Rates_ApplicableFor));
                proc.AddVarcharPara("@TDSTCSRates_ID", 10, Rates_ID);
                DataTable dt = proc.GetTable();
                if (dt != null)
                {
                    output = dt.Rows[0]["msg"].ToString();
                }
                else
                {
                    output = "Please try after some time.";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        protected void LinqServerModeTdsTcsNew_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "TDSTCSRates_ID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(TdsSection.Value);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc.V_TDSTCSNewLists
                    where d.TDSTCSRates_Code == IsFilter
                    orderby d.TDSTCSRates_CreateDateTime descending
                    select d;
            e.QueryableSource = q;
        }

        protected void ASPxTdsTcsNew_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ASPxTdsTcsNew.DataBind();
        }

        [WebMethod]
        public static String DeleteData(string HiddenID)
        {
            String returnValue = "";
            TdsTcsBL objtdstcs = new TdsTcsBL();
            int i = objtdstcs.DeleteNewTDSTCS(HiddenID, "DELETE");
            if (i > 0)
            {
                returnValue = "Deleted sucessfylly.";
            }
            else
            {
                returnValue = "Delete Failed.";
            }
            return returnValue;
        }

        [WebMethod]
        public static NewTdsTcsDetails EditData(String HiddenID)
        {
            TdsTcsBL objtdstcs = new TdsTcsBL();
            NewTdsTcsDetails ret = new NewTdsTcsDetails();
            DataTable dt = objtdstcs.GetNewTDSTcsDetails(HiddenID);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.TDSTCSRates_Code = dt.Rows[0]["TDSTCSRates_Code"].ToString();
                ret.TDSTCSRates_ID = dt.Rows[0]["TDSTCSRates_ID"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["TDSTCSRates_DateFrom"].ToString()))
                {
                    ret.TDSTCSRates_DateFrom = Convert.ToDateTime(dt.Rows[0]["TDSTCSRates_DateFrom"]);
                }
                else
                {
                    ret.TDSTCSRates_DateFrom = DateTime.Now;
                }
                ret.TDSTCSRates_EduCess = dt.Rows[0]["TDSTCSRates_EduCess"].ToString();
                ret.TDSTCSRates_HgrEduCess = dt.Rows[0]["TDSTCSRates_HgrEduCess"].ToString();
                ret.TDSTCSRates_Rate = dt.Rows[0]["TDSTCSRates_Rate"].ToString();
                ret.TDSTCSRates_Rouding = dt.Rows[0]["TDSTCSRates_Rouding"].ToString();
                ret.TDSTCSRates_Surcharge = dt.Rows[0]["TDSTCSRates_Surcharge"].ToString();
                ret.TDSTCSRates_ApplicableFor = dt.Rows[0]["TDSTCSRates_ApplicableFor"].ToString();
                ret.TDSTCSRates_ApplicableAmount = dt.Rows[0]["TDSTCSRates_ApplicableAmount"].ToString();
            }
            return ret;
        }

        [WebMethod]
        public static object BindApplicableforTypeWise(String Type)
        {
            List<ddlContactPerson> listCotact = new List<ddlContactPerson>();
            ProcedureExecute proc = new ProcedureExecute("PRC_TDSMASTERLIST");
            proc.AddPara("@TYPE", Type);
            DataTable tdsMaster = proc.GetTable();
            if (tdsMaster != null && tdsMaster.Rows.Count > 0)
            {
                DataView dvData = new DataView(tdsMaster);
                listCotact = (from DataRow dr in dvData.ToTable().Rows
                              select new ddlContactPerson()
                              {
                                  Id = dr["ID"].ToString(),
                                  Name = dr["TYPE_NAME"].ToString(),
                              }).ToList();
            }

            return listCotact;
        }
    }
}