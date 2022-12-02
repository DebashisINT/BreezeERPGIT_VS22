using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;
using System.Globalization;
using DevExpress.Web;
using System.Collections.Generic;
using EntityLayer.CommonELS;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_ReportBalanceSheet : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ExcelFile objExcel = new ExcelFile();
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        DataTable distinctrecord = new DataTable();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        frm_ReportBalanceSheetBL Ofrm_ReportBalanceSheetBL = new frm_ReportBalanceSheetBL();

        protected void Page_Init(object sender, EventArgs e)
        {
            BranchDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/Reports_frm_ReportBalanceSheet.aspx");
            if (!IsPostBack)
            {
                SetDatteFinYear();
                LoadBranchLookUp();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                dtfor.EditFormatString = oconverter.GetDateFormat("Date");
                //dtfor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//



        }

        protected void LoadBranchLookUp()
        {
            BranchDataSource.SelectCommand = "select branch_id,RTRIM(branch_description) branch_description,RTRIM(branch_code) branch_code from tbl_master_branch where branch_id in (" + Convert.ToString(Session["userbranchHierarchy"])+")";
            GridLookup.DataSource = BranchDataSource;
            GridLookup.DataBind();
            GridLookup.GridView.Selection.SelectAll();
        }
        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
                dtfor.Value = EndDate;
            else
                dtfor.Value = TodayDate;

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

            if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void procedure()
        {
            /*

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "REPORT_BALANCESHEET";

                cmd.CommandType = CommandType.StoredProcedure;
                if (rdSegmentAll.Checked)
                {
                    cmd.Parameters.AddWithValue("@SEGMENT", "ALL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SEGMENT", HiddenField_Segment.Value);
                }
                cmd.Parameters.AddWithValue("@COMPANYID", Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@ASONDATE", dtfor.Value);
                if (rdBanchAll.Checked)
                {
                    cmd.Parameters.AddWithValue("@BRANCHHIERCHY", Session["userbranchHierarchy"].ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BRANCHHIERCHY", HiddenField_Branch.Value);
                }
                cmd.Parameters.AddWithValue("@FINYEAR", Session["LastFinYear"].ToString());
                cmd.Parameters.AddWithValue("@RPTTYPE", ddlrptview.SelectedItem.Value.ToString());
                cmd.Parameters.AddWithValue("@ActiveCurrency",Session["ActiveCurrency"].ToString().Split('~')[0]);
                cmd.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();

                */

            //------------------------------------

            string SEGMENT, BRANCHHIERCHY="";

            if (rdSegmentAll.Checked)
            {
                SEGMENT = "ALL";
            }
            else
            {
                SEGMENT = HiddenField_Segment.Value;
            }

            //if (rdBanchAll.Checked)
            //{
            //    BRANCHHIERCHY = Session["userbranchHierarchy"].ToString();
            //}
            //else
            //{
            //    BRANCHHIERCHY = HiddenField_Branch.Value.ToString();
            //}
            //Get Product Componnet details 

            List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("branch_id");
            foreach (object Pobj in ComponentList)
            {
                BRANCHHIERCHY += "," + Pobj;
            }
            BRANCHHIERCHY = BRANCHHIERCHY.TrimStart(',');


            string xx = (Convert.ToDateTime(dtfor.Value)).ToString("MM/dd/yyyy");

            ds = Ofrm_ReportBalanceSheetBL.REPORT_BALANCESHEET(SEGMENT, Session["LastCompany"].ToString(), (Convert.ToDateTime(dtfor.Value)).ToString("MM/dd/yyyy"), BRANCHHIERCHY,
                                                         Session["LastFinYear"].ToString(), ddlrptview.SelectedItem.Value.ToString(),
                                                         Session["ActiveCurrency"].ToString().Split('~')[0]
                                                         , Session["TradeCurrency"].ToString().Split('~')[0]);


            ViewState["dataset"] = ds;
            //}

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            checkdatabase();
        }
        void checkdatabase()
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "1")
                {
                    htmldisplay();
                }
                else
                {
                    pdfgenerate();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "displayresult", "displayresult(2);", true);
            }
        }
        void htmldisplay()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\" style=\"width:50%\"><b>Liabilities </b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\" style=\"width:50%\"><b>Asset</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            strHtml += "<tr valign=top>";

            //////////////////liability
            DataView viewliability = new DataView();
            viewliability = ds.Tables[0].DefaultView;
            viewliability.RowFilter = "LIABILITYBALN IS NOT NULL";
            DataTable dt = new DataTable();
            dt = viewliability.ToTable();

            DataView viewliability1 = new DataView(dt);
            distinctrecord = new DataTable();
            distinctrecord = viewliability1.ToTable(true, new string[] { "ACID", "ACNAME", "LIABILITYBALN" });

            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:50%\"><table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            for (int i = 0; i < distinctrecord.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + distinctrecord.Rows[i]["ACNAME"].ToString() + "</td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + distinctrecord.Rows[i]["LIABILITYBALN"].ToString() + "</td>";
                strHtml += "</tr>";
            }
            strHtml += "</table></td>";

            //////////////////Asset
            DataView viewasset = new DataView();
            viewasset = ds.Tables[0].DefaultView;
            viewasset.RowFilter = "ASSETBALN IS NOT NULL";
            DataTable dt1 = new DataTable();
            dt1 = viewasset.ToTable();

            DataView viewasset1 = new DataView(dt1);
            distinctrecord = new DataTable();
            distinctrecord = viewasset.ToTable(true, new string[] { "ACID", "ACNAME", "ASSETBALN" });

            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:50%\"><table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            for (int i = 0; i < distinctrecord.Rows.Count; i++)
            {

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + distinctrecord.Rows[i]["ACNAME"].ToString() + "</td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + distinctrecord.Rows[i]["ASSETBALN"].ToString() + "</td>";
                strHtml += "</tr>";
            }
            strHtml += "</table></td>";

            strHtml += "</tr>";


            ///////////sum display
            strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:50%\">";
            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr>";
            strHtml += "<td align=\"left\" style=\"font-size:xx-small;\"><b>Total :</b></td>";
            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[0]["LIABILITYBALN_SUM"].ToString() + "</td>";
            strHtml += "</tr>";
            strHtml += "</table></td>";
            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[0]["ASSETBALN_SUM"].ToString() + "</td>";
            strHtml += "</td>";
            strHtml += "</tr>";


            //////////diff displays
            if (ds.Tables[0].Rows[0]["NAME_SUM"] != DBNull.Value)
            {
                strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:50%\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\"><b>" + ds.Tables[0].Rows[0]["NAME_SUM"].ToString() + "</b></td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[0]["LIABILITYDIFF"].ToString() + "</td>";
                strHtml += "</tr>";
                strHtml += "</table></td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[0]["ASSETDIFF"].ToString() + "</td>";
                strHtml += "</td>";
                strHtml += "</tr>";

                strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:50%\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\"><b>Net :</b></td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[0]["LIABILITYNET"].ToString() + "</td>";
                strHtml += "</tr>";
                strHtml += "</table></td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[0]["ASSETNET"].ToString() + "</td>";
                strHtml += "</td>";
                strHtml += "</tr>";
            }
            strHtml += "</table>";
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "displayresult", "displayresult(1);", true);

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void pdfgenerate()
        {
            ds = (DataSet)ViewState["dataset"];
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\ReportBalanceSheet.xsd");
            ReportDocument report = new ReportDocument();
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\ReportBalanceSheet.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["LIABILTY"].SetDataSource(ds.Tables[0]);
            report.Subreports["ASSET"].SetDataSource(ds.Tables[0]);
            report.VerifyDatabase();

            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Balance Sheet");
            report.Dispose();
            GC.Collect();
        }

        protected void btnRpt_Click(object sender, EventArgs e)
        {
            checkdatabase();
        }

        #region New Report
        protected void aspxPlBlGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            procedure();
            BindNewGrid();
        }


        public void BindNewGrid() 
        {
            if (ViewState["dataset"] != null)
            {
                ds = (DataSet)ViewState["dataset"];

                //////////////////liability
                DataView viewliability = new DataView();
                viewliability = ds.Tables[0].DefaultView;
                viewliability.RowFilter = "LIABILITYBALN IS NOT NULL";
                DataTable dt = new DataTable();
                dt = viewliability.ToTable();

                DataView viewliability1 = new DataView(dt);
                DataTable liabilityDt = new DataTable();
                liabilityDt  = viewliability1.ToTable(true, new string[] { "ACID", "ACNAME", "LIABILITYBALN" });

                //////////////////Asset
                DataView viewasset = new DataView();
                viewasset = ds.Tables[0].DefaultView;
                viewasset.RowFilter = "ASSETBALN IS NOT NULL";
                DataTable dt1 = new DataTable();
                dt1 = viewasset.ToTable();

                DataView viewasset1 = new DataView(dt1);
                DataTable assetDt = new DataTable();
                assetDt = viewasset.ToTable(true, new string[] { "ACID", "ACNAME", "ASSETBALN" });

                #region CreateFinal Data table
                DataTable GridDt = new DataTable();
                GridDt.Columns.Add("libMainAct", typeof(System.String));
                GridDt.Columns.Add("libAmount", typeof(System.Decimal));

                GridDt.Columns.Add("astMainAct", typeof(System.String));
                GridDt.Columns.Add("astAmount", typeof(System.Decimal));

                #endregion

                #region marge liability
                foreach (DataRow libDr in liabilityDt.Rows) {
                    DataRow mainDr = GridDt.NewRow();
                    mainDr["libMainAct"] = libDr["ACNAME"];
                    mainDr["libAmount"] = Convert.ToDecimal( libDr["LIABILITYBALN"]);
                    GridDt.Rows.Add(mainDr);
                }
                #endregion

                #region marge Asset
                int existingRow = 0;
                int existingRowCount = GridDt.Rows.Count;
                foreach (DataRow astDr in assetDt.Rows)
                {
                    if (existingRow < existingRowCount)
                    {
                        GridDt.Rows[existingRow]["astMainAct"] = astDr["ACNAME"];
                        GridDt.Rows[existingRow]["astAmount"] = astDr["ASSETBALN"];
                        existingRow++;
                    }
                    else
                    {
                        DataRow mainDr1 = GridDt.NewRow();
                        mainDr1["astMainAct"] = astDr["ACNAME"];
                        mainDr1["astAmount"] = Convert.ToDecimal(astDr["ASSETBALN"]);
                        GridDt.Rows.Add(mainDr1);
                    }
                }
                #endregion


                #region Calculate Total
                decimal sumLiability = 0, sumAsset = 0;
                sumLiability = Convert.ToDecimal(Convert.ToString( GridDt.Compute("sum(libAmount)", "")) == "" ? "0" : GridDt.Compute("sum(libAmount)", ""));
                sumAsset = Convert.ToDecimal(Convert.ToString( GridDt.Compute("sum(astAmount)", "")) == "" ? "0" : GridDt.Compute("sum(astAmount)", ""));

                DataRow TotalDr = GridDt.NewRow();
                TotalDr["libMainAct"] = "Total";
                TotalDr["libAmount"] = sumLiability;
                TotalDr["astMainAct"] = "Total";
                TotalDr["astAmount"] = sumAsset;

                GridDt.Rows.Add(TotalDr);
                
                #endregion

                #region Select Profit or Loss

                decimal netDif = sumAsset - sumLiability;

                if (netDif > 0)
                { //Profit
                     TotalDr = GridDt.NewRow();
                    TotalDr["libMainAct"] = "Gross Profit For The Period:";
                    TotalDr["libAmount"] = netDif; 

                    GridDt.Rows.Add(TotalDr);

                    DataRow netRw = GridDt.NewRow();
                    netRw["libMainAct"] = "Net:";
                    netRw["libAmount"] = sumLiability + netDif;
                    netRw["astAmount"] = sumAsset;
                    GridDt.Rows.Add(netRw);


                }
                else if (netDif < 0)
                {//loss 
                    TotalDr = GridDt.NewRow();
                    TotalDr["astMainAct"] = "Gross Loss For The Period:";
                    TotalDr["astAmount"] =( netDif*-1);
                    GridDt.Rows.Add(TotalDr);
                    DataRow netRw = GridDt.NewRow();
                    netRw["libMainAct"] = "Net:";
                    netRw["libAmount"] = sumLiability;
                    netRw["astAmount"] = sumAsset + (netDif*-1);
                    GridDt.Rows.Add(netRw);
                }
                


                #endregion


              
                 


              
               

                aspxPlBlGrid.DataSource = GridDt;
                Session["frm_ReportBalanceSheetData"] = GridDt;
                aspxPlBlGrid.DataBind();

            }
        }

        protected void aspxPlBlGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("libMainAct") == "Total" && e.GetValue("astMainAct") == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0x72, 0xD4, 0xE0);
                e.Row.Font.Bold = true;
                return;
            }
            if (Convert.ToString(e.GetValue("libMainAct")).Trim() == "" && Convert.ToString(e.GetValue("astMainAct")).Trim() == "Gross Loss For The Period:")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0xDC, 0x53, 0x53);
                e.Row.Font.Bold = true;
                return;
            }

            if (Convert.ToString(e.GetValue("libMainAct")).Trim() == "Gross Profit For The Period:" && Convert.ToString(e.GetValue("astMainAct")).Trim() == "")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0x83, 0xE7, 0xB9);
                e.Row.Font.Bold = true;
                return;
            }

            if (Convert.ToString(e.GetValue("libMainAct")).Trim() == "Net:" && Convert.ToString(e.GetValue("astMainAct")).Trim() == "")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0x78, 0xF1, 0xFF);
                e.Row.Font.Bold = true;
                return;
            }

        }


        protected void GridLookup_DataBinding(object sender, EventArgs e)
        {
            BranchDataSource.SelectCommand = "select branch_id,RTRIM(branch_description) branch_description,RTRIM(branch_code) branch_code from tbl_master_branch where branch_id in (" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            GridLookup.DataSource = BranchDataSource;

        }


        protected void aspxPlBlGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["frm_ReportBalanceSheetData"] != null) 
            {
                aspxPlBlGrid.DataSource = (DataTable)Session["frm_ReportBalanceSheetData"];
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

        public void bindexport(int Filter)
        {
            exporter.GridViewID = "aspxPlBlGrid";
            exporter.FileName = "Balance Sheet";
            exporter.ReportHeader = "Balance Sheet";
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


        #endregion

    }
}