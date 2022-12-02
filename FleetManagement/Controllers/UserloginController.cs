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
    public class UserloginController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Login(UserLogin model)
        {
            ClassLoginOutput omodel = new ClassLoginOutput();
            UserClass oview = new UserClass();

            try
            {
                String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

                string sessionId =  HttpContext.Current.Session.SessionID;

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

                    if (!ModelState.IsValid)
                    {

                        omodel.status = "213";
                        omodel.message = "Username and Password Mandatory.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                    }
                    else
                    {
              
                        String profileImg = System.Configuration.ConfigurationSettings.AppSettings["ProfileImageURL"];
                        Encryption epasswrd = new Encryption();
                        string Encryptpass = epasswrd.Encrypt(model.password);
                      

                      //  sessionId = Session.SessionID;
                        string location_name = "Login";

                        DataSet dt = new DataSet();
                       
                        
                        String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 
                       // String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        
                        
                        SqlCommand sqlcmd = new SqlCommand();

                        SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();


                        sqlcmd = new SqlCommand("proc_fleet_Userlogin", sqlcon);
                        sqlcmd.Parameters.Add("@userName", model.username);
                        sqlcmd.Parameters.Add("@password", Encryptpass);
                        sqlcmd.Parameters.Add("@SessionToken", sessionId);
                        sqlcmd.Parameters.Add("@latitude", model.latitude);
                        sqlcmd.Parameters.Add("@longitude", model.longitude);
                        sqlcmd.Parameters.Add("@login_time", model.login_time);
                        sqlcmd.Parameters.Add("@version_name", versionname);
                        sqlcmd.Parameters.Add("@Weburl", profileImg);
                        sqlcmd.Parameters.Add("@ImeiNo", model.Imei);


                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcon.Close();



                        if (dt.Tables[0] !=null && dt.Tables[0].Rows.Count > 0  )
                        {
                         
                            oview = APIHelperMethods.ToModel<UserClass>(dt.Tables[0]);

                            if(Convert.ToString(dt.Tables[0].Rows[0]["success"])=="200")
                            {
                                omodel.status = "200";
                                omodel.session_token = sessionId;
                                omodel.user_details = oview;
                                omodel.message = "User successfully logged in.";
                            }

                            if (Convert.ToString(dt.Tables[0].Rows[0]["success"]) == "201")
                            {
                                omodel.status = "201";
                                omodel.message = "User Id and Password combination do not match.";
                            }

                            if (Convert.ToString(dt.Tables[0].Rows[0]["success"]) == "207")
                            {
                                omodel.status = "207";
                                omodel.message = "IMEI not match.Contact to your administrator";
                            }
                        }
                        else
                        {
                            omodel.status = "201";
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
        string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
        string location = string.Empty;

    }




}