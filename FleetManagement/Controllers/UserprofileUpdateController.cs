using FleetManagement.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FleetManagement.Controllers
{
    public class UserprofileController : Controller
    {
        //
        public JsonResult Update(ProfileImageupdationInputData model)
        {
            ProfileImageuShopOutput omodel = new ProfileImageuShopOutput();
            ProfileImageupdation omm = new ProfileImageupdation();
            string ImageName = "";

            try
            {
                ProfileImageuShopOutput oview = new ProfileImageuShopOutput();

                if (model.profile_image != null)
                {
                    ImageName = model.profile_image.FileName;
                }
                string UploadFileDirectory = "~/CommonFolder/ProfileImages";
                try
                {
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

                            case "user_id":
                                {
                                    omm.user_id = value;
                                    break;
                                }

                            case "user_name":
                                {
                                    omm.user_name = value;
                                    break;
                                }

                            case "address":
                                {
                                    omm.address = value;
                                    break;
                                }

                            case "latitude":
                                {
                                    omm.latitude = value;
                                    break;
                                }

                            case "longitude":
                                {
                                    omm.longitude = value;
                                    break;
                                }

                            case "country":
                                {
                                    omm.country = value;
                                    break;
                                }

                            case "state":
                                {
                                    omm.state = value;
                                    break;
                                }

                            case "city":
                                {
                                    omm.city = value;
                                    break;
                                }

                            case "pincode":
                                {
                                    omm.pincode = value;
                                    break;
                                }

                        }
                    }

                }
                catch (Exception msg)
                {

                    omodel.status = "206" + ImageName;
                    omodel.message = msg.Message;

                }



                if (model.profile_image != null)
                {

                    ImageName = omm.session_token + '_' + omm.user_id + '_' + ImageName;
                    string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ProfileImages"), ImageName);
                    model.profile_image.SaveAs(vPath);
                }
                string sessionId = "";



                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 


              //  String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();


                sqlcmd = new SqlCommand("proc_fleet_ApiUpdateprofile", sqlcon);
                //sqlcmd.Parameters.Add("@session_token", omm.session_token);
                sqlcmd.Parameters.Add("@user_id", omm.user_id);
                sqlcmd.Parameters.Add("@user_name", omm.user_name);
                sqlcmd.Parameters.Add("@address", omm.address);
                sqlcmd.Parameters.Add("@latitude", omm.latitude);
                sqlcmd.Parameters.Add("@longitude", omm.longitude);
                sqlcmd.Parameters.Add("@country", omm.country);
                sqlcmd.Parameters.Add("@state", omm.state);
                sqlcmd.Parameters.Add("@city", omm.city);
                sqlcmd.Parameters.Add("@pincode", omm.pincode);
                sqlcmd.Parameters.Add("@profile_image", ImageName);


                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    ///oview = APIHelperMethods.ToModel<ProfileImageuShopOutput>(dt);
                    omodel.status = "200";
                    omodel.session_token = sessionId;
                    omodel.message = "Profile Updated Successfully";
                }
                else
                {
                    omodel.status = "202";
                    omodel.message = "Data not inserted";
                }

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
