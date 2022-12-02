using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.View.Services
{
    /// <summary>
    /// Summary description for CashBankService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CashBankService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetCashBankDtlsForReports(string cashBankId)
        {
            CashBank CashBank = new CashBank();

            CashBankHeaderDetails HeaderDetails = new CashBankHeaderDetails();
            List<CashBankClassAddressDetails> _address_Details = new List<CashBankClassAddressDetails>();
            List<CashBankDetails> CashBankDetails = new List<CashBankDetails>();

            if (HttpContext.Current.Session["userid"] != null)
            {
                try
                {
                    ProcedureExecute proc = new ProcedureExecute("Prc_CashBankViewDetails");
                    proc.AddVarcharPara("@Action", 50, "CashBankDetails");
                    proc.AddVarcharPara("@cashBankId", 50, cashBankId);
                    DataSet ds = proc.GetDataSet();

                    //DataTable dt = ds.Tables[0];
                    //DataTable products = ds.Tables[1];
                    HeaderDetails.CashBankId = ds.Tables[0].Rows[0]["CashBank_ID"].ToString().Trim();
                    HeaderDetails.VoucherType = ds.Tables[0].Rows[0]["VoucherType"].ToString().Trim();
                    HeaderDetails.Unit = ds.Tables[0].Rows[0]["branch_description"].ToString().Trim();
                    HeaderDetails.DocumentNo = ds.Tables[0].Rows[0]["OldVoucherNumber"].ToString().Trim();
                    HeaderDetails.PostingDate = Convert.ToString(ds.Tables[0].Rows[0]["TransactionDate"]);
                    HeaderDetails.ForUnit = ds.Tables[0].Rows[0]["forunit"].ToString().Trim();
                    HeaderDetails.CashBank = ds.Tables[0].Rows[0]["CashBankName"].ToString().Trim();
                    HeaderDetails.Currency = ds.Tables[0].Rows[0]["CurrencyName"].ToString().Trim();
                    HeaderDetails.Rate = ds.Tables[0].Rows[0]["Rate"].ToString().Trim();
                    HeaderDetails.InstrumentType = ds.Tables[0].Rows[0]["InstrumentTypeName"].ToString().Trim();
                    HeaderDetails.InstrumentNo = ds.Tables[0].Rows[0]["InstrumentNumber"].ToString().Trim();
                    HeaderDetails.InstrumentDate = Convert.ToString(ds.Tables[0].Rows[0]["InstrumentDate"]);
                    HeaderDetails.DraweeBank = ds.Tables[0].Rows[0]["CashBank_DraweeBank"].ToString().Trim();
                    HeaderDetails.PaymentAmount = ds.Tables[0].Rows[0]["PaymentAmount"].ToString().Trim();
                    HeaderDetails.PaidTo = ds.Tables[0].Rows[0]["CashBank_PaidTo"].ToString().Trim();
                    HeaderDetails.Contact = Convert.ToString(ds.Tables[0].Rows[0]["CashBank_ContactNo"]);
                    HeaderDetails.Narration = ds.Tables[0].Rows[0]["Narration"].ToString().Trim();
                    HeaderDetails.AmountsAre = ds.Tables[0].Rows[0]["taxGrp_Description"].ToString().Trim();

                    _address_Details = (from DataRow dr in ds.Tables[1].Rows
                                        select new CashBankClassAddressDetails()
                                             {
                                                 AddressType = Convert.ToString(dr["AddressType"]),
                                                 Address1 = Convert.ToString(dr["Address1"]),
                                                 Address2 = Convert.ToString(dr["Address2"]),
                                                 Address3 = Convert.ToString(dr["Address3"]),
                                                 LandMark = Convert.ToString(dr["LandMark"]),
                                                 CountryName = Convert.ToString(dr["CountryName"]),
                                                 StateName = Convert.ToString(dr["StateName"]),
                                                 AreaName = Convert.ToString(dr["AreaName"]),
                                                 Pincode = Convert.ToString(dr["Pincode"]),
                                                 GSTIN = Convert.ToString(dr["GSTIN"]),
                                                 CityName = Convert.ToString(dr["CityName"])
                                             }).ToList();


                    CashBankDetails = (from DataRow dr in ds.Tables[2].Rows
                                       select new CashBankDetails()
                               {
                                   SrlNo = Convert.ToString(dr["SrlNo"]),
                                   CashReportID = Convert.ToString(dr["CashReportID"]),
                                   MainAccount = Convert.ToString(dr["MainAccountID"]),
                                   SubAccount = Convert.ToString(dr["SubAccountID"]),
                                   PaymentAmount = Convert.ToString(dr["PaymentAmount"]),
                                   ReceiptAmount = Convert.ToString(dr["ReceiptAmount"]),
                                   NetAmount = Convert.ToString(dr["NetAmount"]),
                                   Charges = Convert.ToString(dr["TaxAmount"]),
                                   Remarks = Convert.ToString(dr["Remarks"]),
                                  

                               }).ToList();



                    CashBank.HeaderDetails = HeaderDetails;
                    CashBank.AddressDetails = _address_Details;
                    CashBank.CashBankDetails = CashBankDetails;
                    CashBank.msg = "ok";
                    CashBank.msg = "ok";


                }
                catch (Exception ex)
                {
                    CashBank.msg = ex.Message;
                }

            }

            return CashBank;
        }

    }

    public class CashBank
    {
        public string msg { get; set; }
        public CashBankHeaderDetails HeaderDetails { get; set; }
        public List<CashBankClassAddressDetails> AddressDetails { get; set; }
        public List<CashBankDetails>CashBankDetails { get; set; }


    }

    public class CashBankHeaderDetails
    {
        public string CashBankId { get; set; }
        public string VoucherType { get; set; }
        public string Unit { get; set; }
        public string DocumentNo { get; set; }
        public string PostingDate { get; set; }
        public string ForUnit { get; set; }
        public string CashBank { get; set; }
        public string Currency { get; set; }
        public string Rate { get; set; }
        public string InstrumentType { get; set; }
        public string InstrumentNo { get; set; }
        public string InstrumentDate { get; set; }
        public string DraweeBank { get; set; }
        public string PaidTo { get; set; }
        public string Contact { get; set; }
        public string Narration { get; set; }
        public string AmountsAre { get; set; }
        public string PaymentAmount { get; set; }

    }

    public class CashBankClassAddressDetails
    {
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LandMark { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public string GSTIN { get; set; }
    }


    public class CashBankDetails
    {

        public string SrlNo { get; set; }
        public string CashReportID { get; set; }
        public string MainAccount { get; set; }
        public string SubAccount { get; set; }
        public string PaymentAmount { get; set; }
        public string ReceiptAmount { get; set; }
        public string NetAmount { get; set; }
        public string Charges { get; set; }
        public string Remarks { get; set; }

    }

}
