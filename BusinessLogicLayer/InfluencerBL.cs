using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class InfluencerBL
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string JVNumStr { get; set; }
        public string SaveInfluencerSchemeData(infulencerSaveData inf, DataTable prod, DataTable details, string ACTION)
        {

            try
            {


                if (inf.Action == "Edit")
                {
                    JVNumStr = inf.Billno;
                }
                else
                {
                    DateTime UpdatedTime = inf.PostingDate ?? DateTime.Now;
                    string validate = checkNMakeJVCode(Convert.ToString(inf.Billno), Convert.ToInt32(inf.Schema), UpdatedTime);

                    if (validate == "duplicate")
                    {
                        return "Journal Number already exists.";
                    }
                }

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INFLUENCERSCHEME_ADDEDIT", con);
                DataTable dtReceipt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AcTION", inf.Action);
                SqlParameter paramCustomer = cmd.Parameters.Add("@udt_Influencer", SqlDbType.Structured);
                paramCustomer.Value = details;

                //cmd.Parameters.AddWithValue("@Influencer", details);
                //cmd.Parameters.AddWithValue("@InfluencerProduct", prod);

                SqlParameter paramProd = cmd.Parameters.Add("@udt_InfluencerProduct", SqlDbType.Structured);
                paramProd.Value = prod;
                cmd.Parameters.AddWithValue("@PostingDate", inf.PostingDate);
                cmd.Parameters.AddWithValue("@Schema", inf.Schema);
                cmd.Parameters.AddWithValue("@Billno", JVNumStr);
                cmd.Parameters.AddWithValue("@Branch", inf.Branch);
                cmd.Parameters.AddWithValue("@Invoice_Id", inf.Invoice_Id);
                cmd.Parameters.AddWithValue("@IsOpening", inf.IsOpening);
                cmd.Parameters.AddWithValue("@Remarks", inf.Remarks);
                cmd.Parameters.AddWithValue("@Mainacdr", inf.Mainaccr);
                cmd.Parameters.AddWithValue("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                //SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                //output.Direction = ParameterDirection.Output;

                //SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
                //outputText.Direction = ParameterDirection.Output;



                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                //string OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());

                //string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());

                return Convert.ToString("Influencer details saved successfully.");


            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }


        public string SaveInfluencerData(infulencerSaveData inf, DataTable prod, DataTable details, string ACTION)
        {

            try
            {


                if (inf.Action == "Edit")
                {
                    JVNumStr = inf.Billno;
                }
                else
                {
                    DateTime UpdatedTime = inf.PostingDate ?? DateTime.Now;
                    string validate = checkNMakeJVCode(Convert.ToString(inf.Billno), Convert.ToInt32(inf.Schema), UpdatedTime);

                    if (validate == "duplicate")
                    {
                        return "Journal Number already exists.";
                    }
                }

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INFLUENCER_ADDEDIT", con);
                DataTable dtReceipt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AcTION", inf.Action);
                SqlParameter paramCustomer = cmd.Parameters.Add("@udt_Influencer", SqlDbType.Structured);
                paramCustomer.Value = details;

                //cmd.Parameters.AddWithValue("@Influencer", details);
                //cmd.Parameters.AddWithValue("@InfluencerProduct", prod);

                SqlParameter paramProd = cmd.Parameters.Add("@udt_InfluencerProduct", SqlDbType.Structured);
                paramProd.Value = prod;
                cmd.Parameters.AddWithValue("@PostingDate", inf.PostingDate);
                cmd.Parameters.AddWithValue("@Schema", inf.Schema);
                cmd.Parameters.AddWithValue("@Billno", JVNumStr);
                cmd.Parameters.AddWithValue("@Branch", inf.Branch);
                cmd.Parameters.AddWithValue("@Invoice_Id", inf.Invoice_Id);
                cmd.Parameters.AddWithValue("@IsOpening", inf.IsOpening);
                cmd.Parameters.AddWithValue("@Remarks", inf.Remarks);
                cmd.Parameters.AddWithValue("@Mainacdr", inf.Mainaccr);
                cmd.Parameters.AddWithValue("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                //SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                //output.Direction = ParameterDirection.Output;

                //SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
                //outputText.Direction = ParameterDirection.Output;



                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                //string OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());

                //string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());

                return Convert.ToString("Generated auto journal no is " + JVNumStr);


            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        public string GetInfluencerAmount(infulencerSaveData inf, DataTable prod, DataTable details, string ACTION)
        {

            try
            {



                DataTable dsInst = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INFLUENCER_VALIDATE", con);
                DataTable dtReceipt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AcTION", "VALIDATEAMOUNT");
                SqlParameter paramCustomer = cmd.Parameters.Add("@udt_Influencer", SqlDbType.Structured);
                paramCustomer.Value = details;
                cmd.Parameters.AddWithValue("@Invoice_Id", inf.Invoice_Id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                //string OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());

                //string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());

                if (dsInst != null && dsInst.Rows.Count > 0)
                {
                    return Convert.ToString(dsInst.Rows[0][0]);
                }
                else
                {
                    return Convert.ToString("~");
                }

            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }
        public string SaveInfluencerAdjustmentData(DataTable Invoice, DataTable Returns)
        {

            try
            {


                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INFLUENCERADJUSTMENT_ADDEDIT", con);
                DataTable dtReceipt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AcTION", "Add");
                SqlParameter paramCustomer = cmd.Parameters.Add("@udt_InfluencerAdjustmentInvoice", SqlDbType.Structured);
                paramCustomer.Value = Invoice;
                SqlParameter paramProd = cmd.Parameters.Add("@udt_InfluencerAdjustmentReturn", SqlDbType.Structured);
                paramProd.Value = Returns;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return "";

            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        public string SaveInfluencerDataReturn(infulencerSaveData inf, DataTable prod, DataTable details, string ACTION)
        {

            try
            {


                if (inf.Action == "Edit")
                {
                    JVNumStr = inf.Billno;
                }
                else
                {
                    DateTime UpdatedTime = inf.PostingDate ?? DateTime.Now;
                    string validate = checkNMakeJVCode(Convert.ToString(inf.Billno), Convert.ToInt32(inf.Schema), UpdatedTime);

                    if (validate == "duplicate")
                    {
                        return "Journal Number already exists.";
                    }
                }

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INFLUENCER_RETURN_ADDEDIT", con);
                DataTable dtReceipt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AcTION", inf.Action);
                SqlParameter paramCustomer = cmd.Parameters.Add("@udt_Influencer", SqlDbType.Structured);
                paramCustomer.Value = details;

                //cmd.Parameters.AddWithValue("@Influencer", details);
                //cmd.Parameters.AddWithValue("@InfluencerProduct", prod);

                SqlParameter paramProd = cmd.Parameters.Add("@udt_InfluencerProduct", SqlDbType.Structured);
                paramProd.Value = prod;
                cmd.Parameters.AddWithValue("@PostingDate", inf.PostingDate);
                cmd.Parameters.AddWithValue("@Schema", inf.Schema);
                cmd.Parameters.AddWithValue("@Billno", JVNumStr);
                cmd.Parameters.AddWithValue("@Branch", inf.Branch);
                cmd.Parameters.AddWithValue("@Return_Id", inf.Invoice_Id);
                cmd.Parameters.AddWithValue("@Mainacdr", inf.Mainaccr);
                cmd.Parameters.AddWithValue("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                //SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                //output.Direction = ParameterDirection.Output;

                //SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
                //outputText.Direction = ParameterDirection.Output;



                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                //string OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());

                //string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());

                return Convert.ToString("Generated auto journal no is " + JVNumStr);


            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id, DateTime postingdate)
        {
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (postingdate.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
                    sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + postingdate.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    if ((dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") || (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE"))
                    {
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            JVNumStr = startNo.PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            JVNumStr = startNo.PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                }
                else
                {
                    sqlQuery = "SELECT JournalVoucher_BillNumber FROM Trans_JournalVoucher WHERE JournalVoucher_BillNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        public DataSet GetAllDropDownDataByVoucherType(string VoucherType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "AllDropDownDataCRP");
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@VoucherType", 10, VoucherType);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetInfluencerCashBank(string userbranch, string CompanyId, string RecPayId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetCashBankDataCRP");
            proc.AddVarcharPara("@BranchId", 100, userbranch);
            proc.AddVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddVarcharPara("@Payment_ID", 100, RecPayId);
            proc.AddBigIntegerPara("@UserId", Convert.ToInt64(HttpContext.Current.Session["userid"]));

            ds = proc.GetTable();
            return ds;
        }

        public DataTable PopulateContactPerson(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetContactPerson");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }
        public string GetInfluencerTDSRate(string InternalId, string TDSCode,string tdsdate)
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            //DataTable DT = objEngine.GetDataTable(" select TDSTCSRates_Rate,ISNULL(TDSTCSRates_Rouding,0) TDSTCSRates_Rouding from Config_MultiTDSTCSRates RATE INNER JOIN TBL_MASTER_CONTACT CNT ON CNT.TDSRATE_TYPE=RATE.TDSTCSRates_ApplicableFor AND CNT.cnt_internalId='" + InternalId + "' AND RATE.TDSTCSRates_Code='" + TDSCode + "'");
            //if (DT != null && DT.Rows.Count > 0)
            //{
            //    return Convert.ToString(DT.Rows[0][0]) + "~" + Convert.ToString(DT.Rows[0][1]);
            //}
            //else
            //{
            //    return "0.00~0";
            //}


            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            string act_dt = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(tdsdate))
                act_dt = DateTime.ParseExact(tdsdate, "dd-MM-yyyy", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
            DataTable DT = objEngine.GetDataTable(" select TDSTCSRates_Rate,ISNULL(TDSTCSRates_Rouding,0) TDSTCSRates_Rouding from Config_MultiTDSTCSRates RATE INNER JOIN TBL_MASTER_CONTACT CNT ON CNT.TDSRATE_TYPE=RATE.TDSTCSRates_ApplicableFor AND CNT.cnt_internalId='" + InternalId + "' AND RATE.TDSTCSRates_Code='" + TDSCode + "' and  TDSTCSRates_DateFrom<='" + act_dt + "' and isnull(TDSTCSRates_DateTo,dateadd(year,100,getdate()))>='" + act_dt + "'");
            if (DT != null && DT.Rows.Count > 0)
            {
                return Convert.ToString(DT.Rows[0][0]) + "~" + Convert.ToString(DT.Rows[0][1]);
            }
            else
            {
                return "0.00~0";
            }
        }
        public DataTable GetBranchStateCode(string BranchId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetBranchStateCode");
            proc.AddVarcharPara("@BranchId", 100, BranchId);
            ds = proc.GetTable();
            return ds;
        }
        public string DeleteInfluencerData(string invoice_id)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosInfluencer");
            proc.AddVarcharPara("@Action", 100, "Delete");
            proc.AddVarcharPara("@InvoiceId", 100, invoice_id);
            proc.AddVarcharPara("@CreateUser", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();
            return Convert.ToString(ds.Rows[0][0]);
        }
        public DataTable GetCurrentConvertedRate(int BaseCurrencyId, int ConvertedCurrencyId, string CompID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetCurrentConvertedRate");
            proc.AddIntegerPara("@BaseCurrencyId", BaseCurrencyId);
            proc.AddIntegerPara("@ConvertedCurrencyId", ConvertedCurrencyId);
            proc.AddVarcharPara("@CompanyId", 10, CompID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetAllDocumentPayment(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetDocDetailsPayment");
            proc.AddVarcharPara("@InternalId", 100, CustomerId);
            proc.AddVarcharPara("@BranchId", 50, BranchId);
            proc.AddVarcharPara("@Payment_ID", 10, ReceiptPaymentId);
            proc.AddDateTimePara("@Receiptdate", Convert.ToDateTime(TransDate));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetAllDocument(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetDocDetails");
            proc.AddVarcharPara("@InternalId", 100, CustomerId);
            proc.AddVarcharPara("@BranchId", 50, BranchId);
            proc.AddVarcharPara("@Payment_ID", 10, ReceiptPaymentId);
            proc.AddDateTimePara("@Receiptdate", Convert.ToDateTime(TransDate));

            dt = proc.GetTable();
            return dt;
        }
        public string AddEditPayment(ref string OutputId, string ActionType, string strEditCashBankID, string strCashBankBranchID, string strTransactionDate,
            string strCashBankID, string strExchangeSegmentID, string strTransactionType, string strEntryUserProfile, string strVoucherAmount,
            string strCustomer, string strContactName, string strNarration, string strCurrency, string strInstrumentType, string strInstrumentNumber,
            string strInstrumentDate, string strrate, DataTable strReceiptPaymentdt,
            string strEnterBranchID, string DrawnOn, string CompanyId, string LastFinYear, string userid,
            string SCHEMEID, string Doc_No, string MainAccountId, string MainAccountAmount, string tdsSection, string tdsAmount, string ActualAmount, Int64 ProjId, Boolean NILRateTDS)
        {
            try
            {

                DataSet dsInst = new DataSet();
                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_AddEditInfluencerPayment", con);
                DataTable dtPayment = new DataTable();

                dtPayment = GetReceiptDataSource();
                foreach (DataRow dr in strReceiptPaymentdt.Rows)
                {
                    dtPayment.Rows.Add(Convert.ToString(dr["TypeID"]), Convert.ToString(dr["DocumentNo"]), Convert.ToDecimal(dr["Payment"]),
                       Convert.ToString(dr["Remarks"]), Convert.ToString(dr["DocId"]), Convert.ToString(dr["IsOpening"]));
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@PaymentID", strEditCashBankID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                cmd.Parameters.AddWithValue("@FinYear", LastFinYear);
                cmd.Parameters.AddWithValue("@CreateUser", userid);
                cmd.Parameters.AddWithValue("@ForBranchID", strCashBankBranchID);
                cmd.Parameters.AddWithValue("@TransactionDate", DateTime.ParseExact(strTransactionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@CashBankID", strCashBankID);
                cmd.Parameters.AddWithValue("@ExchangeSegmentID", strExchangeSegmentID);
                cmd.Parameters.AddWithValue("@TransactionType", strTransactionType);
                cmd.Parameters.AddWithValue("@Narration", strNarration);
                cmd.Parameters.AddWithValue("@CurrencyID", strCurrency);
                cmd.Parameters.AddWithValue("@rate", strrate);
                cmd.Parameters.AddWithValue("@InstrumentType", strInstrumentType);
                cmd.Parameters.AddWithValue("@InstrumentNumber", strInstrumentNumber);

                if (!string.IsNullOrEmpty(strInstrumentDate) && strInstrumentDate != "01-01-1990")
                {
                    cmd.Parameters.AddWithValue("@InstrumentDate", DateTime.ParseExact(strInstrumentDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InstrumentDate", null);
                }
                cmd.Parameters.AddWithValue("@InfluencerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPersonID", strContactName);
                cmd.Parameters.AddWithValue("@VoucherAmount", strVoucherAmount);
                cmd.Parameters.AddWithValue("@Details", dtPayment);
                cmd.Parameters.AddWithValue("@EnterBranchID", strEnterBranchID);
                cmd.Parameters.AddWithValue("@DrawnOn", Convert.ToString(DrawnOn));
                cmd.Parameters.AddWithValue("@SCHEMEID", SCHEMEID);
                cmd.Parameters.AddWithValue("@Doc_No", Doc_No);
                cmd.Parameters.AddWithValue("@MainAccountId", MainAccountId);
                cmd.Parameters.AddWithValue("@MainAccountAmount", MainAccountAmount);
                cmd.Parameters.AddWithValue("@tdsSection", tdsSection);
                cmd.Parameters.AddWithValue("@tdsAmount", tdsAmount);
                cmd.Parameters.AddWithValue("@ActualAmount", ActualAmount);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);

                 //Nil Rate TDS add Tanmoy 01-12-2020
                cmd.Parameters.AddWithValue("@NILRateTDS", NILRateTDS);
                //Nil Rate TDS add Tanmoy 01-12-2020

                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
                outputText.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
                return Convert.ToString(strCPRID);
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }




        public DataTable GetReceiptDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("TypeID", typeof(System.String));
            RcpDt.Columns.Add("DocNo", typeof(System.String));
            RcpDt.Columns.Add("Receipt", typeof(System.Decimal));
            RcpDt.Columns.Add("Remarks", typeof(System.String));
            RcpDt.Columns.Add("DocId", typeof(System.String));
            RcpDt.Columns.Add("IsOpening", typeof(System.String));
            return RcpDt;
        }

        public DataTable CreatePaymentDataTable()
        {
            DataTable paymentDetails = new DataTable();
            paymentDetails.Columns.Add("Doc_type", typeof(System.String));
            paymentDetails.Columns.Add("Payment_type", typeof(System.String));
            paymentDetails.Columns.Add("PaymentType_details", typeof(System.String));
            paymentDetails.Columns.Add("cardType", typeof(System.String));
            paymentDetails.Columns.Add("AuthNo", typeof(System.String));
            paymentDetails.Columns.Add("payment_remarks", typeof(System.String));
            paymentDetails.Columns.Add("paymentAmount", typeof(System.String));
            paymentDetails.Columns.Add("payment_date", typeof(System.String));
            paymentDetails.Columns.Add("Drawee_date", typeof(System.String));
            paymentDetails.Columns.Add("Payment_mainAccount", typeof(System.String));

            return paymentDetails;
        }

        public DataSet GetEditDetails(string id, string userbranchlist, string FinYear, string userid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetEditDetails");
            proc.AddVarcharPara("@Payment_ID", 100, id);
            proc.AddVarcharPara("@userbranchlist", -1, userbranchlist);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@userid", 100, userid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetEditDetailsPayment(string id, string userbranchlist, string FinYear, string userid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetEditDetailsPayment");
            proc.AddVarcharPara("@Payment_ID", 100, id);
            proc.AddVarcharPara("@userbranchlist", -1, userbranchlist);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@userid", 100, userid);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable GetTaxTable(string hsnCodeTax, decimal Amount, string branchid, string customerstate)
        {

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
            proc.AddVarcharPara("@Action", 100, "GetTaxTable");
            proc.AddVarcharPara("@hsnCodeTax", 100, hsnCodeTax);
            proc.AddPara("@Amount", Amount);
            proc.AddVarcharPara("@branchid", 100, branchid);
            proc.AddVarcharPara("@customerstate", 100, customerstate);

            ds = proc.GetTable();
            return ds;

        }


        public int DeleteInfluencerPayment(string influencerID)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prdn_InfluencerDetails");
                proc.AddVarcharPara("@Action", 100, "Delete");
                proc.AddVarcharPara("@PAYMENTID", 100, influencerID);

                ds = proc.GetTable();
                return 1;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
    public class infulencerSaveData
    {
        public string Action { get; set; }
        public DateTime? PostingDate { get; set; }

        public string Schema { get; set; }
        public string Billno { get; set; }
        public string Branch { get; set; }
        public string Invoice_Id { get; set; }

        public string Remarks { get; set; }

        public string Mainaccr { get; set; }
        public string IsOpening { get; set; }

        public List<ProdDetails> product { get; set; }

        public List<InfluencerTable> Influencer { get; set; }




    }

    public class infulencerReturnAdjustMentDetails
    {
        public string DOC_ID { get; set; }
        public string CON_ID { get; set; }
        public string NAME { get; set; }

        public string  DOC_NUMBER { get; set; }
        public string DOC_DATE { get; set; }
        public string Total_Comm { get; set; }
        public string Unpaid { get; set; }

    }
    public class infulencerReturnAdjustMent
    {
        public List<infulencerReturnAdjustMentDetails> Invoice_Data { get; set; }
        public List<infulencerReturnAdjustMentDetails> Return_Data { get; set; }
    }





    public class INF_ADJ
    {
        public string AMOUNT { get; set; }
        public string DOC_ID { get; set; }
        public string INF_ID { get; set; }

    }




    public class ProdDetails
    {
        public string PRODID { get; set; }
        public string QTY { get; set; }
        public string SALESPRICE { get; set; }
        public string TOTALPRICE { get; set; }
        public string DETID { get; set; }
        public string BASIS { get; set; }
        public string PERSENTAGE { get; set; }
        public string AMOUNT { get; set; }

    }

    public class InfluencerTable
    {
        public string INFID { get; set; }
        public string MACRID { get; set; }
        public string AMT { get; set; }
    }

    public class Inf_Header_Details
    {
        public INF_Inv_Details INF_Inv_Details { get; set; }
        public List<INF_Inv_Products> INF_Inv_Products { get; set; }
        public Influencer Influencer { get; set; }
        public List<Influencer_Details> Influencer_Details { get; set; }
        public string RemainingBalance { get; set; }
    }

    public class INF_Inv_Details
    {
        public string Inv_Id { get; set; }
        public string Inv_No { get; set; }
        public string Amount { get; set; }
        public string Inv_BranchId { get; set; }
        public string AUTOJV_NUMBER { get; set; }
        public string AUTOJV_ID { get; set; }
        

    }

    public class INF_Inv_Products
    {
        public string prod_details_id { get; set; }
        public string Prod_id { get; set; }
        public string Prod_description { get; set; }
        public string prod_Qty { get; set; }
        public string prod_Salesprice { get; set; }

        public string prod_SalespriceWithGST { get; set; }
        public string prod_amt { get; set; }

        public string Prod_Percentage { get; set; }
        public string Applicable_On { get; set; }
        public string PROD_COMM_AMOUNT { get; set; }

    }

    public class Influencer
    {
        public string MainAccount_AccountCode { get; set; }
        public string CALCULATED_AMOUNT { get; set; }
        public string MAINACCOUNT_DR { get; set; }
        public string AMOUNT_DR { get; set; }
        public string AUTOJV_ID { get; set; }
        public string AUTOJV_NUMBER { get; set; }
        public DateTime POSTING_DATE { get; set; }
        public string COMM_AMOUNT { get; set; }
        public string IsTagged { get; set; }
        public string Remarks { get; set; }
    }
    public class Influencer_Details
    {
        public string DET_AMOUNT_CR { get; set; }

        public string DET_MAINACCOUNT_CR { get; set; }

        public string DET_MAINACCOUNT_NAME { get; set; }
        public string DET_INFLUENCER_ID { get; set; }
        public string INF_Name { get; set; }
    }


}
