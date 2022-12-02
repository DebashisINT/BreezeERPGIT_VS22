using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_STTReports : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        string data;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                ReportViewBind();
                date();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//



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
                if (idlist[0].ToString().Trim() == "Clients" || idlist[0].ToString().Trim() == "Company")
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

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Product")
            {
                data = "Product~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }
            else if (idlist[0] == "Company")
            {
                data = "Company~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

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
        void procedure()
        {
            string CompanyId = string.Empty;
            string clients = string.Empty;
            string Segment = string.Empty;
            string Asset = string.Empty;
            string GrpType = string.Empty;
            string GrpId = string.Empty;
            string CalType = string.Empty;
            string RptView = string.Empty;
            string CkConsolidate = string.Empty;
            string CkConsolidateSegmentScrip = string.Empty;
            string CkShowALL = string.Empty;


            if (RdbAllCompany.Checked)
            {
                CompanyId = "ALL";
            }
            else if (RdbCurrentCompany.Checked)
            {
                CompanyId = "'" + Session["LastCompany"].ToString().Trim() + "'";
            }
            else
            {
                CompanyId = Convert.ToString(HiddenField_Company.Value);
            }

            if (rdbClientALL.Checked)
            {
                clients = "ALL";
            }
            else
            {
                clients = Convert.ToString(HiddenField_Client.Value);
            }
            if (rdbSegmentAll.Checked)
            {
                Segment = "ALL";
            }
            else
            {
                Segment = Convert.ToString(HiddenField_Segment.Value);
            }
            if (rdbAssetAll.Checked)
            {
                Asset = "ALL";
            }
            else
            {
                Asset = Convert.ToString(HiddenField_Product.Value);
            }
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GrpType = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Branch.Value;
                }
            }
            else
            {
                GrpType = Convert.ToString(ddlgrouptype.SelectedItem.Text);
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = Convert.ToString(HiddenField_Group.Value);
                }
            }



            if (rdbClientALL.Checked)
            {
                clients = "ALL";
            }
            else
            {
                clients = Convert.ToString(HiddenField_Client.Value);
            }
            if (rdbSegmentAll.Checked)
            {
                Segment = "ALL";
            }
            else
            {
                Segment = Convert.ToString(HiddenField_Segment.Value);
            }


            if (DLLCalculation.SelectedItem.Value.ToString().Trim() == "1")
            {
                CalType = "Prov";
            }
            else if (DLLCalculation.SelectedItem.Value.ToString().Trim() == "2")
            {
                CalType = "Exch";
            }
            else
            {
                CalType = "ALL";
            }
            if (DLLCalculation.SelectedItem.Value.ToString().Trim() == "3")
            {
                if (DLLReportView.SelectedItem.Value.ToString().Trim() == "1")
                {
                    RptView = "6";
                }
                else
                {
                    RptView = "7";
                }
            }
            else
            {
                RptView = Convert.ToString(DLLReportView.SelectedItem.Value);
            }
            if (DLLReportView.SelectedItem.Value.ToString().Trim() == "8" || DLLReportView.SelectedItem.Value.ToString().Trim() == "9" || DLLReportView.SelectedItem.Value.ToString().Trim() == "10")
            {
                if (ChkCOnsolidatedAcrossSegment.Checked)
                {
                    CkConsolidate = "Y";
                }
                else
                {
                    CkConsolidate = "N";
                }
            }
            else
            {
                if (ChkConsolidate.Checked)
                {
                    CkConsolidate = "Y";
                }
                else
                {
                    CkConsolidate = "N";
                }
            }

            if (ChkConsolidateSegmentScrip.Checked)
            {
                CkConsolidateSegmentScrip = "Y";
            }
            else
            {
                CkConsolidateSegmentScrip = "N";
            }
            if (ChkShowALL.Checked)
            {
                CkShowALL = "Y";
            }
            else
            {
                CkShowALL = "N";
            }
            ds = oReports.Report_STTReports(
                CompanyId,
                Convert.ToString(DtFrom.Value),
                 Convert.ToString(DtTo.Value),
                 clients,
                 Segment,
                 Asset,
                 GrpType,
                 GrpId,
                  Convert.ToString(Session["userbranchHierarchy"]),
                   Convert.ToString(Session["LastFinYear"]),
                   CalType,
                   RptView,
                   CkConsolidate,
                   CkConsolidateSegmentScrip,
                   CkShowALL
                );
            ViewState["dataset"] = ds;

            if (DLLReportView.SelectedItem.Value.ToString().Trim() == "8" || DLLReportView.SelectedItem.Value.ToString().Trim() == "9")
            {
                if (ChkCOnsolidatedAcrossSegment.Checked)
                {
                    ds.Tables[0].Columns.Remove("StatusOrder");
                    ds.Tables[0].Columns.Remove("Grp");
                }

            }


        }
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                FnHtml(ds);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }
        }
        void FnHtml(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Calculation Type : " + DLLCalculation.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            if (DLLReportView.SelectedItem.Value.ToString().Trim() != "8" && DLLReportView.SelectedItem.Value.ToString().Trim() != "9" && DLLReportView.SelectedItem.Value.ToString().Trim() != "10")
            {
                //////////For Footer
                String strHtmlFooter = String.Empty;
                strHtmlFooter = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlFooter += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\"> ** [DB/F] - Delivery Buy / Future </td></tr>";
                strHtmlFooter += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\"> ** [DS/O] - Delivery Sell / Options </td></tr>";
                strHtmlFooter += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\"> ** [DF/E] - Difference / Exercise </td></tr>";
                strHtmlFooter += "</table>";
                DivFooter.InnerHtml = strHtmlFooter;
            }

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
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
                        if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                        }
                        else
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                export(ds);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }

        }
        void export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString()) + "; Calculation Type : ";

            if (DLLCalculation.SelectedItem.Value.ToString().Trim() == "1")
            {
                str = str + "Collected From Clients";
            }
            if (DLLCalculation.SelectedItem.Value.ToString().Trim() == "2")
            {
                str = str + "Paid To Exchange";
            }
            if (DLLCalculation.SelectedItem.Value.ToString().Trim() == "3")
            {
                str = str + "Reconciliation Statement";
            }

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);

            if (DLLReportView.SelectedItem.Value.ToString().Trim() != "8" && DLLReportView.SelectedItem.Value.ToString().Trim() != "9" && DLLReportView.SelectedItem.Value.ToString().Trim() != "10")
            {
                if (DLLCalculation.SelectedItem.Value.ToString().Trim() != "3")
                {
                    DataRow DrRowR2 = dtReportHeader.NewRow();
                    DrRowR2[0] = "** [DB/F] - Delivery Buy / Future ; ** [DS/O] - Delivery Sell / Options; ** [DF/E] - Difference / Exercise";
                    dtReportHeader.Rows.Add(DrRowR2);
                }

            }
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

            objExcel.ExportToExcelforExcel(dtExport, "STT Reports", "Client Total", dtReportHeader, dtReportFooter);

        }
        protected void DLLCalculation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportViewBind();
        }
        void ReportViewBind()
        {
            DLLReportView.Items.Clear();
            if (DLLCalculation.SelectedItem.Value.ToString().Trim() == "3")
            {
                DLLReportView.Items.Insert(0, new ListItem("Client Wise", "1"));
                DLLReportView.Items.Insert(1, new ListItem("Client + Date", "2"));

            }
            else
            {
                DLLReportView.Items.Insert(0, new ListItem("Client Wise", "1"));
                DLLReportView.Items.Insert(1, new ListItem("Client + Date", "2"));
                DLLReportView.Items.Insert(2, new ListItem("Client + Instrument", "3"));
                DLLReportView.Items.Insert(3, new ListItem("Intrument Wise", "4"));
                DLLReportView.Items.Insert(4, new ListItem("Instrument + Client", "5"));
                DLLReportView.Items.Insert(5, new ListItem("Month Wise -Across Segment", "8"));
                DLLReportView.Items.Insert(6, new ListItem("Month Wise -Across Segment + Client", "9"));
                DLLReportView.Items.Insert(7, new ListItem("Month Wise -Across Segment + Group/Branch", "10"));
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>fn_CalculationType('" + DLLCalculation.SelectedItem.Value.ToString().Trim() + "');</script>");

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "fn_CalculationType", "fn_CalculationType('" + DLLCalculation.SelectedItem.Value.ToString().Trim() + "');", true);
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            export(ds);
        }
    }
}