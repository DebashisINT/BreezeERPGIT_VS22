using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.YearEnding
{
   public  class YearEndingBL
    {

       public DateTime GetCutOffDate()
       {
           
           ProcedureExecute proc = new ProcedureExecute("Prc_YearEnding");
           proc.AddVarcharPara("@Action", 100, "CutOffDate");
           proc.AddVarcharPara("@master_dbname", 100, Convert.ToString(ConfigurationManager.AppSettings["MasterDBName"]));
           return Convert.ToDateTime(proc.GetTable().Rows[0]["YearEnding_Date"]);
          
       }
       public string GetCutOffDBName()
       {
           string DBname = "";
           ProcedureExecute proc = new ProcedureExecute("Prc_YearEnding");
           proc.AddVarcharPara("@Action", 100, "CutOffDB");
           proc.AddVarcharPara("@master_dbname", 100, Convert.ToString(ConfigurationManager.AppSettings["MasterDBName"]));
           DBname = Convert.ToString(proc.GetTable().Rows[0]["CutOffDbName"]);
           
           return DBname;

       }
       public void PendingCutOffProcess(ref string returnValue,ref string returnText,DateTime CutOffdate)
       {
          // int i;
           //int returnValue=0;
           ProcedureExecute proc = new ProcedureExecute("Prc_Yearending_PendingCutoffProcess");
           proc.AddDateTimePara("@CutOffdate", CutOffdate);
           proc.AddVarcharPara("@MasterDbName", 100, Convert.ToString(ConfigurationManager.AppSettings["MasterDBName"]));
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           proc.AddVarcharPara("@ReturnText", 100, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
           returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
           returnText = Convert.ToString(proc.GetParaValue("@ReturnText"));
          // return returnValue;
       }
       public void NegStockProcess(ref string returnValue, ref string returnText, DateTime CutOffdate)
       {
           // int i;
           //int returnValue=0;
           ProcedureExecute proc = new ProcedureExecute("Prc_Yearending_NegativeStock");
           proc.AddDateTimePara("@CutOffdate", CutOffdate);
           proc.AddVarcharPara("@COMPANYID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FINYEAR", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           proc.AddVarcharPara("@ReturnText", 100, "0", QueryParameterDirection.Output);
           proc.RunActionQuery();
           returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
           returnText = Convert.ToString(proc.GetParaValue("@ReturnText"));
           // return returnValue;
       }
       public string YearendingLastStage()
       {
           DataTable CutoffValue = new DataTable();
           ProcedureExecute pro = new ProcedureExecute("prc_Yearending_LastStage");
           pro.AddVarcharPara("@Masterdbname", 100, Convert.ToString(ConfigurationManager.AppSettings["MasterDBName"]));
           CutoffValue = pro.GetTable();
           return "";
       }

       public string YearendingChallan(DateTime CutOffdate)
       {
           DataTable CutoffValue = new DataTable();
           ProcedureExecute pro = new ProcedureExecute("Prc_Yearending_Challan");
           pro.AddDateTimePara("@CutOffdate", CutOffdate);
           CutoffValue = pro.GetTable();
           return "";
       }
       public DataTable PendingSalesDocumentList(DateTime? CutOffdate)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_Yearending_PendingDocument");
           proc.AddVarcharPara("@Action", 100, "PendingSalesInvoice");
           if (CutOffdate!=null)
           proc.AddPara("@CutOffdate", Convert.ToString(CutOffdate));
           ds = proc.GetTable();
           return ds;
       }
       public DataTable PendingPurchaseDocumentList(DateTime? CutOffdate)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_Yearending_PendingDocument");
           proc.AddVarcharPara("@Action", 100, "PendingPurchaseChallan");
           if (CutOffdate != null)
               proc.AddPara("@CutOffdate", Convert.ToString(CutOffdate));
           ds = proc.GetTable();
           return ds;
       }
       public DataTable PendingNegativeProductList(DateTime? CutOffdate)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_Yearending_PendingDocument");
           proc.AddVarcharPara("@Action", 100, "NegativeStockList");
           if (CutOffdate != null)
               proc.AddPara("@CutOffdate", Convert.ToString(CutOffdate));
           proc.AddVarcharPara("@COMPANYID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FINYEAR", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
           ds = proc.GetTable();
           return ds;
       }
    }
}
