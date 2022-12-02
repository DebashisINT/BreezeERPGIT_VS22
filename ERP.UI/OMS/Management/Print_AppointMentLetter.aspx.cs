using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
//using DevExpress.Web;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_Print_AppointMentLetter : System.Web.UI.Page
    {
        #region GlobalVariable
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oDbConverter = new BusinessLogicLayer.Converter();
        AspxHelper oAx = new AspxHelper();
        GenericXML GX = new GenericXML();
        BusinessLogicLayer.GenericMethod oGM = new BusinessLogicLayer.GenericMethod();
        GenericExcelExport oGenericExcelExport = new GenericExcelExport();
        #endregion

        #region Page Property
        int PageNumber; string DateFrom; string DateTo; string PrintType; int PageSize; string EmpID; string DoAuthorisation; string ShowType;
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
        public string P_PrintType
        {
            get { return PrintType; }
            set { PrintType = value; }
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

        #endregion

        #region PageClass
        DataSet fetchRecord(string PrintType, string FrmDt, string ToDt)
        {
            string[] sqlParameterName = new string[11];
            string[] sqlParameterValue = new string[11];
            string[] sqlParameterType = new string[11];

            if (HDNCheckedEmpCode.Value.ToString() == "" || HDNCheckedEmpCode.Value.ToString() == null)
            {
                sqlParameterName[0] = "WhichCall";
                sqlParameterValue[0] = "PrintLtr";
                sqlParameterType[0] = "V";

                sqlParameterName[1] = "AuthorizeType";
                sqlParameterValue[1] = PrintType;
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
                sqlParameterValue[9] = "A";
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
                sqlParameterValue[1] = ComboLtrPrint.Value.ToString();
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
                sqlParameterValue[9] = "A";
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
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserID = Session["UserID"].ToString();
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script> Height('650','650');</script>");

            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoadFunction", "<script>PageLoad();</script>");
                DateTime Date = oDBEngine.GetDate();
                DtFrom.Value = Date.AddDays((-1 * Date.Day) + 1);
                DtTo.Value = Date;
                DateFrom = Convert.ToDateTime(DtFrom.Date.ToString()).ToString("MM-dd-yyyy");
                DateTo = Convert.ToDateTime(DtTo.Date.ToString()).ToString("MM-dd-yyyy");
                appointmentdate.Date = oDBEngine.GetDate();

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
                PrintType = Hdn_AuthorizeType.Value;

                if (PageSize != 0 && PageNumber != 0)
                {
                    oAx.BindGrid(GvEmployeeDtl, fetchRecord(PrintType, DateFrom, DateTo));
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
            GvEmployeeDtl.JSProperties["cpbtnrptall"] = null;
            GvEmployeeDtl.JSProperties["cpbtnrptselected"] = null;

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
                PrintType = strSplit[1];

                DataSet Ds = new DataSet();
                Ds = fetchRecord(PrintType, DateFrom, DateTo);
                //Session["test"] = Ds;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(Ds.Tables[0].Rows[0]["TotalRecord"].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        oAx.BindGrid(GvEmployeeDtl, Ds);
                        GvEmployeeDtl.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                        /////  string where =

                        //DT = oDBEngine.GetDataTable("", " ", where);
                        string strQuery_Table = "tbl_master_contact contact,tbl_master_employee e,tbl_master_document";
                        string strQuery_FieldName = "top 10 (isnull(contact.cnt_firstName,'') +' '+isnull(contact.cnt_middleName,'')+' '+isnull(contact.cnt_lastName,'') +'['+isnull(contact.cnt_shortName,'')+']') as Name ,e.emp_contactid";
                        string strQuery_WhereClause = "cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_internalid not in (" + Ds.Tables[1].Rows[0][0].ToString().Trim() + ") and contact.cnt_firstName Like '%RequestLetter%' and e.emp_contactId=contact.cnt_internalid and doc_contactId=e.emp_contactid and doc_documentTypeId=(select top 1 dty_id from tbl_master_documentType where dty_documentType='Signature' and dty_applicableFor='Employee') ";

                        string strQuery_OrderBy = "";
                        string strQuery_GroupBy = "";
                        string CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;
                        CombinedQuery = CombinedQuery.Replace("'", "\\'");
                        GvEmployeeDtl.JSProperties["cpAjaxEmployeeSign"] = CombinedQuery.Replace("\\'", "'");


                    }
                    else
                        oAx.BindGrid(GvEmployeeDtl);
                }
                else
                    oAx.BindGrid(GvEmployeeDtl);

                GvEmployeeDtl.JSProperties["cpSetGlobalFields"] = PageSize.ToString() + '~' + PageNumber.ToString() + '~' +
                DateFrom + '~' + DateTo + '~' + PrintType;

            }
            if (WhichCall == "GridBindFilter")
            {
                PageNumber = 1;
                PrintType = strSplit[1];


                if (strSplit[4] == "s")
                    GvEmployeeDtl.Settings.ShowFilterRow = true;
                if (strSplit[4] == "All")
                {
                    GvEmployeeDtl.FilterExpression = string.Empty;
                }


                DataSet Ds = new DataSet();
                Ds = fetchRecord(PrintType, DateFrom, DateTo);
                Session["test"] = Ds;
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
                DateFrom + '~' + DateTo + '~' + PrintType;

            }
            if (WhichCall == "ExcelReport")
            {

                PrintType = strSplit[1];
                GvEmployeeDtl.JSProperties["cpExcelExport"] = "T";

            }

            //if (WhichCall == "AuthorizeGridBind")
            //{

            //    DoAuthorisation = "Authorisation";
            //    EmpID = HDNCheckedEmpCode.Value.ToString();
            //    fetchRecord(PrintType, DateFrom, DateTo);
            //    if (EmpID == "" || EmpID == null)
            //    {
            //        GvEmployeeDtl.JSProperties["cpNoSelection"] = "T";
            //    }
            //    else
            //    {
            //        GvEmployeeDtl.JSProperties["cpAuthorizeSlctd"] = "T";
            //    }
            //}
            if (WhichCall == "AuthorizeGridBind")
            {
                if (HDNCheckedEmpCode.Value.ToString() == "" || HDNCheckedEmpCode.Value.ToString() == null)
                {
                    GvEmployeeDtl.JSProperties["cpNoSelection"] = "T";
                }
                else
                {
                    PrintType = "PrintSelected";
                    GvEmployeeDtl.JSProperties["cpbtnrptselected"] = "rptselected";
                }
            }
            if (WhichCall == "AuthorizeAllGridBind")
            {
                PrintType = "PrintAll";
                GvEmployeeDtl.JSProperties["cpbtnrptall"] = "rptall";

            }


            //if (WhichCall == "UnAuthorizeGridBind")
            //{

            //    DoAuthorisation = "UnAuthorisation";
            //    EmpID = HDNCheckedEmpCode.Value.ToString();
            //    fetchRecord(PrintType, DateFrom, DateTo);
            //    if (EmpID == "" || EmpID == null)
            //    {
            //        GvEmployeeDtl.JSProperties["cpNoSelection"] = "T";
            //    }
            //    else
            //    {
            //        GvEmployeeDtl.JSProperties["cpUnAuthorizeSlctd"] = "T";
            //    }
            //}

            if (WhichCall == "SearchByNavigation")
            {

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

                PrintType = strSplit[1];


                Ds = fetchRecord(PrintType, DateFrom, DateTo);

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
                DateFrom + '~' + DateTo + '~' + PrintType;
            }

        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {

            PrintType = ComboLtrPrint.Value.ToString();
            DataSet DsExport = new DataSet();
            DsExport = fetchRecord(PrintType, DateFrom, DateTo);

            string strHeader = String.Empty;
            string[] ReportHeader = new string[1];
            string strSavePath = String.Empty;

            if (PrintType == "All")
            {
                strHeader = "Print Appointment Letter";
            }
            else if (PrintType == "ToBePrinted")
            {
                strHeader = "Appointment Letter To Be Printed";
            }
            else if (PrintType == "AlrdyPrinted")
            {
                strHeader = "Appointment Letter Already Printed";
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

                    string[] ColumnType = { "I", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                    string[] ColumnSize = { "0", "100", "250", "50", "200", "250", "250", "250", "200", "200", "100", "200", "100", "200", "100", "100", "100" };
                    string[] ColumnWidthSize = { "5", "12", "30", "12", "22", "30", "12", "30", "30", "30", "20", "30", "20", "30", "20", "30", "20" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DsExport.Tables[0], Server.MapPath(strSavePath), "2007", FileName, ReportHeader, null);

                }
        }
        protected void btnrptall_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            PrintType = ComboLtrPrint.Value.ToString();
            DateFrom = Hdn_DateFrom.Value;
            DateTo = Hdn_DateTo.Value;
            EmpID = "";
            DoAuthorisation = "PrintAll";
            dsCrystal = fetchRecord("PrintAll", DateFrom, DateTo);
            if (dsCrystal.Tables.Count > 0)
            {
                if (dsCrystal.Tables[0].Rows.Count > 0)
                {
                    string employeeId = txtSignature_hidden.Text.ToString();
                    string empname = "";
                    string empdesignation = "";
                    byte[] SignatureinByte;
                    byte[] logoinByte;
                    dsCrystal.Tables[0].Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
                    dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                    //dsCrystal.Tables[0].Columns.Add("cmpid", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("cmpnamestatus", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("cmpnameaddress", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("imagestatus", System.Type.GetType("System.String"));
                    //int p = 1;

                    //for (int g = 0; g < dsCrystal.Tables[0].Rows.Count; g++)
                    //{  
                    //    dsCrystal.Tables[0].Rows[g]["cmpid"] = p;
                    //    p++;
                    //}
                    if (Chkcmpname.Checked == true)
                    {
                        for (int s = 0; s < dsCrystal.Tables[0].Rows.Count; s++)
                        {

                            dsCrystal.Tables[0].Rows[s]["cmpnamestatus"] = "1";

                        }
                    }
                    if (Chkcmpaddress.Checked == true)
                    {
                        for (int c = 0; c < dsCrystal.Tables[0].Rows.Count; c++)
                        {

                            dsCrystal.Tables[0].Rows[c]["cmpnameaddress"] = "1";

                        }
                    }
                    if (ChkLogo.Checked == true)
                    {
                        for (int m = 0; m < dsCrystal.Tables[0].Rows.Count; m++)
                        {
                            dsCrystal.Tables[0].Rows[m]["imagestatus"] = "1";
                            if (oDbConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\" + dsCrystal.Tables[0].Rows[m]["Cmp_ID"].ToString() + ".bmp"), out logoinByte) != 2)
                            {

                                dsCrystal.Tables[0].Rows[m]["Image"] = logoinByte;
                            }
                        }
                    }
                    if (chkSignature.Checked == true)
                    {
                        DataTable dtdesignation = oDBEngine.GetDataTable("select ltrim(rtrim(deg_designation)) from tbl_master_designation where deg_id in (select emp_designation from tbl_trans_employeeCTC where emp_cntId='" + employeeId.ToString().Trim() + "')");
                        empdesignation = dtdesignation.Rows[0][0].ToString();
                        DataTable dtempname = oDBEngine.GetDataTable("select ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,''))) from tbl_master_contact where cnt_internalId ='" + employeeId.ToString().Trim() + "'");
                        empname = dtempname.Rows[0][0].ToString();
                        if (oDbConverter.getSignatureImage(employeeId, out SignatureinByte, "") == 1)
                        {
                            for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                            {
                                dsCrystal.Tables[0].Rows[i]["Signature"] = SignatureinByte;

                            }
                        }
                    }
                    dsCrystal.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//Appointmentletter.xsd");
                    //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');MULTI
                    string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');
                    ReportDocument reportObj = new ReportDocument();
                    string ReportPath = Server.MapPath("..\\management\\AppointMentLetter.rpt");
                    reportObj.Load(ReportPath);
                    reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    reportObj.SetDataSource(dsCrystal.Tables[0]);

                    reportObj.SetParameterValue("@SettDate", Convert.ToDateTime(appointmentdate.Value).ToString("dd-MMM-yyyy"));
                    reportObj.SetParameterValue("@empname", empname.ToString().Trim());
                    reportObj.SetParameterValue("@empdeg", empdesignation.ToString().Trim());

                    reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AppointMentLetter");

                    reportObj.Dispose();
                    GC.Collect();
                }
            }
        }

        protected void btnrptselectted_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            PrintType = ComboLtrPrint.Value.ToString();
            DateFrom = Hdn_DateFrom.Value;
            DateTo = Hdn_DateTo.Value;
            EmpID = HDNCheckedEmpCode.Value;
            DoAuthorisation = "PrintSelected";

            dsCrystal = fetchRecord("PrintSelected", DateFrom, DateTo);
            if (dsCrystal.Tables.Count > 0)
            {
                if (dsCrystal.Tables[0].Rows.Count > 0)
                {
                    string employeeId = txtSignature_hidden.Text.ToString();
                    string empname = "";
                    string empdesignation = "";
                    byte[] SignatureinByte;
                    byte[] logoinByte;
                    dsCrystal.Tables[0].Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
                    dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                    //dsCrystal.Tables[0].Columns.Add("cmpid", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("cmpnamestatus", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("cmpnameaddress", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("imagestatus", System.Type.GetType("System.String"));
                    //int p = 1;
                    //for (int g = 0; g < dsCrystal.Tables[0].Rows.Count; g++)
                    //{
                    //    //int k = p;   
                    //    dsCrystal.Tables[0].Rows[g]["cmpid"] = p;
                    //    p++;
                    //}
                    if (ChkLogo.Checked == true)
                    {
                        for (int m = 0; m < dsCrystal.Tables[0].Rows.Count; m++)
                        {
                            dsCrystal.Tables[0].Rows[m]["imagestatus"] = "1";
                            if (oDbConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\" + dsCrystal.Tables[0].Rows[m]["Cmp_ID"].ToString() + ".bmp"), out logoinByte) != 2)
                            {

                                dsCrystal.Tables[0].Rows[m]["Image"] = logoinByte;
                            }
                        }
                    }
                    if (Chkcmpname.Checked == true)
                    {
                        for (int s = 0; s < dsCrystal.Tables[0].Rows.Count; s++)
                        {

                            dsCrystal.Tables[0].Rows[s]["cmpnamestatus"] = "1";

                        }
                    }
                    if (Chkcmpaddress.Checked == true)
                    {
                        for (int c = 0; c < dsCrystal.Tables[0].Rows.Count; c++)
                        {

                            dsCrystal.Tables[0].Rows[c]["cmpnameaddress"] = "1";

                        }
                    }
                    if (chkSignature.Checked == true && txtSignature_hidden.Text.Trim() != String.Empty)
                    {
                        DataTable dtdesignation = oDBEngine.GetDataTable("select ltrim(rtrim(deg_designation)) from tbl_master_designation where deg_id in (select emp_designation from tbl_trans_employeeCTC where emp_cntId='" + employeeId.ToString().Trim() + "')");
                        empdesignation = dtdesignation.Rows[0][0].ToString();
                        DataTable dtempname = oDBEngine.GetDataTable("select ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,''))) from tbl_master_contact where cnt_internalId ='" + employeeId.ToString().Trim() + "'");
                        empname = dtempname.Rows[0][0].ToString();
                        if (oDbConverter.getSignatureImage(employeeId, out SignatureinByte, "") == 1)
                        {
                            for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                            {
                                dsCrystal.Tables[0].Rows[i]["Signature"] = SignatureinByte;

                            }
                        }
                    }
                    dsCrystal.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//Appointmentletter.xsd");
                    //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');MULTI
                    string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');
                    ReportDocument reportObj = new ReportDocument();
                    string ReportPath = Server.MapPath("..\\management\\AppointMentLetter.rpt");
                    reportObj.Load(ReportPath);
                    reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    reportObj.SetDataSource(dsCrystal.Tables[0]);

                    reportObj.SetParameterValue("@SettDate", Convert.ToDateTime(appointmentdate.Value).ToString("dd-MMM-yyyy"));
                    reportObj.SetParameterValue("@empname", empname.ToString().Trim());
                    reportObj.SetParameterValue("@empdeg", empdesignation.ToString().Trim());
                    reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AppointMentLetter");

                    reportObj.Dispose();
                    GC.Collect();
                }
            }

            ///////////////
        }
    }
}