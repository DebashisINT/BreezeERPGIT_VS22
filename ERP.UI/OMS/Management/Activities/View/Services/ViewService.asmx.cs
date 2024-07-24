//*********************************************************************************************************
//  Rev 1.0      Sanchita      V2.0.40   17-10-2023      New Fields required in Sales Quotation - RFQ Number, RFQ Date, Project / Site
//                                                       Mantis: 26871
//  Rev 2.0      Sanchita      V2.0.40   18-10-2023      Few Fields required in the Sales Quotation Entry Module for the Purpose of Quotation Print from ERP. Mantis: 26868
//  Rev 3.0      Priti         V2.0.44	 24-07-2024	     0027624: Send mail check box is not showing in the modify mode or in view mode of Sales Invoice.
// **********************************************************************************************************
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
    public class ViewService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        //Tanmoy
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
                                 ShippingAddress = Convert.ToString(dr["ShippingAddress"]),
                                 tot_taxable_amount = Convert.ToString(dr["tot_taxable_amount"]),
                                 tot_taxamount = Convert.ToString(dr["tot_taxamount"]),
                                 tot_amount = Convert.ToString(dr["tot_amount"]),
                                 Vehicle_No = Convert.ToString(dr["Vehicle_No"]),
                                 total_main_qty = Convert.ToString(dr["total_main_qty"]),
                                 total_alt_qty = Convert.ToString(dr["total_alt_qty"]),

                                 //,InvoiceDiscount = Convert.ToString(dr["InvoiceDiscount"])

                                 BillingfromAddress = Convert.ToString(dr["BILLfromADDRESS"]),
                                 ShippingFromAddress = Convert.ToString(dr["DespatchFromADDRESS"])
                                 // Rev 1.0
                                 ,RFQNumber = Convert.ToString(dr["RFQNumber"]),
                                 RFQDate = Convert.ToString(dr["RFQDate"]),
                                 ProjectSite = Convert.ToString(dr["ProjectSite"]),
                                 ShowRFQ = Convert.ToString(dr["ShowRFQ"]),
                                 ShowProjectSite = Convert.ToString(dr["ShowProjectSite"]),
                                 // End of Rev 1.0

                                 // Rev 3.0
                                 IsMailSend = Convert.ToBoolean(dr["IsMailSend"]),
                                 // Rev 3.0 End

                             }).FirstOrDefault();


                Pdetail = (from DataRow dr in products.Rows
                           select new ProductDetails()
                           {
                               SL = Convert.ToInt64(dr["SL"]),
                               DetailsId = Convert.ToInt64(dr["DetailsId"]),
                               ProductName = Convert.ToString(dr["ProductName"]),
                               Productid = Convert.ToInt64(dr["Productid"]),
                               Quantity = Convert.ToString(dr["Quantity"]),
                               UOM = Convert.ToString(dr["UOM"]),
                               Price = Convert.ToString(dr["Price"]),
                               Amount = Convert.ToString(dr["Amount"]),
                               Charges = Convert.ToString(dr["Charges"]),
                               NetAmount = Convert.ToString(dr["NetAmount"]),
                               UOM2 = Convert.ToString(dr["UOM2"]),
                               QTY2 = Convert.ToString(dr["Qty2"])

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


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetSalesInvoiceDtlsForReports(string InvoiceId)
        {
            SaleInvoiceView SlInv = new SaleInvoiceView();
            InvoiceDetails InDetails = new InvoiceDetails();
            List<ProductDetails> Pdetail = new List<ProductDetails>();
            List<classInvoiceProductTaxDetails> _SalesInvoice_producttax_Details = new List<classInvoiceProductTaxDetails>();
            List<classInvoiceProductTaxDetails> _SalesInvoice_tax_Details = new List<classInvoiceProductTaxDetails>();
            List<SalesINvoiceclassAddressDetails> _Slnv_address_Details = new List<SalesINvoiceclassAddressDetails>();


            if (HttpContext.Current.Session["userid"] != null)
            {
                try
                {
                    ProcedureExecute proc = new ProcedureExecute("Prc_ViewDetails");
                    proc.AddVarcharPara("@Action", 50, "SalesInvoice_Details");
                    proc.AddVarcharPara("@InvoiceId", 50, InvoiceId);
                    DataSet ds = proc.GetDataSet();

                    //DataTable dt = ds.Tables[0];
                    DataTable products = ds.Tables[1];

                    InDetails.Id = Convert.ToInt64(ds.Tables[0].Rows[0]["Id"].ToString().Trim());
                    InDetails.InvoiceType = ds.Tables[0].Rows[0]["inventory_type"].ToString().Trim();
                    InDetails.InvoiceNumber = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString().Trim();
                    InDetails.InvoiceDate = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceDate"]);
                    InDetails.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString().Trim();
                    InDetails.BranchDescription = ds.Tables[0].Rows[0]["branch_description"].ToString().Trim();
                    InDetails.SalesmanName = ds.Tables[0].Rows[0]["SalesmanName"].ToString().Trim();
                    InDetails.ContactPerson = ds.Tables[0].Rows[0]["cp_name"].ToString().Trim();
                    InDetails.TaggedTxt = ds.Tables[0].Rows[0]["InvoiceCreatedFromDoc"].ToString().Trim();
                    InDetails.txtTaggedNumber = ds.Tables[0].Rows[0]["txtTaggedNumber"].ToString().Trim();
                    InDetails.PI_Quotation_OrderDate = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceCreatedFromDocDate"]);
                    InDetails.SalesmanName = ds.Tables[0].Rows[0]["SalesmanName"].ToString().Trim();
                    InDetails.CashBank = ds.Tables[0].Rows[0]["CashBank_Code"].ToString().Trim();
                    InDetails.CreditDays = ds.Tables[0].Rows[0]["CreditDays"].ToString().Trim();
                    InDetails.DueDate = Convert.ToString(ds.Tables[0].Rows[0]["DueDate"]);
                    InDetails.Currency = ds.Tables[0].Rows[0]["CurrencyName"].ToString().Trim();
                    InDetails.Rates = ds.Tables[0].Rows[0]["Rate"].ToString().Trim();
                    InDetails.AmountsAre = ds.Tables[0].Rows[0]["Amountsare"].ToString().Trim();
                    InDetails.Remarks = ds.Tables[0].Rows[0]["Remarks"].ToString().Trim();
                    InDetails.PlaceOfSupply = ds.Tables[0].Rows[0]["SupplyState"].ToString().Trim();
                    InDetails.Reference = ds.Tables[0].Rows[0]["Reference"].ToString().Trim();


                    Pdetail = (from DataRow dr in products.Rows
                               select new ProductDetails()
                               {
                                   DetailsId = Convert.ToInt64(dr["DetailsId"]),
                                   ProductName = Convert.ToString(dr["ProductName"]),
                                   Quantity = Convert.ToString(dr["Quantity"]),
                                   UOM = Convert.ToString(dr["UOM"]),
                                   Price = Convert.ToString(dr["Price"]),
                                   Discount = Convert.ToString(dr["Discount"]),
                                   Amount = Convert.ToString(dr["Amount"]),
                                   Charges = Convert.ToString(dr["Charges"]),
                                   NetAmount = Convert.ToString(dr["NetAmount"])

                               }).ToList();

                    _SalesInvoice_producttax_Details = (from DataRow dr in ds.Tables[2].Rows
                                                        select new classInvoiceProductTaxDetails()
                                                        {
                                                            InvoiceDetails_Id = Convert.ToString(dr["InvoiceDetails_Id"]),
                                                            Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                                            Taxes_Name = Convert.ToString(dr["Taxes_Name"]),
                                                            Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                                            Percentage = Convert.ToString(dr["Percentage"]),
                                                            Amount = Convert.ToString(dr["Amount"])
                                                        }).ToList();

                    _SalesInvoice_tax_Details = (from DataRow dr in ds.Tables[3].Rows
                                                 select new classInvoiceProductTaxDetails()
                                                 {
                                                     InvoiceDetails_Id = Convert.ToString(dr["InvoiceDetails_Id"]),
                                                     Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                                     Taxes_Name = Convert.ToString(dr["Taxes_Name"]),
                                                     Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                                     Percentage = Convert.ToString(dr["Percentage"]),
                                                     Amount = Convert.ToString(dr["Amount"])
                                                 }).ToList();

                    _Slnv_address_Details = (from DataRow dr in ds.Tables[4].Rows
                                             select new SalesINvoiceclassAddressDetails()
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



                    SlInv.HeaderDetails = InDetails;
                    SlInv.ProDetails = Pdetail;
                    SlInv.InvoiceProductTaxDetails = _SalesInvoice_producttax_Details;
                    SlInv.InvoiceTaxDetails = _SalesInvoice_tax_Details;
                    SlInv.AddressDetails = _Slnv_address_Details;
                    SlInv.msg = "ok";


                }
                catch (Exception ex)
                {
                    SlInv.msg = ex.Message;
                }

            }

            return SlInv;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetwarehouseDetails(string InvoiceId, Int64 Details_Id)
        {
            SaleInvoiceView SlInv = new SaleInvoiceView();
            warehouseDetails whouse = new warehouseDetails();
            List<warehouseDetails> wareHouse = new List<warehouseDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_ViewDetails");
                proc.AddVarcharPara("@action", 50, "ProductWiseWarehouseDetails");
                proc.AddVarcharPara("@InvoiceId", 50, InvoiceId);
                proc.AddBigIntegerPara("@Details_Id", Details_Id);
                DataSet ds = proc.GetDataSet();
                DataTable WHDEtails = ds.Tables[0];

                wareHouse = (from DataRow dr in WHDEtails.Rows
                             select new warehouseDetails()
                             {
                                 WarehouseID = Convert.ToInt32(dr["WarehouseID"]),
                                 WarehouseName = Convert.ToString(dr["WarehouseName"]),
                                 OUT_Quantity = Convert.ToString(dr["OUT_Quantity"]),
                                 Alt_StockOut = Convert.ToString(dr["Alt_StockOut"]),

                             }).ToList();

                SlInv.warehouseDetails = wareHouse;
                return SlInv;

            }
            return null;
        }

        // Mantis Issue 25129
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMultipleUOMDetails(string InvoiceId, Int64 Details_Id)
        {
            SaleInvoiceView SlInv = new SaleInvoiceView();
            multiuomdetails muomdet = new multiuomdetails();
            List<multiuomdetails> MultUOMDet = new List<multiuomdetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_ViewDetails");
                proc.AddVarcharPara("@action", 50, "ProductMultiUOMDetails");
                proc.AddVarcharPara("@InvoiceId", 50, InvoiceId);
                proc.AddBigIntegerPara("@Details_Id", Details_Id);
                DataSet ds = proc.GetDataSet();
                DataTable MUOMDEtails = ds.Tables[0];

                MultUOMDet = (from DataRow dr in MUOMDEtails.Rows
                           select new multiuomdetails()
                             {
                                 SrlNo = Convert.ToInt64(dr["SrlNo"]),
                                 Quantity = Convert.ToDecimal(dr["Quantity"]),
                                 UOM = Convert.ToString(dr["UOM"]),
                                 AltUOM = Convert.ToString(dr["AltUOM"]),
                                 AltQuantity = Convert.ToDecimal(dr["AltQuantity"]),
                                 UomId = Convert.ToInt64(dr["UomId"]),
                                 AltUomId = Convert.ToInt64(dr["AltUomId"]),
                                 ProductId = Convert.ToInt64(dr["ProductId"]),
                                 DetailsId = Convert.ToInt64(dr["DetailsId"]),
                                 BaseRate = Convert.ToDecimal(dr["BaseRate"]),
                                 AltRate = Convert.ToDecimal(dr["AltRate"]),
                                 UpdateRow = Convert.ToBoolean(dr["UpdateRow"]),
                                 MultiUOMSR = Convert.ToInt64(dr["MultiUOMSR"]),

                             }).ToList();

                SlInv.multiuomdetails = MultUOMDet;
                return SlInv;

            }
            return null;
        }
        // End of Mantis Issue 25129

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInvoiceTYransporterDetails(Int64 InvoiceId)
        {
            InvoiceTRansporterDetails SlInv = new InvoiceTRansporterDetails();
           
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_ViewDetails");
                proc.AddVarcharPara("@ACTION", 250, "TransporterDetails");
                proc.AddBigIntegerPara("@DETAILS_ID", InvoiceId);
                dt = proc.GetTable();

                if (dt.Rows.Count > 0 && dt != null)
                {
                    SlInv.Address = Convert.ToString(dt.Rows[0]["Address"]);
                    SlInv.Area = Convert.ToString(dt.Rows[0]["Area"]);
                    SlInv.City = Convert.ToString(dt.Rows[0]["City"]);
                    SlInv.Country = Convert.ToString(dt.Rows[0]["Country"]);
                    SlInv.DistanceofDelivery = Convert.ToString(dt.Rows[0]["DistanceofDelivery"]);
                    SlInv.FinalTransporter = Convert.ToString(dt.Rows[0]["FinalTransporter"]);
                    SlInv.Freight = Convert.ToString(dt.Rows[0]["Freight"]);
                    SlInv.GSTIN = Convert.ToString(dt.Rows[0]["GSTIN"]);
                    SlInv.Loading = Convert.ToString(dt.Rows[0]["Loading"]);
                    SlInv.LRDate = Convert.ToString(dt.Rows[0]["LRDate"]);
                    SlInv.LRNO = Convert.ToString(dt.Rows[0]["LRNO"]);
                    SlInv.OtherCharges = Convert.ToString(dt.Rows[0]["OtherCharges"]);
                    SlInv.Parking = Convert.ToString(dt.Rows[0]["Parking"]);
                    SlInv.Phone = Convert.ToString(dt.Rows[0]["Phone"]);
                    SlInv.Pin = Convert.ToString(dt.Rows[0]["Pin"]);
                    SlInv.Point = Convert.ToString(dt.Rows[0]["Point"]);
                    SlInv.Registered = Convert.ToString(dt.Rows[0]["Registered"]);
                    SlInv.State = Convert.ToString(dt.Rows[0]["State"]);
                    SlInv.TollTax = Convert.ToString(dt.Rows[0]["TollTax"]);
                    SlInv.TotalCharges = Convert.ToString(dt.Rows[0]["TotalCharges"]);
                    SlInv.TransportationMode = Convert.ToString(dt.Rows[0]["TransportationMode"]);
                    SlInv.TransporterName = Convert.ToString(dt.Rows[0]["TransporterName"]);
                    SlInv.TransporterType = Convert.ToString(dt.Rows[0]["TransporterType"]);
                    SlInv.Unloading = Convert.ToString(dt.Rows[0]["Unloading"]);
                    SlInv.VehicleNo = Convert.ToString(dt.Rows[0]["VehicleNo"]);
                    SlInv.VehicleOutDate = Convert.ToString(dt.Rows[0]["VehicleOutDate"]);
                    SlInv.VehicleType = Convert.ToString(dt.Rows[0]["VehicleType"]);
                    SlInv.WeighmentCharges = Convert.ToString(dt.Rows[0]["WeighmentCharges"]);
                }

            }
            return SlInv;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInvoiceTermsDetails(Int64 InvoiceId)
        {
            InvoiceTermsDetails SlInv = new InvoiceTermsDetails();

            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_ViewDetails");
                proc.AddVarcharPara("@ACTION", 250, "TermsConditionDetails");
                proc.AddVarcharPara("@INVOICEID",100, Convert.ToString(InvoiceId));
                dt = proc.GetTable();

                if (dt.Rows.Count > 0 && dt != null)
                {
                    SlInv.DeliveryScheduleDate = Convert.ToString(dt.Rows[0]["DeliveryScheduleDate"]);
                    SlInv.AccountNumber = Convert.ToString(dt.Rows[0]["AccountNumber"]);
                    SlInv.BankBranchAddress = Convert.ToString(dt.Rows[0]["BankBranchAddress"]);
                    SlInv.BankBranchLandmark = Convert.ToString(dt.Rows[0]["BankBranchLandmark"]);
                    SlInv.BankBranchName = Convert.ToString(dt.Rows[0]["BankBranchName"]);
                    SlInv.BankBranchPin = Convert.ToString(dt.Rows[0]["BankBranchPin"]);
                    SlInv.BankName = Convert.ToString(dt.Rows[0]["BankName"]);
                    SlInv.DeliveryDetails = Convert.ToString(dt.Rows[0]["DeliveryDetails"]);
                    SlInv.DeliveryRemarks = Convert.ToString(dt.Rows[0]["DeliveryRemarks"]);
                    SlInv.EPermitPermit = Convert.ToString(dt.Rows[0]["EPermitPermit"]);
                    SlInv.FreightCharges = Convert.ToString(dt.Rows[0]["FreightCharges"]);
                    SlInv.FreightRemark = Convert.ToString(dt.Rows[0]["FreightRemark"]);
                    SlInv.IFSCCode = Convert.ToString(dt.Rows[0]["IFSCCode"]);
                    SlInv.InsuranceCoverage = Convert.ToString(dt.Rows[0]["InsuranceCoverage"]);
                    SlInv.OtherRemarks = Convert.ToString(dt.Rows[0]["OtherRemarks"]);
                    SlInv.PaymentTerms = Convert.ToString(dt.Rows[0]["PaymentTerms"]);
                    SlInv.Remarks = Convert.ToString(dt.Rows[0]["Remarks"]);
                    SlInv.RTGS = Convert.ToString(dt.Rows[0]["RTGS"]);
                    SlInv.SWIFTCode = Convert.ToString(dt.Rows[0]["SWIFTCode"]);
                    SlInv.TestCertificateRequired = Convert.ToString(dt.Rows[0]["TestCertificateRequired"]);
                   //Rev work start 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                    SlInv.Project = Convert.ToString(dt.Rows[0]["Project"]);
                    //Rev work close 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                }

            }
            return SlInv;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetHeaderTCSDetails(Int64 InvoiceId)
        {
            HeaderTCSDetails HeaderTCSDetails=new HeaderTCSDetails();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_TCSDetailsForInvoiceView");
                proc.AddVarcharPara("@Action", 250, "ShowTDSDetails");
                proc.AddVarcharPara("@invoice_id", 100, Convert.ToString(InvoiceId));
                dt = proc.GetTable();
               
                if (dt.Rows.Count > 0 && dt != null)
                {
                    HeaderTCSDetails.TCSSection = Convert.ToString(dt.Rows[0]["Code"]);
                    HeaderTCSDetails.TCSApplicableAmount = Convert.ToString(dt.Rows[0]["tds_amount"]);
                    HeaderTCSDetails.TCSPercentage = Convert.ToString(dt.Rows[0]["Rate"]);
                    HeaderTCSDetails.TCSAmount = Convert.ToString(dt.Rows[0]["Amount"]);

                }
            }
            return HeaderTCSDetails;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTCSDetails(Int64 InvoiceId)
        {
            InvoiceTermsDetails SlInv = new InvoiceTermsDetails();

            if (HttpContext.Current.Session["userid"] != null)
            {
                
                DataTable ds = new DataTable();
                TCSDetailsView TCSDetailsView = new TCSDetailsView();
                List<TCSDetails> TCSDetailsViewlst = new List<TCSDetails>();
                ProcedureExecute pro = new ProcedureExecute("prc_TCSDetailsForInvoiceView");
                pro.AddVarcharPara("@Action", 500, "ShowTDSList");
                pro.AddVarcharPara("@invoice_id", 500, Convert.ToString(InvoiceId));
                pro.AddVarcharPara("@module_name", 500, "SI");
                pro.AddVarcharPara("@AddOrEdit", 500, "Edit");
                ds = pro.GetTable();

            

                if (ds.Rows.Count > 0 && ds != null)
                {
                    TCSDetailsViewlst = (from DataRow dr in ds.Rows
                                         select new TCSDetails()
                                 {
                                     SLNO = Convert.ToInt64(dr["SLNO"]),
                                     Invoice_Number = Convert.ToString(dr["Invoice_Number"]),
                                     branch_description = Convert.ToString(dr["branch_description"]),
                                     Doc_Type = Convert.ToString(dr["Doc_Type"]),
                                     Invoice_Date = Convert.ToString(dr["Invoice_Date"]),
                                     TaxableAmount = Convert.ToString(dr["TaxableAmount"]),
                                     NetAmount = Convert.ToString(dr["NetAmount"]),
                                     TaxableRunning = Convert.ToString(dr["TaxableRunning"]),
                                     NetRunning = Convert.ToString(dr["NetRunning"])

                                 }).ToList();
                    TCSDetailsView.TCSDetails = TCSDetailsViewlst;
                    return TCSDetailsView;
                }

            }
            return null;
        }

        // Rev 2.0
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInvoiceOtherDetails(Int64 InvoiceId)
        {
            InvoiceOtherConditionDetails SlInv = new InvoiceOtherConditionDetails();

            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable DT = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_OtherConditionAddEdit");
                proc.AddVarcharPara("@Action", 500, "GetTOCTagDetail");
                proc.AddVarcharPara("@DocId", 500, Convert.ToString(InvoiceId));
                proc.AddVarcharPara("@DocType", 500, "SI");
                DT = proc.GetTable();

                if (DT != null && DT.Rows.Count > 0)
                {

                    SlInv.strPriceBasis = Convert.ToString(DT.Rows[0]["PriceBasis"]);
                    SlInv.strLoadingCharges = Convert.ToString(DT.Rows[0]["LoadingCharges"]);
                    SlInv.strDetentionCharges = Convert.ToString(DT.Rows[0]["DetentionCharges"]);
                    SlInv.strDeliveryPeriod = Convert.ToString(DT.Rows[0]["DeliveryPeriod"]);
                    SlInv.strInspection = Convert.ToString(DT.Rows[0]["Inspection"]);
                    SlInv.strPaymentTermsOther = Convert.ToString(DT.Rows[0]["PaymentTerms"]);
                    SlInv.OfferValidUpto = Convert.ToString(DT.Rows[0]["dtOfferValidUpto"]);
                    SlInv.strQuantityTol = Convert.ToString(DT.Rows[0]["QuantityTol"]);
                    SlInv.strDimensionalTol = Convert.ToString(DT.Rows[0]["DimensionalTol"]);
                    SlInv.strThicknessTol = Convert.ToString(DT.Rows[0]["ThicknessTol"]);
                    SlInv.strWarranty = Convert.ToString(DT.Rows[0]["Warranty"]);
                    SlInv.strDeviation = Convert.ToString(DT.Rows[0]["Deviation"]);
                    SlInv.strLDClause = Convert.ToString(DT.Rows[0]["LDClause"]);
                    SlInv.strInterestClause = Convert.ToString(DT.Rows[0]["InterestClause"]);
                    SlInv.strPriceEscalationClause = Convert.ToString(DT.Rows[0]["PriceEscalationClause"]);
                    SlInv.strInternalCoating = Convert.ToString(DT.Rows[0]["InternalCoating"]);
                    SlInv.strExternalCoating = Convert.ToString(DT.Rows[0]["ExternalCoating"]);
                    SlInv.strSpecialNote = Convert.ToString(DT.Rows[0]["SpecialNote"]);

                }
                else
                {
                    SlInv.strPriceBasis = "";
                    SlInv.strLoadingCharges = "";
                    SlInv.strDetentionCharges = "";
                    SlInv.strDeliveryPeriod = "";
                    SlInv.strInspection = "";
                    SlInv.strPaymentTermsOther = "";
                    SlInv.OfferValidUpto = "";
                    SlInv.strQuantityTol = "";
                    SlInv.strDimensionalTol = "";
                    SlInv.strThicknessTol = "";
                    SlInv.strWarranty = "";
                    SlInv.strDeviation = "";
                    SlInv.strLDClause = "";
                    SlInv.strInterestClause = "";
                    SlInv.strPriceEscalationClause = "";
                    SlInv.strInternalCoating = "";
                    SlInv.strExternalCoating = "";
                    SlInv.strSpecialNote = "";
                }

            }
            return SlInv;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetSystemSettings_OtherCondition(Int64 InvoiceId)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            string IsVisible = "No";

            DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Other_Condition' AND IsActive=1");

            if (DT != null && DT.Rows.Count > 0)
            {
                IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
            }

            return IsVisible;
        }
        // End of Rev 2.0

    }



    public class InvoiceTRansporterDetails
    {

        public Int64 InvoiceId {get;set;}
        public string TransporterName { get; set; }
        public string FinalTransporter { get; set; }
        public string TransporterType { get; set; }
        public string Registered { get; set; }
        public string VehicleNo { get; set; }
        public string GSTIN { get; set; }
        public string Freight { get; set; }
        public string Point { get; set; }
        public string Loading { get; set; }

        public string Unloading { get; set; }
        public string Parking { get; set; }
        public string WeighmentCharges { get; set; }

        public string TollTax { get; set; }

        public string OtherCharges { get; set; }

        public string TotalCharges { get; set; }

        public string DistanceofDelivery { get; set; }
        public string LRNO { get; set; }
        public string LRDate { get; set; }
        public string VehicleOutDate { get; set; }
        public string VehicleType { get; set; }
        public string TransportationMode { get; set; }

        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        public string City { get; set; }
        public string Pin { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
      
    }

    public class TCSDetailsView
    {
        public List<TCSDetails> TCSDetails { get; set; }
    }

        public class HeaderTCSDetails
    {
        
        public string TCSSection { get; set; }
        public string TCSApplicableAmount { get; set; }
        public string TCSPercentage { get; set; }
        public string TCSAmount { get; set; }
        
    }
    public class TCSDetails
    {
        public Int64 SLNO { get; set; }
        public string Invoice_Number { get; set; }
        public string branch_description { get; set; }
        public string Doc_Type { get; set; }
        public string Invoice_Date { get; set; }
        public string TaxableAmount { get; set; }
        public string NetAmount { get; set; }
        public string TaxableRunning { get; set; }
        public string NetRunning { get; set; }
    }

    public class InvoiceTermsDetails
    {

        public Int64 InvoiceId { get; set; }
        
      public string DeliveryScheduleDate { get; set; }
       public string    DeliveryRemarks { get; set; }
      public string  InsuranceCoverage { get; set; }
       public string  FreightCharges { get;  set; }
       public string  FreightRemark { get; set; }
      public string  EPermitPermit { get; set; }
        public string  OtherRemarks { get; set; }
       public string  TestCertificateRequired { get; set; }
      public string  DeliveryDetails { get; set; }
     public string  PaymentTerms { get; set; }
     public string   BankName { get; set; }
      public string  BankBranchName { get; set; }
       public string  BankBranchAddress { get; set; }
     public string  BankBranchLandmark { get; set; }
     public string  BankBranchPin { get; set; }
     public string  AccountNumber { get; set; }
    public string  SWIFTCode { get; set; }
    public string  RTGS { get; set; }
     public string  IFSCCode { get; set; }
    public string  Remarks { get; set; }
        //Rev work start 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"   
    public string Project { get; set; }         
        //Rev work close 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
    }


    public class InvoiceDetails
    {
        public Int64 Id { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string BranchDescription { get; set; }
        public string DelivaryDate { get; set; }
        public string CustomerName { get; set; }

        public string SalesmanName { get; set; }
        public string ContactPerson { get; set; }
        public string TaggedTxt { get; set; }

        public string PI_Quotation_OrderDate { get; set; }

        public string txtTaggedNumber { get; set; }

        public string CashBank { get; set; }

        public string CreditDays { get; set; }
        public string DueDate { get; set; }
        public string Currency { get; set; }
        public string Rates { get; set; }
        public string AmountsAre { get; set; }
        public string Reference { get; set; }

        public string PlaceOfSupply { get; set; }
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
        public string tot_taxable_amount { get; set; }
        public string tot_taxamount { get; set; }
        public string tot_amount { get; set; }
        public string Vehicle_No { get; set; }
        //public String InvoiceDiscount { get; set; }
        public string total_main_qty { get; set; }
        public string total_alt_qty { get; set; }

        public string BillingfromAddress { get; set; }
        public string ShippingFromAddress { get; set; }
        
        // Rev 1.0
        public string RFQNumber { get; set; }
        public string RFQDate { get; set; }
        public string ProjectSite { get; set; }
        public string ShowRFQ { get; set; }
        public string ShowProjectSite { get; set; }

        // End of Rev 1.0

        // Rev 3.0
        public bool IsMailSend { get; set; }
        // Rev 3.0 End

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
        public List<classInvoiceProductTaxDetails> InvoiceProductTaxDetails { get; set; }
        public List<classInvoiceProductTaxDetails> InvoiceTaxDetails { get; set; }
        public List<SalesINvoiceclassAddressDetails> AddressDetails { get; set; }
        public List<warehouseDetails> warehouseDetails { get; set; }
        public string msg { get; set; }

        // Mantis Issue 25129
        public List<multiuomdetails> multiuomdetails { get; set; }
        // End of Mantis Issue 25129
    }


    public class ProductDetails
    {
        public Int64 SL { get; set; }
        public Int64 DetailsId { get; set; }
        public Int64 Productid { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public string Discount { get; set; }
        public string Charges { get; set; }
        public string NetAmount { get; set; }
        public string UOM2 { get; set; }
        public string QTY2 { get; set; }

    }

    public class SalesINvoiceclassAddressDetails
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

    public class classInvoiceProductTaxDetails
    {
        public string InvoiceDetails_Id { get; set; }
        public string Taxes_Code { get; set; }
        public string Taxes_Name { get; set; }
        public string Taxes_ApplicableOn { get; set; }
        public string Percentage { get; set; }
        public string Amount { get; set; }
    }
    public class warehouseDetails
    {
        public Int64 Detailsid { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string OUT_Quantity { get; set; }
        public string Alt_StockOut { get; set; }

    }

    // Mantis Issue 25129
    public class multiuomdetails
    {
        public Int64 SrlNo { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public string AltUOM { get; set; }
        public decimal AltQuantity { get; set; }
        public Int64 UomId { get; set; }
        public Int64 AltUomId { get; set; }
        public Int64 ProductId { get; set; }
        public Int64 DetailsId { get; set; }
        public decimal BaseRate { get; set; }
        public decimal AltRate { get; set; }
        public Boolean UpdateRow { get; set; }
        public Int64 MultiUOMSR { get; set; }
    }
    // End of Mantis Issue 25129

    // Rev 2.0
    public class InvoiceOtherConditionDetails
    {
        public string strPriceBasis {get;set; }
        public string strLoadingCharges { get; set; }
        public string strDetentionCharges { get; set; }
        public string strDeliveryPeriod { get; set; }
        public string strInspection { get; set; }
        public string strPaymentTermsOther { get; set; }
        public string OfferValidUpto { get; set; }
        public string strQuantityTol { get; set; }
        public string strDimensionalTol { get; set; }
        public string strThicknessTol { get; set; }
        public string strWarranty { get; set; }
        public string strDeviation { get; set; }
        public string strLDClause { get; set; }
        public string strInterestClause { get; set; }
        public string strPriceEscalationClause { get; set; }
        public string strInternalCoating { get; set; }
        public string strExternalCoating { get; set; }
        public string strSpecialNote { get; set; }
    }
    // End of Rev 2.0

}
