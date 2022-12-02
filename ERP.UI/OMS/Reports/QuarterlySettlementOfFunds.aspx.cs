using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_QuarterlySettlementOfFunds : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Local Variable
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        public string dp;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        GenericMethod oGenericMethod;
        string data;
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] PageSession = { "dp" };
                oGenericMethod = new GenericMethod();
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
            }
        }

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
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "0")/////if branch
            {
                if (rdbranchclientSelected.Checked == true && HiddenField_Branch.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    spcall();
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
                        spcall();
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
                    spcall();
                }
            }
        }

        protected DataTable runProcedure()
        {
            string[] InputName = new string[8];
            string[] InputType = new string[8];
            string[] InputValue = new string[8];

            InputName[0] = "Company";
            InputName[1] = "FinYear";
            InputName[2] = "FromDate";
            InputName[3] = "ToDate";
            InputName[4] = "ByBranchGroupClient";
            InputName[5] = "IfBranch_OptAndIDs";
            InputName[6] = "IfGroup_OptAndIDsAndType";
            InputName[7] = "IfClient_OptAndIDs";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "D";
            InputType[3] = "D";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";

            InputValue[0] = Session["LastCompany"].ToString();
            InputValue[1] = Session["LastFinYear"].ToString();
            InputValue[2] = dtfrom.Date.ToString();
            InputValue[3] = dtto.Date.ToString();

            if (ddlGroup.SelectedItem.Value.ToString() == "0")/////option Branch 
            {
                InputValue[4] = "Branch";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[5] = "All";
                    InputValue[6] = "";
                    InputValue[7] = "";
                }
                else
                {
                    InputValue[5] = "Selected~" + HiddenField_Branch.Value;
                    InputValue[6] = "";
                    InputValue[7] = "";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////option Group
            {
                InputValue[4] = "Group";
                string groupType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    InputValue[5] = "";
                    InputValue[6] = "ALL~" + groupType;
                    InputValue[7] = "";
                }
                else
                {
                    InputValue[5] = "";
                    InputValue[6] = "Selected~" + HiddenField_Group.Value + "~" + groupType;
                    InputValue[7] = "";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")/////option Clients
            {
                InputValue[4] = "Clients";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[5] = "";
                    InputValue[6] = "";
                    InputValue[7] = "All";
                }
                else
                {
                    InputValue[5] = "";
                    InputValue[6] = "";
                    InputValue[7] = "Selected~" + HiddenField_Client.Value;
                }
            }
            else
            {
                InputValue[4] = "Branch";
                InputValue[5] = "All";
                InputValue[6] = "";
                InputValue[7] = "";
            }
            DataTable dtData = SQLProcedures.SelectProcedureArr("Fetch_QuarterlySettlementOfFunds", InputName, InputType, InputValue);
            return dtData;
        }

        void spcall()
        {
            DataTable dt = runProcedure();
            if (dt.Rows.Count > 0)
            {
                ExportToExcel_Generic(dt, "2007");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD2", "NORECORD(2);", true);
            }
        }

        void ExportToExcel_Generic(DataTable Dt, string ExcelVersion)
        {
            if (Dt.Rows.Count > 0)
            {
                if (Dt.Columns.Contains("cnt_internalId"))
                {
                    Dt.Columns.Remove("cnt_internalId");
                }
                Dt.AcceptChanges();
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group
                {
                    Dt.Columns[0].ColumnName = "Financial Year";//"FinYear";
                    Dt.Columns[1].ColumnName = "Date Of Settlement";//"DateOfSettlement";
                    //Dt.Columns[2].ColumnName = "Quarter";
                    //Dt.Columns[3].ColumnName = "Client";
                    //Dt.Columns[4].ColumnName = "Branch";
                    //Dt.Columns[5].ColumnName = "Group";
                    //Dt.Columns[6].ColumnName = "Group_Type";
                    //Dt.Columns[7].ColumnName = "Ledger";
                    Dt.Columns[8].ColumnName = "Margin Ledger"; //"MarginLedger";
                    Dt.Columns[9].ColumnName = "TDay FO Bill Reversal"; //"TDayFOBillReversal";
                    Dt.Columns[10].ColumnName = "TDay CD Bill Reversal"; //"TDayCDBillReversal";
                    Dt.Columns[11].ColumnName = "TDay CM Bill Reversal"; //"TDayCMBillReversal";
                    Dt.Columns[12].ColumnName = "TM 1 Day CM Bill Reversal"; //"TM1DayCMBillReversal";
                    Dt.Columns[13].ColumnName = "Total Security"; //"TotalSecu";
                    Dt.Columns[14].ColumnName = "Total Funds And Secu"; //"TotalFundsAndSecu";ok
                    Dt.Columns[15].ColumnName = "TDay FO Margin"; //"TDayFOMargin";
                    Dt.Columns[16].ColumnName = "TDay CD Margin"; //"TDayCDMargin";
                    Dt.Columns[17].ColumnName = "Margin Markup";
                    Dt.Columns[18].ColumnName = "FOCD Margin Amount"; //"FOCDMarginAmount";
                    Dt.Columns[19].ColumnName = "TDay FO Payin"; //"TDayFOPayin";
                    Dt.Columns[20].ColumnName = "TDay CD Payin"; //"TDayCDPayin";
                    Dt.Columns[21].ColumnName = "TDay CM Payin"; //"TDayCMPayin";
                    Dt.Columns[22].ColumnName = "TM 1 Day CM Payin"; //"TM1DayCMPayin";
                    Dt.Columns[23].ColumnName = "TDay CM Delivery IN"; //"TDayCMDeliveryIN";
                    Dt.Columns[24].ColumnName = "TM 1 Day CM Delivery IN"; //"TM1DayCMDeliveryIN";
                    Dt.Columns[25].ColumnName = "TDay Value Of CM Trades"; //"TDayValueOfCMTrades";
                    Dt.Columns[26].ColumnName = "Total Funds Security To Retain"; //"TotalFundsSecuToRetain";
                    Dt.Columns[27].ColumnName = "Excess Shortage"; //"ExcessShortage";
                    Dt.Columns[28].ColumnName = "Amount To Return"; //"AmountToReturn";
                    Dt.Columns[29].ColumnName = "Funds Release"; //"FundsRelease";
                    Dt.Columns[30].ColumnName = "Securities Release"; //"SecuritiesRelease";       
                }
                else
                {
                    Dt.Columns[0].ColumnName = "Financial Year";//"FinYear";
                    Dt.Columns[1].ColumnName = "Date Of Settlement";//"DateOfSettlement";
                    //Dt.Columns[2].ColumnName = "Quarter";
                    //Dt.Columns[3].ColumnName = "Client";
                    //Dt.Columns[4].ColumnName = "Branch";
                    //Dt.Columns[5].ColumnName = "Ledger";
                    Dt.Columns[6].ColumnName = "Margin Ledger"; //"MarginLedger";
                    Dt.Columns[7].ColumnName = "TDay FO Bill Reversal"; //"TDayFOBillReversal";
                    Dt.Columns[8].ColumnName = "TDay CD Bill Reversal"; //"TDayCDBillReversal";
                    Dt.Columns[9].ColumnName = "TDay CM Bill Reversal"; //"TDayCMBillReversal";
                    Dt.Columns[10].ColumnName = "TM 1 Day CM Bill Reversal"; //"TM1DayCMBillReversal";
                    Dt.Columns[11].ColumnName = "Total Security"; //"TotalSecu";
                    Dt.Columns[12].ColumnName = "Total Funds And Secu"; //"TotalFundsAndSecu";ok
                    Dt.Columns[13].ColumnName = "ToDay FO Margin"; //"TDayFOMargin";
                    Dt.Columns[14].ColumnName = "ToDay CD Margin"; //"TDayCDMargin";
                    Dt.Columns[15].ColumnName = "Margin Markup";
                    Dt.Columns[16].ColumnName = "FOCD Margin Amount"; //"FOCDMarginAmount";
                    Dt.Columns[17].ColumnName = "TDay FO Payin"; //"TDayFOPayin";
                    Dt.Columns[18].ColumnName = "TDay CD Payin"; //"TDayCDPayin";
                    Dt.Columns[19].ColumnName = "TDay CM Payin"; //"TDayCMPayin";
                    Dt.Columns[20].ColumnName = "TM 1 Day CM Payin"; //"TM1DayCMPayin";
                    Dt.Columns[21].ColumnName = "TDay CM Delivery IN"; //"TDayCMDeliveryIN";
                    Dt.Columns[22].ColumnName = "TM 1 Day CM Delivery IN"; //"TM1DayCMDeliveryIN";
                    Dt.Columns[23].ColumnName = "TDay Value Of CM Trades"; //"TDayValueOfCMTrades";
                    Dt.Columns[24].ColumnName = "Total Funds Security To Retain"; //"TotalFundsSecuToRetain";
                    Dt.Columns[25].ColumnName = "Excess Shortage"; //"ExcessShortage";
                    Dt.Columns[26].ColumnName = "Amount To Return"; //"AmountToReturn";
                    Dt.Columns[27].ColumnName = "Funds Release"; //"FundsRelease";
                    Dt.Columns[28].ColumnName = "Securities Release"; //"SecuritiesRelease";  
                }
                DataColumn dcolSerialNo = Dt.Columns.Add("Srl No", System.Type.GetType("System.String"));
                dcolSerialNo.SetOrdinal(0);
                for (int i = 1; i <= Dt.Rows.Count; i++)
                {
                    Dt.Rows[i - 1]["Srl No"] = i;
                }
                Dt.AcceptChanges();
                string grpBy = null;
                string strReportHeader = null;
                if ((ddlGroup.SelectedItem.Text) != null) grpBy = ddlGroup.SelectedItem.Text;
                //if((ddlgrouptype.SelectedItem.Text)!=null) grpTypeBy=ddlgrouptype.SelectedItem.Text;
                strReportHeader = "Showing Result Search By Date  Between  " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "  to " + oconverter.ArrangeDate2(dtto.Value.ToString());
                if (grpBy != null)
                {
                    strReportHeader += " and Filter By " + grpBy;
                }
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "QuarterSettleFund_" + exlTime;
                strDownloadFileName = "~/Documents/";
                string[] strHead = new string[3];
                strHead[0] = exlDateTime;
                strHead[1] = strReportHeader;
                strHead[2] = "Quarterly Settlements Of Funds  Of " + ViewState["CompanyName"].ToString() + " Company";

                if ((Dt.Columns.Contains("Group")) && (Dt.Columns.Contains("Group_Type")))//Group & Group_Type for search By group                                                  //Margin Markup
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "20", "20", "20", "10", "50", "10", "30", "20", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18", "18,8", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,8", "18,8", "18,8", "18,2", "18,2" };
                    string[] ColumnWidthSize = { "10", "15", "15", "5", "50", "30", "30", "20", "15", "15", "15", "15", "5", "5", "15", "15", "15", "15", "12", "20", "15", "15", "15", "15", "15", "15", "15", "20", "20", "20", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                else   //Without Group & Group_Type for search By Branch and Clients
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "20", "20", "20", "10", "50", "10", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18", "18,8", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,2", "18,8", "18,8", "18,8", "18,2", "18,2" };
                    string[] ColumnWidthSize = { "10", "15", "15", "5", "50", "30", "15", "15", "15", "15", "5", "5", "15", "15", "15", "15", "12", "20", "15", "15", "15", "15", "15", "15", "15", "20", "20", "20", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
            }
        }
    }
}