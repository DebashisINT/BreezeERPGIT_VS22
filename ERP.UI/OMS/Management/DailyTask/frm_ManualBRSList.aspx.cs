using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.Export;
namespace ERP.OMS.Management.DailyTask
{
    public partial class frm_ManualBRSList : ERP.OMS.ViewState_class.VSPage
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();


     //   BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        string IsFirstTimeLoad = string.Empty;
        string CheckStatus = "";
        FinancialAccounting oFinancialAccounting = new FinancialAccounting();
        protected void Page_Load(object sender, EventArgs e)
        {
            IsFirstTimeLoad = "N";
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerDeliveryPendingList.aspx");
            if (!IsPostBack)
            {

                Session["exportval"] = null;
                //Session["CustomerDeliveryPendingdt"] = null;
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                PopulateBranchByHierchy(lastCompany);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                //FillGrid();
                //FillGridOnLoad();
                IsFirstTimeLoad = "Y";
            }

            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoadCalling", "<script language='Javascript'>Page_Laod();</script>");

            //if (IsFirstTimeLoad=="Y")
            //{
            //    FillGrid();
            //    IsFirstTimeLoad = "N";
            //}

            if (Request.QueryString["type"] != null)
            {
                if (Convert.ToString(Request.QueryString["type"]) == "SD")
                {
                    lblHeadertext.Text = "Customer Delivery - SD";
                }

            }
            else
            {
                lblHeadertext.Text = "Manual BRS";
            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBankForManualBRS(userbranchhierchy);
            cmbBankfilter.DataSource = branchtable;
            cmbBankfilter.ValueField = "MainAccount_AccountCode";
            cmbBankfilter.TextField = "IntegrateMainAccount";
            cmbBankfilter.DataBind();
            cmbBankfilter.SelectedIndex = 0;

            //cmbBankfilter.Value = Convert.ToString(Session["userbranchID"]);
        }

        public void FillGridOnLoadDlvType()
        {
            string DlvType = string.Empty;
            DlvType = Convert.ToString(Request.QueryString["type"]);
            if (!string.IsNullOrEmpty(DlvType))
            {
                hddnTypeIdd.Value = "1";
            }
            else
            {
                hddnTypeIdd.Value = "0";
            }

            DataTable dt = GetConfigSettingForBRS();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["Variable_Value"]).ToUpper() == "YES")
                {
                    hddnBRSConfigSettings.Value = "1";
                }
                else
                {
                    hddnBRSConfigSettings.Value = "0";
                }


            }
        }

        protected void btnShow_Click1(object sender, EventArgs e)
        {
            //BindTable();
            if (grdmanualBRS.VisibleRowCount == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "MessageForBlankRecord", "<script language='JavaScript'>jAlert('No Record Found');</script>");

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ShowUpdateCancel", "<script language='Javascript'>ShowUpdateCancelButton();</script>");

                //added by sanjib for search if data exist.
                Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHide1", "<script language='Javascript'>SearchVisible('');</script>");
            }
            //Page.ClientScript.RegisterStartupScript(GetType(), "CallingHeight", "<script language='Javascript'>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHide1", "<script language='Javascript'>SearchVisible('N');</script>");

        }

        protected DateTime Setstatementdate(object container)
        {
            string FormatedStatementDate = string.Empty;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            object value = c.Grid.GetRowValues(c.VisibleIndex, "cashbankdetail_bankstatementdate");
            string[] DateFormatStatement = null;
            DateTime dts = new DateTime(); ;
            if (value != null && !string.IsNullOrEmpty(Convert.ToString(value)))
            {
                if (Convert.ToString(value).Contains("-"))
                {
                    DateFormatStatement = Convert.ToString(value).Split('-');
                    string MM = DateFormatStatement[1].ToString();
                    string day = DateFormatStatement[0].ToString();
                    string Y = DateFormatStatement[2].ToString();
                    if (DateFormatStatement[0].ToString().Length != 2)
                    {
                        day = "0" + DateFormatStatement[0].ToString();
                    }
                    if (DateFormatStatement[1].ToString().Length != 2)
                    {
                        MM = "0" + DateFormatStatement[1].ToString();
                    }
                    FormatedStatementDate = day.Trim() + "-" + MM.Trim() + "-" + Y.Trim();
                    //string dd = Convert.ToString(value);

                    // DateTime.TryParseExact(dd, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dt);
                    DateTime dt = DateTime.ParseExact(FormatedStatementDate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return dt;
                }
                else if (Convert.ToString(value).Contains("/"))
                {
                    DateFormatStatement = Convert.ToString(value).Split('/');
                    string MM = DateFormatStatement[0].ToString();
                    string day = DateFormatStatement[1].ToString();
                    string Y = DateFormatStatement[2].ToString();
                    if (DateFormatStatement[1].ToString().Length != 2)
                    {
                        day = "0" + DateFormatStatement[1].ToString();
                    }
                    if (DateFormatStatement[0].ToString().Length != 2)
                    {
                        MM = "0" + DateFormatStatement[0].ToString();
                    }
                    FormatedStatementDate = day.Trim() + "-" + MM.Trim() + "-" + Y.Trim().Split(' ')[0];
                    //string dd = Convert.ToString(value);

                    // DateTime.TryParseExact(dd, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dt);
                    DateTime dt = DateTime.ParseExact(FormatedStatementDate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return dt;
                }



            }
            return dts;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../ProjectMainPage.aspx");
        }
        protected DateTime Setbankvaluedate(object container)
        {
            string[] DateFormatStatement;
            string FormatedStatementDate = string.Empty;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            object value = c.Grid.GetRowValues(c.VisibleIndex, "cashbankdetail_bankvaluedate");
            string[] DateFormatvalue;
            DateTime dts = new DateTime(); ;
            if (value != null && !string.IsNullOrEmpty(Convert.ToString(value)))
            {

                DateFormatStatement = Convert.ToString(value).Split('-');
                string MM = DateFormatStatement[1].ToString();
                string day = DateFormatStatement[0].ToString();
                string Y = DateFormatStatement[2].ToString();
                if (DateFormatStatement[0].ToString().Length != 2)
                {
                    day = "0" + DateFormatStatement[0].ToString();
                }
                if (DateFormatStatement[1].ToString().Length != 2)
                {
                    MM = "0" + DateFormatStatement[1].ToString();
                }
                FormatedStatementDate = day.Trim() + "-" + MM.Trim() + "-" + Y.Trim();
                //string dd = Convert.ToString(value);

                // DateTime.TryParseExact(dd, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dt);
                // DateTime dt = Convert.ToDateTime(FormatedStatementDate.Trim());
                DateTime myDate = DateTime.ParseExact(FormatedStatementDate.Trim(), "dd-MM-yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                return myDate;
            }
            return dts;
        }
      


        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        


        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            DataSet TempDs = new DataSet();
            DataTable TempTable = TempDs.Tables.Add();
            DataColumn[] keys = new DataColumn[1];
            DataColumn column;
            Boolean isvaluedate = false;




            for (int i = 0; i < grdmanualBRS.VisibleRowCount; i++)
            {


                string cashbank_transactionDate = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbank_transactionDate"));
                string cashbank_vouchernumber = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbank_vouchernumber"));
                string cashbankdetail_instrumentdate = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_instrumentdate"));
                string cashbankdetail_instrumentnumber = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_instrumentnumber"));
                string AccountCode = Convert.ToString(grdmanualBRS.GetRowValues(i, "AccountCode"));
                string cashbankdetail_paymentamount = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_paymentamount"));
                string cashbankdetail_receiptamount = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_receiptamount"));
                //Debashis
                string Module_Type = Convert.ToString(grdmanualBRS.GetRowValues(i, "Type"));
               

                string cashbankdetail_bankstatementdate = string.Empty;
                string cashbankdetail_bankvaluedate = string.Empty;

                //GridViewDataColumn col1 = grdmanualBRS.Columns[11] as GridViewDataColumn;
                GridViewDataColumn col2 = grdmanualBRS.Columns[11] as GridViewDataColumn;

                //ASPxDateEdit chkIsVal = grdmanualBRS.FindRowCellTemplateControl(i, col1, "txt_cashbankdate") as ASPxDateEdit;
                ASPxDateEdit chkIsVal = null;
                ASPxDateEdit textBox = grdmanualBRS.FindRowCellTemplateControl(i, col2, "bankvaluedate") as ASPxDateEdit;

                if (chkIsVal != null && !string.IsNullOrEmpty(chkIsVal.Text))
                {

                    cashbankdetail_bankstatementdate = chkIsVal.Text.Replace("/", "-").Trim();
                }
                else
                {
                    cashbankdetail_bankstatementdate = "1900-01-01";
                }
                if (textBox != null && !string.IsNullOrEmpty(textBox.Text) && textBox.Text.Trim() != "")
                { cashbankdetail_bankvaluedate = textBox.Text.Replace("/", "-").Trim(); }
                else
                {
                    cashbankdetail_bankvaluedate = "1900-01-01";
                }




                //added---06-09-2017-------------------------------
                //DateTime docdate = Convert.ToDateTime(grdmanualBRS.GetRowValues(i, "cashbank_transactionDate"));

                //Subhabrata @ for datetime issue added Parseexact
                DateTime docdate = DateTime.ParseExact(cashbank_transactionDate, "dd-MM-yyyy", null);
                //End
                if (textBox != null)
                {
                    if (textBox.Text.Trim() != "")
                    {
                        // DateTime valuedate = Convert.ToDateTime(textBox.Text.Replace("/", "-").Trim());
                        DateTime valuedate = textBox.Date;
                        if (valuedate >= docdate)
                        {
                            isvaluedate = false;
                        }
                        else
                        {
                            isvaluedate = true;
                            break;
                        }
                    }
                }
                //-------------------------------------------------




                TextBox txtValueDate = new TextBox();
                txtValueDate.Text = cashbankdetail_bankvaluedate;
                TextBox txtStatementDate = new TextBox();
                txtStatementDate.Text = cashbankdetail_bankstatementdate;
                string[] DateFormat;
                string[] DateFormatStatement;
                string FormatedValueDate;
                string FormatedStatementDate = "01-01-1900";
                if (txtValueDate.Text != "" && txtValueDate.Text != "01-01-0100")
                {
                    DateFormat = txtValueDate.Text.Split('-');


                    //Comment Out on 06/03/2017

                    string day = DateFormat[1].ToString();
                    string MM = DateFormat[0].ToString();
                    string Y = DateFormat[2].ToString();
                    FormatedValueDate = MM.Trim() + "-" + day.Trim() + "-" + Y.Trim();
                }
                else
                {

                    FormatedValueDate = "1900-01-01";

                    // FormatedValueDate = string.Empty;
                }
                if (txtStatementDate.Text != "" && txtStatementDate.Text != "01-01-0100")
                {
                    DateFormatStatement = txtStatementDate.Text.Split('-');


                    //Comment Out on 06/03/2017

                    string day = DateFormatStatement[1].ToString();
                    string MM = DateFormatStatement[0].ToString();
                    string Y = DateFormatStatement[2].ToString();

                    FormatedStatementDate = MM.Trim() + "-" + day.Trim() + "-" + Y.Trim();
                }
                else
                {
                    FormatedStatementDate = "1900-01-01";
                }

                Label lblRefID = new Label();
                lblRefID.Text = cashbankdetail_instrumentnumber;
                Label lblVouNo = new Label();
                lblVouNo.Text = cashbank_vouchernumber;
                //Debashis
                Label lblModuleType = new Label();
                lblModuleType.Text = Module_Type;
                //Debashis
                Label lblINo = new Label();
                lblINo.Text = cashbankdetail_instrumentnumber;
                Label lblTDate = new Label();
                if (!string.IsNullOrEmpty(cashbank_transactionDate))
                {
                    //DateTime dts = Convert.ToDateTime(cashbank_transactionDate);

                    //Subhabrata @ for datetime issue added Parseexact
                    DateTime dts = DateTime.ParseExact(cashbank_transactionDate, "dd-MM-yyyy", null);
                    //End

                    lblTDate.Text = dts.Date.ToString("yyyy-MM-dd");
                }
                else { lblTDate.Text = string.Empty; }




               
                if (!string.IsNullOrEmpty(lblVouNo.Text) && !string.IsNullOrEmpty(lblINo.Text))
                {
                    if (TempDs.Tables[0].Rows.Count > 0)
                    {
                        //Comment Out on 06/03/2017
                        //TempDs.Tables[0].Rows.Add(IdForUpdateData, lblMAcc.Text, lblSAcc.Text, lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate, strSegID);
                        //Comment Out on 06/03/2017
                        //Debashis
                        //TempDs.Tables[0].Rows.Add(lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate);
                        TempDs.Tables[0].Rows.Add(lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate, lblModuleType.Text);
                        //Debashis

                    }
                    else
                    {
                        


                        TempTable.Columns.Add("InstNo", typeof(string));
                        TempTable.Columns.Add("VoucherNo", typeof(string));
                        TempTable.Columns.Add("TranDate", typeof(string));
                        TempTable.Columns.Add("ValueDate", typeof(string));
                        TempTable.Columns.Add("StatementDate", typeof(string));
                        //Debashis
                        TempTable.Columns.Add("Module_Type", typeof(string));
                        TempTable.PrimaryKey = keys;
                        TempTable.TableName = "ManualBRS";
                        TempTable.Rows.Add(lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate, lblModuleType.Text);
                        //Debashis

                    }
                }
            }
            DataView TempDV = TempTable.DefaultView;

            if (isvaluedate == false)
            {
                if (TempDV.Count > 0)
                {
                    TempDV.RowFilter = "ValueDate<>''";
                  
                    oFinancialAccounting.UpdateManualBRS(TempDs.GetXml(), Session["userid"].ToString());
                    BindTable();
                    Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideUpdate", "<script language='Javascript'>jAlert('Saved Successfully');</script>");
                }

            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideCheck", "<script language='Javascript'>jAlert('Document Date Can't be greater than Value Date');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideCheck", "<script language='Javascript'>ValueDocAlert('Y');</script>");
            }

            Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideUpdate", "<script language='Javascript'>SearchVisible('N');</script>");


        }

        public void BindTable()
        {
            DataSet DsBind = new DataSet();
            string BankId = Convert.ToString(cmbBankfilter.Value);
            DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));
            if (RdAll.Checked == true)
            {
                CheckStatus = "A";
            }
            else if (RdUnCleared.Checked == true)
            {
                
                    CheckStatus = "U";

            }
            else if (RdCleared.Checked == true)
            {
                CheckStatus = "C";
            }
            
            //string nn = txtBankName_hidden.Value;
            //string dd = Convert.ToString(DateTo.Value);

            //string def = Convert.ToString(FromDate.Value);


            DsBind = oFinancialAccounting.FetchManualBRSData(ToDate.ToString("yyyy-MM-dd"),
                FromDate.ToString("yyyy-MM-dd"),
                CheckStatus, BankId);

            //comment by sanjib due to grid changed
            //grdDetails.DataSource = DsBind;
            //grdDetails.DataBind();

            grdmanualBRS.DataSource = DsBind;
            grdmanualBRS.DataBind();

            //if (DsBind.Tables[0].Rows.Count > 0)
            //{
            //    trhypertext.Visible = true;
            //    MainContent.Visible = true;
            //    ViewState["TableForThePage"] = DsBind.Tables[0];
            //    btnUpdate.Visible = true;
            //    btnCancel.Visible = true;
            //}

        }





        protected void grdmanualBRS_DataBinding(object sender, EventArgs e)
        {
            DataTable CustgGrd = null;
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string DlvType = string.Empty;
            DlvType = Convert.ToString(Request.QueryString["type"]);
            //DataTable dtdata = new DataTable();
            string BankId = Convert.ToString(cmbBankfilter.Value);
            DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

            if (RdAll.Checked == true)
            {
                CheckStatus = "A";
            }
            else if (RdUnCleared.Checked == true)
            {

                CheckStatus = "U";

            }
            else if (RdCleared.Checked == true)
            {
                CheckStatus = "C";
            }

            DataSet dtdata = new DataSet();

            dtdata = oFinancialAccounting.FetchManualBRSData(ToDate.ToString("yyyy-MM-dd"),
            FromDate.ToString("yyyy-MM-dd"),
            CheckStatus, BankId);

            if (dtdata != null)
            {
                grdmanualBRS.DataSource = dtdata;

            }

        }

        public DataTable GetConfigSettingForBRS()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_SystemsettingBRSForODSD");
            proc.AddVarcharPara("@Option", 500, "BRSMandatory");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetGridData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "SalesOrder");
            dt = proc.GetTable();
            return dt;
        }


        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.SalesOrderEditablePermission(Convert.ToInt32(ActiveUser));

            //}
            return Convert.ToString(ispermission);

        }

        [WebMethod]
        public static string GetCustomerId(string KeyVal)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            string ispermission = string.Empty;
            ispermission = objCRMSalesOrderDtlBL.GetInvoiceCustomerId(Convert.ToInt32(KeyVal));


            return Convert.ToString(ispermission);

        }

        [WebMethod]
        public static string GetChallanIdIsExistInSalesInvoice(string keyValue)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.GetIdForCustomerDeliveryPendingExists(keyValue);

            //}
            return Convert.ToString(ispermission);

        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
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

        public void bindexport(int Filter)
        {
            //grdmanualBRS.Columns[5].Visible = false;
            string filename = "ManualBRS";
            exporter.FileName = filename;
            exporter.FileName = "GrdManualBRS";

            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true,true) + Environment.NewLine + "Manual BRS" + Environment.NewLine + "For the period " + Convert.ToDateTime(FormDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(toDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        protected void grdmanualBRS_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            grdmanualBRS.JSProperties["cpinsert"] = null;
            grdmanualBRS.JSProperties["cpEdit"] = null;
            grdmanualBRS.JSProperties["cpUpdate"] = null;
            grdmanualBRS.JSProperties["cpDelete"] = null;
            grdmanualBRS.JSProperties["cpExists"] = null;
            grdmanualBRS.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

            if (WhichCall == "FilterGridByDate")
            {
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BankId = Convert.ToString(e.Parameters.Split('~')[3]);

                if (RdAll.Checked == true)
                {
                    CheckStatus = "A";
                }
                else if (RdUnCleared.Checked == true)
                {

                    CheckStatus = "U";

                }
                else if (RdCleared.Checked == true)
                {
                    CheckStatus = "C";
                }
               

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                DataSet dtdata = new DataSet();

                dtdata = oFinancialAccounting.FetchManualBRSData(ToDate.ToString("yyyy-MM-dd"),
                FromDate.ToString("yyyy-MM-dd"),
                CheckStatus, BankId);

                if (dtdata != null && dtdata.Tables[0].Rows.Count > 0)
                {
                    grdmanualBRS.DataSource = dtdata;
                    grdmanualBRS.DataBind();
                }
                else
                {
                    grdmanualBRS.DataSource = null;
                    grdmanualBRS.DataBind();
                }
            }
        }
        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

           // BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();


            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }
        

       
    }
}