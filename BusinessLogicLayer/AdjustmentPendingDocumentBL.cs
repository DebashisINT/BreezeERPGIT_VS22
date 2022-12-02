using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using EntityLayer;

namespace BusinessLogicLayer
{
   public class AdjustmentPendingDocumentBL
    {
       public DataTable PopulatePendingDocumentForAdjustment(string userbranchlist, string lastCompany, string lastfinyear, string Status,string type)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_AdjustmentOfPendingDocumentDtl");
           proc.AddVarcharPara("@Action", 500, "PopulatePendingDocumentForAdjustment");
           proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
           proc.AddVarcharPara("@lastCompany", 50, lastCompany);
           proc.AddVarcharPara("@FinYear", 50, lastfinyear);
           proc.AddVarcharPara("@Status", 2, Status);
           proc.AddVarcharPara("@type", 2, type);
           dt = proc.GetTable();
           return dt;
       }

        

        public DataTable PopulateCustVendByCondition(string csutven)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_AdjustmentOfPendingDocumentDtl");
           proc.AddVarcharPara("@Action", 500, "PopulateCustVendByCondition");
           proc.AddVarcharPara("@Status", 2, csutven); 
           dt = proc.GetTable();
           return dt;
       }

        
       //public DataTable PopulateDocumentForAdjustmentByVendorid(string userbranchlist, string lastCompany, string fromdate, string todate, string branch, string status, string custvenId)
        public DataTable PopulateDocumentForAdjustmentByVendorid(string branch, string lastCompany, string doctype,  string custvenId)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_AdjustmentOfPendingDocumentDtl");
           proc.AddVarcharPara("@Action", 500, "PopulateDocumentForAdjustmentByVendorid");
            //proc.AddVarcharPara("@BranchList", 500, userbranchlist);
           proc.AddVarcharPara("@CompanyID", 50, lastCompany);
           //proc.AddVarcharPara("@fromdate", 50, fromdate);
           //proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@branchId", 50, branch);
           //proc.AddVarcharPara("@Status", 5, status);
           proc.AddVarcharPara("@doctype", 20, doctype);
           proc.AddVarcharPara("@VendorID", 15, custvenId);
           dt = proc.GetTable();
           return dt;
       }
       
       
       public DataTable PopulatePendingDocumentForAdjustmentByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch, string status, string type)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_AdjustmentOfPendingDocumentDtl");
           //proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataByDate");
           proc.AddVarcharPara("@Action", 500, "PopulatePendingDocumentForAdjustmentByDate");
           proc.AddVarcharPara("@BranchList", 500, userbranchlist);
           proc.AddVarcharPara("@CompanyID", 50, lastCompany);
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@branchId", 50, branch);
           proc.AddVarcharPara("@Status", 5, status);
           proc.AddVarcharPara("@type", 2, type); 
           dt = proc.GetTable();
           return dt;
       }

       public DataTable getBranchListByHierchy(string userbranchhierchy)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_AdjustmentOfPendingDocumentDtl");
           proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
           proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
           ds = proc.GetTable();
           return ds;
       }

       public DataSet PopulateAdjustmentDocDtlByDocIdandDocType(string docid, string doctype)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_AdjustmentOfPendingDocumentDtl");
           proc.AddVarcharPara("@Action", 500, "PopulateAdjustmentDocDtlByDocIdandDocType");
           proc.AddIntegerPara("@Docid", Convert.ToInt32(docid));
           proc.AddVarcharPara("@DocType", 100, doctype);
           dst = proc.GetDataSet();
           return dst;
       }
    }
}
