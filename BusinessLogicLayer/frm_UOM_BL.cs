using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public class frm_UOM_BL
    {
        public DataSet Report_Uom()
        {
            ProcedureExecute proc;
            DataSet rtrnvalue ;//= "";
            try
            {
                using (proc = new ProcedureExecute("Report_Uom"))
                {
                   //proc.RunActionQuery();
                   rtrnvalue = proc.GetDataSet();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                //commented due to giving error while session is expired.
                //throw ex;
                return rtrnvalue = null;
            }

            finally
            {
                proc = null;
            }
        }

    }
}
