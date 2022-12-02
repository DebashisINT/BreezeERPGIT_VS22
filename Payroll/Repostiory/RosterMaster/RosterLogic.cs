using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Payroll.Models;
using System.Data;
using DataAccessLayer;
using DataAccessLayer.Model;

namespace Payroll.Repostiory.RosterMaster
{
    public class RosterLogic:IRosterLogic
    {
        public void save(Roster RosterHeaderDetails, DataTable dtLateRule, DataTable dtEarlyLeavingRule, DataTable dtHalfDayINOUTRules, ref int strIsComplete, ref string strMessage)
        {

            string action = string.Empty;
            DataTable Rosterdtls = new DataTable();
           

            try
            {

                Rosterdtls.Columns.Add("Day", typeof(string));
                Rosterdtls.Columns.Add("ShiftID", typeof(string));


                foreach (RosterDetails dtls in RosterHeaderDetails.dtls)
                {
                    DataRow dr = Rosterdtls.NewRow();
                    dr["Day"] = dtls.Day;
                    if (dtls.ShiftId.ToLower().Contains('|'))
                    {
                        string[] List = dtls.ShiftId.Split('|');
                        dr["ShiftID"] = List[0] + "~" + List[1];
                    }
                    else
                    {
                        dr["ShiftID"] = dtls.ShiftId + "~" + "0";
                    }
                   
                    Rosterdtls.Rows.Add(dr);
                }

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_RosterMasterAddModify";
                    if (RosterHeaderDetails.RosterID != null && RosterHeaderDetails.RosterID !="")
                    {
                        paramList.Add(new KeyObj("@Action", "EditRoster"));
                        paramList.Add(new KeyObj("@RosterID", RosterHeaderDetails.RosterID));
                    }
                    else
                    {
                        paramList.Add(new KeyObj("@Action", "AddRoster"));
                    }
                  
                    paramList.Add(new KeyObj("@user_id", user_id));
                    paramList.Add(new KeyObj("@RosterCode", RosterHeaderDetails.RosterCode));
                    paramList.Add(new KeyObj("@RosterName", RosterHeaderDetails.RosterName));
                    paramList.Add(new KeyObj("@ClassID", RosterHeaderDetails._PClassId));
                    paramList.Add(new KeyObj("@RosterType", RosterHeaderDetails._WeekDay));
                    if (RosterHeaderDetails._WeekDay=="P")
                    {
                     paramList.Add(new KeyObj("@YYMM", RosterHeaderDetails._PeriodID));
                    }

                    //Surojit 26-07-2019
                    paramList.Add(new KeyObj("@ShiftRule", RosterHeaderDetails.ShiftRule));
                    paramList.Add(new KeyObj("@ConsiderFullDay", RosterHeaderDetails.Consider_AttendanceAfter));
                    paramList.Add(new KeyObj("@HalfDayInTimeAfter", RosterHeaderDetails.InTimeAfter));
                    paramList.Add(new KeyObj("@HalfDayOutTimeBefore", RosterHeaderDetails.OutTimeAfter));
                    paramList.Add(new KeyObj("@HalfDayInOutTimeDiff", RosterHeaderDetails.InOutTimeDiff));
                    paramList.Add(new KeyObj("@LateRuleDeductDays", RosterHeaderDetails.LateDayCount));
                    paramList.Add(new KeyObj("@LateRuleDaysLate", RosterHeaderDetails.LateRuleCount));
                    paramList.Add(new KeyObj("@LateRuleDaysAfterLate", RosterHeaderDetails.LateRuleAfterCount));
                    if (dtLateRule != null)
                    {
                        if (dtLateRule.Rows.Count > 0)
                        {
                            paramList.Add(new KeyObj("@udtLateRule", dtLateRule));
                        }
                    }
                    if (dtEarlyLeavingRule != null)
                    {
                        if (dtEarlyLeavingRule.Rows.Count > 0)
                        {
                            paramList.Add(new KeyObj("@udtEarlyLeavingRule", dtEarlyLeavingRule));
                        }
                    }
                    //Surojit 26-07-2019
                    if (dtHalfDayINOUTRules != null)
                    {
                        if (dtHalfDayINOUTRules.Rows.Count > 0)
                        {
                            paramList.Add(new KeyObj("@udtHalfDayINOUTRule", dtHalfDayINOUTRules));
                        }
                    }

                    paramList.Add(new KeyObj("@PARAMTABLE", Rosterdtls));
                    paramList.Add(new KeyObj("@ReturnMessage", strMessage, true));
                    paramList.Add(new KeyObj("@ReturnValue", strIsComplete, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    strMessage = Convert.ToString(execProc.outputPara[0].value);
                    strIsComplete = Convert.ToInt32(execProc.outputPara[1].value); Convert.ToString(execProc.outputPara[1].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Roster getRosterDetailsById(string RosterId)
        {
            DataSet _getRosterDtls = new DataSet();
            Roster obj = new Roster();
            List<RosterDetails> items = new List<RosterDetails>();
           
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_RosterMasterAddModify";

                paramList.Add(new KeyObj("@Action", "GetRosterById"));


                paramList.Add(new KeyObj("@RosterID", RosterId));

                execProc.param = paramList;
                _getRosterDtls = execProc.ExecuteProcedureGetDataSet();

                if (_getRosterDtls.Tables[0].Rows.Count > 0)
                {
                    obj.RosterID = _getRosterDtls.Tables[0].Rows[0]["RosterID"].ToString();
                    obj.RosterCode = _getRosterDtls.Tables[0].Rows[0]["RosterCode"].ToString();
                    obj.RosterName = _getRosterDtls.Tables[0].Rows[0]["RosterName"].ToString();
                    obj._WeekDay = _getRosterDtls.Tables[0].Rows[0]["RosterType"].ToString();
                    obj._PeriodID = _getRosterDtls.Tables[0].Rows[0]["YYMM"].ToString();
                    obj._PClassId = _getRosterDtls.Tables[0].Rows[0]["ClassID"].ToString();
                    obj._Shift_Mon_ID = _getRosterDtls.Tables[0].Rows[0]["Mon"].ToString() + (_getRosterDtls.Tables[0].Rows[0]["MonRotationalShiftID"].ToString() == "0" ? "" : "|" + _getRosterDtls.Tables[0].Rows[0]["MonRotationalShiftID"].ToString());
                    obj._Shift_Tue_ID = _getRosterDtls.Tables[0].Rows[0]["Tue"].ToString() + (_getRosterDtls.Tables[0].Rows[0]["TueRotationalShiftID"].ToString() == "0" ? "" : "|" + _getRosterDtls.Tables[0].Rows[0]["TueRotationalShiftID"].ToString());
                    obj._Shift_Wed_ID = _getRosterDtls.Tables[0].Rows[0]["Wed"].ToString() + (_getRosterDtls.Tables[0].Rows[0]["WedRotationalShiftID"].ToString() == "0" ? "" : "|" + _getRosterDtls.Tables[0].Rows[0]["WedRotationalShiftID"].ToString());
                    obj._Shift_Thur_ID = _getRosterDtls.Tables[0].Rows[0]["Thur"].ToString() + (_getRosterDtls.Tables[0].Rows[0]["ThurRotationalShiftID"].ToString() == "0" ? "" : "|" + _getRosterDtls.Tables[0].Rows[0]["ThurRotationalShiftID"].ToString());
                    obj._Shift_Fri_ID = _getRosterDtls.Tables[0].Rows[0]["Fri"].ToString() + (_getRosterDtls.Tables[0].Rows[0]["FriRotationalShiftID"].ToString() == "0" ? "" : "|" + _getRosterDtls.Tables[0].Rows[0]["FriRotationalShiftID"].ToString());
                    obj._Shift_Sat_ID = _getRosterDtls.Tables[0].Rows[0]["Sat"].ToString() + (_getRosterDtls.Tables[0].Rows[0]["SatRotationalShiftID"].ToString() == "0" ? "" : "|" + _getRosterDtls.Tables[0].Rows[0]["SatRotationalShiftID"].ToString());
                    obj._Shift_Sun_ID = _getRosterDtls.Tables[0].Rows[0]["Sun"].ToString() + (_getRosterDtls.Tables[0].Rows[0]["SunRotationalShiftID"].ToString() == "0" ? "" : "|" + _getRosterDtls.Tables[0].Rows[0]["SunRotationalShiftID"].ToString());

                    obj.ShiftRule = Convert.ToBoolean(_getRosterDtls.Tables[0].Rows[0]["ShiftRule"]);
                    obj.Consider_AttendanceAfter = _getRosterDtls.Tables[0].Rows[0]["ConsiderFullDay"].ToString();
                    obj.InTimeAfter = _getRosterDtls.Tables[0].Rows[0]["HalfDayInTimeAfter"].ToString();
                    obj.OutTimeAfter = _getRosterDtls.Tables[0].Rows[0]["HalfDayOutTimeBefore"].ToString();
                    obj.InOutTimeDiff = _getRosterDtls.Tables[0].Rows[0]["HalfDayInOutTimeDiff"].ToString();
                    obj.LateDayCount = _getRosterDtls.Tables[0].Rows[0]["LateRuleDeductDays"].ToString();

                    obj.LateRuleCount = _getRosterDtls.Tables[0].Rows[0]["LateRuleDaysLate"].ToString();
                    obj.LateRuleAfterCount = _getRosterDtls.Tables[0].Rows[0]["LateRuleDaysAfterLate"].ToString();
                    
                    foreach (DataRow dr in _getRosterDtls.Tables[1].Rows)
                    {

                        items.Add(new RosterDetails
                        {
                            Day = dr["RosterDate"].ToString(),
                            ShiftId = dr["ShiftID"].ToString() + (dr["RotationalShiftID"].ToString() == "0" ? "" : "|" + dr["RotationalShiftID"].ToString())
                           

                        });

                    }

                    obj.dtls = items;

                   


                }

            }
            catch (Exception ex)
            {

            }

            return obj;
        }

        public RosterModify RosterActionByID(String ActionType, String ID)
        {
            DataSet _getRosterDtls = new DataSet();
            RosterModify obj = new RosterModify();
            List<RosterDetails> items = new List<RosterDetails>();

            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_RosterMasterAddModify";

                paramList.Add(new KeyObj("@Action", ActionType));


                paramList.Add(new KeyObj("@RosterID", ID));

                execProc.param = paramList;
                _getRosterDtls = execProc.ExecuteProcedureGetDataSet();

                if (_getRosterDtls.Tables[0].Rows.Count > 0)
                {
                    obj.Success = Convert.ToBoolean(_getRosterDtls.Tables[0].Rows[0]["Success"]);
                    obj.Message = _getRosterDtls.Tables[0].Rows[0]["Message"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public ShiftMasterEngine LeavingLateShiftByID(string ID, ref int strIsComplete, ref string strMessage)
        {
            DataSet _getShiftDetails = new DataSet();
            ShiftMasterEngine objModel = new ShiftMasterEngine();
            List<EarlyLeaving> EL = new List<EarlyLeaving>();
            List<LateRule> LR = new List<LateRule>();
            List<HalfDayINOUT> HD = new List<HalfDayINOUT>();
            
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_RosterMasterAddModify";
                paramList.Add(new KeyObj("@Action", "GetLeavingLateShiftByID"));
                paramList.Add(new KeyObj("@RosterID", ID));
                paramList.Add(new KeyObj("@ReturnMessage", strMessage, true));
                paramList.Add(new KeyObj("@ReturnValue", strIsComplete, true));
                execProc.param = paramList;
                _getShiftDetails = execProc.ExecuteProcedureGetDataSet();
                strMessage = Convert.ToString(execProc.outputPara[0].value);
                strIsComplete = Convert.ToInt32(execProc.outputPara[1].value);


                paramList.Clear();

                if (_getShiftDetails.Tables.Count > 0 && _getShiftDetails != null)
                {
                    foreach (DataRow dr in _getShiftDetails.Tables[0].Rows)
                    {
                        LateRule obj = new LateRule();
                        obj.LateCount = Convert.ToInt32(dr["LateCount"]);
                        obj.InTimeAfter = Convert.ToString(dr["InTimeAfter"]);
                        LR.Add(obj);
                    }

                    foreach (DataRow dr in _getShiftDetails.Tables[1].Rows)
                    {
                        EarlyLeaving obj = new EarlyLeaving();
                        obj.LeavingHours = Convert.ToString(dr["LeavingHours"]);
                        obj.LeavingDays = Convert.ToInt32(dr["LeavingDays"]);
                        obj.Condition = Convert.ToString(dr["Condition"]);
                        EL.Add(obj);
                    }

                    foreach (DataRow dr in _getShiftDetails.Tables[2].Rows)
                    {
                        HalfDayINOUT obj = new HalfDayINOUT();
                        obj.HalfDayInHours = Convert.ToString(dr["HalfDayInHours"]);
                        obj.HalfDayOutHours = Convert.ToString(dr["HalfDayOutHours"]);
                        obj.AfterBefor1 = Convert.ToString(dr["AfterBefor1"]);
                        obj.AfterBefor2 = Convert.ToString(dr["AfterBefor2"]);
                        obj.slcondition1 = Convert.ToString(dr["Condition1"]);
                        obj.slcondition2 = Convert.ToString(dr["Condition2"]);
                        HD.Add(obj);                  
 
                   }
                }

                objModel.LateRules = LR;
                objModel.EarlyLeavingRules = EL;
                objModel.HalfDayINOUTRules = HD;
                objModel.ResponseMessage = strMessage;
            }
            catch (Exception ex)
            {
                throw;
            }

            return objModel;
        }
    }
}