using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.Repostiory.PartialHeaderMenu
{
    public class PartialHeader : IPartialHeader
    {
        public DataSet ViewLayoutHeader(ref string retmsg, string company_id, string branch_id, string user_id)
        {
            DataSet _layoutHeader = new DataSet();
            string output = string.Empty;
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "prc_Mvc_Layout_HeaderData";
                paramList.Add(new KeyObj("@Action", "COMPANYMASTERLAYOUTHEADER"));
                paramList.Add(new KeyObj("@company_Id", company_id));
                paramList.Add(new KeyObj("@branch_id", branch_id));
                paramList.Add(new KeyObj("@userid", user_id));
                paramList.Add(new KeyObj("@is_success", output, true));
                execProc.param = paramList;
                _layoutHeader = execProc.ExecuteProcedureGetDataSet();
                output = Convert.ToString(execProc.outputPara[0].value);
                paramList.Clear();

            }
            catch (Exception ex)
            {
                //throw;

            }
            retmsg = output;
            return _layoutHeader;
        }
    }
}