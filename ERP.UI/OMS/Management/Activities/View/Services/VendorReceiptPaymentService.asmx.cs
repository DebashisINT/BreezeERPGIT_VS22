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
    /// Summary description for VendorReceiptPaymentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class VendorReceiptPaymentService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetVendorPaymentReceiptHeaderDetails(string ReceiptPayment_ID)
        {
            VendorPaymentReceipt VendorPaymentReceipt = new VendorPaymentReceipt();
            VendorPaymentReceiptHeaderDetails HeaderDetails = new VendorPaymentReceiptHeaderDetails();
            List<VendorReceiptPaymentclassAddressDetails> _address_Details = new List<VendorReceiptPaymentclassAddressDetails>();
            List<VendorPaymentReceiptDetails> PaymentReceiptDetails = new List<VendorPaymentReceiptDetails>();
            VendorPaymentReceiptProductDetails ProductDetails = new VendorPaymentReceiptProductDetails();
            List<TotalTaxDetailsForVendorReceiptPayment> TotalTaxDetails = new List<TotalTaxDetailsForVendorReceiptPayment>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                try
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentViewDetails");
                    proc.AddVarcharPara("@Action", 50, "VendorReceiptPaymentViewDetails");
                    proc.AddVarcharPara("@ReceiptPayment_ID", 50, ReceiptPayment_ID);
                    DataSet ds = proc.GetDataSet();

                    //DataTable dt = ds.Tables[0];
                    //DataTable products = ds.Tables[1];
                    HeaderDetails.ReceiptPaymentId = ds.Tables[0].Rows[0]["ReceiptPayment_ID"].ToString().Trim();
                    HeaderDetails.VoucherType = ds.Tables[0].Rows[0]["VoucherType"].ToString().Trim();
                    HeaderDetails.Unit = ds.Tables[0].Rows[0]["Unit"].ToString().Trim();
                    HeaderDetails.forUnit = ds.Tables[0].Rows[0]["forUnit"].ToString().Trim();
                    HeaderDetails.DocumentNo = ds.Tables[0].Rows[0]["DocumentNo"].ToString().Trim();
                    HeaderDetails.PostingDate = Convert.ToString(ds.Tables[0].Rows[0]["PostingDate"]);
                    HeaderDetails.CashBank = ds.Tables[0].Rows[0]["CashBankName"].ToString().Trim();
                    HeaderDetails.Currency = ds.Tables[0].Rows[0]["CurrencyName"].ToString().Trim();
                    HeaderDetails.Customer = ds.Tables[0].Rows[0]["Customer"].ToString().Trim();
                    HeaderDetails.VoucherAmount = ds.Tables[0].Rows[0]["VoucherAmount"].ToString().Trim();
                    HeaderDetails.Rate = ds.Tables[0].Rows[0]["Rate"].ToString().Trim();
                    HeaderDetails.InstrumentType = ds.Tables[0].Rows[0]["InstrumentType"].ToString().Trim();
                    HeaderDetails.InstrumentNo = ds.Tables[0].Rows[0]["InstrumentNo"].ToString().Trim();
                    HeaderDetails.InstrumentDate = Convert.ToString(ds.Tables[0].Rows[0]["InstrumentDate"]);
                    // HeaderDetails.DrawOn = Convert.ToString(ds.Tables[0].Rows[0]["DrawOn"]);
                    // HeaderDetails.PaymentAmount = ds.Tables[0].Rows[0]["PaymentAmount"].ToString().Trim();
                    HeaderDetails.Contact_Person = Convert.ToString(ds.Tables[0].Rows[0]["Contact_Person"]);
                    HeaderDetails.Narration = ds.Tables[0].Rows[0]["Narration"].ToString().Trim();
                    HeaderDetails.GSTApplicable = Convert.ToBoolean(ds.Tables[0].Rows[0]["GSTApplicable"].ToString().Trim());
                    HeaderDetails.contacttype = ds.Tables[0].Rows[0]["contacttype"].ToString().Trim();
                    HeaderDetails.TotalPaymentAmount = ds.Tables[5].Rows[0]["PaymentSUM"].ToString().Trim();
                    HeaderDetails.TotalReceiptAmount = ds.Tables[5].Rows[0]["ReceiptSUM"].ToString().Trim();
                    


                    _address_Details = (from DataRow dr in ds.Tables[1].Rows
                                        select new VendorReceiptPaymentclassAddressDetails()
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


                    PaymentReceiptDetails = (from DataRow dr in ds.Tables[2].Rows
                                             select new VendorPaymentReceiptDetails()
                                       {
                                           SrlNo = Convert.ToString(dr["SrlNo"]),
                                           Type = Convert.ToString(dr["Type"]),
                                           ReceiptDetail_ID = Convert.ToString(dr["ReceiptDetail_ID"]),
                                           DocumentID = Convert.ToString(dr["DocumentID"]),
                                           Payment = Convert.ToString(dr["Payment"]),
                                           Receipt = Convert.ToString(dr["Receipt"]),
                                           Remarks = Convert.ToString(dr["Remarks"]),
                                           DocumentNo = Convert.ToString(dr["DocumentNo"]),
                                           IsOpening = Convert.ToString(dr["IsOpening"])
                                         }).ToList();


                    if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[3].Rows[0]["productIds"])))
                    {
                        ProductDetails.productsname = Convert.ToString(ds.Tables[3].Rows[0]["productsname"]);
                        ProductDetails.productIds = Convert.ToString(ds.Tables[3].Rows[0]["productIds"]);
                        ProductDetails.TypeId = Convert.ToString(ds.Tables[3].Rows[0]["HsnCode"]);
                    }

                    TotalTaxDetails = (from DataRow dr in ds.Tables[4].Rows
                                       select new TotalTaxDetailsForVendorReceiptPayment()
                                                 {
                                                     TaxRates_Sequence = Convert.ToString(dr["TaxRates_Sequence"]),
                                                     Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                                     Taxes_Name = Convert.ToString(dr["Taxes_Name"]),
                                                     Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                                     Percentage = Convert.ToString(dr["Percentage"]),
                                                     Amount = Convert.ToString(dr["Amount"])
                                                 }).ToList();


                    VendorPaymentReceipt.HeaderDetails = HeaderDetails;
                    VendorPaymentReceipt.AddressDetails = _address_Details;
                    VendorPaymentReceipt.PaymentReceiptDetails = PaymentReceiptDetails;
                    VendorPaymentReceipt.ProductDetails = ProductDetails;
                    VendorPaymentReceipt.TotalTaxDetails = TotalTaxDetails;

                    VendorPaymentReceipt.msg = "ok";
                }

                catch (Exception ex)
                {
                    VendorPaymentReceipt.msg = ex.Message;
                }
                
            }
            return VendorPaymentReceipt;
        }
    }
    

    public class VendorPaymentReceipt
    {
        public string msg { get; set; }
        public VendorPaymentReceiptHeaderDetails HeaderDetails { get; set; }
       

        public List<VendorReceiptPaymentclassAddressDetails> AddressDetails { get; set; }

        public List<VendorPaymentReceiptDetails> PaymentReceiptDetails { get; set; }
        public VendorPaymentReceiptProductDetails ProductDetails { get; set; }
        public List<TotalTaxDetailsForVendorReceiptPayment> TotalTaxDetails { get; set; }
    }

    public class VendorPaymentReceiptHeaderDetails
    {
        public string ReceiptPaymentId { get; set; }
        public string VoucherType { get; set; }
        public string Unit { get; set; }
        public string forUnit { get; set; }
        public string DocumentNo { get; set; }
        public string PostingDate { get; set; }
        public string CashBank { get; set; }
        public string Currency { get; set; }
        public string Rate { get; set; }
        public string InstrumentType { get; set; }
        public string InstrumentNo { get; set; }
        public string InstrumentDate { get; set; }
        public string Contact_Person { get; set; }
        public string Narration { get; set; }
        public string VoucherAmount { get; set; }
        //public string PaymentAmount { get; set; }
        public string Customer { get; set; }
        public string CustomerType { get; set; }
        //public string DrawOn { get; set; }
        public bool GSTApplicable { get; set; }
        public string contacttype { get; set; }

        public string TotalPaymentAmount { get; set; }
        public string TotalReceiptAmount { get; set; }


    }
    public class VendorReceiptPaymentclassAddressDetails
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
    public class VendorPaymentReceiptDetails
    {
        public string SrlNo { get; set; }
        public string ReceiptDetail_ID { get; set; }
        public string Type { get; set; }
        public string DocumentID { get; set; }
        public string Payment { get; set; }
        public string Receipt { get; set; }
        public string Remarks { get; set; }
        public string DocumentNo { get; set; }
        public string IsOpening { get; set; }

        
        

    }
    public class VendorPaymentReceiptProductDetails
    {
        public string productIds { get; set; }
        public string productsname { get; set; }
        public string TypeId { get; set; }
    }

    public class TotalTaxDetailsForVendorReceiptPayment
    {
        public string TaxRates_Sequence { get; set; }
        public string Taxes_Code { get; set; }
        public string Taxes_Name { get; set; }
        public string Taxes_ApplicableOn { get; set; }
        public string Percentage { get; set; }
        public string Amount { get; set; }
    }

}
