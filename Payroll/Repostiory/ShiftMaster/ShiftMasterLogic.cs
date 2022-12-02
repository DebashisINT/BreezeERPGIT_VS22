using DataAccessLayer;
using DataAccessLayer.Model;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.ShiftMaster
{
    public class ShiftMasterLogic : IShiftMasterLogic
    {
        public void ShiftMasterSubmit(ShiftMasterEngine model, DataTable dtLateRule, DataTable dtEarlyLeavingRule,DataTable dtRotationalShiftRule, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_ShiftMaster_AddModify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (Convert.ToString(model.Shift_Id) == "")
                {
                    cmd.Parameters.AddWithValue("@Action", "Add");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Action", "Edit");
                    cmd.Parameters.AddWithValue("@ShiftId", model.Shift_Id);
                }
                cmd.Parameters.AddWithValue("@ShiftCode", model.Shift_Code);
                cmd.Parameters.AddWithValue("@ShiftName", model.Shift_Name);

                if (model.Shift_Type == "0")
                {
                    cmd.Parameters.AddWithValue("@ShiftStartTime", model.Shift_Start);
                    cmd.Parameters.AddWithValue("@ShiftEndTime", model.Shift_End);
                    //cmd.Parameters.AddWithValue("@ConsiderLateAfter", model.Consider_LateAfter);
                    cmd.Parameters.AddWithValue("@ConsiderAttendanceAfter", model.Consider_AttendanceAfter);
                    cmd.Parameters.AddWithValue("@ConsiderHalfDayAfter", model.Consider_HalfDayAfter);
                    cmd.Parameters.AddWithValue("@Consider_HalfDayBefore", model.Consider_HalfDayBefore);
                    cmd.Parameters.AddWithValue("@OneLateAfter", model.OneLateAfter);
                    cmd.Parameters.AddWithValue("@TwoLatesAfter", model.TwoLateAfter);
                    cmd.Parameters.AddWithValue("@ThreeLatesAfter", model.ThreeLateAfter);

                    cmd.Parameters.AddWithValue("@ShiftBreak", model.Break);


                    //Rev Surojit 28-06-2019
                    cmd.Parameters.AddWithValue("@ShiftDays", model.Shift_EndDay);
                    cmd.Parameters.AddWithValue("@ConsiderFullDay", model.Consider_AttendanceAfter);
                    cmd.Parameters.AddWithValue("@HalfDayInTimeAfter", model.InTimeAfter);
                    cmd.Parameters.AddWithValue("@HalfDayOutTimeBefore", model.OutTimeAfter);
                    cmd.Parameters.AddWithValue("@HalfDayInOutTimeDiff", model.InOutTimeDiff);
                    cmd.Parameters.AddWithValue("@LateRuleDeductDays", model.LateDayCount);
                    cmd.Parameters.AddWithValue("@LateRuleDaysLate", model.LateRuleCount);
                    cmd.Parameters.AddWithValue("@LateRuleDaysAfterLate", model.LateRuleAfterCount);
                    if (dtLateRule.Rows.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@udtLateRule", dtLateRule);
                    }
                    if (dtEarlyLeavingRule.Rows.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@udtEarlyLeavingRule", dtEarlyLeavingRule);
                    }
                }
                //Rev Surojit 28-06-2019

                //cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt64(HttpContext.Current.Session["userid"]));

                //Rev 23-07-2019 Surojit 0020560: In the Shift Master provide an option for Rotating Shift
                if (model.Shift_Type == "1")
                {
                    cmd.Parameters.AddWithValue("@Shift_Type", model.Shift_Type);
                    cmd.Parameters.AddWithValue("@Shift_Time", model.Shift_Time);
                    cmd.Parameters.AddWithValue("@Shift_Break_Time", model.Shift_Break_Time);

                    if (dtRotationalShiftRule.Rows.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@udtRotationalRule", dtRotationalShiftRule);
                    }
                }
                //Rev 23-07-2019 Surojit 0020560: In the Shift Master provide an option for Rotating Shift

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public ShiftMasterEngine GetShiftById(string ShiftId, ref int strIsComplete, ref string strMessage)
        {
            DataTable _getShiftDetails = new DataTable();
            ShiftMasterEngine objModel = new ShiftMasterEngine();
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_ShiftMaster_AddModify";
                paramList.Add(new KeyObj("@Action", "GetShiftById"));
                paramList.Add(new KeyObj("@ShiftId", ShiftId));
                paramList.Add(new KeyObj("@ReturnMessage", strMessage, true));
                paramList.Add(new KeyObj("@ReturnValue", strIsComplete, true));
                execProc.param = paramList;
                _getShiftDetails = execProc.ExecuteProcedureGetTable();
                strMessage = Convert.ToString(execProc.outputPara[0].value);
                strIsComplete = Convert.ToInt32(execProc.outputPara[1].value);
                
                
                paramList.Clear();

                if (_getShiftDetails.Rows.Count > 0 && _getShiftDetails != null)
                {
                    objModel.Shift_Code = _getShiftDetails.Rows[0]["ShiftCode"].ToString();
                    objModel.Shift_Id = _getShiftDetails.Rows[0]["ShiftID"].ToString();
                    objModel.Shift_Name = _getShiftDetails.Rows[0]["ShiftName"].ToString();
                    objModel.Shift_Start = _getShiftDetails.Rows[0]["ShiftStartTime"].ToString();
                    objModel.Shift_End = _getShiftDetails.Rows[0]["ShiftEndTime"].ToString();
                    //objModel.Consider_LateAfter = _getShiftDetails.Rows[0]["ConsiderLateAfter"].ToString();
                    objModel.Consider_AttendanceAfter = _getShiftDetails.Rows[0]["ConsiderAttendanceAfter"].ToString();
                    objModel.Consider_HalfDayAfter = _getShiftDetails.Rows[0]["ConsiderHalfDayAfter"].ToString();
                    objModel.Break = _getShiftDetails.Rows[0]["ShiftBreak"].ToString();
                    objModel.Consider_HalfDayBefore = _getShiftDetails.Rows[0]["ConsiderHalfDayBefore"].ToString();
                    objModel.OneLateAfter = _getShiftDetails.Rows[0]["OneLateAfter"].ToString();
                    objModel.TwoLateAfter = _getShiftDetails.Rows[0]["TwoLatesAfter"].ToString();
                    objModel.ThreeLateAfter = _getShiftDetails.Rows[0]["ThreeLatesAfter"].ToString();


                    objModel.Shift_EndDay = _getShiftDetails.Rows[0]["ShiftDays"].ToString();
                    objModel.Consider_AttendanceAfter = _getShiftDetails.Rows[0]["ConsiderFullDay"].ToString();
                    objModel.InTimeAfter = _getShiftDetails.Rows[0]["HalfDayInTimeAfter"].ToString();
                    objModel.OutTimeAfter = _getShiftDetails.Rows[0]["HalfDayOutTimeBefore"].ToString();
                    objModel.InOutTimeDiff = _getShiftDetails.Rows[0]["HalfDayInOutTimeDiff"].ToString();

                    objModel.LateDayCount = _getShiftDetails.Rows[0]["LateRuleDeductDays"].ToString();
                    objModel.LateRuleCount = _getShiftDetails.Rows[0]["LateRuleDaysLate"].ToString();
                    objModel.LateRuleAfterCount = _getShiftDetails.Rows[0]["LateRuleDaysAfterLate"].ToString();

                    objModel.Shift_Type = _getShiftDetails.Rows[0]["Shift_Type"].ToString();
                    objModel.Shift_Time = _getShiftDetails.Rows[0]["Shift_Time"].ToString();
                    objModel.Shift_Break_Time = _getShiftDetails.Rows[0]["Shift_Break_Time"].ToString();

                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return objModel;
        }

        public ShiftMasterEngine LeavingLateShiftByID(string ShiftId, ref int strIsComplete, ref string strMessage)
        {
            DataSet _getShiftDetails = new DataSet();
            ShiftMasterEngine objModel = new ShiftMasterEngine();
            List<EarlyLeaving> EL = new List<EarlyLeaving>();
            List<LateRule> LR = new List<LateRule>();
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_ShiftMaster_AddModify";
                paramList.Add(new KeyObj("@Action", "GetLeavingLateShiftByID"));
                paramList.Add(new KeyObj("@ShiftId", ShiftId));
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
                }

                objModel.LateRules = LR;
                objModel.EarlyLeavingRules = EL;
                objModel.ResponseMessage = strMessage;
            }
            catch (Exception ex)
            {
                throw;
            }

            return objModel;
        }

        public string Delete(string ActionType, string id, ref int strIsComplete)
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
                    execProc.ProcedureName = "proll_ShiftMaster_AddModify";
                    paramList.Add(new KeyObj("@Action", ActionType));
                    //paramList.Add(new KeyObj("@user_id", user_id));//ADDEDITCATEGORYIMPORT
                    paramList.Add(new KeyObj("@ShiftId", id));

                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnValue", strIsComplete, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    strIsComplete = Convert.ToInt32(execProc.outputPara[1].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }


        public ShiftMasterEngine RotationalShiftShiftByID(string ShiftId, ref int strIsComplete, ref string strMessage)
        {
            DataSet _getShiftDetails = new DataSet();
            ShiftMasterEngine objModel = new ShiftMasterEngine();
            List<RotationalShiftRule> RS = new List<RotationalShiftRule>();
            
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_ShiftMaster_AddModify";
                paramList.Add(new KeyObj("@Action", "GetRotationalShiftShiftByID"));
                paramList.Add(new KeyObj("@ShiftId", ShiftId));
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
                        RotationalShiftRule obj = new RotationalShiftRule();
                        obj.RotationalShiftStart = Convert.ToString(dr["ShiftStart"]);
                        obj.RotationalShiftEnd = Convert.ToString(dr["ShiftEnd"]);
                        obj.TimeDuration = Convert.ToString(dr["TimeDuration"]);
                        obj.ShiftName = Convert.ToString(dr["ShiftName"]);
                        RS.Add(obj);
                    }

                }

                objModel.RotationalShift = RS;
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