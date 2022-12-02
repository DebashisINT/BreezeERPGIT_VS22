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
    public partial class Reports_QuarterlyStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data;
        static string Branch;
        static string Clients;
        static string Group = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        Converter oDbConverter = new Converter();
        BusinessLogicLayer.FAReportsOther oFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        string SingleDouble = null;
        DataSet DigitalSignatureDs = new DataSet();
        ReportDocument ICEXReportDocument = new ReportDocument();
        DataTable DistinctBillNumber;
        DataTable DistinctClient;
        DataSet Gridbind = new DataSet();
        GenericMethod oGenericMethod = new GenericMethod();
        string result;
        protected string allcontract, ecnenable, deliveryrpt, remaining;
        public string allclient_excel
        {
            get { return (string)Session["allclient_excel"]; }
            set { Session["allclient_excel"] = value; }
        }
        public string release_excel
        {
            get { return (string)Session["release_excel"]; }
            set { Session["release_excel"] = value; }
        }
        public string ecnenable_excel
        {
            get { return (string)Session["ecnenable_excel"]; }
            set { Session["ecnenable_excel"] = value; }
        }
        public string allreadydeliver_excel
        {
            get { return (string)Session["allreadydeliver_excel"]; }
            set { Session["allreadydeliver_excel"] = value; }
        }
        public string TokenType
        {
            get { return (string)Session["P_TokenType"]; }
            set { Session["P_TokenType"] = value; }
        }
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                QuatarLydateBind();
                dtFrom.Date = DateTime.Today;
                dtTo.Date = DateTime.Today;
                Clients = null;
                Branch = null;
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");

                DtSatementDate.EditFormatString = oDbConverter.GetDateFormat("Date");
                string[] QuarDate = oDbConverter.QuarterlyDate(oDBEngine.GetDate()).Split('~');

                DtSatementDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                txtdigitalName.Visible = IsSignExists();
                if (txtdigitalName.Visible == true)
                {
                    td_msg.Visible = false;

                }
                else
                {
                    td_msg.Visible = true;
                }
                SegmentnameFetch();
            }
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            BindAjaxList(oGenericMethod.GetDigitalSignature(), txtdigitalName);
            //txtdigitalName.Attributes.Add("onkeyup", "CallAjax(this,'SearchEmployeesNameWithDigitalSign',event)");
            string abc = txtdigitalName_hidden.Text.Trim();
        }
        void BindAjaxList(String CombinedQuery, TextBox TxtBoxName)
        {
            //CombinedQuery = CombinedQuery.Replace("'", "\\'");
            TxtBoxName.Attributes.Add("onkeyup", "CallAjax1(this,'GenericAjaxList',event,'" + CombinedQuery + "')");
        }
        void SegmentnameFetch()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                HiddenField_SegmentName.Value = "NSE - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                HiddenField_SegmentName.Value = "BSE - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                HiddenField_SegmentName.Value = "CSE - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                HiddenField_SegmentName.Value = "NSE - FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                HiddenField_SegmentName.Value = "BSE - FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                HiddenField_SegmentName.Value = "NSE - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                HiddenField_SegmentName.Value = "BSE - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                HiddenField_SegmentName.Value = "MCX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                HiddenField_SegmentName.Value = "MCXSX - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                HiddenField_SegmentName.Value = "NCDEX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                HiddenField_SegmentName.Value = "DGCX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                HiddenField_SegmentName.Value = "NMCE - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                HiddenField_SegmentName.Value = "ICEX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                HiddenField_SegmentName.Value = "USE - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                HiddenField_SegmentName.Value = "NSEL - SPOT";
        }

        bool IsSignExists()
        {
            string str;
            str = string.Empty;
            str = oFAReportsOther.searchSignatureUser(Convert.ToString(Session["userid"]));
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "searchSignatureUser";

            //    cmd.Parameters.AddWithValue("@userID", Session["userid"]);

            //    cmd.CommandTimeout = 0;
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //    con.Open();
            //    str = Convert.ToString(cmd.ExecuteScalar());
            //    con.Close();
            //}
            if (str == "Y")
                return true;
            else
                return false;

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void QuatarLydateBind()
        {

            DdlQuatarlyDate.Items.Add("30 Jun " + Session["LastFinYear"].ToString().Trim().Substring(0, 4), "0");
            DdlQuatarlyDate.Items.Add("30 Sep " + Session["LastFinYear"].ToString().Trim().Substring(0, 4), "1");
            DdlQuatarlyDate.Items.Add("31 Dec " + Session["LastFinYear"].ToString().Trim().Substring(0, 4), "2");
            DdlQuatarlyDate.Items.Add("31 Mar " + Session["LastFinYear"].ToString().Trim().Substring(5, 4), "3");
            DdlQuatarlyDate.SelectedIndex = 0;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            Clients = null;
            Branch = null;
            string[] cl = idlist[1].Split(',');
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

                if (idlist[0] == "Clients")
                {
                    Clients = str;
                    data = "Clients~" + str;

                }

                else if (idlist[0] == "Group")
                {
                    Group = str;
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    Branch = str;
                    data = "Branch~" + str;
                }
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                BindGroup();
            }
        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
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


        public string generateindivisualPdf(ReportDocument ICEXReportDocument, DataSet dsCrystal,
                                         string digitalSignaturePassword,
                                         string SignPath, string reportPath,
                                         string tmpPDFPath, string CompanyId,
                                         string signPdfPath, string VirtualPath,
                                         string user, string LastFinYear, string DigitalSignatureID)
        {
            string status;
            status = string.Empty;
            DataTable FilterClients = new DataTable();
            DataTable FilterSummary = new DataTable();
            DataTable FilterAccountSummary = new DataTable();
            DataTable FilterHolding = new DataTable();

            DataView viewClients = new DataView(dsCrystal.Tables[1]);
            //DataView viewClients = new DataView(dsCrystal.Tables[1]);
            DistinctBillNumber = viewClients.ToTable(true, new string[] { "client" });
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            foreach (DataRow dr in DistinctBillNumber.Rows)
            {
                try
                {


                    viewClients.RowFilter = "client = '" + dr["client"] + "' AND (Email is not null OR Email<>'')";// AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P')";
                    FilterClients = viewClients.ToTable();
                    if (FilterClients.Rows.Count > 0)
                    {

                        reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\QSR1.rpt");
                        string module = HiddenField_SegmentName.Value.ToString();
                        string pdfstr = string.Empty;
                        //string pdfstr = tmpPDFPath  + "-" + FilterClients.Rows[0]["client"].ToString() + "-" + ".pdf";
                        if (TokenType != "E")
                        {
                            pdfstr = tmpPDFPath + module + "-" + oDBEngine.GetDate().ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["clientucc"].ToString() + "-" + FilterClients.Rows[0]["client"].ToString() + ".pdf";
                        }
                        else
                        {
                            //Convert.ToDateTime(DtSatementDate.Value.ToString()).ToString("ddMMMyyyy")
                            pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(DtSatementDate.Value.ToString()).ToString("ddMMMyyyy") + "-" +
                            FilterClients.Rows[0]["clientucc"].ToString() + "-" + Convert.ToString(Session["ExchangeSegmentID"]) + "-" +
                            FilterClients.Rows[0]["client"].ToString() + "-" + Convert.ToString(Session["UserSegID"]) + "-" + Convert.ToString(Session["UserID"]) + "-" +
                            Convert.ToString(Session["LastFinYear"]).Replace("-", "") + "-" + DigitalSignatureID + "~" +
                            Convert.ToDateTime(FilterClients.Rows[0]["FromDate"].ToString()).ToString("dd-MMM-yyyy") + "~" +
                            Convert.ToDateTime(FilterClients.Rows[0]["Todate"].ToString()).ToString("dd-MMM-yyyy") + ".pdf";

                            //+"-" + Convert.ToString(Session["ExchangeSegmentID"]) + "-" + FilterClients.Rows[0]["CLIENTID"].ToString() + "-" +
                            //Convert.ToString(Session["UserSegID"]) + "-" + Convert.ToString(Session["UserID"]) + "-" +
                            //Convert.ToString(Session["LastFinYear"]).Replace("-", "") + "-" + DigitalSignatureID + ".pdf";
                        }
                        signPdfPath = oDbConverter.DirectoryPath(out VirtualPath);
                        dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//QuarterlyStatementforemail.xsd");
                        dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//QuarterlyStatement.xsd");
                        //dsCrystal.Tables[3].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//QuarterlyStatement.xsd");
                        //dsCrystal.Tables[5].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//QuarterlyStatement.xsd");
                        //dsCrystal.Tables[4].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//QuarterlyStatement.xsd");
                        //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//QuarterlyStatementforemail.xsd");
                        ICEXReportDocument.Load(reportPath);
                        ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                        ICEXReportDocument.SetDataSource(FilterClients);

                        //ICEXReportDocument.Subreports["QS"].SetDataSource(dsCrystal.Tables[2]);
                        // ICEXReportDocument.Subreports["RegisterOFSecurity.rpt"].SetDataSource(dsCrystal.Tables[3]);
                        //  ICEXReportDocument.Subreports["QuatarlyStatement1"].SetDataSource(dsCrystal.Tables[5]);
                        //ICEXReportDocument.Subreports["QuatarlyStatement"].SetDataSource(dsCrystal.Tables[4]);
                        for (int c = 0; c < dsCrystal.Tables[2].Rows.Count; c++)
                        {
                            if (FilterClients.Rows[0]["client"].ToString() == dsCrystal.Tables[2].Rows[c]["clientid"].ToString())
                            {
                                ICEXReportDocument.Subreports["QS"].SetDataSource(dsCrystal.Tables[2]);
                                break;
                            }
                        }
                        for (int s = 0; s < dsCrystal.Tables[3].Rows.Count; s++)
                        {
                            if (FilterClients.Rows[0]["client"].ToString() == dsCrystal.Tables[3].Rows[s]["CUSTOMERID"].ToString())
                            {
                                ICEXReportDocument.Subreports["RegisterOFSecurity.rpt"].SetDataSource(dsCrystal.Tables[3]);
                                break;
                            }
                        }
                        for (int m = 0; m < dsCrystal.Tables[4].Rows.Count; m++)
                        {
                            if (FilterClients.Rows[0]["client"].ToString() == dsCrystal.Tables[4].Rows[m]["ClientID"].ToString())
                            {
                                ICEXReportDocument.Subreports["QuatarlyStatement"].SetDataSource(dsCrystal.Tables[4]);
                                break;
                            }
                        }

                        for (int p = 0; p < dsCrystal.Tables[5].Rows.Count; p++)
                        {
                            if (FilterClients.Rows[0]["client"].ToString() == dsCrystal.Tables[5].Rows[p]["CustomerID"].ToString())
                            {
                                ICEXReportDocument.Subreports["QuatarlyStatement1"].SetDataSource(dsCrystal.Tables[5]);
                                break;
                            }
                        }
                        ICEXReportDocument.SetParameterValue("@SettDate", Convert.ToDateTime(DtSatementDate.Value).ToString("dd-MMM-yyyy"));
                        ICEXReportDocument.SetParameterValue("@SingleDouble", SingleDouble.ToString());
                        ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);
                        string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Quarterly Statement Of Accounts'", 1);
                        string quarter = "";
                        if (ddlrptgeneration.SelectedItem.Value == "1")
                            quarter = DdlQuatarlyDate.SelectedItem.Value.ToString();
                        else
                            //quarter = "4";
                            quarter = dtFrom.Text.Split('-')[2] + dtFrom.Text.Split('-')[1] + dtFrom.Text.Split('-')[0] + dtTo.Text.Split('-')[2] + dtTo.Text.Split('-')[1] + dtTo.Text.Split('-')[0] + "4";
                        //else
                        //{
                        //    if ((dtFrom.Value >= Convert.ToDateTime(Session["FinYear"].ToString().Split('-')[0] + "-04-01") 
                        //        && dtFrom.Value <= Session["FinYear"].ToString().Split('-')[0] + "-06-30") 
                        //        && (dtTo.Value >= Session["FinYear"].ToString().Split('-')[0] + "-04-01" 
                        //        && dtTo.Value <= Session["FinYear"].ToString().Split('-')[0] + "-06-30"))
                        //        quarter = "0";
                        //    if ((dtFrom.Value >= Convert.ToDateTime(Session["FinYear"].ToString().Split('-')[0] + "-07-01") 
                        //        && dtFrom.Value <= Session["FinYear"].ToString().Split('-')[0] + "-09-30") 
                        //        && (dtTo.Value >= Session["FinYear"].ToString().Split('-')[0] + "-07-01" 
                        //        && dtTo.Value <= Session["FinYear"].ToString().Split('-')[0] + "-09-30"))
                        //        quarter = "1";
                        //    if ((dtFrom.Value >= Convert.ToDateTime(Session["FinYear"].ToString().Split('-')[0] + "-10-01") 
                        //        && dtFrom.Value <= Session["FinYear"].ToString().Split('-')[0] + "-12-31") 
                        //        && (dtTo.Value >= Session["FinYear"].ToString().Split('-')[0] + "-10-01" 
                        //        && dtTo.Value <= Session["FinYear"].ToString().Split('-')[0] + "-12-31"))
                        //        quarter = "2";
                        //    if ((dtFrom.Value >= Convert.ToDateTime(Session["FinYear"].ToString().Split('-')[1] + "-01-01") 
                        //        && dtFrom.Value <= Session["FinYear"].ToString().Split('-')[1] + "-03-31") 
                        //        && (dtTo.Value >= Session["FinYear"].ToString().Split('-')[1] + "-01-01" 
                        //        && dtTo.Value <= Session["FinYear"].ToString().Split('-')[1] + "-03-31"))
                        //        quarter = "3";
                        //}
                        if (TokenType != "E")
                        {
                            status = oDbConverter.DigitalCertificate1(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                  DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, Session["usersegid"].ToString(), quarter,//DdlQuatarlyDate.SelectedItem.Value.ToString(),
                                                  DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                  FilterClients.Rows[0]["Email"].ToString(), Convert.ToDateTime(FilterClients.Rows[0]["FromDate"].ToString()).ToString("dd-MMM-yyyy"),
                                                  Convert.ToDateTime(FilterClients.Rows[0]["Todate"].ToString()).ToString("dd-MMM-yyyy"),
                                //DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString()
                                                  FilterClients.Rows[0]["GRPID1"].ToString(),
                                                  VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            if (status != "Success")
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
                                break;
                            }
                        }
                        //else
                        //{
                        //    status = "Success";
                        //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                        //    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + "PDF Generated Successfully!!" + "');", true);
                        //    break;
                        //}
                    }
                    //else
                    //{
                    //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript19", "<script language='javascript'>Page_Load();</script>");
                    //    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Check details of email settings');", true);

                    //}

                }

                catch (Exception)
                {

                    break;
                }
            }
            if (TokenType == "E")
            {
                status = "Success";
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + "PDF Generated Successfully!!" + "');", true);
            }
            return status;

        }



        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            string GRPTYPE = string.Empty;
            string Groupby = string.Empty;
            string ChkDoNPrintSecurities = string.Empty;
            string ChkConsiderEntrPeriod = string.Empty;
            string FORDATE = string.Empty;
            string Fromdatefrmpage = string.Empty;
            string Todatefrmpage = string.Empty;
            string employeeId = txtSignature_hidden.Value;


            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GRPTYPE = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    Groupby = "ALL";

                }
                else
                {
                    Groupby = HiddenField_Branch.Value.ToString().Trim();
                }

            }
            else
            {
                GRPTYPE = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    Groupby = "ALL";
                }
                else
                {
                    Groupby = HiddenField_Group.Value.ToString().Trim();
                }

            }

            if (ChkDoNotPrintSecurities.Checked)
            {
                ChkDoNPrintSecurities = "Y";
            }
            else
            {
                ChkDoNPrintSecurities = "N";
            }

            if (ddlrptgeneration.SelectedItem.Value == "1")
            {
                if (ChkConsiderEntirePeriod.Checked)
                {
                    ChkConsiderEntrPeriod = "Y";
                }
                else
                {
                    ChkConsiderEntrPeriod = "N";
                }
                FORDATE = DdlQuatarlyDate.SelectedItem.Text.ToString().Trim();
                Fromdatefrmpage = "";
                Todatefrmpage = "";
            }
            else
            {
                Fromdatefrmpage = dtFrom.Text.Split('-')[2] + "-" + dtFrom.Text.Split('-')[1] + "-" + dtFrom.Text.Split('-')[0];
                Todatefrmpage = dtTo.Text.Split('-')[2] + "-" + dtTo.Text.Split('-')[1] + "-" + dtTo.Text.Split('-')[0];
                ChkConsiderEntrPeriod = "N";
                FORDATE = "";
            }


            DataSet dsCrystal = new DataSet();

            if (chkBothPrint.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";
            dsCrystal = oFAReportsOther.Report_QuatarlyStatement(
                Convert.ToString(Session["LastFinYear"]),
                 Convert.ToString(Session["LastCompany"]),
                 Convert.ToString(Session["CustomerID"]),
                 Convert.ToString(GRPTYPE),
                 Convert.ToString(Groupby),
                 Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]),
                 Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                 Convert.ToString(txtHeader_hidden.Value),
                 Convert.ToString(txtFooter_hidden.Value),
                 Convert.ToString(ChkDoNPrintSecurities),
                   Convert.ToString(HttpContext.Current.Session["usersegid"]),
                     Convert.ToString(ChkConsiderEntrPeriod),
                 Convert.ToString(FORDATE),
                 Convert.ToString(Fromdatefrmpage),
                   Convert.ToString(Todatefrmpage)
                );

            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText = "[Report_QuatarlyStatement]";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());
            //cmd.Parameters.AddWithValue("@COMPANYID", Session["LastCompany"].ToString());
            //cmd.Parameters.AddWithValue("@CLIENTS", Session["CustomerID"]);
            //if (ddlGroup.SelectedItem.Value.ToString() == "0")
            //{
            //    cmd.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
            //    if (rdbranchAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", "ALL");

            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", HiddenField_Branch.Value.ToString().Trim());
            //    }

            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@GRPTYPE", ddlgrouptype.SelectedItem.Text.ToString().Trim());
            //    if (rdddlgrouptypeAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", "ALL");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", HiddenField_Group.Value.ToString().Trim());
            //    }

            //}
            //cmd.Parameters.AddWithValue("@BRANCHID", HttpContext.Current.Session["userbranchHierarchy"].ToString());
            //cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            //cmd.Parameters.AddWithValue("@Header", txtHeader_hidden.Value);
            //cmd.Parameters.AddWithValue("@Footer", txtFooter_hidden.Value);

            //if (ChkDoNotPrintSecurities.Checked)
            //{
            //    cmd.Parameters.AddWithValue("@ChkDoNotPrintSecurities", "Y");
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@ChkDoNotPrintSecurities", "N");
            //}
            //cmd.Parameters.AddWithValue("@dpid", HttpContext.Current.Session["usersegid"].ToString());

            //if (ddlrptgeneration.SelectedItem.Value == "1")
            //{
            //    if (ChkConsiderEntirePeriod.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ChkConsiderEntirePeriod", "Y");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@ChkConsiderEntirePeriod", "N");
            //    }
            //    cmd.Parameters.AddWithValue("@FORDATE", DdlQuatarlyDate.SelectedItem.Text.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Fromdatefrmpage", "");
            //    cmd.Parameters.AddWithValue("@Todatefrmpage", "");
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@Fromdatefrmpage", dtFrom.Text.Split('-')[2] + "-" + dtFrom.Text.Split('-')[1] + "-" + dtFrom.Text.Split('-')[0]);
            //    cmd.Parameters.AddWithValue("@Todatefrmpage", dtTo.Text.Split('-')[2] + "-" + dtTo.Text.Split('-')[1] + "-" + dtTo.Text.Split('-')[0]);
            //    cmd.Parameters.AddWithValue("@ChkConsiderEntirePeriod", "N");
            //    cmd.Parameters.AddWithValue("@FORDATE", "");
            //}
            //cmd.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd;
            //Adap.Fill(dsCrystal);
            //cmd.Dispose();
            //con.Dispose();
            //GC.Collect();
            byte[] logoinByte;
            byte[] SignatureinByte;
            dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            dsCrystal.Tables[0].Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
            dsCrystal.Tables[0].Columns.Add("Status", System.Type.GetType("System.String"));
            Session["CustomerID"] = null;
            if (ChkLogo.Checked == true)
            {
                DataTable dtcmp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
                if (oDbConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo_" + dtcmp.Rows[0][0].ToString() + ".bmp"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

                }
                else
                {

                    for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                    {
                        dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                    }
                }
            }
            if (ChkSignatory.Checked == true)
            {
                if (oDbConverter.getSignatureImage(employeeId, out SignatureinByte, "NSE") == 1)
                {
                    for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                    {
                        dsCrystal.Tables[0].Rows[i]["Signature"] = SignatureinByte;
                        dsCrystal.Tables[0].Rows[i]["Status"] = "a";
                    }
                }
            }
            else
            {
                for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                {
                    dsCrystal.Tables[0].Rows[i]["Status"] = "This is an electronicaly generated statement that requires no Signature. *";
                }
            }

            //dsCrystal.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//QuarterlyStatement.xsd");
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            ReportDocument reportObj = new ReportDocument();
            string ReportPath = Server.MapPath("..\\Reports\\QSR.rpt");
            reportObj.Load(ReportPath);
            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            reportObj.SetDataSource(dsCrystal);
            DataTable dtcompliancename = oDBEngine.GetDataTable("(select top 1  isnull(Cnt_FirstName,'')+' '+isnull(Cnt_MiddleName,'')+' '+isnull(Cnt_LastName,'')from tbl_master_Contact where cnt_internalid=(select exch_ComplianceOfficer from tbl_Master_CompanyeXchange where exch_internalID='" + HttpContext.Current.Session["usersegid"].ToString() + "'))");
            DataTable dtcompliancephone = oDBEngine.GetDataTable("(select top 1 isnull(phf_countrycode,'')+' '+isnull(phf_areacode,'')+' '+isnull(phf_phonenumber,'') from tbl_master_PhoneFax where  Phf_cntid=(select exch_ComplianceOfficer from tbl_Master_CompanyeXchange where exch_internalID='" + HttpContext.Current.Session["usersegid"].ToString() + "'))");
            DataTable dtcomplianceemail = oDBEngine.GetDataTable("(select top 1 isnull(eml_email,'') from tbl_master_Email where eml_cntid=(select exch_ComplianceOfficer from tbl_Master_CompanyeXchange where exch_internalID='" + HttpContext.Current.Session["usersegid"].ToString() + "'))");
            string name = "Compliance Officer Name : ";
            string phone = "Phone : ";
            string email = "Email : ";
            if (dtcompliancename.Rows.Count > 0)
            {
                reportObj.SetParameterValue("@comname", name + dtcompliancename.Rows[0][0].ToString());
                if (dtcompliancephone.Rows.Count > 0)
                    reportObj.SetParameterValue("@comphone", phone + dtcompliancephone.Rows[0][0].ToString());
                else
                    reportObj.SetParameterValue("@comphone", phone + "");
                if (dtcomplianceemail.Rows.Count > 0)
                    reportObj.SetParameterValue("@comemail", email + dtcomplianceemail.Rows[0][0].ToString());
                else
                    reportObj.SetParameterValue("@comemail", email + "");

            }
            else
            {
                reportObj.SetParameterValue("@comname", name + "");
                reportObj.SetParameterValue("@comphone", phone + "");
                reportObj.SetParameterValue("@comemail", email + "");
            }
            reportObj.SetParameterValue("@SettDate", Convert.ToDateTime(DtSatementDate.Value).ToString("dd-MMM-yyyy"));
            reportObj.SetParameterValue("@SingleDouble", SingleDouble.ToString());
            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "QuatarlyStatementReport" + Session["PageNum"].ToString());
            Session["PageNum"] = null;
            reportObj.Dispose();
            GC.Collect();
        }

        DataSet BindReportViewer(string PageNum, int PageSize)
        {
            string[] sqlParameterName = new string[12];
            string[] sqlParameterType = new string[12];
            string[] sqlParameterValue = new string[12];
            string[] sqlParameterSize = new string[12];
            sqlParameterName[0] = "FINYEAR";
            sqlParameterValue[0] = Session["LastFinYear"].ToString().Trim();
            sqlParameterType[0] = "V";
            sqlParameterSize[0] = "50";

            sqlParameterName[2] = "COMPANYID";
            sqlParameterValue[2] = Session["LastCompany"].ToString();
            sqlParameterType[2] = "V";
            sqlParameterSize[2] = "12";
            sqlParameterName[3] = "CLIENTS";
            if (rdbClientALL.Checked)
            {
                sqlParameterValue[3] = "ALL";
            }
            else
            {
                sqlParameterValue[3] = HiddenField_Client.Value.ToString().Trim();
            }
            sqlParameterType[3] = "V";
            sqlParameterSize[3] = "8000";

            sqlParameterName[4] = "BRANCHID";
            sqlParameterValue[4] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
            sqlParameterType[4] = "V";
            sqlParameterSize[4] = "8000";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                sqlParameterName[5] = "GRPTYPE";
                sqlParameterValue[5] = "BRANCH";
                sqlParameterType[5] = "V";
                sqlParameterSize[5] = "15";
                if (rdbranchAll.Checked)
                {
                    sqlParameterName[6] = "Groupby";
                    sqlParameterValue[6] = "All";
                    sqlParameterType[6] = "V";
                    sqlParameterSize[6] = "8000";
                }
                else
                {
                    sqlParameterName[6] = "Groupby";
                    sqlParameterValue[6] = HiddenField_Branch.Value.ToString().Trim();
                    sqlParameterType[6] = "V";
                    sqlParameterSize[6] = "8000";
                }

            }
            else
            {
                sqlParameterName[5] = "GRPTYPE";
                sqlParameterValue[5] = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                sqlParameterType[5] = "V";
                sqlParameterSize[5] = "15";
                if (rdddlgrouptypeAll.Checked)
                {
                    sqlParameterName[6] = "Groupby";
                    sqlParameterValue[6] = "All";
                    sqlParameterType[6] = "V";
                    sqlParameterSize[6] = "8000";
                }
                else
                {
                    sqlParameterName[6] = "Groupby";
                    sqlParameterValue[6] = HiddenField_Group.Value.ToString().Trim();
                    sqlParameterType[6] = "V";
                    sqlParameterSize[6] = "8000";
                }

            }
            sqlParameterName[7] = "PageNum";
            sqlParameterValue[7] = PageNum;
            sqlParameterType[7] = "I";
            sqlParameterSize[7] = "";
            sqlParameterName[8] = "PageSize";
            sqlParameterValue[8] = PageSize.ToString();
            sqlParameterType[8] = "I";
            sqlParameterSize[8] = "";
            if (ddlrptgeneration.SelectedItem.Value == "1")
            {
                sqlParameterName[1] = "FORDATE";
                sqlParameterValue[1] = DdlQuatarlyDate.SelectedItem.Text.ToString().Trim();
                sqlParameterType[1] = "V";
                sqlParameterSize[1] = "35";
                if (ChkConsiderEntirePeriod.Checked)
                {
                    sqlParameterName[9] = "ChkConsiderEntirePeriod";
                    sqlParameterValue[9] = "Y";
                    sqlParameterType[9] = "V";
                    sqlParameterSize[9] = "3";
                }
                else
                {
                    sqlParameterName[9] = "ChkConsiderEntirePeriod";
                    sqlParameterValue[9] = "N";
                    sqlParameterType[9] = "V";
                    sqlParameterSize[9] = "3";
                }
                sqlParameterName[10] = "Fromdatefrmpage";
                sqlParameterValue[10] = "";
                sqlParameterType[10] = "V";
                sqlParameterSize[10] = "30";

                sqlParameterName[11] = "Todatefrmpage";
                sqlParameterValue[11] = "";
                sqlParameterType[11] = "V";
                sqlParameterSize[11] = "30";
            }
            else
            {
                sqlParameterName[1] = "FORDATE";
                sqlParameterValue[1] = "";
                sqlParameterType[1] = "V";
                sqlParameterSize[1] = "35";

                sqlParameterName[10] = "Fromdatefrmpage";
                sqlParameterValue[10] = dtFrom.Text.Split('-')[2] + "-" + dtFrom.Text.Split('-')[1] + "-" + dtFrom.Text.Split('-')[0];
                sqlParameterType[10] = "V";
                sqlParameterSize[10] = "30";

                sqlParameterName[11] = "Todatefrmpage";
                sqlParameterValue[11] = dtTo.Text.Split('-')[2] + "-" + dtTo.Text.Split('-')[1] + "-" + dtTo.Text.Split('-')[0];
                sqlParameterType[11] = "V";
                sqlParameterSize[11] = "30";
                sqlParameterName[9] = "ChkConsiderEntirePeriod";
                sqlParameterValue[9] = "N";
                sqlParameterType[9] = "V";
                sqlParameterSize[9] = "3";
            }



            return BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("SelectClient_QuatarlyStatement", sqlParameterName, sqlParameterType, sqlParameterValue);
        }

        protected void CbpGenerateReport_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string PageNum = e.Parameter.Split('~')[1].Trim();
            Session["PageNum"] = PageNum;
            int PageSize = 50;
            DataSet DsBindRecord = BindReportViewer(PageNum, PageSize);
            string Command = e.Parameter.Split('~')[0];
            if (Command == "SearchByNavigation")
            {
                if (DsBindRecord.Tables.Count > 0)
                {
                    if (DsBindRecord.Tables[0].Rows.Count > 0)
                    {
                        int TotalItems = Convert.ToInt32(DsBindRecord.Tables[0].Rows[0]["TotalClient"].ToString());
                        int TotalPage = Convert.ToInt32(DsBindRecord.Tables[0].Rows[0]["TotalPDF"].ToString());
                        CbpGenerateReport.JSProperties["cpIsEmptyDsSearch"] = "No~" + PageNum + '~' + TotalPage + "~" + TotalItems;
                        DataTable tempDt;
                        string CustomerID = null;
                        DataRow[] FoundRows;
                        tempDt = DsBindRecord.Tables[0];
                        FoundRows = tempDt.Select();

                        foreach (DataRow dr1 in FoundRows)
                        {
                            if (CustomerID == null)
                            {
                                CustomerID = "'" + dr1["CustomerID"].ToString() + "'";
                            }
                            else
                            {
                                CustomerID = CustomerID + "," + "'" + dr1["CustomerID"].ToString() + "'";
                            }

                        }
                        Session["CustomerID"] = null;
                        Session["CustomerID"] = CustomerID;

                    }
                    else
                    {
                        CbpGenerateReport.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                    }
                }
            }
            if (Command == "GeneratePDF")
            {
                if (DsBindRecord.Tables.Count > 0)
                {
                    if (DsBindRecord.Tables[0].Rows.Count > 0)
                    {
                        int TotalItems = Convert.ToInt32(DsBindRecord.Tables[0].Rows[0]["TotalClient"].ToString());
                        int TotalPage = Convert.ToInt32(DsBindRecord.Tables[0].Rows[0]["TotalPDF"].ToString());
                        CbpGenerateReport.JSProperties["cpIsEmptyDsSearch"] = "No~" + PageNum + '~' + TotalPage + "~" + TotalItems;
                        DataTable tempDt;
                        string CustomerID = null;
                        DataRow[] FoundRows;
                        tempDt = DsBindRecord.Tables[0];
                        FoundRows = tempDt.Select();
                        foreach (DataRow dr1 in FoundRows)
                        {
                            if (CustomerID == null)
                            {
                                CustomerID = "'" + dr1["CustomerID"].ToString() + "'";
                            }
                            else
                            {
                                CustomerID = CustomerID + "," + "'" + dr1["CustomerID"].ToString() + "'";
                            }

                        }
                        Session["CustomerID"] = null;
                        Session["CustomerID"] = CustomerID;

                    }
                    else
                    {
                        CbpGenerateReport.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
                    }
                }
                CbpGenerateReport.JSProperties["cpGeneratePDF"] = "GenratePDF";
            }
        }
        protected void Cexcelexportpanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Cexcelexportpanel.JSProperties["cpallcontract"] = null;
            Cexcelexportpanel.JSProperties["cpecnenable"] = null;
            Cexcelexportpanel.JSProperties["cpreleasetotal"] = null;
            Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = null;
            Cexcelexportpanel.JSProperties["cpvisibletrue"] = null;
            Cexcelexportpanel.JSProperties["cpallcontractpop"] = null;
            Cexcelexportpanel.JSProperties["cpecnenablepop"] = null;
            Cexcelexportpanel.JSProperties["cpproperties"] = null;
            //Cexcelexportpanel.JSProperties["cpGeneratePDF"] = null;
            string WhichCall = e.Parameter.Split('~')[0];
            if ((WhichCall != "allcontract_excel") && (WhichCall != "releasetotal_excel") && (WhichCall != "ecnenable_excel") && (WhichCall != "deliveryrpt_excel"))
            {
                string quater = "";
                if (ddlrptgeneration.SelectedItem.Value == "1")
                    quater = DdlQuatarlyDate.SelectedItem.Value.ToString();
                int remainingf = 0;
                Gridbind = CallStoreProc();
                if (Gridbind.Tables.Count > 0)
                    if (Gridbind.Tables[1].Rows.Count > 0)
                    {
                        allclient_excel = "";
                        release_excel = "";
                        allreadydeliver_excel = "";
                        ecnenable_excel = "";
                        //allcontract = Gridbind.Tables[1].Rows.Count.ToString();
                        //DataRow[] drfilter = Gridbind.Tables[1].Select("Email is not null");

                        Cexcelexportpanel.JSProperties["cpallcontract"] = Gridbind.Tables[6].Rows.Count.ToString();
                        Cexcelexportpanel.JSProperties["cpreleasetotal"] = Gridbind.Tables[1].Rows.Count.ToString();
                        //Cexcelexportpanel.JSProperties["cpecnenable"] = drfilter.Length.ToString();
                        Cexcelexportpanel.JSProperties["cpecnenable"] = Gridbind.Tables[9].Rows.Count.ToString();
                        Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = Gridbind.Tables[7].Rows.Count.ToString();
                        for (int m = 0; m < Gridbind.Tables[6].Rows.Count; m++)
                        {
                            if (allclient_excel == "")
                                allclient_excel = "'" + Gridbind.Tables[6].Rows[m]["client"].ToString() + "'";
                            else
                                allclient_excel = allclient_excel + "," + "'" + Gridbind.Tables[6].Rows[m]["client"].ToString() + "'";
                        }
                        for (int p = 0; p < Gridbind.Tables[1].Rows.Count; p++)
                        {
                            if (release_excel == "")
                                release_excel = "'" + Gridbind.Tables[1].Rows[p]["client"].ToString() + "'";
                            else
                                release_excel = release_excel + "," + "'" + Gridbind.Tables[1].Rows[p]["client"].ToString() + "'";
                        }

                        //string ecnenable = string.Join(",", drfilter);

                        for (int g = 0; g < Gridbind.Tables[9].Rows.Count; g++)
                        {
                            if (ecnenable_excel == "")
                                ecnenable_excel = "'" + Gridbind.Tables[9].Rows[g]["client"].ToString() + "'";
                            else
                                ecnenable_excel = ecnenable_excel + "," + "'" + Gridbind.Tables[9].Rows[g]["client"].ToString() + "'";
                        }


                        for (int b = 0; b < Gridbind.Tables[7].Rows.Count; b++)
                        {
                            if (allreadydeliver_excel == "")
                                allreadydeliver_excel = "'" + Gridbind.Tables[7].Rows[b]["contactid"].ToString() + "'";
                            else
                                allreadydeliver_excel = allreadydeliver_excel + "," + "'" + Gridbind.Tables[7].Rows[b]["contactid"].ToString() + "'";
                        }

                        if (WhichCall == "v1")
                        {

                        }
                        if (WhichCall == "v5")
                        {
                            if (TokenType != "E")
                            {
                                remainingf = Convert.ToInt32(Convert.ToInt32(Gridbind.Tables[9].Rows.Count.ToString()) - Convert.ToInt32(Gridbind.Tables[7].Rows.Count.ToString()));
                                Cexcelexportpanel.JSProperties["cpallcontractpop"] = Gridbind.Tables[7].Rows.Count.ToString();
                                Cexcelexportpanel.JSProperties["cpecnenablepop"] = remainingf;
                                //Cexcelexportpanel.JSProperties["cpallcontractpop"] = "3";
                                //Cexcelexportpanel.JSProperties["cpecnenablepop"] = "1";
                            }
                            else
                            {
                                //Cexcelexportpanel.JSProperties["cpGeneratePDF"] = "Y";
                            }
                        }
                        if (TokenType != "E")
                        {
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

                    }
            }
            else

                Cexcelexportpanel.JSProperties["cpproperties"] = WhichCall;

        }
        DataTable dtResult = new DataTable();

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            oGenericMethod = new GenericMethod();
            if (allclient_excel != "")
            {
                if (ddlGroup.SelectedItem.Value == "0")
                    dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone where Contactandemailandphone.cnt_internalId in (" + allclient_excel.ToString() + ")");
                else
                    dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,Email,PhoneNumber,case when (gpm_Description IS not null and gpm_code IS not null) then ltrim(rtrim(isnull(gpm_Description ,'')))+' [ '+ltrim(rtrim(isnull(gpm_code ,'')))+' ]' else 'NA' end as GroupDetail,gpm_Type as GroupType from(select cnt_internalId,ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone) as total Left outer join (select grp_contactId,gpm_Description,gpm_Type,gpm_code from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "') as Groupselection on Groupselection.grp_contactId=total.cnt_internalId where total.cnt_internalId in (" + allclient_excel.ToString() + ") order by ClientName");
                export(dtResult);
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "JScriptac1", "<script language='javascript'>alertcall();</script>");
        }
        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            oGenericMethod = new GenericMethod();
            if (ddlGroup.SelectedItem.Value == "0")
                dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone where Contactandemailandphone.cnt_internalId in (" + release_excel.ToString() + ")");
            else
                dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,Email,PhoneNumber,case when (gpm_Description IS not null and gpm_code IS not null) then ltrim(rtrim(isnull(gpm_Description ,'')))+' [ '+ltrim(rtrim(isnull(gpm_code ,'')))+' ]' else 'NA' end as GroupDetail,gpm_Type as GroupType from(select cnt_internalId,ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone) as total Left outer join (select grp_contactId,gpm_Description,gpm_Type,gpm_code from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "') as Groupselection on Groupselection.grp_contactId=total.cnt_internalId where total.cnt_internalId in (" + release_excel.ToString() + ") order by ClientName");
            export(dtResult);
        }
        protected void cmbExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            oGenericMethod = new GenericMethod();
            if (ddlGroup.SelectedItem.Value == "0")
                dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone where Contactandemailandphone.cnt_internalId in (" + ecnenable_excel.ToString() + ")");
            else
                dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,Email,PhoneNumber,case when (gpm_Description IS not null and gpm_code IS not null) then ltrim(rtrim(isnull(gpm_Description ,'')))+' [ '+ltrim(rtrim(isnull(gpm_code ,'')))+' ]' else 'NA' end as GroupDetail,gpm_Type as GroupType from(select cnt_internalId,ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone) as total Left outer join (select grp_contactId,gpm_Description,gpm_Type,gpm_code from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "') as Groupselection on Groupselection.grp_contactId=total.cnt_internalId where total.cnt_internalId in (" + ecnenable_excel.ToString() + ") order by ClientName");
            export(dtResult);
        }
        protected void cmbExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            oGenericMethod = new GenericMethod();

            if (ddlGroup.SelectedItem.Value == "0")
                dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone where Contactandemailandphone.cnt_internalId in (" + allreadydeliver_excel.ToString() + ")");
            else
                dtResult = oGenericMethod.GetDataTable("select ClientName,BranchDescription,Email,PhoneNumber,case when (gpm_Description IS not null and gpm_code IS not null) then ltrim(rtrim(isnull(gpm_Description ,'')))+' [ '+ltrim(rtrim(isnull(gpm_code ,'')))+' ]' else 'NA' end as GroupDetail,gpm_Type as GroupType from(select cnt_internalId,ClientName,BranchDescription,isnull(eml_email,'NA') as Email,isnull(phf_phoneNumber,'NA') as PhoneNumber from (Select cnt_internalId,ClientName,BranchDescription,eml_email,phf_phoneNumber from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ClientName,BranchDescription,eml_email from (select cnt_internalId,ltrim(rtrim(isnull(cnt_firstName,'')))+' '+ltrim(rtrim(isnull(cnt_middleName,'')))+' '+ltrim(rtrim(isnull(cnt_lastName,'')))+' [ '+ltrim(rtrim(ISNULL(cnt_UCC,' ')))+' ]' as ClientName,LTRIM(rtrim(isnull(branch_description,'')))+' [ '+LTRIM(rtrim(isnull(branch_code,'')))+' ]' as BranchDescription from tbl_master_contact,tbl_master_branch where cnt_branchid=branch_id) as Contact left outer join tbl_master_email on Contact.cnt_internalId=eml_cntId and eml_type='Official') as email) as Contactandemail left outer join tbl_master_phonefax on Contactandemail.cnt_internalId=phf_cntId and phf_type='Office') as Contactandemailandphone) as total Left outer join (select grp_contactId,gpm_Description,gpm_Type,gpm_code from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "') as Groupselection on Groupselection.grp_contactId=total.cnt_internalId where total.cnt_internalId in (" + allreadydeliver_excel.ToString() + ") order by ClientName");
            export(dtResult);


        }
        void export(DataTable dtExport)
        {
            ExcelFile objExcel = new ExcelFile();

            string searchCriteria = null;
            Converter oconverter = new Converter();
            searchCriteria = ddlGroup.SelectedItem.Text.ToString().Trim() + " wise For";
            if (ddlrptgeneration.SelectedItem.Value == "1")
                searchCriteria = searchCriteria + " Quarter " + DdlQuatarlyDate.SelectedItem.Text.ToString().Trim();
            if (ddlrptgeneration.SelectedItem.Value == "2")
                searchCriteria = searchCriteria + " Date Range " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To   " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            //dtExport = dtecnenale.Copy();
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "QSR_" + exlTime;
            strDownloadFileName = "~/Documents/";
            DataTable dtcompany = oDBEngine.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "QSR Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots
            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "150", "150", "150", "150", "150", "150" };
            string[] ColumnWidthSize = { "40", "30", "30", "20", "30", "30" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
        }
        protected void CbpSuggestISIN_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CbpSuggestISIN.JSProperties["cpallcontract"] = null;
            CbpSuggestISIN.JSProperties["cpecnenable"] = null;
            CbpSuggestISIN.JSProperties["cpreleasetotal"] = null;
            CbpSuggestISIN.JSProperties["cpdeliveryrpt"] = null;
            CbpSuggestISIN.JSProperties["cpvisibletrue"] = null;
            CbpSuggestISIN.JSProperties["cpallcontractpop"] = null;
            CbpSuggestISIN.JSProperties["cpecnenablepop"] = null;
            CbpSuggestISIN.JSProperties["cpGenSuccesssfully"] = null;
            //HiddenField_Client.Value = "";
            string WhichCallsend = e.Parameter.Split('~')[0];
            DataSet dsCrystal = new DataSet();
            string employeeId = txtSignature_hidden.Value;
            dsCrystal = CallStoreProc();
            if (WhichCallsend == "remain")
            {
                HiddenField_Client.Value = dsCrystal.Tables[8].Rows[0][0].ToString();
                dsCrystal = CallStoreProc();
            }
            byte[] logoinByte;
            byte[] SignatureinByte;
            dsCrystal.Tables[1].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            dsCrystal.Tables[1].Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
            dsCrystal.Tables[1].Columns.Add("Status", System.Type.GetType("System.String"));
            //Session["CustomerID"] = null;
            if (ChkLogo.Checked == true)
            {
                DataTable dtcmp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
                if (oDbConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo_" + dtcmp.Rows[0][0].ToString() + ".bmp"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

                }
                else
                {

                    for (int i = 0; i < dsCrystal.Tables[1].Rows.Count; i++)
                    {
                        dsCrystal.Tables[1].Rows[i]["Image"] = logoinByte;
                    }
                }
            }
            if (ChkSignatory.Checked == true)
            {
                if (oDbConverter.getSignatureImage(employeeId, out SignatureinByte, "NSE") == 1)
                {
                    for (int i = 0; i < dsCrystal.Tables[1].Rows.Count; i++)
                    {
                        dsCrystal.Tables[0].Rows[i]["Signature"] = SignatureinByte;
                        dsCrystal.Tables[0].Rows[i]["Status"] = "a";
                    }
                }
            }
            else
            {
                for (int i = 0; i < dsCrystal.Tables[1].Rows.Count; i++)
                {
                    dsCrystal.Tables[1].Rows[i]["Status"] = "This is an electronicaly generated statement that requires no Signature. *";
                }
            }
            ////using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            ////{
            //using (SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmdDigital = new SqlCommand("cdsl_EmployeeName", con1);
            //    cmdDigital.CommandType = CommandType.StoredProcedure;
            //    cmdDigital.Parameters.AddWithValue("@ID", txtdigitalName_hidden.Text.Split('~')[0].ToString().Trim());
            //    cmdDigital.CommandTimeout = 0;
            //    SqlDataAdapter daDigital = new SqlDataAdapter();
            //    daDigital.SelectCommand = cmdDigital;
            //    daDigital.Fill(DigitalSignatureDs);
            //}
            DigitalSignatureDs = oFAReportsOther.cdsl_EmployeeName(Convert.ToString(txtdigitalName_hidden.Text.Split('~')[0]));
            dsCrystal.Tables[1].Columns.Add("branch", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("signname", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("internalid", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("branchid", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("password", System.Type.GetType("System.String"));

            dsCrystal.Tables[1].Columns.Add("companyname", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("compadd", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("compadd1", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("compphone", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("compemail", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("comppan", System.Type.GetType("System.String"));

            dsCrystal.Tables[1].Columns.Add("CompilanceofficerName", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("CompilanceofficerPhone", System.Type.GetType("System.String"));
            dsCrystal.Tables[1].Columns.Add("CompilanceofficerEmail", System.Type.GetType("System.String"));


            string cmpname = dsCrystal.Tables[0].Rows[0]["cmpname"].ToString();
            string campadd = dsCrystal.Tables[0].Rows[0]["cmpaddress"].ToString();
            string cmpadd1 = dsCrystal.Tables[0].Rows[0]["cmpaddress1"].ToString();
            string phone = dsCrystal.Tables[0].Rows[0]["cmpphno"].ToString();
            string email = dsCrystal.Tables[0].Rows[0]["cmpemail"].ToString();
            string pan = dsCrystal.Tables[0].Rows[0]["cmppanno"].ToString();
            string compliancename = dsCrystal.Tables[0].Rows[0]["CompilanceofficerName"].ToString();
            string compliancephone = dsCrystal.Tables[0].Rows[0]["CompilanceofficerPhone"].ToString();
            string complianceemail = dsCrystal.Tables[0].Rows[0]["CompilanceofficerEmail"].ToString();
            for (int M = 0; M < dsCrystal.Tables[1].Rows.Count; M++)
            {
                dsCrystal.Tables[1].Rows[M]["companyname"] = cmpname.ToString();
                dsCrystal.Tables[1].Rows[M]["compadd"] = campadd.ToString();
                dsCrystal.Tables[1].Rows[M]["compadd1"] = cmpadd1.ToString();
                dsCrystal.Tables[1].Rows[M]["compphone"] = phone.ToString();
                dsCrystal.Tables[1].Rows[M]["compemail"] = email.ToString();
                dsCrystal.Tables[1].Rows[M]["comppan"] = pan.ToString();

                dsCrystal.Tables[1].Rows[M]["CompilanceofficerName"] = compliancename.ToString();
                dsCrystal.Tables[1].Rows[M]["CompilanceofficerPhone"] = compliancephone.ToString();
                dsCrystal.Tables[1].Rows[M]["CompilanceofficerEmail"] = complianceemail.ToString();
            }
            //dsCrystal.Tables[1].Rows[M]["clompanyname"] = cmpname.ToString();
            //dsCrystal.Tables[1].Rows[0]["compadd"] = dsCrystal.Tables[0].Rows[0]["cmpaddress"].ToString();
            //dsCrystal.Tables[1].Rows[0]["compadd1"] = dsCrystal.Tables[0].Rows[0]["cmpaddress1"].ToString();
            //dsCrystal.Tables[1].Rows[0]["compphone"] = dsCrystal.Tables[0].Rows[0]["cmpphno"].ToString();
            //dsCrystal.Tables[1].Rows[0]["compemail"] = dsCrystal.Tables[0].Rows[0]["cmpemail"].ToString();
            //dsCrystal.Tables[1].Rows[0]["comppan"] = dsCrystal.Tables[0].Rows[0]["cmppanno"].ToString();

            dsCrystal.Tables[1].Rows[0]["branch"] = DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString();
            dsCrystal.Tables[1].Rows[0]["signname"] = DigitalSignatureDs.Tables[0].Rows[0]["signName"].ToString();
            dsCrystal.Tables[1].Rows[0]["internalid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString();
            dsCrystal.Tables[1].Rows[0]["branchid"] = DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString();
            dsCrystal.Tables[1].Rows[0]["password"] = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();
            string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

            tmpPdfPath = string.Empty;
            ReportPath = string.Empty;
            signPath = string.Empty;
            finalResult = string.Empty;

            digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();

            // DBEngine oDBEngine = new DBEngine(null);
            if (txtdigitalName_hidden.Text != "")
            {
                TokenType =
                oDBEngine.GetDataTable(@"Select Isnull(DigitalSignature_Type,'N') from Master_DigitalSignature 
                             Where DigitalSignature_ID=" + txtdigitalName_hidden.Text.ToString().Split('~')[0].ToString()).Rows[0][0].ToString();
            }
            if (TokenType == "E")
            {
                tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\EToken\");
            }
            else
            {
                //DigiEmpDetail = txtdigitalName_hidden.Text.ToString().Split('~')[1].ToString().Split('[')[0].Trim();
                tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
            }

            ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\QSR1.rpt");
            signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + txtdigitalName_hidden.Text.Split('~')[0].ToString().Trim() + ".pfx";
            signPdfPath = oDbConverter.DirectoryPath(out VirtualPath);
            finalResult = generateindivisualPdf(ICEXReportDocument, dsCrystal, digitalSignaturePassword,
                               signPath, ReportPath
                             , tmpPdfPath, Session["LastCompany"].ToString(), signPdfPath, VirtualPath,
                             HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), txtdigitalName_hidden.Text.Split('~')[0].ToString().Trim());
            if (TokenType != "E")
            {
                if (finalResult == "Success")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript24", "<script language='javascript'>Page_Load();</script>");
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Email Sent Successfully');", true);
                    //return 6;

                }
                if (finalResult == "")
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript22", "<script language='javascript'>Page_Load();</script>");
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Error on sending Try Again !!');", true);

                }

                Gridbind = CallStoreProc();
                int remainingfs = 0;
                //DataRow[] drfilter = Gridbind.Tables[1].Select("Email is not null");
                CbpSuggestISIN.JSProperties["cpallcontract"] = Gridbind.Tables[6].Rows.Count.ToString();
                CbpSuggestISIN.JSProperties["cpreleasetotal"] = Gridbind.Tables[1].Rows.Count.ToString();
                //CbpSuggestISIN.JSProperties["cpecnenable"] = drfilter.Length.ToString();
                CbpSuggestISIN.JSProperties["cpecnenable"] = Gridbind.Tables[9].Rows.Count.ToString();
                CbpSuggestISIN.JSProperties["cpdeliveryrpt"] = Gridbind.Tables[7].Rows.Count.ToString();
                string WhichCall = e.Parameter.Split('~')[0];

                remainingfs = Convert.ToInt32(Convert.ToInt32(Gridbind.Tables[9].Rows.Count.ToString()) - Convert.ToInt32(Gridbind.Tables[7].Rows.Count.ToString()));

                CbpSuggestISIN.JSProperties["cpallcontractpops"] = Gridbind.Tables[7].Rows.Count.ToString();
                CbpSuggestISIN.JSProperties["cpecnenablepops"] = remainingfs;
                //Cexcelexportpanel.JSProperties["cpallcontractpop"] = "3";
                //Cexcelexportpanel.JSProperties["cpecnenablepop"] = "1";
            }
            else
            {
                if (finalResult == "Success")
                    CbpSuggestISIN.JSProperties["cpGenSuccesssfully"] = "Y";
                else
                    CbpSuggestISIN.JSProperties["cpGenSuccesssfully"] = "N";
            }


        }
        DataSet CallStoreProc()
        {
            string employeeId = txtSignature_hidden.Value;
            DataSet dsCrystal = new DataSet();
            string GRPTYPE = string.Empty;
            string CLIENTS = string.Empty;
            string Groupby = string.Empty;
            string ChkDoNPrintSecurities = string.Empty;
            string ChkConsiderEntrPeriod = string.Empty;
            string FORDATE = string.Empty;
            string Fromdatefrmpage = string.Empty;
            string Todatefrmpage = string.Empty;
            //  string employeeId = txtSignature_hidden.Value;

            if (HiddenField_Client.Value.ToString().Trim() == "")
            {
                if (rdbClientALL.Checked)
                {
                    CLIENTS = "ALL";
                }
                else
                {
                    CLIENTS = HiddenField_Client.Value.ToString().Trim();
                }
            }
            else
                CLIENTS = HiddenField_Client.Value.ToString().Trim();
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GRPTYPE = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    Groupby = "ALL";

                }
                else
                {
                    Groupby = HiddenField_Branch.Value.ToString().Trim();
                }

            }
            else
            {
                GRPTYPE = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    Groupby = "ALL";
                }
                else
                {
                    Groupby = HiddenField_Group.Value.ToString().Trim();
                }

            }

            if (ChkDoNotPrintSecurities.Checked)
            {
                ChkDoNPrintSecurities = "Y";
            }
            else
            {
                ChkDoNPrintSecurities = "N";
            }

            if (ddlrptgeneration.SelectedItem.Value == "1")
            {
                if (ChkConsiderEntirePeriod.Checked)
                {
                    ChkConsiderEntrPeriod = "Y";
                }
                else
                {
                    ChkConsiderEntrPeriod = "N";
                }
                FORDATE = DdlQuatarlyDate.SelectedItem.Text.ToString().Trim();
                Fromdatefrmpage = "";
                Todatefrmpage = "";
            }
            else
            {
                Fromdatefrmpage = dtFrom.Text.Split('-')[2] + "-" + dtFrom.Text.Split('-')[1] + "-" + dtFrom.Text.Split('-')[0];
                Todatefrmpage = dtTo.Text.Split('-')[2] + "-" + dtTo.Text.Split('-')[1] + "-" + dtTo.Text.Split('-')[0];
                ChkConsiderEntrPeriod = "N";
                FORDATE = "";
            }



            if (chkBothPrint.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";
            dsCrystal = oFAReportsOther.Report_QuatarlyStatement(
                Convert.ToString(Session["LastFinYear"]),
                 Convert.ToString(Session["LastCompany"]),
                 Convert.ToString(CLIENTS),
                 Convert.ToString(GRPTYPE),
                 Convert.ToString(Groupby),
                 Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]),
                 Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                 Convert.ToString(txtHeader_hidden.Value),
                 Convert.ToString(txtFooter_hidden.Value),
                 Convert.ToString(ChkDoNPrintSecurities),
                   Convert.ToString(HttpContext.Current.Session["usersegid"]),
                     Convert.ToString(ChkConsiderEntrPeriod),
                 Convert.ToString(FORDATE),
                 Convert.ToString(Fromdatefrmpage),
                   Convert.ToString(Todatefrmpage)
                );

            //if (chkBothPrint.Checked == true)
            //    SingleDouble = "D";
            //else
            SingleDouble = "S";
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText = "[Report_QuatarlyStatement]";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());
            //cmd.Parameters.AddWithValue("@COMPANYID", Session["LastCompany"].ToString());
            ////cmd.Parameters.AddWithValue("@CLIENTS", Session["CustomerID"]);

            //if (ddlGroup.SelectedItem.Value.ToString() == "0")
            //{
            //    cmd.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
            //    if (rdbranchAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", "ALL");

            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", HiddenField_Branch.Value.ToString().Trim());
            //    }

            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@GRPTYPE", ddlgrouptype.SelectedItem.Text.ToString().Trim());
            //    if (rdddlgrouptypeAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", "ALL");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Groupby", HiddenField_Group.Value.ToString().Trim());
            //    }

            //}
            //cmd.Parameters.AddWithValue("@BRANCHID", HttpContext.Current.Session["userbranchHierarchy"].ToString());
            //cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            //cmd.Parameters.AddWithValue("@Header", txtHeader_hidden.Value);
            //cmd.Parameters.AddWithValue("@Footer", txtFooter_hidden.Value);

            //if (ChkDoNotPrintSecurities.Checked)
            //{
            //    cmd.Parameters.AddWithValue("@ChkDoNotPrintSecurities", "Y");
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@ChkDoNotPrintSecurities", "N");
            //}
            //cmd.Parameters.AddWithValue("@dpid", HttpContext.Current.Session["usersegid"].ToString());
            //if (ddlrptgeneration.SelectedItem.Value == "1")
            //{
            //    if (ChkConsiderEntirePeriod.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ChkConsiderEntirePeriod", "Y");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@ChkConsiderEntirePeriod", "N");
            //    }
            //    cmd.Parameters.AddWithValue("@FORDATE", DdlQuatarlyDate.SelectedItem.Text.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Fromdatefrmpage", "");
            //    cmd.Parameters.AddWithValue("@Todatefrmpage", "");
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@Fromdatefrmpage", dtFrom.Text.Split('-')[2] + "-" + dtFrom.Text.Split('-')[1] + "-" + dtFrom.Text.Split('-')[0]);
            //    cmd.Parameters.AddWithValue("@Todatefrmpage", dtTo.Text.Split('-')[2] + "-" + dtTo.Text.Split('-')[1] + "-" + dtTo.Text.Split('-')[0]);
            //    cmd.Parameters.AddWithValue("@ChkConsiderEntirePeriod", "N");
            //    cmd.Parameters.AddWithValue("@FORDATE", "");
            //}
            //cmd.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd;
            //Adap.Fill(dsCrystal);
            //  cmd.Dispose();
            //  con.Dispose();
            // GC.Collect();
            //HiddenField_Client.Value = "";
            HiddenField_Client.Value = "";
            return dsCrystal;
        }
    }

}