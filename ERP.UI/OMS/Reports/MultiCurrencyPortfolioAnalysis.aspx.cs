using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_MultiCurrencyPortfolioAnalysis : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Local Variable
        GenericMethod oGenericMethod;
        GenericStoreProcedure oGenericStoreProcedure;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        AspxHelper oAspxHelper;
        BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
        string CombinedGroupByQuery = null;
        string data;
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] PageSession = { null };
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
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad12", "<script>PageLoad();</script>");
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (!IsPostBack)
            {
                dtFrom.Date = oDBEngine.GetDate();
                dtTo.Date = oDBEngine.GetDate();
                Page.ClientScript.RegisterStartupScript(GetType(), "Reset", "<script>Reset();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___// 

        }

        #region CallAjax
        void CallUserList(string WhichCall)
        {
            oGenericMethod = new GenericMethod();
            string CombinedBranchQuery = string.Empty;
            CombinedGroupByQuery = CombinedBranchQuery;
            if (WhichCall == "CallAjax-Branch")
            {
                CombinedBranchQuery = oGenericMethod.GetAllBranch();
                CombinedGroupByQuery = CombinedBranchQuery;
            }
            if (WhichCall.Contains("CallAjax-Group"))
            {
                string groupType = WhichCall.Split('~')[1].ToString();
                string CombinedGroupQuery = oGenericMethod.GetAllGroups(groupType);
                CombinedGroupByQuery = CombinedGroupQuery;
            }
            if (WhichCall == "CallAjax-Client")
            {
                string CombinedClientQuery = oGenericMethod.GetAllContact("CL");
                CombinedGroupByQuery = CombinedClientQuery;
            }
            if (WhichCall == "CallAjax-Asset")
            {
                string CombinedAssetQuery = oGenericMethod.GetUnderLyingAssets("A", Convert.ToInt32(HttpContext.Current.Session["usersegid"]));
                CombinedGroupByQuery = CombinedAssetQuery;
            }
            if (WhichCall == "CallAjax-UserEmail")
            {
                oGenericMethod.GetUserForSendMail("A", ref CombinedGroupByQuery, 10);
            }
        }
        #endregion

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id == "CallAjax-Client" || id == "CallAjax-Branch" || id.Contains("CallAjax-Group") || id == "CallAjax-Asset" || id == "CallAjax-UserEmail")
            {
                CallUserList(id);
                CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
                data = "AjaxQuery!" + CombinedGroupByQuery;
            }
            else
            {
                string[] idlist = id.Split('^');
                string recieveServerIDs = "";
                for (int i = 0; i < idlist.Length; i++)
                {
                    string[] strVal = idlist[i].Split('|');
                    string[] ids = strVal[0].Split('~');
                    string whichCall = ids[ids.Length - 1];
                    if (whichCall == "BRANCH")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = ids[0];
                        else
                            recieveServerIDs += "," + ids[0];
                        data = "Branch!" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "GROUP")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = ids[0];
                        else
                            recieveServerIDs += "," + ids[0];
                        data = "Group!" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "ASSET")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = ids[0];
                        else
                            recieveServerIDs += "," + ids[0];
                        data = "Asset!" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "CLIENT")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = "'" + ids[4] + "'";
                        else
                            recieveServerIDs += ",'" + ids[4] + "'";
                        data = "Client!" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "USERBYEMAIL")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = ids[5] + "~" + ids[4].ToLower();
                        else
                            recieveServerIDs += "#" + ids[5] + "~" + ids[4].ToLower();
                        data = "UserEmail!" + recieveServerIDs.ToString();
                    }
                }
            }
        }

        protected void CmbReportStyle_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            string CallByReportType = e.Parameter.Split('~')[1];
            if (WhichCall == "ReportStyle")
            {
                if (CallByReportType == "Obligation")
                {
                    CmbReportStyle.Items.Clear();
                    CmbReportStyle.Items.Insert(0, new ListEditItem("Select Report Style", "0"));
                    CmbReportStyle.Items.Add("Date Wise", "DateWise");
                    CmbReportStyle.Items.Add("Consolidated", "Consolidated");
                }
                else if (CallByReportType == "OpenPosition")
                {
                    CmbReportStyle.Items.Clear();
                    CmbReportStyle.Items.Insert(0, new ListEditItem("Select Report Style", "0"));
                    CmbReportStyle.Items.Add("Date Wise", "DateWise");
                }
            }
        }

        protected void CmbReport_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            string CallByReportType = e.Parameter.Split('~')[1];
            string CallByReportStyle = e.Parameter.Split('~')[2];
            if (WhichCall == "Report")
            {
                CmbReport.Items.Clear();
                CmbReport.Items.Insert(0, new ListEditItem("Select Report", "0"));
                if (CallByReportStyle == "DateWise")
                {
                    if (CallByReportType == "Obligation")
                    {
                        CmbReport.Items.Add("Client+Date+Series", "1");
                        CmbReport.Items.Add("Client+Date+Asset+Exchange", "3");
                        CmbReport.Items.Add("Client+Date+Asset", "5");
                        CmbReport.Items.Add("Client+Date", "7");
                    }
                    else
                    {
                        CmbReport.Items.Add("Client+Date+Series", "9");
                        CmbReport.Items.Add("Client+Date+Asset+Exchange", "10");
                        CmbReport.Items.Add("Client+Date+Asset", "11");
                        CmbReport.Items.Add("Date+Series+Client", "12");
                        CmbReport.Items.Add("Date+Series", "13");
                        CmbReport.Items.Add("Date+Asset+Exchange+Client", "18");
                        CmbReport.Items.Add("Date+Asset+Exchange", "16");
                        CmbReport.Items.Add("Date+Asset+Client", "19");
                        CmbReport.Items.Add("Date+Asset", "17");
                    }
                }
                else
                {
                    if (CallByReportType == "Obligation")
                    {
                        CmbReport.Items.Add("Client+Series", "2");
                        CmbReport.Items.Add("Client+Asset+Exchange", "4");
                        CmbReport.Items.Add("Client+Asset", "6");
                        CmbReport.Items.Add("Asset+Exchange+Client", "14");
                        CmbReport.Items.Add("Asset+Exchange", "15");
                        CmbReport.Items.Add("Asset+Client", "20");
                        CmbReport.Items.Add("Asset", "21");
                        CmbReport.Items.Add("Client", "8");
                    }
                }
            }
        }

        protected void CmbReportBy_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            string CallByReport = e.Parameter.Split('~')[1];
            if (WhichCall == "ReportBy")
            {
                CmbReportBy.Items.Clear();
                CmbReportBy.Items.Insert(0, new ListEditItem("Select Report Output", "0"));
                CmbReportBy.Items.Add("Excel", "1");
                CmbReportBy.Items.Add("Email", "2");
            }
        }

        protected void CmbGroupBy_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CmbGroupBy.JSProperties["cpShowGroupBy"] = null;
            CmbGroupBy.JSProperties["cpBindUserOnly"] = null;
            CmbGroupBy.JSProperties["cpEnablebtnResult"] = null;
            string WhichCall = e.Parameter.Split('~')[0];
            string CallByReport = e.Parameter.Split('~')[1];
            string CallByReportBy = e.Parameter.Split('~')[2];
            if (WhichCall == "GroupBy")
            {
                if ((CallByReport == "12") || (CallByReport == "13") || (CallByReport == "14") || (CallByReport == "15")
                    || (CallByReport == "16") || (CallByReport == "17") || (CallByReport == "18") || (CallByReport == "19")
                    || (CallByReport == "20") || (CallByReport == "21"))
                {
                    if (CallByReportBy == "1") // For excel  
                    {
                        CmbGroupBy.JSProperties["cpShowGroupBy"] = "N";     //1. DeActive Group By Opt
                        CmbGroupBy.JSProperties["cpBindUserOnly"] = "N";    //2. Hide Mail send Option
                        CmbGroupBy.JSProperties["cpEnablebtnResult"] = "Y"; //3. Active Button
                    }
                    else if (CallByReportBy == "2") // For email  
                    {
                        CmbGroupBy.JSProperties["cpShowGroupBy"] = "N";     //1. DeActive Group By Opt
                        CmbGroupBy.JSProperties["cpBindUserOnly"] = "Y";    //2. Show mail send Option to only User
                        CmbGroupBy.JSProperties["cpEnablebtnResult"] = "N"; //3. Disable Button
                    }
                }
                else
                {
                    CmbGroupBy.Items.Clear();
                    CmbGroupBy.Items.Insert(0, new ListEditItem("Select Group By", "0"));
                    CmbGroupBy.Items.Add("Branch", "B");
                    CmbGroupBy.Items.Add("Group", "G");
                    CmbGroupBy.Items.Add("Client", "C");

                    if (CallByReportBy == "1") // For excel 
                    {
                        CmbGroupBy.JSProperties["cpShowGroupBy"] = "Y";      //1. Active Group By Opt
                        CmbGroupBy.JSProperties["cpBindUserOnly"] = "N";   //2. Hide Mail send Option 
                        CmbGroupBy.JSProperties["cpEnablebtnResult"] = "N";  //3. Disable Button
                    }
                    else if (CallByReportBy == "2") // For email
                    {
                        CmbGroupBy.JSProperties["cpShowGroupBy"] = "Y";       //1. Active Group By Opt 
                        CmbGroupBy.JSProperties["cpBindUserOnly"] = "N";    //2. Hide mail send Option 
                        CmbGroupBy.JSProperties["cpEnablebtnResult"] = "N";   //3. Disable Button
                    }
                }
            }
        }

        protected int BindGroupType()
        {
            int result = 0;
            CmbGroupType.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type as ID,gpm_Type as Value", null);
            if (DtGroup.Rows.Count > 0)
            {
                oAspxHelper = new AspxHelper();
                oAspxHelper.Bind_Combo(CmbGroupType, DtGroup, "ID", "Value", 0);
                CmbGroupType.Items.Insert(0, new ListEditItem("Select GroupType", "0"));
                DtGroup.Dispose();
                result = 1;
            }
            return result;
        }

        protected void CmbGroupType_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CmbGroupType.JSProperties["cpBindGroupType"] = null;
            CmbGroupType.JSProperties["cpSendMailTo"] = null;
            int bindResult = BindGroupType();
            if (bindResult == 1)
                CmbGroupType.JSProperties["cpBindGroupType"] = "Y";
            else
                CmbGroupType.JSProperties["cpBindGroupType"] = "N";

            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "GroupTypeWithSendMail")
                CmbGroupType.JSProperties["cpSendMailTo"] = "Y";
        }

        protected void CmbMailSendTo_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CmbMailSendTo.Items.Clear();
            CmbMailSendTo.Items.Insert(0, new ListEditItem("Select Mail Recipient", "0"));
            string WhichCall = e.Parameter.Split('~')[0];
            string CallByReport = e.Parameter.Split('~')[1];
            if (WhichCall == "MailToBranch")
            {
                if ((CallByReport == "12") || (CallByReport == "13") || (CallByReport == "14") || (CallByReport == "15")
                    || (CallByReport == "16") || (CallByReport == "17") || (CallByReport == "18") || (CallByReport == "19")
                    || (CallByReport == "20") || (CallByReport == "21"))
                {
                    CmbMailSendTo.Items.Add("Mail To User", "4");
                }
                else
                {
                    CmbMailSendTo.Items.Add("Mail To Branch", "1");
                    CmbMailSendTo.Items.Add("Mail To Client", "3");
                    CmbMailSendTo.Items.Add("Mail To User", "4");
                }
            }
            else if (WhichCall == "MailToGroup")
            {
                if ((CallByReport == "12") || (CallByReport == "13") || (CallByReport == "14") || (CallByReport == "15")
                    || (CallByReport == "16") || (CallByReport == "17") || (CallByReport == "18") || (CallByReport == "19")
                    || (CallByReport == "20") || (CallByReport == "21"))
                {
                    CmbMailSendTo.Items.Add("Mail To User", "4");
                }
                else
                {
                    CmbMailSendTo.Items.Add("Mail To Group", "2");
                    CmbMailSendTo.Items.Add("Mail To Client", "3");
                    CmbMailSendTo.Items.Add("Mail To User", "4");
                }
            }
            else if (WhichCall == "MailToClient")
            {
                if ((CallByReport == "12") || (CallByReport == "13") || (CallByReport == "14") || (CallByReport == "15")
                     || (CallByReport == "16") || (CallByReport == "17") || (CallByReport == "18") || (CallByReport == "19")
                     || (CallByReport == "20") || (CallByReport == "21"))
                {
                    CmbMailSendTo.Items.Add("Mail To User", "4");
                }
                else
                {
                    CmbMailSendTo.Items.Add("Mail To Client", "3");
                    CmbMailSendTo.Items.Add("Mail To User", "4");
                }
            }
            else if (WhichCall == "MailToOnlyUser")
            {
                CmbMailSendTo.Items.Add("Mail To User", "4");
            }
        }

        protected bool Check_Validation()
        {
            bool condition = true;
            string report = CmbReport.SelectedItem.Value.ToString();

            if (dtFrom.Value == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate1", "Validate(1);", true);
                condition = false;
            }
            if (dtTo.Value == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate2", "Validate(2);", true);
                condition = false;
            }
            if (CmbReportType.SelectedItem.Value.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate3", "Validate(3);", true);
                condition = false;
            }
            if (CmbReportStyle.SelectedItem.Value.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate4", "Validate(4);", true);
                condition = false;
            }
            if (CmbReport.SelectedItem.Value.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate5", "Validate(5);", true);
                condition = false;
            }
            if (CmbReportBy.SelectedItem.Value.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate6", "Validate(6);", true);
                condition = false;
            }
            if ((report == "1") || (report == "2") || (report == "3") || (report == "4") || (report == "5") || (report == "6")
                 || (report == "7") || (report == "8") || (report == "9") || (report == "10") || (report == "11"))
            {
                if (CmbGroupBy.SelectedItem.Value.ToString() == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate7", "Validate(7);", true);
                    condition = false;
                }
                else if (CmbGroupBy.SelectedItem.Value.ToString() == "G")
                {
                    if (CmbGroupType.SelectedItem.Value.ToString() == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate8", "Validate(8);", true);
                        condition = false;
                    }
                }
            }
            if (CmbReportBy.SelectedItem.Value.ToString() == "2")
            {
                if (CmbMailSendTo.SelectedItem.Value.ToString() == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate9", "Validate(9);", true);
                    condition = false;
                }
            }
            return condition;
        }

        protected DataSet runProcedure()
        {
            string report = CmbReport.SelectedItem.Value.ToString();
            string assetID = null;

            string[] InputName = new string[10];
            string[] InputType = new string[10];
            string[] InputValue = new string[10];

            InputName[0] = "DateFrom";
            InputName[1] = "DateTo";
            InputName[2] = "Company";
            InputName[3] = "GrpBy";
            InputName[4] = "GrpByID";
            InputName[5] = "GrpType";
            InputName[6] = "ProductID";
            InputName[7] = "ReportType";
            InputName[8] = "ReportBy";
            InputName[9] = "MailTo";


            InputType[0] = "D";
            InputType[1] = "D";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "I";
            InputType[8] = "I";
            InputType[9] = "V";


            InputValue[0] = dtFrom.Date.ToString();
            InputValue[1] = dtTo.Date.ToString();
            InputValue[2] = Session["LastCompany"].ToString();

            if ((report == "1") || (report == "2") || (report == "3") || (report == "4") || (report == "5") || (report == "6")
                || (report == "7") || (report == "8") || (report == "9") || (report == "10") || (report == "11"))
            {
                if (CmbGroupBy.SelectedItem.Value.ToString() == "B")/////option Branch 
                {
                    InputValue[3] = "BRANCH";
                    if (RdoGroupByAll.Checked)
                    {
                        InputValue[4] = "ALL";
                        InputValue[5] = "";
                    }
                    else
                    {
                        InputValue[4] = HiddenField_Branch.Value;
                        InputValue[5] = "";
                    }
                }
                else if (CmbGroupBy.SelectedItem.Value.ToString() == "G")/////option Group
                {
                    InputValue[3] = "GROUP";
                    if (RdoGroupByAll.Checked)
                    {
                        InputValue[4] = "ALL";
                        InputValue[5] = CmbGroupType.SelectedItem.Text.ToString().Trim();
                    }
                    else
                    {
                        InputValue[4] = HiddenField_Group.Value;
                        InputValue[5] = CmbGroupType.SelectedItem.Text.ToString().Trim();
                    }
                }
                else if (CmbGroupBy.SelectedItem.Value.ToString() == "C")/////option Clients
                {
                    InputValue[3] = "CLIENT";
                    if (RdoGroupByAll.Checked)
                    {
                        InputValue[4] = "ALL";
                        InputValue[5] = "";
                    }
                    else
                    {
                        InputValue[4] = HiddenField_Client.Value.Replace("'", "");
                        InputValue[5] = "";
                    }
                }
            }
            else
            {
                InputValue[3] = "BRANCH";
                InputValue[4] = "ALL";
                InputValue[5] = "";
            }
            if (RdoAssetAll.Checked == true)
                assetID = "ALL";
            else
                assetID = HiddenField_Asset.Value.Replace("'", "");
            InputValue[6] = assetID;
            InputValue[7] = report;
            InputValue[8] = CmbReportBy.SelectedItem.Value.ToString();
            if (CmbReportBy.SelectedItem.Value.ToString() == "2")
                InputValue[9] = CmbMailSendTo.SelectedItem.Value.ToString();
            else
                InputValue[9] = "0";

            DataSet dsData = SQLProcedures.SelectProcedureArrDS("Report_MultiCurrencyPortfolioAnalysis", InputName, InputType, InputValue);
            return dsData;
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            DataSet resultDS = null;
            if (Check_Validation() == true)
            {
                resultDS = runProcedure();
                if (resultDS != null)
                {
                    if (resultDS.Tables.Count > 0)
                    {
                        if (CmbReportBy.SelectedItem.Value.ToString() == "2")
                            ExportToHTML_Generic(resultDS, "2007");
                        else
                            ExportToExcel_Generic(resultDS, "2007");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", " alert('No Record Found.');", true);
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript1", "<script language='javascript'>Reset();</script>");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD4", " alert('No Record Found.');", true);
                    Page.ClientScript.RegisterStartupScript(GetType(), "jscript2", "<script language='javascript'>Reset();</script>");
                }
            }
        }

        protected void ExportToExcel_Generic(DataSet Ds, string ExcelVersion)
        {
            if (Ds.Tables[0].Rows.Count > 1)
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();

                # region Excel Report Header Content
                string strDownloadFileName = "";

                string strReportHeader = null;
                // oConverter = new Converter();
                strReportHeader = "For the Date Between " + oConverter.ArrangeDate2(dtFrom.Value.ToString()) + "  to " + oConverter.ArrangeDate2(dtTo.Value.ToString()) + ".";

                string searchCriteria = null;
                searchCriteria = "Search By ";
                if (RdoAssetAll.Checked == true)
                    searchCriteria += " All Asset Wise.";
                else
                    searchCriteria += " Selected Asset Wise.";

                string report = CmbReport.SelectedItem.Value.ToString();
                if ((report == "1") || (report == "2") || (report == "3") || (report == "4") || (report == "5") || (report == "6")
                || (report == "7") || (report == "8") || (report == "9") || (report == "10") || (report == "11"))
                {
                    if (RdoGroupByAll.Checked == true)
                        searchCriteria += " And All " + CmbGroupBy.SelectedItem.Text.ToString().Trim();
                    else if (RdoGroupBySelected.Checked == true)
                        searchCriteria += " And Selected " + CmbGroupBy.SelectedItem.Text.ToString().Trim();
                    if ((CmbGroupBy.SelectedItem.Value.ToString() == "G") && (CmbGroupType.SelectedItem.Value.ToString() != "0"))
                        searchCriteria += " With " + CmbGroupType.SelectedItem.Text.ToString() + " GroupType ";
                }
                string ReportCriteria = null;
                ReportCriteria = "Report Type - " + CmbReportType.SelectedItem.Text.ToString() + ", Report Style - " + CmbReportStyle.SelectedItem.Text.ToString() + ", Report - " + CmbReport.SelectedItem.Text.ToString() + ".";

                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");

                string FileName = "MultiCurAnalysis_" + exlTime;
                strDownloadFileName = "~/Documents/";

                string[] strHead = new string[6];
                strHead[0] = exlDateTime;
                strHead[1] = searchCriteria;
                strHead[2] = ReportCriteria;
                strHead[3] = strReportHeader;
                strHead[4] = "Multi Currency PortFolio Analysis Report";
                DataTable dtcompname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                strHead[5] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();
                # endregion

                //Remove columns for show data without unwanted columns
                DataTable Dt = Ds.Tables[0];
                Dt = RemoveColumnForEmail(Dt);

                #region Excel Format For Different Report Type
                if (CmbReport.SelectedItem.Value.ToString() == "21") //T Obligation & Exposure -> Consolidated -> Asset
                {
                    string[] ColumnType = { "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "5", "12", "12", "15", "15", "12", "12", "5" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "20") //T Obligation & Exposure -> Consolidated -> Asset+Client
                {
                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "19") // Open Position -> Date Wise -> Date+Asset+Client
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "18") // Open Position -> Date Wise -> Date+Asset+Exchange+Client
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "15", "150", "5", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "17") // Open Position -> Date Wise -> Date+Asset
                {
                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "16") // Open Position -> Date Wise -> Date+Asset+Exchange
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "5", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "5", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "15") //T Obligation & Exposure -> Consolidated -> Asset+Exchange
                {


                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "5", "5", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "24,2", "24,2", "24,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "5", "5", "12", "12", "12", "12", "12", "12", "12", "12", "15", "12", "15", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "14") //T Obligation & Exposure -> Consolidated -> Asset+Exchange+Client
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "5", "150", "5", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "24,2", "24,2", "24,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "5", "20", "5", "12", "12", "12", "12", "12", "12", "12", "12", "15", "12", "15", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "13") // Open Position -> Date Wise -> Date+Series
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "15", "5", "18,4", "150", "12", "20,4", "20,4", "20,0", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "10", "5", "15", "15", "5", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "12") // Open Position -> Date Wise -> Date+Series+Client
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "15", "5", "18,4", "150", "150", "20,0", "20,4", "28,2", "12", "20,4", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "10", "5", "15", "15", "20", "12", "12", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "11") // Open Position -> Date Wise -> Client+Date+Asset
                {
                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "12", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "10") // Open Position -> Date Wise -> Client+Date+Asset+Exchange
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "12", "20,0", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "5", "12", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "9") // Open Position -> Date Wise -> Client+Date+Series
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "15", "5", "18,4", "150", "20,0", "20,4", "28,4", "12", "20,4", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "10", "5", "12", "15", "12", "12", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "8") // Obligation & Exposure -> Date Wise -> Client
                {
                    string[] ColumnType = { "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "7") // Obligation & Exposure -> Date Wise -> Client+Date
                {
                    string[] ColumnType = { "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "10", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "6") // Obligation & Exposure -> Consolidated -> Client+Asset
                {
                    string[] ColumnType = { "V", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "28,0", "12", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "5") // Obligation & Exposure -> Date Wise -> Client+Date+Asset
                {
                    string[] ColumnType = { "V", "V", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "28,0", "12", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "4") // Obligation & Exposure -> Consolidated -> Client+Asset+Exchange
                {
                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "5", "12", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "5", "5", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "3") // Obligation & Exposure -> Date Wise -> Client+Date+Asset+Exchange
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "12", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "5", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "2") // Obligation & Exposure -> Consolidated -> Client+Series
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "5", "15", "5", "18,4", "10", "150", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "28,2", "12", "22,2", "20,2", "22,2", "20,4", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "20", "5", "10", "5", "12", "5", "15", "12", "12", "12", "12", "12", "12", "12", "12", "12", "5", "12", "12", "12", "12", "12", "12", "12", "15", "15", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (CmbReport.SelectedItem.Value.ToString() == "1") // Obligation & Exposure -> Date Wise -> Client+Date+Series
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "15", "150", "5", "15", "5", "18,4", "10", "150", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "28,2", "12", "22,2", "20,2", "22,2", "20,4", "28,2", "28,2", "20,8", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "20", "5", "10", "5", "12", "5", "15", "12", "12", "12", "12", "12", "12", "12", "12", "12", "5", "12", "12", "12", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                #endregion
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD2", "alert('No Record Found.');", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script language='javascript'>Reset();</script>");
            }
        }

        protected void ExportToHTML_Generic(DataSet Ds, string ExcelVersion)
        {
            if (Ds.Tables.Count > 0)
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();

                # region Email Report Header Content
                string strDownloadFileName = "";
                string strReportHeader = null;
                string searchCriteria = null;
                string ReportCriteria = null;
                //oConverter = new Converter();

                strReportHeader = "Multi Currency PortFolio Analysis Report For the Date Between " + oConverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oConverter.ArrangeDate2(dtTo.Value.ToString()) + ".";

                searchCriteria = "Search By";
                if (RdoAssetAll.Checked == true)
                    searchCriteria += " All Asset Wise.";
                else
                    searchCriteria += " Selected Asset Wise.";

                string report = CmbReport.SelectedItem.Value.ToString();
                if ((report == "1") || (report == "2") || (report == "3") || (report == "4") || (report == "5") || (report == "6")
                || (report == "7") || (report == "8") || (report == "9") || (report == "10") || (report == "11"))
                {
                    if (RdoGroupByAll.Checked == true)
                        searchCriteria += " And All " + CmbGroupBy.SelectedItem.Text.ToString().Trim();
                    else if (RdoGroupBySelected.Checked == true)
                        searchCriteria += " And Selected " + CmbGroupBy.SelectedItem.Text.ToString().Trim();
                    if ((CmbGroupBy.SelectedItem.Value.ToString() == "G") && (CmbGroupType.SelectedItem.Value.ToString() != "0"))
                        searchCriteria += " With " + CmbGroupType.SelectedItem.Text.ToString() + " GroupType";
                }
                ReportCriteria = "Report Type - " + CmbReportType.SelectedItem.Text.ToString() + ", Report Style - " + CmbReportStyle.SelectedItem.Text.ToString() + ", Report - " + CmbReport.SelectedItem.Text.ToString() + ".";

                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");

                string FileName = "MultiCurAnalysis_" + exlTime;
                strDownloadFileName = "~/Documents/";

                string[] strHead = new string[5];
                strHead[0] = exlDateTime;
                strHead[1] = searchCriteria;
                strHead[2] = ReportCriteria;
                strHead[3] = strReportHeader;
                DataTable dtcompname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                strHead[4] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();
                # endregion

                # region Declare Mail Sending Variables
                string branchID = string.Empty;
                string groupID = string.Empty;
                string branchEmail = string.Empty;
                string groupEmail = string.Empty;
                string groupCCEmail = string.Empty;
                string customerID = string.Empty;
                string clientEmail = string.Empty;
                string clientCCEmail = string.Empty;
                string usersList = null;
                string userID = string.Empty;
                string userEmail = string.Empty;
                //========Mail Sending SP Param Value=========
                string mailSubject = strReportHeader;
                string mailBody = string.Empty;
                string contactID = string.Empty;
                string mailID = string.Empty;
                # endregion

                for (int i = 0; i < Ds.Tables.Count; i++)
                {
                    DataTable Dt = Ds.Tables[i];

                    # region Set Mail Sending Variables
                    if (CmbMailSendTo.SelectedItem.Value.ToString() == "1")
                    {
                        if (Dt.Rows[0]["BranchID"].ToString() != string.Empty) branchID = Dt.Rows[0]["BranchID"].ToString();
                        if (Dt.Rows[0]["BranchEmail"].ToString() != string.Empty) branchEmail = Dt.Rows[0]["BranchEmail"].ToString();
                        contactID = branchID;
                        mailID = branchEmail;
                    }
                    else if (CmbMailSendTo.SelectedItem.Value.ToString() == "2")
                    {
                        if (Dt.Rows[0]["GroupID"].ToString() != string.Empty) groupID = Dt.Rows[0]["GroupID"].ToString();
                        if (Dt.Rows[0]["GroupEmail"].ToString() != string.Empty) groupEmail = Dt.Rows[0]["GroupEmail"].ToString();
                        if (Dt.Rows[0]["GroupCCEmail"].ToString() != string.Empty) groupCCEmail = Dt.Rows[0]["GroupCCEmail"].ToString();
                        contactID = groupID;
                        mailID = groupEmail;
                        if (groupCCEmail != string.Empty) mailID += "#" + groupCCEmail;
                        groupCCEmail = string.Empty;
                    }
                    else if (CmbMailSendTo.SelectedItem.Value.ToString() == "3")
                    {
                        if (Dt.Rows[0]["CustomerID"].ToString() != string.Empty) customerID = Dt.Rows[0]["CustomerID"].ToString();
                        if (Dt.Rows[0]["Email"].ToString() != string.Empty) clientEmail = Dt.Rows[0]["Email"].ToString();
                        if (Dt.Rows[0]["CCEmail"].ToString() != string.Empty) clientCCEmail = Dt.Rows[0]["CCEmail"].ToString();
                        contactID = customerID;
                        mailID = clientEmail;
                        if (clientCCEmail != string.Empty) mailID += "#" + clientCCEmail;
                        clientCCEmail = string.Empty;
                    }
                    else if (CmbMailSendTo.SelectedItem.Value.ToString() == "4")
                    {
                        usersList = HiddenField_UserEmail.Value;
                    }
                    # endregion

                    //Remove columns for show data without unwanted columns
                    Dt = RemoveColumnForEmail(Dt);

                    #region Excel Format For Different Report Type
                    if (CmbReport.SelectedItem.Value.ToString() == "21") //T Obligation & Exposure -> Consolidated -> Asset
                    {
                        string[] ColumnType = { "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "5", "12", "12", "15", "15", "12", "12", "5" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "20") //T Obligation & Exposure -> Consolidated -> Asset+Client
                    {
                        string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "19") // Open Position -> Date Wise -> Date+Asset+Client
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "18") // Open Position -> Date Wise -> Date+Asset+Exchange+Client
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "15", "150", "5", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "17") // Open Position -> Date Wise -> Date+Asset
                    {
                        string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "5", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "16") // Open Position -> Date Wise -> Date+Asset+Exchange
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "5", "5", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "5", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "15") //T Obligation & Exposure -> Consolidated -> Asset+Exchange
                    {
                        string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "5", "5", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "24,2", "24,2", "24,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "5", "5", "12", "12", "12", "12", "12", "12", "12", "12", "15", "12", "15", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "14") //T Obligation & Exposure -> Consolidated -> Asset+Exchange+Client
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "5", "150", "5", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "24,2", "24,2", "24,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "5", "20", "5", "12", "12", "12", "12", "12", "12", "12", "12", "15", "12", "15", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "13") // Open Position -> Date Wise -> Date+Series
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "5", "15", "5", "18,4", "150", "12", "20,4", "20,4", "20,0", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "10", "5", "15", "15", "5", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "12") // Open Position -> Date Wise -> Date+Series+Client
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "5", "15", "5", "18,4", "150", "150", "20,0", "20,4", "28,2", "12", "20,4", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "10", "5", "15", "15", "20", "12", "12", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "11") // Open Position -> Date Wise -> Client+Date+Asset
                    {
                        string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "12", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "10") // Open Position -> Date Wise -> Client+Date+Asset+Exchange
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "5", "12", "20,0", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "5", "12", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "9") // Open Position -> Date Wise -> Client+Date+Series
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "5", "15", "5", "18,4", "150", "20,0", "20,4", "28,4", "12", "20,4", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "10", "5", "12", "15", "12", "12", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "8") // Obligation & Exposure -> Date Wise -> Client
                    {
                        string[] ColumnType = { "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "7") // Obligation & Exposure -> Date Wise -> Client+Date
                    {
                        string[] ColumnType = { "V", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "15", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "10", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "6") // Obligation & Exposure -> Consolidated -> Client+Asset
                    {
                        string[] ColumnType = { "V", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "28,0", "12", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "5") // Obligation & Exposure -> Date Wise -> Client+Date+Asset
                    {
                        string[] ColumnType = { "V", "V", "N", "V", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "28,0", "12", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "12", "5", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "4") // Obligation & Exposure -> Consolidated -> Client+Asset+Exchange
                    {
                        string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "5", "12", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "5", "5", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "3") // Obligation & Exposure -> Date Wise -> Client+Date+Asset+Exchange
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "5", "12", "20,0", "20,0", "20,0", "20,0", "28,2", "22,2", "20,2", "22,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "5", "5", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "2") // Obligation & Exposure -> Consolidated -> Client+Series
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "150", "5", "15", "5", "18,4", "10", "150", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "28,2", "12", "22,2", "20,2", "22,2", "20,4", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "20", "5", "10", "5", "12", "5", "15", "12", "12", "12", "12", "12", "12", "12", "12", "12", "5", "12", "12", "12", "12", "12", "12", "12", "15", "15", "12", "12", "12" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    if (CmbReport.SelectedItem.Value.ToString() == "1") // Obligation & Exposure -> Date Wise -> Client+Date+Series
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                        string[] ColumnSize = { "15", "150", "10", "15", "5", "18,4", "10", "150", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "20,0", "20,4", "28,2", "12", "22,2", "20,2", "22,2", "20,4", "28,2", "28,2", "20,8", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                        string[] ColumnWidthSize = { "10", "20", "10", "10", "5", "12", "5", "15", "12", "12", "12", "12", "12", "12", "12", "12", "12", "5", "12", "12", "12", "12", "12", "12", "12", "12", "15", "15", "15", "15", "15" };
                        mailBody = oGenericExcelExport.ExportToHTML(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                    #endregion

                    // Run SP For Save Mail Details in Two Tables On EMailID/CCEmailID and For Different User
                    if (CmbMailSendTo.SelectedItem.Value.ToString() == "2" || CmbMailSendTo.SelectedItem.Value.ToString() == "3")
                    {
                        if (mailID.Contains("#"))
                        {
                            string[] mailIDList = mailID.Split('#');
                            for (int j = 0; j < mailIDList.Length; j++)
                            {
                                MailSend(mailSubject, mailBody, contactID, mailIDList[j]);
                            }
                        }
                        else
                            MailSend(mailSubject, mailBody, contactID, mailID);
                    }
                    else if (CmbMailSendTo.SelectedItem.Value.ToString() == "4")
                    {
                        if (usersList.Contains("#"))
                        {
                            string[] userIDMailList = usersList.Split('#');
                            for (int j = 0; j < userIDMailList.Length; j++)
                            {
                                contactID = userIDMailList[j].Split('~')[0].ToString();
                                mailID = userIDMailList[j].Split('~')[1].ToString();
                                MailSend(mailSubject, mailBody, contactID, mailID);
                            }
                        }
                        else
                        {
                            contactID = usersList.Split('~')[0].ToString();
                            mailID = usersList.Split('~')[1].ToString();
                            MailSend(mailSubject, mailBody, contactID, mailID);
                        }
                    }
                    else
                        MailSend(mailSubject, mailBody, contactID, mailID);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "alert('No Record Found.');", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript4", "<script language='javascript'>Reset();</script>");
            }
        }

        protected DataTable RemoveColumnForEmail(DataTable Dt)
        {
            if (Dt.Columns.Contains("CustomerID"))
            {
                Dt.Columns.Remove("CustomerID");
            }
            if (Dt.Columns.Contains("Email"))
            {
                Dt.Columns.Remove("Email");
            }
            if (Dt.Columns.Contains("CCEmail"))
            {
                Dt.Columns.Remove("CCEmail");
            }
            if (Dt.Columns.Contains("EmailType"))
            {
                Dt.Columns.Remove("EmailType");
            }
            if (Dt.Columns.Contains("BranchID"))
            {
                Dt.Columns.Remove("BranchID");
            }
            if (Dt.Columns.Contains("BranchEmail"))
            {
                Dt.Columns.Remove("BranchEmail");
            }
            if (Dt.Columns.Contains("GroupID"))
            {
                Dt.Columns.Remove("GroupID");
            }
            if (Dt.Columns.Contains("GroupEmail"))
            {
                Dt.Columns.Remove("GroupEmail");
            }
            if (Dt.Columns.Contains("GroupCCEmail"))
            {
                Dt.Columns.Remove("GroupCCEmail");
            }
            Dt.AcceptChanges();
            return Dt;
        }

        protected void MailSend(string mailSubject, string mailBody, string contactID, string mailID)
        {
            string senderEmail = string.Empty;
            string[,] data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + Session["userid"], 1);
            if (data[0, 0] != "n")
                senderEmail = data[0, 0];
            string[] PageName = Request.Url.ToString().Split('/');
            DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
                menuId = dt.Rows[0]["mnu_id"].ToString();
            else
                menuId = "";
            DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + Session["userlastsegment"].ToString() + "'");
            string segmentname = dtsg.Rows[0]["seg_name"].ToString();

            string[] strSpParam = new string[13];
            strSpParam[0] = "Emails_SenderEmailID|" + GenericStoreProcedure.ParamDBType.Varchar + "|150|" + senderEmail + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "Emails_Subject|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + mailSubject + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "Emails_Content|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + mailBody + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "Emails_HasAttachement|" + GenericStoreProcedure.ParamDBType.Char + "|1|N|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "Emails_CreateApplication|" + GenericStoreProcedure.ParamDBType.Int + "|10|" + menuId + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "Emails_CreateUser|" + GenericStoreProcedure.ParamDBType.Int + "|10|" + Session["userid"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[6] = "Emails_Type|" + GenericStoreProcedure.ParamDBType.Char + "|1|N|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[7] = "Emails_CompanyID|" + GenericStoreProcedure.ParamDBType.Char + "|10|" + Session["LastCompany"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[8] = "Emails_Segment|" + GenericStoreProcedure.ParamDBType.Char + "|10|" + segmentname + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[9] = "EmailRecipients_ContactLeadID|" + GenericStoreProcedure.ParamDBType.Char + "|16|" + contactID + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[10] = "EmailRecipients_RecipientEmailID|" + GenericStoreProcedure.ParamDBType.Varchar + "|500|" + mailID + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[11] = "EmailRecipients_RecipientType|" + GenericStoreProcedure.ParamDBType.Char + "|2|TO|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[12] = "EmailRecipients_Status|" + GenericStoreProcedure.ParamDBType.Char + "|1|P|" + GenericStoreProcedure.ParamType.ExParam;


            oGenericStoreProcedure = new GenericStoreProcedure();
            oGenericMethod = new GenericMethod();
            //if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 1)
            //{
            //    oDBEngine = new DBEngine(String.Empty);
            //    string strDateTime = oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
            //    string FilePath = "../ExportFiles/ServerDebugging/Insert_DematEntry" + strDateTime + ".txt";
            //    oGenericMethod.WriteFile(oGenericStoreProcedure.PrepareExecuteSpContent(strSpParam, "Insert_DematEntry"), FilePath, false);
            //}
            int result = Convert.ToInt32(oGenericStoreProcedure.Procedure_String(strSpParam, "Insert_EmailDetails"));
            if (result == 1)
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript1", "<script language='javascript'>alert('Mail Sent Successfully.');</script>");
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript2", "<script language='javascript'>alert('Mail Not Sent.');</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "jscript5", "<script language='javascript'>Reset();</script>");
        }

    }

}