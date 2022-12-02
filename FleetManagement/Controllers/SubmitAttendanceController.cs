using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FleetManagement.Controllers
{
    public class SubmitAttendanceController : ApiController
    {
        [HttpPost]

        public HttpResponseMessage Attendance(AttendancemanageInput model)
        {
            AttendancemanageOutput omodel = new AttendancemanageOutput();
            UserClass oview = new UserClass();

            try
            {

                string token = string.Empty;
                string versionname = string.Empty;
                System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (headers.Contains("version_name"))
                {
                    versionname = headers.GetValues("version_name").First();
                }
                if (headers.Contains("token_Number"))
                {
                    token = headers.GetValues("token_Number").First();
                }

                if (token == tokenmatch)
                {
                    if (!ModelState.IsValid)
                    {
                        omodel.status = "213";
                        omodel.message = "Some input parameters are missing.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                    }
                    else
                    {
                        // String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                        string sessionId = "";


                        DataTable dt = new DataTable();

                        String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];  
                        //String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                        SqlCommand sqlcmd = new SqlCommand();

                        SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();

                        sqlcmd = new SqlCommand("Proc_fleet_Attendancesubmit", sqlcon);


                        sqlcmd.Parameters.Add("@user_id", model.user_id);
                        sqlcmd.Parameters.Add("@SessionToken", model.session_token);
                        sqlcmd.Parameters.Add("@wtype", model.work_type);
                        sqlcmd.Parameters.Add("@wdesc", model.work_desc);
                        sqlcmd.Parameters.Add("@wlatitude", model.work_lat);
                        sqlcmd.Parameters.Add("@wlongitude", model.work_long);
                        sqlcmd.Parameters.Add("@Waddress", model.work_address);
                        sqlcmd.Parameters.Add("@Wdatetime", model.work_date_time);
                        sqlcmd.Parameters.Add("@Isonleave", model.is_on_leave);



                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcon.Close();
                        if (dt.Rows.Count > 0)
                        {
                            omodel.status = "200";
                            omodel.message = "Attendence successfully submitted.";

                        }
                        else
                        {

                            omodel.status = "202";
                            omodel.message = "Invalid user credential.";

                        }

                        var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                        return message;
                    }
                }



                else
                {
                    omodel.status = "205";
                    omodel.message = "Token Id does not matched.";
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;

                }

            }
            catch (Exception ex)
            {


                omodel.status = "209";

                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }






        }


    }
}
