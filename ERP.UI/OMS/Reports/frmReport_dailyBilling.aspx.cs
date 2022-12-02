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
    public partial class Reports_frmReport_dailyBilling : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        static string Client;
        static string Branch;
        static string Group;
        static string Broker;
        string data;
        DataSet ds = new DataSet();


        decimal TPremium = 0;
        decimal TMtm = 0;
        decimal TfinSett = 0;
        decimal TexcasnfinSett = 0;
        decimal TCharges = 0;
        decimal TSerTax = 0;
        decimal TnetOblng = 0;
        decimal TOpeningBalance = 0;
        decimal TNetAdjment = 0;
        decimal TClosingBalance = 0;
        decimal TCashMrgnDeposit = 0;
        decimal TEffectiveDeposit = 0;
        decimal TApplMargn = 0;
        decimal TExcessShortage = 0;
        decimal TExposure = 0;
        decimal TColeteralVal = 0;
        //for email
        //--for sending email
        static string SubClients = "";
        DataTable dtEmail = new DataTable();
        string footerTxt = "";
        string EmailHtmal = "";


        string[] idlist;
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;

        DataTable Distinctgroup = new DataTable();
        DataTable dt = new DataTable();
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
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            if (!IsPostBack)
            {
                date();
                Client = null;
                Branch = null;
                Group = null;

                //_____For performing operation without refreshing page___//

                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                //___________-end here___//
            }


            rbOnlyClient.Attributes.Add("OnClick", "SelectUserClient('Client')");
            rbClientUser.Attributes.Add("OnClick", "SelectUserClient('User')");
            //___________-end here___//
            //for sending email
            txtSelectID.Attributes.Add("onkeyup", "callAjax1(this,'GetMailId',event)");
            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
            FillCombo();

        }

        void date()
        {
            dtFor.EditFormatString = oconverter.GetDateFormat("Date");
            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            dtFor.Value = Convert.ToDateTime(idlist[2]);

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
            string[] cl = idlist[1].Split(',');
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0] != "Clients" && idlist[0] != "Broker")
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
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];

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
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
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
                Client = str;
                data = "Clients~" + str1;
            }
            else if (idlist[0] == "Broker")
            {
                Broker = str;
                data = "Broker~" + str;
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
                if (Group == null)
                {
                    BindGroup();
                }
            }
        }
        private void fn_branch()
        {
            if (rdbranchAll.Checked == true)
            {
                DataTable dtbranch = oDBEngine.GetDataTable("tbl_master_branch", "branch_id", "branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                if (dtbranch.Rows.Count > 0)
                {
                    for (int i = 0; i < dtbranch.Rows.Count; i++)
                    {
                        if (Branch == null)
                            Branch = dtbranch.Rows[i][0].ToString();
                        else
                            Branch += "," + dtbranch.Rows[i][0].ToString();
                    }
                }
            }
        }
        void fn_Client()
        {
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
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
                                    if (Client == null)
                                        Client = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Client += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
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
                                if (Client == null)
                                    Client = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Client += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
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
                                if (Client == null)
                                    Client = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Client += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
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
                                if (Client == null)
                                    Client = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Client += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }

        }
        void fn_Broker()
        {
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'BO%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Broker == null)
                                        Broker = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Broker += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'BO%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Broker == null)
                                    Broker = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Broker += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'BO%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Broker == null)
                                    Broker = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Broker += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'BO%' and cnt_branchid in(" + Branch + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Broker == null)
                                    Broker = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Broker += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }

        }
        void procedure()
        {
            fn_branch();
            if (ddlviewby.SelectedItem.Value == "1")
            {
                fn_Client();
            }
            else
            {
                fn_Broker();
            }


            string CalType = null;
            if (ddlCalType.SelectedValue == "1")
                CalType = "E";
            else
                CalType = "P";

            string grpbranch = "";
            string ClientsID = "";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                grpbranch = "BRANCH";
            }
            else
            {
                grpbranch = ddlgrouptype.SelectedItem.Text.ToString().Trim();
            }
            if (ddlviewby.SelectedItem.Value == "1")
            {
                ClientsID = Client;
            }
            else
            {
                ClientsID = Broker;
            }

            ds = dailyrep.Report_ComDailyBills(Session["usersegid"].ToString(), Session["LastCompany"].ToString(), dtFor.Value.ToString(),
                HttpContext.Current.Session["LastFinYear"].ToString(), grpbranch, ClientsID, Branch, CalType, HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            ViewState["bill"] = ds;
            ViewState["header"] = Convert.ToDateTime(dtFor.Value).ToString("dd-MMM-yyyy");

        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["bill"];
            DataView viewgroup = new DataView(ds.Tables[0]);

            Distinctgroup = viewgroup.ToTable(true, new string[] { "GrpId", "GrpName" });
            foreach (DataRow dr in Distinctgroup.Rows)
            {
                if (Distinctgroup.Rows.Count > 0)
                {
                    cmbgroupPager.Items.Clear();
                    cmbgroupPager.DataSource = Distinctgroup;
                    cmbgroupPager.DataValueField = "GrpId";
                    cmbgroupPager.DataTextField = "GrpName";
                    cmbgroupPager.DataBind();
                }
            }

        }
        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {


            int curentIndex = cmbgroupPager.SelectedIndex;
            int totalNo = cmbgroupPager.Items.Count;
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
                    pageindex = int.Parse(TotalGROUP.Value);
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
            cmbgroupPager.SelectedIndex = curentIndex;

            /////////////////////////////DISPLAY DATE/////////////////////////////////////
            string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);
            ///////////////////////////////END//////////////////////////////////////////
            ScriptManager.RegisterStartupScript(this, this.GetType(), "line", "line();", true);
            othercalculculation();
            gridbind();
        }
        protected void cmbgroupPager_SelectedIndexChanged(object sender, EventArgs e)
        {
            othercalculculation();
            gridbind();
        }

        void gridbind()
        {
            grdBilling.DataSource = dt;
            grdBilling.DataBind();
            footer();
            /////////////////////////////DISPLAY DATE/////////////////////////////////////

            string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

            ///////////////////////////////END//////////////////////////////////////////
            ScriptManager.RegisterStartupScript(this, this.GetType(), "line", "line();", true);
        }
        void othercalculculation()
        {
            TPremium = decimal.Zero;
            TMtm = decimal.Zero;
            TfinSett = decimal.Zero;
            TexcasnfinSett = decimal.Zero;
            TCharges = decimal.Zero;
            TSerTax = decimal.Zero;
            TnetOblng = decimal.Zero;
            TOpeningBalance = decimal.Zero;
            TNetAdjment = decimal.Zero;
            TClosingBalance = decimal.Zero;
            TCashMrgnDeposit = decimal.Zero;
            TEffectiveDeposit = decimal.Zero;
            TApplMargn = decimal.Zero;
            TExcessShortage = decimal.Zero;
            TExposure = decimal.Zero;
            TColeteralVal = decimal.Zero;

            ds = (DataSet)ViewState["bill"];

            dt = new DataTable();
            DataView viewgroup = new DataView();
            viewgroup = ds.Tables[0].DefaultView;
            viewgroup.RowFilter = "GrpId=" + cmbgroupPager.SelectedItem.Value;
            dt = viewgroup.ToTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {


                if (dt.Rows[i]["Premium"] != DBNull.Value || dt.Rows[i]["Premium"].ToString() != "0.00")
                {
                    dt.Rows[i]["Premium"] = dt.Rows[i]["Premium"];
                    TPremium += Convert.ToDecimal(dt.Rows[i]["Premium1"].ToString());
                }
                if (dt.Rows[i]["MTM"] != DBNull.Value)
                {
                    dt.Rows[i]["MTM"] = dt.Rows[i]["MTM"];
                    TMtm += Convert.ToDecimal(dt.Rows[i]["MTM1"].ToString());
                }

                if (dt.Rows[i]["FutureSettlement"] != DBNull.Value)
                {
                    dt.Rows[i]["FutureSettlement"] = dt.Rows[i]["FutureSettlement"];
                    TfinSett += Convert.ToDecimal(dt.Rows[i]["FutureSettlement1"].ToString());
                }


                if (dt.Rows[i]["Charges"] != DBNull.Value)
                {
                    dt.Rows[i]["Charges"] = dt.Rows[i]["Charges"];
                    TCharges += Convert.ToDecimal(dt.Rows[i]["Charges1"].ToString());
                }

                if (dt.Rows[i]["ServTax"] != DBNull.Value)
                {
                    dt.Rows[i]["ServTax"] = dt.Rows[i]["ServTax"];
                    TSerTax += Convert.ToDecimal(dt.Rows[i]["ServTax1"].ToString());
                }

                if (dt.Rows[i]["NetObligation"] != DBNull.Value)
                {
                    dt.Rows[i]["NetObligation"] = dt.Rows[i]["NetObligation"];
                    TnetOblng += Convert.ToDecimal(dt.Rows[i]["NetObligation1"].ToString());
                }

                if (dt.Rows[i]["ApplMargin"] != DBNull.Value)
                {
                    dt.Rows[i]["ApplMargin"] = dt.Rows[i]["ApplMargin"];
                    TApplMargn += Convert.ToDecimal(dt.Rows[i]["ApplMargin1"].ToString());
                }

                if (dt.Rows[i]["Exposure"] != DBNull.Value)
                {
                    dt.Rows[i]["Exposure"] = dt.Rows[i]["Exposure"];
                    TExposure += Convert.ToDecimal(dt.Rows[i]["Exposure1"].ToString());
                }

                if (dt.Rows[i]["NetAdj"] != DBNull.Value)
                {
                    dt.Rows[i]["NetAdj"] = dt.Rows[i]["NetAdj"];
                    TNetAdjment += Convert.ToDecimal(dt.Rows[i]["NetAdj1"].ToString());
                }
                else
                {
                    dt.Rows[i]["NetAdj"] = DBNull.Value;
                }

                if (dt.Rows[i]["CashMarnDeposit"] != DBNull.Value)
                {
                    dt.Rows[i]["CashMarnDeposit"] = dt.Rows[i]["CashMarnDeposit"];
                    TCashMrgnDeposit += Convert.ToDecimal(dt.Rows[i]["CashMarnDeposit1"].ToString());
                }
                else
                {
                    dt.Rows[i]["CashMarnDeposit"] = DBNull.Value;
                }

                if (dt.Rows[i]["OpeningBal"] != DBNull.Value)
                {
                    dt.Rows[i]["OpeningBal"] = dt.Rows[i]["OpeningBal"];
                    TOpeningBalance += Convert.ToDecimal(dt.Rows[i]["OpeningBal1"].ToString());

                }
                else
                {
                    ds.Tables[0].Rows[i]["OpeningBal"] = DBNull.Value;
                }

                if (dt.Rows[i]["ClosingBal"] != DBNull.Value)
                {
                    dt.Rows[i]["ClosingBal"] = dt.Rows[i]["ClosingBal"];
                    TClosingBalance += Convert.ToDecimal(dt.Rows[i]["ClosingBal1"].ToString());
                }
                else
                {
                    dt.Rows[i]["ClosingBal"] = DBNull.Value;
                }


                if (dt.Rows[i]["EffecTiveDeposit"] != DBNull.Value)
                {
                    dt.Rows[i]["EffecTiveDeposit"] = dt.Rows[i]["EffecTiveDeposit"];
                    TEffectiveDeposit += Convert.ToDecimal(dt.Rows[i]["EffecTiveDeposit1"].ToString());
                }
                else
                {
                    ds.Tables[0].Rows[i]["EffecTiveDeposit"] = DBNull.Value;
                }

                if (dt.Rows[i]["ExcessShortage"] != DBNull.Value)
                {
                    dt.Rows[i]["ExcessShortage"] = dt.Rows[i]["ExcessShortage"];
                    TExcessShortage += Convert.ToDecimal(dt.Rows[i]["ExcessShortage"].ToString());
                }
                else
                {
                    dt.Rows[i]["ExcessShortage"] = DBNull.Value;
                }

                if (dt.Rows[i]["ExcessShortage1"] != DBNull.Value)
                {
                    dt.Rows[i]["ExcessShortage1"] = dt.Rows[i]["ExcessShortage1"];
                }
                else
                {
                    dt.Rows[i]["ExcessShortage1"] = DBNull.Value;
                }

                if (dt.Rows[i]["ColeteralVal"] != DBNull.Value)
                {
                    dt.Rows[i]["ColeteralVal"] = dt.Rows[i]["ColeteralVal"];
                    TColeteralVal += Convert.ToDecimal(dt.Rows[i]["ColeteralVal1"].ToString());
                }
                else
                {
                    dt.Rows[i]["ColeteralVal"] = DBNull.Value;
                }

            }
            dtEmail = dt;


        }


        void footer()
        {

            grdBilling.FooterRow.Cells[0].Text = "Total";

            if (TOpeningBalance == 0)
                grdBilling.FooterRow.Cells[2].Text = "";
            else
                grdBilling.FooterRow.Cells[2].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TOpeningBalance));

            if (TPremium == 0)
                grdBilling.FooterRow.Cells[3].Text = "";
            else
                grdBilling.FooterRow.Cells[3].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TPremium));

            if (TMtm == 0)
                grdBilling.FooterRow.Cells[4].Text = "";
            else
                grdBilling.FooterRow.Cells[4].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TMtm));

            if (TfinSett == 0)
                grdBilling.FooterRow.Cells[5].Text = "";
            else
                grdBilling.FooterRow.Cells[5].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TfinSett));


            if (TCharges == 0)
                grdBilling.FooterRow.Cells[6].Text = "";
            else
                grdBilling.FooterRow.Cells[6].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TCharges));

            if (TSerTax == 0)
                grdBilling.FooterRow.Cells[7].Text = "";
            else
                grdBilling.FooterRow.Cells[7].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TSerTax));

            if (TnetOblng == 0)
                grdBilling.FooterRow.Cells[8].Text = "";
            else
                grdBilling.FooterRow.Cells[8].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TnetOblng));

            if (TNetAdjment == 0)
                grdBilling.FooterRow.Cells[9].Text = "";
            else
                grdBilling.FooterRow.Cells[9].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TNetAdjment));

            if (TClosingBalance == 0)
                grdBilling.FooterRow.Cells[10].Text = "";
            else
                grdBilling.FooterRow.Cells[10].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TClosingBalance));

            if (TCashMrgnDeposit == 0)
                grdBilling.FooterRow.Cells[11].Text = "";
            else
                grdBilling.FooterRow.Cells[11].Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TCashMrgnDeposit));

            if (TColeteralVal == 0)
                grdBilling.FooterRow.Cells[12].Text = "";
            else
                grdBilling.FooterRow.Cells[12].Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TColeteralVal));

            if (TEffectiveDeposit == 0)
                grdBilling.FooterRow.Cells[13].Text = "";
            else
                grdBilling.FooterRow.Cells[13].Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TEffectiveDeposit));


            if (TApplMargn == 0)
                grdBilling.FooterRow.Cells[14].Text = "";
            else
                grdBilling.FooterRow.Cells[14].Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TApplMargn));


            if (TExcessShortage == 0)
                grdBilling.FooterRow.Cells[15].Text = "";
            else
                grdBilling.FooterRow.Cells[15].Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TExcessShortage));


            if (TExposure == 0)
                grdBilling.FooterRow.Cells[16].Text = "";
            else
                grdBilling.FooterRow.Cells[16].Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TExposure));

            grdBilling.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            grdBilling.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            grdBilling.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;

            grdBilling.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[2].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[6].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[7].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[8].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[9].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[10].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[11].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[12].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[13].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[14].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[15].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[16].ForeColor = System.Drawing.Color.White;

            grdBilling.FooterRow.Cells[0].Font.Bold = true;
            grdBilling.FooterRow.Cells[2].Font.Bold = true;
            grdBilling.FooterRow.Cells[3].Font.Bold = true;
            grdBilling.FooterRow.Cells[4].Font.Bold = true;
            grdBilling.FooterRow.Cells[5].Font.Bold = true;
            grdBilling.FooterRow.Cells[6].Font.Bold = true;
            grdBilling.FooterRow.Cells[7].Font.Bold = true;
            grdBilling.FooterRow.Cells[8].Font.Bold = true;
            grdBilling.FooterRow.Cells[9].Font.Bold = true;
            grdBilling.FooterRow.Cells[10].Font.Bold = true;
            grdBilling.FooterRow.Cells[11].Font.Bold = true;
            grdBilling.FooterRow.Cells[12].Font.Bold = true;
            grdBilling.FooterRow.Cells[13].Font.Bold = true;
            grdBilling.FooterRow.Cells[14].Font.Bold = true;
            grdBilling.FooterRow.Cells[15].Font.Bold = true;
            grdBilling.FooterRow.Cells[16].Font.Bold = true;

            grdBilling.FooterRow.Cells[2].Wrap = false;
            grdBilling.FooterRow.Cells[3].Wrap = false;
            grdBilling.FooterRow.Cells[4].Wrap = false;
            grdBilling.FooterRow.Cells[5].Wrap = false;
            grdBilling.FooterRow.Cells[6].Wrap = false;
            grdBilling.FooterRow.Cells[7].Wrap = false;
            grdBilling.FooterRow.Cells[8].Wrap = false;
            grdBilling.FooterRow.Cells[9].Wrap = false;
            grdBilling.FooterRow.Cells[10].Wrap = false;
            grdBilling.FooterRow.Cells[11].Wrap = false;
            grdBilling.FooterRow.Cells[12].Wrap = false;
            grdBilling.FooterRow.Cells[13].Wrap = false;
            grdBilling.FooterRow.Cells[14].Wrap = false;
            grdBilling.FooterRow.Cells[15].Wrap = false;
            grdBilling.FooterRow.Cells[16].Wrap = false;
            footerTxt += "<tr style=\"background-color: #FFD4AA; color: Black;\">";
            for (int i = 0; i < 17; i++)
            {

                footerTxt += "<td>&nbsp;" + grdBilling.FooterRow.Cells[i].Text.ToString() + "</td>";

            }
            footerTxt += "<tr>";


        }
        protected void grdBilling_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ds = (DataSet)ViewState["bill"];
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + ds.Tables[0].Rows.Count + "'" + ")");
            }
        }

        void export()
        {

            ds = (DataSet)ViewState["bill"];
            DataTable dtExport = new DataTable();


            dtExport.Columns.Add("CLIENTID", Type.GetType("System.String"));
            dtExport.Columns.Add("GrpId", Type.GetType("System.String"));
            dtExport.Columns.Add("Account Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Account Code", Type.GetType("System.String"));
            dtExport.Columns.Add("Opening Balance", Type.GetType("System.String"));
            dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            dtExport.Columns.Add("MTM", Type.GetType("System.String"));
            dtExport.Columns.Add("Fin Sett", Type.GetType("System.String"));
            //dtExport.Columns.Add("ASN/EXC Fin Sett", Type.GetType("System.String"));
            dtExport.Columns.Add("Charges", Type.GetType("System.String"));
            dtExport.Columns.Add("Serv.Tax", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Oblgtn", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Adjustment", Type.GetType("System.String"));
            dtExport.Columns.Add("Closing Balance", Type.GetType("System.String"));
            dtExport.Columns.Add("Cash Margin Deposit", Type.GetType("System.String"));
            dtExport.Columns.Add("Collateral Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Effective Deposit", Type.GetType("System.String"));
            dtExport.Columns.Add("Appl Margin", Type.GetType("System.String"));
            dtExport.Columns.Add("Excess/Shortage(-)", Type.GetType("System.String"));
            dtExport.Columns.Add("Exposure", Type.GetType("System.String"));

            int ddlcount = cmbgroupPager.Items.Count;
            dtExport.Clear();

            for (int i = 0; i < ddlcount; i++)
            {


                DataView viewgroup = new DataView();
                viewgroup = ds.Tables[0].DefaultView;
                string valItem = cmbgroupPager.Items[i].Value;
                viewgroup.RowFilter = "GrpId='" + valItem + "'";
                dt = viewgroup.ToTable();

                DataRow row4 = dtExport.NewRow();
                dtExport.Rows.Add(row4);

                DataRow row3 = dtExport.NewRow();
                row3["Account Name"] = dt.Rows[0]["GrpName"].ToString();
                dtExport.Rows.Add(row3);

                foreach (DataRow dr1 in dt.Rows)
                {

                    DataRow row2 = dtExport.NewRow();
                    row2["Account Name"] = dr1["clientname"];
                    row2["Account Code"] = dr1["ucc"];

                    if (dr1["OpeningBal"] != DBNull.Value)
                    {
                        row2["Opening Balance"] = dr1["OpeningBal"];
                    }

                    if (dr1["Premium"] != DBNull.Value)
                    {
                        row2["Premium"] = dr1["Premium"];
                    }

                    if (dr1["MTM"] != DBNull.Value)
                    {
                        row2["MTM"] = dr1["MTM"];
                    }

                    if (dr1["FutureSettlement"] != DBNull.Value)
                    {
                        row2["Fin Sett"] = dr1["FutureSettlement"];
                    }

                    //if (dr1["ASNEXCFINSET"] != DBNull.Value)
                    //{
                    //    row2["ASN/EXC Fin Sett"] = dr1["ASNEXCFINSET"];
                    //}

                    if (dr1["Charges"] != DBNull.Value)
                    {
                        row2["Charges"] = dr1["Charges"];
                    }

                    if (dr1["ServTax"] != DBNull.Value)
                    {
                        row2["Serv.Tax"] = dr1["ServTax"];
                    }

                    if (dr1["NetObligation"] != DBNull.Value)
                    {
                        row2["Net Oblgtn"] = dr1["NetObligation"];
                    }

                    if (dr1["NetAdj"] != DBNull.Value)
                    {
                        row2["Net Adjustment"] = dr1["NetAdj"];
                    }

                    if (dr1["ClosingBal"] != DBNull.Value)
                    {
                        row2["Closing Balance"] = dr1["ClosingBal"];
                    }

                    if (dr1["CashMarnDeposit"] != DBNull.Value)
                    {
                        row2["Cash Margin Deposit"] = dr1["CashMarnDeposit"];
                    }

                    if (dr1["ColeteralVal"] != DBNull.Value)
                    {
                        row2["Collateral Value"] = dr1["ColeteralVal"];
                    }

                    if (dr1["EffecTiveDeposit"] != DBNull.Value)
                    {
                        row2["Effective Deposit"] = dr1["EffecTiveDeposit"];
                    }

                    if (dr1["ApplMargin"] != DBNull.Value)
                    {
                        row2["Appl Margin"] = dr1["ApplMargin"];
                    }

                    if (dr1["ExcessShortage"] != DBNull.Value)
                    {
                        row2["Excess/Shortage(-)"] = dr1["ExcessShortage"];
                    }

                    if (dr1["Exposure"] != DBNull.Value)
                    {
                        row2["Exposure"] = dr1["Exposure"].ToString();
                    }

                    dtExport.Rows.Add(row2);
                }
                ///////////////////////TOTAL DISPLAY///////////////////////
                DataRow row = dtExport.NewRow();
                row["Account Name"] = "TOTAL";

                if (dt.Rows[0]["Branch_OpeningBal"] != DBNull.Value)
                {
                    row["Opening Balance"] = dt.Rows[0]["Branch_OpeningBal"].ToString();

                }

                if (dt.Rows[0]["Branch_Premium"] != DBNull.Value)
                {
                    row["Premium"] = dt.Rows[0]["Branch_Premium"].ToString();
                }

                if (dt.Rows[0]["Branch_MTM"] != DBNull.Value)
                {
                    row["MTM"] = dt.Rows[0]["Branch_MTM"].ToString();
                }

                if (dt.Rows[0]["Branch_FutureSettlement"] != DBNull.Value)
                {
                    row["Fin Sett"] = dt.Rows[0]["Branch_FutureSettlement"].ToString();
                }

                //if (dt.Rows[0]["Branch_ASNEXCFINSET"] != DBNull.Value)
                //{
                //    row["ASN/EXC Fin Sett"] = dt.Rows[0]["Branch_ASNEXCFINSET"].ToString();
                //}

                if (dt.Rows[0]["Branch_Charges"] != DBNull.Value)
                {
                    row["Charges"] = dt.Rows[0]["Branch_Charges"].ToString();
                }

                if (dt.Rows[0]["Branch_ServTax"] != DBNull.Value)
                {
                    row["Serv.Tax"] = dt.Rows[0]["Branch_ServTax"].ToString();
                }

                if (dt.Rows[0]["Branch_NetObligation"] != DBNull.Value)
                {
                    row["Net Oblgtn"] = dt.Rows[0]["Branch_NetObligation"].ToString();
                }

                if (dt.Rows[0]["Branch_NetAdj"] != DBNull.Value)
                {
                    row["Net Adjustment"] = dt.Rows[0]["Branch_NetAdj"].ToString();
                }

                if (dt.Rows[0]["Branch_ClosingBal"] != DBNull.Value)
                {
                    row["Closing Balance"] = dt.Rows[0]["Branch_ClosingBal"].ToString();
                }

                if (dt.Rows[0]["Branch_CashMarnDeposit"] != DBNull.Value)
                {
                    row["Cash Margin Deposit"] = dt.Rows[0]["Branch_CashMarnDeposit"].ToString();
                }

                if (dt.Rows[0]["Branch_ColeteralVal"] != DBNull.Value)
                {
                    row["Collateral Value"] = dt.Rows[0]["Branch_ColeteralVal"].ToString();
                }

                if (dt.Rows[0]["Branch_EffecTiveDeposit"] != DBNull.Value)
                {
                    row["Effective Deposit"] = dt.Rows[0]["Branch_EffecTiveDeposit"].ToString();
                }

                if (dt.Rows[0]["Branch_ApplMargin"] != DBNull.Value)
                {
                    row["Appl Margin"] = dt.Rows[0]["Branch_ApplMargin"].ToString();
                }

                if (dt.Rows[0]["Branch_ExcessShortage"] != DBNull.Value)
                {
                    row["Excess/Shortage(-)"] = dt.Rows[0]["Branch_ExcessShortage"].ToString();
                }

                if (dt.Rows[0]["Branch_Exposure"] != DBNull.Value)
                {
                    row["Exposure"] = dt.Rows[0]["Branch_Exposure"].ToString();
                }



                dtExport.Rows.Add(row);


            }
            DataRow row5 = dtExport.NewRow();

            row5["Account Name"] = "GRAND TOTAL";
            row5["Opening Balance"] = ds.Tables[0].Rows[0]["Grand_OpeningBal"];
            row5["Premium"] = ds.Tables[0].Rows[0]["Grand_Premium"];
            row5["MTM"] = ds.Tables[0].Rows[0]["Grand_MTM"];
            row5["Fin Sett"] = ds.Tables[0].Rows[0]["Grand_FutureSettlement"];
            //row5["ASN/EXC Fin Sett"] = ds.Tables[0].Rows[0]["Grand_ASNEXCFINSET"];
            row5["Charges"] = ds.Tables[0].Rows[0]["Grand_Charges"];
            row5["Serv.Tax"] = ds.Tables[0].Rows[0]["Grand_ServTax"];
            row5["Net Oblgtn"] = ds.Tables[0].Rows[0]["Grand_NetObligation"];
            row5["Net Adjustment"] = ds.Tables[0].Rows[0]["Grand_NetAdj"];
            row5["Closing Balance"] = ds.Tables[0].Rows[0]["Grand_ClosingBal"];
            row5["Cash Margin Deposit"] = ds.Tables[0].Rows[0]["Grand_CashMarnDeposit"];
            row5["Collateral Value"] = ds.Tables[0].Rows[0]["Grand_ColeteralVal"];
            row5["Effective Deposit"] = ds.Tables[0].Rows[0]["Grand_EffecTiveDeposit"];
            row5["Appl Margin"] = ds.Tables[0].Rows[0]["Grand_ApplMargin"];
            row5["Excess/Shortage(-)"] = ds.Tables[0].Rows[0]["Grand_ExcessShortage"];
            row5["Exposure"] = ds.Tables[0].Rows[0]["Grand_Exposure"];

            dtExport.Rows.Add(row5);


            //////////////////////////////////SEGMENTNAME FETCH///////////////////////////////////////

            string[,] DTSEGMENTNAME = oDBEngine.GetFieldValue("tbl_master_companyExchange", "(select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId as Comp", "tbl_master_companyExchange.exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'", 1);
            if (DTSEGMENTNAME[0, 0] != "n")
            {
                idlist = DTSEGMENTNAME[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements

            }

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "Obligation Summary Report [" + idlist[0] + "]" + ' ' + "For :" + ' ' + oconverter.ArrangeDate2(dtFor.Value.ToString());
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
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

            dtExport.Columns.Remove("CLIENTID");
            dtExport.Columns.Remove("GrpId");

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")////screen
            {
                if (ddlExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, " F&O Obligation Summary Report", "TOTAL", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    print();
                }
            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "2")////excel
            {
                objExcel.ExportToExcelforExcel(dtExport, "F&O Obligation Summary Report", "Client Net", dtReportHeader, dtReportFooter);
            }


        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            export();
        }
        protected void NavigationLink_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPage.Value) + 1;
                    break;
                case "Prev":
                    pageindex = int.Parse(CurrentPage.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPages.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }

            gridbind();
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

                clsdrp.AddDataToDropDownList(r, cmbsearch);

            }

        }
        protected void btn_show_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["bill"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Filter", "Filter();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript5", "alert('No Record Found !!')", true);
            }
            else
            {
                ddlbandforgroup();
                othercalculculation();
                gridbind();
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript16", "SendmailFilter();", true);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string disptbl = "";
            string NetHtml = "";
            string mailid = "";
            string branchContact = "";
            string branchName = "";
            string emailbdy = "";
            string contactid = "";
            string billdate = oconverter.ArrangeDate2(dtFor.Value.ToString());
            string Subject = "";
            int p = cmbgroupPager.Items.Count;
            for (int m = 0; m < p; m++)
            {
                cmbgroupPager.SelectedItem.Value = cmbgroupPager.Items[m].Value;
                cmbgroupPager.SelectedItem.Text = cmbgroupPager.Items[m].Text;

                othercalculculation();
                footer();
                disptbl = "<table width=\"1350px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"center\">Account Name</td><td align=\"center\">Account Code</td><td align=\"center\">Opening Balance</td><td align=\"center\">Premium</td><td align=\"center\">MTM</td><td align=\"center\">Fin Sett</td><td align=\"center\">Charges</td><td align=\"center\">Serv.Tax</td><td align=\"center\">Net Oblgtn</td><td align=\"center\">Net Adjustment</td><td align=\"center\">Closing Balance</td><td align=\"center\">Cash Margin Deposit</td><td align=\"center\">Collateral Value</td><td align=\"center\">Effective Deposit</td><td align=\"center\">Appl Margin</td><td align=\"center\">Excess/Shortage(-)</td><td align=\"center\">Exposure</td></tr>";
                for (int i = 0; i < dtEmail.Rows.Count; i++)
                {
                    disptbl += "<tr><td>&nbsp;" + dtEmail.Rows[i]["clientname"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ucc"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["OpeningBal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Premium"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["MTM"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["FutureSettlement"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Charges"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ServTax"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["NetObligation"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["NetAdj"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ClosingBal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["CashMarnDeposit"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ColeteralVal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["EffecTiveDeposit"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ApplMargin"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ExcessShortage"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Exposure"] + "</td></tr>";
                }
                disptbl += footerTxt;
                disptbl += "</table>";
                emailbdy = disptbl;
                if (rbOnlyClient.Checked)
                {
                    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                    {
                        emailbdy = disptbl;
                        DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH ", "top 1 *", "branch_id='" + cmbgroupPager.Items[m].Value + "'");
                        mailid = dt1.Rows[0]["branch_cpEmail"].ToString();
                        branchContact = dt1.Rows[0]["branch_head"].ToString();
                        branchName = dt1.Rows[0]["branch_description"].ToString();
                        Subject = "Daily Bills For  " + billdate + "  Branch Name: " + cmbgroupPager.Items[m].Text;
                        if (mailid != "")
                        {
                            if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                            {

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript20", "SendmailFilter();", true);
                                cmbgroupPager.SelectedItem.Value = cmbgroupPager.Items[0].Value;
                                cmbgroupPager.SelectedItem.Text = cmbgroupPager.Items[0].Text;
                                string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY12", "DISPLAY('" + SpanText + "');", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript14", "MailsendT();", true);

                            }
                            else
                            {

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "SendmailFilter();", true);
                                cmbgroupPager.SelectedItem.Value = cmbgroupPager.Items[0].Value;
                                cmbgroupPager.SelectedItem.Text = cmbgroupPager.Items[0].Text;
                                string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY26", "DISPLAY('" + SpanText + "');", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "MailsendF();", true);
                            }
                        }
                    }
                    else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                    {
                        emailbdy = disptbl;
                        DataTable dt2 = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + cmbgroupPager.Items[m].Value + "'");
                        if (dt2.Rows.Count > 0)
                        {
                            mailid = dt2.Rows[0]["gpm_emailID"].ToString().Trim();
                        }
                        branchContact = cmbgroupPager.Items[m].Value;
                        branchName = cmbgroupPager.Items[0].Text;
                        Subject = "Daily Bills For  " + billdate + "  Group Name: " + branchName;
                        if (mailid != "")
                        {
                            if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                            {

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript20", "SendmailFilter();", true);
                                cmbgroupPager.SelectedItem.Value = cmbgroupPager.Items[0].Value;
                                cmbgroupPager.SelectedItem.Text = cmbgroupPager.Items[0].Text;
                                string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY12", "DISPLAY('" + SpanText + "');", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript14", "MailsendT();", true);

                            }
                            else
                            {

                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "SendmailFilter();", true);
                                cmbgroupPager.SelectedItem.Value = cmbgroupPager.Items[0].Value;
                                cmbgroupPager.SelectedItem.Text = cmbgroupPager.Items[0].Text;
                                string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY26", "DISPLAY('" + SpanText + "');", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "MailsendF();", true);
                            }
                        }

                    }

                }
                else
                {
                    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                    {
                        branchName = "Branch Name: " + cmbgroupPager.Items[m].Text;
                    }
                    else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                    {
                        branchName = "Group Name: " + cmbgroupPager.Items[m].Text;
                    }
                    NetHtml += "<tr><td></td></tr><tr style=\"background-color: #FFD4AA; color: Black;\"><td> " + branchName + "</td></tr>" + "<tr><td>" + emailbdy + "</td></tr>";
                    emailbdy = "";
                }
                disptbl = "";
                footerTxt = "";

            }


            if (SubClients != "")
            {

                string[] clnt = SubClients.ToString().Split(',');
                if (rbClientUser.Checked)
                {
                    for (int m = 0; m < clnt.Length; m++)
                    {
                        emailbdy = "<table width=\"900px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">" + NetHtml + "</table>";
                        contactid = clnt[m].ToString();
                        billdate = oconverter.ArrangeDate2(dtFor.Value.ToString());
                        Subject = "Daily Bills For  " + billdate;
                        if (oDBEngine.SendReportSt(emailbdy, contactid, billdate, Subject) == true)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript20", "SendmailFilter();", true);
                            string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY12", "DISPLAY('" + SpanText + "');", true);
                            cmbgroupPager.SelectedItem.Value = cmbgroupPager.Items[0].Value;
                            cmbgroupPager.SelectedItem.Text = cmbgroupPager.Items[0].Text;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script05", "MailsendT();", true);
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "SendmailFilter();", true);
                            string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY26", "DISPLAY('" + SpanText + "');", true);
                            cmbgroupPager.SelectedItem.Value = cmbgroupPager.Items[0].Value;
                            cmbgroupPager.SelectedItem.Text = cmbgroupPager.Items[0].Text;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script01", "MailsendF();", true);
                        }
                    }
                }
            }



        }
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            procedure();
            print();

        }


        void print()
        {
            DataSet dsReport = (DataSet)ViewState["bill"];

            if (dsReport.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {

                //dsReport.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ComDailyBilling.xsd");
                string path = HttpContext.Current.Server.MapPath("..\\Reports\\ComDailyBilling.rpt");
                ReportDocument.Load(path);
                ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                ReportDocument.SetDataSource(dsReport.Tables[0]);
                ReportDocument.Subreports["DailyBillingFOHeader"].SetDataSource(dsReport.Tables[1]);
                ReportDocument.SetParameterValue("@ShowDate", (object)ViewState["header"]);
                ReportDocument.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Obligation Summary");
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            procedure();
            ddlbandforgroup();
            export();
        }

    }
}