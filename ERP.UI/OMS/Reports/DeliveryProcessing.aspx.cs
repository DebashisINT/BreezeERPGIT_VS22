using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_DeliveryProcessing : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        static DataSet DS = new DataSet();
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();

        DataTable Export = new DataTable();

        DailyTask_Demat_Deliveries ObjDailyTask_Demat_Deliveries = new DailyTask_Demat_Deliveries();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string SettNumber = Session["LastSettNo"].ToString();
                txtSettlementNumber.Text = SettNumber;
                DtTransferDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                dtPayindate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                string SettmentNo = SettNumber.Substring(0, 7);
                string SettType = SettNumber.Substring(7);
                txtSettNumberHoldBack.Text = SettmentNo;
                txtSettTypeHoldBack.Text = SettType;
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>height();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JS21", "<script language='JavaScript'>Page_Load();</script>");
                BindPoolAccount();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

            //FertchPayINFromMarginHoldBack
        }
        public void BindGridview()
        {
            DS.Tables.Clear();
            DataTable DtDammy = new DataTable();
            string SettmentNo = txtSettlementNumber.Text.Substring(0, 7);
            string SettType = txtSettlementNumber.Text.Substring(7);
            string SegmentID = null;
            DataTable DtSegComp = new DataTable();
            DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
            ViewState["dtSeg"] = dtSeg;
            if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
            else
                DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            if (DtSegComp.Rows.Count > 0)
            {
                string CompanyID = DtSegComp.Rows[0][0].ToString();
                for (int i = 0; i < DtSegComp.Rows.Count; i++)
                {
                    if (SegmentID == null)
                    {
                        SegmentID = DtSegComp.Rows[i][1].ToString();
                    }
                    else
                    {
                        SegmentID = SegmentID + "," + DtSegComp.Rows[i][1].ToString();
                    }
                }
            }

            DataTable dtDate = oDBEngine.GetDataTable("master_settlements", " settlements_FundsPayout", " settlements_exchangesegmentid=" + Convert.ToInt32(Session["ExchangeSegmentID"].ToString()) + " and settlements_number='" + SettmentNo + "' and settlements_typesuffix='" + SettType + "'");
            #region Client Payout
            if (ddlType.SelectedItem.Value == "CP")
            {
                //if (radClientAll.Checked == true)
                //    HiddenField_Client.Value = "NA";
                fn_Client();
                if (HiddenField_Client.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
                }
                else
                {
                    if (radScripAll.Checked == true)
                        HiddenField_Scrip.Value = "NA";
                    DS.Tables.Clear();

                    /* For Tier Structure ----------------------

                    String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlConnection con = new SqlConnection(conn);
                    SqlCommand cmd3 = new SqlCommand("FetchDeliveryProcessing", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@SettlementNumber", SettmentNo);
                    cmd3.Parameters.AddWithValue("@SettlementType", SettType);
                    cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                    cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                    cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(Session["usersegid"].ToString()));
                    cmd3.Parameters.AddWithValue("@PayoutDate", Convert.ToDateTime(dtDate.Rows[0][0].ToString()));
                    cmd3.Parameters.AddWithValue("@Client", HiddenField_Client.Value);
                    cmd3.Parameters.AddWithValue("@Scrip", HiddenField_Scrip.Value);
                    cmd3.Parameters.AddWithValue("@AllCMSegment", SegmentID);
              
                    cmd3.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd3;
                    Adap.Fill(DS);

                    */

                    DS = ObjDailyTask_Demat_Deliveries.FetchDeliveryProcessing(SettmentNo, SettType, Session["LastFinYear"].ToString(),
                        Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                        dtDate.Rows[0][0].ToString(), HiddenField_Client.Value, HiddenField_Scrip.Value,
                        SegmentID);

                    grdDematProcessing.DataSource = DS.Tables[0];
                    grdDematProcessing.DataBind();
                    GrdInterSegment.DataSource = DtDammy;
                    GrdInterSegment.DataBind();
                    GrdMarketPayIN.DataSource = DtDammy;
                    GrdMarketPayIN.DataBind();
                    grdPayInOwnAcc.DataSource = DtDammy;
                    grdPayInOwnAcc.DataBind();
                    grdPayInFromMarginHoldBack.DataSource = DtDammy;
                    grdPayInFromMarginHoldBack.DataBind();
                    grdOffSetPosition.DataSource = DtDammy;
                    grdOffSetPosition.DataBind();
                    grdDematProcessing.Columns[7].Visible = true;
                    if (DS.Tables[0].Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('b')", true);
                    }

                }
            }
            #endregion
            #region Inter Settlement
            else if (ddlType.SelectedItem.Value == "IS")
            {
                string SeTNumber = String.Empty;
                string SetType = String.Empty;
                if (radAllHold.Checked == true)
                    SeTNumber = "All";
                else
                    SeTNumber = txtSettNumberHoldBack.Text;
                if (radAllHoldSettType.Checked == true)
                    SetType = "All";
                else
                    SetType = txtSettTypeHoldBack.Text;

                DataSet DSInterSegment = new DataSet();

                /* For Tier structure
                String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd3 = new SqlCommand("[Report_FetchForInterSegment]", con);///Prev Sp:FetchForInterSegment
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd3.Parameters.AddWithValue("@PayIn", Convert.ToDateTime(dtPayindate.Value));
                cmd3.Parameters.AddWithValue("@ExchangeSegmentID", Session["ExchangeSegmentID"].ToString());
                cmd3.Parameters.AddWithValue("@SettNumIntSeg", SeTNumber);
                cmd3.Parameters.AddWithValue("@SettTypeIntSeg", SetType);
                cmd3.Parameters.AddWithValue("@DematAccID", ddlSourceAccount.SelectedItem.Value);
                cmd3.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());

          
                cmd3.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd3;
                Adap.Fill(DSInterSegment);

                */

                DSInterSegment = ObjDailyTask_Demat_Deliveries.Report_FetchForInterSegment(Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                    dtPayindate.Value.ToString(), Session["ExchangeSegmentID"].ToString(), SeTNumber, SetType, ddlSourceAccount.SelectedItem.Value.ToString(),
                    HttpContext.Current.Session["LastFinYear"].ToString().Trim());

                ViewState["DSInterSegment"] = DSInterSegment.Tables[0];
                GrdInterSegment.DataSource = DSInterSegment.Tables[0];
                GrdInterSegment.DataBind();
                grdDematProcessing.DataSource = DtDammy;
                grdDematProcessing.DataBind();
                GrdMarketPayIN.DataSource = DtDammy;
                GrdMarketPayIN.DataBind();
                grdPayInOwnAcc.DataSource = DtDammy;
                grdPayInOwnAcc.DataBind();
                grdPayInFromMarginHoldBack.DataSource = DtDammy;
                grdPayInFromMarginHoldBack.DataBind();
                grdOffSetPosition.DataSource = DtDammy;
                grdOffSetPosition.DataBind();
                if (DSInterSegment.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('b')", true);
                }

            }
            #endregion
            #region Market PayIn
            else if (ddlType.SelectedItem.Value == "MP")
            {
                string[] PoolAc = ddlPoolAC.SelectedItem.Value.ToString().Split('~');
                DataSet DtMarketPayIn = new DataSet();
                /* For Tier strucuture 
                 String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                 SqlConnection con = new SqlConnection(conn);
                 SqlCommand cmd3 = new SqlCommand("FetchForMarketPayIN", con);
                 cmd3.CommandType = CommandType.StoredProcedure;
                 cmd3.Parameters.AddWithValue("@SettlementNumber", SettmentNo);
                 cmd3.Parameters.AddWithValue("@SettlementType", SettType);
                 cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                 cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(Session["usersegid"].ToString()));
                 cmd3.Parameters.AddWithValue("@AccID", PoolAc[0].ToString());
                 cmd3.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());
                      
                 cmd3.CommandTimeout = 0;
                 SqlDataAdapter Adap = new SqlDataAdapter();
                 Adap.SelectCommand = cmd3;
                 Adap.Fill(DtMarketPayIn);


                 */

                DtMarketPayIn = ObjDailyTask_Demat_Deliveries.FetchForMarketPayIN(SettmentNo, SettType, Session["LastCompany"].ToString(),
                                                  Session["usersegid"].ToString(), PoolAc[0].ToString(),
                                                  HttpContext.Current.Session["LastFinYear"].ToString().Trim());


                ViewState["DtMarketPayIn"] = DtMarketPayIn.Tables[0];
                GrdMarketPayIN.DataSource = DtMarketPayIn.Tables[0];
                GrdMarketPayIN.DataBind();
                GrdInterSegment.DataSource = DtDammy;
                GrdInterSegment.DataBind();
                grdDematProcessing.DataSource = DtDammy;
                grdDematProcessing.DataBind();
                grdPayInOwnAcc.DataSource = DtDammy;
                grdPayInOwnAcc.DataBind();
                grdPayInFromMarginHoldBack.DataSource = DtDammy;
                grdPayInFromMarginHoldBack.DataBind();
                grdOffSetPosition.DataSource = DtDammy;
                grdOffSetPosition.DataBind();
                if (DtMarketPayIn.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('b')", true);
                }

            }
            #endregion
            #region PayIn OwnAccount
            else if (ddlType.SelectedItem.Value == "PO")
            {
                string[] OwnAccountID = drpOwnAccount.SelectedItem.Value.Split('~');
                /* For Tier Structure
                String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd3 = new SqlCommand("PayINFromOwnAccount", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@companyID", Session["LastCompany"].ToString());
                cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd3.Parameters.AddWithValue("@PayIn", Convert.ToDateTime(dtPayindate.Value));
                cmd3.Parameters.AddWithValue("@OwnAccountID", Convert.ToInt32(OwnAccountID[0].ToString()));
                cmd3.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());

                cmd3.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd3;
                Adap.Fill(DS);

                */

                DS = ObjDailyTask_Demat_Deliveries.PayINFromOwnAccount(Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                                                                 dtPayindate.Value.ToString(), OwnAccountID[0].ToString(),
                                                                  HttpContext.Current.Session["LastFinYear"].ToString().Trim());


                grdPayInOwnAcc.DataSource = DS.Tables[0];
                grdPayInOwnAcc.DataBind();
                GrdMarketPayIN.DataSource = DtDammy;
                GrdMarketPayIN.DataBind();
                GrdInterSegment.DataSource = DtDammy;
                GrdInterSegment.DataBind();
                grdDematProcessing.DataSource = DtDammy;
                grdDematProcessing.DataBind();
                grdPayInFromMarginHoldBack.DataSource = DtDammy;
                grdPayInFromMarginHoldBack.DataBind();
                grdOffSetPosition.DataSource = DtDammy;
                grdOffSetPosition.DataBind();
                if (DS.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('b')", true);
                }
            }
            #endregion
            #region PayIn Margin/Holdback
            else if (ddlType.SelectedItem.Value == "PH")
            {
                DS = new DataSet();
                string SeTNumber = String.Empty;
                string SetType = String.Empty;
                if (radAllHold.Checked == true)
                    SeTNumber = "All";
                else
                    SeTNumber = txtSettNumberHoldBack.Text;
                if (radAllHoldSettType.Checked == true)
                    SetType = "All";
                else
                    SetType = txtSettTypeHoldBack.Text;
                string[] AccountID = drpMarginHoldbackAccount.SelectedItem.Value.Split('~');

                /* For Tier structure
                String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd3 = new SqlCommand("FertchPayINFromMarginHoldBack", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@companyID", Session["LastCompany"].ToString());
                cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd3.Parameters.AddWithValue("@PayIn", Convert.ToDateTime(dtPayindate.Value));
                cmd3.Parameters.AddWithValue("@SettNum", SeTNumber);
                cmd3.Parameters.AddWithValue("@SettType", SetType);
                cmd3.Parameters.AddWithValue("@PayInHoldBackAccount", Convert.ToInt32(AccountID[0].ToString()));
                cmd3.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());
                      
                cmd3.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd3;
                Adap.Fill(DS);

                */



                DS = ObjDailyTask_Demat_Deliveries.FertchPayINFromMarginHoldBack(Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                                                        dtPayindate.Value.ToString(), SeTNumber, SetType, AccountID[0].ToString(),
                                                        HttpContext.Current.Session["LastFinYear"].ToString().Trim());

                grdPayInFromMarginHoldBack.DataSource = DS.Tables[0];
                grdPayInFromMarginHoldBack.DataBind();
                grdDematProcessing.DataSource = DtDammy;
                grdDematProcessing.DataBind();
                GrdMarketPayIN.DataSource = DtDammy;
                GrdMarketPayIN.DataBind();
                grdPayInOwnAcc.DataSource = DtDammy;
                grdPayInOwnAcc.DataBind();
                GrdInterSegment.DataSource = DtDammy;
                GrdInterSegment.DataBind();
                grdOffSetPosition.DataSource = DtDammy;
                grdOffSetPosition.DataBind();
                if (DS.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('b')", true);
                }

            }
            #endregion
            //#region Holdback Release
            //if (ddlType.SelectedItem.Value == "HR")
            //{
            //    if (radClientAll.Checked == true)
            //        Client = "NA";
            //    if (radScripAll.Checked == true)
            //        Scrip = "NA";
            //    DS.Tables.Clear();
            //    String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //    SqlConnection con = new SqlConnection(conn);
            //    SqlCommand cmd3 = new SqlCommand("FetchHoldbackRelease", con);
            //    cmd3.CommandType = CommandType.StoredProcedure;
            //    cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
            //    cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
            //    cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(Session["usersegid"].ToString()));
            //    cmd3.Parameters.AddWithValue("@PayoutDate", Convert.ToDateTime(DtTransferDate.Value));
            //    cmd3.Parameters.AddWithValue("@Client", Client);
            //    cmd3.Parameters.AddWithValue("@Scrip", Scrip);
            //    cmd3.Parameters.AddWithValue("@AllCMSegment", SegmentID);
            //    cmd3.CommandTimeout = 0;
            //    SqlDataAdapter Adap = new SqlDataAdapter();
            //    Adap.SelectCommand = cmd3;
            //    Adap.Fill(DS);

            //    GrdInterSegment.DataSource = DtDammy;
            //    GrdInterSegment.DataBind();
            //    GrdMarketPayIN.DataSource = DtDammy;
            //    GrdMarketPayIN.DataBind();
            //    grdPayInOwnAcc.DataSource = DtDammy;
            //    grdPayInOwnAcc.DataBind();
            //    grdPayInFromMarginHoldBack.DataSource = DtDammy;
            //    grdPayInFromMarginHoldBack.DataBind();
            //    grdOffSetPosition.DataSource = DtDammy;
            //    grdOffSetPosition.DataBind();
            //    grdDematProcessing.Columns[8].Visible = false;
            //    if (DS.Tables.Count == 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
            //    }

            //    else
            //    {
            //        if (DS.Tables[0].Rows.Count == 0)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
            //        }
            //        else
            //        {
            //            grdDematProcessing.DataSource = DS.Tables[0];
            //            grdDematProcessing.DataBind();
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('b')", true);
            //        }
            //    }
            //}
            //#endregion
            #region OffSet Position
            if (ddlType.SelectedItem.Value == "OF")
            {
                string TargetSettNum = txtTargetSettOff.Text.Substring(0, 7);
                string TargetSettType = txtTargetSettOff.Text.Substring(7);
                string SourceSettNum = txtSourceSettOff.Text.Substring(0, 7);
                string SourceSettType = txtSourceSettOff.Text.Substring(7);

                DataSet DtOffSetPos = new DataSet();

                /* For Tier Structure----------------
                String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd3 = new SqlCommand("FetchForOffSetPosition", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@companyID", Session["LastCompany"].ToString());
                cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd3.Parameters.AddWithValue("@SettNumberTarget", TargetSettNum);
                cmd3.Parameters.AddWithValue("@SettTypeTarget", TargetSettType);
                cmd3.Parameters.AddWithValue("@SettNumberSource", SourceSettNum);
                cmd3.Parameters.AddWithValue("@SettTypeSource", SourceSettType);
                cmd3.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());

          
                cmd3.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd3;
                Adap.Fill(DtOffSetPos);

                */

                DtOffSetPos = ObjDailyTask_Demat_Deliveries.FetchForOffSetPosition(Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                                                                  TargetSettNum, TargetSettType, SourceSettNum, SourceSettType,
                                                                  HttpContext.Current.Session["LastFinYear"].ToString().Trim());


                ViewState["DtOffSetPos"] = DtOffSetPos;
                grdOffSetPosition.DataSource = DtOffSetPos;
                grdOffSetPosition.DataBind();
                grdDematProcessing.DataSource = DtDammy;
                grdDematProcessing.DataBind();
                GrdInterSegment.DataSource = DtDammy;
                GrdInterSegment.DataBind();
                GrdMarketPayIN.DataSource = DtDammy;
                GrdMarketPayIN.DataBind();
                grdPayInOwnAcc.DataSource = DtDammy;
                grdPayInOwnAcc.DataBind();
                grdPayInFromMarginHoldBack.DataSource = DtDammy;
                grdPayInFromMarginHoldBack.DataBind();
                if (DtOffSetPos.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('a')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "Visible('b')", true);
                }
            }
            #endregion
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "DeliveryProfile")
            {
                int RowID = Convert.ToInt32(idlist[1]);
                Label lblClientID = (Label)grdDematProcessing.Rows[RowID].FindControl("lblID");
                string expression = "CustomerID = '" + lblClientID.Text + "'";
                if (ddlType.SelectedItem.Value == "CP")
                {
                    DataRow[] rows = DS.Tables[4].Select(expression);
                    if (rows.Length > 0)
                    {
                        string TotalDelVal = null;
                        string LedgerClear = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][2].ToString()));
                        string MarginClear = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][3].ToString()));
                        string LedgerClosing = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][4].ToString()));
                        string MarginClosing = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][5].ToString()));
                        string ProfileName = rows[0][6].ToString();
                        if (rows[0][7] != DBNull.Value)
                            TotalDelVal = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][7].ToString()));
                        data = "DeliveryProfile~" + LedgerClear + "~" + MarginClear + "~" + LedgerClosing + "~" + MarginClosing + "~" + ProfileName + "~" + TotalDelVal;
                    }
                    else
                    {
                        data = "DeliveryProfile~NA~1";
                    }
                }
                else if (ddlType.SelectedItem.Value == "HR")
                {
                    DataRow[] rows = DS.Tables[4].Select(expression);
                    if (rows.Length > 0)
                    {
                        string TotalDelVal = null;
                        string LedgerClear = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][2].ToString()));
                        string MarginClear = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][3].ToString()));
                        string LedgerClosing = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][4].ToString()));
                        string MarginClosing = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][5].ToString()));
                        string ProfileName = rows[0][6].ToString();
                        if (rows[0][7] != DBNull.Value)
                            TotalDelVal = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(rows[0][7].ToString()));
                        data = "DeliveryProfile~" + LedgerClear + "~" + MarginClear + "~" + LedgerClosing + "~" + MarginClosing + "~" + ProfileName + "~" + TotalDelVal;
                    }
                    else
                    {
                        data = "DeliveryProfile~NA~1";
                    }
                }
            }
            else if (idlist[0] == "Clients")
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = "'" + val[0] + "'";
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
                if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
            }
            else if (idlist[0] == "Scrips" || idlist[0] == "Branch" || idlist[0] == "Group")
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = "'" + val[0] + "'";
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }


                if (idlist[0] == "Scrips")
                {
                    data = "Scrips~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }
        void fn_Client()
        {

            DataTable dtclient = new DataTable();
            if (radClientAll.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                        }
                        else
                        {
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", " DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select distinct gpm_id from tbl_master_groupmaster ))");

                        }
                    }
                    else
                    {
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", " DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value + "))");
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    if (rdbranchAll.Checked)
                    {
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value + ")");
                    }
                }

            }

            string Clients = null;
            if (dtclient.Rows.Count > 0)
            {
                for (int i = 0; i < dtclient.Rows.Count; i++)
                {
                    if (Clients == null)
                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                    else
                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                }

            }
            if (Clients != null)
            {
                HiddenField_Client.Value = Clients;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindGridview();
            ScriptManager.RegisterStartupScript(this, GetType(), "JSScript", "DeliveryProcessButton()", true);
        }
        protected void grdDematProcessing_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable mytable = new DataTable();
            DataColumn productidcolumn = new DataColumn("ID");
            DataColumn productnamecolumn = new DataColumn("ClientBankName");
            mytable.Columns.Add(productidcolumn);
            mytable.Columns.Add(productnamecolumn);

            GridViewRow row = e.Row;
            string customerID = null;
            string expression = string.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TextName = String.Empty;
                TextBox txtStock = (TextBox)row.FindControl("txtStock");
                Label lblStock = (Label)row.FindControl("lblStock");
                txtStock.Attributes.Add("onBlur", "javascript:DeliverableValue(" + txtStock.ClientID + ",'" + lblStock.Text + "')");
                string ShortName = null;
                string ShortNameFinal = null;
                string ShortNameFormargin = null;
                string ShortNameFinalFormargin = null;
                string ID = null;
                Label lblID = (Label)row.FindControl("lblID");
                Label lblAccType = (Label)row.FindControl("lblAccType");
                Label lblAccName = (Label)row.FindControl("lblAccName");

                Label lblColourType = (Label)row.FindControl("lblColourType");

                customerID = lblID.Text;
                expression = "CustomerID = '" + customerID + "'";
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlAccountName");
                DropDownList ddlDeliverTo = (DropDownList)e.Row.FindControl("ddlDeliverTo");
                //Label lblFromAccount = (Label)row.FindControl("lblFromAccount");
                string AccountType = ddlDeliverTo.SelectedItem.Value;
                if (AccountType == "OW")
                {
                    ddl.DataSource = DS.Tables[5];
                    ddl.DataValueField = "ID";
                    ddl.DataTextField = "ShortName";
                    ddl.DataBind();
                }
                else
                {
                    DataRow[] rows = DS.Tables[1].Select(expression);
                    foreach (DataRow row1 in rows)
                    {
                        DataRow newrow = mytable.NewRow();
                        newrow["ID"] = row1["ID"];
                        newrow["ClientBankName"] = row1["ClientBankName"];
                        mytable.Rows.Add(newrow);
                    }
                    DataTable rowMargin = DS.Tables[2];
                    for (int i = 0; i < rowMargin.Rows.Count; i++)
                    {
                        DataRow newrow = mytable.NewRow();

                        ShortNameFormargin = rowMargin.Rows[i]["ID"].ToString();
                        string[] srtName = ShortNameFormargin.Split('~');
                        if (srtName.Length > 1)
                        {
                            if (srtName[1].ToString().Trim() == Session["usersegid"].ToString() && srtName[0].ToString().Trim() == lblAccName.Text.ToString().Trim())
                                ShortNameFinalFormargin = ShortNameFormargin;
                        }

                        newrow["ID"] = rowMargin.Rows[i]["ID"].ToString();
                        newrow["ClientBankName"] = rowMargin.Rows[i]["ShortName"].ToString();
                        mytable.Rows.Add(newrow);
                    }
                    DataTable rowHold = DS.Tables[3];
                    for (int i = 0; i < rowHold.Rows.Count; i++)
                    {
                        DataRow newrow = mytable.NewRow();
                        ShortName = rowHold.Rows[i]["ID"].ToString();
                        string[] srtName = ShortName.Split('~');
                        if (srtName.Length > 1)
                        {
                            if (srtName[1].ToString().Trim() == Session["usersegid"].ToString())
                                ShortNameFinal = ShortName;
                        }
                        newrow["ID"] = rowHold.Rows[i]["ID"].ToString();
                        newrow["ClientBankName"] = rowHold.Rows[i]["ShortName"].ToString();
                        mytable.Rows.Add(newrow);
                    }
                    ddl.DataSource = mytable;
                    ddl.DataTextField = "ClientBankName";
                    ddl.DataValueField = "ID";
                    ddl.DataBind();
                    if (lblAccType.Text.Trim() == "HA")
                    {
                        ID = ShortNameFinal;
                    }
                    else if (lblAccType.Text.Trim() == "MA")
                    {
                        ID = ShortNameFinalFormargin; //lblAccName.Text.Trim() + "~" + "MA";
                    }
                    else
                    {
                        string expression1 = "CustomerID = '" + customerID + "' and AcType='Primary'";
                        DataRow[] rows1 = DS.Tables[1].Select(expression1);
                        if (rows1.Length > 0)
                        {
                            ID = rows1[0][2].ToString();
                        }
                        else
                        {
                            string expression2 = "CustomerID = '" + customerID + "' and AcType='Secondary'";
                            DataRow[] rows2 = DS.Tables[1].Select(expression2);
                            if (rows2.Length > 0)
                                ID = rows2[0][2].ToString();
                        }
                    }
                    if (ID != null)
                        ddl.SelectedValue = ID;

                    string expression12 = "CustomerID = '" + customerID + "' and AcType='Primary'";
                    DataRow[] rows12 = DS.Tables[1].Select(expression12);
                    if (rows12.Length > 0)
                    {
                        TextName = rows12[0][3].ToString();
                    }

                    for (int i = 0; i < ddl.Items.Count; i++)
                    {
                        for (int j = 0; j < rowMargin.Rows.Count; j++)
                        {
                            if (ddl.Items[i].Text.ToString() == rowMargin.Rows[j]["ShortName"].ToString())
                            {
                                ddl.Items[i].Attributes.Add("style", "color:red");
                            }
                        }
                        for (int k = 0; k < rowHold.Rows.Count; k++)
                        {
                            if (ddl.Items[i].Text.ToString() == rowHold.Rows[k]["ShortName"].ToString())
                            {
                                ddl.Items[i].Attributes.Add("style", "color:blue");
                            }
                        }
                        if (ddl.Items[i].Text.ToString() == TextName)
                        {
                            ddl.Items[i].Attributes.Add("style", "color:green");
                        }
                    }
                }
                if (lblColourType.Text == "G")
                {
                    e.Row.Cells[1].Style.Add("color", "green");
                    e.Row.Cells[1].Font.Bold = true;
                }
                else if (lblColourType.Text == "B")
                {
                    e.Row.Cells[1].Style.Add("color", "blue");
                    e.Row.Cells[1].Font.Bold = true;
                }

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        protected void ddlDeliverTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string customerID = null;
            string expression = string.Empty;
            string valDeliveryTo = ((DropDownList)sender).SelectedValue.ToString();
            string ClientID1 = ((DropDownList)sender).ClientID;//grdDematProcessing_ctl20_ddlDeliverTo
            string[] RowNo1 = ClientID1.Split('_');
            string RowNo2 = RowNo1[1].ToString();
            int RowNo = Convert.ToInt32(RowNo2.Substring(3));
            DropDownList ddl = (DropDownList)grdDematProcessing.Rows[RowNo - 2].FindControl("ddlAccountName");
            Label lblID = (Label)grdDematProcessing.Rows[RowNo - 2].FindControl("lblID");
            if (valDeliveryTo == "CA")
            {
                DataTable mytable = new DataTable();
                DataColumn productidcolumn = new DataColumn("ID");
                DataColumn productnamecolumn = new DataColumn("ClientBankName");
                mytable.Columns.Add(productidcolumn);
                mytable.Columns.Add(productnamecolumn);
                string ID = null;
                customerID = lblID.Text;
                expression = "CustomerID = '" + customerID + "'";

                string expression1 = "CustomerID = '" + customerID + "' and AcType='Primary'";

                DataRow[] rows1 = DS.Tables[1].Select(expression1);
                if (rows1.Length > 0)
                {
                    ID = rows1[0][2].ToString();
                }
                DataRow[] rows = DS.Tables[1].Select(expression);
                foreach (DataRow row1 in rows)
                {
                    DataRow newrow = mytable.NewRow();
                    newrow["ID"] = row1["ID"];
                    newrow["ClientBankName"] = row1["ClientBankName"];
                    mytable.Rows.Add(newrow);
                }
                ddl.DataSource = mytable;
                ddl.DataTextField = "ClientBankName";
                ddl.DataValueField = "ID";
                ddl.DataBind();
                if (rows1.Length > 0)
                {
                    ddl.SelectedValue = ID;
                }
            }
            else if (valDeliveryTo == "MA")
            {
                ddl.DataSource = DS.Tables[2];
                ddl.DataValueField = "ID";
                ddl.DataTextField = "ShortName";
                ddl.DataBind();
            }
            else
            {
                ddl.DataSource = DS.Tables[3];
                ddl.DataValueField = "ID";
                ddl.DataTextField = "ShortName";
                ddl.DataBind();
            }
        }
        protected void grdDematProcessing_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + grdDematProcessing.Rows.Count + "'" + ")");
            }
        }
        protected void btnProcessing_Click(object sender, EventArgs e)
        {
            int NoofRowsAffect = 0;
            #region Client Payout
            if (ddlType.SelectedItem.Value == "CP")
            {
                string AccName = String.Empty;
                string AccType = String.Empty;
                DataTable DtDematProcessing = new DataTable();
                DtDematProcessing.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
                DtDematProcessing.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtDematProcessing.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
                DtDematProcessing.Columns.Add(new DataColumn("Stock", typeof(Decimal)));//3
                DtDematProcessing.Columns.Add(new DataColumn("DeliverTo", typeof(String)));//4
                DtDematProcessing.Columns.Add(new DataColumn("AccName", typeof(String)));//5
                DtDematProcessing.Columns.Add(new DataColumn("SourceAccID", typeof(String)));//6
                foreach (GridViewRow row in grdDematProcessing.Rows)
                {
                    Label lblID = (Label)row.FindControl("lblID");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblProductID = (Label)row.FindControl("lblProductID");
                    TextBox txtStock = (TextBox)row.FindControl("txtStock");
                    //DropDownList ddlDeliverTo = (DropDownList)row.FindControl("ddlDeliverTo");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    Label lbSourceAccID = (Label)row.FindControl("lbSourceAccID");
                    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                    if (ChkDelivery.Checked == true)
                    {
                        DataRow drDemat = DtDematProcessing.NewRow();
                        drDemat[0] = lblID.Text;
                        drDemat[1] = lblISIN.Text;
                        drDemat[2] = lblProductID.Text;
                        if (txtStock.Text != "")
                            drDemat[3] = txtStock.Text;
                        else
                            drDemat[3] = "0";
                        string[] DelvAccount = ddlAccountName.SelectedItem.Value.ToString().Split('~');
                        AccName = DelvAccount[0].ToString();
                        AccType = DelvAccount[2].ToString();
                        drDemat[4] = AccType;
                        drDemat[5] = AccName;
                        drDemat[6] = lbSourceAccID.Text;
                        DtDematProcessing.Rows.Add(drDemat);
                    }
                }
                string SettmentNo = txtSettlementNumber.Text.ToString().Substring(0, 7);
                string SettType = txtSettlementNumber.Text.ToString().Substring(7);
                string tabledata = oconverter.ConvertDataTableToXML(DtDematProcessing);

                /*
           
                String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand com = new SqlCommand("DeliveryProcessingClientPayout", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@clientPayoutData", tabledata);
                com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
                com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
                com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
                com.Parameters.AddWithValue("@settlementNumber", SettmentNo);
                com.Parameters.AddWithValue("@settlementType", SettType);
                com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
                com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
                                  
                NoofRowsAffect = com.ExecuteNonQuery();
                con.Close();
                */

                NoofRowsAffect = ObjDailyTask_Demat_Deliveries.DeliveryProcessingClientPayout(tabledata, Session["LastFinYear"].ToString(),
                       Session["LastCompany"].ToString(), Session["usersegid"].ToString(), SettmentNo, SettType, DtTransferDate.Value.ToString(),
                           Session["userid"].ToString());
                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Processing Successfully !!');", true);
                }
            }
            #endregion
            #region Inter Segment
            else if (ddlType.SelectedItem.Value == "IS")
            {
                DataTable DtInterSegment = new DataTable();
                DtInterSegment.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
                DtInterSegment.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtInterSegment.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
                DtInterSegment.Columns.Add(new DataColumn("AddJust", typeof(Decimal)));//3
                DtInterSegment.Columns.Add(new DataColumn("AccountID", typeof(String)));//4
                DtInterSegment.Columns.Add(new DataColumn("SettNumberSource", typeof(String)));//5
                DtInterSegment.Columns.Add(new DataColumn("SettTypeSource", typeof(String)));//6
                DtInterSegment.Columns.Add(new DataColumn("SettNumberTarget", typeof(String)));//7
                DtInterSegment.Columns.Add(new DataColumn("SettTypeTarget", typeof(String)));//8
                DtInterSegment.Columns.Add(new DataColumn("SourceAccID", typeof(String)));//9
                foreach (GridViewRow row in GrdInterSegment.Rows)
                {
                    Label lblCustID = (Label)row.FindControl("lblCustID");
                    Label lblSettSource = (Label)row.FindControl("lblSettSource");
                    Label lblSettTarget = (Label)row.FindControl("lblSettTarget");
                    TextBox txtAdjst = (TextBox)row.FindControl("txtAdjst");
                    Label lblAccountid = (Label)row.FindControl("lblAccountid");
                    Label lblProductID = (Label)row.FindControl("lblProductID");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                    string SettmentNoS = lblSettSource.Text.ToString().Substring(0, 7);
                    string SettTypeS = lblSettSource.Text.ToString().Substring(7);
                    string SettmentNoT = lblSettTarget.Text.ToString().Substring(0, 7);
                    string SettTypeT = lblSettTarget.Text.ToString().Substring(7);
                    if (ChkDelivery.Checked == true)
                    {
                        DataRow drDemat = DtInterSegment.NewRow();
                        drDemat[0] = lblCustID.Text;
                        drDemat[1] = lblISIN.Text;
                        drDemat[2] = lblProductID.Text;
                        drDemat[3] = txtAdjst.Text;
                        drDemat[4] = ddlTargetAccount.SelectedItem.Value;
                        drDemat[5] = SettmentNoS;
                        drDemat[6] = SettTypeS;
                        drDemat[7] = SettmentNoT;
                        drDemat[8] = SettTypeT;
                        drDemat[9] = ddlSourceAccount.SelectedItem.Value;
                        DtInterSegment.Rows.Add(drDemat);
                    }
                }
                string tabledata = oconverter.ConvertDataTableToXML(DtInterSegment);

                /* For Tier Structure ------------
                 String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                 SqlConnection con = new SqlConnection(conn);
                 con.Open();
                 SqlCommand com = new SqlCommand("DeliveryProcessingInterSettlement", con);
                 com.CommandType = CommandType.StoredProcedure;
                 com.Parameters.AddWithValue("@InterSettlementData", tabledata);
                 com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
                 com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
                 com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
                 com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
                 com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
                                 

                 com.CommandTimeout = 0;
                 NoofRowsAffect = com.ExecuteNonQuery();
                 con.Close();

                 */

                NoofRowsAffect = ObjDailyTask_Demat_Deliveries.DeliveryProcessingInterSettlement(tabledata, Session["LastFinYear"].ToString(),
                                                                          Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                                                                              DtTransferDate.Value.ToString(), Session["userid"].ToString());
                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Processing Successfully !!');", true);
                }
            }
            #endregion
            #region Market PayIN
            else if (ddlType.SelectedItem.Value == "MP")
            {
                string[] PoolAc = ddlPoolAC.SelectedItem.Value.ToString().Split('~');
                string SettmentNo = txtSettlementNumber.Text.ToString().Substring(0, 7);
                string SettType = txtSettlementNumber.Text.ToString().Substring(7);
                string EarlyPAyIn = string.Empty;
                if (PoolAc[1].ToString() == "NSDL")
                {
                    if (ddlPayInType.SelectedItem.Value == "N")
                        EarlyPAyIn = "N";
                    else
                        EarlyPAyIn = "EP";
                }
                else
                    EarlyPAyIn = "N";
                DataTable DtMarketPayIN = new DataTable();
                DtMarketPayIN.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
                DtMarketPayIN.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtMarketPayIN.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
                DtMarketPayIN.Columns.Add(new DataColumn("Deliverable", typeof(Decimal)));//3
                DtMarketPayIN.Columns.Add(new DataColumn("AccountID", typeof(String)));//4
                DtMarketPayIN.Columns.Add(new DataColumn("SettNumber", typeof(String)));//5
                DtMarketPayIN.Columns.Add(new DataColumn("SettType", typeof(String)));//6
                foreach (GridViewRow row in GrdMarketPayIN.Rows)
                {
                    Label lblExchange = (Label)row.FindControl("lblClientName");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblProductSeriesid = (Label)row.FindControl("lblProductSeriesid");
                    TextBox txtDeliverable = (TextBox)row.FindControl("txtDeliverable");
                    Label lblAccountID = (Label)row.FindControl("lblAccountID");
                    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                    if (ChkDelivery.Checked == true)
                    {
                        DataRow drDemat = DtMarketPayIN.NewRow();
                        drDemat[0] = lblExchange.Text;
                        drDemat[1] = lblISIN.Text;
                        drDemat[2] = lblProductSeriesid.Text;
                        drDemat[3] = txtDeliverable.Text;
                        drDemat[4] = lblAccountID.Text;
                        drDemat[5] = SettmentNo;
                        drDemat[6] = SettType;
                        DtMarketPayIN.Rows.Add(drDemat);
                    }
                }
                string tabledata = oconverter.ConvertDataTableToXML(DtMarketPayIN);

                /*
                  String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                 SqlConnection con = new SqlConnection(conn);
                 con.Open();
                 SqlCommand com = new SqlCommand("DeliveryProcessingMarketPayIN", con);
                 com.CommandType = CommandType.StoredProcedure;
                 com.Parameters.AddWithValue("@MarketPayINData", tabledata);
                 com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
                 com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
                 com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
                 com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
                 com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
                 com.Parameters.AddWithValue("@MrPayIN", EarlyPAyIn);

          
                 com.CommandTimeout = 0;
                 NoofRowsAffect = com.ExecuteNonQuery();
                 con.Close();

                 */

                NoofRowsAffect = ObjDailyTask_Demat_Deliveries.DeliveryProcessingMarketPayIN(tabledata, Session["LastFinYear"].ToString(),
                                                                      Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                                                               DtTransferDate.Value.ToString(), Session["userid"].ToString(), EarlyPAyIn);


                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Processing Successfully !!');", true);
                }
            }
            #endregion
            #region PayIn OwnAccount
            else if (ddlType.SelectedItem.Value == "PO")
            {
                DataTable DtPayInOwnAccount = new DataTable();
                DtPayInOwnAccount.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
                DtPayInOwnAccount.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtPayInOwnAccount.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
                DtPayInOwnAccount.Columns.Add(new DataColumn("TransFerable", typeof(Decimal)));//3
                DtPayInOwnAccount.Columns.Add(new DataColumn("AccountID", typeof(String)));//4
                DtPayInOwnAccount.Columns.Add(new DataColumn("SettNumber", typeof(String)));//5
                DtPayInOwnAccount.Columns.Add(new DataColumn("SettType", typeof(String)));//6
                DtPayInOwnAccount.Columns.Add(new DataColumn("OwnAccTarget", typeof(String)));//7
                DtPayInOwnAccount.Columns.Add(new DataColumn("SettNumberSource", typeof(String)));//8
                DtPayInOwnAccount.Columns.Add(new DataColumn("SettTypeSource", typeof(String)));//9
                DtPayInOwnAccount.Columns.Add(new DataColumn("Type", typeof(String)));//10
                DtPayInOwnAccount.Columns.Add(new DataColumn("SourceSeg", typeof(String)));//11
                DtPayInOwnAccount.Columns.Add(new DataColumn("Remarks", typeof(String)));//12
                foreach (GridViewRow row in grdPayInOwnAcc.Rows)
                {
                    Label lblCliID = (Label)row.FindControl("lblCliID");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblProductSeriesID = (Label)row.FindControl("lblProductSeriesID");
                    TextBox txtTransferable = (TextBox)row.FindControl("txtTransferable");
                    Label lblAccountID = (Label)row.FindControl("lblAccountID");
                    Label lblSettNumber = (Label)row.FindControl("lblSettNumber");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                    Label lblAlternateProductSeriesID = (Label)row.FindControl("lblAlternateProductSeriesID");
                    Label lblDammyAccID = (Label)row.FindControl("lblDammyAccID");
                    Label lblType = (Label)row.FindControl("lblType");


                    string[] AccountID = drpOwnAccount.SelectedItem.Value.Split('~');

                    if (ChkDelivery.Checked == true)
                    {
                        string SettmentNo = lblSettNumber.Text.ToString().Substring(0, 7);
                        string SettType = lblSettNumber.Text.ToString().Substring(7);

                        int random = RandomNumber(1, 2000);

                        DataRow drDemat = DtPayInOwnAccount.NewRow();
                        drDemat[0] = lblCliID.Text;
                        drDemat[1] = lblISIN.Text;
                        if (lblType.Text.Trim() == "Y")
                            drDemat[2] = lblAlternateProductSeriesID.Text;
                        else
                            drDemat[2] = lblProductSeriesID.Text;
                        drDemat[3] = txtTransferable.Text;
                        drDemat[4] = lblAccountID.Text;
                        drDemat[5] = SettmentNo;
                        drDemat[6] = SettType;
                        drDemat[7] = ddlAccountName.SelectedItem.Value;
                        drDemat[8] = "Own";
                        drDemat[9] = "O";
                        drDemat[10] = "IA";
                        if (lblType.Text.Trim() == "Y")
                        {
                            if (AccountID[1].ToString() == "0")
                                drDemat[11] = DBNull.Value;
                            else
                                drDemat[11] = AccountID[1].ToString();
                        }
                        else
                            drDemat[11] = DBNull.Value;
                        drDemat[12] = drpOwnAccount.SelectedItem.Text + " To " + ddlAccountName.SelectedItem.Text + "~" + lblAlternateProductSeriesID.Text + "~" + lblCliID.Text + "~" + SettmentNo + "~" + SettType + "~" + random;
                        DtPayInOwnAccount.Rows.Add(drDemat);

                        if (lblType.Text.Trim() == "Y")
                        {
                            DataRow drDemat1 = DtPayInOwnAccount.NewRow();
                            drDemat1[0] = lblCliID.Text;
                            drDemat1[1] = lblISIN.Text;
                            drDemat1[2] = lblAlternateProductSeriesID.Text;
                            drDemat1[3] = txtTransferable.Text;
                            drDemat1[4] = ddlAccountName.SelectedItem.Value;
                            drDemat1[5] = "SYSTM";
                            drDemat1[6] = "C";
                            drDemat1[7] = lblDammyAccID.Text;
                            drDemat1[8] = SettmentNo;
                            drDemat1[9] = SettType;
                            drDemat1[10] = "XE";
                            drDemat1[11] = DBNull.Value;
                            drDemat1[12] = drpOwnAccount.SelectedItem.Text + " To " + ddlAccountName.SelectedItem.Text + "~" + lblAlternateProductSeriesID.Text + "~" + lblCliID.Text + "~" + SettmentNo + "~" + SettType + "~" + random;
                            DtPayInOwnAccount.Rows.Add(drDemat1);

                            DataRow drDemat2 = DtPayInOwnAccount.NewRow();
                            drDemat2[0] = lblCliID.Text;
                            drDemat2[1] = lblISIN.Text;
                            drDemat2[2] = lblProductSeriesID.Text;
                            drDemat2[3] = txtTransferable.Text;
                            drDemat2[4] = lblDammyAccID.Text;
                            drDemat2[5] = SettmentNo;
                            drDemat2[6] = SettType;
                            drDemat2[7] = ddlAccountName.SelectedItem.Value;
                            drDemat2[8] = "SYSTM";
                            drDemat2[9] = "C";
                            drDemat2[10] = "XE";
                            drDemat2[11] = DBNull.Value;
                            drDemat2[12] = drpOwnAccount.SelectedItem.Text + " To " + ddlAccountName.SelectedItem.Text + "~" + lblAlternateProductSeriesID.Text + "~" + lblCliID.Text + "~" + SettmentNo + "~" + SettType + "~" + random;
                            DtPayInOwnAccount.Rows.Add(drDemat2);
                        }
                    }
                }
                string tabledata = oconverter.ConvertDataTableToXML(DtPayInOwnAccount);

                /* For Tier Structure ------------------
                String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand com = new SqlCommand("DeliveryProcessingPayInOwnAccount", con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@PayInOwnAccount", tabledata);
                com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
                com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
                com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
                com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
                com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());

           
                NoofRowsAffect = com.ExecuteNonQuery();
                con.Close();

                */

                NoofRowsAffect = ObjDailyTask_Demat_Deliveries.DeliveryProcessingPayInOwnAccount(tabledata, Session["LastFinYear"].ToString(),
                    Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                   DtTransferDate.Value.ToString(), Session["userid"].ToString());

                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Processing Successfully !!');", true);
                }
            }
            #endregion
            #region PayIn Margin/HoldBack
            else if (ddlType.SelectedItem.Value == "PH")
            {
                DataTable DtPayInMarginHoldBack = new DataTable();
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("TransFerable", typeof(Decimal)));//3
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("AccountID", typeof(String)));//4
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("SettNumber", typeof(String)));//5
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("SettType", typeof(String)));//6
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("OwnAccTarget", typeof(String)));//7
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("SettNumberSource", typeof(String)));//8
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("SettTypeSource", typeof(String)));//9
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("Type", typeof(String)));//10
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("SourceSeg", typeof(String)));//11
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("Remarks", typeof(String)));//12
                foreach (GridViewRow row in grdPayInFromMarginHoldBack.Rows)
                {
                    Label lblCliID = (Label)row.FindControl("lblCliID");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblProductSeriesID = (Label)row.FindControl("lblProductSeriesID");
                    TextBox txtTransferable = (TextBox)row.FindControl("txtTransferable");
                    Label lblAccountID = (Label)row.FindControl("lblAccountID");
                    Label lblSettNumType = (Label)row.FindControl("lblSettNumType");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    Label lblSettNumberS = (Label)row.FindControl("lblSettNumberS");
                    Label lblSettTypeS = (Label)row.FindControl("lblSettTypeS");
                    Label lblAlternateProductSeriesID = (Label)row.FindControl("lblAlternateProductSeriesID");
                    Label lblDammyAccID = (Label)row.FindControl("lblDammyAccID");
                    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                    Label lblType = (Label)row.FindControl("lblType");

                    string[] AccountID = drpMarginHoldbackAccount.SelectedItem.Value.Split('~');

                    if (ChkDelivery.Checked == true)
                    {
                        int random = RandomNumber(1, 2000);

                        string SettmentNo = lblSettNumType.Text.ToString().Substring(0, 7);
                        string SettType = lblSettNumType.Text.ToString().Substring(7);

                        DataRow drDemat = DtPayInMarginHoldBack.NewRow();
                        drDemat[0] = lblCliID.Text;
                        drDemat[1] = lblISIN.Text;
                        if (lblType.Text.Trim() == "Y")
                            drDemat[2] = lblAlternateProductSeriesID.Text;
                        else
                            drDemat[2] = lblProductSeriesID.Text;
                        drDemat[3] = txtTransferable.Text;
                        drDemat[4] = lblAccountID.Text;
                        drDemat[5] = SettmentNo;
                        drDemat[6] = SettType;
                        drDemat[7] = ddlAccountName.SelectedItem.Value;
                        drDemat[8] = lblSettNumberS.Text;
                        drDemat[9] = lblSettTypeS.Text;
                        drDemat[10] = "IA";
                        if (lblType.Text.Trim() == "Y")
                            drDemat[11] = AccountID[1].ToString();
                        else
                            drDemat[11] = DBNull.Value;

                        drDemat[12] = drpMarginHoldbackAccount.SelectedItem.Text + " To " + ddlAccountName.SelectedItem.Text + "~" + lblAlternateProductSeriesID.Text + "~" + lblCliID.Text + "~" + SettmentNo + "~" + SettType + "~" + random;
                        DtPayInMarginHoldBack.Rows.Add(drDemat);

                        if (lblType.Text.Trim() == "Y")
                        {
                            DataRow drDemat1 = DtPayInMarginHoldBack.NewRow();
                            drDemat1[0] = lblCliID.Text;
                            drDemat1[1] = lblISIN.Text;
                            drDemat1[2] = lblAlternateProductSeriesID.Text;
                            drDemat1[3] = txtTransferable.Text;
                            drDemat1[4] = ddlAccountName.SelectedItem.Value;
                            drDemat1[5] = "SYSTM";
                            drDemat1[6] = "C";
                            drDemat1[7] = lblDammyAccID.Text;
                            drDemat1[8] = SettmentNo;
                            drDemat1[9] = SettType;
                            drDemat1[10] = "XE";
                            drDemat1[11] = DBNull.Value;
                            drDemat1[12] = drpMarginHoldbackAccount.SelectedItem.Text + " To " + ddlAccountName.SelectedItem.Text + "~" + lblAlternateProductSeriesID.Text + "~" + lblCliID.Text + "~" + SettmentNo + "~" + SettType + "~" + random;
                            DtPayInMarginHoldBack.Rows.Add(drDemat1);

                            DataRow drDemat2 = DtPayInMarginHoldBack.NewRow();
                            drDemat2[0] = lblCliID.Text;
                            drDemat2[1] = lblISIN.Text;
                            drDemat2[2] = lblProductSeriesID.Text;
                            drDemat2[3] = txtTransferable.Text;
                            drDemat2[4] = lblDammyAccID.Text;
                            drDemat2[5] = SettmentNo;
                            drDemat2[6] = SettType;
                            drDemat2[7] = ddlAccountName.SelectedItem.Value;
                            drDemat2[8] = "SYSTM";
                            drDemat2[9] = "C";
                            drDemat2[10] = "XE";
                            drDemat2[11] = DBNull.Value;
                            drDemat2[12] = drpMarginHoldbackAccount.SelectedItem.Text + " To " + ddlAccountName.SelectedItem.Text + "~" + lblAlternateProductSeriesID.Text + "~" + lblCliID.Text + "~" + SettmentNo + "~" + SettType + "~" + random;
                            DtPayInMarginHoldBack.Rows.Add(drDemat2);
                        }
                    }
                }
                string tabledata = oconverter.ConvertDataTableToXML(DtPayInMarginHoldBack);

                /* For Tier Structure -----------------------
                 String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                 SqlConnection con = new SqlConnection(conn);
                 con.Open();
                 SqlCommand com = new SqlCommand("DeliveryProcessingPayInMarginHoldBack", con);
                 com.CommandType = CommandType.StoredProcedure;
                 com.Parameters.AddWithValue("@PayInMarginHoldBack", tabledata);
                 com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
                 com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
                 com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
                 com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
                 com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());

                   NoofRowsAffect = com.ExecuteNonQuery();

                 con.Close();

                 */

                NoofRowsAffect = ObjDailyTask_Demat_Deliveries.DeliveryProcessingPayInMarginHoldBack(tabledata, Session["LastFinYear"].ToString(),
                                                                           Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                                                                             DtTransferDate.Value.ToString(), Session["userid"].ToString());


                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Transfers Generated Successfully !!');", true);
                }
            }
            #endregion
            #region Holdback Release
            if (ddlType.SelectedItem.Value == "HR")
            {
                DataTable DtHoldbackRelease = new DataTable();
                DtHoldbackRelease.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
                DtHoldbackRelease.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtHoldbackRelease.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
                DtHoldbackRelease.Columns.Add(new DataColumn("Stock", typeof(Decimal)));//3
                DtHoldbackRelease.Columns.Add(new DataColumn("DeliverTo", typeof(String)));//4
                DtHoldbackRelease.Columns.Add(new DataColumn("AccName", typeof(String)));//5
                DtHoldbackRelease.Columns.Add(new DataColumn("SourceAccID", typeof(String)));//6
                DtHoldbackRelease.Columns.Add(new DataColumn("SettlementNumber", typeof(String)));//7
                DtHoldbackRelease.Columns.Add(new DataColumn("SettlementType", typeof(String)));//8
                foreach (GridViewRow row in grdDematProcessing.Rows)
                {
                    Label lblID = (Label)row.FindControl("lblID");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblProductID = (Label)row.FindControl("lblProductID");
                    TextBox txtStock = (TextBox)row.FindControl("txtStock");
                    DropDownList ddlDeliverTo = (DropDownList)row.FindControl("ddlDeliverTo");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    Label lbSourceAccID = (Label)row.FindControl("lbSourceAccID");
                    Label lblSettNumber = (Label)row.FindControl("lblSettNumber");
                    Label lblSettType = (Label)row.FindControl("lblSettType");
                    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                    if (ChkDelivery.Checked == true)
                    {
                        DataRow drDemat = DtHoldbackRelease.NewRow();
                        drDemat[0] = lblID.Text;
                        drDemat[1] = lblISIN.Text;
                        drDemat[2] = lblProductID.Text;
                        drDemat[3] = txtStock.Text;
                        drDemat[4] = ddlDeliverTo.SelectedItem.Value;
                        drDemat[5] = ddlAccountName.SelectedItem.Value;
                        drDemat[6] = lbSourceAccID.Text;
                        drDemat[7] = lblSettNumber.Text;
                        drDemat[8] = lblSettType.Text;
                        DtHoldbackRelease.Rows.Add(drDemat);
                    }
                }
                string tabledata = oconverter.ConvertDataTableToXML(DtHoldbackRelease);
                /* For Tier Structure-------------------
                 String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                 SqlConnection con = new SqlConnection(conn);
                 con.Open();
                 SqlCommand com = new SqlCommand("DeliveryProcessingMarginHoldBack", con);
                 com.CommandType = CommandType.StoredProcedure;
                 com.Parameters.AddWithValue("@clientPayoutData", tabledata);
                 com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
                 com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
                 com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
                 com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
                 com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
                 NoofRowsAffect = com.ExecuteNonQuery();
                 con.Close();

                 */

                NoofRowsAffect = ObjDailyTask_Demat_Deliveries.DeliveryProcessingMarginHoldBack(tabledata, Session["LastFinYear"].ToString(),
                                                                          Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                                                                            DtTransferDate.Value.ToString(), Session["userid"].ToString());



                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Processing Successfully !!');", true);
                }
            }
            #endregion
            #region OffSet Position
            if (ddlType.SelectedItem.Value == "OF")
            {
                DataTable DtOffSetPosition = new DataTable();
                DtOffSetPosition.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
                DtOffSetPosition.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtOffSetPosition.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
                DtOffSetPosition.Columns.Add(new DataColumn("Qty", typeof(Decimal)));//3
                DtOffSetPosition.Columns.Add(new DataColumn("SettlementNumberSource", typeof(String)));//4
                DtOffSetPosition.Columns.Add(new DataColumn("SettlementTypeSource", typeof(String)));//5
                DtOffSetPosition.Columns.Add(new DataColumn("SettlementNumberTarget", typeof(String)));//6
                DtOffSetPosition.Columns.Add(new DataColumn("SettlementTypeTarget", typeof(String)));//7
                DtOffSetPosition.Columns.Add(new DataColumn("BranchID", typeof(String)));//8
                foreach (GridViewRow row in grdOffSetPosition.Rows)
                {
                    Label lblCliID = (Label)row.FindControl("lblCliID");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblProductSeriesID = (Label)row.FindControl("lblProductSeriesID");
                    Label lblQty = (Label)row.FindControl("lblQty");
                    Label lblSourceSettNumber = (Label)row.FindControl("lblSourceSettNumber");
                    Label lblTargetSettNumber = (Label)row.FindControl("lblTargetSettNumber");
                    Label lblBranchID = (Label)row.FindControl("lblBranchID");
                    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                    if (ChkDelivery.Checked == true)
                    {
                        DataRow drDemat = DtOffSetPosition.NewRow();
                        drDemat[0] = lblCliID.Text;
                        drDemat[1] = lblISIN.Text;
                        drDemat[2] = lblProductSeriesID.Text;
                        drDemat[3] = lblQty.Text;
                        drDemat[4] = lblSourceSettNumber.Text.ToString().Substring(0, 7);
                        drDemat[5] = lblSourceSettNumber.Text.ToString().Substring(7);
                        drDemat[6] = lblTargetSettNumber.Text.ToString().Substring(0, 7);
                        drDemat[7] = lblTargetSettNumber.Text.ToString().Substring(7);
                        drDemat[8] = lblBranchID.Text;
                        DtOffSetPosition.Rows.Add(drDemat);
                    }
                }
                string tabledata = oconverter.ConvertDataTableToXML(DtOffSetPosition);
                /* For Tier Structure-----------------
                String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand com = new SqlCommand("DeliveryProcessingForOffsetPosition", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@OffsetPositionData", tabledata);
                com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
                com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString().Trim() + '~' + txtSourceSettOff.Text.ToString().Trim() + '~' + txtTargetSettOff.Text.ToString().Trim());
                com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
                com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
                com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
                com.CommandTimeout = 0;
                NoofRowsAffect = com.ExecuteNonQuery();
                con.Close();

                */

                NoofRowsAffect = ObjDailyTask_Demat_Deliveries.DeliveryProcessingForOffsetPosition(tabledata, Session["LastFinYear"].ToString(),
                                                                       Session["LastCompany"].ToString().Trim() + '~' + txtSourceSettOff.Text.ToString().Trim() + '~' + txtTargetSettOff.Text.ToString().Trim(),
                                                                       Session["usersegid"].ToString(),
                                                                       DtTransferDate.Value.ToString(), Session["userid"].ToString());


                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Jqw", "alert('Processing Successfully !!');", true);
                }
            }
            #endregion
            //BindGridview();
            ScriptManager.RegisterStartupScript(this, GetType(), "JJ12", "HideOn();", true);
        }
        protected void GrdInterSegment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAllInterSegment('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////////AddJust Qty Checking
                Label lblPendOutgoing = (Label)e.Row.FindControl("lblPendOutgoing");
                Label lblStocksAdjustForCheck = (Label)e.Row.FindControl("lblStocksAdjustForCheck");
                ((TextBox)e.Row.FindControl("txtAdjst")).Attributes.Add("onfocusout", "javascript:fn_AddJustInterSettlement(this,this.value,'" + lblPendOutgoing.Text.ToString().Trim() + "','" + lblStocksAdjustForCheck.Text.ToString().Trim() + "')");

            }
        }
        protected void GrdMarketPayIN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAllInterSegment('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        protected void grdPayInOwnAcc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlAccountName");
                ddl.DataSource = DS.Tables[1];
                ddl.DataTextField = "OwnBankName";
                ddl.DataValueField = "ID";
                ddl.DataBind();
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        protected void grdPayInFromMarginHoldBack_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string expression = " DPID like 'IN%'";

                DataRow[] rows = DS.Tables[1].Select(expression);
                string ID = rows[0][0].ToString();
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlAccountName");
                ddl.DataSource = DS.Tables[1];
                ddl.DataTextField = "OwnBankName";
                ddl.DataValueField = "ID";
                ddl.DataBind();
                ddl.SelectedValue = ID;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ViewState["a"] = "d";
        }
        protected void grdOffSetPosition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        public void BindPoolAccount()
        {
            string ID = "";
            DataTable DtCheck = oDBEngine.GetDataTable("Master_DPAccounts", "DPAccounts_ID", " DPAccounts_AccountType='[POOL]' and DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + " and DPAccounts_DPID like 'IN%'");
            DataTable dtPool = oDBEngine.GetDataTable("Master_DPAccounts", "cast(DPAccounts_ID as varchar)+'~'+(case when DPAccounts_DPID like 'IN%' then 'NSDL' else 'CDSL' end) as ID,DPAccounts_ShortName", " DPAccounts_AccountType in('[POOL]','[PLPAYIN]','[PLEPAY]') and DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + "");
            ddlPoolAC.DataSource = dtPool;
            ddlPoolAC.DataTextField = "DPAccounts_ShortName";
            ddlPoolAC.DataValueField = "ID";
            ddlPoolAC.DataBind();
            if (DtCheck.Rows.Count > 0)
            {
                ID = DtCheck.Rows[0][0].ToString() + "~" + "NSDL";
                ddlPoolAC.SelectedValue = ID;
            }
            DataTable DtInterSegPool = oDBEngine.GetDataTable("Master_DPAccounts", "DPAccounts_ID,DPAccounts_ShortName", " DPAccounts_AccountType in('[POOL]','[PLPAYIN]','[PLPAYOUT]') and DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + "");
            ddlSourceAccount.DataSource = DtInterSegPool;
            ddlSourceAccount.DataTextField = "DPAccounts_ShortName";
            ddlSourceAccount.DataValueField = "DPAccounts_ID";
            ddlSourceAccount.DataBind();
            DataTable DtInterSegPoolTarget = oDBEngine.GetDataTable("Master_DPAccounts", "DPAccounts_ID,DPAccounts_ShortName", " DPAccounts_AccountType in('[POOL]','[PLPAYIN]') and DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + "");
            ddlTargetAccount.DataSource = DtInterSegPoolTarget;
            ddlTargetAccount.DataTextField = "DPAccounts_ShortName";
            ddlTargetAccount.DataValueField = "DPAccounts_ID";
            ddlTargetAccount.DataBind();

            DtInterSegPool.Rows.Clear();
            DtInterSegPool.Dispose();
            DtInterSegPool = oDBEngine.GetDataTable("(select CAST(DPACCOUNTS_ID AS VARCHAR)+'~'+CAST(DPACCOUNTS_EXCHANGESEGMENTID AS VARCHAR) AS ID,DPACCOUNTS_SHORTNAME from MASTER_DPACCOUNTS where DPACCOUNTS_ACCOUNTTYPE IN('[MRGIN]','[HOLDBK]') and DPACCOUNTS_ExchangeSegmentID='" + Session["usersegid"].ToString() + "' union all select CAST(DPACCOUNTS_ID AS VARCHAR)+'~'+CAST(DPACCOUNTS_EXCHANGESEGMENTID AS VARCHAR) AS ID,DPACCOUNTS_SHORTNAME from MASTER_DPACCOUNTS where DPACCOUNTS_ACCOUNTTYPE IN('[MRGIN]','[HOLDBK]') and DPACCOUNTS_ExchangeSegmentID<>'" + Session["usersegid"].ToString() + "') as kk", "ID,DPACCOUNTS_SHORTNAME", null);
            drpMarginHoldbackAccount.DataSource = DtInterSegPool;
            drpMarginHoldbackAccount.DataTextField = "DPACCOUNTS_SHORTNAME";
            drpMarginHoldbackAccount.DataValueField = "ID";
            drpMarginHoldbackAccount.DataBind();

            DtInterSegPool.Rows.Clear();
            DtInterSegPool.Dispose();
            DtInterSegPool = oDBEngine.GetDataTable("MASTER_DPACCOUNTS", "CAST(DPACCOUNTS_ID AS VARCHAR)+'~'+CAST(DPACCOUNTS_EXCHANGESEGMENTID AS VARCHAR) AS ID,DPACCOUNTS_SHORTNAME ", " DPAccounts_AccountType like '%Own%'");
            drpOwnAccount.DataSource = DtInterSegPool;
            drpOwnAccount.DataTextField = "DPACCOUNTS_SHORTNAME";
            drpOwnAccount.DataValueField = "ID";
            drpOwnAccount.DataBind();
        }
        protected void grdDematProcessing_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, " ASC");
            }
        }
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }

        }
        private void SortGridView(string sortExpression, string direction)
        {
            DataTable dtSorting = DS.Tables[0];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            grdDematProcessing.DataSource = dv;
            grdDematProcessing.DataBind();
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ProcessType = String.Empty;
            #region Inter Settlement
            if (ddlType.SelectedItem.Value == "IS")
            {
                ProcessType = "Inter Settlement";
                DataTable DtInterSegment = new DataTable();
                DtInterSegment.Columns.Add(new DataColumn("Client", typeof(String))); //0
                DtInterSegment.Columns.Add(new DataColumn("Scrip", typeof(String)));//1
                DtInterSegment.Columns.Add(new DataColumn("Settl.Source", typeof(String)));//2
                DtInterSegment.Columns.Add(new DataColumn("Pending Outgoing", typeof(Decimal)));//3
                DtInterSegment.Columns.Add(new DataColumn("Settl.Target", typeof(String)));//4
                DtInterSegment.Columns.Add(new DataColumn("Pending InComing", typeof(String)));//5
                DtInterSegment.Columns.Add(new DataColumn("Adjustment", typeof(String)));//6
                foreach (GridViewRow row in GrdInterSegment.Rows)
                {
                    Label lblClientName = (Label)row.FindControl("lblClientName");
                    Label lblScripName = (Label)row.FindControl("lblScripName");
                    Label lblSettSource = (Label)row.FindControl("lblSettSource");
                    Label lblPendOutgoing = (Label)row.FindControl("lblPendOutgoing");
                    Label lblSettTarget = (Label)row.FindControl("lblSettTarget");
                    Label lblPendgOutgoing = (Label)row.FindControl("lblPendgIncoming");
                    TextBox txtAdjst = (TextBox)row.FindControl("txtAdjst");
                    DataRow drInterSettlement = DtInterSegment.NewRow();
                    drInterSettlement[0] = lblClientName.Text;
                    drInterSettlement[1] = lblScripName.Text;
                    drInterSettlement[2] = lblSettSource.Text;
                    drInterSettlement[3] = lblPendOutgoing.Text;
                    drInterSettlement[4] = lblSettTarget.Text;

                    drInterSettlement[5] = lblPendgOutgoing.Text;


                    drInterSettlement[6] = txtAdjst.Text;
                    DtInterSegment.Rows.Add(drInterSettlement);
                }
                Export.Dispose();
                Export.Rows.Clear();
                Export = DtInterSegment.Copy();
            }
            #endregion
            #region Payin From  Margin/HoldBack
            else if (ddlType.SelectedItem.Value == "PH")
            {
                ProcessType = "Payin From  Margin/HoldBack";
                DataTable DtPayInMarginHoldBack = new DataTable();
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("Client Name", typeof(String))); //0
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("Scrip", typeof(String)));//1
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("ISIN", typeof(String)));//2
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("Settl.Number", typeof(String)));//3
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("Pending InComing", typeof(String)));//4
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("Transfer Quantity", typeof(Decimal)));//5
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("From A/C", typeof(String)));//6
                DtPayInMarginHoldBack.Columns.Add(new DataColumn("To A/C", typeof(String)));//6
                foreach (GridViewRow row in grdPayInFromMarginHoldBack.Rows)
                {
                    Label lblCustomerName = (Label)row.FindControl("lblCustomerName");
                    Label lblScrip = (Label)row.FindControl("lblScrip");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblSettNumType = (Label)row.FindControl("lblSettNumType");
                    Label lblIncoming = (Label)row.FindControl("lblIncoming");
                    TextBox txtTransferable = (TextBox)row.FindControl("txtTransferable");
                    Label lblSourceAccName = (Label)row.FindControl("lblSourceAccName");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    DataRow drMarginHoldBack = DtPayInMarginHoldBack.NewRow();
                    drMarginHoldBack[0] = lblCustomerName.Text;
                    drMarginHoldBack[1] = lblScrip.Text;
                    drMarginHoldBack[2] = lblISIN.Text;
                    drMarginHoldBack[3] = lblSettNumType.Text;
                    drMarginHoldBack[4] = lblIncoming.Text;
                    drMarginHoldBack[5] = txtTransferable.Text;
                    drMarginHoldBack[6] = lblSourceAccName.Text;
                    drMarginHoldBack[7] = ddlAccountName.SelectedItem.Text;
                    DtPayInMarginHoldBack.Rows.Add(drMarginHoldBack);
                }
                Export.Dispose();
                Export.Rows.Clear();
                Export = DtPayInMarginHoldBack.Copy();
            }
            #endregion
            #region Payin From Own Account
            else if (ddlType.SelectedItem.Value == "PO")
            {
                ProcessType = "Payin From Own Account";
                DataTable DtPayInOwnAcc = new DataTable();
                DtPayInOwnAcc.Columns.Add(new DataColumn("Scrip", typeof(String))); //0
                DtPayInOwnAcc.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
                DtPayInOwnAcc.Columns.Add(new DataColumn("Settl.Number", typeof(String)));//2
                DtPayInOwnAcc.Columns.Add(new DataColumn("To Receivable", typeof(String)));//3
                DtPayInOwnAcc.Columns.Add(new DataColumn("To Transferable", typeof(String)));//4
                DtPayInOwnAcc.Columns.Add(new DataColumn("From A/C", typeof(String)));//5
                DtPayInOwnAcc.Columns.Add(new DataColumn("To A/C", typeof(String)));//6
                foreach (GridViewRow row in grdPayInOwnAcc.Rows)
                {
                    Label lblScrip = (Label)row.FindControl("lblScrip");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblSettNumber = (Label)row.FindControl("lblSettNumber");
                    Label lblIncoming = (Label)row.FindControl("lblIncoming");
                    TextBox txtTransferable = (TextBox)row.FindControl("txtTransferable");
                    Label lblSourceAccName = (Label)row.FindControl("lblSourceAccName");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    DataRow drPayInOwnAC = DtPayInOwnAcc.NewRow();
                    drPayInOwnAC[0] = lblScrip.Text;
                    drPayInOwnAC[1] = lblISIN.Text;
                    drPayInOwnAC[2] = lblSettNumber.Text;
                    drPayInOwnAC[3] = lblIncoming.Text;
                    drPayInOwnAC[4] = txtTransferable.Text;
                    drPayInOwnAC[5] = lblSourceAccName.Text;
                    drPayInOwnAC[6] = ddlAccountName.SelectedItem.Text;
                    DtPayInOwnAcc.Rows.Add(drPayInOwnAC);
                }
                Export.Dispose();
                Export.Rows.Clear();
                Export = DtPayInOwnAcc.Copy();
            }
            #endregion
            #region Payin From Own Account
            else if (ddlType.SelectedItem.Value == "MP")
            {
                ProcessType = "Market PayIN";
                DataTable DtMarketPayIN = new DataTable();
                DtMarketPayIN.Columns.Add(new DataColumn("ExchangeName", typeof(String))); //0
                DtMarketPayIN.Columns.Add(new DataColumn("Scrip", typeof(String)));//1
                DtMarketPayIN.Columns.Add(new DataColumn("ISIN", typeof(String)));//2
                DtMarketPayIN.Columns.Add(new DataColumn("Qty To Deliver", typeof(String)));//3
                DtMarketPayIN.Columns.Add(new DataColumn("Deliverable", typeof(String)));//4
                foreach (GridViewRow row in GrdMarketPayIN.Rows)
                {
                    Label lblClientName = (Label)row.FindControl("lblClientName");
                    Label lblScripName = (Label)row.FindControl("lblScripName");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblDeliver = (Label)row.FindControl("lblDeliver");
                    TextBox txtDeliverable = (TextBox)row.FindControl("txtDeliverable");
                    DataRow drMarketPayIN = DtMarketPayIN.NewRow();
                    drMarketPayIN[0] = lblClientName.Text;
                    drMarketPayIN[1] = lblScripName.Text;
                    drMarketPayIN[2] = lblISIN.Text;
                    drMarketPayIN[3] = lblDeliver.Text;
                    drMarketPayIN[4] = txtDeliverable.Text;
                    DtMarketPayIN.Rows.Add(drMarketPayIN);
                }
                Export.Dispose();
                Export.Rows.Clear();
                Export = DtMarketPayIN.Copy();
            }
            #endregion
            #region Client PayOut
            else if (ddlType.SelectedItem.Value == "CP")
            {
                ProcessType = "Client Payout";
                DataTable DtClientPayOut = new DataTable();
                DtClientPayOut.Columns.Add(new DataColumn("Client", typeof(String))); //0
                DtClientPayOut.Columns.Add(new DataColumn("UCC", typeof(String)));//1
                DtClientPayOut.Columns.Add(new DataColumn("Scrip", typeof(String)));//2
                DtClientPayOut.Columns.Add(new DataColumn("ISIN", typeof(String)));//3
                DtClientPayOut.Columns.Add(new DataColumn("To Deliver", typeof(String)));//4
                DtClientPayOut.Columns.Add(new DataColumn("Deliverable", typeof(String)));//5
                DtClientPayOut.Columns.Add(new DataColumn("From", typeof(String)));//6
                DtClientPayOut.Columns.Add(new DataColumn("Deliver To", typeof(String)));//7
                DtClientPayOut.Columns.Add(new DataColumn("Account Name", typeof(String)));//8
                foreach (GridViewRow row in grdDematProcessing.Rows)
                {
                    Label lblClient = (Label)row.FindControl("lblClient");
                    Label lblUCC = (Label)row.FindControl("lblUCC");
                    Label lblScrip = (Label)row.FindControl("lblScrip");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblQtyDeliver = (Label)row.FindControl("lblQtyDeliver");
                    TextBox txtStock = (TextBox)row.FindControl("txtStock");
                    Label lblFromAccount = (Label)row.FindControl("lblFromAccount");
                    DropDownList ddlDeliverTo = (DropDownList)row.FindControl("ddlDeliverTo");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    DataRow drClientPayOut = DtClientPayOut.NewRow();
                    drClientPayOut[0] = lblClient.Text;
                    drClientPayOut[1] = lblUCC.Text;
                    drClientPayOut[2] = lblScrip.Text;
                    drClientPayOut[3] = lblISIN.Text;
                    drClientPayOut[4] = lblQtyDeliver.Text;
                    drClientPayOut[5] = txtStock.Text;
                    drClientPayOut[6] = lblFromAccount.Text;
                    if (ddlDeliverTo.SelectedItem != null)
                        drClientPayOut[7] = ddlDeliverTo.SelectedItem.Text;
                    else
                        drClientPayOut[7] = "";
                    if (ddlAccountName.SelectedItem != null)
                        drClientPayOut[8] = ddlAccountName.SelectedItem.Text;
                    else
                        drClientPayOut[8] = "";
                    DtClientPayOut.Rows.Add(drClientPayOut);
                }
                Export.Dispose();
                Export.Rows.Clear();
                Export = DtClientPayOut.Copy();
            }
            #endregion
            #region HoldBack Release
            else if (ddlType.SelectedItem.Value == "HR")
            {
                ProcessType = "HoldBack Release";
                DataTable DtHoldBack = new DataTable();
                DtHoldBack.Columns.Add(new DataColumn("Client", typeof(String))); //0
                DtHoldBack.Columns.Add(new DataColumn("UCC", typeof(String)));//1
                DtHoldBack.Columns.Add(new DataColumn("Scrip", typeof(String)));//2
                DtHoldBack.Columns.Add(new DataColumn("ISIN", typeof(String)));//3
                DtHoldBack.Columns.Add(new DataColumn("To Deliver", typeof(String)));//4
                DtHoldBack.Columns.Add(new DataColumn("Deliverable", typeof(String)));//5
                DtHoldBack.Columns.Add(new DataColumn("Account Name", typeof(String)));//6
                foreach (GridViewRow row in grdDematProcessing.Rows)
                {
                    Label lblClient = (Label)row.FindControl("lblClient");
                    Label lblUCC = (Label)row.FindControl("lblUCC");
                    Label lblScrip = (Label)row.FindControl("lblScrip");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblQtyDeliver = (Label)row.FindControl("lblQtyDeliver");
                    TextBox txtStock = (TextBox)row.FindControl("txtStock");
                    DropDownList ddlAccountName = (DropDownList)row.FindControl("ddlAccountName");
                    DataRow drHoldBack = DtHoldBack.NewRow();
                    drHoldBack[0] = lblClient.Text;
                    drHoldBack[1] = lblUCC.Text;
                    drHoldBack[2] = lblScrip.Text;
                    drHoldBack[3] = lblISIN.Text;
                    drHoldBack[4] = lblQtyDeliver.Text;
                    drHoldBack[5] = txtStock.Text;
                    if (ddlAccountName.SelectedItem != null)
                        drHoldBack[6] = ddlAccountName.SelectedItem.Text;
                    else
                        drHoldBack[6] = "";
                    DtHoldBack.Rows.Add(drHoldBack);
                }
                Export.Dispose();
                Export.Rows.Clear();
                Export = DtHoldBack.Copy();
            }
            #endregion
            #region OffSet Position
            else if (ddlType.SelectedItem.Value == "OF")
            {
                ProcessType = "OffSet Position";
                DataTable DtOffSetPos = new DataTable();
                DtOffSetPos.Columns.Add(new DataColumn("Client Name", typeof(String))); //0
                DtOffSetPos.Columns.Add(new DataColumn("Scrip", typeof(String)));//1
                DtOffSetPos.Columns.Add(new DataColumn("ISIN", typeof(String)));//2
                DtOffSetPos.Columns.Add(new DataColumn("Qty", typeof(String)));//3
                DtOffSetPos.Columns.Add(new DataColumn("Sell Position Sett.", typeof(String)));//4
                DtOffSetPos.Columns.Add(new DataColumn("Buy Position Sett.", typeof(String)));//5
                foreach (GridViewRow row in grdOffSetPosition.Rows)
                {
                    Label lblCustomerName = (Label)row.FindControl("lblCustomerName");
                    Label lblScrip = (Label)row.FindControl("lblScrip");
                    Label lblISIN = (Label)row.FindControl("lblISIN");
                    Label lblQty = (Label)row.FindControl("lblQty");
                    Label lblTargetSettNumber = (Label)row.FindControl("lblTargetSettNumber");
                    Label lblSourceSettNumber = (Label)row.FindControl("lblSourceSettNumber");
                    DataRow drOffSetPos = DtOffSetPos.NewRow();
                    drOffSetPos[0] = lblCustomerName.Text;
                    drOffSetPos[1] = lblScrip.Text;
                    drOffSetPos[2] = lblISIN.Text;
                    drOffSetPos[3] = lblQty.Text;
                    drOffSetPos[4] = lblTargetSettNumber.Text;
                    drOffSetPos[5] = lblSourceSettNumber.Text;
                    DtOffSetPos.Rows.Add(drOffSetPos);
                }
                Export.Dispose();
                Export.Rows.Clear();
                Export = DtOffSetPos.Copy();
            }
            #endregion
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = " Delivery Processing For : " + ProcessType;
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(Export, "Delivery Processing", "Closing Balance", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(Export, "Delivery Processing", "Closing Balance", dtReportHeader, dtReportFooter);
            }
        }

        protected void grdDematProcessing_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DelvTo")
            {
                string customerID = null;
                string expression = string.Empty;
                GridViewRow Row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DropDownList ddlDeliverTo = (DropDownList)Row.FindControl("ddlDeliverTo");
                DropDownList ddl = (DropDownList)Row.FindControl("ddlAccountName");
                Label lblID = (Label)Row.FindControl("lblID");
                string valDeliveryTo = ddlDeliverTo.SelectedItem.Value;

                if (valDeliveryTo == "CA")
                {
                    DataTable mytable = new DataTable();
                    DataColumn productidcolumn = new DataColumn("ID");
                    DataColumn productnamecolumn = new DataColumn("ClientBankName");
                    mytable.Columns.Add(productidcolumn);
                    mytable.Columns.Add(productnamecolumn);
                    string ID = null;
                    customerID = lblID.Text;
                    expression = "CustomerID = '" + customerID + "'";

                    string expression1 = "CustomerID = '" + customerID + "' and AcType='Primary'";

                    DataRow[] rows1 = DS.Tables[1].Select(expression1);
                    if (rows1.Length > 0)
                    {
                        ID = rows1[0][2].ToString();
                    }
                    DataRow[] rows = DS.Tables[1].Select(expression);
                    foreach (DataRow row1 in rows)
                    {
                        DataRow newrow = mytable.NewRow();
                        newrow["ID"] = row1["ID"];
                        newrow["ClientBankName"] = row1["ClientBankName"];
                        mytable.Rows.Add(newrow);
                    }
                    ddl.DataSource = mytable;
                    ddl.DataTextField = "ClientBankName";
                    ddl.DataValueField = "ID";
                    ddl.DataBind();
                    if (rows1.Length > 0)
                    {
                        ddl.SelectedValue = ID;
                    }

                }
                else if (valDeliveryTo == "MA")
                {
                    ddl.DataSource = DS.Tables[2];
                    ddl.DataValueField = "ID";
                    ddl.DataTextField = "ShortName";
                    ddl.DataBind();
                }
                else
                {
                    ddl.DataSource = DS.Tables[3];
                    ddl.DataValueField = "ID";
                    ddl.DataTextField = "ShortName";
                    ddl.DataBind();
                }

            }
        }
        protected void GrdInterSegment_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridViewIntSeg(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridViewIntSeg(sortExpression, " ASC");
            }
        }
        private void SortGridViewIntSeg(string sortExpression, string direction)
        {
            DataTable dtSorting = (DataTable)ViewState["DSInterSegment"];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            GrdInterSegment.DataSource = dv;
            GrdInterSegment.DataBind();
        }
        private void SortGridViewPayInMrgnHoldBack(string sortExpression, string direction)
        {
            DataTable dtSorting = DS.Tables[0];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            grdPayInFromMarginHoldBack.DataSource = dv;
            grdPayInFromMarginHoldBack.DataBind();
        }
        private void SortGridViewMrktPayIN(string sortExpression, string direction)
        {
            DataTable dtSorting = (DataTable)ViewState["DtMarketPayIn"];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            GrdMarketPayIN.DataSource = dv;
            GrdMarketPayIN.DataBind();
        }
        private void SortGridViewPayINOwnAcc(string sortExpression, string direction)
        {
            DataTable dtSorting = DS.Tables[0];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            grdPayInOwnAcc.DataSource = dv;
            grdPayInOwnAcc.DataBind();
        }
        private void SortGridViewOffSetPos(string sortExpression, string direction)
        {
            DataTable dtSorting = (DataTable)ViewState["DtOffSetPos"];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            grdOffSetPosition.DataSource = dv;
            grdOffSetPosition.DataBind();
        }
        protected void GrdMarketPayIN_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridViewMrktPayIN(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridViewMrktPayIN(sortExpression, " ASC");
            }
        }
        protected void grdPayInOwnAcc_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridViewPayINOwnAcc(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridViewPayINOwnAcc(sortExpression, " ASC");
            }
        }
        protected void grdPayInFromMarginHoldBack_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridViewPayInMrgnHoldBack(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridViewPayInMrgnHoldBack(sortExpression, " ASC");
            }
        }
        protected void grdOffSetPosition_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridViewOffSetPos(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridViewOffSetPos(sortExpression, " ASC");
            }
        }


        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


    }
}