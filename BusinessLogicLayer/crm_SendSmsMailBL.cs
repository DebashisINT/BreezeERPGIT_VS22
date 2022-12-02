using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class crm_SendSmsMailBL
    {
        public DataTable PopulateCustomer(string contactType)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_CustomerByType");
                proc.AddVarcharPara("@cnt_contactType", 100, contactType);
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
