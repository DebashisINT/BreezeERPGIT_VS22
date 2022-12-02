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
using CrystalDecisions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing.Imaging;
using BusinessLogicLayer;

public class cdslBillViewCalculation
{
    Converter objConverter = new Converter();
    //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);  MULTI
    DBEngine oDBEngine = new DBEngine();
    CommonUtility ul = new CommonUtility();

    DataTable DistinctBillNumber;
    string result;

    public cdslBillViewCalculation()
    {

        //
        // TODO: Add constructor logic here
        //
    }

    public void cdslBillViewFormatting(string BillingMonth, string CompanyId, string dpId, string SegmentId, string dp)
    {

        String StartDate, EndDate, financilaYear, billnoFinYear;
        financilaYear = HttpContext.Current.Session["LastFinYear"].ToString(); //HttpContext.Current.Session["LastFinYear"].ToString();
        int month = DateTime.Parse("1." + BillingMonth + " 2008").Month;
        string[] yearSplit;

        yearSplit = financilaYear.Split('-');

        billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";

        if (month <= 3)
        {
            financilaYear = yearSplit[1];
        }
        else
        {
            financilaYear = yearSplit[0];
        }

        StartDate = "01 " + BillingMonth + " " + financilaYear;

        DateTime firstDay = new DateTime(Convert.ToInt32(financilaYear), month, 1);
        DateTime lastDayOfMonth = firstDay.AddMonths(1).AddTicks(-1);

        EndDate = String.Format("{0:dd MMM yyyy}", lastDayOfMonth);//dateSplit[1] + " " + month1 + " " + dateSplit[2];





        DataSet cdslClients = new DataSet();

        DataSet cdslSummary = new DataSet();

        //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            if (dp == "CDSL")
                cmd.CommandText = "cdslBill_ShowAllClient1";
            if (dp == "NSDL")
            {
                cmd.CommandText = "sp_NsdlBill_ShowAllClients";
            }

            cmd.CommandType = CommandType.StoredProcedure;

            if (dp == "NSDL")
            {
                cmd.Parameters.AddWithValue("@DpId", dpId);
                cmd.Parameters.AddWithValue("@billNumber", dp + "-" + BillingMonth.ToUpper() + billnoFinYear);
                cmd.Parameters.AddWithValue("@compID", CompanyId);
                cmd.Parameters.AddWithValue("@dp", dp);
                cmd.Parameters.AddWithValue("@firstDay", StartDate);
                cmd.Parameters.AddWithValue("@lastDay", EndDate);
                cmd.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DpId", dpId);
                cmd.Parameters.AddWithValue("@billNumber", "CDSL-" + BillingMonth + billnoFinYear);
                cmd.Parameters.AddWithValue("@compID", CompanyId);
                cmd.Parameters.AddWithValue("@dp", "CDSL");
                cmd.Parameters.AddWithValue("@firstDay", StartDate);
                cmd.Parameters.AddWithValue("@lastDay", EndDate);
                cmd.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);


            }


            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(cdslClients);

            SqlCommand cmdSummary = new SqlCommand("cdslBill_OtherChargeDisplay", con);
            cmdSummary.CommandType = CommandType.StoredProcedure;
            cmdSummary.Parameters.AddWithValue("@DpId", dpId);
            cmdSummary.Parameters.AddWithValue("@billNumber", dp + "-" + BillingMonth.ToUpper() + billnoFinYear);
            cmdSummary.Parameters.AddWithValue("@segmentId", SegmentId);


            cmdSummary.CommandTimeout = 0;
            SqlDataAdapter daSummary = new SqlDataAdapter();
            daSummary.SelectCommand = cmdSummary;
            daSummary.Fill(cdslSummary);


