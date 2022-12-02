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
    public partial class Reports_SettlementbillsCM : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        static string Group;
        static string Branch;
        string data;
        static string Clients;
        DataSet ds = new DataSet();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        int pageindex = 0;
        DataTable Distinctgroup = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtgroupcontactid = new DataTable();
        string[] idlist;
        string[] idlist1;
        int curentIndex;
        DataTable dtExport = new DataTable();
        //--for sending email
        static string SubClients = "";
        string EmailHtml = "";

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
                Clients = null;
                Group = null;
                Branch = null;
                ViewState["curentIndex"] = 0;
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                txtset.Text = Session["LastSettNo"].ToString();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            //for sending email
            rbOnlyClient.Attributes.Add("OnClick", "SelectUserClient('Client')");
            rbClientUser.Attributes.Add("OnClick", "SelectUserClient('User')");
            txtSelectID.Attributes.Add("onkeyup", "callAjax1(this,'GetMailId',event)");
            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
            FillCombo();


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
                        if (idlist[0] == "EM")
                        {
                            str = AcVal[0];
                            str1 = AcVal[0] + ";" + val[1];
                        }
                        else
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                    else
                    {
                        if (idlist[0] == "EM")
                        {
                            str += "," + AcVal[0];
                            str1 += "," + AcVal[0] + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }
            }


            if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str1;
            }
            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str;
            }
            else if (idlist[0] == "EM")
            {
                SubClients = str;
            }

        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (Group == null)
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
        void fn_Client()
        {

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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + Branch + ")");
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
            }
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                Clients = null;

                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Branch + ")");
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
            }
        }
        void alldata()
        {
            string[,] DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar)+','+cast(Settlements_EndDateTime as varchar)", "Ltrim(RTRIM(settlements_Number))='" + txtset.Text.ToString().Trim().Substring(0, 7) + "' and ltrim(RTRIM(settlements_TypeSuffix))='" + txtset.Text.ToString().Trim().Substring(7, 1) + "' and settlements_ExchangeSegmentId='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'", 1);
            if (DtStartEnddate[0, 0] != "n")
            {
                idlist = DtStartEnddate[0, 0].ToString().Split(',');
            }
            string[,] DTSEGMENTNAME = oDBEngine.GetFieldValue("tbl_master_companyExchange", "(select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId as Comp", "tbl_master_companyExchange.exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'", 1);
            if (DTSEGMENTNAME[0, 0] != "n")
            {
                idlist1 = DTSEGMENTNAME[0, 0].ToString().Split(','); // fetch segmentname
            }
        }
        void procedure()
        {
            alldata();
            fn_Client();

            ds = dailyrep.Sp_SettlementTrialNSECM1(Session["usersegid"].ToString(), Session["LastCompany"].ToString(), idlist[0], txtset.Text.ToString().Trim().Substring(0, 7),
                "'" + txtset.Text.ToString().Trim().Substring(7, 1) + "'", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()),
                "Current", idlist[1], idlist1[0], Clients, ddlGroup.SelectedItem.Value.ToString() == "0" ? "NA" : ddlgrouptype.SelectedItem.Text.ToString().Trim(),
                ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : "GROUP");

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbandforgroup();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }
        }
        void ddlbandforgroup()
        {

            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = new DataTable();
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "groupid", "groupname" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "groupid";
                cmbgroup.DataTextField = "groupname";
                cmbgroup.DataBind();

            }

            bind_Details();

        }
        void bind_Details()
        {
            SEBIFEE();
            htmlresult();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY", "DISPLAY();", true);
        }
        void SEBIFEE()
        {
            alldata();
            DataTable dtSEBIFEE = oDBEngine.GetDataTable("config_sebifee", "distinct sebifee_id", "cast('" + idlist[0] + "' as datetime) between cast(CONVERT(VARCHAR(11),sebifee_DateFrom,106) as datetime) and cast(CONVERT(VARCHAR(11),isnull(sebifee_DateTo,'2100-01-01 00:00:00.000'),106) as datetime) and sebifee_ExchangeSegmentID='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and sebifee_CompanyID='" + Session["LastCompany"].ToString() + "' and sebifee_Applicablefor not in('None','NA')");
            if (dtSEBIFEE.Rows.Count > 0)
            {
                ViewState["SEBIFEE"] = "YES";
            }
            else
            {
                ViewState["SEBIFEE"] = "NO";
            }
        }
        void htmlresult()
        {

            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=13 style=\"color:Blue;\">" + ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report For Settlement No. " + txtset.Text.ToString().Trim();
            strHtml1 += "</td></tr></table>";



            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "groupid='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" colspan=2><b>Client Name</b></td>";
            strHtml += "<td align=\"center\"><b>Code</b></td>";
            strHtml += "<td align=\"center\"><b>Difference Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Delivery In Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Delivery Out Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Net Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Tran Charges</b></td>";
            strHtml += "<td align=\"center\"><b>Serv Tax & Cess</b></td>";
            strHtml += "<td align=\"center\"><b>Sec Tran Charges</b></td>";
            strHtml += "<td align=\"center\"><b>Stamp Duty</b></td>";
            if (ViewState["SEBIFEE"].ToString() == "YES")
            {
                strHtml += "<td align=\"center\"><b>SEBI Fee</b></td>";
            }
            strHtml += "<td align=\"center\"><b>Net Receivable (Dr.)</b></td>";
            strHtml += "<td align=\"center\"><b>Net Payable (Cr.)</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" colspan=2>" + dt.Rows[i]["Name"].ToString() + "</td>";
                strHtml += "<td align=\"left\">" + dt.Rows[i]["code"].ToString() + "</td>";

                if (dt.Rows[i]["DiffPL_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["DiffPL_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Dlvvaluein_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Dlvvaluein_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Dlvvalueout_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Dlvvalueout_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["NetAmount_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["NetAmount_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Trancharge"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Trancharge"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["STaxOnTrnCharges"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["STaxOnTrnCharges"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Sttax"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Sttax"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Stamp_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Stamp_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (ViewState["SEBIFEE"].ToString() == "YES")
                {
                    if (dt.Rows[i]["sebifee"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["sebifee"].ToString())) + "</td>";
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                if (dt.Rows[i]["NetDr"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["NetDr"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["NetCr"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["NetCr"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                strHtml += "</tr>";
            }

            strHtml += "<tr>";
            strHtml += "<td style=\"color:Black;\" colspan=3><b>Total</b></td>";
            if (dt.Rows[0]["branchDiffPL_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchDiffPL_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchDlvvaluein_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchDlvvaluein_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchDlvvalueout_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchDlvvalueout_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchNetAmount_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchNetAmount_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchTrancharge"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchTrancharge"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchSTaxOnTrnCharges"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchSTaxOnTrnCharges"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchSttax"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchSttax"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchStamp_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchStamp_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (ViewState["SEBIFEE"].ToString() == "YES")
            {
                if (dt.Rows[0]["branchsebifee"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchsebifee"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
            }
            if (dt.Rows[0]["branchnet"] != DBNull.Value)
            {
                if (Convert.ToDecimal(dt.Rows[0]["branchnet"]) < 0)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(dt.Rows[0]["branchnet"].ToString()))) + "</td>";
                    strHtml += "<td>&nbsp;</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchnet"].ToString())) + "</td>";

                }
            }
            strHtml += "</tr>";
            strHtml += "</table>";

            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void btn_show_Click(object sender, EventArgs e)
        {
            procedure();
        }
        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            curentIndex = cmbgroup.SelectedIndex;
            ViewState["curentIndex"] = curentIndex;
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
                    pageindex = int.Parse(Totalgrp.Value);
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
            bind_Details();
        }
        protected void BTNLODINGDDLGROUP_Click(object sender, EventArgs e)
        {
            bind_Details();
        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Export();
        }
        void Export()
        {

            ds = (DataSet)ViewState["dataset"];
            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Code", Type.GetType("System.String"));
            dtExport.Columns.Add("Difference Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivery In Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivery Out Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Tran Charges", Type.GetType("System.String"));
            dtExport.Columns.Add("Serv Tax & Cess", Type.GetType("System.String"));
            dtExport.Columns.Add("Sec Tran Charges", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp Duty", Type.GetType("System.String"));
            if (ViewState["SEBIFEE"].ToString() == "YES")
            {
                dtExport.Columns.Add("SEBI Fee", Type.GetType("System.String"));

            }
            dtExport.Columns.Add("Net Receivable (Dr.)", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Payable (Cr.)", Type.GetType("System.String"));



            int cmbgroup_count = cmbgroup.Items.Count;
            for (int i = 0; i < cmbgroup_count; i++)
            {
                DataView viewcmbgroup = new DataView();
                viewcmbgroup = ds.Tables[0].DefaultView;
                string valItem = cmbgroup.Items[i].Value;
                viewcmbgroup.RowFilter = "groupid='" + valItem + "'";
                DataTable dt = new DataTable();
                dt = viewcmbgroup.ToTable();

                string grptype = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    grptype = "Branch";
                }
                else
                {
                    grptype = "Group";
                }
                DataRow row = dtExport.NewRow();
                row["Client Name"] = grptype + " : " + cmbgroup.Items[i].Text.ToString().Trim();
                row["Code"] = "Test";
                dtExport.Rows.Add(row);


                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    row2["Client Name"] = dr1["Name"].ToString();
                    row2["Code"] = dr1["code"].ToString();
                    if (dr1["DiffPL_Sum"] != DBNull.Value)
                    {
                        row2["Difference Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["DiffPL_Sum"]));
                    }
                    if (dr1["Dlvvaluein_Sum"] != DBNull.Value)
                    {
                        row2["Delivery In Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["Dlvvaluein_Sum"]));
                    }
                    if (dr1["Dlvvalueout_Sum"] != DBNull.Value)
                    {
                        row2["Delivery Out Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["Dlvvalueout_Sum"]));
                    }
                    if (dr1["NetAmount_Sum"] != DBNull.Value)
                    {
                        row2["Net Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["NetAmount_Sum"]));
                    }
                    if (dr1["Trancharge"] != DBNull.Value)
                    {
                        row2["Tran Charges"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["Trancharge"]));
                    }
                    if (dr1["STaxOnTrnCharges"] != DBNull.Value)
                    {
                        row2["Serv Tax & Cess"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["STaxOnTrnCharges"]));
                    }
                    if (dr1["Sttax"] != DBNull.Value)
                    {
                        row2["Sec Tran Charges"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["Sttax"]));
                    }
                    if (dr1["Stamp_Sum"] != DBNull.Value)
                    {
                        row2["Stamp Duty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["Stamp_Sum"]));
                    }
                    if (ViewState["SEBIFEE"].ToString() == "YES")
                    {
                        if (dr1["sebifee"] != DBNull.Value)
                        {
                            row2["SEBI Fee"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["sebifee"]));
                        }
                    }
                    if (dr1["NetDr"] != DBNull.Value)
                    {
                        row2["Net Receivable (Dr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["NetDr"]));
                    }
                    if (dr1["NetCr"] != DBNull.Value)
                    {
                        row2["Net Payable (Cr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["NetCr"]));
                    }
                    dtExport.Rows.Add(row2);
                }

                DataRow row11 = dtExport.NewRow();
                row11["Client Name"] = "Total";
                if (dt.Rows[0]["branchDiffPL_Sum"] != DBNull.Value)
                {
                    row11["Difference Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchDiffPL_Sum"]));
                }
                if (dt.Rows[0]["branchDlvvaluein_Sum"] != DBNull.Value)
                {
                    row11["Delivery In Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchDlvvaluein_Sum"]));
                }
                if (dt.Rows[0]["branchDlvvalueout_Sum"] != DBNull.Value)
                {
                    row11["Delivery Out Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchDlvvalueout_Sum"]));
                }
                if (dt.Rows[0]["branchNetAmount_Sum"] != DBNull.Value)
                {
                    row11["Net Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchNetAmount_Sum"]));
                }
                if (dt.Rows[0]["branchTrancharge"] != DBNull.Value)
                {
                    row11["Tran Charges"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchTrancharge"]));
                }
                if (dt.Rows[0]["branchSTaxOnTrnCharges"] != DBNull.Value)
                {
                    row11["Serv Tax & Cess"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchSTaxOnTrnCharges"]));
                }
                if (dt.Rows[0]["branchSttax"] != DBNull.Value)
                {
                    row11["Sec Tran Charges"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchSttax"]));
                }
                if (dt.Rows[0]["branchStamp_Sum"] != DBNull.Value)
                {
                    row11["Stamp Duty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchStamp_Sum"]));
                }
                if (ViewState["SEBIFEE"].ToString() == "YES")
                {
                    if (dt.Rows[0]["branchsebifee"] != DBNull.Value)
                    {
                        row11["SEBI Fee"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchsebifee"]));
                    }

                }
                if (dt.Rows[0]["branchnet"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(dt.Rows[0]["branchnet"]) < 0)
                    {
                        row11["Net Receivable (Dr.)"] = oconverter.formatmoneyinUs(Math.Abs(Convert.ToDecimal(dt.Rows[0]["branchnet"])));
                    }
                    else
                    {
                        row11["Net Payable (Cr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["branchnet"]));

                    }
                }
                dtExport.Rows.Add(row11);
            }


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = ds.Tables[1].Rows[0]["CompanyName"].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            DrRowR1[0] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Settlement Bills [" + ds.Tables[1].Rows[0]["SegmentName"].ToString() + "]" + ' ' + "Settlement Number -" + txtset.Text.ToString().Trim() + ' ' + ' ' + "Trade Date :" + ' ' + ds.Tables[1].Rows[0]["Tradedate"].ToString() + ' ' + "Funds PayOut :" + ' ' + ds.Tables[1].Rows[0]["FundsPayout"].ToString();

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
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

            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Settlement Bills", "Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtExport, "Settlement Bills", "Total", dtReportHeader, dtReportFooter);
            }

        }
        private void FillCombo()
        {
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                clsdrp.AddDataToDropDownList(r, cmbsearch);
            }
            else
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                //r[1, 0] = "CL";
                //r[1, 1] = "Customers";
                //r[2, 0] = "LD";
                //r[2, 1] = "Lead";
                //r[3, 0] = "CD";
                //r[3, 1] = "CDSL Client";
                //r[4, 0] = "ND";
                //r[4, 1] = "NSDL Client";
                //r[5, 0] = "BP";
                //r[5, 1] = "Business Partne";
                //r[6, 0] = "RA";
                //r[6, 1] = "Relationship Partner";
                //r[7, 0] = "SB";
                //r[7, 1] = "Sub Broker";
                //r[8, 0] = "FR";
                //r[8, 1] = "Franchisees";
                clsdrp.AddDataToDropDownList(r, cmbsearch);

            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string billdate = "";
            string Subject = " Settlement Bills [" + ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise] Report for Settlement No. " + txtset.Text.ToString().Trim();
            if (rbOnlyClient.Checked)
            {
                int p = cmbgroup.Items.Count;
                for (int i = 0; i < p; i++)
                {
                    //cmbgroup.SelectedItem.Value = cmbgroup.Items[i].Value;
                    //cmbgroup.SelectedItem.Text = cmbgroup.Items[i].Text;
                    Emailhtmlresult(cmbgroup.Items[i].Value);
                    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                    {

                        string emailbdy = "<table width=\"1200px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td>Settlements Bill For Branch: " + cmbgroup.Items[i].Text + " </td></tr><tr><td>" + EmailHtml + "</td></tr></table>";
                        DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH ", "top 1 *", "branch_id='" + cmbgroup.Items[i].Value + "'");
                        string mailid = dt1.Rows[0]["branch_cpEmail"].ToString();
                        string branchContact = dt1.Rows[0]["branch_head"].ToString();
                        if (mailid != "")
                        {
                            if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                            {

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript20", "ForFilterOff();", true);
                                // ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY12", "DISPLAY('" + SpanText + "');", true);
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript8", "<script language='javascript'>MailsendT();</script>");
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript55", "<script language='javascript'>height();</script>");

                            }
                            else
                            {

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "ForFilterOff();", true);
                                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY26", "DISPLAY('" + SpanText + "');", true);
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script language='javascript'>MailsendF();</script>");
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript22", "<script language='javascript'>height();</script>");
                            }
                        }
                    }
                    else
                    {

                        string emailbdy = "<table width=\"1200px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td>Settlements Bill For Group: " + cmbgroup.Items[i].Text + " </td></tr><tr><td>" + EmailHtml + "</td></tr></table>";
                        DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + cmbgroup.Items[i].Value + "'");
                        string mailid = "";
                        if (dt1.Rows.Count > 0)
                        {
                            mailid = dt1.Rows[0]["gpm_emailID"].ToString().Trim();
                        }
                        string branchContact = cmbgroup.SelectedItem.Value;
                        if (mailid != "")
                        {
                            if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                            {

                                string ccemailid = dt1.Rows[0]["gpm_ccemailID"].ToString().Trim();
                                if (ccemailid.Length > 0)
                                {
                                    oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact);
                                }

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript20", "ForFilterOff();", true);
                                //   ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY12", "DISPLAY('" + SpanText + "');", true);
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript8", "<script language='javascript'>MailsendT();</script>");
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript44", "<script language='javascript'>height();</script>");
                                cmbgroup.SelectedItem.Value = cmbgroup.Items[0].Value;
                                cmbgroup.SelectedItem.Text = cmbgroup.Items[0].Text;

                            }
                            else
                            {

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "ForFilterOff();", true);
                                //   ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY26", "DISPLAY('" + SpanText + "');", true);
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script language='javascript'>MailsendF();</script>");
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript33", "<script language='javascript'>height();</script>");
                                cmbgroup.SelectedItem.Value = cmbgroup.Items[0].Value;
                                cmbgroup.SelectedItem.Text = cmbgroup.Items[0].Text;
                            }
                        }
                    }

                }

            }
            if (rbClientUser.Checked)
            {
                string AllHtml = "";
                string SelctEml = "";
                int p = cmbgroup.Items.Count;
                for (int i = 0; i < p; i++)
                {
                    //cmbgroup.SelectedItem.Value = cmbgroup.Items[i].Value;
                    //cmbgroup.SelectedItem.Text = cmbgroup.Items[i].Text;
                    Emailhtmlresult(cmbgroup.Items[i].Value);
                    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                    {
                        SelctEml = "<table width=\"1200px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td>Settlements Bill For Branch: " + cmbgroup.Items[i].Text + " </td></tr><tr><td>" + EmailHtml + "</td></tr></table>";
                    }
                    else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                    {
                        SelctEml = "<table width=\"1200px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td>Settlements Bill For Group: " + cmbgroup.Items[i].Text + " </td></tr><tr><td>" + EmailHtml + "</td></tr></table>";
                    }
                    AllHtml += SelctEml;
                    SelctEml = "";


                }
                if (SubClients != "")
                {
                    string[] clnt = SubClients.ToString().Split(',');
                    for (int m = 0; m < clnt.Length; m++)
                    {
                        string emailbdy = AllHtml;
                        string contactid = clnt[m].ToString();
                        if (oDBEngine.SendReportSt(emailbdy, contactid, billdate, Subject) == true)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript20", "ForFilterOff();", true);
                            //   ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY12", "DISPLAY('" + SpanText + "');", true);
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript8", "<script language='javascript'>MailsendT();</script>");
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript44", "<script language='javascript'>height();</script>");

                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "ForFilterOff();", true);
                            //   ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY26", "DISPLAY('" + SpanText + "');", true);
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script language='javascript'>MailsendF();</script>");
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript33", "<script language='javascript'>height();</script>");

                        }
                    }

                }

            }

        }


        void Emailhtmlresult(string GroupID)
        {

            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=13 style=\"color:Blue;\">" + ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report For Settlement No. " + txtset.Text.ToString().Trim();
            strHtml1 += "</td></tr></table>";



            strHtml = "<table width=\"1200px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "groupid='" + GroupID + "'";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            strHtml += "<tr  style=\"background-color:#BB694D;color:White;\">";
            //strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" colspan=2><b>Client Name</b></td>";
            strHtml += "<td align=\"center\"><b>Code</b></td>";
            strHtml += "<td align=\"center\"><b>Difference Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Delivery In Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Delivery Out Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Net Obligation</b></td>";
            strHtml += "<td align=\"center\"><b>Tran Charges</b></td>";
            strHtml += "<td align=\"center\"><b>Serv Tax & Cess</b></td>";
            strHtml += "<td align=\"center\"><b>Sec Tran Charges</b></td>";
            strHtml += "<td align=\"center\"><b>Stamp Duty</b></td>";
            strHtml += "<td align=\"center\"><b>Net Receivable (Dr.)</b></td>";
            strHtml += "<td align=\"center\"><b>Net Payable (Cr.)</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" colspan=2>" + dt.Rows[i]["Name"].ToString() + "</td>";
                strHtml += "<td align=\"left\">" + dt.Rows[i]["code"].ToString() + "</td>";

                if (dt.Rows[i]["DiffPL_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["DiffPL_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Dlvvaluein_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Dlvvaluein_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Dlvvalueout_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Dlvvalueout_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["NetAmount_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["NetAmount_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Trancharge"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Trancharge"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["STaxOnTrnCharges"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["STaxOnTrnCharges"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Sttax"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Sttax"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["Stamp_Sum"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["Stamp_Sum"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["NetDr"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["NetDr"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                if (dt.Rows[i]["NetCr"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["NetCr"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                }
                strHtml += "</tr>";
            }

            strHtml += "<tr style=\"background-color: #FFD4AA; color: Black;\">";
            strHtml += "<td style=\"color:Black;\" colspan=3><b>Total</b></td>";
            if (dt.Rows[0]["branchDiffPL_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchDiffPL_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchDlvvaluein_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchDlvvaluein_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchDlvvalueout_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchDlvvalueout_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchNetAmount_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchNetAmount_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchTrancharge"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchTrancharge"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchSTaxOnTrnCharges"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchSTaxOnTrnCharges"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchSttax"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchSttax"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchStamp_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchStamp_Sum"].ToString())) + "</td>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
            }
            if (dt.Rows[0]["branchnet"] != DBNull.Value)
            {
                if (Convert.ToDecimal(dt.Rows[0]["branchnet"]) < 0)
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(dt.Rows[0]["branchnet"].ToString()))) + "</td>";
                    strHtml += "<td>&nbsp;</td>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["branchnet"].ToString())) + "</td>";

                }
            }
            strHtml += "</tr>";
            strHtml += "</table>";

            // DIVdisplayPERIOD.InnerHtml = strHtml1;
            //  display.InnerHtml = strHtml;
            EmailHtml = strHtml;
        }
    }
}