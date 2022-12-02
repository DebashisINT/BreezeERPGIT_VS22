using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_cdslBill : System.Web.UI.Page
    {
        public string[] InputName = new string[5];
        public string[] InputType = new string[5];
        public string[] InputValue = new string[5];

        static String CompanyId, dpId, SegmentId;
        public string qstr;
        string path;
        public string FinYear;
        static int ShowFlag = 0;
        static string MonthRange;

        PeriodicalReports PeriodicalReports = new PeriodicalReports();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        static DataTable cdslBillonScreen = new DataTable();
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public int CurrentPage
        {

            get
            {
                if (this.ViewState["CurrentPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
            }

            set
            {
                this.ViewState["CurrentPage"] = value;
            }

        }

        public int LastPage
        {
            get
            {
                if (this.ViewState["LastPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["LastPage"].ToString());
            }
            set
            {
                this.ViewState["LastPage"] = value;
            }

        }

        public int TblStartIndex
        {
            get
            {
                if (this.ViewState["TblStartIndex"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["TblStartIndex"].ToString());
            }
            set
            {
                this.ViewState["TblStartIndex"] = value;
            }

        }

        public int TblEndIndex
        {
            get
            {
                if (this.ViewState["TblEndIndex"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["TblEndIndex"].ToString());
            }
            set
            {
                this.ViewState["TblEndIndex"] = value;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "height", "<script language='javascript'>PageLoad();</script>");

            qstr = Request.QueryString["type"];

            if (!IsPostBack)
            {
                //bindFrmToDate();
                FinYear = HttpContext.Current.Session["LastFinYear"].ToString().Trim();
                DateTime prevMnth = System.DateTime.Today.AddMonths(-1);
                cmbMonthFrom.Value = prevMnth.Month;
                //String.Format("{0:MMM}", prevMnth);
                cmbMonthTo.Value = prevMnth.Month;
                //String.Format("{0:MMM}", prevMnth);

                ShowFlag = 0;
                CompanyId = String.Empty;
                dpId = String.Empty;
                SegmentId = String.Empty;
                txtEmpName.Attributes.Add("onkeyup", "CallAjax(this,'SearchByEmployees',event,'')");

            }
            else
                bindGrid();

        }

        void bindGrid()
        {

            bind_CompanyID_SegmentID();

            ClearArr();
            InputName[0] = "finYear";
            InputName[1] = "dpId";
            InputName[2] = "cmpID";
            InputName[3] = "month";
            InputName[4] = "billamt";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";

            InputValue[0] = HttpContext.Current.Session["LastFinYear"].ToString().Trim();
            InputValue[1] = dpId;
            InputValue[2] = CompanyId;

            if (ShowFlag == 0)
                InputValue[3] = cmbMonthFrom.SelectedItem.Value.ToString();
            else if (ShowFlag == 1)
                InputValue[3] = MonthRange;

            InputValue[4] = txtbilAmt.Text;

            DataTable DTable = new DataTable();

            if (Request.QueryString["type"] == "CDSL")
                DTable = SQLProcedures.SelectProcedureArr("cdslViewBill1", InputName, InputType, InputValue);

            else if (Request.QueryString["type"] == "NSDL")
                DTable = SQLProcedures.SelectProcedureArr("sp_NsdlBill_Show1", InputName, InputType, InputValue);

            //DataView dvData = new DataView(DTable);
            //dvData.RowFilter = "DPBillSummary_NetBillAmount >=" + Convert.ToDecimal(txtbilAmt.Text)+"" ;

            //gridCdslBill.DataSource = dvData;

            gridCdslBill.DataSource = DTable;
            gridCdslBill.DataBind();
            //gridCdslBill.FilterExpression = string.Empty;
        }

        public void ClearArr()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }

        protected void gridCdslBill_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            if (e.Parameters == "s")
            {
                gridCdslBill.Settings.ShowFilterRow = true;

            }
            else if (e.Parameters == "All")
            {
                gridCdslBill.FilterExpression = string.Empty;

            }
            else if (e.Parameters.Split('~')[0] == "Show")
            {
                gridCdslBill.FilterExpression = string.Empty;
                ShowFlag = 1;
                MonthRange = e.Parameters.Split('~')[1].ToString();
                bindGrid();
            }

            //if(gridCdslBill.FocusedRowIndex<0)
            //{
            gridCdslBill.FocusedRowIndex = 0;
            //}

        }
        protected void gridCdslBill_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                if (e.GetValue("DPBillSummary_AccountOpeningCharges") != DBNull.Value)
                {

                    e.Row.Cells[5].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_AccountOpeningCharges")));

                }

                if (e.GetValue("DPBillSummary_AMC") != DBNull.Value)
                {

                    e.Row.Cells[6].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_AMC")));

                }
                if (e.GetValue("DPBillSummary_TransactionCharges") != DBNull.Value)
                {

                    e.Row.Cells[7].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_TransactionCharges")));

                }


                if (e.GetValue("DPBillSummary_Demat") != DBNull.Value)
                {

                    e.Row.Cells[8].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_Demat")));

                }

                if (e.GetValue("DPBillSummary_Pledge") != DBNull.Value)
                {

                    e.Row.Cells[9].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_Pledge")));

                }

                if (e.GetValue("DPBillSummary_HoldingCharges") != DBNull.Value)
                {

                    e.Row.Cells[10].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_HoldingCharges")));

                }

                if (e.GetValue("DPBillSummary_SettlementCharges") != DBNull.Value)
                {

                    e.Row.Cells[11].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_SettlementCharges")));

                }

                if (e.GetValue("otherCharges") != DBNull.Value)
                {

                    e.Row.Cells[12].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("otherCharges")));

                }
                if (e.GetValue("total") != DBNull.Value)
                {

                    e.Row.Cells[13].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("total")));

                }

                if (e.GetValue("allTaxes") != DBNull.Value)
                {

                    e.Row.Cells[14].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("allTaxes")));

                }

                if (e.GetValue("DPBillSummary_RoundOffAmount") != DBNull.Value)
                {

                    e.Row.Cells[15].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_RoundOffAmount")));

                }

                if (e.GetValue("DPBillSummary_NetBillAmount") != DBNull.Value)
                {

                    e.Row.Cells[16].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("DPBillSummary_NetBillAmount")));

                }

            }

        }


        protected void gridCdslBill_SummaryDisplayText2(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.Value));


        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Converter objConverter = new Converter();


            String CompanyId, dpId, SegmentId, billnoFinYear, financilaYear, StartDate, EndDate, AccountNumber, Billnumber;
            int scaningStatus;

            scaningStatus = 0;

            object keyValues = gridCdslBill.GetRowValues(gridCdslBill.FocusedRowIndex, (new string[] { gridCdslBill.KeyFieldName }));



            AccountNumber = String.Empty;
            Billnumber = String.Empty;


            AccountNumber = keyValues.ToString().Split('-').GetValue(3).ToString();
            Billnumber = keyValues.ToString().Substring(5, 3);


            cdslBillViewCalculation cbill = new cdslBillViewCalculation();

            ExcelFile obj = new ExcelFile();

            int UseHeader, UseFooter;

            if ((chbHeader.Checked) && (obj.IsNumeric(txtHeader_hidden.Text.Trim())))
                UseHeader = Convert.ToInt32(txtHeader_hidden.Text);
            else
                UseHeader = 0;

            if ((chbFooter.Checked) && (obj.IsNumeric(txtFooter_hidden.Text.Trim())))
                UseFooter = Convert.ToInt32(txtFooter_hidden.Text);
            else
                UseFooter = 0;

            if (Request.QueryString["type"].ToString() == "CDSL")
            {
                cbill.callCrystalReport(Billnumber, AccountNumber,
                                         "", "PinCode", "CDSL",
                                                txtbilAmt.Text, "Print", scaningStatus, "", "", "", 1408,
                                                UseHeader, UseFooter);


            }
            else
            {
                //showCdslCrystalReport();
                cbill.callCrystalReport(Billnumber, AccountNumber,
                                     "", "PinCode", "NSDL",
                                            txtbilAmt.Text, "Print", scaningStatus, "", "", "", 1244,
                                            UseHeader, UseFooter);
            }


        }

        protected void btnView_Click(object sender, EventArgs e)
        {

            try
            {
                displayCdslBill();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CheckView", "height();", true);

            }
            catch (Exception ex)
            {

                //if (!Page.ClientScript.IsStartupScriptRegistered("CheckView"))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CheckView", "alert('Please Select a Bill.')", true);

            }

        }




        // Generate crystal Report For A Particular Client//

        public void showCdslCrystalReport()
        {
            // cdslBillViewCalculation cdslBill = new cdslBillViewCalculation();

            String AccountNumber, Billnumber, StartDate, EndDate, financilaYear, billnoFinYear, signaturePath;


            // System.Collections.Generic.List<Object> keyValues = gridCdslBill.GetSelectedFieldValues(new string[] { gridCdslBill.KeyFieldName });
            try
            {
                object keyValues = gridCdslBill.GetRowValues(gridCdslBill.FocusedRowIndex, (new string[] { gridCdslBill.KeyFieldName }));

                objConverter.getFirstAndLastDate("December", out StartDate, out EndDate, out billnoFinYear);
                //cmbMonth.SelectedItem.Value.ToString()

                AccountNumber = String.Empty;
                Billnumber = String.Empty;


                AccountNumber = keyValues.ToString().Split('-').GetValue(3).ToString();
                Billnumber = keyValues.ToString().Substring(0, 14);

                DataSet logo = new DataSet();
                DataSet Clients = new DataSet();
                DataSet Holding = new DataSet();
                DataSet Summary = new DataSet();
                DataSet AccountSummary = new DataSet();
                DataSet signature = new DataSet();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    SqlCommand cmdClients = new SqlCommand();
                    cmdClients.Connection = con;

                    //if (qstr == "CDSL")
                    //    cmdClients.CommandText = "cdslBill_Report";

                    //else if (qstr == "NSDL")
                    //    cmdClients.CommandText = "sp_NsdlBill_Report";

                    //cmdClients.CommandType = CommandType.StoredProcedure;

                    //cmdClients.Parameters.AddWithValue("@billNumber", Billnumber);
                    //cmdClients.Parameters.AddWithValue("@BenAccount", AccountNumber);
                    //cmdClients.Parameters.AddWithValue("@group", "NA");
                    //cmdClients.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                    //cmdClients.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
                    //cmdClients.Parameters.AddWithValue("@billamt", txtbilAmt.Text);

                    //cmdClients.CommandTimeout = 0;
                    //SqlDataAdapter daClients = new SqlDataAdapter();
                    //daClients.SelectCommand = cmdClients;
                    //daClients.Fill(Clients);

                    Clients = PeriodicalReports.Bill_Report(qstr, Billnumber, AccountNumber, SegmentId, CompanyId, txtbilAmt.Text);

                    //SqlCommand cmdHolding = new SqlCommand("cdslBill_ReportHolding", con);
                    //cmdHolding.CommandType = CommandType.StoredProcedure;

                    //cmdHolding.Parameters.AddWithValue("@billNumber", Billnumber);
                    //cmdHolding.Parameters.AddWithValue("@BenAccount", AccountNumber);
                    //cmdHolding.Parameters.AddWithValue("@group", "NA");
                    //cmdHolding.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                    //cmdHolding.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
                    //cmdHolding.Parameters.AddWithValue("@dp", Request.QueryString["type"]);


                    //cmdHolding.CommandTimeout = 0;
                    //SqlDataAdapter daHolding = new SqlDataAdapter();
                    //daHolding.SelectCommand = cmdHolding;
                    //daHolding.Fill(Holding);

                    Holding = PeriodicalReports.Bill_ReportReportHolding(Billnumber, AccountNumber, SegmentId, CompanyId, Request.QueryString["type"]);


                    //SqlCommand cmdSummary = new SqlCommand("cdslBill_ReportSummary", con);
                    //cmdSummary.CommandType = CommandType.StoredProcedure;

                    //cmdSummary.Parameters.AddWithValue("@billNumber", Billnumber);
                    //cmdSummary.Parameters.AddWithValue("@BenAccount", AccountNumber);
                    //cmdSummary.Parameters.AddWithValue("@group", "NA");
                    //cmdSummary.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                    //cmdSummary.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);



                    //cmdSummary.CommandTimeout = 0;
                    //SqlDataAdapter daSummary = new SqlDataAdapter();
                    //daSummary.SelectCommand = cmdSummary;
                    //daSummary.Fill(Summary);

                    Summary = PeriodicalReports.cdslBill_ReportSummary(Billnumber, AccountNumber, SegmentId, CompanyId);


                    //SqlCommand cmdAccount = new SqlCommand("cdslBill_ReportAccountsLedger", con);
                    //cmdAccount.CommandType = CommandType.StoredProcedure;

                    //cmdAccount.Parameters.AddWithValue("@startDate", StartDate);
                    //cmdAccount.Parameters.AddWithValue("@endDate", EndDate);
                    //cmdAccount.Parameters.AddWithValue("@dpId", dpId);
                    //cmdAccount.Parameters.AddWithValue("@companyID", CompanyId);
                    //cmdAccount.Parameters.AddWithValue("@SegmentId", SegmentId);

                    //if (qstr == "CDSL")
                    //    cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00042");
                    //else if (qstr == "NSDL")
                    //    cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00043");


                    //cmdAccount.Parameters.AddWithValue("@SubAccountID", "'" + AccountNumber + "'");
                    //cmdAccount.Parameters.AddWithValue("@financialYear", HttpContext.Current.Session["LastFinYear"]);
                    //cmdAccount.Parameters.AddWithValue("@groupCode", "NA");

                    //cmdAccount.CommandTimeout = 0;
                    //SqlDataAdapter daAccount = new SqlDataAdapter();
                    //daAccount.SelectCommand = cmdAccount;
                    //daAccount.Fill(AccountSummary);

                    AccountSummary = PeriodicalReports.cdslBill_ReportAccountsLedger(StartDate, EndDate, dpId, CompanyId, SegmentId, qstr, AccountNumber, HttpContext.Current.Session["LastFinYear"].ToString());
                }

                //if (qstr == "CDSL")
                //{
                //    Clients.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslClients.xsd");
                //    Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslBillHolding.xsd");
                //    Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslBillSummary.xsd");

                //}
                //else if (qstr == "NSDL")
                //{
                //    Clients.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlClients.xsd");
                //    Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                //    Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");


                //}

                AccountSummary.Tables[0].Columns.Add("StartDate", System.Type.GetType("System.String"));
                for (int i = 0; i < AccountSummary.Tables[0].Rows.Count; i++)
                {
                    AccountSummary.Tables[0].Rows[i]["openingBalance"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["openingBalance"].ToString())));
                    if (AccountSummary.Tables[0].Rows[i]["AccountsLedger_amountCr"] != DBNull.Value)
                        AccountSummary.Tables[0].Rows[i]["AccountsLedger_amountCr"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["AccountsLedger_amountCr"].ToString())));

                    if (AccountSummary.Tables[0].Rows[i]["Adjustment"] != DBNull.Value)
                        AccountSummary.Tables[0].Rows[i]["Adjustment"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["Adjustment"].ToString())));

                    AccountSummary.Tables[0].Rows[i]["netBillAmt"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["netBillAmt"].ToString())));
                    AccountSummary.Tables[0].Rows[i]["closingBalance"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["closingBalance"].ToString())));
                    AccountSummary.Tables[0].Rows[i]["StartDate"] = StartDate;

                }

                DataRow drow, drow1;
                logo.Tables.Add();
                logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));

                drow1 = logo.Tables[0].NewRow();
                drow1["Image"] = getLogoImage(MapPath(@"..\images\logo.jpg"));
                logo.Tables[0].Rows.Add(drow1);

                //drow.Delete();
                //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xml");

                signature.Tables.Add();
                signature.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                signature.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
                signature.Tables[0].Columns.Add("signName", System.Type.GetType("System.String"));

                drow = signature.Tables[0].NewRow();

                string tmpimgname;
                if (chkSignature.Checked)
                {
                    signaturePath = oDBEngine.GetFieldValue1(" tbl_master_document ", " doc_source ", " doc_contactId='" + txtEmpName_hidden.Text + "' and doc_documentTypeId=(select top 1 dty_id from tbl_master_documentType where dty_documentType='Signature' and dty_applicableFor='Employee') ", 1).GetValue(0).ToString().Split('~').GetValue(1).ToString();
                    //signaturePath = "b1202280000014620.bmp";
                    tmpimgname = "thumble_" + signaturePath;

                    signaturePath = MapPath(@"..\Documents\" + signaturePath);
                    tmpimgname = MapPath(@"..\Documents\" + tmpimgname);

                    FileStream fs;
                    BinaryReader br;

                    if (File.Exists(signaturePath))
                    {


                        System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(signaturePath);

                        System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

                        System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(200, 20, dummyCallBack, IntPtr.Zero);

                        // String MyString = MyDate.ToString("ddMMyyhhmmss") + ".png";

                        //Save the thumbnail in Png format. You may change it to a diff format with the ImageFormat property
                        thumbNailImg.Save(tmpimgname, ImageFormat.MemoryBmp);

                        thumbNailImg.Dispose();

                        fs = new FileStream(tmpimgname, FileMode.Open);
                        br = new BinaryReader(fs);
                        // define the byte array of filelength 
                        byte[] imgbyte = new byte[fs.Length + 1];
                        // read the bytes from the binary reader 
                        imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
                        drow["Image"] = imgbyte;
                        drow["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
                        drow["signName"] = txtEmpName.Text.Split('[').GetValue(0);
                        // add the image in bytearray 

                        // add row into the datatable 
                        br.Close();
                        // close the binary reader 
                        fs.Close();
                        // close the file stream 
                        File.Delete(tmpimgname);
                    }


                }
                signature.Tables[0].Rows.Add(drow);





                //if (qstr == "CDSL")
                //{
                //    AccountSummary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslBillaccounts.xsd");
                //    signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xsd");
                //    signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xml");
                //}
                //else if (qstr == "NSDL")
                //{
                //    AccountSummary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillaccounts.xsd");
                //    signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlSignature.xsd");
                //    signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlSignature.xml");

                //}



                ReportDocument cdslTransctionReportDocu = new ReportDocument();
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');

                if (qstr == "CDSL")
                    path = Server.MapPath("..\\Reports\\cdslBill.rpt");
                else if (qstr == "NSDL")
                    path = Server.MapPath("..\\Reports\\NsdlBill.rpt");

                cdslTransctionReportDocu.Load(path);
                cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                cdslTransctionReportDocu.SetDataSource(Clients);
                if (qstr == "CDSL")
                {
                    cdslTransctionReportDocu.Subreports["billSummary"].SetDataSource(Summary);
                    cdslTransctionReportDocu.Subreports["cdslAccounts"].SetDataSource(AccountSummary);
                    cdslTransctionReportDocu.Subreports["cdslBillHolding"].SetDataSource(Holding);
                    cdslTransctionReportDocu.Subreports["signature"].SetDataSource(signature);
                }
                else if (qstr == "NSDL")
                {
                    cdslTransctionReportDocu.Subreports["NsdlBillSummary"].SetDataSource(Summary);
                    cdslTransctionReportDocu.Subreports["NsdlBill_Accounts"].SetDataSource(AccountSummary);
                    cdslTransctionReportDocu.Subreports["NsdlBill_Holding"].SetDataSource(Holding);
                    cdslTransctionReportDocu.Subreports["NsdlBill_Signature"].SetDataSource(signature);
                }


                cdslTransctionReportDocu.Subreports["logo"].SetDataSource(logo);

                if (qstr == "CDSL")

                    cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "CDSL Bill");
                else if (qstr == "NSDL")
                    cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NSDL Bill");



            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CheckPrint", "alert('" + ex.Message + "');", true);
            }

        }

        // End of Generate Report For A Particulare Client//


        //Display a Particular Client Bill Details

        void displayCdslBill()
        {
            String AccountNumber, Billnumber;

            object keyValues = gridCdslBill.GetRowValues(gridCdslBill.FocusedRowIndex, (new string[] { gridCdslBill.KeyFieldName }));

            AccountNumber = String.Empty;
            Billnumber = String.Empty;


            AccountNumber = keyValues.ToString().Split('-').GetValue(3).ToString();
            Billnumber = keyValues.ToString().Substring(0, 14);

            cdslBillonScreen.Reset();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmdBillonScreen = new SqlCommand();

            //    cmdBillonScreen.Connection = con;


            //    if (qstr == "CDSL")
            //    {
            //        cmdBillonScreen.CommandText = "cdslBill_DisplayScreen";
            //        //cmdBillonScreen.Parameters.AddWithValue("@billamt", txtbilAmt.Text);
            //        //cmdBillonScreen.Parameters.AddWithValue("@generationOrder", "PinCode");


            //    }
            //    else if (qstr == "NSDL")
            //        cmdBillonScreen.CommandText = "sp_NsdlBill_ClientScreen";

            //    cmdBillonScreen.CommandType = CommandType.StoredProcedure;

            //    cmdBillonScreen.Parameters.AddWithValue("@billNumber", Billnumber);
            //    cmdBillonScreen.Parameters.AddWithValue("@BenAccount", AccountNumber);
            //    cmdBillonScreen.Parameters.AddWithValue("@group", "NA");
            //    cmdBillonScreen.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
            //    cmdBillonScreen.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
            //    cmdBillonScreen.Parameters.AddWithValue("@billamt", txtbilAmt.Text);
            //    cmdBillonScreen.Parameters.AddWithValue("@generationOrder", "PinCode");

            //    cmdBillonScreen.CommandTimeout = 0;
            //    SqlDataAdapter daBillonScreen = new SqlDataAdapter();
            //    daBillonScreen.SelectCommand = cmdBillonScreen;
            //    daBillonScreen.Fill(cdslBillonScreen);
            //}
            cdslBillonScreen = PeriodicalReports.cdslBill_cNsdlBill_ClientScreen(qstr, Billnumber, AccountNumber, CompanyId, SegmentId, txtbilAmt.Text.Trim());
            LastPage = cdslBillonScreen.Rows.Count - 1;
            CurrentPage = 0;
            bind_HoldersDetails();

        }

        void bind_HoldersDetails()
        {
            if (LastPage > -1)
            {
                TblEndIndex = 0;
                TblStartIndex = 0;


                if (qstr == "CDSL")
                {
                    boid.Text = cdslBillonScreen.Rows[CurrentPage]["DPBillSummary_BenAccountNumber"].ToString();
                    category.Text = cdslBillonScreen.Rows[CurrentPage]["CdslClients_AccountCategory"].ToString();
                    status.Text = cdslBillonScreen.Rows[CurrentPage]["CdslClients_AccountStatus"].ToString();
                }
                else if (qstr == "NSDL")
                {
                    boid.Text = cdslBillonScreen.Rows[CurrentPage]["NsdlClients_BenAccountID"].ToString();
                    category.Text = cdslBillonScreen.Rows[CurrentPage]["AccountCategory"].ToString();
                    status.Text = cdslBillonScreen.Rows[CurrentPage]["AccountStatus"].ToString();

                }

                holders.Text = cdslBillonScreen.Rows[CurrentPage]["name"].ToString();
                billNumber.Text = cdslBillonScreen.Rows[CurrentPage]["DPBillSummary_BillNumber"].ToString();
                //listRecord.Text = CurrentPage + 1 + " of " + cdslBillonScreen.Rows.Count.ToString() + " Bill.";
                listRecord.Text = "1 of 1 Bill.";
                bindTransactions();

            }
            ShowHidePreviousNext_of_Holders();


        }

        protected void btnTransFirst_Click(object sender, EventArgs e)
        {
            TblStartIndex = 0;

            bindTable();


        }
        protected void btnTransPrevious_Click(object sender, EventArgs e)
        {
            TblStartIndex = TblStartIndex - 20;
            if (TblStartIndex < 0)
            {

                TblStartIndex = 0;
            }
            bindTable();

        }
        protected void btnTransnNext_Click(object sender, EventArgs e)
        {
            TblStartIndex = TblStartIndex + 20;
            bindTable();


        }
        protected void btnTransLast_Click(object sender, EventArgs e)
        {
            if (TblEndIndex % 20 == 0)
            {
                TblStartIndex = TblEndIndex - 20;
            }
            else
            {
                TblStartIndex = TblEndIndex - TblEndIndex % 20;
            }
            //TblStartIndex = 1135;

            bindTable();


        }

        protected void btnFirst(object sender, EventArgs e)
        {
            CurrentPage = 0;
            bind_HoldersDetails();
        }
        protected void btnPrevious(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage = CurrentPage - 1;
                bind_HoldersDetails();
            }
        }
        protected void btnNext(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                CurrentPage = CurrentPage + 1;
                bind_HoldersDetails();
            }

        }
        protected void btnLast(object sender, EventArgs e)
        {
            CurrentPage = LastPage;
            bind_HoldersDetails();
        }




        void bindTransactions()
        {

            DataTable Transaction = new DataTable();

            if (qstr == "CDSL")
                oDBEngine.DeleteValue("Tmp_CDSL_Transction", " Create_User=" + Session["userid"].ToString());
            else if (qstr == "NSDL")
                oDBEngine.DeleteValue("Tmp_NSDLBill_Transaction", " Create_User=" + Session["userid"].ToString());


            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandType = CommandType.StoredProcedure;


            //    if (qstr == "CDSL")
            //        cmd.CommandText = "cdslBillDisplayonScreen";
            //    else if (qstr == "NSDL")
            //        cmd.CommandText = "sp_NsdlBill_BillScreen";


            //    cmd.Parameters.AddWithValue("@DPBillSummary_BillNumber", billNumber.Text);

            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(Transaction);
            //}
            Transaction = PeriodicalReports.cdslBill_NsdlBill_BillScreen(qstr, billNumber.Text.Trim());

            DataColumn dc = new DataColumn("openingStatus", System.Type.GetType("System.String"));
            Transaction.Columns.Add(dc);

            DataColumn dc1 = new DataColumn("ClosingStatus", System.Type.GetType("System.String"));
            Transaction.Columns.Add(dc1);

            DataColumn dc2 = new DataColumn("Create_User", System.Type.GetType("System.String"));
            Transaction.Columns.Add(dc2);

            DataColumn dc3 = new DataColumn("RowNo", System.Type.GetType("System.String"));
            Transaction.Columns.Add(dc3);


            for (int k = 0; k < Transaction.Rows.Count; k++)
            {

                if (k > 0)
                {
                    if ((qstr == "CDSL"
                        && Transaction.Rows[k - 1]["CDSLISIN_Number"].ToString() == Transaction.Rows[k]["CDSLISIN_Number"].ToString()
                        && Transaction.Rows[k - 1]["CdslTransaction_SettlementID"].ToString() == Transaction.Rows[k]["CdslTransaction_SettlementID"].ToString()
                        && Transaction.Rows[k]["transactionType"].ToString() == Transaction.Rows[k - 1]["transactionType"].ToString())
                        ||
                        (qstr == "NSDL"
                        && Transaction.Rows[k - 1]["NSDLISIN_Number"].ToString() == Transaction.Rows[k]["NSDLISIN_Number"].ToString()
                        && Transaction.Rows[k - 1]["NsdlTransaction_SettlementNumber"].ToString() == Transaction.Rows[k]["NsdlTransaction_SettlementNumber"].ToString()
                        && Transaction.Rows[k]["Transaction_Type"].ToString() == Transaction.Rows[k - 1]["Transaction_Type"].ToString())
                        )
                    {
                        //do nothing
                    }
                    else
                    {
                        Transaction.Rows[k - 1]["ClosingStatus"] = "Y";
                        Transaction.Rows[k]["openingStatus"] = "Y";
                    }
                }
                else
                {

                    Transaction.Rows[0]["openingStatus"] = "Y";
                    Transaction.Rows[Transaction.Rows.Count - 1]["ClosingStatus"] = "Y";
                }


                Transaction.Rows[k]["Create_User"] = Session["userid"].ToString();
                Transaction.Rows[k]["RowNo"] = k + 1;


            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlBulkCopy sbc = new SqlBulkCopy(con);

                if (qstr == "CDSL")
                    sbc.DestinationTableName = "Tmp_CDSL_Transction";

                else if (qstr == "NSDL")
                    sbc.DestinationTableName = "Tmp_NSDLBill_Transaction";

                if (con.State.Equals(ConnectionState.Open))
                {
                    con.Close();
                }
                con.Open();
                sbc.WriteToServer(Transaction);
                con.Close();
            }

            ViewState["totalTransaction"] = Transaction.Rows.Count;

            DataView dvTrans = Transaction.DefaultView;
            if (qstr == "CDSL")
            {
                dvTrans.RowFilter = "CDSLISIN_Number is not null";
            }
            else
            {
                dvTrans.RowFilter = "NsdlTransaction_ID is not null";
            }
            int x = dvTrans.Count;

            TblEndIndex = Transaction.Rows.Count;
            lblTotalTransction.Text = Transaction.Rows.Count.ToString();
            lblTotalTransction1.Text = Transaction.Rows.Count.ToString();

            if (x == 0)
            {
                tdlblTrans.Visible = false;
                tdlblTrans1.Visible = false;
            }

            Transaction.Reset();

            bindTable();

        }

        void bindTable()
        {
            int startIndex, endIndex;

            startIndex = TblStartIndex;
            endIndex = startIndex + 20;

            if (endIndex >= TblEndIndex)
            {
                endIndex = TblEndIndex;
            }

            lblTransction.Text = endIndex.ToString();
            lblTransction1.Text = lblTransction.Text;

            ShowHidePreviousNext_of_Transaction();

            DataTable RstTable = new DataTable();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;

            //    if (qstr == "CDSL")
            //        cmd.CommandText = "cdslFeatchTransctionAfterBilling";
            //    else if (qstr == "NSDL")
            //        cmd.CommandText = "sp_NsdlBill_FetchTransaction";

            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@userid", Session["userid"]);

            //    cmd.Parameters.AddWithValue("@startRowIndex", startIndex);

            //    cmd.Parameters.AddWithValue("@endIndex", endIndex);



            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(RstTable);

            //}
            RstTable = PeriodicalReports.cdslBill_NsdlBill_FetchTransaction(qstr, Convert.ToString(Session["userid"]), startIndex, endIndex);
            ////////////////////////////////////////////////////////////

            String strHtml = String.Empty;


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
            strHtml += "<td colspan=2><b>Particulars</b></td>";
            strHtml += "<td align=\"right\"><b>Rate</b></td>";
            strHtml += "<td align=\"right\"><b>Credit</b></td>";
            strHtml += "<td align=\"right\"><b>Debit</b></td>";
            strHtml += "<td align=\"right\"><b>Current Balance</b></td>";
            strHtml += "<td align=\"right\"><b>Amount</b></td></tr>";


            string prevIsin = "", prevSett = "", prevtrnasType = "";


            int flag = 0;

            for (int k = 0; k < RstTable.Rows.Count; k++)
            {


                if ((qstr == "CDSL"
                    && prevIsin == RstTable.Rows[k]["CDSLISIN_Number"].ToString()
                    && prevSett == RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString()
                    && prevtrnasType == RstTable.Rows[k]["transactionType"].ToString())
                    ||
                    (qstr == "NSDL"
                    && prevIsin == RstTable.Rows[k]["NSDLISIN_Number"].ToString()
                    && prevSett == RstTable.Rows[k]["NsdlTransaction_SettlementID"].ToString()
                    && prevtrnasType == RstTable.Rows[k]["AccountType"].ToString()))
                {
                    flag = flag + 1;

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=3 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }




                    if (qstr == "CDSL")
                    {
                        strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                        strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    }
                    else if (qstr == "NSDL")
                    {
                        strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k]["NsdlTransaction_Date"].ToString() + "</td>";
                        strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    }

                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["Rate"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";

                    if (qstr == "CDSL")
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td>";
                    else if (qstr == "NSDL")
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["NsdlTransaction_Quantity"].ToString() + "</td>";

                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["Amount"].ToString() + "</td></tr>";

                    if (qstr == "CDSL")
                    {
                        if (RstTable.Rows[k]["DPOtherCharge_HoldingNarration"] != DBNull.Value)
                        {
                            strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td></td><td colspan=6>" + RstTable.Rows[k]["DPOtherCharge_HoldingNarration"].ToString() + "</td>";
                            strHtml += "<td align=\"right\">" + RstTable.Rows[k]["holdingCharge"].ToString() + "</td></tr>";

                        }

                    }

                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=3 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


                    }


                }
                else
                {

                    if (qstr == "CDSL")
                    {
                        prevIsin = RstTable.Rows[k]["CDSLISIN_Number"].ToString();
                        prevSett = RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString();
                        prevtrnasType = RstTable.Rows[k]["transactionType"].ToString();

                        strHtml += "<tr style=\"background-color: #FDE9D9;text-align:left\"><td>ISIN</td><td><b>" + RstTable.Rows[k]["CDSLISIN_Number"].ToString() + "</b></td>";
                        strHtml += "<td  colspan=2>Security Name</td><td colspan=2><b>" + RstTable.Rows[k]["CDSLISIN_ShortName"].ToString() + RstTable.Rows[k]["transactionType"].ToString() + "</b></td>";
                        strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() + "</b></td></tr>";

                    }
                    else if (qstr == "NSDL")
                    {
                        prevIsin = RstTable.Rows[k]["NSDLISIN_Number"].ToString();
                        prevSett = RstTable.Rows[k]["NsdlTransaction_SettlementID"].ToString();
                        prevtrnasType = RstTable.Rows[k]["AccountType"].ToString();

                        strHtml += "<tr style=\"background-color: #FDE9D9;text-align:left\"><td>ISIN</td><td><b>" + RstTable.Rows[k]["NSDLISIN_Number"].ToString() + "</b></td>";
                        strHtml += "<td  colspan=2>Security Name</td><td colspan=2><b>" + RstTable.Rows[k]["NsdlISIN_Name"].ToString() + RstTable.Rows[k]["AccountType"].ToString() + "</b></td>";
                        strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k]["NsdlTransaction_SettlementID"].ToString() + "</b></td></tr>";


                    }



                    //strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    //strHtml += "<td colspan=2><b>Particulars</b></td>";
                    //strHtml += "<td align=\"right\"><b>Rate</b></td>";
                    //strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    //strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    //strHtml += "<td align=\"right\"><b>Current Balance</b></td>";
                    //strHtml += "<td align=\"right\"><b>Amount</b></td></tr>";


                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=3 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }



                    flag = flag + 1;

                    if (qstr == "CDSL")
                    {
                        strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                        strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    }
                    else if (qstr == "NSDL")
                    {
                        strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k]["NsdlTransaction_Date"].ToString() + "</td>";
                        strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";

                    }

                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["Rate"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";

                    if (qstr == "CDSL")
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td>";
                    else if (qstr == "NSDL")
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["NsdlTransaction_Quantity"].ToString() + "</td>";

                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["Amount"].ToString() + "</td></tr>";


                    if (qstr == "CDSL")
                    {
                        if (RstTable.Rows[k]["DPOtherCharge_HoldingNarration"] != DBNull.Value)
                        {
                            strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td></td><td colspan=6>" + RstTable.Rows[k]["DPOtherCharge_HoldingNarration"].ToString() + "</td>";
                            strHtml += "<td align=\"right\">" + RstTable.Rows[k]["holdingCharge"].ToString() + "</td></tr>";

                        }
                    }

                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=3 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


                    }

                }

            }



            strHtml += "</table><br/>";

            if (RstTable.Rows.Count == 0)
            {
                strHtml = String.Empty;
            }

            display.InnerHtml = strHtml;

            if (lblTransction.Text == lblTotalTransction.Text)
            {
                strHtml = String.Empty;
                accounts.InnerHtml = bindAccountSummary(strHtml);

                strHtml = String.Empty;
                summary.InnerHtml = bindBillSummary(strHtml);
                tdsummary.Style["display"] = "inline";
            }
            else
            {
                tdsummary.Style["display"] = "none";
            }









            /////////////////////////////////////////////////////////




            ShowHidePreviousNext_of_Transaction();

        }

        private String bindBillSummary(String strHtml)
        {
            DataTable billSummary = new DataTable();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand("cdslBill_SummryDisplayScreen", con);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.AddWithValue("@billNumber", billNumber.Text);


            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(billSummary);
            //}
            billSummary = PeriodicalReports.cdslcdslBill_SummryDisplayScreen(billNumber.Text.Trim());
            strHtml = strHtml + "<table align=\"right\">";
            for (int k = 0; k < billSummary.Rows.Count; k++)
            {

                if (billSummary.Rows[k]["ChargeName"].ToString() == "Taxable Amount:" || billSummary.Rows[k]["ChargeName"].ToString() == "Total Bill Amount:" || billSummary.Rows[k]["ChargeName"].ToString() == "Net Bill Amount:")
                {
                    strHtml += "<tr><td align=\"left\"><u>" + billSummary.Rows[k]["ChargeName"].ToString() + "</u></td><td align=\"right\"><u>" + billSummary.Rows[k]["ChargeAmt"].ToString() + "</u></td></tr>";

                }

                else
                {
                    strHtml += "<tr><td align=\"left\">" + billSummary.Rows[k]["ChargeName"].ToString() + "</td><td align=\"right\">" + billSummary.Rows[k]["ChargeAmt"].ToString() + "</td></tr>";
                }

                if (billSummary.Rows[k]["AmtinWord"] != DBNull.Value)
                {
                    strHtml += "<tr><td colspan=2 style=\"text-aligh:right;color:Maroon \"><b>" + billSummary.Rows[billSummary.Rows.Count - 1]["AmtinWord"].ToString() + "</b></td></tr>";

                }
            }


            strHtml += "</table>";

            return strHtml;

        }

        private String bindAccountSummary(String strHtml)
        {


            String StartDate, EndDate, financilaYear, billnoFinYear, CreditDebit;

            CreditDebit = String.Empty;

            objConverter.getFirstAndLastDate("December", out StartDate, out EndDate, out billnoFinYear);
            //cmbMonth.SelectedItem.Value.ToString()

            DataTable AccountSummary = new DataTable();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand("cdslBillAccountsLedger", con);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.AddWithValue("@startDate", StartDate);
            //    cmd.Parameters.AddWithValue("@endDate", EndDate);
            //    cmd.Parameters.AddWithValue("@dpId", dpId);
            //    cmd.Parameters.AddWithValue("@companyID", CompanyId);
            //    cmd.Parameters.AddWithValue("@SegmentId", SegmentId);

            //    if (qstr == "CDSL")
            //        cmd.Parameters.AddWithValue("@MainAcID", "SYSTM00042");
            //    else if (qstr == "NSDL")
            //        cmd.Parameters.AddWithValue("@MainAcID", "SYSTM00043");

            //    cmd.Parameters.AddWithValue("@SubAccountID", boid.Text);
            //    cmd.Parameters.AddWithValue("@financialYear", HttpContext.Current.Session["LastFinYear"]);

            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(AccountSummary);
            //}

            AccountSummary = PeriodicalReports.cdslcdslBillAccountsLedger(StartDate, EndDate, dpId, CompanyId, SegmentId, qstr, boid.Text.Trim(), HttpContext.Current.Session["LastFinYear"].ToString());

            strHtml = strHtml + "<table width=\"100%\">";

            strHtml = strHtml + "<tr><td align=\"left\" colspan=3><u><b>Accounts Ledger Summary:</b></u></td></tr>";

            strHtml = strHtml + "<tr><td align=\"left\">Opening Balance</td><td align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Rows[0]["openingBalance"].ToString()))) + "</td>";

            if (Convert.ToDouble(AccountSummary.Rows[0]["openingBalance"].ToString()) > 0.00)
            {
                CreditDebit = "Cr";
            }
            else
            {
                CreditDebit = "Dr";
            }

            strHtml = strHtml + "<td>" + CreditDebit + "</td></tr>";

            for (int k = 0; k < AccountSummary.Rows.Count; k++)
            {
                if (AccountSummary.Rows[k]["AccountsLedger_amountCr"] != DBNull.Value)
                {
                    strHtml = strHtml + "<tr><td align=\"left\"> Payment Received Thank you</td><td align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Rows[k]["AccountsLedger_amountCr"].ToString()))) + "</td>";

                    if (Convert.ToDouble(AccountSummary.Rows[k]["AccountsLedger_amountCr"].ToString()) > 0.00)
                    {
                        CreditDebit = "Cr";
                    }
                    else
                    {
                        CreditDebit = "Dr";
                    }

                    strHtml = strHtml + "<td>" + CreditDebit + "</td></tr>";

                    strHtml = strHtml + "<tr><td colspan=3 align=\"left\">[ Cheque No." + AccountSummary.Rows[k]["AccountsLedger_InstrumentNumber"].ToString();
                    strHtml = strHtml + " dt." + AccountSummary.Rows[k]["AccountsLedger_InstrumentDate"].ToString() + "]</td></tr>";


                }
            }

            if (AccountSummary.Rows[0]["Adjustment"] != DBNull.Value)
            {
                strHtml = strHtml + "<tr><td align=\"left\">Adjustment in the period</td><td align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Rows[0]["Adjustment"].ToString()))) + "</td>";

                if (Convert.ToDouble(AccountSummary.Rows[0]["Adjustment"].ToString()) > 0.00)
                {
                    CreditDebit = "Cr";
                }
                else
                {
                    CreditDebit = "Dr";
                }

                strHtml = strHtml + "<td>" + CreditDebit + "</td></tr>";

            }

            if (AccountSummary.Rows[0]["NetBillAmt"] != DBNull.Value)
            {
                strHtml = strHtml + "<tr><td align=\"left\">Current Bill</td><td align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(AccountSummary.Rows[0]["NetBillAmt"].ToString())) + "</td>";

                if (Convert.ToDouble(AccountSummary.Rows[0]["NetBillAmt"].ToString()) < 0.00)
                {
                    CreditDebit = "Cr";
                }
                else
                {
                    CreditDebit = "Dr";
                }

                strHtml = strHtml + "<td>" + CreditDebit + "</td></tr>";

            }


            if (Convert.ToDouble(AccountSummary.Rows[0]["closingBalance"].ToString()) > 0.00)
            {
                strHtml = strHtml + "<tr><td align=\"left\">Net Amount Payable to you:</td><td align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(AccountSummary.Rows[0]["closingBalance"].ToString())) + "</td><td></td></tr>";

            }
            else
            {
                strHtml = strHtml + "<tr><td align=\"left\">Net Amount Receivable from you:</td><td align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Rows[0]["closingBalance"].ToString()))) + "</td><td></td></tr>";

            }

            strHtml = strHtml + "</table>";









            return strHtml;



        }



        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "lavender";
        }

        protected string ZeroCheck(string s)
        {
            if (s == " 0")
                return " ";
            else
                return s;
        }

        void ShowHidePreviousNext_of_Transaction()
        {
            if (Convert.ToInt16(lblTotalTransction.Text) <= 20)
            {
                btnTransFirst.Style["Display"] = "none";
                btnTransPrevious.Style["Display"] = "none";
                btnTransnNext.Style["Display"] = "none";
                btnTransnLast.Style["Display"] = "none";

                btnTransFirst1.Style["Display"] = "none";
                btnTransPrevious1.Style["Display"] = "none";
                btnTransnNext1.Style["Display"] = "none";
                btnTransnLast1.Style["Display"] = "none";


            }
            else
            {
                btnTransFirst.Style["Display"] = "Display";
                btnTransPrevious.Style["Display"] = "Display";
                btnTransnNext.Style["Display"] = "Display";
                btnTransnLast.Style["Display"] = "Display";

                btnTransFirst1.Style["Display"] = "Display";
                btnTransPrevious1.Style["Display"] = "Display";
                btnTransnNext1.Style["Display"] = "Display";
                btnTransnLast1.Style["Display"] = "Display";


            }


            if (Convert.ToInt16(lblTransction.Text) == Convert.ToInt16(lblTotalTransction.Text))
            {
                btnTransFirst.Enabled = true;
                btnTransPrevious.Enabled = true;
                btnTransnNext.Enabled = false;
                btnTransnLast.Enabled = false;

                btnTransFirst1.Enabled = true;
                btnTransPrevious1.Enabled = true;
                btnTransnNext1.Enabled = false;
                btnTransnLast1.Enabled = false;

            }
            else
                if (Convert.ToInt16(lblTransction.Text) <= 20)
                {
                    btnTransFirst.Enabled = false;
                    btnTransPrevious.Enabled = false;
                    btnTransnNext.Enabled = true;
                    btnTransnLast.Enabled = true;

                    btnTransFirst1.Enabled = false;
                    btnTransPrevious1.Enabled = false;
                    btnTransnNext1.Enabled = true;
                    btnTransnLast1.Enabled = true;



                }
                else
                {
                    btnTransFirst.Enabled = true;
                    btnTransPrevious.Enabled = true;
                    btnTransnNext.Enabled = true;
                    btnTransnLast.Enabled = true;

                    btnTransFirst1.Enabled = true;
                    btnTransPrevious1.Enabled = true;
                    btnTransnNext1.Enabled = true;
                    btnTransnLast1.Enabled = true;


                }


        }

        void ShowHidePreviousNext_of_Holders()
        {
            if (LastPage == 0 || LastPage == -1)
            {
                ASPxFirst.Style["Display"] = "none";
                ASPxPrevious.Style["Display"] = "none";
                ASPxNext.Style["Display"] = "none";
                ASPxLast.Style["Display"] = "none";

            }
            else
            {
                ASPxFirst.Style["Display"] = "Display";
                ASPxPrevious.Style["Display"] = "Display";
                ASPxNext.Style["Display"] = "Display";
                ASPxLast.Style["Display"] = "Display";

            }

            if (CurrentPage == LastPage && LastPage != 0)
            {

                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;

                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;

            }
            else
                if (CurrentPage == 0 && LastPage != 0)
                {
                    ASPxFirst.Enabled = false;
                    ASPxPrevious.Enabled = false;

                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;


                }
                else
                {
                    ASPxFirst.Enabled = true;
                    ASPxPrevious.Enabled = true;
                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;
                }
        }

        // End of Display a Particular Client Bill Details




        /// Segmentid, CompanyId, DpId bind


        void bind_CompanyID_SegmentID()
        {
            //string[] yearSplit;

            //string financilaYear = HttpContext.Current.Session["LastFinYear"].ToString(); //HttpContext.Current.Session["LastFinYear"].ToString();




            // yearSplit = financilaYear.Split('-');

            // billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";

            DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
                                                    " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
                                                        " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")");

            CompanyId = lastSegMemt.Rows[0][0].ToString();
            dpId = lastSegMemt.Rows[0][2].ToString();
            SegmentId = lastSegMemt.Rows[0][1].ToString();

        }

        ///End
        ///

        public bool ThumbnailCallback()
        {
            return false;
        }

        private byte[] getLogoImage(string logoPath)
        {
            FileStream fs;
            BinaryReader br;
            String tmpPath = MapPath(@"..\images\thumble_logo.jpg");

            System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(logoPath);

            System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(500, 50, dummyCallBack, IntPtr.Zero);
            thumbNailImg.Save(tmpPath, ImageFormat.MemoryBmp);

            thumbNailImg.Dispose();



            fs = new FileStream(tmpPath, FileMode.Open);
            br = new BinaryReader(fs);
            // define the byte array of filelength 
            byte[] imgbyte = new byte[fs.Length + 1];
            // read the bytes from the binary reader 
            imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
            br.Close();
            // close the binary reader 
            fs.Close();
            // close the file stream 
            File.Delete(tmpPath);

            return imgbyte;
        }

    }
}