using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public static class EinvoiceToken
    {
        public static string token { get; set; }
        public static DateTime Expiry { get; set; }
    }

    public class eInvoiceTokenReturn
    {
        public DataTable eInvoiceDetailsReturn(String Branch_id, String Type)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_GeteInvoiceOnBoardingBranchDetails");
            proc.AddPara("@ApiType", Type);
            proc.AddPara("@CompanyBranch_Id", Branch_id);
            dt = proc.GetTable();
            return dt;
        }
    }

    public static class EInvoiceConfiguration
    {

    }

}