using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class management_CashBankEntryEdit : System.Web.UI.Page
    {
        #region LocalVariable
        BusinessLogicLayer.FAReportsOther objFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        SqlDataSource Obj_Sds;
        BusinessLogicLayer.DBEngine oDBEngine;
        string strCon;
        DataTable DtCurrentSegment;
        DataTable dtXML = new DataTable();
        string QS_TDate, QS_VNum, QS_CID, QS_SegID;
        GenericLogSystem oGenericLogSystem;
        #endregion
        #region Page Property
        public string PMode
        {
            get { return (string)Session["Mode"]; }
            set { Session["Mode"] = value; }
        }
        public int PCounter
        {
            get { return (int)ViewState["Counter"]; }
            set { ViewState["Counter"] = value; }
        }
        public string PCompanyID
        {
            get { return (string)ViewState["CompanyID"]; }
            set { ViewState["CompanyID"] = value; }
        }
        public string PCurrentSegment
        {
            get { return (string)ViewState["CurrentSegment"]; }
            set { ViewState["CurrentSegment"] = value; }
        }
        public string PUserID
        {
            get { return (string)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
        }
        public string PFinYear
        {
            get { return (string)ViewState["FinYear"]; }
            set { ViewState["FinYear"] = value; }
        }
        public string PBranchID
        {
            get { return (string)ViewState["BranchID"]; }
            set { ViewState["BranchID"] = value; }
        }
        public string PXMLPATH
        {
            get { return (string)Session["CashBankVoucherFile_XMLPATH"]; }
            set { Session["CashBankVoucherFile_XMLPATH"] = value; }
        }
        public decimal TotalPayment
        {
            get { return (decimal)ViewState["TotalPayment"]; }
            set { ViewState["TotalPayment"] = value; }
        }
        public decimal TotalRecieve
        {
            get { return (decimal)ViewState["TotalRecieve"]; }
            set { ViewState["TotalRecieve"] = value; }
        }
        public decimal TotalBankBalance
        {
            get { return (decimal)Session["TotalBankBalance"]; }
            set { Session["TotalBankBalance"] = value; }
        }
        public string ChoosenCurrency
        {
            get { return (string)Session["ChoosenCurrency"]; }
            set { Session["ChoosenCurrency"] = value; }
        }
        //This For Log Purpose
        public string LogID
        {
            get { return (string)ViewState["LogID"]; }
            set { ViewState["LogID"] = value; }
        }
        #endregion
        #region PageClass
        void Bind_Combo(ASPxComboBox Combo, SqlDataSource SDs, string strTextField, string strValueField, int SelectedIndex)
        {
            Combo.DataSource = SDs;
            Combo.TextField = strTextField;
            Combo.ValueField = strValueField;
            Combo.DataBind();
            Combo.SelectedIndex = SelectedIndex;

        }
        void Bind_Combo(ASPxComboBox Combo, DataSet Ds, string strTextField, string strValueField, int SelectedIndex)
        {
            if (Ds.Tables.Count > 0)
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    Combo.DataSource = Ds;
                    Combo.TextField = strTextField;
                    Combo.ValueField = strValueField;
                    Combo.DataBind();
                    Combo.SelectedIndex = SelectedIndex;
                }
            }

        }
        void Bind_Combo_WithSelectedValue(ASPxComboBox Combo, DataSet Ds, string strTextField, string strValueField, object strSelectedValue)
        {
            if (Ds.Tables.Count > 0)
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    Combo.DataSource = Ds;
                    Combo.TextField = strTextField;
                    Combo.ValueField = strValueField;
                    Combo.DataBind();
                    if (strSelectedValue.ToString().Trim() != "0")
                        Combo.Value = strSelectedValue;
                    else
                        Combo.SelectedIndex = 0;
                }
            }

        }
        DataSet Bind_Combo(string strcmd)
        {
            DataSet Ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlCommand com = new SqlCommand(strcmd, con))
                {
                    using (SqlDataAdapter Da = new SqlDataAdapter(com))
                    {
                        Da.Fill(Ds);
                    }
                }
            }
            return Ds;
        }
        DataSet Bind_Combo(string strcmd, string parametername, string parametervalue)
        {
            DataSet Ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlCommand com = new SqlCommand(strcmd, con))
                {
                    com.Parameters.AddWithValue(parametername, parametervalue);
                    using (SqlDataAdapter Da = new SqlDataAdapter(com))
                    {
                        Da.Fill(Ds);
                    }
                }
            }
            return Ds;
        }
        void BindGrid(ASPxGridView Grid)
        {
            Grid.DataSource = null;
            Grid.DataBind();
        }
        void BindGrid(ASPxGridView Grid, DataSet Ds)
        {
            if (Ds.Tables.Count > 0)
            {
                Grid.DataSource = Ds;
                Grid.DataBind();
            }
            else
            {
                Grid.DataSource = null;
                Grid.DataBind();
            }
        }
        void BindGrid(ASPxGridView Grid, DataTable Dt)
        {
            if (Dt.Rows.Count > 0)
            {
                Grid.DataSource = Dt;
                Grid.DataBind();
            }
            else
            {
                Grid.DataSource = null;
                Grid.DataBind();
            }
        }
        void BindGrid(ASPxGridView Grid, DataSet Ds, String WhichSort)
        {
            DataView TempDV = new DataView(Ds.Tables[0]);
            TempDV.Sort = "EntryDateTime " + WhichSort;
            Grid.DataSource = TempDV;
            Grid.DataBind();
        }
        void BindGrid(ASPxGridView Grid, DataTable Dt, String WhichSort)
        {
            DataView TempDV = new DataView(Dt);
            TempDV.Sort = "EntryDateTime " + WhichSort;
            Grid.DataSource = TempDV;
            Grid.DataBind();
        }
        DataSet GetDsUsingQuery(string strcmd)
        {
            DataSet Ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlCommand com = new SqlCommand(strcmd, con))
                {
                    using (SqlDataAdapter Da = new SqlDataAdapter(com))
                    {
                        Da.Fill(Ds);
                    }
                }
            }
            return Ds;
        }
        string CreateCBE_XMLFile(string IBRef, string VoucherType)
        {
            string ReturnValue = null;
            string CBEFile_XMLPATH = "../Documents/" + "CBE_" + IBRef;

            if (File.Exists(Server.MapPath(CBEFile_XMLPATH)))
            {
                DataSet Ds_CBE = new DataSet();
                Ds_CBE.ReadXml(Server.MapPath(CBEFile_XMLPATH));
                string UserID = Ds_CBE.Tables[0].Rows[0]["UserID"].ToString();
                string UserName = Ds_CBE.Tables[0].Rows[0]["UserName"].ToString();
                ViewState["CBE_FileAlreadyUsedBy"] = UserName + "~" + UserID;
            }
            else
            {
                ReturnValue = Fetch_CBE_DataSet(IBRef, VoucherType);
            }

            return ReturnValue;
        }
        string Fetch_CBE_DataSet(string IBRef, string VoucherType)
        {
            DataSet Ds_CBE;
            SqlDataAdapter Da_CBE;
            string ReturnValue = null;
            string CBEFile_XMLPATH = "../Documents/" + "CBE_" + IBRef;
            string TransactionType = string.Empty;
            TransactionType = (SCmb_Type.SelectedItem.Value.ToString() == "A" ? VoucherType : SCmb_Type.SelectedItem.Value.ToString());
            Ds_CBE = new DataSet();
            Ds_CBE = objFAReportsOther.Fetch_CBE_DataSet(
                Convert.ToString(Session["userid"]),
                Convert.ToString(IBRef),
                 Convert.ToString(TransactionType),
                 Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0]));
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    Ds_CBE = new DataSet();
            //    using (SqlCommand com = new SqlCommand("Fetch_CBE_DataSet", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
            //        com.Parameters.AddWithValue("@IBRef", IBRef);
            //        com.Parameters.AddWithValue("@TransactionType", SCmb_Type.SelectedItem.Value.ToString() == "A" ? VoucherType : SCmb_Type.SelectedItem.Value.ToString());
            //        com.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);

            //        using (Da_CBE = new SqlDataAdapter(com))
            //        {
            //            Ds_CBE.Clear();
            //            Da_CBE.Fill(Ds_CBE);
            //        }
            //    }
            //}
            if (Ds_CBE.Tables.Count > 0)
            {
                if (Ds_CBE.Tables[0].Rows.Count > 0)
                {
                    Ds_CBE.Tables[0].TableName = "DtCashBankVoucher";
                    Ds_CBE.WriteXml(Server.MapPath(CBEFile_XMLPATH));
                    string VoucherTypeValue = String.Empty, VoucherTypeText = String.Empty, BranchID = String.Empty, BranchName = String.Empty,
                        CashBankName = String.Empty, CashBankID = String.Empty, TransactionDate = String.Empty, Narration = String.Empty;
                    VoucherTypeValue = Ds_CBE.Tables[0].Rows[0]["VoucherType"].ToString();
                    if (VoucherTypeValue == "P") VoucherTypeText = "Payment";
                    if (VoucherTypeValue == "R") VoucherTypeText = "Receipt";
                    if (VoucherTypeValue == "C") VoucherTypeText = "Contra";
                    BranchID = Ds_CBE.Tables[0].Rows[0]["BranchID"].ToString();
                    BranchName = Ds_CBE.Tables[0].Rows[0]["SelectedBranchName"].ToString();
                    CashBankID = Ds_CBE.Tables[0].Rows[0]["CashBankID"].ToString();
                    CashBankName = Ds_CBE.Tables[0].Rows[0]["BanknameAndAcNumber"].ToString();
                    TransactionDate = Ds_CBE.Tables[0].Rows[0]["TransactionDate"].ToString();
                    Narration = Ds_CBE.Tables[0].Rows[0]["Narration"].ToString();
                    //DataTable vsegmentname = oDBEngine.GetDataTable("tbl_master_companyExchange", "isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as CompSegmentName", "exch_compID='" + QS_CID + "' and  exch_internalID=" + QS_SegID);
                    ReturnValue = VoucherTypeValue + "*" + VoucherTypeText + "*" + BranchID + "*" + BranchName + "*" + CashBankID + "*" + CashBankName + "*" + TransactionDate + "*" + Narration;
                    //This For Log Purpose
                    string strLogID = oGenericLogSystem.GetLogID();
                    oGenericLogSystem.CreateLog("Trans_CashBankVouchers", "CashBank_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                    oGenericLogSystem.CreateLog("Trans_CashBankDetail", "CashBankDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                }
            }
            return ReturnValue;
        }
        string Fetch_CBE_XMLFileData(string IBRef)
        {
            DataSet Ds_CBE = new DataSet();
            string CBEFile_XMLPATH = "../Documents/" + "CBE_" + IBRef;
            string ReturnValue = null;
            Ds_CBE.ReadXml(Server.MapPath(CBEFile_XMLPATH));
            string VoucherTypeValue = String.Empty, VoucherTypeText = String.Empty, BranchID = String.Empty, BranchName = String.Empty,
                        CashBankName = String.Empty, CashBankID = String.Empty, TransactionDate = String.Empty, Narration = String.Empty;
            if (Ds_CBE.Tables.Count > 0)
            {
                if (Ds_CBE.Tables[0].Rows.Count > 0)
                {
                    VoucherTypeValue = Ds_CBE.Tables[0].Rows[0]["VoucherType"].ToString();
                    if (VoucherTypeValue == "P") VoucherTypeText = "Payment";
                    if (VoucherTypeValue == "R") VoucherTypeText = "Receipt";
                    if (VoucherTypeValue == "C") VoucherTypeText = "Contra";
                    BranchID = Ds_CBE.Tables[0].Rows[0]["BranchID"].ToString();
                    BranchName = Ds_CBE.Tables[0].Rows[0]["SelectedBranchName"].ToString();
                    CashBankID = Ds_CBE.Tables[0].Rows[0]["CashBankID"].ToString();
                    CashBankName = Ds_CBE.Tables[0].Rows[0]["BanknameAndAcNumber"].ToString();
                    TransactionDate = Ds_CBE.Tables[0].Rows[0]["TransactionDate"].ToString();
                    Narration = Ds_CBE.Tables[0].Rows[0]["Narration"].ToString();
                    ReturnValue = VoucherTypeValue + "*" + VoucherTypeText + "*" + BranchID + "*" + BranchName + "*" + CashBankID + "*" + CashBankName + "*" + TransactionDate + "*" + Narration;
                }
            }
            return ReturnValue;
        }
        void FillSearchGrid(string PageNum)
        {
            DataSet DsSearch = new DataSet();
            string strMainQuery = null;
            string strMainQueryPrefix = null;
            string strMainQuerySuffix = null;
            string PageSize = GvCBSearch.SettingsPager.PageSize.ToString();
            if (Session["strMainQuery"] != null)
            {
                strMainQuery = Session["strMainQuery"].ToString();
                strMainQueryPrefix = "Select * from (";
                strMainQuerySuffix = @") as TempTable
                                    WHERE [Srl. No] BETWEEN (" + PageNum + "- 1) * " + PageSize + @" + 1 AND 
                                    " + PageNum + "* " + PageSize;
                strMainQuery = strMainQueryPrefix + strMainQuery + strMainQuerySuffix;
                DsSearch = GetDsUsingQuery(strMainQuery);
                if (DsSearch.Tables.Count > 0)
                {
                    if (DsSearch.Tables[0].Rows.Count > 0)
                    {
                        BindGrid(GvCBSearch, DsSearch);
                    }
                    else
                    {
                        GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "undefined";
                        Session["strMainQuery"] = null;
                    }
                }
                else
                {
                    GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "undefined";
                    Session["strMainQuery"] = null;
                }
            }
        }
        #endregion

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //ServerControlIDs OServerControlIDs = new ServerControlIDs();
            //OServerControlIDs.Emit(Page);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            strCon = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();
            oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            oGenericLogSystem = new GenericLogSystem();
            Page.ClientScript.RegisterStartupScript(GetType(), "JSc", "<script>PageLoad();</script>");
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            QS_TDate = Request.QueryString["date"] != null ? Request.QueryString["date"] : String.Empty;
            QS_CID = Request.QueryString["Compid"] != null ? Request.QueryString["Compid"] : String.Empty;
            QS_SegID = Request.QueryString["SegID"] != null ? Request.QueryString["SegID"] : String.Empty;
            QS_VNum = Request.QueryString["vNo"] != null ? Request.QueryString["vNo"] : String.Empty;
            if (!IsPostBack)
            {
                //Initialization
                if (Request.QueryString != null)
                {
                    string Where = null;
                    DataTable TempDt;
                    Converter objConverter = new Converter();
                    DataTable vsegmentname = oDBEngine.GetDataTable("tbl_master_companyExchange", "isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as CompSegmentName", "exch_compID='" + QS_CID + "' and  exch_internalID=" + QS_SegID);
                    bSegmentName.InnerText = "Segment : " + vsegmentname.Rows[0][0].ToString();
                    hdn_SegID_SegmentName.Value = QS_SegID + "~" + vsegmentname.Rows[0][0].ToString().Replace("-", " - ");

                    if (QS_TDate != String.Empty && QS_CID != String.Empty && QS_SegID != String.Empty && QS_VNum != String.Empty)
                    {
                        dtTDate.EditFormatString = objConverter.GetDateFormat("Date");
                        dtTDate.Date = Convert.ToDateTime(QS_TDate);
                        Where = " CashBank_TransactionDate='" + QS_TDate + "' and CashBank_VoucherNumber='" + QS_VNum + "' and CashBank_CompanyID='" + QS_CID + "' and CashBank_ExchangeSegmentID=" + QS_SegID;
                        TempDt = oDBEngine.GetDataTable("Trans_CashBankVouchers", "CashBank_VoucherNumber,CashBank_IBRef", Where);
                        if (TempDt.Rows.Count > 0)
                        {
                            Session["IBRef"] = TempDt.Rows[0]["CashBank_IBRef"].ToString();
                            Session["VoucherNumber"] = TempDt.Rows[0]["CashBank_VoucherNumber"].ToString();
                            PCurrentSegment = QS_SegID;
                            hdn_CurrentSegment.Value = QS_SegID;
                            PCompanyID = QS_CID;
                        }
                    }
                }
                hdn_Mode.Value = "Edit";
                TotalPayment = 0;
                TotalRecieve = 0;
                TotalBankBalance = 0;
                //End
                //Bind Branch
                Obj_Sds = new SqlDataSource();
                Obj_Sds.ConnectionString = strCon;
                ASPxComboBox Obj_ComboBox = (ASPxComboBox)PopUp_StartPage.FindControl("ComboBranch");
                Obj_Sds.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH";
                Bind_Combo(Obj_ComboBox, Obj_Sds, "BANKBRANCH_NAME", "BANKBRANCH_ID", 0);

                //This For Log Purpose
                ViewState["LogID"] = oGenericLogSystem.GetLogID();
                //End

                //Current Currency
                B_ChoosenCurrency.InnerText = "Currency : " + Session["ActiveCurrency"].ToString().Split('~')[1].Trim() + "[" +
                       Session["ActiveCurrency"].ToString().Split('~')[2].Trim() + "]";
                if (!CbpChoosenCurrency.IsCallback)
                {
                    if (Session["LocalCurrency"].ToString().Trim() != Session["TradeCurrency"].ToString().Trim())
                    {
                        B_ChoosenCurrency.Attributes.Add("onclick", "ChangeCurrency()");
                        B_ChoosenCurrency.Style.Add("cursor", "hand");
                    }
                }
            }
            if (Session["IBRef"] != null)
            {
                PXMLPATH = "../Documents/" + "CBE_" + Session["IBRef"].ToString();
            }
            OnLoadBindGrid();
        }
        protected void CmbClientBank_OnCallback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string command = e.Parameter.Split('~')[0];
            if (command == "ClientBankBind")
            {
                string SubAccountRefID = e.Parameter.Split('~')[1];
                string StrQuery = "Select * from (select A.* , MB.bnk_id,ltrim(rtrim(MB.bnk_bankName)) as bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,ltrim(rtrim(TCBD.cbd_accountNumber)) as cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_bankCode Union All Select 0,'',0,'','','','','','','Third Party Account','','') as temp order by temp.cbd_Accountcategory";
                DataSet Ds_CmbClientBank = Bind_Combo(StrQuery, "SubAccountCode", SubAccountRefID);
                ASPxCallbackPanel obj_CallBackPanel = (ASPxCallbackPanel)PopUp_InstrumentDetail.FindControl("InstrumentDetail_CallbackPanel");
                ASPxComboBox Obj_CmbClientBank = (ASPxComboBox)obj_CallBackPanel.FindControl("CmbClientBank");
                if (Obj_CmbClientBank.Items.Count > 0) Obj_CmbClientBank.Items.Clear();
                if (Ds_CmbClientBank.Tables.Count > 0)
                {
                    if (Ds_CmbClientBank.Tables[0].Rows.Count > 0)
                    {
                        Obj_CmbClientBank.DataSource = Ds_CmbClientBank;
                        Obj_CmbClientBank.DataBind();
                        CmbClientBank.JSProperties["cpSetIndexZero"] = "False";
                    }
                    else
                    {
                        Obj_CmbClientBank.DataSource = null;
                        Obj_CmbClientBank.DataBind();
                        CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                    }
                }
                else
                {
                    Obj_CmbClientBank.DataSource = null;
                    Obj_CmbClientBank.DataBind();
                    CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                }
            }
            if (command == "SetFocusOnInstType")
            {
                CmbClientBank.JSProperties["cpSetNextInstNo"] = null;
                DataSet DsSetNextInst = new DataSet();
                if (File.Exists(Server.MapPath(PXMLPATH)))
                {
                    DsSetNextInst.ReadXml(Server.MapPath(PXMLPATH));
                    if (DsSetNextInst.Tables[0].Rows.Count > 0)
                    {
                        int RowNumber = Convert.ToInt32(DsSetNextInst.Tables[0].Rows[DsSetNextInst.Tables[0].Rows.Count - 1]["RecordID"].ToString()) - 1;
                        string strZero = String.Empty;
                        if (DsSetNextInst.Tables[0].Rows[RowNumber]["VoucherType"].ToString() == "P" && DsSetNextInst.Tables[0].Rows[RowNumber]["InstrumentNumber"].ToString().Trim() != "E - Net".Trim() && DsSetNextInst.Tables[0].Rows[RowNumber]["InstrumentNumber"].ToString() != "")
                        {
                            if (DsSetNextInst.Tables[0].Rows[RowNumber]["InstrumentNumber"].ToString()[0] == '0')
                            {
                                foreach (char zero in DsSetNextInst.Tables[0].Rows[RowNumber]["InstrumentNumber"].ToString())
                                {
                                    if (zero == '0')
                                        strZero = strZero + zero;
                                    else
                                        break;
                                }

                            }
                            string finalInstNumber = null;
                            if (strZero.Length > 0)
                                finalInstNumber = strZero + Convert.ToString((Convert.ToInt32(DsSetNextInst.Tables[0].Rows[RowNumber]["InstrumentNumber"].ToString()) + 1));
                            else
                                finalInstNumber = (DsSetNextInst.Tables[0].Rows[RowNumber]["InstrumentNumber"].ToString().Trim() != String.Empty) ? Convert.ToString((Convert.ToInt32(DsSetNextInst.Tables[0].Rows[RowNumber]["InstrumentNumber"].ToString()) + 1)) : String.Empty;

                            CmbClientBank.JSProperties["cpSetNextInstNo"] = finalInstNumber;
                        }

                    }
                }
                if (hdn_SubLedgerType.Value.ToUpper() != "CUSTOMERS")
                {
                    CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                }
                else
                {
                    CmbClientBank.JSProperties["cpSetIndexZero"] = "False";
                }


            }
            if (command == "ClientBankBindWithSelectedValue")
            {
                string SubAccountRefID = e.Parameter.Split('~')[1];
                string SelectedValue = e.Parameter.Split('~')[2];
                string StrQuery = "Select * from (select A.* , MB.bnk_id,ltrim(rtrim(MB.bnk_bankName)) as bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,ltrim(rtrim(TCBD.cbd_accountNumber)) as cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_bankCode Union All Select 0,'',0,'','','','','','','Third Party Account','','') as temp order by temp.cbd_Accountcategory";
                DataSet Ds_CmbClientBank = Bind_Combo(StrQuery, "SubAccountCode", SubAccountRefID);
                ASPxCallbackPanel obj_CallBackPanel = (ASPxCallbackPanel)PopUp_InstrumentDetail.FindControl("InstrumentDetail_CallbackPanel");
                ASPxComboBox Obj_CmbClientBank = (ASPxComboBox)obj_CallBackPanel.FindControl("CmbClientBank");
                if (Obj_CmbClientBank.Items.Count > 0) Obj_CmbClientBank.Items.Clear();
                if (Ds_CmbClientBank.Tables.Count > 0)
                {
                    if (Ds_CmbClientBank.Tables[0].Rows.Count > 0)
                    {
                        Bind_Combo_WithSelectedValue(Obj_CmbClientBank, Ds_CmbClientBank, "bnk_bankName", "cbd_bankCode", SelectedValue);
                    }
                    else
                    {
                        Obj_CmbClientBank.DataSource = null;
                        Obj_CmbClientBank.DataBind();
                    }
                }
                else
                {
                    Obj_CmbClientBank.DataSource = null;
                    Obj_CmbClientBank.DataBind();
                }
            }



        }
        protected void CmbIssuingBank_Callback(object source, CallbackEventArgsBase e)
        {
            string command = e.Parameter.Split('~')[0];
            if (command == "ThirdPartySelect")
            {
                string TypedChar = e.Parameter.Split('~')[1];
                string StrQuery = "Select top 10 bnk_id,(isnull(bnk_bankName,'')+ '-'+ isnull(bnk_micrno,'') ) as BankName from tbl_master_Bank Where bnk_bankName like @Like_Char+'%'";
                DataSet Ds_CmbIssuingBank = Bind_Combo(StrQuery, "Like_Char", TypedChar);
                ASPxComboBox Obj_CmbIssuingBank = (ASPxComboBox)PopUp_InstrumentDetail.FindControl("CmbIssuingBank");
                Obj_CmbIssuingBank.Items.Clear();
                Obj_CmbIssuingBank.DataSource = Ds_CmbIssuingBank;
                Obj_CmbIssuingBank.ValueField = "bnk_id";
                Obj_CmbIssuingBank.TextField = "BankName";
                Obj_CmbIssuingBank.DataBind();
                CmbClientBank.JSProperties["cpSetIndexZero"] = "True";

            }
        }



        protected void txtNarration_TextChanged(object sender, EventArgs e)
        {

        }
        void AddData_ToGrid(string BranchID, string VoucherType, string DefaultBranch, string InstType)
        {
            DataSet DsAddXML = new DataSet();
            ASPxCallbackPanel obj_CallBackPanel = (ASPxCallbackPanel)PopUp_InstrumentDetail.FindControl("InstrumentDetail_CallbackPanel");
            ASPxComboBox ComboInstType = (ASPxComboBox)obj_CallBackPanel.FindControl("ComboInstType");
            ASPxTextBox txtInstNo = (ASPxTextBox)obj_CallBackPanel.FindControl("txtInstNo");
            ASPxDateEdit InstDate = (ASPxDateEdit)obj_CallBackPanel.FindControl("InstDate");
            ASPxComboBox CmbClientBank = (ASPxComboBox)obj_CallBackPanel.FindControl("CmbClientBank");
            TextBox txtIssuingBank = (TextBox)obj_CallBackPanel.FindControl("txtIssuingBank");
            ASPxTextBox txtPayment = (ASPxTextBox)obj_CallBackPanel.FindControl("txtPayment");
            ASPxTextBox txtRecieve = (ASPxTextBox)obj_CallBackPanel.FindControl("txtRecieve");
            ASPxTextBox txtAmount = (ASPxTextBox)obj_CallBackPanel.FindControl("txtAmount");
            TextBox txtWithFrom = (TextBox)obj_CallBackPanel.FindControl("txtWithFrom");
            TextBox txtDepositInto = (TextBox)obj_CallBackPanel.FindControl("txtDepositInto");
            TextBox txtAuthLetterRef = (TextBox)obj_CallBackPanel.FindControl("txtAuthLetterRef");
            TextBox txtLineNarration = (TextBox)obj_CallBackPanel.FindControl("txtNarration1");
            HiddenField txtWithFrom_hidden = (HiddenField)obj_CallBackPanel.FindControl("txtWithFrom_hidden");
            HiddenField txtDepositInto_hidden = (HiddenField)obj_CallBackPanel.FindControl("txtDepositInto_hidden");


            string strPaymentAmount = "", strReceiptAmount = "", strDraftIssueBankBranch = "", strClientBankID = "", strPayeeAccountID = "",
                strIsThirdParty = "", strThirdPartyReference = "", strContraAmount = "", strWithDrawFrom = "", strDepositInto = ""
                , strMainAccountID = "", strSubAccountID = "", strBranchID = "", strWithDrawFromName = "",
                strDepostitIntoName = "", strAmount = "", strDraftIssueBankBranchName = "";
            if (VoucherType == "R" && (InstType == "C" || InstType == "E"))
            {
                strPaymentAmount = "0.00";
                strReceiptAmount = txtRecieve.Text;
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
                strMainAccountID = txtMainAccount_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                if (BranchID != "NAB")
                {
                    if (hdn_SegID_SegmentName.Value.Split('~')[1].ToUpper() != "NSDL" && hdn_SegID_SegmentName.Value.Split('~')[1].ToUpper() != "CDSL")
                    {
                        if (CmbClientBank.SelectedItem.Value.ToString() != "0")
                        {
                            strDraftIssueBankBranch = "0";
                            strDraftIssueBankBranchName = String.Empty;
                            strThirdPartyReference = String.Empty;
                            strClientBankID = CmbClientBank.SelectedItem.Value.ToString();
                            strPayeeAccountID = "0";
                            strIsThirdParty = "N";
                        }
                        else
                        {
                            strDraftIssueBankBranch = txtIssuingBank_hidden.Value;
                            strDraftIssueBankBranchName = txtIssuingBank.Text;
                            strThirdPartyReference = txtAuthLetterRef.Text;
                            strClientBankID = "0";
                            strPayeeAccountID = "0";
                            strIsThirdParty = "Y";
                        }
                    }
                    else
                    {
                        strDraftIssueBankBranch = "0";
                        strDraftIssueBankBranchName = String.Empty;
                        strThirdPartyReference = String.Empty;
                        strClientBankID = "0";
                        strPayeeAccountID = "0";
                        strIsThirdParty = "N";
                    }
                }
                else
                {
                    strDraftIssueBankBranch = "0";
                    strDraftIssueBankBranchName = String.Empty;
                    strThirdPartyReference = String.Empty;
                    strClientBankID = "0";
                    strPayeeAccountID = "0";
                    strIsThirdParty = "N";
                }
            }
            else if (VoucherType == "R" && InstType == "D")
            {
                strMainAccountID = txtMainAccount_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                strPaymentAmount = "0.00";
                strReceiptAmount = txtRecieve.Text;
                strDraftIssueBankBranch = txtIssuingBank_hidden.Value;
                strDraftIssueBankBranchName = txtIssuingBank.Text;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
            }
            else if (VoucherType == "R" && InstType == "CH")
            {
                strPaymentAmount = "0.00";
                strReceiptAmount = txtRecieve.Text;
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
                strMainAccountID = txtMainAccount_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                strDraftIssueBankBranch = "0";
                strDraftIssueBankBranchName = String.Empty;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
            }
            else if (VoucherType == "P")
            {
                strMainAccountID = txtMainAccount_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                strPaymentAmount = txtPayment.Text;
                strReceiptAmount = "0.00";
                strDraftIssueBankBranch = String.Empty;
                strDraftIssueBankBranchName = String.Empty;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
                if (hdnAccountType.Value == "EXPENCES")
                {
                    strPayeeAccountID = CmbPayee.SelectedItem.Value.ToString();
                }
                else
                {
                    strPayeeAccountID = "0";
                }
            }
            else
            {
                strMainAccountID = String.Empty;
                strSubAccountID = String.Empty;
                strBranchID = DefaultBranch;
                strContraAmount = txtAmount.Text;
                strDraftIssueBankBranch = "0";
                strDraftIssueBankBranchName = String.Empty;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
                strWithDrawFrom = txtWithFrom_hidden.Value.Split('~')[0];
                strDepositInto = txtDepositInto_hidden.Value.Split('~')[0];
                strWithDrawFromName = txtWithFrom.Text;
                strDepostitIntoName = txtDepositInto.Text;
                strAmount = txtAmount.Text;
            }

            if (File.Exists(Server.MapPath(PXMLPATH)))
            {
                if (DsAddXML.Tables.Count > 0) { DsAddXML.Tables.Remove(DsAddXML.Tables[0]); DsAddXML.Clear(); }
                DsAddXML.ReadXml(Server.MapPath(PXMLPATH));
                if (DsAddXML.Tables[0].Rows.Count > 0)
                {
                    PCounter = Convert.ToInt32(DsAddXML.Tables[0].Rows[DsAddXML.Tables[0].Rows.Count - 1]["RecordID"].ToString()) + 1;
                }
                DataRow drXML = DsAddXML.Tables[0].NewRow();
                drXML[0] = PCounter;
                drXML[1] = strBranchID;
                drXML[2] = strMainAccountID;
                drXML[3] = strSubAccountID;
                drXML[4] = strPaymentAmount;
                drXML[5] = strReceiptAmount;
                drXML[6] = InstType != "CH" ? InstType : "0";
                drXML[7] = (InstType != "CH") ? txtInstNo.Text : String.Empty;
                drXML[8] = (InstDate.Value == null) || (InstType == "CH") ? "01-01-1900" : InstDate.Value.ToString() != "01-01-0100" ? Convert.ToDateTime(InstDate.Value.ToString()).ToString("MM/dd/yyyy HH:mm:ss") : "01-01-1900";
                drXML[9] = strDraftIssueBankBranch;
                drXML[10] = strClientBankID;
                drXML[11] = strPayeeAccountID;
                drXML[12] = strIsThirdParty;
                drXML[13] = strThirdPartyReference;
                drXML[14] = txtLineNarration.Text;
                drXML[15] = strWithDrawFrom;
                drXML[16] = strDepositInto;
                drXML[17] = ComboInstType.Text;
                drXML[18] = oDBEngine.GetDate();
                drXML[19] = txtSubAccount_hidden.Value != "" ? txtSubAccount.Text.Split('[')[0] : String.Empty;
                drXML[20] = txtMainAccount.Text.ToString();
                drXML[21] = hdn_Brch_NonBrch.Value;
                drXML[22] = strWithDrawFromName;
                drXML[23] = strDepostitIntoName;
                drXML[24] = strAmount;
                drXML[25] = hdnAccountType.Value;
                drXML[26] = hdn_SubLedgerType.Value;
                drXML[27] = strDraftIssueBankBranchName;
                drXML[28] = VoucherType;
                drXML[29] = (InstDate.Value == null) || (InstType == "CH") ? "01 Jan 1900" : InstDate.Value.ToString() != "01-01-0100" ? Convert.ToDateTime(InstDate.Value.ToString()).ToString("dd MMM yyyy") : "01 Jan 1900";
                if (VoucherType == "R")
                {
                    drXML[30] = (InstType != "D" && InstType != "CH") && hdn_SubLedgerType.Value == "CUSTOMERS" ? CmbClientBank.SelectedItem.Value.ToString() != "0" ? CmbClientBank.SelectedItem.Value : txtIssuingBank_hidden.Value.Split('~')[0] : (InstType != "D" && InstType != "CH") ? txtIssuingBank_hidden.Value.Trim() != String.Empty ? txtIssuingBank_hidden.Value.Split('~')[0] : "000000" : "000000";
                }
                else
                {
                    drXML[30] = "000000";
                }
                drXML[31] = txtBankAccounts.Text.Split('~')[0];
                drXML[32] = Session["UserID"].ToString();
                drXML[33] = String.Empty;
                drXML[34] = ComboBranch.SelectedItem.Text;
                drXML[35] = txtBankAccounts_hidden.Value;
                drXML[36] = dtTDate.Value;
                drXML[37] = txtNarration.Text;
                drXML[38] = String.Empty;
                drXML[39] = hdn_Mode.Value == "Entry" ? String.Empty : "Y";
                drXML[40] = String.Empty;
                drXML[41] = String.Empty;
                drXML[42] = String.Empty;
                drXML[43] = String.Empty;
                drXML[44] = String.Empty;
                drXML[45] = String.Empty;
                drXML[46] = ChoosenCurrency;
                DsAddXML.Tables[0].Rows.Add(drXML);
                DsAddXML.Tables[0].AcceptChanges();
                DsAddXML.Tables[0].WriteXml(Server.MapPath(PXMLPATH));
                DsAddXML.Dispose();
                txtSubAccount_hidden.Value = String.Empty;
                hdn_Brch_NonBrch.Value = String.Empty;
            }
            else
            {
                if (DsAddXML.Tables.Count > 0) { DsAddXML.Tables.Remove(DsAddXML.Tables[0]); DsAddXML.Clear(); }
                dtXML = DsAddXML.Tables.Add();
                dtXML.Columns.Add(new DataColumn("RecordID", typeof(int))); //0
                dtXML.Columns.Add(new DataColumn("BranchID", typeof(string)));//1
                dtXML.Columns.Add(new DataColumn("MainAccountID", typeof(string)));//2
                dtXML.Columns.Add(new DataColumn("SubAccountID", typeof(string)));//3
                dtXML.Columns.Add(new DataColumn("PaymentAmount", typeof(string)));//4
                dtXML.Columns.Add(new DataColumn("ReceiptAmount", typeof(string)));//5
                dtXML.Columns.Add(new DataColumn("InstrumentType", typeof(string)));//6
                dtXML.Columns.Add(new DataColumn("InstrumentNumber", typeof(string)));//7
                dtXML.Columns.Add(new DataColumn("InstrumentDate", typeof(string)));//8
                dtXML.Columns.Add(new DataColumn("DraftIssueBankBranch", typeof(string)));//9
                dtXML.Columns.Add(new DataColumn("ClientBankID", typeof(string)));//10
                dtXML.Columns.Add(new DataColumn("PayeeAccountID", typeof(string)));//11
                dtXML.Columns.Add(new DataColumn("IsThirdParty", typeof(string)));//12
                dtXML.Columns.Add(new DataColumn("ThirdPartyReference", typeof(string)));//13
                dtXML.Columns.Add(new DataColumn("LineNarration", typeof(string)));//14
                dtXML.Columns.Add(new DataColumn("WithDrawFrom", typeof(string)));//15
                dtXML.Columns.Add(new DataColumn("DepositInto", typeof(string)));//16
                dtXML.Columns.Add(new DataColumn("InstrumentTypeName", typeof(string)));//17
                dtXML.Columns.Add(new DataColumn("EntryDateTime", typeof(DateTime)));//18
                dtXML.Columns.Add(new DataColumn("SubAccountName", typeof(string)));//19
                dtXML.Columns.Add(new DataColumn("MainAccountName", typeof(string)));//20
                dtXML.Columns.Add(new DataColumn("BranchNonBranch", typeof(string)));//21
                dtXML.Columns.Add(new DataColumn("WithFromName", typeof(string)));//22
                dtXML.Columns.Add(new DataColumn("DepositIntoName", typeof(string)));//23
                dtXML.Columns.Add(new DataColumn("Amount", typeof(string)));//24
                dtXML.Columns.Add(new DataColumn("AccountType", typeof(string)));//25
                dtXML.Columns.Add(new DataColumn("SubLedgerType", typeof(string)));//26
                dtXML.Columns.Add(new DataColumn("DraftIssueBankBranchName", typeof(string)));//27
                dtXML.Columns.Add(new DataColumn("VoucherType", typeof(string)));//28
                dtXML.Columns.Add(new DataColumn("FormatedInstrumentDate", typeof(string)));//29
                dtXML.Columns.Add(new DataColumn("BanknameAndAcNumber", typeof(string)));//30
                dtXML.Columns.Add(new DataColumn("CashBankName", typeof(string)));//31
                dtXML.Columns.Add(new DataColumn("UserID", typeof(string)));//32
                dtXML.Columns.Add(new DataColumn("UserName", typeof(string)));//33
                dtXML.Columns.Add(new DataColumn("SelectedBranchName", typeof(string)));//34
                dtXML.Columns.Add(new DataColumn("CashBankID", typeof(string)));//35
                dtXML.Columns.Add(new DataColumn("TransactionDate", typeof(string)));//36
                dtXML.Columns.Add(new DataColumn("Narration", typeof(string)));//37
                dtXML.Columns.Add(new DataColumn("OldVoucherNumber", typeof(string)));//38 Editing Purpose
                dtXML.Columns.Add(new DataColumn("IsVoucherNumberChange", typeof(string)));//39 Editing Purpose
                dtXML.Columns.Add(new DataColumn("ValueDate", typeof(string)));//40 Editing Purpose
                dtXML.Columns.Add(new DataColumn("ExchangeSegmentID", typeof(string)));//41 Editing Purpose
                dtXML.Columns.Add(new DataColumn("SegmentName", typeof(string)));//42 Editing Purpose
                dtXML.Columns.Add(new DataColumn("RealCreateDateTime", typeof(string)));//43 Editing Purpose
                dtXML.Columns.Add(new DataColumn("RealCreateUser", typeof(string)));//44 Editing Purpose
                dtXML.Columns.Add(new DataColumn("BankStatementDate", typeof(string)));//45 Editing Purpose
                dtXML.Columns.Add(new DataColumn("ChoosenCurrency", typeof(string)));//46 To Remember Last ChoosenCurrency
                DataRow drXML = dtXML.NewRow();
                drXML[0] = 1;
                drXML[1] = strBranchID;
                drXML[2] = strMainAccountID;
                drXML[3] = strSubAccountID;
                drXML[4] = strPaymentAmount;
                drXML[5] = strReceiptAmount;
                drXML[6] = InstType != "CH" ? InstType : "0";
                drXML[7] = (InstType != "CH") ? txtInstNo.Text : String.Empty;
                drXML[8] = (InstDate.Value == null) || (InstType == "CH") ? "01-01-1900" : InstDate.Value.ToString() != "01-01-0100" ? Convert.ToDateTime(InstDate.Value.ToString()).ToString("MM/dd/yyyy HH:mm:ss") : "01-01-1900";
                drXML[9] = strDraftIssueBankBranch;
                drXML[10] = strClientBankID;
                drXML[11] = strPayeeAccountID;
                drXML[12] = strIsThirdParty;
                drXML[13] = strThirdPartyReference;
                drXML[14] = txtLineNarration.Text;
                drXML[15] = strWithDrawFrom;
                drXML[16] = strDepositInto;
                drXML[17] = ComboInstType.Text;
                drXML[18] = oDBEngine.GetDate();
                drXML[19] = txtSubAccount_hidden.Value != "" ? txtSubAccount.Text.Split('[')[0] : String.Empty;
                drXML[20] = txtMainAccount.Text.ToString();
                drXML[21] = hdn_Brch_NonBrch.Value;
                drXML[22] = strWithDrawFromName;
                drXML[23] = strDepostitIntoName;
                drXML[24] = strAmount;
                drXML[25] = hdnAccountType.Value;
                drXML[26] = hdn_SubLedgerType.Value;
                drXML[27] = strDraftIssueBankBranchName;
                drXML[28] = VoucherType;
                drXML[29] = (InstDate.Value == null) || (InstType == "CH") ? "01 Jan 1900" : InstDate.Value.ToString() != "01-01-0100" ? Convert.ToDateTime(InstDate.Value.ToString()).ToString("dd MMM yyyy") : "01 Jan 1900";
                if (VoucherType == "R")
                {
                    drXML[30] = (InstType != "D" && InstType != "CH") && hdn_SubLedgerType.Value == "CUSTOMERS" ? CmbClientBank.SelectedItem.Value.ToString() != "0" ? CmbClientBank.SelectedItem.Value : txtIssuingBank_hidden.Value.Split('~')[0] : (InstType != "D" && InstType != "CH") ? txtIssuingBank_hidden.Value.Trim() != String.Empty ? txtIssuingBank_hidden.Value.Split('~')[0] : "000000" : "000000";
                }
                else
                {
                    drXML[30] = "000000";
                }
                drXML[31] = txtBankAccounts.Text.Split('~')[0];
                drXML[32] = Session["UserID"].ToString();
                drXML[33] = String.Empty;
                drXML[34] = ComboBranch.SelectedItem.Text;
                drXML[35] = txtBankAccounts_hidden.Value;
                drXML[36] = dtTDate.Value;
                drXML[37] = txtNarration.Text;
                drXML[38] = String.Empty;
                drXML[39] = hdn_Mode.Value == "Entry" ? String.Empty : "Y";
                drXML[40] = String.Empty;
                drXML[41] = String.Empty;
                drXML[42] = String.Empty;
                drXML[43] = String.Empty;
                drXML[44] = String.Empty;
                drXML[45] = String.Empty;
                drXML[46] = ChoosenCurrency;
                dtXML.Rows.Add(drXML);
                dtXML.AcceptChanges();
                DsAddXML.Tables[0].TableName = "DtCashBankVoucher";
                DsAddXML.WriteXml(Server.MapPath(PXMLPATH));

            }
            BindGrid(GvAddRecordDisplay, DsAddXML, "DESC");
            DsAddXML.Dispose();
        }
        protected void GvAddRecordDisplay_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GvAddRecordDisplay.JSProperties["cpSuccessDiscard"] = "undefined";
            GvAddRecordDisplay.JSProperties["cpSubAccountChange"] = "undefined";
            GvAddRecordDisplay.JSProperties["cpMainAccountChange"] = "undefined";
            GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] = "undefined";
            GvAddRecordDisplay.JSProperties["cpAfterAddData_ToGride"] = "undefined";
            GvAddRecordDisplay.JSProperties["cpSaveSuccessOrFail"] = "undefined";
            GvAddRecordDisplay.JSProperties["cpSetValueOnLoad"] = null;
            GvAddRecordDisplay.JSProperties["cpClearHiddenField"] = null;
            GvAddRecordDisplay.JSProperties["cpExit"] = null;
            string strSplitCommand = e.Parameters.Split('~')[0];
            string strVoucherType = e.Parameters.Split('~')[2];
            if (strSplitCommand == "VerificationForAddRecord")
            {
                Boolean IsCashInHannd = txtBankAccounts_hidden.Value != String.Empty ? txtBankAccounts_hidden.Value.Split('~')[1].ToUpper() != "CASH" : false;
                if (txtInstNo.Text.Trim() != String.Empty && ComboInstType.Text != "E.Transfer" && IsCashInHannd)
                {
                    string strInstDuplicacy = e.Parameters.Split('~')[4];
                    DataTable DtInstDuplicacy = new DataTable();
                    oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    string QueryPart_Tables = null;
                    string QueryPart_Field = null;
                    string QueryPart_Where = null;
                    string QueryPart_OrderBy = null;
                    QueryPart_Tables = "Trans_CashBankDetail,Trans_CashBankVouchers";
                    QueryPart_Field = @"CashBankDetail_VoucherID,
                (Select MainAccount_Name from Master_MainAccount Where MainAccount_AccountCode=CashBankDetail_MainAccountID) as CashBankDetail_MainAccountID,
                (Select 
                Case 
                    When (Select Count(SubAccount_Name) from Master_SubAccount where 
                    SubAccount_MainAcReferenceID=
                    (Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_AccountCode=CashBankDetail_MainAccountID) 
                    and SubAccount_Code = CashBankDetail_SubAccountID)=1
                    Then (Select SubAccount_Name from Master_SubAccount where SubAccount_MainAcReferenceID=
                            (Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_AccountCode=CashBankDetail_MainAccountID) 
                            and SubAccount_Code=CashBankDetail_SubAccountID)
                    Else ''
                End) as CashBankDetail_SubAccountID,
                CashBankDetail_ReceiptAmount,CashBankDetail_PaymentAmount,
                CashBank_VoucherNumber,Convert(Varchar,CashBank_TransactionDate,106) as CashBank_TransactionDate
                ,(Select isnull((select exh_shortName from tbl_master_Exchange 
                where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) 
                as CompSegmentName from tbl_master_companyExchange WHERE exch_compID='COR0000002 ' 
                and  exch_internalID=CashBank_ExchangeSegmentID) as CashBank_ExchangeSegmentID";
                    QueryPart_OrderBy = "CashBank_TransactionDate";
                    if (strInstDuplicacy == "CLIENTBANK")
                    {
                        QueryPart_Where = "CashBankDetail_ClientBankID='" + CmbClientBank.SelectedItem.Value.ToString() + "' and CashBankDetail_InstrumentNumber='" + txtInstNo.Text.Trim() + @"' and CashBank_ID=CashBankDetail_VoucherID and CashBank_TransactionType='" + strVoucherType + "'";
                        DtInstDuplicacy = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where, QueryPart_OrderBy);
                    }
                    else if (strInstDuplicacy == "ISSUINGBANK")
                    {
                        QueryPart_Where = @"CashBankDetail_DraftIssueBankBranch='" + txtIssuingBank_hidden.Value + @"' and CashBankDetail_InstrumentNumber='" +
                        txtInstNo.Text.Trim() + @"'
                and CashBank_ID=CashBankDetail_VoucherID and CashBank_TransactionType='" + strVoucherType + "'";
                        DtInstDuplicacy = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where, QueryPart_OrderBy);
                    }
                    else if (strInstDuplicacy == "CASHBANK")
                    {
                        QueryPart_Where = @"CashBank_CashBankID='" + txtBankAccounts_hidden.Value.Split('~')[0] + @"' and CashBankDetail_InstrumentNumber='" +
                        txtInstNo.Text.Trim() + @"'
                and CashBank_ID=CashBankDetail_VoucherID and CashBank_TransactionType='" + strVoucherType + "'";
                        DtInstDuplicacy = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where, QueryPart_OrderBy);
                    }
                    else if (strInstDuplicacy == "NonBranch")
                    {
                        QueryPart_Where = @"CashBankDetail_ClientBankID=0 and CashBankDetail_DraftIssueBankBranch=0 and CashBankDetail_InstrumentNumber='" +
                        txtInstNo.Text.Trim() + @"'
                and CashBank_ID=CashBankDetail_VoucherID and CashBank_TransactionType='" + strVoucherType + "'";
                        DtInstDuplicacy = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where, QueryPart_OrderBy);
                    }
                    else
                    {
                        QueryPart_Where = @"CashBankDetail_DraftIssueBankBranch='" + txtWithFrom_hidden.Value.Split('~')[0] + @"' and CashBankDetail_InstrumentNumber='" +
                        txtInstNo.Text.Trim() + @"'
                and CashBank_ID=CashBankDetail_VoucherID and CashBank_TransactionType='" + strVoucherType + "'";
                        DtInstDuplicacy = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where, QueryPart_OrderBy);
                    }

                    if (DtInstDuplicacy.Rows.Count > 0)
                    {
                        GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] = "Instrument Number Already Exist.\n";
                        int count = 0;
                        foreach (DataRow Rows in DtInstDuplicacy.Rows)
                        {
                            if (strVoucherType == "R")
                            {
                                string strTemp = (++count) + ". MainAccountID : " + Rows["CashBankDetail_MainAccountID"].ToString().Trim()
                                + " || SubAccountID : " + Rows["CashBankDetail_SubAccountID"].ToString().Trim()
                                + " || ReceiptAmount : " + Rows["CashBankDetail_ReceiptAmount"].ToString().Trim()
                                + " || TransactionDate : " + Convert.ToDateTime(Rows["CashBank_TransactionDate"]).ToString("dd-MMM-yyyy")
                                + " || VoucherNumber : " + Rows["CashBank_VoucherNumber"].ToString().Trim()
                                + " || Segment : " + Rows["CashBank_ExchangeSegmentID"].ToString().Trim()
                                + "\n\n";
                                GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] = GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] + strTemp;
                            }
                            else if (strVoucherType == "P")
                            {
                                string strTemp = (++count) + ". MainAccountID : " + Rows["CashBankDetail_MainAccountID"].ToString().Trim()
                                + " || SubAccountID : " + Rows["CashBankDetail_SubAccountID"].ToString().Trim()
                                + " || PayementAmount : " + Rows["CashBankDetail_PaymentAmount"].ToString().Trim()
                                + " || TransactionDate : " + Convert.ToDateTime(Rows["CashBank_TransactionDate"]).ToString("dd-MMM-yyyy")
                                + " || VoucherNumber : " + Rows["CashBank_VoucherNumber"].ToString().Trim()
                                + " || Segment : " + Rows["CashBank_ExchangeSegmentID"].ToString().Trim()
                                + "\n\n";
                                GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] = GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] + strTemp;
                            }
                            else
                            {
                                string strTemp = (++count) + ". MainAccountID : " + Rows["CashBankDetail_MainAccountID"].ToString().Trim()
                                + " || SubAccountID : " + Rows["CashBankDetail_SubAccountID"].ToString().Trim()
                                + " || ReceiptAmount : " + Rows["CashBankDetail_ReceiptAmount"].ToString().Trim()
                                + " || PayementAmount : " + Rows["CashBankDetail_PaymentAmount"].ToString().Trim()
                                + " || TransactionDate : " + Convert.ToDateTime(Rows["CashBank_TransactionDate"]).ToString("dd-MMM-yyyy")
                                + " || VoucherNumber : " + Rows["CashBank_VoucherNumber"].ToString().Trim()
                                + " || Segment : " + Rows["CashBank_ExchangeSegmentID"].ToString().Trim()
                                + "\n\n";
                                GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] = GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] + strTemp;
                            }
                        }
                    }
                    else
                    {
                        GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] = "undefined~AddRecord";
                    }
                }
                else
                {
                    GvAddRecordDisplay.JSProperties["cpInstDuplicateRecord"] = "undefined~AddRecord";
                }
                GvAddRecordDisplay.JSProperties["cpSuccessDiscard"] = "undefined";
            }
            if (strSplitCommand == "Add")
            {
                string strSplitBranch = e.Parameters.Split('~')[1];
                string strDefalutBranch = e.Parameters.Split('~')[3];
                string strInstType = e.Parameters.Split('~')[5];
                ASPxCallbackPanel obj_CallBackPanel = (ASPxCallbackPanel)PopUp_InstrumentDetail.FindControl("InstrumentDetail_CallbackPanel");
                ASPxTextBox txtRecieve = (ASPxTextBox)obj_CallBackPanel.FindControl("txtRecieve");
                ASPxTextBox txtPayment = (ASPxTextBox)obj_CallBackPanel.FindControl("txtPayment");
                AddData_ToGrid(strSplitBranch, strVoucherType, strDefalutBranch, strInstType);
                if (strVoucherType == "R")
                {
                    GvAddRecordDisplay.JSProperties["cpAfterAddData_ToGride"] = "True~R~" + txtRecieve.Text;
                }
                if (strVoucherType == "P")
                {
                    GvAddRecordDisplay.JSProperties["cpAfterAddData_ToGride"] = "True~P~" + txtPayment.Text;
                }
            }
            if (strSplitCommand == "ShowHideColumn")
            {
                GvAddRecordDisplay.Columns[0].Visible = true;
                GvAddRecordDisplay.Columns[1].Visible = true;
                GvAddRecordDisplay.Columns[2].Visible = true;
                GvAddRecordDisplay.Columns[3].Visible = true;
                GvAddRecordDisplay.Columns[7].Visible = true;
                GvAddRecordDisplay.Columns[8].Visible = true;
                GvAddRecordDisplay.Columns[9].Visible = true;


                if (strVoucherType == "R")
                {
                    GvAddRecordDisplay.Columns[7].Visible = false; //Payment
                    GvAddRecordDisplay.Columns[1].Visible = false; //WithDraw
                    GvAddRecordDisplay.Columns[3].Visible = false; //Deposit
                    GvAddRecordDisplay.Columns[9].Visible = false; //Amount
                }
                else if (strVoucherType == "P")
                {
                    GvAddRecordDisplay.Columns[8].Visible = false; //Reiceve
                    GvAddRecordDisplay.Columns[1].Visible = false; //WithDraw
                    GvAddRecordDisplay.Columns[3].Visible = false; //Deposit
                    GvAddRecordDisplay.Columns[9].Visible = false; //Amount
                }
                else
                {
                    GvAddRecordDisplay.Columns[0].Visible = false; //MainAc
                    GvAddRecordDisplay.Columns[2].Visible = false; //SubAc
                    GvAddRecordDisplay.Columns[7].Visible = false; //Payment
                    GvAddRecordDisplay.Columns[8].Visible = false; //Recieve
                }
                OnLoadBindGrid();
            }
            if (strSplitCommand == "Discard")
            {
                GvAddRecordDisplay.JSProperties["cpSuccessDiscard"] = Discard_All();
            }
            if (strSplitCommand == "MainAccountChange")
            {
                TextBox txtSubAccountE = (TextBox)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE");
                HiddenField txtMainAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtMainAccountE_hidden");
                HiddenField txtSubAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE_hidden");
                DataSet DsCheckMainAccountChange = new DataSet();
                int rowindex = GvAddRecordDisplay.EditingRowVisibleIndex;
                if (rowindex != -1)
                {
                    string KeyValue = GvAddRecordDisplay.GetRowValues(rowindex, "RecordID").ToString();
                    if (File.Exists(Server.MapPath(PXMLPATH)))
                    {
                        if (DsCheckMainAccountChange.Tables.Count > 0) { DsCheckMainAccountChange.Tables.Remove(DsCheckMainAccountChange.Tables[0]); DsCheckMainAccountChange.Clear(); }
                        DsCheckMainAccountChange.ReadXml(Server.MapPath(PXMLPATH));
                        DsCheckMainAccountChange.Tables[0].PrimaryKey = new DataColumn[] { DsCheckMainAccountChange.Tables[0].Columns["RecordID"] };
                        DataRow row = DsCheckMainAccountChange.Tables[0].Rows.Find(KeyValue);
                        if (row["MainAccountID"].ToString().Split('~')[0] != txtMainAccountE_hidden.Value.Split('~')[0])
                        {
                            txtSubAccountE.Text = String.Empty;
                            GvAddRecordDisplay.JSProperties["cpMainAccountChange"] = txtMainAccountE_hidden.Value;
                            row["IsVoucherNumberChange"] = "Y";
                            DsCheckMainAccountChange.AcceptChanges();
                        }
                        else
                        {
                            GvAddRecordDisplay.JSProperties["cpMainAccountChange"] = "undefined";
                        }
                    }
                }
            }
            if (strSplitCommand == "SubAccountChange")
            {
                TextBox txtSubAccountE = (TextBox)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE");
                HiddenField txtMainAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtMainAccountE_hidden");
                HiddenField txtSubAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE_hidden");
                DataSet DsCheckMainAccountChange = new DataSet();
                int rowindex = GvAddRecordDisplay.EditingRowVisibleIndex;
                if (rowindex != -1)
                {
                    string KeyValue = GvAddRecordDisplay.GetRowValues(rowindex, "RecordID").ToString();
                    if (File.Exists(Server.MapPath(PXMLPATH)))
                    {
                        if (DsCheckMainAccountChange.Tables.Count > 0) { DsCheckMainAccountChange.Tables.Remove(DsCheckMainAccountChange.Tables[0]); DsCheckMainAccountChange.Clear(); }
                        DsCheckMainAccountChange.ReadXml(Server.MapPath(PXMLPATH));
                        DsCheckMainAccountChange.Tables[0].PrimaryKey = new DataColumn[] { DsCheckMainAccountChange.Tables[0].Columns["RecordID"] };
                        DataRow row = DsCheckMainAccountChange.Tables[0].Rows.Find(KeyValue);
                        if (row["MainAccountID"].ToString().Split('~')[0] != txtMainAccountE_hidden.Value.Split('~')[0] && row["SubAccountID"].ToString().Split('~')[0] == txtSubAccountE_hidden.Value.Split('~')[0] || row["SubAccountID"].ToString().Split('~')[0] != txtSubAccountE_hidden.Value.Split('~')[0])
                        {
                            GvAddRecordDisplay.JSProperties["cpSubAccountChange"] = txtSubAccountE_hidden.Value.Split('~')[0];
                            row["IsVoucherNumberChange"] = "Y";
                            DsCheckMainAccountChange.AcceptChanges();
                        }
                        else
                        {
                            GvAddRecordDisplay.JSProperties["cpSubAccountChange"] = "undefined";
                        }
                    }
                }
            }
            if (strSplitCommand == "CancelEdit")
            {
                GvAddRecordDisplay.CancelEdit();
            }
            if (strSplitCommand == "Save")
            {
                GvAddRecordDisplay.JSProperties["cpSaveSuccessOrFail"] = Save_Records();
            }
            if (strSplitCommand == "ClearSession")
            {
                Page_RefreshAllSessionOrHiddenField();
                GvAddRecordDisplay.JSProperties["cpClearHiddenField"] = "ClearAll~" + e.Parameters.Split('~')[1];
            }
            if (strSplitCommand == "Exit")
            {
                string IsSave = e.Parameters.Split('~')[1];
                if (IsSave == "WithOutSave")
                {
                    string CBEFile_XMLPATH = "../Documents/" + "CBE_" + Session["IBRef"];
                    if (File.Exists(Server.MapPath(CBEFile_XMLPATH)))
                    {
                        File.Delete(Server.MapPath(CBEFile_XMLPATH));
                        //This For Log Purpose
                        string IBRef = Session["IBRef"].ToString();
                        string strLogID = ViewState["LogID"].ToString();
                        oGenericLogSystem.CreateLog("Trans_CashBankVouchers", "CashBank_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                        oGenericLogSystem.CreateLog("Trans_CashBankDetail", "CashBankDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                    }
                }
                GvAddRecordDisplay.JSProperties["cpExit"] = "Exit";
            }
            if (strSplitCommand == "PCB_DeleteBtnOkE")
            {
                int RowUpdated = 0;
                string IBRef = null;
                if (Session["IBRef"] != null && Session["VoucherNumber"] != null)
                {
                    IBRef = Session["IBRef"].ToString();
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                    oGenericLogSystem.CreateLog("Trans_CashBankVouchers", "CashBank_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleting, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                    oGenericLogSystem.CreateLog("Trans_CashBankDetail", "CashBankDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleting, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    {
                        using (SqlCommand com = new SqlCommand("Delete_CB", con))
                        {
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("@IBRef", IBRef);
                            con.Open();
                            RowUpdated = com.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    if (RowUpdated > 0)
                    {
                        GvAddRecordDisplay.JSProperties["cpCBDelete"] = "Successfully Deleted";
                        //This For Log Purpose
                        oGenericLogSystem.CreateLog("Trans_CashBankVouchers", "CashBank_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                        oGenericLogSystem.CreateLog("Trans_CashBankDetail", "CashBankDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                    }
                    else
                    {
                        GvAddRecordDisplay.JSProperties["cpCBDelete"] = "Problem in Deleting.Sry for Inconvenience";
                        //This For Log Purpose
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                    }
                }
            }
        }
        public void OnLoadBindGrid()
        {
            DataSet DsOnLoad = new DataSet();
            if (File.Exists(Server.MapPath(PXMLPATH)))
            {
                if (DsOnLoad.Tables.Count > 0) { DsOnLoad.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoad.Clear(); }
                DsOnLoad.ReadXml(Server.MapPath(PXMLPATH));
                if (DsOnLoad.Tables.Count > 0)
                {
                    BindGrid(GvAddRecordDisplay, DsOnLoad, "DESC");
                    if (DsOnLoad.Tables[0].Rows.Count > 0)
                    {
                        int LastRow;
                        LastRow = Convert.ToInt32(DsOnLoad.Tables[0].Rows.Count - 1);
                        string strBankAccounts = DsOnLoad.Tables[0].Rows[LastRow]["CashBankName"].ToString();
                        string strBankAccounts_hidden = DsOnLoad.Tables[0].Rows[LastRow]["CashBankID"].ToString();
                        string strMainNarration = DsOnLoad.Tables[0].Rows[LastRow]["Narration"].ToString();
                        string TransactionDate = DsOnLoad.Tables[0].Rows[LastRow]["TransactionDate"].ToString();
                        //Set Last Currency Choosen
                        ChoosenCurrency = DsOnLoad.Tables[0].Rows[LastRow]["ChoosenCurrency"].ToString();
                        GvAddRecordDisplay.JSProperties["cpSetValueOnLoad"] = strBankAccounts + '*' + strBankAccounts_hidden + '*' + strMainNarration + '*' + Convert.ToDateTime(TransactionDate).ToString("MM-dd-yyyy") + '*' + ChoosenCurrency.Trim();
                    }

                }
            }
            else
            {
                BindGrid(GvAddRecordDisplay);
                ChoosenCurrency = Session["ActiveCurrency"].ToString();
            }
            if (Session["strSearchByMainQuery"] != null)
            {

                Session["strMainQuery"] = null;
                Session["strSearchBy"] = null;
                Session["strSearchByMainQuery"] = null;
                BindGrid(GvCBSearch);

            }
            DsOnLoad.Dispose();
        }
        protected void GvAddRecordDisplay_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DataSet DsRowDeleteXML = new DataSet();
            string MainAccountID = null;
            if (File.Exists(Server.MapPath(PXMLPATH)))
            {
                if (DsRowDeleteXML.Tables.Count > 0) { DsRowDeleteXML.Tables.Remove(DsRowDeleteXML.Tables[0]); DsRowDeleteXML.Clear(); }
                DsRowDeleteXML.ReadXml(Server.MapPath(PXMLPATH));
                DsRowDeleteXML.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXML.Tables[0].Columns["RecordID"] };
                DataRow row = DsRowDeleteXML.Tables[0].Rows.Find(e.Keys["RecordID"]);
                MainAccountID = row["CashBankID"].ToString();
                DsRowDeleteXML.Tables[0].Rows.Remove(row);
                e.Cancel = true;
                BindGrid(GvAddRecordDisplay, DsRowDeleteXML, "DESC");
                try
                {
                    if (File.Exists(Server.MapPath(PXMLPATH)))
                    {
                        File.Delete(Server.MapPath(PXMLPATH));
                        if (DsRowDeleteXML.Tables.Count > 0)
                        {
                            if (DsRowDeleteXML.Tables[0].Rows.Count > 0)
                            {
                                DsRowDeleteXML.WriteXml(Server.MapPath(PXMLPATH));
                            }
                        }
                    }
                }
                catch
                {
                    DsRowDeleteXML.Dispose();
                }
            }
            GvAddRecordDisplay.JSProperties["cpAfterRowDeleted"] = "AfterRowDeleted~" + MainAccountID.Split('~')[0];
            DsRowDeleteXML.Dispose();
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
        string Discard_All()
        {
            if (File.Exists(Server.MapPath(PXMLPATH)))
            {
                try
                {
                    File.Delete(Server.MapPath(PXMLPATH));
                    File.Delete(Server.MapPath(PXMLPATH));
                }
                catch
                {
                    File.Delete(Server.MapPath(PXMLPATH));
                    File.Delete(Server.MapPath(PXMLPATH));
                }
                BindGrid(GvAddRecordDisplay);
                return "SuccessDiscard";
            }
            else if (File.Exists(Server.MapPath(PXMLPATH)) == false)
            {
                return "NoRecord";
            }
            return "Problem";
        }

        protected void InstrumentDetail_CallbackPanel_Callback(object source, CallbackEventArgsBase e)
        {
            DataSet DsCallBackPaneylXML = new DataSet();
            string AccountType = null, CashBankType = null, InstType = null, KeyValue = null;
            string DefaultBranch = null, strSubAccountID = null, strMainAccountID = null;
            string SubLedgerType = null, SubAccountID = null, SubAccountBranch = null;
            string SetValuesParameter = null;//InstType~InstNo~InstDate~CustBank~IssueBank~AuthLetterRef~Payee~LineNarration~Payment~Recieve~WithDrawFrom~DepositInto~Amount~IssueBankName~WithdrawName~DepositeintoName
            DataRow row = null;
            int TotalParameter = 0;
            string strSplitCommand = e.Parameter.Split('~')[0];
            string strVoucherType = e.Parameter.Split('~')[1];
            int rowindex = GvAddRecordDisplay.EditingRowVisibleIndex;
            if (rowindex != -1)
            {
                KeyValue = GvAddRecordDisplay.GetRowValues(rowindex, "RecordID").ToString();
                DsCallBackPaneylXML.ReadXml(Server.MapPath(PXMLPATH));
                DsCallBackPaneylXML.Tables[0].PrimaryKey = new DataColumn[] { DsCallBackPaneylXML.Tables[0].Columns["RecordID"] };
                row = DsCallBackPaneylXML.Tables[0].Rows.Find(KeyValue);
                HiddenField txtMainAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtMainAccountE_hidden");
                HiddenField txtSubAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE_hidden");
                if (txtMainAccountE_hidden.Value == "No Record Found" || txtSubAccountE_hidden.Value == "No Record Found")
                {
                    return;
                }
                if (txtMainAccountE_hidden.Value == String.Empty || txtSubAccountE_hidden.Value == String.Empty)
                {
                    hdnType.Value = row["VoucherType"].ToString();
                    strVoucherType = row["VoucherType"].ToString();
                    if (txtMainAccountE_hidden.Value.Trim() == String.Empty)
                    {
                        strMainAccountID = row["MainAccountID"].ToString();
                        txtMainAccountE_hidden.Value = row["MainAccountID"].ToString();
                        hdnAccountType.Value = row["AccountType"].ToString();
                        hdn_SubLedgerType.Value = row["SubLedgerType"].ToString();
                        hdn_MainAcc_Type.Value = row["SubLedgerType"].ToString().ToUpper() == "NONE" ? "None" : String.Empty;
                    }
                    else
                    {
                        strMainAccountID = txtMainAccountE_hidden.Value.Split('~')[0];
                        hdnAccountType.Value = txtMainAccountE_hidden.Value.Split('~')[3];
                        hdn_SubLedgerType.Value = txtMainAccountE_hidden.Value.Split('~')[1];
                        hdn_MainAcc_Type.Value = txtMainAccountE_hidden.Value.Split('~')[1].ToUpper() == "NONE" ? "None" : String.Empty;
                    }
                    if (hdn_SubLedgerType.Value.ToUpper() != "NONE")
                    {
                        if (txtSubAccountE_hidden.Value.Trim() == String.Empty)
                        {
                            strSubAccountID = row["SubAccountID"].ToString();
                            hdn_Brch_NonBrchE.Value = row["BranchNonBranch"].ToString();
                            SubAccountBranch = row["BranchNonBranch"].ToString();
                            DefaultBranch = row["BranchNonBranch"].ToString() != "NAB" ? row["BranchNonBranch"].ToString() : row["BranchID"].ToString();
                            hdnDefaultBranch.Value = DefaultBranch;
                            txtSubAccountE_hidden.Value = row["SubAccountID"].ToString();
                        }
                        else
                        {
                            hdn_Brch_NonBrchE.Value = txtSubAccountE_hidden.Value.Split('~')[1];
                            SubAccountBranch = txtSubAccountE_hidden.Value.Split('~')[1];
                            DefaultBranch = row["BranchID"].ToString();
                            hdnDefaultBranch.Value = DefaultBranch;
                            strSubAccountID = txtSubAccountE_hidden.Value.Split('~')[1] != "NAB" ? txtSubAccountE_hidden.Value.Split('~')[0] + "~" + txtSubAccountE_hidden.Value.Split('~')[1] + "~" + txtSubAccountE_hidden.Value.Split('~')[2] : txtSubAccountE_hidden.Value.Split('~')[0];
                        }
                    }
                    else
                    {
                        hdn_Brch_NonBrchE.Value = "NAB";
                        SubAccountBranch = "NAB";
                        if (hdn_Mode.Value == "Entry")
                        {
                            DefaultBranch = ComboBranch.SelectedItem.Value.ToString();
                            hdnDefaultBranch.Value = DefaultBranch;
                        }
                        else
                        {
                            DefaultBranch = row["BranchID"].ToString();
                            hdnDefaultBranch.Value = row["BranchID"].ToString();
                        }
                    }
                    InstrumentDetail_CallbackPanel.JSProperties["cpSetHdnFldValueOnEditDetail"] = hdnType.Value + '~'
                        + SubAccountBranch + '~' + hdnAccountType.Value
                        + '~' + hdn_SubLedgerType.Value + '~' + hdn_MainAcc_Type.Value
                        + '~' + txtSubAccountE_hidden.Value + '~' + DefaultBranch;
                }
                else
                {
                    if (hdn_Mode.Value == "Edit")
                    {
                        hdnType.Value = row["VoucherType"].ToString();
                        strVoucherType = row["VoucherType"].ToString();
                        hdnDefaultBranch.Value = row["BranchNonBranch"].ToString() != "NAB" ? row["BranchNonBranch"].ToString() : row["BranchID"].ToString();
                    }
                    strMainAccountID = txtMainAccountE_hidden.Value.Split('~')[0];
                    strSubAccountID = txtSubAccountE_hidden.Value.Split('~')[0] + "~" + txtSubAccountE_hidden.Value.Split('~')[1] + "~" + txtSubAccountE_hidden.Value.Split('~')[2];
                }
            }
            if (strSplitCommand == "BindControl")
            {
                InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = null;
                SetValuesParameter = row["InstrumentType"].ToString() + '~' + row["InstrumentNumber"].ToString() + '~' + row["InstrumentDate"].ToString();
                if (strVoucherType != "C")
                {
                    AccountType = row["AccountType"].ToString();
                    CashBankType = row["CashBankID"].ToString().Split('~')[1];
                    InstType = row["InstrumentType"].ToString();
                    if (strVoucherType == "P")
                    {
                        string strParameter = null;
                        if (AccountType == "EXPENCES" && CashBankType.ToUpper() != "CASH")
                        {
                            TotalParameter = 2;
                            strParameter = "tdPayeeLable~tdPayeeValue";
                            if (InstType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_ETrans";
                            if (InstType == "D") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_DRAFT";
                            if (InstType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_Cheque";
                        }
                        if (AccountType == "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType != "0")
                        {
                            TotalParameter = 2;
                            strParameter = "tdPayeeLable~tdPayeeValue";
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_DRAFT";
                        }
                        if (AccountType == "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType == "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_CASH";
                        }
                        if (AccountType != "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType != "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_DRAFT";
                        }
                        if (AccountType != "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType == "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_CASH";
                        }
                        if (AccountType != "EXPENCES" && CashBankType.ToUpper() != "CASH")
                        {
                            if (InstType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_ETrans";
                            if (InstType == "D") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_DRAFT";
                            if (InstType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_Cheque";
                        }
                        TotalParameter = TotalParameter + 5;
                        InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdpayment~tdpaymentValue" + "~" + strParameter;
                        if (TotalParameter == 7)
                        {
                            SetValuesParameter = "EXPENCES~" + SetValuesParameter + "~NotSet~NotSet~NotSet~" + row["PayeeAccountID"].ToString() + '~' + row["LineNarration"].ToString() + '~' + row["PaymentAmount"].ToString() + "~NotSet~~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                            string strPayeeAcID = e.Parameter.Split('~')[1];
                            string strQuery = @"Select '0' as cnt_internalId,'Select' as Payee union Select cnt_internalId,isnull(ltrim(rtrim(cnt_firstName)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'')+' ['+isnull(cnt_shortname,'')+']' as Payee from tbl_master_contact where cnt_internalId like 'VR%'";
                            DataSet DsPayee = Bind_Combo(strQuery);
                            Bind_Combo_WithSelectedValue(CmbPayee, DsPayee, "Payee", "cnt_internalId", (object)row["PayeeAccountID"].ToString());
                            InstrumentDetail_CallbackPanel.JSProperties["cpBindPayee"] = "BindPayee~" + (object)row["PayeeAccountID"].ToString();
                        }
                        else
                        {
                            SetValuesParameter = "NonEXPENCES~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + '~' + row["PaymentAmount"].ToString() + "~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                            InstrumentDetail_CallbackPanel.JSProperties["cpBindPayee"] = "undefined";
                        }
                        InstrumentDetail_CallbackPanel.JSProperties["cpSetValuesParameter"] = SetValuesParameter;
                    }
                    if (strVoucherType == "R")
                    {
                        string strInstrumentType = row["InstrumentType"].ToString();
                        CashBankType = row["CashBankID"].ToString().Split('~')[1];
                        if (strInstrumentType == "C" || strInstrumentType == "E")
                        {
                            SubLedgerType = row["SubLedgerType"].ToString();
                            SubAccountID = row["SubAccountID"] != null ? row["SubAccountID"].ToString().Split('~')[0] : String.Empty;
                            if (SubLedgerType.ToUpper() == "CUSTOMERS" && CashBankType.ToUpper() != "CASH")
                            {
                                string StrQuery = "Select * from (select A.* , MB.bnk_id,ltrim(rtrim(MB.bnk_bankName)) as bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,ltrim(rtrim(TCBD.cbd_accountNumber)) as cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_bankCode Union All Select 0,'',0,'','','','','','','Third Party Account','','') as temp order by temp.cbd_Accountcategory";
                                DataSet Ds_CmbClientBank = Bind_Combo(StrQuery, "SubAccountCode", SubAccountID);
                                if (CmbClientBank.Items.Count > 0) CmbClientBank.Items.Clear();
                                if (Ds_CmbClientBank.Tables.Count > 0)
                                {
                                    if (Ds_CmbClientBank.Tables[0].Rows.Count > 0)
                                    {
                                        CmbClientBank.DataSource = Ds_CmbClientBank;
                                        CmbClientBank.DataBind();
                                        CmbClientBank.JSProperties["cpSetIndexZero"] = "False";

                                    }
                                    else
                                    {
                                        CmbClientBank.DataSource = null;
                                        CmbClientBank.DataBind();
                                        CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                                    }
                                }
                                else
                                {
                                    CmbClientBank.DataSource = null;
                                    CmbClientBank.DataBind();
                                    CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                                }
                                if (row["ClientBankID"].ToString() != "0")
                                {
                                    TotalParameter = 7;
                                    InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdCBankLable~tdCBankValue~tdRecieve~tdRecieveValue";
                                    SetValuesParameter = "CUSTOMERS~" + SetValuesParameter + '~' + row["ClientBankID"].ToString() + '~' + row["DraftIssueBankBranch"].ToString() + '~' + row["ThirdPartyReference"].ToString() + '~' + row["PayeeAccountID"].ToString() + '~' + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                                    CmbClientBank.Value = (object)row["ClientBankID"].ToString();
                                }
                                else
                                {
                                    TotalParameter = 11;
                                    InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdCBankLable~tdCBankValue~tdIBankLable~tdIBankValue~tdAuthLable~tdAuthValue~tdRecieve~tdRecieveValue";
                                    SetValuesParameter = "CUSTOMERS~" + SetValuesParameter + '~' + row["ClientBankID"].ToString() + '~' + row["DraftIssueBankBranch"].ToString() + '~' + row["ThirdPartyReference"].ToString() + '~' + row["PayeeAccountID"].ToString() + '~' + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~" + row["DraftIssueBankBranchName"].ToString() + "~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                                    CmbClientBank.SelectedIndex = 0;
                                }

                            }
                            else
                            {
                                TotalParameter = 5;
                                InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~" + "tdRecieve~tdRecieveValue";
                                SetValuesParameter = "NonCUSTOMERS~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~" + row["DraftIssueBankBranchName"].ToString() + "~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + row["SubAccountID"].ToString();
                                CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                            }
                            if (strInstrumentType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "BANK_Cheque";
                            if (strInstrumentType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "BANK_ETrans";

                        }
                        else if (strInstrumentType == "D")
                        {
                            if (CashBankType.ToUpper() == "CASH") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_DRAFT";
                            if (CashBankType.ToUpper() == "BANK") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "BANK_DRAFT";
                            TotalParameter = 7;
                            SubLedgerType = row["SubLedgerType"].ToString();
                            InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdIBankLable~tdIBankValue~tdRecieve~tdRecieveValue";
                            SetValuesParameter = SubLedgerType + "~" + SetValuesParameter + "~NotSet~" + row["DraftIssueBankBranch"].ToString() + "~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~" + row["DraftIssueBankBranchName"].ToString() + "~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                        }
                        else if (strInstrumentType == "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_CASH";
                            TotalParameter = 5;
                            SubLedgerType = row["SubLedgerType"].ToString();
                            InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdRecieve~tdRecieveValue";
                            SetValuesParameter = SubLedgerType + "~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                        }
                        InstrumentDetail_CallbackPanel.JSProperties["cpSetValuesParameter"] = SetValuesParameter;
                    }
                }
                else
                {
                    TotalParameter = 4;
                    InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdContraEntry";
                    SetValuesParameter = "CONTRA~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~NotSet~" + row["WithDrawFrom"].ToString() + '~' + row["DepositInto"].ToString() + '~' + row["Amount"].ToString() + "~NotSet~" + row["WithFromName"].ToString() + '~' + row["DepositIntoName"].ToString() + '~' + row["MainAccountID"].ToString() + '~' + SubAccountID;
                    InstrumentDetail_CallbackPanel.JSProperties["cpSetValuesParameter"] = SetValuesParameter;
                    txtWithFrom_hidden.Value = row["WithDrawFrom"].ToString() + "~~";
                    txtDepositInto_hidden.Value = row["DepositInto"].ToString() + "~~";
                    InstType = row["InstrumentType"].ToString() == "0" ? "CH" : row["InstrumentType"].ToString(); //Contra Change
                    if (InstType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_ETrans";
                    if (InstType == "D") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_DRAFT";
                    if (InstType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_Cheque";
                    if (InstType == "CH") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "SETTYPEFORCONTRA_Cash"; //Contra Change
                }
                InstrumentDetail_CallbackPanel.JSProperties["cpUpdated"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpMainAsSubAcChangeOnEdit"] = null;

            }
            if (strSplitCommand == "BindControlWhenAccountChange")
            {
                InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = null;
                SetValuesParameter = row["InstrumentType"].ToString() + '~' + row["InstrumentNumber"].ToString() + '~' + row["InstrumentDate"].ToString();
                if (strVoucherType != "C")
                {
                    AccountType = hdnAccountType.Value;
                    CashBankType = row["CashBankID"].ToString().Split('~')[1];
                    InstType = row["InstrumentType"].ToString();
                    if (strVoucherType == "P")
                    {
                        string strParameter = null;
                        if (AccountType == "EXPENCES" && CashBankType.ToUpper() != "CASH")
                        {
                            TotalParameter = 2;
                            strParameter = "tdPayeeLable~tdPayeeValue";
                            if (InstType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_ETrans";
                            if (InstType == "D") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_DRAFT";
                            if (InstType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_Cheque";
                        }
                        if (AccountType == "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType != "0")
                        {
                            TotalParameter = 2;
                            strParameter = "tdPayeeLable~tdPayeeValue";
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_DRAFT";
                        }
                        if (AccountType == "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType == "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_CASH";
                        }
                        if (AccountType != "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType != "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_DRAFT";
                        }
                        if (AccountType != "EXPENCES" && CashBankType.ToUpper() == "CASH" && InstType == "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_CASH";
                        }
                        if (AccountType != "EXPENCES" && CashBankType.ToUpper() != "CASH")
                        {
                            if (InstType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_ETrans";
                            if (InstType == "D") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_DRAFT";
                            if (InstType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_Cheque";
                        }
                        TotalParameter = TotalParameter + 5;
                        InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdpayment~tdpaymentValue" + "~" + strParameter;
                        if (TotalParameter == 7)
                        {
                            SetValuesParameter = "EXPENCES~" + SetValuesParameter + "~NotSet~NotSet~NotSet~" + row["PayeeAccountID"].ToString() + '~' + row["LineNarration"].ToString() + '~' + row["PaymentAmount"].ToString() + "~NotSet~~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                            string strPayeeAcID = e.Parameter.Split('~')[1];
                            string strQuery = @"Select '0' as cnt_internalId,'Select' as Payee union Select cnt_internalId,isnull(ltrim(rtrim(cnt_firstName)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'')+' ['+isnull(cnt_shortname,'')+']' as Payee from tbl_master_contact where cnt_internalId like 'VR%'";
                            DataSet DsPayee = Bind_Combo(strQuery);
                            Bind_Combo_WithSelectedValue(CmbPayee, DsPayee, "Payee", "cnt_internalId", (object)row["PayeeAccountID"].ToString());
                            InstrumentDetail_CallbackPanel.JSProperties["cpBindPayee"] = "BindPayee~" + (object)row["PayeeAccountID"].ToString();
                        }
                        else
                        {
                            SetValuesParameter = "NonEXPENCES~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + '~' + row["PaymentAmount"].ToString() + "~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + row["MainAccountID"].ToString() + '~' + SubAccountID;
                            InstrumentDetail_CallbackPanel.JSProperties["cpBindPayee"] = "undefined";
                        }
                        InstrumentDetail_CallbackPanel.JSProperties["cpSetValuesParameter"] = SetValuesParameter;
                    }
                    if (strVoucherType == "R")
                    {
                        string strInstrumentType = row["InstrumentType"].ToString();
                        CashBankType = row["CashBankID"].ToString().Split('~')[1];
                        if (strInstrumentType == "C" || strInstrumentType == "E")
                        {
                            SubLedgerType = hdn_SubLedgerType.Value;
                            SubAccountID = SubLedgerType != "NONE" ? strSubAccountID != String.Empty ? strSubAccountID.Split('~')[0] : String.Empty : String.Empty;
                            if (SubLedgerType.ToUpper() == "CUSTOMERS" && CashBankType.ToUpper() != "CASH")
                            {
                                string StrQuery = "Select * from (select A.* , MB.bnk_id,ltrim(rtrim(MB.bnk_bankName)) as bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,ltrim(rtrim(TCBD.cbd_accountNumber)) as cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_bankCode Union All Select 0,'',0,'','','','','','','Third Party Account','','') as temp order by temp.cbd_Accountcategory";
                                DataSet Ds_CmbClientBank = Bind_Combo(StrQuery, "SubAccountCode", SubAccountID);
                                if (CmbClientBank.Items.Count > 0) CmbClientBank.Items.Clear();
                                if (Ds_CmbClientBank.Tables.Count > 0)
                                {
                                    if (Ds_CmbClientBank.Tables[0].Rows.Count > 0)
                                    {
                                        CmbClientBank.DataSource = Ds_CmbClientBank;
                                        CmbClientBank.DataBind();
                                        CmbClientBank.JSProperties["cpSetIndexZero"] = "False";

                                    }
                                    else
                                    {
                                        CmbClientBank.DataSource = null;
                                        CmbClientBank.DataBind();
                                        CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                                    }
                                }
                                else
                                {
                                    CmbClientBank.DataSource = null;
                                    CmbClientBank.DataBind();
                                    CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                                }
                                if (CmbClientBank.Items.Count > 1)
                                {
                                    TotalParameter = 7;
                                    string strClientBankID = CmbClientBank.Items[1].Value.ToString();
                                    InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdCBankLable~tdCBankValue~tdRecieve~tdRecieveValue";
                                    SetValuesParameter = "CUSTOMERS~" + SetValuesParameter + '~' + strClientBankID + '~' + row["DraftIssueBankBranch"].ToString() + '~' + row["ThirdPartyReference"].ToString() + '~' + row["PayeeAccountID"].ToString() + '~' + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + strMainAccountID + '~' + SubAccountID;
                                    CmbClientBank.Value = (object)row["ClientBankID"].ToString();
                                }
                                else
                                {
                                    TotalParameter = 11;
                                    InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdCBankLable~tdCBankValue~tdIBankLable~tdIBankValue~tdAuthLable~tdAuthValue~tdRecieve~tdRecieveValue";
                                    SetValuesParameter = "CUSTOMERS~" + SetValuesParameter + '~' + "0" + '~' + row["DraftIssueBankBranch"].ToString() + '~' + row["ThirdPartyReference"].ToString() + '~' + row["PayeeAccountID"].ToString() + '~' + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~" + row["DraftIssueBankBranchName"].ToString() + "~NotSet~NotSet~" + strMainAccountID + '~' + SubAccountID;
                                    CmbClientBank.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                TotalParameter = 5;
                                InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~" + "tdRecieve~tdRecieveValue";
                                SetValuesParameter = "NonCUSTOMERS~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~" + row["DraftIssueBankBranchName"].ToString() + "~NotSet~NotSet~" + strMainAccountID + '~' + strSubAccountID;
                                CmbClientBank.JSProperties["cpSetIndexZero"] = "undefined";
                            }
                            if (strInstrumentType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "BANK_Cheque";
                            if (strInstrumentType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "BANK_ETrans";

                        }
                        else if (strInstrumentType == "D")
                        {
                            if (CashBankType.ToUpper() == "CASH") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_DRAFT";
                            if (CashBankType.ToUpper() == "BANK") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "BANK_DRAFT";
                            TotalParameter = 7;
                            SubLedgerType = row["SubLedgerType"].ToString();
                            InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdIBankLable~tdIBankValue~tdRecieve~tdRecieveValue";
                            SetValuesParameter = SubLedgerType + "~" + SetValuesParameter + "~NotSet~" + row["DraftIssueBankBranch"].ToString() + "~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~" + row["DraftIssueBankBranchName"].ToString() + "~NotSet~NotSet~" + strMainAccountID + '~' + SubAccountID;
                        }
                        else if (strInstrumentType == "0")
                        {
                            InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "CASH_CASH";
                            TotalParameter = 5;
                            SubLedgerType = row["SubLedgerType"].ToString();
                            InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdRecieve~tdRecieveValue";
                            SetValuesParameter = SubLedgerType + "~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~" + row["ReceiptAmount"].ToString() + "~NotSet~NotSet~NotSet~NotSet~NotSet~NotSet~" + strMainAccountID + '~' + SubAccountID;
                        }
                        InstrumentDetail_CallbackPanel.JSProperties["cpSetValuesParameter"] = SetValuesParameter;
                    }
                }
                else
                {
                    TotalParameter = 4;
                    InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = TotalParameter.ToString() + '~' + strVoucherType + '~' + SubAccountID + "~tdContraEntry";
                    SetValuesParameter = "CONTRA~" + SetValuesParameter + "~NotSet~NotSet~NotSet~NotSet~" + row["LineNarration"].ToString() + "~NotSet~NotSet~" + row["WithDrawFrom"].ToString() + '~' + row["DepositInto"].ToString() + '~' + row["Amount"].ToString() + "~NotSet~" + row["WithFromName"].ToString() + '~' + row["DepositIntoName"].ToString() + '~' + row["MainAccountID"].ToString() + '~' + SubAccountID;
                    InstrumentDetail_CallbackPanel.JSProperties["cpSetValuesParameter"] = SetValuesParameter;
                    txtWithFrom_hidden.Value = row["WithDrawFrom"].ToString() + "~~";
                    txtDepositInto_hidden.Value = row["DepositInto"].ToString() + "~~";
                    InstType = row["InstrumentType"].ToString();
                    if (InstType == "E") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_ETrans";
                    if (InstType == "D") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_DRAFT";
                    if (InstType == "C") InstrumentDetail_CallbackPanel.JSProperties["cpCashBankType_InstTypeE"] = "Bank_Cheque";
                }
                InstrumentDetail_CallbackPanel.JSProperties["cpUpdated"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpMainAsSubAcChangeOnEdit"] = null;

            }
            if (strSplitCommand == "SetValue")
            {
                CmbPayee.SelectedIndex = 1;
                InstrumentDetail_CallbackPanel.JSProperties["cpUpdated"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpBindPayee"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpMainAsSubAcChangeOnEdit"] = null;
            }

            if (strSplitCommand == "Update")
            {
                string strSplitBranch = e.Parameter.Split('~')[2];
                string strDefalutBranch = hdnDefaultBranch.Value;
                string strRecordID = KeyValue;
                string strClientBankID = e.Parameter.Split('~')[4];
                string strPayeeAcID = e.Parameter.Split('~')[5];
                string strInstType = e.Parameter.Split('~')[6];
                UpdateData_ToGrid(strSplitBranch, strVoucherType, strDefalutBranch, strRecordID, strClientBankID, strPayeeAcID, strInstType);
                InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpUpdated"] = "True";
                InstrumentDetail_CallbackPanel.JSProperties["cpBindPayee"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpMainAsSubAcChangeOnEdit"] = null;
                //This For Log Purpose
                if (Session["IBRef"] != null)
                {
                    string IBRef = Session["IBRef"].ToString();
                    string strLogID = ViewState["LogID"].ToString();
                    oGenericLogSystem.CreateLog("Trans_CashBankVouchers", "CashBank_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                    oGenericLogSystem.CreateLog("Trans_CashBankDetail", "CashBankDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                }
            }
            if (strSplitCommand == "MainAsSubAcChangeOnEdit")
            {
                InstrumentDetail_CallbackPanel.JSProperties["cpMainAsSubAcChangeOnEdit"] = "True";
                InstrumentDetail_CallbackPanel.JSProperties["cpUpdated"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpBindPayee"] = "undefined";
                InstrumentDetail_CallbackPanel.JSProperties["cpVisbleControlString"] = "undefined";
            }
        }
        protected void CmbPayee_Callback(object source, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string strPayeeAcID = e.Parameter.Split('~')[1];
            string strQuery = @"Select '0' as cnt_internalId,'Select' as Payee union Select cnt_internalId,isnull(ltrim(rtrim(cnt_firstName)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'')+' ['+isnull(cnt_shortname,'')+']' as Payee from tbl_master_contact where cnt_internalId like 'VR%'";
            if (strSplitCommand == "BindPayeeCombo")
            {
                DataSet DsPayee = Bind_Combo(strQuery);
                Bind_Combo(CmbPayee, DsPayee, "Payee", "cnt_internalId", 0);
            }
        }
        protected void GvAddRecordDisplay_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            int rowindex = GvAddRecordDisplay.EditingRowVisibleIndex;
            string VoucherType = GvAddRecordDisplay.GetRowValues(rowindex, "VoucherType").ToString();
            DataSet DsGvAddRecordXML = new DataSet();
            string KeyValue = GvAddRecordDisplay.GetRowValues(rowindex, "RecordID").ToString();
            DsGvAddRecordXML.ReadXml(Server.MapPath(PXMLPATH));
            DsGvAddRecordXML.Tables[0].PrimaryKey = new DataColumn[] { DsGvAddRecordXML.Tables[0].Columns["RecordID"] };
            DataRow row = DsGvAddRecordXML.Tables[0].Rows.Find(KeyValue);
            HiddenField txtMainAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtMainAccountE_hidden");
            HiddenField txtSubAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE_hidden");
            if (VoucherType == "C")
            {
                TextBox TxtMainAccountE = (TextBox)GvAddRecordDisplay.FindEditFormTemplateControl("txtMainAccountE");
                TextBox TxtSubAccountE = (TextBox)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE");
                TxtMainAccountE.Visible = false;
                TxtSubAccountE.Visible = false;
            }
            if (row["MainAccountID"].ToString().Split('~')[0] == txtMainAccountE_hidden.Value.Split('~')[0] || txtMainAccountE_hidden.Value == String.Empty)
            {
                txtMainAccountE_hidden.Value = String.Empty;
                txtSubAccountE_hidden.Value = String.Empty;
                GvAddRecordDisplay.JSProperties["cpSetMainAcCodeSubLedgerTypeWhenEdit"] = row["MainAccountID"].ToString().Split('~')[0] + '~' + row["SubLedgerType"].ToString();
            }
        }
        void UpdateData_ToGrid(string BranchID, string VoucherType, string DefaultBranch, string RowID, string SelectedClientBankID, string SelectedPayeeAcID, string InstType)
        {
            DataSet DsUpdateXML = new DataSet();
            string strPaymentAmount = "", strReceiptAmount = "", strDraftIssueBankBranch = "", strClientBankID = "", strPayeeAccountID = "",
                strIsThirdParty = "", strThirdPartyReference = "", strContraAmount = "", strWithDrawFrom = "", strDepositInto = ""
                , strMainAccountID = "", strSubAccountID = "", strBranchID = "", strCashBankID = "", strWithDrawFromName = "",
                strDepostitIntoName = "", strAmount = "", strDraftIssueBankBranchName = "";

            TextBox txtMainAccountE = (TextBox)GvAddRecordDisplay.FindEditFormTemplateControl("txtMainAccountE");
            TextBox txtSubAccountE = (TextBox)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE");
            HiddenField txtMainAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtMainAccountE_hidden");
            HiddenField txtSubAccountE_hidden = (HiddenField)GvAddRecordDisplay.FindEditFormTemplateControl("txtSubAccountE_hidden");

            if (VoucherType == "R" && (InstType == "C" || InstType == "E"))
            {
                strPaymentAmount = "0.00";
                strReceiptAmount = txtRecieve.Text;
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
                strMainAccountID = txtMainAccountE_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccountE_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[2] : txtSubAccountE_hidden.Value != String.Empty ? txtSubAccountE_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                if (BranchID != "NAB")
                {
                    if (hdn_SegID_SegmentName.Value.Split('~')[1].ToUpper() != "NSDL" && hdn_SegID_SegmentName.Value.Split('~')[1].ToUpper() != "CDSL")
                    {
                        if (SelectedClientBankID != "0")
                        {
                            strDraftIssueBankBranch = "0";
                            strDraftIssueBankBranchName = String.Empty;
                            strThirdPartyReference = String.Empty;
                            strClientBankID = SelectedClientBankID;
                            strPayeeAccountID = "0";
                            strIsThirdParty = "N";
                        }
                        else
                        {
                            strDraftIssueBankBranch = txtIssuingBank_hidden.Value;
                            strDraftIssueBankBranchName = txtIssuingBank.Text;
                            strThirdPartyReference = txtAuthLetterRef.Text;
                            strClientBankID = "0";
                            strPayeeAccountID = "0";
                            strIsThirdParty = "Y";
                        }
                    }
                    else
                    {
                        strDraftIssueBankBranch = "0";
                        strDraftIssueBankBranchName = String.Empty;
                        strThirdPartyReference = String.Empty;
                        strClientBankID = "0";
                        strPayeeAccountID = "0";
                        strIsThirdParty = "N";
                    }
                }
                else
                {
                    strDraftIssueBankBranch = "0";
                    strDraftIssueBankBranchName = String.Empty;
                    strThirdPartyReference = String.Empty;
                    strClientBankID = "0";
                    strPayeeAccountID = "0";
                    strIsThirdParty = "N";
                }
            }
            else if (VoucherType == "R" && InstType == "D")
            {
                strMainAccountID = txtMainAccountE_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccountE_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[2] : txtSubAccountE_hidden.Value != String.Empty ? txtSubAccountE_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                strPaymentAmount = "0.00";
                strReceiptAmount = txtRecieve.Text;
                strDraftIssueBankBranch = txtIssuingBank_hidden.Value;
                strDraftIssueBankBranchName = txtIssuingBank.Text;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
            }
            else if (VoucherType == "R" && InstType == "CH")
            {
                strPaymentAmount = "0.00";
                strReceiptAmount = txtRecieve.Text;
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
                strMainAccountID = txtMainAccountE_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccountE_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[2] : txtSubAccountE_hidden.Value != String.Empty ? txtSubAccountE_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                strDraftIssueBankBranch = "0";
                strDraftIssueBankBranchName = String.Empty;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
            }
            else if (VoucherType == "P")
            {
                strMainAccountID = txtMainAccountE_hidden.Value.Split('~')[0];
                strSubAccountID = (BranchID != "NAB") ? txtSubAccountE_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccountE_hidden.Value.ToString().Split('~')[2] : txtSubAccountE_hidden.Value != String.Empty ? txtSubAccountE_hidden.Value.Split('~')[0] : String.Empty;
                strBranchID = (BranchID != "NAB") ? BranchID : DefaultBranch;
                strPaymentAmount = txtPayment.Text;
                strReceiptAmount = "0.00";
                strDraftIssueBankBranch = String.Empty;
                strDraftIssueBankBranchName = String.Empty;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
                strWithDrawFrom = String.Empty;
                strWithDrawFromName = String.Empty;
                strDepositInto = String.Empty;
                strDepostitIntoName = String.Empty;
                strAmount = String.Empty;
                if (hdnAccountType.Value == "EXPENCES")
                {
                    strPayeeAccountID = SelectedPayeeAcID;
                }
                else
                {
                    strPayeeAccountID = "0";
                }
            }
            else
            {
                strMainAccountID = String.Empty;
                strSubAccountID = String.Empty;
                strBranchID = DefaultBranch;
                strContraAmount = txtAmount.Text;
                strDraftIssueBankBranch = "0";
                strDraftIssueBankBranchName = String.Empty;
                strThirdPartyReference = String.Empty;
                strClientBankID = "0";
                strPayeeAccountID = "0";
                strIsThirdParty = "N";
                strWithDrawFrom = txtWithFrom_hidden.Value.Split('~')[0];
                strDepositInto = txtDepositInto_hidden.Value.Split('~')[0];
                strWithDrawFromName = txtWithFrom.Text;
                strDepostitIntoName = txtDepositInto.Text;
                strAmount = txtAmount.Text;
            }

            if (DsUpdateXML.Tables.Count > 0) { DsUpdateXML.Tables.Remove(DsUpdateXML.Tables[0]); DsUpdateXML.Clear(); }
            DsUpdateXML.ReadXml(Server.MapPath(PXMLPATH));
            DsUpdateXML.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXML.Tables[0].Columns["RecordID"] };
            DataRow row = DsUpdateXML.Tables[0].Rows.Find(RowID);
            if (row["SubAccountID"].ToString().Split('~')[0] != strSubAccountID.Split('~')[0] || row["MainAccountID"].ToString().Split('~')[0] != strMainAccountID.Split('~')[0])
            {
                row["IsVoucherNumberChange"] = "Y";
            }
            row["BranchID"] = strBranchID;
            row["MainAccountID"] = strMainAccountID;
            row["SubAccountID"] = strSubAccountID;
            row["PaymentAmount"] = strPaymentAmount;
            row["ReceiptAmount"] = strReceiptAmount;
            row["InstrumentType"] = InstType != "CH" ? InstType : "0";
            row["InstrumentNumber"] = InstType != "CH" ? txtInstNo.Text : String.Empty;
            row["InstrumentDate"] = (InstDate.Value == null) || (InstType == "CH") ? "01-01-1900" : InstDate.Value.ToString() != "01-01-0100" ? Convert.ToDateTime(InstDate.Value.ToString()).ToString("MM/dd/yyyy HH:mm:ss") : "01-01-1900";
            row["DraftIssueBankBranch"] = strDraftIssueBankBranch;
            row["ClientBankID"] = strClientBankID;
            row["PayeeAccountID"] = strPayeeAccountID;
            row["IsThirdParty"] = strIsThirdParty;
            row["ThirdPartyReference"] = strThirdPartyReference;
            row["LineNarration"] = txtNarration1.Text;
            row["WithDrawFrom"] = strWithDrawFrom;
            row["DepositInto"] = strDepositInto;
            row["InstrumentTypeName"] = ComboInstType.Text;
            row["SubAccountName"] = txtSubAccountE_hidden.Value != "" ? txtSubAccountE.Text.Split('[')[0] : String.Empty;
            row["MainAccountName"] = txtMainAccountE.Text.ToString();
            row["BranchNonBranch"] = BranchID;
            row["WithFromName"] = strWithDrawFromName;
            row["DepositIntoName"] = strDepostitIntoName;
            row["Amount"] = strAmount;
            row["AccountType"] = hdnAccountType.Value;
            row["SubLedgerType"] = hdn_SubLedgerType.Value;
            row["DraftIssueBankBranchName"] = strDraftIssueBankBranchName;
            row["VoucherType"] = VoucherType;
            row["FormatedInstrumentDate"] = (InstDate.Value == null) || (InstType == "CH") ? "01 Jan 1900" : InstDate.Value.ToString() != "01-01-0100" ? Convert.ToDateTime(InstDate.Value.ToString()).ToString("dd MMM yyyy") : "01 Jan 1900";
            row["CashBankName"] = txtBankAccounts.Text.Split('~')[0];
            if (VoucherType == "R")
            {
                //This Updation was for cashbank but now its change for client bank code So just think BanknameAndAcNumber=ClientBankCode
                row["BanknameAndAcNumber"] = (InstType != "D" && InstType != "CH") && hdn_SubLedgerType.Value == "CUSTOMERS" ? SelectedClientBankID != "0" ? SelectedClientBankID : txtIssuingBank_hidden.Value.Split('~')[0] : (InstType != "D" && InstType != "CH") ? txtIssuingBank_hidden.Value.Trim() != String.Empty ? txtIssuingBank_hidden.Value.Split('~')[0] : "000000" : "000000";
            }
            else
            {
                row["BanknameAndAcNumber"] = "000000";
            }
            row.AcceptChanges();
            File.Delete(Server.MapPath(PXMLPATH));
            DsUpdateXML.WriteXml(Server.MapPath(PXMLPATH));
            BindGrid(GvAddRecordDisplay, DsUpdateXML, "DESC");
            DsUpdateXML.Dispose();

        }

        protected void GvAddRecordDisplay_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            DataSet DsCancelXML = new DataSet();
            if (DsCancelXML.Tables.Count > 0) { DsCancelXML.Tables.Remove(DsCancelXML.Tables[0]); DsCancelXML.Clear(); }
            DsCancelXML.ReadXml(Server.MapPath(PXMLPATH));
            DsCancelXML.Tables[0].PrimaryKey = new DataColumn[] { DsCancelXML.Tables[0].Columns["RecordID"] };
            DataRow row = DsCancelXML.Tables[0].Rows.Find(e.EditingKeyValue);
            GvAddRecordDisplay.JSProperties["cpClearEditSetUp"] = "True~" + row["CashBankID"];
        }
        protected void ComboInstType_Callback(object source, CallbackEventArgsBase e)
        {
            ComboInstType.JSProperties["cpSetValue"] = null;
            string strSplitCommand = e.Parameter.Split('~')[0];
            string CashBankType = e.Parameter.Split('~')[1];
            if (strSplitCommand == "SetItems")
            {
                ComboInstType.Items.Clear();
                if (CashBankType.Trim().ToUpper() == "CASH")
                {
                    ComboInstType.Items.Add(new ListEditItem("Cash", "CH"));
                    ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                }
                else if ((CashBankType.Trim().ToUpper() == "SETTYPEFORCONTRA"))//Contra Change
                {
                    //New Change in Contra Enter For CashType: Cash
                    ComboInstType.Items.Add(new ListEditItem("Cheque", "C"));
                    ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                    ComboInstType.Items.Add(new ListEditItem("E.Transfer", "E"));
                    ComboInstType.Items.Add(new ListEditItem("Cash", "CH"));
                }
                else
                {
                    if (ComboType.SelectedItem.Value.ToString() != "C")
                    {
                        ComboInstType.Items.Add(new ListEditItem("Cheque", "C"));
                        ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                        ComboInstType.Items.Add(new ListEditItem("E.Transfer", "E"));
                    }
                    else
                    {
                        //New Change in Contra Enter For CashType: Cash
                        ComboInstType.Items.Add(new ListEditItem("Cheque", "C"));
                        ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                        ComboInstType.Items.Add(new ListEditItem("E.Transfer", "E"));
                        ComboInstType.Items.Add(new ListEditItem("Cash", "CH"));
                    }
                }
            }
            if (strSplitCommand == "SetItemsAndSelectValue")
            {
                string SelectedInstType = e.Parameter.Split('~')[2];
                string SelectedVoucherType = e.Parameter.Split('~')[3];
                ComboInstType.Items.Clear();
                if (CashBankType.Trim() == "CASH")
                {
                    ComboInstType.Items.Add(new ListEditItem("Cash", "CH"));
                    ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                }
                else if ((CashBankType.Trim().ToUpper() == "SETTYPEFORCONTRA"))//Contra Change
                {
                    //New Change in Contra Enter For CashType: Cash
                    ComboInstType.Items.Add(new ListEditItem("Cash", "CH"));
                    ComboInstType.Items.Add(new ListEditItem("Cheque", "C"));
                    ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                    ComboInstType.Items.Add(new ListEditItem("E.Transfer", "E"));
                }
                else
                {
                    if (SelectedVoucherType.Trim() != "C")
                    {
                        ComboInstType.Items.Add(new ListEditItem("Cheque", "C"));
                        ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                        ComboInstType.Items.Add(new ListEditItem("E.Transfer", "E"));
                        ComboInstType.JSProperties["cpSetValue"] = SelectedInstType;
                    }
                    else
                    {
                        //New Change in Contra Enter For CashType: Cash
                        ComboInstType.Items.Add(new ListEditItem("Cheque", "C"));
                        ComboInstType.Items.Add(new ListEditItem("Draft", "D"));
                        ComboInstType.Items.Add(new ListEditItem("E.Transfer", "E"));
                        ComboInstType.Items.Add(new ListEditItem("Cash", "CH"));
                    }

                }

            }
        }
        string Save_Records()
        {
            DataSet DsSaveRecordXML = new DataSet();
            DataSet DsFetchToPrint;
            SqlDataAdapter DaFetchToPrint;
            DataTable DtSaveRecordXML;
            DataRow[] result = null;
            DataRow[] remainresult = null;
            if (File.Exists(Server.MapPath(PXMLPATH)))
            {
                if (DsSaveRecordXML.Tables.Count > 0) { DsSaveRecordXML.Tables.Remove(DsSaveRecordXML.Tables[0]); DsSaveRecordXML.Clear(); }
                DsSaveRecordXML.ReadXml(Server.MapPath(PXMLPATH));
                if (DsSaveRecordXML.Tables.Count > 0)
                {
                    if (DsSaveRecordXML.Tables[0].Rows.Count > 0)
                    {
                        DtSaveRecordXML = DsSaveRecordXML.Tables[0];
                        result = DtSaveRecordXML.Select("IsVoucherNumberChange = 'Y'");
                        remainresult = DtSaveRecordXML.Select("IsVoucherNumberChange <> 'Y'");
                        if (DsSaveRecordXML.Tables[0].Rows[0]["CashBankID"].ToString().Split('~')[0] != txtBankAccounts_hidden.Value.Split('~')[0])
                        {
                            for (int i = 0; i < DsSaveRecordXML.Tables[0].Rows.Count; i++)
                            {
                                DsSaveRecordXML.Tables[0].Rows[i]["CashBankID"] = txtBankAccounts_hidden.Value;
                                DsSaveRecordXML.Tables[0].Rows[i]["CashBankName"] = txtBankAccounts.Text.Split('~')[0];
                            }
                            DsSaveRecordXML.AcceptChanges();
                        }
                        if (hdn_Mode.Value == "Edit" && result.Length > 0)
                        {
                            for (int i = 0; i < DsSaveRecordXML.Tables[0].Rows.Count; i++)
                            {
                                DsSaveRecordXML.Tables[0].Rows[i]["OldVoucherNumber"] = String.Empty;
                                if (remainresult.Length > 0)
                                {
                                    DsSaveRecordXML.Tables[0].Rows[i]["ExchangeSegmentID"] = remainresult[0]["ExchangeSegmentID"].ToString();
                                    DsSaveRecordXML.Tables[0].Rows[i]["SegmentName"] = remainresult[0]["SegmentName"].ToString();
                                    DsSaveRecordXML.Tables[0].Rows[i]["RealCreateDateTime"] = remainresult[0]["RealCreateDateTime"].ToString();
                                    DsSaveRecordXML.Tables[0].Rows[i]["RealCreateUser"] = remainresult[0]["RealCreateUser"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            if (DsSaveRecordXML.Tables[0].Rows.Count > 0)
            {
                if (PCurrentSegment != null && hdn_Mode.Value.Trim() != String.Empty)
                {
                    string ExchangeSegmentIdE = (hdn_Mode.Value == "Edit") ? DsSaveRecordXML.Tables[0].Rows[0]["ExchangeSegmentID"].ToString() : String.Empty;
                    string TransactionDateE = (hdn_Mode.Value == "Edit") ? DsSaveRecordXML.Tables[0].Rows[0]["TransactionDate"].ToString() : String.Empty;
                    DsFetchToPrint = new DataSet();




                    // {
                    //using (SqlCommand com = new SqlCommand("Insert_CashBankVoucherEntry", con))
                    // {
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.AddWithValue("@CashBankXML", DsSaveRecordXML.GetXml());
                    //com.Parameters.AddWithValue("@CashBankID", txtBankAccounts_hidden.Value.Split('~')[0]);
                    //com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
                    //com.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                    //com.Parameters.AddWithValue("@CompanyID", PCompanyID);
                    //com.Parameters.AddWithValue("@TransactionDate", Convert.ToDateTime(dtTDate.Value));
                    //com.Parameters.AddWithValue("@ExchangeSegmentID", (hdn_Mode.Value != "Edit") ? PCurrentSegment.Trim() : ExchangeSegmentIdE.Trim());
                    //com.Parameters.AddWithValue("@TransactionType", (hdn_Mode.Value == "Entry") ? ComboType.SelectedItem.Value.ToString() : SCmb_Type.SelectedItem.Value.ToString());
                    //com.Parameters.AddWithValue("@EntryUserProfile", Session["EntryProfileType"].ToString());
                    //com.Parameters.AddWithValue("@Narration", txtNarration.Text);
                    //com.Parameters.AddWithValue("@FormMode", hdn_Mode.Value);
                    //com.Parameters.AddWithValue("@OldIBRef", Session["IBRef"] != null ? Session["IBRef"].ToString() : String.Empty);
                    //com.Parameters.AddWithValue("@IsVoucherNumberChange", hdn_Mode.Value == "Entry" ? result.Length == 0 ? "Y" : String.Empty : (result.Length > 0 || Convert.ToDateTime(TransactionDateE) != Convert.ToDateTime(dtTDate.Value.ToString())) ? "Y" : String.Empty);
                    //com.Parameters.AddWithValue("@CurrencyID", ChoosenCurrency.Split('~')[0]);
                    //SqlParameter OutParam = null;
                    //OutParam = com.Parameters.Add("@OutIBRef", SqlDbType.VarChar, 100);
                    //OutParam.Direction = ParameterDirection.Output;
                    //com.CommandTimeout = 0;
                    string ExchangeSegmentID = string.Empty;
                    string TransactionType = string.Empty;
                    string OldIBRef = string.Empty;
                    string FormMode = string.Empty;
                    string OutIBRef = string.Empty;
                    ExchangeSegmentID = (hdn_Mode.Value == "Entry") ? ComboType.SelectedItem.Value.ToString() : SCmb_Type.SelectedItem.Value.ToString();
                    TransactionType = (hdn_Mode.Value == "Entry") ? ComboType.SelectedItem.Value.ToString() : SCmb_Type.SelectedItem.Value.ToString();
                    OldIBRef = Session["IBRef"] != null ? Session["IBRef"].ToString() : String.Empty;
                    FormMode = (hdn_Mode.Value == "Entry" ? result.Length == 0 ? "Y" : String.Empty : (result.Length > 0 || Convert.ToDateTime(TransactionDateE) != Convert.ToDateTime(dtTDate.Value.ToString())) ? "Y" : String.Empty);
                    DsFetchToPrint = objFAReportsOther.InsertCashBankVoucherEntry(DsSaveRecordXML.GetXml(), txtBankAccounts_hidden.Value.Split('~')[0], Session["userid"].ToString(),
                        Session["LastFinYear"].ToString(), PCompanyID, Convert.ToDateTime(dtTDate.Value).ToString("yyyy-MM-dd"), ExchangeSegmentID, TransactionType, Session["EntryProfileType"].ToString(),
                        txtNarration.Text, hdn_Mode.Value, OldIBRef, FormMode, Convert.ToInt32(ChoosenCurrency.Split('~')[0]), out OutIBRef);

                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                    if (hdn_Mode.Value == "Edit")
                    {
                        if (Session["IBRef"] != null)
                        {
                            string IBRef = Session["IBRef"].ToString();
                            oGenericLogSystem.CreateLog("Trans_CashBankVouchers", "CashBank_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlSaving, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                            oGenericLogSystem.CreateLog("Trans_CashBankDetail", "CashBankDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlSaving, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                        }
                    }

                    //  using (DaFetchToPrint = new SqlDataAdapter(com))
                    //  {
                    //  DsFetchToPrint.Clear();
                    // DaFetchToPrint.Fill(DsFetchToPrint);
                    // string strOutIBRefs = com.Parameters["@OutIBRef"].Value.ToString();
                    string strOutIBRefs = OutIBRef.ToString();
                    //This For Log Purpose
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), strOutIBRefs, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                    oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), strOutIBRefs, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                    //  }
                    if (DsFetchToPrint.Tables.Count < 3)
                    {
                        if (DsFetchToPrint.Tables.Count > 0)
                        {
                            try
                            {
                                File.Delete(Server.MapPath(PXMLPATH));
                            }
                            catch
                            {
                                File.Delete(Server.MapPath(PXMLPATH));
                            }
                            BindGrid(GvAddRecordDisplay);
                            Session["DSJournalReturn"] = DsFetchToPrint;
                            //Set Currency as Trade Currency Default Mode
                            Session["ActiveCurrency"] = Session["TradeCurrency"];
                            return "Success";
                        }
                        else
                        {
                            //This For Log Purpose
                            oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), Session["IBRef"].ToString(), "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                            return "Problem";
                        }
                    }
                    else
                    {
                        //This For Log Purpose
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), Session["IBRef"].ToString(), "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                        return "Problem";
                    }
                    //}
                    // }
                }
                else return "Problem";
            }
            DsSaveRecordXML.Dispose();
            return "Problem";
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataSet DsReturnData = new DataSet();
            DsReturnData = (DataSet)Session["DSJournalReturn"];
            DsReturnData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//CashBank.xsd");
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            ReportDocument reportObj = new ReportDocument();
            string ReportPath = Server.MapPath("..\\Reports\\CashBank.rpt");
            reportObj.Load(ReportPath);
            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            reportObj.SetDataSource(DsReturnData);
            //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "CashBank");
            reportObj.Dispose();
            GC.Collect();
        }


        protected void CbpBankBalance_Callback(object source, CallbackEventArgsBase e)
        {
            Converter oConverter = new Converter();
            CbpBankBalance.JSProperties["cpBankBalance"] = "undefined";
            string command = e.Parameter.Split('~')[0];
            if (command == "BankBalance")
            {
                string MainAccountID = e.Parameter.Split('~')[1];
                string QueryPart_Tables = "Trans_AccountsLedger";
                string QueryPart_Field = "Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr)";
                string QueryPart_Where = "AccountsLedger_MainAccountID='" + MainAccountID + "' and AccountsLedger_FinYear='" + Session["LastFinYear"] + @"' 
            and AccountsLedger_CompanyID='" + Session["LastCompany"].ToString() + @"' and 
            isnull(AccountsLedger_Currency,
            ((Select 
				(Select ExchangeSegment_TradeCurrencyID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID 
				and ExchangeSegment_Code=Exch_SegmentID) as [ExchangeSegmentID] 
				from (Select Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
				Where Exh_CntId=Exch_ExchID and 
				exch_compId= '" + PCompanyID + @"'
				and exch_internalId=" + PCurrentSegment + @"
                ) as T1,Master_Exchange
				Where Exchange_ShortName=Exh_ShortName)))=" + ChoosenCurrency.Split('~')[0];
                DataTable DtBankBalance = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where);
                if (DtBankBalance.Rows.Count > 0 && DtBankBalance.Rows[0][0].ToString() != String.Empty)
                {
                    string strColor = null;
                    string Result = null;
                    if (TotalRecieve > 0)
                    {
                        TotalBankBalance = Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString());
                        Result = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(TotalBankBalance + TotalRecieve).ToString();
                        Result = Convert.ToDecimal(Result) > 0 ? Result + " Dr." : Result + " Cr.";
                        strColor = Convert.ToDecimal(Result) > 0 ? "Blue" : "Red";
                    }
                    else if (TotalPayment > 0)
                    {
                        TotalBankBalance = Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString());
                        Result = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(TotalBankBalance - TotalPayment);
                        Result = Convert.ToDecimal(Result) > 0 ? Result + " Dr." : Result + " Cr.";
                        strColor = Convert.ToDecimal(Result) > 0 ? "Blue" : "Red";
                    }
                    else
                    {
                        Result = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString())).ToString();
                        Result = Convert.ToDecimal(Result) > 0 ? Result + " Dr." : Result + " Cr.";
                        strColor = (Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString()) > 0) ? "Blue" : "Red";
                        TotalBankBalance = Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString());
                    }
                    CbpBankBalance.JSProperties["cpBankBalance"] = Result + '~' + strColor;
                }
                else
                {
                    CbpBankBalance.JSProperties["cpBankBalance"] = "undefined";
                    TotalBankBalance = 0;
                }
            }
            if (command == "BankBalanceAfterReceipt")
            {
                string Result = null;
                string strColor = null;
                Result = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(TotalBankBalance + TotalRecieve).ToString();
                Result = Convert.ToDecimal(Result) > 0 ? Result + " Dr." : Result + " Cr.";
                strColor = Convert.ToDecimal(Result) > 0 ? "Blue" : "Red";
                CbpBankBalance.JSProperties["cpBankBalance"] = Result + '~' + strColor;
            }
            if (command == "BankBalanceAfterPayement")
            {
                string Result = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(TotalBankBalance - TotalPayment);
                Result = Convert.ToDecimal(Result) > 0 ? Result + " Dr." : Result + " Cr.";
                string strColor = null;
                strColor = Convert.ToDecimal(Result) > 0 ? "Blue" : "Red";
                CbpBankBalance.JSProperties["cpBankBalance"] = Result + '~' + strColor;
            }

        }
        protected void CbpAcBalance_Callback(object source, CallbackEventArgsBase e)
        {
            CbpAcBalance.JSProperties["cpAcBalance"] = "undefined";
            string command = e.Parameter.Split('~')[0];
            if (command == "AcBalance")
            {
                string MainAccountID = e.Parameter.Split('~')[1];
                string SubAccountID = e.Parameter.Split('~')[2] != String.Empty ? e.Parameter.Split('~')[2] : null;
                string[,] MainAcCode = oDBEngine.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + MainAccountID + "", 1);
                DateTime FinYearEnd = Convert.ToDateTime(Session["FinYearEnd"].ToString()); //Session["FinYearEnd"].ToString();
                DataTable DtAcBalance = oDBEngine.OpeningBalanceJournal1("'" + MainAcCode[0, 0] + "'", SubAccountID, FinYearEnd, PCurrentSegment, PCompanyID, FinYearEnd, Convert.ToInt32(ChoosenCurrency.Split('~')[0]));
                Converter oConverter = new Converter();
                if (DtAcBalance.Rows.Count > 0 && DtAcBalance.Rows[0][0].ToString() != String.Empty)
                {
                    string strColor = null;
                    string strDrCr = (Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString()) > 0) ? " Dr." : " Cr.";
                    strColor = (Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString()) > 0) ? "Blue" : "Red";
                    CbpAcBalance.JSProperties["cpAcBalance"] = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString())) +
                        strDrCr + '~' + strColor;
                }
                else
                {
                    CbpAcBalance.JSProperties["cpAcBalance"] = "undefined";
                }

            }
        }
        protected void CbpWithBankBalance_Callback(object source, CallbackEventArgsBase e)
        {
            CbpWithBankBalance.JSProperties["cpWithBalance"] = "undefined";
            string command = e.Parameter.Split('~')[0];
            if (command == "WithBalance")
            {
                string MainAccountID = e.Parameter.Split('~')[1];
                string[,] MainAcCode = oDBEngine.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + MainAccountID + "", 1);
                string QueryPart_Tables = "Trans_AccountsLedger";
                string QueryPart_Field = "Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr)";
                string QueryPart_Where = "AccountsLedger_MainAccountID='" + MainAcCode[0, 0] + "' and AccountsLedger_FinYear='" +
                    Session["LastFinYear"] + @"' and 
            isnull(AccountsLedger_Currency,
            ((Select 
				(Select ExchangeSegment_TradeCurrencyID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID 
				and ExchangeSegment_Code=Exch_SegmentID) as [ExchangeSegmentID] 
				from (Select Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
				Where Exh_CntId=Exch_ExchID and 
				exch_compId= '" + PCompanyID + @"'
				and exch_internalId=" + PCurrentSegment + @"
                ) as T1,Master_Exchange
				Where Exchange_ShortName=Exh_ShortName)))=" + ChoosenCurrency.Split('~')[0];
                DataTable DtBankBalance = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where);
                Converter oConverter = new Converter();
                if (DtBankBalance.Rows.Count > 0 && DtBankBalance.Rows[0][0].ToString() != String.Empty)
                {
                    string strColor = null;
                    string strDrCr = (Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString()) > 0) ? " Dr." : " Cr.";
                    strColor = (Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString()) > 0) ? "Blue" : "Red";
                    CbpWithBankBalance.JSProperties["cpWithBalance"] = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString())) +
                        strDrCr + '~' + strColor;
                }
                else
                {
                    CbpWithBankBalance.JSProperties["cpWithBalance"] = "undefined";
                }

            }
        }
        protected void CbpDepstBankBalance_Callback(object source, CallbackEventArgsBase e)
        {
            CbpDepstBankBalance.JSProperties["cpDepstBalance"] = "undefined";
            string command = e.Parameter.Split('~')[0];
            if (command == "DepstBalance")
            {
                string MainAccountID = e.Parameter.Split('~')[1];
                string[,] MainAcCode = oDBEngine.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + MainAccountID + "", 1);
                string QueryPart_Tables = "Trans_AccountsLedger";
                string QueryPart_Field = "Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr)";
                string QueryPart_Where = "AccountsLedger_MainAccountID='" + MainAcCode[0, 0] + "' and AccountsLedger_FinYear='" +
                    Session["LastFinYear"] + @"' and 
            isnull(AccountsLedger_Currency,
            ((Select 
				(Select ExchangeSegment_TradeCurrencyID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID 
				and ExchangeSegment_Code=Exch_SegmentID) as [ExchangeSegmentID] 
				from (Select Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
				Where Exh_CntId=Exch_ExchID and 
				exch_compId= '" + PCompanyID + @"'
				and exch_internalId=" + PCurrentSegment + @"
                ) as T1,Master_Exchange
				Where Exchange_ShortName=Exh_ShortName)))=" + ChoosenCurrency.Split('~')[0];
                DataTable DtBankBalance = oDBEngine.GetDataTable(QueryPart_Tables, QueryPart_Field, QueryPart_Where);
                Converter oConverter = new Converter();
                if (DtBankBalance.Rows.Count > 0 && DtBankBalance.Rows[0][0].ToString() != String.Empty)
                {
                    string strColor = null;
                    string strDrCr = (Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString()) > 0) ? " Dr." : " Cr.";
                    strColor = (Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString()) > 0) ? "Blue" : "Red";
                    CbpDepstBankBalance.JSProperties["cpDepstBalance"] = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtBankBalance.Rows[0][0].ToString())) +
                        strDrCr + '~' + strColor;
                }
                else
                {
                    CbpDepstBankBalance.JSProperties["cpDepstBalance"] = "undefined";
                }

            }
        }
        protected void SCmbBranch_Callback(object source, CallbackEventArgsBase e)
        {
            Obj_Sds = new SqlDataSource();
            Obj_Sds.ConnectionString = strCon;
            ASPxComboBox Obj_ComboBoxE = (ASPxComboBox)Popup_Search.FindControl("SCmbBranch");
            Obj_Sds.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH";
            Bind_Combo(Obj_ComboBoxE, Obj_Sds, "BANKBRANCH_NAME", "BANKBRANCH_ID", 0);
            Obj_ComboBoxE.Items.Insert(0, new ListEditItem("Select"));
        }

        protected void GvCBSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "undefined";
            GvCBSearch.JSProperties["cpCBClose"] = null;
            GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = null;
            GvCBSearch.JSProperties["cpEntryEventFire"] = null;
            GvCBSearch.JSProperties["cpCBDelete"] = null;
            GvCBSearch.JSProperties["cpClearHiddenField"] = null;
            string command = e.Parameters.Split('~')[0];
            string strVoucherType = e.Parameters.Split('~')[1].Split('#')[0];
            string strQuery_NextPart = null;
            string strMainQuery = null;
            string strMainQueryPrefix = null;
            string strMainQuerySuffix = null;
            string PageNum = null, PageSize = null;
            DataSet DsSearch = new DataSet();
            GvCBSearch.Columns[0].FixedStyle = GridViewColumnFixedStyle.Left;
            GvCBSearch.Columns[1].FixedStyle = GridViewColumnFixedStyle.Left;
            GvCBSearch.Columns[2].FixedStyle = GridViewColumnFixedStyle.Left;
            if (command == "Search")
            {
                string SearchParam = e.Parameters.Split('#')[1];
                PageNum = e.Parameters.Split('~')[2].Split('#')[0];
                PageSize = GvCBSearch.SettingsPager.PageSize.ToString();
                string BranchID = SearchParam.Split('^')[0] != "Select" ? SearchParam.Split('^')[0] : String.Empty;
                DateTime TransactionDate = SDtTDate.Value != null ? Convert.ToDateTime(SDtTDate.Value) : Convert.ToDateTime("0100-01-01");
                string VoucherNumber = SearchParam.Split('^')[2], CashBankAc = SearchParam.Split('^')[3].Split('~')[0],
                        MainNarration = SearchParam.Split('^')[4], MainAc = SearchParam.Split('^')[5].Split('~')[0],
                        SubAc = SearchParam.Split('^')[6].Split('~')[0];
                string InstType = SearchParam.Split('^')[7] != "S" ? SearchParam.Split('^')[7] : String.Empty;
                string InstNo = SearchParam.Split('^')[8];
                DateTime Instdate = SDtInstdate.Value != null ? Convert.ToDateTime(SDtInstdate.Value) : Convert.ToDateTime("0100-01-01");
                string CustBank = SearchParam.Split('^')[10].Split('~')[0], IssueBank = SearchParam.Split('^')[11].Split('~')[0],
                        WithFrom = SearchParam.Split('^')[12].Split('~')[0], DepstInto = SearchParam.Split('^')[13].Split('~')[0],
                        AuthLetterRef = SearchParam.Split('^')[14], PayeeAc = SearchParam.Split('^')[15].Split('~')[0],
                        Payment = SearchParam.Split('^')[16], Recieve = SearchParam.Split('^')[17],
                        LineNarration = SearchParam.Split('^')[18];


                if (strVoucherType == "A")
                {
                    //strQuery_NextPart = " and CashBank_TransactionType in ('R','C','P')";
                    //strQuery_NextPart = strQuery_NextPart + (BranchID != String.Empty ? "and CashBank_BranchID='" + BranchID + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (TransactionDate.ToString("yyyy-MM-dd") != "0100-01-01" ? "and CashBank_TransactionDate='" + TransactionDate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstNo != String.Empty ? "and CashBankDetail_InstrumentNumber='" + InstNo + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstType != String.Empty ? "and CashBankDetail_InstrumentType='" + InstType + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Instdate.ToString("yyyy-MM-dd") != "0100-01-01" ? "and CashBankDetail_InstrumentDate='" + Instdate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (MainNarration != String.Empty ? " and CashBank_Narration='" + MainNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (LineNarration != String.Empty ? " and CashBankDetail_Narration='" + LineNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (VoucherNumber != String.Empty ? " and CashBank_VoucherNumber='" + VoucherNumber + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (CashBankAc != String.Empty ? " and CashBank_CashBankID='" + CashBankAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (MainAc != String.Empty ? " and CashBankDetail_MainAccountID=(Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_ReferenceID='" + MainAc + "')" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (SubAc != String.Empty ? " and CashBankDetail_SubAccountID='" + SubAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (CustBank != String.Empty ? " and CashBankDetail_ClientBankID='" + CustBank + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (IssueBank != String.Empty ? " and CashBankDetail_DraftIssueBankBranch='" + IssueBank + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (AuthLetterRef != String.Empty ? " and CashBankDetail_ThirdPartyReference='" + AuthLetterRef + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Recieve != "0.00" ? " and CashBankDetail_ReceiptAmount= Cast('" + Recieve + "' as money)" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Payment != "0.00" ? " and CashBankDetail_PaymentAmount=Cast('" + Payment + "' as money)" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (PayeeAc != String.Empty ? " and CashBankDetail_PayeeAccountID='" + PayeeAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (WithFrom != String.Empty ? " and CashBankDetail_MainAccountID=(Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_ReferenceID='" + WithFrom + "')" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (DepstInto != String.Empty ? " and CashBankDetail_MainAccountID=(Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_ReferenceID='" + DepstInto + "')" : String.Empty);
                    //GvCBSearch.Columns[2].Visible = true;
                    //GvCBSearch.Columns[4].Visible = true;
                }
                else if (strVoucherType == "R")
                {
                    //strQuery_NextPart = " and CashBank_TransactionType='R'";
                    //strQuery_NextPart = strQuery_NextPart + (BranchID != String.Empty ? "and CashBank_BranchID='" + BranchID + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (TransactionDate.ToString("yyyy-MM-dd") !="0100-01-01" ? "and CashBank_TransactionDate='" + TransactionDate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstNo != String.Empty ? "and CashBankDetail_InstrumentNumber='" + InstNo + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstType != String.Empty ? "and CashBankDetail_InstrumentType='" + InstType + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Instdate.ToString("yyyy-MM-dd") != "0100-01-01" ? "and CashBankDetail_InstrumentDate='" + Instdate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (MainNarration != String.Empty ? " and CashBank_Narration='" + MainNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (LineNarration != String.Empty ? " and CashBankDetail_Narration='" + LineNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (VoucherNumber != String.Empty ? " and CashBank_VoucherNumber='" + VoucherNumber + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (CashBankAc != String.Empty ? " and CashBank_CashBankID='" + CashBankAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (MainAc != String.Empty ? " and CashBankDetail_MainAccountID=(Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_ReferenceID='" + MainAc + "')" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (SubAc != String.Empty ? " and CashBankDetail_SubAccountID='" + SubAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (CustBank != String.Empty ? " and CashBankDetail_ClientBankID='" + CustBank + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (IssueBank != String.Empty ? " and CashBankDetail_DraftIssueBankBranch='" + IssueBank + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (AuthLetterRef != String.Empty ? " and CashBankDetail_ThirdPartyReference='" + AuthLetterRef + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Recieve != "0.00" ? " and CashBankDetail_ReceiptAmount= Cast('" + Recieve + "' as money)" : String.Empty);
                    //GvCBSearch.Columns[2].Visible = true;
                    //GvCBSearch.Columns[4].Visible = true;
                }
                else if (strVoucherType == "P")
                {
                    //strQuery_NextPart = " and CashBank_TransactionType='P'";
                    //strQuery_NextPart = strQuery_NextPart + (BranchID != String.Empty ? "and CashBank_BranchID='" + BranchID + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (TransactionDate.ToString("yyyy-MM-dd") != "0100-01-01" ? "and CashBank_TransactionDate='" + TransactionDate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstNo != String.Empty ? "and CashBankDetail_InstrumentNumber='" + InstNo + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstType != String.Empty ? "and CashBankDetail_InstrumentType='" + InstType + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Instdate.ToString("yyyy-MM-dd") != "0100-01-01" ? "and CashBankDetail_InstrumentDate='" + Instdate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (MainNarration != String.Empty ? " and CashBank_Narration='" + MainNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (LineNarration != String.Empty ? " and CashBankDetail_Narration='" + LineNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (VoucherNumber != String.Empty ? " and CashBank_VoucherNumber='" + VoucherNumber + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (CashBankAc != String.Empty ? " and CashBank_CashBankID='" + CashBankAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (MainAc != String.Empty ? " and CashBankDetail_MainAccountID=(Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_ReferenceID='" + MainAc + "')" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (SubAc != String.Empty ? " and CashBankDetail_SubAccountID='" + SubAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Payment != "0.00" ? " and CashBankDetail_PaymentAmount=Cast('" + Payment + "' as money)" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (PayeeAc != String.Empty ? " and CashBankDetail_PayeeAccountID='" + PayeeAc + "'" : String.Empty);
                    //GvCBSearch.Columns[2].Visible = true;
                    //GvCBSearch.Columns[4].Visible = true;
                }
                else
                {
                    //strQuery_NextPart = " and CashBank_TransactionType='C'";
                    //strQuery_NextPart = strQuery_NextPart + (BranchID != String.Empty ? "and CashBank_BranchID='" + BranchID + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (TransactionDate.ToString("yyyy-MM-dd") != "0100-01-01" ? "and CashBank_TransactionDate='" + TransactionDate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstNo != String.Empty ? "and CashBankDetail_InstrumentNumber='" + InstNo + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (InstType != String.Empty ? "and CashBankDetail_InstrumentType='" + InstType + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Instdate.ToString("yyyy-MM-dd") != "0100-01-01" ? "and CashBankDetail_InstrumentDate='" + Instdate.ToString("yyyy-MM-dd") + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (MainNarration != String.Empty ? " and CashBank_Narration='" + MainNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (LineNarration != String.Empty ? " and CashBankDetail_Narration='" + LineNarration + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (VoucherNumber != String.Empty ? " and CashBank_VoucherNumber='" + VoucherNumber + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (CashBankAc != String.Empty ? " and CashBank_CashBankID='" + CashBankAc + "'" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (WithFrom != String.Empty ? " and CashBankDetail_MainAccountID=(Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_ReferenceID='" + WithFrom + "')" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (DepstInto != String.Empty ? " and CashBankDetail_MainAccountID=(Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_ReferenceID='" + DepstInto + "')" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Recieve != "0.00" ? " and CashBankDetail_ReceiptAmount= Cast('" + Recieve + "' as money)" : String.Empty);
                    //strQuery_NextPart = strQuery_NextPart + (Payment != "0.00" ? " and CashBankDetail_PaymentAmount=Cast('" + Payment + "' as money)" : String.Empty);
                    //GvCBSearch.Columns[2].Visible = false;
                    //GvCBSearch.Columns[4].Visible = false;
                }
                //            string strCommanQueryPart = @"
                //                                    Select ROW_NUMBER()  OVER (ORDER BY  CashBank_ID) As [Srl. No],CashBank_ID as CBID,CashBank_CashBankID as CashBankID,Convert(Varchar,CashBank_TransactionDate,106) as TransactionDate,
                //                                    CashBank_VoucherNumber as VoucherNumber,
                //                                    Case
                //                                        When CashBank_TransactionType='P'
                //                                        Then ''
                //                                        Else Cast(CashBankDetail_ReceiptAmount as Varchar)
                //                                    End as ReceiptAmount,
                //                                    Case
                //                                        When CashBank_TransactionType='R'
                //                                        Then ''
                //                                        Else Cast(CashBankDetail_PaymentAmount as Varchar)
                //                                    End as PaymentAmount,CashBankDetail_InstrumentNumber as InstrumentNumber,
                //                                    (Select MainAccount_Name from Master_MainAccount Where MainAccount_AccountCode=CashBankDetail_MainAccountID) as MainAccountID,
                //                                   (Select 
                //                                    Case 
                //	                                    When (Select Count(SubAccount_Name) from Master_SubAccount where 
                //	                                    SubAccount_MainAcReferenceID=
                //	                                    (Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_AccountCode=CashBankDetail_MainAccountID) 
                //	                                    and SubAccount_Code = CashBankDetail_SubAccountID)=1
                //	                                    Then (Select SubAccount_Name from Master_SubAccount where SubAccount_MainAcReferenceID=
                //			                                    (Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_AccountCode=CashBankDetail_MainAccountID) 
                //			                                    and SubAccount_Code=CashBankDetail_SubAccountID)
                //	                                    Else ''
                //                                    End) as SubAccountID,
                //                                    CashBank_Narration as Narration,
                //                                    CashBank_ExchangeSegmentID as ExchangeSegmentID,
                //                                    CashBank_IBRef as IBRef,(Select Count(*) from Trans_CashBankVouchers,Trans_CashBankDetail
                //                                    where CashBank_IBRef=CashBankDetail_IBRef
                //                                    and CashBank_CompanyID='" + Session["LastCompany"].ToString() + @"'
                //                                    and CashBank_FinYear='" + Session["LastFinYear"].ToString() + "'"
                //                                    + strQuery_NextPart + @") as TotalRow,CashBankDetail_BankValueDate as ValueDate
                //                                    from Trans_CashBankVouchers,Trans_CashBankDetail
                //                                    where CashBank_IBRef=CashBankDetail_IBRef
                //                                    and CashBank_CompanyID='" + Session["LastCompany"].ToString() + @"'
                //                                    and CashBank_FinYear='" + Session["LastFinYear"].ToString() + "'";
                //            strMainQuery = strCommanQueryPart + strQuery_NextPart;
                //            Session["strMainQuery"] = strMainQuery;

                //            string []strSearchBy = strQuery_NextPart.Replace("and","~").Split('~');
                //            string strSplitSearchBy=String.Empty;
                //            foreach (string str in strSearchBy)
                //            {
                //                if (str.Trim() != String.Empty)
                //                    strSplitSearchBy = strSplitSearchBy + str.Split('_')[1].Split('=')[0] + ",";

                //            }
                //            Session["strSearchBy"] = strSplitSearchBy;
                //            strMainQueryPrefix = "Select * from (";
                //            strMainQuerySuffix = @") as TempTable
                //                                    WHERE [Srl. No] BETWEEN (" + PageNum + "- 1) * " + PageSize + @" + 1 AND 
                //                                    " + PageNum + "* " + PageSize;
                //            strMainQuery = strMainQueryPrefix + strMainQuery + strMainQuerySuffix;
                //            Session["strSearchByMainQuery"] = strMainQuery;
                //            DsSearch = GetDsUsingQuery(strMainQuery);
                if (DsSearch.Tables.Count > 0)
                {
                    if (DsSearch.Tables[0].Rows.Count > 0)
                    {
                        //BindGrid(GvCBSearch, DsSearch);
                        //int TotalItems =Convert.ToInt32(DsSearch.Tables[0].Rows[0]["TotalRow"].ToString());
                        //int TotalPage = TotalItems % Convert.ToInt32(PageSize) == 0 ? (TotalItems / Convert.ToInt32(PageSize)) : (TotalItems / Convert.ToInt32(PageSize)) + 1;
                        //GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "No~1~" + TotalPage + "~" + TotalItems + '~' + strSplitSearchBy.Substring(0, strSplitSearchBy.LastIndexOf(','));

                    }
                    else
                    {
                        GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                        Session["strMainQuery"] = null;
                        Session["strSearchBy"] = null;
                        Session["strSearchByMainQuery"] = null;
                    }
                }
                else
                {
                    GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                    Session["strMainQuery"] = null;
                    Session["strSearchBy"] = null;
                    Session["strSearchByMainQuery"] = null;
                }
                GvCBSearch.JSProperties["cpCBClose"] = null;
                GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = null;
                GvCBSearch.JSProperties["cpEntryEventFire"] = null;
                GvCBSearch.JSProperties["cpCBDelete"] = null;
            }
            if (command == "SearchByNavigation")
            {
                strMainQuery = null;
                PageNum = e.Parameters.Split('~')[1];
                PageSize = GvCBSearch.SettingsPager.PageSize.ToString();
                if (Session["strMainQuery"] != null)
                {
                    strMainQuery = Session["strMainQuery"].ToString();
                    strMainQueryPrefix = "Select * from (";
                    strMainQuerySuffix = @") as TempTable
                                    WHERE [Srl. No] BETWEEN (" + PageNum + "- 1) * " + PageSize + @" + 1 AND 
                                    " + PageNum + "* " + PageSize;
                    strMainQuery = strMainQueryPrefix + strMainQuery + strMainQuerySuffix;
                    //DsSearch = GetDsUsingQuery(strMainQuery);
                    if (DsSearch.Tables.Count > 0)
                    {
                        if (DsSearch.Tables[0].Rows.Count > 0)
                        {
                            //BindGrid(GvCBSearch, DsSearch);
                            //int TotalItems = Convert.ToInt32(DsSearch.Tables[0].Rows[0]["TotalRow"].ToString());
                            //int TotalPage = TotalItems % Convert.ToInt32(PageSize) == 0 ? (TotalItems / Convert.ToInt32(PageSize)) : (TotalItems / Convert.ToInt32(PageSize)) + 1;
                            //string strSplitSearchBy = Session["strSearchBy"] != null ? Session["strSearchBy"].ToString() : String.Empty;
                            //GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "No~" + PageNum + '~' + TotalPage + "~" + TotalItems + '~' + strSplitSearchBy.Substring(0, strSplitSearchBy.LastIndexOf(','));

                        }
                        else
                        {
                            GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                            Session["strMainQuery"] = null;
                            Session["strSearchBy"] = null;
                            Session["strSearchByMainQuery"] = null;
                        }
                    }
                    else
                    {
                        GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                        Session["strMainQuery"] = null;
                        Session["strSearchBy"] = null;
                        Session["strSearchByMainQuery"] = null;
                    }
                }
                GvCBSearch.JSProperties["cpCBClose"] = null;
                GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = null;
                GvCBSearch.JSProperties["cpEntryEventFire"] = null;
                GvCBSearch.JSProperties["cpCBDelete"] = null;
            }
            DataSet Ds_CBE;
            string IBRef;
            string CBEFile_XMLPATH;
            string SelectedVoucherType;
            if (command == "PCB_BtnOkE")
            {
                if (Session["IBRef"] != null && Session["VoucherNumber"] != null)
                {
                    IBRef = Session["IBRef"].ToString();
                    SelectedVoucherType = Session["VoucherNumber"].ToString().Split('/')[0];
                    string RequiredParameterForEditing = CreateCBE_XMLFile(IBRef, SelectedVoucherType);
                    if (ViewState["CBE_FileAlreadyUsedBy"] != null)
                    {
                        if (ViewState["CBE_FileAlreadyUsedBy"].ToString().Split('~')[1] == Session["userid"].ToString())
                        {
                            GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = "HimSelef~";
                        }
                        else
                        {
                            GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = "Other~" + ViewState["CBE_FileAlreadyUsedBy"].ToString();
                        }
                        GvCBSearch.JSProperties["cpEntryEventFire"] = null;

                    }
                    else
                    {
                        BindGrid(GvCBSearch);
                        GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = null;
                        GvCBSearch.JSProperties["cpEntryEventFire"] = RequiredParameterForEditing;
                    }
                    GvCBSearch.JSProperties["cpCBDelete"] = null;
                    GvCBSearch.JSProperties["cpCBClose"] = null;
                    GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "undefined";
                }
            }
            if (command == "PCB_BindAfterDelete")
            {

            }
            if (command == "PCB_ContinueWith")
            {
                if (Session["IBRef"] != null && Session["VoucherNumber"] != null)
                {
                    IBRef = Session["IBRef"].ToString();
                    CBEFile_XMLPATH = "../Documents/" + "CBE_" + IBRef;

                    if (File.Exists(Server.MapPath(CBEFile_XMLPATH)))
                    {
                        string RequiredParameterForEditing = Fetch_CBE_XMLFileData(IBRef);
                        //BindGrid(GvCBSearch);
                        GvCBSearch.JSProperties["cpEntryEventFire"] = RequiredParameterForEditing;
                        //This For Log Purpose
                        string strLogID = ViewState["LogID"].ToString();
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                    }
                }
                GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = null;
                GvCBSearch.JSProperties["cpCBDelete"] = null;
                GvCBSearch.JSProperties["cpCBClose"] = null;
                GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "undefined";
            }
            if (command == "PCB_FreshEntry")
            {
                if (Session["IBRef"] != null && Session["VoucherNumber"] != null)
                {
                    IBRef = Session["IBRef"].ToString();
                    SelectedVoucherType = Session["VoucherNumber"].ToString().Split('/')[0];
                    CBEFile_XMLPATH = "../Documents/" + "CBE_" + IBRef;
                    Ds_CBE = new DataSet();
                    try
                    {
                        File.Delete(Server.MapPath(CBEFile_XMLPATH));
                        //BindGrid(GvCBSearch);`
                        string RequiredParameterForEditing = CreateCBE_XMLFile(IBRef, SelectedVoucherType);
                        GvCBSearch.JSProperties["cpEntryEventFire"] = RequiredParameterForEditing;
                        //This For Log Purpose
                        string strLogID = ViewState["LogID"].ToString();
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_CashBankVouchers", strLogID, GenericLogSystem.LogType.CB);
                        oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_CashBankDetail", strLogID, GenericLogSystem.LogType.CB);
                    }
                    catch
                    {
                        File.Delete(Server.MapPath(CBEFile_XMLPATH));
                    }
                }
                GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = null;
                GvCBSearch.JSProperties["cpCBDelete"] = null;
                GvCBSearch.JSProperties["cpCBClose"] = null;
                GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "undefined";
            }
            if (command == "PCB_CloseEntry")
            {
                if (Session["IBRef"] != null && Session["VoucherNumber"] != null)
                {
                    IBRef = Session["IBRef"].ToString();
                    CBEFile_XMLPATH = "../Documents/" + "CBE_" + IBRef;
                    if (File.Exists(Server.MapPath(CBEFile_XMLPATH)))
                    {
                        File.Delete(Server.MapPath(CBEFile_XMLPATH));
                    }
                    Session["IBRef"] = null;
                }
                GvCBSearch.JSProperties["cpCBClose"] = "File Successfully Close";
                GvCBSearch.JSProperties["cpCBE_FileAlreadyUsedBy"] = null;
                GvCBSearch.JSProperties["cpEntryEventFire"] = null;
                GvCBSearch.JSProperties["cpCBDelete"] = null;
                GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "undefined";
            }
            if (command == "SearchByLink")
            {
                if (Session["strSearchByMainQueryInstance"] != null)
                {
                    Session["strSearchByMainQuery"] = Session["strSearchByMainQueryInstance"];
                    Session["strSearchByMainQueryInstance"] = null;
                }

                if (Session["strSearchByMainQuery"] != null)
                {
                    string strQuery = Session["strSearchByMainQuery"].ToString();
                    Page_RefreshAllSessionOrHiddenField();
                    DataSet DsSearchByLink = new DataSet();
                    if (DsSearchByLink.Tables.Count > 0) { DsSearchByLink.Tables.Remove(DsSearchByLink.Tables[0]); DsSearchByLink.Clear(); }
                    DsSearchByLink = new DataSet();
                    DsSearchByLink = Bind_Combo(strQuery);
                    if (DsSearchByLink.Tables.Count > 0)
                    {
                        if (DsSearchByLink.Tables[0].Rows.Count > 0)
                        {
                            //BindGrid(GvCBSearch, DsSearchByLink);
                        }
                        else
                        {
                            // BindGrid(GvCBSearch);
                        }
                    }
                    else
                    {
                        BindGrid(GvCBSearch);
                    }
                    DsSearchByLink.Dispose();
                }
            }
            if (command == "ClearSession")
            {
                if (Session["strSearchByMainQuery"] != null)
                {
                    Session["strSearchByMainQueryInstance"] = Session["strSearchByMainQuery"];
                }
                Page_RefreshAllSessionOrHiddenField();
                GvCBSearch.JSProperties["cpClearHiddenField"] = "ClearAll~" + e.Parameters.Split('~')[1];
            }

        }
        protected void CbpPayee_Callback(object source, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string strPayeeAcID = e.Parameter.Split('~')[1];
            string strQuery = @"Select '0' as cnt_internalId,'Select' as Payee union Select cnt_internalId,isnull(ltrim(rtrim(cnt_firstName)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'')+' ['+isnull(cnt_shortname,'')+']' as Payee from tbl_master_contact where cnt_internalId like 'VR%'";
            if (strSplitCommand == "BindPayeeComboWithValue")
            {
                DataSet DsPayee = Bind_Combo(strQuery);
                Bind_Combo_WithSelectedValue(CmbPayee, DsPayee, "Payee", "cnt_internalId", (object)strPayeeAcID);
            }
        }

        protected void GvAddRecordDisplay_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            GvAddRecordDisplay.JSProperties["cpOneTagEntry"] = "undefined~NotExists";
            if (hdn_Mode.Value != "Edit") return;
            object ValueDate = GvAddRecordDisplay.GetRowValues(e.VisibleIndex, "ValueDate");
            if (ValueDate != null)
            {
                if (ValueDate.ToString() == String.Empty || ValueDate.ToString() == "01-01-0001") return;
                if (Convert.ToDateTime(ValueDate).ToString("MM-dd-yyyy") != "01-01-1900")
                {
                    e.Visible = false;
                    GvAddRecordDisplay.JSProperties["cpOneTagEntry"] = "defined~Exists";
                }
            }
        }
        private void Page_RefreshAllSessionOrHiddenField()
        {
            Session["strMainQuery"] = null;
            Session["strSearchBy"] = null;
            Session["strSearchByMainQuery"] = null;
            Session["IBRef"] = null;
            Session["Mode"] = null;
            Session["CashBankVoucherFile_XMLPATH"] = null;
            Session["ChoosenCurrency"] = null;
            hdnDefaultBranch.Value = null;
            hdnType.Value = null;
            hdnAccountType.Value = null;
            txtMainAccount_hidden.Value = null;
            txtSubAccount_hidden.Value = null;
            hdn_Brch_NonBrch.Value = null;
            hdn_SubLedgerType.Value = null;
            hdn_MainAcc_Type.Value = null;
            hdn_SubAccountIDE.Value = null;
            txtBankAccounts_hidden.Value = null;
            hdn_Mode.Value = null;
            hdn_PayeeIDOnUpdate.Value = null;
            hdn_Brch_NonBrchE.Value = null;
            StxtPayeeAc_hidden.Value = null;
            StxtDepstInto_hidden.Value = null;
            StxtWithFrom_hidden.Value = null;
            StxtIssueBank_hidden.Value = null;
            StxtCustBank_hidden.Value = null;
            StxtSubAccount_hidden.Value = null;
            StxtMainAccount_hidden.Value = null;
            StxtCashBankAc_hidden.Value = null;
            txtDepositInto_hidden.Value = null;
            txtWithFrom_hidden.Value = null;
            txtIssuingBank_hidden.Value = null;
            hdn_CashBankType_InstTypeE.Value = null;
        }
        protected void GvAddRecordDisplay_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
        {
            //if (e.Column.FieldName == "TotalRecPay")
            //{
            //    if (e.GetListSourceFieldValue("ReceiptAmount").ToString().Trim() != String.Empty && e.GetListSourceFieldValue("PaymentAmount").ToString().Trim() != String.Empty)
            //    {
            //        TotalRecieve = TotalRecieve + Convert.ToDecimal(e.GetListSourceFieldValue("ReceiptAmount"));
            //        TotalPayment = TotalPayment + Convert.ToDecimal(e.GetListSourceFieldValue("PaymentAmount"));
            //    }
            //}
        }

        protected void CbpChoosenCurrency_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = null;

            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "ChangeCurrency")
            {
                string ActiveCurrencyID = String.Empty;
                string ActiveCurrencyName = String.Empty;
                string ActiveCurrencySymbol = String.Empty;
                string CurrencyInXmlFile = String.Empty;
                string ActiveCurrency = Session["ActiveCurrency"].ToString();
                string TradeCurrency = Session["TradeCurrency"].ToString();
                string LocalCurrency = Session["LocalCurrency"].ToString();

                //Change ChoosenCurrency In XmlFile if Exists
                if (File.Exists(Server.MapPath(PXMLPATH)))
                {
                    DataSet DsChangeChooseCurrency = new DataSet();
                    DsChangeChooseCurrency.ReadXml(Server.MapPath(PXMLPATH));
                    CurrencyInXmlFile = DsChangeChooseCurrency.Tables[0].Rows[0]["ChoosenCurrency"].ToString();
                    if (CurrencyInXmlFile != ActiveCurrency)
                    {
                        if (ActiveCurrency == TradeCurrency)
                        {
                            ActiveCurrencyID = TradeCurrency.Split('~')[0];
                            ActiveCurrencyName = TradeCurrency.Split('~')[1];
                            ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
                            Session["ActiveCurrency"] = TradeCurrency;
                        }
                        else
                        {
                            ActiveCurrencyID = LocalCurrency.Split('~')[0];
                            ActiveCurrencyName = LocalCurrency.Split('~')[1];
                            ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
                            Session["ActiveCurrency"] = LocalCurrency;
                        }
                        for (int i = 0; i < DsChangeChooseCurrency.Tables[0].Rows.Count; i++)
                        {
                            DsChangeChooseCurrency.Tables[0].Rows[i]["ChoosenCurrency"] = Session["ActiveCurrency"].ToString();
                        }
                        DsChangeChooseCurrency.AcceptChanges();
                        File.Delete(Server.MapPath(PXMLPATH));
                        DsChangeChooseCurrency.WriteXml(Server.MapPath(PXMLPATH));
                        DsChangeChooseCurrency.Dispose();
                        CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
                    }
                    else
                    {
                        CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = CurrencyInXmlFile.Split('~')[1] + '~' + CurrencyInXmlFile.Split('~')[2];

                    }
                }
                else
                {
                    if (ActiveCurrency == TradeCurrency)
                    {
                        ActiveCurrencyID = TradeCurrency.Split('~')[0];
                        ActiveCurrencyName = TradeCurrency.Split('~')[1];
                        ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
                        Session["ActiveCurrency"] = TradeCurrency;
                    }
                    else
                    {
                        ActiveCurrencyID = LocalCurrency.Split('~')[0];
                        ActiveCurrencyName = LocalCurrency.Split('~')[1];
                        ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
                        Session["ActiveCurrency"] = LocalCurrency;
                    }
                    CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
                }
            }
        }

    }
}


