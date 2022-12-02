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
    public class LogoutFleetController : ApiController
    {
        [HttpPost]

        public HttpResponseMessage UserLogout(Model_Logout model)
        {
            Model_LogoutOutput omodel = new Model_LogoutOutput();
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
                       // String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                        SqlCommand sqlcmd = new SqlCommand();

                        SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();

                        sqlcmd = new SqlCommand("Proc_fleet_ApiShopUserLogout", sqlcon);


                        sqlcmd.Parameters.Add("@user_id", model.user_id);
                        sqlcmd.Parameters.Add("@SessionToken", model.session_token);
                        sqlcmd.Parameters.Add("@latitude", model.latitude);
                        sqlcmd.Parameters.Add("@longitude", model.longitude);
                        sqlcmd.Parameters.Add("@logout_time", model.logout_time);
                        sqlcmd.Parameters.Add("@Autologout", model.Autologout);


                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcon.Close();
                        if (dt.Rows.Count > 0)
                        {
                            omodel.status = "200";
                            omodel.message = "User successfully logged out.";

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
