using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_ArbStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        Converter oconverter = new Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;

        #region ARB Statement Addtion
        GenericMethod oGenericMethod = null;
        string CombinedQuery = null;

        string GetSegmentID_From_ExchangeAndSegment()
        {
            oGenericMethod = new GenericMethod();
            string SelectedExchanges = null;
            string SelectedSegments = null;
            string UserSegIDs = null;
            DataTable DTArbSegments = null;

            //Find Selected Exchange
            SelectedExchanges = RdoExchange_ALL.Checked ? "ALL" : HiddenField_ArbExchange.Value.Length > 0 ? HiddenField_ArbExchange.Value : null;

            if (SelectedExchanges != null)
            {
                //Find Selected Segments
                if (ChkSegment_CDX.Checked) SelectedSegments = "'CDX'";
                if (ChkSegment_CM.Checked) SelectedSegments = SelectedSegments == null ? "'CM'" : SelectedSegments + ",'CM'";
                if (ChkSegment_FO.Checked) SelectedSegments = SelectedSegments == null ? "'FO'" : SelectedSegments + ",'FO'";
                if (ChkSegment_COMM.Checked) SelectedSegments = SelectedSegments == null ? "'COMM'" : SelectedSegments + ",'COMM'";

                if (SelectedSegments != null)
                {
                    if (SelectedExchanges == "ALL")
                    {
                        DTArbSegments = oGenericMethod.GetExchangeSegment_FullDetail("D", ref CombinedQuery, 1000,
                        @"exch_segmentId in (" + SelectedSegments + @") and 
                    exch_compId='" + Session["LastCompany"].ToString() + @"' and UserSegID in (Select ArbPLSegments_SegmentID from Trans_ArbplSegments 
                    Where ArbPLSegments_CompanyID='" + Session["LastCompany"].ToString() + @"'
                    and ArbPLSegments_ToDate is Null)", "UserSegID", null, null, null, 1);
                    }
                    else
                    {
                        DTArbSegments = oGenericMethod.GetExchangeSegment_FullDetail("D", ref CombinedQuery, 1000, @"exh_shortName 
                    in (" + SelectedExchanges + @") and exch_segmentId in (" + SelectedSegments + @") and 
                    exch_compId='" + Session["LastCompany"].ToString() + @"' and UserSegID in (Select ArbPLSegments_SegmentID from Trans_ArbplSegments 
                    Where ArbPLSegments_CompanyID='" + Session["LastCompany"].ToString() + @"'
                    and ArbPLSegments_ToDate is Null)", "UserSegID", null, null, null, 1);
                    }


                    if (DTArbSegments.Rows.Count > 0)
                    {
                        foreach (DataRow DrSeg in DTArbSegments.Rows)
                        {
                            if (UserSegIDs == null)
                                UserSegIDs = DrSeg[0].ToString();
                            else
                                UserSegIDs = UserSegIDs + "," + DrSeg[0].ToString();
                        }
                    }
                    return UserSegIDs;
                }
                else
                    return "SegSelectionErr";
            }
            else
                return "ExchSelectionErr";
        }
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load('" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "');</script>");
                Date();
                SettlementCycle();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
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

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "ArbGroup")
            {
                data = "ArbGroup~" + str;
            }
            else if (idlist[0] == "ArbExchange")
            {
                data = "ArbExchange~" + str;
            }
        }

        void Date()
        {
            DtFor.EditFormatString = oconverter.GetDateFormat("Date");
            DtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            SettelmentCycleForADate();
        }
        void SettlementCycle()
        {
            DataTable DtCycle = oDBEngine.GetDataTable("Master_ArbCycle", "convert(varchar(11),ArbCycle_StartDate,106)+' To '+convert(varchar(11),ArbCycle_EndDate,106) as TextFeild,cast(ArbCycle_ID as varchar)+'~'+cast(ArbCycle_StartDate as varchar)+'~'+cast(ArbCycle_EndDate as varchar) as ValueFeild", "ArbCycle_FinYear='" + Session["LastFinYear"].ToString().Trim() + "'");
            if (DtCycle.Rows.Count > 0)
            {
                DdlCycle.DataSource = DtCycle;
                DdlCycle.DataTextField = "TextFeild";
                DdlCycle.DataValueField = "ValueFeild";
                DdlCycle.DataBind();
                DtCycle.Dispose();

                string CycleDdl = DdlCycle.SelectedItem.Value.ToString().Trim();
                string[] Cycle = CycleDdl.Split('~');


                DtFrom.Value = Convert.ToDateTime(Cycle[1].ToString().Trim());
                dtFrom1.Value = Convert.ToDateTime(Cycle[1].ToString().Trim());

                DtTo.Value = Convert.ToDateTime(Cycle[2].ToString().Trim());
                dtTo1.Value = Convert.ToDateTime(Cycle[2].ToString().Trim());
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            SettelmentCycleForADate();
        }
        void SettelmentCycleForADate()
        {
            DataTable DtCycle = oDBEngine.GetDataTable("Master_ArbCycle", "'Cycle:' +convert(varchar(11),ArbCycle_StartDate,106)+' To '+convert(varchar(11),ArbCycle_EndDate,106) as TextFeild,ArbCycle_ID as ValueFeild", "ArbCycle_FinYear='" + Session["LastFinYear"].ToString().Trim() + "' and '" + DtFor.Value.ToString().Trim() + "' Between ArbCycle_StartDate and ArbCycle_EndDate");
            if (DtCycle.Rows.Count > 0)
            {
                DivDisplay.InnerHtml = DtCycle.Rows[0]["TextFeild"].ToString().Trim();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnValueInsert", "FnValueInsert('" + DtCycle.Rows[0]["ValueFeild"].ToString().Trim() + "');", true);
            }
        }
        void ProcedureCall1()
        {
            string GetSegment = GetSegmentID_From_ExchangeAndSegment();
            if (!GetSegment.Contains("SegSelectionErr") && !GetSegment.Contains("ExchSelectionErr"))
            {
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "[Report_ArbPLSettlement]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@Segments", GetSegment);
                    cmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());
                    if (rdArbGroupAll.Checked)
                    {
                        cmd.Parameters.AddWithValue("@ArbGroup", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ArbGroup", HiddenField_ArbGroup.Value.ToString().Trim());
                    }
                    if (rdbClientALL.Checked)
                    {
                        cmd.Parameters.AddWithValue("@Client", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Client", HiddenField_Client.Value.ToString().Trim());
                    }
                    if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        cmd.Parameters.AddWithValue("@Cycleid", HiddenField_CycleFordate.Value.ToString().Trim());
                        cmd.Parameters.AddWithValue("@FromDate", DtFor.Value.ToString().Trim());
                        cmd.Parameters.AddWithValue("@ToDate", DtFor.Value.ToString().Trim());
                    }
                    else
                    {
                        string CycleDdl = DdlCycle.SelectedItem.Value.ToString().Trim();
                        //string[] Cycle = CycleDdl.Split('~');
                        string[,] Cycle = oDBEngine.GetFieldValue("Master_ArbCycle", "top 1 ArbCycle_ID", "ArbCycle_StartDate <= '" + DtFrom.Value.ToString().Trim() + "' and ArbCycle_EndDate>='" + DtTo.Value.ToString().Trim() + "' and ArbCycle_FinYear='" + Session["LastFinYear"].ToString().Trim() + "'", 1);
                        cmd.Parameters.AddWithValue("@Cycleid", Cycle[0, 0].ToString().Trim());
                        cmd.Parameters.AddWithValue("@FromDate", DtFrom.Value.ToString().Trim());
                        cmd.Parameters.AddWithValue("@ToDate", DtTo.Value.ToString().Trim());
                    }
                    cmd.Parameters.AddWithValue("@RptType", DdlRptType.SelectedItem.Value.ToString().Trim());

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    ds.Reset();
                    da.Fill(ds);
                    da.Dispose();
                    ////ViewState["dataset"] = ds;
                    ////FnGenerateType(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Export(ds);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert();", true);
                    }
                }
            }
            else
            {
                if (GetSegment.Contains("SegSelectionErr"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnSegmentErr", "FnSegmentSelectionErr();", true);
                else if (GetSegment.Contains("ExchSelectionErr"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnExchangeErr", "FnExchangeSelectionErr();", true);
            }
        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();

            string str = null;
            str = "Report Type : " + DdlRptType.SelectedItem.Text.ToString().Trim();

            if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")
            {
                str = str + " ; For :" + oconverter.ArrangeDate2(DtFor.Value.ToString()) + " ; " + DivDisplay.InnerHtml.ToString().Trim();
            }
            else
            {
                str = str + " ; Period :" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString()) + " ; Cycle:" + DdlCycle.SelectedItem.Text.ToString().Trim();
            }


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            dtReportHeader.Columns.Add(new DataColumn("Header2", typeof(String))); //1
            dtReportHeader.Columns.Add(new DataColumn("Header3", typeof(String))); //2


            //Find Selected Segments
            string SelectedSegments = null;
            if (ChkSegment_CDX.Checked) SelectedSegments = "'CDX'";
            if (ChkSegment_CM.Checked) SelectedSegments = SelectedSegments == null ? "'CM'" : SelectedSegments + ",'CM'";
            if (ChkSegment_FO.Checked) SelectedSegments = SelectedSegments == null ? "'FO'" : SelectedSegments + ",'FO'";
            if (ChkSegment_COMM.Checked) SelectedSegments = SelectedSegments == null ? "'COMM'" : SelectedSegments + ",'COMM'";

            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow HeaderRowExchange = dtReportHeader.NewRow();
            HeaderRowExchange[0] = (RdoExchange_ALL.Checked ? "ALL" : HiddenField_ArbExchange.Value);
            HeaderRowExchange[0] = "Exchange(s) : " + HeaderRowExchange[0];
            dtReportHeader.Rows.Add(HeaderRowExchange);
            DataRow HeaderRowSegment = dtReportHeader.NewRow();
            HeaderRowSegment[0] = "Segment(s) : " + SelectedSegments;
            dtReportHeader.Rows.Add(HeaderRowSegment);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Daily Arbitrage Statement", "Arb Group:", dtReportHeader, dtReportFooter);

        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ProcedureCall1();
            if (ViewState["dataset"] != null)
            {
                ds = (DataSet)ViewState["dataset"];
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Export(ds);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert();", true);
                    }
                }
            }
        }
        protected void btnemail_Click(object sender, EventArgs e)
        {
            ProcedureCall();
            ds = (DataSet)ViewState["dataset"];

            if (ds.Tables[0].Rows.Count > 0)
            {
                //mail();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void EmailClientWise(DataSet ds, string Date)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[1].DefaultView;
            viewData.RowFilter = " ClientName<>'ZZZZZZZ' and Clientid is not null";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable Distinctclient = new DataTable();
            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "Clientid", "ClientName" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbrecord.DataSource = Distinctclient;
                cmbrecord.DataValueField = "Clientid";
                cmbrecord.DataTextField = "ClientName";
                cmbrecord.DataBind();

            }
            ViewState["mailsendresult"] = "mail";
            /////////For Client Email
            for (int k = 0; k < cmbrecord.Items.Count; k++)
            {
                FnHtml(ds, cmbrecord.Items[k].Value.ToString().Trim());

                if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbrecord.Items[k].Value.ToString().Trim(), Date.ToString().Trim(), "Daily Arbitrage Statement [" + Date.ToString().Trim() + "]") == true)
                {
                    if (ViewState["mailsendresult"].ToString().Trim() == "Error")
                    {
                        ViewState["mailsendresult"] = "SomeClientError";
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "Success";
                    }
                }
                else
                {

                    ViewState["mailsendresult"] = "Error";
                }
            }

            if (ViewState["mailsendresult"].ToString().Trim() == "Error")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('4');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('5');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('6');", true);
            }
        }
        void FnGenerateType(DataSet ds)
        {
            Email(ds);
        }
        void Email(DataSet ds)
        {
            string Date = oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            if (DdlRptType.SelectedItem.Value == "2")
            {
                EmailClientWise(ds, Date);
            }
            else
            {
                EmailClientWise1(ds, Date);
            }
        }

        void FnHtml(DataSet ds, string parameter)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;

            //if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
            //{
            //str = ddlGroupBy.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtFrom1.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo1.Value.ToString());


            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[1].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            //}

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";




            DataView viewclient = new DataView();
            DataView viewclient2 = new DataView();
            viewclient2 = ds.Tables[0].DefaultView;

            viewclient = ds.Tables[1].DefaultView;
            viewclient.RowFilter = "(Clientid='" + parameter.ToString().Trim() + "' and Clientid is not null and Clientid<>'ZZZZZZZ')";
            viewclient2.RowFilter = "(Clientid='" + parameter.ToString().Trim() + "')";
            DataTable dt1 = new DataTable();
            dt1 = viewclient2.ToTable();
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt1.Columns.Remove("Clientid");
            dt1.Columns.Remove("Name");
            dt.Columns.Remove("Clientid");
            dt.Columns.Remove("ClientName");
            dt.Columns.Remove("GroupCode");
            //dt.Columns.Remove("GrpEmail");



            //if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "Screen")
            //{
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";

            //}
            dt.Rows[0].Delete();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Total:"))
                        {
                            strHtml += "</tr>";
                            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                // if strhtml += "<td>&nbsp;</td>"

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            ViewState["mail"] = strHtmlheader + strHtml;
            /////////////////////////////////////////////////////////////////////////////////
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr2 in dt1.Rows)
                {
                    //  strHtmlheader = null;
                    string str1 = null;
                    str1 = "Arbitrage Sheet Summary as on  ";


                    str1 = str1 + oconverter.ArrangeDate2(DtTo.Value.ToString()) + " ; Cycle:" + DdlCycle.SelectedItem.Text.ToString().Trim();

                    strHtml = null;
                    // strHtml = null;
                    // strHtmlheader = null;
                    //strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    //strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";
                    strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + str1 + "</b></td>";
                    strHtml += "</tr>";
                    //dt1.Rows[0].Delete();
                    //}


                    //////////////TABLE HEADER BIND

                    strHtml += "<table><tr style=\"background-color: #DBEEF3;\">";
                    for (int i = 0; i < dt1.Columns.Count; i++)
                    {
                        strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt1.Columns[i].ColumnName + "</b></td>";
                    }
                    strHtml += "</tr>";

                    flag = flag;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    for (int M = 0; M < dt1.Columns.Count; M++)
                    {
                        if (dr2[M] != DBNull.Value)
                        {
                            if (dr2[M].ToString().Trim().StartsWith("Total:"))
                            {
                                strHtml += "</tr>";
                                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt1.Columns[M].ColumnName + "\"><b>" + dr2[M] + "</b></td>";
                            }
                            else if (dr2[M].ToString().Trim().StartsWith("Test"))
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                            else
                            {
                                if (IsNumeric(dr2[M].ToString()) == true)
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt1.Columns[M].ColumnName + "\">" + dr2[M] + "</td>";

                                }
                                else
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[M].ColumnName + "\">" + dr2[M] + "</td>";

                                }
                            }
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                    }
                    // if strhtml += "<td>&nbsp;</td>"

                    strHtml += "</tr>";
                }




                /////////////////////////////////////////////////////////////////////////////////
                strHtml += "</table>";
                if (Request.QueryString["Custid"] != null)//////////PopUp Display From Delivery Center
                {
                    Div2.InnerHtml = strHtml;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay1", "RecordDisplay('8');", true);

                }
                else
                {
                    //if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
                    //{
                    //    DivHeader.InnerHtml = strHtmlheader;
                    //    Divdisplay.InnerHtml = strHtml;
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('3');", true);

                    //}
                    //else
                    //{
                    ViewState["mail"] += strHtml;
                    //}
                }
            }




        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        protected void cmbrecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            FnHtml(ds, cmbrecord.SelectedItem.Value.ToString().Trim());
        }
        void ProcedureCall()
        {

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[Report_ArbPLSettlementforemail]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString().Trim());
                cmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());
                if (rdArbGroupAll.Checked)
                {
                    cmd.Parameters.AddWithValue("@ArbGroup", "ALL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ArbGroup", HiddenField_ArbGroup.Value.ToString().Trim());
                }
                if (rdbClientALL.Checked)
                {
                    cmd.Parameters.AddWithValue("@Client", "ALL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Client", HiddenField_Client.Value.ToString().Trim());
                }
                if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")
                {
                    cmd.Parameters.AddWithValue("@Cycleid", HiddenField_CycleFordate.Value.ToString().Trim());
                    cmd.Parameters.AddWithValue("@FromDate", DtFor.Value.ToString().Trim());
                    cmd.Parameters.AddWithValue("@ToDate", DtFor.Value.ToString().Trim());
                }
                else
                {
                    string CycleDdl = DdlCycle.SelectedItem.Value.ToString().Trim();
                    //string[] Cycle = CycleDdl.Split('~');
                    string[,] Cycle = oDBEngine.GetFieldValue("Master_ArbCycle", "top 1 ArbCycle_ID", "ArbCycle_StartDate <= '" + DtFrom.Value.ToString().Trim() + "' and ArbCycle_EndDate>='" + DtTo.Value.ToString().Trim() + "' and ArbCycle_FinYear='" + Session["LastFinYear"].ToString().Trim() + "'", 1);
                    cmd.Parameters.AddWithValue("@Cycleid", Cycle[0, 0].ToString().Trim());
                    //cmd.Parameters.AddWithValue("@Cycleid", "11");
                    cmd.Parameters.AddWithValue("@FromDate", DtFrom.Value.ToString().Trim());
                    cmd.Parameters.AddWithValue("@ToDate", DtTo.Value.ToString().Trim());
                }
                if (DdlRptType.SelectedItem.Value == "2")
                {
                    // cmd.Parameters.AddWithValue("@RptType", DdlRptType.SelectedItem.Value.ToString().Trim());
                    cmd.Parameters.AddWithValue("@RptType", "2");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RptType", "4");
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();
                ViewState["dataset"] = ds;
                FnGenerateType(ds);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    Export(ds);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert();", true);
                //}


            }
        }

        void EmailClientWise1(DataSet ds, string Date)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " ClientName<>'ZZZZZZZ' and Clientid is not null";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable Distinctclient = new DataTable();
            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "Clientid", "ClientName" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbrecord.DataSource = Distinctclient;
                cmbrecord.DataValueField = "Clientid";
                cmbrecord.DataTextField = "ClientName";
                cmbrecord.DataBind();

            }
            ViewState["mailsendresult"] = "mail";
            /////////For Client Email
            for (int k = 0; k < cmbrecord.Items.Count; k++)
            {
                FnHtml1(ds, cmbrecord.Items[k].Value.ToString().Trim());

                if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbrecord.Items[k].Value.ToString().Trim(), Date.ToString().Trim(), "Daily Arbitrage Statement [" + Date.ToString().Trim() + "] [ Only Open Position ]") == true)
                {
                    if (ViewState["mailsendresult"].ToString().Trim() == "Error")
                    {
                        ViewState["mailsendresult"] = "SomeClientError";
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "Success";
                    }
                }
                else
                {

                    ViewState["mailsendresult"] = "Error";
                }
            }

            if (ViewState["mailsendresult"].ToString().Trim() == "Error")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('4');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('5');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('6');", true);
            }
        }
        void FnHtml1(DataSet ds, string parameter)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;

            //if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
            //{
            //str = ddlGroupBy.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtFrom1.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo1.Value.ToString());


            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            //}

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";




            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "(Clientid='" + parameter.ToString().Trim() + "' and Clientid is not null and Clientid<>'ZZZZZZZ')";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt.Columns.Remove("Clientid");
            dt.Columns.Remove("ClientName");
            dt.Columns.Remove("GroupCode");
            //dt.Columns.Remove("GrpEmail");



            //if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "Screen")
            //{
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";

            //}
            dt.Rows[0].Delete();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Total:"))
                        {
                            strHtml += "</tr>";
                            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                // if strhtml += "<td>&nbsp;</td>"

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            if (Request.QueryString["Custid"] != null)//////////PopUp Display From Delivery Center
            {
                Div2.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay1", "RecordDisplay('8');", true);

            }
            else
            {
                //if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
                //{
                //    DivHeader.InnerHtml = strHtmlheader;
                //    Divdisplay.InnerHtml = strHtml;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('3');", true);

                //}
                //else
                //{
                ViewState["mail"] = strHtmlheader + strHtml;
                //}
            }




        }
    }
}
