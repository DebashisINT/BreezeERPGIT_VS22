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
    public class GETLeavesofEMPController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GETLeavesofEMP(LeaveInfoModelInput model)
        {

            LeaveListOutput oview = new LeaveListOutput();
            List<LeaveListClass> obj = new List<LeaveListClass>();
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
                    DataTable dt = new DataTable();
                    String con = Convert.ToString(APIConnction.ApiConnction);;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_MY_LEAVES");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    
                    //DataTable dts = dt.Tables[0];
                    if (dt != null)
                    {
                        obj = (from DataRow dr in dt.Rows
                                select new LeaveListClass()
                                {
                                    ID = Convert.ToString(dr["ID"]),
                                    USER_ID = Convert.ToString(dr["USER_ID"]),
                                    LEAVE_START_DATE = Convert.ToString(dr["LEAVE_START_DATE"]),
                                    LEAVE_END_DATE = Convert.ToString(dr["LEAVE_END_DATE"]),
                                    LEAVE_TYPE = Convert.ToString(dr["LEAVE_TYPE"]),
                                    LEAVE_REASON = Convert.ToString(dr["LEAVE_REASON"]),
                                    CREATED_DATE = Convert.ToString(dr["CREATED_DATE"]),
                                    CURRENT_STATUS = Convert.ToString(dr["CURRENT_STATUS"]),
                                    SUPERVISOR_NAME = Convert.ToString(dr["SUPERVISOR_NAME"]),
                                    SUPERVISOR_EMAIL = Convert.ToString(dr["SUPERVISOR_EMAIL"]),
                                    SUPERVISOR_ID = Convert.ToString(dr["SUPERVISOR_ID"]),
                                    LeaveType = Convert.ToString(dr["LeaveType"]),
                                    //rev Pratik
                                    isDelete = Convert.ToString(dr["isDelete"]),
                                    isEdit = Convert.ToString(dr["isEdit"])
                                    //End of rev Pratik
                                }).ToList();
                    }





                    oview.leaveList = obj;
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
        public HttpResponseMessage DeleteLeave(DeleteReq model)
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
                    sqlcmd.Parameters.Add("@ACTION", "DELETE_Leave");
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
        public HttpResponseMessage GetOnLeaveTOday(LeaveInfoModelInput model)
        {

            onLeaveTodayOut oview = new onLeaveTodayOut();
            List<onLeaveToday> obj = new List<onLeaveToday>();
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
                    DataTable dt = new DataTable();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "OnLeaveToday");
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];
                    if (dt != null)
                    {
                        obj = (from DataRow dr in dt.Rows
                               select new onLeaveToday()
                               {
                                   user_name = Convert.ToString(dr["user_name"]),
                                   USER_ID = Convert.ToString(dr["USER_ID"]),     
                               }).ToList();
                    }

                    oview.leaveList = obj;
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
    }
}
