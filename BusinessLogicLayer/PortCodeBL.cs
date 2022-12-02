using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class PortCodeBL
    {
       public bool insertPortCode(string PortCode,  string PortDescription,string PortCountryId,string PortStateId ,int userId)
        {
            bool retValue = true;
            ProcedureExecute proc = new ProcedureExecute("Prc_master_PortCode");
            proc.AddVarcharPara("@Action", 50, "insertPortCode");
            proc.AddVarcharPara("@PortCode", 50, PortCode);
            proc.AddVarcharPara("@PortDesc", 500, PortDescription);
            proc.AddVarcharPara("@CountryId", 50, PortCountryId);
            proc.AddVarcharPara("@StateId", 50, PortStateId);
            proc.AddIntegerPara("@CreateUser", userId);
             
            try
            {
                int RowsNo = proc.RunActionQuery();
            }
            catch (Exception e)
            {
                retValue = false;
            }

            return true;
        }

       public bool updatePortCode(int portId, string PortDescription, string PortCountryId, string PortStateId, int userId)
        {
            bool retValue = true;
            ProcedureExecute proc = new ProcedureExecute("Prc_master_PortCode");
            proc.AddVarcharPara("@Action", 50, "updatePortCode");
            //proc.AddVarcharPara("@PortCode", 50, PortCode);
            proc.AddIntegerPara("@PortId", portId);
            proc.AddVarcharPara("@PortDesc", 500, PortDescription);
            proc.AddVarcharPara("@CountryId", 50, PortCountryId);
            proc.AddVarcharPara("@StateId", 50, PortStateId);
            proc.AddIntegerPara("@CreateUser", userId);
            try
            {
                int RowsNo = proc.RunActionQuery();
            }
            catch (Exception e)
            {
                retValue = false;
            }

            return true;
        }


       public int deletePortCode(int portId)
        {
            int RowsNo = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_master_PortCode");
            proc.AddVarcharPara("@Action", 50, "deletePortCode");
            proc.AddIntegerPara("@PortId", portId);
            try
            {
                RowsNo = proc.RunActionQuery();
            }
            catch (Exception e)
            {

            }

            return RowsNo;
        }

       public DataTable GetPortCodeById(int portId)
        {
            DataTable Dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_master_PortCode");
            proc.AddVarcharPara("@Action", 50, "GetPortCodeById");
            proc.AddIntegerPara("@PortId", portId);
            try
            {
                Dt = proc.GetTable();
            }
            catch (Exception e)
            {

            }

            return Dt;
        }
    }
}
