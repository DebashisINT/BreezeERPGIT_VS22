using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_INMXBrokerageTOAnalysis : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Local Variable
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        public string dp;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        string data;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (HttpContext.Current.Session["Segmentname"] != null)
            {
                string strSegmentName = HttpContext.Current.Session["Segmentname"].ToString();
                dp = strSegmentName;
                Session["dp"] = strSegmentName;
                hdnDPSessionValue.Value = strSegmentName;
            }
            else
            {
                dp = HttpContext.Current.Session["userlastsegment"].ToString();
            }
            if (!IsPostBack)
            {
                dtfrom.Date = oDBEngine.GetDate();
                dtto.Date = oDBEngine.GetDate();
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");
                DataTable dtname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                ViewState["CompanyName"] = dtname.Rows[0]["cmp_Name"].ToString();
                FillConCurrency();
            }


            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___// 

            //if (ddlRptType.SelectedIndex > 0 && ddlConCurrencyType.SelectedIndex > 0 && ddlGenerate.SelectedValue == "G")
            //{
            //   BindGrid();
            //}
            if (ddlGenerate.SelectedValue == "G")
                BindGrid();
        }

        protected void FillConCurrency()
        {
            string LCurrencyValue = Session["LocalCurrency"].ToString().Split('~')[0];
            string LCurrencyText = Session["LocalCurrency"].ToString().Split('~')[1];
            string TCurrencyValue = Session["TradeCurrency"].ToString().Split('~')[0];
            string TCurrencyText = Session["TradeCurrency"].ToString().Split('~')[1];
            if (ddlConCurrencyType.Items.Count > 0) ddlConCurrencyType.Items.Clear();
            ddlConCurrencyType.Items.Insert(0, new ListItem("Select Currency", "0"));
            ddlConCurrencyType.Items.Insert(1, new ListItem(LCurrencyText, LCurrencyValue));
            ddlConCurrencyType.Items.Insert(2, new ListItem(TCurrencyText, TCurrencyValue));
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

        protected DataTable BindData()
        {
            DataTable dt = null;
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "0")/////if branch
            {
                if (rdbranchclientSelected.Checked == true && HiddenField_Branch.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    dt = runProcedure();
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")/////if group
            {
                if (ddlgrouptype.SelectedItem.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    if (rdddlgrouptypeSelected.Checked == true && HiddenField_Group.Value.ToString().Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                    }
                    else
                    {
                        dt = runProcedure();
                    }
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")/////if client wise
            {
                if (rdbranchclientSelected.Checked == true && HiddenField_Client.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    dt = runProcedure();
                }
            }
            return dt;
        }

        protected DataTable runProcedure()
        {
            string[] InputName = new string[11];
            string[] InputType = new string[11];
            string[] InputValue = new string[11];

            InputName[0] = "CompanyID";
            InputName[1] = "SegmentID";
            InputName[2] = "FinYear";
            InputName[3] = "DateFrom";
            InputName[4] = "DateTo";
            InputName[5] = "ReportType";
            InputName[6] = "ConCurrency";
            InputName[7] = "ByBranchGroupClient";
            InputName[8] = "IfBranch_OptAndIDs";
            InputName[9] = "IfGroup_OptAndIDsAndType";
            InputName[10] = "IfClient_OptAndIDs";

            InputType[0] = "V";
            InputType[1] = "I";
            InputType[2] = "V";
            InputType[3] = "D";
            InputType[4] = "D";
            InputType[5] = "I";
            InputType[6] = "I";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";

            InputValue[0] = Session["LastCompany"].ToString();
            InputValue[1] = Session["UserSegID"].ToString();
            InputValue[2] = Session["LastFinYear"].ToString();
            InputValue[3] = dtfrom.Date.ToString();
            InputValue[4] = dtto.Date.ToString();
            InputValue[5] = ddlRptType.SelectedValue.ToString();
            InputValue[6] = ddlConCurrencyType.SelectedValue.ToString();
            if (ddlGroup.SelectedItem.Value.ToString() == "0")/////option Branch 
            {
                InputValue[7] = "Branch";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[8] = "All";
                    InputValue[9] = "";
                    InputValue[10] = "";
                }
                else
                {
                    InputValue[8] = "Selected~" + HiddenField_Branch.Value;
                    InputValue[9] = "";
                    InputValue[10] = "";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////option Group
            {
                InputValue[7] = "Group";
                string groupType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    InputValue[8] = "";
                    InputValue[9] = "ALL~" + groupType;
                    InputValue[10] = "";
                }
                else
                {
                    InputValue[8] = "";
                    InputValue[9] = "Selected~" + HiddenField_Group.Value + "~" + groupType;
                    InputValue[10] = "";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")/////option Clients
            {
                InputValue[7] = "Clients";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[8] = "";
                    InputValue[9] = "";
                    InputValue[10] = "All";
                }
                else
                {
                    InputValue[8] = "";
                    InputValue[9] = "";
                    InputValue[10] = "Selected~" + HiddenField_Client.Value.Replace("'", "");
                }
            }
            else
            {
                InputValue[7] = "Branch";
                InputValue[8] = "All";
                InputValue[9] = "";
                InputValue[10] = "";
            }
            DataTable dtData = SQLProcedures.SelectProcedureArr("Report_INMXBrokerageTOAnalysis", InputName, InputType, InputValue);
            return dtData;
        }

        protected void BindGrid()
        {
            DataTable resultDt = null;
            try
            {
                resultDt = BindData();
                if (resultDt.Rows.Count > 1)
                {
                    resultDt.Rows.RemoveAt(resultDt.Rows.Count - 1);
                    resultDt.AcceptChanges();

                    if (ddlRptType.SelectedValue == "1")
                    {
                        GridVWINMX.Columns["Srl"].Visible = false;//R-4
                        GridVWINMX.Columns["Client"].Visible = true;
                        GridVWINMX.Columns["Trade Date"].Visible = false;//R-4
                        GridVWINMX.Columns["TradeCode"].Visible = true;
                        GridVWINMX.Columns["Product"].Visible = false;//R-3
                        GridVWINMX.Columns["Instrument"].Visible = false;//R-4
                        GridVWINMX.Columns["Exchange"].Visible = true;
                        GridVWINMX.Columns["Exp.Date"].Visible = false;//R-4
                        GridVWINMX.Columns["Trd.Cat"].Visible = true;
                        GridVWINMX.Columns["Trd.Cur"].Visible = true;
                        GridVWINMX.Columns["Conv.Cur"].Visible = true;
                        GridVWINMX.Columns["Lots"].Visible = false;//R-4
                        GridVWINMX.Columns["LotSize"].Visible = false;//R-3
                        GridVWINMX.Columns["Total Lots"].Visible = true;
                        GridVWINMX.Columns["Trd.Qty"].Visible = false;//R-3
                        GridVWINMX.Columns["Trd.Unit"].Visible = false;//R-3
                        GridVWINMX.Columns["Conv.Rate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.Qty"].Visible = false;//R-3
                        GridVWINMX.Columns["Conv.Unit"].Visible = false;//R-3
                        GridVWINMX.Columns["TurnOver"].Visible = true;
                        GridVWINMX.Columns["Exch.Charge"].Visible = true;
                        GridVWINMX.Columns["Brokerage"].Visible = true;
                        GridVWINMX.Columns["Net Earning"].Visible = true;
                        GridVWINMX.Columns["FxRate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.To"].Visible = true;
                        GridVWINMX.Columns["Conv.Exch.Chrg"].Visible = true;
                        GridVWINMX.Columns["Conv.Brkg"].Visible = true;
                        GridVWINMX.Columns["Conv.Net.Earning"].Visible = true;
                        GridVWINMX.Columns["Spread Charge"].Visible = false;//R-5
                        GridVWINMX.Columns["Expiry Cost"].Visible = false;//R-5
                        GridVWINMX.Columns["Symbol"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTLots"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTAmount"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPLots"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPAmount"].Visible = false;//R-6
                        //===================================================
                    }
                    else if (ddlRptType.SelectedValue == "2")
                    {
                        GridVWINMX.Columns["Srl"].Visible = false;//R-4
                        GridVWINMX.Columns["Client"].Visible = true;
                        GridVWINMX.Columns["Trade Date"].Visible = false;//R-4
                        GridVWINMX.Columns["TradeCode"].Visible = true;
                        GridVWINMX.Columns["Product"].Visible = false;
                        GridVWINMX.Columns["Instrument"].Visible = true;//R-4
                        GridVWINMX.Columns["Exchange"].Visible = true;
                        GridVWINMX.Columns["Exp.Date"].Visible = true;//R-4
                        GridVWINMX.Columns["Trd.Cat"].Visible = true;
                        GridVWINMX.Columns["Trd.Cur"].Visible = true;
                        GridVWINMX.Columns["Conv.Cur"].Visible = true;
                        GridVWINMX.Columns["Lots"].Visible = false;//R-4
                        GridVWINMX.Columns["LotSize"].Visible = true;
                        GridVWINMX.Columns["Total Lots"].Visible = true;
                        GridVWINMX.Columns["Trd.Qty"].Visible = true;
                        GridVWINMX.Columns["Trd.Unit"].Visible = true;
                        GridVWINMX.Columns["Conv.Rate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.Qty"].Visible = true;
                        GridVWINMX.Columns["Conv.Unit"].Visible = true;
                        GridVWINMX.Columns["TurnOver"].Visible = true;
                        GridVWINMX.Columns["Exch.Charge"].Visible = true;
                        GridVWINMX.Columns["Brokerage"].Visible = true;
                        GridVWINMX.Columns["Net Earning"].Visible = true;
                        GridVWINMX.Columns["FxRate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.To"].Visible = true;
                        GridVWINMX.Columns["Conv.Exch.Chrg"].Visible = true;
                        GridVWINMX.Columns["Conv.Brkg"].Visible = true;
                        GridVWINMX.Columns["Conv.Net.Earning"].Visible = true;
                        GridVWINMX.Columns["Spread Charge"].Visible = false;//R-5
                        GridVWINMX.Columns["Expiry Cost"].Visible = false;//R-5
                        GridVWINMX.Columns["Symbol"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTLots"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTAmount"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPLots"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPAmount"].Visible = false;//R-6
                    }
                    else if (ddlRptType.SelectedValue == "3")
                    {
                        GridVWINMX.Columns["Srl"].Visible = false;//R-4
                        GridVWINMX.Columns["Client"].Visible = true;
                        GridVWINMX.Columns["Trade Date"].Visible = false;//R-4
                        GridVWINMX.Columns["TradeCode"].Visible = true;
                        GridVWINMX.Columns["Product"].Visible = true;
                        GridVWINMX.Columns["Instrument"].Visible = false;//R-4
                        GridVWINMX.Columns["Exchange"].Visible = true;
                        GridVWINMX.Columns["Exp.Date"].Visible = false;//R-4
                        GridVWINMX.Columns["Trd.Cat"].Visible = true;
                        GridVWINMX.Columns["Trd.Cur"].Visible = true;
                        GridVWINMX.Columns["Conv.Cur"].Visible = true;
                        GridVWINMX.Columns["Lots"].Visible = false;//R-4
                        GridVWINMX.Columns["LotSize"].Visible = true;
                        GridVWINMX.Columns["Total Lots"].Visible = true;
                        GridVWINMX.Columns["Trd.Qty"].Visible = true;
                        GridVWINMX.Columns["Trd.Unit"].Visible = true;
                        GridVWINMX.Columns["Conv.Rate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.Qty"].Visible = true;
                        GridVWINMX.Columns["Conv.Unit"].Visible = true;
                        GridVWINMX.Columns["TurnOver"].Visible = true;
                        GridVWINMX.Columns["Exch.Charge"].Visible = true;
                        GridVWINMX.Columns["Brokerage"].Visible = true;
                        GridVWINMX.Columns["Net Earning"].Visible = true;
                        GridVWINMX.Columns["FxRate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.To"].Visible = true;
                        GridVWINMX.Columns["Conv.Exch.Chrg"].Visible = true;
                        GridVWINMX.Columns["Conv.Brkg"].Visible = true;
                        GridVWINMX.Columns["Conv.Net.Earning"].Visible = true;
                        GridVWINMX.Columns["Spread Charge"].Visible = false;//R-5
                        GridVWINMX.Columns["Expiry Cost"].Visible = false;//R-5
                        GridVWINMX.Columns["Symbol"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTLots"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTAmount"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPLots"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPAmount"].Visible = false;//R-6
                    }
                    else if (ddlRptType.SelectedValue == "4")
                    {
                        GridVWINMX.Columns["Srl"].Visible = true;
                        GridVWINMX.Columns["Trade Date"].Visible = true;
                        GridVWINMX.Columns["Client"].Visible = true;
                        GridVWINMX.Columns["TradeCode"].Visible = true;
                        GridVWINMX.Columns["Product"].Visible = false;//R-3
                        GridVWINMX.Columns["Instrument"].Visible = true;
                        GridVWINMX.Columns["Exchange"].Visible = true;
                        GridVWINMX.Columns["Exp.Date"].Visible = true;
                        GridVWINMX.Columns["Trd.Cat"].Visible = true;
                        GridVWINMX.Columns["Trd.Cur"].Visible = true;
                        GridVWINMX.Columns["Conv.Cur"].Visible = false;//R-3
                        GridVWINMX.Columns["Lots"].Visible = true;
                        GridVWINMX.Columns["LotSize"].Visible = true;
                        GridVWINMX.Columns["Total Lots"].Visible = false;//R-3
                        GridVWINMX.Columns["Trd.Qty"].Visible = true;
                        GridVWINMX.Columns["Trd.Unit"].Visible = true;
                        GridVWINMX.Columns["Conv.Rate"].Visible = true;
                        GridVWINMX.Columns["Conv.Qty"].Visible = true;
                        GridVWINMX.Columns["Conv.Unit"].Visible = true;
                        GridVWINMX.Columns["TurnOver"].Visible = true;
                        GridVWINMX.Columns["Exch.Charge"].Visible = true;
                        GridVWINMX.Columns["Brokerage"].Visible = true;
                        GridVWINMX.Columns["Net Earning"].Visible = true;
                        GridVWINMX.Columns["FxRate"].Visible = true;
                        GridVWINMX.Columns["Conv.To"].Visible = true;
                        GridVWINMX.Columns["Conv.Exch.Chrg"].Visible = true;
                        GridVWINMX.Columns["Conv.Brkg"].Visible = true;
                        GridVWINMX.Columns["Conv.Net.Earning"].Visible = true;
                        GridVWINMX.Columns["Spread Charge"].Visible = false;//R-5
                        GridVWINMX.Columns["Expiry Cost"].Visible = false;//R-5
                        GridVWINMX.Columns["Symbol"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTLots"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTAmount"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPLots"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPAmount"].Visible = false;//R-6

                    }
                    else if (ddlRptType.SelectedValue == "5")
                    {
                        GridVWINMX.Columns["Srl"].Visible = false;//R-4
                        GridVWINMX.Columns["Client"].Visible = true;
                        GridVWINMX.Columns["Trade Date"].Visible = false;//R-4
                        GridVWINMX.Columns["TradeCode"].Visible = false;//R-1
                        GridVWINMX.Columns["Product"].Visible = false;//R-3
                        GridVWINMX.Columns["Instrument"].Visible = false;//R-4
                        GridVWINMX.Columns["Exchange"].Visible = false;//R-1
                        GridVWINMX.Columns["Exp.Date"].Visible = false;//R-4
                        GridVWINMX.Columns["Trd.Cat"].Visible = false;//R-1
                        GridVWINMX.Columns["Trd.Cur"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Cur"].Visible = false;//R-1
                        GridVWINMX.Columns["Lots"].Visible = false;//R-4
                        GridVWINMX.Columns["LotSize"].Visible = false;//R-3
                        GridVWINMX.Columns["Total Lots"].Visible = false;//R-1
                        GridVWINMX.Columns["Trd.Qty"].Visible = false;//R-3
                        GridVWINMX.Columns["Trd.Unit"].Visible = false;//R-3
                        GridVWINMX.Columns["Conv.Rate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.Qty"].Visible = false;//R-3
                        GridVWINMX.Columns["Conv.Unit"].Visible = false;//R-3
                        GridVWINMX.Columns["TurnOver"].Visible = false;//R-1
                        GridVWINMX.Columns["Exch.Charge"].Visible = false;//R-1
                        GridVWINMX.Columns["Brokerage"].Visible = false;//R-1
                        GridVWINMX.Columns["Net Earning"].Visible = false;//R-1
                        GridVWINMX.Columns["FxRate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.To"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Exch.Chrg"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Brkg"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Net.Earning"].Visible = false;//R-1
                        GridVWINMX.Columns["Spread Charge"].Visible = true;
                        GridVWINMX.Columns["Expiry Cost"].Visible = true;
                        GridVWINMX.Columns["Symbol"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTLots"].Visible = false;//R-6
                        GridVWINMX.Columns["SOTAmount"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPLots"].Visible = false;//R-6
                        GridVWINMX.Columns["EXPAmount"].Visible = false;//R-6

                    }
                    else if (ddlRptType.SelectedValue == "6")
                    {
                        GridVWINMX.Columns["Srl"].Visible = false;//R-4
                        GridVWINMX.Columns["Client"].Visible = true;
                        GridVWINMX.Columns["Trade Date"].Visible = false;//R-4
                        GridVWINMX.Columns["TradeCode"].Visible = true;
                        GridVWINMX.Columns["Product"].Visible = false;//R-3
                        GridVWINMX.Columns["Instrument"].Visible = false;//R-4
                        GridVWINMX.Columns["Exchange"].Visible = false;//R-1
                        GridVWINMX.Columns["Exp.Date"].Visible = false;//R-4
                        GridVWINMX.Columns["Trd.Cat"].Visible = false;//R-1
                        GridVWINMX.Columns["Trd.Cur"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Cur"].Visible = false;//R-1
                        GridVWINMX.Columns["Lots"].Visible = false;//R-4
                        GridVWINMX.Columns["LotSize"].Visible = false;//R-3
                        GridVWINMX.Columns["Total Lots"].Visible = false;//R-1
                        GridVWINMX.Columns["Trd.Qty"].Visible = false;//R-3
                        GridVWINMX.Columns["Trd.Unit"].Visible = false;//R-3
                        GridVWINMX.Columns["Conv.Rate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.Qty"].Visible = false;//R-3
                        GridVWINMX.Columns["Conv.Unit"].Visible = false;//R-3
                        GridVWINMX.Columns["TurnOver"].Visible = false;//R-1
                        GridVWINMX.Columns["Exch.Charge"].Visible = false;//R-1
                        GridVWINMX.Columns["Brokerage"].Visible = false;//R-1
                        GridVWINMX.Columns["Net Earning"].Visible = false;//R-1
                        GridVWINMX.Columns["FxRate"].Visible = false;//R-4
                        GridVWINMX.Columns["Conv.To"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Exch.Chrg"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Brkg"].Visible = false;//R-1
                        GridVWINMX.Columns["Conv.Net.Earning"].Visible = false;//R-1
                        GridVWINMX.Columns["Spread Charge"].Visible = false;//R-5
                        GridVWINMX.Columns["Expiry Cost"].Visible = false;//R-5
                        GridVWINMX.Columns["Symbol"].Visible = true;
                        GridVWINMX.Columns["SOTLots"].Visible = true;
                        GridVWINMX.Columns["SOTAmount"].Visible = true;
                        GridVWINMX.Columns["EXPLots"].Visible = true;
                        GridVWINMX.Columns["EXPAmount"].Visible = true;
                    }
                    GridVWINMX.DataSource = resultDt;
                    GridVWINMX.DataBind();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset111", "Reset();", true);
                }
                else
                {
                    GridVWINMX.DataSource = null;
                    GridVWINMX.DataBind();
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(2);", true);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            finally
            {
                resultDt.Dispose();
            }
        }

        protected void btnShowGrid_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlGenerate.SelectedValue == "G")
                {
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message.ToString() + "<br>";
                lblStatus.Text += "Error Showing. Please try again";
            }
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            DataTable resultDt = null;
            try
            {
                if (ddlGenerate.SelectedValue == "E")
                {
                    resultDt = BindData();
                    ExportToExcel_Generic(resultDt, "2007");
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message.ToString() + "<br>";
                lblStatus.Text += "Error Exporting. Please try again";
            }
        }

        void ExportToExcel_Generic(DataTable Dt, string ExcelVersion)
        {
            if (Dt.Rows.Count > 1)
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string grpBy = null;
                string reportType = null;
                string strReportHeader = null;
                if ((ddlGroup.SelectedItem.Text) != null) grpBy = ddlGroup.SelectedItem.Text;
                if ((ddlRptType.SelectedItem.Value) != "0") reportType = ddlRptType.SelectedItem.Text;
                strReportHeader = "Showing " + reportType + " Report Search By Date  Between  " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "  to " + oconverter.ArrangeDate2(dtto.Value.ToString());
                if (grpBy != null)
                {
                    strReportHeader += " and Filter By " + grpBy;
                }
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "INMXBrokerage_" + exlTime;
                strDownloadFileName = "~/Documents/";
                string[] strHead = new string[3];
                strHead[0] = exlDateTime;
                strHead[1] = strReportHeader;
                strHead[2] = "Client/Product wise Turnover cum Net Earning Analysis Of " + ViewState["CompanyName"].ToString();
                if (ddlRptType.SelectedValue == "1")//Exchange
                {
                    if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "I", "N", "N", "N", "N", "N", "N", "N", "N", "V", "V" };
                        string[] ColumnSize = { "30", "20", "20", "20", "20", "20", "10", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "30", "20" };
                        string[] ColumnWidthSize = { "30", "10", "10", "10", "10", "10", "10", "20", "20", "20", "20", "20", "20", "20", "20", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else   //Without Group & Group_Type for search By Branch and Clients
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "I", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "30", "20", "20", "20", "20", "20", "10", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2" };
                        string[] ColumnWidthSize = { "30", "10", "10", "10", "10", "10", "10", "20", "20", "20", "20", "20", "20", "20", "20" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                if (ddlRptType.SelectedValue == "2")//Instrument
                {
                    if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "V", "V" };
                        string[] ColumnSize = { "40", "20", "50", "10", "15", "15", "10", "10", "10", "10", "10", "10", "15,3", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "30", "15" };
                        string[] ColumnWidthSize = { "20", "15", "30", "10", "15", "10", "10", "10", "10", "10", "20", "10", "20", "20", "20", "20", "20", "20", "20", "20", "20", "20", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else   //Without Group & Group_Type for search By Branch and Clients
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "40", "20", "50", "10", "15", "15", "10", "10", "10", "10", "10", "10", "15,3", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2" };
                        string[] ColumnWidthSize = { "20", "15", "30", "10", "15", "10", "10", "10", "10", "10", "20", "10", "20", "20", "20", "20", "20", "20", "20", "20", "20", "20", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                if (ddlRptType.SelectedValue == "3")//Product
                {
                    if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "I", "I", "N", "V", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N", "V", "V" };
                        string[] ColumnSize = { "30", "15", "40", "20", "10", "10", "10", "12", "10", "15,3", "10", "15,3", "10", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "30", "20" };
                        string[] ColumnWidthSize = { "20", "10", "30", "20", "10", "10", "10", "10", "10", "20", "10", "20", "10", "20", "20", "20", "20", "20", "20", "20", "20", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else   //Without Group & Group_Type for search By Branch and Clients
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "I", "I", "N", "V", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "30", "15", "40", "20", "10", "10", "10", "12", "10", "15,3", "10", "15,3", "10", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2", "15,2" };
                        string[] ColumnWidthSize = { "20", "10", "30", "20", "10", "10", "10", "10", "10", "20", "10", "20", "10", "20", "20", "20", "20", "20", "20", "20", "20" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                if (ddlRptType.SelectedValue == "4")//srl
                {
                    if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group
                    {
                        string[] ColumnType = { "I", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "N", "V", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "V", "V" };
                        string[] ColumnSize = { "6", "15", "30", "10", "50", "25", "15", "10", "10", "10", "6", "10", "15,3", "10", "12,8", "15,3", "10", "15,2", "15,2", "15,2", "15,2", "15,8", "15,2", "15,2", "15,2", "15,2", "20", "20" };
                        string[] ColumnWidthSize = { "5", "15", "30", "15", "30", "20", "15", "10", "10", "10", "5", "10", "20", "10", "20", "20", "10", "20", "20", "20", "20", "25", "20", "20", "20", "20", "15", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else   //Without Group & Group_Type for search By Branch and Clients
                    {
                        string[] ColumnType = { "I", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "N", "V", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "6", "15", "30", "10", "50", "25", "15", "10", "10", "10", "6", "10", "15,3", "10", "12,8", "15,3", "10", "15,2", "15,2", "15,2", "15,2", "15,8", "15,2", "15,2", "15,2", "15,2" };
                        string[] ColumnWidthSize = { "5", "15", "30", "15", "30", "20", "15", "10", "10", "10", "5", "10", "20", "10", "20", "20", "10", "20", "20", "20", "20", "25", "20", "20", "20", "20" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                if (ddlRptType.SelectedValue == "5")
                {
                    if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group
                    {
                        string[] ColumnType = { "V", "N", "N", "V", "V" };
                        string[] ColumnSize = { "20", "15,2", "15,2", "40", "20" };
                        string[] ColumnWidthSize = { "20", "20", "20", "30", "20" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else   //Without Group & Group_Type for search By Branch and Clients
                    {
                        string[] ColumnType = { "V", "N", "N" };
                        string[] ColumnSize = { "20", "15,2", "15,2" };
                        string[] ColumnWidthSize = { "20", "20", "20" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                if (ddlRptType.SelectedValue == "6")
                {
                    if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group
                    {
                        string[] ColumnType = { "V", "V", "V", "I", "N", "I", "N", "V", "V" };
                        string[] ColumnSize = { "30", "10", "30", "10", "15,2", "10", "15,2", "40", "20" };
                        string[] ColumnWidthSize = { "25", "10", "20", "10", "20", "10", "20", "30", "20" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else   //Without Group & Group_Type for search By Branch and Clients
                    {
                        string[] ColumnType = { "V", "V", "V", "I", "N", "I", "N" };
                        string[] ColumnSize = { "30", "10", "30", "10", "15,2", "10", "15,2" };
                        string[] ColumnWidthSize = { "25", "10", "20", "10", "20", "10", "20" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset111", "Reset();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(2);", true);
            }
        }

        protected void GridVWINMX_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
        {
            //if (e.Column.FieldName == "TotalRecPay")
            //{
            //    if (e.GetListSourceFieldValue("ReceiptAmount").ToString().Trim() != String.Empty && e.GetListSourceFieldValue("PaymentAmount").ToString().Trim() != String.Empty)
            //    {
            //        TotalRecieve = TotalRecieve + Convert.ToDecimal(e.GetListSourceFieldValue("ReceiptAmount"));
            //        TotalPayment = TotalPayment + Convert.ToDecimal(e.GetListSourceFieldValue("PaymentAmount"));
            //    }
            //}
        }
    }
}