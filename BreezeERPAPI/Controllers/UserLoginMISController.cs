using BreezeERPAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BreezeERPAPI.Controllers
{
    public class UserLoginMISController : Controller
    {
        [AcceptVerbs("POST")]
        public ActionResult UserLogin(MISUserLogin model)
        {
            MISUserLoginOutput omodel = new MISUserLoginOutput();
            ErrorModel omodelerror = new ErrorModel();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.token)
                {
                    string sessionId = HttpContext.Session.SessionID;

                    DataTable dt = new DataTable();
                    Encryption epasswrd = new Encryption();
                    string Encryptpass = epasswrd.Encrypt(model.password);
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();



                    sqlcmd = new SqlCommand("Proc_UserLoginMIS", sqlcon);
                    sqlcmd.Parameters.Add("@userName", model.user_name);
                    sqlcmd.Parameters.Add("@password", Encryptpass);

                    //sqlcmd.Parameters.Add("@Imei_no", model.Imei_no);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);




                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0)
                    {

                        omodel = APIHelperMethods.ToModel<MISUserLoginOutput>(dt);
                        omodel.session_token = sessionId;
                        omodel.ResponseCode = "200";
                        omodel.Responsedetails = "Successfully Logged In";
                        return Json(omodel);

                    }
                    else
                    {
                        omodel.ResponseCode = "201";
                        omodel.Responsedetails = "Invalid User name /password";
                        return Json(omodelerror);
                    }


                }
                else
                {
                    omodelerror.ResponseCode = "203";
                    omodelerror.Responsedetails = "Token Id not match";
                    return Json(omodelerror);

                }

            }
            catch
            {

                omodel.ResponseCode = "103";
                omodel.Responsedetails = "Error occured";
                return Json(omodelerror);

            }

        }
    }
}