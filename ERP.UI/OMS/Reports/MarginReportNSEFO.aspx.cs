using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{


    public partial class MarginReportNSEFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string data;
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        int pageindex = 0;
        string ClientName = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
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
                if (idlist[0] != "Clients")
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
        void date()
        {
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");
            DataTable dtexpiryeffectuntil = new DataTable();
            dtexpiryeffectuntil = oDBEngine.GetDataTable("master_equity", " DISTINCT top 2 equity_effectuntil", "  month(equity_effectuntil)<=month(getdate()) and year(equity_effectuntil)=year(getdate())", " equity_effectuntil desc");
            if (dtexpiryeffectuntil.Rows.Count == 2)
            {
                dtfrom.Value = Convert.ToDateTime(new DateTime(Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Year, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Month, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Day).AddDays(1).ToString());
                dtto.Value = Convert.ToDateTime(dtexpiryeffectuntil.Rows[0][0].ToString());
            }
            else
            {
                dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            }
            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            dtfor.Value = Convert.ToDateTime(idlist[2]);

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
            string Clients;
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                Clients = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
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
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
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
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value.ToString().Trim() + ")");
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
                    }
                }
                HiddenField_Client.Value = Clients;
            }

        }
        void fn_segment()
        {

            DataTable DtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange", "EXCH_INTERNALID", "exch_segmentid like 'CM%'", null);
            if (DtSeg.Rows.Count > 0)
            {
                for (int i = 0; i < DtSeg.Rows.Count; i++)
                {
                    ViewState["segment"] += "," + DtSeg.Rows[i][0].ToString();
                }

            }
        }


        void procedure()
        {

            fn_Client();
            string CalType = null;
            if (ddlCalType.SelectedValue == "0")
                CalType = "E";
            else
                CalType = "P";

            ds = dailyrep.SP_MarginReportNSEFO(ddlrpttype.SelectedValue == "0" ? "Details" : "Summary", ddldate.SelectedValue == "1" ? dtfrom.Value.ToString() : dtfor.Value.ToString(),
                ddldate.SelectedValue == "1" ? dtto.Value.ToString() : dtfor.Value.ToString(), HiddenField_Client.Value.ToString().Trim(), ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(),
                HttpContext.Current.Session["usersegid"].ToString(), CalType);
            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
            }
            // }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbandforgroup();
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    htmltable_rpttype0();
                }
                else
                {
                    htmltable_rpttype1();
                }

            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
            {
                export_rpttype0();
            }
            else
            {
                export_rpttype1();
            }

        }
        void export_rpttype0()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Columns.Add("Client", Type.GetType("System.String"));
            dtExport.Columns.Add("Underlying Asset", Type.GetType("System.String"));
            dtExport.Columns.Add("Span Margin", Type.GetType("System.String"));
            dtExport.Columns.Add("Exposure Margin", Type.GetType("System.String"));
            dtExport.Columns.Add("Total Margin", Type.GetType("System.String"));
            dtExport.Columns.Add("(%) of Total Margin", Type.GetType("System.String"));

            int ddlcount = cmbgroup.Items.Count;
            dtExport.Clear();
            for (int i = 0; i < ddlcount; i++)
            {
                DataView viewgroup = new DataView();
                viewgroup = ds.Tables[0].DefaultView;
                string valItem = cmbgroup.Items[i].Value;
                viewgroup.RowFilter = "GROUPID='" + valItem + "'";
                dt = viewgroup.ToTable();

                DataRow row4 = dtExport.NewRow();
                dtExport.Rows.Add(row4);

                DataRow row3 = dtExport.NewRow();
                row3["Client"] = dt.Rows[0]["GRPNAME"].ToString();
                dtExport.Rows.Add(row3);

                foreach (DataRow dr1 in dt.Rows)
                {

                    DataRow row = dtExport.NewRow();
                    if (ClientName != dr1["ClientName"].ToString())
                    {
                        if (dr1["ClientName"] != DBNull.Value)
                        {
                            row["Client"] = dr1["ClientName"].ToString();
                        }
                        if (dr1["Client_SpanMargin"] != DBNull.Value)
                        {
                            row["Span Margin"] = dr1["Client_SpanMargin"].ToString();
                        }

                        if (dr1["Client_MTMMargin"] != DBNull.Value)
                        {
                            row["Exposure Margin"] = dr1["Client_MTMMargin"].ToString();
                        }

                        if (dr1["Client_TotalMargin"] != DBNull.Value)
                        {
                            row["Total Margin"] = dr1["Client_TotalMargin"].ToString();
                        }
                        dtExport.Rows.Add(row);
                    }
                    ClientName = dr1["ClientName"].ToString();
                    DataRow row5 = dtExport.NewRow();
                    if (dr1["ProductName"] != DBNull.Value)
                    {
                        row5["Underlying Asset"] = dr1["ProductName"].ToString();
                    }


                    if (dr1["SpanMargin"] != DBNull.Value)
                    {
                        row5["Span Margin"] = dr1["SpanMargin"].ToString();
                    }

                    if (dr1["MTMMargin"] != DBNull.Value)
                    {
                        row5["Exposure Margin"] = dr1["MTMMargin"].ToString();
                    }

                    if (dr1["TotalMargin"] != DBNull.Value)
                    {
                        row5["Total Margin"] = dr1["TotalMargin"].ToString();
                    }

                    if (dr1["Ratio"] != DBNull.Value)
                    {
                        row5["(%) of Total Margin"] = dr1["Ratio"].ToString();
                    }
                    dtExport.Rows.Add(row5);
                }
                DataRow row1 = dtExport.NewRow();
                row1["Client"] = " Branch Total : ";

                if (dt.Rows[0]["Branch_SpanMargin"] != DBNull.Value)
                {
                    row1["Span Margin"] = dt.Rows[0]["Branch_SpanMargin"].ToString();

                }

                if (dt.Rows[0]["Branch_MTMMargin"] != DBNull.Value)
                {
                    row1["Exposure Margin"] = dt.Rows[0]["Branch_MTMMargin"].ToString();
                }


                if (dt.Rows[0]["Branch_TotalMargin"] != DBNull.Value)
                {
                    row1["Total Margin"] = dt.Rows[0]["Branch_TotalMargin"].ToString();
                }


                dtExport.Rows.Add(row1);


            }
            DataRow row2 = dtExport.NewRow();
            row2["Client"] = "Grand Total : ";
            row2["Span Margin"] = ds.Tables[0].Rows[0]["Grand_SpanMargin"];
            row2["Exposure Margin"] = ds.Tables[0].Rows[0]["Grand_MTMMargin"];
            row2["Total Margin"] = ds.Tables[0].Rows[0]["Grand_TotalMargin"];

            dtExport.Rows.Add(row2);



            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }

            DrRowR1[0] = "F&O Margin Report:" + str;

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(DrRowR2);
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
            if (cmbExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "FO Margin Report", "Asset Total", dtReportHeader, dtReportFooter);
            }
        }

        void export_rpttype1()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport1 = new DataTable();
            dtExport1.Columns.Add("Client", Type.GetType("System.String"));
            dtExport1.Columns.Add("Span Margin", Type.GetType("System.String"));
            dtExport1.Columns.Add("Total Margin", Type.GetType("System.String"));
            dtExport1.Columns.Add("Exposure Margin", Type.GetType("System.String"));
            dtExport1.Columns.Add("Applicable Margin", Type.GetType("System.String"));

            int ddlcount = cmbgroup.Items.Count;
            dtExport1.Clear();

            for (int i = 0; i < ddlcount; i++)
            {


                DataView viewgroup = new DataView();
                viewgroup = ds.Tables[0].DefaultView;
                string valItem = cmbgroup.Items[i].Value;
                viewgroup.RowFilter = "GROUPID='" + valItem + "'";
                dt = viewgroup.ToTable();

                DataRow row4 = dtExport1.NewRow();
                dtExport1.Rows.Add(row4);

                DataRow row3 = dtExport1.NewRow();
                row3["Client"] = dt.Rows[0]["GRPNAME"].ToString();
                dtExport1.Rows.Add(row3);
                string branchname = null;
                string SpanMargin = null;
                string TotalMargin = null;
                string ExposureMargin = null;
                string ApplicableMargin = null;

                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow row = dtExport1.NewRow();
                    if (dr1["ClientName"] != DBNull.Value)
                    {
                        row["Client"] = dr1["ClientName"].ToString();
                    }

                    if (dr1["SpanMargin"] != DBNull.Value)
                    {
                        row["Span Margin"] = dr1["SpanMargin"].ToString();
                    }


                    if (dr1["TotalMargin"] != DBNull.Value)
                    {
                        row["Total Margin"] = dr1["TotalMargin"].ToString();
                    }

                    if (dr1["ExposureMargin"] != DBNull.Value)
                    {
                        row["Exposure Margin"] = dr1["ExposureMargin"].ToString();
                    }


                    if (dr1["ApplicableMargin"] != DBNull.Value)
                    {
                        row["Applicable Margin"] = dr1["ApplicableMargin"].ToString();
                    }


                    dtExport1.Rows.Add(row);

                }
                DataRow row1 = dtExport1.NewRow();
                row1["Client"] = "Branch Total : ";

                if (dt.Rows[0]["Branch_SpanMargin"] != DBNull.Value)
                {
                    row1["Span Margin"] = dt.Rows[0]["Branch_SpanMargin"].ToString();

                }

                if (dt.Rows[0]["Branch_TotalMargin"] != DBNull.Value)
                {
                    row1["Total Margin"] = dt.Rows[0]["Branch_TotalMargin"].ToString();
                }

                if (dt.Rows[0]["Branch_ExposureMargin"] != DBNull.Value)
                {
                    row1["Exposure Margin"] = dt.Rows[0]["Branch_ExposureMargin"].ToString();
                }

                if (dt.Rows[0]["Branch_ApplicableMargin"] != DBNull.Value)
                {
                    row1["Applicable Margin"] = dt.Rows[0]["Branch_ApplicableMargin"].ToString();
                }

                dtExport1.Rows.Add(row1);


            }

            DataRow row2 = dtExport1.NewRow();
            row2["Client"] = "Grand Total : ";
            row2["Span Margin"] = ds.Tables[0].Rows[0]["Grand_SpanMargin"];
            row2["Total Margin"] = ds.Tables[0].Rows[0]["Grand_TotalMargin"];
            row2["Exposure Margin"] = ds.Tables[0].Rows[0]["Grand_ExposureMargin"];
            row2["Applicable Margin"] = ds.Tables[0].Rows[0]["Grand_ApplicableMargin"];

            dtExport1.Rows.Add(row2);

            DataTable dtReportHeader1 = new DataTable();
            DataTable dtReportFooter1 = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader1.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader1.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader1.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader1.NewRow();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }

            DrRowR1[0] = "F&O Margin Report:" + str;

            dtReportHeader1.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader1.NewRow();
            dtReportHeader1.Rows.Add(DrRowR2);
            DataRow HeaderRow1 = dtReportHeader1.NewRow();
            dtReportHeader1.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader1.NewRow();
            dtReportHeader1.Rows.Add(HeaderRow2);

            dtReportFooter1.Columns.Add(new DataColumn("Footer", typeof(String)));
            DataRow FooterRow1 = dtReportFooter1.NewRow();
            dtReportFooter1.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter1.NewRow();
            dtReportFooter1.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter1.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter1.Rows.Add(FooterRow);
            if (cmbExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport1, "FO Margin Report", "Asset Total", dtReportHeader1, dtReportFooter1);
            }

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void ddlbandforClient()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "GROUPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "ClientID", "CLIENTNAME" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "ClientID";
                cmbclient.DataTextField = "CLIENTNAME";
                cmbclient.DataBind();

            }
            ViewState["ClientID"] = Distinctclient;
            LastPage = Distinctclient.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }

        void bind_Details()
        {

            Distinctclient = (DataTable)ViewState["ClientID"];
            cmbclient.SelectedIndex = CurrentPage;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
        }
        void htmltable_rpttype0()
        {

            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "GROUPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Client</b></td>";
            strHtml += "<td align=\"center\" ><b>Underlying Asset</b></td>";
            strHtml += "<td align=\"center\"><b>Span Margin </b></td>";
            strHtml += "<td align=\"center\"><b>Exposure Margin</b></td>";
            strHtml += "<td align=\"center\"><b>Total Margin </b></td>";
            strHtml += "<td align=\"center\"><b>(%) of Total Margin</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            int i;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                ///////////////////////ALL CLIENT 

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                if (ClientName != ds.Tables[0].Rows[i]["ClientName"].ToString().Trim())
                {
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    // strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan='" + colcount.ToString().Trim() + "'><b>" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</b></td>";
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=1><b>" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\" colspan=2><b>" + ds.Tables[0].Rows[i]["Client_SpanMargin"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\" colspan=1><b>" + ds.Tables[0].Rows[i]["Client_MTMMargin"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\" colspan=1><b>" + ds.Tables[0].Rows[i]["Client_TotalMargin"].ToString() + "</b></td>";
                    strHtml += "</tr>";
                    flag = flag + 1;

                }
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                ClientName = ds.Tables[0].Rows[i]["ClientName"].ToString().Trim();
                strHtml += "<td> </td>";
                strHtml += "<td align=\"left\">" + dt1.Rows[i]["ProductName"].ToString() + " </td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["SpanMargin"].ToString() + " </td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["MTMMargin"].ToString() + " </td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["TotalMargin"].ToString() + " </td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["Ratio"].ToString() + " </td>";
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            }
            ///////////////////////GROUP TOTAL

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=2 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\"><B>Branch/Group Total :</B></td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["Branch_SpanMargin"].ToString() + "</B> </td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["Branch_MTMMargin"].ToString() + "</B></td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["Branch_TotalMargin"].ToString() + "</B></td>";
            strHtml += "</tr>";

            ///////////////////////////////////////////GROUP TOTAL END
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            lblMsg.Text = oconverter.ArrangeDate2(dtfor.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "height2", "height();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
        }
        void htmltable_rpttype1()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "GROUPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Client</b></td>";
            strHtml += "<td align=\"center\"><b>Span Margin </b></td>";
            strHtml += "<td align=\"center\"><b>Total Margin</b></td>";
            strHtml += "<td align=\"center\"><b>Exposure Margin </b></td>";
            strHtml += "<td align=\"center\"><b>Applicable Margin </b></td>";
            strHtml += "</tr>";
            int flag = 0;


            int i;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                ///////////////////////ALL CLIENT 

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\">" + dt1.Rows[i]["CLIENTNAME"].ToString() + "</td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["SpanMargin"].ToString() + " </td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["TotalMargin"].ToString() + " </td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["ExposureMargin"].ToString() + " </td>";
                strHtml += "<td align=\"right\">" + dt1.Rows[i]["ApplicableMargin"].ToString() + " </td>";

            }
            ///////////////////////GROUP TOTAL

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=1 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\"><B>Branch/Group Total :</B></td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["Branch_SpanMargin"].ToString() + "</B> </td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["Branch_TotalMargin"].ToString() + "</B></td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["Branch_ExposureMargin"].ToString() + "</B></td>";
            strHtml += "<td align=\"right\"> <B>" + dt1.Rows[0]["Branch_ApplicableMargin"].ToString() + "</B></td>";
            strHtml += "</tr>";
            ///////////////////////////////////////////GROUP TOTAL END
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            lblMsg.Text = oconverter.ArrangeDate2(dtfor.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "height1", "height();", true);

        }

        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            hiddencount.Value = "0";
            int curentIndex = cmbgroup.SelectedIndex;
            int totalNo = cmbgroup.Items.Count;
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    curentIndex = curentIndex + 1;
                    break;
                case "Prev":
                    curentIndex = curentIndex - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalGrp.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            if (curentIndex >= totalNo)
            {
                curentIndex = totalNo - 1;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }
            cmbgroup.SelectedIndex = curentIndex;
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
            {
                htmltable_rpttype0();
            }
            else
            {
                htmltable_rpttype1();
            }
        }
        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
            {
                htmltable_rpttype0();
            }
            else
            {
                htmltable_rpttype1();
            }
        }

        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = LastPage;
            bind_Details();
        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GROUPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GROUPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }

        }
        protected void btnprint_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            ReportDocument report = new ReportDocument();
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));

            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Image"] = logoinByte;
                }
            }
            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\MarginCalculation.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Portfolio Performance Report");
            report.Dispose();
            GC.Collect();
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




    }
}