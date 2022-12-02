using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;


namespace BusinessLogicLayer.SalesmanAddress
{
    public class SalesmanAddress
    {
        public DataTable GetBranchdetails()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Salesmanuser_Address");
            proc.AddPara("@Action", "Branch");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetStatesdetails()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Salesmanuser_Address");
            proc.AddPara("@Action", "States");
            ds = proc.GetTable();
            return ds;
        }


        public DataTable GetUser(int branch, string action,string Pid)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Salesmanuser_Address");
            proc.AddPara("@BranchId", branch);
            proc.AddPara("@Action", action);
            proc.AddPara("@PID", Pid);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetModifydata(int UserSddressId, string Action)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Salesmanuser_Address");
            proc.AddPara("@UserAddressId", UserSddressId);
            proc.AddPara("@Action", Action);
            ds = proc.GetTable();
            return ds;
        }

        public int InsertSalesManAddress(UserAddressClass model)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Proc_Salesmanuser_Address");
            proc.AddPara("@UserId", model.User);
            proc.AddPara("@Address", model.Address);
            proc.AddPara("@latitude", model.latitude);
            proc.AddPara("@longitude", model.longitude);
            proc.AddPara("@Action", model.Action);
            proc.AddPara("@UserAddressId", model.UserAddressId);
            proc.AddPara("@CretemodifyBy", model.CretemodifyBy);
            proc.AddPara("@StateId", model.stateId);
            return proc.RunActionQuery();
        }


        public static int DeletAddress(int UseraddId, string action)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Proc_Salesmanuser_Address");
            proc.AddPara("@UserAddressId", UseraddId);
            proc.AddPara("@Action", action);
            return proc.RunActionQuery();
        }

        public DataTable GetListofUserAddress()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Salesmanuser_Address");
            proc.AddPara("@Action", "List");
            ds = proc.GetTable();
            return ds;
        }
    }



    public class UserAddressClass
    {

        public string User { get; set; }
        public string Address { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string Action { get; set; }
        public int UserAddressId { get; set; }
        public string CretemodifyBy { get; set; }

        public string stateId { get; set; }
    }
}
