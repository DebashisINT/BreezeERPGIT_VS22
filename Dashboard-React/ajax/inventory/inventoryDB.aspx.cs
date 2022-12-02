using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.inventory
{
    public partial class inventoryDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static object GetStockRequisition(string asondate)
        {


            List<StockRequisition> lEfficency = new List<StockRequisition>();
            ProcedureExecute proc = new ProcedureExecute("PRC_STOCKREQDB_REPORT");
            proc.AddVarcharPara("@ASONDATE", 100, asondate);
            //proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new StockRequisition()
                          {
                              BRANCHREQ = Convert.ToString(dr["BRANCHREQ"]),
                              APPRPENDING = Convert.ToString(dr["APPRPENDING"]),
                              OPENREQ = Convert.ToString(dr["OPENREQ"]),
                              CLOSEREQ = Convert.ToString(dr["CLOSEREQ"]),
                              APPRREQ = Convert.ToString(dr["APPRREQ"]),
                          }).ToList();
            return lEfficency;
        }
        // for branch population
        [WebMethod]
        public static object GetBaranchData()
        {

            List<branchClass> lEfficency = new List<branchClass>();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
            proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
         
            //proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new branchClass()
                          {
                              value = Convert.ToString(dr["branch_id"]),
                              label = Convert.ToString(dr["branch_description"]),
                             
                          }).ToList();
            return lEfficency;
        }


        [WebMethod]
        public static object GetProcurement(string date, string branchid, string ProdClass, string Prodid)
        {

            List<ProcurementRequisition> TodoThisMonth = new List<ProcurementRequisition>();
        
            ProcedureExecute proc = new ProcedureExecute("PRC_PROCUREMENTREQDB_REPORT");

            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 5000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            proc.AddVarcharPara("@USERID", 500, Convert.ToString(HttpContext.Current.Session["userid"]));

            //proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            TodoThisMonth = (from DataRow dr in CallData.Rows
                             select new ProcurementRequisition() 
                             {
                                 PURCHASEREQ = Convert.ToString(dr["PURCHASEREQ"]),
                                 OPENREQ = Convert.ToString(dr["OPENREQ"]),
                                 TOTPO = Convert.ToString(dr["TOTPO"]),
                                 APPRPO = Convert.ToString(dr["APPRPO"]),
                                 APPRREQ = Convert.ToString(dr["APPRREQ"]),
                             }).ToList();

            return TodoThisMonth;
        }
        public class ProcurementRequisition
        {

            public string PURCHASEREQ { get; set; }
            public string OPENREQ { get; set; }
            public string TOTPO { get; set; }
            public string APPRPO { get; set; }
            public string APPRREQ { get; set; }
        }
        public class StockRequisition
        {
            public string BRANCHREQ { get; set; }
            public string APPRPENDING { get; set; }
            public string OPENREQ { get; set; }
            public string CLOSEREQ { get; set; }
            public string APPRREQ { get; set; }
        }

        public class branchClass
        { 
            public string value { get; set; }
            public string label { get; set; }
        }
    }
}