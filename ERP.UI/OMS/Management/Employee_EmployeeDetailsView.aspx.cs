using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_Employee_EmployeeDetailsView : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {

        #region Global Variable
        string data;
        int PageSize;
        int PageNumber;
        int TotalItems;
        int TotalPage;
        Converter oconverter;
        DBEngine oDBEngine;
        BusinessLogicLayer.GenericMethod oGenericMethod;
        #endregion

        #region Page Class
        void BindGrid(ASPxGridView Grid, DataSet Ds)
        {
            if (Ds.Tables[0].Rows.Count > 0)
            {
                Grid.DataSource = Ds.Tables[0];
                Grid.DataBind();
            }
            else
            {
                Grid.DataSource = null;
                Grid.DataBind();
            }
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

        protected DataSet BindGridSearchParam(string viewType)
        {
            string EmployeeStatus = "";
            string CompanyVal = "";
            string BranchVal = "";
            string EmployeeVal = "";
            string ReportToVal = "";
            string TypeVal = "";
            string DateRangeSelection = "";
            string fromDate = "01/01/1900";
            string toDate = "12/31/9999";
            string EmployeeSelection = "";
            if (RadEmployeeS.Checked == true)
            {
                EmployeeSelection = "Y";
                EmployeeVal = HDNEmployee.Value;
            }
            if (RadAll.Checked == true)
            {
                EmployeeStatus = "B";
            }
            else if (RadActive.Checked == true)
            {
                EmployeeStatus = "C";
            }
            else if (RadClosed.Checked == true)
            {
                EmployeeStatus = "A";
            }
            if (RadDateRangeS.Checked == true)
            {
                DateRangeSelection = "Y";
            }
            if (DateRangeSelection == "Y")
            {
                fromDate = Convert.ToString(dtFrom.Text);
                toDate = Convert.ToString(dtTo.Text);
                fromDate = fromDate.Split('-')[1] + "/" + fromDate.Split('-')[0] + "/" + fromDate.Split('-')[2];
                toDate = toDate.Split('-')[1] + "/" + toDate.Split('-')[0] + "/" + toDate.Split('-')[2];
            }
            if (RadCompanyS.Checked == true)
                CompanyVal = HDNCompany.Value;
            else
                CompanyVal = HttpContext.Current.Session["userCompanyHierarchy"].ToString();
            if (RadBranchS.Checked == true)
                BranchVal = HDNBranch.Value;
            else
                BranchVal = HttpContext.Current.Session["userbranchHierarchy"].ToString();

            if (RadReportToS.Checked == true)
                ReportToVal = HDNReportTo.Value;
            if (TadTypeS.Checked == true)
                TypeVal = HDNType.Value;

            string bindParams = "Show~" + EmployeeSelection + '@' + EmployeeVal + '@' + EmployeeStatus + '@' + DateRangeSelection + '@' + fromDate + '@' + toDate + '@' + CompanyVal + '@' + BranchVal + '@' + ReportToVal + '@' + TypeVal;
            return Procedure(bindParams, viewType);
        }

        protected DataSet Procedure(string bindParameters, string viewType)
        {
            DataSet ds = new DataSet();

            string[] splitCondition = bindParameters.Split('~');
            if (splitCondition[0] == "Show")
            {
                string[] parameters = (splitCondition[1].ToString()).Split('@');

                string[] InputName = new string[13];
                string[] InputType = new string[13];
                string[] InputValue = new string[13];

                InputName[0] = "EmployeeSelection";
                InputName[1] = "Employee";
                InputName[2] = "EmployeeStatus";
                InputName[3] = "DateRangeSelection";
                InputName[4] = "FromDate";
                InputName[5] = "ToDate";
                InputName[6] = "Company";
                InputName[7] = "Branch";
                InputName[8] = "ReportTo";
                InputName[9] = "Type";
                InputName[10] = "ViewType";
                InputName[11] = "PageSize";
                InputName[12] = "PageNumber";

                InputType[0] = "C";
                InputType[1] = "V";
                InputType[2] = "V";
                InputType[3] = "C";
                InputType[4] = "V";
                InputType[5] = "V";
                InputType[6] = "V";
                InputType[7] = "V";
                InputType[8] = "V";
                InputType[9] = "V";
                InputType[10] = "V";
                InputType[11] = "I";
                InputType[12] = "I";

                InputValue[0] = parameters[0];
                InputValue[1] = parameters[1];
                InputValue[2] = parameters[2];
                InputValue[3] = parameters[3];
                InputValue[4] = parameters[4];
                InputValue[5] = parameters[5];
                InputValue[6] = parameters[6];
                InputValue[7] = parameters[7];
                InputValue[8] = parameters[8];
                InputValue[9] = parameters[9];
                InputValue[10] = viewType;
                InputValue[11] = PageSize.ToString();
                InputValue[12] = PageNumber.ToString();

                ds = SQLProcedures.SelectProcedureArrDS("HR_Report_EmployeeDetail", InputName, InputType, InputValue);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        ds = null;
                }
            }
            return ds;
        }
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                //oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                oDBEngine = new DBEngine();
                oDBEngine.Call_CheckPageaccessebility(sPath);

                string[] PageSession = { "PageNum" };
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                oGenericMethod.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                //Session["GridBindBy"] = null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //if ((Session["GridBindBy"] != null) && (Session["GridBindBy"].ToString() != "GridCustomCallBackFirst"))
            //{
            //    Session["GridBindBy"] = null;
            //    Session["GridBindBy"] = "OnPageLoad";
            //}

            if (!IsPostBack)
            {
                oconverter = new Converter();
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtFrom.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.Value = oDBEngine.GetDate();
                //if (Session["GridBindBy"] != null) Session["GridBindBy"] = null;
                //Session["GridBindBy"] = "GridCustomCallBackFirst";
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "Hiight", "<script>height();</script>");
            //if (IsCallback && (Session["GridBindBy"].ToString() == "OnPageLoad"))
            if (IsCallback)
                GridBindOnPageLoad();

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
        }

        protected void GridBindOnPageLoad()
        {
            if (Session["PageNum"] != null)
            {
                PageNumber = Convert.ToInt32(Session["PageNum"].ToString());
            }
            else
            {
                PageNumber = 1;
            }
            PageSize = 10;
            DataSet dsonPageload = BindGridSearchParam("Screen");
            if (dsonPageload != null)
            {
                if (dsonPageload.Tables.Count > 0)
                {
                    if (dsonPageload.Tables[0].Rows.Count > 0)
                    {
                        TotalItems = Convert.ToInt32(dsonPageload.Tables[1].Rows[0][0].ToString());
                        TotalPage = TotalItems % PageSize == 0 ? (TotalItems / PageSize) : (TotalItems / PageSize) + 1;
                        EmployeeGrid.JSProperties["cpIsEmptyDsSearch"] = "No~" + PageNumber + "~" + TotalPage + "~" + TotalItems;
                        BindGrid(EmployeeGrid, dsonPageload);
                    }
                }
            }
        }

        #region Grid Related  Events
        protected void EmployeeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataSet DsSearch;
            EmployeeGrid.JSProperties["cpPagerSetting"] = null;
            EmployeeGrid.JSProperties["cpRefreshNavPanel"] = null;
            EmployeeGrid.JSProperties["cpIsEmptyDsSearch"] = null;
            //if (Session["GridBindBy"] != null) Session["GridBindBy"] = null;
            //Session["GridBindBy"] = "GridCustomCallBack";
            string command = e.Parameters.Split('~')[0];
            if (command == "Filter")
            {
                if (e.Parameters.Split('~')[1] == "s")
                {
                    EmployeeGrid.Settings.ShowFilterRow = true;
                }
                else if (e.Parameters.Split('~')[1] == "All")
                {
                    EmployeeGrid.FilterExpression = string.Empty;
                }
                else
                {
                    EmployeeGrid.ClearSort();
                }
            }
            if (command == "Show")
            {
                PageNumber = 1;
                if (Session["PageNum"] != null) Session["PageNum"] = null;
                Session["PageNum"] = Convert.ToString(PageNumber);

                PageSize = Convert.ToInt32(EmployeeGrid.SettingsPager.PageSize.ToString());
                DsSearch = new DataSet();
                DsSearch = BindGridSearchParam("Screen");
                if (DsSearch != null)
                {
                    if (DsSearch.Tables.Count > 0)
                    {
                        BindGrid(EmployeeGrid, DsSearch);
                        if (DsSearch.Tables[1].Rows.Count > 0)
                        {
                            TotalItems = Convert.ToInt32(DsSearch.Tables[1].Rows[0][0].ToString());
                            TotalPage = TotalItems % PageSize == 0 ? (TotalItems / PageSize) : (TotalItems / PageSize) + 1;
                            EmployeeGrid.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                        }
                    }
                    else
                    {
                        EmployeeGrid.DataSource = null;
                        EmployeeGrid.DataBind();
                        EmployeeGrid.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                    }
                }
                else
                {
                    EmployeeGrid.DataSource = null;
                    EmployeeGrid.DataBind();
                    EmployeeGrid.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                }
                if (DsSearch != null)
                    DsSearch.Dispose();
            }
            if (command == "SearchByNavigation")
            {
                string strPageNum = String.Empty;
                string strNavDirection = String.Empty;
                int PageNumAfterNav = 0;
                strPageNum = e.Parameters.Split('~')[1];
                strNavDirection = e.Parameters.Split('~')[2];

                //Set Page Number
                if (strNavDirection == "RightNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) + 10;
                if (strNavDirection == "LeftNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) - 10;
                if (strNavDirection == "PageNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum);


                if (Session["PageNum"] != null) Session["PageNum"] = null;
                Session["PageNum"] = Convert.ToString(PageNumAfterNav);
                PageNumber = PageNumAfterNav;
                PageSize = Convert.ToInt32(EmployeeGrid.SettingsPager.PageSize.ToString());
                DsSearch = new DataSet();
                DsSearch = BindGridSearchParam("Screen");
                if (DsSearch != null)
                {
                    if (DsSearch.Tables.Count > 0)
                    {
                        BindGrid(EmployeeGrid, DsSearch);
                        if (DsSearch.Tables[1].Rows.Count > 0)
                        {
                            TotalItems = Convert.ToInt32(DsSearch.Tables[1].Rows[0][0].ToString());
                            TotalPage = TotalItems % PageSize == 0 ? (TotalItems / PageSize) : (TotalItems / PageSize) + 1;
                        }
                    }
                    EmployeeGrid.JSProperties["cpRefreshNavPanel"] = strNavDirection + '~' + strPageNum + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
                }
                else
                {
                    EmployeeGrid.DataSource = null;
                    EmployeeGrid.DataBind();
                    EmployeeGrid.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                }
                if (DsSearch != null)
                    DsSearch.Dispose();
            }
        }
        #endregion

        #region Export Related Events
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (hdnExportValue.Value == "E")
            {
                DataSet ds = BindGridSearchParam("Excel");
                ExportToExcel_Generic(ds.Tables[0], "2007");
            }
        }

        protected void ExportToExcel_Generic(DataTable Dt, string ExcelVersion)
        {
            if (Dt.Rows.Count > 0)
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string strReportHeader = "Employee Details";
                if (RadDateRangeS.Checked == true)
                {
                    oconverter = new Converter();
                    strReportHeader = strReportHeader + " For the Date Between " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  to " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + ".";
                }
                //oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                oDBEngine = new DBEngine();
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "EmployeeDetail_" + exlTime;
                strDownloadFileName = "~/ExportFiles/";
                string[] strHead = new string[2];
                strHead[0] = exlDateTime;
                strHead[1] = strReportHeader;

                string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "N", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                string[] ColumnSize = { "150", "30", "100", "50", "15", "15", "50", "50", "50", "50", "28,2", "30", "50", "30", "150", "30", "15", "50", "50", "30", "30", "30" };
                string[] ColumnWidthSize = { "24", "14", "22", "15", "10", "10", "14", "20", "8", "27", "12", "12", "25", "11", "20", "22", "10", "15", "20", "15", "15", "18" };
                oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelNORECORD", "NORECORD(Excel);", true);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //string AccountStatus = "";
            //DateTime FromDate;
            //DateTime ToDate;
            //string AccType = "";
            //string AccSubType = "";
            //string BenCategory = "";
            //string BenOCP = "";
            //string ShowNHolder = "";
            //string ShowNPOA = "";
            //string ShowAcMinor = "";
            //string ShowAcNNom = "";
            //string ShowAcMNom = "";
            //string DateRangeSelection = "";
            //string SelectedClient = "";
            //if (RadClientSelected.Checked == true)
            //{
            //    SelectedClient = HDNClient.Value;
            //}
            //if (RadDateRangeS.Checked == true)
            //{
            //    DateRangeSelection = "Y";
            //}
            //if (RadAll.Checked == true)
            //{
            //    AccountStatus = "A";
            //}
            //else if (RadActive.Checked == true)
            //{
            //    AccountStatus = "B";
            //}
            //else if (RadClosed.Checked == true)
            //{
            //    AccountStatus = "C";
            //}

            //if (rdbMainSelected.Checked == true)
            //    AccType = HDNType.Value;
            //if (rdSelSegment.Checked == true)
            //    AccSubType = HDNSubType.Value;
            //if (ddlCat.SelectedItem.Value != "0")
            //    BenCategory = ddlCat.SelectedItem.Value;
            //if (ddlOcp.SelectedItem.Value != "0")
            //    BenOCP = ddlOcp.SelectedItem.Value;
            //if (ChkAHolder.Checked == true)
            //    ShowNHolder = "Y";
            //if (ChkPOA.Checked == true)
            //    ShowNPOA = "Y";
            //if (chkMinor.Checked == true)
            //    ShowAcMinor = "Y";
            //if (chkNom.Checked == true)
            //    ShowAcNNom = "Y";
            //if (chkMinorNom.Checked == true)
            //    ShowAcMNom = "Y";

            //DataSet ds = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter("Fetch_NSDLClientMaster", con))
            //    {
            //        da.SelectCommand.Parameters.AddWithValue("@SelectedClient", SelectedClient);
            //        da.SelectCommand.Parameters.AddWithValue("@DateRangeSelection", DateRangeSelection);
            //        da.SelectCommand.Parameters.AddWithValue("@AccountStatus", AccountStatus);
            //        da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
            //        da.SelectCommand.Parameters.AddWithValue("@ToDate", dtTo.Value);
            //        da.SelectCommand.Parameters.AddWithValue("@AccType", AccType);
            //        da.SelectCommand.Parameters.AddWithValue("@AccSubType", AccSubType);
            //        da.SelectCommand.Parameters.AddWithValue("@BenCategory", BenCategory);
            //        da.SelectCommand.Parameters.AddWithValue("@BenOcp", BenOCP);
            //        da.SelectCommand.Parameters.AddWithValue("@ShowNHolder", ShowNHolder);
            //        da.SelectCommand.Parameters.AddWithValue("@ShowNPOA", ShowNPOA);
            //        da.SelectCommand.Parameters.AddWithValue("@ShowAcMinor", ShowAcMinor);
            //        da.SelectCommand.Parameters.AddWithValue("@ShowAcNNom", ShowAcNNom);
            //        da.SelectCommand.Parameters.AddWithValue("@ShowAcMNom", ShowAcMNom);
            //        da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //        da.SelectCommand.CommandTimeout = 0;

            //        if (con.State == ConnectionState.Closed)
            //            con.Open();
            //        ds.Reset();
            //        da.Fill(ds);
            //        ViewState["dataset"] = ds;

            //    }
            //}
            ////   DataSet ds = new DataSet();
            //oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company", " cmp_name,(Select top 1 phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId=cmp_internalid) as cmpphno,(select top 1(isnull(add_address1,'')+' '+ isnull(add_address2,'')+' '+isnull(add_address3,'')+','+isnull(city_name,'')+'-'+  isnull(add_pin,'')) from tbl_master_address,tbl_master_city where add_city=city_id and add_cntID=cmp_internalid AND add_entity='Company' AND add_addressType='Office')as cmpaddress,(select top 1 eml_email from tbl_master_email   where eml_cntid=cmp_internalid) as Email  ", " cmp_internalid in('" + Session["LastCompany"].ToString() + "') ");
            //// ds = (DataSet)ViewState["dataset"];
            //byte[] logoinByte;
            //ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            //decimal mTotAmt = 0;
            //if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
            //{
            //    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            //}
            //else
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        ds.Tables[0].Rows[i]["Image"] = logoinByte;

            //    }
            //}
            //DataSet dn = new DataSet();

            //ReportDocument report = new ReportDocument();
            //// ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\NSDLClientMaster.xsd");
            ////ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\ClientMasterMainDetail.xsd");

            //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //string tmpPdfPath = string.Empty;
            //tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\NSDLClientMaster.rpt");
            //report.Load(tmpPdfPath);
            //report.SetDataSource(ds.Tables[0]);
            //report.VerifyDatabase();
            //if (dtComp.Rows.Count > 0)
            //{
            //    if (dtComp.Rows[0]["cmp_name"].ToString() != "")
            //    {
            //        report.SetParameterValue("@CompanyName", (string)dtComp.Rows[0]["cmp_name"].ToString());
            //    }
            //    else
            //    {
            //        report.SetParameterValue("@CompanyName", (string)"COMPANY NAME");
            //    }
            //    if (dtComp.Rows[0]["cmpphno"].ToString() != "")
            //    {
            //        report.SetParameterValue("@CompanyPhone", (object)dtComp.Rows[0]["cmpphno"].ToString());
            //    }
            //    else
            //    {
            //        report.SetParameterValue("@CompanyPhone", (object)"COMPANY Phone");
            //    }
            //    if (dtComp.Rows[0]["cmpaddress"].ToString() != "")
            //    {
            //        report.SetParameterValue("@CompanyAddress", (object)dtComp.Rows[0]["cmpaddress"].ToString());
            //    }
            //    else
            //    {
            //        report.SetParameterValue("@CompanyAddress", (object)"COMPANY Address");
            //    }
            //    if (dtComp.Rows[0]["Email"].ToString() != "")
            //    {
            //        report.SetParameterValue("@Email", (object)dtComp.Rows[0]["Email"].ToString());
            //    }
            //    else
            //    {
            //        report.SetParameterValue("@Email", (object)"COMPANY Email");
            //    }
            //}
            //report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSDLClientsMaster");
            //report.Dispose();
            //GC.Collect();
        }
        #endregion
    }
}