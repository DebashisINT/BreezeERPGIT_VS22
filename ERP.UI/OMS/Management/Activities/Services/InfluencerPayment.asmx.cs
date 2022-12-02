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
    /// Summary description for InfluencerPayment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class InfluencerPayment : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNumberingSchemeByType(string VoucherType)
        {
            InfluencerBL objInfluencerBL = new InfluencerBL();
            DataSet AllDetailsByVoucherType = objInfluencerBL.GetAllDropDownDataByVoucherType(VoucherType);


            DataTable numberingschemeData = AllDetailsByVoucherType.Tables[0];
            AllddlINF All = new AllddlINF();
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



            //if (AllDetailsByVoucherType.Tables[3] != null && AllDetailsByVoucherType.Tables[3].Rows.Count > 0)
            //{
            //    All.SysSetting = Convert.ToString(AllDetailsByVoucherType.Tables[3].Rows[0]["Variable_Value"]);

            //}

            if (AllDetailsByVoucherType.Tables[3] != null && AllDetailsByVoucherType.Tables[3].Rows.Count > 0)
            {
                All.SatrtDate = Convert.ToDateTime(AllDetailsByVoucherType.Tables[3].Rows[0]["finYearStartDate"]);
                All.EndDate = Convert.ToDateTime(AllDetailsByVoucherType.Tables[3].Rows[0]["finYearEndDate"]);
            }
            if (AllDetailsByVoucherType.Tables[4] != null && AllDetailsByVoucherType.Tables[4].Rows.Count > 0)
            {
                All.UDFCount = Convert.ToString(AllDetailsByVoucherType.Tables[4].Rows[0]["cnt"]);
            }

            All.TdsSection = (from DataRow dr in AllDetailsByVoucherType.Tables[5].Rows
                              select new ddlClass()
                              {
                                  Id = dr["tdscode"].ToString(),
                                  Name = dr["tdsdescription"].ToString()
                              }).ToList();



            return All;
        }

        

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object BindCashBankAccountJson(string userbranch)
        {
            List<ddlClass> listCB = new List<ddlClass>();
            InfluencerBL objInfluencerBL = new InfluencerBL();
            DataTable dtCB = new DataTable();
            string CompanyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            dtCB = objInfluencerBL.GetInfluencerCashBank(userbranch, CompanyId, "");
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

        public object GetContactPerson(string InfluencerId)
        {
            List<ddlClass> listCB = new List<ddlClass>();
            InfluencerBL objInfluencerBL = new InfluencerBL();
            DataTable dtCB = new DataTable();
            string CompanyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            dtCB = objInfluencerBL.PopulateContactPerson(InfluencerId);
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

        public object GetInflencerTDSRate(string InfluencerId,string TDSSection,string TDSDate)
        {
            InfluencerBL objInfluencerBL = new InfluencerBL();           
            string TdsRate = objInfluencerBL.GetInfluencerTDSRate(InfluencerId, TDSSection,TDSDate);
            return TdsRate;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetAllDocument(string VoucherType, string InfluencerId, string BranchId, string PaymentId, string TransDate)
        {
            List<AllDocument> listCB = new List<AllDocument>();
            InfluencerBL objInfluencerBL = new InfluencerBL();
            DataTable dtCB = new DataTable();
                    dtCB = objInfluencerBL.GetAllDocument(VoucherType, InfluencerId, BranchId, PaymentId, DateTime.ParseExact(TransDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
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
                              UniqueId = Convert.ToString(dr["UniqueId"])
                          }).ToList();

            }

            return listCB;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetAllDocumentPayment(string VoucherType, string InfluencerId, string BranchId, string PaymentId, string TransDate)
        {
            List<AllDocument> listCB = new List<AllDocument>();
            InfluencerBL objInfluencerBL = new InfluencerBL();
            DataTable dtCB = new DataTable();
            dtCB = objInfluencerBL.GetAllDocumentPayment(VoucherType, InfluencerId, BranchId, PaymentId, DateTime.ParseExact(TransDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString());
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
                              UniqueId = Convert.ToString(dr["UniqueId"])
                          }).ToList();

            }

            return listCB;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRate(string Currency_ID)
        {

            InfluencerBL objPurchaseOrderBL = new InfluencerBL();
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
        public object GetHSNWiseProduct(string SearchKey, string HSNID)
        {
            DataTable dt = new DataTable();
            List<ProductList> prod = new List<ProductList>();

            DBEngine blDb = new DBEngine();
            if (!string.IsNullOrEmpty(HSNID))
            {
                dt = blDb.GetDataTable("Select top 10 sProducts_ID,sProducts_Code,sProducts_name,sProducts_HsnCode from Master_sProducts where (sProducts_Code like '%" + Convert.ToString(SearchKey) + "%' or sProducts_name like '%" + Convert.ToString(SearchKey) + "%') and sProducts_HsnCode='" + Convert.ToString(HSNID) + "'");

            }
            else
            {
                dt = blDb.GetDataTable("Select top 10 sProducts_ID,sProducts_Code,sProducts_name,sProducts_HsnCode from Master_sProducts where (sProducts_Code like '%" + Convert.ToString(SearchKey) + "%' or sProducts_name like '%" + Convert.ToString(SearchKey) + "%') and isnull(sProducts_HsnCode,'')<>''");

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
        public object GetTaxTableSet(string hsnCodeTax, decimal Amount, string branchid, string Influencerstate)
        {
            List<GetTaxTable> prod = new List<GetTaxTable>();

            InfluencerBL objInfluencerBL = new InfluencerBL();
            DataTable dtTax = new DataTable();


            dtTax = objInfluencerBL.GetTaxTable(hsnCodeTax, Amount, branchid, Influencerstate);



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

            InfluencerBL objInfluencerBL = new InfluencerBL();
            DataTable dtStateCode = new DataTable();


            dtStateCode = objInfluencerBL.GetBranchStateCode(BranchId);






            return Convert.ToString(dtStateCode.Rows[0]["StateCode"]);
        }

        

    }



    public class AllddlINF
    {
        public List<ddlClass> NumberingSchema { get; set; }
        public List<ddlClass> ForBranch { get; set; }
        public List<ddlClass> Currency { get; set; }

        public List<ddlClass> TdsSection { get; set; }

        public DateTime SatrtDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SysSetting { get; set; }

        public string UDFCount { get; set; }


    }




}
