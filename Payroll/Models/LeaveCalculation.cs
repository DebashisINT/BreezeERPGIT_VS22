using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class LeaveCalculation
    {

        public string ManualLeaveCalculation(string ClassId,string yymm, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;

            try
            {


                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "prc_Proll_LeaveCalculation";
                    paramList.Add(new KeyObj("@PayClassID", ClassId));
                    paramList.Add(new KeyObj("@yymm", yymm));
                    paramList.Add(new KeyObj("@ReturnValue", ReturnCode, true));
                    paramList.Add(new KeyObj("@ReturnMessage", output, true));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    ReturnCode = Convert.ToInt32(execProc.outputPara[0].value);
                    output = Convert.ToString(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }
    }
}