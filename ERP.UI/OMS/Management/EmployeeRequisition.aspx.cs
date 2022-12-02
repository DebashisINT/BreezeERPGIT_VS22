using System;
using System.Data;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_EmployeeRequisition : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new DBEngine();
        BusinessLogicLayer.GenericMethod oGenericMethod;
        string WhichCall;
        DataSet Ds_Global;
        AspxHelper oAspxHelper;
        BusinessLogicLayer.GenericExcelExport oGenericExcelExport;
        string CombinedGroupByQuery = null;
        protected DateTime currentFromDate;
        protected DateTime currentToDate;
        #endregion

        #region PropertyVariable
        int PageSize; int PageNumber; string Emp_ContactID; string IsExported; string IsAuthorized; string DateFrom; string DateTo;
        string Company; string Branch; string ReportTo; string EmployeeType; string RequestType; string ReportType;
        #endregion

        #region Page Properties
        public int P_PageSize
        {
            get { return PageSize; }
            set { PageSize = value; }
        }
        public int P_PageNumber
        {
            get { return PageNumber; }
            set { PageNumber = value; }
        }
        public string P_Emp_ContactID
        {
            get { return Emp_ContactID; }
            set { Emp_ContactID = value; }
        }
        public string P_IsExported
        {
            get { return IsExported; }
            set { IsExported = value; }
        }
        public string P_IsAuthorized
        {
            get { return IsAuthorized; }
            set { IsAuthorized = value; }
        }
        public string P_DateFrom
        {
            get { return DateFrom; }
            set { DateFrom = value; }
        }
        public string P_DateTo
        {
            get { return DateTo; }
            set { DateTo = value; }
        }
        public string P_Company
        {
            get { return Company; }
            set { Company = value; }
        }
        public string P_Branch
        {
            get { return Branch; }
            set { Branch = value; }
        }
        public string P_ReportTo
        {
            get { return ReportTo; }
            set { ReportTo = value; }
        }
        public string P_EmployeeType
        {
            get { return EmployeeType; }
            set { EmployeeType = value; }
        }
        public string P_RequestType
        {
            get { return RequestType; }
            set { RequestType = value; }
        }
        public string P_ReportType
        {
            get { return ReportType; }
            set { ReportType = value; }
        }
        #endregion

        #region CallAjax
        void CallUserList(string WhichCall)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            if (WhichCall == "CallAjax-Employee")
            {
                oGenericMethod.GetContact("A", ref CombinedGroupByQuery, 10, "EM");
            }
            if (WhichCall == "CallAjax-Company")
            {
                oGenericMethod.GetCompany("A", ref CombinedGroupByQuery, 10);
            }
            if (WhichCall == "CallAjax-Branch")
            {
                string CombinedBranchQuery = oGenericMethod.GetAllBranch();
                CombinedGroupByQuery = CombinedBranchQuery;
            }
            if (WhichCall == "CallAjax-ReportTo")
            {
                oGenericMethod.GetContact("A", ref CombinedGroupByQuery, 10, "RT");
            }
            if (WhichCall == "CallAjax-EmployeeType")
            {
                oGenericMethod.GetEmployeeType("A", ref CombinedGroupByQuery, 10);
            }
        }
        #endregion

        #region Page Methods
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id == "CallAjax-Employee" || id == "CallAjax-Company" || id == "CallAjax-Branch" || id == "CallAjax-ReportTo" || id == "CallAjax-EmployeeType")
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
                    if (whichCall == "EMPLOYEE")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = "'" + ids[4] + "'";
                        else
                            recieveServerIDs += ",'" + ids[4] + "'";
                        data = "Employee@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "COMPANY")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = ids[0];
                        else
                            recieveServerIDs += "," + ids[0];
                        data = "Company@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "BRANCH")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = ids[0];
                        else
                            recieveServerIDs += "," + ids[0];
                        data = "Branch@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "REPORTTO")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = "'" + ids[4] + "'";
                        else
                            recieveServerIDs += ",'" + ids[4] + "'";
                        data = "ReportTo@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "EMPLOYEETYPE")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = ids[0];
                        else
                            recieveServerIDs += "," + ids[0];
                        data = "EmployeeType@" + recieveServerIDs.ToString();
                    }
                }
            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        #endregion

        #region User Define Methods
        DataSet Fetch_EmployeeData()
        {
            string[] InputName = new string[17];
            string[] InputType = new string[17];
            string[] InputValue = new string[17];

            InputName[0] = "PageSize";
            InputName[1] = "PageNumber";
            InputName[2] = "Emp_ContactID";
            InputName[3] = "IsExported";
            InputName[4] = "IsAuthorized";
            InputName[5] = "DateFrom";
            InputName[6] = "DateTo";
            InputName[7] = "Company";
            InputName[8] = "Branch";
            InputName[9] = "ReportTo";
            InputName[10] = "EmployeeType";
            InputName[11] = "Type";
            InputName[12] = "ReportType";
            InputName[13] = "SearchBy";
            InputName[14] = "QueryType";
            InputName[15] = "CreateUser";
            InputName[16] = "BranchHierarchy";

            InputType[0] = "I";
            InputType[1] = "I";
            InputType[2] = "V";
            InputType[3] = "C";
            InputType[4] = "C";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "C";
            InputType[13] = "V";
            InputType[14] = "V";
            InputType[15] = "V";
            InputType[16] = "V";

            InputValue[0] = PageSize.ToString();
            InputValue[1] = PageNumber.ToString();
            InputValue[2] = Emp_ContactID;
            InputValue[3] = IsExported;
            InputValue[4] = IsAuthorized;
            InputValue[5] = DateFrom;
            InputValue[6] = DateTo;
            InputValue[7] = Company;
            InputValue[8] = Branch;
            InputValue[9] = ReportTo;
            InputValue[10] = EmployeeType;
            InputValue[11] = RequestType;
            InputValue[12] = ReportType;
            InputValue[13] = HDNSearchBy.Value;
            InputValue[14] = "F";
            InputValue[15] = "";
            InputValue[16] = Session["userbranchHierarchy"].ToString();
            return BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("HR_EmployeeRequisition", InputName, InputType, InputValue);
        }

        DataSet Insert_EmployeeLogData(string empContactID)
        {
            string[] InputName = new string[17];
            string[] InputType = new string[17];
            string[] InputValue = new string[17];

            InputName[0] = "PageSize";
            InputName[1] = "PageNumber";
            InputName[2] = "Emp_ContactID";
            InputName[3] = "IsExported";
            InputName[4] = "IsAuthorized";
            InputName[5] = "DateFrom";
            InputName[6] = "DateTo";
            InputName[7] = "Company";
            InputName[8] = "Branch";
            InputName[9] = "ReportTo";
            InputName[10] = "EmployeeType";
            InputName[11] = "Type";
            InputName[12] = "ReportType";
            InputName[13] = "SearchBy";
            InputName[14] = "QueryType";
            InputName[15] = "CreateUser";
            InputName[16] = "BranchHierarchy";

            InputType[0] = "I";
            InputType[1] = "I";
            InputType[2] = "V";
            InputType[3] = "C";
            InputType[4] = "C";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "C";
            InputType[13] = "V";
            InputType[14] = "V";
            InputType[15] = "V";
            InputType[16] = "V";

            InputValue[0] = PageSize.ToString();
            InputValue[1] = PageNumber.ToString();
            InputValue[2] = empContactID;
            InputValue[3] = IsExported;
            InputValue[4] = IsAuthorized;
            InputValue[5] = DateFrom;
            InputValue[6] = DateTo;
            InputValue[7] = Company;
            InputValue[8] = Branch;
            InputValue[9] = ReportTo;
            InputValue[10] = EmployeeType;
            InputValue[11] = RequestType;
            InputValue[12] = ReportType;
            InputValue[13] = HDNSearchBy.Value;
            InputValue[14] = "I";
            InputValue[15] = HttpContext.Current.Session["userID"].ToString();
            InputValue[16] = Session["userbranchHierarchy"].ToString();

            return BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("HR_EmployeeRequisition", InputName, InputType, InputValue);
        }

        DataSet Delete_EmployeeLogData(string delEmpContactID)
        {
            string[] InputName = new string[17];
            string[] InputType = new string[17];
            string[] InputValue = new string[17];

            InputName[0] = "PageSize";
            InputName[1] = "PageNumber";
            InputName[2] = "Emp_ContactID";
            InputName[3] = "IsExported";
            InputName[4] = "IsAuthorized";
            InputName[5] = "DateFrom";
            InputName[6] = "DateTo";
            InputName[7] = "Company";
            InputName[8] = "Branch";
            InputName[9] = "ReportTo";
            InputName[10] = "EmployeeType";
            InputName[11] = "Type";
            InputName[12] = "ReportType";
            InputName[13] = "SearchBy";
            InputName[14] = "QueryType";
            InputName[15] = "CreateUser";
            InputName[16] = "BranchHierarchy";

            InputType[0] = "I";
            InputType[1] = "I";
            InputType[2] = "V";
            InputType[3] = "C";
            InputType[4] = "C";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "C";
            InputType[13] = "V";
            InputType[14] = "V";
            InputType[15] = "V";
            InputType[16] = "V";


            InputValue[0] = PageSize.ToString();
            InputValue[1] = PageNumber.ToString();
            InputValue[2] = delEmpContactID;
            InputValue[3] = IsExported;
            InputValue[4] = IsAuthorized;
            InputValue[5] = DateFrom;
            InputValue[6] = DateTo;
            InputValue[7] = Company;
            InputValue[8] = Branch;
            InputValue[9] = ReportTo;
            InputValue[10] = EmployeeType;
            InputValue[11] = RequestType;
            InputValue[12] = ReportType;
            InputValue[13] = HDNSearchBy.Value;
            InputValue[14] = "D";
            InputValue[15] = "";
            InputValue[16] = Session["userbranchHierarchy"].ToString();

            return BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("HR_EmployeeRequisition", InputName, InputType, InputValue);
        }

        void SetPropertiesValue()
        {
            //PageSize
            PageSize = 10;
            //Employee
            if (RblEmployee.SelectedIndex == 0) Emp_ContactID = "A";
            else Emp_ContactID = (HDNEmployee.Value.Trim() != "" ? HDNEmployee.Value : "Error:Employee");
            //IsExported
            IsExported = CmbExported.SelectedItem.Value.ToString();
            //IsAuthorized
            IsAuthorized = CmbAuthorized.SelectedItem.Value.ToString();
            //Date
            DateFrom = Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd");
            DateTo = Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd");
            //Company
            if (RblCompany.SelectedIndex == 0) Company = "A";
            else Company = (HDNCompany.Value.Trim() != "" ? HDNCompany.Value : "Error:Company");
            //Branch
            if (RblBranch.SelectedIndex == 0) Branch = "A";
            else Branch = (HDNBranch.Value.Trim() != "" ? HDNBranch.Value : "Error:Branch");
            //ReportTo
            if (RblRptTo.SelectedIndex == 0) ReportTo = "A";
            else ReportTo = (HDNReportTo.Value.Trim() != "" ? HDNReportTo.Value : "Error:ReportTo");
            //EmployeeType
            if (RblEmpType.SelectedIndex == 0) EmployeeType = "A";
            else EmployeeType = (HDNEmpType.Value.Trim() != "" ? HDNEmpType.Value : "Error:EmployeeType");
            //Requisition Type
            RequestType = CmbType.SelectedItem.Value.ToString();
            //ReportType
            ReportType = CmbRptType.SelectedItem.Value.ToString();
        }

        string PageValidation()
        {
            string strError = String.Empty;
            if (Emp_ContactID != "")
                if (Emp_ContactID.Split(':')[0] == "Error")
                    strError = "There is No Proper Employee Selection!!!";
            if (Company != "" && strError != String.Empty)
                if (Company.Split(':')[0] == "Error")
                    strError = "There is No Proper Company Selection!!!";
            if (Branch != "" && strError != String.Empty)
                if (Branch.Split(':')[0] == "Error")
                    strError = "There is No Proper Branch Selection!!!";
            if (ReportTo != "" && strError != String.Empty)
                if (ReportTo.Split(':')[0] == "Error")
                    strError = "There is No Proper ReportTo Selection!!!";
            if (EmployeeType != "" && strError != String.Empty)
                if (EmployeeType.Split(':')[0] == "Error")
                    strError = "There is No Proper EmployeeType Selection!!!";

            return strError;
        }

        void ExportToExcel(DataSet DsExport)
        {
            oGenericExcelExport = new BusinessLogicLayer.GenericExcelExport();
            oDBEngine = new BusinessLogicLayer.DBEngine(null);
            DataTable DtExport = new DataTable();
            string strHeader = String.Empty;
            string[] ReportHeader = new string[1];
            string strSavePath = String.Empty;
            if (HDNSearchBy.Value == "ToReq")
                strHeader = "Employee Detail About To Be Requisitioned";
            else
                strHeader = "Employee Detail About Already Requisitioned";
            if (RequestType != "A") strHeader = strHeader + " For Appointment Letter";
            else if (RequestType != "I") strHeader = strHeader + " For Identity Card";
            else if (RequestType != "V") strHeader = strHeader + " For Visiting Card";

            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");

            if (DsExport.Tables.Count > 0)
                if (DsExport.Tables[0].Rows.Count > 0)
                {
                    DtExport = DsExport.Tables[0];
                    ReportHeader[0] = strHeader;
                    string FileName = "EmployeeRequisitionDetail_" + exlTime;
                    strSavePath = "~/Documents/";
                    if (HDNSearchBy.Value == "ToReq")
                    {
                        //SRLNO,Name,EmpCode,FatherName,DOB,DOJ,DOL,Department,BranchName,CTC,ReportTo,Designation,Company,
                        //Email_Ids,PhoneMobile_Numbers,PanCardNumber,Address,Bank AcDetail,CreatedBy
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                        string[] ColumnSize = { "10", "150", "150", "50", "30", "30", "30", "100", "100", "100", "100", "150", "150", "150", "150", "20", "200", "150", "150" };
                        string[] ColumnWidthSize = { "5", "30", "15", "12", "15", "15", "15", "23", "15", "25", "23", "25", "25", "25", "20", "15", "30", "20", "20" };

                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, ReportHeader, null);
                    }
                    else
                    {
                        //SRLNO,Name,EmpCode,Request_Type,IsExported,ExportedBy,Export_Date,CreatedBy,Create_Date,
                        //FatherName,DOB,DOJ,DOL,Department,BranchName,CTC,ReportTo,Designation,Company,
                        //Email_Ids,PhoneMobile_Numbers,PanCardNumber,Address,Bank AcDetail,CreatedBy
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                        string[] ColumnSize = { "10", "150", "150", "50", "30", "150", "15", "150", "15", "150", "15", "150", "15", "150", "15", "150", "30", "30", "30", "100", "100", "100", "100", "150", "150", "150", "20", "200", "200", "150" };
                        string[] ColumnWidthSize = { "5", "30", "15", "20", "15", "20", "10", "20", "10", "20", "10", "20", "10", "20", "10", "20", "15", "15", "15", "23", "15", "25", "23", "25", "25", "25", "15", "15", "30", "20" };

                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, ReportHeader, null);
                    }
                }
        }
        #endregion

        #region BusinessLogic
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] PageSession = null;
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                oGenericMethod.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
            if (!IsPostBack)
            {
                oDBEngine = new BusinessLogicLayer.DBEngine(null);
                DateTime Date = oDBEngine.GetDate();
                currentFromDate = Date.AddDays((-1 * Date.Day) + 1);
                currentToDate = Date;
                DtFrom.Value = currentFromDate;
                DtTo.Value = currentToDate;
            }

            if (hdn_GridBindOrNotBind.Value != "False")
            {
                PageSize = Hdn_PageSize.Value != "" ? Convert.ToInt32(Hdn_PageSize.Value) : 0;
                PageNumber = Hdn_PageNumber.Value != "" ? Convert.ToInt32(Hdn_PageNumber.Value) : 0;
                Emp_ContactID = Hdn_Emp_ContactID.Value;
                IsExported = Hdn_Exported.Value;
                IsAuthorized = Hdn_Authorized.Value;
                DateFrom = Hdn_DateFrom.Value;
                DateTo = Hdn_DateTo.Value;
                Company = Hdn_Company.Value;
                Branch = Hdn_Branch.Value;
                ReportTo = Hdn_ReportTo.Value;
                EmployeeType = Hdn_EmployeeType.Value;
                RequestType = Hdn_Type.Value;
                ReportType = Hdn_ReportType.Value;
                if (PageSize != 0 && PageNumber != 0)
                {
                    oAspxHelper = new AspxHelper();
                    //For Column Show / Hide Related=============================================
                    GridColumnShowHide();

                    oAspxHelper.BindGrid(GrdEmployeeRequisition, Fetch_EmployeeData());
                }
            }
        }
        protected void GridColumnShowHide()
        {
            //For Column Show / Hide Related=============================================
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int countAuth = oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", "999~GS_NOOFREQAUTH");

            if (HDNSearchBy.Value == "ToReq")
            {
                GrdEmployeeRequisition.Columns["Srl."].Visible = true;
                GrdEmployeeRequisition.Columns["Name"].Visible = true;
                GrdEmployeeRequisition.Columns["EmpCode"].Visible = true;
                //---------------------Start Log Table Data ------------------------------------
                GrdEmployeeRequisition.Columns["Requisition Type"].Visible = false;
                GrdEmployeeRequisition.Columns["Export"].Visible = false;
                GrdEmployeeRequisition.Columns["Exported By"].Visible = false;
                GrdEmployeeRequisition.Columns["Export Date"].Visible = false;
                GrdEmployeeRequisition.Columns["First Authorized By"].Visible = false;
                GrdEmployeeRequisition.Columns["First Authorized Date"].Visible = false;
                GrdEmployeeRequisition.Columns["Second Authorized By"].Visible = false;
                GrdEmployeeRequisition.Columns["Second Authorized Date"].Visible = false;
                GrdEmployeeRequisition.Columns["Third Authorized By"].Visible = false;
                GrdEmployeeRequisition.Columns["Third Authorized Date"].Visible = false;
                GrdEmployeeRequisition.Columns["Created By"].Visible = false;
                GrdEmployeeRequisition.Columns["Create Date"].Visible = false;
                //--------------------End Log Table Data----------------------------------------
                GrdEmployeeRequisition.Columns["Father's Name"].Visible = true;
                GrdEmployeeRequisition.Columns["DOB"].Visible = true;
                GrdEmployeeRequisition.Columns["DOJ"].Visible = true;
                GrdEmployeeRequisition.Columns["DOL"].Visible = true;
                GrdEmployeeRequisition.Columns["Department"].Visible = true;
                GrdEmployeeRequisition.Columns["Branch"].Visible = true;
                GrdEmployeeRequisition.Columns["CTC"].Visible = true;
                GrdEmployeeRequisition.Columns["ReportTo"].Visible = true;
                GrdEmployeeRequisition.Columns["Designation"].Visible = true;
                GrdEmployeeRequisition.Columns["Company"].Visible = true;
                GrdEmployeeRequisition.Columns["EmpType"].Visible = true;
                GrdEmployeeRequisition.Columns["EmailId(s)"].Visible = true;
                GrdEmployeeRequisition.Columns["Phone/Mobile"].Visible = true;
                GrdEmployeeRequisition.Columns["PanCard"].Visible = true;
                GrdEmployeeRequisition.Columns["Address"].Visible = true;
                GrdEmployeeRequisition.Columns["Ac[BankName][Branch][AcType]"].Visible = true;
                GrdEmployeeRequisition.Columns["Emp CreatedBy"].Visible = true;
            }
            else if (HDNSearchBy.Value == "Req")
            {

                GrdEmployeeRequisition.Columns["Srl."].Visible = true;
                GrdEmployeeRequisition.Columns["Name"].Visible = true;
                GrdEmployeeRequisition.Columns["EmpCode"].Visible = true;
                //---------------------Start Log Table Data ------------------------------------
                GrdEmployeeRequisition.Columns["Requisition Type"].Visible = true;
                GrdEmployeeRequisition.Columns["Export"].Visible = true;
                GrdEmployeeRequisition.Columns["Exported By"].Visible = true;
                GrdEmployeeRequisition.Columns["Export Date"].Visible = true;
                if (countAuth == 3)
                {
                    GrdEmployeeRequisition.Columns["First Authorized By"].Visible = true;
                    GrdEmployeeRequisition.Columns["First Authorized Date"].Visible = true;
                    GrdEmployeeRequisition.Columns["Second Authorized By"].Visible = true;
                    GrdEmployeeRequisition.Columns["Second Authorized Date"].Visible = true;
                    GrdEmployeeRequisition.Columns["Third Authorized By"].Visible = true;
                    GrdEmployeeRequisition.Columns["Third Authorized Date"].Visible = true;
                }
                else if (countAuth == 2)
                {
                    GrdEmployeeRequisition.Columns["First Authorized By"].Visible = true;
                    GrdEmployeeRequisition.Columns["First Authorized Date"].Visible = true;
                    GrdEmployeeRequisition.Columns["Second Authorized By"].Visible = true;
                    GrdEmployeeRequisition.Columns["Second Authorized Date"].Visible = true;
                    GrdEmployeeRequisition.Columns["Third Authorized By"].Visible = false;
                    GrdEmployeeRequisition.Columns["Third Authorized Date"].Visible = false;
                }
                else if (countAuth == 1)
                {
                    GrdEmployeeRequisition.Columns["First Authorized By"].Visible = true;
                    GrdEmployeeRequisition.Columns["First Authorized Date"].Visible = true;
                    GrdEmployeeRequisition.Columns["Second Authorized By"].Visible = false;
                    GrdEmployeeRequisition.Columns["Second Authorized Date"].Visible = false;
                    GrdEmployeeRequisition.Columns["Third Authorized By"].Visible = false;
                    GrdEmployeeRequisition.Columns["Third Authorized Date"].Visible = false;
                }
                else
                {
                    GrdEmployeeRequisition.Columns["First Authorized By"].Visible = false;
                    GrdEmployeeRequisition.Columns["First Authorized Date"].Visible = false;
                    GrdEmployeeRequisition.Columns["Second Authorized By"].Visible = false;
                    GrdEmployeeRequisition.Columns["Second Authorized Date"].Visible = false;
                    GrdEmployeeRequisition.Columns["Third Authorized By"].Visible = false;
                    GrdEmployeeRequisition.Columns["Third Authorized Date"].Visible = false;
                }
                GrdEmployeeRequisition.Columns["Created By"].Visible = true;
                GrdEmployeeRequisition.Columns["Create Date"].Visible = true;
                //---------------------End Log Table Data ------------------------------------
                GrdEmployeeRequisition.Columns["Father's Name"].Visible = true;
                GrdEmployeeRequisition.Columns["DOB"].Visible = true;
                GrdEmployeeRequisition.Columns["DOJ"].Visible = true;
                GrdEmployeeRequisition.Columns["DOL"].Visible = true;
                GrdEmployeeRequisition.Columns["Department"].Visible = true;
                GrdEmployeeRequisition.Columns["Branch"].Visible = true;
                GrdEmployeeRequisition.Columns["CTC"].Visible = true;
                GrdEmployeeRequisition.Columns["ReportTo"].Visible = true;
                GrdEmployeeRequisition.Columns["Designation"].Visible = true;
                GrdEmployeeRequisition.Columns["Company"].Visible = true;
                GrdEmployeeRequisition.Columns["EmpType"].Visible = true;
                GrdEmployeeRequisition.Columns["EmailId(s)"].Visible = true;
                GrdEmployeeRequisition.Columns["Phone/Mobile"].Visible = true;
                GrdEmployeeRequisition.Columns["PanCard"].Visible = true;
                GrdEmployeeRequisition.Columns["Address"].Visible = true;
                GrdEmployeeRequisition.Columns["Ac[BankName][Branch][AcType]"].Visible = true;
                GrdEmployeeRequisition.Columns["Emp CreatedBy"].Visible = false;
            }
        }
        protected void GrdEmployeeRequisition_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdEmployeeRequisition.JSProperties["cpErrorMsg"] = null;
            GrdEmployeeRequisition.JSProperties["cpPagerSetting"] = null;
            GrdEmployeeRequisition.JSProperties["cpExcelExport"] = null;
            GrdEmployeeRequisition.JSProperties["cpRefreshNavPanel"] = null;
            GrdEmployeeRequisition.JSProperties["cpCallOtherWhichCallCondition"] = null;
            GrdEmployeeRequisition.JSProperties["cpSetGlobalFields"] = null;
            GrdEmployeeRequisition.JSProperties["cpInfoMsg"] = null;

            //For Column Show / Hide Related=============================================
            GridColumnShowHide();

            //Initialize Required Objects
            Ds_Global = new DataSet();
            oAspxHelper = new AspxHelper();

            WhichCall = e.Parameters.Split('~')[0];
            string strFromDOJ = String.Empty, strToDOJ = String.Empty, strSearchString = String.Empty,
                strSearchBy = String.Empty, strFindOption = String.Empty;
            string strPageValidationMsg = String.Empty;

            string infoMessage = String.Empty;

            //common parameter
            oDBEngine = new BusinessLogicLayer.DBEngine(null);
            strFromDOJ = oDBEngine.GetDate(DBEngine.DateConvertFrom.UTCToOnlyDate, e.Parameters.Split('~')[1]);
            strToDOJ = oDBEngine.GetDate(DBEngine.DateConvertFrom.UTCToOnlyDate, e.Parameters.Split('~')[2]);

            int TotalItems = 0;
            int TotalPage = 0;
            if (WhichCall == "Show")
            {
                //Set Properties Value
                SetPropertiesValue();

                //Set UnCommon Property 
                PageNumber = 1;
                ReportType = "S";

                strPageValidationMsg = PageValidation();
                if (strPageValidationMsg == String.Empty)
                {

                    Ds_Global = Fetch_EmployeeData();
                    if (Ds_Global.Tables.Count > 0)
                    {
                        if (Ds_Global.Tables[0].Rows.Count > 0)
                        {
                            TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
                            TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                            GrdEmployeeRequisition.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();

                            oAspxHelper.BindGrid(GrdEmployeeRequisition, Ds_Global);
                        }
                        else
                            oAspxHelper.BindGrid(GrdEmployeeRequisition);
                    }
                    else
                        oAspxHelper.BindGrid(GrdEmployeeRequisition);
                }
                else
                    GrdEmployeeRequisition.JSProperties["cpErrorMsg"] = strPageValidationMsg;

                //Assign Value To HiddenField So That PageLoad Binding Can Use These HiddenField
                GrdEmployeeRequisition.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' + Emp_ContactID + '~' +
                IsExported + '~' + IsAuthorized + '~' + DateFrom + '~' + DateTo + '~' + Company + '~' + Branch + '~' + ReportTo + '~' + EmployeeType + '~' +
                RequestType + '~' + "S";
            }
            if (WhichCall == "SearchByNavigation")
            {
                string strPageNum = String.Empty;
                string strNavDirection = String.Empty;
                int PageNumAfterNav = 0;
                strPageNum = e.Parameters.Split('~')[3];
                strNavDirection = e.Parameters.Split('~')[4];

                //Set Page Number
                if (strNavDirection == "RightNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) + 10;
                if (strNavDirection == "LeftNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) - 10;
                if (strNavDirection == "PageNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum);

                Ds_Global = new DataSet();

                //When Show Filter Active With SearchString
                //Then Grid Bind With SearchString Cretaria
                //other normally as it bind

                //Set Properties Value
                SetPropertiesValue();
                //Set UnCommon Property 
                PageNumber = PageNumAfterNav;
                ReportType = "S";

                Ds_Global = Fetch_EmployeeData();

                if (Ds_Global.Tables.Count > 0)
                {
                    if (Ds_Global.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;

                        oAspxHelper.BindGrid(GrdEmployeeRequisition, Ds_Global);
                        GrdEmployeeRequisition.JSProperties["cpRefreshNavPanel"] = strNavDirection + '~' + strPageNum + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAspxHelper.BindGrid(GrdEmployeeRequisition);
                }
                else
                    oAspxHelper.BindGrid(GrdEmployeeRequisition);

                //Assign Value To HiddenField So That PageLoad Binding Can Use These HiddenField
                GrdEmployeeRequisition.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' + Emp_ContactID + '~' +
                IsExported + '~' + IsAuthorized + '~' + DateFrom + '~' + DateTo + '~' + Company + '~' + Branch + '~' + ReportTo + '~' + EmployeeType + '~' +
                RequestType + '~' + "S";
            }
            if (WhichCall == "ExcelExport")
            {
                GrdEmployeeRequisition.JSProperties["cpExcelExport"] = "T";
            }
            if (WhichCall == "GenerateAll")
            {
                Ds_Global = Insert_EmployeeLogData("A");

                if (Ds_Global.Tables.Count > 0)
                {
                    if (Ds_Global.Tables[0].Rows.Count > 0)
                    {
                        infoMessage = Ds_Global.Tables[0].Rows[0][0].ToString();
                    }
                }
                GrdEmployeeRequisition.JSProperties["cpInfoMsg"] = infoMessage;
            }
            if (WhichCall == "DeleteAll")
            {
                Ds_Global = Delete_EmployeeLogData("A");
                if (Ds_Global.Tables.Count > 0)
                {
                    if (Ds_Global.Tables[0].Rows.Count > 0)
                    {
                        infoMessage = Ds_Global.Tables[0].Rows[0][0].ToString();
                    }
                }
                GrdEmployeeRequisition.JSProperties["cpInfoMsg"] = infoMessage;
            }
            if (WhichCall == "Generate")
            {
                if ((HDNCheckedEmpID.Value != null) && (HDNCheckedEmpID.Value.Trim() != String.Empty))
                {
                    Ds_Global = Insert_EmployeeLogData(HDNCheckedEmpID.Value);

                    if (Ds_Global.Tables.Count > 0)
                    {
                        if (Ds_Global.Tables[0].Rows.Count > 0)
                        {
                            infoMessage = Ds_Global.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    GrdEmployeeRequisition.JSProperties["cpInfoMsg"] = infoMessage;
                }
                else
                {
                    GrdEmployeeRequisition.JSProperties["cpInfoMsg"] = "Please Try Again!!!";
                }
            }
            if (WhichCall == "Delete")
            {
                if ((HDNCheckedEmpID.Value != null) && (HDNCheckedEmpID.Value.Trim() != String.Empty))
                {
                    Ds_Global = new DataSet();
                    Ds_Global = Delete_EmployeeLogData(HDNCheckedEmpID.Value);

                    if (Ds_Global.Tables.Count > 0)
                    {
                        if (Ds_Global.Tables[0].Rows.Count > 0)
                        {
                            infoMessage = Ds_Global.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    GrdEmployeeRequisition.JSProperties["cpInfoMsg"] = infoMessage;
                }
                else
                {
                    GrdEmployeeRequisition.JSProperties["cpInfoMsg"] = "Please Try Again!!!";
                }
            }
            //Dispose Object
            if (Ds_Global != null)
                Ds_Global.Dispose();

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Set Properties Value
            SetPropertiesValue();
            PageNumber = 0;
            ReportType = "E";
            ExportToExcel(Fetch_EmployeeData());
        }
        #region Inactive Code Generate/Delete By Button Click
        //protected void BtnAllGenerateRequest_Click(object sender, EventArgs e)
        //{
        //    Insert_EmployeeLogData("A");
        //}
        //protected void BtnGenerateRequest_Click(object sender, EventArgs e)
        //{
        //    if (HDNCheckedEmpID.Value != null || HDNCheckedEmpID.Value.Trim() != String.Empty)
        //    {
        //        Insert_EmployeeLogData(HDNCheckedEmpID.Value);
        //    }
        //    //hdn fileld empty
        //}
        //protected void BtnAllDeleteRequest_Click(object sender, EventArgs e)
        //{
        //    Delete_EmployeeLogData("A");
        //}
        //protected void BtnDeleteRequest_Click(object sender, EventArgs e)
        //{
        //    if (HDNCheckedEmpID.Value != null || HDNCheckedEmpID.Value.Trim() != String.Empty)
        //    {
        //        Delete_EmployeeLogData(HDNCheckedEmpID.Value);
        //    }
        //}
        #endregion
        #endregion
    }
}