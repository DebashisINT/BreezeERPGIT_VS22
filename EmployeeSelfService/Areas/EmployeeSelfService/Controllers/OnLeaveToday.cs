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
    public class OnLeaveToday : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getOnLeaveToday(OnLeaveInfoModelInput model)
        {

            OnLeaveInfoModelOutput oview = new OnLeaveInfoModelOutput();

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
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "OnLeaveToday");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    OnLeaveInfo obj = new OnLeaveInfo();
                    obj.EmpName = Convert.ToString(dts.Rows[0]["EmpName"]);
                    obj.ProfileIMG = Convert.ToString(dts.Rows[0]["ProfileIMG"]);


                    oview.LeaveTodayinfo = obj;
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