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
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class PurchaseServices : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PurchaseInvoice_Details(string id)
        {
            PurchaseInvoiceDetails return_obj = new PurchaseInvoiceDetails();

            classHeaderDetails ob = new classHeaderDetails();
            List<classProductDetails> _pnv_header_Details = new List<classProductDetails>();
            classSummaryDetails _CalculatedAmount = new classSummaryDetails();
            List<classAddressDetails> _pnv_address_Details = new List<classAddressDetails>();
            List<classProductTaxDetails> _pnv_producttax_Details = new List<classProductTaxDetails>();
            List<classProductTaxDetails> _pnv_invoicetax_Details = new List<classProductTaxDetails>();

            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_view_pnv_details");
                    proc.AddVarcharPara("@Action", 50, "PurchaseInvoice_Details");
                    proc.AddVarcharPara("@id", 50, id);
                    DataSet ds = proc.GetDataSet();

                    ob.InvoiceNumber = ds.Tables[0].Rows[0]["doument_no"].ToString().Trim();
                    ob.BranchDescription = ds.Tables[0].Rows[0]["branch_description"].ToString().Trim();
                    ob.invoice_date = ds.Tables[0].Rows[0]["invoice_date"].ToString().Trim();
                    ob.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString().Trim();
                    ob.PartyInvoiceNo = ds.Tables[0].Rows[0]["PartyInvoiceNo"].ToString().Trim();
                    ob.Invoice_Reference = ds.Tables[0].Rows[0]["Invoice_Reference"].ToString().Trim();
                    ob.contact_name = ds.Tables[0].Rows[0]["cp_name"].ToString().Trim();
                    ob.Currency_AlphaCode = ds.Tables[0].Rows[0]["Currency_AlphaCode"].ToString().Trim();
                    ob.Currency_Conversion_Rate = ds.Tables[0].Rows[0]["Currency_Conversion_Rate"].ToString().Trim();
                    ob.Invoice_ReverseMechanism = ds.Tables[0].Rows[0]["Invoice_ReverseMechanism"].ToString().Trim();
                    ob.Amounts_are = ds.Tables[0].Rows[0]["Amounts are"].ToString().Trim();
                    ob.EWayBillNumber = ds.Tables[0].Rows[0]["EWayBillNumber"].ToString().Trim();
                    ob.Entry_date = ds.Tables[0].Rows[0]["Entry date"].ToString().Trim();
                    ob.Invoice_Remarks = ds.Tables[0].Rows[0]["Invoice_Remarks"].ToString().Trim();
                    ob.grn_no = ds.Tables[0].Rows[0]["grn_no"].ToString().Trim();
                    ob.invoice_type = ds.Tables[0].Rows[0]["invoice_type"].ToString().Trim();
                    ob.party_invoic_dt = ds.Tables[0].Rows[0]["party_invoic_dt"].ToString().Trim();
                    ob.vendor_type = ds.Tables[0].Rows[0]["vendor_type"].ToString().Trim();
                    ob.TaggedDate = ds.Tables[0].Rows[0]["TaggedDate"].ToString().Trim();
                    ob.InvoiceCreatedFromDoc = ds.Tables[0].Rows[0]["InvoiceCreatedFromDoc"].ToString().Trim();
                    ob.SupplyState = ds.Tables[0].Rows[0]["SupplyState"].ToString().Trim();

                    _pnv_header_Details = (from DataRow dr in ds.Tables[1].Rows
                                           select new classProductDetails()
                                           {
                                               InvoiceDetails_Id = Convert.ToString(dr["InvoiceDetails_Id"]),
                                               InvoceProductDescriptiopn = Convert.ToString(dr["InvoceProductDescriptiopn"]),
                                               DetailsQuantity = Convert.ToString(dr["DetailsQuantity"]),
                                               Unit = Convert.ToString(dr["Unit"]),
                                               PurchasePrice = Convert.ToString(dr["PurchasePrice"]),
                                               DiscountAmount = Convert.ToString(dr["DiscountAmount"]),
                                               DetailsAmount = Convert.ToString(dr["DetailsAmount"]),
                                               TaxAmount = Convert.ToString(dr["TaxAmount"]),
                                               AmountBaseCurrency = Convert.ToString(dr["AmountBaseCurrency"])
                                           }).ToList();

                    if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        _CalculatedAmount.ProductAmount = Convert.ToString(ds.Tables[2].Rows[0]["ProductAmount"]);
                        _CalculatedAmount.ProductTotalAmount = Convert.ToString(ds.Tables[2].Rows[0]["ProductTotalAmount"]);
                        _CalculatedAmount.NetAmount = Convert.ToString(ds.Tables[2].Rows[0]["NetAmount"]);
                        _CalculatedAmount.ChargesAmount = Convert.ToString(ds.Tables[2].Rows[0]["ChargesAmount"]);
                        _CalculatedAmount.ProductTaxAmount = Convert.ToString(ds.Tables[2].Rows[0]["ProductTaxAmount"]);
                        _CalculatedAmount.ProductQuantity = Convert.ToString(ds.Tables[2].Rows[0]["ProductQuantity"]);
                    }
                    else
                    {
                        _CalculatedAmount.ProductAmount = "0.00";
                        _CalculatedAmount.ProductTotalAmount = "0.00";
                        _CalculatedAmount.NetAmount = "0.00";
                        _CalculatedAmount.ChargesAmount = "0.00";
                        _CalculatedAmount.ProductTaxAmount = "0.00";
                        _CalculatedAmount.ProductQuantity = "0.00";
                    }

                    _pnv_address_Details = (from DataRow dr in ds.Tables[3].Rows
                                            select new classAddressDetails()
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
                                               GSTIN = Convert.ToString(dr["GSTIN"])
                                           }).ToList();

                     _pnv_producttax_Details = (from DataRow dr in ds.Tables[4].Rows
                                            select new classProductTaxDetails()
                                           {
                                               InvoiceDetails_Id = Convert.ToString(dr["InvoiceDetails_Id"]),
                                               Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                               Taxes_Name = Convert.ToString(dr["Taxes_Name"]),
                                               Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                               Percentage = Convert.ToString(dr["Percentage"]),
                                               Amount = Convert.ToString(dr["Amount"])
                                           }).ToList();

                     _pnv_invoicetax_Details = (from DataRow dr in ds.Tables[5].Rows
                                                select new classProductTaxDetails()
                                                {
                                                    InvoiceDetails_Id = Convert.ToString(dr["InvoiceDetails_Id"]),
                                                    Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                                    Taxes_Name = Convert.ToString(dr["Taxes_Name"]),
                                                    Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                                    Percentage = Convert.ToString(dr["Percentage"]),
                                                    Amount = Convert.ToString(dr["Amount"])
                                                }).ToList();
                }

                return_obj.HeaderDetails = ob;
                return_obj.ProductDetails = _pnv_header_Details;
                return_obj.SummaryDetails = _CalculatedAmount;
                return_obj.AddressDetails = _pnv_address_Details;
                return_obj.ProductTaxDetails = _pnv_producttax_Details;
                return_obj.InvoiceTaxDetails = _pnv_invoicetax_Details;
                return_obj.msg = "ok";
            }
            catch (Exception ex)
            {
                return_obj.msg = ex.Message;
            }
            return return_obj;

        }
    }

    public class classHeaderDetails
    {
        public string InvoiceNumber { get; set; }
        public string BranchDescription { get; set; }
        public string invoice_date { get; set; }
        public string VendorName { get; set; }
        public string PartyInvoiceNo { get; set; }
        public string Invoice_Reference { get; set; }
        public string contact_name { get; set; }
        public string Currency_AlphaCode { get; set; }
        public string Currency_Conversion_Rate { get; set; }
        public string Invoice_ReverseMechanism { get; set; }
        public string Amounts_are { get; set; }
        public string Entry_date { get; set; }
        public string Invoice_Remarks { get; set; }
        public string EWayBillNumber { get; set; }
        public string value { get; set; }
        public string billing_address { get; set; }
        public string shipping_address { get; set; }
        public string grn_no { get; set; }
        public string invoice_type { get; set; }
        public string party_invoic_dt { get; set; }
        public string vendor_type { get; set; }
        public string TaggedDate { get; set; }
        public string InvoiceCreatedFromDoc { get; set; }
        public string SupplyState { get; set; }
    }
    public class classProductDetails
    {
        public string InvoiceDetails_Id { get; set; }
        public string InvoceProductDescriptiopn { get; set; }
        public string DetailsQuantity { get; set; }
        public string Unit { get; set; }
        public string PurchasePrice { get; set; }
        public string DiscountAmount { get; set; }
        public string DetailsAmount { get; set; }
        public string TaxAmount { get; set; }
        public string AmountBaseCurrency { get; set; }
    }
    public class classSummaryDetails
    {
        public string ProductAmount { get; set; }
        public string ProductTotalAmount { get; set; }
        public string NetAmount { get; set; }
        public string ChargesAmount { get; set; }
        public string ProductTaxAmount { get; set; }
        public string ProductQuantity { get; set; }
    }
    public class classAddressDetails
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
    public class classProductTaxDetails
    {
        public string InvoiceDetails_Id { get; set; }
        public string Taxes_Code { get; set; }	
        public string Taxes_Name { get; set; }
        public string Taxes_ApplicableOn { get; set; }
        public string Percentage { get; set; }
        public string Amount { get; set; }
    }								
    public class PurchaseInvoiceDetails
    {
        public string msg { get; set; }
        public classHeaderDetails HeaderDetails { get; set; }
        public List<classProductDetails> ProductDetails { get; set; }
        public classSummaryDetails SummaryDetails { get; set; }
        public List<classAddressDetails> AddressDetails { get; set; }
        public List<classProductTaxDetails> ProductTaxDetails { get; set; }
        public List<classProductTaxDetails> InvoiceTaxDetails { get; set; }
    }
}
