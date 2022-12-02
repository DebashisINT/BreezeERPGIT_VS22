using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Management.DailyTask
{

    public partial class management_DailyTask_frm_Account_Transfer : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        Transaction oTransaction = new Transaction();
        public string dp;
        DataTable dt = new DataTable();
        public string usedDate;
        public string cancelledDate;
        string MainAcId;
        DataTable dtExchSegId = new DataTable();
        DataTable dtBal = new DataTable();
        DataSet datatemp = new DataSet();
        public string sourceex = "";
        string data;
        int DetailGridCounter = 1;
        DataSet ds = new DataSet();

        clsNsdlHolding objclsNsdlHolding = new clsNsdlHolding();
        clsCdslHolding objclsCdslHolding = new clsCdslHolding();
        #region pdf var

        static DataTable pdfdt = new DataTable();
        static DataTable pdfdt1 = new DataTable();
        static string BenType;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
        static decimal VALUE_Sum;
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        string[] InputName = new string[6];
        string[] InputType = new string[6];
        string[] InputValue = new string[6];

        #endregion
        #region Display_Signature
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        public static string ServerVaraible1;
        DataTable DT = new DataTable();
        public static string tran, dpId;
        int quantity = 1;
        int Counter;
        #endregion

        #region Page Property
        public int PCounter
        {
            get { return (int)ViewState["Counter"]; }
            set { ViewState["Counter"] = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////________This script is for firing javascript when page load first___//
            //if (!ClientScript.IsStartupScriptRegistered("Today"))
            //    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");

            ServerVaraible1 = Session["usersegid"].ToString();


            dpId = HttpContext.Current.Session["usersegid"].ToString();

            txtdpid.Attributes.Add("onkeyup", "CallAjaxdpid(this,'Searchdpid',event)");

            txtAccountNo.Attributes.Add("onkeyup", "CallAjaxclient(this,'Searchclientid',event)");
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff1", "<script>PageLoad1();</script>");
                //dttran.Value = oDBEngine.GetDate();
                //dtexec.Value = oDBEngine.GetDate();
                dttran.Value = oDBEngine.GetDate();
                dtexec.Value = oDBEngine.GetDate();
                DataTable dtname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                ViewState["CompanyName"] = dtname.Rows[0]["cmp_Name"].ToString();

            }
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff1", "<script>PageLoad1();</script>");

            //dp = Request.QueryString["dp"].ToString();

            if (dpId.Substring(0, 2) == "IN")
                Session["dp"] = "NSDL";
            else
                Session["dp"] = "CDSL";
            dp = Session["dp"].ToString();
            bind();
            DataTable dtExchSegId = oDBEngine.GetDataTable("tbl_master_companyexchange, tbl_master_segment", "exch_internalid", "exch_membershipType = seg_name and seg_id ='" + HttpContext.Current.Session["userlastsegment"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
            ViewState["dtExchSegId"] = dtExchSegId.Rows[0][0].ToString();

            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            string dpId = HttpContext.Current.Session["usersegid"].ToString();

            //DataTable dtCheckSlip = oDBEngine.GetDataTable("trans_dpslipsusage", "dpslipsusage_Status", "dpslipsusage_DPID='" + dpId + "' and dpslipsusage_BenID='" + txtAccountNo.Text.Trim().Split(' ')[0] + "' and dpslipsusage_SlipNumber='" + txtSlip.Text.Trim() + "' and dpslipsusage_SlipType=" + ddlSlipType.SelectedItem.Value);
            //if (dtCheckSlip.Rows.Count > 0)
            //{
            //if (dtCheckSlip.Rows[0][0].ToString() == "0")
            //{

            //using (SqlConnection objCon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand objCmd = new SqlCommand("Fetch_DetailsForAccountTransfer", objCon))
            //    {
            //objCmd.CommandType = CommandType.StoredProcedure;
            //objCmd.Parameters.AddWithValue("@dp", dp);
            //objCmd.Parameters.AddWithValue("@dpId", dpId);
            //objCmd.Parameters.AddWithValue("@benaccno", txtAccountNo.Text.Trim().Split(' ')[0]);//10000004

            dt = oTransaction.FetchDetailsForAccountTransfer(dp, dpId, txtAccountNo.Text.Trim().Split(' ')[0]);

            //using (SqlDataAdapter objDap = new SqlDataAdapter(objCmd))
            //{
            //dt.Clear();
            //objDap.Fill(dt);
            if (dp == "NSDL")
                MainAcId = "SYSTM00043";
            else if (dp == "CDSL")
                MainAcId = "SYSTM00042";
            if (dt.Rows.Count > 0)
            {
                ViewState["SlipNumber"] = "AcClose-" + txtAccountNo.Text.Trim().Split(' ')[0];
                txtClient.Text = dt.Rows[0]["clientid"].ToString();
                dtExchSegId = oDBEngine.GetDataTable("tbl_master_companyexchange, tbl_master_segment", "exch_internalid", "exch_membershipType = seg_name and seg_id ='" + HttpContext.Current.Session["userlastsegment"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
                if (dp == "NSDL")
                {
                    ViewState["BenAccountNumber"] = dt.Rows[0]["NsdlClients_BenAccountId"].ToString();
                    //dtBal = oDBEngine.OpeningBalance(MainAcId, dt.Rows[0]["NsdlClients_BenAccountId"].ToString(), oDBEngine.GetDate(), dtExchSegId.Rows[0][0].ToString(), HttpContext.Current.Session["LastCompany"].ToString());
                    dtBal = oDBEngine.OpeningBalance(MainAcId, dt.Rows[0]["NsdlClients_BenAccountId"].ToString(), oDBEngine.GetDate(), dtExchSegId.Rows[0][0].ToString(), HttpContext.Current.Session["LastCompany"].ToString());
                    lblSecondHolderName.Text = dt.Rows[0]["NsdlClients_BenSecondHolderName"].ToString();
                    lblThirdHolderName.Text = dt.Rows[0]["NsdlClients_BenThirdHolderName"].ToString();
                    ViewState["branchid"] = dt.Rows[0]["NsdlClients_BranchID"].ToString();
                }
                else if (dp == "CDSL")
                {
                    ViewState["BenAccountNumber"] = dt.Rows[0]["CdslClients_BenAccountNumber"].ToString();
                    //dtBal = oDBEngine.OpeningBalance(MainAcId, dt.Rows[0]["CdslClients_BenAccountNumber"].ToString(), oDBEngine.GetDate(), dtExchSegId.Rows[0][0].ToString(), HttpContext.Current.Session["LastCompany"].ToString());
                    dtBal = oDBEngine.OpeningBalance(MainAcId, dt.Rows[0]["CdslClients_BenAccountNumber"].ToString(), oDBEngine.GetDate(), dtExchSegId.Rows[0][0].ToString(), HttpContext.Current.Session["LastCompany"].ToString());
                    lblSecondHolderName.Text = dt.Rows[0]["CdslClients_SecondHolderName"].ToString();
                    lblThirdHolderName.Text = dt.Rows[0]["CdslClients_ThirdHolderName"].ToString();
                    ViewState["branchid"] = dt.Rows[0]["CdslClients_BranchID"].ToString();

                }
                txtTransDate.Text = dt.Rows[0]["LastTransDate"].ToString();
                txtBal.Text = dtBal.Rows[0][0].ToString();
                if (txtBal.Text != "")
                {
                    if (Convert.ToDouble(txtBal.Text) < 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "DebitExistn", "<script>ShowDebitExist();</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "DebitExistp", "<script>HidDebitExist();</script>");

                    }

                }

                lbl_sdclientid.Text = dt.Rows[0]["clientid"].ToString();
                lbl_sdedate.InnerText = Convert.ToDateTime(dtexec.Value.ToString()).ToString("dd MMM yyyy");
                lbl_sdtdate.Text = Convert.ToDateTime(dttran.Value.ToString()).ToString("dd MMM yyyy");
                // lbl_sdsliptype.Text = ddlSlipType.SelectedItem.Text;
                //lbl_sdsliptype.Text = "";//acccountno
                //lbl_sdslipno.Text = ViewState["SlipNumber"].ToString();

                hiddenbenid.Value = dt.Rows[0]["clientid"].ToString();

                gridSign.DataSource = dt;
                gridSign.DataBind();
                Page.ClientScript.RegisterStartupScript(GetType(), "Showslipdetails", "<script>AfterShowClickView_IfSlipExist();</script>");
                display_signature_load();

                if (txtTransDate.Text != "")
                {
                    //TimeSpan UserAccDormancy = oDBEngine.GetDate().Subtract(Convert.ToDateTime(txtTransDate.Text));
                    TimeSpan UserAccDormancy = oDBEngine.GetDate().Subtract(Convert.ToDateTime(txtTransDate.Text));
                    int UserAccDormancyDay = UserAccDormancy.Days;
                    if (Check_AccountNo_Dormancy(UserAccDormancyDay))
                    {
                        lbldormantStatus.InnerText = "This Account is Dormant for " + UserAccDormancyDay + " Days";
                    }
                    else
                    {
                        lbldormantStatus.InnerText = "";
                    }
                }
                else
                {
                    lbldormantStatus.InnerText = "";
                }
            }
            else
            {
                dt.Clear();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record Not Exist');", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff1", "<script>PageLoad1();</script>");



            }

            //}


            //    }

            //}
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "checkslip", "<script>alert('Slip already in used');</script>");

            //}
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "noslip", "<script>alert('Invalid slip');</script>");
            //}
        }
        Boolean Check_AccountNo_Dormancy(int UserAccDormancy)
        {
            DataTable MaxLimit_Dormancy = new DataTable();
            string Where = null;
            if (ViewState["dtExchSegId"] != null)
            {
                Where = "GlobalSettings_segmentID='" + ViewState["dtExchSegId"].ToString() + "' and GlobalSettings_Name='GS_Dormancy'";
                MaxLimit_Dormancy = oDBEngine.GetDataTable("Config_globalSettings", "GlobalSettings_Value", Where);
                if (Convert.ToInt32(MaxLimit_Dormancy.Rows[0][0].ToString()) < UserAccDormancy)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        protected void DetailsGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "ConfirmHighTran")
            {
                ViewState["ConfirmHighTran"] = Check_Account_HighValue();
            }
            if (e.Parameters == "Add")
            {
                //AddData_ToGrid();
                DataSet ds = AddToData();
                DetailsGrid.DataSource = ds;
                DetailsGrid.DataBind();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
                if (ds == null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "nodatamsg", "<script>alert('No data exists');</script>");
                }

            }
            if (e.Parameters == "Save")
            {
                SaveRecords();
                //Save_Records();
            }
            if (e.Parameters == "CancelAll")
            {
                Cancel_All();
            }
            if (e.Parameters == "Cancel")
            {
                Cancel();

            }
        }
        protected void DetailsGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }
        protected void DetailsGrid_CustomJSProperties1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //Converter Oconverter = new Converter();
            //if (ViewState["ConfirmHighTran"] != null)
            //{
            //    if (Convert.ToBoolean(ViewState["ConfirmHighTran"].ToString().Split('~')[0]))
            //        e.Properties["cpretValue"] = "High Value Transaction~" + Oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(ViewState["ConfirmHighTran"].ToString().Split('~')[1]));
            //    else
            //        e.Properties["cpretValue"] = "Normal Transaction";
            //}
            //else
            //{
            //    if (ViewState["RowAffected"] != null)
            //    {
            //        e.Properties["cpretValue"] = ViewState["RowAffected"].ToString();
            //    }
            //    else
            //    {
            //        e.Properties["cpretValue"] = null;
            //    }
            //}
        }
        protected void dtexec_DateChanged(object sender, EventArgs e)
        {
            //Session["executiondate"] = dtexec.Text;

        }


        protected void cmbholding_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

        }
        protected void cmbholding_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            if (ViewState["holding"] != null)
                e.Properties["cpretValue"] = ViewState["holding"].ToString();
        }
        void display_signature_load()
        {
            string exc = null;
            DataTable dtmarketfrom = new DataTable();
            if (ViewState["BenAccountNumber"] != null)
            {
                if (Session["dp"].ToString() == "NSDL")
                {
                    DataTable dtExchange = new DataTable();
                    dtExchange = oDBEngine.GetDataTable("master_nsdlclients", "nsdlclients_correspondingbpid", "NsdlClients_BenAccountCategory=3 and nsdlclients_benaccountid='" + ViewState["BenAccountNumber"].ToString() + "'");
                    if (dtExchange.Rows.Count > 0)
                    {
                        exc = dtExchange.Rows[0][0].ToString();
                    }
                    else
                    {
                        exc = String.Empty;
                    }
                }
                else
                {
                    string boid = HttpContext.Current.Session["usersegid"].ToString() + ViewState["BenAccountNumber"].ToString();
                    exc = oDBEngine.GetDataTable("master_cdslclients", "cdslClients_Exchangeid", "cdslclients_boid='" + boid + "'").Rows[0][0].ToString();
                }
            }

            try
            {
                if (exc != null)
                {
                    ddltran.Items.Clear();
                    if (Session["dp"].ToString() == "NSDL")
                    {

                        ddltran.Items.Add(new ListItem("Select", "0"));
                        ddltran.Items.Add(new ListItem("Off-Market", "2"));
                        ddltran.Items.Add(new ListItem("Inter-Depository(OffMkt.)", "4"));
                        sourceex = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_associatedccid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=3 and nsdlbplist_bpid='" + exc + "'").Rows[0][1].ToString();
                        ViewState["sourceex"] = sourceex;
                    }
                    else
                    {

                        ddltran.Items.Add(new ListItem("Select", "0"));
                        ddltran.Items.Add(new ListItem("Off-Market", "2"));
                        ddltran.Items.Add(new ListItem("Inter-Depository(OffMkt.)", "4"));
                        sourceex = exc;
                        ViewState["sourceex"] = sourceex;
                    }
                    dtmarketfrom = Fetch_MarketType(sourceex);


                }
                else
                {
                    ddltran.Items.Clear();
                    //ddltran.Items.Add(new ListItem("Select", "0"));
                    //ddltran.Items.Add(new ListItem("Market", "1"));
                    //ddltran.Items.Add(new ListItem("Off-Market", "2"));
                    //ddltran.Items.Add(new ListItem("Inter-Depository(Mkt.)", "3"));
                    //ddltran.Items.Add(new ListItem("Inter-Depository(OffMkt.)", "4"));
                    ddltran.Items.Add(new ListItem("Select", "0"));
                    ddltran.Items.Add(new ListItem("Off-Market", "2"));
                    ddltran.Items.Add(new ListItem("Inter-Depository(OffMkt.)", "4"));


                    //sourceex = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_associatedccid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=1 and nsdlbplist_bpid='" + dt.Rows[0]["id"].ToString() + "'").Rows[0][1].ToString();
                }
            }
            catch { }


        }

        DataTable Fetch_MarketType(string ccid)
        {
            string CCIDConvertCDSLExch = "";
            string ExcIDConvertCCId = "";
            string where = null;
            if (Session["dp"].ToString() == "NSDL")
            {
                if (ccid == "12")
                {
                    ExcIDConvertCCId = "IN001002";
                }
                else if (ccid == "11")
                {
                    ExcIDConvertCCId = "IN001019";
                }
                else if (ccid == "13")
                {
                    ExcIDConvertCCId = "IN001027";
                }
                else
                {
                    ExcIDConvertCCId = ccid;
                }

                where = "NsdlMarketType_CCID = '" + ExcIDConvertCCId + "'";
                return oDBEngine.GetDataTable("Master_NsdlMarketTypes", "NsdlMarketType_dpmcode , NsdlMarketType_Description  ", where);

            }
            else
            {
                if (ccid == "IN001002")
                {
                    CCIDConvertCDSLExch = "12";
                }
                else if (ccid == "IN001019")
                {
                    CCIDConvertCDSLExch = "11";
                }
                else if (ccid == "IN001027")
                {
                    CCIDConvertCDSLExch = "13";
                }
                else
                {
                    CCIDConvertCDSLExch = ccid;
                }
                where = "CdslMarketTypes_ExchangeID = '" + CCIDConvertCDSLExch + "'";
                return oDBEngine.GetDataTable("Master_cdslMarketTypes", "cdslMarketTypes_typeid as NsdlMarketType_dpmcode, cdslMarketTypes_Description as NsdlMarketType_Description  ", where);
            }

        }

        string Check_Account_HighValue()
        {
            DataTable ISIN_FaceValue = new DataTable();
            DataTable DtHighValue = new DataTable();
            DataSet DsTemp = new DataSet();
            string Where = null;
            double HighValue = 0.00;
            double Isin_Qty_Product = 0.00;
            string NSDLCDSL_XMLPATH = null;
            if (Session["dp"].ToString() == "NSDL")
                NSDLCDSL_XMLPATH = "../Documents/" + "accountno" + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "nsdl";//"../Documents/" + ddlSlipType.SelectedItem.Text + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "nsdl";
            else
                NSDLCDSL_XMLPATH = "../Documents/" + "accountno" + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "cdsl";//"../Documents/" + ddlSlipType.SelectedItem.Text + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "cdsl";

            if (ViewState["dtExchSegId"] != null)
            {
                if (Session["dp"].ToString() == "NSDL")
                {
                    Where = "GlobalSettings_segmentID='" + ViewState["dtExchSegId"].ToString() + "' and GlobalSettings_Name='GS_HIGHVALUETRNDP'";
                    DtHighValue = oDBEngine.GetDataTable("Config_globalSettings", "GlobalSettings_Value", Where);
                    if (DtHighValue.Rows.Count > 0)
                    {
                        HighValue = Convert.ToDouble(DtHighValue.Rows[0][0].ToString());
                        if (File.Exists(Server.MapPath(NSDLCDSL_XMLPATH)))
                        {
                            DsTemp.ReadXml(Server.MapPath(NSDLCDSL_XMLPATH));
                            foreach (DataRow dr in DsTemp.Tables[0].Rows)
                            {
                                Where = "DPPRice_DP='NSDL' and DPPrice_ISIN='" + dr["isin"].ToString() + "' and DPPrice_Date=(Select max(DPPrice_Date) from trans_dpprice where DPPRice_DP='NSDL' and DPPrice_ISIN='" + dr["isin"].ToString() + "')";
                                ISIN_FaceValue = oDBEngine.GetDataTable("trans_dpprice", "DPPrice_Price", Where);
                                if (ISIN_FaceValue.Rows.Count > 0)
                                {
                                    Isin_Qty_Product = Isin_Qty_Product + (Convert.ToDouble(ISIN_FaceValue.Rows[0][0].ToString()) * Convert.ToDouble(dr["quantity"].ToString()));
                                }
                            }
                        }
                        if (HighValue < Isin_Qty_Product)
                        {
                            return "true~" + Isin_Qty_Product;
                        }
                        else
                        {
                            return "false~";
                        }
                    }
                    else
                    {
                        return "false~";
                    }

                }
                else
                {
                    if (Session["dp"].ToString() == "CDSL")
                    {
                        Where = "GlobalSettings_segmentID='" + ViewState["dtExchSegId"].ToString() + "' and GlobalSettings_Name='GS_HIGHVALUETRNDP'";
                        DtHighValue = oDBEngine.GetDataTable("Config_globalSettings", "GlobalSettings_Value", Where);
                        if (DtHighValue.Rows.Count > 0)
                        {
                            HighValue = Convert.ToDouble(DtHighValue.Rows[0][0].ToString());
                            if (File.Exists(Server.MapPath(NSDLCDSL_XMLPATH)))
                            {
                                DsTemp.ReadXml(Server.MapPath(NSDLCDSL_XMLPATH));
                                foreach (DataRow dr in DsTemp.Tables[0].Rows)
                                {
                                    Where = "DPPRice_DP='CDSL' and DPPrice_ISIN='" + dr["isin"].ToString() + "' and DPPrice_Date=(Select max(DPPrice_Date) from trans_dpprice where DPPRice_DP='CDSL' and DPPrice_ISIN='" + dr["isin"].ToString() + "')";
                                    ISIN_FaceValue = oDBEngine.GetDataTable("trans_dpprice", "DPPrice_Price", Where);
                                    if (ISIN_FaceValue.Rows.Count > 0)
                                    {
                                        Isin_Qty_Product = Isin_Qty_Product + (Convert.ToDouble(ISIN_FaceValue.Rows[0][0].ToString()) * Convert.ToDouble(dr["quantity"].ToString()));
                                    }
                                }
                            }
                            if (HighValue < Isin_Qty_Product)
                            {
                                return "true~" + Isin_Qty_Product;
                            }
                            else
                            {
                                return "false~";
                            }
                        }
                        else
                        {
                            return "false~";
                        }
                    }
                }
            }
            else
            {
                return "false~";
            }
            return "false~";
        }



        DataSet AddToData()
        {
            string strMaxHoldingDatetime = "";
            if (Session["dp"].ToString() == "CDSL")
                strMaxHoldingDatetime = objclsCdslHolding.populateCmbTime();
            else
                strMaxHoldingDatetime = populateCmbTimeNSDL();

            //using (SqlConnection objCon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand objCmd = new SqlCommand("Fetch_AccountTransfer", objCon))
            //    {
            //string[] arrDtTran = dttran.Text.Trim().Split('-');
            //string[] arrDtExec = dtexec.Text.Trim().Split('-');
            //objCmd.CommandType = CommandType.StoredProcedure;
            //objCmd.Parameters.AddWithValue("@Seg", Session["dp"].ToString());
            //objCmd.Parameters.AddWithValue("@DpId", Session["usersegid"].ToString());
            //objCmd.Parameters.AddWithValue("@BenAccNo", txtAccountNo.Text.Trim().Split(' ')[0]);
            //objCmd.Parameters.AddWithValue("@ClientId", txtAccountNo.Text.Trim().Split(' ')[0]);
            //objCmd.Parameters.AddWithValue("@CompanyId", Session["LastCompany"].ToString());
            //objCmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
            //objCmd.Parameters.AddWithValue("@Transactiondate", arrDtTran[1] + "-" + arrDtTran[0] + "-" + arrDtTran[2]);
            //objCmd.Parameters.AddWithValue("@ExecutionDate", arrDtExec[1] + "-" + arrDtExec[0] + "-" + arrDtExec[2]);
            //objCmd.Parameters.AddWithValue("@TransactionType", ddltran.SelectedValue);
            //objCmd.Parameters.AddWithValue("@SlipNo", "AcClose-" + txtAccountNo.Text.Trim().Split(' ')[0]);
            //objCmd.Parameters.AddWithValue("@OtherDpId", txtdpid.Text.Trim().Split(' ')[0]);
            //objCmd.Parameters.AddWithValue("@OtherClientId", txtclient1.Text.Trim());
            //objCmd.Parameters.AddWithValue("@SlipType", 1);
            //objCmd.Parameters.AddWithValue("@EnteredBy", Session["userid"].ToString());
            //objCmd.Parameters.AddWithValue("@HoldingDatetime", strMaxHoldingDatetime);

            //using (SqlDataAdapter objDap = new SqlDataAdapter(objCmd))
            //{
            //    objDap.Fill(ds);
            //    //DetailsGrid.DataSource = ds;
            //    //DetailsGrid.DataBind();

            //}
            //    }
            //}
            string[] arrDtTran = dttran.Text.Trim().Split('-');
            string[] arrDtExec = dtexec.Text.Trim().Split('-');
            int sliptype = 1;
            ds = oTransaction.FetchAccountTransfer(Session["dp"].ToString(), Session["usersegid"].ToString(), txtAccountNo.Text.Trim().Split(' ')[0],
                txtAccountNo.Text.Trim().Split(' ')[0], Session["LastCompany"].ToString(), Session["LastFinYear"].ToString(), arrDtTran[1] + "-" + arrDtTran[0] + "-" + arrDtTran[2],
                arrDtExec[1] + "-" + arrDtExec[0] + "-" + arrDtExec[2], Convert.ToInt32(ddltran.SelectedValue.ToString()), "AcClose-" + txtAccountNo.Text.Trim().Split(' ')[0],
                txtdpid.Text.Trim().Split(' ')[0], txtclient1.Text.Trim(), sliptype, Session["userid"].ToString(), strMaxHoldingDatetime);

            return ds;


        }
        void Cancel()
        {
            DetailsGrid.DataSource = null;
            DetailsGrid.DataBind();

        }
        void SaveRecords()
        {


            DataSet ds1 = new DataSet();// = AddToData();
            DataSet ds = AddToData();
            ds.Tables[0].TableName = "Transaction";
            //using (SqlConnection objCon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand objCmd = new SqlCommand("Insert_AccountTransfer", objCon))
            //    {
            //objCmd.CommandType = CommandType.StoredProcedure;
            //objCmd.Parameters.AddWithValue("@seg", Session["dp"].ToString());
            //objCmd.Parameters.AddWithValue("@TransactionType", ddltran.SelectedValue);
            //objCmd.Parameters.AddWithValue("@SlipNo", "AcClose-" + txtAccountNo.Text.Trim().Split(' ')[0]);
            //objCmd.Parameters.AddWithValue("@SlipType", 1);
            //objCmd.Parameters.AddWithValue("@doc", ds.GetXml());


            //using (SqlDataAdapter objDap = new SqlDataAdapter(objCmd))
            //{
            //    objDap.Fill(ds1);


            //}


            //if (objCon.State != ConnectionState.Open)
            //    objCon.Open();
            ////int i = 1;
            //int i = objCmd.ExecuteNonQuery();

            //if (objCon.State == ConnectionState.Open)
            //    objCon.Close();
            int sliptype = 1;
            int i = oTransaction.InsertAccountTransfer(Session["dp"].ToString(), Convert.ToInt32(ddltran.SelectedValue.ToString()),
                "AcClose-" + txtAccountNo.Text.Trim().Split(' ')[0], sliptype, ds.GetXml());

            if (i > 0)
            {
                DetailsGrid.JSProperties["cpsavemsg"] = "Successfully saved";
                //  Page.ClientScript.RegisterStartupScript(GetType(), "savemsg", "<script>alert('Successfully saved');</script>");

            }
            //    }




            //}


        }

        void Cancel_All()
        {
            string NSDLCDSL_XMLPATH = null;
            if (Session["dp"].ToString() == "NSDL")
                NSDLCDSL_XMLPATH = "../Documents/" + "accountno" + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "nsdl";//"../Documents/" + ddlSlipType.SelectedItem.Text + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "nsdl";
            else
                NSDLCDSL_XMLPATH = "../Documents/" + "accountno" + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "cdsl";//"../Documents/" + ddlSlipType.SelectedItem.Text + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "cdsl";
            if (File.Exists(Server.MapPath(NSDLCDSL_XMLPATH)))
            {
                File.Delete(Server.MapPath(NSDLCDSL_XMLPATH));
            }
            bind();
        }

        public void bind()
        {
            //Session["transactiontype"] = ddltran.SelectedItem.Value;
            string NSDLCDSL_XMLPATH = null;
            if (ViewState["SlipNumber"] != null)
            {
                if (Session["dp"].ToString() == "NSDL")
                    NSDLCDSL_XMLPATH = "../Documents/" + "accountno" + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "nsdl";//"../Documents/" + ddlSlipType.SelectedItem.Text + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "nsdl";
                else
                    NSDLCDSL_XMLPATH = "../Documents/" + "accountno" + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "cdsl";//"../Documents/" + ddlSlipType.SelectedItem.Text + "_" + ViewState["SlipNumber"].ToString() + "_" + Session["userid"].ToString() + "cdsl";
                if (File.Exists(Server.MapPath(NSDLCDSL_XMLPATH)))
                {
                    datatemp.Tables.Clear();
                    datatemp.ReadXml(Server.MapPath(NSDLCDSL_XMLPATH));
                    if (datatemp.Tables.Count > 0)
                    {
                        if (datatemp.Tables[0].Rows[0]["slipno"].ToString() == ViewState["SlipNumber"].ToString())
                        {
                            DetailsGrid.DataSource = datatemp;
                            DetailsGrid.DataBind();
                        }
                    }
                }
                else
                {
                    DetailsGrid.DataSource = null;
                    DetailsGrid.DataBind();
                }
            }


        }
        public string populateCmbTimeNSDL()
        {
            DataTable dttime = new DataTable();
            //cmbTime.DataSource = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " CAST(DAY(CdslHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, CdslHolding_HoldingDateTime) + ' ' + CAST(YEAR(CdslHolding_HoldingDateTime) AS VARCHAR(4)) = '" + dt + "' ", " CdslHolding_HoldingDateTime desc");
            //cmbTime.DataSource = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " CAST(DAY(CdslHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, CdslHolding_HoldingDateTime) + ' ' + CAST(YEAR(CdslHolding_HoldingDateTime) AS VARCHAR(4)) = '" + txtDate.Value.ToString() + "' ", " CdslHolding_HoldingDateTime desc");
            ////dttime = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " convert(varchar(12),CdslHolding_HoldingDateTime,113) = convert(varchar(12),cast('" + populateCmbDate() + "' as datetime),113) ", " CdslHolding_HoldingDateTime desc");
            dttime = oDBEngine.GetDataTable(" Trans_nsdlHolding ", "distinct substring(convert(varchar(20), NsdlHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), NsdlHolding_HoldingDateTime, 9), 25, 2) as time,NsdlHolding_HoldingDateTime", null, " NsdlHolding_HoldingDateTime desc");
            string strtime = dttime.Rows[0]["NsdlHolding_HoldingDateTime"].ToString();
            return strtime;

        }
        #region CallBackEvent

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            DataTable DtCountRow = new DataTable();
            string Where = "";
            string[] SplitArr = eventArgument.Split('~');
            if (Session["dp"].ToString() == "NSDL")
            {
                if (SplitArr[1].ToString() == "4")
                {
                    Where = "CDSLClients_DPID='" + SplitArr[0].ToString().Substring(3, 5) + "'";
                    DtCountRow = oDBEngine.GetDataTable("Master_CDSLClients", "top 1 *", Where);
                }
                else
                {
                    Where = "NsdlClients_DPID='" + SplitArr[0].ToString() + "'";
                    DtCountRow = oDBEngine.GetDataTable("Master_NsdlClients", "top 1 *", Where);
                }
            }
            else
            {
                if (ddltran.SelectedItem.Value == "4")
                {
                    Where = "NsdlClients_DPID='" + SplitArr[0].ToString() + "'";
                    DtCountRow = oDBEngine.GetDataTable("Master_NsdlClients", "top 1 *", Where);
                }
                else
                {
                    Where = "CDSLClients_DPID='" + SplitArr[0].ToString().Substring(3, 5) + "'";
                    DtCountRow = oDBEngine.GetDataTable("Master_CDSLClients", "top 1 *", Where);
                }
            }
            if (DtCountRow.Rows.Count > 0)
            {
                data = "Clients";
            }
            else
            {
                data = "NoClients";
            }
        }
        #endregion


    }

}