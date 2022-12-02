using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmClosingRatelist : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        GenericMethod oGenericMethod = new GenericMethod();

        public string _ExchangeSegmentID
        {
            get { return (string)Session["ExchangeSegmentID"]; }
        }
        static string data;
        static string SubClients = "";
        static string Instruments;
        string Segid = "";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            gridClosingDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            gridVarDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Segid = Session["ExchangeSegmentID"].ToString();

            //txtSelectionID.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetAssetsOrDerivative("D", Convert.ToInt32(Segid), 0, "NA") + "')");
            //txtSelectionID.Attributes.Add("onkeyup", "showOptions(this,'SearchByInstruments',event)");
            cmbForDate.EditFormatString = OConvert.GetDateFormat("Date");
            txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
            txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");



            if (!IsPostBack)
            {
                rdInstrumentAll.Attributes.Add("OnClick", "rdbtnSegAll('Instrument')");
                rdInstrumentSelected.Attributes.Add("OnClick", "rdbtnSelected('Instrument')");
                Instruments = null;
                txtFromDate.Value = Convert.ToDateTime(DateTime.Today);
                txtToDate.Value = Convert.ToDateTime(DateTime.Today);
                cmbForDate.Value = Convert.ToDateTime(DateTime.Today);
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");


            }
            else
            {

                if (rbClosingRate.Checked == true)
                    fillGrid();
                else
                    FillGridVar();

            }
            //_____For performing operation without refreshing page___//


            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>divscroll('" + HttpContext.Current.Session["ExchangeSegmentID"] + "');</script>");

            if (rdInstrumentAll.Checked == true)
            {
                dtTo_hidden.Text = cmbForDate.Text;
            }
            else
            {
                dtTo_hidden.Text = txtFromDate.Text;
            }



        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Clients")
                    {
                        if (idlist[0] == "SettlementType")
                        {
                            string[] val = cl[i].Split(';');
                            if (str == "")
                            {
                                str = "'" + val[0] + "'";
                                str1 = "'" + val[0] + "'" + ";" + val[1];
                            }
                            else
                            {
                                str += ",'" + val[0] + "'";
                                str1 += "," + "'" + val[0] + "'" + ";" + val[1];
                            }
                        }
                        else
                        {
                            string[] val = cl[i].Split(';');
                            if (str == "")
                            {
                                str = val[0];
                                str1 = val[0] + ";" + val[1];
                            }
                            else
                            {
                                str += "," + val[0];
                                str1 += "," + val[0] + ";" + val[1];
                            }
                        }
                    }
                    else
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }


                if (idlist[0] == "Instruments")
                {
                    Instruments = str;
                    // data = "Instruments~" + str1;
                    data = str;
                }

            }
        }

        protected void btn_show_Click(object sender, EventArgs e)
        {
            if (rbClosingRate.Checked == true)
                fillGrid();
            else
                FillGridVar();


        }
        protected void FillGridVar()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
            {
                string stlist = data;
                if (rdInstrumentAll.Checked == true)
                {
                    string fromDate = cmbForDate.Date.Month.ToString() + "/" + cmbForDate.Date.Day.ToString() + "/" + cmbForDate.Date.Year.ToString();
                    gridVarDataSource.SelectCommand = "select DailyVar_ID,convert(varchar, DailyVar_Date, 106) as DailyVar_Date, DailyVar_SecurityVar,DailyVar_IndexVar,DailyVar_VarMargin,DailyVar_ExtremeLossRate,DailyVar_AdhocMargin" +
                    ",DailyVar_SpecialMargin,DailyVar_AppMargin,DailyVar_ProductSeriesID,DailyVar_ExchangeSegmentID,Equity_TickerSymbol from trans_dailyvar,master_equity where DailyVar_ProductSeriesID=Equity_SeriesID and DailyVar_Date='" + fromDate + "' and DailyVar_ExchangeSegmentID=" + HttpContext.Current.Session["ExchangeSegmentID"].ToString();

                    gridVarRate.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscriptvar2", "ForVarFilterOff();", true);
                    string Spantext = "Var Rate For : ";
                    string SpanText1 = OConvert.ArrangeDate2(cmbForDate.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSctvar9", "ShowHideVar('" + SpanText1 + "','" + Spantext + "')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript3", "height();", true);
                }
                else
                {
                    if (stlist != null)
                    {
                        string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString();
                        string enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString();
                        gridVarDataSource.SelectCommand = "select DailyVar_ID,convert(varchar, DailyVar_Date, 106) as DailyVar_Date, DailyVar_SecurityVar,DailyVar_IndexVar,DailyVar_VarMargin,DailyVar_ExtremeLossRate,DailyVar_AdhocMargin" +
                   ",DailyVar_SpecialMargin,DailyVar_AppMargin,DailyVar_ProductSeriesID,DailyVar_ExchangeSegmentID,Equity_TickerSymbol from trans_dailyvar,master_equity where  DailyVar_ProductSeriesID=Equity_SeriesID and (cast(DailyVar_Date as Datetime)>=Convert(varchar,'" + startdate + "',101) and  cast(DailyVar_Date as Datetime)<=convert(varchar,'" + enddate + "',101)) and DailyVar_ProductSeriesID in (" + stlist + ") and DailyVar_ExchangeSegmentID=" + HttpContext.Current.Session["ExchangeSegmentID"].ToString();

                        gridVarRate.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscriptvar2", "ForVarFilterOff();", true);
                        string Spantext = "Var Rate of Period : ";
                        string SpanText1 = OConvert.ArrangeDate2(txtFromDate.Value.ToString()) + "  - " + OConvert.ArrangeDate2(txtToDate.Value.ToString());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSctvar9", "ShowHideVar('" + SpanText1 + "','" + Spantext + "')", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript3", "height();", true);
                    }


                }
            }



        }
        protected void fillGrid()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
            {
                string stlist = data;
                if (rdInstrumentAll.Checked == true)
                {
                    string fromDate = cmbForDate.Date.Month.ToString() + "/" + cmbForDate.Date.Day.ToString() + "/" + cmbForDate.Date.Year.ToString();
                    // gridClosingDataSource.SelectCommand = "select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'') from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,(select Equity_FoIdentifier from master_equity where Equity_SeriesID=DailyStat_ProductSeriesID) as instruments,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where DailyStat_DateTime='" + fromDate + "' and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'";
                    gridClosingDataSource.SelectCommand = "select * from (select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')+' '+ isnull(convert(varchar(9),Equity_EffectUntil,6),'')+' '+isnull(cast(cast(round(Equity_StrikePrice,2) as numeric(28,4)) as varchar),'')   from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,rtrim(ltrim(DailyStat_ProductSeriesID))as DailyStat_ProductSeriesID,ltrim(rtrim(DailyStat_DateTime)) as DailyStat_DateTime,ltrim(rtrim(cast(DailyStat_Open as decimal(18,4)))) as DailyStat_Open ,ltrim(rtrim(cast(DailyStat_High as decimal(18,4)))) as DailyStat_High,ltrim(rtrim(cast(DailyStat_Low as decimal(18,4)))) as DailyStat_Low,ltrim(rtrim(cast(DailyStat_Close as decimal(18,4)))) as DailyStat_Close,ltrim(rtrim(cast(DailyStat_SettlementPrice as decimal(18,4)))) as DailyStat_SettlementPrice ,ltrim(rtrim(cast(DailyStat_AssetPrice as decimal(18,4)))) as DailyStat_AssetPrice  From  trans_dailystatistics where DailyStat_DateTime='" + fromDate + "' and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "')as T       order by  T.instruments,Dailystat_DateTime asc";
                    gridClosing.DataBind();
                    gridClosing.Columns[8].Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript1", "height();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript2", "ForFilterOff();", true);
                    string Spantext = "Closing Rate For : ";
                    string SpanText1 = OConvert.ArrangeDate2(cmbForDate.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "ShowHide('" + SpanText1 + "','" + Spantext + "')", true);

                }
                else
                {
                    if (stlist != null)
                    {
                        string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString();
                        string enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString();
                        // gridClosingDataSource.SelectCommand = "select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')  from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,(select Equity_FoIdentifier from master_equity where Equity_SeriesID=DailyStat_ProductSeriesID) as instruments,DailyStat_DateTime,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where (CAST(DailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(DailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + "', 101)) and   DailyStat_ProductSeriesID in (" + stlist + ")  and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'";
                        gridClosingDataSource.SelectCommand = "select * from (select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')  from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,rtrim(ltrim(DailyStat_ProductSeriesID))as DailyStat_ProductSeriesID,ltrim(rtrim(DailyStat_DateTime)) as DailyStat_DateTime,ltrim(rtrim(cast(DailyStat_Open as decimal(18,4)))) as DailyStat_Open ,ltrim(rtrim(cast(DailyStat_High as decimal(18,4)))) as DailyStat_High,ltrim(rtrim(cast(DailyStat_Low as decimal(18,4)))) as DailyStat_Low,ltrim(rtrim(cast(DailyStat_Close as decimal(18,4)))) as DailyStat_Close,ltrim(rtrim(cast(DailyStat_SettlementPrice as decimal(18,4)))) as DailyStat_SettlementPrice ,ltrim(rtrim(cast(DailyStat_AssetPrice as decimal(18,4)))) as DailyStat_AssetPrice  From  trans_dailystatistics where (CAST(DailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(DailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + "', 101)) and   DailyStat_ProductSeriesID in (" + stlist + ")  and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "')as T      order by T.instruments,Dailystat_DateTime asc";


                        gridClosing.DataBind();
                        gridClosing.Columns[8].Visible = false;

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript4", "ForFilterOff();", true);
                        string Spantext = "Closing Rate of Period : ";
                        string SpanText1 = OConvert.ArrangeDate2(txtFromDate.Value.ToString()) + "  - " + OConvert.ArrangeDate2(txtToDate.Value.ToString());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "','" + Spantext + "')", true);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript3", "height();", true);
                    }
                }
            }
            else if ((HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20"))
            {
                string stlist = data;
                if (rdInstrumentAll.Checked == true)
                {
                    string fromDate = cmbForDate.Date.Month.ToString() + "/" + cmbForDate.Date.Day.ToString() + "/" + cmbForDate.Date.Year.ToString();
                    //   gridClosingDataSource.SelectCommand = "select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')+' '+ isnull(convert(varchar(9),Equity_EffectUntil,6),'')+' '+isnull(cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar),'')   from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,(select Equity_FoIdentifier from master_equity where Equity_SeriesID=DailyStat_ProductSeriesID) as instruments,DailyStat_DateTime,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where DailyStat_DateTime='" + fromDate + "' and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'";
                    gridClosingDataSource.SelectCommand = "select * from (select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,4) as numeric(28,4)) as varchar) end) from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments ,rtrim(ltrim(DailyStat_ProductSeriesID))as DailyStat_ProductSeriesID,ltrim(rtrim(DailyStat_DateTime)) as DailyStat_DateTime,ltrim(rtrim(cast(DailyStat_Open as decimal(18,4)))) as DailyStat_Open ,ltrim(rtrim(cast(DailyStat_High as decimal(18,4)))) as DailyStat_High,ltrim(rtrim(cast(DailyStat_Low as decimal(18,4)))) as DailyStat_Low,ltrim(rtrim(cast(DailyStat_Close as decimal(18,4)))) as DailyStat_Close,ltrim(rtrim(cast(DailyStat_SettlementPrice as decimal(18,4)))) as DailyStat_SettlementPrice ,ltrim(rtrim(cast(DailyStat_AssetPrice as decimal(18,4)))) as DailyStat_AssetPrice  From  trans_dailystatistics where DailyStat_DateTime='" + fromDate + "' and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "')as T      order by  T.instruments,Dailystat_DateTime asc";
                    gridClosing.DataBind();
                    gridClosing.Columns[8].Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript2", "ForFilterOff();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript1", "height();", true);
                    string Spantext = "Closing Rate For Date : ";
                    string SpanText1 = OConvert.ArrangeDate2(cmbForDate.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "ShowHide('" + SpanText1 + "','" + Spantext + "')", true);

                }
                else
                {
                    if (stlist != null)
                    {
                        string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString();
                        string enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString();
                        // gridClosingDataSource.SelectCommand = "select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')+' '+ isnull(convert(varchar(9),Equity_EffectUntil,6),'')+' '+isnull(cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar),'')   from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,(select Equity_FoIdentifier from master_equity where Equity_SeriesID=DailyStat_ProductSeriesID) as instruments,DailyStat_DateTime,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where (CAST(DailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(DailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + "', 101)) and   DailyStat_ProductSeriesID in (" + stlist + ")  and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'";
                        //gridClosingDataSource.SelectCommand = "select * from (select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')+' '+ isnull(convert(varchar(9),Equity_EffectUntil,6),'')+' '+isnull(cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar),'')   from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,DailyStat_DateTime,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where (CAST(DailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(DailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + "', 101)) and   DailyStat_ProductSeriesID in (" + stlist + ")  and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "')as T order by instruments";
                        gridClosingDataSource.SelectCommand = "select * from (select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,4) as numeric(28,4)) as varchar) end) from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,rtrim(ltrim(DailyStat_ProductSeriesID))as DailyStat_ProductSeriesID,ltrim(rtrim(DailyStat_DateTime)) as DailyStat_DateTime,ltrim(rtrim(cast(DailyStat_Open as decimal(18,4)))) as DailyStat_Open ,ltrim(rtrim(cast(DailyStat_High as decimal(18,4)))) as DailyStat_High,ltrim(rtrim(cast(DailyStat_Low as decimal(18,4)))) as DailyStat_Low,ltrim(rtrim(cast(DailyStat_Close as decimal(18,4)))) as DailyStat_Close,ltrim(rtrim(cast(DailyStat_SettlementPrice as decimal(18,4)))) as DailyStat_SettlementPrice ,ltrim(rtrim(cast(DailyStat_AssetPrice as decimal(18,4)))) as DailyStat_AssetPrice  From  trans_dailystatistics where (CAST(DailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(DailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + "', 101)) and   DailyStat_ProductSeriesID in (" + stlist + ")  and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "')as T      order by  T.instruments,Dailystat_DateTime asc";
                        gridClosing.DataBind();
                        gridClosing.Columns[8].Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript3", "height();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript4", "ForFilterOff();", true);

                        string Spantext = "Closing Rate of Period :";
                        string SpanText1 = OConvert.ArrangeDate2(txtFromDate.Value.ToString()) + "  - " + OConvert.ArrangeDate2(txtToDate.Value.ToString());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "','" + Spantext + "')", true);

                    }
                }

            }
            else
            {

                string stlist = data;
                if (rdInstrumentAll.Checked == true)
                {
                    string fromDate = cmbForDate.Date.Month.ToString() + "/" + cmbForDate.Date.Day.ToString() + "/" + cmbForDate.Date.Year.ToString();
                    //   gridClosingDataSource.SelectCommand = "select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')+' '+ isnull(convert(varchar(9),Equity_EffectUntil,6),'')+' '+isnull(cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar),'')   from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,(select Equity_FoIdentifier from master_equity where Equity_SeriesID=DailyStat_ProductSeriesID) as instruments,DailyStat_DateTime,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where DailyStat_DateTime='" + fromDate + "' and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'";
                    gridClosingDataSource.SelectCommand = @"select * from ( select ComDailyStat_ID as DailyStat_ID, 
                convert(varchar, ComDailystat_DateTime, 105)  as Dailystat_DateTime1, ComDailyStat_ExchangeSegmentID, 
                (Select (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+' '+
                cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+' '+convert(varchar(11),Commodity_ExpiryDate,113)) as TickerSymbol from Master_Commodity 
                WHERE Commodity_ProductSeriesID= ComDailyStat_ProductSeriesID and Commodity_ExchangeSegmentID='" +
                    HttpContext.Current.Session["ExchangeSegmentID"].ToString() + @"')  as instruments , 
                rtrim(ltrim(ComDailyStat_ProductSeriesID))as DailyStat_ProductSeriesID, ltrim(rtrim(ComDailyStat_DateTime)) as DailyStat_DateTime,
                ltrim(rtrim(cast(ComDailyStat_Open as decimal(18,4)))) as DailyStat_Open , ltrim(rtrim(cast(ComDailyStat_High as decimal(18,4)))) 
                as DailyStat_High, ltrim(rtrim(cast(ComDailyStat_Low as decimal(18,4)))) as DailyStat_Low, 
                ltrim(rtrim(cast(ComDailyStat_Close as decimal(18,4)))) as DailyStat_Close, 
                ltrim(rtrim(cast(ComDailyStat_SettlementPrice as decimal(18,4)))) as DailyStat_SettlementPrice , 
                ltrim(rtrim(cast(ComDailyStat_AssetPrice as decimal(18,4)))) as DailyStat_AssetPrice   From  Trans_ComDailyStat  
                where ComDailyStat_DateTime='" + fromDate + "' and  ComDailyStat_ExchangeSegmentID='" +
                    HttpContext.Current.Session["ExchangeSegmentID"].ToString() + @"') as T      
                order by T.instruments,Dailystat_DateTime asc";
                    gridClosing.DataBind();
                    gridClosing.Columns[8].Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript2", "ForFilterOff();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript1", "height();", true);
                    string Spantext = "Closing Rate For Date : ";
                    string SpanText1 = OConvert.ArrangeDate2(cmbForDate.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "ShowHide('" + SpanText1 + "','" + Spantext + "')", true);

                }
                else
                {
                    if (stlist != null)
                    {
                        string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString();
                        string enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString();
                        // gridClosingDataSource.SelectCommand = "select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')+' '+ isnull(convert(varchar(9),Equity_EffectUntil,6),'')+' '+isnull(cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar),'')   from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,(select Equity_FoIdentifier from master_equity where Equity_SeriesID=DailyStat_ProductSeriesID) as instruments,DailyStat_DateTime,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where (CAST(DailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(DailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + "', 101)) and   DailyStat_ProductSeriesID in (" + stlist + ")  and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'";
                        //gridClosingDataSource.SelectCommand = "select * from (select DailyStat_ID,  convert(varchar, Dailystat_DateTime, 106)    as Dailystat_DateTime1, DailyStat_ExchangeSegmentID,(select isnull(Equity_TickerSymbol,'')+' '+isnull(Equity_Series,'')+' '+ isnull(convert(varchar(9),Equity_EffectUntil,6),'')+' '+isnull(cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar),'')   from Master_Equity  WHERE Equity_SeriesID=DailyStat_ProductSeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as instruments,DailyStat_ProductSeriesID,DailyStat_DateTime,cast(DailyStat_Open as decimal(18,2)) as DailyStat_Open ,cast(DailyStat_High as decimal(18,2)) as DailyStat_High,cast(DailyStat_Low as decimal(18,2)) as DailyStat_Low,cast(DailyStat_Close as decimal(18,2))as DailyStat_Close,cast(DailyStat_SettlementPrice as decimal(18,2)) as DailyStat_SettlementPrice ,cast(DailyStat_AssetPrice as decimal(18,2)) as DailyStat_AssetPrice From  trans_dailystatistics where (CAST(DailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(DailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + "', 101)) and   DailyStat_ProductSeriesID in (" + stlist + ")  and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "')as T order by instruments";
                        gridClosingDataSource.SelectCommand = @"select * from ( select ComDailyStat_ID as DailyStat_ID, convert(varchar, ComDailystat_DateTime, 105)  as Dailystat_DateTime1, 
                    ComDailyStat_ExchangeSegmentID, (Select  (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+' '+
                    cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+' '+convert(varchar(11),Commodity_ExpiryDate,113)) as TickerSymbol from Master_Commodity 
                    WHERE Commodity_ProductSeriesID= ComDailyStat_ProductSeriesID and Commodity_ExchangeSegmentID='" +
                        HttpContext.Current.Session["ExchangeSegmentID"].ToString() + @"')  as instruments , rtrim(ltrim(ComDailyStat_ProductSeriesID))
                    as DailyStat_ProductSeriesID, ltrim(rtrim(ComDailyStat_DateTime)) as DailyStat_DateTime, 
                    ltrim(rtrim(cast(ComDailyStat_Open as decimal(18,4)))) as DailyStat_Open , 
                    ltrim(rtrim(cast(ComDailyStat_High as decimal(18,4)))) as DailyStat_High, 
                    ltrim(rtrim(cast(ComDailyStat_Low as decimal(18,4)))) as DailyStat_Low, 
                    ltrim(rtrim(cast(ComDailyStat_Close as decimal(18,4)))) as DailyStat_Close, 
                    ltrim(rtrim(cast(ComDailyStat_SettlementPrice as decimal(18,4)))) as DailyStat_SettlementPrice , 
                    ltrim(rtrim(cast(ComDailyStat_AssetPrice as decimal(18,4)))) as DailyStat_AssetPrice   
                    From  Trans_ComDailyStat  where   (CAST(ComDailyStat_DateTime AS datetime) >= CONVERT(varchar,'" + startdate + @"', 101)) 
                    and (CAST(ComDailyStat_DateTime AS datetime) <= CONVERT(varchar,'" + enddate + @"', 101)) and   ComDailyStat_ProductSeriesID 
                    in (" + stlist + ")   and  ComDailyStat_ExchangeSegmentID='" +
                        HttpContext.Current.Session["ExchangeSegmentID"].ToString() + @"') as T      
                    order by T.instruments,Dailystat_DateTime asc";
                        gridClosing.DataBind();
                        gridClosing.Columns[8].Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript3", "height();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript4", "ForFilterOff();", true);

                        string Spantext = "Closing Rate of Period :";
                        string SpanText1 = OConvert.ArrangeDate2(txtFromDate.Value.ToString()) + "  - " + OConvert.ArrangeDate2(txtToDate.Value.ToString());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "','" + Spantext + "')", true);

                    }
                }

            }
        }
        protected void gridClosing_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
            {
                if (e.Parameters == "s")
                {
                    gridClosing.Settings.ShowFilterRow = true;
                }
                else if (e.Parameters == "All")
                {
                    gridClosing.FilterExpression = string.Empty;
                }
                else if (e.Parameters != "")
                {
                    string tranid = e.Parameters.ToString();
                    oDBEngine.DeleteValue("trans_dailystatistics", "DailyStat_ID ='" + tranid + "'");
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");

                }
            }
            else
            {
                if (e.Parameters == "s")
                {
                    gridClosing.Settings.ShowFilterRow = true;
                }
                else if (e.Parameters == "All")
                {
                    gridClosing.FilterExpression = string.Empty;
                }
                else if (e.Parameters != "")
                {
                    string tranid = e.Parameters.ToString();
                    oDBEngine.DeleteValue("Trans_ComDailyStat", "ComDailyStat_ID ='" + tranid + "'");
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");

                }
            }
            fillGrid();

        }
        protected void gridVarRate_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
            {
                gridVarRate.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters == "All")
            {
                gridVarRate.FilterExpression = string.Empty;
            }
            else if (e.Parameters != "")
            {
                string tranid = e.Parameters.ToString();
                oDBEngine.DeleteValue("trans_dailyvar", "DailyVar_ID ='" + tranid + "'");
                this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");

            }
            // this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
            // FillGridVar();
        }
        protected void CbptxtSelectionID_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            string Values = e.Parameter.Split('~')[0];
            string CombinedQuery = String.Empty;
            if (WhichCall == "BindProduct")
            {
                CombinedQuery = oGenericMethod.GetAssetsOrDerivative("D", Convert.ToInt32(_ExchangeSegmentID), 0, "NA", txtFromDate.Date);
                CombinedQuery = CombinedQuery.Replace("\\'", "'");
                CbptxtSelectionID.JSProperties["cpBindProduct"] = CombinedQuery;

            }
        }
    }
}