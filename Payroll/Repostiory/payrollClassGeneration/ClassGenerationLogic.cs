using DataAccessLayer;
using DataAccessLayer.Model;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollClassGeneration
{
    public class ClassGenerationLogic : IClassGenrationLogic
    {
        public PClassGenerationEngine GetClassById(string PClassId, ref string IsGenerate, ref string GenerateLastDate)
        {
            DataSet _getClassDetails = new DataSet();
            PClassGenerationEngine objModel = new PClassGenerationEngine();
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_ClassGenerationModify";
                paramList.Add(new KeyObj("@Action", "GetClassById"));
                paramList.Add(new KeyObj("@classId", PClassId));
                execProc.param = paramList;
                _getClassDetails = execProc.ExecuteProcedureGetDataSet();

                if (_getClassDetails.Tables[0].Rows.Count > 0  && _getClassDetails.Tables[0] != null)
                {
                    objModel._PClassName = _getClassDetails.Tables[0].Rows[0]["PayrollClassName"].ToString();
                    objModel._PClassId = _getClassDetails.Tables[0].Rows[0]["PayrollClassID"].ToString();
                    if (_getClassDetails.Tables[0].Rows[0]["PeriodFrom"] != null)
                        objModel._PeriodFrm = Convert.ToDateTime(_getClassDetails.Tables[0].Rows[0]["PeriodFrom"].ToString());
                    if (_getClassDetails.Tables[0].Rows[0]["PeriodTo"] != null)
                        objModel._PeriodTo = Convert.ToDateTime(_getClassDetails.Tables[0].Rows[0]["PeriodTo"].ToString());
                    objModel._PUnit = _getClassDetails.Tables[0].Rows[0]["PayrollUnit"].ToString();
                    objModel._PGeneration = _getClassDetails.Tables[0].Rows[0]["PayrollGeneration"].ToString();
                    if (_getClassDetails.Tables[0].Rows[0]["BranchUnit"].ToString() != "0")
                        objModel._PBranchUnit = _getClassDetails.Tables[0].Rows[0]["BranchUnit"].ToString();
                    if (_getClassDetails.Tables[0].Rows[0]["PHoliDay"].ToString() != "0")
                        objModel._PHoliday = _getClassDetails.Tables[0].Rows[0]["PHoliDay"].ToString();


                }

                if (_getClassDetails.Tables[1].Rows.Count > 0 && _getClassDetails.Tables[1] != null)
                {
                    IsGenerate = _getClassDetails.Tables[1].Rows[0]["IsGenerate"].ToString();
                    GenerateLastDate = _getClassDetails.Tables[1].Rows[0]["GenerateLastDate"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return objModel;
        }
        public void save(PClassGenerationEngine apply, ref int ReturnCode, ref string ReturnMsg)
        {

            string action = string.Empty;
          

            try
            {
                if (apply._PClassId == null || apply._PClassId == "")
                {
                    action = "AddClass";
                }
                else
                {
                    action = "EditClass";
                }
                
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_ClassGenerationModify";
                    if (apply._PClassId!=null || apply._PClassId != "")
                    {
                        paramList.Add(new KeyObj("@classId", apply._PClassId));
                    }
                    paramList.Add(new KeyObj("@Action", action));
                    paramList.Add(new KeyObj("@PHolidayId", apply._PHoliday));
                    paramList.Add(new KeyObj("@UserID", user_id));//ADDEDITCATEGORYIMPORT
                    paramList.Add(new KeyObj("@ClassName", apply._PClassName));
                    paramList.Add(new KeyObj("@PeriodFrm", apply._PeriodFrm));
                    paramList.Add(new KeyObj("@PeriodTo", apply._PeriodTo));
                    paramList.Add(new KeyObj("@Unit", apply._PUnit));
                    paramList.Add(new KeyObj("@Generation", apply._PGeneration));
                    if (apply._PBranchUnit == null || apply._PBranchUnit=="")
                    {
                        paramList.Add(new KeyObj("@Branch",0));
                    }
                    else
                    {
                        paramList.Add(new KeyObj("@Branch",Convert.ToInt32(apply._PBranchUnit)));
                    }
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

        public string Delete(string ActionType, string id, ref int ReturnCode)
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
                    execProc.ProcedureName = "proll_ClassGenerationModify";
                    paramList.Add(new KeyObj("@Action", ActionType));
                    //paramList.Add(new KeyObj("@user_id", user_id));//ADDEDITCATEGORYIMPORT
                    paramList.Add(new KeyObj("@classId", id));

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