using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_ContractNote : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.FAReportsOther objFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        string data;
        string path;
        string customerstr = "";
        string Contractstr = "";
        string StrID = "";
        string client = "";
        int j;
        int scaningStatus;
        string reporttype = "";
        string trdate = "";
        string abcd1 = "";
        string efgh = "";
        string sOutputFileName = "";
        protected string allcontract, ecnenable, deliveryrpt, remaining;
        DataTable dtnew = new DataTable();
        DataTable dtecnenale = new DataTable();
        static string idfilename = string.Empty;
        static string idfilepath = string.Empty;
        string id;
        public DataTable Dtecn
        {
            get { return (DataTable)Session["dtecn"]; }
            set { Session["dtcn"] = value; }
        }
        public string filename
        {
            get { return (string)Session["file"]; }
            set { Session["file"] = value; }
        }
        public string savefilepath
        {
            get { return (string)Session["filepath"]; }
            set { Session["filepath"] = value; }
        }
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
            CbpSuggestISIN.JSProperties["cpsuccessandfailmsg"] = null; ;
            CbpSuggestISIN.JSProperties["cpnorecord"] = null;
            if (!IsPostBack)
            {
                Session["Error"] = null;
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                FnDosPrint();
                Date();
                FnSelectionTypeBind();

                SegmentnameFetch();
                txtdigitalName.Visible = IsSignExists();
                if ((HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1") || (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4") || (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15") || (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19"))
                    dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_SettlementNumber='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7) + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                //dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_SettlementNumber='" + HttpContext.Current.Session["LastSettNo"].ToString() + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");

                //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "18")
                //{
                //    filldrp();

                //}

                if (dtnew.Rows.Count > 0)
                {
                    divdisplay();
                }
                if (txtdigitalName.Visible == true)
                {
                    td_msg.Visible = false;


                }
                else
                {
                    td_msg.Visible = true;

                }
                string tradedate = tdate();
                Hddndate.Value = tdate();

                txtdigitalName.Attributes.Add("onkeyup", "CallAjax(this,'SearchEmployeesNameWithDigitalSign',event)");
                Add_contractnoteExchange();

            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Add_contractnoteExchange()
        {

            if (Session["Segmentname"].ToString() == "NSE-FO")
            {
                //Add Into DDL Of Format
                DdlPrintType.Items.Insert(DdlPrintType.Items.Count, new ListItem("Contract Note Only (Exchange)", "5"));
                DdlPrintType1.Items.Insert(DdlPrintType1.Items.Count, new ListItem("Contract Note Only (Exchange)", "4"));
            }
        }


        protected void CbpSuggestISIN_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session["Error"] = null;
            DataTable dtallcontract = new DataTable();
            DataTable dtecnenable = new DataTable();
            DataTable dtdeliveryrpt = new DataTable();

            DataTable dtremaining = new DataTable();
            string WhichCall = e.Parameter.Split('~')[0];
            CbpSuggestISIN.JSProperties["cpallcontractpops"] = null;
            CbpSuggestISIN.JSProperties["cpecnenablepops"] = null;
            CbpSuggestISIN.JSProperties["cpdownloadcomplete"] = null;
            CbpSuggestISIN.JSProperties["cpsuccessandfailmsg"] = null;
            CbpSuggestISIN.JSProperties["cpnorecord"] = null;
            string dp = HiddenField_SegmentName.Value.ToString();
            string Type = DdlGeneRationType.SelectedItem.Value.ToString();
            ViewRegulatoryReportNameSpace.ViewRegulatoryReport view = new ViewRegulatoryReportNameSpace.ViewRegulatoryReport();
            string CompanyID = HttpContext.Current.Session["LastCompany"].ToString();
            string UserId = HttpContext.Current.Session["usersegid"].ToString();
            string all = "";
            string groupbytext = "";
            string sendtypeparameter = "";
            string Groupbyvalue = ddlGroupBy.SelectedItem.Value;
            string brkgflag = "";
            string strchknet = "";
            if (chknet.Checked == true)
                strchknet = "true";
            else
                strchknet = "false";

            if (chkBrokerage.Checked == true)
            {
                brkgflag = "True";
            }
            else
            {
                brkgflag = "False";
            }
            if ((ddlGroupBy.SelectedItem.Value == "Clients") || (ddlGroupBy.SelectedItem.Value == "Branch"))
            {
                if (RadioBtnOtherGroupByAll.Checked == true)// || RadioBtnGroupAll.Checked==true)
                {
                    all = "A";
                }
                if (RadioBtnOtherGroupBySelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                {
                    all = "S";
                }

                if (RadioBtnOtherGroupByallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                {
                    all = "D";
                }
            }
            if (ddlGroupBy.SelectedItem.Value == "Group")
            {
                groupbytext = ddlGroup.SelectedItem.Text.ToString().Trim();
                if (RadioBtnGroupAll.Checked == true)// || RadioBtnGroupAll.Checked==true)
                {
                    all = "A";
                }
                if (RadioBtnGroupSelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                {
                    all = "S";
                }

                if (RadioBtnGroupallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                {
                    all = "D";
                }
            }

            string CustomerID = HiddenField_Client.Value.ToString();
            string client = HiddenField_Client.Value.ToString();
            StrID = HiddenField_Client.Value.ToString();
            //if (Hddndate.Value == "")
            //{
            if (WhichCall != "html")

                Hddndate.Value = Convert.ToDateTime(dtFromDate.Value).ToString("yyyy-MM-dd");
            else
                Hddndate.Value = tdate();
            string[] strContractDate = Hddndate.Value.Split('#');

            string ContractNo = HiddenField_Contractnoteno.Value.ToString();
            string[,] AuthorisedCustomer = null;
            string AuthorisedCustomerName = "";

            AuthorisedCustomer = oDBEngine.GetFieldValue(" master_AuthorizedSignatory MAS,tbl_master_contact MC", "MC.cnt_firstName+' '+isnull(MC.cnt_MiddleName,'')+' '+MC.cnt_lastName", "MAS.AuthorizedSignatory_CompanyID='" + CompanyID + "' AND MAS.AuthorizedSignatory_SegmentID=" + UserId + " AND MAS.AuthorizedSignatory_EmployeeID=MC.cnt_internalID", 1, "AuthorizedSignatory_ID");
            for (int i = 0; i <= AuthorisedCustomer.Length - 1; i++)
            {
                AuthorisedCustomerName = "" + AuthorisedCustomerName + "</br>" + (string)AuthorisedCustomer[i, 0] + "";


            }
            AuthorisedCustomerName = AuthorisedCustomerName.Substring(5, AuthorisedCustomerName.Length - 5);

            string strFundpayputdate = date();
            string contract = "";
            string tradedate = "";
            DataSet dsData = new DataSet();
            string group = HiddenField_Group.Value;
            if (WhichCall == "html")
            {
                //string path = Server.MapPath(@"../KRA/ExportFiles/NEWKRA/") + "html.htm";
                if (RadioBtnOtherGroupBySelected.Checked == true)
                {
                    string datevariable = Convert.ToString(Convert.ToDateTime(dtFromDate.Value).ToString("yyyy-MM-dd") + "to" + Convert.ToDateTime(dtToDate.Value).ToString("yyyy-MM-dd")); ;
                    tradedate = datevariable.ToString();
                }
                else
                {
                    tradedate = strContractDate[0].ToString();
                }
                dsData = objFAReportsOther.Contract_Report14(
                    Convert.ToString(CompanyID),
                      Convert.ToString(UserId),
                      Convert.ToString(dp),
                      Convert.ToString(tradedate),
                      Convert.ToString(AuthorisedCustomerName),
                      Convert.ToString(all),
                      Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                      Convert.ToString(strFundpayputdate),
                      Convert.ToString(brkgflag),
                      Convert.ToString(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)),
                      Convert.ToString(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)),
                      Convert.ToString(HiddenField_Branch.Value),
                      Convert.ToString(HiddenField_Client.Value),
                      Convert.ToString(HiddenField_Group.Value),
                       Convert.ToString(groupbytext.ToString().Trim()),
                       "Print",
                       Convert.ToString(Groupbyvalue.ToString().Trim()),
                       "All",
                       "",
                       "",
                       "HTML",
                       Convert.ToString(ddlOutputtype.SelectedItem.Value)
                    );
                //   Convert.ToString(Session["userid"]));
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                //    SqlCommand cmd = new SqlCommand();
                //    cmd.Connection = con;
                //    cmd.CommandText = "[Contract_Report14]";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                //    cmd.Parameters.AddWithValue("@DpId", UserId);
                //    cmd.Parameters.AddWithValue("@dp", dp);
                //    if (RadioBtnOtherGroupBySelected.Checked==true)
                //    {
                //        string datevariable = Convert.ToString(Convert.ToDateTime(dtFromDate.Value).ToString("yyyy-MM-dd") + "to" + Convert.ToDateTime(dtToDate.Value).ToString("yyyy-MM-dd")); ;
                //        cmd.Parameters.AddWithValue("@tradedate", datevariable.ToString());
                //    }
                //    else

                //    cmd.Parameters.AddWithValue("@tradedate", strContractDate[0].ToString());
                //    cmd.Parameters.AddWithValue("@AuthorizeName", AuthorisedCustomerName);
                //    cmd.Parameters.AddWithValue("@Mode", all);
                //    cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                //    cmd.Parameters.AddWithValue("@strFundPayoutDate", strFundpayputdate);
                //    cmd.Parameters.AddWithValue("@BrkgFlag", brkgflag);
                //    cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                //    cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                //    cmd.Parameters.AddWithValue("@Branch", HiddenField_Branch.Value.ToString());
                //    cmd.Parameters.AddWithValue("@Customer", HiddenField_Client.Value.ToString());
                //    cmd.Parameters.AddWithValue("@Group", HiddenField_Group.Value.ToString());
                //    cmd.Parameters.AddWithValue("@Groupbytext", groupbytext.ToString().Trim());
                //    cmd.Parameters.AddWithValue("@Reporttype", "Print");
                //    cmd.Parameters.AddWithValue("@Groupbyvalue", Groupbyvalue.ToString().Trim());
                //    cmd.Parameters.AddWithValue("@sendtypeparameter", "All");
                //    cmd.Parameters.AddWithValue("@Employeename", "");
                //    cmd.Parameters.AddWithValue("@netammountchk", "");
                //    cmd.Parameters.AddWithValue("@pdforhtml", "HTML");
                //    cmd.Parameters.AddWithValue("@outputhtmltype", ddlOutputtype.SelectedItem.Value.ToString());


                //    cmd.CommandTimeout = 0;
                //    SqlDataAdapter da = new SqlDataAdapter();
                //    da.SelectCommand = cmd;
                //    da.Fill(dsData);
                //}
                if (dsData.Tables.Count > 0)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        //string exlDateTime = oDBEngine.GetDate(113).ToString();
                        //string exlTime = exlDateTime.Replace(":", "");
                        //exlTime = exlTime.Replace(" ", "");
                        //string fname = "Contractnote_" + exlTime;
                        //filename = fname + ".htm";//+ txtBatch.Text;
                        //savefilepath = @"ExportFiles/HTMLCONTRACTNOTE/" + filename; ///////////FILE SAVE INTO FOLDER
                        //path = Server.MapPath(@"../ExportFiles/HTMLCONTRACTNOTE/") + filename;///////////FILE SAVE INTO DATABASE

                        if (!Directory.Exists(Server.MapPath(@"../ExportFiles")))
                            Directory.CreateDirectory(Server.MapPath(@"../ExportFiles"));

                        if (!Directory.Exists(Server.MapPath(@"../ExportFiles/HTMLCONTRACTNOTE")))
                            Directory.CreateDirectory(Server.MapPath(@"../ExportFiles/HTMLCONTRACTNOTE"));

                        DataTable dthtml = new DataTable();
                        dthtml.Columns.Add("htmlfield", System.Type.GetType("System.String"));
                        dthtml.Columns.Add("clientid", System.Type.GetType("System.String"));
                        dthtml.Columns.Add("contractnoteno", System.Type.GetType("System.String"));
                        dthtml.Columns.Add("clientnamewithucc", System.Type.GetType("System.String"));
                        dthtml.Columns.Add("tradedate", System.Type.GetType("System.String"));
                        dthtml.Columns.Add("dp", System.Type.GetType("System.String"));
                        dthtml.TableName = "html";
                        DataSet dshtml = new DataSet();
                        //dshtml.Tables.Remove("html");
                        if (ddlOutputtype.SelectedItem.Value == "1")
                        {
                            string exlDateTime = oDBEngine.GetDate(113).ToString();
                            string exlTime = exlDateTime.Replace(":", "");
                            exlTime = exlTime.Replace(" ", "");
                            string fname = "Contractnote_" + exlTime;
                            filename = fname + ".htm";//+ txtBatch.Text;
                            savefilepath = @"ExportFiles/HTMLCONTRACTNOTE/" + filename; ///////////FILE SAVE INTO FOLDER
                            path = Server.MapPath(@"../ExportFiles/HTMLCONTRACTNOTE/") + filename;///////////FILE SAVE INTO DATABASE

                            using (StreamWriter sw = new StreamWriter(path, false))
                            {
                                DataRow dr = dsData.Tables[0].NewRow();
                                for (int puc = 0; puc < dsData.Tables.Count; puc++)
                                {
                                    dr["htmlfield"] = dsData.Tables[puc].Rows[0][0];
                                    dr["clientid"] = dsData.Tables[puc].Rows[0][1];
                                    dr["contractnoteno"] = dsData.Tables[puc].Rows[0][2];
                                    dr["clientnamewithucc"] = dsData.Tables[puc].Rows[0][3];
                                    dr["tradedate"] = dsData.Tables[puc].Rows[0][4];
                                    dr["dp"] = dsData.Tables[puc].Rows[0][5];
                                    dthtml.Rows.Add(dr.ItemArray);

                                }
                                dthtml.AcceptChanges();
                                foreach (DataRow dr1 in dthtml.Rows)
                                {
                                    for (int j = 0; j < 1; j++)
                                    {
                                        sw.Write(dr1[j]);
                                    }

                                    sw.Write("</br>" + sw.NewLine);
                                }
                            }
                            string tablename = "Trans_ExportFiles";
                            string fieldName = "ExportFiles_Segment,ExportFiles_Type,ExportFiles_Name,ExportFiles_Path,ExportFiles_CreateUser,ExportFiles_CreateDateTime ";
                            string fieldValue = "'" + Session["usersegid"].ToString() + "','htmlcontract','" + filename + "','" + savefilepath + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "'";
                            oGenericMethod = new BusinessLogicLayer.GenericMethod();
                            DataTable dtResult = oGenericMethod.CallGeneric_StoreProcedure("InsertIntoTable", "SPECIFICCOL~" + tablename + "~" + fieldName + "~" + fieldValue);
                            CbpSuggestISIN.JSProperties["cpdownloadcomplete"] = "yes";
                        }
                        else
                        {
                            int totalcount = 0;
                            int successcount = 0;
                            int failedcount = 0;
                            string countervalue = "";
                            DataRow dr = dsData.Tables[0].NewRow();
                            if (dsData.Tables.Count > 1)
                            {
                                totalcount = 1;
                                for (int puc1 = 0; puc1 < dsData.Tables.Count; puc1++)
                                {
                                    dr["htmlfield"] = dsData.Tables[puc1].Rows[0][0];
                                    dr["clientid"] = dsData.Tables[puc1].Rows[0][1];
                                    dr["contractnoteno"] = dsData.Tables[puc1].Rows[0][2];
                                    dr["clientnamewithucc"] = dsData.Tables[puc1].Rows[0][3];
                                    dr["tradedate"] = dsData.Tables[puc1].Rows[0][4];
                                    dr["dp"] = dsData.Tables[puc1].Rows[0][5];
                                    dthtml.Rows.Add(dr.ItemArray);

                                }
                            }
                            else
                            {
                                totalcount = Convert.ToInt32(dthtml.Rows.Count.ToString());
                                for (int puc2 = 0; puc2 < dsData.Tables[0].Rows.Count; puc2++)
                                {
                                    dr["htmlfield"] = dsData.Tables[0].Rows[puc2][0];
                                    dr["clientid"] = dsData.Tables[0].Rows[puc2][1];
                                    dr["contractnoteno"] = dsData.Tables[0].Rows[puc2][2];
                                    dr["clientnamewithucc"] = dsData.Tables[0].Rows[puc2][3];
                                    dr["tradedate"] = dsData.Tables[0].Rows[puc2][4];
                                    dr["dp"] = dsData.Tables[0].Rows[puc2][5];
                                    dthtml.Rows.Add(dr.ItemArray);

                                }
                            }
                            dthtml.AcceptChanges();
                            //totalcount = Convert.ToInt32(dthtml.Rows.Count.ToString());
                            for (int puc = 0; puc < dthtml.Rows.Count; puc++)
                            {
                                string exlDateTime = oDBEngine.GetDate(113).ToString();
                                string exlTime = exlDateTime.Replace(":", "");
                                exlTime = exlTime.Replace(" ", "");
                                string fname = "Contractnote_" + exlTime;
                                filename = fname + ".htm";//+ txtBatch.Text;
                                savefilepath = @"ExportFiles/HTMLCONTRACTNOTE/" + filename; ///////////FILE SAVE INTO FOLDER
                                path = Server.MapPath(@"../ExportFiles/HTMLCONTRACTNOTE/") + filename;///////////FILE SAVE INTO DATABASE

                                using (StreamWriter sw = new StreamWriter(path, false))
                                {

                                    //sw.Write(dsData.Tables[0].Rows[puc][0].ToString());
                                    if (dsData.Tables.Count > 1)
                                    {
                                        foreach (DataRow dr1 in dthtml.Rows)
                                        {
                                            for (int j = 0; j < 1; j++)
                                            {
                                                sw.Write(dr1[j]);
                                            }

                                            sw.Write("</br>" + sw.NewLine);
                                        }
                                    }
                                    else
                                        sw.Write(dsData.Tables[0].Rows[puc][0].ToString());

                                }
                                if (dsData.Tables.Count > 1)
                                {
                                    string strEmailSubject = "HTML ContractNote For   client_ '" + dthtml.Rows[puc][3].ToString() + "'";
                                    string strEmailBody = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Dear Customer," +
                                        "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Please check HTML ContractNote with this email." +
                                        "</td></tr></table>";
                                    if (SendMail(strEmailBody, dthtml.Rows[puc][1].ToString(), strEmailSubject, "ExportFiles/HTMLCONTRACTNOTE/" + filename) == true)
                                    {
                                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "alert1", "<script>alertcall('true');</script>");
                                        successcount = successcount + 1;
                                        // CbpSuggestISIN.JSProperties["cpdownloadcomplete11"] = CbpSuggestISIN.JSProperties["cpdownloadcomplete12"]+"_alertsuc";
                                    }
                                    else
                                    {
                                        failedcount = failedcount + 1;
                                        //CbpSuggestISIN.JSProperties["cpdownloadcomplete12"] = CbpSuggestISIN.JSProperties["cpdownloadcomplete11"]+"_alertfail";

                                    }
                                    break;
                                }
                                else
                                {
                                    string strEmailSubject = "HTML ContractNote For [ '" + dsData.Tables[0].Rows[puc][4].ToString() + "' ] in '" + dsData.Tables[0].Rows[puc][5].ToString() + "' client_ '" + dsData.Tables[0].Rows[puc][3].ToString() + "' cntr no. '" + dsData.Tables[0].Rows[puc][2].ToString() + "'";
                                    string strEmailBody = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Dear Customer," +
                                        "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Please check HTML ContractNote with this email." +
                                        "</td></tr></table>";
                                    if (SendMail(strEmailBody, dsData.Tables[0].Rows[puc][1].ToString(), strEmailSubject, "ExportFiles/HTMLCONTRACTNOTE/" + filename) == true)
                                    {
                                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "alert1", "<script>alertcall('true');</script>");
                                        successcount = successcount + 1;
                                        // CbpSuggestISIN.JSProperties["cpdownloadcomplete11"] = CbpSuggestISIN.JSProperties["cpdownloadcomplete12"]+"_alertsuc";
                                    }
                                    else
                                    {
                                        failedcount = failedcount + 1;
                                        //CbpSuggestISIN.JSProperties["cpdownloadcomplete12"] = CbpSuggestISIN.JSProperties["cpdownloadcomplete11"]+"_alertfail";

                                    }
                                }
                            }
                            //for (int puc = 0; puc < dsData.Tables[0].Rows.Count; puc++)
                            //{

                            //    using (StreamWriter sw = new StreamWriter(path, false))
                            //    {

                            //        //sw.Write(dsData.Tables[0].Rows[puc][0].ToString());

                            //        foreach (DataRow dr1 in dthtml.Rows)
                            //        {
                            //            for (int j = 0; j < 1; j++)
                            //            {
                            //                sw.Write(dr1[j]);
                            //            }

                            //            sw.Write("</br>" + sw.NewLine);
                            //        }

                            //    }
                            //    string strEmailSubject = "HTML ContractNote For [ '" + dsData.Tables[0].Rows[puc][4].ToString() + "' ] in '" + dsData.Tables[0].Rows[0][5].ToString() + "' client_ '" + dsData.Tables[0].Rows[0][3].ToString() + "' cntr no. '" + dsData.Tables[0].Rows[0][2].ToString() + "'";
                            //    string strEmailBody = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Dear Customer," +
                            //        "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Please check HTML ContractNote with this email." +
                            //        "</td></tr></table>";
                            //    if (SendMail(strEmailBody, dsData.Tables[0].Rows[puc][1].ToString(), strEmailSubject, "ExportFiles/HTMLCONTRACTNOTE/" + filename) == true)
                            //    {
                            //        //this.Page.ClientScript.RegisterStartupScript(GetType(), "alert1", "<script>alertcall('true');</script>");
                            //        successcount = successcount + 1;
                            //       // CbpSuggestISIN.JSProperties["cpdownloadcomplete11"] = CbpSuggestISIN.JSProperties["cpdownloadcomplete12"]+"_alertsuc";
                            //    }
                            //    else
                            //    {
                            //        failedcount = failedcount + 1;
                            //        //CbpSuggestISIN.JSProperties["cpdownloadcomplete12"] = CbpSuggestISIN.JSProperties["cpdownloadcomplete11"]+"_alertfail";

                            //    }
                            //}
                            if (totalcount == successcount)
                                countervalue = "totalsuccess";
                            else if (totalcount == failedcount)
                                countervalue = "totalfail";
                            else
                                countervalue = "fewsuccessandfewfail";
                            CbpSuggestISIN.JSProperties["cpsuccessandfailmsg"] = countervalue;
                        }
                    }
                    else
                    {
                        CbpSuggestISIN.JSProperties["cpnorecord"] = "norecord";

                    }
                }
                else
                {
                    CbpSuggestISIN.JSProperties["cpnorecord"] = "norecord";

                }
            }
            if (WhichCall != "html")
            {
                if (WhichCall == "all")

                    sendtypeparameter = "All";


                else

                    sendtypeparameter = "Remaining";


                //j = view.viewdata(CompanyID, UserId, dp, strContractDate[0], contract, client, AuthorisedCustomerName, "", "", reporttype, scaningStatus, all, txtdigitalName_hidden.Text, strFundpayputdate, brkgflag, DdlPrintType1.SelectedValue.ToString(), HiddenField_Branch.Value.ToString(), HiddenField_Client.Value.ToString());
                //Note:Here UserID=UserSegID(24,29),DP=SegmentName(NSE - CM)
                j = view.viewdata(CompanyID, UserId, dp, strContractDate[0], AuthorisedCustomerName, "", "",
                    "Digital", 2, all, txtdigitalName_hidden.Text, strFundpayputdate, brkgflag,
                    DdlPrintType1.SelectedValue.ToString(), HiddenField_Branch.Value.ToString(),
                    HiddenField_Client.Value.ToString(), HiddenField_Group.Value.ToString(),
                    groupbytext.ToString().Trim(), Groupbyvalue.ToString().Trim(),
                    sendtypeparameter.ToString().Trim(), strchknet);




                if (ddlGroupBy.SelectedItem.Value == "Clients")
                {
                    if (RadioBtnOtherGroupByAll.Checked == true)
                    {

                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }


                    if (RadioBtnOtherGroupBySelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' ");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }

                    if (RadioBtnOtherGroupByallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ")  and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' ");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ")  and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ") and ContractNotes_EmailedBy is not null  and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ")  and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }



                }
                if (ddlGroupBy.SelectedItem.Value == "Branch")
                {
                    if (RadioBtnOtherGroupByAll.Checked == true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }

                    if (RadioBtnOtherGroupBySelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_Status is null and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }
                    if (RadioBtnOtherGroupByallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_Status is null and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }

                }
                if (ddlGroupBy.SelectedItem.Value == "Group")
                {
                    if (RadioBtnGroupAll.Checked == true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' ");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }
                    if (RadioBtnGroupSelected.Checked == true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_CustomerID  in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' ");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_CustomerID  in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_CustomerID  in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_CustomerID  in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }
                    if (RadioBtnGroupallbutSelected.Checked == true)
                    {
                        dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_CustomerID not in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' ");
                        dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_CustomerID not in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0) and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                        dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_Status is null and ContractNotes_CustomerID not in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and ContractNotes_EmailDateTime is not null");
                        dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID not in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                    }
                }

                allcontract = Convert.ToString(dtallcontract.Rows[0]["CntrNo"]);
                ecnenable = Convert.ToString(dtecnenable.Rows[0]["CntrNo"]);
                deliveryrpt = Convert.ToString(dtdeliveryrpt.Rows[0]["CntrNo"]);
                remaining = Convert.ToString(dtremaining.Rows[0]["CntrNo"]);
                CbpSuggestISIN.JSProperties["cpallcontractpops"] = deliveryrpt;
                CbpSuggestISIN.JSProperties["cpecnenablepops"] = remaining;
                //CbpSuggestISIN.JSProperties["cpecnenablepops"] = "1";

                //------- For Old Style Change For ECN ()

                if (j == 6 && DdlGeneRationType.SelectedItem.ToString() == "ECN")
                {

                    CbpSuggestISIN.JSProperties["cpallcontractpops"] = allcontract;
                    CbpSuggestISIN.JSProperties["cpecnenablepops"] = 0;

                }


            }


        }
        protected bool SendMail(string emailbdy, string contactid, string Subject, string strFilePath)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            string atchflile = "Y";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = oGenericMethod.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();

            }
            else
            {
                menuId = "";
            }

            try
            {
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                DataTable dt1 = oGenericMethod.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                string mailid = "";
                string ccmail = "";
                if (dt1 != null && dt1.Rows.Count > 0)
                {

                    mailid = Convert.ToString(dt1.Rows[0]["eml_email"]);
                    // ccmail = Convert.ToString(dt1.Rows[0]["eml_ccemail"]);
                }

                if (mailid != "")
                {
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = oGenericMethod.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    DataTable dtsg = oGenericMethod.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    DataTable dtname = oGenericMethod.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + contactid + "' ");
                    string ClientName = dtname.Rows[0]["ClientName"].ToString();

                    string senderemail = "";
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    string[,] data = oGenericMethod.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];

                    }

                    //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //  SqlConnection lcon = new SqlConnection(con);
                    string InternalID = string.Empty;
                    string result = string.Empty;
                    objFAReportsOther.InsertTransEmail(
                         Convert.ToString(senderemail),
                           Convert.ToString(Subject),
                           Convert.ToString("<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>"),
                           Convert.ToString(atchflile),
                           Convert.ToString(menuId),
                           Convert.ToString(HttpContext.Current.Session["userid"]),
                           "N",
                           Convert.ToString(HttpContext.Current.Session["LastCompany"]),
                           Convert.ToString(segmentname),
                        out  result
                         );
                    InternalID = result;
                    string fValues3 = "'" + InternalID + "','" + contactid + "','" + mailid + "','TO','" + oDBEngine.GetDate().ToString() + "','" + "P" + "'";
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    oGenericMethod.Insert_Table("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    string strAtchFValues = InternalID.ToString() + ",'" + strFilePath + "'";
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    oGenericMethod.Insert_Table("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", strAtchFValues);


                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    //{
                    //    lcon.Open();
                    //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                    //    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                    //    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                    //    parameter.Direction = ParameterDirection.Output;
                    //    lcmdEmplInsert.Parameters.Add(parameter);
                    //    lcmdEmplInsert.ExecuteNonQuery();
                    //    string InternalID = parameter.Value.ToString();
                    //      ###########---recipients-----------------                   

                    //    DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                    //    string mailid = dt1.Rows[0]["eml_email"].ToString();

                    //    string fValues3 = "'" + InternalID + "','" + contactid + "','" + mailid + "','TO','" + oDBEngine.GetDate().ToString() + "','" + "P" + "'";
                    //    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    //    oGenericMethod.Insert_Table("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    //    string strAtchFValues = InternalID.ToString() + ",'" + strFilePath + "'";
                    //    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    //    oGenericMethod.Insert_Table("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", strAtchFValues);
                    //}

                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {
                return false;
            }
            return true;


        }
        protected void cmbExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGenericMethod.GetDataTable("Trans_ExportFiles", "ExportFiles_ID", "ExportFiles_Segment='" + Session["usersegid"].ToString() + "' and ExportFiles_Type='htmlcontract'  and ExportFiles_Name='" + filename + "' and ExportFiles_Path='" + savefilepath + "' and ExportFiles_CreateUser='" + Session["userid"].ToString() + "' and ExportFiles_CreateDateTime='" + oDBEngine.GetDate().ToShortDateString() + "'");
            if (dt.Rows.Count > 0)
            {
                id = dt.Rows[0][0].ToString();

            }

            DataTable dt1 = oGenericMethod.GetDataTable("Trans_ExportFiles", "ExportFiles_Name,ExportFiles_Path", "ExportFiles_Segment='" + Session["usersegid"].ToString() + "' and ExportFiles_Type='htmlcontract' and ExportFiles_CreateUser='" + Session["userid"].ToString() + "' and ExportFiles_ID='" + id + "'");
            if (dt1.Rows.Count > 0)
            {
                idfilename = dt1.Rows[0][0].ToString();
                idfilepath = dt1.Rows[0][1].ToString();
            }
            //DataFetchandGridBind("NA", "L", "Screen");
            export_function();

        }
        protected void export_function()
        {

            string filename = Server.MapPath("..\\" + idfilepath);
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                string[] filename1;
                filename1 = fileInfo.Name.Split('\\');
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename1[0]);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                //Response.Redirect("'abc','_blank')");
                Response.WriteFile(fileInfo.FullName);
                Response.End();

            }

        }
        protected void Cexcelexportpanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable dtallcontract = new DataTable();
            DataTable dtecnenable = new DataTable();
            DataTable dtdeliveryrpt = new DataTable();
            DataTable dtremaining = new DataTable();

            if (ddlGroupBy.SelectedItem.Value == "Clients")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                }


                if (RadioBtnOtherGroupBySelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") ");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ")");
                }

                if (RadioBtnOtherGroupByallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ") ");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ") and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ") and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ")");
                }



            }
            if (ddlGroupBy.SelectedItem.Value == "Branch")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                }

                if (RadioBtnOtherGroupBySelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") ");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") ");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") ");
                }
                if (RadioBtnOtherGroupByallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") ");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") ");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") ");
                }

            }
            if (ddlGroupBy.SelectedItem.Value == "Group")
            {
                if (RadioBtnGroupAll.Checked == true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' ");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null ");
                }
                if (RadioBtnGroupSelected.Checked == true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_CustomerID in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null and ContractNotes_CustomerID in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null and ContractNotes_CustomerID in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                }
                if (RadioBtnGroupallbutSelected.Checked == true)
                {
                    dtallcontract = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID not in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                    dtecnenable = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID  in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId  and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_CustomerID not in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                    dtdeliveryrpt = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_EmailedBy is not null and ContractNotes_EmailDateTime is not null and ContractNotes_CustomerID not in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                    dtremaining = oDBEngine.GetDataTable("select  count(ContractNotes_Number) as CntrNo from Trans_ContractNotes,tbl_master_contact where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and contractnotes_tradedate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_Status is null and ContractNotes_CustomerID in (select eml_cntId  from tbl_master_email  where eml_type='Official' and len(eml_email)>0 and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(eml_Status,'Y')='Y') and ContractNotes_CustomerID=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailedBy is  null and ContractNotes_EmailDateTime is  null and ContractNotes_CustomerID not in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                }
            }
            allcontract = Convert.ToString(dtallcontract.Rows[0]["CntrNo"]);
            ecnenable = Convert.ToString(dtecnenable.Rows[0]["CntrNo"]);
            deliveryrpt = Convert.ToString(dtdeliveryrpt.Rows[0]["CntrNo"]);
            remaining = Convert.ToString(dtremaining.Rows[0]["CntrNo"]);


            string WhichCall = e.Parameter.Split('~')[0];

            Cexcelexportpanel.JSProperties["cpproperties"] = "";
            Cexcelexportpanel.JSProperties["cpallcontract"] = null;
            Cexcelexportpanel.JSProperties["cpecnenable"] = null;
            Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = null;
            Cexcelexportpanel.JSProperties["cpallcontractpop"] = null;
            Cexcelexportpanel.JSProperties["cpecnenablepop"] = null;
            Cexcelexportpanel.JSProperties["cpvisibletrue"] = null;
            if (WhichCall == "v2")
            {

                Cexcelexportpanel.JSProperties["cpproperties"] = "Export";
            }
            if (WhichCall == "v1")
            {

                Cexcelexportpanel.JSProperties["cpproperties"] = "Exportall";
            }
            if (WhichCall == "v3")
            {

                Cexcelexportpanel.JSProperties["cpproperties"] = "Exportdelivery";
            }
            if (WhichCall == "v4")
            {

                Cexcelexportpanel.JSProperties["cpallcontract"] = allcontract;
                Cexcelexportpanel.JSProperties["cpecnenable"] = ecnenable;
                Cexcelexportpanel.JSProperties["cpdeliveryrpt"] = deliveryrpt;
            }
            if (WhichCall == "v5")
            {


                Cexcelexportpanel.JSProperties["cpallcontractpop"] = deliveryrpt;
                Cexcelexportpanel.JSProperties["cpecnenablepop"] = remaining;
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

        void export(DataTable dtExport)
        {
            ExcelFile objExcel = new ExcelFile();

            string searchCriteria = null;
            BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
            searchCriteria = "For " + oconverter.ArrangeDate2(dtFromDate.Value.ToString()) + " Report of   " + ddlGroupBy.SelectedItem.Value + " Wise";

            dtExport = dtecnenale.Copy();
            BusinessLogicLayer.GenericExcelExport oGenericExcelExport = new BusinessLogicLayer.GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "Contractnote_" + exlTime;
            strDownloadFileName = "~/Documents/";
            DataTable dtcompany = oDBEngine.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "Contract Note Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots
            string[] ColumnType = { "I", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "10", "30", "50", "50", "10", "10", "30", "30", "10", "5" };
            string[] ColumnWidthSize = { "5", "10", "40", "30", "6", "7", "12", "25", "10", "5" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlGroupBy.SelectedItem.Value == "Clients")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0  and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' order by ContractNotes_Number ");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") order by ContractNotes_Number ");
                if (RadioBtnOtherGroupByallbutSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ") order by ContractNotes_Number ");

            }
            if (ddlGroupBy.SelectedItem.Value == "Branch")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' order by ContractNotes_Number ");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ") order by ContractNotes_Number ");
                if (RadioBtnOtherGroupByallbutSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") order by ContractNotes_Number ");

            }
            if (ddlGroupBy.SelectedItem.Value == "Group")
            {
                if (RadioBtnGroupAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' order by ContractNotes_Number ");
                if (RadioBtnGroupSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_CustomerID  in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) order by ContractNotes_Number ");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and ContractNotes_Status is null and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_CustomerID not in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + ")) order by ContractNotes_Number ");

            }

            export(dtecnenale);

        }
        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlGroupBy.SelectedItem.Value == "Clients")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and contractnotes_customerid in (" + HiddenField_Client.Value.ToString() + ") and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
                if (RadioBtnOtherGroupByallbutSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString() + ") and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
            }
            if (ddlGroupBy.SelectedItem.Value == "Branch")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and contractnotes_branchid in (" + HiddenField_Branch.Value.ToString() + ") and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
                if (RadioBtnOtherGroupByallbutSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and contractnotes_branchid not in (" + HiddenField_Branch.Value.ToString() + ") and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
            }
            if (ddlGroupBy.SelectedItem.Value == "Group")
            {
                if (RadioBtnGroupAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
                if (RadioBtnGroupSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_CustomerID  in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value + "))  and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select Srlno,t1.CntrNo,t1.ClientName,isnull(tbl_master_email.eml_email,'NA')as Email,isnull(tbl_master_email.eml_type,'NA')as EmailType ,t1.CntrDlvMode,t1.Settno,t1.Setttype from (select contractnotes_customerid,ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,ContractNotes_SettlementNumber as Settno,ContractNotes_SettlementType as Setttype from Trans_ContractNotes,tbl_master_contact where contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_CustomerID not in (select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value + "))  and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and  ContractNotes_CustomerID=cnt_internalId) as t1 left outer join tbl_master_email on t1.ContractNotes_CustomerID=eml_cntId order by t1.CntrNo");

            }




            export(dtecnenale);

        }
        protected void cmbExport2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlGroupBy.SelectedItem.Value == "Clients")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null order by ContractNotes_Number ");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null and contractnotes_customerid in (" + HiddenField_Client.Value.ToString().Trim() + ") order by ContractNotes_Number ");
                if (RadioBtnOtherGroupByallbutSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null and contractnotes_customerid not in (" + HiddenField_Client.Value.ToString().Trim() + ") order by ContractNotes_Number ");

            }
            if (ddlGroupBy.SelectedItem.Value == "Branch")
            {
                if (RadioBtnOtherGroupByAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null order by ContractNotes_Number ");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null and contractnotes_customerid in (" + HiddenField_Branch.Value.ToString().Trim() + ") order by ContractNotes_Number ");
                if (RadioBtnOtherGroupByallbutSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null and contractnotes_customerid not in (" + HiddenField_Branch.Value.ToString().Trim() + ") order by ContractNotes_Number ");

            }
            if (ddlGroupBy.SelectedItem.Value == "Group")
            {
                if (RadioBtnGroupAll.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null order by ContractNotes_Number ");
                if (RadioBtnGroupSelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null and ContractNotes_CustomerID  in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString() + ")) order by ContractNotes_Number ");
                if (RadioBtnOtherGroupBySelected.Checked == true)
                    dtecnenale = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by ContractNotes_Number) as Srlno, ContractNotes_Number as CntrNo,(LTRIM(rtrim(isnull(cnt_firstName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_middleName,' '))) +' '+  LTRIM(rtrim(isnull(cnt_lastName,' '))) + ' [ '+  LTRIM(rtrim(isnull(cnt_UCC,' ')))+ ' ] ') as ClientName,LTRIM(rtrim(isnull(eml_email,''))) as Email,eml_type as EmailType,case when isnull(cnt_ContractDeliveryMode,'P')='B' then 'Both' when isnull(cnt_ContractDeliveryMode,'P')='P' then 'Print' when isnull(cnt_ContractDeliveryMode,'P')='E' then 'Only ECN' end as CntrDlvMode,(select top 1 LTRIM(rtrim(user_name))+' [ '+  ltrim(rtrim(user_loginId))+ ' ] ' from tbl_master_user where ContractNotes_EmailedBy=user_id) as SentUser,convert(varchar,ContractNotes_EmailDateTime,106)+' '+substring(convert(varchar,ContractNotes_EmailDateTime,109),12,24) as sentdate,ContractNotes_SettlementNumber as SettNo,ContractNotes_SettlementType as SettType from Trans_ContractNotes,tbl_master_contact,tbl_master_email where  contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_TradeDate='" + dtFromDate.Value.ToString() + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and isnull(eml_Status,'Y')='Y' and len(eml_email)>0 and eml_type='Official' and len(eml_email)>0 and ContractNotes_CustomerID=cnt_internalId and eml_cntId=ContractNotes_CustomerID and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P' and ContractNotes_EmailDateTime is not null and ContractNotes_EmailedBy is not null and ContractNotes_CustomerID not in(select grp_contactid from tbl_trans_group where grp_grouptype='" + ddlGroup.SelectedItem.Text.ToString().Trim() + "' and grp_groupmaster in(" + HiddenField_Group.Value.ToString() + ")) order by ContractNotes_Number ");

            }


            export(dtecnenale);


        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void divdisplay()
        {
            //ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            int flag = 0;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=3><b>List of Contract Nos for this Settlement/Date</b></td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b> Start No</b></td>";
            strHtml += "<td align=\"center\" ><b> End No</b></td>";
            strHtml += "<td align=\"center\" ><b>Total No Of Contract</b></td>";
            if (ddlselection.SelectedItem.Value == "1")
            {
                strHtml += "<td align=\"center\" ><b>Sett. Number</b></td>";
                strHtml += "<td align=\"center\" ><b>Sett. Type</b></td></tr>";
            }


            string count = dtnew.Rows.Count.ToString();
            flag = flag + 1;
            strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\">" + dtnew.Rows[0]["contractnotes_number"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + dtnew.Rows[dtnew.Rows.Count - 1]["contractnotes_number"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + count + "</td>";
            if (ddlselection.SelectedItem.Value == "1")
            {
                strHtml += "<td align=\"left\">" + dtnew.Rows[0]["ContractNotes_SettlementNumber"].ToString() + " </td>";
                strHtml += "<td align=\"left\">" + dtnew.Rows[0]["ContractNotes_SettlementType"].ToString() + " </td>";
            }
            strHtml += "</tr>";
            //}
            strHtml += "</table>";
            display.InnerHtml = strHtml;

        }
        void Date()
        {
            dtFromDate.EditFormatString = oconverter.GetDateFormat("Date");
            dtToDate.EditFormatString = oconverter.GetDateFormat("Date");

            dtFromDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }
        void FnSelectionTypeBind()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1"
                || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4"
                || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15"
                || (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19"))
            {

                ddlselection.Items.Insert(0, new ListItem("Current Settelment No", "1"));
            }





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
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                HiddenField_SegmentName.Value = "MCXSX - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                HiddenField_SegmentName.Value = "MCXSX - FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "21")
                HiddenField_SegmentName.Value = "BFX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "22")
                HiddenField_SegmentName.Value = "INSX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "23")
                HiddenField_SegmentName.Value = "INFX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "24")
                HiddenField_SegmentName.Value = "UCX - COMM";
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
                if (idlist[0].ToString().Trim() == "Clients")
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

                else
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

            }
            data = idlist[0] + "~" + str;


        }

        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGroup.DataSource = DtGroup;
                ddlGroup.DataTextField = "gpm_Type";
                ddlGroup.DataValueField = "gpm_Type";
                ddlGroup.DataBind();
                DtGroup.Dispose();

            }

        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        public void FnDosPrint()
        {
            ddlLocation.Items.Clear();
            DataTable DtDosPrint = oDBEngine.GetDataTable("Config_DosPrinter", "distinct DosPrinter_Name+'['+DosPrinter_Location+']' as DosPrintName, DosPrinter_Location", "DosPrinter_User='" + HttpContext.Current.Session["userid"].ToString() + "'"); //replace(DosPrinter_Location,'/','\\') as
            if (DtDosPrint.Rows.Count > 0)
            {
                ddlLocation.DataSource = DtDosPrint;
                ddlLocation.DataTextField = "DosPrintName";
                ddlLocation.DataValueField = "DosPrinter_Location";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("Select DosPrint Location", "0"));
                DtDosPrint.Dispose();

            }
            else
            {
                ddlLocation.Items.Insert(0, new ListItem("Select DosPrint Location", "0"));
            }

        }


        DataSet SPCall()
        {
            string[] InputName = new string[13];
            string[] InputType = new string[13];
            string[] InputValue = new string[13];



            /////////////////Parameter Name
            InputName[0] = "Company";
            InputName[1] = "Segment";
            InputName[2] = "BranchHierchy";
            InputName[3] = "Clientid";
            InputName[4] = "GroupId";
            InputName[5] = "GrpType";
            InputName[6] = "SelectionType";
            InputName[7] = "SettNoType";
            InputName[8] = "FromDate";
            InputName[9] = "ToDate";
            InputName[10] = "ContractNo";
            InputName[11] = "FinYear";
            InputName[12] = "dosmode";

            /////////////////Parameter Data Type
            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "V";



            /////////////////Parameter Value
            InputValue[0] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
            InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
            InputValue[2] = HttpContext.Current.Session["userbranchHierarchy"].ToString().Trim();

            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Clients")
            {
                if (RadioBtnOtherGroupByAll.Checked)
                    InputValue[3] = "ALL~ALL";
                if (RadioBtnOtherGroupBySelected.Checked)
                    InputValue[3] = "Selected~" + HiddenField_Client.Value.ToString().Trim();
                if (RadioBtnOtherGroupByallbutSelected.Checked)
                    InputValue[3] = "AllButSelected~" + HiddenField_Client.Value.ToString().Trim();

                InputValue[4] = "ALL~ALL";
                InputValue[5] = "BRANCH";
            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Branch")
            {
                InputValue[3] = "ALL~ALL";
                if (RadioBtnOtherGroupByAll.Checked)
                    InputValue[4] = "ALL~ALL";
                if (RadioBtnOtherGroupBySelected.Checked)
                    InputValue[4] = "Selected~" + HiddenField_Branch.Value.ToString().Trim();
                if (RadioBtnOtherGroupByallbutSelected.Checked)
                    InputValue[4] = "AllButSelected~" + HiddenField_Branch.Value.ToString().Trim();
                InputValue[5] = "BRANCH";
            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                InputValue[3] = "ALL~ALL";
                if (RadioBtnGroupAll.Checked)
                    InputValue[4] = "ALL~ALL";
                if (RadioBtnGroupSelected.Checked)
                    InputValue[4] = "Selected~" + HiddenField_Group.Value.ToString().Trim();
                if (RadioBtnGroupallbutSelected.Checked)
                    InputValue[4] = "AllButSelected~" + HiddenField_Group.Value.ToString().Trim();
                InputValue[5] = ddlGroup.SelectedItem.Value.ToString().Trim();
            }
            InputValue[6] = ddlselection.SelectedItem.Value.ToString().Trim();
            InputValue[7] = HttpContext.Current.Session["LastSettNo"].ToString().Trim();
            InputValue[8] = dtFromDate.Value.ToString().Trim();
            InputValue[9] = dtToDate.Value.ToString().Trim();

            if (rdbCntrNoSelected.Checked)
                InputValue[10] = "Select~" + HiddenField_Contractnoteno.Value.ToString().Trim();
            if (rdbCntrNoRange.Checked)
                InputValue[10] = "Range~" + txtFromNo.Text.ToString().Trim() + " and " + txtToNo.Text.ToString().Trim();
            InputValue[11] = HttpContext.Current.Session["LastFinYear"].ToString().Trim();
            InputValue[12] = ddlfordos.SelectedItem.Value.ToString().Trim();
            if

               (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1"
               || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4"
               || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5"
                || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19" || (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20"))
            {

                ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Report_ContractNoteCustomerTrades]", InputName, InputType, InputValue);
            }
            else
            {
                ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[commoditycontract]", InputName, InputType, InputValue);
            }

            //ds = SQLProcedures.SelectProcedureArrDS("[Report_ContractNoteCustomerTrades]", InputName, InputType, InputValue);



            return ds;



        }

        protected string abcd()
        {
            ds = SPCall();
            string ab = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["a1"] = ds;

                string sData = null;
                string[] sArQuery = new string[1];
                sArQuery[0] = "";
                string PrintLoaction = null;
                if (ddlLocation.SelectedItem.Value.ToString().Trim() == "0")
                {
                    PrintLoaction = ConfigurationManager.AppSettings["SaveCSVsql"];
                }
                else
                {
                    PrintLoaction = ddlLocation.SelectedItem.Value.ToString().Trim();

                }
                string FileName = "OutputContract_" + Session["userid"].ToString() + "_" + Session["usersegid"].ToString() + "_" + oDBEngine.GetDate().ToString("ddMMyyyy") + "_" + oDBEngine.GetDate().ToString("hhmmss") + ".txt";

                string XmlFileName = Server.MapPath("ReportFormat") + "\\ContractNote_" + HttpContext.Current.Session["usersegid"].ToString() + ".xml";
                sOutputFileName = Server.MapPath("ReportOutput") + "\\OutputContract_" + Session["userid"].ToString() + "_" + Session["usersegid"].ToString() + "_" + oDBEngine.GetDate().ToString("ddMMyyyy") + "_" + oDBEngine.GetDate().ToString("hhmmss") + ".txt";
                hdnpath.Value = "\\OutputContract_" + Session["userid"].ToString() + "_" + Session["usersegid"].ToString() + "_" + oDBEngine.GetDate().ToString("ddMMyyyy") + "_" + oDBEngine.GetDate().ToString("hhmmss") + ".txt";
                ab = hdnpath.Value;
                hdnLocationPath.Value = path;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript40", "ajaxFunction();", true);
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript40", "ajaxFunction('\\"+ ab + "','\\"+ path + FileName + "');", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                if ((HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "1") && (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "4") && (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "15"))
                {
                    if (DdlGeneRationType.SelectedItem.Value == "3")
                    {
                        if (ddlselection.SelectedItem.Value == "3")
                        {
                            if (rdbCntrNoSelected.Checked == true)
                            {
                                dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number in (" + HiddenField_Contractnoteno.Value + ")", "contractnotes_number");
                            }
                            else
                            {
                                dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number  between '" + txtFromNo.Text.ToString().Trim() + "' and '" + txtToNo.Text.ToString().Trim() + "'", "contractnotes_number");
                            }
                        }
                        if (ddlselection.SelectedItem.Value == "2")
                        {
                            dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_tradedate between '" + dtFromDate.Value.ToString() + "'and '" + dtToDate.Value.ToString() + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                        }

                    }
                    else
                    {

                        dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_tradedate = '" + dtFromDate.Value.ToString() + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                    }

                    if (dtnew.Rows.Count > 0)
                    {
                        divdisplay();
                    }
                }
                else
                {
                    if (DdlGeneRationType.SelectedItem.Value != "3")
                    {
                        dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7) + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                    }
                    else
                    {
                        if (ddlselection.SelectedItem.Value == "3")
                        {
                            if (rdbCntrNoSelected.Checked == true)
                            {
                                dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number in (" + HiddenField_Contractnoteno.Value + ")", "contractnotes_number");
                            }
                            if (rdbCntrNoRange.Checked == true)
                            {
                                dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number  between '" + txtFromNo.Text.ToString().Trim() + "' and '" + txtToNo.Text.ToString().Trim() + "'", "contractnotes_number");
                            }
                        }
                        if (ddlselection.SelectedItem.Value == "2")
                        {
                            dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_tradedate between '" + dtFromDate.Value.ToString() + "'and '" + dtToDate.Value.ToString() + "'", "contractnotes_number");
                        }
                        if (ddlselection.SelectedItem.Value == "1")
                        {
                            dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7) + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                        }
                    }
                    if (dtnew.Rows.Count > 0)
                    {
                        divdisplay();
                    }
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript24", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('No Record Found');</script>");
            }
            return sOutputFileName;

        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            txtdigitalName_hidden.Text = "";
            if ((HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "1") && (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "4") && (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "15") && (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "19"))
            {
                if (DdlGeneRationType.SelectedItem.Value == "3")
                {
                    if (ddlselection.SelectedItem.Value == "3")
                    {
                        if (rdbCntrNoSelected.Checked == true)
                        {
                            dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number in (" + HiddenField_Contractnoteno.Value + ")", "contractnotes_number");
                        }
                        else
                        {
                            dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number  between '" + txtFromNo.Text.ToString().Trim() + "' and '" + txtToNo.Text.ToString().Trim() + "'", "contractnotes_number");
                        }
                    }
                    if (ddlselection.SelectedItem.Value == "2")
                    {
                        dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_tradedate between '" + dtFromDate.Value.ToString() + "'and '" + dtToDate.Value.ToString() + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                    }
                }
                else
                {
                    dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_tradedate = '" + dtFromDate.Value.ToString() + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                }

                if (dtnew.Rows.Count > 0)
                {
                    divdisplay();
                }
            }
            else
            {
                if (DdlGeneRationType.SelectedItem.Value != "3")
                {
                    dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7) + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                }
                else
                {
                    if (ddlselection.SelectedItem.Value == "3")
                    {
                        if (rdbCntrNoSelected.Checked == true)
                        {
                            dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number in (" + HiddenField_Contractnoteno.Value + ")", "contractnotes_number");
                        }
                        else
                        {
                            dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and contractnotes_number  between '" + txtFromNo.Text.ToString().Trim() + "' and '" + txtToNo.Text.ToString().Trim() + "'", "contractnotes_number");
                        }
                    }
                    if (ddlselection.SelectedItem.Value == "2")
                    {
                        dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "'  and ContractNotes_Status is null and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "' and ContractNotes_tradedate between '" + dtFromDate.Value.ToString() + "'and '" + dtToDate.Value.ToString() + "'", "contractnotes_number");
                    }
                    if (ddlselection.SelectedItem.Value == "1")
                    {
                        dtnew = oDBEngine.GetDataTable("trans_contractnotes", "contractnotes_number,ContractNotes_SettlementNumber,ContractNotes_SettlementType", "contractnotes_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7) + "' and contractnotes_settlementtype='" + HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1) + "' and contractnotes_segmentid='" + HttpContext.Current.Session["usersegid"].ToString() + "' and contractnotes_companyid='" + Session["LastCompany"].ToString() + "'", "contractnotes_number");
                    }
                }
                if (dtnew.Rows.Count > 0)
                {
                    divdisplay();
                }
            }
            if (dtnew.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1012", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1011", "<script language='javascript'>alert('No Record Found');</script>");

            }
            else
            {


                //////////////////ds = SPCall();
                /////////////////Session["a1"] = ds;
                DataTable dtbranch = new DataTable();
                DataTable dtcustomer = new DataTable();
                string cnbranch = null;
                string custbranch = null;
                string all = "";
                // btnshow.Enabled = false;

                string groupbytext = "";
                string sendtypeparameter = "";
                string Groupbyvalue = ddlGroupBy.SelectedItem.Value;

                if ((ddlGroupBy.SelectedItem.Value == "Clients") || (ddlGroupBy.SelectedItem.Value == "Branch"))
                {
                    if (RadioBtnOtherGroupByAll.Checked == true)// || RadioBtnGroupAll.Checked==true)
                    {
                        all = "A";
                    }
                    if (RadioBtnOtherGroupBySelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                    {
                        all = "S";
                    }

                    if (RadioBtnOtherGroupByallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                    {
                        all = "D";
                    }
                }
                if (ddlGroupBy.SelectedItem.Value == "Group")
                {
                    groupbytext = ddlGroup.SelectedItem.Text.ToString().Trim();
                    if (RadioBtnGroupAll.Checked == true)// || RadioBtnGroupAll.Checked==true)
                    {
                        all = "A";
                    }
                    if (RadioBtnGroupSelected.Checked == true)// || RadioBtnGroupSelected.Checked==true)
                    {
                        all = "S";
                    }

                    if (RadioBtnGroupallbutSelected.Checked == true)// || RadioBtnGroupallbutSelected.Checked==true)
                    {
                        all = "D";
                    }
                }






                if (DdlGeneRationType.SelectedItem.Value != "3")
                {


                    reporttype = "Print";
                    scaningStatus = 1;


                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript641", "<script language='javascript'>display();</script>");


                    string dp = HiddenField_SegmentName.Value.ToString();

                    string Type = DdlGeneRationType.SelectedItem.Value.ToString();
                    ViewRegulatoryReportNameSpace.ViewRegulatoryReport view = new ViewRegulatoryReportNameSpace.ViewRegulatoryReport();
                    string CompanyID = HttpContext.Current.Session["LastCompany"].ToString();
                    string UserId = HttpContext.Current.Session["usersegid"].ToString();
                    string CustomerID = HiddenField_Client.Value.ToString();
                    string client = HiddenField_Client.Value.ToString();
                    StrID = HiddenField_Client.Value.ToString();
                    if (Hddndate.Value == "")
                    {
                        Hddndate.Value = Convert.ToDateTime(dtFromDate.Value).ToString("yyyy-MM-dd");
                    }
                    string[] strContractDate = Hddndate.Value.Split('#');

                    string ContractNo = HiddenField_Contractnoteno.Value.ToString();
                    string[,] AuthorisedCustomer = null;
                    string AuthorisedCustomerName = "";
                    string txtEmp = txtEmpName_hidden.Text.ToString();
                    string brkgflag = "";
                    string deselectclient = "";
                    string strchknet = "";
                    if (chknet.Checked == true)
                        strchknet = "true";
                    else
                        strchknet = "false";
                    if (chkBrokerage.Checked == true)
                    {
                        brkgflag = "True";
                    }
                    else
                    {
                        brkgflag = "False";
                    }

                    AuthorisedCustomer = oDBEngine.GetFieldValue(" master_AuthorizedSignatory MAS,tbl_master_contact MC", "MC.cnt_firstName+' '+isnull(MC.cnt_MiddleName,'')+' '+MC.cnt_lastName", "MAS.AuthorizedSignatory_CompanyID='" + CompanyID + "' AND MAS.AuthorizedSignatory_SegmentID=" + UserId + " AND MAS.AuthorizedSignatory_EmployeeID=MC.cnt_internalID", 1, "AuthorizedSignatory_ID");
                    for (int i = 0; i <= AuthorisedCustomer.Length - 1; i++)
                    {
                        AuthorisedCustomerName = "" + AuthorisedCustomerName + "</br>" + (string)AuthorisedCustomer[i, 0] + "";


                    }
                    AuthorisedCustomerName = AuthorisedCustomerName.Substring(5, AuthorisedCustomerName.Length - 5);

                    string strFundpayputdate = date();
                    string contract = "";
                    string test = DdlPrintType.SelectedItem.Text;
                    j = view.viewdata(CompanyID, UserId, dp, strContractDate[0], AuthorisedCustomerName,
                        txtEmp, txtEmpName.Text, "Print", scaningStatus, all.ToString().Trim(),
                        txtdigitalName_hidden.Text, strFundpayputdate, brkgflag,
                        DdlPrintType.SelectedValue.ToString(), HiddenField_Branch.Value.ToString(),
                        HiddenField_Client.Value.ToString(), HiddenField_Group.Value.ToString(), groupbytext.ToString().Trim(),
                        Groupbyvalue.ToString().Trim(), "All", strchknet);

                    if (j == 5)
                    {
                        //ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>Page_Load();</script>");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "hideProg1();", true);
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript12gf", "<script language='javascript'>Page_Load();</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript14bf", "<script language='javascript'>alert('No Record Found');</script>");
                    }
                    else if (j == 6)
                    {

                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript14", "<script language='javascript'>alert('Mail Sent Successfully to Your Selected Client');</script>");

                    }
                    else if (j == 16)
                    {

                        ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>Customeremail();</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "hideProg1();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "show('Div1');", true);

                    }
                }



                else
                {
                    string str = "";
                    str = abcd();

                    string UploadPath = str;
                    string[] filename = UploadPath.Split('\\');
                    string[] Location = oDBEngine.GetFieldValue1("Config_DosPrinter", "DosPrinter_Location", "DosPrinter_User='" + HttpContext.Current.Session["userid"].ToString() + "'", 1);
                    string path = ddlLocation.Text + filename[filename.Length - 1];
                    hdnLocationPath.Value = path;



                }
            }
        }
        bool IsSignExists()
        {
            string str;
            str = string.Empty;
            str = objFAReportsOther.searchSignatureUser(
                Convert.ToString(Session["userid"]));
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


        private string date()
        {
            string[,] DtStartEnddate = null;
            string dtdate = null;
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
            {


                DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "cast(Settlements_FundsPayOut as varchar)", "settlements_Number='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and settlements_TypeSuffix='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", 1);
                if (DtStartEnddate[0, 0] != "n")
                {
                    string[] idlist = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                    dtdate = Convert.ToDateTime(idlist[0]).ToString("dd MMM yyyy");


                }

            }
            else
            {
                DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "cast(Settlements_FundsPayOut as varchar)", "settlements_Number='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and settlements_TypeSuffix='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", 1);
                if (DtStartEnddate[0, 0] != "n")
                {
                    string[] idlist = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                    dtdate = Convert.ToDateTime(idlist[0]).ToString("dd MMM yyyy");


                }
            }

            return dtdate;
        }

        private string tdate()
        {
            string[,] DtStartEnddate = null;
            string dt1date = null;
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
            {


                DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "cast(Settlements_startdatetime as varchar)", "settlements_Number='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and settlements_TypeSuffix='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", 1);
                if (DtStartEnddate[0, 0] != "n")
                {
                    string[] idlist = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                    dt1date = Convert.ToDateTime(idlist[0]).ToString("dd MMM yyyy");


                }

            }


            return dt1date;
        }
        protected string fn_trdate()
        {
            ds = SPCall();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (trdate == "")
                        trdate = "'" + ds.Tables[0].Rows[i]["contractnotes_tradedate"].ToString() + "'";
                    else
                        trdate += "," + "'" + ds.Tables[0].Rows[i]["contractnotes_tradedate"].ToString() + "'";
                }

            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct48", "alert('No record Found!');", true);
                Response.End();
            }

            return trdate;
        }

    }
}