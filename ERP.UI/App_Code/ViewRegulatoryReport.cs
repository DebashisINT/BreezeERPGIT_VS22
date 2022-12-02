using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing.Imaging;
using BusinessLogicLayer;

namespace ViewRegulatoryReportNameSpace
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
        string SingleDouble = "S";
        string inherit = "";
        string cseheader = "";
        string csesignature = "";
        string csefooter1 = "";
        string csefooter2 = "";
        string csefooter3 = "";
        string csefooter4 = "";
        string csefooter5 = "";
        string csefooter6 = "";
        string csefooter7 = "";
        string csefooter8 = "";
        string csefooter9 = "";
        string csefooter10 = "";
        string csefooter11 = "";
        string csefooter12 = "";
        string csefooter13 = "";

        public ViewRegulatoryReport()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int viewdata(string CompanyId, string dpID, string dp, string strDate, string AuthorizeName,
            string EmpName, string EName, string ReportType, int status, string Option, string digitalSignatureid,
            string strFundPayoutDate, string brkflag, string Annexturevalue, string hdnbranch,
            string hdnCustomerIDS, string hdngroup, string groupbytext, string groupbyvalue, string sendparamtype, string netamount)
        {
            DataSet dsData = new DataSet();
            DataSet logo = new DataSet();
            DataSet Signature = new DataSet();
            DataSet dsAnnextureTrade = new DataSet();
            DataSet dsAnnextureSttax = new DataSet();
            DataTable dtblclients = new DataTable();
            DataSet spcall = new DataSet();
            DataRow drow, drow1;
            byte[] logoinByte;
            byte[] SignatureinByte;
            int logoStatus, signatureStatus;
            logoStatus = 1;
            signatureStatus = 1;
            string broker = "";
            string exchange = "";

            DataTable dtbroker = oDBEngine.GetDataTable("Select top 1 isnull(exch_GrievanceID,'') as exch_GrievanceID from tbl_master_companyexchange where exch_internalID  = '" + HttpContext.Current.Session["usersegid"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
            DataTable dtexchange = oDBEngine.GetDataTable("Select top 1 isnull(exch_InvestorGrievanceID,'') as exch_InvestorGrievanceID from tbl_master_companyexchange where exch_internalID='" + HttpContext.Current.Session["usersegid"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
            //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');  MULTI
            string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');

            DataSet DigitalSignatureDs = new DataSet();
            DataTable dtcmp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
            inherit = Annexturevalue.ToString();



            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;


                if ((dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                    cmd.CommandText = "[Contract_Report14]";
                else if (dp == "ICEX - COMM" || dp == "NSEL - SPOT" || dp == "NCDEX - COMM" || dp == "MCXSX - CDX" || dp == "NSE - CDX" || dp == "USE - CDX" || dp == "MCX - COMM" || dp == "UCX - COMM" || dp == "BSE - CDX")
                    cmd.CommandText = "[ICEXContract_Report]";
                //else if ((dp == "BSE - CM") || (dp == "BSE - FO"))
                //    cmd.CommandText = "[Contract_Report_BSECM]";

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                cmd.Parameters.AddWithValue("@DpId", dpID);
                cmd.Parameters.AddWithValue("@dp", dp);
                cmd.Parameters.AddWithValue("@tradedate", strDate);
                cmd.Parameters.AddWithValue("@AuthorizeName", AuthorizeName);
                cmd.Parameters.AddWithValue("@Mode", Option);
                cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                cmd.Parameters.AddWithValue("@strFundPayoutDate", strFundPayoutDate);
                cmd.Parameters.AddWithValue("@BrkgFlag", brkflag);
                cmd.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                cmd.Parameters.AddWithValue("@Branch", hdnbranch.ToString());
                cmd.Parameters.AddWithValue("@Customer", hdnCustomerIDS);
                cmd.Parameters.AddWithValue("@Group", hdngroup.ToString());
                cmd.Parameters.AddWithValue("@Groupbytext", groupbytext);
                cmd.Parameters.AddWithValue("@Reporttype", ReportType.ToString().Trim());
                cmd.Parameters.AddWithValue("@Groupbyvalue", groupbyvalue.ToString().Trim());
                cmd.Parameters.AddWithValue("@sendtypeparameter", sendparamtype.ToString().Trim());
                cmd.Parameters.AddWithValue("@Employeename", EName.Split('[').GetValue(0).ToString().Trim());
                cmd.Parameters.AddWithValue("@netammountchk", netamount.ToString().Trim());


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dsData);
                if (dsData.Tables[0].Rows.Count > 0)
                {
                    if (ReportType == "Print")
                    {
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.Connection = con;

                        if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                            cmd1.CommandText = "Contract_AnnextureExchnageTrades1";
                        else if (dp == "ICEX - COMM" || dp == "NSEL - SPOT" || dp == "NCDEX - COMM" || dp == "MCXSX - CDX" || dp == "NSE - CDX" || dp == "USE - CDX" || dp == "MCX - COMM" || dp == "UCX - COMM" || dp == "BSE - CDX")
                            cmd1.CommandText = "[Contract_AnnextureExchnageTrades]";

                        cmd1.CommandType = CommandType.StoredProcedure;
                        //if ((dp == "BSE - FO") || (dp == "BSE - CM") || (dp == "MCXSX - FO") || (dp == "NSE - FO") || (dp == "MCXSX - CM") || (dp == "NSE - CM") || (dp == "NCDEX - COMM") || (dp == "NSEL - SPOT") || (dp == "MCX - COMM") || (dp == "ICEX - COMM") || (dp == "MCXSX - CDX") || (dp == "USE - CDX") || (dp == "NSE - CDX"))
                        //{

                        cmd1.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd1.Parameters.AddWithValue("@DpId", dpID);
                        cmd1.Parameters.AddWithValue("@CustomerID", "pdf");
                        cmd1.Parameters.AddWithValue("@ContractNote", "");
                        cmd1.Parameters.AddWithValue("@tradedate", strDate);
                        cmd1.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd1.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                        cmd1.Parameters.AddWithValue("@Reporttype", ReportType.ToString().Trim());

                        //}
                        cmd1.CommandTimeout = 0;
                        SqlDataAdapter da1 = new SqlDataAdapter();
                        da1.SelectCommand = cmd1;
                        da1.Fill(dsAnnextureTrade);
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = con;
                        cmd2.CommandText = "Contract_AnnextureSttax";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CompanyID", CompanyId);
                        cmd2.Parameters.AddWithValue("@DpId", dpID);
                        cmd2.Parameters.AddWithValue("@CustomerID", "pdf");
                        cmd2.Parameters.AddWithValue("@tradedate", strDate);
                        cmd2.Parameters.AddWithValue("@SettlementNumber", Convert.ToInt32(HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7)));
                        cmd2.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                        cmd2.Parameters.AddWithValue("@Reporttype", ReportType.ToString().Trim());

                        cmd2.CommandTimeout = 0;
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.SelectCommand = cmd2;
                        da2.Fill(dsAnnextureSttax);

                    }
                }
                //else
                //{
                //    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hi11", "alert('No Data Found');", true);

                //}
            }




            logo.Tables.Add();
            logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            drow1 = logo.Tables[0].NewRow();

            string seglogo = "";
            seglogo = "logo_" + dtcmp.Rows[0][0].ToString() + ".bmp";
            if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\" + seglogo), out logoinByte) == 1)
            {
                drow1["Image"] = logoinByte;

            }
            else
            {
                logoStatus = 0;
                //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

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

                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        drow["Image"] = SignatureinByte;
                        drow["Status"] = "1";
                        drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();
                        drow["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                        if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                        {
                            drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["CustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["ComExchangeTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                        }

                    }
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hi20", "alert('No Data Found');", true);

                    //}


                }
                else
                {

                    signatureStatus = 0;
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                        {

                            drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();
                            drow["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                            drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["CustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");


                        }
                        else if ((dp == "ICEX - COMM") || (dp == "MCX - COMM") || (dp == "UCX - COMM") || (dp == "USE - CDX") || (dp == "NSE - CDX") || (dp == "MCXSX - CDX") || (dp == "NCDEX - COMM") || (dp == "NSEL - SPOT") || dp == "BSE - CDX")
                        {

                            drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();
                            drow["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                            drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["ComCustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");


                        }
                    }
                    //else
                    //    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "sign", "alert('Signature not Found.');", true);

                }

            }


            if (dsData.Tables[0].Rows.Count > 0)
            {

                if (ReportType == "Print")
                {
                    if ((dp == "ICEX - COMM"))
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSESPOTRegulatory.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");

                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }
                    else if (dp == "MCXSX - CDX")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\MCXRegulatory.xsd");
                        // dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        // dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        // Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if (dp == "MCX - COMM")
                    {
                        //  dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSECDXRegulatory.xsd");
                        // dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        // dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        // dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        // Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }

                    else if (dp == "NSE - CDX")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSECDXRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //  Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if (dp == "USE - CDX")
                    {
                        // dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSECDXRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //  Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if (dp == "NSEL - SPOT")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSESPOTRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if (dp == "NCDEX - COMM")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NCDEXRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        // logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContractNotesBSECM.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContractNotes.xsd");
                        ////    ////Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        ////    ////Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchange.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }
                    if (status == 1)
                    {
                        //Add Company name and Employee name in the signature//
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();//.Split('[').GetValue(0);
                            drow["signName"] = EName.Split('[').GetValue(0);
                            //signature.Tables[0].Rows.Add(drow);
                        }

                    }
                    else
                    {
                        drow["Status"] = "2";
                    }
                    Signature.Tables[0].Rows.Add(drow);
                    dsData.Tables[0].Columns.Add("DPName", System.Type.GetType("System.Byte[]"));
                    for (int puc = 0; puc < dsData.Tables[0].Rows.Count; puc++)
                    {
                        dsData.Tables[0].Rows[puc]["DPName"] = Signature.Tables[0].Rows[0]["Image"];
                    }
                    dsData.AcceptChanges();

                }
                else if (ReportType == "Digital")
                {
                    if ((dp == "ICEX - COMM") || (dp == "MCX - COMM"))
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSESPOTRegulatory.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }
                    else if (dp == "NCDEX - COMM")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NCDEXRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if (dp == "MCXSX - CDX")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\MCXRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        // dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }
                    else if (dp == "NSE - CDX")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSECDXRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        // logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if (dp == "USE - CDX")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSECDXRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        // logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml
                    }
                    else if (dp == "NSEL - SPOT")
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSESPOTRegulatory.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchangeSPOT.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ICEXRegulatory.xsd");
                        //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        //Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }
                    // else if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO"))
                    else if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                    {
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContractNotesBSECM.xsd");
                        //dsData.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContractNotes.xsd");
                        ////// Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
                        //// //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
                        //dsAnnextureTrade.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureExchange.xsd");
                        //dsAnnextureSttax.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AnnextureSttax.xsd");
                        //logo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\logo.xsd");
                        //Signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xsd");
                        // Signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\Signature.xml");
                    }



                    ReportDocument ICEXReportDocument = new ReportDocument();
                    //ReportDocument cdslTransctionReportDocu = new ReportDocument();




                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        drow["companyName"] = dsData.Tables[0].Rows[0]["cmp_name"].ToString();//.Split('[').GetValue(0);
                        drow["signName"] = EName.Split('[').GetValue(0);
                        drow["Status"] = "3";
                    }

                    if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["CustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                            drow["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                        }
                    }
                    else if ((dp == "ICEX - COMM") || (dp == "NSEL - SPOT") || (dp == "NSE - CDX") || (dp == "MCXSX - CDX") || (dp == "USE - CDX") || (dp == "MCX - COMM") || (dp == "UCX - COMM") || (dp == "NCDEX - COMM") || dp == "BSE - CDX")
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            drow["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["comCustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                            drow["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                        }
                    }


                    Signature.Tables[0].Rows.Add(drow);

                    dsData.Tables[0].Columns.Add("DPName", System.Type.GetType("System.Byte[]"));
                    for (int puc = 0; puc < dsData.Tables[0].Rows.Count; puc++)
                    {
                        dsData.Tables[0].Rows[puc]["DPName"] = Signature.Tables[0].Rows[0]["Image"];
                    }
                    dsData.AcceptChanges();

                    if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\")))
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\"));
                    }


                    //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
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
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        if ((dp == "NSE - CM") || (dp == "NSE - FO") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                        {
                            DigitalSignatureDs.Tables[0].Rows[0]["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["CustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                            DigitalSignatureDs.Tables[0].Rows[0]["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                        }
                        else if ((dp == "ICEX - COMM") || (dp == "NSEL - SPOT") || (dp == "NSE - CDX") || (dp == "MCXSX - CDX") || (dp == "NSE - CDX") || (dp == "MCX - COMM") || (dp == "UCX - COMM") || (dp == "NCDEX - COMM") || dp == "BSE - CDX")
                        {
                            DigitalSignatureDs.Tables[0].Rows[0]["Date"] = Convert.ToDateTime(dsData.Tables[0].Rows[0]["comCustomerTrades_TradeDate"]).ToString("dd-MMM-yyyy");
                            DigitalSignatureDs.Tables[0].Rows[0]["Place"] = dsData.Tables[0].Rows[0]["CompanyCity"].ToString();
                        }

                    }
                    if (EName == "")
                    {
                        for (int M = 0; M < DigitalSignatureDs.Tables[0].Rows.Count; M++)
                        {
                            DigitalSignatureDs.Tables[0].Rows[M]["signName"] = "";
                        }

                    }
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hi25", "alert('No Data Found');", true);

                    //}

                    string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

                    tmpPdfPath = string.Empty;
                    ReportPath = string.Empty;
                    signPath = string.Empty;
                    finalResult = string.Empty;

                    digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();

                    tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");

                    signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + digitalSignatureid + ".pfx";
                    signPdfPath = objConverter.DirectoryPath(out VirtualPath);


                    finalResult = generateindivisualPdf(strDate, ICEXReportDocument, dsData, Signature, logo, dp, "Yes", digitalSignaturePassword,
                              DigitalSignatureDs, signPath, ReportPath
                             , tmpPdfPath, CompanyId, dpID, dpID, signPdfPath, VirtualPath,
                             HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), dsAnnextureTrade, dsAnnextureSttax);




                    if (finalResult == "Success")
                    {

                        //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Successfully Document Generated.');", true);
                        return 6;

                    }
                    if (finalResult == "")
                    {
                        return 16;
                    }


                }
                //else
                //{
                //    //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Option Not Selected!');", true);
                //    //return;
                //}




                string path;
                path = string.Empty;


                ReportDocument ICEXDoc = new ReportDocument();



                if (dp == "ICEX - COMM")
                //path = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatoryonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "MCXSX - CDX")//Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\mcx_cdx.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\mcx_cdxonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "BSE - CDX")//Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\bsecdx.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\bsecdxonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "NSE - CDX")
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\nse_cdx.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\nse_cdxonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "USE - CDX")
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\use_cdx.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\use_cdxonly.rpt");
                    }
                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "NSEL - SPOT")
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\NSESPOTRegulatory.rpt");
                    }

                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\NSESPOTRegulatoryonly.rpt");


                    }





                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureSPOT.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "NCDEX - COMM")
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\NCDEXRegulatory.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\NCDEXRegulatoryonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }

                if (dp == "MCX - COMM")
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatory.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatoryonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }
                if (dp == "UCX - COMM")
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ucx.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ucxonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnextureNCDEX.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                }


                if (dp == "NSE - CM")//Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotes.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\Contractonly.rpt");
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
                if (dp == "MCXSX - CM")//Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\McxcmContract.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\McxcmContractonly.rpt");
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
                if (dp == "MCXSX - FO")//Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\McxfoContract.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\McxfoContractonly.rpt");
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
                if (dp == "NSE - FO")//Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotes.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotesonly.rpt");
                    }

                    else if (Annexturevalue == "3")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeAnnexture.rpt");
                    }
                    else if (Annexturevalue == "4")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\AnnextureSttax.rpt");
                    }
                    else if (Annexturevalue == "5")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotesexchange.rpt");
                    }
                }
                if (dp == "BSE - CM")// Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotesBSECM.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {

                        path = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotesBSECMonly.rpt");

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
                if (dp == "BSE - FO")//Done
                {
                    if (Annexturevalue == "1")
                    {
                        path = HttpContext.Current.Server.MapPath("..\\Reports\\bsefo.rpt");
                    }
                    else if (Annexturevalue == "2")
                    {

                        path = HttpContext.Current.Server.MapPath("..\\Reports\\bsefoonly.rpt");

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

                ICEXDoc.Load(path);
                ICEXDoc.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);


                if ((dp == "ICEX - COMM"))
                {
                    //ICEXDoc.SetDataSource(dsData.Tables[0]);
                    //ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                    //ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                    //ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }


                }

                else if ((dp == "MCXSX - CDX"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }

                else if ((dp == "BSE - CDX"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }

                else if ((dp == "NSE - CDX"))
                {
                    if (Annexturevalue == "1")
                    {
                        //ICEXDoc.SetParameterValue("@Field3", (object)br);
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }
                else if ((dp == "USE - CDX"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }

                else if ((dp == "NSE - CM") || (dp == "BSE - CM") || (dp == "BSE - FO") || (dp == "MCXSX - CM") || (dp == "MCXSX - FO"))
                {
                    try
                    {
                        if (Annexturevalue == "1")
                        {
                            ICEXDoc.SetDataSource(dsData.Tables[0]);
                            ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                            ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                            ICEXDoc.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";
                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        }
                        else if (Annexturevalue == "2")
                        {




                            ICEXDoc.SetDataSource(dsData.Tables[0]);
                            ICEXDoc.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        }
                        else if (Annexturevalue == "3")
                        {
                            ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                            ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        }
                        else if (Annexturevalue == "4")
                        {
                            ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                            ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        }
                    }
                    catch (System.Runtime.InteropServices.COMException exn)
                    {

                        //throw;
                    }
                }
                else if (dp == "NSE - FO")
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if ((Annexturevalue == "2") || (Annexturevalue == "5"))
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";

                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        if (Annexturevalue == "5")
                            ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
                        else
                            ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }
                else if ((dp == "NSEL - SPOT"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }
                else if ((dp == "NCDEX - COMM"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";

                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";

                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                }

                else if ((dp == "MCX - COMM"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }


                }
                else if ((dp == "UCX - COMM"))
                {
                    if (Annexturevalue == "1")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["AnnextureExchange"].SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.Subreports["AnnextureSttax"].SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "2")
                    {
                        ICEXDoc.SetDataSource(dsData.Tables[0]);
                        ICEXDoc.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                        ICEXDoc.Subreports["Signature"].SetDataSource(Signature.Tables[0]);
                        if (dtbroker.Rows.Count > 0)
                        {
                            if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                            {
                                broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                            }
                            else
                                broker = "";
                        }
                        else
                            broker = "";
                        if (dtexchange.Rows.Count > 0)
                        {
                            if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                            {
                                exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                            }
                            else
                                exchange = "";
                        }
                        else
                            exchange = "";
                        ICEXDoc.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                        ICEXDoc.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "3")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureTrade.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }
                    else if (Annexturevalue == "4")
                    {
                        ICEXDoc.SetDataSource(dsAnnextureSttax.Tables[0]);
                        ICEXDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    }


                }
                if (digitalSignatureid == "")
                {

                    ICEXDoc.SetParameterValue("@itact1", "");
                    ICEXDoc.SetParameterValue("@itact2", "");
                    if (dp == "MCXSX - CDX")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "MCXSX-CDX Contract Report");
                        return 4;
                    }
                    if (dp == "NSE - CDX")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSE-CDX Contract Report");
                        return 4;
                    }
                    if (dp == "BSE - CDX")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "BSE-CDX Contract Report");
                        return 4;
                    }
                    if (dp == "USE - CDX")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "USE-CDX Contract Report");
                        return 4;
                    }
                    if (dp == "NSEL - SPOT")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSEL-SPOT Contract Report");
                        return 4;
                    }
                    if (dp == "NCDEX - COMM")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NCDEX-COMM Contract Report");
                        return 4;
                    }
                    else if (dp == "MCX - COMM")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "MCX-COMM Contract Report");
                        return 4;

                    }
                    else if (dp == "UCX - COMM")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "UCX-COMM Contract Report");
                        return 4;

                    }
                    else if (dp == "ICEX - COMM")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "ICEX-COMM Contract Report");
                        return 4;

                    }
                    else if (dp == "NSE - CM")
                    {
                        //ICEXDoc.SetParameterValue("@sttatached", "");
                        ICEXDoc.SetParameterValue("@sttatached", (inherit == "1") ? "STT & Trade statement is furnished as annexure to the contract Note." : "");
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSE-CM Contract Report");
                        return 4;

                    }
                    else if (dp == "MCXSX - CM")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "MCXSX-CM Contract Report");
                        return 4;

                    }
                    else if (dp == "MCXSX - FO")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "MCXSX-FO Contract Report");
                        return 4;

                    }
                    else if (dp == "NSE - FO")
                    {
                        //ICEXDoc.SetParameterValue("@sttatached", "");
                        ICEXDoc.SetParameterValue("@sttatached", (inherit == "1") ? "STT & Trade statement is furnished as annexure to the contract Note." : "");
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSE-FO Contract Report");
                        //ICEXDoc.Dispose();
                        // GC.Collect();
                        return 4;

                    }
                    else if (dp == "BSE - CM")
                    {
                        DataTable dtcse = oDBEngine.GetDataTable("select isnull(exch_membershipType,'') from tbl_master_companyExchange where exch_internalId='" + HttpContext.Current.Session["usersegid"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
                        if (dtcse.Rows.Count > 0)
                        {
                            if (dtcse.Rows[0][0].ToString().Trim() == "CTM")
                            {
                                cseheader = "SUBJECT TO CALCUTTA JURISDICTION";
                                csesignature = "Trading Member(s) of the Calcutta Stock Exchange Association Limited";
                                csefooter1 = "1. This Contract is made subject to the Rules, Bye-laws and Regulations and usages of The Calcutta Stock Exchange Association Limited.";
                                csefooter2 = "2. Brokerage has been charged as stated above at rates not exceeding the official scale of brokerage.";
                                csefooter3 = "3. This contract is subject to the jurisdiction of the Courts in Kolkata.";
                                csefooter4 = "4. In the event of any claim (whether admitted or not) difference or dispute arising between you and me/us out of these transactions the matter shall be referred to";
                                csefooter5 = "arbitration in Kolkata as provided in the Rules, Bye-laws and Regulations of The Calcutta Stock Exchange Association Limited.";
                                csefooter6 = "5. This contract constitutes and shall be deemed to constitute as provided overleaf an agreement between you and me/us that all claims (whether admitted or not),";
                                csefooter7 = "differences and disputes in respect of any dealings, transactions and contracts of a date prior or subsequent to the date of this contract (including any question";
                                csefooter8 = "whether such dealings, transactions or contracts have been entered into or not) shall be submitted to and decided by arbitration in Kolkata as provided in the Rules";
                                csefooter9 = "Bye-laws and Regulations of The Calcutta Stock Exchange Association Limited.";
                                csefooter10 = "6. The provisions printed overleaf form a part of the contract.";
                                csefooter11 = "*Trades executed on the Bombay Stock Exchange. In terms of the agreement between the Calcutta and Bombay Stock Exchange under Section 13 of the Securities Contracts";
                                csefooter12 = "(Regulation) Act, 1956.";

                            }
                            else
                            {
                                cseheader = "BOMBAY STOCK EXCHANGE LIMITED CM";
                                csesignature = "Trading Member(s) of the Bombay Stock Exchange Limited";
                                csefooter1 = "1. This Contract shall be governed and is made subject to the Rules, Bye-laws and Regulations and circulars/directions of BSE Ltd(Exchange) and SEBI from time to time.";
                                csefooter2 = "3. In the event of any claim (whether admitted or not) difference or dispute arising between you and me/us out of these contract,the matter shall be referred to arbitration in the concerned Regional Arbitration Centre as provided in the Rules, Bye-laws and ";
                                csefooter3 = "Regulations of Exchange,the details of which are available on www.bseindia.com.";
                                csefooter4 = "4. The Courts in Mumbai shall have exclusive jurisdiction in respect of all proceedings to which the Exchange is a party, and in respect of all other proceedings, the Courts having jurisdiction over the area in which the respective Regional Arbitration Centre is ";
                                csefooter5 = "situated, shall have jurisdiction.";
                                csefooter6 = "";
                                csefooter7 = "";
                                csefooter8 = "";
                                csefooter9 = "";
                                csefooter10 = "";
                                csefooter11 = "";
                                csefooter12 = "";
                            }

                        }
                        ICEXDoc.SetParameterValue("@cseheader", (object)cseheader.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csesig", (object)csesignature.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter1", (object)csefooter1.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter2", (object)csefooter2.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter3", (object)csefooter3.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter4", (object)csefooter4.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter5", (object)csefooter5.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter6", (object)csefooter6.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter7", (object)csefooter7.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter8", (object)csefooter8.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter9", (object)csefooter9.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter10", (object)csefooter10.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter11", (object)csefooter11.ToString().Trim());
                        ICEXDoc.SetParameterValue("@csefooter12", (object)csefooter12.ToString().Trim());

                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "BSE-CM Contract Report");
                        return 4;

                    }
                    else if (dp == "BSE - FO")
                    {
                        ICEXDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "BSE-FO Contract Report");
                        return 4;

                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    return 6;
                }
            }
            else
            {
                return 5;
            }

        }

        public string generateindivisualPdf(string strDate, ReportDocument ICEXReportDocument, DataSet Clients, DataSet signature, DataSet logo,
                                          string module, string DigitalCertified, string digitalSignaturePassword,
                                          DataSet DigitalSignatureDs, string SignPath, string reportPath
                                          , string tmpPDFPath, string CompanyId, string dpId,
                                            string SegmentId, string signPdfPath, string VirtualPath,
                                             string user, string LastFinYear, DataSet dstradeannexture, DataSet dsSttaxannexture)
        {


            string status;
            status = string.Empty;
            DataTable FilterClients = new DataTable();
            DataTable FilterSummary = new DataTable();
            DataTable FilterAccountSummary = new DataTable();
            DataTable FilterHolding = new DataTable();
            string broker = "";
            string exchange = "";
            ICEXReportDocument = Utility_CrystalReport.GetReport(ICEXReportDocument.GetType());
            DataTable dtbroker = oDBEngine.GetDataTable("Select top 1 isnull(exch_GrievanceID,'') as exch_GrievanceID from tbl_master_companyexchange where exch_internalID  = '" + HttpContext.Current.Session["usersegid"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
            DataTable dtexchange = oDBEngine.GetDataTable("Select top 1 isnull(exch_InvestorGrievanceID,'') as exch_InvestorGrievanceID from tbl_master_companyexchange where exch_internalID='" + HttpContext.Current.Session["usersegid"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");

            DataView viewClients = new DataView(Clients.Tables[0]);
            //if ((module == "ICEX - COMM"))
            //    DistinctBillNumber = viewClients.ToTable(true, new string[] { "ComCustomerTrades_CustomerID", "ComCustomerTrades_ContractNoteNumber" });
            if ((module == "NSE - CM") || (module == "NSE - FO") || (module == "BSE - CM") || (module == "BSE - FO") || (module == "MCXSX - CM") || (module == "MCXSX - FO"))
                DistinctBillNumber = viewClients.ToTable(true, new string[] { "CustomerTrades_CustomerID", "CustomerTrades_ContractNoteNumber", "CustomerTrades_SettlementNumber", "CustomerTrades_SettlementType", "CustomerTrades_TradeDate" });
            if ((module == "ICEX - COMM") || (module == "NSEL - SPOT") || (module == "NCDEX - COMM") || (module == "MCXSX - CDX") || (module == "NSE - CDX") || (module == "USE - CDX") || (module == "MCX - COMM") || (module == "UCX - COMM") || (module == "BSE - CDX"))
                DistinctBillNumber = viewClients.ToTable(true, new string[] { "ComCustomerTrades_CustomerID", "ComCustomerTrades_ContractNoteNumber", "ComCustomerTrades_SettlementNumber", "ComCustomerTrades_SettlementType", "ComCustomerTrades_TradeDate" });

            //else if (module == "NSDL")
            //    DistinctBillNumber = viewClients.ToTable(true, new string[] { "DPBillSummary_BillNumber", "NsdlClients_BenAccountID" });
            //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');  MULTI
            string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');


            foreach (DataRow dr in DistinctBillNumber.Rows)
            {

                /////////////////////////////
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    //if ((module == "NSE - CM") || (module == "BSE - CM") || (module == "MCXSX - CM"))
                    //{

                    dsSttaxannexture.Clear();
                    dsSttaxannexture.AcceptChanges();


                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = con;
                    cmd2.CommandText = "Contract_AnnextureSttax";
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    cmd2.Parameters.AddWithValue("@DpId", HttpContext.Current.Session["UserSegid"].ToString());
                    if ((module == "NSE - CM") || (module == "NSE - FO") || (module == "BSE - CM") || (module == "BSE - FO") || (module == "MCXSX - CM") || (module == "MCXSX - FO"))
                    {
                        cmd2.Parameters.AddWithValue("@CustomerID", "'" + dr["CustomerTrades_CustomerID"].ToString() + "'");
                        cmd2.Parameters.AddWithValue("@SettlementNumber", dr["CustomerTrades_SettlementNumber"].ToString().Trim());
                        cmd2.Parameters.AddWithValue("@SettlementType", dr["CustomerTrades_SettlementType"].ToString().Trim());
                        cmd2.Parameters.AddWithValue("@tradedate", Convert.ToDateTime(dr["CustomerTrades_TradeDate"].ToString().Trim()));
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@CustomerID", "'" + dr["ComCustomerTrades_CustomerID"].ToString() + "'");
                        cmd2.Parameters.AddWithValue("@SettlementNumber", dr["ComCustomerTrades_SettlementNumber"].ToString().Trim());
                        cmd2.Parameters.AddWithValue("@SettlementType", dr["ComCustomerTrades_SettlementType"].ToString().Trim());
                        cmd2.Parameters.AddWithValue("@tradedate", Convert.ToDateTime(dr["ComCustomerTrades_TradeDate"].ToString().Trim()));
                    }
                    cmd2.Parameters.AddWithValue("@Reporttype", "Print");


                    cmd2.CommandTimeout = 0;
                    SqlDataAdapter da2 = new SqlDataAdapter();
                    da2.SelectCommand = cmd2;
                    da2.Fill(dsSttaxannexture);

                    dstradeannexture.Clear();
                    dstradeannexture.AcceptChanges();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = con;
                    if ((module == "NSE - CM") || (module == "NSE - FO") || (module == "BSE - CM") || (module == "BSE - FO") || (module == "MCXSX - CM") || (module == "MCXSX - FO"))
                        cmd1.CommandText = "Contract_AnnextureExchnageTrades1";
                    else
                        cmd1.CommandText = "Contract_AnnextureExchnageTrades";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    cmd1.Parameters.AddWithValue("@DpId", HttpContext.Current.Session["UserSegid"].ToString());
                    if ((module == "NSE - CM") || (module == "NSE - FO") || (module == "BSE - CM") || (module == "BSE - FO") || (module == "MCXSX - CM") || (module == "MCXSX - FO"))
                    {
                        cmd1.Parameters.AddWithValue("@CustomerID", "'" + dr["CustomerTrades_CustomerID"].ToString() + "'");
                        cmd1.Parameters.AddWithValue("@tradedate", Convert.ToDateTime(dr["CustomerTrades_TradeDate"].ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@ContractNote", "'" + dr["CustomerTrades_ContractNoteNumber"].ToString() + "'");
                        cmd1.Parameters.AddWithValue("@SettlementNumber", dr["CustomerTrades_SettlementNumber"].ToString().Trim());
                        cmd1.Parameters.AddWithValue("@SettlementType", dr["CustomerTrades_SettlementType"].ToString().Trim());
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@CustomerID", "'" + dr["ComCustomerTrades_CustomerID"].ToString() + "'");
                        cmd1.Parameters.AddWithValue("@tradedate", Convert.ToDateTime(dr["ComCustomerTrades_TradeDate"].ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@ContractNote", "'" + dr["ComCustomerTrades_ContractNoteNumber"].ToString() + "'");
                        cmd1.Parameters.AddWithValue("@SettlementNumber", dr["ComCustomerTrades_SettlementNumber"].ToString().Trim());
                        cmd1.Parameters.AddWithValue("@SettlementType", dr["ComCustomerTrades_SettlementType"].ToString().Trim());
                    }
                    cmd1.Parameters.AddWithValue("@Reporttype", "Print");

                    cmd1.CommandTimeout = 0;
                    SqlDataAdapter da3 = new SqlDataAdapter();
                    da3.SelectCommand = cmd1;
                    da3.Fill(dstradeannexture);


                }










                //////////////////////
                if (module == "ICEX - COMM")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatoryonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ICEXRegulatory.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "ICEX - COMM")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }
                }

                else if (module == "MCX - COMM")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatory.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatoryonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\MCXRegulatory.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "MCX - COMM")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }

                else if (module == "UCX - COMM")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ucx.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ucxonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ucx.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "UCX - COMM")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "MCXSX - CDX")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\mcx_cdx.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\mcx_cdxonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\mcx_cdx.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "MCXSX - CDX")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }

                else if (module == "BSE - CDX")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\bsecdx.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\bsecdxonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\bsecdx.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "BSE - CDX")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }

                else if (module == "NSE - CDX")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\nse_cdx.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\nse_cdxonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\nse_cdx.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "NSE - CDX")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "USE - CDX")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\use_cdx.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\use_cdxonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\use_cdx.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "USE - CDX")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }

                else if (module == "NSEL - SPOT")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NSESPOTRegulatory.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NSESPOTRegulatoryonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NSESPOTRegulatory.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "NSEL - SPOT")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "NCDEX - COMM")
                {
                    try
                    {
                        viewClients.RowFilter = "ComCustomerTrades_CustomerID = '" + dr["ComCustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NCDEXRegulatory.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NCDEXRegulatoryonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NCDEXRegulatory.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["ComCustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["ComCustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);

                            }
                            ICEXReportDocument.Subreports["subContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "NCDEX - COMM")
                            {
                                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                                status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "7",
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                                FilterClients.Rows[0]["eml_email"].ToString(), Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"),
                                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                                VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            }

                            if (status != "Success")
                            {

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "NSE - CM")
                {
                    try
                    {



                        viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' and CustomerTrades_SettlementNumber = '" + dr["CustomerTrades_SettlementNumber"] + "' and CustomerTrades_ContractNoteNumber = '" + dr["CustomerTrades_ContractNoteNumber"] + "' and CustomerTrades_SettlementType = '" + dr["CustomerTrades_SettlementType"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {

                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotes.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\Contractonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotes.rpt");
                            }


                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);
                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            if (inherit == "1")
                            {
                                if (dstradeannexture.Tables[0].Rows.Count > 0)
                                {
                                    for (int p = 0; p < dstradeannexture.Tables[0].Rows.Count; p++)
                                    {
                                        if (FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() == dstradeannexture.Tables[0].Rows[p]["CustomerTrades_CustomerID"].ToString())
                                        {
                                            ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                            break;
                                        }
                                    }
                                }
                                for (int m = 0; m < dsSttaxannexture.Tables[0].Rows.Count; m++)
                                {
                                    if ((FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_CustomerID"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_SettlementNumber"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementType"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_SettlementType"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_ContractNoteNumber"].ToString().Trim()))
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                for (int m1 = 0; m1 < dsSttaxannexture.Tables[0].Rows.Count; m1++)
                                {
                                    if ((FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_CustomerID"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_SettlementNumber"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementType"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_SettlementType"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_ContractNoteNumber"].ToString().Trim()))
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }

                            }
                            ICEXReportDocument.SetDataSource(FilterClients);
                            ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.SetParameterValue("@sttatached", (inherit == "3") ? "" : (inherit == "2") ? "STT statement is furnished as annexure to the contract Note." : "STT & Trade statement is furnished as annexure to the contract Note.");
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

                                break;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "MCXSX - CM")
                {
                    try
                    {


                        //viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' and CustomerTrades_SettlementNumber = '" + dr["CustomerTrades_SettlementNumber"] + "' and CustomerTrades_SettlementType = '" + dr["CustomerTrades_SettlementType"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {

                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\McxcmContract.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\McxcmContractonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\McxcmContract.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            if (inherit == "1")
                            {
                                if (dstradeannexture.Tables[0].Rows.Count > 0)
                                {
                                    for (int p = 0; p < dstradeannexture.Tables[0].Rows.Count; p++)
                                    {
                                        if (FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() == dstradeannexture.Tables[0].Rows[p]["CustomerTrades_CustomerID"].ToString())
                                        {
                                            ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                            break;
                                        }
                                    }
                                }
                                for (int m = 0; m < dsSttaxannexture.Tables[0].Rows.Count; m++)
                                {
                                    if ((FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_CustomerID"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_SettlementNumber"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementType"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_SettlementType"].ToString().Trim()))
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                for (int m1 = 0; m1 < dsSttaxannexture.Tables[0].Rows.Count; m1++)
                                {
                                    if ((FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_CustomerID"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_SettlementNumber"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementType"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_SettlementType"].ToString().Trim()))
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }

                            }
                            ICEXReportDocument.SetDataSource(FilterClients);


                            ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "MCXSX - CM")
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

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "NSE - FO")
                {
                    try
                    {
                        viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotes.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotesonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotes.rpt");
                            }
                            if (inherit == "4")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\FOContractNotesexchange.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                for (int m1 = 0; m1 < dsSttaxannexture.Tables[0].Rows.Count; m1++)
                                {
                                    if (FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_CustomerID"].ToString())
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }

                            }
                            ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            //ICEXReportDocument.SetParameterValue("@sttatached", "STT statement is furnished as annexure to the contract Note.");
                            //ICEXReportDocument.SetParameterValue("@sttatached", (inherit == "3") ? "" : "STT & Trade statement is furnished as annexure to the contract Note.");
                            ICEXReportDocument.SetParameterValue("@sttatached", (inherit == "3") ? "" : (inherit == "2") ? "STT statement is furnished as annexure to the contract Note." : "STT & Trade statement is furnished as annexure to the contract Note.");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "NSE - FO")
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

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "MCXSX - FO")
                {
                    try
                    {
                        viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\McxfoContract.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\McxfoContractonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\McxfoContract.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                for (int m1 = 0; m1 < dsSttaxannexture.Tables[0].Rows.Count; m1++)
                                {
                                    if (FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_CustomerID"].ToString())
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }

                            }
                            ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "MCXSX - FO")
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

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }


                }
                else if (module == "BSE - CM")
                {
                    try
                    {
                        //viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' and CustomerTrades_SettlementNumber = '" + dr["CustomerTrades_SettlementNumber"] + "' and CustomerTrades_SettlementType = '" + dr["CustomerTrades_SettlementType"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotesBSECM.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotesBSECMonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ContractNotesBSECM.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                            ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);


                            //if (inherit == "1")
                            //{

                            //    ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                            //    ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            //}
                            if (inherit == "1")
                            {
                                if (dstradeannexture.Tables[0].Rows.Count > 0)
                                {
                                    for (int p = 0; p < dstradeannexture.Tables[0].Rows.Count; p++)
                                    {
                                        if (FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() == dstradeannexture.Tables[0].Rows[p]["CustomerTrades_CustomerID"].ToString())
                                        {
                                            ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                            break;
                                        }
                                    }
                                }
                                for (int m = 0; m < dsSttaxannexture.Tables[0].Rows.Count; m++)
                                {
                                    if ((FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_CustomerID"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_SettlementNumber"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementType"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m]["Sttax_SettlementType"].ToString().Trim()))
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                for (int m1 = 0; m1 < dsSttaxannexture.Tables[0].Rows.Count; m1++)
                                {
                                    if ((FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_CustomerID"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementNumber"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_SettlementNumber"].ToString().Trim()) && (FilterClients.Rows[0]["CustomerTrades_SettlementType"].ToString().Trim() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_SettlementType"].ToString().Trim()))
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }

                            }




                            ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            DataTable dtcse = new DataTable();
                            dtcse = oDBEngine.GetDataTable("select isnull(exch_membershipType,'') from tbl_master_companyExchange where exch_internalId='" + HttpContext.Current.Session["usersegid"].ToString() + "' and exch_compid='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
                            if (dtcse.Rows.Count > 0)
                            {
                                if (dtcse.Rows[0][0].ToString().Trim() == "CTM")
                                {
                                    cseheader = "SUBJECT TO CALCUTTA JURISDICTION";
                                    csesignature = "Trading Member(s) of the Calcutta Stock Exchange Association Limited";
                                    csefooter1 = "1. This Contract is made subject to the Rules, Bye-laws and Regulations and usages of The Calcutta Stock Exchange Association Limited.";
                                    csefooter2 = "2. Brokerage has been charged as stated above at rates not exceeding the official scale of brokerage.";
                                    csefooter3 = "3. This contract is subject to the jurisdiction of the Courts in Kolkata.";
                                    csefooter4 = "4. In the event of any claim (whether admitted or not) difference or dispute arising between you and me/us out of these transactions the matter shall be referred to";
                                    csefooter5 = "arbitration in Kolkata as provided in the Rules, Bye-laws and Regulations of The Calcutta Stock Exchange Association Limited.";
                                    csefooter6 = "5. This contract constitutes and shall be deemed to constitute as provided overleaf an agreement between you and me/us that all claims (whether admitted or not),";
                                    csefooter7 = "differences and disputes in respect of any dealings, transactions and contracts of a date prior or subsequent to the date of this contract (including any question";
                                    csefooter8 = "whether such dealings, transactions or contracts have been entered into or not) shall be submitted to and decided by arbitration in Kolkata as provided in the Rules";
                                    csefooter9 = "Bye-laws and Regulations of The Calcutta Stock Exchange Association Limited.";
                                    csefooter10 = "6. The provisions printed overleaf form a part of the contract.";
                                    csefooter11 = "*Trades executed on the Bombay Stock Exchange. In terms of the agreement between the Calcutta and Bombay Stock Exchange under Section 13 of the Securities Contracts";
                                    csefooter12 = "(Regulation) Act, 1956.";

                                }
                                else
                                {
                                    cseheader = "BOMBAY STOCK EXCHANGE LIMITED CM";
                                    csesignature = "Trading Member(s) of the Bombay Stock Exchange Limited";
                                    csefooter1 = "1. This Contract shall be governed and is made subject to the Rules, Bye-laws and Regulations and circulars/directions of BSE Ltd(Exchange) and SEBI from time to time.";
                                    csefooter2 = "3. In the event of any claim (whether admitted or not) difference or dispute arising between you and me/us out of these contract,the matter shall be referred to arbitration in the concerned Regional Arbitration Centre as provided in the Rules, Bye-laws and ";
                                    csefooter3 = "Regulations of Exchange,the details of which are available on www.bseindia.com.";
                                    csefooter4 = "4. The Courts in Mumbai shall have exclusive jurisdiction in respect of all proceedings to which the Exchange is a party, and in respect of all other proceedings, the Courts having jurisdiction over the area in which the respective Regional Arbitration Centre is ";
                                    csefooter5 = "situated, shall have jurisdiction.";
                                    csefooter6 = "";
                                    csefooter7 = "";
                                    csefooter8 = "";
                                    csefooter9 = "";
                                    csefooter10 = "";
                                    csefooter11 = "";
                                    csefooter12 = "";
                                }

                            }
                            ICEXReportDocument.SetParameterValue("@cseheader", (object)cseheader.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csesig", (object)csesignature.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter1", (object)csefooter1.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter2", (object)csefooter2.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter3", (object)csefooter3.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter4", (object)csefooter4.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter5", (object)csefooter5.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter6", (object)csefooter6.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter7", (object)csefooter7.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter8", (object)csefooter8.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter9", (object)csefooter9.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter10", (object)csefooter10.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter11", (object)csefooter11.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@csefooter12", (object)csefooter12.ToString().Trim());
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "BSE - CM")
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

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }
                }

                else if (module == "BSE - FO")
                {
                    try
                    {
                        viewClients.RowFilter = "CustomerTrades_CustomerID = '" + dr["CustomerTrades_CustomerID"] + "' AND (eml_email is not null OR eml_email<>'') AND (cnt_ContractDeliveryMode is null or cnt_ContractDeliveryMode<>'P') AND (eml_status<>'N')";
                        FilterClients = viewClients.ToTable();
                        if (FilterClients.Rows.Count > 0)
                        {
                            if (inherit == "1")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\bsefo.rpt");
                            }
                            if (inherit == "3")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\bsefoonly.rpt");
                            }
                            if (inherit == "2")
                            {
                                reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\bsefo.rpt");
                            }

                            string pdfstr = tmpPDFPath + module + "-" + Convert.ToDateTime(strDate).ToString("ddMMyyyy") + "-" + FilterClients.Rows[0]["CustomerTrades_ContractNoteNumber"].ToString() + "-" + FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() + ".pdf";
                            signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                            ICEXReportDocument.Load(reportPath);

                            ICEXReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                            ICEXReportDocument.SetDataSource(FilterClients);
                            if (inherit == "1")
                            {

                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                            }
                            if (inherit == "2")
                            {
                                for (int pom = 0; pom < dstradeannexture.Tables[0].Rows.Count; pom++)
                                {

                                    dstradeannexture.Tables[0].Rows[pom].Delete();

                                }
                                dstradeannexture.AcceptChanges();
                                ICEXReportDocument.Subreports["AnnextureExchange"].SetDataSource(dstradeannexture.Tables[0]);
                                for (int m1 = 0; m1 < dsSttaxannexture.Tables[0].Rows.Count; m1++)
                                {
                                    if (FilterClients.Rows[0]["CustomerTrades_CustomerID"].ToString() == dsSttaxannexture.Tables[0].Rows[m1]["Sttax_CustomerID"].ToString())
                                    {
                                        ICEXReportDocument.Subreports["AnnextureSttax"].SetDataSource(dsSttaxannexture.Tables[0]);
                                        break;
                                    }
                                }

                            }
                            ICEXReportDocument.Subreports["SubContract"].SetDataSource(logo.Tables[0]);
                            ICEXReportDocument.Subreports["Signature"].SetDataSource(DigitalSignatureDs.Tables[0]);
                            if (dtbroker.Rows.Count > 0)
                            {
                                if (dtbroker.Rows[0]["exch_GrievanceID"] != "")
                                {
                                    broker = "Grievance EmailID[Member]: - " + dtbroker.Rows[0]["exch_GrievanceID"].ToString();
                                }
                                else
                                    broker = "";
                            }
                            else
                                broker = "";

                            if (dtexchange.Rows.Count > 0)
                            {
                                if (dtexchange.Rows[0]["exch_InvestorGrievanceID"] != "")
                                {
                                    exchange = "Grievance EmailID[Exchange]: - " + dtexchange.Rows[0]["exch_InvestorGrievanceID"].ToString();
                                }
                                else
                                    exchange = "";
                            }
                            else
                                exchange = "";
                            ICEXReportDocument.SetParameterValue("@Broker", (object)broker.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@Exchange", (object)exchange.ToString().Trim());
                            ICEXReportDocument.SetParameterValue("@itact1", "**This contract is duly authenticated by means of digital signature as specified in the IT ACT,2000 and the rules made there under.**");
                            ICEXReportDocument.SetParameterValue("@itact2", "**Please be informed that Non-receipt of bounced mail notification by the trading member shall amount to delivery of contract note at the e-mail ID of the Constituent.***");
                            ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);

                            if (module == "BSE - FO")
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

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "err";
                        HttpContext.Current.Session["Error"] = "err";
                        break;
                    }

                }
            }
            return status;

        }

        public string generateindivisualPdfcombined(ReportDocument ICEXReportDocument, DataSet Datafetch,
         string DigitalCertified, string digitalSignaturePassword, string SignPath, string reportPath,
         string tmpPDFPath, string CompanyId, string signPdfPath, string VirtualPath, string user,
         string LastFinYear, string digitalbranch, string digitalbranchid, string digitalemployeeid,
         string datefor, string IsSegAllOrCurrent, string EmpName)
        {
            string status;
            status = string.Empty;
            DataSet DataFetch_Temp = new DataSet();
            DataTable FilterClients = new DataTable();
            DataTable FilterClients1 = new DataTable();
            DataTable FilterClients2 = new DataTable();
            DataTable FilterClients3 = new DataTable();
            DataTable FilterClients4 = new DataTable();
            DataTable FilterClients5 = new DataTable();
            DataTable FilterClients6 = new DataTable();
            DataTable FilterClients7 = new DataTable();
            DataTable FilterClients8 = new DataTable();


            DataTable FilterSummary = new DataTable();
            DataTable FilterAccountSummary = new DataTable();
            DataTable FilterHolding = new DataTable();

            ICEXReportDocument = Utility_CrystalReport.GetReport(ICEXReportDocument.GetType());

            DataView viewClients0 = null;
            viewClients0 = new DataView(Datafetch.Tables[0]);
            DistinctBillNumber = viewClients0.ToTable(true, new string[] { "Clientid", "tradedatecast" });
            //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';'); MULTI
            string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');

            string TokenType = EmpName.Split('*')[0];
            string DigitalSignatureID = null;
            int GeneratedPDFCount = 0;

            if (TokenType == "E")
            {
                DigitalSignatureID = EmpName.Split('*')[1].Split('~')[0].ToString();
                EmpName = EmpName.Split('*')[1].Split('~')[1].Split('[')[0].Trim();
            }

            foreach (DataRow dr in DistinctBillNumber.Rows)
            {


                //////////////////////
                DataView viewClients = null;
                DataView viewClients1 = null;
                DataView viewClients2 = null;
                DataView viewClients3 = null;
                DataView viewClients4 = null;
                DataView viewClients5 = null;
                DataView viewClients6 = null;
                DataView viewClients7 = null;
                DataView viewClients8 = null;

                try
                {
                    viewClients = new DataView(Datafetch.Tables[0]);
                    viewClients1 = new DataView(Datafetch.Tables[1]);
                    viewClients2 = new DataView(Datafetch.Tables[2]);
                    viewClients3 = new DataView(Datafetch.Tables[3]);
                    viewClients4 = new DataView(Datafetch.Tables[4]);
                    viewClients5 = new DataView(Datafetch.Tables[5]);
                    viewClients6 = new DataView(Datafetch.Tables[6]);
                    viewClients7 = new DataView(Datafetch.Tables[7]);
                    viewClients8 = new DataView(Datafetch.Tables[8]);

                    viewClients.RowFilter = "Clientid = '" + dr["Clientid"] + "' AND (ClientEmail is not null OR ClientEmail<>'') AND clientemailtype='Official' and Clientdeliverytype<>'P'";
                    viewClients1.RowFilter = "Clientid = '" + dr["Clientid"] + "'";
                    viewClients2.RowFilter = "Clientid = '" + dr["Clientid"] + "'";
                    viewClients3.RowFilter = "Clientid = '" + dr["Clientid"] + "'";
                    viewClients4.RowFilter = "Clientid = '" + dr["Clientid"] + "'";
                    viewClients5.RowFilter = "Clientid = '" + dr["Clientid"] + "'";
                    viewClients6.RowFilter = "Clientid = '" + dr["Clientid"] + "'";

                    FilterClients = viewClients.ToTable();
                    FilterClients1 = viewClients1.ToTable();
                    FilterClients2 = viewClients2.ToTable();
                    FilterClients3 = viewClients3.ToTable();
                    FilterClients4 = viewClients4.ToTable();
                    FilterClients5 = viewClients5.ToTable();
                    FilterClients6 = viewClients6.ToTable();
                    FilterClients7 = viewClients7.ToTable();
                    FilterClients8 = viewClients8.ToTable();

                    DataFetch_Temp.Tables.Add(FilterClients);
                    DataFetch_Temp.Tables.Add(FilterClients1);
                    DataFetch_Temp.Tables.Add(FilterClients2);
                    DataFetch_Temp.Tables.Add(FilterClients3);
                    DataFetch_Temp.Tables.Add(FilterClients4);
                    DataFetch_Temp.Tables.Add(FilterClients5);
                    DataFetch_Temp.Tables.Add(FilterClients6);
                    DataFetch_Temp.Tables.Add(FilterClients7);
                    DataFetch_Temp.Tables.Add(FilterClients8);

                    if (FilterClients.Rows.Count > 0)
                    {


                        reportPath = HttpContext.Current.Server.MapPath("..\\Reports\\Contractnote_combinedsegment.rpt");
                        string pdfstr = tmpPDFPath + "Combined_Contractnote" + "-" + Convert.ToDateTime(datefor).ToString("ddMMMyyyy") + "-" + FilterClients.Rows[0]["Clientucc"].ToString() + "-" + FilterClients.Rows[0]["id"].ToString() + "-" + FilterClients.Rows[0]["Clientid"].ToString() + "-" + HttpContext.Current.Session["UserSegID"] + "-" + user + "-" + HttpContext.Current.Session["LastFinYear"].ToString().Replace("-", "") + "-" + DigitalSignatureID + ".pdf";
                        signPdfPath = objConverter.DirectoryPath(out VirtualPath);
                        ICEXReportDocument.Load(reportPath);
                        ICEXReportDocument.SetDataSource(DataFetch_Temp);
                        ICEXReportDocument.Subreports["contractnote_cumbill"].SetDataSource(DataFetch_Temp.Tables[5]);
                        ICEXReportDocument.VerifyDatabase();
                        ICEXReportDocument.SetParameterValue("@empname", EmpName);
                        ICEXReportDocument.SetParameterValue("@SingleOrAllSegment", IsSegAllOrCurrent);
                        ICEXReportDocument.SetParameterValue("@SingleDoublePrint", "0");
                        ICEXReportDocument.SetParameterValue("@NtShowTotlBrkg", "0");
                        ICEXReportDocument.SetParameterValue("@NtShowBrnchName", "0");
                        ICEXReportDocument.SetParameterValue("@NtShowOrderTradeNo", "0");
                        ICEXReportDocument.SetParameterValue("@NtPrntTrdAnnexure", "0");
                        ICEXReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);
                        //oGenericMethod = new GenericMethod();


                        if (TokenType != "E")
                        {
                            string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='Contract Note/Annexure Printing'", 1);
                            status = objConverter.DigitalCertificate(pdfstr, SignPath, digitalSignaturePassword, "Authentication",
                            digitalbranch, CompanyId, HttpContext.Current.Session["usersegid"].ToString(), "20",
                            digitalemployeeid, FilterClients.Rows[0]["ClientEmail"].ToString(), Convert.ToDateTime(datefor).ToString("dd-MMM-yyyy"),
                            digitalbranchid, VirtualPath, signPdfPath, HttpContext.Current.Session["userid"].ToString(),
                            HttpContext.Current.Session["LastFinYear"].ToString(), Convert.ToInt32(str[0]));
                            if (status != "Success")
                            {
                                break;
                            }
                        }
                        else
                        {
                            GeneratedPDFCount = GeneratedPDFCount + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    status = "err";
                    //HttpContext.Current.Session["Error"] = "err";
                    break;
                }

                DataFetch_Temp.Tables.Clear();
                FilterClients.Clear(); FilterClients1.Clear(); FilterClients2.Clear(); FilterClients3.Clear(); FilterClients4.Clear();
                FilterClients5.Clear(); FilterClients6.Clear(); FilterClients7.Clear(); FilterClients8.Clear();

            }
            if (TokenType == "E")
                status = status + "~" + GeneratedPDFCount;
            return status;
        }
    }

}