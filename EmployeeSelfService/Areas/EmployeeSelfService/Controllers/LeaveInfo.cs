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
    public class LeaveInfoController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getLeaveInfo(LeaveInfoModelInput model)
        {

            LeaveInformationOutput oview = new LeaveInformationOutput();
            List<LeaveInformationList> obj = new List<LeaveInformationList>();
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
                    String con = Convert.ToString(APIConnction.ApiConnction);
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@EMPCODE", model.EMPCODE);
                    
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null)
                    {
                        obj = (from DataRow dr in dt.Rows
                               select new LeaveInformationList()
                               {
                                   EMPLOYEECODE = Convert.ToString(dr["EMPLOYEECODE"]),
                                   LEAVESTRUCTURECODE = Convert.ToString(dr["LEAVESTRUCTURECODE"]),
                                   LEAVEID = Convert.ToString(dr["LEAVEID"]),
                                   LEAVENAME = Convert.ToString(dr["LEAVENAME"]),
                                   AVAILED = Convert.ToString(dr["AVAILED"]),
                                   BALANCE = Convert.ToString(dr["BALANCE"]),
                                   TotalLeave = Convert.ToString(dr["TotalLeave"])
                               }).ToList();
                    }

                    oview.LeaveInformation = obj;
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