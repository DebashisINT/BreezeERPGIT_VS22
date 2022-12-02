using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web;
using BusinessLogicLayer;

using EntityLayer.CommonELS;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_GeneralTrialDetail : System.Web.UI.Page
    {
        String MainAccount = string.Empty;
        string SegmentID = string.Empty;
        string FromDate = string.Empty;
        string Todate = string.Empty;
        string ShowType = string.Empty;
        string AccountName = string.Empty;
        string AccountType = string.Empty;
        string ZeroBal = string.Empty;
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
           

            MainAccount = Request.QueryString["mainacc"].ToString();
            SegmentID = Request.QueryString["Segment"].ToString();
            FromDate = Request.QueryString["date"].ToString();
            Todate = Request.QueryString["TDate"].ToString();
            ShowType = Request.QueryString["Type"].ToString();
            AccountName = Request.QueryString["AccName"].ToString();
            AccountType = Request.QueryString["AccType"].ToString();
            ZeroBal = Request.QueryString["ZeroBal"].ToString();

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/frmReport_GeneralTrialDetail.aspx");
            if (!IsPostBack)
            {
                Session.Remove("frm_rpt_RptGeneralTrialDetal");
                ShowDetailForP();
            } 

        }
        public void ShowDetailForP()
        {

            ds = rep.Fetch_GeneralTrialSubAccount(MainAccount, FromDate, Todate, SegmentID,Convert.ToString( Session["LastFinYear"]) ,Convert.ToString( Session["LastCompany"] ),
               Convert.ToString(ShowType).Trim(), ZeroBal, Convert.ToString(Session["ActiveCurrency"]).Split('~')[0], Convert.ToString(Session["TradeCurrency"]).Split('~')[0]);
            ViewState["dataset"] = ds;
            gridStatus.DataSource = ds.Tables[1];
            gridStatus.DataBind();
            Session["frm_rpt_RptGeneralTrialDetal"] = ds.Tables[1];
            if (ShowType.ToString().Trim() == "A")
            {
                gridStatus.Columns[4].Visible = false;
                gridStatus.Columns[5].Visible = false;
                gridStatus.Columns[6].Visible = false;
                gridStatus.Columns[7].Visible = false;
            }
            // Make the group footers always visible.
            //gridStatus.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //// Create and setup the first summary item.
            //grid
            //GridGroupSummaryItem item = new GridGroupSummaryItem();
            //item.FieldName = "OpeningDr";
            //item.SummaryType = DevExpress.Data.SummaryItemType.Count;
            //gridStatus.GroupSummary.Add(item);
            //// Create and setup the second summary item.
            //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            //item1.FieldName = "OpeningCr";
            //item1.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //item1.DisplayFormat = "Total {0:c2}";
            //item1.ShowInGroupColumnFooter = colUnitPrice;
            //gridStatus.GroupSummary.Add(item1);

            //String strHtml = String.Empty;

            //strHtml = "<table width=\"932px\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            //strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            //strHtml += "<td align=\"center\" ><b>Sub A/C Name</b></td>";
            //strHtml += "<td align=\"center\" ><b>Branch</b></td>";
            //strHtml += "<td align=\"center\" ><b>UCC</b></td>";
            //if (ShowType == "P")
            //{
            //    strHtml += "<td align=\"center\" ><b>Opening Debit</b></td>";
            //    strHtml += "<td align=\"center\" ><b>Opening Credit</b></td>";
            //    strHtml += "<td align=\"center\" ><b>Amount Debit</b></td>";
            //    strHtml += "<td align=\"center\" ><b>Amount Credit</b></td>";
            //}
            //strHtml += "<td align=\"center\" ><b>Closing Debit</b></td>";
            //strHtml += "<td align=\"center\" ><b>Closing Credit</b></td>";
            //strHtml += "</tr>";

            //int flag = 0;

            //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            //{
            //    flag = flag + 1;
            //    strHtml += "<tr valign=top id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            //    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][3].ToString() + "</td>";
            //    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][0].ToString() + "</td>";
            //    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][1].ToString() + "</td>";
            //    if (ShowType == "P")
            //    {
            //        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][4].ToString() + "</td>";
            //        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][5].ToString() + "</td>";
            //        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][6].ToString() + "</td>";
            //        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][7].ToString() + "</td>";
            //    }
            //    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][8].ToString() + "</td>";
            //    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[1].Rows[i][9].ToString() + "</td>";

            //    strHtml += "</tr>";
            // }
            // strHtml += "<tr valign=top ><td align=\"left\" style=\"font-size:8px;font-weight:bold;\" >Total:</td><td align=\"left\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;</td><td align=\"left\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;</td>";
            // if (ShowType == "P")
            // {
            //     strHtml += "<td align=\"right\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;" + ds.Tables[0].Rows[0][0].ToString() + "</td><td align=\"right\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;" + ds.Tables[0].Rows[0][1].ToString() + "</td><td align=\"right\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;" + ds.Tables[0].Rows[0][2].ToString() + "</td><td align=\"right\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;" + ds.Tables[0].Rows[0][3].ToString() + "</td>";
            // }
            // strHtml += "<td align=\"right\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;" + ds.Tables[0].Rows[0][4].ToString() + "</td><td align=\"right\" style=\"font-size:8px;font-weight:bold;\" >&nbsp;" + ds.Tables[0].Rows[0][5].ToString() + "</td></tr>";
            // strHtml += "</table>";


            //display.InnerHtml = strHtml;
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "DisplayCon();", true);

            //  }
            // }
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }

        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString() == "s")
            {
                gridStatus.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                gridStatus.FilterExpression = string.Empty;
            }


        }


        string TotalUnitPrice;
        public string GetTotalOPDr()
        {
            DataSet ds1 = (DataSet)ViewState["dataset"];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                TotalUnitPrice = ds1.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                TotalUnitPrice = "0.00";
            }
            return TotalUnitPrice;
        }
        public string GetTotalOPCr()
        {
            DataSet ds1 = (DataSet)ViewState["dataset"];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                TotalUnitPrice = ds1.Tables[0].Rows[0][1].ToString();
            }
            else
            {
                TotalUnitPrice = "0.00";
            }
            return TotalUnitPrice;
        }
        public string GetTotalAmDr()
        {
            DataSet ds1 = (DataSet)ViewState["dataset"];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                TotalUnitPrice = ds1.Tables[0].Rows[0][2].ToString();
            }
            else
            {
                TotalUnitPrice = "0.00";
            }
            return TotalUnitPrice;
        }
        public string GetTotalAmCr()
        {
            DataSet ds1 = (DataSet)ViewState["dataset"];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                TotalUnitPrice = ds1.Tables[0].Rows[0][3].ToString();
            }
            else
            {
                TotalUnitPrice = "0.00";
            }
            return TotalUnitPrice;

        }
        public string GetTotalCLDr()
        {
            DataSet ds1 = (DataSet)ViewState["dataset"];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                TotalUnitPrice = ds1.Tables[0].Rows[0][4].ToString();
            }
            else
            {
                TotalUnitPrice = "0.00";
            }
            return TotalUnitPrice;
        }
        public string GetTotalCLCr()
        {
            DataSet ds1 = (DataSet)ViewState["dataset"];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                TotalUnitPrice = ds1.Tables[0].Rows[0][5].ToString();
            }
            else
            {
                TotalUnitPrice = "0.00";
            }
            return TotalUnitPrice;
        }


        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            export();
        }

        void export()
        {
            DataTable dtEx = new DataTable();
            DataSet dsNew = (DataSet)ViewState["dataset"];
            DataTable dtMain = dsNew.Tables[1];
            dtEx.Columns.Add("Name");
            dtEx.Columns.Add("Branch");
            dtEx.Columns.Add("UCC");
            if (ShowType == "P")
            {
                dtEx.Columns.Add("Opening Debit");
                dtEx.Columns.Add("Opening Credit");
                dtEx.Columns.Add("Amount Debit");
                dtEx.Columns.Add("Amount Credit");
            }
            dtEx.Columns.Add("Closing Debit");
            dtEx.Columns.Add("Closing Credit");
            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                DataRow newRow = dtEx.NewRow();
                newRow["Name"] = dtMain.Rows[i]["accountsledger_subaccountName"].ToString();
                newRow["Branch"] = dtMain.Rows[i]["BRANCH"].ToString();
                newRow["UCC"] = dtMain.Rows[i]["UCC"].ToString();
                if (ShowType == "P")
                {
                    newRow["Opening Debit"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    newRow["Opening Credit"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    newRow["Amount Debit"] = dtMain.Rows[i]["AmountDr"].ToString();
                    newRow["Amount Credit"] = dtMain.Rows[i]["AmountCr"].ToString();
                }
                newRow["Closing Debit"] = dtMain.Rows[i]["ClosingDr"].ToString();
                newRow["Closing Credit"] = dtMain.Rows[i]["ClosingCr"].ToString();
                dtEx.Rows.Add(newRow);
            }
            DataRow newRow1 = dtEx.NewRow();
            newRow1["Name"] = "Total";
            newRow1["Branch"] = "";
            newRow1["UCC"] = "";
            if (ShowType == "P")
            {
                newRow1["Opening Debit"] = dsNew.Tables[0].Rows[0][0].ToString();
                newRow1["Opening Credit"] = dsNew.Tables[0].Rows[0][1].ToString();
                newRow1["Amount Debit"] = dsNew.Tables[0].Rows[0][2].ToString();
                newRow1["Amount Credit"] = dsNew.Tables[0].Rows[0][3].ToString();
            }
            newRow1["Closing Debit"] = dsNew.Tables[0].Rows[0][4].ToString();
            newRow1["Closing Credit"] = dsNew.Tables[0].Rows[0][5].ToString();
            dtEx.Rows.Add(newRow1);

            DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            if (ShowType.ToString().Trim() == "A")
            {
                DrRowR1[0] = "General Trial For Date  [" + oconverter.ArrangeDate2(FromDate.ToString()) + "] For Main Account : " + AccountName + "";

            }
            else
            {
                DrRowR1[0] = "General Trial Period [" + oconverter.ArrangeDate2(FromDate.ToString()) + " To " + oconverter.ArrangeDate2(Todate.ToString()) + "] For Main Account : " + AccountName + "";
            }
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
            //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(oDBEngine.GetDate().ToString(), "Test") + "]";
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);
            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtEx, "GeneralTrialSubAccount", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }



        }

        protected void gridStatus_DataBound(object sender, EventArgs e)
        {



        }

        protected void gridStatus_DataBinding(object sender, EventArgs e)
        {
            if (Session["frm_rpt_RptGeneralTrialDetal"] != null)
            {
                gridStatus.DataSource = (DataTable)Session["frm_rpt_RptGeneralTrialDetal"];
            }
        }
        protected void gridStatus_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void gridStatus_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                //if (Request.QueryString["AccType"].ToString() == "Customers")
                //{
                string lblSubID = e.GetValue("ClosingID").ToString();
                string SubAccName = e.GetValue("accountsledger_subaccountName").ToString();
                string UCC = e.GetValue("UCC").ToString();

                e.Row.Cells[0].Style.Add("cursor", "hand");
              //  e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowLedger('" + MainAccount + "','" + lblSubID + "','" + SegmentID + "','" + AccountName + "','" + SubAccName + "','" + UCC + "','" + Todate + "');");
                e.Row.Cells[0].Text = e.GetValue("accountsledger_subaccountName").ToString();
                e.Row.Cells[0].ToolTip = "Click to View Detail!";

                //((Label)e.Row.FindControl("lblTrDate")).Attributes.Add("onclick", "javascript:ShowLedger('" + lblMainID + "','" + lblSubID + "','" + ViewState["SegmentID"].ToString() + "','" + MainAccName + "','" + SubAccName + "','" + UCC + "','" + dt + "');");
                //e.Row.Cells[1].Style.Add("cursor", "hand");
                //e.Row.Cells[1].ToolTip = "Click to View Detail!";
                //}

            }
        }

        #region Export
        public void bindexport(int Filter)
        {
            string filename = "";
                filename = "Accounts Details";
                exporter.FileName = filename;
            exporter.PageHeader.Left = filename;
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



    }
}