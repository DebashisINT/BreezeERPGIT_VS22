using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class LeaveApplyController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ApplyLeave(leaveApplyInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction);;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APPLYLEVE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.Parameters.Add("@LEAVE_START_DATE", model.LeaveFrom);
                    sqlcmd.Parameters.Add("@LEAVE_END_DATE", model.LeaveTo);
                    sqlcmd.Parameters.Add("@LEAVE_TYPE", model.LeaveType);
                    sqlcmd.Parameters.Add("@LEAVE_REASON", model.LeaveReason);
                    sqlcmd.Parameters.Add("@SUPERVISOR_NAME", model.SupervisorName);
                    sqlcmd.Parameters.Add("@SUPERVISOR_EMAIL", model.SupervisorEmail);
                    sqlcmd.Parameters.Add("@SUPERVISOR_ID", model.SupervisorId);
                    sqlcmd.Parameters.Add("@EMPCODE", model.EMPCODE);
                    

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }
        //rev Pratik
        [HttpPost]
        public HttpResponseMessage ApplyLeaveEdit(leaveApplyInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APPLYLEVE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_LEAVE_EDIT");
                   // sqlcmd.Parameters.Add("@ID", model.id);
                    sqlcmd.Parameters.Add("@ID", model.UserId);
                    sqlcmd.Parameters.Add("@LEAVE_START_DATE", model.LeaveFrom);
                    sqlcmd.Parameters.Add("@LEAVE_END_DATE", model.LeaveTo);
                    sqlcmd.Parameters.Add("@LEAVE_TYPE", model.LeaveType);
                    sqlcmd.Parameters.Add("@LEAVE_REASON", model.LeaveReason);
                    sqlcmd.Parameters.Add("@SUPERVISOR_NAME", model.SupervisorName);
                    sqlcmd.Parameters.Add("@SUPERVISOR_EMAIL", model.SupervisorEmail);
                    sqlcmd.Parameters.Add("@SUPERVISOR_ID", model.SupervisorId);
                    sqlcmd.Parameters.Add("@EMPCODE", model.EMPCODE);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }
        //end of rev Pratik
        [HttpPost]
        public HttpResponseMessage ApplyWFH(WFHApplyInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction);;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APPLYWORKFROMHOME", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_WFH");
                    sqlcmd.Parameters.Add("@USER_ID", model.USER_ID);
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@WFH_STARTDATE", model.WFH_STARTDATE);
                    sqlcmd.Parameters.Add("@WFH_ENDDATE", model.WFH_ENDDATE);
                    sqlcmd.Parameters.Add("@WFH_REASON", model.WFH_REASON);
                    sqlcmd.Parameters.Add("@WORK_PLAN", model.WORK_PLAN);
                    sqlcmd.Parameters.Add("@SUPERVISOR_NAME", model.SUPERVISOR_NAME);
                    sqlcmd.Parameters.Add("@SUPERVISOR_EMAIL", model.SUPERVISOR_EMAIL);
                    sqlcmd.Parameters.Add("@SUPERVISOR_ID", model.SUPERVISOR_ID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        //rev Pratik

        [HttpPost]
        public HttpResponseMessage ApplyWFHEdit(WFHApplyInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APPLYWORKFROMHOME", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_WFH_EDIT");
                    sqlcmd.Parameters.Add("@ID", model.USER_ID);
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@WFH_STARTDATE", model.WFH_STARTDATE);
                    sqlcmd.Parameters.Add("@WFH_ENDDATE", model.WFH_ENDDATE);
                    sqlcmd.Parameters.Add("@WFH_REASON", model.WFH_REASON);
                    sqlcmd.Parameters.Add("@WORK_PLAN", model.WORK_PLAN);
                    sqlcmd.Parameters.Add("@SUPERVISOR_NAME", model.SUPERVISOR_NAME);
                    sqlcmd.Parameters.Add("@SUPERVISOR_EMAIL", model.SUPERVISOR_EMAIL);
                    sqlcmd.Parameters.Add("@SUPERVISOR_ID", model.SUPERVISOR_ID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        //End of rev Pratik

        [HttpPost]
        public HttpResponseMessage AddInTime(InOutInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ATTENDENCE", sqlcon);
                    //sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.Parameters.Add("@ACTION", "APPLY_ATTENDECE_IN");
                    sqlcmd.Parameters.Add("@EMP_INTERNAL_ID", model.EmpId);
             

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }
        [HttpPost]
        public HttpResponseMessage AddOutTime(InOutInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ATTENDENCE", sqlcon);
                    //sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.Parameters.Add("@ACTION", "APPLY_ATTENDECE_OUT");
                    sqlcmd.Parameters.Add("@EMP_INTERNAL_ID", model.EmpId);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage GetWFHLIST(LeaveInfoModelInput model)
        {
            List<WFHListClass> _msg = new List<WFHListClass>();
            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                //DataTable dt = objEngine.GetDataTable(@"select Leave_Id, LeaveType from ERP_Leavetype");

                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction);;
                SqlCommand sqlcmd;

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                //rev Pratik
                //sqlcmd = new SqlCommand("select * from ERP_WORKFROM_HOME_APPLICATION where USER_ID='" + model.UserId + "'", sqlcon);
                sqlcmd = new SqlCommand("select *,CASE WHEN (WFH_STARTDATE>GETDATE() AND ERP_WORKFROM_HOME_APPLICATION.CURRENT_STATUS<>'Approved') THEN 1 ELSE 0 END AS isDelete,CASE WHEN (WFH_STARTDATE>GETDATE() AND ERP_WORKFROM_HOME_APPLICATION.CURRENT_STATUS<>'Approved') THEN 1 ELSE 0 END AS isEdit from ERP_WORKFROM_HOME_APPLICATION where USER_ID='" + model.UserId + "'", sqlcon);

                //end of rev Pratik
                sqlcmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new WFHListClass()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                USER_ID = Convert.ToString(dr["USER_ID"]),
                                WFH_STARTDATE = Convert.ToString(dr["WFH_STARTDATE"]),
                                WFH_ENDDATE = Convert.ToString(dr["WFH_ENDDATE"]),
                                WFH_REASON = Convert.ToString(dr["WFH_REASON"]),
                                WORK_PLAN = Convert.ToString(dr["WORK_PLAN"]),
                                APPLIED_DATE = Convert.ToString(dr["APPLIED_DATE"]),
                                CURRENT_STATUS = Convert.ToString(dr["CURRENT_STATUS"]),
                                SUPERVISOR_NAME = Convert.ToString(dr["SUPERVISOR_NAME"]),
                                SUPERVISOR_EMAIL = Convert.ToString(dr["SUPERVISOR_EMAIL"]),
                                SUPERVISOR_ID = Convert.ToString(dr["SUPERVISOR_ID"]),
                                //rev Pratik
                                isDelete = Convert.ToString(dr["isDelete"]),
                                isEdit = Convert.ToString(dr["isEdit"]),
                                //End of rev Pratik
                            }).ToList();
                }
            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }

        //rev Pratik

        [HttpPost]
        public HttpResponseMessage DeleteWFH(DeleteReq model)
        {

            RMApplyOutput oview = new RMApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ESSDeleteReq", sqlcon);
                    sqlcmd.Parameters.Add("@ACTION", "DELETE_WFH");
                    sqlcmd.Parameters.Add("@ID", model.ID);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully deleted.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        //End of Rev Pratik

        [HttpPost]
        public HttpResponseMessage AddTimesheet(timesheetInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction);;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APPLYTIMESHEET", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_TIMESHEET");
                    sqlcmd.Parameters.Add("@USER_ID", model.USER_ID);
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    
                    sqlcmd.Parameters.Add("@TIMESHEET_PROJECT_ID", model.TIMESHEET_PROJECT_ID);
                    sqlcmd.Parameters.Add("@TIMESHEET_DATE", model.TIMESHEET_DATE);
                    sqlcmd.Parameters.Add("@TIMESHEET_HOUR", model.TIMESHEET_HOUR);
                    sqlcmd.Parameters.Add("@TIMESHEET_MINUTE", model.TIMESHEET_MINUTE);
                    sqlcmd.Parameters.Add("@TIMESHEET_COMMENT", model.TIMESHEET_COMMENT);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }
        //rev Pratik
        [HttpPost]
        public HttpResponseMessage AddTimesheetEdit(timesheetInputs model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APPLYTIMESHEET", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_TIMESHEET_EDIT");
                    sqlcmd.Parameters.Add("@USER_ID", model.USER_ID);
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);

                    sqlcmd.Parameters.Add("@TIMESHEET_PROJECT_ID", model.TIMESHEET_PROJECT_ID);
                    sqlcmd.Parameters.Add("@TIMESHEET_DATE", model.TIMESHEET_DATE);
                    sqlcmd.Parameters.Add("@TIMESHEET_HOUR", model.TIMESHEET_HOUR);
                    sqlcmd.Parameters.Add("@TIMESHEET_MINUTE", model.TIMESHEET_MINUTE);
                    sqlcmd.Parameters.Add("@TIMESHEET_COMMENT", model.TIMESHEET_COMMENT);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }
        //End of rev Pratik


        [HttpPost]
        public HttpResponseMessage FetchTimeSheet(fetchTimesheetInput model)
        {
            timesheetOutputList oview = new timesheetOutputList();
            List<timesheetTopLevelList> topLevelList = new List<timesheetTopLevelList>();


            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction);;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_FETCHTIMESHEET", sqlcon);
                    //sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.Parameters.Add("@MM", model.mm);
                    sqlcmd.Parameters.Add("@YYYY", model.yyyy);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();

                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            timesheetTopLevelList topLevel = new timesheetTopLevelList();
                            topLevel.TIMESHEET_DATE = Convert.ToString(item["TIMESHEET_DATE"]);

                            List<timesheetDownLevelList> downlevelList = new List<timesheetDownLevelList>();

                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow item2 in ds.Tables[1].Rows)
                                {
                                    timesheetDownLevelList downlevel = new timesheetDownLevelList();
                                    if (Convert.ToString(item["TIMESHEET_DATE"])==Convert.ToString(item2["TIMESHEET_DATE"]))
                                    {
                                        downlevel.TIMESHEET_COMMENT = Convert.ToString(item2["TIMESHEET_COMMENT"]);
                                        downlevel.TIMESHEET_ID = Convert.ToString(item2["TIMESHEET_ID"]);
                                        downlevel.TIMESHEET_HOUR = Convert.ToString(item2["TIMESHEET_HOUR"]);
                                        downlevel.TIMESHEET_MINUTE = Convert.ToString(item2["TIMESHEET_MINUTE"]);
                                        downlevel.TIMESHEET_DATE = Convert.ToString(item2["TIMESHEET_DATE"]);
                                        downlevel.isDelete = Convert.ToString(item2["isDelete"]);
                                        downlevel.isEdit = Convert.ToString(item2["isEdit"]);
                                        downlevelList.Add(downlevel);
                                    }
                                   
                                }                                
                            }
                            topLevel.info = downlevelList;

                            topLevelList.Add(topLevel);
                        }

                    }


                    oview.status = "200";
                    oview.message = "Successfully add task.";
                    oview.dt = topLevelList;
                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        //rev Pratik

        [HttpPost]
        public HttpResponseMessage DeleteTimeSheet(DeleteReq model)
        {

            RMApplyOutput oview = new RMApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ESSDeleteReq", sqlcon);
                    sqlcmd.Parameters.Add("@ACTION", "DELETE_TSHEET");
                    sqlcmd.Parameters.Add("@ID", model.ID);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully deleted.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        //End of Rev Pratik

    }
}
