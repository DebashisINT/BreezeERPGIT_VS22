using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using EntityLayer.CommonELS;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Text;
using System.Data.Linq;
using System.Text.RegularExpressions;
using DataAccessLayer;
using System.Web.Services;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee : System.Web.UI.Page
    {
        /*-------------------Arindam------------------*/

        ExcelFile ex = new ExcelFile();
        FileInfo FIICXCSV = null;
        StreamReader strReader;
        StringBuilder strbuilder = new StringBuilder();
        String readline = string.Empty;
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        DataTable dt1 = new DataTable();
        string FilePath = "";
        DataSet dsdata = new DataSet();
        DataView dvUnmatched = new DataView();
        private static String path, path1, FileName, s, time, cannotParse;





        /*-------------------Arindam------------------*/

        /*-------------------Tanmoy------------------*/
        MasterSettings masterbl = new MasterSettings();
        public bool SrvBranchMap { get; set; }
        /*-------------------Tanmoy------------------*/

        #region Global Variable
        public string pageAccess;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        GlobalSettings globalsetting = new GlobalSettings();
        //GenericExcelExport oGenericExcelExport;
        //GenericMethod oGenericMethod;
        BusinessLogicLayer.GenericExcelExport oGenericExcelExport;
        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        string WhichCall;
        DataSet Ds_Global;
        AspxHelper oAspxHelper = new AspxHelper();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        #endregion
        //Session Used in This Page : PageSize,FromDOJ,ToDoj,PageNumAfterNav,SerachString,SearchBy,FindOption
        #region Page Properties
        public string P_PageSize
        {
            get { return (string)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }
        public string P_FromDOJ
        {
            get { return (string)Session["FromDOJ"]; }
            set { Session["FromDOJ"] = value; }
        }
        public string P_ToDOJ
        {
            get { return (string)Session["ToDOJ"]; }
            set { Session["ToDOJ"] = value; }
        }
        public string P_PageNumAfterNav
        {
            get { return (string)Session["PageNumAfterNav"]; }
            set { Session["PageNumAfterNav"] = value; }
        }
        public string P_SearchString
        {
            get { return (string)Session["SearchString"]; }
            set { Session["SearchString"] = value; }
        }
        public string P_SearchBy
        {
            get { return (string)Session["SearchBy"]; }
            set { Session["SearchBy"] = value; }
        }
        public string P_FindOption
        {
            get { return (string)Session["FindOption"]; }
            set { Session["FindOption"] = value; }
        }
        public string P_ShowFilter_SearchString
        {
            get { return (string)Session["ShowFilter_SearchString"]; }
            set { Session["ShowFilter_SearchString"] = value; }
        }


        #endregion
        #region User Define Methods
        DataSet Fetch_EmployeeData(string FromDOJ, string ToDOJ, string PageSize, string PageNumber,
            string SearchString, string SearchBy, string FindOption, string ExportType, string DevXFilterOn, string DevXFilterString)
        {
            string[] InputName = new string[10];
            string[] InputType = new string[10];
            string[] InputValue = new string[10];

            InputName[0] = "FromJoinDate";
            InputName[1] = "ToJoinDate";
            InputName[2] = "PageSize";
            InputName[3] = "PageNumber";
            InputName[4] = "SearchString";
            InputName[5] = "SearchBy";
            InputName[6] = "FindOption";
            InputName[7] = "ExportType";
            InputName[8] = "DevXFilterOn";
            InputName[9] = "DevXFilterString";

            InputType[0] = "D";
            InputType[1] = "D";
            InputType[2] = "I";
            InputType[3] = "I";
            InputType[4] = "V";
            InputType[5] = "C";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";

            InputValue[0] = FromDOJ;
            InputValue[1] = ToDOJ;
            InputValue[2] = PageSize;
            InputValue[3] = PageNumber;
            InputValue[4] = SearchString;
            InputValue[5] = SearchBy;
            InputValue[6] = FindOption;
            InputValue[7] = ExportType;
            InputValue[8] = DevXFilterOn;
            InputValue[9] = DevXFilterString;

            return BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("HR_Fetch_Employees", InputName, InputType, InputValue);
        }

        void ExportToExcel(DataSet DsExport, string FromDOJ, string ToDOJ, string SearchString, string SearchBy, string FindOption)
        {
            oGenericExcelExport = new BusinessLogicLayer.GenericExcelExport();
            DataTable DtExport = new DataTable();
            string strHeader = String.Empty;
            string[] ReportHeader = new string[1];
            string strSavePath = String.Empty;

            strHeader = "Employee Detail From " + Convert.ToDateTime(P_FromDOJ == "" ? "1900-01-01" : P_FromDOJ).ToString("dd-MMM-yyyy") + " To " +
                Convert.ToDateTime(P_ToDOJ == "" ? "9999-12-31" : P_ToDOJ).ToString("dd-MMM-yyyy");

            strHeader = strHeader + (SearchBy != "" ? (SearchBy == "EC" ? " (Search By : Employee Code " + (FindOption == "0" ? " Like '" : " = '") + SearchString :
                " (Search By : Employee Name " + (FindOption == "0" ? " Like '" : " = '") + SearchString) : "");

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

                    //SRLNO,Name,FatherName,DOJ,Department,BranchName,CTC,ReportTo,Designation,Company,
                    //Email_Ids,PhoneMobile_Numbers,PanCardNumber,CreatedBy
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                    string[] ColumnSize = { "10", "150", "150", "50", "50", "100", "100", "100", "100", "150", "150", "150", "150", "150" };
                    string[] ColumnWidthSize = { "5", "30", "30", "12", "22", "23", "15", "25", "23", "25", "25", "25", "20", "20" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, ReportHeader, null);
                }
        }
        #endregion

        #region Business Logic
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Code  Added and Commented By Priti on 16122016 to add Convert.ToString instead of ToString()
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                //string sPath = HttpContext.Current.Request.Url.ToString();
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);

                string[] PageSession = { "PageSize", "FromDOJ", "ToDoj", "PageNumAfterNav", "SerachString", "SearchBy", "FindOption", "ShowFilter_SearchString" };
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                oGenericMethod.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                oGenericMethod.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_Page, PageSession);

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/employee.aspx");
            /*-------------------Tanmoy------------------*/

            CommonBL config = new CommonBL();
           // string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
            string mastersettings = config.GetSystemSettingsResult("IsMultiBranchInEmployee");

            if (mastersettings == "0")
            {
                SrvBranchMap = false;
            }
            else
            {
                SrvBranchMap = true;
            }
            /*-------------------Tanmoy------------------*/

            if (HttpContext.Current.Session["userid"] == null)
            {
                ////Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }



            if (!IsPostBack)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "PageLD", "<script>Pageload();</script>");
                //Initialize Variable
                P_PageSize = "10";
                DtFrom.Date = oDBEngine.GetDate().AddDays(-30);
                DtTo.Date = oDBEngine.GetDate();
                //code Added By Priti on 21122016 to use Export Header,date
                Session["exportval"] = null;
                //....end...
            }
            BindGrid();
            //else { Page.ClientScript.RegisterStartupScript(GetType(), "PageLD", "<script>Pageload();</script>"); }
            //if (hdn_GridBindOrNotBind.Value != "False")
            //{
            //    if (P_FromDOJ != null && P_FromDOJ.Trim() != String.Empty && P_ToDOJ != null && P_ToDOJ.Trim() != String.Empty &&
            //        P_PageSize != null && P_PageNumAfterNav != null)
            //    {
            //        //When Show Filter Active With SearchString
            //        //Then Grid Bind With SearchString Cretaria
            //        //Otherwise normally as it bind
            //        if (P_ShowFilter_SearchString != null)
            //            if ( Convert.ToString(P_ShowFilter_SearchString) != String.Empty)
            //                oAspxHelper.BindGrid(GrdEmployee, Fetch_EmployeeData(String.IsNullOrEmpty(P_FromDOJ ) ? "1900-01-01" : P_FromDOJ, String.IsNullOrEmpty(P_ToDOJ) ? "9999-12-31" : P_ToDOJ, P_PageSize,
            //                   Convert.ToString(P_PageNumAfterNav), P_SearchString, P_SearchBy, P_FindOption, "S", "Y", P_ShowFilter_SearchString));
            //            else
            //                oAspxHelper.BindGrid(GrdEmployee, Fetch_EmployeeData(String.IsNullOrEmpty(P_FromDOJ) ? "1900-01-01" : P_FromDOJ, String.IsNullOrEmpty(P_ToDOJ) ? "9999-12-31" : P_ToDOJ, P_PageSize,
            //                Convert.ToString(P_PageNumAfterNav), P_SearchString, P_SearchBy, P_FindOption, "S", "N", String.Empty));
            //        else
            //            oAspxHelper.BindGrid(GrdEmployee, Fetch_EmployeeData(String.IsNullOrEmpty(P_FromDOJ ) ? "1900-01-01" : P_FromDOJ, String.IsNullOrEmpty(P_ToDOJ) ? "9999-12-31" : P_ToDOJ, P_PageSize,
            //            Convert.ToString(P_PageNumAfterNav), P_SearchString, P_SearchBy, P_FindOption, "S", "N", String.Empty));
            //    }
            //  }
            // 
        }

        protected void BindGrid()
        {

            Console.WriteLine("1");

            string strFromDOJ = String.Empty, strToDOJ = String.Empty, strSearchString = String.Empty,
                strSearchBy = String.Empty, strFindOption = String.Empty;

            Console.WriteLine("2");

            Ds_Global = new DataSet();


            Ds_Global = Fetch_EmployeeData(strFromDOJ == "" ? "1900-01-01" : strFromDOJ, strToDOJ == "" ? "9999-12-31" : strToDOJ, P_PageSize,
                "1", strSearchString, strSearchBy, strFindOption, "S", "N", String.Empty);

            Console.WriteLine("3");

            if (Ds_Global.Tables.Count > 0)
            {
                //Debjyoti 070217
                //Reason : Filter all employee to current employee 
                string CurrentComp = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

                // Code Commented And Modified By Sam due to Show All child 
                //company employee with parent company if we log in with Parent Company 
                //version 1.0.0.1
                //string[] cmpId = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_id", " cmp_internalid='" + CurrentComp + "'", 1);


                // DataRow[] extraRow= Ds_Global.Tables[0].Select("organizationid <>" + cmpId[0]);
                //foreach (DataRow dr in extraRow)
                //{
                //    Ds_Global.Tables[0].Rows.Remove(dr);
                //} 
                // GrdEmployee.DataSource = Ds_Global.Tables[0];
                // GrdEmployee.DataBind();

                //version 1.0.0.1 End
                Employee_BL objEmploye = new Employee_BL();
                string ListOfCompany = "";
                string[] cmpId = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_id", " cmp_internalid='" + CurrentComp + "'", 1);
                string Companyid = Convert.ToString(cmpId[0]);
                string Allcompany = "";
                string ChildCompanyid = objEmploye.getChildCompany(CurrentComp, ListOfCompany);
                if (ChildCompanyid != "")
                {
                    Allcompany = Companyid + "," + ChildCompanyid;
                    Allcompany = Allcompany.TrimEnd(',');
                }
                else
                {
                    Allcompany = Companyid;
                }
                DataRow[] extraRow = Ds_Global.Tables[0].Select("organizationid not in(" + Allcompany + ")");
                foreach (DataRow dr in extraRow)
                {
                    Ds_Global.Tables[0].Rows.Remove(dr);
                }
                GrdEmployee.DataSource = Ds_Global.Tables[0];
                GrdEmployee.DataBind();


            }

        }

        protected void GrdEmployee_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdEmployee.JSProperties["cpPagerSetting"] = null;
            GrdEmployee.JSProperties["cpExcelExport"] = null;
            GrdEmployee.JSProperties["cpRefreshNavPanel"] = null;
            GrdEmployee.JSProperties["cpCallOtherWhichCallCondition"] = null;

            // Code  Added  By Priti on 16122016 to use for delete employee
            GrdEmployee.JSProperties["cpDelete"] = null;
            WhichCall = e.Parameters.Split('~')[0];
            string strFromDOJ = String.Empty, strToDOJ = String.Empty, strSearchString = String.Empty,
            strSearchBy = String.Empty, strFindOption = String.Empty;
            string WhichType = null;

            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            }
            if (WhichCall == "Delete")
            {
                
                DataTable dtUser = new DataTable();
                dtUser = oDBEngine.GetDataTable("tbl_master_user", " top 1 * ", "user_contactId='" + WhichType + "'");

                #region Rajdip For Validation against payroll
                int flag = 0;
                DataTable dtifexistsinpayroll = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_checkemployeexistanceinpayroll");
                proc.AddNVarcharPara("@action", 100, "Existanceofanemployeeeinpayroll");
                proc.AddNVarcharPara("@cnt_internalId", 100, WhichType);
                dtifexistsinpayroll = proc.GetTable();
                if (dtifexistsinpayroll.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtifexistsinpayroll.Rows[0]["ExistinPayRoll"]) != 0)
                    {
                        GrdEmployee.JSProperties["cpDelete"] = "Existinpayroll";
                    }
                    else
                    {
                    /// Coded By Samrat Roy -- 18/04/2017 
                    /// To Delete Contact Type selection on Employee Type (DME/ISD)
                    Employee_BL objEmployee_BL = new Employee_BL();
                    objEmployee_BL.DeleteContactType(WhichType);

                    if (dtUser.Rows.Count > 0)
                    {


                        globalsetting.EmployeeDeleteBySelctName(Convert.ToString(WhichType), Convert.ToString(dtUser.Rows[0]["user_id"]));
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "height12", "<script>alert('Employee has been deleted.');</script>");
                        //GrdEmployee.JSProperties["cpDelete"] = "Success";
                        BindGrid();
                    }
                    else
                    {
                        globalsetting.EmployeeDeleteBySelctName(Convert.ToString(WhichType), DBNull.Value.ToString());
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "height15", "<script>alert('Employee has been deleted.');</script>");
                        // GrdEmployee.JSProperties["cpDelete"] = "Success";
                        BindGrid();

                    }
                  }
                }

                #endregion Rajdip
             
            }

            //.............end........................








            //common parameter
            //  strFromDOJ = oDBEngine.GetDate(BusinessLogicLayer.DBEngine.DateConvertFrom.UTCToOnlyDate, e.Parameters.Split('~')[1]);
            //  strToDOJ = oDBEngine.GetDate(BusinessLogicLayer.DBEngine.DateConvertFrom.UTCToOnlyDate, e.Parameters.Split('~')[2]);

            //if (Rb_SearchBy.SelectedItem.Value.ToString() != "N")
            //{
            //    if (Rb_SearchBy.SelectedItem.Value.ToString() == "EN")//Find By Emp Name
            //    {
            //        strSearchString = txtEmpName.Text;
            //        strSearchBy = "EN";
            //        strFindOption = cmbEmpNameFindOption.SelectedItem.Value.ToString();
            //    }
            //    else //Find By Emp Code
            //    {
            //        strSearchString = txtEmpCode.Text;
            //        strSearchBy = "EC";
            //        strFindOption = cmbEmpCodeFindOption.SelectedItem.Value.ToString();
            //    }
            //}
            int TotalItems = 0;
            int TotalPage = 0;
            //if (WhichCall == "Show")
            //{
            //Set Show filter's FilterExpression Empty and For Fresh Record Fetch
            GrdEmployee.FilterExpression = string.Empty;
            P_ShowFilter_SearchString = null;

            Ds_Global = new DataSet();
            Ds_Global = Fetch_EmployeeData(strFromDOJ == "" ? "1900-01-01" : strFromDOJ, strToDOJ == "" ? "9999-12-31" : strToDOJ, P_PageSize,
                "1", strSearchString, strSearchBy, strFindOption, "S", "N", String.Empty);
            if (Ds_Global.Tables.Count > 0)
            {
                // Count Employee Grid Data Start

                string CurrentComp = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

                Employee_BL objEmploye = new Employee_BL();
                string ListOfCompany = "";
                string[] cmpId = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_id", " cmp_internalid='" + CurrentComp + "'", 1);
                string Companyid = Convert.ToString(cmpId[0]);
                string Allcompany = "";
                string ChildCompanyid = objEmploye.getChildCompany(CurrentComp, ListOfCompany);
                if (ChildCompanyid != "")
                {
                    Allcompany = Companyid + "," + ChildCompanyid;
                    Allcompany = Allcompany.TrimEnd(',');
                }
                else
                {
                    Allcompany = Companyid;
                }
                DataRow[] extraRow = Ds_Global.Tables[0].Select("organizationid not in(" + Allcompany + ")");
                foreach (DataRow dr in extraRow)
                {
                    Ds_Global.Tables[0].Rows.Remove(dr);
                }

                // Count Employee Grid Data End

                if (Ds_Global.Tables[0].Rows.Count > 0)
                {
                    //Mantis Issue 24689
                    //TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
                    TotalItems = Convert.ToInt32(Ds_Global.Tables[1].Rows[0]["TotalRecord"].ToString());
                    //End of Mantis Issue 24689
                    TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                    GrdEmployee.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                    oAspxHelper.BindGrid(GrdEmployee, Ds_Global);
                }
                else
                    oAspxHelper.BindGrid(GrdEmployee);
            }
            else
                oAspxHelper.BindGrid(GrdEmployee);

            P_PageNumAfterNav = "1";
            //For All Date
            if (rbDOJ_Specific_All.SelectedItem.Value.ToString() == "A")
            {
                P_FromDOJ = String.Empty;
                P_ToDOJ = String.Empty;
            }
            //}
            //if (WhichCall == "SearchByNavigation")
            //{
            //    //strFromDOJ = oDBEngine.GetDate(BusinessLogicLayer.DBEngine.DateConvertFrom.UTCToOnlyDate, e.Parameters.Split('~')[1]);
            //    //strToDOJ = oDBEngine.GetDate(BusinessLogicLayer.DBEngine.DateConvertFrom.UTCToOnlyDate, e.Parameters.Split('~')[2]);
            //    string strPageNum = String.Empty;
            //    string strNavDirection = String.Empty;
            //    int PageNumAfterNav = 0;
            //    strPageNum = e.Parameters.Split('~')[3];
            //    strNavDirection = e.Parameters.Split('~')[4];

            //    //Set Page Number
            //    if (strNavDirection == "RightNav")
            //        PageNumAfterNav = Convert.ToInt32(strPageNum) + 10;
            //    if (strNavDirection == "LeftNav")
            //        PageNumAfterNav = Convert.ToInt32(strPageNum) - 10;
            //    if (strNavDirection == "PageNav")
            //        PageNumAfterNav = Convert.ToInt32(strPageNum);

            //    Ds_Global = new DataSet();

            //    //When Show Filter Active With SearchString
            //    //Then Grid Bind With SearchString Cretaria
            //    //other normally as it bind
            //    if (P_ShowFilter_SearchString != null)
            //        if (P_ShowFilter_SearchString.ToString() != String.Empty)
            //            Ds_Global = Fetch_EmployeeData(P_FromDOJ == "" ? "1900-01-01" : P_FromDOJ, P_ToDOJ == "" ? "9999-12-31" : P_ToDOJ, P_PageSize,
            //                        PageNumAfterNav.ToString(), "", "", "", "S", "Y", P_ShowFilter_SearchString.ToString());
            //        else
            //            Ds_Global = Fetch_EmployeeData(strFromDOJ == "" ? "1900-01-01" : strFromDOJ, strToDOJ == "" ? "9999-12-31" : strToDOJ, P_PageSize,
            //                        PageNumAfterNav.ToString(), strSearchString, strSearchBy, strFindOption, "S", "N", String.Empty);
            //    else
            //        Ds_Global = Fetch_EmployeeData(strFromDOJ == "" ? "1900-01-01" : strFromDOJ, strToDOJ == "" ? "9999-12-31" : strToDOJ, P_PageSize,
            //                    PageNumAfterNav.ToString(), strSearchString, strSearchBy, strFindOption, "S", "N", String.Empty);

            //    if (Ds_Global.Tables.Count > 0)
            //    {
            //        if (Ds_Global.Tables[0].Rows.Count > 0)
            //        {
            //            TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
            //            TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
            //            //GrdEmployee.JSProperties["cpPagerSetting"] = strPageNum + "~" + TotalPage + "~" + TotalItems;
            //            oAspxHelper.BindGrid(GrdEmployee, Ds_Global);
            //            GrdEmployee.JSProperties["cpRefreshNavPanel"] = strNavDirection + '~' + strPageNum + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
            //        }
            //        else
            //            oAspxHelper.BindGrid(GrdEmployee);
            //    }
            //    else
            //        oAspxHelper.BindGrid(GrdEmployee);

            //    P_PageNumAfterNav = PageNumAfterNav.ToString();
            //}
            if (WhichCall == "ExcelExport")
            {
                GrdEmployee.JSProperties["cpExcelExport"] = "T";
            }
            if (WhichCall == "ShowHideFilter")
            {
                if (e.Parameters.Split('~')[3] == "s")
                    GrdEmployee.Settings.ShowFilterRow = true;

                if (e.Parameters.Split('~')[3] == "All")
                {
                    GrdEmployee.FilterExpression = string.Empty;
                    P_ShowFilter_SearchString = null; //Close Search Cretaria When All Record Show filter On
                    GrdEmployee.JSProperties["cpCallOtherWhichCallCondition"] = "Show";//Reset On Starting Position
                }
            }

            //Assing Value in Properties(Contain Session) To Maintain Call Back Value To Be Used On Server Side Events
            P_FromDOJ = strFromDOJ;
            P_ToDOJ = strToDOJ;
            P_SearchString = strSearchString;
            P_FindOption = strFindOption;
            P_SearchBy = strSearchBy;

            //Dispose Object
            if (Ds_Global != null)
                Ds_Global.Dispose();

            // Page.ClientScript.RegisterStartupScript(GetType(), "PageLD", "<script>Pageload();</script>");

        }
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Ds_Global = new DataSet();
        //    string strSearchString = String.Empty,
        //       strSearchBy = String.Empty, strFindOption = String.Empty;
        //    if (Rb_SearchBy.SelectedItem.Value.ToString() != "N")
        //    {
        //        if (Rb_SearchBy.SelectedItem.Value.ToString() == "EN")
        //        {
        //            strSearchString = txtEmpName.Text;
        //            strSearchBy = "EN";
        //            strFindOption = cmbEmpNameFindOption.SelectedItem.Value.ToString();
        //        }
        //        else
        //        {
        //            strSearchString = txtEmpCode.Text;
        //            strSearchBy = "EC";
        //            strFindOption = cmbEmpCodeFindOption.SelectedItem.Value.ToString();
        //        }
        //    }

        //    if (P_ShowFilter_SearchString != null)
        //        if (P_ShowFilter_SearchString.ToString() != String.Empty)
        //            Ds_Global = Fetch_EmployeeData(P_FromDOJ == "" ? "1900-01-01" : P_FromDOJ, P_ToDOJ == "" ? "9999-12-31" : P_ToDOJ, "0",
        //                        "0", strSearchString, strSearchBy, strFindOption, "E", "Y", P_ShowFilter_SearchString.ToString());
        //        else
        //            Ds_Global = Fetch_EmployeeData(P_FromDOJ == "" ? "1900-01-01" : P_FromDOJ, P_ToDOJ == "" ? "9999-12-31" : P_ToDOJ, "0",
        //                        "0", strSearchString, strSearchBy, strFindOption, "E", "N", String.Empty);
        //    else
        //        Ds_Global = Fetch_EmployeeData(P_FromDOJ == "" ? "1900-01-01" : P_FromDOJ, P_ToDOJ == "" ? "9999-12-31" : P_ToDOJ, "0",
        //                    "0", strSearchString, strSearchBy, strFindOption, "E", "N", String.Empty);
        //    ExportToExcel(Ds_Global, P_FromDOJ, P_ToDOJ, strSearchString, strSearchBy, strFindOption);

        //    //Dispose Object
        //    if (Ds_Global != null)
        //        Ds_Global.Dispose();

        //}

        #endregion

        //protected void GrdEmployee_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
        //{
        //    if (P_ShowFilter_SearchString != null)
        //        if (P_ShowFilter_SearchString.ToString().Trim() != String.Empty)
        //            P_ShowFilter_SearchString = null;

        //    Ds_Global = new DataSet();
        //    int TotalItems = 0;
        //    int TotalPage = 0;
        //    //For All Date
        //    if (rbDOJ_Specific_All.SelectedItem.Value.ToString() == "A")
        //    {
        //        P_FromDOJ = String.Empty;
        //        P_ToDOJ = String.Empty;
        //    }
        //    if (P_ShowFilter_SearchString != null)
        //    {
        //        if (P_ShowFilter_SearchString.ToString().Trim() != String.Empty)
        //            P_ShowFilter_SearchString = P_ShowFilter_SearchString.ToString().Trim() + " And " + e.Criteria.ToString().Trim();
        //        else
        //            P_ShowFilter_SearchString = e.Criteria.ToString().Trim();
        //    }
        //    else
        //        P_ShowFilter_SearchString = e.Criteria.ToString().Trim();

        //    Ds_Global = Fetch_EmployeeData(P_FromDOJ == "" ? "1900-01-01" : P_FromDOJ, P_ToDOJ == "" ? "9999-12-31" : P_ToDOJ, P_PageSize,
        //           P_PageNumAfterNav, "", "", "", "S", "Y", P_ShowFilter_SearchString.ToString());

        //    if (Ds_Global.Tables.Count > 0)
        //    {
        //        if (Ds_Global.Tables[0].Rows.Count > 0)
        //        {
        //            TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
        //            TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
        //            oAspxHelper.BindGrid(GrdEmployee, Ds_Global);
        //            //Here I Passed ShowBtnClick Parameter Cause It ReInitialize Grid Like Show Button Functionality
        //            GrdEmployee.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick" + '~' + P_PageNumAfterNav + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
        //        }
        //        else
        //            oAspxHelper.BindGrid(GrdEmployee);
        //    }
        //    else
        //        oAspxHelper.BindGrid(GrdEmployee);

        //    //Dispose Object
        //    if (Ds_Global != null)
        //        Ds_Global.Dispose();


        //}
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 21122016 to use Export Header,date
            GrdEmployee.Columns[10].Visible = false;
            string filename = "Employees";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Employees";
            exporter.MaxColumnWidth = 70;
            // exporter.LeftMargin = 0;
            // exporter.Styles.Cell.Font.Size = 8;
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bindUserGroups();           
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //Code  Added and Commented By Priti on 21122016 to use Export Header,date
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
            //Page.Response.End();
        }
        /*------------------- Arindam--------------------------*/
        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {

            string strFileName = "Employee Master Data.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx"; 
            Response.AppendHeader("Content-Disposition", "attachment; filename=Employee Master Data.xlsx");
            Response.TransmitFile(strPath);
            Response.End();

        }

        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        public static int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;

        }


        public Boolean Import_To_Grid(string FilePath, string Extension, HttpPostedFile file)
        {
            Employee_BL objEmploye = new Employee_BL();
            Boolean Success = false;
            Boolean HasLog = false;
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();
                   
                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    {
                        
                        Sheet sheet = spreadSheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                        Worksheet worksheet = (spreadSheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                        foreach (Row row in rows)
                        {
                            if (row.RowIndex.Value == 1)
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    if (cell.CellValue != null)
                                    {
                                        dt.Columns.Add(GetValue(spreadSheetDocument, cell));
                                    }
                                }
                            }
                            else
                            {
                                DataRow tempRow = dt.NewRow();
                                int columnIndex = 0;
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    // Gets the column index of the cell with data
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--; //zero based index
                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            tempRow[columnIndex] = ""; //Insert blank data here;
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }
                                    try
                                    {
                                        tempRow[columnIndex] = GetValue(spreadSheetDocument, cell);
                                    } 
                                    catch
                                    {
                                        tempRow[columnIndex] = "";
                                    }

                                    columnIndex++;
                                }
                                dt.Rows.Add(tempRow);
                            }
                        }



                    }
                  
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string EmployeeCode = string.Empty;

                        foreach (DataRow row in dt.Rows)
                        {
                            loopcounter++;
                            try
                            {

                                EmployeeCode = Convert.ToString(row["Emp. Code*"]);
                                string Salutation = Convert.ToString(row["Salutation"]);
                                string FirstName = Convert.ToString(row["First Name*"]);
                                string MiddileName = Convert.ToString(row["Middle Name"]);
                                string LastName = Convert.ToString(row["Last Name"]);
                                string Dob = Convert.ToString(row["D.O.B"]);
                                string Gender = Convert.ToString(row["Gender"]);
                                string doj = Convert.ToString(row["Date Of Joining*"]);
                                string Grade = Convert.ToString(row["Grade"]);
                                string BloodGroup = Convert.ToString(row["Blood Group"]);
                                string MaritalStatus = Convert.ToString(row["Marital Status"]);
                                string Organization = Convert.ToString(row["Organization*"]);
                                string JobResposibility = Convert.ToString(row["Job Responsibility*"]);
                                string Branch = Convert.ToString(row["Branch*"]);
                                string Designation = Convert.ToString(row["Designation*"]);
                                string EmployeeType = Convert.ToString(row["Employee Type*"]);
                                string ReportTo = Convert.ToString(row["Report To*"]);
                                string AddressTypeResidence = Convert.ToString(row["Address Type_Res"]);
                                string Address1_Res = Convert.ToString(row["Address1_Res"]);
                                string Address2_Res = Convert.ToString(row["Address2_Res"]);
                                string Address3_Res = Convert.ToString(row["Address3_Res"]);
                                string Country_Res = Convert.ToString(row["Country:_Res"]);
                                string State_res = Convert.ToString(row["State:_Res"]);
                                string City_District_Res = Convert.ToString(row["City / District:_Res"]);
                                string Pin_Zip_Res = Convert.ToString(row["Pin / Zip:_Res"]);
                                string AddressType_off = Convert.ToString(row["Address Type:_Off"]);
                                string Address1_off = Convert.ToString(row["Address1_Off"]);
                                string Address2_off = Convert.ToString(row["Address2_Off"]);
                                string Address3_off = Convert.ToString(row["Address3_Off"]);
                                string Country_off = Convert.ToString(row["Country:_Off"]);
                                string State_off = Convert.ToString(row["State:_Off"]);

                                string City_District_Off = Convert.ToString(row["City / District:_Off"]);
                                string Pin_Zip_Off = Convert.ToString(row["Pin / Zip:_Off"]);
                                string Phone_type_res = Convert.ToString(row["Phone Type:_Res"]);
                                string Number_Res = Convert.ToString(row["Number:_Res"]);
                                string Phone_type_off = Convert.ToString(row["Phone Type:_Off"]);
                                string Number_Off = Convert.ToString(row["Number:_Off"]);
                                string Email_Type = Convert.ToString(row["Email Type:"]);
                                string Email_Id = Convert.ToString(row["Email Id"]);
                                string Relationship_1 = Convert.ToString(row["Relationship:_1"]);
                                string Name_1 = Convert.ToString(row["Name:_1"]);
                                string RelationShip_2 = Convert.ToString(row["Relationship:_2"]);
                                string Name_2 = Convert.ToString(row["Name:_2"]);
                                string Current_Ctc = Convert.ToString(row["Current CTC"]);
                                string Pan = Convert.ToString(row["PAN"]);
                                string Aadhar = Convert.ToString(row["Aadhaar"]);
                                string Passport = Convert.ToString(row["Passport"]);
                                string ValidUpTo = Convert.ToString(row["Valid Upto"]);
                                string BankName = Convert.ToString(row["Bank Name"]);
                                string AccountType = Convert.ToString(row["Account Type"]);
                                string Epic = Convert.ToString(row["EPIC"]);
                                string Account_No = Convert.ToString(row["Account No"]);
                                string Pf_Applicable = Convert.ToString(row["PF APPLICABLE"]);
                                string Pf_No = Convert.ToString(row["PF No.*"]);
                                string Uan = Convert.ToString(row["UAN *"]);
                                string Esi_Applicable = Convert.ToString(row["ESI Applicable"]);
                                string Esi_No = Convert.ToString(row["ESI No.*"]);
                                string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                                string ContactType = Convert.ToString(HttpContext.Current.Session["ContactType"]);

                               



                                DataSet dt2 = objEmploye.InsertEmployeeDataFromExcel(
                                  EmployeeCode, UserId, ContactType, Salutation, FirstName, MiddileName, LastName, Dob, Gender, doj, Grade, BloodGroup, MaritalStatus, Organization, JobResposibility, Branch, Designation, EmployeeType, ReportTo, AddressTypeResidence, Address1_Res, Address2_Res, Address3_Res, Country_Res, State_res, City_District_Res, Pin_Zip_Res, Phone_type_res, Number_Res, AddressType_off, Address1_off, Address2_off, Address3_off, Country_off, State_off, City_District_Off, Pin_Zip_Off, Phone_type_off, Number_Off, Email_Type, Email_Id, Relationship_1, Name_1, RelationShip_2, Name_2, Current_Ctc, Pan, Aadhar, Passport, ValidUpTo, Epic, BankName, Account_No, AccountType, Pf_Applicable, Pf_No, Uan, Esi_Applicable, Esi_No
                                       );


                                if (dt2 != null && dt2.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in dt2.Tables[0].Rows)
                                    {
                                        Success = Convert.ToBoolean(row2["Success"]);
                                        HasLog = Convert.ToBoolean(row2["HasLog"]);
                                    }
                                }

                                if(!HasLog)
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    int loginsert = objEmploye.InsertEmployeeImportLOg(EmployeeCode, loopcounter, FirstName, UserId, Session["FileName"].ToString(), description,"Failed");
                                }

                                else
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    int loginsert = objEmploye.InsertEmployeeImportLOg(EmployeeCode, loopcounter, FirstName, UserId, Session["FileName"].ToString(), description, "Success");
                                }



                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                               // string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                int loginsert = objEmploye.InsertEmployeeImportLOg(EmployeeCode, loopcounter, "", "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed");
                            }

                        }
                    }

                }
                else
                {
                    
                }
            }
            return HasLog;
        }

        private static int CellReferenceToIndex(Cell cell)
        {
            int index = 0;
            string reference = cell.CellReference.ToString().ToUpper();
            foreach (char ch in reference)
            {
                if (Char.IsLetter(ch))
                {
                    int value = (int)ch - (int)'A';
                    index = (index == 0) ? value : ((index + 1) * 26) + value;
                }
                else
                    return index;
            }
            return index;
        }
        protected void ReadFile(string txtFilePath)
        {
            String fileInfo;
            int number = 0;
            strReader = File.OpenText(txtFilePath);
            string StrGrand = null;
            try
            {
                using (StreamReader rwOpenTemplate = new StreamReader(txtFilePath))
                {
                    string a = StrGrand;

                    while ((fileInfo = rwOpenTemplate.ReadLine()) != null)
                    {
                        number = number + 1;
                        readline = strReader.ReadLine();
                        if (readline != "")
                        {
                            if (IsNumeric(readline.Substring(0, 1)))
                            {
                                strbuilder.AppendLine(readline);
                            }
                        }
                    }
                    rwOpenTemplate.Dispose();
                    rwOpenTemplate.Close();
                    strReader.Dispose();
                    strReader.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }

        protected void BtnSaveexcel_Click1(object sender, EventArgs e)
        {
            string fName = string.Empty;
            Boolean HasLog = false;
            if (OFDBankSelect.FileContent.Length != 0)
            {
                path = String.Empty;
                path1 = String.Empty;
                FileName = String.Empty;
                s = String.Empty;
                time = String.Empty;
                cannotParse = String.Empty;
                string strmodule = "InsertTradeData";


                BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                FilePath = Path.GetFullPath(OFDBankSelect.PostedFile.FileName);
                FileName = Path.GetFileName(FilePath);
                string fileExtension = Path.GetExtension(FileName);

                if (fileExtension.ToUpper()!= ".XLS" && fileExtension.ToUpper()!= ".XLSX")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Uploaded file format not supported by the system');</script>");
                    return;
                }


                if (fileExtension.Equals(".xlsx"))
                {
                    fName = FileName.Replace(".xlsx", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                }
                
                else if(fileExtension.Equals(".xls"))
                {
                    fName = FileName.Replace(".xls",DateTime.Now.ToString("ddMMyyyyhhmmss")+".xls");
                }

                else if(fileExtension.Equals(".csv"))
                {
                    fName = FileName.Replace(".csv", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv");
                }

                Session["FileName"] = fName;

                String UploadPath = Server.MapPath((Convert.ToString(ConfigurationManager.AppSettings["SaveCSV"]) + Session["FileName"].ToString()));
                OFDBankSelect.PostedFile.SaveAs(UploadPath);

                ClearArray();


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                try
                {
                    HttpPostedFile file = OFDBankSelect.PostedFile;
                    String extension = Path.GetExtension(FileName);
                    HasLog = Import_To_Grid(UploadPath, extension, file);
                }
                catch (Exception ex)
                {
                    HasLog = false;
                }


                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Import Process Successfully Completed!'); ShowLogData('" + HasLog + "');</script>");


                     
                
                

               
               


            }


            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Selected File Cannot Be Blank');</script>");
            }

        }

        public bool IsNumeric(string value)
        {
            try
            {
                Convert.ToDecimal(value.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        {
            Employee_BL objEmploye = new Employee_BL();
            string fileName = Convert.ToString(Session["FileName"]);
            DataSet dt2 = objEmploye.GetEmployeeLog(fileName);
            GvJvSearch.DataSource = dt2.Tables[0];

        }

        /*------------------- Arindam--------------------------*/

        /*-------------------Tanmoy------------------*/
        [WebMethod]
        public static List<BranchList> GetBranchList(string EMPID)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            List<BranchList> omodel = new List<BranchList>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_User_BranchMAP");
            proc.AddPara("@EMPID", EMPID);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                omodel = UtilityLayer.APIHelperMethods.ToModelList<BranchList>(dt);
            }
            return omodel;
        }

        [WebMethod]
        public static bool GetBranchListSubmit(string EMPID, List<string> Branchlist)
        {
            Employee_BL objEmploye = new Employee_BL();
            string BranchId = "";
            int i = 1;

            if (Branchlist != null && Branchlist.Count > 0)
            {
                foreach (string item in Branchlist)
                {
                    if (item == "0")
                    {
                        BranchId = "0";
                        break;
                    }
                    else
                    {
                        if (i > 1)
                            BranchId = BranchId + "," + item;
                        else
                            BranchId = item;
                        i++;
                    }
                }

            }

            DataTable dtfromtosumervisor = objEmploye.SubmitEmployeeBranch(EMPID, BranchId, Convert.ToString(HttpContext.Current.Session["userid"]));

            return true;
        }

        /*-------------------Tanmoy------------------*/
    }
    /*-------------------Tanmoy------------------*/
    public class BranchList
    {
        public long branch_id { get; set; }
        public String branch_description { get; set; }
        public bool IsChecked { get; set; }
        public string status { get; set; }
    }
    /*-------------------Tanmoy------------------*/
}


