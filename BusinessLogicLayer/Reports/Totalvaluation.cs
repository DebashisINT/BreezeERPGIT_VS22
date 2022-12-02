using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class TotalvaluationClass
    {


       public DataTable Getvaluationdetails(string branch, string product, string company, string Finyear, string FROMDATE, string TODATE, string classname, string brand, int userid)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ProductValuation_Report");
           proc.AddPara("@BRANCHID", branch);
           proc.AddPara("@PRODUCT_ID", product);
           proc.AddPara("@COMPANYID", company);
           proc.AddPara("@FINYEAR", Finyear);
           proc.AddPara("@GetType", "Details");
           proc.AddPara("@FROMDATE", FROMDATE);
           proc.AddPara("@TODATE", TODATE);
           proc.AddPara("@val_type", "F");
           proc.AddPara("@Class", classname);
           proc.AddPara("@Brand", brand);
           proc.AddPara("@USERID", userid);

           ds = proc.GetTable();

           return ds;
       }



       public DataTable GetvaluationSummary(string branch, string product, string company, string Finyear, string FROMDATE, string TODATE,string valtype,string gettype,string classname,string brand)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_ProductValuation_Report");
           proc.AddPara("@BRANCHID", branch);
           proc.AddPara("@PRODUCT_ID", product);
           proc.AddPara("@COMPANYID", company);
           proc.AddPara("@FINYEAR", Finyear);
           proc.AddPara("@GetType", "Summary");
           proc.AddPara("@val_type", valtype);
           proc.AddPara("@TODATE", TODATE);
           proc.AddPara("@FROMDATE", FROMDATE);

           proc.AddPara("@Class", classname);
           proc.AddPara("@Brand", brand);

           ds = proc.GetTable();

           return ds;
       }


    }
}
