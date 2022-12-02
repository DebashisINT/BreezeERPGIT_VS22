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
    public partial class Reports_frmReport_TransactionSingleClientNSDL : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string companyId;
        string dp = "NSDL";
        private static DataTable DT = new DataTable();
        static string isinid = "", cmpid = "", SettlementID = "";
        static int counter = 0, pageindex = 0, totolRecord = 0;
        PagedDataSource pds = new PagedDataSource();
        private int Repcounter = 0;
        static int BenType;
        static string BenAccId;
        private DataTable dtBenTypeSubtype = new DataTable();
        static string stdate, endDate;
        string data;
        static string Clients;

        //---------For Sending Email
        ExcelFile objExcel = new ExcelFile();
        string EmailHTML = "";
        Converter oconverter = new Converter();
        static DataTable dtTemp = new DataTable();

        DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//

            Page.RegisterStartupScript("myScript", "<script language=JavaScript>test();</script>");
            if (!IsPostBack)
            {
                counter = 0;
                bindFrmToDate();
                txtName.Attributes.Add("onkeyup", "CallAjax(this,'NSDLTransaction',event)");
                txtISIN.Attributes.Add("onkeyup", "CallAjax(this,'NSDLTransactionISIN',event)");
                txtSettlement.Attributes.Add("onkeyup", "CallAjax(this,'NSDLTransactionSettlement',event)");

                list.Style["display"] = "none";
                norecord.Style["display"] = "none";
                tblpage.Style["display"] = "none";


                //*
                Page.RegisterStartupScript("myScripthid", "<script language=JavaScript>hidesearch();</script>");
                cmpid = Request.QueryString["benaccno"].ToString();
                norecord.Visible = false;
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();



                //*
            }



            //if (rbUser.SelectedIndex == 1 && txtName_hidden.Text.Trim() != "")
            //{
            //    // cmpid = txtName_hidden.Text;
            //    cmpid = Clients;
            //}
            //else if (rbUser.SelectedIndex == 0)
            //{
            //    cmpid = "";
            //}

            if (rbISIN.SelectedIndex == 1 && txtISIN_hidden.Text.Trim() != "")
            {
                isinid = txtISIN_hidden.Text;
            }
            else if (rbISIN.SelectedIndex == 0)
            {
                isinid = "";
            }

            if (rbSettlement.SelectedIndex == 1 && txtSettlement_hidden.Text.Trim() != "")
            {
                SettlementID = txtSettlement_hidden.Text;
            }
            else if (rbSettlement.SelectedIndex == 0)
            {
                SettlementID = "";
            }

            if (counter != 0 && RBReportType.SelectedIndex == 1)
                showCrystalReport();
            counter = counter + 1;

            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);

        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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

                        if (idlist[0] == "Clients")
                        {
                            str = AcVal[0];

                        }
                        else
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                    else
                    {

                        if (idlist[0] == "Clients")
                        {
                            str += "," + AcVal[0];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }

            }

            if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str1;
            }


        }
        void bindFrmToDate()
        {
            int transMonth, transYear;

            if (oDBEngine.GetDate().Month == 1)
            {
                transMonth = 12;
                transYear = oDBEngine.GetDate().Year - 1;
            }
            else
            {
                transMonth = oDBEngine.GetDate().Month - 1;
                transYear = oDBEngine.GetDate().Year;
            }

            //DateTime firstDay = new DateTime(transYear, transMonth, 1);
            //DateTime lastDayOfMonth = firstDay.AddMonths(1).AddTicks(-1);
            //string month = String.Format("{0:MMMM}", lastDayOfMonth);
            //string date = String.Format("{0:MM/dd/yyyy}", lastDayOfMonth);
            //string[] dateSplit = date.Split('/');


            DateTime firstDay = DateTime.Today.AddDays(-30);
            DateTime lastDayOfMonth = DateTime.Today;
            string month = String.Format("{0:MMMM}", lastDayOfMonth);
            string date = String.Format("{0:MM/dd/yyyy}", lastDayOfMonth);
            string[] dateSplit = date.Split('/');



            txtendDate.Text = dateSplit[1] + " " + month + " " + dateSplit[2];
            //txtendDate.Value = dateSplit[1] + " " + month + " " + dateSplit[2];

            month = String.Format("{0:MMMM}", firstDay);
            date = String.Format("{0:MM/dd/yyyy}", firstDay);
            dateSplit = date.Split('/');
            txtstartDate.Text = "01" + " " + month + " " + dateSplit[2];
            //txtstartDate.Text = dateSplit[1] + " " + month + " " + dateSplit[2];
            //txtstartDate.Text = dateSplit[1] + " " + month + " 2009";// +dateSplit[2];



            //txtstartDate.Value = dateSplit[1] + " " + month + " " + dateSplit[2];

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            norecord.Visible = false;
            //CurrentPage = 0;
            if (RBReportType.SelectedIndex == 1)
            {

                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                showCrystalReport();

            }
            else
            {

                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);

        }
        public void showCrystalReport()
        {

            string select, where;
            //string stdate, endDate;
            stdate = txtstartDate.Text + " 00:00:00";
            endDate = txtendDate.Text + " 23:59:59";
            companyId = Session["LastCompany"].ToString();

            DataSet transDs = new DataSet();
            DataSet holdingDs = new DataSet();
            DataTable dtDate = oDBEngine.GetDataTable("master_finyear", "Finyear_id", "Finyear_Code='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (convert(datetime,'" + txtstartDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105)) and (convert(datetime,'" + txtendDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105))");
            if (dtDate.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    // SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransactionReport", con);
                    SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransactionReportSingleClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stdate", stdate);
                    cmd.Parameters.AddWithValue("@eddate", endDate);
                    cmd.Parameters.AddWithValue("@compID", companyId);
                    cmd.Parameters.AddWithValue("@dp", dp);


                    if (cmpid != "")
                    {

                        cmd.Parameters.AddWithValue("@benAccNo", cmpid);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@benAccNo", "na");
                    }

                    if (isinid != "")
                    {

                        cmd.Parameters.AddWithValue("@isin", isinid);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@isin", "na");
                    }

                    if (SettlementID != "")
                    {

                        cmd.Parameters.AddWithValue("@settlement_id", SettlementID);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@settlement_id", "na");
                    }

                    //if (ASPxComboBox1.Value == "All")
                    //{
                    //    cmd.Parameters.AddWithValue("@bentype", "na");
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@bentype", ASPxComboBox1.Value);
                    //}
                    cmd.Parameters.AddWithValue("@branchid", HttpContext.Current.Session["userbranchHierarchy"].ToString());



                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(transDs);

                    SqlCommand cmdHolding = new SqlCommand("sp_ShowNsdlTransactionHoldingReport", con);
                    cmdHolding.CommandType = CommandType.StoredProcedure;
                    cmdHolding.Parameters.AddWithValue("@stdate", txtendDate.Text + " 00:00:00");
                    cmdHolding.Parameters.AddWithValue("@eddate", txtendDate.Text + " 23:59:59");

                    if (cmpid != "")
                    {

                        cmdHolding.Parameters.AddWithValue("@benAccNo", cmpid);

                    }
                    else
                    {
                        cmdHolding.Parameters.AddWithValue("@benAccNo", "na");
                    }

                    cmdHolding.CommandTimeout = 0;
                    SqlDataAdapter daHolding = new SqlDataAdapter();
                    daHolding.SelectCommand = cmdHolding;


                    daHolding.Fill(holdingDs);



                }


                for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
                {

                    if (k > 0)
                    {
                        if (transDs.Tables[0].Rows[k - 1]["NSDLISIN_Number"].ToString() == transDs.Tables[0].Rows[k]["NSDLISIN_Number"].ToString()
                            && transDs.Tables[0].Rows[k - 1]["NsdlTransaction_SettlementNumber"].ToString() == transDs.Tables[0].Rows[k]["NsdlTransaction_SettlementNumber"].ToString()
                            && transDs.Tables[0].Rows[k - 1]["AccountType"].ToString() == transDs.Tables[0].Rows[k]["AccountType"].ToString())
                        {
                            transDs.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k - 1]["NsdlTransaction_Quantity"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));

                        }

                        else
                        {
                            transDs.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));

                        }
                    }

                    else
                    {
                        transDs.Tables[0].Rows[0]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[0]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[0]["debit"].ToString()));



                    }





                }




                if (transDs.Tables[0].Rows.Count > 0)
                {


                    //transDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransaction.xsd");
                    //transDs.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransaction.xml");

                    //transDs.WriteXmlSchema("C:\\NsdlTransaction.xsd");
                    //transDs.WriteXml("C:\\NsdlTransaction.xml");


                    if (holdingDs.Tables[0].Rows.Count == 0)
                    {
                        DataRow dataRow = holdingDs.Tables[0].NewRow();
                        dataRow[0] = "00000";
                        dataRow[2] = "00000";
                        dataRow[3] = "00000";
                        dataRow[4] = "00000";
                        dataRow[5] = "00000";
                        dataRow[6] = "00000";
                        dataRow[7] = "00000";
                        dataRow[8] = "00000";
                        dataRow[9] = "00000";
                        holdingDs.Tables[0].Rows.Add(dataRow);

                    }
                    //holdingDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlHolding.xsd");
                    //holdingDs.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlHolding.xml");

                    //holdingDs.WriteXmlSchema("C:\\NsdlHolding.xsd");
                    //holdingDs.WriteXml("C:\\NsdlHolding.xml");





                    ReportDocument cdslTransctionReportDocu = new ReportDocument();

                    string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');






                    string path = Server.MapPath("..\\Reports\\NsdlTransaction.rpt");
                    cdslTransctionReportDocu.Load(path);

                    cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    cdslTransctionReportDocu.SetDataSource(transDs.Tables[0]);

                    cdslTransctionReportDocu.Subreports[0].SetDataSource(holdingDs);








                    cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NSDL Transaction");







                    transDs.Clear();
                    transDs.Dispose();
                    holdingDs.Clear();
                    holdingDs.Dispose();


                }
                else
                {
                    //norecord.Style["display"] = "display";
                    ReportDocument cdslTransctionReportDocu = new ReportDocument();
                    string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');
                    string path = Server.MapPath("..\\Reports\\NsdlTransaction.rpt");
                    cdslTransctionReportDocu.Load(path);
                    cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    cdslTransctionReportDocu.SetDataSource(transDs.Tables[1]);
                    cdslTransctionReportDocu.Subreports[0].SetDataSource(holdingDs);
                    cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NSDL Transaction");
                    transDs.Clear();
                    transDs.Dispose();
                    holdingDs.Clear();
                    holdingDs.Dispose();
                }

            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSdtval1", "alert('Date Is Outside Of Financial Year!!');", true);

            }

        }

        void bindDetails()
        {
            string select, where;
            //string stdate, endDate;
            stdate = txtstartDate.Text + " 00:00:00";
            endDate = txtendDate.Text + " 23:59:59";
            DT.Clear();
            DT.Dispose();

            totolRecord = 0;
            pageindex = 0;
            DataTable dtDate = oDBEngine.GetDataTable("master_finyear", "Finyear_id", "Finyear_Code='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (convert(datetime,'" + txtstartDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105)) and (convert(datetime,'" + txtendDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105))");
            if (dtDate.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    // SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransactionHeaderList", con);
                    SqlCommand cmd = new SqlCommand("ShowNsdlTransactionHeaderListSingleClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stdate", stdate);
                    cmd.Parameters.AddWithValue("@eddate", endDate);

                    if (cmpid != "")
                    {

                        cmd.Parameters.AddWithValue("@benAccNo", cmpid);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@benAccNo", "na");
                    }

                    if (isinid != "")
                    {

                        cmd.Parameters.AddWithValue("@isin", isinid);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@isin", "na");
                    }

                    if (SettlementID != "")
                    {

                        cmd.Parameters.AddWithValue("@settlement_id", SettlementID);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@settlement_id", "na");
                    }

                    //if (ASPxComboBox1.Value == "All")
                    //{
                    //    cmd.Parameters.AddWithValue("@bentype", "na");
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@bentype", ASPxComboBox1.Value);

                    //}

                    cmd.Parameters.AddWithValue("@userid", Session["userid"]);
                    cmd.Parameters.AddWithValue("@branchid", HttpContext.Current.Session["userbranchHierarchy"].ToString());

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(DT);
                }

                if (DT.Rows.Count > 0)
                {
                    totolRecord = DT.Rows.Count;
                    bindTopHeader(0);
                    pageing();
                    norecord.Visible = false;
                    listRecord.Text = pageindex + 1 + " of " + totolRecord + " Clients.";

                    if (DT.Rows.Count == 1)
                    {
                        tblpage.Style["display"] = "none";
                    }
                    else
                        tblpage.Style["display"] = "display";
                }

                else
                {
                    norecord.Visible = true;
                    list.Style["display"] = "none";
                    tblpage.Style["display"] = "none";
                    norecord.Style["display"] = "display";
                }

            }
            else
            {
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSdtshow", "test();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSdtval", "alert('Date Is Outside Of Financial Year!!');", true);

            }


        }

        protected void btnFirst(object sender, EventArgs e)
        {
            pageindex = 0;
            bindTopHeader(pageindex);
            pageing();
        }
        protected void btnPrevious(object sender, EventArgs e)
        {
            if (pageindex > 0)
            {
                pageindex = pageindex - 1;

            }

            bindTopHeader(pageindex);
            pageing();
        }
        protected void btnNext(object sender, EventArgs e)
        {
            if (pageindex < totolRecord)
            {
                pageindex = pageindex + 1;


            }

            bindTopHeader(pageindex);
            pageing();
        }
        protected void btnLast(object sender, EventArgs e)
        {
            pageindex = totolRecord - 1;
            bindTopHeader(pageindex);
            pageing();
        }

        void bindTopHeader(int i)
        {

            lblClientId.Text = DT.Rows[i]["NsdlClients_BenAccountID"].ToString();

            status.Text = DT.Rows[i]["NsdlClients_BeneficiaryStatus"].ToString();
            holders.Text = DT.Rows[i]["names"].ToString();

            BenAccId = DT.Rows[i]["NsdlClients_BenAccountID"].ToString().Trim();
            BenType = Convert.ToInt32(DT.Rows[i]["NsdlClients_BenType"]);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                SqlCommand cmd = new SqlCommand("sp_NsdlTransaction_FetchTypeSubtype_totaltrans", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stdate", stdate);
                cmd.Parameters.AddWithValue("@eddate", endDate);
                cmd.Parameters.AddWithValue("@BenAccNo", lblClientId.Text.Trim());
                cmd.Parameters.AddWithValue("@BenType", BenType);


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dtBenTypeSubtype);


            }
            category.Text = dtBenTypeSubtype.Rows[0]["TypeSubtype"].ToString();
            lblTotalTransction.Text = dtBenTypeSubtype.Rows[0]["totaltrans"].ToString();
            lblTotalTransction1.Text = lblTotalTransction.Text;


            oDBEngine.DeleteValue("Tmp_NSDL_Transaction", " Create_User=" + Session["userid"].ToString());

            ViewState["startRowIndex"] = 0;
            ViewState["pageSize"] = 30;
            ViewState["totalRecord"] = 0;
            ViewState["List"] = null;
            ViewState["prevIsin"] = "";
            ViewState["prevSett"] = "";




            //CurrentPage = 0;
            bindList();
        }

        void pageing()
        {
            if (pageindex == 0)
            {
                ASPxFirst.Enabled = false;
                ASPxPrevious.Enabled = false;
                ASPxNext.Enabled = true;
                ASPxLast.Enabled = true;
            }

            if (pageindex == totolRecord - 1)
            {

                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;
                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;
            }

            if (pageindex > 0 && pageindex < totolRecord - 1)
            {
                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;
                ASPxNext.Enabled = true;
                ASPxLast.Enabled = true;
            }

            if (totolRecord == 1)
            {
                ASPxFirst.Enabled = false;
                ASPxPrevious.Enabled = false;
                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;
                tblpage.Style["display"] = "none";
            }
            else
            {
                tblpage.Style["display"] = "display";
            }
            listRecord.Text = pageindex + 1 + " of " + totolRecord + " Client's Transactions.";
        }

        void bindList()
        {
            //string stdate, endDate;
            stdate = txtstartDate.Text + " 00:00:00";
            endDate = txtendDate.Text + " 23:59:59";
            DataTable List = new DataTable();
            List.Clear();
            DataTable dtDate = oDBEngine.GetDataTable("master_finyear", "Finyear_id", "Finyear_Code='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (convert(datetime,'" + txtstartDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105)) and (convert(datetime,'" + txtendDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105))");
            if (dtDate.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    //SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransactionISIN", con);
                    SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransaction", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stdate", stdate);
                    cmd.Parameters.AddWithValue("@eddate", endDate);
                    cmd.Parameters.AddWithValue("@benAccNo", lblClientId.Text.Trim());
                    cmd.Parameters.AddWithValue("@benType", BenType);

                    if (isinid != "")
                    {

                        cmd.Parameters.AddWithValue("@isin", isinid);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@isin", "na");
                    }
                    if (SettlementID != "")
                    {

                        cmd.Parameters.AddWithValue("@settlement_id", SettlementID);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@settlement_id", "na");
                    }

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(List);

                    DataColumn dc = new DataColumn("openingStatus", System.Type.GetType("System.String"));
                    List.Columns.Add(dc);

                    DataColumn dc1 = new DataColumn("ClosingStatus", System.Type.GetType("System.String"));
                    List.Columns.Add(dc1);

                    DataColumn dc2 = new DataColumn("Create_User", System.Type.GetType("System.String"));
                    List.Columns.Add(dc2);

                    DataColumn dc3 = new DataColumn("RowNo", System.Type.GetType("System.String"));
                    List.Columns.Add(dc3);






                    for (int k = 0; k < List.Rows.Count; k++)
                    {

                        if (k > 0)
                        {
                            if (List.Rows[k - 1]["NSDLISIN_Number"].ToString() == List.Rows[k]["NSDLISIN_Number"].ToString()
                                && List.Rows[k - 1]["NsdlTransaction_SettlementNumber"].ToString() == List.Rows[k]["NsdlTransaction_SettlementNumber"].ToString()
                                && List.Rows[k - 1]["AccountType"].ToString() == List.Rows[k]["AccountType"].ToString())
                            {
                                if (List.Rows[k]["AccountType"].ToString() == "F")
                                {
                                    List.Rows[k]["FreeQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["FreeQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                }

                                else if (List.Rows[k]["AccountType"].ToString() == "D")
                                {
                                    List.Rows[k]["DematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["DematQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                }

                                else if (List.Rows[k]["AccountType"].ToString() == "R")
                                {
                                    List.Rows[k]["RematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["RematQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                }

                                else if (List.Rows[k]["AccountType"].ToString() == "P")
                                {
                                    List.Rows[k]["PledgedQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["PledgedQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                }
                                //List.Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["NsdlTransaction_Quantity"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                            }

                            else
                            {
                                if (List.Rows[k]["AccountType"].ToString() == "F")
                                {
                                    List.Rows[k]["FreeQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningFreeBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                    List.Rows[k]["openingStatus"] = "F";
                                    List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Free )";
                                }

                                else if (List.Rows[k]["AccountType"].ToString() == "D")
                                {
                                    List.Rows[k]["DematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningDematBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                    List.Rows[k]["openingStatus"] = "D";
                                    List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Demat )";
                                }

                                else if (List.Rows[k]["AccountType"].ToString() == "R")
                                {
                                    List.Rows[k]["RematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningRematBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                    List.Rows[k]["openingStatus"] = "R";
                                    List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Remat )";
                                }

                                else if (List.Rows[k]["AccountType"].ToString() == "P")
                                {
                                    List.Rows[k]["PledgedQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningPledgedBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                    List.Rows[k]["openingStatus"] = "P";
                                    List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Pledged )";
                                }

                                if (List.Rows[k - 1]["AccountType"].ToString() == "F")
                                {
                                    List.Rows[k - 1]["ClosingStatus"] = "F";
                                }

                                else if (List.Rows[k - 1]["AccountType"].ToString() == "D")
                                {
                                    List.Rows[k - 1]["ClosingStatus"] = "D";
                                }

                                else if (List.Rows[k - 1]["AccountType"].ToString() == "R")
                                {
                                    List.Rows[k - 1]["ClosingStatus"] = "R";
                                }

                                else if (List.Rows[k - 1]["AccountType"].ToString() == "P")
                                {
                                    List.Rows[k - 1]["ClosingStatus"] = "P";
                                }
                                //List.Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                //List.Rows[k - 1]["ClosingStatus"] = "Y";
                                //List.Rows[k]["openingStatus"] = "Y";
                            }
                        }

                        else
                        {
                            if (List.Rows[0]["AccountType"].ToString() == "F")
                            {
                                List.Rows[0]["FreeQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningFreeBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                                List.Rows[0]["openingStatus"] = "F";
                                List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Free )";
                                //List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "F";
                            }
                            else if (List.Rows[0]["AccountType"].ToString() == "D")
                            {
                                List.Rows[0]["DematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningDematBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                                List.Rows[0]["openingStatus"] = "D";
                                List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Demat )";
                                //List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "D";
                            }
                            else if (List.Rows[0]["AccountType"].ToString() == "R")
                            {
                                List.Rows[0]["RematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningRematBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                                List.Rows[0]["openingStatus"] = "R";
                                List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Remat )";
                                //List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "R";
                            }
                            else if (List.Rows[0]["AccountType"].ToString() == "P")
                            {
                                List.Rows[0]["PledgedQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningPledgedBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                                List.Rows[0]["openingStatus"] = "P";
                                List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Pledged )";
                                //List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "P";
                            }

                            if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "F")
                            {
                                List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "F";
                            }

                            else if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "D")
                            {
                                List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "D";
                            }

                            else if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "R")
                            {
                                List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "R";
                            }

                            else if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "P")
                            {
                                List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "P";
                            }

                            //List.Rows[0]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                            //List.Rows[0]["openingStatus"] = "Y";
                            //List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "Y";
                        }


                        List.Rows[k]["Create_User"] = Session["userid"].ToString();
                        List.Rows[k]["RowNo"] = k + 1;


                    }

                    SqlBulkCopy sbc = new SqlBulkCopy(con);
                    sbc.DestinationTableName = "Tmp_NSDL_Transaction";

                    if (con.State.Equals(ConnectionState.Open))
                    {
                        con.Close();
                    }
                    con.Open();
                    sbc.WriteToServer(List);
                    con.Close();



                    if (List.Rows.Count > 0)
                    {
                        list.Style["display"] = "display";
                        tblpage.Style["display"] = "display";

                        ViewState["List"] = List;
                        ViewState["totalRecord"] = List.Rows.Count;


                        if (List.Rows.Count < (int)ViewState["pageSize"])
                        {
                            btnTransnNext.Visible = false;
                            btnTransnNext1.Visible = false;

                            btnTransPrevious.Visible = false;
                            btnTransPrevious1.Visible = false;
                        }
                        else
                        {

                            btnTransnNext.Visible = true;
                            btnTransnNext1.Visible = true;

                            btnTransnNext.Enabled = true;
                            btnTransnNext1.Enabled = true;

                            btnTransPrevious.Visible = true;
                            btnTransPrevious1.Visible = true;

                            btnTransPrevious.Enabled = false;
                            btnTransPrevious1.Enabled = false;

                        }

                        generateTable();
                    }
                    else
                    {
                        list.Style["display"] = "none";
                        tblpage.Style["display"] = "none";

                    }

                    List.Clear();
                    List.Dispose();

                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSdtval", "alert('Date Is Outside Of Financial Year!!');", true);

            }

            //if (List.Rows.Count > 0)
            //{
            //    list.Style["display"] = "display";
            //    tblInnerPage.Style["display"] = "display";
            //}
            //else
            //{
            //    list.Style["display"] = "none";
            //    tblInnerPage.Style["display"] = "none";

            //}

            //pds.DataSource = List.DefaultView;
            //pds.AllowPaging = true;
            //pds.PageSize = 10;
            //pds.CurrentPageIndex = CurrentPage;
            //ASPxFirstInner.Enabled = !pds.IsFirstPage;
            //ASPxNextInner.Enabled = !pds.IsLastPage;
            //ASPxPreviousInner.Enabled = !pds.IsFirstPage;
            //ASPxLastInner.Enabled = !pds.IsLastPage;



            //LastPage = pds.PageCount - 1;

            //DataList1.DataSource = pds;
            //DataList1.DataBind();
            //Label1.Text = "Page " + (CurrentPage + 1) + " of " + pds.PageCount + " Pages ( Total " + pds.DataSourceCount.ToString() + " Records Found ).";



        }

        void generateTable()
        {


            int startIndex, endIndex;

            startIndex = (int)ViewState["startRowIndex"];
            endIndex = startIndex + (int)ViewState["pageSize"];

            if (endIndex >= (int)ViewState["totalRecord"])
            {
                endIndex = (int)ViewState["totalRecord"];
            }


            lblTransction.Text = endIndex.ToString();
            lblTransction1.Text = lblTransction.Text;



            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand("sp_Nsdl_FetchTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", Session["userid"]);
                cmd.Parameters.AddWithValue("@startRowIndex", startIndex);
                cmd.Parameters.AddWithValue("@endIndex", endIndex);
                //cmd.Parameters.AddWithValue("@BenAccNo", BenAccId);



                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(RstTable);



            }
            dtTemp = RstTable;











            //////////////////////////////////////////////////


            String strHtml = String.Empty;


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            string prevIsin = "", prevSett = "", prevAccountType = "";


            int flag = 0;

            for (int k = 0; k < RstTable.Rows.Count; k++)
            {


                if (prevIsin == RstTable.Rows[k][3].ToString() && prevSett == RstTable.Rows[k][6].ToString()
                && prevAccountType == RstTable.Rows[k]["AccountType"].ToString())
                {
                    flag = flag + 1;

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][12].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][14].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][16].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][18].ToString())) + "</b></td></tr>";
                    }




                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td >" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";

                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";





                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";

                    }


                }
                else
                {


                    prevIsin = RstTable.Rows[k][3].ToString();
                    prevSett = RstTable.Rows[k][6].ToString();
                    prevAccountType = RstTable.Rows[k]["AccountType"].ToString();

                    strHtml += "<tr style=\"background-color: #FDE9D9\"><td>ISIN:</td><td><b>" + RstTable.Rows[k][3].ToString() + "</b></td>";
                    strHtml += "<td >Security Name:</td>";

                    string ISINName = RstTable.Rows[k]["NsdlISIN_Name"].ToString();

                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                    {

                        ISINName = ISINName.Replace("( Free )", "");
                    }


                    strHtml += "<td ><b>" + ISINName + "</b></td>";
                    strHtml += "<td></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k][6].ToString() + "</b></td></tr>";


                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td><b>Ref. No.</b></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningFreeBalance"])) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningPledgedBalance"].ToString())) + "</b></td></tr>";
                    }



                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td>" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";

                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";





                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";

                    }






                }

            }



            strHtml += "</table>";

            display.InnerHtml = strHtml;
            // EmailHTML = strHtml;

            ////ViewState["prevIsin"] = prevIsin;
            ///ViewState["prevSett"] = prevSett; 

            ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);


        }

        //protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    string isin;
        //    isin = DataList1.DataKeys[e.Item.ItemIndex].ToString();
        //    Repeater rep = (Repeater)e.Item.FindControl("Repeater1");

        //    Label opening = (Label)e.Item.FindControl("opening");
        //    Label TransactionSettlement = (Label)e.Item.FindControl("TransactionSettlement");

        //    string stdate, endDate;
        //    stdate = txtstartDate.Text + " 00:00:00";
        //    endDate = txtendDate.Text + " 23:59:59";
        //    DataTable RepISIN = new DataTable();

        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransactionISINDetail", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@stdate", stdate);
        //        cmd.Parameters.AddWithValue("@eddate", endDate);
        //        cmd.Parameters.AddWithValue("@benAccNo", lblClientId.Text.Trim());
        //        cmd.Parameters.AddWithValue("@benType", BenType);

        //        cmd.Parameters.AddWithValue("@isin", isin);

        //        if (TransactionSettlement.Text.Trim() == "")
        //        {
        //            cmd.Parameters.AddWithValue("@holdingSettlement", "na");

        //        }
        //        else
        //        {
        //            cmd.Parameters.AddWithValue("@holdingSettlement", TransactionSettlement.Text.Trim());
        //        }

        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(RepISIN);
        //    }

        //    if (RepISIN.Rows.Count > 0)
        //    {
        //        RepISIN.Rows[0]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(opening.Text.ToString()) + Convert.ToDecimal(RepISIN.Rows[0]["credit"].ToString()) - Convert.ToDecimal(RepISIN.Rows[0]["debit"].ToString()));
        //        for (int k = 1; k < RepISIN.Rows.Count; k++)
        //        {
        //            RepISIN.Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(RepISIN.Rows[k - 1]["NsdlTransaction_Quantity"].ToString()) + Convert.ToDecimal(RepISIN.Rows[k]["credit"].ToString()) - Convert.ToDecimal(RepISIN.Rows[k]["debit"].ToString()));
        //        }
        //    }

        //    rep.DataSource = RepISIN;
        //    rep.DataBind();


        //}
        //protected void btnFirstInner(object sender, EventArgs e)
        //{
        //    CurrentPage = 0;
        //    bindList();
        //}
        //protected void btnPreviousInner(object sender, EventArgs e)
        //{
        //    CurrentPage = CurrentPage - 1;
        //    bindList();
        //}
        //protected void btnNextInner(object sender, EventArgs e)
        //{
        //    CurrentPage = CurrentPage + 1;
        //    bindList();
        //}
        //protected void btnLastInner(object sender, EventArgs e)
        //{
        //    CurrentPage = LastPage;
        //    bindList();
        //}
        //protected void DataList1_DataBinding(object sender, EventArgs e)
        //{


        //    if (pds.PageCount > 1)
        //    {
        //        tblInnerPage.Visible = true;

        //    }
        //    else
        //    {
        //        tblInnerPage.Visible = false;

        //    }
        //}
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "lavender";
        }

        protected string ZeroCheck(string s)
        {
            //decimal d = Convert.ToDecimal(s);
            //if (d == 0)
            //    return " ";
            //else
            //{
            if ((s.IndexOf('.') == -1))     //whole number, no fractional part OR empty string
                return s;
            else
            {
                string[] arrS = s.Split('.');
                if (arrS[0] == "")
                    return "0" + s;
                else
                    return s;
            }

            //}
        }

        protected void btnTransnNext_Click(object sender, EventArgs e)
        {
            ViewState["startRowIndex"] = ((int)ViewState["startRowIndex"] + (int)ViewState["pageSize"]);


            //if ((int)ViewState["startRowIndex"] >= (int)ViewState["totalRecord"])
            if (((int)ViewState["totalRecord"] - (int)ViewState["startRowIndex"]) <= (int)ViewState["pageSize"])
            {
                btnTransnNext.Enabled = false;
                btnTransnNext1.Enabled = false;
            }
            else
            {
                btnTransnNext.Enabled = true;
                btnTransnNext1.Enabled = true;
            }
            btnTransPrevious.Enabled = true;
            btnTransPrevious1.Enabled = true;

            generateTable();

        }

        protected void btnTransPrevious_Click(object sender, EventArgs e)
        {
            ViewState["startRowIndex"] = (int)ViewState["startRowIndex"] - (int)ViewState["pageSize"];
            if ((int)ViewState["startRowIndex"] < 0)
            {

                ViewState["startRowIndex"] = 0;
            }

            if ((int)ViewState["startRowIndex"] == 0)
            {
                btnTransPrevious.Enabled = false;
                btnTransPrevious1.Enabled = false;
            }
            else
            {
                btnTransPrevious.Enabled = true;
                btnTransPrevious1.Enabled = true;
            }

            btnTransnNext.Enabled = true;
            btnTransnNext1.Enabled = true;
            generateTable();

        }
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            string disptbl = "";
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                //lblClientId.Text = DT.Rows[i]["NsdlClients_BenAccountID"].ToString();
                pageindex = i;
                bindTopHeader(pageindex);
                pageing();
                generateEmailTable();
                disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                disptbl += "<tr><td><table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"left\">Client ID:  " + lblClientId.Text + "</td><td align=\"left\"> Category:  " + category.Text + "</td><td align=\"left\">Status:  " + status.Text + "</td><td align=\"left\">Name Of Holders:  " + holders.Text + "</td></tr></table></td></tr>";
                disptbl += "<tr><td>";
                string emailbdy = disptbl + EmailHTML + "</td></tr></table>";
                string mailid = string.Empty;
                string dpid = string.Empty;
                string emlcnt = string.Empty;

                DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + DT.Rows[i]["NsdlClients_BenAccountId"] + "'");
                dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + DT.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_email  ", "  *  ", "eml_cntId='" + emlcnt + "'");

                //if (dtCnt.Rows.Count > 0)
                //{
                //    mailid = dtCnt.Rows[0]["eml_email"].ToString().Trim();
                //}
                string branchContact = DT.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                string billdate = oconverter.ArrangeDate2(txtstartDate.Value.ToString()) + " To " + oconverter.ArrangeDate2(txtendDate.Value.ToString());
                string Subject = "NSDL Transaction Report for  " + billdate;
                for (int k = 0; k < dtCnt.Rows.Count; k++)
                {
                    mailid = dtCnt.Rows[k]["eml_email"].ToString().Trim();
                    if (mailid.Length > 0)
                    {
                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                        {
                            pageindex = 0;
                            bindTopHeader(pageindex);
                            pageing();
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>hidesearch();</script>");
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                        }
                        else
                        {
                            if (DT.Rows.Count <= 1)
                            {
                                pageindex = 0;
                                bindTopHeader(pageindex);
                                pageing();
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>hidesearch();</script>");
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                            }

                        }
                    }
                    else
                    {
                        pageindex = 0;
                        bindTopHeader(pageindex);
                        pageing();
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript25", "<script>hidesearch();</script>");
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript59", "<script>alert('Mail id not found....');</script>");


                    }
                }

                EmailHTML = "";
                disptbl = "";
            }


        }


        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            export();
        }

        void export()
        {



            DataTable dtEx = new DataTable();


            dtEx.Columns.Add("Date");
            dtEx.Columns.Add("Ref. No.");
            dtEx.Columns.Add("Particulars");
            dtEx.Columns.Add("Credit");
            dtEx.Columns.Add("Debit");
            dtEx.Columns.Add("Current Balance");
            // DataRow row3 = dtEx.NewRow();
            //row3[0] = "Beneficiary AccountID: " + dt.Rows[i][0].ToString() + "   ||  " + "Beneficiary Name:  " + dt.Rows[i][1].ToString() + "  ||  " + "Second Holder:  " + dt.Rows[i][2].ToString() + "  ||  " + "Third Holder:  " + dt.Rows[i][3].ToString();
            //row3[1] = "Test";
            //dtEx.Rows.Add(row3);

            string prevIsin = "", prevSett = "", prevAccountType = "";
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                dtTemp.Clear();
                pageindex = i;
                bindTopHeader(pageindex);
                pageing();

                DataRow rowHead = dtEx.NewRow();
                rowHead[0] = "  ";
                rowHead[1] = "Test";
                dtEx.Rows.Add(rowHead);

                DataRow row9 = dtEx.NewRow();
                row9[0] = "Client ID: " + lblClientId.Text + " ..........||.......... Category:" + category.Text + " ..........||.......... Status:  " + status.Text + "..........||.......... Name Of Holders:  " + holders.Text;
                row9[1] = "Test";
                dtEx.Rows.Add(row9);

                generateExpTable();
                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {

                    if (prevIsin == dtTemp.Rows[k][3].ToString() && prevSett == dtTemp.Rows[k][6].ToString()
                    && prevAccountType == dtTemp.Rows[k]["AccountType"].ToString())
                    {
                        //flag = flag + 1;
                        DataRow row6 = dtEx.NewRow();

                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "F")
                        {
                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][12].ToString()));
                            dtEx.Rows.Add(row6);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][12].ToString())) + "</b></td></tr>";
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "D")
                        {

                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][14].ToString()));
                            dtEx.Rows.Add(row6);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][14].ToString())) + "</b></td></tr>";
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "R")
                        {

                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][16].ToString()));
                            dtEx.Rows.Add(row6);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][16].ToString())) + "</b></td></tr>";
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "P")
                        {
                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][18].ToString()));
                            dtEx.Rows.Add(row6);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][18].ToString())) + "</b></td></tr>";
                        }




                        DataRow row7 = dtEx.NewRow();
                        row7[0] = dtTemp.Rows[k][5].ToString();
                        row7[1] = dtTemp.Rows[k][11].ToString();
                        row7[2] = dtTemp.Rows[k]["Particulars"].ToString();
                        row7[3] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["credit"].ToString())));
                        row7[4] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["debit"].ToString())));



                        //strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                        //strHtml += "<td >" + RstTable.Rows[k][11].ToString() + "</td>";
                        //strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";

                        if (dtTemp.Rows[k]["AccountType"].ToString() == "F")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["FreeQty"].ToString())));
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "D")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["DematQty"].ToString())));
                        //  strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "R")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["RematQty"].ToString())));
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "P")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["PledgedQty"].ToString())));
                        // strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";
                        dtEx.Rows.Add(row7);


                        DataRow row8 = dtEx.NewRow();


                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "F")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingFreeBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";

                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "D")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingDematBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";

                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "R")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingRematBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";

                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "P")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingPledgedBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";

                        }


                    }
                    else
                    {







                        prevIsin = dtTemp.Rows[k][3].ToString();
                        prevSett = dtTemp.Rows[k][6].ToString();
                        prevAccountType = dtTemp.Rows[k]["AccountType"].ToString();
                        //strHtml += "<tr style=\"background-color: #FDE9D9\"><td>ISIN:</td><td><b>" + RstTable.Rows[k][3].ToString() + "</b></td>";
                        //strHtml += "<td >Security Name:</td>";


                        string ISINName = dtTemp.Rows[k]["NsdlISIN_Name"].ToString();
                        if (dtTemp.Rows[k]["AccountType"].ToString() == "F")
                        {

                            ISINName = ISINName.Replace("( Free )", "");
                        }


                        //row1[4] = "";

                        // strHtml += "<td ><b>" + ISINName + "</b></td>";
                        //strHtml += "<td></td>";
                        //strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k][6].ToString() + "</b></td></tr>";
                        DataRow row1 = dtEx.NewRow();
                        //row1[0] = "ISIN: ";
                        row1[0] = "ISIN: " + dtTemp.Rows[k][3].ToString() + "  ||   Security Name:" + ISINName + "  ||   Settlement No:" + dtTemp.Rows[k][6].ToString();
                        row1[1] = "Test";
                        //row1[1] = dtTemp.Rows[k][3].ToString();
                        //row1[2] = "Security Name: ";
                        //row1[3] = ISINName;
                        //row1[4] = "Settlement No:";
                        //row1[5] = dtTemp.Rows[k][6].ToString();
                        dtEx.Rows.Add(row1);

                        DataRow row2 = dtEx.NewRow();
                        row2[0] = "Date";
                        row2[1] = "Ref. No.";
                        row2[2] = "Particulars";
                        row2[3] = "Credit";
                        row2[4] = "Debit";
                        row2[5] = "Current Balance";
                        dtEx.Rows.Add(row2);

                        //strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                        //strHtml += "<td><b>Ref. No.</b></td>";
                        //strHtml += "<td colspan=2><b>Particulars</b></td>";
                        //strHtml += "<td align=\"right\"><b>Credit</b></td>";
                        //strHtml += "<td align=\"right\"><b>Debit</b></td>";
                        //strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";
                        DataRow row3 = dtEx.NewRow();
                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "F")
                        {
                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningFreeBalance"]));
                            dtEx.Rows.Add(row3);
                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningFreeBalance"])) + "</b></td></tr>";
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "D")
                        {

                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningDematBalance"].ToString()));
                            dtEx.Rows.Add(row3);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningDematBalance"].ToString())) + "</b></td></tr>";
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "R")
                        {
                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningRematBalance"].ToString()));
                            dtEx.Rows.Add(row3);
                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningRematBalance"].ToString())) + "</b></td></tr>";
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "P")
                        {
                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningPledgedBalance"].ToString()));
                            dtEx.Rows.Add(row3);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningPledgedBalance"].ToString())) + "</b></td></tr>";
                        }
                        DataRow row4 = dtEx.NewRow();
                        row4[0] = dtTemp.Rows[k][5].ToString();
                        row4[1] = dtTemp.Rows[k][11].ToString();
                        row4[2] = dtTemp.Rows[k]["Particulars"].ToString();
                        row4[3] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["credit"].ToString())));
                        row4[4] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["debit"].ToString())));

                        ////flag = flag + 1;
                        //strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                        //strHtml += "<td>" + RstTable.Rows[k][11].ToString() + "</td>";
                        //strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";

                        if (dtTemp.Rows[k]["AccountType"].ToString() == "F")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["FreeQty"].ToString())));
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "D")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["DematQty"].ToString())));
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "R")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["RematQty"].ToString())));
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "P")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["PledgedQty"].ToString())));
                        //strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";
                        dtEx.Rows.Add(row4);


                        DataRow row5 = dtEx.NewRow();

                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "F")
                        {
                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingFreeBalance"].ToString()));
                            dtEx.Rows.Add(row5);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";

                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "D")
                        {

                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingDematBalance"].ToString()));
                            dtEx.Rows.Add(row5);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";

                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "R")
                        {
                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingRematBalance"].ToString()));
                            dtEx.Rows.Add(row5);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";

                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "P")
                        {
                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingPledgedBalance"].ToString()));
                            dtEx.Rows.Add(row5);

                            //strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";

                        }






                    }

                }

            }


            pageindex = 0;
            bindTopHeader(pageindex);
            pageing();




            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "NSDL Transaction Report (From  " + oconverter.ArrangeDate2(txtstartDate.Value.ToString()) + " To " + oconverter.ArrangeDate2(txtendDate.Value.ToString()) + ")";
            dtReportHeader.Rows.Add(DrRowR1);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);



            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtEx, "NSDL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtEx, "NSDL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }



        }


        void generateExpTable()
        {


            int startIndex, endIndex;

            startIndex = 0;
            endIndex = (int)ViewState["totalRecord"];

            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand("sp_Nsdl_FetchTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", Session["userid"]);
                cmd.Parameters.AddWithValue("@startRowIndex", startIndex);
                cmd.Parameters.AddWithValue("@endIndex", endIndex);
                //cmd.Parameters.AddWithValue("@BenAccNo", BenAccId);



                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(RstTable);



            }
            dtTemp = RstTable;
        }
        void generateEmailTable()
        {


            int startIndex, endIndex;

            // startIndex = (int)ViewState["startRowIndex"];
            startIndex = 0;
            endIndex = (int)ViewState["totalRecord"];
            //endIndex = startIndex + (int)ViewState["pageSize"];

            //if (endIndex >= (int)ViewState["totalRecord"])
            //{
            //    endIndex = (int)ViewState["totalRecord"];
            //}


            lblTransction.Text = endIndex.ToString();
            lblTransction1.Text = lblTransction.Text;



            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand("sp_Nsdl_FetchTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", Session["userid"]);
                cmd.Parameters.AddWithValue("@startRowIndex", startIndex);
                cmd.Parameters.AddWithValue("@endIndex", endIndex);
                //cmd.Parameters.AddWithValue("@BenAccNo", BenAccId);



                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(RstTable);



            }
            dtTemp = RstTable;











            //////////////////////////////////////////////////


            String strHtml = String.Empty;


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            string prevIsin = "", prevSett = "", prevAccountType = "";


            int flag = 0;

            for (int k = 0; k < RstTable.Rows.Count; k++)
            {


                if (prevIsin == RstTable.Rows[k][3].ToString() && prevSett == RstTable.Rows[k][6].ToString()
                && prevAccountType == RstTable.Rows[k]["AccountType"].ToString())
                {
                    flag = flag + 1;

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][12].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][14].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][16].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][18].ToString())) + "</b></td></tr>";
                    }




                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td >" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";

                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";





                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";

                    }


                }
                else
                {


                    prevIsin = RstTable.Rows[k][3].ToString();
                    prevSett = RstTable.Rows[k][6].ToString();
                    prevAccountType = RstTable.Rows[k]["AccountType"].ToString();

                    strHtml += "<tr style=\"background-color: #FDE9D9\"><td>ISIN:</td><td><b>" + RstTable.Rows[k][3].ToString() + "</b></td>";
                    strHtml += "<td >Security Name:</td>";

                    string ISINName = RstTable.Rows[k]["NsdlISIN_Name"].ToString();

                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                    {

                        ISINName = ISINName.Replace("( Free )", "");
                    }


                    strHtml += "<td ><b>" + ISINName + "</b></td>";
                    strHtml += "<td></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k][6].ToString() + "</b></td></tr>";


                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td><b>Ref. No.</b></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningFreeBalance"])) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningPledgedBalance"].ToString())) + "</b></td></tr>";
                    }



                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td>" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";

                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";





                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";

                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";

                    }






                }

            }



            strHtml += "</table>";

            //display.InnerHtml = strHtml;
            EmailHTML = strHtml;

            ////ViewState["prevIsin"] = prevIsin;
            ///ViewState["prevSett"] = prevSett; 



        }

    }
}