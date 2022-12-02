using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;
using System.Data.SqlClient;


namespace BusinessLogicLayer
{
   public partial class Settlement_Processing
    {

       public SqlDataReader Process_CMBills(string segmentid,string companyID,string settlementNumber,string settlementType,string Tdate,
                                        string vdate,string type,string createUser,string finYear,string masterSegment)
       {


           SqlDataReader dt ;
           ProcedureExecute proc = new ProcedureExecute("Process_CMBills");

           proc.AddVarcharPara("@segmentid", 100, segmentid);
           proc.AddNVarcharPara("@companyID", 100, companyID);
           proc.AddVarcharPara("@settlementNumber", 100, settlementNumber);
           proc.AddVarcharPara("@settlementType", 100, settlementType);
           proc.AddNVarcharPara("@Tdate", 100, Tdate);
           proc.AddVarcharPara("@vdate", 100, vdate);
           proc.AddVarcharPara("@type", 100, type);
           proc.AddNVarcharPara("@createUser", 100, createUser);
           proc.AddVarcharPara("@finYear", 100, finYear);
           proc.AddVarcharPara("@masterSegment", 100, masterSegment);

           dt = proc.GetReader();
           return dt;
       }

    }
}
