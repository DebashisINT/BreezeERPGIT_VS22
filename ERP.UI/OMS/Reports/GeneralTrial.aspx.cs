using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using BusinessLogicLayer;
using DevExpress.Web;

namespace ERP.OMS.Reports
{
    public partial class Reports_GeneralTrial : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataSet ds = new DataSet();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
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
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();
              //  BindBranchFrom();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        //public void BindBranchFrom()
        //{
        //    //dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
        //    dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH";
        //    ddlBranch.TextField = "BANKBRANCH_NAME";
        //    ddlBranch.ValueField = "BANKBRANCH_ID";
        //    ddlBranch.DataBind();
        //}
        protected void listBox_Init(object sender, EventArgs e)
        {
            // Branch List

            DataTable dt = oDBEngine.GetDataTable(" (Select '0' as branch_id,'All' as branch_description Union SELECT branch_id,branch_description FROM tbl_master_Branch) as tbl", " branch_id,branch_description ", " BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ") OR BRANCH_id=0 Order By branch_description asc  ");
            ASPxListBox lb = sender as ASPxListBox;
            lb.DataSource = dt;
            lb.ValueField = "branch_id";
            lb.TextField = "branch_description";
            lb.ValueType = typeof(string);
            lb.DataBind();

            // Branch List
        }
        void Date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtAsOnDate.EditFormatString = oconverter.GetDateFormat("Date");

            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_StartDATE,FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            if (dtFinYear.Rows.Count > 0)
            {
                DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
                DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0][1].ToString());
                DtFrom.Value = Convert.ToDateTime(StartDate.ToShortDateString());
                DtTo.Value = Convert.ToDateTime(EndDate.ToShortDateString());
                DtAsOnDate.Value = Convert.ToDateTime(EndDate.ToShortDateString());

            }
            else
            {
                DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                DtAsOnDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            }
            //SegmentnameFetch();

        }
        //void SegmentnameFetch()
        //{
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]).Trim() == "1")
        //        litSegmentMain.InnerText = "NSE-CM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]).Trim() == "4")
        //        litSegmentMain.InnerText = "BSE-CM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "15")
        //        litSegmentMain.InnerText = "CSE-CM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "2")
        //        litSegmentMain.InnerText = "NSE-FO";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "4")
        //        litSegmentMain.InnerText = "BSE-FO";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "3")
        //        litSegmentMain.InnerText = "NSE-CDX";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "6")
        //        litSegmentMain.InnerText = "BSE-CDX";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "7")
        //        litSegmentMain.InnerText = "MCX-COMM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "8")
        //        litSegmentMain.InnerText = "MCXSX-CDX";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "9")
        //        litSegmentMain.InnerText = "NCDEX-COMM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "10")
        //        litSegmentMain.InnerText = "DGCX-COMM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "11")
        //        litSegmentMain.InnerText = "NMCE-COMM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "12")
        //        litSegmentMain.InnerText = "ICEX-COMM";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "13")
        //        litSegmentMain.InnerText = "USE-CDX";
        //    if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "14")
        //        litSegmentMain.InnerText = "NSEL-SPOT";
        //}

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
                    if (idlist[0].ToString().Trim() == "Company" || idlist[0].ToString().Trim() == "ExcludeMainAc")
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + val[1];
                        }


                    }
                    else
                    {
                        string[] val = cl[i].Split(';');
                        if (str == "")
                        {
                            str = val[0];
                            str1 = val[1];
                        }
                        else
                        {
                            str += "," + val[0];
                            str1 += "," + val[1];
                        }
                    }
                }


                data = idlist[0] + '~' + str + '~' + str1;

            }
        }



        protected void btnScreen_Click(object sender, EventArgs e)
        {
            DisPlay();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DisPlay();
        }
        DataSet Procedure()
        {
            string[] InputName = new string[17];
            string[] InputType = new string[17];
            string[] InputValue = new string[17];



            /////////////////Parameter Name
            InputName[0] = "FromDate";
            InputName[1] = "ToDate";
            InputName[2] = "MonthlyBreakUp";
            InputName[3] = "SubLedgerBreakUp";
            InputName[4] = "CompanyId";
            InputName[5] = "Segmentid";
            InputName[6] = "Branch";
            InputName[7] = "ReportView";
            InputName[8] = "ReportStyle";
            InputName[9] = "ZeroAmntAc";
            InputName[10] = "FinYear";
            InputName[11] = "OnlyForMonthlyBreakUpCheck";
            InputName[12] = "OnlyForSubLedgerBreakUpCheck";
            InputName[13] = "OnlyForSubLedgerBreakUpExcludeAc";
            InputName[14] = "DiaplyCompanyinColumns";
            InputName[15] = "ActiveCurrency";
            InputName[16] = "TradeCurrency";

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
            InputType[15] = "V";
            InputType[16] = "V";


            /////////////////Parameter Value
            if (ChkMonthlyBreakUp.Checked || ChkSubLedgerBreakUp.Checked)
            {
                InputValue[0] = "NA";
                InputValue[1] = DtAsOnDate.Value.ToString().Trim();

            }
            else
            {
                if (DdlDateSelection.SelectedItem.Value.ToString().Trim() == "1")
                {
                    InputValue[0] = "NA";
                    InputValue[1] = DtAsOnDate.Value.ToString().Trim();
                }
                else
                {
                    InputValue[0] = DtFrom.Value.ToString().Trim();
                    InputValue[1] = DtTo.Value.ToString().Trim();

                }
            }
            InputValue[2] = ChkMonthlyBreakUp.Checked.ToString().Trim();
            InputValue[3] = ChkSubLedgerBreakUp.Checked.ToString().Trim();

            //if (RdbAllCompany.Checked)
            //    InputValue[4] = "ALL";
            //else if (RdbCurrentCompany.Checked)
            //    InputValue[4] = "'" + Session["LastCompany"].ToString().Trim() + "'";
            //else
            //    InputValue[4] = HiddenField_Company.Value;

            InputValue[4] = "'" + Session["LastCompany"].ToString().Trim() + "'";

            //if (rdbSegmentAll.Checked)
            //    InputValue[5] = "ALL";
            //else if (rdSegmentSelected.Checked)
            //    InputValue[5] = HiddenField_Segment.Value.ToString().Trim();
            //else
            //    InputValue[5] = Session["usersegid"].ToString().Trim();

            InputValue[5] = Convert.ToString(1);

            //if (RdBranchAll.Checked)
            //    InputValue[6] = "ALL";
            //else
                InputValue[6] = HiddenField_Branch.Value;

            InputValue[7] = DdlrptView.SelectedItem.Value.ToString().Trim();
            InputValue[8] = DdlrptStyle.SelectedItem.Value.ToString().Trim();
            InputValue[9] = ChkZeroAmntAc.Checked.ToString().Trim();
            InputValue[10] = Session["LastFinYear"].ToString().Trim();

            if (RdbMonthWiseGross.Checked)
                InputValue[11] = "Gross";
            else
                InputValue[11] = "Net";

            if (RdbSubLedgerBreakUpDays.Checked)
                InputValue[12] = TxtShowUnclearedformorethan.Value.ToString().Trim();
            else
                InputValue[12] = "0";

            if (RdbSubLedgerBreakUpFollowingClients.Checked)
            {
                if (HiddenField_ExcludeMainAc.Value.ToString().Trim() == "")
                    InputValue[13] = "NA";
                else
                    InputValue[13] = HiddenField_ExcludeMainAc.Value.ToString().Trim();
            }
            else
                InputValue[13] = "NA";

            InputValue[14] = ChkCompanyColumns.Checked.ToString().Trim();
            InputValue[15] = Session["ActiveCurrency"].ToString().Split('~')[0];
            InputValue[16] = Session["TradeCurrency"].ToString().Split('~')[0];


            //////////////Sp Call
            ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Report_GeneralTrail]", InputName, InputType, InputValue);


            if (ChkSubLedgerBreakUp.Checked == true && ChkCompanyColumns.Checked == false)
            {
                if (DdlrptView.SelectedItem.Value.ToString().Trim() == "1")
                    ds.Tables[0].Columns.Remove("StOrder");
            }
            if (ChkMonthlyBreakUp.Checked)
            {
                ds.Tables[0].Columns.Remove("CmpName");
                ds.Tables[0].Columns.Remove("ProfitLossType");
                ds.Tables[0].Columns.Remove("StOrder");
                ds.Tables[0].Columns.Remove("MainAcountName");
                ds.Tables[0].Columns.Remove("AcGrpName");
            }
            if (ChkCompanyColumns.Checked == true && ChkSubLedgerBreakUp.Checked == true)
            {
                if (DdlrptView.SelectedItem.Value.ToString().Trim() == "1")
                    ds.Tables[0].Columns.Remove("Autoid");
            }
            if (ChkMonthlyBreakUp.Checked == false && ChkSubLedgerBreakUp.Checked == false && ChkCompanyColumns.Checked == true)
            {
                ds.Tables[0].Columns.Remove("CmpName");
                ds.Tables[0].Columns.Remove("ProfitLossType");
                ds.Tables[0].Columns.Remove("StOrder");
                ds.Tables[0].Columns.Remove("MainAcountName");
                ds.Tables[0].Columns.Remove("AcGrpName");
            }
            ViewState["dataset"] = ds;
            return ds;
        }

        void DisPlay()
        {
            ds = Procedure();
            string strDate = null;
            string strCompany = null;
            string strSegment = null;
            string strBranch = null;
            strDate = " Report View:" + DdlrptView.SelectedItem.Text.ToString().Trim() + " ; Report Style:" + DdlrptStyle.SelectedItem.Text.ToString().Trim();

            if (ChkMonthlyBreakUp.Checked || ChkSubLedgerBreakUp.Checked)
            {
                strDate = strDate + " ; As On Date : " + oconverter.ArrangeDate2(DtAsOnDate.Value.ToString());
            }
            else
            {
                if (DdlDateSelection.SelectedItem.Value.ToString().Trim() == "1")
                {
                    strDate = strDate + " ; As On Date : " + oconverter.ArrangeDate2(DtAsOnDate.Value.ToString());

                }
                else
                {
                    strDate = strDate + " ; For The Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());

                }
            }

            //if (RdbAllCompany.Checked)
            //{
            //    strCompany = "Company :ALL";
            //}
            //else if (RdbCurrentCompany.Checked)
            //{
            //    strCompany = "Company :Current";
            //}
            //else
            //{
            //    strCompany = "Company :" + HiddenField_CompanyName.Value.ToString().Trim();
            //}
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            strCompany = "Company :" + CompanyName.Rows[0]["cmp_Name"].ToString().Trim();
            //if (rdbSegmentAll.Checked)
            //{
            //    strSegment = "Segment :ALL";
            //}
            //else if (rdSegmentSelected.Checked)
            //{
            //    strSegment = "Segment :" + HiddenField_SegmentName.Value.ToString().Trim();
            //}
            //else
            //{
            //    strSegment = "Segment :Current";
            //}
            strSegment = "Segment :Current";
            //if (RdBranchAll.Checked)
            //{
            //    strBranch = "Branch :All";
            //}
            //else
            //{
            //    strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();
            //}
            strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();

            //////////Function Call

            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ChkSubLedgerBreakUp.Checked)
                    {
                        Export(ds, strDate.ToString().Trim(), strCompany.ToString().Trim(), strSegment.ToString().Trim(), strBranch.ToString().Trim());

                    }
                    else
                    {
                        if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                        {
                            FnHtml(ds, strDate.ToString().Trim(), strCompany.ToString().Trim(), strSegment.ToString().Trim(), strBranch.ToString().Trim());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('2');", true);
                        }
                        if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                        {
                            Export(ds, strDate.ToString().Trim(), strCompany.ToString().Trim(), strSegment.ToString().Trim(), strBranch.ToString().Trim());

                        }
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('1');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('1');", true);
            }
        }
        void FnHtml(DataSet ds, string strDate, string strCompany, string strSegment, string strBranch)
        {
            //////////For header
            String strHtmlheader = String.Empty;

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strDate + "</td></tr>";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strCompany + "</td></tr>";
            //strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strSegment + "</td></tr>";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strBranch + "</td></tr></table>";

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ColumnName.ToString().Trim() == "Detail")
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;width:5px;\"><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";

                }
                else
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("1.Liability") || dr1[j].ToString().Trim().StartsWith("Liability") || dr1[j].ToString().Trim().StartsWith("2.Asset") || dr1[j].ToString().Trim().StartsWith("Asset") || dr1[j].ToString().Trim().StartsWith("3.Income") || dr1[j].ToString().Trim().StartsWith("Income") || dr1[j].ToString().Trim().StartsWith("4.Expenditure") || dr1[j].ToString().Trim().StartsWith("Expenditure"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><u><b>" + dr1[j] + "</b></u></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("**") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("A/c Group") || dr1[j].ToString().Trim().StartsWith("All") || dr1[j].ToString().Trim().StartsWith("Difference"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;

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
        void Export(DataSet ds, string strDate, string strCompany, string strSegment, string strBranch)
        {
            DataTable dtExport = ds.Tables[0].Copy();


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = strDate.ToString().Trim() + " " + strCompany.ToString().Trim();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = strSegment.ToString().Trim() + " " + strBranch.ToString().Trim();
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

            if (dtExport.Columns.Count < 4)
            {
                dtExport.Columns.Add("Remarks", Type.GetType("System.String"));
            }
            objExcel.ExportToExcelforExcel(dtExport, "General Trial", "Total:", dtReportHeader, dtReportFooter);

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            string strDate = null;
            string strCompany = null;
            string strSegment = null;
            string strBranch = null;
            strDate = " Report View:" + DdlrptView.SelectedItem.Text.ToString().Trim() + " ; Report Style:" + DdlrptStyle.SelectedItem.Text.ToString().Trim();

            if (ChkMonthlyBreakUp.Checked || ChkSubLedgerBreakUp.Checked)
            {
                strDate = strDate + " ; As On Date : " + oconverter.ArrangeDate2(DtAsOnDate.Value.ToString());
            }
            else
            {
                if (DdlDateSelection.SelectedItem.Value.ToString().Trim() == "1")
                {
                    strDate = strDate + " ; As On Date : " + oconverter.ArrangeDate2(DtAsOnDate.Value.ToString());

                }
                else
                {
                    strDate = strDate + " ; For The Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());

                }
            }



            //if (RdbAllCompany.Checked)
            //{
            //    strCompany = "Company :ALL";
            //}
            //else if (RdbCurrentCompany.Checked)
            //{
            //    DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            //    strCompany = "Company :" + CompanyName.Rows[0]["cmp_Name"].ToString().Trim();
            //}
            //else
            //{
            //    strCompany = "Company :" + HiddenField_CompanyName.Value.ToString().Trim();
            //}
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            strCompany = "Company :" + CompanyName.Rows[0]["cmp_Name"].ToString().Trim();
            //if (rdbSegmentAll.Checked)
            //{
            //    strSegment = "Segment :ALL";
            //}
            //else if (rdSegmentSelected.Checked)
            //{
            //    strSegment = "Segment :" + HiddenField_SegmentName.Value.ToString().Trim();
            //}
            //else
            //{
            //    strSegment = "Segment :Current";
            //}
            strSegment = "Segment :Current";
            //if (RdBranchAll.Checked)
            //{
            //    strBranch = "Branch :All";
            //}
            //else
            //{
            //    strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();
            //}
            strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();

            Export(ds, strDate.ToString().Trim(), strCompany.ToString().Trim(), strSegment.ToString().Trim(), strBranch.ToString().Trim());

        }
        protected void BtnPdf_Click(object sender, EventArgs e)
        {
            ds = Procedure();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string strDate = null;
                string strCompany = null;
                string strSegment = null;
                string strBranch = null;

                strDate = " As On Date : " + oconverter.ArrangeDate2(DtAsOnDate.Value.ToString());

                //if (RdbAllCompany.Checked)
                //{
                //    strCompany = "Company :ALL";
                //}
                //else if (RdbCurrentCompany.Checked)
                //{
                //    DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                //    strCompany = "Company :" + CompanyName.Rows[0]["cmp_Name"].ToString().Trim();
                //}
                //else
                //{
                //    strCompany = "Company :" + HiddenField_CompanyName.Value.ToString().Trim();
                //}
                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                strCompany = "Company :" + CompanyName.Rows[0]["cmp_Name"].ToString().Trim();
                //if (rdbSegmentAll.Checked)
                //{
                //    strSegment = "Segment :ALL";
                //}
                //else if (rdSegmentSelected.Checked)
                //{
                //    strSegment = "Segment :" + HiddenField_SegmentName.Value.ToString().Trim();
                //}
                //else
                //{
                //    strSegment = "Segment :" + litSegmentMain.InnerText.ToString().Trim();
                //}
                strSegment = "Segment :" + HiddenField_SegmentName.Value.ToString().Trim();
                //if (RdBranchAll.Checked)
                //{
                //    strBranch = "Branch :All";
                //}
                //else
                //{
                //    strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();
                //}
                strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();
                ReportDocument report = new ReportDocument();
                //ds.Tables[0].WriteXmlSchema("D:\\RPTXSD\\GeneralTrailWithSubLedger.xsd");


                string tmpPdfPath = string.Empty;
                tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\GeneralTrailWithSubLedger.rpt");
                report.Load(tmpPdfPath);
                report.SetDataSource(ds.Tables[0]);
                report.VerifyDatabase();


                report.SetParameterValue("@strDate", (object)strDate.ToString().Trim());
                report.SetParameterValue("@strCompany", (object)strCompany.ToString().Trim());
                report.SetParameterValue("@strSegment", (object)strSegment.ToString().Trim());
                report.SetParameterValue("@strBranch", (object)strBranch.ToString().Trim());
                report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "General Trial With SubLedger");
                report.Dispose();
                GC.Collect();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('1');", true);
            }

        }
    }
}