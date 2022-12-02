using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.Services
{
    /// <summary>
    /// Summary description for CustomerNote
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CustomerNote : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNumberingSchemeByType(string VoucherType)
        {
            CustomerNoteBL objCustRecPayBL = new CustomerNoteBL();
            DataSet AllDetailsByVoucherType = objCustRecPayBL.GetAllDropDownDataByVoucherType(VoucherType);


            DataTable numberingschemeData = AllDetailsByVoucherType.Tables[0];
            AllddlCustNote All = new AllddlCustNote();
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

            if (AllDetailsByVoucherType.Tables[6] != null && AllDetailsByVoucherType.Tables[6].Rows.Count > 0)
            {
                All.reason = (from DataRow dr in AllDetailsByVoucherType.Tables[6].Rows
                                 select new ddlClass()
                                 {
                                     Id = dr["ReasonID"].ToString(),
                                     Name = dr["Reason"].ToString()
                                 }).ToList();
            }


            return All;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSaleInvoiceForCustomer(string customerId, DateTime TransDate, string BranchID)
        {

            List<Services.ddlClass> ddlClass = new List<Services.ddlClass>();

            CustomerNoteBL objCustRecPayBL = new CustomerNoteBL();

            DataTable salesInvoiceList = objCustRecPayBL.GetAllInvoiceForCustomer(customerId, TransDate, BranchID);


            ddlClass = (from DataRow dr in salesInvoiceList.Rows
                            select new ddlClass()
                            {
                                Id = dr["Invoice_Id"].ToString(),
                                Name = dr["Invoice_Number"].ToString()
                            }).ToList();



            return ddlClass;
            
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSaleInvoiceForCustomerWithProject(string customerId, DateTime TransDate, string BranchID, Int64 ProjectId)
        {

            List<Services.ddlClass> ddlClass = new List<Services.ddlClass>();

            CustomerNoteBL objCustRecPayBL = new CustomerNoteBL();

            DataTable salesInvoiceList = objCustRecPayBL.GetAllInvoiceForCustomerWithProject(customerId, TransDate, BranchID, ProjectId);


            ddlClass = (from DataRow dr in salesInvoiceList.Rows
                        select new ddlClass()
                        {
                            Id = dr["Invoice_Id"].ToString(),
                            Name = dr["Invoice_Number"].ToString()
                        }).ToList();



            return ddlClass;

        }

    }

    public class AllddlCustNote
    {
        public List<ddlClass> NumberingSchema { get; set; }
        public List<ddlClass> ForBranch { get; set; }
        public List<ddlClass> Currency { get; set; }

        public List<ddlClass> reason { get; set; }

        public DateTime SatrtDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SysSetting { get; set; }

        public string UDFCount { get; set; }


    }
}
