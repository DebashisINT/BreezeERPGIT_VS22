using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_dailymrgnrprttoclient : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        CommonUtility util = new CommonUtility();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ReportDocument ICEXReportDocument = new ReportDocument();
        GenericMethod oGenericMethod = new GenericMethod();
        //DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataSet DigitalSignatureDs = new DataSet();
        DataTable forexcelexport = new DataTable();
        DataTable DistinctBillNumber;
        DataTable Distinctclient;
        string data;
        protected string allcontract, ecnenable, deliveryrpt, remaining;
        int pageindex = 0;
        #endregion

        #region Property
        public string ecnenable_excel
        {
            get { return (string)Session["ecnenable_excel"]; }
            set { Session["ecnenable_excel"] = value; }
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

        public string deliver_excel
        {
            get { return (string)Session["deliver_excel"]; }
            set { Session["deliver_excel"] = value; }
        }

        public string ecnenable_excelcc
        {
            get { return (string)Session["ecnenable_excelcc"]; }
            set { Session["ecnenable_excelcc"] = value; }
        }

        public string remaining_excelcc
        {
            get { return (string)Session["remaining_excelcc"]; }
            set { Session["remaining_excelcc"] = value; }
        }

        public string allcontract_excelcc
        {
            get { return (string)Session["allcontract_excelcc"]; }
            set { Session["allcontract_excelcc"] = value; }
        }

        public string deliver_excelcc
        {
            get { return (string)Session["deliver_excelcc"]; }
            set { Session["deliver_excelcc"] = value; }
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
        #endregion

        #region Page Methods
        protected void page_preinit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] param = { null };
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, param);
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_Page, param);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                BindAjaxList(oGenericMethod.GetDigitalSignature(), txtdigitalName);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                date();
                fn_Segment();
                txtdigitalName.Visible = IsSignExists();
                if (txtdigitalName.Visible == true)
                {
                    td_msg.Visible = false;

                }
                else
                {
                    td_msg.Visible = true;
                }
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

            //___________-end here___//



        }
        void BindAjaxList(String CombinedQuery, TextBox TxtBoxName)
        {
            //CombinedQuery = CombinedQuery.Replace("'", "\\'");
            TxtBoxName.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedQuery + "')");
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0] != "Clients")
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
                else
                {

                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {

                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {

                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }

            }
            if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }
            else if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                CurrentPage = 0;
                ddlbandforCLIENT();
            }

        }
        #endregion

        #region GridView Paging
        protected void ASPxFirst_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = 0;
            bind_Details();
        }

        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage - 1;
                bind_Details();
            }
        }

        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage + 1;
                bind_Details();
            }
        }

        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = LastPage;
            bind_Details();
        }
        #endregion

        #region PDF
        protected void btnprint_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                byte[] logoinByte;
                ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                if (CHKLOGOPRINT.Checked == true)
                {
                    if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["Image"] = logoinByte;
                        }
                    }
                }
                if (CHKDETAIL.Checked == true)
                {
                    if (ds.Tables.Count > 2)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            if (ds.Tables[2].Rows[i]["Quantity"] != DBNull.Value)
                                ds.Tables[2].Rows[i]["Quantity"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Quantity"])));
                            if (ds.Tables[2].Rows[i]["closeprice"] != DBNull.Value)
                                ds.Tables[2].Rows[i]["closeprice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["closeprice"])));
                            if (ds.Tables[2].Rows[i]["varmargin"] != DBNull.Value)
                                ds.Tables[2].Rows[i]["varmargin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["varmargin"])));
                            if (ds.Tables[2].Rows[i]["Stocksresult"] != DBNull.Value)
                                ds.Tables[2].Rows[i]["Stocksresult"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Stocksresult"])));
                            if (ds.Tables[2].Rows[i]["SUMStocksresult"] != DBNull.Value)
                                ds.Tables[2].Rows[i]["SUMStocksresult"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["SUMStocksresult"])));
                        }
                    }
                }
                ReportDocument report = new ReportDocument();
                ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//dailymarginrpttoclient.xsd");
                ds.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//dailyrpttoclient1.xsd");
                ds.Tables[2].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//dailyrpttoclient2.xsd");


                string tmpPdfPath = string.Empty;
                tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\dailymarginrpttoclient.rpt");
                report.Load(tmpPdfPath);
                report.SetDataSource(ds.Tables[0]);
                report.Subreports["combined"].SetDataSource(ds.Tables[1]);
                report.Subreports["Securities"].SetDataSource(ds.Tables[2]);
                report.VerifyDatabase();
                report.SetParameterValue("@Field", (object)ds.Tables[0].Columns[7].ColumnName.ToString());
                report.SetParameterValue("@Field1", (object)ds.Tables[0].Columns[8].ColumnName.ToString());
                report.SetParameterValue("@Field2", (object)ds.Tables[0].Columns[9].ColumnName.ToString());


                report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Daily Margin Reporting To Clients");

                report.Dispose();
                GC.Collect();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "H", "NORECORD();", true);

            }
        }

        public string generateindivisualPdf(ReportDocument ICEXReportDocument, DataSet dsCrystal,
                                          string DigitalCertified, string digitalSignaturePassword,
                                         string SignPath, string reportPath
                                         , string tmpPDFPath, string CompanyId,
                                            string signPdfPath, string VirtualPath,
                                            string user, string LastFinYear, string EmpName)
        {
            string status;
            status = string.Empty;
            string TokenType = "";
            string DigitalSignatureID = "";
            int GeneratedPDFCount = 0;
            DataTable FilterClients = new DataTable();
            DataTable FilterSummary = new DataTable();
            DataTable FilterAccountSummary = new DataTable();
            DataTable FilterHolding = new DataTable();
            DataTable dtsegmentname = oDbEngine.GetDataTable("tbl_master_companyExchange", "distinct CASE WHEN EXCH_EXCHID='EXN0000002' AND exch_segmentId='CM' THEN 'NSE - CM' WHEN EXCH_EXCHID='EXB0000001' AND exch_segmentId='CM' THEN 'BSE - CM' WHEN EXCH_EXCHID='EXN0000002' AND exch_segmentId='FO' THEN 'NSE - FO' WHEN EXCH_EXCHID='EXN0000002' AND exch_segmentId='CDX' THEN 'NSE - CDX' WHEN EXCH_EXCHID='EXB0000001' AND exch_segmentId='CDX' THEN 'BSE - CDX' WHEN EXCH_EXCHID='EXB0000001' AND exch_segmentId='FO' THEN 'BSE - FO' WHEN EXCH_EXCHID='EXM0000001' AND exch_segmentId='COMM' THEN 'MCX - COMM' WHEN EXCH_EXCHID='EXM0000002' AND exch_segmentId='CDX' THEN 'MCXSX - CDX' WHEN EXCH_EXCHID='EXN0000004' AND exch_segmentId='COMM' THEN 'NCDEX - COMM' WHEN EXCH_EXCHID='EXD0000001' AND exch_segmentId='COMM' THEN 'DGCX - COMM' WHEN EXCH_EXCHID='EXN0000005' AND exch_segmentId='COMM' THEN 'NMCE - COMM' WHEN EXCH_EXCHID='EXI0000001' AND exch_segmentId='COMM' THEN 'ICEX - COMM' WHEN EXCH_EXCHID='EXU0000001' AND exch_segmentId='CDX' THEN 'USE - CDX' WHEN EXCH_EXCHID='EXN0000006' AND exch_segmentId='SPOT' THEN 'NSEL - SPOT' WHEN EXCH_EXCHID='EXC0000001' AND exch_segmentId='CM' THEN 'CSE - CM' WHEN EXCH_EXCHID='EXA0000002' AND exch_segmentId='COMM' THEN 'ACE - COMM' WHEN EXCH_EXCHID='EXI0000002' AND exch_segmentId='COMM' THEN 'INMX - COMM' ELSE NULL END AS SEGMENT", "EXCH_INTERNALID='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'");

            DataView viewClients = new DataView(dsCrystal.Tables[0]);
            DistinctBillNumber = viewClients.ToTable(true, new string[] { "CLIENTID" });
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            string pdfstr = String.Empty;
            if (EmpName != "")
            {
                TokenType = EmpName.Split('*')[0];
                DigitalSignatureID = null;

                if (TokenType == "E")
                {
                    DigitalSignatureID = EmpName.Split('*')[1].Split('~')[0].ToString();
                    EmpName = EmpName.Split('*')[1].Split('~')[1].Split('[')[0].Trim();
                }
            }

            foreach (DataRow dr in DistinctBillNumber.Rows)
            {
                viewClients.RowFilter = "CLIENTID = '" + dr["CLIENTID"] + "' AND (clientemail is not null OR clientemail<>'')";// AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P')";
                FilterClients = viewClients.ToTable();

                if (FilterClients.Rows.Count > 0)
                {
                    ReportDocument ICEXReportDocument1 = new ReportDocument();
                    reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\dailymarginrpttoclientmail.rpt");
                    string module = dtsegmentname.Rows[0][0].ToString();

                    if (TokenType != "E")
                    {
                        pdfstr = tmpPDFPath + module + "-" + System.DateTime.Now.ToString("ddMMyyyy") + "-" +
                            FilterClients.Rows[0]["UCC"].ToString() + "-" + FilterClients.Rows[0]["CLIENTID"].ToString() +
                            ".pdf";
                    }
                    else
                    {
                        pdfstr = tmpPDFPath + "DailyMargin" + "-" + System.DateTime.Now.ToString("ddMMMyyyy") + "-" +
                            FilterClients.Rows[0]["UCC"].ToString() + "-" + Convert.ToString(Session["ExchangeSegmentID"]) + "-" + FilterClients.Rows[0]["CLIENTID"].ToString() + "-" +
                            Convert.ToString(Session["UserSegID"]) + "-" + Convert.ToString(Session["UserID"]) + "-" +
                            Convert.ToString(Session["LastFinYear"]).Replace("-", "") + "-" + DigitalSignatureID + ".pdf";
                    }

                    signPdfPath = oconverter.DirectoryPath(out VirtualPath);

                    ICEXReportDocument1.Load(reportPath);
                    ICEXReportDocument1.SetDataSource(FilterClients);
                    ICEXReportDocument1.Subreports["combined"].SetDataSource(dsCrystal.Tables[1]);
                    ICEXReportDocument1.Subreports["Securities"].SetDataSource(dsCrystal.Tables[2]);
                    ICEXReportDocument1.VerifyDatabase();
                    ICEXReportDocument1.SetParameterValue("@Field", (object)dsCrystal.Tables[0].Columns[7].ColumnName.ToString());
                    ICEXReportDocument1.SetParameterValue("@Field1", (object)dsCrystal.Tables[0].Columns[8].ColumnName.ToString());
                    ICEXReportDocument1.SetParameterValue("@Field2", (object)dsCrystal.Tables[0].Columns[9].ColumnName.ToString());
                    ICEXReportDocument1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);
                    ICEXReportDocument1.Dispose();
                    if (TokenType != "E")
                    {
                        string[] str = oDbEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Daily Margin Reporting To Clients'", 1);
                        status = util.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                              DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, Session["usersegid"].ToString(), "99",
                                              DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                              FilterClients.Rows[0]["clientemail"].ToString(), Convert.ToDateTime(dtfor.Value).ToString("dd-MMM-yyyy"),
                                              DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                              VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                        if (status != "Success")
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
                            break;
                        }
                    }
                    else
                    {
                        GeneratedPDFCount = GeneratedPDFCount + 1;
                        status = Convert.ToString(GeneratedPDFCount);
                    }
                }
            }
            return status;
        }
        #endregion

        #region Mail
        protected void btnmailsend_Click(object sender, EventArgs e)
        {
            string chk = "ok";
            for (int i = 0; i < cmbclient.Items.Count; i++)
            {
                htmltable(cmbclient.Items[i].Value);
                if (oDbEngine.SendReportSt(ViewState["email"].ToString(), cmbclient.Items[i].Value, oconverter.ArrangeDate2(dtfor.Value.ToString()), "Daily Margin Reporting To Clients  " + oconverter.ArrangeDate2(dtfor.Value.ToString())) == true)
                {
                    chk = "ok";
                }
                else
                {
                    chk = "no";
                }
            }
            if (chk.ToString().Trim() == "ok")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "H", "MAILSEND(1);", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "H", "MAILSEND(3);", true);

            }
        }

        // Newly Develop for mail sent through attachment/////////
        protected void btnemail_Click(object sender, EventArgs e)
        {
            if (txtdigitalName_hidden.Text.Trim().Length > 0)
            {
                procedure();

                DataSet dsCrystal = (DataSet)ViewState["dataset"];
                if (dsCrystal.Tables.Count > 0)
                {
                    if (dsCrystal.Tables[0].Rows.Count > 0)
                    {
                        if (CHKDETAIL.Checked == true)
                        {
                            if (dsCrystal.Tables.Count > 2)
                            {
                                for (int i = 0; i < dsCrystal.Tables[2].Rows.Count; i++)
                                {
                                    if (dsCrystal.Tables[2].Rows[i]["Quantity"] != DBNull.Value)
                                        dsCrystal.Tables[2].Rows[i]["Quantity"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Quantity"])));
                                    if (dsCrystal.Tables[2].Rows[i]["closeprice"] != DBNull.Value)
                                        dsCrystal.Tables[2].Rows[i]["closeprice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["closeprice"])));
                                    if (dsCrystal.Tables[2].Rows[i]["varmargin"] != DBNull.Value)
                                        dsCrystal.Tables[2].Rows[i]["varmargin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["varmargin"])));
                                    if (dsCrystal.Tables[2].Rows[i]["Stocksresult"] != DBNull.Value)
                                        dsCrystal.Tables[2].Rows[i]["Stocksresult"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Stocksresult"])));
                                    if (dsCrystal.Tables[2].Rows[i]["SUMStocksresult"] != DBNull.Value)
                                        dsCrystal.Tables[2].Rows[i]["SUMStocksresult"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["SUMStocksresult"])));
                                }
                            }
                        }
                        byte[] logoinByte;

                        dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                        //DataTable dtcmp = oDbEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
                        //if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo_" + dtcmp.Rows[0][0].ToString() + ".bmp"), out logoinByte) != 1)
                        if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
                        {
                            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Present');", true);

                        }
                        else
                        {

                            for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                            {
                                dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                            }
                        }
                        //using (SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                        //{

                        //    SqlCommand cmdDigital = new SqlCommand("cdsl_EmployeeName", con1);
                        //    cmdDigital.CommandType = CommandType.StoredProcedure;
                        //    cmdDigital.Parameters.AddWithValue("@ID", txtdigitalName_hidden.Text.Trim());
                        //    cmdDigital.CommandTimeout = 0;
                        //    SqlDataAdapter daDigital = new SqlDataAdapter();
                        //    daDigital.SelectCommand = cmdDigital;
                        //    daDigital.Fill(DigitalSignatureDs);
                        //}

                        DigitalSignatureDs = dailyrep.cdsl_EmployeeName(txtdigitalName_hidden.Text.Trim());

                        dsCrystal.Tables[0].Columns.Add("branch", System.Type.GetType("System.String"));
                        dsCrystal.Tables[0].Columns.Add("signname", System.Type.GetType("System.String"));
                        dsCrystal.Tables[0].Columns.Add("internalid", System.Type.GetType("System.String"));
                        dsCrystal.Tables[0].Columns.Add("branchid", System.Type.GetType("System.String"));
                        dsCrystal.Tables[0].Columns.Add("password", System.Type.GetType("System.String"));
                        dsCrystal.Tables[0].Rows[0]["branch"] = DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString();
                        dsCrystal.Tables[0].Rows[0]["signname"] = DigitalSignatureDs.Tables[0].Rows[0]["signName"].ToString();
                        dsCrystal.Tables[0].Rows[0]["internalid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString();
                        dsCrystal.Tables[0].Rows[0]["branchid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString();
                        dsCrystal.Tables[0].Rows[0]["password"] = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();
                        string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

                        tmpPdfPath = string.Empty;
                        ReportPath = string.Empty;
                        signPath = string.Empty;
                        finalResult = string.Empty;

                        digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();
                        tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                        signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + txtdigitalName_hidden.Text.Trim() + ".pfx";
                        signPdfPath = oconverter.DirectoryPath(out VirtualPath);
                        finalResult = generateindivisualPdf(ICEXReportDocument, dsCrystal, "Yes", digitalSignaturePassword,
                                           signPath, ReportPath
                                         , tmpPdfPath, Session["LastCompany"].ToString(), signPdfPath, VirtualPath,
                                         HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), "");
                        if (finalResult == "Success")
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript24", "<script language='javascript'>Page_Load();</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "mailsendwithreload(1);", true);



                        }
                        if (finalResult == "")
                        {

                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript22", "<script language='javascript'>Page_Load();</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "mailsendwithreload(3);", true);


                        }

                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript299", "<script language='javascript'>Reload();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript2277", "<script language='javascript'>alert('Please Select Signature !!');</script>");
            }
        }
        #endregion

        #region Excel
        protected void Cexcelexportpanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Cexcelexportpanel.JSProperties["cpallcontract"] = null;
            Cexcelexportpanel.JSProperties["cpecnenable"] = null;
            Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = null;
            Cexcelexportpanel.JSProperties["cpremaining"] = null;
            Cexcelexportpanel.JSProperties["cpallcontractpop"] = null;
            Cexcelexportpanel.JSProperties["cpecnenablepop"] = null;
            Cexcelexportpanel.JSProperties["cpproperties"] = null;
            Cexcelexportpanel.JSProperties["cpvisibletrue"] = null;
            Cexcelexportpanel.JSProperties["cpexport"] = null;
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "v4")
            {
                fn_Segment();
                fn_Client();

                ds = dailyrep.DailyMarginReportToClient(dtfor.Value.ToString(), HiddenField_Client.Value.ToString(), HiddenField_Segment.Value.ToString(),
                    ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(),
                    Session["LastCompany"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), CHKAPPMRGN.Checked ? "CHK" : "UNCHK",
                    CHKDETAIL.Checked ? "CHK" : "UNCHK", ((rbPrint.Checked) || (rdbmail.Checked)) ? "CHK" : "UNCHK", rdbVarmarginElm.Checked ? "AppMrgn" : "VarMrgn");

                if (ds.Tables.Count > 0)
                {

                    allcontract = ds.Tables[3].Rows[0][0].ToString();
                    ecnenable = ds.Tables[4].Rows[0][0].ToString();
                    deliveryrpt = ds.Tables[5].Rows[0][0].ToString();
                    remaining = ds.Tables[6].Rows[0][0].ToString();
                    ecnenable_excel = ds.Tables[7].Rows[0][0].ToString();
                    remaining_excel = ds.Tables[8].Rows[0][0].ToString();
                    allcontract_excel = ds.Tables[9].Rows[0][0].ToString();//allcontract.ToString();
                    deliver_excel = ds.Tables[10].Rows[0][0].ToString();//deliveryrpt.ToString();

                    ecnenable_excelcc = ecnenable;
                    allcontract_excelcc = allcontract;
                    remaining_excelcc = remaining;
                    deliver_excelcc = deliveryrpt;

                    if (WhichCall == "v4")
                    {
                        Cexcelexportpanel.JSProperties["cpallcontract"] = allcontract;
                        Cexcelexportpanel.JSProperties["cpecnenable"] = ecnenable;
                        Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = deliveryrpt;
                    }
                    //if (WhichCall == "v5")
                    //{
                    //    Cexcelexportpanel.JSProperties["cpallcontractpop"] = deliveryrpt;
                    //    Cexcelexportpanel.JSProperties["cpecnenablepop"] = remaining;
                    //}
                }
            }


            //allcontract = ds.Tables[3].Rows[0][0].ToString();
            //ecnenable = ds.Tables[4].Rows[0][0].ToString();
            //deliveryrpt = ds.Tables[5].Rows[0][0].ToString();
            //remaining = ds.Tables[6].Rows[0][0].ToString();
            //ecnenable_excel = ds.Tables[7].Rows[0][0].ToString();
            //remaining_excel = ds.Tables[8].Rows[0][0].ToString();

            if (WhichCall == "v1")
                Cexcelexportpanel.JSProperties["cpproperties"] = "Exportall";
            if (WhichCall == "v2")
                Cexcelexportpanel.JSProperties["cpproperties"] = "Export";
            if (WhichCall == "v3")
                Cexcelexportpanel.JSProperties["cpproperties"] = "Exportdelivery";
            //if (WhichCall == "v4")
            //{
            //    Cexcelexportpanel.JSProperties["cpallcontract"] = allcontract;
            //    Cexcelexportpanel.JSProperties["cpecnenable"] = ecnenable;
            //    Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = deliveryrpt;
            //}
            if (WhichCall == "v5")
            {
                if (remaining_excel == "")
                    remaining_excel = "0";
                if (deliver_excel == "")
                    deliver_excel = "0";
                Cexcelexportpanel.JSProperties["cpallcontractpop"] = deliver_excelcc.ToString();
                Cexcelexportpanel.JSProperties["cpecnenablepop"] = remaining_excelcc.ToString();
                //Cexcelexportpanel.JSProperties["cpallcontract"] = allcontract;
                //Cexcelexportpanel.JSProperties["cpecnenable"] = ecnenable;
                //Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = deliveryrpt;
            }

            txtdigitalName.Visible = IsSignExists();
            if (txtdigitalName.Visible == true)
            {
                Cexcelexportpanel.JSProperties["cpvisibletrue"] = "no";



            }
            else
            {

                Cexcelexportpanel.JSProperties["cpvisibletrue"] = "yes";

            }
        }

        protected void CbpSuggestISIN_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Cexcelexportpanel.JSProperties["cpallcontract"] = null;
            Cexcelexportpanel.JSProperties["cpecnenable"] = null;
            Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = null;
            Cexcelexportpanel.JSProperties["cpremaining"] = null;
            Cexcelexportpanel.JSProperties["cpallcontractpop"] = null;
            Cexcelexportpanel.JSProperties["cpecnenablepop"] = null;
            Cexcelexportpanel.JSProperties["cpproperties"] = null;
            Cexcelexportpanel.JSProperties["cpvisibletrue"] = null;
            CbpSuggestISIN.JSProperties["cpallcontractpops"] = null;
            CbpSuggestISIN.JSProperties["cpecnenablepops"] = null;
            DataSet dsCrystal = new DataSet();
            fn_Segment();
            string WhichCall = e.Parameter.Split('~')[0];
            string client = "";
            if ((WhichCall == "all") || (WhichCall == "GeneratePDF"))
                client = ecnenable_excel.ToString();
            else
                client = remaining_excel.ToString();

            dsCrystal = dailyrep.DailyMarginReportToClient(dtfor.Value.ToString(), HiddenField_Client.Value.ToString(), HiddenField_Segment.Value.ToString(),
                    ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(),
                    Session["LastCompany"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), CHKAPPMRGN.Checked ? "CHK" : "UNCHK",
                    CHKDETAIL.Checked ? "CHK" : "UNCHK", ((rbPrint.Checked) || (rdbmail.Checked)) ? "CHK" : "UNCHK", rdbVarmarginElm.Checked ? "AppMrgn" : "VarMrgn");


            if (dsCrystal.Tables.Count > 0)
            {
                if (dsCrystal.Tables[0].Rows.Count > 0)
                {
                    if (CHKDETAIL.Checked == true)
                    {
                        if (dsCrystal.Tables.Count > 2)
                        {
                            for (int i = 0; i < dsCrystal.Tables[2].Rows.Count; i++)
                            {
                                if (dsCrystal.Tables[2].Rows[i]["Quantity"] != DBNull.Value)
                                    dsCrystal.Tables[2].Rows[i]["Quantity"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(dsCrystal.Tables[2].Rows[i]["Quantity"])));
                                if (dsCrystal.Tables[2].Rows[i]["closeprice"] != DBNull.Value)
                                    dsCrystal.Tables[2].Rows[i]["closeprice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(dsCrystal.Tables[2].Rows[i]["closeprice"])));
                                if (dsCrystal.Tables[2].Rows[i]["varmargin"] != DBNull.Value)
                                    dsCrystal.Tables[2].Rows[i]["varmargin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(dsCrystal.Tables[2].Rows[i]["varmargin"])));
                                if (dsCrystal.Tables[2].Rows[i]["Stocksresult"] != DBNull.Value)
                                    dsCrystal.Tables[2].Rows[i]["Stocksresult"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(dsCrystal.Tables[2].Rows[i]["Stocksresult"])));
                                if (dsCrystal.Tables[2].Rows[i]["SUMStocksresult"] != DBNull.Value)
                                    dsCrystal.Tables[2].Rows[i]["SUMStocksresult"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(dsCrystal.Tables[2].Rows[i]["SUMStocksresult"])));
                            }
                        }
                    }
                    byte[] logoinByte;

                    dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                    //DataTable dtcmp = oDbEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
                    //if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo_" + dtcmp.Rows[0][0].ToString() + ".bmp"), out logoinByte) != 1)
                    if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Present');", true);

                    }
                    else
                    {

                        for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                        {
                            dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                        }
                    }

                    DigitalSignatureDs = dailyrep.cdsl_EmployeeName(txtdigitalName_hidden.Text.Split('~')[0].ToString());
                    dsCrystal.Tables[0].Columns.Add("branch", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("signname", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("internalid", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("branchid", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Columns.Add("password", System.Type.GetType("System.String"));
                    dsCrystal.Tables[0].Rows[0]["branch"] = DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString();
                    dsCrystal.Tables[0].Rows[0]["signname"] = DigitalSignatureDs.Tables[0].Rows[0]["signName"].ToString();
                    dsCrystal.Tables[0].Rows[0]["internalid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString();
                    dsCrystal.Tables[0].Rows[0]["branchid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString();
                    dsCrystal.Tables[0].Rows[0]["password"] = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();
                    string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

                    tmpPdfPath = string.Empty;
                    ReportPath = string.Empty;
                    signPath = string.Empty;
                    finalResult = string.Empty;


                    digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();
                    tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                    //signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + txtdigitalName_hidden.Text.Trim() + ".pfx";
                    signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + txtdigitalName_hidden.Text.Split('~')[0].ToString().Trim() + ".pfx";
                    signPdfPath = oconverter.DirectoryPath(out VirtualPath);

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

                    finalResult = generateindivisualPdf(ICEXReportDocument, dsCrystal, "Yes", digitalSignaturePassword,
                                       signPath, ReportPath
                                     , tmpPdfPath, Session["LastCompany"].ToString(), signPdfPath, VirtualPath,
                                     HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString()
                                     , DigiEmpDetail);
                    if (finalResult == "Success")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript24", "<script language='javascript'>Page_Load();</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "mailsendwithreload(1);", true);

                    }
                    if (finalResult == "")
                    {

                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript22", "<script language='javascript'>Page_Load();</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "mailsendwithreload(3);", true);
                    }
                    if (IsNumeric(finalResult))
                    {
                        if (TokenType == "E")
                        {
                            CbpSuggestISIN.JSProperties["cpNoPDFGenerated"] = finalResult;
                        }
                    }

                }
            }
            ds = dailyrep.DailyMarginReportToClient(dtfor.Value.ToString(), HiddenField_Client.Value.ToString(), HiddenField_Segment.Value.ToString(),
                   ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(),
                   Session["LastCompany"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), CHKAPPMRGN.Checked ? "CHK" : "UNCHK",
                   CHKDETAIL.Checked ? "CHK" : "UNCHK", ((rbPrint.Checked) || (rdbmail.Checked)) ? "CHK" : "UNCHK", rdbVarmarginElm.Checked ? "AppMrgn" : "VarMrgn");

            //allcontract = Convert.ToString(dtallcontract.Rows[0]["CntrNo"]);
            //ecnenable = Convert.ToString(dtecnenable.Rows[0]["CntrNo"]);
            //deliveryrpt = Convert.ToString(dtdeliveryrpt.Rows[0]["CntrNo"]);
            //remaining = Convert.ToString(dtremaining.Rows[0]["CntrNo"]);
            allcontract = ds.Tables[3].Rows[0][0].ToString();
            ecnenable = ds.Tables[4].Rows[0][0].ToString();
            deliveryrpt = ds.Tables[5].Rows[0][0].ToString();
            remaining = ds.Tables[6].Rows[0][0].ToString();
            ecnenable_excel = ds.Tables[7].Rows[0][0].ToString();
            remaining_excel = ds.Tables[8].Rows[0][0].ToString();
            CbpSuggestISIN.JSProperties["cpallcontractpops"] = deliveryrpt;
            CbpSuggestISIN.JSProperties["cpecnenablepops"] = remaining;
        }

        void export(DataTable dtExport)
        {
            ExcelFile objExcel = new ExcelFile();

            string searchCriteria = null;
            Converter oconverter = new Converter();
            searchCriteria = "For " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + " Report of   " + ddlGroup.SelectedItem.Value + " Wise";

            // dtExport = dtecnenale.Copy();
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oDbEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "Daily Margin Reporting To Client_" + exlTime;
            strDownloadFileName = "~/Documents/";
            DataTable dtcompany = oDbEngine.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "Daily Margin Reporting To Client Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots
            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "50", "50", "50", "50" };
            string[] ColumnWidthSize = { "40", "40", "40", "40" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cexcelexportpanel.JSProperties["cpexport"] = null;
            if (allcontract_excel.ToString() != "")
            {

                forexcelexport = oGenericMethod.GetDataTable("select ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,isnull(cnt_firstName,'')+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'') as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone where Contactandemailandphone.cnt_internalId in (" + allcontract_excel.ToString() + ")");
                export(forexcelexport);
            }
            //else
            //    Cexcelexportpanel.JSProperties["cpexport"] = "norecord";
        }

        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cexcelexportpanel.JSProperties["cpexport"] = null;
            if (ecnenable_excel.ToString() != "")
            {
                forexcelexport = oGenericMethod.GetDataTable("select ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,isnull(cnt_firstName,'')+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'') as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone where Contactandemailandphone.cnt_internalId in (" + ecnenable_excel.ToString() + ")");
                export(forexcelexport);
            }
            //else
            //    Cexcelexportpanel.JSProperties["cpexport"] = "norecord";
        }

        protected void cmbExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cexcelexportpanel.JSProperties["cpexport"] = null;
            if (deliver_excel.ToString() != "")
            {
                forexcelexport = oGenericMethod.GetDataTable("select ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,isnull(cnt_firstName,'')+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'') as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone where Contactandemailandphone.cnt_internalId in (" + deliver_excel.ToString() + ")");
                export(forexcelexport);
            }
            //else
            //    Cexcelexportpanel.JSProperties["cpexport"] = "norecord";
        }
        #endregion

        #region User Defined Methods
        void date()
        {
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtfor.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());

        }

        void fn_Client()
        {

            DataTable dtclient = new DataTable();
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                        }
                        else
                        {
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select distinct gpm_id from tbl_master_groupmaster ))");

                        }
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value + "))");
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    if (rdbranchAll.Checked)
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value + ")");
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "2")
                {
                    if (rdbranchAll.Checked)
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers) ");
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + HiddenField_BranchGroup.Value + "))");
                    }
                }
            }
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                        }
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value + "))");
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers) ");
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + HiddenField_BranchGroup.Value + "))");
                    }
                }
            }
            string Clients = null;
            if (dtclient.Rows.Count > 0)
            {
                for (int i = 0; i < dtclient.Rows.Count; i++)
                {
                    if (Clients == null)
                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                    else
                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                }
            }
            if (Clients != null)
            {
                HiddenField_Client.Value = Clients;
            }
        }

        private void fn_Segment()
        {
            string Segment = null;
            //===If segment Checked ALL
            if (rdbSegmentAll.Checked == true)
            {
                DataTable dtSegment = oDbEngine.GetDataTable("tbl_master_companyExchange", "distinct EXCH_INTERNALID", "exch_segmentId in('CM','FO','COMM','CDX','SPOT') AND exch_compid='" + Session["LastCompany"].ToString() + "' AND EXCH_EXCHID IN('EXN0000002','EXB0000001','EXM0000001','EXM0000002','EXN0000004','EXN0000005','EXN0000006','EXD0000001','EXI0000001','EXI0000002','EXU0000001','EXC0000001','EXA0000002','EXB0000001')");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            //===If Current segment Checked
            if (rdbSegmentSpecific.Checked == true)
            {
                DataTable dtSegment = oDbEngine.GetDataTable("tbl_master_companyExchange", "distinct CASE WHEN EXCH_EXCHID='EXN0000002' AND exch_segmentId='CM' THEN 'NSE-CM' WHEN EXCH_EXCHID='EXB0000001' AND exch_segmentId='CM' THEN 'BSE-CM' WHEN EXCH_EXCHID='EXN0000002' AND exch_segmentId='FO' THEN 'NSE-FO' WHEN EXCH_EXCHID='EXN0000002' AND exch_segmentId='CDX' THEN 'NSE-CDX' WHEN EXCH_EXCHID='EXB0000001' AND exch_segmentId='FO' THEN 'BSE-FO' WHEN EXCH_EXCHID='EXB0000001' AND exch_segmentId='CDX' THEN 'BSE-CDX' WHEN EXCH_EXCHID='EXM0000001' AND exch_segmentId='COMM' THEN 'MCX-COMM' WHEN EXCH_EXCHID='EXM0000002' AND exch_segmentId='CDX' THEN 'MCXSX-CDX' WHEN EXCH_EXCHID='EXM0000002' AND exch_segmentId='CM' THEN 'MCXSX-CM' WHEN EXCH_EXCHID='EXM0000002' AND exch_segmentId='FO' THEN 'MCXSX-FO' WHEN EXCH_EXCHID='EXN0000004' AND exch_segmentId='COMM' THEN 'NCDEX-COMM' WHEN EXCH_EXCHID='EXD0000001' AND exch_segmentId='COMM' THEN 'DGCX-COMM' WHEN EXCH_EXCHID='EXN0000005' AND exch_segmentId='COMM' THEN 'NMCE-COMM' WHEN EXCH_EXCHID='EXI0000001' AND exch_segmentId='COMM' THEN 'ICEX-COMM' WHEN EXCH_EXCHID='EXU0000001' AND exch_segmentId='CDX' THEN 'USE-CDX' WHEN EXCH_EXCHID='EXN0000006' AND exch_segmentId='SPOT' THEN 'NSEL-SPOT' WHEN EXCH_EXCHID='EXC0000001' AND exch_segmentId='CM' THEN 'CSE-CM' WHEN EXCH_EXCHID='EXA0000002' AND exch_segmentId='COMM' THEN 'ACE-COMM' WHEN EXCH_EXCHID='EXI0000002' AND exch_segmentId='COMM' THEN 'INMX-COMM' ELSE NULL END AS SEGMENT", "EXCH_INTERNALID='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    Segment = Session["usersegid"].ToString();
                    litSegmentMain.InnerText = dtSegment.Rows[0][0].ToString();
                }
            }
            //===Selected Segments already filled By Ajax in HiddenField_Segment       
            if (Segment != null)
            {
                HiddenField_Segment.Value = Segment;
            }
        }

        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }

        void procedure()
        {
            fn_Segment();
            fn_Client();


            ds = dailyrep.DailyMarginReportToClient(dtfor.Value.ToString(), HiddenField_Client.Value.ToString(), HiddenField_Segment.Value.ToString(),
              ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(),
              Session["LastCompany"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), CHKAPPMRGN.Checked ? "CHK" : "UNCHK",
              CHKDETAIL.Checked ? "CHK" : "UNCHK", ((rbPrint.Checked) || (rdbmail.Checked)) ? "CHK" : "UNCHK", rdbVarmarginElm.Checked ? "AppMrgn" : "VarMrgn");

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD();", true);
            }

        }

        void ddlbandforCLIENT()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewCLIENT = new DataView(ds.Tables[0]);
            Distinctclient = viewCLIENT.ToTable(true, new string[] { "CLIENTID", "Name" });
            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "CLIENTID";
                cmbclient.DataTextField = "Name";
                cmbclient.DataBind();

            }
            LastPage = cmbclient.Items.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }

        void bind_Details()
        {
            cmbclient.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + cmbclient.Items.Count.ToString() + " Record.";

            }
            htmltable(cmbclient.SelectedItem.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
            ShowHidePreviousNext_of_Clients();
        }

        void ShowHidePreviousNext_of_Clients()
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

        void htmltable(string CLIENTID)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            String strHtml2 = String.Empty;

            DataView viewclient = new DataView();
            DataView viewCompliance = new DataView();

            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CLIENTID='" + CLIENTID + "'";
            viewCompliance = ds.Tables[4].DefaultView;

            DataTable dtCompliance = new DataTable();
            dtCompliance = viewCompliance.ToTable();
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();
            dt.Columns.Remove("SRNO");
            dt.Columns.Remove("CLIENTID");
            dt.Columns.Remove("Name");
            dt.Columns.Remove("UCC");
            dt.Columns.Remove("SEGMNETID");
            dt.Columns.Remove("GRPID");
            dt.Columns.Remove("GRPNAME");
            int COLCOUNT = dt.Columns.Count;
            int flag = 0;
            ////////////////////////////////HEADER BIND
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());

            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + COLCOUNT + " style=\"color:Blue;\">" + str + "</td></tr></table>";
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            strHtml1 = null;

            ////===========Start Compliance details=================
            strHtml2 = "<table style=\"background-color:#DBEEF3;\" width=\"100%\" border=" + 0 + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";

            strHtml2 += "<tr><td style=\"color:Black;\"><b style=\"text-decoration:underline;font-size:13px;\">Compliance Officer Details</b></td></tr>";
            strHtml2 += "<tr style=\"background-color:#DBEEF3;\"><td><b style=\"font-size:12px;\">" + dtCompliance.Rows[0]["Name"] + "</b></td></tr>";
            strHtml2 += "<tr style=\"background-color:#DBEEF3;\"><td><b style=\"font-size:12px;\">" + dtCompliance.Rows[0]["Addr"] + "</b></td></tr>";
            strHtml2 += "<tr style=\"background-color:#DBEEF3;\"><td style=\"padding-left:58px;\"><b style=\"font-size:12px;\">" + dtCompliance.Rows[0]["Pin"] + "</b></td></tr>";
            strHtml2 += "<tr style=\"background-color:#DBEEF3;\"><td><b style=\"font-size:12px;\">" + dtCompliance.Rows[0]["Phone"] + "</b></td></tr>";
            strHtml2 += "<tr style=\"background-color:#DBEEF3;\"><td><b style=\"font-size:12px;\">" + dtCompliance.Rows[0]["Email"] + "</b></td></tr></table>";

            DIVdisplayCompliance.InnerHtml = strHtml2;
            ///============End compliance details======================


            ///============Start DETAILS BIND===========================
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            ////===========Start TABLE HEADER BIND===========
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < COLCOUNT; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;nowrap=nowrap;\"><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";
            ////===========End TABLE HEADER BIND==============

            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();
            string segmentname = null;

            //=======Start Group name With Client Name===============
            flag = flag + 1;
            strHtml1 = "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml1 += "<td align=\"left\" style=\"font-size;\" colspan=" + COLCOUNT + ">Group :" + dt1.Rows[0]["GRPNAME"].ToString() + " Client Name :<b>" + dt1.Rows[0]["Name"].ToString() + " </b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b>]</td>";
            strHtml1 += "</tr>";
            //=======End Group Name With Client Name===============
            strHtml += strHtml1;
            //=====Start Segment Wise Particulars Detail============
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < COLCOUNT; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\" >" + dr1[j] + "</td>";
                        }
                        else
                        {
                            if (dt.Columns[j].ColumnName == "SEGMENT")
                            {
                                if (segmentname != dr1[j].ToString())
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;color=Green;background-color: #DDECFE\" ><b>" + dr1[j] + "</b></td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller\" >&nbsp;</td>";
                                }
                                segmentname = dr1[j].ToString();
                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\" >" + dr1[j] + "</td>";
                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:smaller\" >&nbsp;</td>";
                    }
                }
            }
            //=====End Segment Wise Particulars Detail============
            //=====Start Combined Wise Particulars Detail============
            strHtml += "<tr>";
            if ((rdbSegmentAll.Checked == true) || (rdbSegmentSelect.Checked == true))
            {
                if (ds.Tables[1].Columns.Count > 1)
                {
                    DataView viewclient1 = new DataView();
                    //viewclient1 = ds.Tables[1].DefaultView;
                    //====Modifying Mukut==========
                    viewclient1 = ds.Tables[2].DefaultView;
                    viewclient1.RowFilter = "CLIENTID='" + CLIENTID + "'";
                    DataTable dt3 = new DataTable();
                    dt3 = viewclient1.ToTable();
                    dt3.Columns.Remove("CLIENTID");

                    dt3.Columns.Remove("Combined");
                    strHtml += "<tr><td></td></tr>";
                    strHtml += "<tr><td><b><u>Combined</u></b></td></tr>";
                    foreach (DataRow dr1 in dt3.Rows)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtml += "<td align=\"left\" style=\"font-size:smaller\" >&nbsp;</td>";

                        for (int k = 0; k < dt3.Columns.Count; k++)
                        {
                            if (dr1[k] != DBNull.Value)
                            {
                                if (IsNumeric(dr1[k].ToString()) == true)
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\" >" + dr1[k] + "</td>";
                                }
                                else
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\" >" + dr1[k] + "</td>";
                                }
                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller\" >&nbsp;</td>";
                            }
                        }
                        strHtml += "<tr>";
                    }
                }
            }
            //=====End Combined Wise Particulars Detail============

            //===========Start Show Holding Details If CheckDetail True================
            if (CHKDETAIL.Checked == true)
            {
                if (ds.Tables.Count > 2)
                {
                    DataView viewclient2 = new DataView();
                    viewclient2 = ds.Tables[3].DefaultView;
                    // viewclient2 = ds.Tables[2].DefaultView;
                    viewclient2.RowFilter = "CLIENTID='" + CLIENTID + "'";
                    DataTable dt2 = new DataTable();
                    dt2 = viewclient2.ToTable();
                    dt2.Columns.Remove("CLIENTID");

                    if (dt2.Rows.Count > 0)
                    {
                        strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                        strHtml += "<tr style=\"background-color:#CC9933;\"><td><b>Detail Of Securities</b></td></tr>";
                        strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                        strHtml += "<td align=\"center\" >SEGMENT</td>";
                        strHtml += "<td align=\"center\">Our DP Account </td>";
                        strHtml += "<td align=\"center\">DP-ID / CLIENT-ID</td>";
                        strHtml += "<td align=\"center\">Scrip Name</td>";
                        strHtml += "<td align=\"center\">ISIN</td>";
                        strHtml += "<td align=\"center\">Quantity</td>";
                        strHtml += "<td align=\"center\">Rate</td>";
                        strHtml += "<td align=\"center\">Haircut</td>";
                        strHtml += "<td align=\"center\">Value</td></tr>";
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            flag = flag + 1;
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\" >" + dt2.Rows[i]["segmentname"].ToString() + "</td>";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\" >" + dt2.Rows[i]["DPAccounts_ShortName"].ToString() + "</td>";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\" >" + dt2.Rows[i]["dpcliid"].ToString() + "</td>";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\" >" + dt2.Rows[i]["scripname"].ToString() + "</td>";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\" >" + dt2.Rows[i]["isinno"].ToString() + "</td>";
                            if (dt2.Rows[i]["Quantity"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\" >" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dt2.Rows[i]["Quantity"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td  >&nbsp;</td>";
                            }
                            if (dt2.Rows[i]["closeprice"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[i]["closeprice"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td  >&nbsp;</td>";
                            }
                            if (dt2.Rows[i]["varmargin"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[i]["varmargin"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td >&nbsp;</td>";
                            }
                            if (dt2.Rows[i]["Stocksresult"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[i]["Stocksresult"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td >&nbsp;</td>";
                            }
                            strHtml += "<tr>";
                        }
                        strHtml += "<tr style=\"background-color:white\"><td colspan=8 style=\"color:maroon;\"><b>Total</b></td>";
                        strHtml += "<td align=\"right\" style=\"color:maroon;\" ><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["SUMStocksresult"].ToString())) + "</b></td>";
                        strHtml += "</tr></table>";
                    }
                }
            }
            //===========End Show Holding Details If CheckDetail True================        
            strHtml += "<tr></table>";
            display.InnerHtml = strHtml;
            ViewState["email"] = "<br/>" + strHtml2 + strHtml;
        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }

        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
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
        #endregion
    }
}