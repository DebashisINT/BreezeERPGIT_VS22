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
    public partial class Reports_frmReport_BrokerageStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        PeriodicalReports periodiaclrep = new PeriodicalReports();
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
                date();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

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
        void date()
        {
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

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


                if (idlist[0] == "ScripsExchange")
                {
                    data = "ScripsExchange~" + str;
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
                if (idlist[0] == "SettlementType")
                {
                    data = "SettlementType~" + str;
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
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }


        void procedure()
        {

            string CommandText = "BokerageChargesStatement";
            string Broker = "";
            string Asset = "";
            string TerminalId = "";
            string ClientId = "";
            string grptype = "";
            string grpid = "";
            if (rdbClientALL.Checked)
            {
                ClientId = "ALL";
            }
            else
            {
                ClientId = HiddenField_Client.Value;
            }

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                grptype = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    grpid = "ALL";
                }
                else
                {
                    grpid = HiddenField_Branch.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                grptype = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    grpid = "ALL";
                }
                else
                {
                    grpid = HiddenField_Group.Value;
                }
            }
            else
            {
                grptype = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    grpid = "ALL";
                }
                else
                {
                    grpid = HiddenField_BranchGroup.Value;
                }
            }

            ds = periodiaclrep.BokerageChargesStatement_All(CommandText, dtfrom.Value.ToString(), dtto.Value.ToString(), Broker, ClientId, Asset, TerminalId,
                 Session["usersegid"].ToString(), Session["LastCompany"].ToString(), grptype, grpid, Session["userbranchHierarchy"].ToString(), "", "", "",
                 "", "", "", "");

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GRPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }
            ViewState["GRPID"] = dtgroupcontactid;
            LastPage = dtgroupcontactid.Rows.Count - 1;
            CurrentPage = 0;
            htmltable(cmbgroup.SelectedItem.Value.ToString(), cmbgroup.SelectedItem.Text.ToString().Trim());

        }
        void htmltable(string grpid, string grpname)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
            string str = null;
            /////////********FOR HEADER BEGIN
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")/////////for mail
            {
                str = str + " [ <b>" + grpname.ToString().Trim() + "</b> ]";
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=11 style=\"color:Blue;\">" + str + "</td></tr></table>";
            /////////********FOR HEADER END


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "GRPID='" + grpid.ToString().Trim() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();


            /////////HTML TABLE HEADER
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Code</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" ><b>CF Brkg</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>CF To</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Brkg %</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sqr Brkg</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sqr To</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Brkg %</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Total Brkg</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Total To</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Brkg %</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>CF Ratio</b></td>";
            strHtml += "</tr>";



            for (int k = 0; k < dt1.Rows.Count; k++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["CLIENTNAME"].ToString().Trim() + "</td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["UCC"].ToString() + "</td>";
                if (dt1.Rows[k]["DLVTotalBrokerage"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVTotalBrokerage"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["DLVMARKETVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  >" + dt1.Rows[k]["DLVMARKETVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["DLVPERCENT"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  >" + dt1.Rows[k]["DLVPERCENT"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["SQRTotalBrokerage"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[k]["SQRTotalBrokerage"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["SQRMARKETVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SQRMARKETVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["SQRPERCENT"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SQRPERCENT"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["TOTALBRKG"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["TOTALBRKG"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["TOTALMKTVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["TOTALMKTVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[k]["TOTALPERCENT"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["TOTALPERCENT"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (dt1.Rows[k]["DLVRATIO"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVRATIO"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "</tr>";
            }
            //////////GRP TOTAL DISPLAY
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=2><b>Group Total :</b></td>";
            if (dt1.Rows[0]["DLVTotalBrokerage_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["DLVTotalBrokerage_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["DLVMARKETVALUE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  >" + dt1.Rows[0]["DLVMARKETVALUE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["DLVPERCENT_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  >" + dt1.Rows[0]["DLVPERCENT_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["SQRTotalBrokerage_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SQRTotalBrokerage_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["SQRMARKETVALUE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["SQRMARKETVALUE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["SQRPERCENT_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["SQRPERCENT_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["TOTALBRKG_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["TOTALBRKG_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["TOTALMKTVALUE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["TOTALMKTVALUE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["TOTALPERCENT_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["TOTALPERCENT_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dt1.Rows[0]["DLVRATIO_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["DLVRATIO_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "</tr>";

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")/////////for groupwise mail
            {
                //////////TOTAL DISPLAY
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=2><b>Total :</b></td>";
                if (dt1.Rows[0]["DLVTotalBrokerage_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["DLVTotalBrokerage_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["DLVMARKETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  >" + dt1.Rows[0]["DLVMARKETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["DLVPERCENT_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  >" + dt1.Rows[0]["DLVPERCENT_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["SQRTotalBrokerage_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SQRTotalBrokerage_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["SQRMARKETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["SQRMARKETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["SQRPERCENT_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["SQRPERCENT_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["TOTALBRKG_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["TOTALBRKG_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["TOTALMKTVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["TOTALMKTVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["TOTALPERCENT_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["TOTALPERCENT_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dt1.Rows[0]["DLVRATIO_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["DLVRATIO_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "</tr>";
            }
            strHtml += "</table>";


            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")/////////for other
            {
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;
            }
        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
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

            CurrentPage = 0;
            htmltable(cmbgroup.SelectedItem.Value.ToString(), cmbgroup.SelectedItem.Text.ToString().Trim());


        }
        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
            htmltable(cmbgroup.SelectedItem.Value.ToString(), cmbgroup.SelectedItem.Text.ToString().Trim());

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GRPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                export_generation();
            }
        }
        void export_generation()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Code", Type.GetType("System.String"));
            dtExport.Columns.Add("CF Brkg", Type.GetType("System.String"));
            dtExport.Columns.Add("CF To", Type.GetType("System.String"));
            dtExport.Columns.Add("Dlv Brkg %", Type.GetType("System.String"));
            dtExport.Columns.Add("Sqr To", Type.GetType("System.String"));
            dtExport.Columns.Add("Sqr Brkg", Type.GetType("System.String"));
            dtExport.Columns.Add("Sqr Brkg %", Type.GetType("System.String"));
            dtExport.Columns.Add("Total Brkg", Type.GetType("System.String"));
            dtExport.Columns.Add("Total To", Type.GetType("System.String"));
            dtExport.Columns.Add("Total Brkg %", Type.GetType("System.String"));
            dtExport.Columns.Add("CF Ratio", Type.GetType("System.String"));



            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row[0] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row[1] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt1 = new DataTable();
                dt1 = viewgrp.ToTable();



                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow row2 = dtExport.NewRow();
                    row2[0] = dt1.Rows[i]["CLIENTNAME"].ToString();
                    row2[1] = dt1.Rows[i]["UCC"].ToString();
                    if (dt1.Rows[i]["DLVTotalBrokerage"] != DBNull.Value)
                        row2[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVTotalBrokerage"].ToString()));
                    if (dt1.Rows[i]["DLVMARKETVALUE"] != DBNull.Value)
                        row2[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVMARKETVALUE"].ToString()));
                    if (dt1.Rows[i]["DLVPERCENT"] != DBNull.Value)
                        row2[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVPERCENT"].ToString()));
                    if (dt1.Rows[i]["SQRTotalBrokerage"] != DBNull.Value)
                        row2[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SQRTotalBrokerage"].ToString()));
                    if (dt1.Rows[i]["SQRMARKETVALUE"] != DBNull.Value)
                        row2[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SQRMARKETVALUE"].ToString()));
                    if (dt1.Rows[i]["SQRPERCENT"] != DBNull.Value)
                        row2[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SQRPERCENT"].ToString()));
                    if (dt1.Rows[i]["TOTALBRKG"] != DBNull.Value)
                        row2[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALBRKG"].ToString()));
                    if (dt1.Rows[i]["TOTALMKTVALUE"] != DBNull.Value)
                        row2[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALMKTVALUE"].ToString()));
                    if (dt1.Rows[i]["TOTALPERCENT"] != DBNull.Value)
                        row2[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPERCENT"].ToString()));
                    if (dt1.Rows[i]["DLVRATIO"] != DBNull.Value)
                        row2[11] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVRATIO"].ToString()));
                    dtExport.Rows.Add(row2);
                }
                /////////GRP SUM DISPLAY
                DataRow row3 = dtExport.NewRow();
                row3[0] = "Group Total :";
                if (dt1.Rows[0]["DLVTotalBrokerage_GRPSUM"] != DBNull.Value)
                    row3[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DLVTotalBrokerage_GRPSUM"].ToString()));
                if (dt1.Rows[0]["DLVMARKETVALUE_GRPSUM"] != DBNull.Value)
                    row3[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DLVMARKETVALUE_GRPSUM"].ToString()));
                if (dt1.Rows[0]["DLVPERCENT_GRPSUM"] != DBNull.Value)
                    row3[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DLVPERCENT_GRPSUM"].ToString()));
                if (dt1.Rows[0]["SQRTotalBrokerage_GRPSUM"] != DBNull.Value)
                    row3[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SQRTotalBrokerage_GRPSUM"].ToString()));
                if (dt1.Rows[0]["SQRMARKETVALUE_GRPSUM"] != DBNull.Value)
                    row3[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SQRMARKETVALUE_GRPSUM"].ToString()));
                if (dt1.Rows[0]["SQRPERCENT_GRPSUM"] != DBNull.Value)
                    row3[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SQRPERCENT_GRPSUM"].ToString()));
                if (dt1.Rows[0]["TOTALBRKG_GRPSUM"] != DBNull.Value)
                    row3[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["TOTALBRKG_GRPSUM"].ToString()));
                if (dt1.Rows[0]["TOTALMKTVALUE_GRPSUM"] != DBNull.Value)
                    row3[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["TOTALMKTVALUE_GRPSUM"].ToString()));
                if (dt1.Rows[0]["TOTALPERCENT_GRPSUM"] != DBNull.Value)
                    row3[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["TOTALPERCENT_GRPSUM"].ToString()));
                if (dt1.Rows[0]["DLVRATIO_GRPSUM"] != DBNull.Value)
                    row3[11] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DLVRATIO_GRPSUM"].ToString()));
                dtExport.Rows.Add(row3);
            }
            /////////SUM DISPLAY
            DataRow row4 = dtExport.NewRow();
            row4[0] = "Total :";
            if (ds.Tables[0].Rows[0]["DLVTotalBrokerage_SUM"] != DBNull.Value)
                row4[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["DLVTotalBrokerage_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["DLVMARKETVALUE_SUM"] != DBNull.Value)
                row4[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["DLVMARKETVALUE_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["DLVPERCENT_SUM"] != DBNull.Value)
                row4[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["DLVPERCENT_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["SQRTotalBrokerage_SUM"] != DBNull.Value)
                row4[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["SQRTotalBrokerage_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["SQRMARKETVALUE_SUM"] != DBNull.Value)
                row4[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["SQRMARKETVALUE_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["SQRPERCENT_SUM"] != DBNull.Value)
                row4[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["SQRPERCENT_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["TOTALBRKG_SUM"] != DBNull.Value)
                row4[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["TOTALBRKG_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["TOTALMKTVALUE_SUM"] != DBNull.Value)
                row4[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["TOTALMKTVALUE_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["TOTALPERCENT_SUM"] != DBNull.Value)
                row4[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["TOTALPERCENT_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["DLVRATIO_SUM"] != DBNull.Value)
                row4[11] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["DLVRATIO_SUM"].ToString()));
            dtExport.Rows.Add(row4);
            ViewState["dtExport"] = dtExport;

            export();
        }

        void export()
        {
            dtExport = (DataTable)ViewState["dtExport"];
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
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());


            DrRowR1[0] = "Brokerage Statement :" + str;

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
                objExcel.ExportToExcelforExcel(dtExport, "Brokerage Statement", "Total :", dtReportHeader, dtReportFooter);
            }
            if (cmbExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Brokerage Statement", "Total :", dtReportHeader, dtReportFooter);
            }
        }

        protected void btnmailsend_Click(object sender, EventArgs e)
        {
            procedure();
            mail();
        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbandforgroup();
                CurrentPage = 0;
            }
        }
        void mail()
        {
            ViewState["billdate"] = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            GROUPDROPDOWN();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")
                {
                    branhgroupemail();
                }
                if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "2")
                {
                    optionemail();
                }
            }

        }
        void GROUPDROPDOWN()
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GRPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }
        }
        void branhgroupemail()
        {
            ds = (DataSet)ViewState["dataset"];
            ViewState["GRPmail"] = "mail";
            ViewState["mailsendresult"] = "no";
            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                htmltable(cmbgroup.Items[j].Value.ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim());
                if (ViewState["GRPmail"].ToString().Trim() == "mail")
                {
                    ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                }
                else
                {
                    ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                }
                DataView viewclient = new DataView();
                viewclient = ds.Tables[0].DefaultView;
                viewclient.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewclient.ToTable();
                if (oDBEngine.SendReportBr(ViewState["GRPmail"].ToString().Trim(), dt.Rows[0]["EMAIL"].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Brokerage Statement [" + ViewState["billdate"].ToString().Trim() + "]", cmbgroup.Items[j].Value) == true)
                {
                    if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                    {
                        ViewState["mailsendresult"] = "someclienterror";
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "success";
                    }
                }
                else
                {

                    ViewState["mailsendresult"] = "errorsuccess";
                }
                ViewState["GRPmail"] = "mail";
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
            }


        }
        void optionemail()
        {
            ds = (DataSet)ViewState["dataset"];
            ViewState["GRPmail"] = "mail";
            ViewState["Usermail"] = "UserMail";
            ViewState["mailsendresult"] = "no";
            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                htmltable(cmbgroup.Items[j].Value.ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim());
                if (ViewState["GRPmail"].ToString().Trim() == "mail")
                {
                    ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                }
                else
                {
                    ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                }
            }
            ViewState["Usermail"] = ViewState["GRPmail"].ToString().Trim();

            string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
            int kk = clnt.Length;
            for (int i = 0; i < clnt.Length; i++)
            {
                if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Brokerage Statement [" + ViewState["billdate"].ToString().Trim() + "]") == true)
                {
                    if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                    {
                        ViewState["mailsendresult"] = "someclienterror";
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "success";
                    }
                }
                else
                {

                    ViewState["mailsendresult"] = "errorsuccess";
                }
            }


            if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
            }

        }
    }

}