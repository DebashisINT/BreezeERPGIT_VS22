using DataAccessLayer;
using DataAccessLayer.Model;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollLeaveStructure
{
    public class LeaveStructureLogic : ILeaveStructureLogic
    {
        public void LeaveStructureModify(LeaveStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_LeaveStructureModify", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (Convert.ToString(model.StructureID) == "")
                {
                    cmd.Parameters.AddWithValue("@Action", "AddLeaveStructure");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Action", "EditLeaveStructure");
                    cmd.Parameters.AddWithValue("@StructureID", model.StructureID);
                }
                cmd.Parameters.AddWithValue("@LeaveStructureName", model.StructureName);
                cmd.Parameters.AddWithValue("@LeaveStructureCode", model.StructureCode);
                cmd.Parameters.AddWithValue("@LeavePeriodFrom", model.FromDate);
                cmd.Parameters.AddWithValue("@LeavePeriodTo", model.ToDate);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["userid"]));

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@ReturnDocID", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnDocID"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                StructureID = Convert.ToString(cmd.Parameters["@ReturnDocID"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public void LeaveDefinationModify(LeaveStructureEngine model, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("proll_LeaveStructureModify", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Convert.ToString(model.LeaveID) == "")
                    {
                        cmd.Parameters.AddWithValue("@Action", "AddLeaveDefination");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Action", "EditLeaveDefination");
                        cmd.Parameters.AddWithValue("@LeaveID", model.LeaveID);
                    }

                    //cmd.Parameters.AddWithValue("@Action", "AddLeaveDefination");
                    cmd.Parameters.AddWithValue("@LeaveStructureID", model.StructureID);
                    cmd.Parameters.AddWithValue("@LeaveName", model.LeaveName);
                    cmd.Parameters.AddWithValue("@LeaveCode", model.LeaveCode);
                    cmd.Parameters.AddWithValue("@LeaveApplicableFor", model.ApplicableForCode);
                    cmd.Parameters.AddWithValue("@LeaveType", model.LeaveTypeCode);
                    cmd.Parameters.AddWithValue("@Is_IncludeWeeklyOff", model.is_WeeklyOff);
                    cmd.Parameters.AddWithValue("@Is_IncludePublicHoliday", model.is_IncludePublicHoliday);
                    cmd.Parameters.AddWithValue("@HolidayRule", model.HolidayRuleCode);
                    cmd.Parameters.AddWithValue("@Is_Eligibility_Probation", model.is_probationperiod);
                    cmd.Parameters.AddWithValue("@Is_Eligibility_Confirmation", model.is_onconfirmation);
                    cmd.Parameters.AddWithValue("@Is_Eligibility_NoticePeriod", model.is_noticeperiod);
                    cmd.Parameters.AddWithValue("@Is_Entitlement", model.is_chkEntitlement);
                    cmd.Parameters.AddWithValue("@LeaveAddition_Days", model.LeaveDaysAdd);
                    cmd.Parameters.AddWithValue("@LeaveAddition_BaseOnDays", model.DaysPer);
                    cmd.Parameters.AddWithValue("@LeaveAddition_BaseOnCalander", model.ddlEntitlementBase);
                    //cmd.Parameters.AddWithValue("@Is_Encashable", model.is_encashable);
                    cmd.Parameters.AddWithValue("@Is_AdjustOtherLeave", model.is_adjstleave);
                    cmd.Parameters.AddWithValue("@Is_CarryForward", model.is_carryforward);
                    cmd.Parameters.AddWithValue("@CarryForward_MaxDays", model.DaysForMaximum);
                    cmd.Parameters.AddWithValue("@CarryForward_BaseYear", model.Years);
                    cmd.Parameters.AddWithValue("@Is_DependServiceLength", model.is_lengthofservice);
                    cmd.Parameters.AddWithValue("@ServiceLength_Years", model.ServiceYears);
                    cmd.Parameters.AddWithValue("@ServiceLength_Months", model.ServiceMonths);
                    cmd.Parameters.AddWithValue("@ServiceLength_Days", model.ServiceDays);
                    cmd.Parameters.AddWithValue("@Is_Period_Basis", model.is_periodbasis);
                    //cmd.Parameters.AddWithValue("@Is_Weekly_Off_Perperiod", model.is_weeklyoffperperiod);
                    //cmd.Parameters.AddWithValue("@Is_Public_Holiday_Period", model.is_publicholidayinperiod);
                    //cmd.Parameters.AddWithValue("@Is_Carry_Forward_Next_Period", model.is_carryforwardtonextperiod);

                    cmd.Parameters.AddWithValue("@WeeklyOffDays", model.WeeklyOffDays);

                    if (Convert.ToString(model.BasicCode) == "WO")
                    {
                        cmd.Parameters.AddWithValue("@Is_Weekly_Off_Perperiod", 1);
                        cmd.Parameters.AddWithValue("@Is_Public_Holiday_Period", 0);
                    }
                    else if (Convert.ToString(model.BasicCode) == "PH")
                    {
                        cmd.Parameters.AddWithValue("@Is_Weekly_Off_Perperiod", 0);
                        cmd.Parameters.AddWithValue("@Is_Public_Holiday_Period", 1);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Is_Weekly_Off_Perperiod", 0);
                        cmd.Parameters.AddWithValue("@Is_Public_Holiday_Period", 0);
                    }

                    cmd.Parameters.AddWithValue("@Holiday_ID", model._PHoliday);

                    if (Convert.ToString(model.Unavailed) == "EN")
                    {
                        cmd.Parameters.AddWithValue("@Is_Encashable", 1);
                        cmd.Parameters.AddWithValue("@Is_Carry_Forward_Next_Period", 0);
                    }
                    else if (Convert.ToString(model.Unavailed) == "NP")
                    {
                        cmd.Parameters.AddWithValue("@Is_Encashable", 0);
                        cmd.Parameters.AddWithValue("@Is_Carry_Forward_Next_Period", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Is_Encashable", 0);
                        cmd.Parameters.AddWithValue("@Is_Carry_Forward_Next_Period", 0);
                    }                  

                    cmd.Parameters.AddWithValue("@SamePeriod", model._SamePeriod);
                    cmd.Parameters.AddWithValue("@EndLeavePeriod", model._EndLeavePeriod);                     
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["userid"]));

                    cmd.Parameters.AddWithValue("@Fixed", model.chkFixed);
                    cmd.Parameters.AddWithValue("@Number", model.txtNumber);
                    cmd.Parameters.AddWithValue("@ProrataBasis", model.ChkProrataBasis);
                    cmd.Parameters.AddWithValue("@Credited", model.dtCredited);
                    cmd.Parameters.AddWithValue("@NotCrWeekoffDays", model.chkNotCrWeekoffDays);
                    cmd.Parameters.AddWithValue("@NotCrPublicHolidays", model.chkNotCrPublicHolidays);
                    cmd.Parameters.AddWithValue("@Is_Entitled", model.chkEntitled);
                    cmd.Parameters.AddWithValue("@Entitled", model.ddlEntitled);
                    cmd.Parameters.AddWithValue("@Is_EligeableAvail", model.chkEligeableAvail);
                    cmd.Parameters.AddWithValue("@EligeableAvail", model.ddlEligeableAvail);
                    cmd.Parameters.AddWithValue("@ApplyFor", model.chkApplyFor);
                    cmd.Parameters.AddWithValue("@ApplyForMin", model.txtApplyForMin);
                    cmd.Parameters.AddWithValue("@ApplyForMax", model.txtApplyForMax);
                    cmd.Parameters.AddWithValue("@FixdCF", model.chkFixdCF);
                    cmd.Parameters.AddWithValue("@CarryUpto", model.ddlCarryUpto);
                    cmd.Parameters.AddWithValue("@MaximumBal", model.chkMaximumBal);
                    cmd.Parameters.AddWithValue("@MaximumNumber", model.txtMaximumNumber);
                    cmd.Parameters.AddWithValue("@AllowLeaveZeroBalance", model.chkAllowLeaveZeroBalance);
                    cmd.Parameters.AddWithValue("@UptoDays", model.txtUptoDays);             
                    
                    cmd.Parameters.AddWithValue("@UptoMonths", model.txtUptoMonths);
                    cmd.Parameters.AddWithValue("@UptoYears", model.txtUptoYears);
                    cmd.Parameters.AddWithValue("@FixedEncashable", model.FixedEncashable);
                    cmd.Parameters.AddWithValue("@EncashableAt", model.ddlEncashableAt);
                    cmd.Parameters.AddWithValue("@EncashableBalance", model.ddlEncashableBalance);
                    cmd.Parameters.AddWithValue("@ConsiderBeforeCF", model.ConsiderBeforeCF);                
                    
                    cmd.Parameters.AddWithValue("@ConjuncAllowedOtherLeaves", model.ConjuncAllowedOtherLeaves);
                    cmd.Parameters.AddWithValue("@AdjustableOtherLeaves", model.chkAdjustableOtherLeaves);



                    cmd.Parameters.AddWithValue("@IS_AutoAdjusted", model.is_AutoAdjusted);
                    cmd.Parameters.AddWithValue("@IS_AdjustLeaveApproval", model.is_AdjustLeaveApproval);
                    cmd.Parameters.AddWithValue("@IS_AdjustLeaveConsider", model.is_AdjustLeaveConsider);    



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

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public LeaveStructureEngine GetLeaveStructureDefination(string LeaveStructureID)
        {
            DataTable _getLeaveDetails = new DataTable();
            LeaveStructureEngine objModel = new LeaveStructureEngine();
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_LeaveStructureModify";
                paramList.Add(new KeyObj("@Action", "GetLeaveStructureDefination"));
                paramList.Add(new KeyObj("@StructureID", LeaveStructureID));
                execProc.param = paramList;
                _getLeaveDetails = execProc.ExecuteProcedureGetTable();

                if (_getLeaveDetails.Rows.Count > 0 && _getLeaveDetails != null)
                {
                    objModel.StructureID = _getLeaveDetails.Rows[0]["LeaveStructureID"].ToString();
                    objModel.StructureName = _getLeaveDetails.Rows[0]["LeaveStructureName"].ToString();
                    objModel.StructureCode = _getLeaveDetails.Rows[0]["LeaveStructureCode"].ToString();
                    if (_getLeaveDetails.Rows[0]["LeavePeriodFrom"] != null)
                        objModel.FromDate = Convert.ToDateTime(_getLeaveDetails.Rows[0]["LeavePeriodFrom"].ToString());
                    if (_getLeaveDetails.Rows[0]["LeavePeriodTo"] != null)
                        objModel.ToDate = Convert.ToDateTime(_getLeaveDetails.Rows[0]["LeavePeriodTo"].ToString());



                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return objModel;
        }

        public LeaveStructureEngine EditLeaveDefination(string LeaveStructureID, string LeaveId, ref int strIsComplete, ref string strMessage)
        {
            DataTable _getLeaveDetails = new DataTable();
            LeaveStructureEngine objModel = new LeaveStructureEngine();
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_LeaveStructureModify";
                paramList.Add(new KeyObj("@Action", "SelectLeaveDefination"));
                paramList.Add(new KeyObj("@StructureID", LeaveStructureID));
                paramList.Add(new KeyObj("@LeaveID", LeaveId));
                paramList.Add(new KeyObj("@ReturnValue", strIsComplete, true));
                paramList.Add(new KeyObj("@ReturnMessage", strMessage, true));
                execProc.param = paramList;
                _getLeaveDetails = execProc.ExecuteProcedureGetTable();
                strIsComplete = Convert.ToInt32(execProc.outputPara[0].value);
                strMessage = Convert.ToString(execProc.outputPara[1].value);
                paramList.Clear();

                if (_getLeaveDetails.Rows.Count > 0 && _getLeaveDetails != null)
                {
                    objModel.LeaveID = _getLeaveDetails.Rows[0]["LeaveID"].ToString();
                    objModel.StructureID = _getLeaveDetails.Rows[0]["LeaveStructureID"].ToString();
                    objModel.LeaveName = _getLeaveDetails.Rows[0]["LeaveName"].ToString();
                    objModel.LeaveCode = _getLeaveDetails.Rows[0]["LeaveCode"].ToString();
                    objModel.ApplicableForCode = _getLeaveDetails.Rows[0]["LeaveApplicableFor"].ToString();
                    objModel.LeaveTypeCode = _getLeaveDetails.Rows[0]["LeaveType"].ToString();
                    objModel.is_WeeklyOff = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_IncludeWeeklyOff"].ToString());
                    objModel.is_IncludePublicHoliday = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_IncludePublicHoliday"].ToString());
                    objModel.HolidayRuleCode = _getLeaveDetails.Rows[0]["HolidayRule"].ToString();

                    objModel.is_probationperiod = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Eligibility_Probation"].ToString());
                    objModel.is_onconfirmation = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Eligibility_Confirmation"].ToString());
                    objModel.is_noticeperiod = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Eligibility_NoticePeriod"].ToString());
                    objModel.is_chkEntitlement = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Entitlement"].ToString());
                    objModel.LeaveDaysAdd = _getLeaveDetails.Rows[0]["LeaveAddition_Days"].ToString();
                    objModel.DaysPer = _getLeaveDetails.Rows[0]["LeaveAddition_BaseOnDays"].ToString();
                    objModel.ddlEntitlementBase = _getLeaveDetails.Rows[0]["LeaveAddition_BaseOnCalander"].ToString();
                    //objModel.is_encashable = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Encashable"].ToString());
                    objModel.is_adjstleave = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_AdjustOtherLeave"].ToString());
                    objModel.is_carryforward = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_CarryForward"].ToString());
                    objModel.DaysForMaximum = _getLeaveDetails.Rows[0]["CarryForward_MaxDays"].ToString();
                    objModel.Years = _getLeaveDetails.Rows[0]["CarryForward_BaseYear"].ToString();
                    objModel.is_lengthofservice = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_DependServiceLength"].ToString());
                    objModel.ServiceYears = _getLeaveDetails.Rows[0]["ServiceLength_Years"].ToString();
                    objModel.ServiceMonths = _getLeaveDetails.Rows[0]["ServiceLength_Months"].ToString();
                    objModel.ServiceDays = _getLeaveDetails.Rows[0]["ServiceLength_Days"].ToString();
                    objModel.is_periodbasis = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_PeriodBasis"].ToString());
                    //objModel.is_weeklyoffperperiod = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_WeeklyOffPerperiod"].ToString());
                    //objModel.is_publicholidayinperiod = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_PublicHolidayPeriod"].ToString());
                    //objModel.is_carryforwardtonextperiod = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_CarryForwardNextPeriod"].ToString());

                    if (Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_WeeklyOffPerperiod"].ToString())==true)
                    {
                        objModel.BasicCode = "WO";
                    }
                    else if (Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_PublicHolidayPeriod"].ToString()) == true)
                    {
                        objModel.BasicCode = "PH";
                    }
                    if (Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_CarryForwardNextPeriod"].ToString())==true)                        
                    {
                        objModel.Unavailed = "NP";
                    }
                    else if (Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Encashable"].ToString())==true)
                    {
                        objModel.Unavailed = "EN";
                    }                  
                    objModel._PHoliday = _getLeaveDetails.Rows[0]["Holiday_ID"].ToString();
                    objModel._SamePeriod = _getLeaveDetails.Rows[0]["SamePeriod"].ToString();
                    objModel._EndLeavePeriod = _getLeaveDetails.Rows[0]["EndLeavePeriod"].ToString();                 

                    //objModel.is_carryforwardtonextperiod = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_CarryForwardNextPeriod"].ToString());
                    objModel.WeeklyOffDays = Convert.ToString(_getLeaveDetails.Rows[0]["WeeklyOffDays"]);

                    objModel.chkFixed=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Fixed"].ToString());
                    objModel.txtNumber=Convert.ToString(_getLeaveDetails.Rows[0]["Number"].ToString());
                    objModel.ChkProrataBasis=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_ProrataBasis"].ToString());
                    objModel.dtCredited=Convert.ToString(_getLeaveDetails.Rows[0]["Credited"].ToString());
                    objModel.chkNotCrWeekoffDays=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_DoNotCreditWeekoffDays"].ToString());
                    objModel.chkNotCrPublicHolidays=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_DoNotCreditPublicHolidays"].ToString());
                    objModel.chkEntitled=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_Entitled"].ToString());
                    objModel.ddlEntitled=Convert.ToString(_getLeaveDetails.Rows[0]["Entitled"].ToString());
                    objModel.chkEligeableAvail=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_EligeableAvail"].ToString());
                    objModel.ddlEligeableAvail=Convert.ToString(_getLeaveDetails.Rows[0]["EligeableToAvail"].ToString());
                    objModel.chkApplyFor=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_ApplyFor"].ToString());
                 
                    objModel.txtApplyForMin=Convert.ToString(_getLeaveDetails.Rows[0]["ApplyForMin"].ToString());
                    objModel.txtApplyForMax=Convert.ToString(_getLeaveDetails.Rows[0]["ApplyForMax"].ToString());
                    objModel.chkFixdCF=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_FixedCarryForward"].ToString());
                    objModel.ddlCarryUpto=Convert.ToString(_getLeaveDetails.Rows[0]["FixedCarryForward"].ToString());
                    objModel.chkMaximumBal=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_MaximumBalance"].ToString());
                    objModel.txtMaximumNumber=Convert.ToString(_getLeaveDetails.Rows[0]["MaximumNumber"].ToString());
                    objModel.chkAllowLeaveZeroBalance=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_AllowLeaveCaseZeroBalance"].ToString());
                    objModel.txtUptoDays=Convert.ToString(_getLeaveDetails.Rows[0]["CFUptoDays"].ToString());
                    objModel.txtUptoMonths=Convert.ToString(_getLeaveDetails.Rows[0]["CFUptoMonths"].ToString());
                    objModel.txtUptoYears=Convert.ToString(_getLeaveDetails.Rows[0]["CFUptoYears"].ToString());
                    objModel.FixedEncashable=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_FixedEncashable"].ToString());

                    objModel.ddlEncashableAt=Convert.ToString(_getLeaveDetails.Rows[0]["EncashableOne"].ToString());
                    objModel.ddlEncashableBalance=Convert.ToString(_getLeaveDetails.Rows[0]["EncashableTwo"].ToString());
                    objModel.ConsiderBeforeCF=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_ConsiderBeforeCarryForward"].ToString());
                    objModel.ConjuncAllowedOtherLeaves=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_ConjunctionAllowedLeaves"].ToString());
                    objModel.chkAdjustableOtherLeaves=Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_AdjustableOtherLeaves"].ToString());


                    objModel.is_AutoAdjusted = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_AutoAdjusted"].ToString());
                    objModel.is_AdjustLeaveApproval = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_ApprovalRequired"].ToString());
                    objModel.is_AdjustLeaveConsider = Convert.ToBoolean(_getLeaveDetails.Rows[0]["Is_ConsiderWD"].ToString());

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return objModel;
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
                    execProc.ProcedureName = "proll_LeaveStructureModify";
                    paramList.Add(new KeyObj("@Action", ActionType));
                    //paramList.Add(new KeyObj("@user_id", user_id));//ADDEDITCATEGORYIMPORT
                    if (ActionType == "DeleteStructure")
                    {
                        paramList.Add(new KeyObj("@StructureID", id));
                    }
                    else if (ActionType == "DeleteLeaveDefination")
                    {
                        paramList.Add(new KeyObj("@LeaveID", id));
                    }

                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnValue", ReturnCode, true));
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