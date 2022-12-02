using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_NoTransOnlyHolding : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        string dp, dtFrom, dtTo, duplex;
        DataTable DistinctBillNumber;
        int signStatus, logoStatus, signatureStatus, EmailCreateAppMenuId;
        DataSet ds = new DataSet();
        byte[] logoinByte;
        byte[] SignatureinByte;
        DataRow drow, drow1;
        DataSet logo = new DataSet();
        DataSet signature = new DataSet();
        string result = "";
        string CompanyId, dpId, SegmentId, qstr1;
        DataTable dtexcel = new DataTable();
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        CommonUtility util = new CommonUtility();

        void bindgroupDropDown()
        {
            DataTable dt = oDBEngine.GetDataTable(" tbl_master_groupmaster", " distinct gpm_Type", null);
            cmbBillGroupType.DataSource = dt;
            cmbBillGroupType.TextField = "gpm_Type";
            cmbBillGroupType.ValueField = "gpm_Type";
            cmbBillGroupType.DataBind();
            cmbBillGroupType.Items.Insert(0, new DevExpress.Web.ListEditItem("Select", "NA"));
            cmbBillGroupType.SelectedIndex = 0;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            tr_export.Visible = false;
            dp = Request.QueryString["type"];
            if (dp == "CDSL")
                EmailCreateAppMenuId = 10011;
            else
                EmailCreateAppMenuId = 9011;
            Page.ClientScript.RegisterStartupScript(GetType(), "pl1", "<script language='JavaScript'>PageLoad();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "h", "<script>height();</script>");
            txtdigitalName.Attributes.Add("onkeyup", "CallAjax(this,'SearchEmployeesNameWithDigitalSign',event)");

            if (!IsPostBack)
            {
                string date = String.Format("{0:dd MMMM yyyy}", DateTime.Today);
                txtDateFrom.Text = date;
                txtDateTo.Text = date;
                Session["tables"] = "";
                bindgroupDropDown();
            }
        }
        void bind_CompanyID_SegmentID(out String CompanyId, out String dpId, out String SegmentId)
        {
            string[] yearSplit;

            String financilaYear = HttpContext.Current.Session["LastFinYear"].ToString(); //HttpContext.Current.Session["LastFinYear"].ToString();




            yearSplit = financilaYear.Split('-');

            //  billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";

            DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
                                                    " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
                                                        " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")");

            CompanyId = lastSegMemt.Rows[0][0].ToString();
            dpId = lastSegMemt.Rows[0][2].ToString();
            SegmentId = lastSegMemt.Rows[0][1].ToString();

        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            string digi = txtdigitalName_hidden.Text;
            qstr1 = Request.QueryString["type"];
            dtFrom = txtDateFrom.Date.ToString();
            dtTo = txtDateTo.Date.ToString();
            duplex = "N";
            string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

            tmpPdfPath = string.Empty;
            ReportPath = string.Empty;
            signPath = string.Empty;
            finalResult = string.Empty;
            bind_CompanyID_SegmentID(out CompanyId, out dpId, out SegmentId);

            ds = rep.sp_NoTransOnlyHolding(dtFrom, dtTo, dp, HttpContext.Current.Session["usercontactID"].ToString(), HttpContext.Current.Session["userlastsegment"].ToString(),
                HttpContext.Current.Session["LastCompany"].ToString(), duplex, "BranchID", txtHeader_hidden.Value, txtFooter_hidden.Value,
                cmbBillGenerationOrder.SelectedItem.Value == "G" ? groupid.Value : "", cmbBillGenerationOrder.SelectedItem.Value == "G" ? cmbBillGroupType.SelectedItem.Text.ToString().Trim() : "", ddloutput.SelectedItem.Value);
            if (ds.Tables[1].Rows.Count > 0)
            {
                //if (CheckBox1.Checked == true)
                //{
                signature.Tables.Add();
                signature.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                signature.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
                signature.Tables[0].Columns.Add("signName", System.Type.GetType("System.String"));
                signature.Tables[0].Columns.Add("Status", System.Type.GetType("System.String"));
                drow = signature.Tables[0].NewRow();
                logo.Tables.Add();
                logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                drow1 = logo.Tables[0].NewRow();
                string seglogo = "logo.bmp";
                if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\" + seglogo), out logoinByte) == 1)
                {
                    drow1["Image"] = logoinByte;

                }
                else
                {
                    logoStatus = 0;
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

                }

                logo.Tables[0].Rows.Add(drow1);

                drow["Status"] = "3";
                signature.Tables[0].Rows.Add(drow);
                logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");

                DataSet DigitalSignatureDs = new DataSet();

                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\"));
                }
                tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                // ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NoTransOnlyHoldingformail.rpt");
                signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                if (CheckBox1.Checked == true)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    {

                        SqlCommand cmdDigital = new SqlCommand("cdsl_EmployeeName", con);
                        cmdDigital.CommandType = CommandType.StoredProcedure;
                        cmdDigital.Parameters.AddWithValue("@ID", txtdigitalName_hidden.Text.Trim());
                        cmdDigital.CommandTimeout = 0;
                        SqlDataAdapter daDigital = new SqlDataAdapter();
                        daDigital.SelectCommand = cmdDigital;
                        daDigital.Fill(DigitalSignatureDs);
                    }

                    DigitalSignatureDs.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
                    DigitalSignatureDs.Tables[0].Rows[0]["companyName"] = ds.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);



                    digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();


                    signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + txtdigitalName_hidden.Text.Trim() + ".pfx";
                    //signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                    finalResult = generateindivisualPdf(ds,
                        //Summary, AccountSummary, Holding,
                                       signature, logo,
                                      qstr1, "Yes", digitalSignaturePassword,
                                      DigitalSignatureDs, signPath, ReportPath
                                     , tmpPdfPath, CompanyId, dpId, SegmentId, signPdfPath, VirtualPath,
                                     HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), EmailCreateAppMenuId);
                }
                else
                {
                    finalResult = generateindivisualPdf(ds,
                        //Summary, AccountSummary, Holding,
                                   signature, logo,
                                  qstr1, "Yes", "",
                                  DigitalSignatureDs, "", ReportPath
                                 , tmpPdfPath, CompanyId, dpId, SegmentId, signPdfPath, VirtualPath,
                                 HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), EmailCreateAppMenuId);
                }

                if (finalResult == "Success")
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Successfully Document Generated.');", true);

                }



            }
        }
        public string generateindivisualPdf(DataSet ds,
            //DataSet Summary, DataSet AccountSummary,DataSet Holding,
                                          DataSet signature, DataSet logo,
                                         string module, string DigitalCertified, string digitalSignaturePassword,
                                         DataSet DigitalSignatureDs, string SignPath, string reportPath
                                         , string tmpPDFPath, string CompanyId, string dpId,
                                           string SegmentId, string signPdfPath, string VirtualPath,
                                            string user, string LastFinYear, int EmailCreateAppMenuId)
        {
            result = string.Empty;
            string status;
            status = string.Empty;
            DataTable FilterClients = new DataTable();
            DataTable FilterSummary = new DataTable();
            DataTable FilterAccountSummary = new DataTable();
            DataTable FilterHolding = new DataTable();

            DataView viewClients = new DataView(ds.Tables[1]);
            DataView viewSummary = new DataView(ds.Tables[0]);
            //DataView viewAccountSummary = new DataView(AccountSummary.Tables[0]);
            //DataView viewHolding = new DataView(Holding.Tables[0]);

            if (module == "CDSL")
                DistinctBillNumber = viewClients.ToTable(true, new string[] { "BenAccountNumber" });
            else if (module == "NSDL")
                DistinctBillNumber = viewClients.ToTable(true, new string[] { "BenAccountNumber", });

            foreach (DataRow dr in DistinctBillNumber.Rows)
            {
                if (module == "NSDL")
                {
                    viewClients.RowFilter = "BenAccountNumber = '" + dr["BenAccountNumber"] + "' and NsdlClients_FirstHolderEmail<>'abcdefgh'";
                }
                else
                {
                    viewClients.RowFilter = "BenAccountNumber = '" + dr["BenAccountNumber"] + "' and CdslClients_EmailID<>'abcdefgh'";
                }
                FilterClients = viewClients.ToTable();
                FilterSummary = viewSummary.ToTable();
                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NoTransOnlyHoldingformail.rpt");
                if (FilterClients.Rows.Count >= 1)
                {
                    //viewSummary.RowFilter = "BillNumber = '" + dr["DPBillSummary_BillNumber"] + "' ";
                    //FilterSummary = viewSummary.ToTable();

                    //if (module == "CDSL")
                    //    viewAccountSummary.RowFilter = "benAccountnumber = '" + dr["DPBillSummary_BenAccountNumber"] + "'";
                    //else if (module == "NSDL")
                    //    viewAccountSummary.RowFilter = "benAccountnumber = '" + dr["NsdlClients_BenAccountID"] + "' ";

                    //FilterAccountSummary = viewAccountSummary.ToTable();

                    //viewHolding.RowFilter = "billNumber = '" + dr["DPBillSummary_BillNumber"] + "' ";
                    //FilterHolding = viewHolding.ToTable();

                    //generatePdf(FilterClients, FilterSummary, FilterAccountSummary, FilterHolding,
                    //                      signature.Tables[0], logo.Tables[0], module, DigitalCertified, DigitalSignPassword);

                    status = digitallySignedPDF(FilterClients, FilterSummary,
                                           signature.Tables[0], logo.Tables[0], module,
                                            reportPath, tmpPDFPath,
                                            SignPath, digitalSignaturePassword,
                                            CompanyId, dpId, SegmentId, DigitalSignatureDs, reportPath,
                                            VirtualPath, signPdfPath, user, LastFinYear, EmailCreateAppMenuId);


                    FilterClients.Reset();
                    FilterSummary.Reset();
                    FilterAccountSummary.Reset();
                    FilterHolding.Reset();
                    //}
                    if (status != "Success")
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
                        break;
                    }
                }
            }

            return status;
            //cdslTransctionReportDocu = null;
            //GC.Collect();

        }
        string digitallySignedPDF(DataTable Clients, DataTable summary,
                                    DataTable signature, DataTable logo, string module
                                    , string Reportpath, string tmpPDFPath, string signPath
                                       , string DigitalSignPassword, string CompanyId
                                       , string dpId, string SegmentId, DataSet DigitalSignatureDs,
                                   string reportPath, string VirtualPath,
                                   string signPdfPath, string user, string LastFinYear,
                                   int EmailCreateAppMenuId)
        {

            string path;
            path = string.Empty;
            result = string.Empty;
            ReportDocument cdslTransctionReportDocu = new ReportDocument();
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            cdslTransctionReportDocu.Load(reportPath);
            cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);



            cdslTransctionReportDocu.SetDataSource(Clients);
            cdslTransctionReportDocu.Subreports["logo1"].SetDataSource(logo);
            cdslTransctionReportDocu.Subreports["header"].SetDataSource(summary);
            if (ddloutput.SelectedItem.Value == "1")
                cdslTransctionReportDocu.SetParameterValue("@param", "****No Transaction Recorded For The Given Period****");
            else
                cdslTransctionReportDocu.SetParameterValue("@param", "****No Transaction and No Holding Recorded For The Given Period****");


            //if (module == "CDSL")
            //    cdslTransctionReportDocu.Subreports["digitalSignature"].SetDataSource(DigitalSignatureDs);
            //else 
            //if (module == "NSDL")
            //    cdslTransctionReportDocu.Subreports["DigitalSignature"].SetDataSource(DigitalSignatureDs);

            string pdfstr = tmpPDFPath + "--" + dp.ToString().Trim() + "-" + dpId.ToString() + Clients.Rows[0]["BenAccountNumber"].ToString() + ".pdf";


            cdslTransctionReportDocu.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);
            if (module == "CDSL")
            {
                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Quaterly No Transaction Statement'", 1);
                if (DigitalSignPassword.Length > 0)
                {
                    result = util.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
                                    DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "5",
                                    DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                    Clients.Rows[0]["CdslClients_EmailID"].ToString(), dtTo.ToString(),
                                    DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                    VirtualPath, signPdfPath, user, LastFinYear, EmailCreateAppMenuId);
                }
                else
                {
                    result = util.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
                                    "", CompanyId, dpId, "5",
                                   "",
                                    Clients.Rows[0]["CdslClients_EmailID"].ToString(), dtTo.ToString(),
                                    "",
                                    VirtualPath, signPdfPath, user, LastFinYear, EmailCreateAppMenuId);
                }
            }
            else if (module == "NSDL")
            {
                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Quaterly No Transaction Statement'", 1);
                //result = objConverter.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
                //                   DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(),
                //                   CompanyId, dpId, "5", DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                //                   Clients.Rows[0]["NsdlClients_FirstHolderEmail"].ToString(), dtTo.ToString(),
                //                   DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                //                   VirtualPath, signPdfPath, user, LastFinYear, Convert.ToInt32(str[0]));

                if (DigitalSignPassword.Length > 0)
                {
                    result = util.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
                                    DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "5",
                                    DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                    Clients.Rows[0]["NsdlClients_FirstHolderEmail"].ToString(), dtTo.ToString(),
                                    DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                    VirtualPath, signPdfPath, user, LastFinYear, EmailCreateAppMenuId);
                }
                else
                {
                    result = util.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
                                    "", CompanyId, dpId, "5",
                                   "",
                                    Clients.Rows[0]["NsdlClients_FirstHolderEmail"].ToString(), dtTo.ToString(),
                                    "",
                                    VirtualPath, signPdfPath, user, LastFinYear, EmailCreateAppMenuId);
                }
            }

            cdslTransctionReportDocu.Dispose();
            cdslTransctionReportDocu = null;
            GC.Collect();


            return result;
        }

        protected void btnscreen_Click(object sender, EventArgs e)
        {
            tr_export.Visible = true;
            dtFrom = txtDateFrom.Date.ToString();
            dtTo = txtDateTo.Date.ToString();

            ds = rep.sp_NoTransOnlyHolding(dtFrom, dtTo, dp, HttpContext.Current.Session["usercontactID"].ToString(), HttpContext.Current.Session["userlastsegment"].ToString(),
               HttpContext.Current.Session["LastCompany"].ToString(), duplex, "BranchID", txtHeader_hidden.Value, txtFooter_hidden.Value,
               cmbBillGenerationOrder.SelectedItem.Value == "G" ? groupid.Value : "", cmbBillGenerationOrder.SelectedItem.Value == "G" ? cmbBillGroupType.SelectedItem.Text.ToString().Trim() : "", ddloutput.SelectedItem.Value);

            string status;
            status = string.Empty;
            DataTable FilterClients = new DataTable();
            DataTable FilterSummary = new DataTable();
            DataTable FilterAccountSummary = new DataTable();
            DataTable FilterHolding = new DataTable();

            DataView viewClients = new DataView(ds.Tables[1]);
            DataView viewSummary = new DataView(ds.Tables[0]);
            DistinctBillNumber = viewClients.ToTable(true, new string[] { "BenAccountNumber", "name1", "Address1", "PinNo", "PhoneNo" });
            Session["tables"] = "";
            Session["tables"] = DistinctBillNumber;
            GridReminder.DataSource = DistinctBillNumber.DefaultView;
            GridReminder.DataBind();


        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtbind = (DataTable)HttpContext.Current.Session["tables"];
            GridReminder.DataSource = dtbind.DefaultView;
            GridReminder.DataBind();
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

        protected void GridReminder_PageIndexChanging(object sender, EventArgs e)
        {
            DataTable dtsession = (DataTable)HttpContext.Current.Session["tables"];
            GridReminder.DataSource = dtsession.DefaultView;
            GridReminder.DataBind();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Session["tables"] = "";
            dtFrom = txtDateFrom.Date.ToString();
            dtTo = txtDateTo.Date.ToString();

            if (chbDuplex.Checked)
                duplex = "Y";
            else
                duplex = "N";
            string printtest = cmbBillGroupType.SelectedItem.Text.ToString().Trim();

            ds = rep.sp_NoTransOnlyHolding(dtFrom, dtTo, dp, HttpContext.Current.Session["usercontactID"].ToString(), HttpContext.Current.Session["userlastsegment"].ToString(),
                HttpContext.Current.Session["LastCompany"].ToString(), duplex, "BranchID", txtHeader_hidden.Value, txtFooter_hidden.Value,
                cmbBillGenerationOrder.SelectedItem.Value == "G" ? groupid.Value : "", cmbBillGenerationOrder.SelectedItem.Value == "G" ? cmbBillGroupType.SelectedItem.Text.ToString().Trim() : "", ddloutput.SelectedItem.Value);

            ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NoTransOnlyHolding.xsd");

            //////////////////////////////////////// 

            DataSet logo = new DataSet();
            DataSet signature = new DataSet();



            ////---- Logo ----////



            logo.Tables.Add();
            logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            drow = logo.Tables[0].NewRow();

            Converter objConverter = new Converter();
            if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) == 1)
            {
                drow["Image"] = logoinByte;
                logo.Tables[0].Rows.Add(drow);
                //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");

            }
            else
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                return;
            }

            ////----- Signature -----/////

            if (chkSignature.Checked)
                signStatus = 1;
            else
                signStatus = 0;


            signature.Tables.Add();
            signature.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            signature.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
            signature.Tables[0].Columns.Add("signName", System.Type.GetType("System.String"));
            signature.Tables[0].Columns.Add("status", System.Type.GetType("System.String"));

            drow1 = signature.Tables[0].NewRow();


            if (signStatus == 1)
            {



                if (objConverter.getSignatureImage(txtEmpName_hidden.Text.Trim(), out SignatureinByte, dp) == 1)
                {

                    drow1["Image"] = SignatureinByte;
                    drow1["companyName"] = ds.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
                    drow1["signName"] = txtEmpName.Text.Split('[').GetValue(0);
                    // txtemp.text.tostring().trim();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "sign", "alert('Signature not Found.');", true);
                    return;
                }
            }

            drow1["status"] = signStatus;

            signature.Tables[0].Rows.Add(drow1);
            //signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\sign.xsd");


            /////////////////////////////////////////

            if (ds.Tables[1].Rows.Count > 0)
            {
                ReportDocument rptDoc = new ReportDocument();
                string rptPath = HttpContext.Current.Server.MapPath("..\\Reports\\NoTransOnlyHolding.rpt");
                //string rptPath = HttpContext.Current.Server.MapPath("..\\Reports\\logo_test.rpt");
                string pdfName = "Qtr_NoTransactionStmt_" + dp;

                rptDoc.Load(rptPath);
                rptDoc.SetDataSource(ds);

                rptDoc.Subreports["Logo"].SetDataSource(logo);
                rptDoc.Subreports["Signature"].SetDataSource(signature);
                if (ddloutput.SelectedItem.Value == "1")
                    rptDoc.SetParameterValue("@param", "****No Transaction Recorded For The Given Period****");
                else
                    rptDoc.SetParameterValue("@param", "****No Transaction and No Holding Recorded For The Given Period****");
                rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, pdfName);

                rptDoc.Dispose();
                rptDoc = null;
                GC.Collect();
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "norecord", "<script>alert('No Record Found');</script>");
        }

    }
}