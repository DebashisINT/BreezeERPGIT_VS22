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
    /// <summary>
    /// Summary description for ViewRegulatoryReport
    /// </summary>
    public class ViewRegulatoryReport
    {
        Converter objConverter = new Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        DBEngine oDBEngine = new DBEngine();

        DataTable DistinctBillNumber;
        DataTable DistinctClient;
        string result;

        public ViewRegulatoryReport()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int viewdata(string CompanyId, string dpID, string dp, string strDate, string StrID, string ContractNote, string AuthorizeName, string EmpName, string EName, string ReportType, int status, string Option, string digitalSignatureid, string strFundPayoutDate, string brkflag, string Annexturevalue)
        {
            DataSet dsData = new DataSet();
            DataSet logo = new DataSet();
            DataSet Signature = new DataSet();
            DataSet dsAnnextureTrade = new DataSet();
            DataSet dsAnnextureSttax = new DataSet();
            DataTable dtblclients = new DataTable();
            DataRow drow, drow1;
            byte[] logoinByte;
            byte[] SignatureinByte;
            int logoStatus, signatureStatus;
            logoStatus = 1;
            signatureStatus = 1;
            //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');  MULTI
            string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');
            if (Option == "S")
            {

                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                        cmd.CommandText = "[ICEXRegulatory_Report]";
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                        cmd.CommandText = "[Contract_Report]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (dp == "ICEX - COMM")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "MCX - COMM")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "NSE - CM")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    else if (dp == "NSE - FO")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));

                        //cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dsData);
                    if ((dp == "NSE - FO") || (dp == "NSE - CM"))
                    {
                        //dsData.Tables[0].Columns.Add("Customer_email", Type.GetType("System.String"));
                        //DataRow datarow = dsData.Tables[0].NewRow(); 
                        //foreach (DataRow dr in dsData.Tables[0].Rows)
                        //{
                        //    //dr = dsData.Tables[0].NewRow();
                        //    string[] str = oDBEngine.GetFieldValue1("tbl_master_email", "top(1) eml_email", "eml_cntID='" + dr["customertrades_customerID"] + "'AND eml_entity='Customer/Client' AND eml_type='Official'", 1);
                        //    //select top(1) eml_email from tbl_master_email where eml_entity='Customer/Client' AND eml_type='Official'AND eml_cntID='CLP0000302'
                        //    datarow["Customer_email"] = str[0].ToString();
                        //    dr["Customer_email"] = datarow["Customer_email"];
                        //}
                    }
                    else if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        //dsData.Tables[0].Columns.Add("Customer_email", Type.GetType("System.String"));
                        //DataRow datarow = dsData.Tables[0].NewRow(); 
                        //foreach (DataRow dr in dsData.Tables[0].Rows)
                        //{
                        //    //dr = dsData.Tables[0].NewRow();
                        //    string[] str = oDBEngine.GetFieldValue1("tbl_master_email", "top(1) eml_email", "eml_cntID='" + dr["comcustomertrades_customerID"] + "'AND eml_entity='Customer/Client' AND eml_type='Official'", 1);
                        //    //select top(1) eml_email from tbl_master_email where eml_entity='Customer/Client' AND eml_type='Official'AND eml_cntID='CLP0000302'
                        //    datarow["Customer_email"] = str[0].ToString();
                        //    dr["Customer_email"] = datarow["Customer_email"];
                        //}
                    }

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = con;
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                        cmd1.CommandText = "[ICEXRegulatory_Report]";
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                        cmd1.CommandText = "Contract_AnnextureExchnageTrades";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    if (dp == "ICEX - COMM")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd1.Parameters.AddWithValue("@Mode", Option);
                        cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "MCX - COMM")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd1.Parameters.AddWithValue("@Mode", Option);
                        cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "NSE - CM")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        //cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);

                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        //cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        //cmd1.Parameters.AddWithValue("@Mode", Option);
                        //cmd1.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        //cmd1.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        //cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd1.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd1.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                    }
                    else if (dp == "NSE - FO")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        //cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);

                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        //cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        //cmd1.Parameters.AddWithValue("@Mode", Option);
                        //cmd1.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        //cmd1.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        //cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd1.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd1.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));

                        //cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    cmd1.CommandTimeout = 0;
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    da1.SelectCommand = cmd1;
                    da1.Fill(dsAnnextureTrade);

                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = con;
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                        cmd2.CommandText = "[ICEXRegulatory_Report]";
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                        cmd2.CommandText = "Contract_AnnextureSttax";
                    cmd2.CommandType = CommandType.StoredProcedure;
                    if (dp == "ICEX - COMM")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        cmd2.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd2.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd2.Parameters.AddWithValue("@Mode", Option);
                        cmd2.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "MCX - COMM")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        cmd2.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd2.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd2.Parameters.AddWithValue("@Mode", Option);
                        cmd2.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "NSE - CM")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        //cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);

                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        //cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        //cmd1.Parameters.AddWithValue("@Mode", Option);
                        //cmd1.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        //cmd1.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        //cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd2.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd2.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));

                        //cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    else if (dp == "NSE - FO")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        cmd2.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd2.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd2.Parameters.AddWithValue("@Mode", Option);
                        cmd2.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd2.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        cmd2.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd2.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd2.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));

                        //cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    cmd2.CommandTimeout = 0;
                    SqlDataAdapter da2 = new SqlDataAdapter();
                    da2.SelectCommand = cmd2;
                    da2.Fill(dsAnnextureSttax);



                }
            }
            else if (Option == "A")
            {

                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                        cmd.CommandText = "[ICEXRegulatory_Report]";
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                        cmd.CommandText = "[Contract_Report]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (dp == "ICEX - COMM")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "MCX - COMM")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);

                    }
                    else if (dp == "NSE - CM")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd.Parameters.AddWithValue("@strFundPayoutDate", Convert.ToDateTime(strFundPayoutDate).ToString("dd MMM yyyy"));
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                    }
                    else if (dp == "NSE - FO")
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd.Parameters.AddWithValue("@DpId", dpID);
                        cmd.Parameters.AddWithValue("@dp", dp);
                        cmd.Parameters.AddWithValue("@tradedate", strDate);
                        cmd.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd.Parameters.AddWithValue("@Mode", Option);
                        cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dsData);
                    if (dp == "NSE - CM" || dp == "NSE - FO")
                    {
                        if (dsData.Tables[0].Rows.Count > 1)
                        {
                            DataView filterCusromercontactnote = new DataView(dsData.Tables[0]);
                            DistinctClient = filterCusromercontactnote.ToTable(true, new string[] { "CustomerTrades_CustomerID", "CustomerTrades_ContractNoteNumber" });
                            foreach (DataRow dr in DistinctClient.Rows)
                            {

                                ContractNote = "" + ContractNote + ",'" + dr["CustomerTrades_ContractNoteNumber"].ToString() + "'";
                                StrID = "" + StrID + ",'" + dr["CustomerTrades_CustomerID"].ToString() + "'";

                            }
                            //foreach (DataRow dr in dtblclients.Rows)
                            //{
                            //    ContractNote = "" + ContractNote + ",'" + dr["CustomerTrades_ContractNoteNumber"].ToString() + "'";
                            //    StrID = "" + StrID + ",'" + dr["CustomerTrades_CustomerID"].ToString() + "'";
                            //    //str += "," + "'" + val + "'";
                            //}

                            ContractNote = ContractNote.Substring(1, ContractNote.Length - 1);
                            StrID = StrID.Substring(1, StrID.Length - 1);
                        }
                    }

                    DataRow row1 = null;
                    if ((dp == "NSE - FO") || (dp == "NSE - CM"))
                    {
                        //dsData.Tables[0].Columns.Add("Customer_email", Type.GetType("System.String"));
                        //DataRow datarow = dsData.Tables[0].NewRow(); 
                        //foreach (DataRow dr in dsData.Tables[0].Rows)
                        //{

                        //    string[] str = oDBEngine.GetFieldValue1("tbl_master_email", "top(1) eml_email", "eml_cntID='" + dr["customertrades_customerID"] + "'AND eml_entity='Customer/Client' AND eml_type='Official'", 1);
                        //    //select top(1) eml_email from tbl_master_email where eml_entity='Customer/Client' AND eml_type='Official'AND eml_cntID='CLP0000302'
                        //    datarow["Customer_email"] = str[0].ToString();
                        //    dr["Customer_email"] = datarow["Customer_email"];
                        //}
                    }
                    else if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        //dsData.Tables[0].Columns.Add("Customer_email", Type.GetType("System.String"));
                        //DataRow datarow = dsData.Tables[0].NewRow(); 
                        //foreach (DataRow dr in dsData.Tables[0].Rows)
                        //{
                        //    //dr = dsData.Tables[0].NewRow();
                        //    string[] str = oDBEngine.GetFieldValue1("tbl_master_email", "top(1) eml_email", "eml_cntID='" + dr["comcustomertrades_customerID"] + "'AND eml_entity='Customer/Client' AND eml_type='Official'", 1);
                        //    //select top(1) eml_email from tbl_master_email where eml_entity='Customer/Client' AND eml_type='Official'AND eml_cntID='CLP0000302'
                        //    datarow["Customer_email"] = str[0].ToString();
                        //    dr["Customer_email"] = datarow["Customer_email"];
                        //}
                    }

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = con;
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                        cmd1.CommandText = "[ICEXRegulatory_Report]";
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                        cmd1.CommandText = "Contract_AnnextureExchnageTrades";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    if (dp == "ICEX - COMM")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd1.Parameters.AddWithValue("@Mode", Option);
                        cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "MCX - COMM")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd1.Parameters.AddWithValue("@Mode", Option);
                        cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "NSE - CM")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        //cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);

                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        //cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        //cmd1.Parameters.AddWithValue("@Mode", Option);
                        //cmd1.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        //cmd1.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        //cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd1.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd1.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                    }
                    else if (dp == "NSE - FO")
                    {
                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        //cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd1.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);

                        cmd1.Parameters.AddWithValue("@ContractNote", ContractNote);
                        //cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        //cmd1.Parameters.AddWithValue("@Mode", Option);
                        //cmd1.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        //cmd1.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        //cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd1.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd1.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));

                        //cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    cmd1.CommandTimeout = 0;
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    da1.SelectCommand = cmd1;
                    da1.Fill(dsAnnextureTrade);



                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = con;
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                        cmd2.CommandText = "[ICEXRegulatory_Report]";
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                        cmd2.CommandText = "Contract_AnnextureSttax";
                    cmd2.CommandType = CommandType.StoredProcedure;
                    if (dp == "ICEX - COMM")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        cmd2.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd2.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd2.Parameters.AddWithValue("@Mode", Option);
                        cmd2.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "MCX - COMM")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        cmd2.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd2.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd2.Parameters.AddWithValue("@Mode", Option);
                        cmd2.Parameters.AddWithValue("@BrkgFlag", brkflag);
                    }
                    else if (dp == "NSE - CM")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        //cmd1.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);

                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        //cmd1.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        //cmd1.Parameters.AddWithValue("@Mode", Option);
                        //cmd1.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        //cmd1.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        //cmd1.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd2.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd2.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));

                        //cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    else if (dp == "NSE - FO")
                    {
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        cmd2.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);
                        cmd2.Parameters.AddWithValue("@CustomerID", StrID);
                        cmd2.Parameters.AddWithValue("@ContractNote", ContractNote);
                        cmd2.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                        cmd2.Parameters.AddWithValue("@Mode", Option);
                        cmd2.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd2.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                        cmd2.Parameters.AddWithValue("@BrkgFlag", brkflag);
                        cmd2.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd2.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));

                        //cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        //cmd.Parameters.AddWithValue("@SettlementType", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1)));
                    }
                    cmd2.CommandTimeout = 0;
                    SqlDataAdapter da2 = new SqlDataAdapter();
                    da2.SelectCommand = cmd2;
                    da2.Fill(dsAnnextureSttax);



                }
            }

            logo.Tables.Add();
            logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            drow1 = logo.Tables[0].NewRow();
            string seglogo = "";
            if (HttpContext.Current.Session["usersegid"].ToString() != "")
            {
                seglogo = "cntrlogo_" + HttpContext.Current.Session["usersegid"].ToString() + ".jpg";
            }
            else
            {
                seglogo = "logo.jpg";
            }

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

            //signatory//
            signatureStatus = 1;


            Signature.Tables.Add();
            Signature.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            Signature.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
            Signature.Tables[0].Columns.Add("signName", System.Type.GetType("System.String"));
            Signature.Tables[0].Columns.Add("Status", System.Type.GetType("System.String"));
            Signature.Tables[0].Columns.Add("Place", System.Type.GetType("System.String"));
            Signature.Tables[0].Columns.Add("Date", System.Type.GetType("System.String"));

            drow = Signature.Tables[0].NewRow();

            if (status == 1)
            {
                if (objConverter.getSignatureImage(EmpName, out SignatureinByte, dp) == 1)
                {
                    //byte[] S;
                    //String tmpPath1 =HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\dig-signature.pfx");
                    //System.Security.Cryptography.X509Certificates.X509Certificate2 cert2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(tmpPath1, "123456");
                    //S=cert2.RawData;
                    //drow["Image"] = S;

                    drow["Image"] = SignatureinByte;
                    drow["Status"] = "1";
                    drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();
                    drow["Place"] = dsData.Tables[0].Rows[0]["city_name"].ToString();
                    //drow["signName"] = "txtEmpName.Text.Split('[').GetValue(0)";
                    drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["ComExchangeTrades_TradeDate"]).ToString("dd-MMM-yyyy");


                }
                else
                {

                    signatureStatus = 0;

                    //drow["signName"] = "txtEmpName.Text.Split('[').GetValue(0)";
                    if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                    {
                        drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();
                        drow["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                        drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["CustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                    }
                    else if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();
                        drow["Place"] = dsData.Tables[0].Rows[0]["city_name"].ToString();
                        drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["ComExchangeTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                    }
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "sign", "alert('Signature not Found.');", true);
                    //return 10;
                }
            }

            if (dsData.Tables[0].Rows.Count > 0)
            {

                if (ReportType == "Print")
                {
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");

                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                    {
                        dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContractNotes.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchange.xsd");
                        dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }

                    if (status == 1)
                    {
                        //Add Company name and Employee name in the signature//

                        drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();//.Split('[').GetValue(0);
                        drow["signName"] = EName.Split('[').GetValue(0);
                        //signature.Tables[0].Rows.Add(drow);

                    }
                    else
                    {
                        drow["Status"] = "2";
                    }
                    Signature.Tables[0].Rows.Add(drow);
                    //  signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xml");
                    //  signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xsd");

                    //generatePdf(Clients.Tables[0], Summary.Tables[0], AccountSummary.Tables[0], Holding.Tables[0],
                    //              signature.Tables[0], logo.Tables[0], qstr1, "No", digitalSignatureid);

                }
                else if (ReportType == "Digital")
                {
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                    {
                        dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContractNotes.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchange.xsd");
                        dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }



                    ReportDocument ICEXReportDocument = new ReportDocument();
                    //ReportDocument cdslTransctionReportDocu = new ReportDocument();





                    drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();//.Split('[').GetValue(0);
                    drow["signName"] = EName.Split('[').GetValue(0);
                    drow["Status"] = "3";

                    if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                    {
                        drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["CustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                        drow["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                    }
                    else if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["ComExchangeTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                        drow["Place"] = dsData.Tables[0].Rows[0]["city_name"].ToString();
                    }
                    Signature.Tables[0].Rows.Add(drow);

                    DataSet DigitalSignatureDs = new DataSet();

                    if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\")))
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\"));
                    }


                    //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
                    using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {

                        SqlCommand cmdDigital = new SqlCommand("cdsl_EmployeeName", con);
                        cmdDigital.CommandType = CommandType.StoredProcedure;
                        cmdDigital.Parameters.AddWithValue("@ID", digitalSignatureid);
                        cmdDigital.CommandTimeout = 0;
                        SqlDataAdapter daDigital = new SqlDataAdapter();
                        daDigital.SelectCommand = cmdDigital;
                        daDigital.Fill(DigitalSignatureDs);
                    }

                    DigitalSignatureDs.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
                    DigitalSignatureDs.Tables[0].Columns.Add("Place", System.Type.GetType("System.String"));
                    DigitalSignatureDs.Tables[0].Columns.Add("Date", System.Type.GetType("System.String"));
                    DigitalSignatureDs.Tables[0].Rows[0]["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();

                    //drow["signName"] = "txtEmpName.Text.Split('[').GetValue(0)";
                    if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                    {
                        DigitalSignatureDs.Tables[0].Rows[0]["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["CustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                        DigitalSignatureDs.Tables[0].Rows[0]["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                    }
                    else if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        DigitalSignatureDs.Tables[0].Rows[0]["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["ComExchangeTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                        DigitalSignatureDs.Tables[0].Rows[0]["Place"] = dsData.Tables[0].Rows[0]["city_name"].ToString();
                    }

                    string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

                    tmpPdfPath = string.Empty;
                    ReportPath = string.Empty;
                    signPath = string.Empty;
                    finalResult = string.Empty;

                    digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();

                    tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");

                    //string pdfstr = tmpPdfPath + dp + "-" + Convert.ToDateTime(strDate).Month +"-" +HttpContext.Current.Session["LastFinYear"].ToString()+"-"+dsData.Tables[0].Rows[0]["ComExchangeTrades_SettlementNumber"].ToString() + ".pdf";


                    if (dp == "ICEX - COMM")
                    {
                        ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");

                    }
                    if (dp == "MCX - COMM")
                    {
                        ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatory.rpt");
                    }
                    if (dp == "NSE - CM")
                    {
                        ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotes.rpt");
                    }
                    if (dp == "NSE - FO")
                    {
                        ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotes.rpt");

                    }




                    signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + digitalSignatureid + ".pfx";
                    signPdfPath = objConverter.DirectoryPath(out VirtualPath);


                    finalResult = generateindivisualPdf(strDate, ICEXReportDocument, dsData, Signature, logo, dp, "Yes", digitalSignaturePassword,
                              DigitalSignatureDs, signPath, ReportPath
                             , tmpPdfPath, CompanyId, dpID, dpID, signPdfPath, VirtualPath,
                             HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString());




                    if (finalResult == "Success")
                    {

                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Successfully Document Generated.');", true);
                        return 6;

                    }
                    if (finalResult == "")
                    {
                        return 16;
                    }


                }
                else
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Option Not Selected!');", true);
                    //return;
                }




                string path;
                path = string.Empty;


                ReportDocument ICEXDoc = new ReportDocument();
                //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');


                //if (module == "CDSL")
                //    path = HttpContext.Current.Server.MapPath("..\\Reports\\cdslBill.rpt");

                //else if (module == "NSDL")
                //    path = HttpContext.Current.Server.MapPath("..\\Reports\\NsdlBill.rpt");
                if (dp == "ICEX - COMM")
                    path = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");
                if (dp == "MCX - COMM")
                    path = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatory.rpt");
                if (dp == "NSE - CM")
                {
                    if (Annexturevalue == "1" || Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotes.rpt");
                    }
                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnexture.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "NSE - FO")
                    path = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotes.rpt");

                ICEXDoc.Load(path);
                //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + path + "');", true);
                ICEXDoc.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + connPath[2].Substring(connPath[2].IndexOf("=")).Trim() + connPath[3].Substring(connPath[3].IndexOf("=")).Trim() + connPath[0].Substring(connPath[0].IndexOf("=")).Trim() + connPath[1].Substring(connPath[1].IndexOf("=")).Trim() + "');", true);

                if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                {
                    ICEXDoc.SetDataSource(dsData.Tables[0]);
                    ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                    ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                    ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                }
                else if ((dp == "NSE - CM") || (dp == "NSE - FO"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        //ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        //ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }
                if (digitalSignatureid == "")
                {
                    ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "ICEX Contract Report");
                    return 4;
                }
                else
                {

                    return 6;
                }
            }
            else
            {

                //ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                return 5;
            }

        }
        public void generatePdf(DataTable Clients, DataTable Summary, DataTable AccountSummary,
                                              DataTable Holding, DataTable signature, DataTable logo, string module, string DigitalCertified, string digitalSignatureid)
        {
            //string path;
            //path = string.Empty;


            //ReportDocument ICEXDoc = new ReportDocument();
            //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');


            //if (module == "CDSL")
            //    path = HttpContext.Current.Server.MapPath("..\\Reports\\cdslBill.rpt");

            //else if (module == "NSDL")
            //    path = HttpContext.Current.Server.MapPath("..\\Reports\\NsdlBill.rpt");
            //else if (module == "ICEX - COMM")
            //    path = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");


            //ICEXDoc.Load(path);
            ////ICEXDoc.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            //ICEXDoc.SetDataSource(Clients);
        }
        public string generateindivisualPdf(string strDate, ReportDocument ICEXReportDocument, DataSet Clients, DataSet signature, DataSet logo,
                                          string module, string DigitalCertified, string digitalSignaturePassword,
                                          DataSet DigitalSignatureDs, string SignPath, string reportPath
                                          , string tmpPDFPath, string CompanyId, string dpId,
                                            string SegmentId, string signPdfPath, string VirtualPath,
                                             string user, string LastFinYear)
        {


            string status;
            status = string.Empty;
            DataTable FilterClients = new DataTable();
            DataTable FilterSummary = new DataTable();
            DataTable FilterAccountSummary = new DataTable();
            DataTable FilterHolding = new DataTable();

            DataView viewClients = new DataView(Clients.Tables[0]);
            if ((module == "ICEX - COMM") || (module == "MCX - COMM"))
                DistinctBillNumber = viewClients.ToTable(true, new string[] { "ComCustomerTrades_CustomerID", "ComCustomerTrades_ContractNoteNumber" });
            if ((module == "NSE - CM") || (module == "NSE - FO"))
                DistinctBillNumber = viewClients.ToTable(true, new string[] { "CustomerTrades_CustomerID", "CustomerTrades_ContractNoteNumber" });
            //else if (module == "NSDL")
            //    DistinctBillNumber = viewClients.ToTable(true, new string[] { "DPBillSummary_BillNumber", "NsdlClients_BenAccountID" });
            //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';'); MULTI

            string[] connPath =Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');

            foreach (DataRow dr in DistinctBillNumber.Rows)
            {
                if (module == "ICEX - COMM")
                {
                    viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "'AND ComCustomerTrades_ContractNoteNumber= '" + dr["ComCustomerTrades_ContractNoteNumber"] + "'";
                    FilterClients = viewClients.ToTable();

                    reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");

                    string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                    //ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);



                    //SignPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + DigitalCertified + ".pfx";
                    signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                    ICEXReportDocument.Load(reportPath);


                    ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    ICEXReportDocument.SetDataSource(FilterClients);
                    ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                    ICEXReportDocument.Subreports["Signature"].SetDataSource(signature.Tables[0]);
                    ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);


                    if (module == "ICEX - COMM")
                    {
                        //Select * from tbl_trans_menu where mnu_segmentid=11 and mnu_menuName='Contract Note/Annexure Printing'
                        string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                        status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                        DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "1",
                                        DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                        FilterClients.Rows[0]["CustEmail"].ToString(), strDate,
                                        DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                        VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                    }





                    if (status != "Success")
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
                        break;
                    }
                }
                else if (module == "MCX - COMM")
                {
                    viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "'AND ComCustomerTrades_ContractNoteNumber= '" + dr["ComCustomerTrades_ContractNoteNumber"] + "'";
                    FilterClients = viewClients.ToTable();

                    reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatory.rpt");

                    string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                    //ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);



                    //SignPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + DigitalCertified + ".pfx";
                    signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                    ICEXReportDocument.Load(reportPath);


                    ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    ICEXReportDocument.SetDataSource(FilterClients);
                    ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                    ICEXReportDocument.Subreports["Signature"].SetDataSource(signature.Tables[0]);
                    ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);


                    if (module == "MCX - COMM")
                    {
                        string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                        status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                        DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "1",
                                        DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                        FilterClients.Rows[0]["CustEmail"].ToString(), strDate,
                                        DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                        VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                    }
                    if (status != "Success")
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
                        break;
                    }


                }
                else if (module == "NSE - CM")
                {
                    viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' and eml_email IS Not NULL";
                    FilterClients = viewClients.ToTable();
                    if (FilterClients.Rows.Count > 0)
                    {

                        reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotes.rpt");

                        string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";




                        //SignPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + DigitalCertified + ".pfx";
                        signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                        ICEXReportDocument.Load(reportPath);

                        ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                        ICEXReportDocument.SetDataSource(FilterClients);
                        ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                        ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                        ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                        if (module == "NSE - CM")
                        {
                            string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                            status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                            DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                            DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                            FilterClients.Rows[0]["eml_email"].ToString(), strDate,
                                            DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                            VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                        }

                        if (status != "Success")
                        {
                            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
                            break;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert(Customers Not allowed);", true);

                    }

                }
                else if (module == "NSE - FO")
                {
                    viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' and eml_email IS Not NULL";
                    FilterClients = viewClients.ToTable();
                    if (FilterClients.Rows.Count > 0)
                    {

                        reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotes.rpt");

                        string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";
                        //ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);



                        //SignPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + DigitalCertified + ".pfx";
                        signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                        ICEXReportDocument.Load(reportPath);

                        ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                        ICEXReportDocument.SetDataSource(FilterClients);
                        ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                        ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                        ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                        if (module == "NSE - FO")
                        {
                            string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                            status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                            DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "8",
                                            DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                            FilterClients.Rows[0]["eml_email"].ToString(), strDate,
                                            DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                            VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                        }
                        if (status != "Success")
                        {
                            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
                            break;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert(Customers Not allowed);", true);
                        break;
                    }
                }
            }
            return status;
        }
    }
}