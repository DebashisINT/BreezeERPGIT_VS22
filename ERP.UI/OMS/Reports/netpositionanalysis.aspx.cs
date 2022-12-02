using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_netpositionanalysis : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        int pageindex = 0;
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

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();
                chkboxliststyle();

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

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                litSegmentMain.InnerText = "NSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                litSegmentMain.InnerText = "BSE-FO";

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                litSegmentMain.InnerText = "NSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                litSegmentMain.InnerText = "BSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                litSegmentMain.InnerText = "MCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                litSegmentMain.InnerText = "MCXSX-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                litSegmentMain.InnerText = "NCDEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                litSegmentMain.InnerText = "DGCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                litSegmentMain.InnerText = "NMCE-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                litSegmentMain.InnerText = "ICEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                litSegmentMain.InnerText = "USE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                litSegmentMain.InnerText = "NSEL-SPOT";
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

                if (idlist[0] == "Scrips")
                {
                    data = "Scrips~" + str;

                }
                else if (idlist[0] == "Clients")
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
        void SelectionChkBoxlist()
        {
            string parameter = "";
            int colcount = 0;
            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "Buy Qty")
                    {
                        parameter = "[Buy Qty]";
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Buy Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Buy Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Buy Value]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Buy Avg")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Buy Avg]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Buy Avg]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Sell Qty")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sell Qty]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sell Qty]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Sell Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sell Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sell Value]";

                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Sell Avg")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sell Avg]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sell Avg]";
                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Open BuyQty")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Open BuyQty]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Open BuyQty]";
                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Open SellQty")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Open SellQty]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Open SellQty]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Net Value")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Net Value]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Net Value]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Close Price")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Close Price]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Close Price]";
                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "Exposure")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Exposure]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Exposure]";
                        }

                        colcount = colcount + 1;
                    }
                }
            }
            if (colcount > 2)
            {
                Procedure(parameter.ToString().Trim());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertRecord", "AlertRecord(1);", true);
            }

        }
        void Procedure(string parameter)
        {

            string Segmentid = string.Empty;
            string Clientid = string.Empty;
            string GrpType = string.Empty;
            string GrpId = string.Empty;
            string Productid = string.Empty;
            if (rdbSegmentAll.Checked)
            {
                Segmentid = "ALL";

            }
            else if (rdbSegmentSpecific.Checked)
            {
                Segmentid = Convert.ToString(Session["usersegid"]);

            }
            else
            {
                Segmentid = Convert.ToString(HiddenField_Segment.Value);

            }
            if (rdbClientALL.Checked)
            {
                Clientid = "ALL";
            }
            else if (rdPOAClient.Checked)
            {
                Clientid = "POA";
            }
            else
            {
                Clientid = Convert.ToString(HiddenField_Client.Value);
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
                    GrpId = Convert.ToString(HiddenField_Branch.Value);
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")
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
            else
            {
                GrpType = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = Convert.ToString(HiddenField_BranchGroup.Value);
                }
            }


            if (rdbScripAll.Checked)
            {
                Productid = "ALL";
            }
            else
            {
                Productid = Convert.ToString(HiddenField_Scrips.Value);
            }
            ds = oReports.Report_NetPositionAnalysis(
                Convert.ToString(DtFrom.Value),
                Convert.ToString(DtTo.Value),
                  Convert.ToString(Session["LastCompany"]),
                  Segmentid,
                  Clientid,
                  GrpType,
                  GrpId,
                   Convert.ToString(Session["userbranchHierarchy"]),
                   Productid,
                    Convert.ToString(DLLRptView.SelectedItem.Value),
                   parameter
                );
            ViewState["dataset"] = ds;
            SpProcessCall(ds);


        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            SelectionChkBoxlist();
        }
        void SpProcessCall(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Export(ds);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertRecord", "AlertRecord(2);", true);
            }
        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());

            str = str + " ; Report View : " + DLLRptView.SelectedItem.Text.ToString().Trim();


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

            objExcel.ExportToExcelforExcel(dtExport, "Net Position Analysis", "Total:", dtReportHeader, dtReportFooter);

        }
    }
}