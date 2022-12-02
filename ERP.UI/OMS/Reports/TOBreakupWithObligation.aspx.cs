using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_TOBreakupWithObligation : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Local Variable
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //public string dp;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        string data;
        string segmentType = null;
        string segmentsList = null;
        string segSNameIDList = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
           // //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (HttpContext.Current.Session["Segmentname"] != null)
            {
                string strSegmentName = HttpContext.Current.Session["Segmentname"].ToString();
                hdnDPSessionValue.Value = strSegmentName;
            }
            if (!IsPostBack)
            {
                bindFrmToDate();
                PopulateExchange_SegmentDetail();
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//        
        }

        void PopulateExchange_SegmentDetail()
        {
            DataTable DtSegmentType = oDBEngine.GetCompanyDetail(Session["LastCompany"].ToString(), Session["UserSegID"].ToString());
            segmentType = DtSegmentType.Rows[0]["Exch_SegmentID"].ToString();
            DtSegmentType.Clear();
            DtSegmentType = oDBEngine.GetCompanyDetail(Session["LastCompany"].ToString());
            string exchSNameList = null;
            string all_exch_Id = null;
            if (DtSegmentType.Rows.Count > 0)
            {
                string expression;
                string segments = null;
                string exchSName = null;
                string exch_id = null;
                string segSNameID = null;
                expression = "Exch_SegmentID='" + segmentType + "'";
                DataRow[] foundRows;
                foundRows = DtSegmentType.Select(expression);
                for (int i = 0; i < foundRows.Length; i++)
                {
                    if (i == 0)
                    {
                        segments = foundRows[i][4] + " - " + segmentType;
                        exchSName = "'" + foundRows[i][4].ToString() + "'";
                        exch_id = foundRows[i][3].ToString();
                        segSNameID = foundRows[i][4].ToString() + "~" + foundRows[i][3].ToString();
                    }
                    else if (i == foundRows.Length - 1)
                    {
                        segments = " & " + foundRows[i][4] + " - " + segmentType;
                        exchSName = ",'" + foundRows[i][4].ToString() + "'";
                        exch_id = "," + foundRows[i][3].ToString();
                        segSNameID = "," + foundRows[i][4].ToString() + "~" + foundRows[i][3].ToString();
                    }
                    else
                    {
                        segments = ", " + foundRows[i][4] + " - " + segmentType;
                        exchSName = ",'" + foundRows[i][4].ToString() + "'";
                        exch_id = "," + foundRows[i][3].ToString();
                        segSNameID = "," + foundRows[i][4].ToString() + "~" + foundRows[i][3].ToString();
                    }
                    segmentsList += segments;
                    exchSNameList += exchSName;
                    all_exch_Id += exch_id;
                    segSNameIDList += segSNameID;
                }
            }
            //===========Fill Segment DropDownList====================
            ddlSeg.Items.Clear();
            DataTable dtExchange = oDBEngine.GetDataTable("tbl_master_exchange", "exh_shortName,exh_name", "exh_shortName in (" + exchSNameList + ")");
            ddlSeg.DataSource = dtExchange;
            if (dtExchange.Rows.Count > 0)
            {
                ddlSeg.Items.Add(new ListItem("Select Segment", "0"));
                ddlSeg.Items.Add(new ListItem("ALL", segSNameIDList));
                string[] strSplit = segSNameIDList.Split(',');
                string seg = string.Empty;
                string exch = string.Empty;
                for (int i = 0; i <= strSplit.Length - 1; i++)
                {
                    exch = strSplit[i].Split('~')[0];
                    seg = strSplit[i].Split('~')[1];
                    for (int j = 0; j <= (dtExchange.Rows.Count - 1); j++)
                    {
                        if (exch == dtExchange.Rows[j]["exh_shortName"].ToString())
                        {
                            ddlSeg.Items.Add(new ListItem(dtExchange.Rows[j]["exh_name"].ToString(), strSplit[i]));
                        }
                    }
                }
                dtExchange.Dispose();
            }
            else
            {
                ddlSeg.Items.Insert(0, new ListItem("Select Segment", "0"));
            }
        }

        void bindFrmToDate()
        {
            int transMonth, transYear;
            if (oDBEngine.GetDate().Month == 1)
            {
                transMonth = 12;
                transYear = oDBEngine.GetDate().Year - 1;
            }
            else
            {
                transMonth = oDBEngine.GetDate().Month - 1;
                transYear = oDBEngine.GetDate().Year;
            }
            DateTime firstDay = new DateTime(transYear, transMonth, 1);
            DateTime lastDayOfMonth = firstDay.AddMonths(1).AddTicks(-1);
            string month = String.Format("{0:MM}", lastDayOfMonth);
            string date = String.Format("{0:dd-MM-yyyy}", lastDayOfMonth);
            string[] dateSplit = date.Split('-');

            dtto.Text = dateSplit[0] + "-" + month + "-" + dateSplit[2];

            month = String.Format("{0:MM}", firstDay);
            date = String.Format("{0:dd-MM-yyyy}", firstDay);
            dateSplit = date.Split('-');

            dtfrom.Text = dateSplit[0] + "-" + month + "-" + dateSplit[2];
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
            DataTable DtSegType = oDBEngine.GetCompanyDetail(Session["LastCompany"].ToString(), Session["UserSegID"].ToString());
            string segType = DtSegType.Rows[0]["Exch_SegmentID"].ToString();
            DtSegType.Dispose();
            string segId = null;
            string segName = null;
            string segIds = null;
            string segNames = null;
            string segValue = ddlSeg.SelectedValue.ToString();
            if (segValue.Contains(","))
            {
                string[] strSplit = ddlSeg.SelectedValue.ToString().Split(',');
                for (int i = 0; i <= strSplit.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        segName = strSplit[i].Substring(0, strSplit[i].LastIndexOf('~')).ToString() + " - " + segType;
                        segId = strSplit[i].Substring(strSplit[i].LastIndexOf('~') + 1).ToString();
                    }
                    else if (i == strSplit.Length - 1)
                    {
                        segName = " & " + strSplit[i].Substring(0, strSplit[i].LastIndexOf('~')).ToString() + " - " + segType;
                        segId = "," + strSplit[i].Substring(strSplit[i].LastIndexOf('~') + 1).ToString();
                    }
                    else
                    {
                        segName = "," + strSplit[i].Substring(0, strSplit[i].LastIndexOf('~')).ToString() + " - " + segType;
                        segId = "," + strSplit[i].Substring(strSplit[i].LastIndexOf('~') + 1).ToString();
                    }
                    segNames += segName;
                    segIds += segId;
                }
            }
            else
            {
                segName = segValue.Substring(0, segValue.LastIndexOf('~')).ToString() + " - " + segType;
                segNames = segName;

                segId = segValue.Substring(segValue.LastIndexOf('~') + 1).ToString();
                segIds = segId;
            }
            ViewState["Segment"] = segNames;

            string[] InputName = new string[11];
            string[] InputType = new string[11];
            string[] InputValue = new string[11];

            InputName[0] = "CompanyID";
            InputName[1] = "FinYear";
            InputName[2] = "SegType";
            InputName[3] = "DateFrom";
            InputName[4] = "DateTo";
            InputName[5] = "GrpBy";
            InputName[6] = "GrpByID";
            InputName[7] = "GrpType";
            InputName[8] = "ReportType";
            InputName[9] = "SegmentID";
            InputName[10] = "consolidateRecord";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "D";
            InputType[4] = "D";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "C";

            InputValue[0] = Session["LastCompany"].ToString();
            InputValue[1] = Session["LastFinYear"].ToString();
            InputValue[2] = segType;
            InputValue[3] = dtfrom.Date.ToString();
            InputValue[4] = dtto.Date.ToString();
            if (ddlGroup.SelectedItem.Value.ToString() == "0")/////option Branch 
            {
                InputValue[5] = "Branch";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[6] = "All";
                    InputValue[7] = "";
                }
                else
                {
                    InputValue[6] = HiddenField_Branch.Value;
                    InputValue[7] = "";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////option Group
            {
                InputValue[5] = "Group";
                string groupType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    InputValue[6] = "All";
                    InputValue[7] = groupType;
                }
                else
                {
                    InputValue[6] = HiddenField_Group.Value;
                    InputValue[7] = groupType;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")/////option Clients
            {
                InputValue[5] = "Client";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[6] = "All";
                    InputValue[7] = "";
                }
                else
                {
                    InputValue[6] = HiddenField_Client.Value.Replace("'", "");
                    InputValue[7] = "";
                }
            }
            else
            {
                InputValue[5] = "Branch";
                InputValue[6] = "All";
                InputValue[7] = "";
            }
            InputValue[8] = ddlRptType.SelectedValue.ToString();
            InputValue[9] = segIds;
            if (rdoConsolidateRec.Checked == true)
                InputValue[10] = "T";
            else
                InputValue[10] = "F";

            DataTable dtData = SQLProcedures.SelectProcedureArr("Report_TOBreakupWithObligation", InputName, InputType, InputValue);
            return dtData;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable resultDt = null;
            if (dtfrom.Value == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(1);", true);
            }
            else if (dtto.Value == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(2);", true);
            }
            else if (ddlRptType.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(3);", true);
            }
            else if (ddlSeg.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(4);", true);
            }
            else
            {
                resultDt = BindData();
                ExportToExcel_Generic(resultDt, "2007");
            }
        }

        protected bool ValidateExportData(DataTable Dt)
        {
            bool result = true;
            if (Dt.Rows.Count > 1)
            {
                if ((ddlRptType.SelectedValue == "Summary") && (rdoConsolidateRec.Checked == true))
                {
                    if (ddlGroup.SelectedValue == "0")
                    {
                        if (Dt.Rows[0]["BranchName"].ToString().Contains("Grand Total"))
                        {
                            result = false;
                        }
                    }
                    else if (ddlGroup.SelectedValue == "1")
                    {
                        if (Dt.Rows[0]["GroupName"].ToString().Contains("Grand Total"))
                        {
                            result = false;
                        }
                    }
                    else if (ddlGroup.SelectedValue == "1")
                    {
                        if (Dt.Rows[0]["ClientName"].ToString().Contains("Grand Total"))
                        {
                            result = false;
                        }
                    }
                }
                else
                {
                    if (Dt.Rows[0]["ClientName"].ToString().Contains("Grand Total"))
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected void ExportToExcel_Generic(DataTable Dt, string ExcelVersion)
        {
            if (ValidateExportData(Dt) == true)
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string grpBy = null;
                string reportType = null;
                string strReportHeader = null;
                strReportHeader = "For the Date Between " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "  to " + oconverter.ArrangeDate2(dtto.Value.ToString()) + ". Segments : " + ViewState["Segment"];
                if ((ddlGroup.SelectedItem.Text) != null) grpBy = ddlGroup.SelectedItem.Text;
                if ((ddlRptType.SelectedItem.Value) != "0") reportType = ddlRptType.SelectedItem.Text;
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "TOBreakupWithObligation_" + exlTime;
                strDownloadFileName = "~/Documents/";
                string[] strHead = new string[4];
                strHead[0] = exlDateTime;
                strHead[1] = strReportHeader;
                strHead[2] = "Futures & Options TO With Obligation - " + ddlRptType.SelectedItem.Value;
                DataTable dtcompname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                strHead[3] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();
                if (ddlRptType.SelectedValue == "Detail")//Detail
                {
                    //Excel Report with  Total and Grand Total On Branch wise, Client wise and Group wise                
                    if (ddlGroup.SelectedValue == "0")  // Branch
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "20", "15", "15", "18,0", "28,2", "10,6", "18,2", "18,0", "28,2", "10,6", "18,2", "18,0", "28,2", "18,2", "28,2", "18,2", "28,2" };
                        string[] ColumnWidthSize = { "30", "10", "10", "10", "15", "20", "10", "10", "15", "15", "10", "10", "15", "15", "10", "15", "10", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else if (ddlGroup.SelectedValue == "1" || ddlGroup.SelectedValue == "2")  //Detail -> Group & Client
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "20", "150", "15", "15", "18,0", "28,2", "10,6", "18,2", "18,0", "28,2", "10,6", "18,2", "18,0", "28,2", "18,2", "28,2", "18,2", "28,2" };
                        string[] ColumnWidthSize = { "30", "10", "30", "10", "10", "15", "20", "10", "10", "15", "15", "10", "10", "15", "15", "10", "15", "10", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                if (ddlRptType.SelectedValue == "Summary" && rdoConsolidateRec.Checked == true)//Summary wise 
                {
                    //Excel Report with  Total and Grand Total On Branch wise and Group wise                
                    if (ddlGroup.SelectedValue == "0" || ddlGroup.SelectedValue == "1")
                    {
                        string[] ColumnType = { "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "15", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "28,2", "18,2", "28,2" };
                        string[] ColumnWidthSize = { "30", "10", "15", "15", "10", "15", "15", "10", "15", "15", "10", "15", "10", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else if (ddlGroup.SelectedValue == "2")
                    { //Excel Report with  Total and Grand Total On Client wise 
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "20", "150", "15", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "28,2", "18,2", "28,2" };
                        string[] ColumnWidthSize = { "30", "10", "30", "10", "15", "15", "10", "15", "15", "10", "15", "15", "10", "15", "10", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
                else if (ddlRptType.SelectedValue == "Summary" && rdoConsolidateRec.Checked == false)//Summary wise 
                {
                    //Excel Report with  Total and Grand Total On Branch wise and Group wise                
                    if (ddlGroup.SelectedValue == "0" || ddlGroup.SelectedValue == "1")
                    {
                        string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "15", "15", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "28,2", "18,2", "28,2" };
                        string[] ColumnWidthSize = { "30", "10", "10", "15", "15", "10", "15", "15", "10", "15", "15", "10", "15", "10", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    else if (ddlGroup.SelectedValue == "2")
                    { //Excel Report with  Total and Grand Total On Client wise 
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "20", "150", "15", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "18,0", "28,2", "18,2", "28,2", "18,2", "28,2" };
                        string[] ColumnWidthSize = { "30", "10", "30", "10", "15", "15", "10", "15", "15", "10", "15", "15", "10", "15", "10", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(2);", true);
            }
        }
    }
}