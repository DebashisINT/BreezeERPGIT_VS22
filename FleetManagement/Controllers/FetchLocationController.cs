using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.IO;
using System.Text;
using FleetManagement.Models;

namespace FleetManagement.Controllers
{
    public class FetchLocationController : ApiController
    {
        public HttpResponseMessage List(LocationfetchInput model)
        {

            List<Locationfetch> oview = new List<Locationfetch>();
            LocationfetchOutput odata = new LocationfetchOutput();
             try
            {
                if (!ModelState.IsValid)
                {
                    odata.status = "213";
                    odata.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, odata);
                }
                else
                {

                    string tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                    System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

                    string token = string.Empty;
                    string versionname = string.Empty;
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

                        List<Locationupdate> omedl2 = new List<Locationupdate>();

                        DataTable dt = new DataTable();
                        String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 
                        //String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        SqlCommand sqlcmd = new SqlCommand();
                        SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();
                        sqlcmd = new SqlCommand("proc_fleet_Locationfetch", sqlcon);
                        sqlcmd.Parameters.Add("@date_span", model.date_span);
                        sqlcmd.Parameters.Add("@from_date", model.from_date);
                        sqlcmd.Parameters.Add("@to_date", model.to_date);
                        sqlcmd.Parameters.Add("@user_id", model.user_id);


                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcon.Close();
                        if (dt.Rows.Count > 0)
                        {
                            oview = APIHelperMethods.ToModelList<Locationfetch>(dt);

                            odata.location_details = oview;
                            odata.status = "200";
                            odata.message = "Shop details  available";



                        }
                        else
                        {

                            odata.status = "205";
                            odata.message = "No data found";

                        }

                        var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                        return message;
                    }
                    else
                    {
                        odata.status = "205";
                        odata.message = "Token Id does not matched.";
                        var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                        return message;

                    }

                }
             }
                   catch (Exception ex)
            {
                odata.status = "209";
                odata.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                return message;
            }
            

        }
    }
}
