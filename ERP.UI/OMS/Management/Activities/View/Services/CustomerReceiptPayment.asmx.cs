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
    /// Summary description for CustomerReceiptPayment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CustomerReceiptPayment : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetCustomerPaymentReceiptHeaderDetails(string ReceiptPayment_ID)
        {
            CustomerPaymentReceipt CustomerPaymentReceipt = new CustomerPaymentReceipt();
            CustomerPaymentReceiptHeaderDetails HeaderDetails = new CustomerPaymentReceiptHeaderDetails();
            List<CustomerReceiptPaymentclassAddressDetails> _address_Details = new List<CustomerReceiptPaymentclassAddressDetails>();
            MultipleTypePayment PaymentDetails = new MultipleTypePayment();
            List<CustomerPaymentReceiptDetails> PaymentReceiptDetails = new List<CustomerPaymentReceiptDetails>();
            CustomerPaymentReceiptProductDetails ProductDetails = new CustomerPaymentReceiptProductDetails();

            if (HttpContext.Current.Session["userid"] != null)
            {
                try
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentViewDetails");
                    proc.AddVarcharPara("@Action", 50, "CustomerReceiptPaymentViewDetails");
                    proc.AddVarcharPara("@ReceiptPayment_ID", 50, ReceiptPayment_ID);
                    DataSet ds = proc.GetDataSet();

                    //DataTable dt = ds.Tables[0];
                    //DataTable products = ds.Tables[1];
                    //HeaderDetails.CashBankId = ds.Tables[0].Rows[0]["CashBank_ID"].ToString().Trim();
                    HeaderDetails.VoucherType = ds.Tables[0].Rows[0]["VoucherType"].ToString().Trim();
                    HeaderDetails.Unit = ds.Tables[0].Rows[0]["Unit"].ToString().Trim();
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
                    HeaderDetails.DrawOn = Convert.ToString(ds.Tables[0].Rows[0]["DrawOn"]);
                   // HeaderDetails.PaymentAmount = ds.Tables[0].Rows[0]["PaymentAmount"].ToString().Trim();
                    HeaderDetails.Contact_Person = Convert.ToString(ds.Tables[0].Rows[0]["Contact_Person"]);
                    HeaderDetails.Narration = ds.Tables[0].Rows[0]["Narration"].ToString().Trim();
                    HeaderDetails.GSTApplicable = ds.Tables[0].Rows[0]["GSTApplicable"].ToString().Trim();
                    HeaderDetails.PaymentType = ds.Tables[0].Rows[0]["PaymentType"].ToString().Trim();
                   

                    _address_Details = (from DataRow dr in ds.Tables[1].Rows
                                        select new CustomerReceiptPaymentclassAddressDetails()
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

                    if (ds.Tables[2].Rows.Count > 0 && ds.Tables[2]!=null)
                    {
                        PaymentDetails.Payment_type = ds.Tables[2].Rows[0]["Payment_type"].ToString().Trim();
                        PaymentDetails.PaymentType_details = ds.Tables[2].Rows[0]["PaymentType_details"].ToString().Trim();
                        PaymentDetails.CardType = ds.Tables[2].Rows[0]["CardType"].ToString().Trim();
                        PaymentDetails.AuthNo = Convert.ToString(ds.Tables[2].Rows[0]["AuthNo"]);
                        //HeaderDetails.CashBank = ds.Tables[0].Rows[0]["CashBankName"].ToString().Trim();
                        PaymentDetails.Payment_Remarks = ds.Tables[2].Rows[0]["Payment_Remarks"].ToString().Trim();
                        PaymentDetails.PaymentAmount = ds.Tables[2].Rows[0]["PaymentAmount"].ToString().Trim();
                        PaymentDetails.Payment_date = ds.Tables[2].Rows[0]["Payment_date"].ToString().Trim();
                        PaymentDetails.Payment_mainAccount = ds.Tables[2].Rows[0]["Payment_mainAccount"].ToString().Trim();
                    }

                    PaymentReceiptDetails = (from DataRow dr in ds.Tables[3].Rows
                                             select new CustomerPaymentReceiptDetails()
                                       {
                                           SrlNo = Convert.ToString(dr["SrlNo"]),
                                           Type = Convert.ToString(dr["Type"]),
                                           TypeId = Convert.ToString(dr["TypeId"]),
                                           DocumentNo = Convert.ToString(dr["DocumentNo"]),
                                           Receipt = Convert.ToString(dr["Receipt"]),
                                           Remarks = Convert.ToString(dr["Remarks"]),
                                           DocId = Convert.ToString(dr["DocId"]),
                                           ReceiptDetail_ID = Convert.ToString(dr["ReceiptDetail_ID"]),
                                           ActualAmount = Convert.ToString(dr["ActualAmount"]),
                                           UpdateEdit = Convert.ToString(dr["UpdateEdit"]),


                                       }).ToList();


                    if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[4].Rows[0]["productIds"])))
                    {
                        ProductDetails.productsname = Convert.ToString(ds.Tables[4].Rows[0]["productsname"]);
                        ProductDetails.productIds = Convert.ToString(ds.Tables[4].Rows[0]["productIds"]);
                        ProductDetails.TypeId = Convert.ToString(ds.Tables[4].Rows[0]["HsnCode"]);
                       
                       
                    }

         
                   



                    
                    CustomerPaymentReceipt.HeaderDetails = HeaderDetails;
                    CustomerPaymentReceipt.PaymentDetails = PaymentDetails;

                    CustomerPaymentReceipt.AddressDetails = _address_Details;
                    CustomerPaymentReceipt.PaymentReceiptDetails = PaymentReceiptDetails;
                    CustomerPaymentReceipt.ProductDetails = ProductDetails;

                    CustomerPaymentReceipt.msg = "ok";

                }
            
                catch (Exception ex)
                {
                    CustomerPaymentReceipt.msg = ex.Message;
                }

            }

            return CustomerPaymentReceipt;
        }
    }

    public class CustomerPaymentReceipt
    {
        public string msg { get; set; }
        public CustomerPaymentReceiptHeaderDetails HeaderDetails { get; set; }
        public MultipleTypePayment PaymentDetails { get; set; }

        public List<CustomerReceiptPaymentclassAddressDetails> AddressDetails { get; set; }

        public List<CustomerPaymentReceiptDetails> PaymentReceiptDetails { get; set; }
        public CustomerPaymentReceiptProductDetails ProductDetails { get; set; }
    }

        public class CustomerPaymentReceiptHeaderDetails
    {
        public string ReceiptPaymentId { get; set; }
        public string VoucherType { get; set; }
        public string Unit { get; set; }
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
        public string DrawOn {get;set;}
        public string GSTApplicable{get;set;}
        public string PaymentType { get; set; }


    }

        public class CustomerReceiptPaymentclassAddressDetails
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

    public class MultipleTypePayment
    {
        public string Payment_type { get; set; }
        public string PaymentType_details { get; set; }
        public string CardType { get; set; }
        public string AuthNo { get; set; }
        public string Payment_Remarks { get; set; }
        public string PaymentAmount { get; set; }
        public string Payment_date { get; set; }
        public string Drawee_date { get; set; }
        public string Payment_mainAccount { get; set; }
       
    }

    public class CustomerPaymentReceiptDetails
    {
        public string SrlNo { get; set; }
        public string Type { get; set; }
        public string TypeId { get; set; }
        public string DocumentNo { get; set; }
        public string Receipt { get; set; }
        public string Remarks { get; set; }
        public string DocId { get; set; }
        public string ReceiptDetail_ID { get; set; }
        public string ActualAmount { get; set; }
        public string UpdateEdit { get; set; }
        
    }

    public class CustomerPaymentReceiptProductDetails
    {
        public string productIds { get; set; }
        public string productsname { get; set; }
        public string TypeId { get; set; }
    }
}
