using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace OpeningEntry.OpeningEntry.OpeningServices
{
    /// <summary>
    /// Summary description for OpeningViewService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class OpeningViewService : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]


        public object GetInvoiceDetails(string InvoiceId)
        {
            SaleInvoiceView SlInv = new SaleInvoiceView();
            InvoiceDetails InDetails = new InvoiceDetails();
            List<OldUnitDetails> Olddet = new List<OldUnitDetails>();

            List<ProductDetails> Pdetail = new List<ProductDetails>();
            List<TaxDetails> TDetail = new List<TaxDetails>();
            List<Adjustment> Adjust = new List<Adjustment>();
            List<PaymentDetails> PayDetail = new List<PaymentDetails>();
            List<InvoiceTaxDetails> InvTax = new List<InvoiceTaxDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_ViewDetails");
                proc.AddVarcharPara("@Action", 50, "SaleInvoiceDetailsById");
                proc.AddVarcharPara("@InvoiceId", 50, InvoiceId);
                DataSet ds = proc.GetDataSet();

                DataTable dt = ds.Tables[0];
                DataTable products = ds.Tables[1];
                DataTable Tax = ds.Tables[2];
                DataTable UnitDet = ds.Tables[3];
                DataTable AdjustDocu = ds.Tables[4];
                DataTable PayAcc = ds.Tables[5];
                DataTable SaleInTax = ds.Tables[6];
                InDetails = (from DataRow dr in dt.Rows
                             select new InvoiceDetails()
                             {

                                 Id = Convert.ToInt64(dr["Id"]),
                                 InvoiceNumber = Convert.ToString(dr["InvoiceNumber"]),
                                 InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                                 DelivaryDate = Convert.ToString(dr["DelivaryDate"]),
                                 CustomerName = Convert.ToString(dr["CustomerName"]),
                                 SalesmanName = Convert.ToString(dr["SalesmanName"]),
                                 Reference = Convert.ToString(dr["Reference"]),
                                 challan_no = Convert.ToString(dr["challan_no"]),
                                 ChallanDate = Convert.ToString(dr["ChallanDate"]),
                                 Remarks = Convert.ToString(dr["Remarks"]),
                                 EntryType = Convert.ToString(dr["EntryType"]),
                                 FinancerName = Convert.ToString(dr["FinancerName"]),
                                 ExcutiveName = Convert.ToString(dr["ExcutiveName"]),
                                 EmiDetails = Convert.ToString(dr["EmiDetails"]),
                                 Scheme = Convert.ToString(dr["Scheme"]),
                                 SFCode = Convert.ToString(dr["SFCode"]),
                                 DBD = Convert.ToDecimal(dr["DBD"]),
                                 DBDPercent = Convert.ToDecimal(dr["DBDPercent"]),
                                 Downpayment = Convert.ToString(dr["Downpayment"]),
                                 Fee = Convert.ToString(dr["Fee"]),
                                 EMIOtherCharges = Convert.ToString(dr["EMIOtherCharges"]),
                                 TotalDPAmount = Convert.ToString(dr["TotalDPAmount"]),
                                 FinancerDue = Convert.ToString(dr["FinancerDue"]),
                                 FinancerChallanNo = Convert.ToString(dr["FinancerChallanNo"]),
                                 FinancerChallanDate = Convert.ToString(dr["FinancerChallanDate"]),
                                 isFromPos = Convert.ToBoolean(dr["isFromPos"]),
                                 BillingAddress = Convert.ToString(dr["BillingAddress"]),
                                 ShippingAddress = Convert.ToString(dr["ShippingAddress"])

                             }).FirstOrDefault();


                Pdetail = (from DataRow dr in products.Rows
                           select new ProductDetails()
                           {
                               DetailsId = Convert.ToInt64(dr["DetailsId"]),
                               ProductName = Convert.ToString(dr["ProductName"]),
                               Quantity = Convert.ToString(dr["Quantity"]),
                               UOM = Convert.ToString(dr["UOM"]),
                               Price = Convert.ToString(dr["Price"]),
                               Amount = Convert.ToString(dr["Amount"]),
                               Charges = Convert.ToString(dr["Charges"]),
                               NetAmount = Convert.ToString(dr["NetAmount"])

                           }).ToList();


                TDetail = (from DataRow dr in Tax.Rows
                           select new TaxDetails()
                           {

                               ProductTaxId = Convert.ToString(dr["ProductTaxId"]),
                               TaxPercentage = Convert.ToString(dr["TaxPercentage"]),
                               TaxAmount = Convert.ToString(dr["TaxAmount"]),
                               TaxSchemeName = Convert.ToString(dr["TaxSchemeName"])

                           }).ToList();


                Olddet = (from DataRow dr in UnitDet.Rows
                          select new OldUnitDetails()
                          {

                              UnitValue = Convert.ToString(dr["UnitValue"]),
                              ProductName = Convert.ToString(dr["ProductName"]),
                              Quantity = Convert.ToString(dr["Quantity"])

                          }).ToList();

                Adjust = (from DataRow dr in AdjustDocu.Rows
                          select new Adjustment()
                          {

                              DocumentNumber = Convert.ToString(dr["DocumentNumber"]),
                              DocumentType = Convert.ToString(dr["DocumentType"]),
                              AdjustmentAmount = Convert.ToString(dr["AdjustmentAmount"])

                          }).ToList();

                PayDetail = (from DataRow dr in PayAcc.Rows
                             select new PaymentDetails()
                             {
                                 PaymentType = Convert.ToString(dr["PaymentType"]),
                                 InstrumentNo = Convert.ToString(dr["InstrumentNo"]),
                                 cardType = Convert.ToString(dr["cardType"]),
                                 ApprovalCode = Convert.ToString(dr["ApprovalCode"]),
                                 EnterDate = Convert.ToString(dr["EnterDate"]),
                                 DraweeDate = Convert.ToString(dr["DraweeDate"]),
                                 Remarks = Convert.ToString(dr["Remarks"]),
                                 Amount = Convert.ToString(dr["Amount"]),
                                 AccountCode = Convert.ToString(dr["AccountCode"])

                             }).ToList();
                InvTax = (from DataRow dr in SaleInTax.Rows
                          select new InvoiceTaxDetails()
                          {
                              TaxSchemeName = Convert.ToString(dr["TaxSchemeName"]),
                              TaxPercentage = Convert.ToString(dr["TaxPercentage"]),
                              TaxAmount = Convert.ToString(dr["TaxAmount"]),

                          }).ToList();

                SlInv.InvoiceTax = InvTax;
                SlInv.HeaderDetails = InDetails;
                SlInv.ProDetails = Pdetail;
                SlInv.TxDetail = TDetail;
                SlInv.OldDetails = Olddet;
                SlInv.AdjustDetails = Adjust;
                SlInv.Payment = PayDetail;

                return SlInv;
            }

            return null;
        }




        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object GetInvoiceProductDetails(string InvoiceDetailsId)
        //{
        //    List<ProductDetails> Pdetail = new List<ProductDetails>();

        //    if (HttpContext.Current.Session["userid"] != null)
        //    {
        //        ProcedureExecute proc = new ProcedureExecute("Prc_ViewDetails");
        //        proc.AddVarcharPara("@Action", 50, "ProductInvoiceDetailsById");
        //        proc.AddVarcharPara("@InvoiceDetailsId", 50, InvoiceDetailsId);
        //        DataTable Dttable = proc.GetTable();

        //        Pdetail = (from DataRow dr in Dttable.Rows
        //                   select new ProductDetails()
        //                   {
        //                       DetailsId = Convert.ToInt64(dr["DetailsId"]),
        //                       ProductName = Convert.ToString(dr["ProductName"]),
        //                       Quantity = Convert.ToString(dr["Quantity"]),
        //                       UOM = Convert.ToString(dr["UOM"]),
        //                       Price = Convert.ToString(dr["Price"]),
        //                       Amount = Convert.ToString(dr["Amount"]),
        //                       Charges = Convert.ToString(dr["Charges"]),
        //                       NetAmount = Convert.ToString(dr["NetAmount"])

        //                   }).ToList();

        //        return Pdetail;

        //    }
        //    return null;
        //}
    }


    public class InvoiceDetails
    {
        public Int64 Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string DelivaryDate { get; set; }
        public string CustomerName { get; set; }

        public string SalesmanName { get; set; }
        public string Reference { get; set; }
        public string challan_no { get; set; }
        public string ChallanDate { get; set; }

        public string Remarks { get; set; }
        public string EntryType { get; set; }
        public string FinancerName { get; set; }
        public string ExcutiveName { get; set; }
        public string EmiDetails { get; set; }
        public string Scheme { get; set; }
        public string SFCode { get; set; }
        public decimal DBD { get; set; }
        public decimal DBDPercent { get; set; }
        public string Downpayment { get; set; }
        public string Fee { get; set; }
        public string EMIOtherCharges { get; set; }
        public string TotalDPAmount { get; set; }
        public string FinancerDue { get; set; }
        public string FinancerChallanNo { get; set; }
        public string FinancerChallanDate { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public bool isFromPos { get; set; }
    }

    public class SaleInvoiceView
    {

        public InvoiceDetails HeaderDetails { get; set; }

        public List<OldUnitDetails> OldDetails { get; set; }
        public List<ProductDetails> ProDetails { get; set; }

        public List<TaxDetails> TxDetail { get; set; }

        public List<Adjustment> AdjustDetails { get; set; }

        public List<PaymentDetails> Payment { get; set; }
        public List<InvoiceTaxDetails> InvoiceTax { get; set; }
    }


    public class ProductDetails
    {

        public Int64 DetailsId { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public string Charges { get; set; }
        public string NetAmount { get; set; }

    }


    public class TaxDetails
    {
        public string ProductTaxId { get; set; }
        public string TaxPercentage { get; set; }
        public string TaxAmount { get; set; }
        public string TaxSchemeName { get; set; }

    }

    public class OldUnitDetails
    {
        public string UnitValue { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }

    }

    public class Adjustment
    {
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public string AdjustmentAmount { get; set; }
    }

    public class PaymentDetails
    {
        public string PaymentType { get; set; }
        public string InstrumentNo { get; set; }
        public string cardType { get; set; }
        public string ApprovalCode { get; set; }
        public string EnterDate { get; set; }
        public string DraweeDate { get; set; }
        public string Remarks { get; set; }
        public string Amount { get; set; }
        public string AccountCode { get; set; }
    }


    public class InvoiceTaxDetails
    {
        public string TaxSchemeName { get; set; }
        public string TaxPercentage { get; set; }
        public string TaxAmount { get; set; }
    }

}