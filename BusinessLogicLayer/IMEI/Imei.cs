﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;
namespace BusinessLogicLayer.IMEI
{
   public  class Imei
    {
       public DataTable GetBranchdetails()
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Proc_IMEI_GetallBranch");
           proc.AddPara("@Action", "Branch");
           ds = proc.GetTable();

           return ds;
       }


       public DataTable GetUser(int branch,string action)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Proc_IMEI_GetallBranch");
           proc.AddPara("@BranchId", branch);
           proc.AddPara("@Action", action);
           ds = proc.GetTable();
           return ds;
       }

       public DataTable GetModifydata(int UserImeiId,string Action)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Proc_IMEI_GetallBranch");
           proc.AddPara("@UserImeiId", @UserImeiId);
           proc.AddPara("@Action", Action);
           ds = proc.GetTable();
           return ds;
       }


       public int InsertImei(IMEIClass model)
       {
           DataSet dsInst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Proc_IMEI_GetallBranch");
           proc.AddPara("@UserId", model.User);
           proc.AddPara("@imei", model.Imei);
           proc.AddPara("@Action", model.Action);
           proc.AddPara("@UserImeiId", model.ImeiId);
           proc.AddPara("@CretemodifyBy", model.CretemodifyBy);
           return proc.RunActionQuery();
       }


       public static int Deletemei(int UserImeiId,string action)
       {
           DataSet dsInst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Proc_IMEI_GetallBranch");
           proc.AddPara("@UserImeiId", UserImeiId);
           proc.AddPara("@Action", action);
           return proc.RunActionQuery();
       }
       public DataTable GetListofImei()
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Proc_IMEI_GetallBranch");
           proc.AddPara("@Action", "ListImei");
           ds = proc.GetTable();

           return ds;
       }
    }

    public class IMEIClass
    {

        public string User { get; set; }
        public string Imei { get; set; }
        public string Action { get; set; }
        public int ImeiId { get; set; }
        public string CretemodifyBy { get; set; }
    }
}
