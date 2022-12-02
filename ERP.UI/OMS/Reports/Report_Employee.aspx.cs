using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class reports_report_employee : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        string data;
        // DBEngine oDBEngine;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        GenericMethod oGenericMethod;

        string WhichCall;
        DataSet Ds_Global;
        AspxHelper oAspxHelper;
        GenericExcelExport oGenericExcelExport;
        #endregion

        #region PropertyVariable
        int PageSize; int PageNumber; string Emp_ContactID; string Emp_Status; string DateFrom; string DateTo; string Company;
        string Branch; string ReportTo; string EmployeeType; string DevXFilterOn; string DevXFilterString; string ReportType;
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
        public string P_Emp_Status
        {
            get { return Emp_Status; }
            set { Emp_Status = value; }
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
        public string P_DevXFilterOn
        {
            get { return DevXFilterOn; }
            set { DevXFilterOn = value; }
        }
        public string P_DevXFilterString
        {
            get { return DevXFilterString; }
            set { DevXFilterString = value; }
        }
        public string P_ReportType
        {
            get { return ReportType; }
            set { ReportType = value; }
        }

        #endregion
        #region Page Methods
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + val[0] + ";" + val[1];
                }
            }
            if (idlist[0] == "Company")
            {
                data = "Company~" + str;
            }
            if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            if (idlist[0] == "Employee")
            {
                data = "Employee~" + str;
            }
            if (idlist[0] == "ReportTo")
            {
                data = "ReportTo~" + str;
            }
            if (idlist[0] == "Type")
            {
                data = "Type~" + str;
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
            string[] InputName = new string[13];
            string[] InputType = new string[13];
            string[] InputValue = new string[13];

            InputName[0] = "PageSize";
            InputName[1] = "PageNumber";
            InputName[2] = "Emp_ContactID";
            InputName[3] = "Emp_Status";
            InputName[4] = "DateFrom";
            InputName[5] = "DateTo";
            InputName[6] = "Company";
            InputName[7] = "Branch";
            InputName[8] = "ReportTo";
            InputName[9] = "EmployeeType";
            InputName[10] = "DevXFilterOn";
            InputName[11] = "DevXFilterString";
            InputName[12] = "ReportType";

            InputType[0] = "I";
            InputType[1] = "I";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "C";

            InputValue[0] = PageSize.ToString();
            InputValue[1] = PageNumber.ToString();
            InputValue[2] = Emp_ContactID;
            InputValue[3] = Emp_Status;
            InputValue[4] = DateFrom;
            InputValue[5] = DateTo;
            InputValue[6] = Company;
            InputValue[7] = Branch;
            InputValue[8] = ReportTo;
            InputValue[9] = EmployeeType;
            InputValue[10] = DevXFilterOn;
            InputValue[11] = DevXFilterString;
            InputValue[12] = ReportType;

            return SQLProcedures.SelectProcedureArrDS("HR_Report_Employees", InputName, InputType, InputValue);
        }
        void SetPropertiesValue()
        {
            //Employee
            if (RadEmployeeA.Checked) Emp_ContactID = "ALL";
            else Emp_ContactID = (HDNEmployee.Value.Trim() != "" ? HDNEmployee.Value : "Error:Employee");
            //Status
            if (RadActiveEmp.Checked) Emp_Status = "A";
            else if (RadExEmp.Checked) Emp_Status = "E";
            else if (RadActiveExBoth.Checked) Emp_Status = "AcExB";
            //Date
            if (RadDateRangeA.Checked)
            {
                DateFrom = "1900-01-01";
                DateTo = "9999-12-31";
            }
            else
            {
                DateFrom = Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd");
                DateTo = Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd");
            }
            //Company
            if (RadCompanyA.Checked) Company = "ALL";
            else Company = (HDNCompany.Value.Trim() != "" ? HDNCompany.Value : "Error:Company");
            //Branch
            if (RadBranchA.Checked) Branch = "ALL";
            else Branch = (HDNBranch.Value.Trim() != "" ? HDNBranch.Value : "Error:Branch");
            //ReportTo
            if (RadReportToA.Checked) ReportTo = "ALL";
            else ReportTo = (HDNReportTo.Value.Trim() != "" ? HDNReportTo.Value : "Error:ReportTo");
            //Type
            if (TadTypeA.Checked) EmployeeType = "ALL";
            else EmployeeType = (HDNType.Value.Trim() != "" ? HDNType.Value : "Error:Type");
            //PageSize
            PageSize = 10;
            DevXFilterOn = "N";
            DevXFilterString = "";
        }
        void SetPropertiesValue(string ShowFilterString)
        {
            //Employee
            if (RadEmployeeA.Checked) Emp_ContactID = "ALL";
            else Emp_ContactID = (HDNEmployee.Value.Trim() != "" ? HDNEmployee.Value : "Error:Employee");
            //Status
            if (RadActiveEmp.Checked) Emp_Status = "A";
            else if (RadExEmp.Checked) Emp_Status = "E";
            else if (RadActiveExBoth.Checked) Emp_Status = "AcExB";
            //Date
            if (RadDateRangeA.Checked)
            {
                DateFrom = "1900-01-01";
                DateTo = "9999-12-31";
            }
            else
            {
                DateFrom = Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd");
                DateTo = Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd");
            }
            //Company
            if (RadCompanyA.Checked) Company = "ALL";
            else Company = (HDNCompany.Value.Trim() != "" ? HDNCompany.Value : "Error:Company");
            //Branch
            if (RadBranchA.Checked) Branch = "ALL";
            else Branch = (HDNBranch.Value.Trim() != "" ? HDNBranch.Value : "Error:Branch");
            //ReportTo
            if (RadReportToA.Checked) ReportTo = "ALL";
            else ReportTo = (HDNReportTo.Value.Trim() != "" ? HDNReportTo.Value : "Error:ReportTo");
            //Type
            if (TadTypeA.Checked) EmployeeType = "ALL";
            else EmployeeType = (HDNType.Value.Trim() != "" ? HDNType.Value : "Error:Type");
            //PageSize
            PageSize = 10;
            DevXFilterOn = "Y";
            DevXFilterString = ShowFilterString;
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
            oGenericExcelExport = new GenericExcelExport();
            //oDBEngine = new DBEngine(null);
            DataTable DtExport = new DataTable();
            string strHeader = String.Empty;
            string[] ReportHeader = new string[1];
            string strSavePath = String.Empty;

            strHeader = "Employee Detail";

            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");

            if (DsExport.Tables.Count > 0)
                if (DsExport.Tables[0].Rows.Count > 0)
                {
                    DtExport = DsExport.Tables[0];
                    ReportHeader[0] = strHeader;
                    string FileName = "EmployeeDetail_" + exlTime;
                    strSavePath = "~/Documents/";

                    //SRLNO,Name,EmpCode,FatherName,DOB,DOJ,DOL,Department,BranchName,CTC,ReportTo,Designation,Company,
                    //Email_Ids,PhoneMobile_Numbers,PanCardNumber,Address,Bank AcDetail,CreatedBy
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                    string[] ColumnSize = { "10", "150", "150", "50", "50", "50", "50", "100", "100", "100", "100", "150", "150", "150", "150", "200", "200", "150", "150" };
                    string[] ColumnWidthSize = { "5", "30", "30", "12", "22", "23", "22", "23", "15", "25", "23", "25", "25", "25", "20", "30", "30", "20", "20" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, ReportHeader, null);
                }
        }
        #endregion

        #region BusinessLogic
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
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
            if (!IsPostBack)
            {
                // oDBEngine=new DBEngine(null);
                DateTime Date = oDBEngine.GetDate();
                DtFrom.Value = Date.AddDays((-1 * Date.Day) + 1);
                DtTo.Value = Date;
            }

            if (hdn_GridBindOrNotBind.Value != "False")
            {
                PageSize = Hdn_PageSize.Value != "" ? Convert.ToInt32(Hdn_PageSize.Value) : 0;
                PageNumber = Hdn_PageNumber.Value != "" ? Convert.ToInt32(Hdn_PageNumber.Value) : 0;
                Emp_ContactID = Hdn_Emp_ContactID.Value;
                Emp_Status = Hdn_Emp_Status.Value;
                DateFrom = Hdn_DateFrom.Value;
                DateTo = Hdn_DateTo.Value;
                Company = Hdn_Company.Value;
                Branch = Hdn_Branch.Value;
                ReportTo = Hdn_ReportTo.Value;
                EmployeeType = Hdn_EmployeeType.Value;
                DevXFilterOn = Hdn_DevXFilterOn.Value;
                DevXFilterString = Hdn_DevXFilterString.Value;
                ReportType = Hdn_ReportType.Value;
                if (PageSize != 0 && PageNumber != 0)
                {
                    oAspxHelper = new AspxHelper();
                    oAspxHelper.BindGrid(GrdEmployee, Fetch_EmployeeData());
                }
            }
        }
        protected void GrdEmployee_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdEmployee.JSProperties["cpErrorMsg"] = null;
            GrdEmployee.JSProperties["cpPagerSetting"] = null;
            GrdEmployee.JSProperties["cpExcelExport"] = null;
            GrdEmployee.JSProperties["cpRefreshNavPanel"] = null;
            GrdEmployee.JSProperties["cpCallOtherWhichCallCondition"] = null;
            GrdEmployee.JSProperties["cpSetGlobalFields"] = null;

            //Initialize Required Objects
            Ds_Global = new DataSet();
            oAspxHelper = new AspxHelper();

            WhichCall = e.Parameters.Split('~')[0];
            string strFromDOJ = String.Empty, strToDOJ = String.Empty, strSearchString = String.Empty,
                strSearchBy = String.Empty, strFindOption = String.Empty;
            string strPageValidationMsg = String.Empty;

            //common parameter
            //oDBEngine = new DBEngine(null);
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
                            GrdEmployee.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                            oAspxHelper.BindGrid(GrdEmployee, Ds_Global);
                        }
                        else
                            oAspxHelper.BindGrid(GrdEmployee);
                    }
                    else
                        oAspxHelper.BindGrid(GrdEmployee);
                }
                else
                    GrdEmployee.JSProperties["cpErrorMsg"] = strPageValidationMsg;

                //Assign Value To HiddenField So That PageLoad Binding Can Use These HiddenField
                GrdEmployee.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' + Emp_ContactID + '~' +
                Emp_Status + '~' + DateFrom + '~' + DateTo + '~' + Company + '~' + Branch + '~' + ReportTo + '~' + EmployeeType + '~' + DevXFilterOn + '~' +
                DevXFilterString + '~' + "S";
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
                        oAspxHelper.BindGrid(GrdEmployee, Ds_Global);
                        GrdEmployee.JSProperties["cpRefreshNavPanel"] = strNavDirection + '~' + strPageNum + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAspxHelper.BindGrid(GrdEmployee);
                }
                else
                    oAspxHelper.BindGrid(GrdEmployee);

                //Assign Value To HiddenField So That PageLoad Binding Can Use These HiddenField
                GrdEmployee.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' + Emp_ContactID + '~' +
                Emp_Status + '~' + DateFrom + '~' + DateTo + '~' + Company + '~' + Branch + '~' + ReportTo + '~' + EmployeeType + '~' + DevXFilterOn + '~' +
                DevXFilterString + '~' + "S";
            }
            if (WhichCall == "ExcelExport")
            {
                GrdEmployee.JSProperties["cpExcelExport"] = "T";
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
        protected void GrdEmployee_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
        {
            //if (P_ShowFilter_SearchString != null)
            //    if (P_ShowFilter_SearchString.ToString().Trim() != String.Empty)
            //        P_ShowFilter_SearchString = null;

            //Ds_Global = new DataSet();
            //int TotalItems = 0;
            //int TotalPage = 0;
            ////For All Date
            //if (P_ShowFilter_SearchString != null)
            //{
            //    if (P_ShowFilter_SearchString.ToString().Trim() != String.Empty)
            //        P_ShowFilter_SearchString = P_ShowFilter_SearchString.ToString().Trim() + " And " + e.Criteria.ToString().Trim();
            //    else
            //        P_ShowFilter_SearchString = e.Criteria.ToString().Trim();
            //}
            //else
            //    P_ShowFilter_SearchString = e.Criteria.ToString().Trim();

            //Ds_Global = Fetch_EmployeeData(P_FromDOJ == "" ? "1900-01-01" : P_FromDOJ, P_ToDOJ == "" ? "9999-12-31" : P_ToDOJ, P_PageSize,
            //       P_PageNumAfterNav, "", "", "", "S", "Y", P_ShowFilter_SearchString.ToString());

            //if (Ds_Global.Tables.Count > 0)
            //{
            //    if (Ds_Global.Tables[0].Rows.Count > 0)
            //    {
            //        TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
            //        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
            //        oAspxHelper.BindGrid(GrdEmployee, Ds_Global);
            //        //Here I Passed ShowBtnClick Parameter Cause It ReInitialize Grid Like Show Button Functionality
            //        GrdEmployee.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick" + '~' + P_PageNumAfterNav + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
            //    }
            //    else
            //        oAspxHelper.BindGrid(GrdEmployee);
            //}
            //else
            //    oAspxHelper.BindGrid(GrdEmployee);

            ////Dispose Object
            //if (Ds_Global != null)
            //    Ds_Global.Dispose();


        }
        #endregion

    }
}