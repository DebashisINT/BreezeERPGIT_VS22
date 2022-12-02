using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_DatewiseClientMarginReport : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        string data;
        ExcelFile objExcel = new ExcelFile();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //if (!IsPostBack)                    
            //{
            //    //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
            //    string sPath = HttpContext.Current.Request.Url.ToString();
            //    oDbEngine.Call_CheckPageaccessebility(sPath);
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtdate.Date = System.DateTime.Today;
                dtToDate.Date = System.DateTime.Today;
                Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>Page_Load();</script>");

                string PCompanyID = HttpContext.Current.Session["LastCompany"].ToString();

                DataTable DtCurrentSegment = oDbEngine.GetDataTable("(select exch_internalId, isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                string PCurrentSegment = DtCurrentSegment.Rows[0][0].ToString();

                DataTable vsegmentname = oDbEngine.GetDataTable("tbl_master_companyExchange", "isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as CompSegmentName", "exch_compID='" + PCompanyID + "' and  exch_internalID=" + PCurrentSegment);
                string strSegmentname = vsegmentname.Rows[0][0].ToString();
                Page.ClientScript.RegisterStartupScript(GetType(), "setbalue", "<script>SetValueforclient('" + strSegmentname + "','" + PCompanyID + "','" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + "');</script>");
                BindGroup();

            }
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);


        }
        protected DataSet FetchData()
        {
            string[] InputName = new string[20];
            string[] InputType = new string[20];
            string[] InputValue = new string[20];

            DataSet ds = new DataSet();

            string bbb = HiddenField_Group.Value;
            string aaa = HiddenField_Client.Value;
            //string[] InputName = new string[17];
            //string[] InputType = new string[17];
            //string[] InputValue = new string[17];



            ///////////////////Parameter Name
            InputName[0] = "FromDate";
            InputName[1] = "ToDate";
            InputName[2] = "Selectedby";
            InputName[3] = "Groupby";
            InputName[4] = "Valueday";
            InputName[5] = "Holdingday";
            InputName[6] = "Tradeday";
            InputName[7] = "Marginday";
            InputName[8] = "Haircut";
            InputName[9] = "DonotHaircut";
            InputName[10] = "Considercmseg";
            InputName[11] = "InitialMargin";
            InputName[12] = "Segment";
            InputName[13] = "FinYear";
            InputName[14] = "UnapprovedHaircut";
            InputName[15] = "Companyid";
            InputName[16] = "Branches";
            InputName[17] = "ClientAllorSpecific";
            //InputName[3] = "MasterSegment";
            //InputName[4] = "Rptview";


            ///////////////////Parameter Data Type
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
            InputType[17] = "V";
            //InputType[4] = "V";

            ///////////////////Parameter Value
            InputValue[0] = dtdate.Text.Split('-')[2] + "-" + dtdate.Text.Split('-')[1] + "-" + dtdate.Text.Split('-')[0];
            InputValue[1] = dtToDate.Text.Split('-')[2] + "-" + dtToDate.Text.Split('-')[1] + "-" + dtToDate.Text.Split('-')[0];
            InputValue[2] = ddlGroup.SelectedValue;
            InputValue[3] = drpBranchFilter.SelectedValue;
            InputValue[4] = drpCollatValueDay.SelectedValue;
            InputValue[5] = drpCollatHoldingDay.SelectedValue;
            InputValue[6] = drpTradeAccBalanceDay.SelectedValue;
            InputValue[7] = drpMarginFDRDay.SelectedValue;
            InputValue[8] = txtUnApprovedShares.Text;
            if (chkhaicut.Checked == true)
                InputValue[9] = "YES";
            else
                InputValue[9] = "NO";

            if (chkcmsegment.Checked == true)
                InputValue[10] = "YES";
            else
                InputValue[10] = "NO";

            InputValue[11] = DdlmarginType.SelectedValue;
            InputValue[12] = Session["usersegid"].ToString();
            InputValue[13] = HttpContext.Current.Session["LastFinYear"].ToString();
            if (chkUnApprovedShares.Checked == true)
                InputValue[14] = "YES";
            else
                InputValue[14] = "NO";

            InputValue[15] = Session["LastCompany"].ToString();
            if (ddlGroup.SelectedValue == "2")
            {
                string branchids = "";
                if (rdbranchAll.Checked == true)
                {
                    DataTable dtbranchid = oDbEngine.GetDataTable("trans_branchgroupmembers", "distinct BranchGroupMembers_BranchID", null);
                    if (dtbranchid != null)
                    {
                        if (dtbranchid.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtbranchid.Rows.Count; r++)
                            {
                                if (branchids == "")
                                    branchids = Convert.ToString(dtbranchid.Rows[r][0]);
                                else
                                    branchids = branchids + "," + Convert.ToString(dtbranchid.Rows[r][0]);

                            }

                        }
                    }
                }
                else
                {
                    DataTable dtbranchid = oDbEngine.GetDataTable("trans_branchgroupmembers", "distinct BranchGroupMembers_BranchID", "BranchGroupMembers_BranchGroupID in(" + HiddenField_BranchGroup.Value + ")");

                    if (dtbranchid != null)
                    {
                        if (dtbranchid.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtbranchid.Rows.Count; r++)
                            {
                                if (branchids == "")
                                    branchids = Convert.ToString(dtbranchid.Rows[r][0]);
                                else
                                    branchids = branchids + "," + Convert.ToString(dtbranchid.Rows[r][0]);

                            }

                        }
                    }
                }
                InputValue[16] = branchids;
            }
            else
            {
                if (rdbranchAll.Checked == true)
                    InputValue[16] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                else
                {
                    if (HiddenField_Branch.Value != "")
                        InputValue[16] = HiddenField_Branch.Value;
                    else
                        InputValue[16] = HttpContext.Current.Session["userbranchHierarchy"].ToString();

                }
            }
            if (rbClientAll.Checked == true)
                InputValue[17] = "All";
            else
            {
                if (HiddenField_Client.Value != "")
                    InputValue[17] = HiddenField_Client.Value;
                else
                    InputValue[17] = "All";

            }
            //InputValue[1] = Session["usersegid"].ToString().Trim();
            //InputValue[2] = Session["LastCompany"].ToString().Trim();
            //InputValue[3] = Session["ExchangeSegmentID"].ToString().Trim();
            //InputValue[4] = dllrptview.SelectedItem.Value.ToString().Trim();


            //////////////Sp Call
            string[,] segname = oDbEngine.GetFieldValue("tbl_master_companyexchange", "exch_segmentid", "exch_internalId=" + Session["usersegid"].ToString(), 1);

            if (segname[0, 0] == "CDX" || segname[0, 0] == "COMM")
            {
                InputName[18] = "MasterSegment";
                InputType[18] = "V";

                InputValue[18] = Session["ExchangeSegmentID"].ToString();


                //*
                InputName[19] = "Groups";
                InputType[19] = "V";
                if (rdddlgrouptypeAll.Checked == true)
                {
                    DataTable dtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "gpm_id", "rtrim(ltrim(gpm_type))='" + ddlgrouptype.SelectedValue + "'");
                    string strGroupIds = "";
                    if (dtGroup != null)
                    {

                        if (dtGroup.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtGroup.Rows.Count; r++)
                            {
                                if (strGroupIds == "")
                                    strGroupIds = Convert.ToString(dtGroup.Rows[r][0]);
                                else
                                    strGroupIds = strGroupIds + "," + Convert.ToString(dtGroup.Rows[r][0]);


                            }

                        }

                    }
                    InputValue[19] = strGroupIds;
                }
                else
                {
                    InputValue[19] = HiddenField_Group.Value;
                }



                //*

                ds = SQLProcedures.SelectProcedureArrDS("Report_ExchangeMarginReportingFile_Com_DateRangeWise", InputName, InputType, InputValue);

            }
            else if (segname[0, 0] == "FO")
            {
                InputName[19] = "Groups";
                InputType[19] = "V";
                if (rdddlgrouptypeAll.Checked == true)
                {
                    DataTable dtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "gpm_id", "rtrim(ltrim(gpm_type))='" + ddlgrouptype.SelectedValue + "'");
                    string strGroupIds = "";
                    if (dtGroup != null)
                    {

                        if (dtGroup.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtGroup.Rows.Count; r++)
                            {
                                if (strGroupIds == "")
                                    strGroupIds = Convert.ToString(dtGroup.Rows[r][0]);
                                else
                                    strGroupIds = strGroupIds + "," + Convert.ToString(dtGroup.Rows[r][0]);


                            }

                        }

                    }
                    InputValue[19] = strGroupIds;
                }
                else
                {
                    InputValue[19] = HiddenField_Group.Value;
                }
                ds = SQLProcedures.SelectProcedureArrDS("[Report_ExchangeMargin_DateRangeWise]", InputName, InputType, InputValue);

            }

            ViewState["dataset"] = ds;
            return ds;
        }
        protected void btn_show_Click(object sender, EventArgs e)
        {
            DataSet ds = FetchData();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ddlGroup.SelectedValue == "0")
                            BranchHtml(ds);
                        else if (ddlGroup.SelectedValue == "1")
                            GroupHtml(ds);
                        else if (ddlGroup.SelectedValue == "2")
                            BranchGroupHtml(ds);

                        Page.ClientScript.RegisterStartupScript(GetType(), "hidfilter", "<script>Hide('tabFilter');</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "pheight", "<script>height();</script>");

                    }
                    else
                    {
                        // Page.ClientScript.RegisterStartupScript(GetType(), "hidfilter", "<script>Hide('tabFilter');</script>");
                        //Page.ClientScript.RegisterStartupScript(GetType(), "hidexport", "<script>Hide('tr_export');</script>");
                        //Page.ClientScript.RegisterStartupScript(GetType(), "hidshowfilter", "<script>Hide('showFilter');</script>");
                        //Page.ClientScript.RegisterStartupScript(GetType(), "hidresult", "<script>Hide('tabResult');</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "norecord", "<script>norecord();</script>");
                    }
                }
                else
                {
                    // Page.ClientScript.RegisterStartupScript(GetType(), "hidfilter", "<script>Hide('tabFilter');</script>");
                    //Page.ClientScript.RegisterStartupScript(GetType(), "hidexport", "<script>Hide('tr_export');</script>");
                    //Page.ClientScript.RegisterStartupScript(GetType(), "hidshowfilter", "<script>Hide('showFilter');</script>");
                    //Page.ClientScript.RegisterStartupScript(GetType(), "hidresult", "<script>Hide('tabResult');</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "norecord", "<script>norecord();</script>");

                }
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
                            if (idlist[0] == "MAILEMPLOYEE")
                            {
                                str = AcVal[0];

                            }
                            else
                            {
                                str = "'" + AcVal[0] + "'";
                                str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                        else
                        {
                            if (idlist[0] == "MAILEMPLOYEE")
                            {
                                str += "," + AcVal[0];
                            }
                            else
                            {
                                str += ",'" + AcVal[0] + "'";
                                str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                    }
                }


                if (idlist[0] == "Product")
                {
                    data = "Product~" + str;
                    // seleectedunderlying();
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
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    data = "MAILEMPLOYEE~" + str;
                }
            }
        }

        protected void btngenerate_Click(object sender, EventArgs e)
        {
            DataSet ds = FetchData();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ddlGroup.SelectedValue == "0")
                            export("E"); ;

                        //Page.ClientScript.RegisterStartupScript(GetType(), "hidfilter", "<script>Hide('tabFilter');</script>");
                        //Page.ClientScript.RegisterStartupScript(GetType(), "pheight", "<script>height();</script>");

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "norecord", "<script>norecord();</script>");
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "norecord", "<script>norecord();</script>");

                }
            }
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExport.SelectedItem.Value == "E")
                export("E");
            else if (ddlExport.SelectedItem.Value == "P")
                export("P");
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
            DataTable DtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
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
        void maxdate()
        {
            DataTable Dtclosingdate = oDbEngine.GetDataTable("Trans_DailyStatistics", "convert(varchar(11),max(DailyStat_DateTime),106)", "DailyStat_DateTime<='" + dtdate.Value + "' and DailyStat_ExchangeSegmentID=1");
            DataTable Dtvardate = oDbEngine.GetDataTable("Trans_DailyVar", "convert(varchar(11),max(DailyVar_Date),106)", "DailyVar_Date<='" + dtdate.Value + "' and DailyVar_ExchangeSegmentID=1");

            String strhtml = String.Empty;

            strhtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strhtml += "<tr style=\"background-color: #fff0f5;\"><td colspan=2>Last Date For </td></tr>";
            strhtml += "<tr style=\"background-color: White;\">";
            if (Dtclosingdate.Rows.Count > 0)
            {
                strhtml += "<td>Closing Rate :</td><td>" + Dtclosingdate.Rows[0][0].ToString() + "</td>";
            }
            strhtml += "</tr>";
            strhtml += "<tr style=\"background-color: White;\">";
            if (Dtvardate.Rows.Count > 0)
            {
                strhtml += "<td>Var Rate :</td><td>" + Dtvardate.Rows[0][0].ToString() + "</td>";
            }
            strhtml += "</tr></table>";
            display.InnerHtml = strhtml;
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
        protected void BranchHtml(DataSet ds)
        {

            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            String strHtml = String.Empty;
            str = "Report For :" + oconverter.ArrangeDate2(dtdate.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
            if (drpBranchFilter.SelectedValue == "c")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total : </td>";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {

                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                //if (dr1[j].ToString().Trim().StartsWith("Branch") || dr1[j].ToString().Trim().StartsWith("Total:") || dr1[j].ToString().Trim().StartsWith("Client Net :") || dr1[j].ToString().Trim().StartsWith("Round"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Test") || dr1[j].ToString().Trim().StartsWith("ZZ"))
                                //{
                                //    strHtml += "<td>&nbsp;</td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Diff (If Any) :"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;color:red;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Exchange Obligation :"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;color:green;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else
                                //{
                                //    if (IsNumeric(dr1[j].ToString()) == true)
                                //    {
                                //        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                //    }
                                //    else
                                //    {

                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                //    }
                                //}
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";
            }
            else if (drpBranchFilter.SelectedValue == "d")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\" >Total : </td>";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";
            }
            else if (drpBranchFilter.SelectedValue == "cd")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 2; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + "'>Client : " + Convert.ToString(dr1["CLIENTNAME"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";

            }
            else if (drpBranchFilter.SelectedValue == "dc")
            {

                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    //// if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                    ////     strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count-1) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + "</td></tr>";
                    ////else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    ////{
                    ////    strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\" >Total : </td>";

                    ////    for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                    ////    {
                    ////        if (dr1[j] != DBNull.Value)
                    ////        {
                    ////            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                    ////        }
                    ////        else
                    ////            strHtml += "<td>&nbsp;</td>";

                    ////    }
                    ////    strHtml += "</tr>";

                    ////}
                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Date : " + Convert.ToString(dr1["MARGINDATE"]) + "</td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total : </td>";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";

            }


            divShow.InnerHtml = strHtmlheader + strHtml;
        }

        protected void GroupHtml(DataSet ds)
        {
            String strHtmlheader = String.Empty;
            string str = null;
            String strHtml = String.Empty;
            str = "Report For :" + oconverter.ArrangeDate2(dtdate.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
            if (drpBranchFilter.SelectedValue == "c")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Group : " + Convert.ToString(dr1["GROUPS"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total : </td>";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {

                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                //if (dr1[j].ToString().Trim().StartsWith("Branch") || dr1[j].ToString().Trim().StartsWith("Total:") || dr1[j].ToString().Trim().StartsWith("Client Net :") || dr1[j].ToString().Trim().StartsWith("Round"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Test") || dr1[j].ToString().Trim().StartsWith("ZZ"))
                                //{
                                //    strHtml += "<td>&nbsp;</td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Diff (If Any) :"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;color:red;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Exchange Obligation :"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;color:green;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else
                                //{
                                //    if (IsNumeric(dr1[j].ToString()) == true)
                                //    {
                                //        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                //    }
                                //    else
                                //    {

                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                //    }
                                //}
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";
            }
            else if (drpBranchFilter.SelectedValue == "d")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Group : " + Convert.ToString(dr1["GROUPS"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\" >Total : </td>";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";
            }
            else if (drpBranchFilter.SelectedValue == "cd")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 2; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + "'>Group : " + Convert.ToString(dr1["GROUPS"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total(Group) : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + "'>Client : " + Convert.ToString(dr1["CLIENTNAME"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total(Client) : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";

            }
            else if (drpBranchFilter.SelectedValue == "dc")
            {

                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    //// if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                    ////     strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count-1) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + "</td></tr>";
                    ////else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    ////{
                    ////    strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\" >Total : </td>";

                    ////    for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                    ////    {
                    ////        if (dr1[j] != DBNull.Value)
                    ////        {
                    ////            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                    ////        }
                    ////        else
                    ////            strHtml += "<td>&nbsp;</td>";

                    ////    }
                    ////    strHtml += "</tr>";

                    ////}
                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Date : " + Convert.ToString(dr1["MARGINDATE"]) + "</td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total : </td>";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";

            }
            //    strHtml += "</table>";
            //}

            divShow.InnerHtml = strHtmlheader + strHtml;

        }

        protected void BranchGroupHtml(DataSet ds)
        {

            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            String strHtml = String.Empty;
            string strBranchGroupValue = "";
            str = "Report For :" + oconverter.ArrangeDate2(dtdate.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
            if (drpBranchFilter.SelectedValue == "c")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 2; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {


                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA" && Convert.ToString(dr1["BRANCH"]) == "AAAAAAA")
                    {
                        strBranchGroupValue = Convert.ToString(dr1["BRANCHGROUP"]) == "" ? "NA" : Convert.ToString(dr1["BRANCHGROUP"]);
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>BranchGroup : " + strBranchGroupValue + " </td></tr>";
                    }
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA" && Convert.ToString(dr1["BRANCH"]) != "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ" && Convert.ToString(dr1["BRANCH"]) != "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total(Branch) : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ" && Convert.ToString(dr1["BRANCH"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total(BranchGroup) : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {

                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                //if (dr1[j].ToString().Trim().StartsWith("Branch") || dr1[j].ToString().Trim().StartsWith("Total:") || dr1[j].ToString().Trim().StartsWith("Client Net :") || dr1[j].ToString().Trim().StartsWith("Round"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Test") || dr1[j].ToString().Trim().StartsWith("ZZ"))
                                //{
                                //    strHtml += "<td>&nbsp;</td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Diff (If Any) :"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;color:red;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else if (dr1[j].ToString().Trim().StartsWith("Exchange Obligation :"))
                                //{
                                //    strHtml += "<td align=\"left\" style=\"font-size:smaller;color:green;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                //}
                                //else
                                //{
                                //    if (IsNumeric(dr1[j].ToString()) == true)
                                //    {
                                //        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                //    }
                                //    else
                                //    {

                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                //    }
                                //}
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";
            }
            else if (drpBranchFilter.SelectedValue == "d")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 2; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900" && Convert.ToString(dr1["BRANCH"]) == "AAAAAAA")
                    {
                        strBranchGroupValue = Convert.ToString(dr1["BRANCHGROUP"]) == "" ? "NA" : Convert.ToString(dr1["BRANCHGROUP"]);
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>BranchGroup : " + strBranchGroupValue + " </td></tr>";
                    }
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900" && Convert.ToString(dr1["BRANCH"]) != "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999" && Convert.ToString(dr1["BRANCH"]) != "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\" >Total(Branch) : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999" && Convert.ToString(dr1["BRANCH"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\" >Total(BranchGroup) : </td>";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";
            }
            else if (drpBranchFilter.SelectedValue == "cd")
            {
                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 3; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    if (Convert.ToString(dr1["BRANCH"]) == "AAAAAAA" && Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA" && Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                    {
                        strBranchGroupValue = Convert.ToString(dr1["BRANCHGROUP"]) == "" ? "NA" : Convert.ToString(dr1["BRANCHGROUP"]);
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 3) + "'>BranchGroup : " + strBranchGroupValue + " </td></tr>";

                    }
                    else if (Convert.ToString(dr1["BRANCH"]) != "AAAAAAA" && Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA" && Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 3) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["BRANCH"]) != "AAAAAAA" && Convert.ToString(dr1["CLIENTNAME"]) != "AAAAAAA" && Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 3) + "'>Client : " + Convert.ToString(dr1["CLIENTNAME"]) + " </td></tr>";
                    else if (Convert.ToString(dr1["BRANCH"]) != "ZZZZZZZ" && Convert.ToString(dr1["CLIENTNAME"]) != "ZZZZZZZ" && Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total(Client) : </td>";

                        for (int j = 4; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else if (Convert.ToString(dr1["BRANCH"]) != "ZZZZZZZ" && Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ" && Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total(Branch) : </td>";

                        for (int j = 4; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else if (Convert.ToString(dr1["BRANCH"]) == "ZZZZZZZ" && Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ" && Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total(BranchGroup) : </td>";

                        for (int j = 4; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";

            }
            else if (drpBranchFilter.SelectedValue == "dc")
            {

                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + Convert.ToString(ds.Tables[0].Columns.Count - 2) + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                ////////Detail Display

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                int flag = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {

                    //// if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                    ////     strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count-1) + "'>Branch : " + Convert.ToString(dr1["BRANCH"]) + "</td></tr>";
                    ////else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    ////{
                    ////    strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\" >Total : </td>";

                    ////    for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                    ////    {
                    ////        if (dr1[j] != DBNull.Value)
                    ////        {
                    ////            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                    ////        }
                    ////        else
                    ////            strHtml += "<td>&nbsp;</td>";

                    ////    }
                    ////    strHtml += "</tr>";

                    ////}
                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td colspan='" + Convert.ToString(ds.Tables[0].Columns.Count - 1) + "'>Date : " + Convert.ToString(dr1["MARGINDATE"]) + "</td></tr>";
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align='left' style=\"color:maroon;font-size:xx-small;\">Total : </td>";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                                strHtml += "<td>&nbsp;</td>";

                        }
                        strHtml += "</tr>";

                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                }
                strHtml += "</table>";

            }


            divShow.InnerHtml = strHtmlheader + strHtml;
        }

        void export(string expType)
        {
            DataSet ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();

            // TEMP
            ////for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            ////{

            ////    if (Convert.ToString(ds.Tables[0].Rows[i]["CLIENTNAME"]) == "AAAAAAA")
            ////        ds.Tables[0].Rows[i].Delete();
            ////    else if (Convert.ToString(ds.Tables[0].Rows[i]["CLIENTNAME"]) == "ZZZZZZZ")
            ////        ds.Tables[0].Rows[i]["CLIENTNAME"] = "";
            ////    else  if (Convert.ToString(ds.Tables[0].Rows[i]["MARGINDATE"]) == "01 Jan 1900")
            ////        ds.Tables[0].Rows[i].Delete();
            ////    else if (Convert.ToString(ds.Tables[0].Rows[i]["MARGINDATE"]) == "31 Dec 2999")
            ////        ds.Tables[0].Rows[i]["MARGINDATE"] = "";


            ////}

            // TEMP

            dtExport = ds.Tables[0].Copy();

            dtExport.Clear();
            if (drpBranchFilter.SelectedValue == "c")
            {

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        row2[1] = "Branch : " + Convert.ToString(dr1["BRANCH"]);
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        row2[1] = "Total : ";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                row2[j] = dr1[j];

                            }
                            else
                                row2[j] = " ";

                        }

                    }
                    else
                    {


                        // strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {

                                row2[j] = dr1[j];

                            }
                            else
                            {
                                row2[j] = " ";
                            }
                        }

                    }
                    dtExport.Rows.Add(row2);
                }
                dtExport.Columns.Remove("BRANCH");
            }
            else if (drpBranchFilter.SelectedValue == "d")
            {


                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        row2[1] = "Branch : " + Convert.ToString(dr1["BRANCH"]);
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        row2[1] = "Total : ";

                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                row2[j] = dr1[j];

                            }
                            else
                                row2[j] = " ";

                        }

                    }
                    else
                    {


                        // strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {

                                row2[j] = dr1[j];

                            }
                            else
                            {
                                row2[j] = " ";
                            }
                        }

                    }
                    dtExport.Rows.Add(row2);
                }
                dtExport.Columns.Remove("BRANCH");


            }
            else if (drpBranchFilter.SelectedValue == "cd")
            {

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        row2[2] = "Branch : " + Convert.ToString(dr1["BRANCH"]);
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "01 Jan 1900")
                        row2[2] = "Client : " + Convert.ToString(dr1["CLIENTNAME"]);
                    else if (Convert.ToString(dr1["MARGINDATE"]) == "31 Dec 2999")
                    {
                        row2[2] = "Total : ";

                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                row2[j] = dr1[j];

                            }
                            else
                                row2[j] = " ";

                        }

                    }
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        row2[2] = "Total Branch: ";
                        for (int j = 3; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                row2[j] = dr1[j];

                            }
                            else
                                row2[j] = " ";

                        }

                    }
                    else
                    {


                        // strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {

                                row2[j] = dr1[j];

                            }
                            else
                            {
                                row2[j] = " ";
                            }
                        }

                    }
                    dtExport.Rows.Add(row2);
                }
                dtExport.Columns.Remove("BRANCH");
                dtExport.Columns.Remove("CLIENTNAME");

            }
            else if (drpBranchFilter.SelectedValue == "dc")
            {

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    if (Convert.ToString(dr1["CLIENTNAME"]) == "AAAAAAA")
                        row2[1] = "Date : " + Convert.ToString(dr1["MARGINDATE"]);
                    else if (Convert.ToString(dr1["CLIENTNAME"]) == "ZZZZZZZ")
                    {
                        row2[1] = "Total : ";
                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                row2[j] = dr1[j];

                            }
                            else
                                row2[j] = " ";

                        }

                    }
                    else
                    {

                        for (int j = 1; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {

                                row2[j] = dr1[j];

                            }
                            else
                            {
                                row2[j] = " ";
                            }
                        }

                    }
                    dtExport.Rows.Add(row2);
                }
                dtExport.Columns.Remove("MARGINDATE");
            }



            ////dtExport.Clear();
            ////dtExport.Columns[0].ColumnName = "Client Name";
            ////dtExport.Columns[1].ColumnName = "UCC";
            ////dtExport.Columns[2].ColumnName = "Branch";
            ////dtExport.Columns[3].ColumnName = "Appl. Margin";
            ////dtExport.Columns[4].ColumnName = "Span Margin";
            ////dtExport.Columns[5].ColumnName = "Initial Margin";
            ////dtExport.Columns[6].ColumnName = "Leadger Balnc.";
            ////dtExport.Columns[7].ColumnName = "Margin Dep.";
            ////dtExport.Columns[8].ColumnName = "Collaterals Val.";
            ////dtExport.Columns[9].ColumnName = "Effective Deposit";
            ////dtExport.Columns[10].ColumnName = "Excess Shortage";
            ////dtExport.Columns[11].ColumnName = "Sort (%)";

            ////foreach (DataRow dr1 in ds.Tables[0].Rows)
            ////{

            ////    DataRow row2 = dtExport.NewRow();

            ////    row2["Client Name"] = dr1["Name"];
            ////    row2["UCC"] = dr1["UCC"];
            ////    row2["Branch"] = dr1["branchname"];
            ////    if (dr1["appmargin"] != DBNull.Value)
            ////    {
            ////        row2["Appl. Margin"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["appmargin"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Appl. Margin"] = dr1["appmargin"];
            ////    }
            ////    if (dr1["SpanMargin"] != DBNull.Value)
            ////    {
            ////        row2["Span Margin"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["SpanMargin"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Span Margin"] = dr1["SpanMargin"];
            ////    }
            ////    if (dr1["InitialMargin"] != DBNull.Value)
            ////    {
            ////        row2["Initial Margin"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["InitialMargin"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Initial Margin"] = dr1["InitialMargin"];
            ////    }
            ////    if (dr1["trdac"] != DBNull.Value)
            ////    {
            ////        row2["Leadger Balnc."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["trdac"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Leadger Balnc."] = dr1["trdac"];
            ////    }
            ////    if (dr1["mrgdep"] != DBNull.Value)
            ////    {
            ////        row2["Margin Dep."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["mrgdep"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Margin Dep."] = dr1["mrgdep"];
            ////    }
            ////    if (dr1["ColeteralVal"] != DBNull.Value)
            ////    {
            ////        row2["Collaterals Val."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["ColeteralVal"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Collaterals Val."] = dr1["ColeteralVal"];
            ////    }
            ////    if (dr1["effective"] != DBNull.Value)
            ////    {
            ////        row2["Effective Deposit"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["effective"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Effective Deposit"] = dr1["effective"];
            ////    }
            ////    if (dr1["excessshortage"] != DBNull.Value)
            ////    {
            ////        row2["Excess Shortage"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["excessshortage"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Excess Shortage"] = dr1["excessshortage"];
            ////    }
            ////    if (dr1["sortpercent"] != DBNull.Value)
            ////    {
            ////        row2["Sort (%)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["sortpercent"]));
            ////    }
            ////    else
            ////    {
            ////        row2["Sort (%)"] = dr1["sortpercent"];
            ////    }

            ////    dtExport.Rows.Add(row2);

            ////}
            ////decimal sort = decimal.Zero;
            ////if (ds.Tables[5].Rows[0]["excessshortage_SUM1"] == DBNull.Value)
            ////{
            ////    ds.Tables[5].Rows[0]["excessshortage_SUM1"] = 0;
            ////}
            ////if (ds.Tables[4].Rows[0]["InitialMargin_SUM1"] == DBNull.Value)
            ////{
            ////    ds.Tables[4].Rows[0]["InitialMargin_SUM1"] = 0;
            ////}
            ////sort = Math.Abs(((Convert.ToDecimal(ds.Tables[5].Rows[0]["excessshortage_SUM1"].ToString()) / Convert.ToDecimal(ds.Tables[4].Rows[0]["InitialMargin_SUM1"].ToString())) * 100));

            ////DataRow row3 = dtExport.NewRow();
            ////row3["Client Name"] = "Total";
            ////row3["Appl. Margin"] = ds.Tables[4].Rows[0]["appmargin_SUM"].ToString();
            ////row3["Span Margin"] = ds.Tables[4].Rows[0]["SpanMargin_SUM"].ToString();
            ////row3["Initial Margin"] = ds.Tables[4].Rows[0]["InitialMargin_SUM"].ToString();
            ////row3["Leadger Balnc."] = ds.Tables[4].Rows[0]["trdac_SUM"].ToString();
            ////row3["Margin Dep."] = ds.Tables[4].Rows[0]["mrgdep_SUM"].ToString();
            ////row3["Collaterals Val."] = ds.Tables[4].Rows[0]["ColeteralVal_SUM"].ToString();
            ////row3["Effective Deposit"] = ds.Tables[4].Rows[0]["effective_SUM"].ToString();
            ////row3["Excess Shortage"] = ds.Tables[5].Rows[0]["excessshortage_SUM"].ToString();
            ////row3["Sort (%)"] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(sort)));
            ////dtExport.Rows.Add(row3);

            // dtExport.Columns.Remove("sortpercent");

            DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            //// DrRowR1[0] = "Datewise Margin Reporting File[" + ds.Tables[3].Rows[0]["segmentname"].ToString() + "]" + ' ' + oconverter.ArrangeDate2(dtdate.Value.ToString()) + ' ' + oconverter.ArrangeDate2(dtToDate.Value.ToString());
            DrRowR1[0] = "Datewise Margin Reporting File For " + ' ' + oconverter.ArrangeDate2(dtdate.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtToDate.Value.ToString());


            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
            dtReportHeader.Rows.Add(DrRowR2);
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
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);


            if (expType == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Datewise Margin Reporting File", "Total", dtReportHeader, dtReportFooter);
            }
            else if (expType == "P")
            {
                objExcel.ExportToPDF(dtExport, "Datewise Margin Reporting File", "Total", dtReportHeader, dtReportFooter);
            }

        }
    }
}