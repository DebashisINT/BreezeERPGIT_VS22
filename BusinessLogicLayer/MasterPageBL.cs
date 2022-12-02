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
   public class MasterPageBL
    {
       public DataSet CompanyMasterDetail(string CompanyId, string exchangeSegmentID, string userid, string userlastsegment, string usergrp)
       {
           try
           {
               DataSet dst = new DataSet();
               ProcedureExecute proc = new ProcedureExecute("prc_MasterDetail");  
               proc.AddVarcharPara("@Action", 100, "CompanyMasterDetail");
               proc.AddVarcharPara("@campany_Id", 10, CompanyId);
               proc.AddVarcharPara("@exchangeSegmentID", 10, exchangeSegmentID);
               proc.AddVarcharPara("@userid", 10, userid);
               proc.AddVarcharPara("@userlastsegment", 10, userlastsegment);
               proc.AddVarcharPara("@usergrp", 50, usergrp);
               dst = proc.GetDataSet();
               return dst;
           }
           catch
           {
               return null;
           }
       }

       public DataTable PopulateAccessPages(string userid, string userlastsegment)
       {
           try
           {
               DataTable dt = new DataTable();
               ProcedureExecute proc = new ProcedureExecute("prc_MasterDetail");
               proc.AddVarcharPara("@Action", 100, "PopulateAccessPages");
               proc.AddVarcharPara("@userid", 7, userid);
               proc.AddVarcharPara("@userlastsegment", 10, userlastsegment);

               dt = proc.GetTable();
               return dt;
           }
           catch
           {
               return null;
           }
       }
    }
}
