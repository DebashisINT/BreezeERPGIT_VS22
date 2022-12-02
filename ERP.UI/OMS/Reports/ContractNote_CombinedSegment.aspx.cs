using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
//using System.Data.SqlClient;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{


    public partial class Reports_ContractNote_CombinedSegment : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        Converter oconverter = new Converter();
        protected string allcontract, ecnenable, deliveryrpt, remaining;
        string data;
        string strDosPrint;
        ReportDocument ICEXReportDocument = new ReportDocument();
        GenericMethod oGenericMethod;
        GenericStoreProcedure oGenericStoreProcedure;
        GenericExcelExport oGenericExcelExport;
        AspxHelper oAspxHelper;
        string CombinedGroupByQuery = null;
        DataTable dtsignature;
        DataTable dtlogo;
        DataSet DigitalSignatureDs;// = new DataSet();
        DataTable DistinctBillNumber;
        public DataSet Datafetch
        {
            get { return (DataSet)Session["Datafetch"]; }
            set { Session["Datafetch"] = value; }
        }
        public string ecnenable_excel
        {
            get { return (string)Session["ecnenable_excel"]; }
            set { Session["ecnenable_excel"] = value; }
        }
        public string ecnenable_excelforsp
        {
            get { return (string)Session["ecnenable_excelforsp"]; }
            set { Session["ecnenable_excelforsp"] = value; }
        }

        public string remaining_excel
        {
            get { return (string)Session["remaining_excel"]; }
            set { Session["remaining_excel"] = value; }
        }

        public string allcontract_excel
        {
            get { return (string)Session["allcontract_excel"]; }
            set { Session["allcontract_excel"] = value; }
        }
        public string allreadydeliver_excel
        {
            get { return (string)Session["allreadydeliver_excel"]; }
            set { Session["allreadydeliver_excel"] = value; }
        }

        #endregion

        #region PropertyVariable
        string pDTPosition; string GroupBy; string GroupByID; string GroupByType;
        #endregion

        #region Page Properties
        public string P_pDTPosition
        {
            get { return pDTPosition; }
            set { pDTPosition = value; }
        }
        public string P_GroupBy
        {
            get { return GroupBy; }
            set { GroupBy = value; }
        }
        public string P_GroupByID
        {
            get { return GroupByID; }
            set { GroupByID = value; }
        }
        public string P_GroupByType
        {
            get { return GroupByType; }
            set { GroupByType = value; }
        }
        public string P_SelectedIds
        {
            get { return (string)Session["SelectedIds"]; }
            set { Session["SelectedIds"] = value; }
        }
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string[] PageSession = null;
                oGenericMethod = new GenericMethod();
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_Page, PageSession);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CbpExportPanel.JSProperties["cppdfclick"] = null;
            CbpExportPanel.JSProperties["cpallcontract"] = null;
            CbpExportPanel.JSProperties["cpecnenable"] = null;
            CbpExportPanel.JSProperties["cpdeliveryrpt"] = null;
            CbpExportPanel.JSProperties["cpdosprint"] = null;
            CbpExportPanel.JSProperties["cpsign"] = null;
            CbpExportPanel.JSProperties["cpallcontractpop"] = null;
            CbpExportPanel.JSProperties["cpecnenablepop"] = null;
            CbpSuggestISIN.JSProperties["cpforalert"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>Height('500','500');</script>");
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad12", "<script>PageLoad();</script>");
                FnDosPrint();
                SetDateFinYear();
                oGenericMethod = new GenericMethod();
                BindAjaxList(oGenericMethod.GetDigitalSignature(), txtdigitalName);
                BindAjaxList(oGenericMethod.GetEmployeeWithSignature(), txtEmpName);
                txtdigitalName.Visible = IsSignExists();
                if (txtdigitalName.Visible == true)
                    td_msg.Visible = false;
                else
                    td_msg.Visible = true;
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
            if (WhichCall == "CallAjax-Client")
            {
                string specificSeg = Session["Segmentname"].ToString().Replace("-", " - ");
                //CombinedGroupByQuery = oGenericMethod.GetClient_SegmentFilter(Session["LastCompany"].ToString(), "'" + specificSeg + "'");
                CombinedGroupByQuery = oGenericMethod.GetAllContact("CL");
            }
            if (WhichCall == "CallAjax-Branch")
            {
                CombinedGroupByQuery = oGenericMethod.GetAllBranch(10);
            }
            if (WhichCall.Contains("CallAjax-Group"))
            {
                string groupType = WhichCall.Split('~')[1].ToString();
                CombinedGroupByQuery = oGenericMethod.GetAllGroups(groupType, 10);
            }
            if (WhichCall == "CallAjax-Segment")
            {
                CombinedGroupByQuery = oGenericMethod.GetSegmentName();
            }
        }
        #endregion

        protected void SetDateFinYear()
        {
            DateTime StartDate = Convert.ToDateTime(Session["FinYearStart"].ToString());
            DateTime EndDate = Convert.ToDateTime(Session["FinYearEnd"].ToString());

            oGenericMethod = new GenericMethod();
            DateTime TodayDate = oGenericMethod.GetDate();
            if ((Session["ExchangeSegmentID"].ToString() == "1") || (Session["ExchangeSegmentID"].ToString() == "4") || (Session["ExchangeSegmentID"].ToString() == "15") || (Session["ExchangeSegmentID"].ToString() == "19"))
            {

                TodayDate = Convert.ToDateTime(Session["StartdateFundsPayindate"].ToString().Split(',')[0]);//oGenericMethod.GetDate();
                dtPosition.Value = TodayDate;
            }
            else
            {
                if (EndDate < TodayDate)
                    dtPosition.Value = EndDate;
                else if (EndDate > TodayDate)
                    dtPosition.Value = TodayDate;
                else
                    dtPosition.Value = TodayDate;

            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id == "CallAjax-Client" || id == "CallAjax-Branch" || id.Contains("CallAjax-Group"))
            {
                CallUserList(id);
                CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
                data = "AjaxQuery@" + CombinedGroupByQuery;
            }
            else
            {
                string[] idlist = id.Split('^');
                string recieveServerIDs = "";
                for (int i = 0; i < idlist.Length; i++)
                {
                    string[] strVal = idlist[i].Split('!');
                    string[] ids = strVal[0].Split('~');
                    string whichCall = ids[ids.Length - 1];
                    if (whichCall == "BRANCH")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Branch@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "GROUP")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Group@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "CLIENT")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Client@" + recieveServerIDs.ToString();
                    }
                }
            }
        }

        void SetPropertiesValue()
        {
            //Date
            pDTPosition = Convert.ToDateTime(dtPosition.Value).ToString("yyyy-MM-dd");
            //GroupBy ,GroupByID and GroupByType
            if (CmbGroupBy.SelectedIndex == 0)
            {
                GroupBy = "CLIENT";
                if (RblClient.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HiddenField_ClientBranchGroup.Value.Trim() != "")
                    {
                        string[] clntItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
                        for (int i = 0; i < clntItems.Length; i++)
                        {
                            string[] clntItem = clntItems[i].Split('!');
                            string[] clntIds = clntItem[0].Split('~');
                            if (GroupByID == "")
                                GroupByID = clntIds[4];
                            else
                                GroupByID = GroupByID + "," + clntIds[4];
                        }
                    }
                    else
                        GroupByID = "Error:Client";
                }
                GroupByType = "";

            }
            else if (CmbGroupBy.SelectedIndex == 1)
            {
                GroupBy = "BRANCH";
                if (RblBranch.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HiddenField_ClientBranchGroup.Value.Trim() != "")
                    {
                        string[] branchItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
                        for (int i = 0; i < branchItems.Length; i++)
                        {
                            string[] branchItem = branchItems[i].Split('!');
                            string[] branchIds = branchItem[0].Split('~');
                            if (GroupByID == "")
                                GroupByID = branchIds[0];
                            else
                                GroupByID = GroupByID + "," + branchIds[0];
                        }
                    }
                    else
                        GroupByID = "Error:Branch";
                }
                GroupByType = "";
            }
            else if (CmbGroupBy.SelectedIndex == 2)
            {
                GroupBy = "GROUP";
                if (RblGroup.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HiddenField_ClientBranchGroup.Value.Trim() != "")
                    {
                        string[] grpItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
                        for (int i = 0; i < grpItems.Length; i++)
                        {
                            string[] grpItem = grpItems[i].Split('!');
                            string[] grpIds = grpItem[0].Split('~');
                            if (GroupByID == "")
                                GroupByID = grpIds[0];
                            else
                                GroupByID = GroupByID + "," + grpIds[0];
                        }
                    }
                    else
                        GroupByID = "Error:Group";
                }
                if (CmbGroupType.SelectedIndex != 0) GroupByType = CmbGroupType.SelectedItem.Value.ToString();
            }

        }

        string PageValidation()
        {
            string strError = String.Empty;
            if (GroupBy == "BRANCH")
            {
                if (GroupByID.Split(':')[0] == "Error")
                    strError = "BranchErr";
            }
            else if (GroupBy == "GROUP")
            {
                if (GroupByID.Split(':')[0] == "Error")
                    strError = "GroupErr";
            }
            else
            {
                if (GroupByID.Split(':')[0] == "Error")
                    strError = "ClientErr";
            }
            return strError;
        }

        void BindAjaxList(String CombinedQuery, TextBox TxtBoxName)
        {
            //CombinedQuery = CombinedQuery.Replace("'", "\\'");
            TxtBoxName.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedQuery + "')");
        }

        # region Bind Group Type
        protected int BindGroupType()
        {
            int result = 0;
            CmbGroupType.Items.Clear();
            oGenericMethod = new GenericMethod();
            DataTable DtGroup = oGenericMethod.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type as ID,gpm_Type as Value", null);
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
            int bindResult = BindGroupType();
            if (bindResult == 1)
                CmbGroupType.JSProperties["cpBindGroupType"] = "Y";
            else
                CmbGroupType.JSProperties["cpBindGroupType"] = "N";
        }
        # endregion

        void Procedurecallandexport()
        {
            SetPropertiesValue();
            string strPageValidationMsg = PageValidation();
            if (strPageValidationMsg == String.Empty)
            {
                string mode = "";
                string onlycontractornote = "";
                if (chkonlybill.Checked == true)
                    onlycontractornote = "true";
                else
                    onlycontractornote = "false";
                //DataSet Datafetch = new DataSet();
                oGenericMethod = new GenericMethod();
                string[] strSpParam = new string[15];
                strSpParam[0] = "company|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + Session["LastCompany"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
                mode = RblClient.SelectedItem.Value.ToString();
                if (CmbGroupBy.SelectedItem.Text == "Client")
                    mode = RblClient.SelectedItem.Value.ToString();
                if (CmbGroupBy.SelectedItem.Text == "Branch")
                    mode = RblBranch.SelectedItem.Value.ToString();

                if (CmbGroupBy.SelectedItem.Text == "Group")

                    mode = RblGroup.SelectedItem.Value.ToString();



                strSpParam[1] = "mode|" + GenericStoreProcedure.ParamDBType.Varchar + "|1|" + mode + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[2] = "clienttypeparam|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + GroupBy + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[3] = "clientbranchgroupid|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + GroupByID + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[4] = "onlyforgrouptext|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + GroupByType + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "Tradedate|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + pDTPosition + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[6] = "Finyear|" + GenericStoreProcedure.ParamDBType.Varchar + "|500|" + Session["LastFinYear"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[7] = "SpecificSegment|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + RbSegment.Value.ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[8] = "ReportType|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + CmbRptType.SelectedItem.Value.ToString().Trim() + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[9] = "OnlyContractnote|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + onlycontractornote.ToString().Trim() + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[10] = "Ecncall|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "Normal" + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[11] = "UserSegidfrompage|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + Session["UserSegID"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
                if (CmbRptType.Value.ToString().Trim() == "DOS")
                {
                    if (strDosPrint == "true")
                    {
                        strSpParam[12] = "DosViewOrPrint|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "Print" + "|" + GenericStoreProcedure.ParamType.ExParam;
                        strSpParam[13] = "DosPrint_IDs|" + GenericStoreProcedure.ParamDBType.Varchar + "|500|" + P_SelectedIds + "|" + GenericStoreProcedure.ParamType.ExParam;
                    }
                    else
                    {
                        strSpParam[12] = "DosViewOrPrint|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "View" + "|" + GenericStoreProcedure.ParamType.ExParam;
                        strSpParam[13] = "DosPrint_IDs|" + GenericStoreProcedure.ParamDBType.Varchar + "|500|" + " " + "|" + GenericStoreProcedure.ParamType.ExParam;
                    }

                }
                else
                {
                    strSpParam[12] = "DosViewOrPrint|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "" + "|" + GenericStoreProcedure.ParamType.ExParam;
                    strSpParam[13] = "DosPrint_IDs|" + GenericStoreProcedure.ParamDBType.Varchar + "|500|" + " " + "|" + GenericStoreProcedure.ParamType.ExParam;
                }
                strSpParam[14] = "IsAverage|" + GenericStoreProcedure.ParamDBType.Varchar + "|2|" + (chkAvgType.Checked ? 1 : 0) + "|" + GenericStoreProcedure.ParamType.ExParam;
                oGenericStoreProcedure = new GenericStoreProcedure();

                //if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 1)
                //{
                //    DBEngine oDBEngine = new DBEngine(String.Empty);
                //    string strDateTime = oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
                //    string FilePath = "../ExportFiles/ServerDebugging/ContractnoteCumBill_Combined" + strDateTime + ".txt";
                //    oGenericMethod.WriteFile(oGenericStoreProcedure.PrepareExecuteSpContent(strSpParam, "ContractnoteCumBill_Combined"), FilePath, false);
                //}
                Datafetch = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "ContractnoteCumBill_Combined");
                byte[] logoinByte = null;
                byte[] SignatureinByte;
                dtlogo = new DataTable();

                dtlogo.Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                DataRow drlogo = dtlogo.NewRow();
                string filePath = "";
                filePath = @"..\images\cntrlogo_" + Session["LastCompany"].ToString() + ".jpg";
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(filePath), out logoinByte) == 1)
                {
                    drlogo["Image"] = logoinByte;
                    dtlogo.Rows.Add(drlogo);
                }
                Datafetch.Tables.Add(dtlogo);
                dtsignature = new DataTable();
                dtsignature.Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
                DataRow drsignature = dtsignature.NewRow();
                if (chkSignature.Checked == true)
                {
                    if (oconverter.getSignatureImage(txtEmpName_hidden.Text.Split('~')[0].ToString(), out SignatureinByte, "NSE") == 1)
                    {
                        //dtsignature.Rows[0]["Signature"] = SignatureinByte;
                        drsignature["Signature"] = SignatureinByte;
                        dtsignature.Rows.Add(drsignature);
                    }
                }
                Datafetch.Tables.Add(dtsignature);
                Datafetch.AcceptChanges();

            }
            else if (strPageValidationMsg == "BranchErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(BranchErr);", true);
            }
            else if (strPageValidationMsg == "GroupErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg2", "ErrorMsg(GroupErr);", true);
            }
            else if (strPageValidationMsg == "ClientErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg3", "ErrorMsg(ClientErr);", true);
            }
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            //Procedurecallandexport();
            ReportDocument reportObj = new ReportDocument();
            if (CmbRptOf.Value.ToString() == "1")
            {
                Datafetch.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + @"\pics\combinedcontractnote.xsd");
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                string ReportPath = Server.MapPath("..\\Reports\\Contractnote_combinedsegment.rpt");
                reportObj.Load(ReportPath);
                reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                reportObj.SetDataSource(Datafetch);
                reportObj.Subreports["contractnote_cumbill"].SetDataSource(Datafetch.Tables[5]);
                reportObj.Subreports["TradeAnnexure"].SetDataSource(Datafetch.Tables[6]);
                //Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad13", "<script>changetext();</script>");
                if (chkSignature.Checked == true)
                {
                    reportObj.SetParameterValue("@empname", (object)txtEmpName.Text.ToString().Split('[')[0].Trim());
                }
                else
                    reportObj.SetParameterValue("@empname", "");

                if (RbSegment.Value.ToString() == "True")
                    reportObj.SetParameterValue("@SingleOrAllSegment", "true");
                else
                    reportObj.SetParameterValue("@SingleOrAllSegment", "false");
                reportObj.SetParameterValue("@SingleDoublePrint", (chkBothSidePrnt.Checked ? 1 : 0));
                reportObj.SetParameterValue("@NtShowTotlBrkg", (chkTotlBrkg.Checked ? 1 : 0));
                reportObj.SetParameterValue("@NtShowBrnchName", (chkBrnchName.Checked ? 1 : 0));
                reportObj.SetParameterValue("@NtShowOrderTradeNo", (chkAvgType.Checked ? 1 : 0));
                reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "contractnote_cumbill");
            }
            else
            {
                Datafetch.Tables[7].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + @"\pics\Annexure.xsd");
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                string ReportPath = Server.MapPath("..\\Reports\\Annexure.rpt");
                reportObj.Load(ReportPath);
                reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                reportObj.SetDataSource(Datafetch.Tables[7]);
                //Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad13", "<script>changetext();</script>");
                //if (chkSignature.Checked == true)
                //{
                //    reportObj.SetParameterValue("@empname", (object)txtEmpName.Text.ToString().Split('[')[0].Trim());
                //}
                //else
                //    reportObj.SetParameterValue("@empname", "");

                //if (RbSegment.Value.ToString() == "True")
                //    reportObj.SetParameterValue("@SingleOrAllSegment", "true");
                //else
                //    reportObj.SetParameterValue("@SingleOrAllSegment", "false");
                //reportObj.SetParameterValue("@SingleDoublePrint", (chkBothSidePrnt.Checked ? 1 : 0));
                //reportObj.SetParameterValue("@NtShowBrnchName", (chkBrnchName.Checked ? 1 : 0));
                reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Trade_Annexure");

            }
            //reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, HttpContext.Current.Response, true, "contractnote_cumbill");
            reportObj.Dispose();
            GC.Collect();


        }
        protected void btndosprint_Click(object sender, EventArgs e)
        {
            string str = "";
            strDosPrint = "true";
            str = fileprepare();
            string UploadPath = str;
            string[] filename = UploadPath.Split('\\');
            //string[] Location = oDBEngine.GetFieldValue1("Config_DosPrinter", "DosPrinter_Location", "DosPrinter_User='" + HttpContext.Current.Session["userid"].ToString() + "'", 1);
            string path = ddlLocation.Text + filename[filename.Length - 1];
            hdnLocationPath.Value = path;

        }
        protected string fileprepare()
        {
            string sOutputFileName = "";
            string path = "";
            Procedurecallandexport();
            if (Datafetch.Tables[0].Rows.Count > 0)
            {
                Session["a1"] = Datafetch;

                string sData = null;
                string[] sArQuery = new string[1];
                sArQuery[0] = "";
                string PrintLoaction = null;
                if (ddlLocation.SelectedItem.Value.ToString().Trim() == "0")
                {
                    PrintLoaction = ConfigurationManager.AppSettings["SaveCSVsql"];
                }
                else
                {
                    PrintLoaction = ddlLocation.SelectedItem.Value.ToString().Trim();

                }
                string FileName = "OutputContract_Combined" + Session["userid"].ToString() + "_" + Session["LastCompany"].ToString() + "_" + oGenericMethod.GetDate().ToString("ddMMyyyy") + "_" + oGenericMethod.GetDate().ToString("hhmmss") + ".txt";

                string XmlFileName = Server.MapPath("ReportFormat") + "\\ContractNote_Combined" + HttpContext.Current.Session["LastCompany"].ToString() + ".xml";
                sOutputFileName = Server.MapPath("ReportOutput") + "\\OutputContract_Combined" + Session["userid"].ToString() + "_" + Session["LastCompany"].ToString() + "_" + oGenericMethod.GetDate().ToString("ddMMyyyy") + "_" + oGenericMethod.GetDate().ToString("hhmmss") + ".txt";
                hdnpath.Value = "\\OutputContract_Combined" + Session["userid"].ToString() + "_" + Session["LastCompany"].ToString() + "_" + oGenericMethod.GetDate().ToString("ddMMyyyy") + "_" + oGenericMethod.GetDate().ToString("hhmmss") + ".txt";
                string ab = hdnpath.Value;
                hdnLocationPath.Value = path;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript40", "ajaxFunction();", true);
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript40", "ajaxFunction('\\"+ ab + "','\\"+ path + FileName + "');", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
            }
            return sOutputFileName;

        }
        protected void CbpExportPanel_Callback(object source, CallbackEventArgsBase e)
        {
            SetPropertiesValue();
            string strPageValidationMsg = PageValidation();
            if (strPageValidationMsg == String.Empty)
            {
                CbpExportPanel.JSProperties["cppdfclick"] = null;
                CbpExportPanel.JSProperties["cpallcontract"] = null;
                CbpExportPanel.JSProperties["cpecnenable"] = null;
                CbpExportPanel.JSProperties["cpdeliveryrpt"] = null;
                CbpExportPanel.JSProperties["cpdosprint"] = null;
                CbpExportPanel.JSProperties["cpsign"] = null;
                CbpExportPanel.JSProperties["cpallcontractpop"] = null;
                CbpExportPanel.JSProperties["cpecnenablepop"] = null;
                string WhichCall = e.Parameter.Split('~')[0];
                if (WhichCall == "PDF")
                {
                    Procedurecallandexport();
                    CbpExportPanel.JSProperties["cppdfclick"] = "Success";
                }
                if (WhichCall == "DOS")
                    CbpExportPanel.JSProperties["cpdosprint"] = "click";
                else
                {
                    //string WhichCall = e.Parameter.Split('~')[0];
                    //string strValue = GroupBy + "~" + GroupByID + "~" + GroupByType;
                    if (WhichCall == "ECN")
                    {

                        txtdigitalName.Visible = IsSignExists();
                        if (txtdigitalName.Visible == true)
                        {
                            td_msg.Visible = false;
                            CbpExportPanel.JSProperties["cpsign"] = "true";
                        }
                        else
                        {
                            td_msg.Visible = true;
                            CbpExportPanel.JSProperties["cpsign"] = "false";
                        }
                        Procedurecallandexport();
                        CbpExportPanel.JSProperties["cpallcontract"] = Datafetch.Tables[1].Rows.Count;
                        CbpExportPanel.JSProperties["cpecnenable"] = Datafetch.Tables[2].Rows.Count;
                        CbpExportPanel.JSProperties["cpdeliveryrpt"] = Datafetch.Tables[3].Rows.Count;
                        if (Datafetch.Tables[0].Rows.Count > 0)
                            allcontract_excel = Datafetch.Tables[0].Rows[0]["Allcontract"].ToString();
                        else
                            allcontract_excel = "";
                        if (Datafetch.Tables[4].Rows.Count > 0)
                            remaining_excel = Datafetch.Tables[4].Rows[0]["RemainingEcn"].ToString();
                        else
                            remaining_excel = "";
                        if (Datafetch.Tables[2].Rows.Count > 0)
                        {
                            ecnenable_excel = Datafetch.Tables[2].Rows[0]["ecnenablecontractforpage"].ToString();
                            ecnenable_excelforsp = Datafetch.Tables[2].Rows[0]["EcnEnableContract"].ToString();
                        }
                        else
                        {
                            ecnenable_excel = "";
                            ecnenable_excelforsp = "";
                        }
                        if (Datafetch.Tables[3].Rows.Count > 0)
                            allreadydeliver_excel = Datafetch.Tables[3].Rows[0]["AllreadyDeliver"].ToString();
                        else
                            allreadydeliver_excel = "";

                    }
                    if (WhichCall == "showpopup")
                    {
                        CbpExportPanel.JSProperties["cpallcontractpop"] = Datafetch.Tables[3].Rows.Count;
                        CbpExportPanel.JSProperties["cpecnenablepop"] = Datafetch.Tables[4].Rows.Count;
                    }
                    if (WhichCall == "GeneratePDF")
                    {

                    }

                }
            }
            else if (strPageValidationMsg == "BranchErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(BranchErr);", true);
            }
            else if (strPageValidationMsg == "GroupErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg2", "ErrorMsg(GroupErr);", true);
            }
            else if (strPageValidationMsg == "ClientErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg3", "ErrorMsg(ClientErr);", true);
            }
        }
        public void FnDosPrint()
        {
            ddlLocation.Items.Clear();
            DataTable DtDosPrint = oGenericMethod.GetDataTable("Config_DosPrinter", "distinct DosPrinter_Name+'['+DosPrinter_Location+']' as DosPrintName, DosPrinter_Location", "DosPrinter_User='" + HttpContext.Current.Session["userid"].ToString() + "'"); //replace(DosPrinter_Location,'/','\\') as
            if (DtDosPrint.Rows.Count > 0)
            {
                ddlLocation.DataSource = DtDosPrint;
                ddlLocation.DataTextField = "DosPrintName";
                ddlLocation.DataValueField = "DosPrinter_Location";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("Select DosPrint Location", "0"));
                DtDosPrint.Dispose();

            }
            else
            {
                ddlLocation.Items.Insert(0, new ListItem("Select DosPrint Location", "0"));
            }

        }
        bool IsSignExists()
        {
            string str;
            str = string.Empty;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "searchSignatureUser";

                cmd.Parameters.AddWithValue("@userID", Session["userid"]);

                cmd.CommandTimeout = 0;
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                str = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }
            if (str == "Y")
                return true;
            else
                return false;

        }

        protected void CbpSuggestISIN_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            CbpSuggestISIN.JSProperties["cpallsendtclick"] = null;
            CbpSuggestISIN.JSProperties["cpforalert"] = null;
            CbpSuggestISIN.JSProperties["cpNoPDFGenerated"] = null;

            if (WhichCall == "all")
            {
                pdfcreationandmergrd();
                oGenericMethod = new GenericMethod();
                DataTable dtdelivercount = oGenericMethod.GetDataTable("select count(*) from Trans_ContractnotesCombined where ContractnotesCombined_Company='" + Session["LastCompany"].ToString() + "' and ContractnotesCombined_Finyear='" + Session["LastFinyear"].ToString() + "' and ContractnotesCombined_Tradedate='" + dtPosition.Value + "' and ContractnotesCombined_CustomerID in (" + ecnenable_excel + ")");
                CbpSuggestISIN.JSProperties["cpallcontractpops"] = dtdelivercount.Rows[0][0].ToString();
                CbpSuggestISIN.JSProperties["cpecnenablepops"] = Convert.ToString(Convert.ToInt32(Datafetch.Tables[2].Rows.Count) - Convert.ToInt32(dtdelivercount.Rows[0][0].ToString()));

            }
            if (WhichCall == "GeneratePDF")
            {
                pdfcreationandmergrd();


            }
            if (WhichCall == "remaining")
            {
                Datafetch = null;
                string mode = "";
                string onlycontractornote = "";
                if (chkonlybill.Checked == true)
                    onlycontractornote = "true";
                else
                    onlycontractornote = "false";
                //DataSet Datafetch = new DataSet();
                oGenericMethod = new GenericMethod();
                string[] strSpParam = new string[12];
                strSpParam[0] = "company|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + Session["LastCompany"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;

                mode = "S";
                GroupByID = ecnenable_excelforsp.ToString();
                GroupByID = GroupByID.Replace("'", "");

                strSpParam[1] = "mode|" + GenericStoreProcedure.ParamDBType.Varchar + "|1|" + "S" + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[2] = "clienttypeparam|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "Client" + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[3] = "clientbranchgroupid|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + GroupByID + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[4] = "onlyforgrouptext|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + "" + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "Tradedate|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + Convert.ToDateTime(dtPosition.Value).ToString("yyyy-MM-dd") + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[6] = "Finyear|" + GenericStoreProcedure.ParamDBType.Varchar + "|500|" + Session["LastFinYear"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[7] = "SpecificSegment|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + RbSegment.Value.ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[8] = "ReportType|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "ECN" + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[9] = "OnlyContractnote|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + onlycontractornote.ToString().Trim() + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[10] = "Ecncall|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "Remaining" + "|" + GenericStoreProcedure.ParamType.ExParam;
                strSpParam[11] = "UserSegidfrompage|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + Session["UserSegID"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
                oGenericStoreProcedure = new GenericStoreProcedure();
                Datafetch = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "ContractnoteCumBill_Combined");
                byte[] logoinByte = null;
                byte[] SignatureinByte;
                dtlogo = new DataTable();

                dtlogo.Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                DataRow drlogo = dtlogo.NewRow();
                string filePath = "";
                filePath = @"..\images\cntrlogo_" + Session["LastCompany"].ToString() + ".jpg";
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(filePath), out logoinByte) == 1)
                {
                    drlogo["Image"] = logoinByte;
                    dtlogo.Rows.Add(drlogo);
                }
                Datafetch.Tables.Add(dtlogo);
                dtsignature = new DataTable();
                dtsignature.Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
                DataRow drsignature = dtsignature.NewRow();
                if (chkSignature.Checked == true)
                {
                    if (oconverter.getSignatureImage(txtEmpName_hidden.Text.Split('~')[0].ToString(), out SignatureinByte, "NSE") == 1)
                    {
                        //dtsignature.Rows[0]["Signature"] = SignatureinByte;
                        drsignature["Signature"] = SignatureinByte;
                        dtsignature.Rows.Add(drsignature);
                    }
                }
                Datafetch.Tables.Add(dtsignature);
                Datafetch.AcceptChanges();
                if (Datafetch.Tables[2].Rows.Count > 0)
                {
                    ecnenable_excel = Datafetch.Tables[2].Rows[0]["ecnenablecontractforpage"].ToString();
                    ecnenable_excelforsp = Datafetch.Tables[2].Rows[0]["EcnEnableContract"].ToString();
                }
                pdfcreationandmergrd();
                oGenericMethod = new GenericMethod();
                DataTable dtdelivercount = oGenericMethod.GetDataTable("select count(*) from Trans_ContractnotesCombined where ContractnotesCombined_Company='" + Session["LastCompany"].ToString() + "' and ContractnotesCombined_Finyear='" + Session["LastFinyear"].ToString() + "' and ContractnotesCombined_Tradedate='" + dtPosition.Value + "' and ContractnotesCombined_CustomerID in (" + ecnenable_excel + ")");
                CbpSuggestISIN.JSProperties["cpallcontractpops"] = dtdelivercount.Rows[0][0].ToString();
                CbpSuggestISIN.JSProperties["cpecnenablepops"] = Convert.ToString(Convert.ToInt32(Datafetch.Tables[2].Rows.Count) - Convert.ToInt32(dtdelivercount.Rows[0][0].ToString()));

            }

        }
        void pdfcreationandmergrd()
        {
            ViewRegulatoryReportNameSpace.ViewRegulatoryReport view = new ViewRegulatoryReportNameSpace.ViewRegulatoryReport();
            ReportDocument ICEXReportDocument = new ReportDocument();
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmdDigital = new SqlCommand("cdsl_EmployeeName", con1);
                cmdDigital.CommandType = CommandType.StoredProcedure;
                cmdDigital.Parameters.AddWithValue("@ID", txtdigitalName_hidden.Text.Split('~')[0].ToString());
                cmdDigital.CommandTimeout = 0;
                SqlDataAdapter daDigital = new SqlDataAdapter();
                daDigital.SelectCommand = cmdDigital;
                DigitalSignatureDs = new DataSet();
                daDigital.Fill(DigitalSignatureDs);
            }
            if (!Datafetch.Tables[0].Columns.Contains("password"))
            {
                Datafetch.Tables[0].Columns.Add("branch", System.Type.GetType("System.String"));
                Datafetch.Tables[0].Columns.Add("signname", System.Type.GetType("System.String"));
                Datafetch.Tables[0].Columns.Add("internalid", System.Type.GetType("System.String"));
                Datafetch.Tables[0].Columns.Add("branchid", System.Type.GetType("System.String"));
                Datafetch.Tables[0].Columns.Add("password", System.Type.GetType("System.String"));
                Datafetch.Tables[0].Rows[0]["branch"] = DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString();
                Datafetch.Tables[0].Rows[0]["signname"] = DigitalSignatureDs.Tables[0].Rows[0]["signName"].ToString();
                Datafetch.Tables[0].Rows[0]["internalid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString();
                Datafetch.Tables[0].Rows[0]["branchid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString();
                Datafetch.Tables[0].Rows[0]["password"] = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();
            }
            string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult, IsSegALLOrCurrent;

            tmpPdfPath = string.Empty;
            ReportPath = string.Empty;
            signPath = string.Empty;
            finalResult = string.Empty;

            digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();
            tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
            signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + txtdigitalName_hidden.Text.Split('~')[0].ToString() + ".pfx";
            signPdfPath = oconverter.DirectoryPath(out VirtualPath);

            if (RbSegment.Value.ToString() == "True")
                IsSegALLOrCurrent = "true";
            else
                IsSegALLOrCurrent = "false";

            //Checking For E-Token
            string TokenType = null, DigiEmpDetail = null;
            DBEngine oDBEngine = new DBEngine(null);
            TokenType =
            oDBEngine.GetDataTable(@"Select Isnull(DigitalSignature_Type,'N') from Master_DigitalSignature 
                    Where DigitalSignature_ID=" + txtdigitalName_hidden.Text.ToString().Split('~')[0].ToString()).Rows[0][0].ToString();

            if (TokenType == "E")
            {
                tmpPdfPath = tmpPdfPath + "\\EToken\\";
                DigiEmpDetail = TokenType + '*' + txtdigitalName_hidden.Text;
            }
            else
            {
                DigiEmpDetail = txtdigitalName_hidden.Text.ToString().Split('~')[1].ToString().Split('[')[0].Trim();
            }

            finalResult = view.generateindivisualPdfcombined(ICEXReportDocument, Datafetch, "Yes", digitalSignaturePassword,
            signPath, ReportPath
            , tmpPdfPath, Session["LastCompany"].ToString(), signPdfPath, VirtualPath,
            HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(),
            Datafetch.Tables[0].Rows[0]["branch"].ToString(), Datafetch.Tables[0].Rows[0]["branchid"].ToString(),
            Datafetch.Tables[0].Rows[0]["internalid"].ToString(), dtPosition.Value.ToString(), IsSegALLOrCurrent,
            DigiEmpDetail);

            if (TokenType == "E")
            {
                CbpSuggestISIN.JSProperties["cpNoPDFGenerated"] = finalResult.Split('~')[1];
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtexcellall = new DataTable();
            oGenericMethod = new GenericMethod();
            dtexcellall = oGenericMethod.GetDataTable("select ROW_NUMBER() over(order by cnt_internalId) as Srlno,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,''))) +' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(isnull(cnt_UCC,'')))+' ]' as ClientName, case when ltrim(rtrim(isnull(cnt_ContractDeliveryMode,'B')))='B' then 'Both' when ltrim(rtrim(cnt_ContractDeliveryMode))='P' then 'Print' when ltrim(rtrim(cnt_ContractDeliveryMode))='E' then 'ECN' end as Deliverytype, ltrim(rtrim(isnull(eml_email,'NA'))) as EmailId,isnull(eml_type,'NA') as EmailType from tbl_master_contact  left join  tbl_master_email on cnt_internalId=eml_cntid where cnt_internalId in (" + allcontract_excel + ")");
            export(dtexcellall);
        }
        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtexcellecnenable = new DataTable();
            oGenericMethod = new GenericMethod();
            dtexcellecnenable = oGenericMethod.GetDataTable("select ROW_NUMBER() over(order by cnt_internalId) as Srlno,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,''))) +' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(isnull(cnt_UCC,'')))+' ]' as ClientName, case when ltrim(rtrim(isnull(cnt_ContractDeliveryMode,'B')))='B' then 'Both' when ltrim(rtrim(cnt_ContractDeliveryMode))='P' then 'Print' when ltrim(rtrim(cnt_ContractDeliveryMode))='E' then 'ECN' end as Deliverytype, ltrim(rtrim(isnull(eml_email,'NA'))) as EmailId,isnull(eml_type,'NA') as EmailType from tbl_master_contact  left join  tbl_master_email on cnt_internalId=eml_cntid where cnt_internalId in (" + ecnenable_excel + ")");
            export(dtexcellecnenable);
        }
        protected void cmbExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtexcelldeliver = new DataTable();
            oGenericMethod = new GenericMethod();
            dtexcelldeliver = oGenericMethod.GetDataTable("select ROW_NUMBER() over(order by cnt_internalId) as Srlno,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,''))) +' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(isnull(cnt_UCC,'')))+' ]' as ClientName, case when ltrim(rtrim(isnull(cnt_ContractDeliveryMode,'B')))='B' then 'Both' when ltrim(rtrim(cnt_ContractDeliveryMode))='P' then 'Print' when ltrim(rtrim(cnt_ContractDeliveryMode))='E' then 'ECN' end as Deliverytype, ltrim(rtrim(isnull(eml_email,'NA'))) as EmailId,isnull(eml_type,'NA') as EmailType from tbl_master_contact  left join  tbl_master_email on cnt_internalId=eml_cntid where cnt_internalId in (" + allreadydeliver_excel + ")");
            export(dtexcelldeliver);
        }
        void export(DataTable dtExport)
        {
            ExcelFile objExcel = new ExcelFile();
            oGenericMethod = new GenericMethod();
            string searchCriteria = null;
            Converter oconverter = new Converter();
            searchCriteria = "For " + oconverter.ArrangeDate2(dtPosition.Value.ToString()) + " Report of   " + CmbGroupBy.SelectedItem.Text + " Wise";

            //dtExport = dtecnenale.Copy();
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oGenericMethod.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "Combined_Contractnote_" + exlTime;
            strDownloadFileName = "~/Documents/";
            DataTable dtcompany = oGenericMethod.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "Combined_Contract Note Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots
            string[] ColumnType = { "I", "V", "V", "V", "V" };
            string[] ColumnSize = { "10", "50", "20", "40", "20" };
            string[] ColumnWidthSize = { "5", "50", "20", "40", "20" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
        }

        #region EToken Code
        protected void ASPxCBP_PDFGenerate_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
        }
        #endregion
        protected void GvDOSPrintView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string WhichCall = e.Parameters.Split('~')[0];
            if (WhichCall == "Bind")
            {
                Procedurecallandexport();
                oAspxHelper = new AspxHelper();
                oAspxHelper.BindGrid(GvDOSPrintView, Datafetch);
            }
            if (WhichCall == "Print")
            {
                P_SelectedIds = "";
                int TotalRecord = GvDOSPrintView.VisibleRowCount;
                int counter = 0;
                for (int i = 0; i < TotalRecord; i++)
                {

                    if (GvDOSPrintView.Selection.IsRowSelected(i))
                    {
                        P_SelectedIds = P_SelectedIds + "'" + GvDOSPrintView.GetRowValues(i, "UID").ToString() + "',";
                        counter += 1;
                    }

                    //DeSelectedIds = DeSelectedIds + GvAuthRecord.GetRowValues(i, "kycmain_id").ToString() + ",";
                }
                GvDOSPrintView.JSProperties["cpprint"] = "click";

                //string str = "";
                //strDosPrint = "true";
                //str = fileprepare();
                //string UploadPath = str;
                //string[] filename = UploadPath.Split('\\');
                ////string[] Location = oDBEngine.GetFieldValue1("Config_DosPrinter", "DosPrinter_Location", "DosPrinter_User='" + HttpContext.Current.Session["userid"].ToString() + "'", 1);
                //string path = ddlLocation.Text + filename[filename.Length - 1];
                //hdnLocationPath.Value = path;
            }

        }
        protected void GvDOSPrintView_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
        {

        }
        protected void GvDOSPrintView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void cbAll_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            ASPxGridView grid = (chk.NamingContainer as GridViewHeaderTemplateContainer).Grid;
            if (grid.VisibleRowCount != 0)
                chk.Checked = (grid.Selection.Count == grid.VisibleRowCount);

        }

    }
}