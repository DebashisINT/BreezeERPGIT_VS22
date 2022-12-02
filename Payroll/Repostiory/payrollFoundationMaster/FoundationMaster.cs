using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollFoundationMaster
{
    public class FoundationMaster:IFoundationMaster
    {
        public void save(string desc,string code ,ref int ReturnCode, ref string ReturnMsg)
        {

            string action = string.Empty;


            try
            {
                action = "AddMaster";
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_MasterModify";
                    paramList.Add(new KeyObj("@Action", action));
                    paramList.Add(new KeyObj("@UserID", user_id));//ADDEDITCATEGORYIMPORT
                    paramList.Add(new KeyObj("@RID", code));
                    paramList.Add(new KeyObj("@Desc",desc));
                    paramList.Add(new KeyObj("@ReturnMessage", ReturnMsg, true));
                    paramList.Add(new KeyObj("@ReturnCode", ReturnCode, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Delete(string ActionType, string code, ref int ReturnCode)
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
                    execProc.ProcedureName = "proll_MasterModify";
                    paramList.Add(new KeyObj("@Action", ActionType));
                    //paramList.Add(new KeyObj("@user_id", user_id));//ADDEDITCATEGORYIMPORT
                    paramList.Add(new KeyObj("@MasterCode", code));

                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnCode", ReturnCode, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
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