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
    public class LocationSubmitController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Locationupdate(LocationSubmitclass model)
        {
            LocationupdateOutput omodel = new LocationupdateOutput();
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
                    String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
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

                        foreach (var s2 in model.location_details)
                        {
                            omedl2.Add(new Locationupdate()
                            {
                                location_name = s2.location_name,
                                latitude = s2.latitude,
                                longitude = s2.longitude,
                                distance_covered = s2.distance_covered,
                                last_update_time = s2.last_update_time,
                                date = s2.date,
                                order_delivered = s2.order_delivered

                            });

                        }


                        string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                        DataTable dt = new DataTable();
                        String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 
                       // String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                        SqlCommand sqlcmd = new SqlCommand();
                        SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();
                        sqlcmd = new SqlCommand("proc_Fleet_ApiLocationupdate", sqlcon);
                        sqlcmd.Parameters.Add("@user_id", model.user_id);
                        sqlcmd.Parameters.Add("@JsonXML", JsonXML);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcon.Close();
                        if (dt.Rows.Count > 0)
                        {
                            omodel.status = "200";
                            omodel.message = "Location details successfully updated.";
                        }
                        else
                        {
                            omodel.status = "202";
                            omodel.message = "User id or Session token does not matched";
                        }

                        var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                        return message;

                    }

                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Token Id does not matched.";
                        var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                        return message;

                    }
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
