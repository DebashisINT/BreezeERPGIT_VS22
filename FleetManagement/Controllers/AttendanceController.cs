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
    public class AttendanceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage List(AttendancerecordInput model)
        {
            AttendancerecordOutput omodel = new AttendancerecordOutput();
            List<Attendancerecord> oview = new List<Attendancerecord>();
            DatalistsAttendance odata = new DatalistsAttendance();
            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
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

                        string sessionId = "";
                        DataTable dt = new DataTable();
                        String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 
                        //String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        SqlCommand sqlcmd = new SqlCommand();
                        SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();
                        sqlcmd = new SqlCommand("Proc_fleet_Attendance", sqlcon);
                        //sqlcmd.Parameters.Add("@session_token", model.session_token);
                        sqlcmd.Parameters.Add("@user_id", model.user_id);
                        sqlcmd.Parameters.Add("@start_date", model.start_date);
                        sqlcmd.Parameters.Add("@end_date", model.end_date);

                        sqlcmd.CommandType = CommandType.StoredProcedure;


                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcon.Close();
                        if (dt.Rows.Count > 0)
                        {
                            oview = APIHelperMethods.ToModelList<Attendancerecord>(dt);
                            odata.session_token = model.session_token;
                            //  odata.shop_list = oview;
                            omodel.status = "200";
                            omodel.message = "Attendance list for last 15 days / start day to end date";
                            // omodel.data = odata;
                            omodel.attendance_list = oview;

                        }
                        else
                        {

                            omodel.status = "205";
                            omodel.message = "No data found";

                        }
                        var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                        return message;


                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Token Id does not matched.";
                        var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                        return message;

                    }

                }

            }

            catch (Exception ex)
            {
                omodel.status = "209";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                return message;
            }

        }

    }

}

