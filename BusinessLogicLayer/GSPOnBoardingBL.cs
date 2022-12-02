using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class GSPOnBoardingBL
    {
        public DataTable getGSPValue()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("SP_GSPStep1Details");
            proc.AddVarcharPara("@Action", 50, "GSPData");
            dt = proc.GetTable();
          
            return dt;
        }
    }
}
