using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_TranscationReport : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {

        #region GlobalVariable
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        string data;
        ExcelFile objExcel = new ExcelFile();
        String strHtml = String.Empty;
        String strHtmlheader = String.Empty;
        DataTable Distinctclient = new DataTable();
        DataTable dt = new DataTable();
        DataView viewData = new DataView();
        AspxHelper oAx = new AspxHelper();
        #endregion

        #region Page Property
        int PageNumber; int PageSize; string DateFrom; string DateTo; string strheading; string ReportType;
        public int P_PageSize
        {
            get { return PageSize; }
            set { PageSize = value; }
        }
        public int P_PageNumber
        {
            get { return PageNumber; }
            set { PageNumber = value; }
        }
        public string P_DateFrom
        {
            get { return DateFrom; }
            set { DateFrom = value; }
        }
        public string P_DateTo
        {
            get { return DateTo; }
            set { DateTo = value; }
        }
        public string P_strheading
        {
            get { return strheading; }
            set { strheading = value; }
        }
        public string P_ReportType
        {
            get { return ReportType; }
            set { ReportType = value; }
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
               // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();
                SettlementBind();
                SegmentName();
            }
            if (ddlGeneration.SelectedItem.Text == "EMail")
                ReportType = "EMail";
            else if (ddlGeneration.SelectedItem.Text == "Export")
                ReportType = "Export";
            else if (ddlGeneration.SelectedItem.Text == "Screen")
                ReportType = "Screen";

            if (hdn_GridBindOrNotBind.Value != "False")
            {
                PageSize = Hdn_PageSize.Value != "" ? Convert.ToInt32(Hdn_PageSize.Value) : 0;
                PageNumber = Hdn_PageNumber.Value != "" ? Convert.ToInt32(Hdn_PageNumber.Value) : 0;
                DateFrom = Hdn_DateFrom.Value;
                DateTo = Hdn_DateTo.Value;

                if (PageSize != 0 && PageNumber != 0)
                {
                    ds = Procedure();
                    oAx.BindGrid(GvTransactionDtl, ds);
                }
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }

        void Date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");

            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
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
                if (idlist[0].ToString().Trim() == "Clients" || idlist[0].ToString().Trim() == "ISIN" || idlist[0].ToString().Trim() == "TranType")
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
            data = idlist[0] + "~" + str;


        }

        public void SettlementBind()
        {
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("Master_Settlements ", "RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)+'  ' + REPLACE(CONVERT(VARCHAR(9), settlements_StartDateTime, 6), ' ', '-') AS [DD-Mon-YY],RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)", "settlements_exchangesegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() + "' and  Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and RTRIM(settlements_Number)='" + Session["LastSettNo"].ToString().Substring(0, 7).ToString().Trim() + "' and RTRIM(settlements_TypeSuffix)='" + Session["LastSettNo"].ToString().Substring(7, 1).ToString().Trim() + "'");
            if (DT.Rows.Count > 0)
            {
                txtSettlements.Text = DT.Rows[0][0].ToString().Trim();
                txtSettlements_hidden.Text = DT.Rows[0][1].ToString().Trim();
            }

        }

        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                DdlGrpType.DataSource = DtGroup;
                DdlGrpType.TextField = "gpm_Type";
                DdlGrpType.ValueField = "gpm_Type";
                DdlGrpType.DataBind();
                DtGroup.Dispose();

            }

        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (DdlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                BindGroup();
            }
        }

        void SegmentName()
        {
            HiddenField_Segment.Value = Session["usersegid"].ToString();

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";

        }


        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            ReportType = "Export";
            SPCall();
        }
        protected void BtnEmail_Click(object sender, EventArgs e)
        {
            ReportType = "EMail";
            SPCall();
        }

        DataSet Procedure()
        {
            string[] InputName = new string[22];
            string[] InputType = new string[22];
            string[] InputValue = new string[22];


            /////////////////Parameter Name
            InputName[0] = "Companyid";
            InputName[1] = "FinYear";
            InputName[2] = "FromDate";
            InputName[3] = "ToDate";
            InputName[4] = "Segmentid";
            InputName[5] = "DPAccount";
            InputName[6] = "SettlementsFor";
            InputName[7] = "Settlements";
            InputName[8] = "For";
            InputName[9] = "GrpType";
            InputName[10] = "GrpId";
            InputName[11] = "BranchHierchy";
            InputName[12] = "SearchScripBy";
            InputName[13] = "SearchScrip";
            InputName[14] = "ReportStyle";
            InputName[15] = "ExcelAllColumn";
            InputName[16] = "emailoption";
            InputName[17] = "IsOnlyTransaction";
            InputName[18] = "SelectedTranType";
            InputName[19] = "ReportType";
            InputName[20] = "PageSize";
            InputName[21] = "PageNumber";



            /////////////////Parameter Data Type
            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "V";
            InputType[13] = "V";
            InputType[14] = "V";
            InputType[15] = "C";
            InputType[16] = "V";
            InputType[17] = "C";
            InputType[18] = "V";
            InputType[19] = "V";
            InputType[20] = "I";
            InputType[21] = "I";

            /////////////////Parameter Value
            InputValue[0] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
            InputValue[1] = HttpContext.Current.Session["LastFinYear"].ToString().Trim();
            InputValue[2] = DtFrom.Value.ToString().Trim();
            InputValue[3] = DtTo.Value.ToString().Trim();
            if (rdbSegmentAll.Checked)
                InputValue[4] = "ALL";
            else
                InputValue[4] = HiddenField_Segment.Value.ToString().Trim();
            if (RdbDpAccountAll.Checked)
                InputValue[5] = "ALL";
            else
                InputValue[5] = HiddenField_DpAccount.Value.ToString().Trim();
            InputValue[6] = DdlSettlements.SelectedItem.Value.ToString().Trim();
            InputValue[7] = txtSettlements_hidden.Text.ToString().Trim();

            if (ChkForClients.Checked == true && ChkForExchange.Checked == true)
                InputValue[8] = "Both";
            if (ChkForClients.Checked == true && ChkForExchange.Checked == false)
                InputValue[8] = "Client";
            if (ChkForClients.Checked == false && ChkForExchange.Checked == true)
                InputValue[8] = "Exchange";

            if (DdlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
                InputValue[9] = DdlGrpType.SelectedItem.Value.ToString().Trim();
            else
                InputValue[9] = DdlGroupBy.SelectedItem.Value.ToString().Trim();

            if (rdAll.Checked)
                InputValue[10] = "ALL";
            else
                InputValue[10] = HiddenField_ALL.Value.ToString().Trim();

            InputValue[11] = HttpContext.Current.Session["userbranchHierarchy"].ToString().Trim();
            InputValue[12] = DdlSearchScripBy.SelectedItem.Value.ToString().Trim();

            if (RdSearchScripByAll.Checked)
                InputValue[13] = "ALL";
            else
                InputValue[13] = HiddenField_SearchScrip.Value.ToString().Trim();
            InputValue[14] = DdlReportStyle.SelectedItem.Value.ToString().Trim();

            InputValue[15] = ChkShowAllColumn.Checked ? "T" : "F";
            if (ddlGeneration.SelectedItem.Text == "EMail")
                InputValue[16] = ddloptionformail.SelectedItem.Text.ToString().Trim();
            else
                InputValue[16] = "NOT APP";

            InputValue[17] = DdlReportStyle.Value.ToString().Trim() == "C" ? ChkOnlyTransaction.Checked ? "T" : "F" : "T";
            InputValue[18] = ddlTrantype.Value.ToString().Trim() == "S" ? HiddenField_TranType.Value : "NA";
            InputValue[19] = ReportType;
            //if (ddlGeneration.SelectedItem.Value == "EMail")
            //    InputValue[19] = "EMail";
            //else if(ddlGeneration.SelectedItem.Value == "Export")
            //    InputValue[19] = "Export";
            //else if(ddlGeneration.SelectedItem.Value == "Screen")
            //    InputValue[19] = "Screen";
            InputValue[20] = PageSize.ToString();
            InputValue[21] = PageNumber.ToString();
            ////Please Do Notice It. This is Very Important thing..........
            //DebugHelper oDebugHelper = new DebugHelper();
            //oDebugHelper.GetPrepareSpToRun("Report_DematTransaction", InputType, InputName, InputValue, "24", "1");


            ds = SQLProcedures.SelectProcedureArrDS("[Report_DematTransaction]", InputName, InputType, InputValue);

            return ds;
        }

        string ScreenHeader(int Count)
        {
            ////////////////Heading Bind//////////////////////
            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + Count + " style=\"color:Blue;\">" + strheading + "</td></tr>";
            strHtmlheader += "</table>";
            DivHeaderr.InnerHtml = strHtmlheader;
            GvTransactionDtl.JSProperties["cpShowHideFilter"] = strHtmlheader;
            return strHtmlheader.ToString().Trim();
        }

        void SPCall()
        {
            ds = Procedure();
            if (ds.Tables.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('No Record Found !!','1');", true);
                return;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {

                strheading = "Report DateTime : " + oDBEngine.GetDate(113).ToString() + " ; ";
                strheading = strheading.ToString().Trim() + "Report Style : " + DdlReportStyle.SelectedItem.Text.ToString().Trim();

                if (DdlReportStyle.SelectedItem.Value.ToString().Trim() != "D")
                    strheading = strheading.ToString().Trim() + " ; For The Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + "-" + oconverter.ArrangeDate2(DtTo.Value.ToString());
                else
                    strheading = strheading.ToString().Trim() + " ;Settlements: " + txtSettlements.Text.ToString().Trim();

                /////////////////////Heading End/////////////////////

                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
                {
                    ViewState["DataSet"] = ds.Tables[0];
                    ViewState["strheading"] = strheading.ToString().Trim();
                    ScreenHeader(ds.Tables[0].Columns.Count);
                    //FnHtml(ds.Tables[0], strheading.ToString().Trim());

                }
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Export")
                    ExportToExcel_Generic(ds.Tables[0], ChkShowAllColumn.Checked, "2007");
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "EMail")
                {
                    if (ddloptionformail.SelectedItem.Text == "User")
                    {
                        FnUserEmail(ds.Tables[0], strheading.ToString().Trim());
                    }
                    else
                    {
                        email(ds);
                    }
                }
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('No Record Found !!','1');", true);
        }
        void email(DataSet ds)
        {
            string Date = oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " Clientid<>'ZZZ'";
            DataTable dtclient = new DataTable();
            dtclient = viewData.ToTable();

            Distinctclient = new DataTable();
            DataView viewClient = new DataView(dtclient);
            Distinctclient = viewClient.ToTable(true, new string[] { "clientid" });
            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "clientid";
                cmbclient.DataTextField = "clientid";
                cmbclient.DataBind();

            }
            for (int i = 0; i < cmbclient.Items.Count; i++)
            {
                DataView viewsingleclient = new DataView();
                viewsingleclient = ds.Tables[0].DefaultView;
                viewsingleclient.RowFilter = "(clientid='" + cmbclient.Items[i].Value + "')";

                dt = viewsingleclient.ToTable();
                dt.Columns.Remove("clientid");
                String strHtml = String.Empty;
                String strHtmlheader = String.Empty;
                string strFooter = string.Empty;
                // strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int cname = 0; cname < dt.Columns.Count; cname++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[cname].ColumnName + "</b></td>";
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
                            if (dr1[j].ToString().Trim().StartsWith("Test"))
                                strHtml += "<td>&nbsp;</td>";
                            else if (dr1[j].ToString().Trim().StartsWith("**"))
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                            else if (IsNumeric(dr1[j].ToString()) == true)
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                            else
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                        }
                        else
                            strHtml += "<td>&nbsp;</td>";
                    }

                }
                strHtml += "</tr>";

                strHtml = strHtmlheader + strHtml + strFooter + "</table>";
                // strHtml += "</table>";


                if (oDBEngine.SendReportSt(strHtml, cmbclient.Items[i].Value, oDBEngine.GetDate().ToString(), "Demat Transaction [" + Date.ToString().Trim() + "]") == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);

                }
            }


        }

        string FnHtml(DataTable Dt, string strheading)
        {

            ////////////////Heading Bind//////////////////////
            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + Dt.Columns.Count + " style=\"color:Blue;\">" + strheading.ToString().Trim() + "</td></tr>";
            strHtmlheader += "</tr></table>";

            ////////Detail Display
            //String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + Dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in Dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Test"))
                            strHtml += "<td>&nbsp;</td>";
                        else if (dr1[j].ToString().Trim().StartsWith("**"))
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        else if (IsNumeric(dr1[j].ToString()) == true)
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                    }
                    else
                        strHtml += "<td>&nbsp;</td>";
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            //if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
            //{
            //    DivHeader.InnerHtml = strHtmlheader;
            //    Divdisplay.InnerHtml = strHtml;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('DisPlay'," + ds.Tables[0].Columns.Count + ");", true);
            //}
            return strHtmlheader.ToString().Trim() + strHtml.ToString().Trim();
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
        //void Export(DataTable dtExport, string strheading)
        //{

        //    for (int i = 0; i < dtExport.Rows.Count; i++)
        //    {
        //        if (dtExport.Rows[i]["Description"].ToString().Trim().Contains("</BR>"))
        //        {
        //            string strDescriptionTemp = dtExport.Rows[i]["Description"].ToString().Trim();
        //            strDescriptionTemp = strDescriptionTemp.Replace("</BR>", " ");
        //            dtExport.Rows[i]["Description"] = strDescriptionTemp;
        //        }
        //        if (dtExport.Rows[i]["Remarks"].ToString().Trim().Contains("</BR>"))
        //        {
        //            string strRemarksTemp = dtExport.Rows[i]["Remarks"].ToString().Trim();
        //            strRemarksTemp = strRemarksTemp.Replace("</BR>", " ");
        //            dtExport.Rows[i]["Remarks"] = strRemarksTemp;
        //        }
        //    }
        //    dtExport.AcceptChanges();
        //    DataTable dtReportHeader = new DataTable();
        //    DataTable dtReportFooter = new DataTable();

        //    DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
        //    dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
        //    DataRow HeaderRow = dtReportHeader.NewRow();
        //    HeaderRow[0] = strheading.ToString().Trim();
        //    dtReportHeader.Rows.Add(HeaderRow);


        //    DataRow HeaderRow1 = dtReportHeader.NewRow();
        //    dtReportHeader.Rows.Add(HeaderRow1);
        //    DataRow HeaderRow2 = dtReportHeader.NewRow();
        //    dtReportHeader.Rows.Add(HeaderRow2);

        //    dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
        //    DataRow FooterRow1 = dtReportFooter.NewRow();
        //    dtReportFooter.Rows.Add(FooterRow1);
        //    DataRow FooterRow2 = dtReportFooter.NewRow();
        //    dtReportFooter.Rows.Add(FooterRow2);
        //    DataRow FooterRow = dtReportFooter.NewRow();
        //    FooterRow[0] = "* * *  End Of Report * * *   ";
        //    dtReportFooter.Rows.Add(FooterRow);

        //    objExcel.ExportToExcelforExcel(dtExport, "Transaction Report", "Total:", dtReportHeader, dtReportFooter);

        //}


        void FnUserEmail(DataTable DtEmail, string strheading)
        {
            if (HiddenField_Email.Value.ToString().Trim() == "")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('E-Mail Id Could Not Be Found !!','1');", true);
            else
            {
                string MailOutPut = "Error on sending!Try again.. !!";
                string Html = FnHtml(DtEmail, strheading.ToString().Trim());
                string[] clnt = HiddenField_Email.Value.ToString().Split(',');
                int kk = clnt.Length;

                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(Html.ToString().Trim(), clnt[i].ToString().Trim(), oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString()), "Delivery Position") == true)
                        MailOutPut = "Mail Sent Successfully !!";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('" + MailOutPut.ToString().Trim() + "','1');", true);
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportType = "Export";
            SPCall();
            //Export(ds.Tables[0],strheading);
            ExportToExcel_Generic(ds.Tables[0], ChkShowAllColumn.Checked, "2007");

        }
        void ExportToExcel_Generic(DataTable Dt, Boolean IsShowAllColumn, string ExcelVersion)
        {
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                if (Dt.Rows[i]["Description"].ToString().Trim().Contains("</BR>"))
                {
                    string strDescriptionTemp = Dt.Rows[i]["Description"].ToString().Trim();
                    strDescriptionTemp = strDescriptionTemp.Replace("</BR>", " ");
                    Dt.Rows[i]["Description"] = strDescriptionTemp;
                }
                if (Dt.Rows[i]["Remarks"].ToString().Trim().Contains("</BR>"))
                {
                    string strRemarksTemp = Dt.Rows[i]["Remarks"].ToString().Trim();
                    strRemarksTemp = strRemarksTemp.Replace("</BR>", " ");
                    Dt.Rows[i]["Remarks"] = strRemarksTemp;
                }
            }
            Dt.AcceptChanges();
            //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            //string[] MYHeader = { "Header" };
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
            string[] MYHeader = new string[3];
            MYHeader[0] = strheading;
            MYHeader[1] = CompanyName.Rows[0][0].ToString() + " - " + HttpContext.Current.Session["Segmentname"].ToString();
            MYHeader[2] = "Demat Transaction Report";
            string FileName = "ReportingFile_" + exlTime;
            strDownloadFileName = "~/Documents/";
            if (DdlReportStyle.Value.ToString().Trim() == "A")
            {
                if (IsShowAllColumn)
                {
                    string[] ColumnType = { "V", "V", "V", "I", "V", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "150", "100", "15", "4", "30", "15", "250", "10", "70", "250", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "25", "15", "15", "15", "15", "30", "10", "25", "30", "30", "10", "10", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
                else
                {
                    string[] ColumnType = { "V", "I", "V", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "150", "4", "30", "15", "250", "10", "70", "250", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "15", "20", "15", "40", "10", "25", "30", "30", "10", "10", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
            }
            if (DdlReportStyle.Value.ToString().Trim() == "B")
            {
                if (IsShowAllColumn)
                {
                    string[] ColumnType = { "V", "V", "V", "V", "I", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "150", "10", "50", "20", "4", "15", "250", "250", "40", "250", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "15", "20", "15", "12", "25", "15", "25", "25", "20", "30", "10", "10", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
                else
                {
                    string[] ColumnType = { "V", "I", "V", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "150", "4", "30", "15", "250", "250", "70", "250", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "15", "20", "15", "40", "40", "25", "30", "30", "10", "10", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
            }
            if (DdlReportStyle.Value.ToString().Trim() == "C")
            {
                if (IsShowAllColumn)
                {
                    string[] ColumnType = { "V", "V", "V", "V", "I", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "150", "10", "250", "20", "4", "100", "50", "250", "250", "100", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "10", "40", "10", "10", "10", "13", "40", "40", "15", "40", "7", "7", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
                else
                {
                    string[] ColumnType = { "V", "I", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "250", "10", "30", "20", "250", "250", "30", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "10", "10", "15", "25", "25", "15", "13", "7", "7", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
            }
            else if (DdlReportStyle.Value.ToString().Trim() == "D")
            {
                if (IsShowAllColumn)
                {
                    string[] ColumnType = { "V", "V", "V", "V", "I", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "150", "10", "250", "22", "4", "12", "50", "250", "250", "100", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "10", "40", "15", "10", "12", "15", "50", "50", "18", "50", "10", "10", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
                else
                {
                    string[] ColumnType = { "V", "I", "V", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "250", "10", "30", "20", "250", "250", "30", "30", "250", "18,4", "18,4", "18,4" };
                    string[] ColumnWidthSize = { "25", "10", "10", "15", "25", "25", "15", "15", "25", "7", "7", "12" };
                    //string FileName = "ReportingFile_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                    //strDownloadFileName = "~/Documents/";
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, MYHeader, null);
                }
            }
        }



        protected void GvTransactionDtl_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GvTransactionDtl.JSProperties["cpRefreshNavPanel"] = null;
            GvTransactionDtl.JSProperties["cpSetGlobalFields"] = null;
            GvTransactionDtl.JSProperties["cpShowHideFilter"] = null;
            GvTransactionDtl.JSProperties["cpNoRecordFound"] = null;
            string[] strSplit = e.Parameters.Split('~');
            string WhichCall = strSplit[0];
            int TotalItems = 0;
            int TotalPage = 0;
            PageSize = 20;
            ReportType = "Screen";
            if (WhichCall == "ShowRecords")
            {
                string Selection = strSplit[1];

                GvTransactionDtl.Columns[0].Visible = true;
                GvTransactionDtl.Columns[1].Visible = true;
                GvTransactionDtl.Columns[2].Visible = true;
                GvTransactionDtl.Columns[3].Visible = true;
                GvTransactionDtl.Columns[4].Visible = true;
                GvTransactionDtl.Columns[5].Visible = true;
                GvTransactionDtl.Columns[6].Visible = true;
                GvTransactionDtl.Columns[7].Visible = true;
                GvTransactionDtl.Columns[8].Visible = true;
                GvTransactionDtl.Columns[9].Visible = true;
                GvTransactionDtl.Columns[10].Visible = true;
                GvTransactionDtl.Columns[11].Visible = true;
                GvTransactionDtl.Columns[12].Visible = true;

                if (Selection == "A")
                {
                    GvTransactionDtl.Columns[6].Visible = false; //DP Account/Sett
                }
                else
                {
                    GvTransactionDtl.Columns[5].Visible = false;//Sett No.
                }
                if (Selection == "C")
                {
                    GvTransactionDtl.Columns[7].Visible = false;//Client/Code
                }
                PageNumber = 1;
                SPCall();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        oAx.BindGrid(GvTransactionDtl, ds);

                    }
                    else
                    {
                        oAx.BindGrid(GvTransactionDtl);
                        GvTransactionDtl.JSProperties["cpNoRecordFound"] = "T";
                    }
                }
                else
                {
                    oAx.BindGrid(GvTransactionDtl);
                    GvTransactionDtl.JSProperties["cpNoRecordFound"] = "T";
                }
                GvTransactionDtl.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                GvTransactionDtl.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' + DateFrom + '~' + DateTo;
            }
            if (WhichCall == "ShowRecordsFilter")
            {
                PageNumber = 1;

                if (strSplit[1] == "s")
                    GvTransactionDtl.Settings.ShowFilterRow = true;
                if (strSplit[1] == "All")
                    GvTransactionDtl.FilterExpression = string.Empty;


                SPCall();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        oAx.BindGrid(GvTransactionDtl, ds);
                        GvTransactionDtl.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAx.BindGrid(GvTransactionDtl);
                }
                else
                    oAx.BindGrid(GvTransactionDtl);

                GvTransactionDtl.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' + DateFrom + '~' + DateTo;

            }
            if (WhichCall == "SearchByNavigation")
            {
                string strPageNum = String.Empty;
                string strNavDirection = String.Empty;
                int PageNumAfterNav = 0;
                strPageNum = strSplit[1];
                strNavDirection = strSplit[2];


                //Set Page Number
                if (strNavDirection == "RightNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) + 10;
                if (strNavDirection == "LeftNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) - 10;
                if (strNavDirection == "PageNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum);


                PageNumber = PageNumAfterNav;

                ds = Procedure();

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        oAx.BindGrid(GvTransactionDtl, ds);
                        GvTransactionDtl.JSProperties["cpRefreshNavPanel"] = strNavDirection + '~' + strPageNum + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAx.BindGrid(GvTransactionDtl);
                }
                else
                    oAx.BindGrid(GvTransactionDtl);

                //Assign Value To HiddenField So That PageLoad Binding Can Use These HiddenField
                GvTransactionDtl.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' +
                DateFrom + '~' + DateTo;
            }

        }

        //protected void GvTransactionDtl_Load(object sender, EventArgs e)
        //{

        //   GridViewDataTextColumn column1 = new GridViewDataTextColumn();
        //   GridViewDataTextColumn column2 = new GridViewDataTextColumn();
        //   if(DdlReportStyle.Value.ToString().Trim() == "A")
        //   {
        //      column1.FieldName = "Sett No.";
        //      column1.Caption = "Sett No.";
        //      column1.VisibleIndex = 6;
        //      column1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //      GvTransactionDtl.Columns.Add(column1);
        //   }
        //   else
        //   {
        //      GvTransactionDtl.Columns.RemoveAt(6);

        //      column1.FieldName = "DP Account/Sett";
        //      column1.Caption = "DP Account/Sett";
        //      column1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //      column1.VisibleIndex = 6;
        //      column1.Width = 200;
        //      GvTransactionDtl.Columns.Add(column1);

        //   }
        //    if(DdlReportStyle.Value.ToString().Trim() == "C")
        //   {
        //       GvTransactionDtl.Columns.RemoveAt(7);
        //   }
        //   else
        //   {
        //      column2.FieldName = "Client/Code";
        //      column2.Caption = "Client/Code";
        //      column2.Width = 150;
        //      column2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //      column2.VisibleIndex = 7;
        //      GvTransactionDtl.Columns.Add(column2);
        //   }

        //}
    }
}