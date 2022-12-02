using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
//////using DevExpress.Web;
//using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_EmployeeAuthorisation : System.Web.UI.Page
    {
        #region GlobalVariable
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        AspxHelper oAx = new AspxHelper();
        BusinessLogicLayer.GenericXML GX = new BusinessLogicLayer.GenericXML();
        BusinessLogicLayer.GenericMethod oGM = new BusinessLogicLayer.GenericMethod();
        BusinessLogicLayer.GenericExcelExport oGenericExcelExport = new BusinessLogicLayer.GenericExcelExport();
        #endregion

        #region Page Property
        int PageNumber; string DateFrom; string DateTo; string AuthorizeType; int PageSize; string EmpID; string DoAuthorisation; string ShowType; string AuthorizeFor;
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
        public string P_AuthorizeType
        {
            get { return AuthorizeType; }
            set { AuthorizeType = value; }
        }
        public string P_EmpID
        {
            get { return EmpID; }
            set { EmpID = value; }
        }
        public string P_DoAuthorisation
        {
            get { return DoAuthorisation; }
            set { DoAuthorisation = value; }
        }
        public string P_ShowType
        {
            get { return ShowType; }
            set { ShowType = value; }
        }
        public string P_AuthorizeFor
        {
            get { return AuthorizeFor; }
            set { AuthorizeFor = value; }
        }
        #endregion

        #region PageClass
        DataSet fetchRecord(string AuthorizeType, string FrmDt, string ToDt)
        {
            string[] sqlParameterName = new string[11];
            string[] sqlParameterValue = new string[11];
            string[] sqlParameterType = new string[11];

            if (HDNCheckedEmpCode.Value.ToString() == "" || HDNCheckedEmpCode.Value.ToString() == null)
            {
                sqlParameterName[0] = "WhichCall";
                sqlParameterValue[0] = "Employee";
                sqlParameterType[0] = "V";

                sqlParameterName[1] = "AuthorizeType";
                sqlParameterValue[1] = AuthorizeType;
                sqlParameterType[1] = "V";

                sqlParameterName[2] = "FromDate";
                sqlParameterValue[2] = FrmDt;
                sqlParameterType[2] = "D";

                sqlParameterName[3] = "ToDate";
                sqlParameterValue[3] = ToDt;
                sqlParameterType[3] = "D";

                sqlParameterName[4] = "PageSize";
                sqlParameterValue[4] = PageSize.ToString();
                sqlParameterType[4] = "I";

                sqlParameterName[5] = "PageNumber";
                sqlParameterValue[5] = PageNumber.ToString();
                sqlParameterType[5] = "I";

                sqlParameterName[6] = "EmpID";
                sqlParameterValue[6] = "null";
                sqlParameterType[6] = "V";

                sqlParameterName[7] = "User";
                sqlParameterValue[7] = Session["UserID"].ToString();
                sqlParameterType[7] = "V";

                sqlParameterName[8] = "ShowType";
                sqlParameterValue[8] = ShowType;
                sqlParameterType[8] = "V";

                sqlParameterName[9] = "AuthorizeFor";
                sqlParameterValue[9] = AuthorizeFor;
                sqlParameterType[9] = "V";

                sqlParameterName[10] = "BranchHierarchy";
                sqlParameterValue[10] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                sqlParameterType[10] = "V";
            }
            else
            {
                sqlParameterName[0] = "WhichCall";
                sqlParameterValue[0] = DoAuthorisation;
                sqlParameterType[0] = "V";

                sqlParameterName[1] = "AuthorizeType";
                sqlParameterValue[1] = "null";
                sqlParameterType[1] = "V";

                sqlParameterName[2] = "FromDate";
                sqlParameterValue[2] = FrmDt;
                sqlParameterType[2] = "D";

                sqlParameterName[3] = "ToDate";
                sqlParameterValue[3] = ToDt;
                sqlParameterType[3] = "D";

                sqlParameterName[4] = "PageSize";
                sqlParameterValue[4] = "0";
                sqlParameterType[4] = "I";

                sqlParameterName[5] = "PageNumber";
                sqlParameterValue[5] = "0";
                sqlParameterType[5] = "I";

                sqlParameterName[6] = "EmpID";
                sqlParameterValue[6] = EmpID;
                sqlParameterType[6] = "V";

                sqlParameterName[7] = "User";
                sqlParameterValue[7] = Session["UserID"].ToString();
                sqlParameterType[7] = "V";

                sqlParameterName[8] = "ShowType";
                sqlParameterValue[8] = "null";
                sqlParameterType[8] = "V";

                sqlParameterName[9] = "AuthorizeFor";
                sqlParameterValue[9] = AuthorizeFor;
                sqlParameterType[9] = "V";

                sqlParameterName[10] = "BranchHierarchy";
                sqlParameterValue[10] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                sqlParameterType[10] = "V";

            }


            return SQLProcedures.SelectProcedureArrDS("hr_EmployeeAuthorization", sqlParameterName, sqlParameterType, sqlParameterValue);


        }
        protected void ChkBxAll_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            ASPxGridView grid = (chk.NamingContainer as GridViewHeaderTemplateContainer).Grid;
            chk.Checked = (grid.Selection.Count == grid.VisibleRowCount);

        }
        protected void GridVwColumnShowHide()
        {
            int countAuth = oGM.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", "999~GS_NOOFREQAUTH");

            if (countAuth == 3)
            {
                GvEmployeeDtl.Columns["AuthorizeUser1"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeDateTime1"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeUser2"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeDateTime2"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeUser3"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeDateTime3"].Visible = true;
            }
            else if (countAuth == 2)
            {
                GvEmployeeDtl.Columns["AuthorizeUser1"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeDateTime1"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeUser2"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeDateTime2"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeUser3"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeDateTime3"].Visible = false;
            }
            else if (countAuth == 1)
            {
                GvEmployeeDtl.Columns["AuthorizeUser1"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeDateTime1"].Visible = true;
                GvEmployeeDtl.Columns["AuthorizeUser2"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeDateTime2"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeUser3"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeDateTime3"].Visible = false;
            }
            else
            {
                GvEmployeeDtl.Columns["AuthorizeUser1"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeDateTime1"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeUser2"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeDateTime2"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeUser3"].Visible = false;
                GvEmployeeDtl.Columns["AuthorizeDateTime3"].Visible = false;
            }
        }
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] PageSession = null;
                oGM.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                oGM.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_Page, PageSession);
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserID = Session["UserID"].ToString();
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script> Height('650','650');</script>");

            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoadFunction", "<script>PageLoad();</script>");
                DateTime Date = oDBEngine.GetDate();
                DtFrom.Value = Date.AddDays((-1 * Date.Day) + 1);
                DtTo.Value = Date;
                DateFrom = Convert.ToDateTime(DtFrom.Date.ToString()).ToString("MM-dd-yyyy");
                DateTo = Convert.ToDateTime(DtTo.Date.ToString()).ToString("MM-dd-yyyy");

            }
            ShowType = "Show";
            if (HDNShowBy.Value.ToString() == "Excel")
            {
                DateFrom = Hdn_DateFrom.Value;
                DateTo = Hdn_DateTo.Value;
                if (DateFrom == "")
                {
                    DateFrom = Convert.ToDateTime(DtFrom.Date.ToString()).ToString("MM-dd-yyyy");
                }
                if (DateTo == "")
                {
                    DateTo = Convert.ToDateTime(DtTo.Date.ToString()).ToString("MM-dd-yyyy");
                }
                ShowType = "Excel";
            }
            GridVwColumnShowHide();
            if (hdn_GridBindOrNotBind.Value != "False")
            {
                PageSize = Hdn_PageSize.Value != "" ? Convert.ToInt32(Hdn_PageSize.Value) : 0;
                PageNumber = Hdn_PageNumber.Value != "" ? Convert.ToInt32(Hdn_PageNumber.Value) : 0;
                DateFrom = Hdn_DateFrom.Value;
                DateTo = Hdn_DateTo.Value;
                AuthorizeType = Hdn_AuthorizeType.Value;
                AuthorizeFor = Hdn_AuthorizeFor.Value;
                if (PageSize != 0 && PageNumber != 0)
                {
                    oAx.BindGrid(GvEmployeeDtl, fetchRecord(AuthorizeType, DateFrom, DateTo));
                }
            }

        }

        protected void GvEmployeeDtl_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GvEmployeeDtl.JSProperties["cpRefreshNavPanel"] = null;
            GvEmployeeDtl.JSProperties["cpAuthorizeSlctd"] = null;
            GvEmployeeDtl.JSProperties["cpAuthorizeAll"] = null;
            GvEmployeeDtl.JSProperties["cpUnAuthorizeSlctd"] = null;
            GvEmployeeDtl.JSProperties["cpUnAuthorizeAll"] = null;
            GvEmployeeDtl.JSProperties["cpExcelGenerated"] = null;
            GvEmployeeDtl.JSProperties["cpNoSelection"] = null;
            GvEmployeeDtl.JSProperties["cpExcelExport"] = null;

            string[] strSplit = e.Parameters.Split('~');
            string WhichCall = strSplit[0];
            int TotalItems = 0;
            int TotalPage = 0;
            PageSize = 10;

            string dtFrmGiven = strSplit[2];
            string[] strSplitNw1 = dtFrmGiven.Split('-');
            string year1 = strSplitNw1[2].Trim();
            string month1 = strSplitNw1[1].Trim();
            string date1 = strSplitNw1[0].Trim();
            string DateFrom = month1 + "-" + date1 + "-" + year1;

            string dtToGiven = strSplit[3];
            string[] strSplitNw2 = dtToGiven.Split('-');
            string year2 = strSplitNw2[2].Trim();
            string month2 = strSplitNw2[1].Trim();
            string date2 = strSplitNw2[0].Trim();
            string DateTo = month2 + "-" + date2 + "-" + year2;

            if (WhichCall == "GridBind")
            {

                PageNumber = 1;
                AuthorizeType = strSplit[1];
                AuthorizeFor = strSplit[4];

                DataSet Ds = new DataSet();
                Ds = fetchRecord(AuthorizeType, DateFrom, DateTo);
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(Ds.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        oAx.BindGrid(GvEmployeeDtl, Ds);
                        GvEmployeeDtl.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAx.BindGrid(GvEmployeeDtl);
                }
                else
                    oAx.BindGrid(GvEmployeeDtl);

                GvEmployeeDtl.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' +
                DateFrom + '~' + DateTo + '~' + AuthorizeType + '~' + AuthorizeFor;

            }
            if (WhichCall == "GridBindFilter")
            {
                PageNumber = 1;
                AuthorizeType = strSplit[1];
                AuthorizeFor = strSplit[4];

                if (strSplit[5] == "s")
                    GvEmployeeDtl.Settings.ShowFilterRow = true;
                if (strSplit[5] == "All")
                {
                    GvEmployeeDtl.FilterExpression = string.Empty;
                }


                DataSet Ds = new DataSet();
                Ds = fetchRecord(AuthorizeType, DateFrom, DateTo);
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(Ds.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        oAx.BindGrid(GvEmployeeDtl, Ds);
                        GvEmployeeDtl.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAx.BindGrid(GvEmployeeDtl);
                }
                else
                    oAx.BindGrid(GvEmployeeDtl);

                GvEmployeeDtl.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' +
                DateFrom + '~' + DateTo + '~' + AuthorizeType + '~' + AuthorizeFor;

            }
            if (WhichCall == "ExcelReport")
            {

                AuthorizeType = strSplit[1];
                AuthorizeFor = strSplit[4];
                GvEmployeeDtl.JSProperties["cpExcelExport"] = "T";

            }
            if (WhichCall == "AuthorizeGridBind")
            {
                AuthorizeFor = strSplit[4];
                DoAuthorisation = "Authorisation";
                EmpID = HDNCheckedEmpCode.Value.ToString();
                fetchRecord(AuthorizeType, DateFrom, DateTo);
                if (EmpID == "" || EmpID == null)
                {
                    GvEmployeeDtl.JSProperties["cpNoSelection"] = "T";
                }
                else
                {
                    GvEmployeeDtl.JSProperties["cpAuthorizeSlctd"] = "T";
                }
            }
            if (WhichCall == "AuthorizeAllGridBind")
            {
                AuthorizeFor = strSplit[4];
                DoAuthorisation = "AuthorisationAll";
                EmpID = HDNCheckedEmpCode.Value.ToString();
                fetchRecord(AuthorizeType, DateFrom, DateTo);
                GvEmployeeDtl.JSProperties["cpAuthorizeAll"] = "T";
            }
            if (WhichCall == "UnAuthorizeGridBind")
            {
                AuthorizeFor = strSplit[4];
                DoAuthorisation = "UnAuthorisation";
                EmpID = HDNCheckedEmpCode.Value.ToString();
                fetchRecord(AuthorizeType, DateFrom, DateTo);
                if (EmpID == "" || EmpID == null)
                {
                    GvEmployeeDtl.JSProperties["cpNoSelection"] = "T";
                }
                else
                {
                    GvEmployeeDtl.JSProperties["cpUnAuthorizeSlctd"] = "T";
                }
            }
            if (WhichCall == "UnAuthorizeAllGridBind")
            {
                AuthorizeFor = strSplit[4];
                DoAuthorisation = "UnAuthorisationAll";
                EmpID = HDNCheckedEmpCode.Value.ToString();
                fetchRecord(AuthorizeType, DateFrom, DateTo);
                GvEmployeeDtl.JSProperties["cpUnAuthorizeAll"] = "T";
            }
            if (WhichCall == "SearchByNavigation")
            {
                AuthorizeFor = strSplit[4];
                string strPageNum = String.Empty;
                string strNavDirection = String.Empty;
                int PageNumAfterNav = 0;
                strPageNum = strSplit[5];
                strNavDirection = strSplit[6];


                //Set Page Number
                if (strNavDirection == "RightNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) + 10;
                if (strNavDirection == "LeftNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) - 10;
                if (strNavDirection == "PageNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum);

                DataSet Ds = new DataSet();

                PageNumber = PageNumAfterNav;

                AuthorizeType = strSplit[1];


                Ds = fetchRecord(AuthorizeType, DateFrom, DateTo);

                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(Ds.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        oAx.BindGrid(GvEmployeeDtl, Ds);
                        GvEmployeeDtl.JSProperties["cpRefreshNavPanel"] = strNavDirection + '~' + strPageNum + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAx.BindGrid(GvEmployeeDtl);
                }
                else
                    oAx.BindGrid(GvEmployeeDtl);

                //Assign Value To HiddenField So That PageLoad Binding Can Use These HiddenField
                GvEmployeeDtl.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' +
                DateFrom + '~' + DateTo + '~' + AuthorizeType + '~' + AuthorizeFor;
            }

        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            AuthorizeFor = ComboAuthType.Value.ToString();
            AuthorizeType = ComboAuthorize.Value.ToString();
            DataSet DsExport = new DataSet();
            DsExport = fetchRecord(AuthorizeType, DateFrom, DateTo);

            string strHeader = String.Empty;
            string[] ReportHeader = new string[1];
            string strSavePath = String.Empty;

            if (AuthorizeType == "All")
            {
                strHeader = "Employee Authorization";
            }
            else if (AuthorizeType == "UnAuthorized")
            {
                strHeader = "UnAuthorized Employee";
            }
            else if (AuthorizeType == "Authorized")
            {
                strHeader = "Authorized Employee";
            }

            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            ShowType = "Show";

            if (DsExport.Tables.Count > 0)
                if (DsExport.Tables[0].Rows.Count > 0)
                {
                    ReportHeader[0] = strHeader;
                    string FileName = "EmployeeDetail_" + exlTime;
                    strSavePath = "~/Documents/";

                    //(SRLNO int,ContactID Varchar(100),Name Varchar(500),EmpCode Varchar(50),Department Varchar(200),BranchName Varchar(500),
                    // CTC Varchar(500),Designation Varchar(500),Company Varchar(200),EmpLog_AuthorizeUser1 int,EmpLog_AuthorizeDateTime1 varchar(100),
                    // EmpLog_AuthorizeUser2 int,EmpLog_AuthorizeDateTime2 varchar(100),EmpLog_AuthorizeUser3 int,EmpLog_AuthorizeDateTime3 varchar(100))

                    string[] ColumnType = { "I", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                    string[] ColumnSize = { "0", "100", "250", "50", "200", "250", "250", "250", "200", "200", "100", "200", "100", "200", "100" };
                    string[] ColumnWidthSize = { "5", "12", "30", "12", "22", "30", "12", "30", "30", "30", "20", "30", "20", "30", "20" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DsExport.Tables[0], Server.MapPath(strSavePath), "2007", FileName, ReportHeader, null);

                }
        }




    }
}