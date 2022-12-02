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
    public partial class Reports_clientrisk : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        MISReports mis = new MISReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
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
        public int CurrentPage
        {

            get
            {
                if (this.ViewState["CurrentPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
            }

            set
            {
                this.ViewState["CurrentPage"] = value;
            }

        }

        public int LastPage
        {
            get
            {
                if (this.ViewState["LastPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["LastPage"].ToString());
            }
            set
            {
                this.ViewState["LastPage"] = value;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                chkboxliststyle();
                RateDateFetch();
            }


        }

        void chkboxliststyle()
        {

            foreach (ListItem item in chktfilter.Items)
            {
                item.Attributes.Add("style", "font-family:Times New Roman;color:#461B7E;font-size:9px");
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
                    if (idlist[0].ToString().Trim() != "Clients")
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


                if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "BranchGroup")
                {
                    data = "BranchGroup~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str;
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
        protected void btnhide_Click(object sender, EventArgs e)
        {

            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        void FnGeneRationCall(DataSet ds)
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                {
                    if (DdlrptView.SelectedItem.Value.ToString() == "Detail")///Report View 
                    {
                        BindGroup(ds);
                    }
                    if (DdlrptView.SelectedItem.Value.ToString() == "Summary")///Report View 
                    {
                        FnHtml(ds, "NA");
                    }
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                {
                    Export(ds);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('1',0);", true);
            }
        }
        void FilterColumnCheck()
        {
            string Stocks = "N";
            string Sales = "N";
            string Pur = "N";
            string AppMargin = "N";
            string ShortExcess = "N";
            string DP = "N";

            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "Mrgn/Hldbk")
                    {
                        Stocks = "Y";
                    }
                    if (listitem.Value == "DP")
                    {
                        DP = "Y";
                    }
                    if (listitem.Value == "Pndg.Sales")
                    {
                        Sales = "Y";
                    }
                    if (listitem.Value == "Pndg.Pur")
                    {
                        Pur = "Y";
                    }
                    if (listitem.Value == "App.Margin")
                    {
                        AppMargin = "Y";
                    }
                    if (listitem.Value == "Short(-)/Excess")
                    {
                        ShortExcess = "Y";
                    }
                }
            }
            Procedure(Stocks, DP, Sales, Pur, AppMargin, ShortExcess);
        }
        void Procedure(string Stocks, string DP, string Sales, string Pur, string AppMargin, string ShortExcess)
        {
            string GrpType = "";
            string GrpId = "";
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
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                GrpType = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_BranchGroup.Value;
                }
            }
            else
            {
                GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Group.Value;
                }
            }
            ds = mis.Report_ClientRisk(rdbClientALL.Checked ? "ALL" : HiddenField_Client.Value, Session["userbranchHierarchy"].ToString(),
                rdbSegmentAll.Checked ? "ALL" : HiddenField_Segment.Value, GrpType, GrpId, Session["LastCompany"].ToString().Trim(),
                Session["LastFinYear"].ToString().Trim(), DdlrptView.SelectedItem.Value.ToString().Trim(), ChkApplyHaircut.Checked.ToString().Trim(),
                DdlValuationMethod.SelectedItem.Value.ToString().Trim(), ChkOnlyShortageClient.Checked.ToString().Trim(), DdlShortageMoreThanType.SelectedItem.Value.ToString().Trim(),
                DdlShortageMoreThanType.SelectedItem.Value.ToString().Trim() == "Value" ? TxtValueShortage.Value.ToString() : TxtPercentageShortage.Value.ToString(),
                 Stocks.ToString().Trim(), Sales.ToString().Trim(), Pur.ToString().Trim(), AppMargin.ToString().Trim(), ShortExcess.ToString().Trim(), DP);

            ViewState["dataset"] = ds;
            FnGeneRationCall(ds);
        }
        void FnHtml(DataSet ds, string GrpName)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "As On  " + oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString());
            str = str + " ; Report View : " + DdlrptView.SelectedItem.Text.ToString().Trim();


            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataTable dt = new DataTable();
            if (GrpName.ToString().Trim() != "NA")
            {
                DataView viewclient = new DataView();
                viewclient = ds.Tables[0].DefaultView;
                viewclient.RowFilter = " GrpName='" + GrpName.ToString().Trim() + "'";
                dt = viewclient.ToTable();

                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][3].ToString().Trim() + "</b></td>";
                strHtml += "</tr>";

                dt.Rows[0].Delete();
            }
            else
            {
                dt = ds.Tables[0].Copy();

            }
            dt.Columns.Remove("GrpName");





            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToString().Trim() != "SegId" && dt.Columns[i].ColumnName.ToString().Trim() != "Customerid")
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
                }
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString().Trim() != "SegId" && dt.Columns[j].ColumnName.ToString().Trim() != "Customerid")
                    {
                        if (dr1[j] != DBNull.Value)
                        {
                            if (dr1[j].ToString().Trim().StartsWith("Group/Branch") || dr1[j].ToString().Trim().StartsWith("Total:"))
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                            }
                            else if (dr1[j].ToString().Trim().StartsWith("Test"))
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Unclrd Balnc")
                            {
                                if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','UnClear'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Mrgn/Hldbk")
                            {
                                if (DdlrptView.SelectedItem.Value.ToString().Trim() == "Detail")
                                {
                                    if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:" && dt.Rows[flag - 1]["Segment"].ToString().Trim() != "NSDL" && dt.Rows[flag - 1]["Segment"].ToString().Trim() != "CDSL")
                                    {
                                        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','MarginHldbk'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                    }
                                    else if (dt.Rows[flag - 1]["Segment"].ToString().Trim() == "NSDL" || dt.Rows[flag - 1]["Segment"].ToString().Trim() == "CDSL")
                                    {
                                        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','DP'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                    }
                                    else
                                    {
                                        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                    }
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                }
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Pndg.Pur")
                            {
                                if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','PendgPur'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                            }
                            else if (dt.Columns[j].ColumnName.ToString().Trim() == "Pndg.Sales")
                            {
                                if (dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Group/Branch Total:" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Grand Total :" && dt.Rows[flag - 1]["Client Name"].ToString().Trim() != "Total:")
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dt.Rows[flag - 1]["Customerid"].ToString().Trim() + "','PendgSale'," + dt.Rows[flag - 1]["SegId"].ToString().Trim() + ")>" + dr1[j] + "</a></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
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
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;
            if (GrpName.ToString().Trim() != "NA")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('2'," + dt.Columns.Count + ");", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('4'," + dt.Columns.Count + ");", true);
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
        void BindGroup(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " GrpName<>'ZZZZZZ'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctRecord = new DataTable();
            DataView viewRecord = new DataView(dt);
            DistinctRecord = viewRecord.ToTable(true, new string[] { "GrpName" });

            if (DistinctRecord.Rows.Count > 0)
            {
                cmbrecord.DataSource = DistinctRecord;
                cmbrecord.DataValueField = "GrpName";
                cmbrecord.DataTextField = "GrpName";
                cmbrecord.DataBind();

            }
            LastPage = DistinctRecord.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void bind_Details()
        {
            cmbrecord.SelectedIndex = CurrentPage;
            ds = (DataSet)ViewState["dataset"];
            FnHtml(ds, cmbrecord.SelectedItem.Value.ToString());
            ShowHidePreviousNext_of_Clients();
        }
        void ShowHidePreviousNext_of_Clients()
        {
            if (LastPage == 0 || LastPage == -1)
            {
                ASPxFirst.Style["Display"] = "none";
                ASPxPrevious.Style["Display"] = "none";
                ASPxNext.Style["Display"] = "none";
                ASPxLast.Style["Display"] = "none";

            }
            else
            {
                ASPxFirst.Style["Display"] = "Display";
                ASPxPrevious.Style["Display"] = "Display";
                ASPxNext.Style["Display"] = "Display";
                ASPxLast.Style["Display"] = "Display";

            }

            if (CurrentPage == LastPage && LastPage != 0)
            {

                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;

                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;

            }
            else
                if (CurrentPage == 0 && LastPage != 0)
                {
                    ASPxFirst.Enabled = false;
                    ASPxPrevious.Enabled = false;

                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;


                }
                else
                {
                    ASPxFirst.Enabled = true;
                    ASPxPrevious.Enabled = true;
                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;
                }
        }
        protected void ASPxFirst_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = 0;
            bind_Details();
        }
        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage - 1;
                bind_Details();
            }
        }
        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage + 1;
                bind_Details();
            }
        }
        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = LastPage;
            bind_Details();
        }

        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            dtExport.Columns.Remove("GrpName");
            dtExport.Columns.Remove("Customerid");
            dtExport.Columns.Remove("SegId");
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "As On  " + oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString());
            str = str + " ; Report View : " + DdlrptView.SelectedItem.Text.ToString().Trim();




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

            objExcel.ExportToExcelforExcel(dtExport, "Client Risk", "Group/Branch Total:", dtReportHeader, dtReportFooter);

        }

        protected void BtnDateDisply_Click(object sender, EventArgs e)
        {
            RateDateFetch();
        }
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            FilterColumnCheck();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            FilterColumnCheck();
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }

        void RateDateFetch()
        {
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText = "[Report_RateDateFetch]";
            //cmd.CommandType = CommandType.StoredProcedure;
            //if (rdbSegmentAll.Checked)
            //{
            //    cmd.Parameters.AddWithValue("@Segment", "ALL");

            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@Segment", HiddenField_Segment.Value);

            //}

            //cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cmd;
            //cmd.CommandTimeout = 0;
            //ds1.Reset();
            //ds1.Clear();
            //da.Fill(ds1);
            //da.Dispose();
            ds1 = mis.Report_RateDateFetch(rdbSegmentAll.Checked ? "ALL" : HiddenField_Segment.Value, Session["LastCompany"].ToString());
            if (ds1.Tables[0].Rows.Count > 0)
            {
                String strHtml = String.Empty;
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 0; i < ds1.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds1.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    for (int j = 0; j < ds1.Tables[0].Columns.Count; j++)
                    {
                        if (dr1[j] != DBNull.Value)
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds1.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                    }

                    strHtml += "</tr>";
                }
                strHtml += "</table>";

                DivDateDisply.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('6',0);", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('5',0);", true);

            }
            // }

        }
    }
}