            //SqlCommand cmdHolding = new SqlCommand("cdslBill_holdingDisplay", con);

        }


        int status = 0;
        if (dp == "CDSL")
        {
            for (int k = 0; k < cdslClients.Tables[0].Rows.Count; k++)
            {
                if (cdslClients.Tables[0].Rows[k]["CdslTransaction_BenAccountNumber"] != DBNull.Value)
                {
                    if (status > 0)
                    {

                        if (cdslClients.Tables[0].Rows[k - 1]["CdslTransaction_BenAccountNumber"].ToString() == cdslClients.Tables[0].Rows[k]["CdslTransaction_BenAccountNumber"].ToString()
                            && cdslClients.Tables[0].Rows[k - 1]["CDSLISIN_Number"].ToString() == cdslClients.Tables[0].Rows[k]["CDSLISIN_Number"].ToString()
                            && cdslClients.Tables[0].Rows[k - 1]["CdslTransaction_SettlementID"].ToString() == cdslClients.Tables[0].Rows[k]["CdslTransaction_SettlementID"].ToString()
                              && cdslClients.Tables[0].Rows[k - 1]["Transaction_Type"].ToString() == cdslClients.Tables[0].Rows[k]["Transaction_Type"].ToString())
                        {

                            cdslClients.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(cdslClients.Tables[0].Rows[k - 1]["CdslTransaction_Quantity"].ToString()) + Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["debit"].ToString()));


                        }
                        else
                        {
                            cdslClients.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["debit"].ToString()));
                        }
                    }
                    else
                    {
                        status++;
                        cdslClients.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["debit"].ToString()));

                    }
                }

            }
        }

        else if (dp == "NSDL")
        {
            for (int k = 0; k < cdslClients.Tables[0].Rows.Count; k++)
            {

                if (cdslClients.Tables[0].Rows[k]["NsdlTransaction_Quantity"] != DBNull.Value)
                {
                    if (status > 0)
                    {

                        if (cdslClients.Tables[0].Rows[k - 1]["DPBillSummary_BenAccountNumber"].ToString() == cdslClients.Tables[0].Rows[k]["DPBillSummary_BenAccountNumber"].ToString()
                            && cdslClients.Tables[0].Rows[k - 1]["NSDLISIN_Number"].ToString() == cdslClients.Tables[0].Rows[k]["NSDLISIN_Number"].ToString()
                            && cdslClients.Tables[0].Rows[k - 1]["NsdlTransaction_SettlementNumber"].ToString() == cdslClients.Tables[0].Rows[k]["NsdlTransaction_SettlementNumber"].ToString()
                              && cdslClients.Tables[0].Rows[k - 1]["Transaction_Type"].ToString() == cdslClients.Tables[0].Rows[k]["Transaction_Type"].ToString())
                        {

                            cdslClients.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(cdslClients.Tables[0].Rows[k - 1]["NsdlTransaction_Quantity"].ToString()) + Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["debit"].ToString()));


                        }
                        else
                        {
                            cdslClients.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(Convert.ToString(cdslClients.Tables[0].Rows[k]["openingbalance"])) + Convert.ToDecimal(Convert.ToString(cdslClients.Tables[0].Rows[k]["credit"])) - Convert.ToDecimal(Convert.ToString(cdslClients.Tables[0].Rows[k]["debit"])));
                        }
                    }
                    else
                    {
                        status++;
                        cdslClients.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["debit"].ToString()));

                    }
                }

            }
        }

        status = 0; //cdslClients.Tables[0].Rows.Count





        if (dp == "CDSL")
        {
            for (int k = 0; k < cdslClients.Tables[0].Rows.Count; k++)
            {


                if (cdslClients.Tables[0].Rows[k]["CdslTransaction_BenAccountNumber"] != DBNull.Value)
                {
                    cdslClients.Tables[0].Rows[k]["CdslTransaction_Quantity"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["CdslTransaction_Quantity"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["credit"].ToString().Trim() == "0.000")
                        cdslClients.Tables[0].Rows[k]["credit"] = DBNull.Value;
                    else
                        cdslClients.Tables[0].Rows[k]["credit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["credit"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["debit"].ToString().Trim() == "0.000")
                        cdslClients.Tables[0].Rows[k]["debit"] = DBNull.Value;
                    else
                        cdslClients.Tables[0].Rows[k]["debit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["debit"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["Rate"] != DBNull.Value)
                        cdslClients.Tables[0].Rows[k]["Rate"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["Rate"].ToString()));


                    cdslClients.Tables[0].Rows[k]["openingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["openingbalance"].ToString()));
                    cdslClients.Tables[0].Rows[k]["closingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["closingbalance"].ToString()));


                    if (cdslClients.Tables[0].Rows[k]["Amount"] != DBNull.Value)
                        cdslClients.Tables[0].Rows[k]["Amount"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["Amount"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["DPOtherCharge_HoldingNarration"] != DBNull.Value)
                        cdslClients.Tables[0].Rows[k]["DPOtherCharge_HoldingNarration"] = "\n" + cdslClients.Tables[0].Rows[k]["DPOtherCharge_HoldingNarration"];

                    if (cdslClients.Tables[0].Rows[k]["holdingCharge"] != DBNull.Value && cdslClients.Tables[0].Rows[k]["Amount"] != DBNull.Value)
                        cdslClients.Tables[0].Rows[k]["holdingCharge"] = "\n" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["holdingCharge"].ToString()));
                    else
                        if (cdslClients.Tables[0].Rows[k]["holdingCharge"] != DBNull.Value && cdslClients.Tables[0].Rows[k]["Amount"] == DBNull.Value)
                            cdslClients.Tables[0].Rows[k]["holdingCharge"] = "  \n" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["holdingCharge"].ToString()));

                }

            }
        }

        else if (dp == "NSDL")
        {
            for (int k = 0; k < cdslClients.Tables[0].Rows.Count; k++)
            {


                if (cdslClients.Tables[0].Rows[k]["NsdlTransaction_Quantity"] != DBNull.Value)
                {
                    cdslClients.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["NsdlTransaction_Quantity"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["credit"].ToString().Trim() == "0.000")
                        cdslClients.Tables[0].Rows[k]["credit"] = DBNull.Value;
                    else
                        cdslClients.Tables[0].Rows[k]["credit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["credit"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["debit"].ToString().Trim() == "0.000")
                        cdslClients.Tables[0].Rows[k]["debit"] = DBNull.Value;
                    else
                        cdslClients.Tables[0].Rows[k]["debit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["debit"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["Rate"] != DBNull.Value)
                        cdslClients.Tables[0].Rows[k]["Rate"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["Rate"].ToString()));


                    cdslClients.Tables[0].Rows[k]["openingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["openingbalance"].ToString()));
                    cdslClients.Tables[0].Rows[k]["closingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["closingbalance"].ToString()));

                    if (cdslClients.Tables[0].Rows[k]["Amount"] != DBNull.Value)
                        cdslClients.Tables[0].Rows[k]["Amount"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslClients.Tables[0].Rows[k]["Amount"].ToString()));


                }

            }
        }



        for (int k = 0; k < cdslSummary.Tables[0].Rows.Count; k++)
        {
            cdslSummary.Tables[0].Rows[k]["ChargeAmt"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslSummary.Tables[0].Rows[k]["ChargeAmt"].ToString()));
        }

        if (cdslClients.Tables[0].Rows.Count > 0)
        {
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {

                SqlBulkCopy sbc = new SqlBulkCopy(con);

                if (dp == "CDSL")
                    sbc.DestinationTableName = "Report_CdslBill";
                else if (dp == "NSDL")
                {
                    sbc.DestinationTableName = "Report_NsdlBill";

                }



                if (con.State.Equals(ConnectionState.Open))
                {
                    con.Close();
                }
                con.Open();
                sbc.WriteToServer(cdslClients.Tables[0]);


                //if (dp == "CDSL")       
                sbc.DestinationTableName = "Report_CdslBillSummary";
                //else if (dp == "NSDL")   
                //sbc.DestinationTableName = "Report_NsdlBillSummary";

                sbc.WriteToServer(cdslSummary.Tables[0]);


                con.Close();

            }
        }
        holdingCalculation(BillingMonth, CompanyId, dpId, SegmentId, dp, EndDate, billnoFinYear);

    }

    public void holdingCalculation(string BillingMonth, string CompanyId, string dpId, string SegmentId, string dp, string EndDate, string billnoFinYear)
    {
        DataSet cdslHolding = new DataSet();

        //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {
            SqlCommand cmdHolding = new SqlCommand();
            cmdHolding.Connection = con;
            if (dp == "CDSL")
                cmdHolding.CommandText = "cdslBill_holdingDisplay1";
            if (dp == "NSDL")
                cmdHolding.CommandText = "sp_NsdlBill_Holding";



            cmdHolding.CommandType = CommandType.StoredProcedure;
            cmdHolding.Parameters.AddWithValue("@DpId", dpId);
            cmdHolding.Parameters.AddWithValue("@billNumber", dp + "-" + BillingMonth.ToUpper() + billnoFinYear);


            if (dp == "NSDL")
            {
                cmdHolding.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmdHolding.Parameters.AddWithValue("@lastDay", EndDate);
            }


            cmdHolding.CommandTimeout = 0;
            SqlDataAdapter daHolding = new SqlDataAdapter();
            daHolding.SelectCommand = cmdHolding;
            daHolding.Fill(cdslHolding);

        }


        if (cdslHolding.Tables[0].Rows.Count == 0)
        {
            DataRow dataRow = cdslHolding.Tables[0].NewRow();
            dataRow[0] = "00000";
            dataRow[2] = "00000";
            dataRow[3] = "00000";
            dataRow[4] = "00000";
            dataRow[5] = "00000";
            dataRow[6] = "00000";
            dataRow[7] = "00000";
            dataRow[8] = "00000";
            dataRow[9] = "00000";
            cdslHolding.Tables[0].Rows.Add(dataRow);

        }
        else
        {
            if (dp == "CDSL")
            {
                for (int k = 0; k < cdslHolding.Tables[0].Rows.Count; k++)
                {
                    if (cdslHolding.Tables[0].Rows[k]["CdslHolding_CurrentBalance"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["CdslHolding_CurrentBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["CdslHolding_CurrentBalance"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["CdslHolding_FreeBalance"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["CdslHolding_FreeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["CdslHolding_FreeBalance"].ToString()));


                    if (cdslHolding.Tables[0].Rows[k]["CdslHolding_PledgeBalance"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["CdslHolding_PledgeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["CdslHolding_PledgeBalance"].ToString()));


                    if (cdslHolding.Tables[0].Rows[k]["CdslHolding_EarmarkedBalance"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["CdslHolding_EarmarkedBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["CdslHolding_EarmarkedBalance"].ToString()));


                    if (cdslHolding.Tables[0].Rows[k]["CdslHolding_PendingRematBalance"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["CdslHolding_PendingRematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["CdslHolding_PendingRematBalance"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["CdslHolding_PendingDematBalance"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["CdslHolding_PendingDematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["CdslHolding_PendingDematBalance"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["ISINVAlue"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["ISINVAlue"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["ISINVAlue"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["Rate"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["Rate"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["Rate"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["SumValue"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["SumValue"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["SumValue"].ToString()));

                }
            }

            else if (dp == "NSDL")
            {
                for (int k = 0; k < cdslHolding.Tables[0].Rows.Count; k++)
                {

                    if (cdslHolding.Tables[0].Rows[k]["Free"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["Free"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["Free"].ToString()));


                    if (cdslHolding.Tables[0].Rows[k]["Demat"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["Demat"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["Demat"].ToString()));


                    if (cdslHolding.Tables[0].Rows[k]["Remat"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["Remat"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["Remat"].ToString()));


                    if (cdslHolding.Tables[0].Rows[k]["Pledged"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["Pledged"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["Pledged"].ToString()));

                    //if (cdslHolding.Tables[0].Rows[k]["Total"] != DBNull.Value)
                    //    cdslHolding.Tables[0].Rows[k]["Total"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["Total"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["Rate"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["Rate"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["Rate"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["ISINVAlue"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["ISINVAlue"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["ISINVAlue"].ToString()));

                    if (cdslHolding.Tables[0].Rows[k]["TotalValue"] != DBNull.Value)
                        cdslHolding.Tables[0].Rows[k]["TotalValue"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(cdslHolding.Tables[0].Rows[k]["TotalValue"].ToString()));

                }
            }
        }

        //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {

            SqlBulkCopy sbc = new SqlBulkCopy(con);
            if (dp == "CDSL")
                sbc.DestinationTableName = "Report_CdslBillHolding";
            else if (dp == "NSDL")
                sbc.DestinationTableName = "Report_NsdlBillHolding";

            if (con.State.Equals(ConnectionState.Open))
            {
                con.Close();
            }
            con.Open();


            sbc.WriteToServer(cdslHolding.Tables[0]);

            con.Close();
        }


    }


    #region Generate Crystal Report

    public int callCrystalReport(string month, string benid, string groupid
                                    , string GenerationOrder, string qstr1,
                                        string billAmount, string reportType,
                                            int scanStatus, string employeeId, string ScanemployeeName,
                                                string digitalSignatureid, int EmailCreateAppMenuId,
                                                    int UseHeader, int UseFooter)
    {


        String signaturePath, SegmentId, CompanyId, billnoFinYear, dpId, StartDate, EndDate;

        int logoStatus, signatureStatus;

        DataSet logo = new DataSet();
        DataSet Clients = new DataSet();
        DataSet Holding = new DataSet();
        DataSet Summary = new DataSet();
        DataSet AccountSummary = new DataSet();
        DataSet signature = new DataSet();

        DataRow drow, drow1;
        byte[] logoinByte;
        byte[] SignatureinByte;

        bind_CompanyID_SegmentID(out CompanyId, out dpId, out SegmentId);
        objConverter.getFirstAndLastDate(month, out StartDate, out EndDate, out billnoFinYear);


        logoStatus = 1;
        signatureStatus = 1;


        signature.Tables.Add();
        signature.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
        signature.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
        signature.Tables[0].Columns.Add("signName", System.Type.GetType("System.String"));
        signature.Tables[0].Columns.Add("Status", System.Type.GetType("System.String"));


        drow = signature.Tables[0].NewRow();

        if (scanStatus == 1)
        {
            if (objConverter.getSignatureImage(employeeId, out SignatureinByte, qstr1) == 1)
            {
                //byte[] S;
                //String tmpPath1 =HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\dig-signature.pfx");
                //System.Security.Cryptography.X509Certificates.X509Certificate2 cert2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(tmpPath1, "123456");
                //S=cert2.RawData;
                //drow["Image"] = S;

                drow["Image"] = SignatureinByte;
                drow["Status"] = "1";
                //drow["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
                //drow["signName"] = "txtEmpName.Text.Split('[').GetValue(0)";



            }
            else
            {
                signatureStatus = 0;
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "sign", "alert('Signature not Found.');", true);
                return 10;
            }
        }
        //else
        //    if (scanStatus == 2)
        //    {

        //    }


        //signature.Tables[0].Rows.Add(drow);

        logo.Tables.Add();
        logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
        drow1 = logo.Tables[0].NewRow();


        if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) == 1)
        {
            drow1["Image"] = logoinByte;

        }
        else
        {
            logoStatus = 0;
            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

        }

        logo.Tables[0].Rows.Add(drow1);


        if (logoStatus == 1 && signatureStatus == 1)
        {

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmdClients = new SqlCommand();
                cmdClients.Connection = con;

                if (qstr1 == "CDSL")
                    cmdClients.CommandText = "cdslBill_Report1";

                else if (qstr1 == "NSDL")
                    cmdClients.CommandText = "sp_NsdlBill_Report";


                cmdClients.CommandType = CommandType.StoredProcedure;

                cmdClients.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

                if (benid == "")
                {
                    cmdClients.Parameters.AddWithValue("@BenAccount", "NA");
                }
                else
                {
                    cmdClients.Parameters.AddWithValue("@BenAccount", benid);
                }

                if (groupid == "")
                {
                    cmdClients.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdClients.Parameters.AddWithValue("@group", groupid);

                }


                cmdClients.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                cmdClients.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
                cmdClients.Parameters.AddWithValue("@billamt", billAmount);
                cmdClients.Parameters.AddWithValue("@generationOrder", GenerationOrder);
                cmdClients.Parameters.AddWithValue("@HeaderID", UseHeader);
                cmdClients.Parameters.AddWithValue("@FooterID", UseFooter);


                cmdClients.CommandTimeout = 0;
                SqlDataAdapter daClients = new SqlDataAdapter();
                daClients.SelectCommand = cmdClients;
                daClients.Fill(Clients);

                //Clients.WriteXmlSchema(@"D:\CdslClients.xsd");

                SqlCommand cmdHolding = new SqlCommand("cdslBill_ReportHolding", con);
                cmdHolding.CommandType = CommandType.StoredProcedure;

                cmdHolding.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

                if (benid == "")
                {
                    cmdHolding.Parameters.AddWithValue("@BenAccount", "NA");
                }
                else
                {
                    cmdHolding.Parameters.AddWithValue("@BenAccount", benid);
                }

                if (groupid == "")
                {
                    cmdHolding.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdHolding.Parameters.AddWithValue("@group", groupid);

                }

                cmdHolding.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                cmdHolding.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
                cmdHolding.Parameters.AddWithValue("@dp", qstr1);
                cmdHolding.Parameters.AddWithValue("@billamt", billAmount);
                cmdHolding.Parameters.AddWithValue("@generationOrder", GenerationOrder);
                cmdHolding.Parameters.AddWithValue("@dpId", dpId);

                cmdHolding.CommandTimeout = 0;
                SqlDataAdapter daHolding = new SqlDataAdapter();
                daHolding.SelectCommand = cmdHolding;
                daHolding.Fill(Holding);



                SqlCommand cmdSummary = new SqlCommand("cdslBill_ReportSummary", con);
                cmdSummary.CommandType = CommandType.StoredProcedure;

                cmdSummary.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

                if (benid == "")
                {
                    cmdSummary.Parameters.AddWithValue("@BenAccount", "NA");
                }
                else
                {
                    cmdSummary.Parameters.AddWithValue("@BenAccount", benid);

                }

                if (groupid == "")
                {
                    cmdSummary.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdSummary.Parameters.AddWithValue("@group", groupid);

                }




                cmdSummary.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                cmdSummary.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);

                cmdSummary.Parameters.AddWithValue("@dp", qstr1);
                cmdSummary.Parameters.AddWithValue("@billamt", billAmount);
                cmdSummary.Parameters.AddWithValue("@generationOrder", GenerationOrder);
                cmdSummary.Parameters.AddWithValue("@dpId", dpId);



                cmdSummary.CommandTimeout = 0;
                SqlDataAdapter daSummary = new SqlDataAdapter();
                daSummary.SelectCommand = cmdSummary;
                daSummary.Fill(Summary);

                SqlCommand cmdAccount = new SqlCommand("cdslBill_ReportAccountsLedger1", con);
                cmdAccount.CommandType = CommandType.StoredProcedure;

                cmdAccount.Parameters.AddWithValue("@startDate", StartDate);
                cmdAccount.Parameters.AddWithValue("@endDate", EndDate);
                cmdAccount.Parameters.AddWithValue("@dpId", dpId);
                cmdAccount.Parameters.AddWithValue("@companyID", CompanyId);
                cmdAccount.Parameters.AddWithValue("@SegmentId", SegmentId);

                if (qstr1 == "CDSL")
                    cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00042");
                else if (qstr1 == "NSDL")
                    cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00043");

                if (benid == "")
                {
                    cmdAccount.Parameters.AddWithValue("@SubAccountID", "NA");
                }
                else
                {

                    cmdAccount.Parameters.AddWithValue("@SubAccountID", benid);
                }

                if (groupid == "")
                {
                    cmdAccount.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdAccount.Parameters.AddWithValue("@group", groupid);

                }


                cmdAccount.Parameters.AddWithValue("@financialYear", HttpContext.Current.Session["LastFinYear"]);
                cmdAccount.Parameters.AddWithValue("@dp", qstr1);
                cmdAccount.Parameters.AddWithValue("@billamt", billAmount);
                cmdAccount.Parameters.AddWithValue("@generationOrder", GenerationOrder);




                cmdAccount.CommandTimeout = 0;
                SqlDataAdapter daAccount = new SqlDataAdapter();
                daAccount.SelectCommand = cmdAccount;
                daAccount.Fill(AccountSummary);



            }


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

            //if (qstr1 == "NSDL")
            //{
            //Clients.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslClients.xsd");
            //Clients.WriteXmlSchema(@"D:\NsdlClients.xsd");
            //Holding.WriteXmlSchema(@"D:\NsdlBillHolding.xsd");
            //Summary.WriteXmlSchema(@"D:\NsdlBillSummary.xsd");

            //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
            //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
            //}

            if (Clients.Tables[0].Rows.Count > 0)
            {

                if (reportType == "Print")
                {

                    if (scanStatus == 1)
                    {
                        //Add Company name and Employee name in the signature//

                        drow["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
                        drow["signName"] = ScanemployeeName.Split('[').GetValue(0);
                        //signature.Tables[0].Rows.Add(drow);

                    }
                    else
                    {
                        drow["Status"] = "2";
                    }
                    signature.Tables[0].Rows.Add(drow);
                    //  signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xml");
                    //  signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xsd");

                    generatePdf(Clients.Tables[0], Summary.Tables[0], AccountSummary.Tables[0], Holding.Tables[0],
                                  signature.Tables[0], logo.Tables[0], qstr1, "No", digitalSignatureid, qstr1);

                }
                else
                    if (reportType == "Digital")
                    {

                        drow["Status"] = "3";
                        signature.Tables[0].Rows.Add(drow);

                        DataSet DigitalSignatureDs = new DataSet();

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
                        DigitalSignatureDs.Tables[0].Rows[0]["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);

                        string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

                        tmpPdfPath = string.Empty;
                        ReportPath = string.Empty;
                        signPath = string.Empty;
                        finalResult = string.Empty;

                        digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();

                        tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                        if (qstr1 == "CDSL")
                        {
                            ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\cdslBill.rpt");

                        }
                        else
                        {
                            ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NsdlBill.rpt");
                        }

                        signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + digitalSignatureid + ".pfx";
                        signPdfPath = objConverter.DirectoryPath(out VirtualPath);

                        //BackProcess bp = new BackProcess();
                        //bp.Clients = Clients;
                        //bp.Summary = Summary;
                        //bp.AccountSummary = AccountSummary;
                        //bp.Holding = Holding;
                        //bp.signature = signature;
                        //bp.logo = logo;
                        //bp.module = qstr1;
                        //bp.DigitalCertified = "Yes";
                        //bp.digitalSignaturePassword = digitalSignaturePassword;
                        //bp.DigitalSignatureDs = DigitalSignatureDs;
                        //bp.SignPath = signPath;
                        //bp.reportPath = ReportPath;
                        //bp.tmpPDFPath = tmpPdfPath;
                        //bp.CompanyId = CompanyId;
                        //bp.dpId = dpId;
                        //bp.SegmentId = SegmentId;
                        //bp.signPdfPath = signPdfPath;
                        //bp.VirtualPath = VirtualPath;
                        //bp.LastFinYear = HttpContext.Current.Session["LastFinYear"].ToString();
                        //bp.user = HttpContext.Current.Session["userid"].ToString();
                        //bp.backGroundWork();
                        finalResult = generateindivisualPdf(Clients, Summary, AccountSummary,
                                   Holding, signature, logo,
                                   qstr1, "Yes", digitalSignaturePassword,
                                   DigitalSignatureDs, signPath, ReportPath
                                  , tmpPdfPath, CompanyId, dpId, SegmentId, signPdfPath, VirtualPath,
                                  HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), EmailCreateAppMenuId);
                        if (finalResult == "Success")
                        {
                            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Successfully Document Generated.');", true);

                        }

                    }
            }
            else
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('No Record Found');", true);

            }

        }
        return 0;
    }


    public int callCrystalReport(string month, string benid, string groupid
                                    , string GenerationOrder, string qstr1,
                                        string billAmount, string reportType,
                                            int scanStatus, string employeeId, string ScanemployeeName,
                                                string digitalSignatureid, int EmailCreateAppMenuId,
                                                    int UseHeader, int UseFooter, string isSinglepage,string isinvalueclass)
    {


        String signaturePath, SegmentId, CompanyId, billnoFinYear, dpId, StartDate, EndDate;

        int logoStatus, signatureStatus;

        DataSet logo = new DataSet();
        DataSet Clients = new DataSet();
        DataSet Holding = new DataSet();
        DataSet Summary = new DataSet();
        DataSet AccountSummary = new DataSet();
        DataSet signature = new DataSet();

        DataRow drow, drow1;
        byte[] logoinByte;
        byte[] SignatureinByte;

        bind_CompanyID_SegmentID(out CompanyId, out dpId, out SegmentId);
        objConverter.getFirstAndLastDate(month, out StartDate, out EndDate, out billnoFinYear);


        logoStatus = 1;
        signatureStatus = 1;


        signature.Tables.Add();
        signature.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
        signature.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
        signature.Tables[0].Columns.Add("signName", System.Type.GetType("System.String"));
        signature.Tables[0].Columns.Add("Status", System.Type.GetType("System.String"));


        drow = signature.Tables[0].NewRow();

        if (scanStatus == 1)
        {
            if (objConverter.getSignatureImage(employeeId, out SignatureinByte, qstr1) == 1)
            {
                //byte[] S;
                //String tmpPath1 =HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\dig-signature.pfx");
                //System.Security.Cryptography.X509Certificates.X509Certificate2 cert2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(tmpPath1, "123456");
                //S=cert2.RawData;
                //drow["Image"] = S;

                drow["Image"] = SignatureinByte;
                drow["Status"] = "1";
                //drow["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
                //drow["signName"] = "txtEmpName.Text.Split('[').GetValue(0)";



            }
            else
            {
                signatureStatus = 0;
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "sign", "alert('Signature not Found.');", true);
                return 10;
            }
        }
        //else
        //    if (scanStatus == 2)
        //    {

        //    }


        //signature.Tables[0].Rows.Add(drow);

        logo.Tables.Add();
        logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
        drow1 = logo.Tables[0].NewRow();


        if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) == 1)
        {
            drow1["Image"] = logoinByte;

        }
        else
        {
            logoStatus = 0;
            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

        }

        logo.Tables[0].Rows.Add(drow1);


        if (logoStatus == 1 && signatureStatus == 1)
        {

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmdClients = new SqlCommand();
                cmdClients.Connection = con;

                if (qstr1 == "CDSL")
                    cmdClients.CommandText = "cdslBill_Report1";

                else if (qstr1 == "NSDL")
                    cmdClients.CommandText = "sp_NsdlBill_Report";


                cmdClients.CommandType = CommandType.StoredProcedure;

                cmdClients.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

                if (benid == "")
                {
                    cmdClients.Parameters.AddWithValue("@BenAccount", "NA");
                }
                else
                {
                    cmdClients.Parameters.AddWithValue("@BenAccount", benid);
                }

                if (groupid == "")
                {
                    cmdClients.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdClients.Parameters.AddWithValue("@group", groupid);

                }

                cmdClients.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                cmdClients.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
                cmdClients.Parameters.AddWithValue("@billamt", billAmount);
                cmdClients.Parameters.AddWithValue("@generationOrder", GenerationOrder);
                cmdClients.Parameters.AddWithValue("@HeaderID", UseHeader);
                cmdClients.Parameters.AddWithValue("@FooterID", UseFooter);

                cmdClients.Parameters.AddWithValue("@isSinglepage", isSinglepage);


                cmdClients.CommandTimeout = 0;
                SqlDataAdapter daClients = new SqlDataAdapter();
                daClients.SelectCommand = cmdClients;
                daClients.Fill(Clients);

                //  Clients.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslClients.xsd");

                SqlCommand cmdHolding = new SqlCommand("cdslBill_ReportHolding", con);
                cmdHolding.CommandType = CommandType.StoredProcedure;

                cmdHolding.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

                if (benid == "")
                {
                    cmdHolding.Parameters.AddWithValue("@BenAccount", "NA");
                }
                else
                {
                    cmdHolding.Parameters.AddWithValue("@BenAccount", benid);
                }

                if (groupid == "")
                {
                    cmdHolding.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdHolding.Parameters.AddWithValue("@group", groupid);

                }

                cmdHolding.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                cmdHolding.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
                cmdHolding.Parameters.AddWithValue("@dp", qstr1);
                cmdHolding.Parameters.AddWithValue("@billamt", billAmount);
                cmdHolding.Parameters.AddWithValue("@generationOrder", GenerationOrder);
                cmdHolding.Parameters.AddWithValue("@dpId", dpId);

                cmdHolding.CommandTimeout = 0;
                SqlDataAdapter daHolding = new SqlDataAdapter();
                daHolding.SelectCommand = cmdHolding;
                daHolding.Fill(Holding);
                if (isinvalueclass == "Y")
                {
                    string mainclients = "";
                    string mainisin = "";
                    decimal value = 0;
                    for (int m = 0; m < Clients.Tables[0].Rows.Count; m++)
                    {
                        mainclients = Clients.Tables[0].Rows[m]["NsdlClients_BenAccountID"].ToString();
                        mainisin = Clients.Tables[0].Rows[m]["NSDLISIN_Number"].ToString();
                        for (int p = 0; p < Holding.Tables[0].Rows.Count; p++)
                        {
                            if (mainclients == Holding.Tables[0].Rows[p]["NsdlHolding_BenAccountNumber"].ToString())
                            {
                                if (mainisin == Holding.Tables[0].Rows[p]["NsdlHolding_ISIN"].ToString())
                                {
                                    Holding.Tables[0].Rows[p].Delete();
                                    Holding.AcceptChanges();
                                }

                            }


                        }
                    }
                    DataTable DistinctBillNumber;
                    DataView viewClients = new DataView(Holding.Tables[0]);
                    DistinctBillNumber = viewClients.ToTable(true, new string[] { "NsdlHolding_BenAccountNumber" });
                    for (int pu = 0; pu < DistinctBillNumber.Rows.Count; pu++)
                    {
                        mainclients = DistinctBillNumber.Rows[pu]["NsdlHolding_BenAccountNumber"].ToString();
                        for (int puc = 0; puc < Holding.Tables[0].Rows.Count; puc++)
                        {
                            if (mainclients == Holding.Tables[0].Rows[puc]["NsdlHolding_BenAccountNumber"].ToString())
                            {
                                
                                Holding.Tables[0].Rows[puc]["TotalValue"] = Convert.ToString(value + Convert.ToDecimal(Holding.Tables[0].Rows[puc]["ISINValue"].ToString()));
                                value += Convert.ToDecimal(Holding.Tables[0].Rows[puc]["ISINValue"].ToString());
                                Holding.Tables[0].Rows[puc]["HoldingDate"] = Holding.Tables[0].Rows[puc]["HoldingDate"] + " No transaction recorded for the following ISIN";
                                Holding.AcceptChanges();
                                
                            }
                            else
                                value = 0;
                        }
                    }
                }
                SqlCommand cmdSummary = new SqlCommand("cdslBill_ReportSummary", con);
                cmdSummary.CommandType = CommandType.StoredProcedure;

                cmdSummary.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

                if (benid == "")
                {
                    cmdSummary.Parameters.AddWithValue("@BenAccount", "NA");
                }
                else
                {
                    cmdSummary.Parameters.AddWithValue("@BenAccount", benid);

                }

                if (groupid == "")
                {
                    cmdSummary.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdSummary.Parameters.AddWithValue("@group", groupid);

                }


                cmdSummary.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
                cmdSummary.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);

                cmdSummary.Parameters.AddWithValue("@dp", qstr1);
                cmdSummary.Parameters.AddWithValue("@billamt", billAmount);
                cmdSummary.Parameters.AddWithValue("@generationOrder", GenerationOrder);
                cmdSummary.Parameters.AddWithValue("@dpId", dpId);



                cmdSummary.CommandTimeout = 0;
                SqlDataAdapter daSummary = new SqlDataAdapter();
                daSummary.SelectCommand = cmdSummary;
                daSummary.Fill(Summary);

                SqlCommand cmdAccount = new SqlCommand("cdslBill_ReportAccountsLedger1", con);
                cmdAccount.CommandType = CommandType.StoredProcedure;

                cmdAccount.Parameters.AddWithValue("@startDate", StartDate);
                cmdAccount.Parameters.AddWithValue("@endDate", EndDate);
                cmdAccount.Parameters.AddWithValue("@dpId", dpId);
                cmdAccount.Parameters.AddWithValue("@companyID", CompanyId);
                cmdAccount.Parameters.AddWithValue("@SegmentId", SegmentId);

                if (qstr1 == "CDSL")
                    cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00042");
                else if (qstr1 == "NSDL")
                    cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00043");

                if (benid == "")
                {
                    cmdAccount.Parameters.AddWithValue("@SubAccountID", "NA");
                }
                else
                {

                    cmdAccount.Parameters.AddWithValue("@SubAccountID", benid);
                }

                if (groupid == "")
                {
                    cmdAccount.Parameters.AddWithValue("@group", "NA");
                }
                else
                {
                    cmdAccount.Parameters.AddWithValue("@group", groupid);

                }


                cmdAccount.Parameters.AddWithValue("@financialYear", HttpContext.Current.Session["LastFinYear"]);
                cmdAccount.Parameters.AddWithValue("@dp", qstr1);
                cmdAccount.Parameters.AddWithValue("@billamt", billAmount);
                cmdAccount.Parameters.AddWithValue("@generationOrder", GenerationOrder);




                cmdAccount.CommandTimeout = 0;
                SqlDataAdapter daAccount = new SqlDataAdapter();
                daAccount.SelectCommand = cmdAccount;
                daAccount.Fill(AccountSummary);


            }


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

            //if (qstr1 == "NSDL")
            //{
            //Clients.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslClients.xsd");
            //Clients.WriteXmlSchema(@"D:\NsdlClients.xsd");
            //Holding.WriteXmlSchema(@"D:\NsdlBillHolding.xsd");
            //Summary.WriteXmlSchema(@"D:\NsdlBillSummary.xsd");

            //Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
            //Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
            //}

            if (Clients.Tables[0].Rows.Count > 0)
            {

                if (reportType == "Print")
                {

                    if (scanStatus == 1)
                    {
                        //Add Company name and Employee name in the signature//

                        drow["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
                        drow["signName"] = ScanemployeeName.Split('[').GetValue(0);
                        //signature.Tables[0].Rows.Add(drow);

                    }
                    else
                    {
                        drow["Status"] = "2";
                    }
                    signature.Tables[0].Rows.Add(drow);
                    //  signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xml");
                    //  signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xsd");

                    generatePdf(Clients.Tables[0], Summary.Tables[0], AccountSummary.Tables[0], Holding.Tables[0],
                                  signature.Tables[0], logo.Tables[0], qstr1, "No", digitalSignatureid, qstr1);

                }
                else if (reportType == "Digital")
                    {

                        drow["Status"] = "3";
                        signature.Tables[0].Rows.Add(drow);
                        string TokenType = digitalSignatureid.Split('*')[0];
                        int GeneratedPDFCount = 0;

                        if (TokenType == "E")
                        {
                            digitalSignatureid = digitalSignatureid.Split('*')[1].Split('~')[0].ToString().Trim();
                        }

                        DataSet DigitalSignatureDs = new DataSet();

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
                        DigitalSignatureDs.Tables[0].Rows[0]["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);

                        string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

                        tmpPdfPath = string.Empty;
                        ReportPath = string.Empty;
                        signPath = string.Empty;
                        finalResult = string.Empty;

                        digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();

                        if (TokenType == "E")
                        {
                            tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\EToken\");
                            signPath = digitalSignatureid;
                        }
                        else
                        {
                            tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                            signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") + digitalSignatureid + ".pfx";
                        }
                        if (qstr1 == "CDSL")
                        {
                            ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\cdslBill.rpt");

                        }
                        else
                        {
                            ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NsdlBill.rpt");
                        }

                        
                        signPdfPath = objConverter.DirectoryPath(out VirtualPath);

                        //BackProcess bp = new BackProcess();
                        //bp.Clients = Clients;
                        //bp.Summary = Summary;
                        //bp.AccountSummary = AccountSummary;
                        //bp.Holding = Holding;
                        //bp.signature = signature;
                        //bp.logo = logo;
                        //bp.module = qstr1;
                        //bp.DigitalCertified = "Yes";
                        //bp.digitalSignaturePassword = digitalSignaturePassword;
                        //bp.DigitalSignatureDs = DigitalSignatureDs;
                        //bp.SignPath = signPath;
                        //bp.reportPath = ReportPath;
                        //bp.tmpPDFPath = tmpPdfPath;
                        //bp.CompanyId = CompanyId;
                        //bp.dpId = dpId;
                        //bp.SegmentId = SegmentId;
                        //bp.signPdfPath = signPdfPath;
                        //bp.VirtualPath = VirtualPath;
                        //bp.LastFinYear = HttpContext.Current.Session["LastFinYear"].ToString();
                        //bp.user = HttpContext.Current.Session["userid"].ToString();
                        //bp.backGroundWork();
                        finalResult = generateindivisualPdf(Clients, Summary, AccountSummary,
                                   Holding, signature, logo,
                                   qstr1, "Yes", digitalSignaturePassword,
                                   DigitalSignatureDs, signPath, ReportPath
                                  , tmpPdfPath, CompanyId, dpId, SegmentId, signPdfPath, VirtualPath,
                                  HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), EmailCreateAppMenuId);
                        if (finalResult == "Success")
                        {
                            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Successfully Document Generated.');", true);

                        }

                    }
            }
            else
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('No Record Found');", true);

            }

        }
        return 0;
    }


    //public int callCrystalReport1(string month, string benid, string groupid
    //                                ,string GenerationOrder, string qstr1,
    //                                    string billAmount, string reportType, 
    //                                        int scanStatus, string employeeId, string ScanemployeeName,
    //                                            string digitalSignatureid, int EmailCreateAppMenuId)
    //{


    //    String signaturePath, SegmentId, CompanyId, billnoFinYear, dpId, StartDate, EndDate;

    //    int logoStatus, signatureStatus;

    //    DataSet logo = new DataSet();
    //    DataSet Clients = new DataSet();
    //    DataSet Holding = new DataSet();
    //    DataSet Summary = new DataSet();
    //    DataSet AccountSummary = new DataSet();
    //    DataSet signature = new DataSet();

    //    DataRow drow, drow1;
    //    byte[] logoinByte;
    //    byte[] SignatureinByte;

    //    bind_CompanyID_SegmentID(out CompanyId, out dpId, out SegmentId);
    //    objConverter.getFirstAndLastDate(month, out StartDate, out EndDate, out billnoFinYear);


    //    logoStatus = 1;
    //    signatureStatus = 1;


    //    signature.Tables.Add();
    //    signature.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
    //    signature.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
    //    signature.Tables[0].Columns.Add("signName", System.Type.GetType("System.String"));
    //    signature.Tables[0].Columns.Add("Status", System.Type.GetType("System.String"));


    //    drow = signature.Tables[0].NewRow();

    //    if (scanStatus == 1)
    //    {
    //        if (objConverter.getSignatureImage(employeeId, out SignatureinByte, qstr1) == 1)
    //        {
    //            //byte[] S;
    //            //String tmpPath1 =HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\dig-signature.pfx");
    //            //System.Security.Cryptography.X509Certificates.X509Certificate2 cert2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(tmpPath1, "123456");
    //            //S=cert2.RawData;
    //            //drow["Image"] = S;

    //            drow["Image"] = SignatureinByte;
    //            drow["Status"] = "1";
    //            //drow["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
    //            //drow["signName"] = "txtEmpName.Text.Split('[').GetValue(0)";



    //        }
    //        else
    //        {
    //            signatureStatus = 0;
    //            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "sign", "alert('Signature not Found.');", true);
    //            return 10;
    //        }
    //    }
    //    //else
    //    //    if (scanStatus == 2)
    //    //    {

    //    //    }


    //    //signature.Tables[0].Rows.Add(drow);

    //    logo.Tables.Add();
    //    logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
    //    drow1 = logo.Tables[0].NewRow();


    //    if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) == 1)
    //    {
    //        drow1["Image"] = logoinByte;

    //    }
    //    else
    //    {
    //        logoStatus = 0;
    //        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page , HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

    //    }

    //    logo.Tables[0].Rows.Add(drow1);


    //    if (logoStatus == 1 && signatureStatus == 1)
    //    {

    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
    //        {
    //            SqlCommand cmdClients = new SqlCommand();
    //            cmdClients.Connection = con;

    //            if (qstr1 == "CDSL")
    //                cmdClients.CommandText = "cdslBill_Report1";

    //            else if (qstr1 == "NSDL")
    //                cmdClients.CommandText = "sp_NsdlBill_Report";


    //            cmdClients.CommandType = CommandType.StoredProcedure;

    //            cmdClients.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

    //            if (benid == "")
    //            {
    //                cmdClients.Parameters.AddWithValue("@BenAccount", "NA");
    //            }
    //            else
    //            {
    //                cmdClients.Parameters.AddWithValue("@BenAccount", benid);
    //            }

    //            if (groupid == "")
    //            {
    //                cmdClients.Parameters.AddWithValue("@group", "NA");
    //            }
    //            else
    //            {
    //                cmdClients.Parameters.AddWithValue("@group", groupid);

    //            }


    //            cmdClients.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
    //            cmdClients.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
    //            cmdClients.Parameters.AddWithValue("@billamt", billAmount);
    //            cmdClients.Parameters.AddWithValue("@generationOrder", GenerationOrder);


    //            cmdClients.CommandTimeout = 0;
    //            SqlDataAdapter daClients = new SqlDataAdapter();
    //            daClients.SelectCommand = cmdClients;
    //            daClients.Fill(Clients);



    //            SqlCommand cmdHolding = new SqlCommand("cdslBill_ReportHolding", con);
    //            cmdHolding.CommandType = CommandType.StoredProcedure;

    //            cmdHolding.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

    //            if (benid == "")
    //            {
    //                cmdHolding.Parameters.AddWithValue("@BenAccount", "NA");
    //            }
    //            else
    //            {
    //                cmdHolding.Parameters.AddWithValue("@BenAccount", benid);
    //            }

    //            if (groupid == "")
    //            {
    //                cmdHolding.Parameters.AddWithValue("@group", "NA");
    //            }
    //            else
    //            {
    //                cmdHolding.Parameters.AddWithValue("@group", groupid);

    //            }

    //            cmdHolding.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
    //            cmdHolding.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);
    //            cmdHolding.Parameters.AddWithValue("@dp", qstr1);
    //            cmdHolding.Parameters.AddWithValue("@billamt", billAmount);
    //            cmdHolding.Parameters.AddWithValue("@generationOrder", GenerationOrder);
    //            cmdHolding.Parameters.AddWithValue("@dpId", dpId);

    //            cmdHolding.CommandTimeout = 0;
    //            SqlDataAdapter daHolding = new SqlDataAdapter();
    //            daHolding.SelectCommand = cmdHolding;
    //            daHolding.Fill(Holding);



    //            SqlCommand cmdSummary = new SqlCommand("cdslBill_ReportSummary", con);
    //            cmdSummary.CommandType = CommandType.StoredProcedure;

    //            cmdSummary.Parameters.AddWithValue("@billNumber", qstr1 + "-" + month + billnoFinYear);

    //            if (benid == "")
    //            {
    //                cmdSummary.Parameters.AddWithValue("@BenAccount", "NA");
    //            }
    //            else
    //            {
    //                cmdSummary.Parameters.AddWithValue("@BenAccount", benid);

    //            }

    //            if (groupid == "")
    //            {
    //                cmdSummary.Parameters.AddWithValue("@group", "NA");
    //            }
    //            else
    //            {
    //                cmdSummary.Parameters.AddWithValue("@group", groupid);

    //            }




    //            cmdSummary.Parameters.AddWithValue("@DPChargeMembers_SegmentID", SegmentId);
    //            cmdSummary.Parameters.AddWithValue("@DPChargeMembers_CompanyID", CompanyId);

    //            cmdSummary.Parameters.AddWithValue("@dp", qstr1);
    //            cmdSummary.Parameters.AddWithValue("@billamt", billAmount);
    //            cmdSummary.Parameters.AddWithValue("@generationOrder", GenerationOrder);
    //            cmdSummary.Parameters.AddWithValue("@dpId", dpId);



    //            cmdSummary.CommandTimeout = 0;
    //            SqlDataAdapter daSummary = new SqlDataAdapter();
    //            daSummary.SelectCommand = cmdSummary;
    //            daSummary.Fill(Summary);

    //            SqlCommand cmdAccount = new SqlCommand("cdslBill_ReportAccountsLedger1", con);
    //            cmdAccount.CommandType = CommandType.StoredProcedure;

    //            cmdAccount.Parameters.AddWithValue("@startDate", StartDate);
    //            cmdAccount.Parameters.AddWithValue("@endDate", EndDate);
    //            cmdAccount.Parameters.AddWithValue("@dpId", dpId);
    //            cmdAccount.Parameters.AddWithValue("@companyID", CompanyId);
    //            cmdAccount.Parameters.AddWithValue("@SegmentId", SegmentId);

    //            if (qstr1 == "CDSL")
    //                cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00042");
    //            else if (qstr1 == "NSDL")
    //                cmdAccount.Parameters.AddWithValue("@MainAcID", "SYSTM00043");

    //            if (benid == "")
    //            {
    //                cmdAccount.Parameters.AddWithValue("@SubAccountID", "NA");
    //            }
    //            else
    //            {

    //                cmdAccount.Parameters.AddWithValue("@SubAccountID", benid);
    //            }

    //            if (groupid == "")
    //            {
    //                cmdAccount.Parameters.AddWithValue("@group", "NA");
    //            }
    //            else
    //            {
    //                cmdAccount.Parameters.AddWithValue("@group", groupid);

    //            }


    //            cmdAccount.Parameters.AddWithValue("@financialYear", HttpContext.Current.Session["LastFinYear"]);
    //            cmdAccount.Parameters.AddWithValue("@dp", qstr1);
    //            cmdAccount.Parameters.AddWithValue("@billamt", billAmount);
    //            cmdAccount.Parameters.AddWithValue("@generationOrder", GenerationOrder);




    //            cmdAccount.CommandTimeout = 0;
    //            SqlDataAdapter daAccount = new SqlDataAdapter();
    //            daAccount.SelectCommand = cmdAccount;
    //            daAccount.Fill(AccountSummary);




    //        }


    //        AccountSummary.Tables[0].Columns.Add("StartDate", System.Type.GetType("System.String"));
    //        for (int i = 0; i < AccountSummary.Tables[0].Rows.Count; i++)
    //        {
    //            AccountSummary.Tables[0].Rows[i]["openingBalance"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["openingBalance"].ToString())));
    //            if (AccountSummary.Tables[0].Rows[i]["AccountsLedger_amountCr"] != DBNull.Value)
    //                AccountSummary.Tables[0].Rows[i]["AccountsLedger_amountCr"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["AccountsLedger_amountCr"].ToString())));

    //            if (AccountSummary.Tables[0].Rows[i]["Adjustment"] != DBNull.Value)
    //                AccountSummary.Tables[0].Rows[i]["Adjustment"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["Adjustment"].ToString())));

    //            AccountSummary.Tables[0].Rows[i]["netBillAmt"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["netBillAmt"].ToString())));
    //            AccountSummary.Tables[0].Rows[i]["closingBalance"] = objConverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(AccountSummary.Tables[0].Rows[i]["closingBalance"].ToString())));
    //            AccountSummary.Tables[0].Rows[i]["StartDate"] = StartDate;
    //        }

    //        //if (qstr1 == "NSDL")
    //        //{
    //        //    Clients.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlClients.xsd");
    //        //    Holding.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillHolding.xsd");
    //        //    Summary.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlBillSummary.xsd");
    //        //}

    //     if (Clients.Tables[0].Rows.Count > 0)
    //     {

    //        if (reportType == "Print")
    //        {

    //            if (scanStatus == 1)
    //            {
    //                //Add Company name and Employee name in the signature//

    //                drow["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);
    //                drow["signName"] = ScanemployeeName.Split('[').GetValue(0);
    //                //signature.Tables[0].Rows.Add(drow);

    //            }
    //            else
    //            {
    //                drow["Status"] = "2";
    //            }
    //            signature.Tables[0].Rows.Add(drow);
    //          //  signature.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xml");
    //          //  signature.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslSignature.xsd");

    //            generatePdf(Clients.Tables[0], Summary.Tables[0], AccountSummary.Tables[0], Holding.Tables[0],
    //                          signature.Tables[0], logo.Tables[0], qstr1, "No", digitalSignatureid);

    //        }
    //        else
    //            if (reportType == "Digital")
    //            {

    //                drow["Status"] = "3";
    //                signature.Tables[0].Rows.Add(drow);

    //                DataSet DigitalSignatureDs = new DataSet();

    //                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\")))
    //                {
    //                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\"));
    //                }


    //                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
    //                {

    //                    SqlCommand cmdDigital = new SqlCommand("cdsl_EmployeeName", con);
    //                    cmdDigital.CommandType = CommandType.StoredProcedure;
    //                    cmdDigital.Parameters.AddWithValue("@ID", digitalSignatureid);
    //                    cmdDigital.CommandTimeout =  0;
    //                    SqlDataAdapter daDigital = new SqlDataAdapter();
    //                    daDigital.SelectCommand = cmdDigital;
    //                    daDigital.Fill(DigitalSignatureDs);
    //                }

    //                DigitalSignatureDs.Tables[0].Columns.Add("companyName", System.Type.GetType("System.String"));
    //                DigitalSignatureDs.Tables[0].Rows[0]["companyName"] = Clients.Tables[0].Rows[0]["cmpname"].ToString().Split('[').GetValue(0);

    //                string tmpPdfPath, ReportPath, signPath, digitalSignaturePassword, signPdfPath, VirtualPath, finalResult;

    //                tmpPdfPath=string.Empty;
    //                ReportPath=string.Empty;
    //                signPath = string.Empty;
    //                finalResult = string.Empty;

    //                digitalSignaturePassword = DigitalSignatureDs.Tables[0].Rows[0]["pass"].ToString();

    //                tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
    //                if (qstr1 == "CDSL")
    //                {
    //                  ReportPath= HttpContext.Current.Server.MapPath("..\\Reports\\cdslBill.rpt");

    //                }
    //                else
    //                {
    //                    ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\NsdlBill.rpt");
    //                }

    //                signPath = HttpContext.Current.Server.MapPath(@"..\Documents\DigitalSignature\") +digitalSignatureid+ ".pfx";
    //                signPdfPath = objConverter.DirectoryPath(out VirtualPath);

    //                //BackProcess bp = new BackProcess();
    //                //bp.Clients = Clients;
    //                //bp.Summary = Summary;
    //                //bp.AccountSummary = AccountSummary;
    //                //bp.Holding = Holding;
    //                //bp.signature = signature;
    //                //bp.logo = logo;
    //                //bp.module = qstr1;
    //                //bp.DigitalCertified = "Yes";
    //                //bp.digitalSignaturePassword = digitalSignaturePassword;
    //                //bp.DigitalSignatureDs = DigitalSignatureDs;
    //                //bp.SignPath = signPath;
    //                //bp.reportPath = ReportPath;
    //                //bp.tmpPDFPath = tmpPdfPath;
    //                //bp.CompanyId = CompanyId;
    //                //bp.dpId = dpId;
    //                //bp.SegmentId = SegmentId;
    //                //bp.signPdfPath = signPdfPath;
    //                //bp.VirtualPath = VirtualPath;
    //                //bp.LastFinYear = HttpContext.Current.Session["LastFinYear"].ToString();
    //                //bp.user = HttpContext.Current.Session["userid"].ToString();
    //                //bp.backGroundWork();
    //               finalResult= generateindivisualPdf(Clients, Summary, AccountSummary,
    //                          Holding, signature, logo,
    //                          qstr1, "Yes", digitalSignaturePassword,
    //                          DigitalSignatureDs, signPath, ReportPath
    //                         , tmpPdfPath, CompanyId, dpId, SegmentId, signPdfPath, VirtualPath,
    //                         HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), 
    //                         EmailCreateAppMenuId);
    //               if (finalResult == "Success")
    //               {
    //                   ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('Successfully Document Generated.');", true);

    //               }

    //            }
    //     }
    //    else
    //     {
    //        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page , HttpContext.Current.GetType(), "hie", "alert('No Record Found');", true);

    //     }

    //    }
    //    return 0;
    // }



    public string generateindivisualPdf(DataSet Clients, DataSet Summary, DataSet AccountSummary,
                                      DataSet Holding, DataSet signature, DataSet logo, 
                                      string module, string DigitalCertified, string digitalSignaturePassword, 
                                      DataSet DigitalSignatureDs, string SignPath, string reportPath
                                      , string tmpPDFPath, string CompanyId, string dpId, 
                                        string SegmentId, string signPdfPath, string VirtualPath,
                                         string user, string LastFinYear, int EmailCreateAppMenuId)
    {


        string status;
        status = string.Empty;
        DataTable FilterClients = new DataTable();
        DataTable FilterSummary = new DataTable();
        DataTable FilterAccountSummary = new DataTable();
        DataTable FilterHolding = new DataTable();

        DataView viewClients = new DataView(Clients.Tables[0]);
        DataView viewSummary = new DataView(Summary.Tables[0]);
        DataView viewAccountSummary = new DataView(AccountSummary.Tables[0]);
        DataView viewHolding = new DataView(Holding.Tables[0]);

        if (module == "CDSL")
            DistinctBillNumber = viewClients.ToTable(true, new string[] { "DPBillSummary_BillNumber", "CdslClients_BOID", "DPBillSummary_BenAccountNumber" });
        else if (module == "NSDL")
            DistinctBillNumber = viewClients.ToTable(true, new string[] { "DPBillSummary_BillNumber", "NsdlClients_BenAccountID" });



        foreach (DataRow dr in DistinctBillNumber.Rows)
        {
            if (module == "NSDL")
            {
                viewClients.RowFilter = "DPBillSummary_BillNumber = '" + dr["DPBillSummary_BillNumber"] + "' and NsdlClients_FirstHolderEmail<>'abcdefgh'";
            }
            else
            {
                viewClients.RowFilter = "DPBillSummary_BillNumber = '" + dr["DPBillSummary_BillNumber"] + "' and CdslClients_EmailID<>'abcdefgh'";
            }
            FilterClients = viewClients.ToTable();
            if (FilterClients.Rows.Count >= 1)
            {
                viewSummary.RowFilter = "BillNumber = '" + dr["DPBillSummary_BillNumber"] + "' ";
                FilterSummary = viewSummary.ToTable();

                if (module == "CDSL")
                    viewAccountSummary.RowFilter = "benAccountnumber = '" + dr["DPBillSummary_BenAccountNumber"] + "'";
                else if (module == "NSDL")
                    viewAccountSummary.RowFilter = "benAccountnumber = '" + dr["NsdlClients_BenAccountID"] + "' ";

                FilterAccountSummary = viewAccountSummary.ToTable();

                viewHolding.RowFilter = "billNumber = '" + dr["DPBillSummary_BillNumber"] + "' ";
                FilterHolding = viewHolding.ToTable();

                //generatePdf(FilterClients, FilterSummary, FilterAccountSummary, FilterHolding,
                //                      signature.Tables[0], logo.Tables[0], module, DigitalCertified, DigitalSignPassword);

                status = digitallySignedPDF(FilterClients, FilterSummary, FilterAccountSummary, FilterHolding,
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




    public void generatePdf(DataTable Clients, DataTable Summary, DataTable AccountSummary,
                            DataTable Holding, DataTable signature, DataTable logo, 
                            string module, string DigitalCertified, string digitalSignatureid,string qstr1)
    {
        string path, oldTMPFolder;
        path = string.Empty;

        oldTMPFolder = string.Empty;

        if (Holding.Rows.Count == 0)
        {
            DataRow hld = Holding.NewRow();
            if (qstr1 == "CDSL")
                hld["CdslHolding_BenAccountNumber"] = "0";
            else if (qstr1 == "NSDL")
                hld["NsdlHolding_BenAccountNumber"] = "0";

            Holding.Rows.Add(hld);
        }

        if (Summary.Rows.Count == 0)
        {
            DataRow ben = Summary.NewRow();
            ben["BenAccountno"] = "0";
            Summary.Rows.Add(ben);
        }

        if (AccountSummary.Rows.Count == 0)
        {
            DataRow ben = AccountSummary.NewRow();
            ben["benAccountnumber"] = "0";
            AccountSummary.Rows.Add(ben);
        }



        //oldTMPFolder = System.Environment.GetEnvironmentVariable("TMP");
        //oldTEMPFolder = System.Environment.GetEnvironmentVariable("TEMP");
        //string tmpPath = HttpRuntime.CodegenDir;
        //System.Environment.SetEnvironmentVariable("TMP", tmpPath);
        //System.Environment.SetEnvironmentVariable("TEMP", tmpPath);

        //System.Environment.SetEnvironmentVariable("TMP", HttpContext.Current.Server.MapPath(@"..\Documents\"));


        ReportDocument cdslTransctionReportDocu = new ReportDocument();

        DataTable digi = new DataTable();
        digi.Columns.Add("companyName", System.Type.GetType("System.String"));
        digi.Columns.Add("Branch", System.Type.GetType("System.String"));
        digi.Columns.Add("signName", System.Type.GetType("System.String"));
        DataRow digirow = digi.NewRow();
        digirow[0] = DBNull.Value;
        digirow[1] = DBNull.Value;
        digirow[2] = DBNull.Value;
        digi.Rows.Add(digirow);

        // string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        // HttpContext.Current.SkipAuthorization = true;

        if (module == "CDSL")
        {
            path = HttpContext.Current.Server.MapPath("..\\Reports\\CDSLBill.rpt");
            //path = HttpContext.Current.Server.MapPath("..\\Reports\\test.rpt");
        }

        else if (module == "NSDL")
            path = HttpContext.Current.Server.MapPath("..\\Reports\\NsdlBill.rpt");


        cdslTransctionReportDocu.Load(path);

        //cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim());
        cdslTransctionReportDocu.SetDataSource(Clients);

        if (module == "CDSL")
        {
            cdslTransctionReportDocu.Subreports["billSummary"].SetDataSource(Summary);
            cdslTransctionReportDocu.Subreports["cdslAccounts"].SetDataSource(AccountSummary);
            cdslTransctionReportDocu.Subreports["cdslBillHolding"].SetDataSource(Holding);
            cdslTransctionReportDocu.Subreports["signature"].SetDataSource(signature);
            cdslTransctionReportDocu.Subreports["digitalSignature"].SetDataSource(digi);

        }
        else if (module == "NSDL")
        {
            cdslTransctionReportDocu.Subreports["NsdlBillSummary"].SetDataSource(Summary);
            cdslTransctionReportDocu.Subreports["NsdlBill_Accounts"].SetDataSource(AccountSummary);
            cdslTransctionReportDocu.Subreports["NsdlBill_Holding"].SetDataSource(Holding);
            cdslTransctionReportDocu.Subreports["NsdlBill_Signature"].SetDataSource(signature);
            cdslTransctionReportDocu.Subreports["DigitalSignature"].SetDataSource(digi);

        }
        cdslTransctionReportDocu.Subreports["logo"].SetDataSource(logo);



        if (module == "CDSL")
        {


            // cdslTransctionReportDocu.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,  HttpContext.Current.Server.MapPath(@"..\Documents\")+"1.pdf");

            if (HttpContext.Current.Session["mail"] != null)
            {
                cdslTransctionReportDocu.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Server.MapPath("../Documents") + "\\" + Summary.Rows[0][4] + ".pdf");


                HttpContext.Current.Session["mailpath"] = HttpContext.Current.Session["mailpath"].ToString() + HttpContext.Current.Server.MapPath("../Documents") + "\\" + Summary.Rows[0][4] + ".pdf" + "~";


            }
            else
            {
                cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "CDSL Bill");
            }


        }

        else if (module == "NSDL")
            cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSDL Bill");


        //System.Environment.SetEnvironmentVariable("TMP", oldTempFolder);
        //System.Environment.SetEnvironmentVariable("TMP", oldTMPFolder);
        //System.Environment.SetEnvironmentVariable("TEMP", oldTEMPFolder);

        cdslTransctionReportDocu.Dispose();
        cdslTransctionReportDocu = null;
        GC.Collect();
        //cdslTransctionReportDocu = null;
        //GC.Collect();

    }

    string digitallySignedPDF(DataTable Clients, DataTable Summary, DataTable AccountSummary,
                                DataTable Holding, DataTable signature, DataTable logo, string module
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
        string pdfstr = string.Empty;
        ReportDocument cdslTransctionReportDocu = new ReportDocument();
        //string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';'); MULTI
        string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');
        cdslTransctionReportDocu.Load(reportPath);
        cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);



        cdslTransctionReportDocu.SetDataSource(Clients);

        if (module == "CDSL")
        {
            cdslTransctionReportDocu.Subreports["billSummary"].SetDataSource(Summary);
            cdslTransctionReportDocu.Subreports["cdslAccounts"].SetDataSource(AccountSummary);
            cdslTransctionReportDocu.Subreports["cdslBillHolding"].SetDataSource(Holding);
            cdslTransctionReportDocu.Subreports["signature"].SetDataSource(signature);
        }
        else if (module == "NSDL")
        {
            cdslTransctionReportDocu.Subreports["NsdlBillSummary"].SetDataSource(Summary);
            cdslTransctionReportDocu.Subreports["NsdlBill_Accounts"].SetDataSource(AccountSummary);
            cdslTransctionReportDocu.Subreports["NsdlBill_Holding"].SetDataSource(Holding);
            cdslTransctionReportDocu.Subreports["NsdlBill_Signature"].SetDataSource(signature);

        }
        cdslTransctionReportDocu.Subreports["logo"].SetDataSource(logo);

        if (module == "CDSL")
            cdslTransctionReportDocu.Subreports["digitalSignature"].SetDataSource(DigitalSignatureDs);
        else if (module == "NSDL")
            cdslTransctionReportDocu.Subreports["DigitalSignature"].SetDataSource(DigitalSignatureDs);

        if (tmpPDFPath.Contains("EToken"))
        {
            string digSigID = signPath;
            pdfstr = tmpPDFPath + Clients.Rows[0]["DPBillSummary_BillNumber"].ToString() + "-" + Convert.ToDateTime(oDBEngine.GetDate()).ToString("ddMMMyyyy") + "-" + HttpContext.Current.Session["UserSegID"] + "-" + user + "-" + HttpContext.Current.Session["LastFinYear"].ToString().Replace("-", "") + "-" + digSigID + ".pdf";
        }
        else
        {
            pdfstr = tmpPDFPath + Clients.Rows[0]["DPBillSummary_BillNumber"].ToString() + ".pdf";
        }


        cdslTransctionReportDocu.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);
        if (tmpPDFPath.Contains("EToken"))
        {
            result = "Success";
        }
        else
        {
            if (module == "CDSL")
            {
                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='bill printing'", 1);
                result = ul.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
                                DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "5",
                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                Clients.Rows[0]["CdslClients_EmailID"].ToString(), Clients.Rows[0]["DPBillSummary_BillDate"].ToString(),
                                DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                VirtualPath, signPdfPath, user, LastFinYear, EmailCreateAppMenuId);
            }
            else if (module == "NSDL")
            {
                string[] str = oDBEngine.GetFieldValue1("tbl_trans_menu", "mnu_id", "mnu_segmentid=" + HttpContext.Current.Session["userlastsegment"] + " and mnu_menuName='bill printing'", 1);
                result = ul.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
                                  DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(),
                                  CompanyId, dpId, "5", DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
                                  Clients.Rows[0]["NsdlClients_FirstHolderEmail"].ToString(), Clients.Rows[0]["DPBillSummary_BillDate"].ToString(),
                                  DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
                                  VirtualPath, signPdfPath, user, LastFinYear, Convert.ToInt32(str[0]));

            }
        }

        cdslTransctionReportDocu.Dispose();
        cdslTransctionReportDocu = null;
        GC.Collect();


        return result;
    }

    //string digitallySignedPDF1(DataTable Clients, DataTable Summary, DataTable AccountSummary,
    //                            DataTable Holding, DataTable signature, DataTable logo, string module
    //                             , string Reportpath, string tmpPDFPath, string signPath
    //                                , string DigitalSignPassword, string CompanyId
    //                                , string dpId, string SegmentId, DataSet DigitalSignatureDs,
    //                            string reportPath, string VirtualPath,
    //                            string signPdfPath, string user, string LastFinYear, int EmailCreateAppMenuId)
    //{
    //    string path;
    //    path = string.Empty;
    //    result = string.Empty;
    //    ReportDocument cdslTransctionReportDocu = new ReportDocument();
    //    string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
    //    cdslTransctionReportDocu.Load(reportPath);
    //    cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);



    //    cdslTransctionReportDocu.SetDataSource(Clients);

    //    if (module == "CDSL")
    //    {
    //        cdslTransctionReportDocu.Subreports["billSummary"].SetDataSource(Summary);
    //        cdslTransctionReportDocu.Subreports["cdslAccounts"].SetDataSource(AccountSummary);
    //        cdslTransctionReportDocu.Subreports["cdslBillHolding"].SetDataSource(Holding);
    //        cdslTransctionReportDocu.Subreports["signature"].SetDataSource(signature);
    //    }
    //    else if (module == "NSDL")
    //    {
    //        cdslTransctionReportDocu.Subreports["NsdlBillSummary"].SetDataSource(Summary);
    //        cdslTransctionReportDocu.Subreports["NsdlBill_Accounts"].SetDataSource(AccountSummary);
    //        cdslTransctionReportDocu.Subreports["NsdlBill_Holding"].SetDataSource(Holding);
    //        cdslTransctionReportDocu.Subreports["NsdlBill_Signature"].SetDataSource(signature);

    //    }
    //    cdslTransctionReportDocu.Subreports["logo"].SetDataSource(logo);



    //    if (module == "CDSL")
    //        cdslTransctionReportDocu.Subreports["digitalSignature"].SetDataSource(DigitalSignatureDs);
    //    else if (module == "NSDL")
    //        cdslTransctionReportDocu.Subreports["DigitalSignature"].SetDataSource(DigitalSignatureDs);

    //    string pdfstr = tmpPDFPath + Clients.Rows[0]["DPBillSummary_BillNumber"].ToString() + ".pdf";


    //    cdslTransctionReportDocu.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfstr);


    //    if (module == "CDSL")
    //        result = objConverter.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
    //                        DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(), CompanyId, dpId, "5",
    //                        DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
    //                        Clients.Rows[0]["CdslClients_EmailID"].ToString(),"",
    //                        DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
    //                        VirtualPath, signPdfPath, user, LastFinYear,EmailCreateAppMenuId);

    //    else if (module == "NSDL")
    //        result = objConverter.DigitalCertificate(pdfstr, signPath, DigitalSignPassword, "Authentication",
    //                           DigitalSignatureDs.Tables[0].Rows[0]["Branch"].ToString(),
    //                           CompanyId, dpId, "5", DigitalSignatureDs.Tables[0].Rows[0]["cnt_internalId"].ToString(),
    //                           Clients.Rows[0]["NsdlClients_FirstHolderEmail"].ToString(),"",
    //                           DigitalSignatureDs.Tables[0].Rows[0]["cnt_branchid"].ToString(),
    //                           VirtualPath, signPdfPath, user, LastFinYear,EmailCreateAppMenuId);



    //    cdslTransctionReportDocu.Dispose();
    //    cdslTransctionReportDocu = null;
    //    GC.Collect();


    //    return result;
    //}

    //public string generateindivisualPdf1(DataSet Clients, DataSet Summary, DataSet AccountSummary,
    //                                  DataSet Holding, DataSet signature, DataSet logo,
    //                                  string module, string DigitalCertified, string digitalSignaturePassword,
    //                                  DataSet DigitalSignatureDs, string SignPath, string reportPath
    //                                  , string tmpPDFPath, string CompanyId, string dpId,
    //                                    string SegmentId, string signPdfPath, string VirtualPath,
    //                                     string user, string LastFinYear, int EmailCreateAppMenuId)
    //{


    //    string status;
    //    status = string.Empty;
    //    DataTable FilterClients = new DataTable();
    //    DataTable FilterSummary = new DataTable();
    //    DataTable FilterAccountSummary = new DataTable();
    //    DataTable FilterHolding = new DataTable();

    //    DataView viewClients = new DataView(Clients.Tables[0]);
    //    DataView viewSummary = new DataView(Summary.Tables[0]);
    //    DataView viewAccountSummary = new DataView(AccountSummary.Tables[0]);
    //    DataView viewHolding = new DataView(Holding.Tables[0]);

    //    if (module == "CDSL")
    //        DistinctBillNumber = viewClients.ToTable(true, new string[] { "DPBillSummary_BillNumber", "CdslClients_BOID", "DPBillSummary_BenAccountNumber" });
    //    else if (module == "NSDL")
    //        DistinctBillNumber = viewClients.ToTable(true, new string[] { "DPBillSummary_BillNumber", "NsdlClients_BenAccountID" });





    //    foreach (DataRow dr in DistinctBillNumber.Rows)
    //    {

    //        viewClients.RowFilter = "DPBillSummary_BillNumber = '" + dr["DPBillSummary_BillNumber"] + "'";
    //        FilterClients = viewClients.ToTable();

    //        viewSummary.RowFilter = "BillNumber = '" + dr["DPBillSummary_BillNumber"] + "'";
    //        FilterSummary = viewSummary.ToTable();

    //        if (module == "CDSL")
    //            viewAccountSummary.RowFilter = "benAccountnumber = '" + dr["DPBillSummary_BenAccountNumber"] + "'";
    //        else if (module == "NSDL")
    //            viewAccountSummary.RowFilter = "benAccountnumber = '" + dr["NsdlClients_BenAccountID"] + "'";

    //        FilterAccountSummary = viewAccountSummary.ToTable();

    //        viewHolding.RowFilter = "billNumber = '" + dr["DPBillSummary_BillNumber"] + "'";
    //        FilterHolding = viewHolding.ToTable();

    //        //generatePdf(FilterClients, FilterSummary, FilterAccountSummary, FilterHolding,
    //        //                      signature.Tables[0], logo.Tables[0], module, DigitalCertified, DigitalSignPassword);

    //        status = digitallySignedPDF(FilterClients, FilterSummary, FilterAccountSummary, FilterHolding,
    //                               signature.Tables[0], logo.Tables[0], module,
    //                                reportPath, tmpPDFPath,
    //                                SignPath, digitalSignaturePassword,
    //                                CompanyId, dpId, SegmentId, DigitalSignatureDs, reportPath,
    //                                VirtualPath, signPdfPath, user, LastFinYear,EmailCreateAppMenuId);


    //        FilterClients.Reset();
    //        FilterSummary.Reset();
    //        FilterAccountSummary.Reset();
    //        FilterHolding.Reset();

    //        if (status != "Success")
    //        {
    //            ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "hie", "alert('" + status + "');", true);
    //            break;
    //        }

    //    }

    //    return status;
    //    //cdslTransctionReportDocu = null;
    //    //GC.Collect();

    //}

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


    #endregion


}
