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
    public partial class Reports_frmReport_TransactionSingleClientCDSL : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        string dp = "CDSL";
        private static DataTable DT = new DataTable();
        String dpId, SegmentId, billnoFinYear, financilaYear;
        static string isinid = "", cmpid = "", SettlementID = "";
        static int counter = 0, pageindex = 0, totolRecord = 0;
        PagedDataSource pds = new PagedDataSource();
        private int Repcounter = 0;
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        string data;
        static string Clients;

        //---------For Sending Email
        static string EmailHTML = "";
        //Converter oconverter = new Converter();
        ExcelFile objExcel = new ExcelFile();
        static DataTable dtTemp = new DataTable();
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

            Page.RegisterStartupScript("myScript", "<script language=JavaScript>ShowFilter();</script>");
            ////
            if (!IsPostBack)
            {
                counter = 0;
                bindFrmToDate();
                txtName.Attributes.Add("onkeyup", "CallAjax(this,'CDSLTransction',event)");
                //txtName.Attributes.Add("onkeyup", "CallAjax(this,'cdslBillClientSelection',event)");
                txtISIN.Attributes.Add("onkeyup", "CallAjax(this,'cdslTransctionisin',event)");
                txtSettlement.Attributes.Add("onkeyup", "CallAjax(this,'cdslTransctionSettlement',event)");

                list.Style["display"] = "none";
                norecord.Style["display"] = "none";
                tblpage.Style["display"] = "none";

                //txtstartDate.EditFormatString = objConverter.GetDateFormat("Date");
                //txtendDate.EditFormatString = objConverter.GetDateFormat("Date");

                //*
                cmpid = Request.QueryString["boid"].ToString();

                norecord.Style["display"] = "none";
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
                //// ScriptManager.RegisterStartupScript(this, this.GetType(), "hid", "<script language=JavaScript>hidesearch();</script>", true);
                Page.RegisterStartupScript("hidsearch", "<script language=JavaScript>hidesearch();</script>");
                //*

            }
            //txtstartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().Month + "-" + "01" + "-" + oDBEngine.GetDate().Year).ToString();

            //DateTime dt = oDBEngine.GetDate();
            //DateTime lstDay = new DateTime(dt.Year, dt.Month, 1); 
            //lstDay = lstDay.AddDays(-1);

            //DateTime fstDay = new DateTime(dt.Year, dt.Month, 1); 
            // Console.WriteLine("Month: {0}, LastDate: {1}", lstDay.Month, lstDay.Day); Console.Read();

            // DateTime aa = oDBEngine.GetDate().AddMonths(-1) ;
            //DateTime cmonth = Convert.ToDateTime(Convert.ToDateTime(oDBEngine.GetDate().Month).AddMonths(-1));
            // txtstartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().Month.ToString() + "-" + "01" + "-" + oDBEngine.GetDate().Year.ToString());
            //txtendDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
            //  txtstartDate.Value = Convert.ToDateTime(aa);

            //txtstartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().Month.ToString() + "-" + "01" + "-" + oDBEngine.GetDate().Year.ToString());
            //txtendDate.Value = Convert.ToDateTime(lstDay);




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

            if (counter > 1 && RBReportType.SelectedIndex == 1)
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
            int mnth = oDBEngine.GetDate().Month - 1;
            int Cyer = oDBEngine.GetDate().Year;
            if (mnth == 0)
            {
                mnth = 12;
                Cyer = oDBEngine.GetDate().Year - 1;
            }
            //DateTime firstDay = new DateTime(Cyer, mnth, 1);
            //DateTime lastDayOfMonth = firstDay.AddMonths(1).AddTicks(-1);
            //string month = String.Format("{0:MM}", lastDayOfMonth);
            //string date = String.Format("{0:MM-dd-yyyy}", lastDayOfMonth);
            //string[] dateSplit = date.Split('-');

            DateTime firstDay = DateTime.Today.AddDays(-30);
            DateTime lastDayOfMonth = DateTime.Today;
            string month = String.Format("{0:MM}", lastDayOfMonth);
            string date = String.Format("{0:MM-dd-yyyy}", lastDayOfMonth);
            string[] dateSplit = date.Split('-');

            txtendDate.Text = dateSplit[1] + "-" + month + "-" + dateSplit[2];
            month = String.Format("{0:MM}", firstDay);
            date = String.Format("{0:MM-dd-yyyy}", firstDay);
            dateSplit = date.Split('-');
            txtstartDate.Text = "01" + "-" + month + "-" + dateSplit[2];
            //txtstartDate.Text = dateSplit[1] + "-" + month + "-" + dateSplit[2];
            //  txtstartDate.Text = dateSplit[1] + "-" + month + "-2009";

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {



            norecord.Style["display"] = "none";
            if (RBReportType.SelectedIndex == 1)
            {

                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                showCrystalReport();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
            }
            else
            {

                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
            }


        }

        public void showCrystalReport()
        {

            string select, where;
            //string stdate, endDate;
            int logoStatus;

            DataSet transDs = new DataSet();
            DataSet holdingDs = new DataSet();
            DataSet logo = new DataSet();

            DataRow drow;
            byte[] logoinByte;

            DataTable dtDate = oDBEngine.GetDataTable("master_finyear", "Finyear_id", "Finyear_Code='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (convert(datetime,'" + txtstartDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105)) and (convert(datetime,'" + txtendDate.Text + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105))");
            if (dtDate.Rows.Count > 0)
            {


                //stdate = txtstartDate.Value + " 00:00:00";
                //endDate = txtendDate.Value + " 23:59:00";
                string[] stdate = txtstartDate.Value.ToString().Split(' ');
                string startdate = stdate[0].ToString() + " 00:00:00";
                string[] endDate = txtendDate.Value.ToString().Split(' ');
                string enddt = endDate[0].ToString() + " 23:59:00";
                logoStatus = 1;

                logo.Tables.Add();
                logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                drow = logo.Tables[0].NewRow();


                if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) == 1)
                {
                    drow["Image"] = logoinByte;

                }
                else
                {
                    logoStatus = 0;
                    ScriptManager.RegisterStartupScript(this, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

                }

                logo.Tables[0].Rows.Add(drow);


                if (logoStatus == 1)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    {
                        ////SqlCommand cmd = new SqlCommand("cdslTransctionShowwithDematandPledge", con);
                        //SqlCommand cmd = new SqlCommand("cdslTransctionShowwithDematandPledgeSingleClient", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@stdate", startdate);
                        //cmd.Parameters.AddWithValue("@eddate", enddt);
                        //cmd.Parameters.AddWithValue("@compID", companyId());
                        //cmd.Parameters.AddWithValue("@dp", dp);


                        //if (cmpid != "")
                        //{

                        //    cmd.Parameters.AddWithValue("@BoID", cmpid);

                        //}
                        //else
                        //{
                        //    cmd.Parameters.AddWithValue("@BoID", "na");
                        //}

                        //if (isinid != "")
                        //{

                        //    cmd.Parameters.AddWithValue("@isin", isinid);

                        //}
                        //else
                        //{
                        //    cmd.Parameters.AddWithValue("@isin", "na");
                        //}

                        //if (SettlementID != "")
                        //{

                        //    cmd.Parameters.AddWithValue("@SettlementID", SettlementID);

                        //}
                        //else
                        //{
                        //    cmd.Parameters.AddWithValue("@SettlementID", "na");
                        //}

                        ////if (ASPxComboBox1.Value == "All")
                        ////{
                        ////    cmd.Parameters.AddWithValue("@boStatus", "na");
                        ////}
                        ////else
                        ////{
                        ////    cmd.Parameters.AddWithValue("@boStatus", ASPxComboBox1.Value);
                        ////}
                        //cmd.Parameters.AddWithValue("@branchid", HttpContext.Current.Session["userbranchHierarchy"].ToString());



                        //cmd.CommandTimeout = 0;
                        //SqlDataAdapter da = new SqlDataAdapter();
                        //da.SelectCommand = cmd;
                        //da.Fill(transDs);

                        transDs = rep.cdslTransctionShowwithDematandPledgeSingleClient(startdate, enddt, companyId(), dp, cmpid != "" ? cmpid : "na",
                            isinid != "" ? isinid : "na", SettlementID != "" ? SettlementID : "na", HttpContext.Current.Session["userbranchHierarchy"].ToString());
                        /*
                        SqlCommand cmdHolding = new SqlCommand("cdslTransctionHolding", con);
                        cmdHolding.CommandType = CommandType.StoredProcedure;
                        //cmdHolding.Parameters.AddWithValue("@stdate", txtendDate.Text + " 00:00:00");
                        //cmdHolding.Parameters.AddWithValue("@eddate", txtendDate.Text + " 23:59:00");
                        cmdHolding.Parameters.AddWithValue("@stdate",startdate);
                        cmdHolding.Parameters.AddWithValue("@eddate", enddt);

                        if (cmpid != "")
                        {

                            cmdHolding.Parameters.AddWithValue("@boid", cmpid);

                        }
                        else
                        {
                            cmdHolding.Parameters.AddWithValue("@boid", "na");
                        }

                        cmdHolding.CommandTimeout = 0;
                        SqlDataAdapter daHolding = new SqlDataAdapter();
                        daHolding.SelectCommand = cmdHolding;


                        daHolding.Fill(holdingDs);



                    }*/
                        String CompanyId, dpId, SegmentId;
                        DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
                                                       " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
                                                           " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")");

                        CompanyId = lastSegMemt.Rows[0][0].ToString();
                        dpId = lastSegMemt.Rows[0][2].ToString();
                        SegmentId = lastSegMemt.Rows[0][1].ToString();

                        String financilaYear, billnoFinYear, month;

                        financilaYear = HttpContext.Current.Session["LastFinYear"].ToString(); //HttpContext.Current.Session["LastFinYear"].ToString();
                        string[] yearSplit;

                        yearSplit = financilaYear.Split('-');

                        billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";
                        if (txtendDate.Text.Split('-')[1] == "01")
                        {
                            month = "JAN";
                        }
                        else if (txtendDate.Text.Split('-')[1] == "02")
                        {
                            month = "FEB";
                        }
                        else if (txtendDate.Text.Split('-')[1] == "03")
                        {
                            month = "MAR";
                        }
                        else if (txtendDate.Text.Split('-')[1] == "04")
                        {
                            month = "APR";
                        }
                        else if (txtendDate.Text.Split('-')[1] == "05")
                        {
                            month = "MAY";
                        }
                        else if (txtendDate.Text.Split('-')[1] == "06")
                        {
                            month = "JUN";

                        }
                        else if (txtendDate.Text.Split('-')[1] == "07")
                        {
                            month = "JUL";

                        }
                        else if (txtendDate.Text.Split('-')[1] == "08")
                        {
                            month = "AUG";

                        }
                        else if (txtendDate.Text.Split('-')[1] == "09")
                        {
                            month = "SEP";

                        }
                        else if (txtendDate.Text.Split('-')[1] == "10")
                        {
                            month = "OCT";

                        }
                        else if (txtendDate.Text.Split('-')[1] == "11")
                        {
                            month = "NOV";

                        }
                        else
                        {
                            month = "DEC";

                        }

                        //SqlCommand cmdHolding = new SqlCommand("cdslBill_ReportHolding_transaction", con);
                        //cmdHolding.CommandType = CommandType.StoredProcedure;

                        //cmdHolding.Parameters.AddWithValue("@billNumber", "CDSL" + "-" + month + billnoFinYear);

                        //if (cmpid == "")
                        //{
                        //    cmdHolding.Parameters.AddWithValue("@BenAccount", "NA");
                        //}
                        //else
                        //{
                        //    cmdHolding.Parameters.AddWithValue("@BenAccount", cmpid.Substring(8));
                        //}

                        //cmdHolding.Parameters.AddWithValue("@group", "NA");



                        //cmdHolding.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                        //cmdHolding.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
                        //cmdHolding.Parameters.AddWithValue("@dp", "CDSL");
                        //cmdHolding.Parameters.AddWithValue("@billamt", "0.00");
                        //cmdHolding.Parameters.AddWithValue("@generationOrder", "PinCode");
                        //cmdHolding.Parameters.AddWithValue("@dpId", dpId);

                        //cmdHolding.CommandTimeout = 0;
                        //SqlDataAdapter daHolding = new SqlDataAdapter();
                        //daHolding.SelectCommand = cmdHolding;
                        //daHolding.Fill(holdingDs);

                        holdingDs = rep.cdslBill_ReportHolding_transaction("CDSL" + "-" + month + billnoFinYear, cmpid == "" ? "NA" : cmpid.Substring(8),
                            "NA", SegmentId, CompanyId, "CDSL", "0.00", "PinCode", dpId);

                        if (transDs.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
                            {

                                if (k > 0)
                                {
                                    if (transDs.Tables[0].Rows[k - 1]["CDSLISIN_Number"].ToString() == transDs.Tables[0].Rows[k]["CDSLISIN_Number"].ToString() && transDs.Tables[0].Rows[k - 1]["CdslTransaction_SettlementID"].ToString() == transDs.Tables[0].Rows[k]["CdslTransaction_SettlementID"].ToString()
                                                                 && transDs.Tables[0].Rows[k - 1]["transactionType"].ToString() == transDs.Tables[0].Rows[k]["transactionType"].ToString())
                                    {
                                        transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k - 1]["CdslTransaction_Quantity"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                                    }
                                    else
                                    {
                                        transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                                    }
                                }
                                else
                                {
                                    transDs.Tables[0].Rows[0]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[0]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[0]["debit"].ToString()));
                                }

                            }

                            for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
                            {
                                if (transDs.Tables[0].Rows[k]["credit"].ToString() != "0.000")
                                    transDs.Tables[0].Rows[k]["credit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()));
                                else
                                    transDs.Tables[0].Rows[k]["credit"] = DBNull.Value;

                                if (transDs.Tables[0].Rows[k]["debit"].ToString() != "0.000")
                                    transDs.Tables[0].Rows[k]["debit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                                else
                                    transDs.Tables[0].Rows[k]["debit"] = DBNull.Value;

                                transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"].ToString()));
                                transDs.Tables[0].Rows[k]["openingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()));
                                transDs.Tables[0].Rows[k]["closingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["closingbalance"].ToString()));
                            }
                        }


                        //if (transDs.Tables[0].Rows.Count > 0)
                        //{

                        /*
                        // transDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"]+"\\Reports\\cdsltransction.xsd");
                        //  transDs.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"]+"\\Reports\\cdsltransction.xml");
                        DataTable tmp = new DataTable();

                        tmp = holdingDs.Tables[0].Copy();
                        holdingDs.Tables[0].Reset();

                        holdingDs.Tables[0].Columns.Add("holdingDate", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("Holdingboid", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CdslHolding_ISIN", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CDSLISIN_ShortName", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CdslHolding_SettlementID", Type.GetType("System.String"));


                        holdingDs.Tables[0].Columns.Add("CdslHolding_CurrentBalance", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CdslHolding_FreeBalance", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CdslHolding_PledgeBalance", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CdslHolding_EarmarkedBalance", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CdslHolding_PendingRematBalance", Type.GetType("System.String"));
                        holdingDs.Tables[0].Columns.Add("CdslHolding_PendingDematBalance", Type.GetType("System.String"));




                        if (tmp.Rows.Count == 0)
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
                  
                        else
                        {
                            for (int j = 0; j < tmp.Rows.Count; j++)
                            {
                                DataRow dataRow = holdingDs.Tables[0].NewRow();

                                dataRow["holdingDate"] = tmp.Rows[j]["holdingDate"];
                                dataRow["Holdingboid"] = tmp.Rows[j]["Holdingboid"];
                                dataRow["CdslHolding_ISIN"] = tmp.Rows[j]["CdslHolding_ISIN"];
                                dataRow["CDSLISIN_ShortName"] = tmp.Rows[j]["CDSLISIN_ShortName"];
                                dataRow["CdslHolding_SettlementID"] = tmp.Rows[j]["CdslHolding_SettlementID"];

                                if (tmp.Rows[j]["CdslHolding_CurrentBalance"].ToString() != "0.000")
                                    dataRow["CdslHolding_CurrentBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_CurrentBalance"].ToString()));
                                else
                                    dataRow["CdslHolding_CurrentBalance"] = DBNull.Value;

                                if (tmp.Rows[j]["CdslHolding_FreeBalance"].ToString() != "0.000")
                                    dataRow["CdslHolding_FreeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_FreeBalance"].ToString()));
                                else
                                    dataRow["CdslHolding_FreeBalance"] = DBNull.Value;

                                if (tmp.Rows[j]["CdslHolding_PledgeBalance"].ToString() != "0.000")
                                    dataRow["CdslHolding_PledgeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_PledgeBalance"].ToString()));
                                else
                                    dataRow["CdslHolding_PledgeBalance"] = DBNull.Value;

                                if (tmp.Rows[j]["CdslHolding_EarmarkedBalance"].ToString() != "0.000")
                                    dataRow["CdslHolding_EarmarkedBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_EarmarkedBalance"].ToString()));
                                else
                                    dataRow["CdslHolding_EarmarkedBalance"] = DBNull.Value;

                                if (tmp.Rows[j]["CdslHolding_PendingRematBalance"].ToString() != "0.000")
                                    dataRow["CdslHolding_PendingRematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_PendingRematBalance"].ToString()));
                                else
                                    dataRow["CdslHolding_PendingRematBalance"] = DBNull.Value;

                                if (tmp.Rows[j]["CdslHolding_PendingDematBalance"].ToString() != "0.000")
                                    dataRow["CdslHolding_PendingDematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_PendingDematBalance"].ToString()));
                                else
                                    dataRow["CdslHolding_PendingDematBalance"] = DBNull.Value;

                                holdingDs.Tables[0].Rows.Add(dataRow);

                            }
                        }
                        holdingDs.AcceptChanges();*/
                        //holdingDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslholding.xsd");
                        // holdingDs.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"]+"\\Reports\\cdslholding.xml");




                        ReportDocument cdslTransctionReportDocu = new ReportDocument();

                        string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');






                        string path = Server.MapPath("..\\Reports\\cdsltrans_holding.rpt");
                        cdslTransctionReportDocu.Load(path);

                        cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                        if (transDs.Tables[0].Rows.Count > 0)
                        {
                            cdslTransctionReportDocu.SetDataSource(transDs.Tables[0]);
                        }
                        else
                        {
                            cdslTransctionReportDocu.SetDataSource(transDs.Tables[1]);
                        }

                        cdslTransctionReportDocu.Subreports["logo"].SetDataSource(logo);
                        cdslTransctionReportDocu.Subreports["holding"].SetDataSource(holdingDs);









                        cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "CDSL Transction");







                        transDs.Clear();
                        transDs.Dispose();
                        holdingDs.Clear();
                        holdingDs.Dispose();


                        //}
                        //else
                        //{
                        //    norecord.Style["display"] = "display";
                        //}

                    }

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
            string stdate1, endDate2, stdate, endDate;
            //stdate = txtstartDate.Text + " 00:00:00";
            //endDate = txtendDate.Text + " 23:59:00";
            stdate1 = txtstartDate.Text;
            endDate2 = txtendDate.Text;

            string[] dtSplit1 = stdate1.Split('-');
            stdate = dtSplit1[1] + "-" + dtSplit1[0] + "-" + dtSplit1[2] + " 00:00:00";
            string[] dtSplit2 = endDate2.Split('-');
            endDate = dtSplit2[1] + "-" + dtSplit2[0] + "-" + dtSplit2[2] + " 23:59:00";
            DT.Clear();
            DT.Dispose();

            totolRecord = 0;
            pageindex = 0;
            DataTable dtDate = oDBEngine.GetDataTable("master_finyear", "Finyear_id", "Finyear_Code='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (convert(datetime,'" + stdate1 + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105)) and (convert(datetime,'" + endDate2 + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105))");
            if (dtDate.Rows.Count > 0)
            {
                // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                ////SqlCommand cmd = new SqlCommand("cdslTransctionShowList", con);
                //SqlCommand cmd = new SqlCommand("cdslTransctionShowListSingleClient", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@stdate", stdate);
                //cmd.Parameters.AddWithValue("@eddate", endDate);
                //cmd.Parameters.AddWithValue("@companyId", companyId());


                //if (cmpid != "")
                //{

                //    cmd.Parameters.AddWithValue("@BoID", cmpid);

                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@BoID", "na");
                //}

                //if (isinid != "")
                //{

                //    cmd.Parameters.AddWithValue("@isin", isinid);

                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@isin", "na");
                //}

                //if (SettlementID != "")
                //{

                //    cmd.Parameters.AddWithValue("@SettlementID", SettlementID);

                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@SettlementID", "na");
                //}

                ////if (ASPxComboBox1.Value == "All")
                ////{
                ////    cmd.Parameters.AddWithValue("@boStatus", "na");
                ////}
                ////else
                ////{
                ////    cmd.Parameters.AddWithValue("@boStatus", ASPxComboBox1.Value);
                ////}

                //cmd.Parameters.AddWithValue("@userid", Session["userid"]);
                //cmd.Parameters.AddWithValue("@branchid", HttpContext.Current.Session["userbranchHierarchy"].ToString());

                //cmd.CommandTimeout = 0;
                //SqlDataAdapter da = new SqlDataAdapter();
                //da.SelectCommand = cmd;
                //da.Fill(DT);

                DT = rep.cdslTransctionShowListSingleClient(stdate, endDate, companyId(), cmpid != "" ? cmpid : "na", isinid != "" ? isinid : "na",
                    SettlementID != "" ? SettlementID : "na", Session["userid"].ToString(), HttpContext.Current.Session["userbranchHierarchy"].ToString());
                // }

                if (DT.Rows.Count > 0)
                {
                    totolRecord = DT.Rows.Count;
                    bindTopHeader(0);
                    pageing();
                    norecord.Style["display"] = "none";
                    listRecord.Text = pageindex + 1 + " of " + totolRecord + " Reocrds.";

                }
                else
                {
                    norecord.Style["display"] = "display";
                    list.Style["display"] = "none";
                    tblpage.Style["display"] = "none";
                    norecord.Style["display"] = "display";
                }


            }
            else
            {
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSdtshow", "ShowFilter();", true);
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
            boid.Text = DT.Rows[i]["CdslClients_BOID"].ToString();
            category.Text = DT.Rows[i]["CdslClients_AccountCategory"].ToString();
            status.Text = DT.Rows[i]["CdslClients_AccountStatus"].ToString();
            holders.Text = DT.Rows[i]["name"].ToString();
            //lblTotalTransction.Text = DT.Rows[i]["totaltrans"].ToString();
            //lblTotalTransction1.Text = lblTotalTransction.Text;

            oDBEngine.DeleteValue("Tmp_CDSL_Transction", " Create_User=" + Session["userid"].ToString());

            ViewState["startRowIndex"] = 0;
            ViewState["pageSize"] = 30;
            ViewState["totalRecord"] = 0;
            ViewState["List"] = null;
            ViewState["prevIsin"] = "";
            ViewState["prevSett"] = "";



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
            listRecord.Text = pageindex + 1 + " of " + totolRecord + " Clients Transction.";
        }

        void bindList()
        {
            string stdate, endDate, stdate1, endDate2;
            ////stdate = txtstartDate.Text + " 00:00:00";
            ////endDate = txtendDate.Text + " 23:59:00";
            stdate1 = txtstartDate.Text;
            endDate2 = txtendDate.Text;

            string[] dtSplit1 = stdate1.Split('-');
            stdate = dtSplit1[1] + "-" + dtSplit1[0] + "-" + dtSplit1[2] + " 00:00:00";
            string[] dtSplit2 = endDate2.Split('-');
            endDate = dtSplit2[1] + "-" + dtSplit2[0] + "-" + dtSplit2[2] + " 23:59:00";
            DataTable List = new DataTable();
            List.Clear();
            List.Dispose();

            DataTable dtDate = oDBEngine.GetDataTable("master_finyear", "Finyear_id", "Finyear_Code='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (convert(datetime,'" + stdate1 + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105)) and (convert(datetime,'" + endDate2 + "',105) between convert(datetime,Finyear_startDate,105) and convert(datetime,FinYear_EndDate,105))");
            if (dtDate.Rows.Count > 0)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    //SqlCommand cmd = new SqlCommand("cdslTransctionDisplayupdated", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@stdate", stdate);
                    //cmd.Parameters.AddWithValue("@eddate", endDate);
                    //cmd.Parameters.AddWithValue("@boid", boid.Text);

                    //if (isinid != "")
                    //{

                    //    cmd.Parameters.AddWithValue("@isin", isinid);

                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@isin", "na");
                    //}
                    //if (SettlementID != "")
                    //{

                    //    cmd.Parameters.AddWithValue("@SettlementID", SettlementID);

                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@SettlementID", "na");
                    //}
                    //cmd.Parameters.AddWithValue("userbranchHierarchy", HttpContext.Current.Session["userbranchHierarchy"].ToString());


                    //cmd.CommandTimeout = 0;
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.SelectCommand = cmd;
                    //da.Fill(List);
                    List = rep.cdslTransctionDisplayupdated(stdate, endDate, boid.Text, isinid != "" ? isinid : "na", SettlementID != "" ? SettlementID : "na", HttpContext.Current.Session["userbranchHierarchy"].ToString());
                    DataColumn dc0 = new DataColumn("CdslTransaction_ID", System.Type.GetType("System.String"));
                    List.Columns.Add(dc0);


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
                            if (List.Rows[k - 1]["CDSLISIN_Number"].ToString() == List.Rows[k]["CDSLISIN_Number"].ToString() && List.Rows[k - 1]["CdslTransaction_SettlementID"].ToString() == List.Rows[k]["CdslTransaction_SettlementID"].ToString()
                                        && List.Rows[k]["transactionType"].ToString() == List.Rows[k - 1]["transactionType"].ToString())
                            {
                                List.Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["CdslTransaction_Quantity"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                            }
                            else
                            {
                                List.Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                                List.Rows[k - 1]["ClosingStatus"] = "Y";
                                List.Rows[k]["openingStatus"] = "Y";
                            }
                        }
                        else
                        {

                            List.Rows[0]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                            List.Rows[0]["openingStatus"] = "Y";
                            List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "Y";
                        }


                        List.Rows[k]["Create_User"] = Session["userid"].ToString();
                        List.Rows[k]["RowNo"] = k + 1;


                    }

                    for (int k = 0; k < List.Rows.Count; k++)
                    {
                        List.Rows[k]["CdslTransaction_Quantity"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["CdslTransaction_Quantity"].ToString()));
                        List.Rows[k]["openingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["openingbalance"].ToString()));
                        List.Rows[k]["closingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["closingbalance"].ToString()));
                        List.Rows[k]["credit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["credit"].ToString()));
                        List.Rows[k]["debit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["debit"].ToString()));

                    }



                    SqlBulkCopy sbc = new SqlBulkCopy(con);
                    sbc.DestinationTableName = "Tmp_CDSL_Transction";

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

                        lblTotalTransction.Text = List.Rows.Count.ToString();
                        lblTotalTransction1.Text = lblTotalTransction.Text;



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


                }
                List.Clear();
                List.Dispose();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSdtval", "alert('Date Is Outside Of Financial Year!!');", true);

            }

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
            // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            // {

            //SqlCommand cmd = new SqlCommand("cdslFeatchTransction", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@userid", Session["userid"]);

            //cmd.Parameters.AddWithValue("@startRowIndex", startIndex);

            //cmd.Parameters.AddWithValue("@endIndex", endIndex);



            //cmd.CommandTimeout = 0;
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cmd;
            //da.Fill(RstTable);
            RstTable = rep.cdslFeatchTransction(Session["userid"].ToString(), startIndex, endIndex);


            // }

            dtTemp = RstTable;









            //////////////////////////////////////////////////


            String strHtml = String.Empty;


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            string prevIsin = "", prevSett = "", prevtrnasType = "";


            int flag = 0;

            for (int k = 0; k < RstTable.Rows.Count; k++)
            {


                if (prevIsin == RstTable.Rows[k]["CDSLISIN_Number"].ToString() && prevSett == RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() &&
                        prevtrnasType == RstTable.Rows[k]["transactionType"].ToString())
                {
                    flag = flag + 1;

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }





                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";


                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


                    }


                }
                else
                {


                    prevIsin = RstTable.Rows[k]["CDSLISIN_Number"].ToString();
                    prevSett = RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString();
                    prevtrnasType = RstTable.Rows[k]["transactionType"].ToString();

                    strHtml += "<tr style=\"background-color: #FDE9D9;text-align:left\"><td>ISIN</td><td><b>" + RstTable.Rows[k]["CDSLISIN_Number"].ToString() + "</b></td>";
                    strHtml += "<td>Security Name</td><td><b>" + RstTable.Rows[k]["CDSLISIN_ShortName"].ToString() + RstTable.Rows[k]["transactionType"].ToString() + "</b></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() + "</b></td></tr>";


                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }



                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";

                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


                    }

                }

            }



            strHtml += "</table>";

            display.InnerHtml = strHtml;
            EmailHTML = strHtml;
            ////ViewState["prevIsin"] = prevIsin;
            ///ViewState["prevSett"] = prevSett; 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);


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










        protected void btnTransnNext_Click(object sender, EventArgs e)
        {
            ViewState["startRowIndex"] = ((int)ViewState["startRowIndex"] + (int)ViewState["pageSize"]);

            if ((int)ViewState["startRowIndex"] >= (int)ViewState["totalRecord"])
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



        private string companyId()
        {
            string[] yearSplit;

            financilaYear = HttpContext.Current.Session["LastFinYear"].ToString(); //HttpContext.Current.Session["LastFinYear"].ToString();




            yearSplit = financilaYear.Split('-');

            billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";

            DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
                                                    " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
                                                        " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")");

            //companyId = lastSegMemt.Rows[0][0].ToString();
            //dpId = lastSegMemt.Rows[0][2].ToString();
            //SegmentId = lastSegMemt.Rows[0][1].ToString();

            return lastSegMemt.Rows[0][0].ToString();
        }




        protected void btnEmail_Click(object sender, EventArgs e)
        {
            string disptbl = "";
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    generateEmailTable();
                    string emailbdy = "";
                    if (DT.Rows.Count == 1)
                    {
                        disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                        disptbl += "<tr><td><table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"left\">Client ID:  " + boid.Text + "</td><td align=\"left\"> Category:  " + category.Text + "</td><td align=\"left\">Status:  " + status.Text + "</td><td align=\"left\">Name Of Holders:  " + holders.Text + "</td></tr></table></td></tr>";
                        disptbl += "<tr><td>";
                        emailbdy = disptbl + EmailHTML + "</td></tr></table>";

                    }
                    else
                    {

                        pageindex = i;
                        bindTopHeader(pageindex);
                        pageing();
                        disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                        disptbl += "<tr><td><table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\">Client ID:" + boid.Text + "</td><td align=\"left\"> Category:" + category.Text + "</td><td align=\"left\">Status:" + status.Text + "</td><td align=\"left\">Name Of Holders:" + holders.Text + "</td></tr></table></td></tr>";
                        disptbl += "<tr><td>";
                        emailbdy = disptbl + EmailHTML + "</td></tr></table>";

                    }

                    DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_email  ", "  * ", "eml_cntId='" + DT.Rows[i]["CdslClients_BOID"] + "'");
                    string mailid = string.Empty;
                    //if (dtCnt.Rows.Count > 0)
                    //{
                    //    mailid = dtCnt.Rows[0]["eml_email"].ToString().Trim();
                    //}


                    //DataTable dtCnt = oDBEngine.GetDataTable(" master_cdslclients  ", " * ", "CdslClients_BOID='" + DT.Rows[i]["CdslClients_BOID"] + "'");
                    //string mailid = dtCnt.Rows[0]["CdslClients_emailid"].ToString();
                    string branchContact = DT.Rows[i]["CdslClients_BOID"].ToString();
                    string billdate = objConverter.ArrangeDate2(txtstartDate.Value.ToString()) + " To " + objConverter.ArrangeDate2(txtendDate.Value.ToString());
                    string Subject = "CDSL Transaction Report for  " + billdate;
                    for (int k = 0; k < dtCnt.Rows.Count; k++)
                    {
                        mailid = dtCnt.Rows[k]["eml_email"].ToString().Trim();
                        if (mailid.Length > 0)
                        {
                            if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                            {
                                //if (i == DT.Rows.Count - 1)
                                //{
                                //    pageindex = 0;
                                //    bindTopHeader(pageindex);
                                //    pageing();
                                //}
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>hidesearch();</script>");
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                            }
                            else
                            {
                                if (DT.Rows.Count <= 1)
                                {
                                    //if (i == DT.Rows.Count - 1)
                                    //{
                                    //    pageindex = 0;
                                    //    bindTopHeader(pageindex);
                                    //    pageing();
                                    //}
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>hidesearch();</script>");
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                }

                            }
                        }
                        else
                        {
                            //if (i == DT.Rows.Count - 1)
                            //{
                            //    pageindex = 0;
                            //    bindTopHeader(pageindex);
                            //    pageing();
                            //}
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript25", "<script>hidesearch();</script>");
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript59", "<script>alert('Mail id not found....');</script>");


                        }
                    }
                    EmailHTML = "";
                    disptbl = "";
                }
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
            // dtEx.Columns.Add("Ref. No.");
            dtEx.Columns.Add("Particulars");
            dtEx.Columns.Add("Credit");
            dtEx.Columns.Add("Debit");
            dtEx.Columns.Add("Current Balance");

            //strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
            //strHtml += "<td colspan=2><b>Particulars</b></td>";
            //strHtml += "<td align=\"right\"><b>Credit</b></td>";
            //strHtml += "<td align=\"right\"><b>Debit</b></td>";
            //strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";

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
                row9[0] = "Client ID: " + boid.Text + "   *******    Category:" + category.Text + "   *******   Status:  " + status.Text + "   ********   Name Of Holders:  " + holders.Text;
                row9[1] = "Test";
                dtEx.Rows.Add(row9);


                string prevIsin = "", prevSett = "", prevtrnasType = "";
                int flag = 0;

                dtTemp.Clear();
                generateExTable();

                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {


                    if (prevIsin == dtTemp.Rows[k]["CDSLISIN_Number"].ToString() && prevSett == dtTemp.Rows[k]["CdslTransaction_SettlementID"].ToString() &&
                            prevtrnasType == dtTemp.Rows[k]["transactionType"].ToString())
                    {
                        flag = flag + 1;

                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "Y")
                        {
                            DataRow row1 = dtEx.NewRow();
                            row1[0] = dtTemp.Rows[k]["stdate"].ToString();
                            row1[1] = "Opening Balance";
                            row1[2] = "";
                            row1[3] = "";
                            row1[4] = dtTemp.Rows[k]["openingbalance"].ToString();
                            dtEx.Rows.Add(row1);
                            //strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                        }



                        DataRow row2 = dtEx.NewRow();
                        row2[0] = dtTemp.Rows[k]["CdslTransaction_Date"].ToString();
                        row2[1] = dtTemp.Rows[k]["CdslTransaction_Description"].ToString();
                        row2[2] = ZeroCheck(dtTemp.Rows[k]["credit"].ToString());
                        row2[3] = ZeroCheck(dtTemp.Rows[k]["debit"].ToString());
                        row2[4] = dtTemp.Rows[k]["CdslTransaction_Quantity"].ToString();
                        dtEx.Rows.Add(row2);

                        //strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                        //strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                        //strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";

                        DataRow row3 = dtEx.NewRow();
                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "Y")
                        {

                            row3[0] = dtTemp.Rows[k]["eddate"].ToString();
                            row3[1] = "Closing Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = dtTemp.Rows[k]["closingbalance"].ToString();
                            dtEx.Rows.Add(row3);

                            //strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


                        }


                    }
                    else
                    {


                        prevIsin = dtTemp.Rows[k]["CDSLISIN_Number"].ToString();
                        prevSett = dtTemp.Rows[k]["CdslTransaction_SettlementID"].ToString();
                        prevtrnasType = dtTemp.Rows[k]["transactionType"].ToString();

                        DataRow row4 = dtEx.NewRow();
                        row4[0] = "ISIN : " + dtTemp.Rows[k]["CDSLISIN_Number"].ToString() + " .............. Security Name: " + dtTemp.Rows[k]["CDSLISIN_ShortName"].ToString() + dtTemp.Rows[k]["transactionType"].ToString() + " ..............  Settlement No:" + dtTemp.Rows[k]["CdslTransaction_SettlementID"].ToString();
                        row4[1] = "Test";
                        //row4[1] = dtTemp.Rows[k]["CDSLISIN_Number"].ToString();
                        //row4[2] = "Security Name";
                        //row4[3] = dtTemp.Rows[k]["CDSLISIN_ShortName"].ToString() + dtTemp.Rows[k]["transactionType"].ToString();
                        //row4[4] = "Settlement No:" + dtTemp.Rows[k]["CdslTransaction_SettlementID"].ToString();
                        dtEx.Rows.Add(row4);

                        //strHtml += "<tr style=\"background-color: #FDE9D9;text-align:left\"><td>ISIN</td><td><b>" + RstTable.Rows[k]["CDSLISIN_Number"].ToString() + "</b></td>";
                        //strHtml += "<td>Security Name</td><td><b>" + RstTable.Rows[k]["CDSLISIN_ShortName"].ToString() + RstTable.Rows[k]["transactionType"].ToString() + "</b></td>";
                        //strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() + "</b></td></tr>";


                        DataRow row5 = dtEx.NewRow();
                        row5[0] = "Date";
                        row5[1] = "Particulars";
                        row5[2] = "Credit";
                        row5[3] = "Debit";
                        row5[4] = "Current Balance";
                        dtEx.Rows.Add(row5);

                        //strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                        //strHtml += "<td colspan=2><b>Particulars</b></td>";
                        //strHtml += "<td align=\"right\"><b>Credit</b></td>";
                        //strHtml += "<td align=\"right\"><b>Debit</b></td>";
                        //strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";

                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "Y")
                        {
                            DataRow row6 = dtEx.NewRow();
                            row6[0] = dtTemp.Rows[k]["stdate"].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = dtTemp.Rows[k]["openingbalance"].ToString();
                            dtEx.Rows.Add(row6);

                            //strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                        }



                        // flag = flag + 1;
                        DataRow row7 = dtEx.NewRow();
                        row7[0] = dtTemp.Rows[k]["CdslTransaction_Date"].ToString();
                        row7[1] = dtTemp.Rows[k]["CdslTransaction_Description"].ToString();
                        row7[2] = ZeroCheck(dtTemp.Rows[k]["credit"].ToString());
                        row7[3] = ZeroCheck(dtTemp.Rows[k]["debit"].ToString());
                        row7[4] = dtTemp.Rows[k]["CdslTransaction_Quantity"].ToString();
                        dtEx.Rows.Add(row7);

                        //strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                        //strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                        //strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                        //strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";

                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "Y")
                        {
                            DataRow row8 = dtEx.NewRow();
                            row8[0] = dtTemp.Rows[k]["eddate"].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = dtTemp.Rows[k]["closingbalance"].ToString();
                            dtEx.Rows.Add(row8);

                            //strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                            //strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td></td>";
                            //strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


                        }

                    }

                }


            }

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "CDSL Transaction Report (From  " + objConverter.ArrangeDate2(txtstartDate.Value.ToString()) + " To " + objConverter.ArrangeDate2(txtendDate.Value.ToString()) + ")";
            dtReportHeader.Rows.Add(DrRowR1);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);



            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtEx, "CDSL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtEx, "CDSL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }


        }


        void generateExTable()
        {


            int startIndex, endIndex;

            startIndex = 0;
            //endIndex = startIndex + (int)ViewState["pageSize"];

            //if (endIndex >= (int)ViewState["totalRecord"])
            //{
            //    endIndex = (int)ViewState["totalRecord"];
            //}
            endIndex = (int)ViewState["totalRecord"];

            lblTransction.Text = endIndex.ToString();
            lblTransction1.Text = lblTransction.Text;



            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand("cdslFeatchTransction", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@userid", Session["userid"]);

            //    cmd.Parameters.AddWithValue("@startRowIndex", startIndex);

            //    cmd.Parameters.AddWithValue("@endIndex", endIndex);



            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(RstTable);



            //}
            RstTable = rep.cdslFeatchTransction(Session["userid"].ToString(), startIndex, endIndex);
            dtTemp = RstTable;



        }


        void generateEmailTable()
        {


            int startIndex, endIndex;

            startIndex = 0;
            endIndex = (int)ViewState["totalRecord"];
            //endIndex = startIndex + (int)ViewState["pageSize"];

            //if (endIndex >= (int)ViewState["totalRecord"])
            //{
            //    endIndex = (int)ViewState["totalRecord"];
            //}


            //lblTransction.Text = endIndex.ToString();
            //lblTransction1.Text = lblTransction.Text;



            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand("cdslFeatchTransction", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@userid", Session["userid"]);

            //    cmd.Parameters.AddWithValue("@startRowIndex", startIndex);

            //    cmd.Parameters.AddWithValue("@endIndex", endIndex);



            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(RstTable);



            //}
            RstTable = rep.cdslFeatchTransction(Session["userid"].ToString(), startIndex, endIndex);
            dtTemp = RstTable;









            //////////////////////////////////////////////////


            String strHtml = String.Empty;


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            string prevIsin = "", prevSett = "", prevtrnasType = "";


            int flag = 0;

            for (int k = 0; k < RstTable.Rows.Count; k++)
            {


                if (prevIsin == RstTable.Rows[k]["CDSLISIN_Number"].ToString() && prevSett == RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() &&
                        prevtrnasType == RstTable.Rows[k]["transactionType"].ToString())
                {
                    flag = flag + 1;

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }





                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";


                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


                    }


                }
                else
                {


                    prevIsin = RstTable.Rows[k]["CDSLISIN_Number"].ToString();
                    prevSett = RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString();
                    prevtrnasType = RstTable.Rows[k]["transactionType"].ToString();

                    strHtml += "<tr style=\"background-color: #FDE9D9;text-align:left\"><td>ISIN</td><td><b>" + RstTable.Rows[k]["CDSLISIN_Number"].ToString() + "</b></td>";
                    strHtml += "<td>Security Name</td><td><b>" + RstTable.Rows[k]["CDSLISIN_ShortName"].ToString() + RstTable.Rows[k]["transactionType"].ToString() + "</b></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() + "</b></td></tr>";


                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }



                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";

                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {

                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";


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