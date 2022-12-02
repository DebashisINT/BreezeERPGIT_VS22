using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.Services
{
    /// <summary>
    /// Summary description for CustomerReceiptPayment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class CustomerReceiptPayment : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNumberingSchemeByType(string VoucherType)
        {
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataSet AllDetailsByVoucherType = objCustRecPayBL.GetAllDropDownDataByVoucherType(VoucherType);


            DataTable numberingschemeData = AllDetailsByVoucherType.Tables[0];
            Allddl All = new Allddl();
            All.NumberingSchema = (from DataRow dr in numberingschemeData.Rows
                                   select new ddlClass()
                                   {
                                       Id = dr["Id"].ToString(),
                                       Name = dr["SchemaName"].ToString()
                                   }).ToList();




            All.Currency = (from DataRow dr in AllDetailsByVoucherType.Tables[1].Rows
                            select new ddlClass()
                            {
                                Id = dr["Id"].ToString(),
                                Name = dr["Currency_AlphaCode"].ToString()
                            }).ToList();

            All.ForBranch = (from DataRow dr in AllDetailsByVoucherType.Tables[2].Rows
                             select new ddlClass()
                             {
                                 Id = dr["Id"].ToString(),
                                 Name = dr["branch_description"].ToString()
                             }).ToList();



            if (AllDetailsByVoucherType.Tables[3] != null && AllDetailsByVoucherType.Tables[3].Rows.Count > 0)
            {
                All.SysSetting = Convert.ToString(AllDetailsByVoucherType.Tables[3].Rows[0]["Variable_Value"]);
               
            }

            if (AllDetailsByVoucherType.Tables[4] != null && AllDetailsByVoucherType.Tables[4].Rows.Count > 0)
            {
                All.SatrtDate = Convert.ToDateTime(AllDetailsByVoucherType.Tables[4].Rows[0]["finYearStartDate"]);
                All.EndDate = Convert.ToDateTime(AllDetailsByVoucherType.Tables[4].Rows[0]["finYearEndDate"]);
            }
            if (AllDetailsByVoucherType.Tables[5] != null && AllDetailsByVoucherType.Tables[5].Rows.Count > 0)
            {
                All.UDFCount = Convert.ToString(AllDetailsByVoucherType.Tables[5].Rows[0]["cnt"]);
            }

            return All;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object BindCashBankAccountJson(string userbranch)
        {
            List<ddlClass> listCB = new List<ddlClass>();
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            string CompanyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            dtCB = objCustRecPayBL.GetCustomerCashBankCRP(userbranch, CompanyId, "");
            if (dtCB.Rows.Count > 0)
            {


                listCB = (from DataRow dr in dtCB.Rows
                          select new ddlClass()
                          {
                              Name = dr["IntegrateMainAccount"].ToString(),
                              Id = dr["MainAccount_ReferenceID"].ToString()
                          }).ToList();

            }

            return listCB;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetContactPerson(string CustomerId)
        {
            List<ddlClass> listCB = new List<ddlClass>();
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            string CompanyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            dtCB = objCustRecPayBL.PopulateContactPerson(CustomerId);
            if (dtCB.Rows.Count > 0)
            {


                listCB = (from DataRow dr in dtCB.Rows
                          select new ddlClass()
                          {
                              Id = dr["Id"].ToString(),
                              Name = dr["ContactPerson"].ToString()
                          }).ToList();

            }

            return listCB;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetProformaInvoice(string CustomerId, string AsOnDate)
        {
            List<ddlClass> listCB = new List<ddlClass>();
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            string CompanyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            dtCB = objCustRecPayBL.PopulateProformaInvoice(CustomerId, AsOnDate);
            if (dtCB.Rows.Count > 0)
            {


                listCB = (from DataRow dr in dtCB.Rows
                          select new ddlClass()
                          {
                              Id = dr["Id"].ToString(),
                              Name = dr["Quote_Number"].ToString()
                          }).ToList();

            }

            return listCB;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetAllDocument(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate)
        {
            List<AllDocumentSegment> listCB = new List<AllDocumentSegment>();
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            dtCB = objCustRecPayBL.GetAllDocument(VoucherType, CustomerId, BranchId, ReceiptPaymentId, DateTime.ParseExact(TransDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString());
            if (dtCB.Rows.Count > 0)
            {


                listCB = (from DataRow dr in dtCB.Rows
                          select new AllDocumentSegment()
                          {
                              Invoice_Id = Convert.ToString(dr["Invoice_Id"].ToString()),
                              DocumentNumber = Convert.ToString(dr["DocumentNumber"].ToString()),
                              DocumentType = Convert.ToString(dr["DocumentType"].ToString()),
                              UnPaidAmount = Convert.ToString(dr["UnPaidAmount"]),
                              DocDate = Convert.ToString(dr["DocDate"]),
                              branch = Convert.ToString(dr["branch"]),
                              Type = Convert.ToString(dr["Type"]),
                              UniqueId = Convert.ToString(dr["UniqueId"]),
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              Project_Code = Convert.ToString(dr["Project_Code"]),
                              Segment1 = Convert.ToString(dr["Segment1"]),
                              Segment2 = Convert.ToString(dr["Segment2"]),
                              Segment3 = Convert.ToString(dr["Segment3"]),
                              Segment4 = Convert.ToString(dr["Segment4"]),
                              Segment5 = Convert.ToString(dr["Segment5"])
                          }).ToList();

            }

            return listCB;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetAllDocumentWithProject(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate, string ProjectId)
        {
            List<AllDocument> listCB = new List<AllDocument>();
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            dtCB = objCustRecPayBL.GetAllDocumentWithProject(VoucherType, CustomerId, BranchId, ReceiptPaymentId, DateTime.ParseExact(TransDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString(), ProjectId);
            if (dtCB.Rows.Count > 0)
            {


                listCB = (from DataRow dr in dtCB.Rows
                          select new AllDocument()
                          {
                              Invoice_Id = Convert.ToString(dr["Invoice_Id"].ToString()),
                              DocumentNumber = Convert.ToString(dr["DocumentNumber"].ToString()),
                              DocumentType = Convert.ToString(dr["DocumentType"].ToString()),
                              UnPaidAmount = Convert.ToString(dr["UnPaidAmount"]),
                              DocDate = Convert.ToString(dr["DocDate"]),
                              branch = Convert.ToString(dr["branch"]),
                              Type = Convert.ToString(dr["Type"]),
                              UniqueId = Convert.ToString(dr["UniqueId"]),
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              Project_Code = Convert.ToString(dr["Project_Code"])
                          }).ToList();

            }

            return listCB;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetAllDocumentPayment(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate)
        {
            List<AllDocument> listCB = new List<AllDocument>();
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            dtCB = objCustRecPayBL.GetAllDocumentPayment(VoucherType, CustomerId, BranchId, ReceiptPaymentId, DateTime.ParseExact(TransDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString());
            if (dtCB.Rows.Count > 0)
            {


                listCB = (from DataRow dr in dtCB.Rows
                          select new AllDocument()
                          {
                              Invoice_Id = Convert.ToString(dr["Invoice_Id"].ToString()),
                              DocumentNumber = Convert.ToString(dr["DocumentNumber"].ToString()),
                              DocumentType = Convert.ToString(dr["DocumentType"].ToString()),
                              UnPaidAmount = Convert.ToString(dr["UnPaidAmount"]),
                              DocDate = Convert.ToString(dr["DocDate"]),
                              branch = Convert.ToString(dr["branch"]),
                              Type = Convert.ToString(dr["Type"]),
                              UniqueId = Convert.ToString(dr["UniqueId"]),
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              Project_Code = Convert.ToString(dr["Project_Code"])
                          }).ToList();

            }

            return listCB;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetAllDocumentPaymentWithProject(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate, string ProjectId)
        {
            List<AllDocument> listCB = new List<AllDocument>();
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            dtCB = objCustRecPayBL.GetAllDocumentPaymentWithProject(VoucherType, CustomerId, BranchId, ReceiptPaymentId, DateTime.ParseExact(TransDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString(), ProjectId);
            if (dtCB.Rows.Count > 0)
            {


                listCB = (from DataRow dr in dtCB.Rows
                          select new AllDocument()
                          {
                              Invoice_Id = Convert.ToString(dr["Invoice_Id"].ToString()),
                              DocumentNumber = Convert.ToString(dr["DocumentNumber"].ToString()),
                              DocumentType = Convert.ToString(dr["DocumentType"].ToString()),
                              UnPaidAmount = Convert.ToString(dr["UnPaidAmount"]),
                              DocDate = Convert.ToString(dr["DocDate"]),
                              branch = Convert.ToString(dr["branch"]),
                              Type = Convert.ToString(dr["Type"]),
                              UniqueId = Convert.ToString(dr["UniqueId"]),
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              Project_Code = Convert.ToString(dr["Project_Code"])
                          }).ToList();

            }

            return listCB;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRate(string Currency_ID)
        {

            CustRecPayBL objPurchaseOrderBL = new CustRecPayBL();
            string CompanyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
            string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
            DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), CompanyId);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
            }

            return SalesRate;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public  object GetHSNWiseProduct(string SearchKey, string HSNID)
        {
            DataTable dt=new DataTable();
            List<ProductList> prod = new List<ProductList>();

            DBEngine blDb = new DBEngine();
            if (!string.IsNullOrEmpty(HSNID))
            {
                dt = blDb.GetDataTable("Select top 10 sProducts_ID,sProducts_Code,sProducts_name,sProducts_HsnCode from Master_sProducts where (sProducts_Code like '%" + Convert.ToString(SearchKey) + "%' or sProducts_name like '%" + Convert.ToString(SearchKey) + "%') and sProducts_HsnCode='" + Convert.ToString(HSNID) + "' and sProduct_Status<>'D'");

            }
            else
            {
                dt = blDb.GetDataTable("Select top 10 sProducts_ID,sProducts_Code,sProducts_name,sProducts_HsnCode from Master_sProducts where (sProducts_Code like '%" + Convert.ToString(SearchKey) + "%' or sProducts_name like '%" + Convert.ToString(SearchKey) + "%') and isnull(sProducts_HsnCode,'')<>'' and sProduct_Status<>'D'");

            }

            prod = (from DataRow dr in dt.Rows
                    select new ProductList()
                    {
                        id = Convert.ToString(dr["sProducts_ID"]),
                        sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                        Name = Convert.ToString(dr["sProducts_name"]),
                        sProducts_HsnCode = Convert.ToString(dr["sProducts_HsnCode"])
                    }).ToList();


            return prod;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTaxTableSet(string hsnCodeTax,decimal Amount,string branchid,string customerstate)
        {
            List<GetTaxTable> prod = new List<GetTaxTable>();

            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtTax = new DataTable();


            dtTax = objCustRecPayBL.GetTaxTable(hsnCodeTax, Amount, branchid, customerstate);



            prod = (from DataRow dr in dtTax.Rows
                    select new GetTaxTable()
                    {
                        hdnslno = Convert.ToString(dr["hdnslno"]),
                        slno = Convert.ToString(dr["slno"]),
                        Taxes_Name = Convert.ToString(dr["Taxes_Name"]),
                        TaxTypeCode = Convert.ToString(dr["TaxTypeCode"]),
                        TaxRates_Rate = Convert.ToString(dr["TaxRates_Rate"]),
                        taxCodeName = Convert.ToString(dr["taxCodeName"]),
                        TaxableAmount = Convert.ToString(dr["TaxableAmount"]),
                        TaxAmount = Convert.ToString(dr["TaxAmount"])
                    }).ToList();


            return prod;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetBranchStateCode(string BranchId)
        {
            List<GetTaxTable> prod = new List<GetTaxTable>();

            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtStateCode = new DataTable();


            dtStateCode = objCustRecPayBL.GetBranchStateCode(BranchId);






            return Convert.ToString(dtStateCode.Rows[0]["StateCode"]);
        }

        


    }


    public class ProductList
    {
        
        public string id { get; set; }
        public string sProducts_Code { get; set; }
        public string Name { get; set; }
        public string sProducts_HsnCode { get; set; }

    }



    public class ddlClass
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    public class Allddl
    {
        public List<ddlClass> NumberingSchema { get; set; }
        public List<ddlClass> ForBranch { get; set; }
        public List<ddlClass> Currency { get; set; }

        public DateTime SatrtDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SysSetting { get; set; }

        public string UDFCount { get; set; }


    }

    public class AllDocumentSegment
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string UnPaidAmount { get; set; }
        public string Invoice_Id { get; set; }
        public string DocDate { get; set; }
        public string branch { get; set; }
        public string Type { get; set; }
        public string UniqueId { get; set; }
        public Int64 ProjectId { get; set; }
        public string Project_Code { get; set; }
        public string Segment1 { get; set; }
        public string Segment2 { get; set; }
        public string Segment3 { get; set; }
        public string Segment4 { get; set; }

        public string Segment5 { get; set; }

    }

    public class AllDocument
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string UnPaidAmount { get; set; }
        public string Invoice_Id { get; set; }
        public string DocDate { get; set; }
        public string branch { get; set; }
        public string Type { get; set; }
        public string UniqueId { get; set; }
        public Int64 ProjectId { get; set; }

        public string Project_Code { get; set; }
    }

    public class GetTaxTable
    {
        public string hdnslno { get; set; }
        public string slno { get; set; }
        public string Taxes_Name { get; set; }

        public string taxCodeName { get; set; }

        public string TaxRates_Rate { get; set; }

        public string TaxTypeCode { get; set; }

        public string TaxableAmount { get; set; }
        public string TaxAmount { get; set; }

    }

}
