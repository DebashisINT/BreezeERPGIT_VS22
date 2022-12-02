using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace BusinessLogicLayer
{
    public class ApplicationErrorBL
    {
        public int ApplicationErrorInsert(string userid, string xmlcontent)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("UpdateApplicationErrorLog"))
                {
                    proc.AddNVarcharPara("@UserId",50, userid);
                    //proc.AddNTextPara("@ErrorData", xmlcontent); 
                    proc.AddNTextPara("@ErrorData", xmlcontent); 
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

    }
}
