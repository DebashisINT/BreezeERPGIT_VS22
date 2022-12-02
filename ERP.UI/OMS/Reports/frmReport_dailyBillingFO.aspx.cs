using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_dailyBillingFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        BusinessLogicLayer.DailyReports dailyrep = new BusinessLogicLayer.DailyReports();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        static string Client;
        static string Branch;
        static string Group;
        string data;
        DataSet ds = new DataSet();

        //for email
        //--for sending email
        static string SubClients = "";
        DataTable dtEmail = new DataTable();
        string footerTxt = "";
        string[] idlist;
        int pageindex = 0;
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
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Clients" && idlist[0] != "SettlementType" && idlist[0] != "Broker")
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
                            ///if (idlist[0] == "MAILEMPLOYEE")
                            if (idlist[0] == "EM")
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
                            //if (idlist[0] == "MAILEMPLOYEE")
                            if (idlist[0] == "EM")
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


                if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Broker")
                {
                    data = "Broker~" + str;
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

        void procedure()
        {

            string CalType = null;
            if (ddlCalType.SelectedValue == "1")
                CalType = "E";
            else
                CalType = "P";

            string grpbranch = "";
            string GRPID = "";
            string Broker = "";
            string ClientsID = "";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")/////option branch 
            {
                grpbranch = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_Branch.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////option group
            {
                grpbranch = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_Group.Value;
                }
            }

            else
            {
                grpbranch = "BRANCH";
                GRPID = "ALL";

            }

            if (ddlviewby.SelectedItem.Value == "2")
            {
                Broker = "BO";
                if (rdbbrokerall.Checked)
                {
                    ClientsID = "ALL";
                }
                else
                {
                    ClientsID = HiddenField_Broker.Value;
                }

            }

            if (ddlviewby.SelectedItem.Value == "1")
            {
                Broker = "CL";
                if (rdbClientALL.Checked)
                {
                    ClientsID = "ALL";
                }
                else
                {
                    ClientsID = HiddenField_Client.Value;
                }
            }

            ds = dailyrep.Report_DailyBillsFO(Session["usersegid"].ToString(), Session["LastCompany"].ToString(), dtFor.Value.ToString(), HttpContext.Current.Session["LastFinYear"].ToString(),
                Session["userbranchHierarchy"].ToString(), grpbranch, GRPID, Broker, ClientsID, CalType, HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                ddlAppMargin.SelectedItem.Value.ToString() == "1" ? "Include" : "Exclude", chkShowMargin.Checked == true ? "Chk" : "UnChk");
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
            gridbind();
        }
        protected void cmbgroupPager_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridbind();
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
                gridbind();
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript16", "SendmailFilter();", true);
            }
        }
        void gridbind()
        {
            ds = (DataSet)ViewState["bill"];

            dt = new DataTable();
            DataView viewgroup = new DataView();
            viewgroup = ds.Tables[0].DefaultView;
            viewgroup.RowFilter = "GrpId=" + cmbgroupPager.SelectedItem.Value;
            dt = viewgroup.ToTable();
            grdBilling.DataSource = dt;
            grdBilling.DataBind();
            if (chkShowMargin.Checked == false)
            {
                grdBilling.Columns[15].Visible = false;
                grdBilling.Columns[16].Visible = false;
                grdBilling.Columns[17].Visible = false;
            }
            else
            {
                grdBilling.Columns[15].Visible = true;
                grdBilling.Columns[16].Visible = true;
                grdBilling.Columns[17].Visible = true;
            }
            footer(dt);
            /////////////////////////////DISPLAY DATE/////////////////////////////////////
            divgrid.Attributes.Add("style", "width: " + hidScreenWd.Value + "px; overflow:scroll");

            string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

            ///////////////////////////////END//////////////////////////////////////////
            ScriptManager.RegisterStartupScript(this, this.GetType(), "line", "line();", true);
        }

        void footer(DataTable dt)
        {

            grdBilling.FooterRow.Cells[0].Text = "Total";

            if (dt.Rows[0]["Branch_OpeningBal"].ToString() == "0")
                grdBilling.FooterRow.Cells[2].Text = "";
            else
                grdBilling.FooterRow.Cells[2].Text = dt.Rows[0]["Branch_OpeningBal"].ToString();

            if (dt.Rows[0]["Branch_Premium"].ToString() == "0")
                grdBilling.FooterRow.Cells[3].Text = "";
            else
                grdBilling.FooterRow.Cells[3].Text = dt.Rows[0]["Branch_Premium"].ToString();

            if (dt.Rows[0]["Branch_MTM"].ToString() == "0")
                grdBilling.FooterRow.Cells[4].Text = "";
            else
                grdBilling.FooterRow.Cells[4].Text = dt.Rows[0]["Branch_MTM"].ToString();


            if (dt.Rows[0]["Branch_FutureSettlement"].ToString() == "0")
                grdBilling.FooterRow.Cells[5].Text = "";
            else
                grdBilling.FooterRow.Cells[5].Text = dt.Rows[0]["Branch_FutureSettlement"].ToString();


            if (dt.Rows[0]["Branch_ASNEXCFINSET"].ToString() == "0")
                grdBilling.FooterRow.Cells[6].Text = "";
            else
                grdBilling.FooterRow.Cells[6].Text = dt.Rows[0]["Branch_ASNEXCFINSET"].ToString();

            if (dt.Rows[0]["Branch_Charges"].ToString() == "0")
                grdBilling.FooterRow.Cells[7].Text = "";
            else
                grdBilling.FooterRow.Cells[7].Text = dt.Rows[0]["Branch_Charges"].ToString();

            if (dt.Rows[0]["Branch_ServTax"].ToString() == "0")
                grdBilling.FooterRow.Cells[8].Text = "";
            else
                grdBilling.FooterRow.Cells[8].Text = dt.Rows[0]["Branch_ServTax"].ToString();

            if (dt.Rows[0]["Branch_NetObligation"].ToString() == "0")
                grdBilling.FooterRow.Cells[9].Text = "";
            else
                grdBilling.FooterRow.Cells[9].Text = dt.Rows[0]["Branch_NetObligation"].ToString();

            if (dt.Rows[0]["Branch_NetAdj"].ToString() == "0")
                grdBilling.FooterRow.Cells[10].Text = "";
            else
                grdBilling.FooterRow.Cells[10].Text = dt.Rows[0]["Branch_NetAdj"].ToString();

            if (dt.Rows[0]["Branch_ClosingBal"].ToString() == "0")
                grdBilling.FooterRow.Cells[11].Text = "";
            else
                grdBilling.FooterRow.Cells[11].Text = dt.Rows[0]["Branch_ClosingBal"].ToString();

            if (dt.Rows[0]["Branch_CashMarnDeposit"].ToString() == "0")
                grdBilling.FooterRow.Cells[12].Text = "";
            else
                grdBilling.FooterRow.Cells[12].Text = dt.Rows[0]["Branch_CashMarnDeposit"].ToString();

            if (dt.Rows[0]["Branch_ColeteralVal"].ToString() == "0")
                grdBilling.FooterRow.Cells[13].Text = "";
            else
                grdBilling.FooterRow.Cells[13].Text = dt.Rows[0]["Branch_ColeteralVal"].ToString();

            if (dt.Rows[0]["Branch_EffecTiveDeposit"].ToString() == "0")
                grdBilling.FooterRow.Cells[14].Text = "";
            else
                grdBilling.FooterRow.Cells[14].Text = dt.Rows[0]["Branch_EffecTiveDeposit"].ToString();
            if (chkShowMargin.Checked == true)
            {
                if (dt.Rows[0]["Branch_Spanmargin"].ToString() == "0")
                    grdBilling.FooterRow.Cells[15].Text = "";
                else
                    grdBilling.FooterRow.Cells[15].Text = dt.Rows[0]["Branch_Spanmargin"].ToString();


                if (dt.Rows[0]["Branch_PremiumMargin"].ToString() == "0")
                    grdBilling.FooterRow.Cells[16].Text = "";
                else
                    grdBilling.FooterRow.Cells[16].Text = dt.Rows[0]["Branch_PremiumMargin"].ToString();


                if (dt.Rows[0]["Branch_ExposureMargin"].ToString() == "0")
                    grdBilling.FooterRow.Cells[17].Text = "";
                else
                    grdBilling.FooterRow.Cells[17].Text = dt.Rows[0]["Branch_ExposureMargin"].ToString();
            }

            if (dt.Rows[0]["Branch_ApplMargin"].ToString() == "0")
                grdBilling.FooterRow.Cells[18].Text = "";
            else
                grdBilling.FooterRow.Cells[18].Text = dt.Rows[0]["Branch_ApplMargin"].ToString();


            if (dt.Rows[0]["Branch_ExcessShortage"].ToString() == "0")
                grdBilling.FooterRow.Cells[19].Text = "";
            else
                grdBilling.FooterRow.Cells[19].Text = dt.Rows[0]["Branch_ExcessShortage"].ToString();


            if (dt.Rows[0]["Branch_Exposure"].ToString() == "0")
                grdBilling.FooterRow.Cells[20].Text = "";
            else
                grdBilling.FooterRow.Cells[20].Text = dt.Rows[0]["Branch_Exposure"].ToString();

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
            grdBilling.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;
            grdBilling.FooterRow.Cells[19].HorizontalAlign = HorizontalAlign.Right;

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
            grdBilling.FooterRow.Cells[17].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[18].ForeColor = System.Drawing.Color.White;
            grdBilling.FooterRow.Cells[19].ForeColor = System.Drawing.Color.White;

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
            grdBilling.FooterRow.Cells[17].Font.Bold = true;
            grdBilling.FooterRow.Cells[18].Font.Bold = true;
            grdBilling.FooterRow.Cells[19].Font.Bold = true;

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
            grdBilling.FooterRow.Cells[17].Wrap = false;
            grdBilling.FooterRow.Cells[18].Wrap = false;
            grdBilling.FooterRow.Cells[19].Wrap = false;


            if (chkShowMargin.Checked == false)
            {
                footerTxt += "<tr style=\"background-color: #FFD4AA; color: Black;\">";
                for (int i = 0; i < 21; i++)
                {
                    if (i != 15 && i != 16 && i != 17)
                    {
                        footerTxt += "<td>&nbsp;" + grdBilling.FooterRow.Cells[i].Text.ToString() + "</td>";
                    }

                }
                footerTxt += "<tr>";
            }
            else
            {
                footerTxt += "<tr style=\"background-color: #FFD4AA; color: Black;\">";
                for (int i = 0; i < 21; i++)
                {
                    footerTxt += "<td>&nbsp;" + grdBilling.FooterRow.Cells[i].Text.ToString() + "</td>";
                }
                footerTxt += "<tr>";
            }
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
            dtExport.Columns.Add("ASN/EXC Fin Sett", Type.GetType("System.String"));
            dtExport.Columns.Add("Charges", Type.GetType("System.String"));
            dtExport.Columns.Add("Serv.Tax", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Oblgtn", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Adjustment", Type.GetType("System.String"));
            dtExport.Columns.Add("Closing Balance", Type.GetType("System.String"));
            dtExport.Columns.Add("Cash Margin Deposit", Type.GetType("System.String"));
            dtExport.Columns.Add("Collateral Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Effective Deposit", Type.GetType("System.String"));
            if (chkShowMargin.Checked == true)
            {
                dtExport.Columns.Add("Span Margin", Type.GetType("System.String"));
                dtExport.Columns.Add("Premium Margin", Type.GetType("System.String"));
                dtExport.Columns.Add("Exposure Margin", Type.GetType("System.String"));

            }
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

                    if (dr1["ASNEXCFINSET"] != DBNull.Value)
                    {
                        row2["ASN/EXC Fin Sett"] = dr1["ASNEXCFINSET"];
                    }

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
                    if (chkShowMargin.Checked == true)
                    {
                        if (dr1["Spanmargin"] != DBNull.Value)
                        {
                            row2["Span Margin"] = dr1["Spanmargin"];
                        }

                        if (dr1["PremiumMargin"] != DBNull.Value)
                        {
                            row2["Premium Margin"] = dr1["PremiumMargin"];
                        }

                        if (dr1["ExposureMargin"] != DBNull.Value)
                        {
                            row2["Exposure Margin"] = dr1["ExposureMargin"].ToString();
                        }

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

                if (dt.Rows[0]["Branch_ASNEXCFINSET"] != DBNull.Value)
                {
                    row["ASN/EXC Fin Sett"] = dt.Rows[0]["Branch_ASNEXCFINSET"].ToString();
                }

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

                if (chkShowMargin.Checked == true)
                {

                    if (dt.Rows[0]["Branch_Spanmargin"] != DBNull.Value)
                    {
                        row["Span Margin"] = dt.Rows[0]["Branch_Spanmargin"].ToString();
                    }

                    if (dt.Rows[0]["Branch_PremiumMargin"] != DBNull.Value)
                    {
                        row["Premium Margin"] = dt.Rows[0]["Branch_PremiumMargin"].ToString();
                    }

                    if (dt.Rows[0]["Branch_ExposureMargin"] != DBNull.Value)
                    {
                        row["Exposure Margin"] = dt.Rows[0]["Branch_ExposureMargin"].ToString();
                    }

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
            row5["ASN/EXC Fin Sett"] = ds.Tables[0].Rows[0]["Grand_ASNEXCFINSET"];
            row5["Charges"] = ds.Tables[0].Rows[0]["Grand_Charges"];
            row5["Serv.Tax"] = ds.Tables[0].Rows[0]["Grand_ServTax"];
            row5["Net Oblgtn"] = ds.Tables[0].Rows[0]["Grand_NetObligation"];
            row5["Net Adjustment"] = ds.Tables[0].Rows[0]["Grand_NetAdj"];
            row5["Closing Balance"] = ds.Tables[0].Rows[0]["Grand_ClosingBal"];
            row5["Cash Margin Deposit"] = ds.Tables[0].Rows[0]["Grand_CashMarnDeposit"];
            row5["Collateral Value"] = ds.Tables[0].Rows[0]["Grand_ColeteralVal"];
            row5["Effective Deposit"] = ds.Tables[0].Rows[0]["Grand_EffecTiveDeposit"];
            if (chkShowMargin.Checked == true)
            {
                row5["Span Margin"] = ds.Tables[0].Rows[0]["Grand_Spanmargin"];
                row5["Premium Margin"] = ds.Tables[0].Rows[0]["Grand_PremiumMargin"];
                row5["Exposure Margin"] = ds.Tables[0].Rows[0]["Grand_ExposureMargin"];
            }
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

                dt = new DataTable();
                DataView viewgroup = new DataView();
                viewgroup = ds.Tables[0].DefaultView;
                viewgroup.RowFilter = "GrpId=" + cmbgroupPager.SelectedItem.Value;
                dt = viewgroup.ToTable();
                dtEmail = dt;

                footer(dt);
                if (chkShowMargin.Checked == false)
                {
                    disptbl = "<table width=\"1350px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"center\">Account Name</td><td align=\"center\">Account Code</td><td align=\"center\">Opening Balance</td><td align=\"center\">Premium</td><td align=\"center\">MTM</td><td align=\"center\">Fin Sett</td><td align=\"center\">ASN/EXC Fin Sett</td><td align=\"center\">Charges</td><td align=\"center\">Serv.Tax</td><td align=\"center\">Net Oblgtn</td><td align=\"center\">Net Adjustment</td><td align=\"center\">Closing Balance</td><td align=\"center\">Cash Margin Deposit</td><td align=\"center\">Collateral Value</td><td align=\"center\">Effective Deposit</td><td align=\"center\">Appl Margin</td><td align=\"center\">Excess/Shortage(-)</td><td align=\"center\">Exposure</td></tr>";
                }
                else
                {
                    disptbl = "<table width=\"1350px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"center\">Account Name</td><td align=\"center\">Account Code</td><td align=\"center\">Opening Balance</td><td align=\"center\">Premium</td><td align=\"center\">MTM</td><td align=\"center\">Fin Sett</td><td align=\"center\">ASN/EXC Fin Sett</td><td align=\"center\">Charges</td><td align=\"center\">Serv.Tax</td><td align=\"center\">Net Oblgtn</td><td align=\"center\">Net Adjustment</td><td align=\"center\">Closing Balance</td><td align=\"center\">Cash Margin Deposit</td><td align=\"center\">Collateral Value</td><td align=\"center\">Effective Deposit</td><td align=\"center\">Span Margin</td><td align=\"center\">Premium Margin</td><td align=\"center\">Exposure Margin</td><td align=\"center\">Appl Margin</td><td align=\"center\">Excess/Shortage(-)</td><td align=\"center\">Exposure</td></tr>";

                }
                for (int i = 0; i < dtEmail.Rows.Count; i++)
                {
                    if (chkShowMargin.Checked == false)
                    {
                        disptbl += "<tr><td>&nbsp;" + dtEmail.Rows[i]["clientname"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ucc"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["OpeningBal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Premium"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["MTM"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["FutureSettlement"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ASNEXCFINSET"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Charges"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ServTax"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["NetObligation"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["NetAdj"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ClosingBal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["CashMarnDeposit"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ColeteralVal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["EffecTiveDeposit"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ApplMargin"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ExcessShortage"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Exposure"] + "</td></tr>";
                    }
                    else
                    {
                        disptbl += "<tr><td>&nbsp;" + dtEmail.Rows[i]["clientname"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ucc"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["OpeningBal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Premium"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["MTM"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["FutureSettlement"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ASNEXCFINSET"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Charges"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ServTax"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["NetObligation"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["NetAdj"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ClosingBal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["CashMarnDeposit"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ColeteralVal"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["EffecTiveDeposit"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["SpanMargin"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["PremiumMargin"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ExposureMargin"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ApplMargin"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["ExcessShortage"] + "</td><td align=\"right\">&nbsp;" + dtEmail.Rows[i]["Exposure"] + "</td></tr>";
                    }

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
                //dsReport.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\DailyBillingFO.xsd");
                string path = HttpContext.Current.Server.MapPath("..\\Reports\\DailyBillingFO.rpt");
                ReportDocument.Load(path);
                ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                ReportDocument.SetDataSource(dsReport.Tables[0]);
                ReportDocument.Subreports["DailyBillingFOHeader"].SetDataSource(dsReport.Tables[1]);
                ReportDocument.SetParameterValue("@ShowDate", (object)ViewState["header"]);
                if (chkShowMargin.Checked == true)
                {
                    ReportDocument.SetParameterValue("@ShowMargin", "Chk");
                }
                else
                {
                    ReportDocument.SetParameterValue("@ShowMargin", "UnChk");
                    //ReportDocument.PrintOptions.PaperSize=CrystalDecisions.Shared.PaperSize.PaperA4;
                    //CrystalDecisions.Shared.PageMargins PM=new CrystalDecisions.Shared.PageMargins(360,360,250,360);
                    //ReportDocument.PrintOptions.ApplyPageMargins(PM);

                }
                ReportDocument.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "F&O Obligation Summary");
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