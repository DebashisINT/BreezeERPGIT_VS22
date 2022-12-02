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
    public class LeaveApprovalAPIController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage GETLeavesofEMPforAPPROVAL(BMInputs model)
        {

            LeaveApprovalOutput oview = new LeaveApprovalOutput();
            List<LeaveListClassForApp> obj = new List<LeaveListClassForApp>();
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
                    sqlcmd = new SqlCommand("PRC_EMP_LEAVEAPPROVAL", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_LISTOFAPPLEAVE");
                    //Mantis Issue 24411
                    sqlcmd.Parameters.Add("@USER_ID", model.ID);
                    //End of Mantis Issue 24411
                    sqlcmd.Parameters.Add("@EMPLOYEE_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@CURRENT_STATUS", model.STATUS);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];
                    if (dt != null)
                    {
                        obj = (from DataRow dr in dt.Rows
                               select new LeaveListClassForApp()
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
                                   SUPERVISOR_ID = Convert.ToString(dr["SUPERVISOR_ID"]) ,
                                   user_name = Convert.ToString(dr["user_name"])
                               }).ToList();
                    }





                    oview.leaveListforApp = obj;
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
        public HttpResponseMessage ApproveLeaveReq(appLeaveReqClass model)
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
                    sqlcmd = new SqlCommand("PRC_EMP_LEAVEAPPROVAL", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "APPROVE_LEAVES");
                    sqlcmd.Parameters.Add("@LEAVE_IDS", model.LEAVE_IDS);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Approved";

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
        public HttpResponseMessage RejectLeaveReq(appLeaveReqClass model)
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
                    sqlcmd = new SqlCommand("PRC_EMP_LEAVEAPPROVAL", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "REJECT_LEAVES");
                    sqlcmd.Parameters.Add("@LEAVE_IDS", model.LEAVE_IDS);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Rejected";

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
