using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using FleetManagement.Models;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Web.UI;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.Net.Http;


namespace FleetManagement.Controllers
{
    public class VehiclelogSubmitController : Controller
    {

        [AcceptVerbs("POST")]

        public JsonResult AddVehiclelog(VehiclelogSubmit model)
        {

            VehiclelogShopOutput omodel = new VehiclelogShopOutput();
            VehicleInput omm = new VehicleInput();
            string ImageName = "";

            try
            {
                // RegisterShopInputData model = new RegisterShopInputData();

                //TextWriter tw = new StreamWriter("date.txt");
                //// write a line of text to the file
                //tw.WriteLine(DateTime.Now + model.data);
                //tw.Close();

                  string token = string.Empty;
                string versionname = string.Empty;
                HttpRequestMessage re = new HttpRequestMessage();
                var headers = re.Headers;

                String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (headers.Contains("version_name"))
                {
                    versionname = headers.GetValues("version_name").First();
                }
                if (headers.Contains("token_Number"))
                {
                    token = headers.GetValues("token_Number").First();
                }

                //if (token == tokenmatch)
                //{
                    // close the stream

                    VehiclelogShopOutput oview = new VehiclelogShopOutput();
                    if (model.vehicle_image != null)
                    {
                        ImageName = model.vehicle_image.FileName;
                    }

                    string UploadFileDirectory = "~/CommonFolder";
                    
                    
                    var details = JObject.Parse(model.data);

                    foreach (var item in details)
                    {
                        string param = item.Key;
                        string value = Convert.ToString(item.Value);
                        switch (param)
                        {
                            case "session_token":
                                {
                                    omm.session_token = value;
                                    break;
                                }

                            case "log_type":
                                {
                                    omm.log_type = value;
                                    break;
                                }

                            case "log_text":
                                {
                                    omm.log_text = value;
                                    break;
                                }


                            case "current_address":
                                {
                                    omm.current_address = value;
                                    break;
                                }

                            case "current_lat":
                                {
                                    omm.current_lat = value;
                                    break;
                                }

                            case "current_long":
                                {
                                    omm.current_long = value;
                                    break;
                                }

                            case "current_location":
                                {
                                    omm.current_location = value;
                                    break;
                                }

                            case "date_time":
                                {
                                    omm.date_time = value;
                                    break;
                                }

                            case "user_id":
                                {

                                    omm.user_id = value;
                                    break;
                                }

                        }

                    }








                    if (model.vehicle_image != null)
                    {
                        ImageName = omm.session_token+'_' + omm.user_id + '_' + ImageName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder"), ImageName);
                        model.vehicle_image.SaveAs(vPath);
                    }

                    
                    string sessionId = "";


                    DataTable dt = new DataTable();
                   String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 


                   // String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("proc_fleet_vehiclelogManage", sqlcon);
                    sqlcmd.Parameters.Add("@session_token", omm.session_token);
                    sqlcmd.Parameters.Add("@user_id", omm.user_id);
                    sqlcmd.Parameters.Add("@log_type", omm.log_type);
                    sqlcmd.Parameters.Add("@log_text", omm.log_text);
                    sqlcmd.Parameters.Add("@current_address", omm.current_address);
                    sqlcmd.Parameters.Add("@current_location", omm.current_location);
                    sqlcmd.Parameters.Add("@current_lat", omm.current_lat);
                    sqlcmd.Parameters.Add("@current_long", omm.current_long);
                    sqlcmd.Parameters.Add("@v_date_time", omm.date_time);
                    sqlcmd.Parameters.Add("@vehicle_image", ImageName);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0)
                    {

                        omodel.status = "200";
                        omodel.message = "Vehicle log submitted successfully.";
                    }
                    else
                    {
                        omodel.status = "202";
                        omodel.message = "Data not inserted";

                    }
                //}
                //else
                //{
                //    omodel.status = "205";
                //    omodel.message = "Token Id does not matched.";
                  

                //}

            }
            catch (Exception msg)
            {
                omodel.status = "204" + ImageName;
                omodel.message = msg.Message;

            }
            // var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
            return Json(omodel);
        }


    }
}
