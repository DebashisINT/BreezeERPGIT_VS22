using BreezeERPAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;



namespace BreezeERPAPI.Controllers
{
    public class APIController : Controller
    {
        ///string connectionstring="Server=You server name or comp name;Database=Yourdatabasename;Trusted_Connectopn= True"); 
        String producturl = System.Configuration.ConfigurationSettings.AppSettings["Productimage"];
        AndroidFCMPushNotificationStatus pushmodel = new AndroidFCMPushNotificationStatus();


        #region User Login Logout
        [AcceptVerbs("POST")]
        public JsonResult UserLogin(UserLoginInputParameters model)
        {
            UserLoginOutputParameters omodel = new UserLoginOutputParameters();
            ErrorModel omodelerror = new ErrorModel();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    string sessionId = HttpContext.Session.SessionID;

                    DataTable dt = new DataTable();
                    Encryption epasswrd = new Encryption();
                    string Encryptpass = epasswrd.Encrypt(model.password);
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();



                    sqlcmd = new SqlCommand("Sp_ApiLogin", sqlcon);
                    sqlcmd.Parameters.Add("@userName", model.user_name);
                    sqlcmd.Parameters.Add("@password", Encryptpass);
                    sqlcmd.Parameters.Add("@DeviceId", model.device_id);
                    sqlcmd.Parameters.Add("@Devicetype", model.device_type);
                    sqlcmd.Parameters.Add("@SessionToken", sessionId);
                    sqlcmd.Parameters.Add("@Imei_no", model.Imei_no);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);




                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0" && dt.Rows[0][0].ToString() != "-1")
                        {
                            omodel = APIHelperMethods.ToModel<UserLoginOutputParameters>(dt);
                            omodel.session_token = sessionId;
                            return Json(omodel);
                        }
                        else
                        {
                            if (dt.Rows[0][0].ToString() == "-1")
                            {
                                omodelerror.ResponseCode = "202";
                                omodelerror.Responsedetails = "Already User Logged In";
                                return Json(omodelerror);
                            }
                            else
                            {
                                omodelerror.ResponseCode = "201";
                                omodelerror.Responsedetails = "Invalid User name /password";
                                return Json(omodelerror);
                            }
                        }

                    }
                    else
                    {
                        omodelerror.ResponseCode = "201";
                        omodelerror.Responsedetails = "Invalid User name /password";
                        return Json(omodelerror);
                    }


                }
                else
                {
                    omodelerror.ResponseCode = "103";
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

        [AcceptVerbs("POST")]
        public JsonResult UserLogout(string user_id, string Token, int User_login_Id, string session_token)
        {
            UserLogOutputParameters omodel = new UserLogOutputParameters();
            ErrorModel omodelerror = new ErrorModel();
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiLogout", sqlcon);
                    sqlcmd.Parameters.Add("@userId", user_id);
                    sqlcmd.Parameters.Add("@User_login_Id", User_login_Id);
                    sqlcmd.Parameters.Add("@SessionToken", session_token);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {


                            omodel = APIHelperMethods.ToModel<UserLogOutputParameters>(dt);

                            return Json(omodel);
                        }
                        else
                        {
                            omodelerror.ResponseCode = "201";
                            omodelerror.Responsedetails = "Invalid User name /password";

                            return Json(omodelerror);

                        }

                    }
                    else
                    {

                        omodelerror.ResponseCode = "201";
                        omodelerror.Responsedetails = "Invalid User name /password";
                        return Json(omodelerror);

                    }

                }
                else
                {
                    omodelerror.ResponseCode = "103";
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

        #endregion

        #region CountryList
        [AcceptVerbs("GET")]
        public JsonResult CountryList(string Token)
        {
            countyryListRespose omodel = new countyryListRespose();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APICountryList", sqlcon);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.country_list = APIHelperMethods.ToModelList<CountryList>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "token Id not match";

                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }





        }
        #endregion

        #region New List For 3 New Feild in TAB

        [AcceptVerbs("POST")]
        public JsonResult GetLeadSource(string user_id, string Token)
        {

            LeadSourceList objomodel = new LeadSourceList();
            List<LeadSource> omodel = new List<LeadSource>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "LeadSource");
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<LeadSource>(dt);
                            objomodel.lead_source_list = omodel;
                            return Json(objomodel);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "Invalid User name /password";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }


        [AcceptVerbs("POST")]
        public JsonResult GetProfession(string user_id, string Token)
        {

            ProffessionList objomodel = new ProffessionList();
            List<Proffession> omodel = new List<Proffession>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "LeadProfession");
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<Proffession>(dt);
                            objomodel.professsion_list = omodel;

                            return Json(objomodel);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "Invalid User name /password";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }


        [AcceptVerbs("POST")]
        public JsonResult GetAssignTo(string user_id, string Token)
        {

            AssignToList objomodel = new AssignToList();
            List<Assigns> omodel = new List<Assigns>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "AssignTo");
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<Assigns>(dt);
                            objomodel.assigned_to_list = omodel;

                            return Json(objomodel);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "Invalid User name /password";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }


        #endregion

        #region StateList
        [AcceptVerbs("GET")]
        public JsonResult StateList(string Token)
        {
            StateListResponse omodel = new StateListResponse();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APIStateList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.state_list = APIHelperMethods.ToModelList<StateList>(dt);
                            omodel.isUpdated = "false";
                            return Json(omodel, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }



            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }





        }
        #endregion

        #region CityList
        [AcceptVerbs("GET")]
        public JsonResult CityList(string Token)
        {
            CityListResponse omodel = new CityListResponse();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APICityList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.city_list = APIHelperMethods.ToModelList<CityList>(dt);
                            omodel.isUpdated = "false";
                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }


                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";

                    return Json(oerror, JsonRequestBehavior.AllowGet);

                }

            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }





        }
        #endregion

        #region Pin Code
        [AcceptVerbs("POST")]
        public JsonResult PinCodeList(string Token, string city_Id)
        {
            PincodeListResponse omodel = new PincodeListResponse();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APIPincodeList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@city_Id", city_Id);
                    //  SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    SqlDataReader rdr = sqlcmd.ExecuteReader();
                    //    da.Fill(dt);


                    List<PincodeList> pincodelist1 = new List<PincodeList>();
                    while (rdr.Read())
                    {
                        pincodelist1.Add(new PincodeList()
                        {

                            pincode_id = (int)rdr["pincode_id"],
                            pincode_no = (string)rdr["pincode_no"],
                            city_id = (decimal)rdr["city_id"]

                        });

                    }
                    sqlcon.Close();
                    rdr.Close();

                    if (pincodelist1.Count() > 0)
                    {
                        omodel.ResponseCode = "200";
                        omodel.Responsedetails = "Success";
                        //   omodel.pincodelist = APIHelperMethods.ToModelList<PincodeList>(dt);
                        omodel.pincodelist = pincodelist1;
                        return Json(omodel);
                    }

                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }


                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (dt.Rows[0][0].ToString() != "0")
                    //    {
                    //        omodel.ResponseCode = "200";
                    //        omodel.Responsedetails = "Success";
                    //     //   omodel.pincodelist = APIHelperMethods.ToModelList<PincodeList>(dt);
                    //        omodel.pincodelist = pincodelist1;
                    //        return Json(omodel);
                    //    }
                    //    else
                    //    {
                    //        oerror.ResponseCode = "201";
                    //        oerror.Responsedetails = "Error";
                    //        return Json(oerror);
                    //    }

                    //}
                    //else
                    //{


                    //    oerror.ResponseCode = "201";
                    //    oerror.Responsedetails = "Error";

                    //    return Json(oerror);
                    //}


                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";

                    return Json(oerror);

                }

            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }





        }
        #endregion

        #region Customer Add
        [AcceptVerbs("POST")]
        public JsonResult CustomerAdd(CustomerInputParameters model)
        {
            CustomerOutputParameters omodel = new CustomerOutputParameters();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APICustomerAdd", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@mobile_no", model.mobile_no);
                    sqlcmd.Parameters.Add("@alternate_mobile_no", model.alternate_mobile_no);
                    sqlcmd.Parameters.Add("@email", model.email);
                    sqlcmd.Parameters.Add("@pan_number", model.pan_number);
                    sqlcmd.Parameters.Add("@aadhar_no", model.aadhar_no);
                    sqlcmd.Parameters.Add("@cust_name", model.cust_name);
                    sqlcmd.Parameters.Add("@gender", model.gender);
                    sqlcmd.Parameters.Add("@date_of_birth", model.date_of_birth);
                    sqlcmd.Parameters.Add("@block_no", model.block_no);
                    sqlcmd.Parameters.Add("@street_no", model.street_no);
                    sqlcmd.Parameters.Add("@flat_no", model.flat_no);
                    sqlcmd.Parameters.Add("@floor", model.floor);
                    sqlcmd.Parameters.Add("@landmark", model.landmark);
                    sqlcmd.Parameters.Add("@country", model.country);
                    sqlcmd.Parameters.Add("@state", model.state);
                    sqlcmd.Parameters.Add("@city", model.city);
                    sqlcmd.Parameters.Add("@pin_code", model.pin_code);
                    sqlcmd.Parameters.Add("@sales_man_id", model.sales_man_id);



                    sqlcmd.Parameters.Add("@lead_source_id", model.lead_source_id);
                    sqlcmd.Parameters.Add("@professsion_id", model.professsion_id);
                    sqlcmd.Parameters.Add("@assign_to_id ", model.assign_to_id);
                    sqlcmd.Parameters.Add("@isSendSms ", model.isSendSms);

                    sqlcmd.Parameters.Add("@lead_type_id", model.lead_type_id);
                    sqlcmd.Parameters.Add("@request_type_list ", model.req_id_list);



                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel = APIHelperMethods.ToModel<CustomerOutputParameters>(dt);

                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Customer  Update
        [AcceptVerbs("POST")]
        public JsonResult CustomerUpdate(CustomerupdateInputParameters model)
        {
            CustomerUpdateOutputParameters omodel = new CustomerUpdateOutputParameters();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APICustomerUpdate", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@mobile_no", model.mobile_no);
                    sqlcmd.Parameters.Add("@alternate_mobile_no", model.alternate_mobile_no);
                    sqlcmd.Parameters.Add("@email", model.email);
                    sqlcmd.Parameters.Add("@pan_number", model.pan_number);
                    sqlcmd.Parameters.Add("@aadhar_no", model.aadhar_no);
                    sqlcmd.Parameters.Add("@cust_name", model.cust_name);
                    sqlcmd.Parameters.Add("@gender", model.gender);
                    sqlcmd.Parameters.Add("@date_of_birth", model.date_of_birth);
                    sqlcmd.Parameters.Add("@block_no", model.block_no);
                    sqlcmd.Parameters.Add("@street_no", model.street_no);
                    sqlcmd.Parameters.Add("@flat_no", model.flat_no);
                    sqlcmd.Parameters.Add("@floor", model.floor);
                    sqlcmd.Parameters.Add("@landmark", model.landmark);
                    sqlcmd.Parameters.Add("@country", model.country);
                    sqlcmd.Parameters.Add("@state", model.state);
                    sqlcmd.Parameters.Add("@city", model.city);
                    sqlcmd.Parameters.Add("@pin_code", model.pin_code);
                    sqlcmd.Parameters.Add("@sales_man_id", model.sales_man_id);
                    sqlcmd.Parameters.Add("@Customer_Id", model.customer_id);

                    sqlcmd.Parameters.Add("@lead_source_id", model.lead_source_id);
                    sqlcmd.Parameters.Add("@professsion_id", model.professsion_id);
                    sqlcmd.Parameters.Add("@assign_to_id ", model.assign_to_id);
                    sqlcmd.Parameters.Add("@isSendSms ", model.isSendSms);

                    sqlcmd.Parameters.Add("@lead_type_id", model.lead_type_id);
                    sqlcmd.Parameters.Add("@request_type_list ", model.req_id_list);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel = APIHelperMethods.ToModel<CustomerUpdateOutputParameters>(dt);

                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region PRoductList
        [AcceptVerbs("GET")]
        public JsonResult ProductList(string Token, int pageno, int rowcount)
        {
            ProductListOutput omodel = new ProductListOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];

                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Productlist", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);
                    sqlcmd.Parameters.Add("@Weburl", producturl);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.product_details = APIHelperMethods.ToModelList<ProductDetails>(dt);
                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record found";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Product Search
        [AcceptVerbs("POST")]
        public JsonResult ProductSearch(string product_name, string Token, int pageno, int rowcount)
        {
            ProductsearchOutput omodel = new ProductsearchOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Productlist", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@ProductName", product_name);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);
                    sqlcmd.Parameters.Add("@Weburl", producturl);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();



                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.product_details = APIHelperMethods.ToModelList<ProductDetails>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No record Found.";

                        return Json(oerror);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror);
                }



            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region  Brands List

        [AcceptVerbs("GET")]
        public JsonResult BrandsList(string Token, int pageno, int rowcount)
        {

            Brandlistoutput omodel = new Brandlistoutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Brandlist", sqlcon);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);

                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.Responsedetails = "Success";
                            omodel.brand_details = APIHelperMethods.ToModelList<Brands>(dt);
                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record found";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region  Brands Search

        [AcceptVerbs("POST")]
        public JsonResult BrandsSearch(string brand_name, string Token, int pageno, int rowcount)
        {

            Brandlistoutput omodel = new Brandlistoutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Brandlist", sqlcon);
                    sqlcmd.Parameters.Add("@BrandName", brand_name);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.brand_details = APIHelperMethods.ToModelList<Brands>(dt);
                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record found";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region  Category List

        [AcceptVerbs("GET")]
        public JsonResult CategoryList(string Token, int pageno, int rowcount)
        {

            Categorylistoutput omodel = new Categorylistoutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_CategoryList", sqlcon);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);

                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.category_details = APIHelperMethods.ToModelList<CategoryClass>(dt);
                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record found";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region  Category Search

        [AcceptVerbs("POST")]
        public JsonResult CategorySearch(string category_name, string Token, int pageno, int rowcount)
        {

            Categorylistoutput omodel = new Categorylistoutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_CategoryList", sqlcon);
                    sqlcmd.Parameters.Add("@CategoryName", category_name);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);

                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.category_details = APIHelperMethods.ToModelList<CategoryClass>(dt);
                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record found";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region Product Search By Category and Brand
        [AcceptVerbs("POST")]
        public JsonResult ProductSearchByCategoryBrand(string[] brand_ids, string[] category_ids, string Token, int pageno, int rowcount)
        {

            ProductsearchOutput omodel = new ProductsearchOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    //string bandsjoin = string.Concat(brand_ids);
                    //string Categoryclass = string.Concat(category_ids);

                    string bandsjoin = string.Join(",", brand_ids);
                    string Categoryclass = string.Join(",", category_ids);
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();


                    sqlcmd = new SqlCommand("API_Productlist", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@CategoryId", Categoryclass);
                    sqlcmd.Parameters.Add("@BrandId", bandsjoin);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);
                    sqlcmd.Parameters.Add("@Weburl", producturl);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.product_details = APIHelperMethods.ToModelList<ProductDetails>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No record Found.";

                        return Json(oerror);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror);
                }



            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Cutomer Basket Add
        [AcceptVerbs("POST")]
        public JsonResult BasketAdd(BasketInputParameters model)
        {
            BasketOutputParameters omodel = new BasketOutputParameters();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();


                    sqlcmd = new SqlCommand("BasketAdd", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@product_id", model.product_id);
                    sqlcmd.Parameters.Add("@product_price", model.product_price);
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@quantity", model.quantity);
                    sqlcmd.Parameters.Add("@salesman_id", model.salesman_id);
                    sqlcmd.Parameters.Add("@discount_percent", model.discount_percent);
                    sqlcmd.Parameters.Add("@piece_quantity", model.piece_quantity);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel = APIHelperMethods.ToModel<BasketOutputParameters>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region Saleonprogress
        [AcceptVerbs("GET")]
        public JsonResult Saleonprogress(string user_id, string Token, int pageno, int rowcount)
        {
            saleProgressOutputpara omodel = new saleProgressOutputpara();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();
                    sqlcmd = new SqlCommand("SaleOnProgress_SaleDetails", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@UserId", user_id);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.sale_on_progress_list = APIHelperMethods.ToModelList<Customer>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "no record exists";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror, JsonRequestBehavior.AllowGet);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region Saleonprogress Search
        [AcceptVerbs("POST")]
        public JsonResult SaleonprogressSearch(saleProgressSearchInputpara model)
        {
            saleProgressOutputpara omodel = new saleProgressOutputpara();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("SaleOnProgress_SaleDetails", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@UserId", model.user_id);
                    sqlcmd.Parameters.Add("@customer_name", model.customer_name);
                    sqlcmd.Parameters.Add("@PageNo", model.pageno);
                    sqlcmd.Parameters.Add("@Pagerows", model.rowcount);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.totalcount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                            omodel.sale_on_progress_list = APIHelperMethods.ToModelList<Customer>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region Customer Basket View
        [AcceptVerbs("GET")]
        public JsonResult CutomerbasketView(string customer_id, string sales_man_id, string Token)
        {
            Customerbasketviewoutput omodel = new Customerbasketviewoutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("API_CustomerBasket", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", customer_id);
                    sqlcmd.Parameters.Add("@sales_man_id", sales_man_id);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.RequestId = Convert.ToInt64(dt.Rows[0]["RequestId"]);

                            omodel.customer_basket_details = APIHelperMethods.ToModelList<productbasket>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Id not exists";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Basket Delete
        [AcceptVerbs("POST")]
        public JsonResult Basketdelete(Basketdeleteinputparameter model)
        {
            Basketdeleteoutputparameter omodel = new Basketdeleteoutputparameter();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_CustomerBasketDelete", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@product_id", model.product_id);
                    sqlcmd.Parameters.Add("@sales_man_id", model.sales_man_id);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel = APIHelperMethods.ToModel<Basketdeleteoutputparameter>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region Customer Delete
        [AcceptVerbs("POST")]
        public JsonResult Customerdelete(Customerdeleteinputparameter model)
        {
            Customerdeleteoutputparameter omodel = new Customerdeleteoutputparameter();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    string bandsjoin = null;

                    if (model.temp_unique_id_list != null)
                    {
                        bandsjoin = string.Join(",", model.temp_unique_id_list);
                    }

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_CustomerDelete", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@temp_unique_id", model.temp_unique_id);
                    sqlcmd.Parameters.Add("@temp_unique_id_list", bandsjoin);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel = APIHelperMethods.ToModel<Customerdeleteoutputparameter>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region FinancerList

        [AcceptVerbs("GET")]
        public JsonResult FinancerList(string User_login_Id, string Token, string financer_name, string finance_req_id, string financer_id)
        {
            FinacOutput omodel = new FinacOutput();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    if (financer_name == "")
                    {

                        financer_name = null;
                    }
                    if (finance_req_id == "")
                    {

                        finance_req_id = null;
                    }
                    if (financer_id == "")
                    {

                        financer_id = null;
                    }

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("API_FinancerList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@User_login_Id", User_login_Id);
                    sqlcmd.Parameters.Add("@FinaceName", financer_name);
                    sqlcmd.Parameters.Add("@finance_req_id", finance_req_id);
                    sqlcmd.Parameters.Add("@financer_id", financer_id);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.financer_details = APIHelperMethods.ToModelList<finace>(dt);

                            return Json(omodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record found";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No record found";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror, JsonRequestBehavior.AllowGet);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region Sales on progress - view customer basket- Send Discount Request

        [AcceptVerbs("POST")]
        public JsonResult SaleonProgressSendDiscountRequest(SenddiscountSalesonprogressInput model)
        {
            SenddiscountSalesonprogressOuput omodel = new SenddiscountSalesonprogressOuput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    List<Productsrequestdetails> obj = new List<Productsrequestdetails>();
                    foreach (var item in model.request_details)
                    {
                        obj.Add(new Productsrequestdetails()
                        {
                            discount_percentage = item.discount_percentage,
                            product_quantity = item.product_quantity,
                            product_id = item.product_id,
                            Salesman_Isapplied = item.Salesman_Isapplied,
                            discount_amount = item.discount_amount,
                            final_discount_price = item.final_discount_price,
                            piece_quantity = item.piece_quantity
                        });
                    }



                    string ProductXML = APIHelperMethods.ConvertToXml(obj, 0);
                    string ProductOldunitXML = null;
                    List<ProductsOldunitrequestdetails> oldunit_obj = new List<ProductsOldunitrequestdetails>();
                    if (model.oldunit_details != null)
                    {
                        foreach (var item in model.oldunit_details)
                        {
                            oldunit_obj.Add(new ProductsOldunitrequestdetails()
                            {
                                product_id = item.product_id,
                                quantity = item.quantity,
                                amount = item.amount
                            });
                        }
                        ProductOldunitXML = APIHelperMethods.ConvertToXml(oldunit_obj, 0);
                    }



                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();


                    sqlcmd = new SqlCommand("APICustomerDiscountquantityRequest", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@request_type", model.request_type);
                    sqlcmd.Parameters.Add("@payment_type", model.payment_type);
                    sqlcmd.Parameters.Add("@exchange_amount", model.exchange_amount);
                    sqlcmd.Parameters.Add("@financer_id", model.financer_id);
                    sqlcmd.Parameters.Add("@RequestId", model.RequestId);
                    sqlcmd.Parameters.Add("@ProductList", ProductXML);
                    sqlcmd.Parameters.Add("@ProductOldunitList", ProductOldunitXML);
                    sqlcmd.Parameters.Add("@ORDER_TYPE", null);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel = APIHelperMethods.ToModel<SenddiscountSalesonprogressOuput>(dt);
                            DataTable getdevicelist = new DataTable();
                            getdevicelist = GetdeviceId(model.user_id, "Salesman", model.customer_id, model.request_type);
                            if (getdevicelist != null)
                            {
                                for (int i = 0; i < getdevicelist.Rows.Count; i++)
                                {
                                    if (Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]) != "")
                                    {
                                        SendPushNotification("", Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]), Convert.ToString(getdevicelist.Rows[i]["CustomerName"]), model.request_type);
                                    }
                                }
                            }

                            return Json(omodel);

                        }
                        else
                        {

                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);

                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Billing Request with Address

        [AcceptVerbs("POST")]
        public JsonResult BillingRequestAddresss(BillingDetailscustomerInput model)
        {
            SenddiscountSalesonprogressOuput omodel = new SenddiscountSalesonprogressOuput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();


                    sqlcmd = new SqlCommand("API_Customer_BillingRequest", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@temp_request_id", model.temp_request_id);
                    sqlcmd.Parameters.Add("@delivery_option_type", model.delivery_option_type);
                    sqlcmd.Parameters.Add("@is_delivery_address_same", model.is_delivery_address_same);
                    sqlcmd.Parameters.Add("@block_no", model.block_no);
                    sqlcmd.Parameters.Add("@street_no", model.street_no);
                    sqlcmd.Parameters.Add("@flat_no", model.flat_no);
                    sqlcmd.Parameters.Add("@floor", model.floor);
                    sqlcmd.Parameters.Add("@landmark", model.landmark);
                    sqlcmd.Parameters.Add("@country", model.country);
                    sqlcmd.Parameters.Add("@state", model.state);
                    sqlcmd.Parameters.Add("@city", model.city);
                    sqlcmd.Parameters.Add("@pin_code", model.pin_code);
                    sqlcmd.Parameters.Add("@delivery_date", model.delivery_date);
                    sqlcmd.Parameters.Add("@gstin", model.gstin);
                    sqlcmd.Parameters.Add("@branch_id", model.branch_id);
                    sqlcmd.Parameters.Add("@Customer_name", model.Customer_name);
                    sqlcmd.Parameters.Add("@paymentType", model.paymentType);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            oerror.ResponseCode = "200";
                            oerror.Responsedetails = "Success";
                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }
                    }

                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";
                        return Json(oerror);
                    }


                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region View Customer Approval

        [AcceptVerbs("POST")]
        public JsonResult Customerapprovalshow(CustomerlistforapprovalInput model)
        {
            CustomerlistforapprovalOutput omodel = new CustomerlistforapprovalOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_ViewCustomerApproval", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_name", model.customer_name);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@discount_requested_status", model.discount_requested_status);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.customer_approval_details = APIHelperMethods.ToModelList<Customerapprove>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion

        #region View Discount Approval

        [AcceptVerbs("POST")]
        public JsonResult ViewDiscountApproval(string customer_id, string Token, string sales_man_id, string RequestId)
        {
            Salesmandiscountapproval omodel = new Salesmandiscountapproval();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_DiscountApproval", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", customer_id);
                    sqlcmd.Parameters.Add("@sales_man_id", sales_man_id);
                    sqlcmd.Parameters.Add("@RequestId", RequestId);
                    sqlcmd.Parameters.Add("@Mode", 1);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.RequestId = Convert.ToInt32(dt.Rows[0]["RequestId"]);
                            omodel.exchange_amount = Convert.ToDecimal(dt.Rows[0]["exchange_amount"]);
                            omodel.view_approval_details = APIHelperMethods.ToModelList<productfordiscountsalesman>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion

        #region View Discount Approval - delete product

        [AcceptVerbs("POST")]
        public JsonResult ViewDiscountApprovalDelete(string customer_id, string product_id, string Token, string sales_man_id, string RequestId)
        {
            CustomerlistforapprovalOutput omodel = new CustomerlistforapprovalOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_DiscountApproval", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", customer_id);
                    sqlcmd.Parameters.Add("@product_id", product_id);
                    sqlcmd.Parameters.Add("@sales_man_id", sales_man_id);
                    sqlcmd.Parameters.Add("@RequestId", RequestId);
                    sqlcmd.Parameters.Add("@Mode", 2);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            oerror.ResponseCode = "200";
                            oerror.Responsedetails = "Success";
                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion


        #region  View Discount Approval- Send Request

        [AcceptVerbs("POST")]
        public JsonResult DiscountApprovalSendRequest(ViewsendRequestCustomer model)
        {
            SDiscountApprovalSendRequestOutput omodel = new SDiscountApprovalSendRequestOutput();
            Commonclass oerror = new Commonclass();
            string products = "";
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    if (model.product_ids != null)
                    {
                        products = string.Join(",", model.product_ids);
                    }

                    string ProductOldunitXML = null;
                    List<ProductsOldunitrequestdetails> oldunit_obj = new List<ProductsOldunitrequestdetails>();
                    if (model.oldunit_details != null)
                    {
                        foreach (var item in model.oldunit_details)
                        {
                            oldunit_obj.Add(new ProductsOldunitrequestdetails()
                            {
                                product_id = item.product_id,
                                quantity = item.quantity,
                                amount = item.amount
                            });
                        }
                        ProductOldunitXML = APIHelperMethods.ConvertToXml(oldunit_obj, 0);
                    }



                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Customersendrequestdiscount", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@request_type", model.request_type);
                    sqlcmd.Parameters.Add("@exchange_amount", model.exchange_amount);
                    sqlcmd.Parameters.Add("@payment_type", model.payment_type);
                    sqlcmd.Parameters.Add("@financer_id", model.financer_id);
                    sqlcmd.Parameters.Add("@product_ids", model.product_ids);
                    sqlcmd.Parameters.Add("@RequestId", model.RequestId);
                    sqlcmd.Parameters.Add("@ProductOldunitList", ProductOldunitXML);



                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel = APIHelperMethods.ToModel<SDiscountApprovalSendRequestOutput>(dt);

                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion

        #region  Sales Manager Discount Request

        [AcceptVerbs("POST")]
        public JsonResult SalesManagerDiscountRequest(ViewDiscountRequestInput model)
        {
            ViewDiscountRequestOutput omodel = new ViewDiscountRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_SP_ViewDiscountdetailscustomerwise", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@request_status", model.request_status);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@Isappliedtop", model.Isappliedtop);
                    sqlcmd.Parameters.Add("@user_type", model.user_type);
                    sqlcmd.Parameters.Add("@Mode", 1);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.disc_request_list = APIHelperMethods.ToModelList<DiscountrequestList>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region  Sales Manager Discount Request(s) - details

        [AcceptVerbs("POST")]
        public JsonResult SalesManagerDiscountRequestDetailproduct(SalesmandiscountRequestProductInput model)
        {
            SalesmandiscountRequestProductOutput omodel = new SalesmandiscountRequestProductOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_SP_ViewDiscountdetailscustomerwise", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@salesman_id", model.salesman_id);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@Requestid", model.Requestid);
                    sqlcmd.Parameters.Add("@Mode", 2);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.Requestid = Convert.ToInt32(dt.Rows[0]["Requestid"]);
                            omodel.exchange_amount = Convert.ToDecimal(dt.Rows[0]["exchange_amount"]);
                            omodel.discount_request_details = APIHelperMethods.ToModelList<Discountsalesman>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Discount Request(s) - Approve / Reject

        [AcceptVerbs("POST")]
        public JsonResult SalesManagerDiscountRequestApprove(DiscountApprovalInput model)
        {
            SalesmandiscountRequestProductOutput omodel = new SalesmandiscountRequestProductOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    List<Productsrequestdetails> obj = new List<Productsrequestdetails>();
                    foreach (var item in model.request_details)
                    {
                        obj.Add(new Productsrequestdetails()
                        {
                            discount_percentage = item.discount_percentage,
                            product_quantity = item.product_quantity,
                            product_id = item.product_id,
                            final_discount_price = item.final_discount_price

                        });
                    }
                    string ProductXML = APIHelperMethods.ConvertToXml(obj, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APICustomerDiscountquantityResponsefromsalesmanager", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@request_type", model.request_type);
                    sqlcmd.Parameters.Add("@Isappliedtop", model.Isappliedtop);
                    sqlcmd.Parameters.Add("@ProductList", ProductXML);
                    sqlcmd.Parameters.Add("@sales_man_id", model.sales_man_id);
                    sqlcmd.Parameters.Add("@Requestid", model.Requestid);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            oerror.ResponseCode = "200";
                            oerror.Responsedetails = "Success";


                            DataTable getdevicelist = new DataTable();

                            getdevicelist = GetdeviceId(model.sales_man_id, "Manager", model.customer_id, model.request_type);
                            if (getdevicelist != null)
                            {
                                for (int i = 0; i < getdevicelist.Rows.Count; i++)
                                {
                                    if (Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]) != "")
                                    {
                                        SendPushNotification("", Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]), Convert.ToString(getdevicelist.Rows[i]["CustomerName"]), model.request_type);
                                    }
                                }
                            }

                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region   Sales Manager Finance Request List

        [AcceptVerbs("POST")]
        public JsonResult SalesMangerFinanceRequestList(string user_id, string finance_request_status, string Token, int? pageno, int? rowcount)
        {
            FinanceRequestOutput omodel = new FinanceRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    if (finance_request_status == "")
                    {
                        finance_request_status = null;
                    }
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("GetFinanceList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@finance_request_status", finance_request_status);
                    sqlcmd.Parameters.Add("@Mode", 1);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.total_count = Convert.ToInt32(dt.Rows[0]["total_count"]);
                            omodel.finance_request_list = APIHelperMethods.ToModelList<FinanceRequest>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region   Sales Manager Finance Request List  (Search)

        [AcceptVerbs("POST")]
        public JsonResult SalesMangerFinanceRequestListSearch(string user_id, string customer_name, string finance_request_status, string Token, int? pageno, int? rowcount)
        {
            FinanceRequestOutput omodel = new FinanceRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    if (customer_name == "")
                    {
                        customer_name = null;
                    }

                    if (finance_request_status == "")
                    {
                        finance_request_status = null;
                    }
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("GetFinanceList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@customer_name", customer_name);
                    sqlcmd.Parameters.Add("@finance_request_status", finance_request_status);
                    sqlcmd.Parameters.Add("@Mode", 1);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.total_count = Convert.ToInt32(dt.Rows[0]["total_count"]);
                            omodel.finance_request_list = APIHelperMethods.ToModelList<FinanceRequest>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region Sales Manager  Finance Request Details

        [AcceptVerbs("POST")]
        public JsonResult SalesManagerFinanceRequestDetails(string finance_req_id, string Token)
        {
            FinanceRequestDetailssalesfinanceOutput omodel = new FinanceRequestDetailssalesfinanceOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_GetFinanceRequestDetailsslamanfinance", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@finance_req_id", finance_req_id);
                    sqlcmd.Parameters.Add("@Mode", 1);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.exchange_amount = Convert.ToString(dt.Rows[0]["exchange_amount"]);
                            omodel.view_finance_details = APIHelperMethods.ToModelList<productbasketforFinanceRequest>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region   SalesMan List and Search

        [AcceptVerbs("POST")]
        public JsonResult SalesmanListSearch(string user_id, string Token, int? pageno, int? rowcount, string salesman_name = null)
        {
            SalesmanagersalesmanRequestOutput omodel = new SalesmanagersalesmanRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    if (salesman_name == "")
                    {
                        salesman_name = null;
                    }
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_GetFinanceSalesManAllList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@customer_name", salesman_name);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.total_count = Convert.ToInt32(dt.Rows[0]["total_count"]);

                            omodel.salesman_list = APIHelperMethods.ToModelList<SalesmanListManger>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region   FINANCER Finance Request List

        [AcceptVerbs("POST")]
        public JsonResult FinancerfinanceRequestList(string user_id, string Token, string finance_request_status, int? pageno, int? rowcount)
        {
            FinanceRequestOutput omodel = new FinanceRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    if (finance_request_status == "")
                    {
                        finance_request_status = null;
                    }


                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("GetFinanceList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@finance_request_status", finance_request_status);
                    sqlcmd.Parameters.Add("@Mode", 2);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.total_count = Convert.ToInt32(dt.Rows[0]["total_count"]);
                            omodel.finance_request_list = APIHelperMethods.ToModelList<FinanceRequest>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region   FINANCER Finance Request List  (Search)

        [AcceptVerbs("POST")]
        public JsonResult FinancerfinanceRequestListSearch(string user_id, string customer_name, string finance_request_status, string Token, int? pageno, int? rowcount)
        {
            FinanceRequestOutput omodel = new FinanceRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    if (customer_name == "")
                    {
                        customer_name = null;

                    }
                    if (finance_request_status == "")
                    {
                        finance_request_status = null;
                    }
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("GetFinanceList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@customer_name", customer_name);
                    sqlcmd.Parameters.Add("@finance_request_status", finance_request_status);
                    sqlcmd.Parameters.Add("@Mode", 2);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.total_count = Convert.ToInt32(dt.Rows[0]["total_count"]);
                            omodel.finance_request_list = APIHelperMethods.ToModelList<FinanceRequest>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region  Finance Request Details (Accept)
        [AcceptVerbs("POST")]
        public JsonResult FinanceRequestDetailsAccept(FinancedetailsAcceptInput model)
        {
            FinanceRequestOutput omodel = new FinanceRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();


                    sqlcmd = new SqlCommand("API_Finance_Acceptdetails", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@finance_userid", model.finance_userid);

                    sqlcmd.Parameters.Add("@finance_req_id", model.finance_req_id);
                    sqlcmd.Parameters.Add("@finance_approval_no", model.finance_approval_no);
                    sqlcmd.Parameters.Add("@total_amount", model.total_amount);
                    sqlcmd.Parameters.Add("@processing_fee", model.processing_fee);
                    sqlcmd.Parameters.Add("@loan_amount", model.loan_amount);
                    sqlcmd.Parameters.Add("@other_charges", model.other_charges);
                    sqlcmd.Parameters.Add("@finance_scheme", model.finance_scheme);
                    sqlcmd.Parameters.Add("@dbd_no", model.dbd_no);
                    sqlcmd.Parameters.Add("@downpayment", model.downpayment);
                    sqlcmd.Parameters.Add("@no_of_emi", model.no_of_emi);
                    sqlcmd.Parameters.Add("@emi_amount", model.emi_amount);
                    sqlcmd.Parameters.Add("@Mode", 3);  //for billing previous was 1
                    sqlcmd.Parameters.Add("@branch_id", model.branch_id);
                    sqlcmd.Parameters.Add("@Customer_name", model.Customer_name);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            oerror.ResponseCode = "200";
                            oerror.Responsedetails = "Success";
                            // omodel.finance_request_list = APIHelperMethods.ToModelList<FinanceRequest>(dt);
                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }


        #endregion

        #region Finance Request Details (Reject)
        [AcceptVerbs("POST")]
        public JsonResult FinanceRequestDetailsDelete(FinancedetailsAcceptInput model)
        {
            FinanceRequestOutput omodel = new FinanceRequestOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Finance_Acceptdetails", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@finance_userid", model.finance_userid);
                    sqlcmd.Parameters.Add("@finance_req_id", model.finance_req_id);

                    sqlcmd.Parameters.Add("@reason", model.reason);
                    sqlcmd.Parameters.Add("@Mode", 2);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            oerror.ResponseCode = "200";
                            oerror.Responsedetails = "Success";
                            // omodel.finance_request_list = APIHelperMethods.ToModelList<FinanceRequest>(dt);
                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region #### Get customer Details by mobile number ####

        [AcceptVerbs("POST")]
        public JsonResult Getcustomerbymobilenumber(string customer_mobile_no, string Token)
        {
            saleProgressOutputpara omodel = new saleProgressOutputpara();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataSet dt = new DataSet();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Mobileduplicatecheck", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@phone", customer_mobile_no);

                    sqlcmd.Parameters.Add("@Mode", 1);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        if (dt.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.sale_on_progress_list = APIHelperMethods.ToModelList<Customer>(dt.Tables[0]);

                            foreach (Customer CLS in omodel.sale_on_progress_list)
                            {
                                DataRow[] DRR = dt.Tables[1].Select("INTERNAL_ID='" + CLS.customer_id + "'");
                                DataTable dtNew = new DataTable();

                                if (DRR.Length > 0)
                                {
                                    dtNew = DRR.CopyToDataTable();
                                }

                                CLS.req_list = APIHelperMethods.ToModelList<requirement>(dtNew);

                            }



                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region View Finance Status Customer

        [AcceptVerbs("POST")]

        public JsonResult SalesManCustomerFinanceList(string user_id, string Token, string request_status, string cust_name = null)
        {
            DataTable dt2 = new DataTable();
            ViewFinanceStatusCustomerOutput omodel = new ViewFinanceStatusCustomerOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    if (cust_name == "")
                    {

                        cust_name = null;
                    }

                    if (request_status == "")
                    {

                        request_status = null;
                    }
                    List<FinanceRequestCustomer> omm = new List<FinanceRequestCustomer>();
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_ViewFinanceStatusCustomer", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@cust_name", cust_name);
                    sqlcmd.Parameters.Add("@request_status", request_status);
                    sqlcmd.Parameters.Add("@Mode", 1);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                long finance_req_id = Convert.ToInt64(dt.Rows[i]["finance_req_id"]);
                                string customer_id = Convert.ToString(dt.Rows[i]["customer_id"]);
                                string customer_name = Convert.ToString(dt.Rows[i]["customer_name"]);
                                string customer_mobile_no = Convert.ToString(dt.Rows[i]["customer_mobile_no"]);
                                string salesman_id = Convert.ToString(dt.Rows[i]["salesman_id"]);
                                string finance_requested_date = Convert.ToString(dt.Rows[i]["finance_requested_date"]);
                                string finance_requested_status = Convert.ToString(dt.Rows[i]["finance_requested_status"]);




                                dt2.Clear();
                                FinanceRequestCustomer omodel2 = new FinanceRequestCustomer();
                                sqlcmd = new SqlCommand("API_ViewFinanceStatusCustomer", sqlcon);
                                sqlcmd.CommandType = CommandType.StoredProcedure;
                                sqlcmd.Parameters.Add("@finance_req_id", Convert.ToInt32(dt.Rows[i]["finance_req_id"]));
                                sqlcmd.Parameters.Add("@Mode", 2);
                                sqlcmd.Parameters.Add("@cust_name", cust_name);
                                SqlDataAdapter da1 = new SqlDataAdapter(sqlcmd);
                                da1.Fill(dt2);
                                sqlcon.Close();

                                List<FinanceRequestFinance> omm2 = new List<FinanceRequestFinance>();

                                if (dt2.Rows.Count > 0)
                                {
                                    if (dt2.Rows[0][0].ToString() != "0")
                                    {
                                        for (int j = 0; j < dt2.Rows.Count; j++)
                                        {

                                            omm2.Add(
                                      new FinanceRequestFinance()
                                      {
                                          financer_name = Convert.ToString(dt2.Rows[j]["financer_name"]),
                                          financer_id = Convert.ToString(dt2.Rows[j]["financer_id"]),
                                          rejected_reason = Convert.ToString(dt2.Rows[j]["rejected_reason"]),
                                          finance_status = Convert.ToString(dt2.Rows[j]["finance_status"]),
                                          finance_status_date = Convert.ToString(dt2.Rows[j]["finance_status_date"]),
                                          Is_financer_to_apply = Convert.ToBoolean(dt2.Rows[j]["Is_financer_to_apply"]),
                                      });

                                        }

                                    }

                                }

                                omm.Add(
                                  new FinanceRequestCustomer()
                                  {
                                      finance_req_id = finance_req_id,
                                      customer_id = customer_id,
                                      customer_name = customer_name,
                                      customer_mobile_no = customer_mobile_no,
                                      salesman_id = salesman_id,
                                      finance_requested_date = finance_requested_date,
                                      finance_requested_status = finance_requested_status,
                                      financer_list = omm2
                                  });



                            }

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.finance_status_details = omm;
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region View Finance Status (Apply Again)
        [AcceptVerbs("POST")]
        public JsonResult FinancestatusApplyAgain(Viewfinancestatusinput model)
        {

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];

                if (token == model.Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Customer_Applyagain_forFinance", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@financer_id", model.financer_id);
                    sqlcmd.Parameters.Add("@finance_request_id", model.finance_request_id);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            oerror.ResponseCode = "200";
                            oerror.Responsedetails = "Success";
                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Notification SalesMan
        [AcceptVerbs("POST")]
        public JsonResult Salesmannotification(string user_id, string Token)
        {
            ViewNotificationsales omodel = new ViewNotificationsales();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Notifications", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@userid", user_id);
                    sqlcmd.Parameters.Add("@Mode", 1);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.notification_details = APIHelperMethods.ToModelList<ViewNotificationsalesdiscount>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Notification SalesManager
        [AcceptVerbs("POST")]
        public JsonResult Salesmanagernotification(string user_id, string Token)
        {
            ViewNotificationsalesmanager omodel = new ViewNotificationsalesmanager();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Notifications", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@userid", user_id);
                    sqlcmd.Parameters.Add("@Mode", 2);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.notification_details = APIHelperMethods.ToModelList<ViewNotificationsalesmanagerdiscount>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Notification Financer
        [AcceptVerbs("POST")]
        public JsonResult Financernotification(string user_id, string Token)
        {
            ViewNotificationfinancerOutput omodel = new ViewNotificationfinancerOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Notifications", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@userid", user_id);
                    sqlcmd.Parameters.Add("@Mode", 3);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.notification_list = APIHelperMethods.ToModelList<ViewNotificationfinance>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Notification Top Approval
        [AcceptVerbs("POST")]
        public JsonResult Topapprovalnotification(string user_id, string Token)
        {
            ViewNotificationsalesmanager omodel = new ViewNotificationsalesmanager();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Notifications", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@userid", user_id);
                    sqlcmd.Parameters.Add("@Mode", 5);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.notification_details = APIHelperMethods.ToModelList<ViewNotificationsalesmanagerdiscount>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }


        }
        #endregion

        #region Pushnotification by FCM
        public static void SendPushNotification(string message, string deviceid, string Customer, string Requesttype)
        {
            try
            {
                string applicationID = "AIzaSyBec9GYzFbhHN3R1VffHKi4WSYSPRyV4Q4";
                string senderId = "392508903088";
                string deviceId = deviceid;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var data2 = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = message,
                        title = ""
                    },
                    data = new
                    {
                        customer = Customer,
                        customerId = Requesttype
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data2);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }


        #endregion

        #region GetdeviceId

        public DataTable GetdeviceId(string UserId, string usertype, string customerId, string Requesttype)
        {
            string DeviceId = "";
            DataTable dt2 = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();

            sqlcmd = new SqlCommand("Get_DeviceId", sqlcon);

            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.Add("@userid", UserId);
            sqlcmd.Parameters.Add("@Usertype", usertype);
            sqlcmd.Parameters.Add("@customerId", customerId);
            sqlcmd.Parameters.Add("@Requesttype", Requesttype);

            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt2);

            if (dt2.Rows.Count > 0)
            {
                // DeviceId = Convert.ToString(dt2.Rows[0]["Mac_Address"]);
                return dt2;
            }

            sqlcon.Close();

            //return DeviceId;
            return null;
        }
        #endregion

        #region  New API:- Change Notification read status
        [AcceptVerbs("POST")]
        public JsonResult ChangeNotificationreadstatus(string notification_id, string user_type, string Token)
        {
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_Notifications", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@notification_id", notification_id);
                    sqlcmd.Parameters.Add("@user_type", user_type);
                    sqlcmd.Parameters.Add("@Mode", 4);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            oerror.ResponseCode = "200";
                            oerror.Responsedetails = "Success";

                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Get notificatiion count
        public JsonResult GetNotificationCount(string user_id, string user_type, string Token)
        {
            UserLoginOutputParameters omodel = new UserLoginOutputParameters();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Proc_API_GetnotificationCount", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@UserId", user_id);
                    sqlcmd.Parameters.Add("@User_Type", user_type);



                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.notification_count = Convert.ToString(dt.Rows[0]["notification_count"]);
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No unread Count found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region Check IMEI Number
        public JsonResult GetIMEIExists(string imei_number, string Token)
        {
            //  UserLoginOutputParameters omodel = new UserLoginOutputParameters();
            IMEIClass omodel = new IMEIClass();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Proc_API_IMEICheck", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@imei_number", imei_number);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel = APIHelperMethods.ToModel<IMEIClass>(dt);

                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No unread Count found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }

        #endregion

        #region ### GetCompany Logo   ####
        public JsonResult GetCompanyLogo()
        {
            CompanyLogoclass oerror = new CompanyLogoclass();
            string FilePath = ConfigurationManager.AppSettings["Companylogo"].ToString();


            oerror.ResponseCode = "200";
            oerror.Responsedetails = "Success";
            oerror.logo_url = FilePath;
            return Json(oerror, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region My Sales Report
        [AcceptVerbs("POST")]
        public JsonResult MySalesReport(string user_id, string request_type, string Token)
        {
            MysalesReportOutput omodel = new MysalesReportOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Proc_MysalesCustomer", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@userid", user_id);
                    sqlcmd.Parameters.Add("@request_type", request_type);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.total_sales = Convert.ToInt32(dt.Rows[0]["total_sales"]);
                            omodel.customer_approval_details = APIHelperMethods.ToModelList<MysalesReportCustomerOutput>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region My Sales Details Report
        [AcceptVerbs("POST")]
        public JsonResult MySalesDetailsReport(string user_id, string request_id, string Token)
        {
            Salesmandiscountapproval omodel = new Salesmandiscountapproval();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Proc_MysalesDetailsCustomer", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@request_id", request_id);


                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.RequestId = Convert.ToInt32(dt.Rows[0]["RequestId"]);
                            omodel.exchange_amount = Convert.ToDecimal(dt.Rows[0]["exchange_amount"]);
                            omodel.payment_type = Convert.ToString(dt.Rows[0]["payment_type"]);
                            omodel.view_approval_details = APIHelperMethods.ToModelList<productfordiscountsalesman>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";

                        return Json(oerror);
                    }

                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";

                    return Json(oerror);

                }


            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }

        }
        #endregion

        #region User Change password
        [AcceptVerbs("POST")]
        public JsonResult Changepassword(ChangePasswordInput model)
        {

            ErrorModel omodelerror = new ErrorModel();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    string sessionId = HttpContext.Session.SessionID;

                    DataTable dt = new DataTable();
                    Encryption epasswrd = new Encryption();
                    string Encryptpassold = epasswrd.Encrypt(model.Old_password);
                    string Encryptpassnew = epasswrd.Encrypt(model.New_password);

                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];

                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("Sp_ApiChangePassword", sqlcon);
                    sqlcmd.Parameters.Add("@User_id", model.User_id);
                    sqlcmd.Parameters.Add("@Old_password", Encryptpassold);
                    sqlcmd.Parameters.Add("@New_password", Encryptpassnew);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodelerror.ResponseCode = "200";
                            omodelerror.Responsedetails = "Success";
                            return Json(omodelerror);
                        }
                        else
                        {
                            omodelerror.ResponseCode = "201";
                            omodelerror.Responsedetails = "Old Password does not matched";
                            return Json(omodelerror);
                        }

                    }
                    else
                    {
                        omodelerror.ResponseCode = "201";
                        omodelerror.Responsedetails = "Invalid User Id";
                        return Json(omodelerror);
                    }


                }
                else
                {
                    omodelerror.ResponseCode = "103";
                    omodelerror.Responsedetails = "Token Id not match";
                    return Json(omodelerror);

                }

            }
            catch
            {

                omodelerror.ResponseCode = "103";
                omodelerror.Responsedetails = "Error occured";
                return Json(omodelerror);

            }


        }

        #endregion


        #region PRoductList-Old Unit
        [AcceptVerbs("GET")]
        public JsonResult ProductListforOldunit(string Token)
        {
            ProductListOldunitOutput omodel = new ProductListOldunitOutput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("API_ProductlistOldunit", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";

                            omodel.productold_details = APIHelperMethods.ToModelList<ProductDetailsOld>(dt);
                            return Json(omodel, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No record Found.";
                            return Json(oerror, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record found";

                        return Json(oerror, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not Match";
                    return Json(oerror, JsonRequestBehavior.AllowGet);
                }



            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Saleon Progress Send Discount Request For Order 16-04-2019
        [AcceptVerbs("POST")]
        public JsonResult SaleonProgressSendDiscountRequestOrder(SenddiscountSalesonprogressInput model)
        {
            SenddiscountSalesonprogressOuput omodel = new SenddiscountSalesonprogressOuput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    List<Productsrequestdetails> obj = new List<Productsrequestdetails>();
                    foreach (var item in model.request_details)
                    {
                        obj.Add(new Productsrequestdetails()
                        {
                            discount_percentage = item.discount_percentage,
                            product_quantity = item.product_quantity,
                            product_id = item.product_id,
                            Salesman_Isapplied = item.Salesman_Isapplied,
                            discount_amount = item.discount_amount,
                            final_discount_price = item.final_discount_price,
                            piece_quantity = item.piece_quantity
                        });
                    }

                    string ProductXML = APIHelperMethods.ConvertToXml(obj, 0);
                    string ProductOldunitXML = null;
                    List<ProductsOldunitrequestdetails> oldunit_obj = new List<ProductsOldunitrequestdetails>();
                    if (model.oldunit_details != null)
                    {
                        foreach (var item in model.oldunit_details)
                        {
                            oldunit_obj.Add(new ProductsOldunitrequestdetails()
                            {
                                product_id = item.product_id,
                                quantity = item.quantity,
                                amount = item.amount
                            });
                        }
                        ProductOldunitXML = APIHelperMethods.ConvertToXml(oldunit_obj, 0);
                    }

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APICustomerDiscountquantityRequest", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@request_type", model.request_type);
                    sqlcmd.Parameters.Add("@payment_type", model.payment_type);
                    sqlcmd.Parameters.Add("@exchange_amount", model.exchange_amount);
                    sqlcmd.Parameters.Add("@financer_id", model.financer_id);
                    sqlcmd.Parameters.Add("@RequestId", model.RequestId);
                    sqlcmd.Parameters.Add("@ProductList", ProductXML);
                    sqlcmd.Parameters.Add("@ProductOldunitList", ProductOldunitXML);
                    sqlcmd.Parameters.Add("@ORDER_TYPE", "ORD");

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel = APIHelperMethods.ToModel<SenddiscountSalesonprogressOuput>(dt);
                            DataTable getdevicelist = new DataTable();
                            getdevicelist = GetdeviceId(model.user_id, "Salesman", model.customer_id, model.request_type);
                            if (getdevicelist != null)
                            {
                                for (int i = 0; i < getdevicelist.Rows.Count; i++)
                                {
                                    if (Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]) != "")
                                    {
                                        SendPushNotification("", Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]), Convert.ToString(getdevicelist.Rows[i]["CustomerName"]), model.request_type);
                                    }
                                }
                            }
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion

        #region Saleon Progress Send Discount Request For Quotation 16-04-2019
        [AcceptVerbs("POST")]
        public JsonResult SaleonProgressSendDiscountRequestQuotation(SenddiscountSalesonprogressInput model)
        {
            SenddiscountSalesonprogressOuput omodel = new SenddiscountSalesonprogressOuput();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    List<Productsrequestdetails> obj = new List<Productsrequestdetails>();
                    foreach (var item in model.request_details)
                    {
                        obj.Add(new Productsrequestdetails()
                        {
                            discount_percentage = item.discount_percentage,
                            product_quantity = item.product_quantity,
                            product_id = item.product_id,
                            Salesman_Isapplied = item.Salesman_Isapplied,
                            discount_amount = item.discount_amount,
                            final_discount_price = item.final_discount_price,
                            piece_quantity = item.piece_quantity
                        });
                    }

                    string ProductXML = APIHelperMethods.ConvertToXml(obj, 0);
                    string ProductOldunitXML = null;
                    List<ProductsOldunitrequestdetails> oldunit_obj = new List<ProductsOldunitrequestdetails>();
                    if (model.oldunit_details != null)
                    {
                        foreach (var item in model.oldunit_details)
                        {
                            oldunit_obj.Add(new ProductsOldunitrequestdetails()
                            {
                                product_id = item.product_id,
                                quantity = item.quantity,
                                amount = item.amount
                            });
                        }
                        ProductOldunitXML = APIHelperMethods.ConvertToXml(oldunit_obj, 0);
                    }

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("APICustomerDiscountquantityRequest", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@request_type", model.request_type);
                    sqlcmd.Parameters.Add("@payment_type", model.payment_type);
                    sqlcmd.Parameters.Add("@exchange_amount", model.exchange_amount);
                    sqlcmd.Parameters.Add("@financer_id", model.financer_id);
                    sqlcmd.Parameters.Add("@RequestId", model.RequestId);
                    sqlcmd.Parameters.Add("@ProductList", ProductXML);
                    sqlcmd.Parameters.Add("@ProductOldunitList", ProductOldunitXML);
                    sqlcmd.Parameters.Add("@ORDER_TYPE", "QUOT");

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel = APIHelperMethods.ToModel<SenddiscountSalesonprogressOuput>(dt);
                            DataTable getdevicelist = new DataTable();
                            getdevicelist = GetdeviceId(model.user_id, "Salesman", model.customer_id, model.request_type);
                            if (getdevicelist != null)
                            {
                                for (int i = 0; i < getdevicelist.Rows.Count; i++)
                                {
                                    if (Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]) != "")
                                    {
                                        SendPushNotification("", Convert.ToString(getdevicelist.Rows[i]["Mac_Address"]), Convert.ToString(getdevicelist.Rows[i]["CustomerName"]), model.request_type);
                                    }
                                }
                            }
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Record Found";
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "No Record Found";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion

        #region Order Details with product 18-04-2019
        [AcceptVerbs("POST")]
        public JsonResult OrderDetails(orderDetailsInput ordInput)
        {
            Commonclass oerror = new Commonclass();
            DataSet ds = new DataSet();

            String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
            if (token == ordInput.Token)
            {
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("API_VIEW_OREDR_INVOICE", sqlcon);
                sqlcmd.Parameters.Add("@USER_ID", ordInput.user_id);
                sqlcmd.Parameters.Add("@FROM_DATE", ordInput.from_date);
                sqlcmd.Parameters.Add("@TODATE", ordInput.to_date);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(ds);
                sqlcon.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    OrderDetailsProduct orddetlprd = new OrderDetailsProduct();
                    List<oredrdetailsMain> ordemodel = new List<oredrdetailsMain>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        List<OrderDetailsProductList> ordemodelprod = new List<OrderDetailsProductList>();
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {


                            int i1 = 0;
                            if (Convert.ToString(ds.Tables[1].Rows[j]["Order_Id"]) == Convert.ToString(ds.Tables[0].Rows[i]["Order_Id"]))
                            {
                                ordemodelprod.Add(new OrderDetailsProductList()
                                 {
                                     product_id = Convert.ToInt32(ds.Tables[1].Rows[j]["product_id"]),
                                     product_name = Convert.ToString(ds.Tables[1].Rows[j]["product_name"]),
                                     qty = Convert.ToDecimal(ds.Tables[1].Rows[j]["qty"]),
                                     unit = Convert.ToString(ds.Tables[1].Rows[j]["unit"]),
                                     price = Convert.ToDecimal(ds.Tables[1].Rows[j]["price"])
                                 });

                            }
                        }

                        ordemodel.Add(new oredrdetailsMain()
                           {
                               order_id = Convert.ToString(ds.Tables[0].Rows[i]["Order_Id"]),
                               order_no = Convert.ToString(ds.Tables[0].Rows[i]["Order_Number"]),
                               invoice_no = Convert.ToString(ds.Tables[0].Rows[i]["Invoice_Number"]),
                               order_date = Convert.ToString(ds.Tables[0].Rows[i]["Order_Date"]) != "" ? Convert.ToDateTime(ds.Tables[0].Rows[i]["Order_Date"]).ToString("dd/MM/yy") : "",
                               invoice_date = Convert.ToString(ds.Tables[0].Rows[i]["Invoice_Date"]) != "" ? Convert.ToDateTime(ds.Tables[0].Rows[i]["Invoice_Date"]).ToString("dd/MM/yy") : "",
                               item_no = Convert.ToInt32(ds.Tables[0].Rows[i]["item_no"]),
                               amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["amount"]),
                               order_owner_name = Convert.ToString(ds.Tables[0].Rows[i]["order_owner_name"]),
                               order_address = Convert.ToString(ds.Tables[0].Rows[i]["order_address"]),
                               order_owner_phn_no = Convert.ToString(ds.Tables[0].Rows[i]["order_owner_phn_no"]),
                               invoice_amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["invoice_amount"]),
                               product_list = ordemodelprod
                           });
                    }
                    orddetlprd.ResponseCode = "200";
                    orddetlprd.Responsedetails = "success";
                    orddetlprd.order_list = ordemodel;
                    return Json(orddetlprd);

                }
                else
                {
                    oerror.ResponseCode = "201";
                    oerror.Responsedetails = "No data found";
                    return Json(oerror);
                }
            }

            else
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Token Id not match";
                return Json(oerror);

            }
        }
        #endregion

        #region New Api for Amit

        #region Registration
        [AcceptVerbs("POST")]
        public JsonResult CustomerRegistration(CustomerRegistrationInput model)
        {
            CustomerRegistrationOutput omodel = new CustomerRegistrationOutput();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    if (!string.IsNullOrEmpty(model.FirstName))
                    {
                        if (!string.IsNullOrEmpty(model.UniqueID))
                        {
                            string output = string.Empty;
                            DataTable dt = new DataTable();
                            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                            SqlCommand sqlcmd = new SqlCommand();
                            SqlConnection sqlcon = new SqlConnection(con);
                            sqlcon.Open();
                            sqlcmd = new SqlCommand("PROC_API_LEADCONTACTINSERT", sqlcon);
                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.Parameters.Add("@contacttype", "Lead");
                            sqlcmd.Parameters.Add("@cnt_contactType", "LD");
                            sqlcmd.Parameters.Add("@cnt_firstName", model.FirstName);
                            sqlcmd.Parameters.Add("@cnt_lastName", model.LastName);
                            sqlcmd.Parameters.Add("@salutation", model.Salutation);
                            sqlcmd.Parameters.Add("@cnt_UCC", model.UniqueID);
                            sqlcmd.Parameters.Add("@result", SqlDbType.Char, 50);
                            sqlcmd.Parameters["@result"].Direction = ParameterDirection.Output;
                            sqlcmd.CommandTimeout = 0;
                            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                            da.Fill(dt);
                            output = (string)sqlcmd.Parameters["@result"].Value;
                            sqlcon.Close();
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0][0].ToString() != "0")
                                {
                                    omodel.Lead_InternalID = dt.Rows[0]["customer_id"].ToString();
                                    omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                                    omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                                    return Json(omodel);
                                }
                                else
                                {
                                    oerror.ResponseCode = "201";
                                    oerror.Responsedetails = "Error";
                                    return Json(oerror);
                                }

                            }
                            else
                            {
                                oerror.ResponseCode = "201";
                                oerror.Responsedetails = "Error";
                                return Json(oerror);
                            }
                        }
                        else
                        {
                            oerror.ResponseCode = "103";
                            oerror.Responsedetails = "Unique ID is Mandatory.";
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "103";
                        oerror.Responsedetails = "First Name is Mandatory.";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion

        #region LeadsCustomerDetails
        [AcceptVerbs("POST")]
        public JsonResult LeadsCustomerDetails(CustomerRegistrationInput model)
        {
            List<LeadUserDetails> omodel = new List<LeadUserDetails>();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                String leadSync = System.Configuration.ConfigurationSettings.AppSettings["SyncLeads"];
                if (token == model.Token)
                {
                    string output = string.Empty;
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlCommand sqlcmdUP = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    SqlConnection sqlconUP = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PROC_API_LEADCONTACTVIEW", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@leadSync", leadSync);
                    sqlcmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["MSG"].ToString() == "sucess")
                        {
                            omodel = APIHelperMethods.ToModelList<LeadUserDetails>(dt);
                            sqlconUP.Open();
                            sqlcmdUP = new SqlCommand("PROC_API_LEADSYNCUPDATE", sqlconUP);
                            sqlcmdUP.CommandType = CommandType.StoredProcedure;
                            sqlcmdUP.ExecuteNonQuery();
                            sqlconUP.Close();
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Data Found";
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion

        [AcceptVerbs("POST")]
        public JsonResult ProductCatalogue(CustomerRegistrationInput ordInput)
        {
            Commonclass oerror = new Commonclass();
            DataSet ds = new DataSet();

            String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
            String leadSync = System.Configuration.ConfigurationSettings.AppSettings["SyncLeads"];
            if (token == ordInput.Token)
            {
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PROC_API_ProductDetailsComponent", sqlcon);
                sqlcmd.Parameters.Add("@ProductSync", leadSync);
                sqlcmd.CommandTimeout = 0;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(ds);
                sqlcon.Close();
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "Sucess")
                    {
                        productDetails detlprd = new productDetails();
                        List<productCatalogue> prdCatelog = new List<productCatalogue>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (prdCatelog.Count == 1097)
                            {

                            }
                            List<ComponentsList> Compoprod = new List<ComponentsList>();
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                {
                                    int i1 = 0;
                                    if (Convert.ToString(ds.Tables[1].Rows[j]["Product_id"]) == Convert.ToString(ds.Tables[0].Rows[i]["sProducts_ID"]))
                                    {
                                        Compoprod.Add(new ComponentsList()
                                        {
                                            ComponentName = Convert.ToString(ds.Tables[1].Rows[j]["ComponentName"]),
                                            Componentid = Convert.ToString(ds.Tables[1].Rows[j]["Component_prodId"]),
                                            Product_id = Convert.ToString(ds.Tables[1].Rows[j]["Product_id"])
                                        });

                                    }
                                }
                            }

                            prdCatelog.Add(new productCatalogue()
                            {
                                sProducts_ID = Convert.ToString(ds.Tables[0].Rows[i]["sProducts_ID"]),
                                sProducts_Code = Convert.ToString(ds.Tables[0].Rows[i]["sProducts_Code"]),
                                sProducts_Name = Convert.ToString(ds.Tables[0].Rows[i]["sProducts_Name"]),
                                sProducts_Description = Convert.ToString(ds.Tables[0].Rows[i]["sProducts_Description"]),
                                ProductColor = Convert.ToString(ds.Tables[0].Rows[i]["ProductColor"]),
                                ProductSize = Convert.ToString(ds.Tables[0].Rows[i]["ProductSize"]),
                                Brand = Convert.ToString(ds.Tables[0].Rows[i]["Brand"]),
                                InstallationRequired = Convert.ToString(ds.Tables[0].Rows[i]["InstallationRequired"]),
                                ProductSeries = Convert.ToString(ds.Tables[0].Rows[i]["ProductSeries"]),
                                Surface = Convert.ToString(ds.Tables[0].Rows[i]["Surface"]),
                                LeadTime = Convert.ToString(ds.Tables[0].Rows[i]["LeadTime"]),
                                WEIGHT = Convert.ToString(ds.Tables[0].Rows[i]["WEIGHT"]),
                                SUBCATEGORY = Convert.ToString(ds.Tables[0].Rows[i]["SUBCATEGORY"]),
                                LENGTH = Convert.ToString(ds.Tables[0].Rows[i]["LENGTH"]),
                                WIDTH = Convert.ToString(ds.Tables[0].Rows[i]["WIDTH"]),
                                THICKNESS = Convert.ToString(ds.Tables[0].Rows[i]["THICKNESS"]),
                                UOM = Convert.ToString(ds.Tables[0].Rows[i]["UOM"]),
                                CoverageArea = Convert.ToString(ds.Tables[0].Rows[i]["CoverageArea"]),
                                VOLUME = Convert.ToString(ds.Tables[0].Rows[i]["VOLUME"]),
                                ProductNature_Name = Convert.ToString(ds.Tables[0].Rows[i]["ProductNature_Name"]),
                                ProductApplication_Name = Convert.ToString(ds.Tables[0].Rows[i]["ProductApplication_Name"]),
                                APPLICATION_NAME = Convert.ToString(ds.Tables[0].Rows[i]["APPLICATION_NAME"]),
                                CATEGORY_NAME = Convert.ToString(ds.Tables[0].Rows[i]["CATEGORY_NAME"]),
                                MOV_NAME = Convert.ToString(ds.Tables[0].Rows[i]["MOV_NAME"]),
                                ProductPedestalNo = Convert.ToString(ds.Tables[0].Rows[i]["ProductPedestalNo"]),
                                ProductCatNo = Convert.ToString(ds.Tables[0].Rows[i]["ProductCatNo"]),
                                ProductWarranty = Convert.ToString(ds.Tables[0].Rows[i]["ProductWarranty"]),

                                Component = Compoprod
                            });
                        }
                        detlprd.ResponseCode = "200";
                        detlprd.Responsedetails = "success";
                        detlprd.ProductList = prdCatelog;
                        return Json(detlprd);
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = ds.Tables[0].Rows[0]["MSG"].ToString();
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "201";
                    oerror.Responsedetails = "No data found";
                    return Json(oerror);
                }
            }

            else
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Token Id not match";
                return Json(oerror);

            }
        }

        [AcceptVerbs("POST")]
        public JsonResult CartInsert(CartDetailsInput model)
        {
            CartDetailsOutPut omodel = new CartDetailsOutPut();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    string output = string.Empty;
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PROC_API_CARTINSERT", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@product_id", model.product_id);
                    sqlcmd.Parameters.Add("@product_price", model.product_price);
                    sqlcmd.Parameters.Add("@customer_id", model.customer_id);
                    sqlcmd.Parameters.Add("@quantity", model.quantity);
                    sqlcmd.Parameters.Add("@salesman_id", model.salesman_id);
                    sqlcmd.Parameters.Add("@discount_percent", model.discount_percent);
                    sqlcmd.Parameters.Add("@piece_quantity", model.piece_quantity);
                    sqlcmd.Parameters.Add("@result", SqlDbType.Char, 50);
                    sqlcmd.Parameters["@result"].Direction = ParameterDirection.Output;
                    sqlcmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    output = (string)sqlcmd.Parameters["@result"].Value;
                    sqlcon.Close();
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Responsedetails"].ToString() == "SUCESS")
                        {
                            omodel.CART_ID = dt.Rows[0]["CART_ID"].ToString();
                            omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Error";
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult InsertAbandonedCart(CartDetailsBatchInput model)
        {
            CartDetailsOutPut omodel = new CartDetailsOutPut();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    string output = string.Empty;
                    List<CartDetailsBatch> obj = new List<CartDetailsBatch>();
                    foreach (var item in model.CartDetailsList)
                    {
                        obj.Add(new CartDetailsBatch()
                        {
                            product_id = item.product_id,
                            product_price = item.product_price,
                            customer_id = item.customer_id,
                            quantity = item.quantity,
                            salesman_id = item.salesman_id,
                            discount_percent = item.discount_percent,
                            piece_quantity = item.piece_quantity
                        });
                    }

                    string abandonedcartXML = APIHelperMethods.ConvertToXml(obj, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("proc_APIBatchCartInsert", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@CART_TYPE", "Abandoned");
                    sqlcmd.Parameters.Add("@abandonedcartList", abandonedcartXML);
                    sqlcmd.Parameters.Add("@result", SqlDbType.Char, 50);
                    sqlcmd.Parameters["@result"].Direction = ParameterDirection.Output;
                    // sqlcmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    output = (string)sqlcmd.Parameters["@result"].Value;
                    sqlcon.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Responsedetails"].ToString() == "SUCESS")
                        {
                            omodel.CART_ID = dt.Rows[0]["CART_ID"].ToString();
                            omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(omodel);
                        }
                        else
                        {
                            omodel.CART_ID = dt.Rows[0]["CART_ID"].ToString();
                            omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(omodel);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult AddBasketOrder(BasketMain model)
        {
            BasketDetailsOutPut omodel = new BasketDetailsOutPut();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                String SaleManID = System.Configuration.ConfigurationSettings.AppSettings["SaleManID"];
                if (token == model.Token)
                {
                    List<BasketDetails> obj = new List<BasketDetails>();
                    foreach (var item in model.BasketDetailsList)
                    {
                        obj.Add(new BasketDetails()
                        {
                            product_id = item.product_id,
                            ProductName = item.ProductName,
                            ProductPrice = item.ProductPrice,
                            ProductQuantity = item.ProductQuantity,
                            TotalProductPrice = item.TotalProductPrice,
                            Finaldiscountedamount = item.Finaldiscountedamount,
                            piece_quantity = item.piece_quantity
                        });
                    }

                    string ProductcartXML = APIHelperMethods.ConvertToXml(obj, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("PROC_API_BasketAdd", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@CustomerId", model.CustomerId);
                    sqlcmd.Parameters.Add("@SalesmanId", SaleManID);
                    sqlcmd.Parameters.Add("@ORDER_TYPE", "ORD");
                    sqlcmd.Parameters.Add("@ProductList", ProductcartXML);
                    // sqlcmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Responsedetails"].ToString() == "SUCESS")
                        {
                            omodel.Order_Awaiting_id = dt.Rows[0]["temp_request_id"].ToString();
                            omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(omodel);
                        }
                        else
                        {
                            omodel.Order_Awaiting_id = dt.Rows[0]["temp_request_id"].ToString();
                            omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(omodel);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult AddBasketWishlist(BasketMain model)
        {
            BasketDetailsOutPut omodel = new BasketDetailsOutPut();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                String SaleManID = System.Configuration.ConfigurationSettings.AppSettings["SaleManID"];
                if (token == model.Token)
                {
                    List<BasketDetails> obj = new List<BasketDetails>();
                    foreach (var item in model.BasketDetailsList)
                    {
                        obj.Add(new BasketDetails()
                        {
                            product_id = item.product_id,
                            ProductName = item.ProductName,
                            ProductPrice = item.ProductPrice,
                            ProductQuantity = item.ProductQuantity,
                            TotalProductPrice = item.TotalProductPrice,
                            Finaldiscountedamount = item.Finaldiscountedamount,
                            piece_quantity = item.piece_quantity
                        });
                    }

                    string ProductcartXML = APIHelperMethods.ConvertToXml(obj, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("PROC_API_BasketAdd", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@CustomerId", model.CustomerId);
                    sqlcmd.Parameters.Add("@SalesmanId", SaleManID);
                    sqlcmd.Parameters.Add("@ORDER_TYPE", "WISHLIST");
                    sqlcmd.Parameters.Add("@ProductList", ProductcartXML);
                    // sqlcmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Responsedetails"].ToString() == "SUCESS")
                        {
                            omodel.Order_Awaiting_id = dt.Rows[0]["temp_request_id"].ToString();
                            omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(omodel);
                        }
                        else
                        {
                            omodel.Order_Awaiting_id = dt.Rows[0]["temp_request_id"].ToString();
                            omodel.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(omodel);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult OrderDetailsCustomer(CustomerOrderDetaisInput ordInput)
        {
            Commonclass oerror = new Commonclass();
            DataSet ds = new DataSet();

            String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
            if (token == ordInput.Token)
            {
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PROC_API_ORDERDETAILSCONTACT", sqlcon);
                sqlcmd.Parameters.Add("@Customer_Id", ordInput.CustomerId);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(ds);
                sqlcon.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    CustomerOrderDetaisOutPut orddetl = new CustomerOrderDetaisOutPut();
                    List<OrderDetailsLists> ordemodel = new List<OrderDetailsLists>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Decimal OrderAmount = 0;
                        List<OrderDetailsProductList> ordemodelprod = new List<OrderDetailsProductList>();
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {


                            int i1 = 0;
                            if (Convert.ToString(ds.Tables[1].Rows[j]["Order_Id"]) == Convert.ToString(ds.Tables[0].Rows[i]["Order_Id"]))
                            {
                                ordemodelprod.Add(new OrderDetailsProductList()
                                {
                                    product_id = Convert.ToInt32(ds.Tables[1].Rows[j]["product_id"]),
                                    product_name = Convert.ToString(ds.Tables[1].Rows[j]["product_name"]),
                                    qty = Convert.ToDecimal(ds.Tables[1].Rows[j]["qty"]),
                                    unit = Convert.ToString(ds.Tables[1].Rows[j]["unit"]),
                                    price = Convert.ToDecimal(ds.Tables[1].Rows[j]["price"])
                                });

                            }
                        }
                        ordemodel.Add(new OrderDetailsLists()
                        {
                            Order_Number = Convert.ToString(ds.Tables[0].Rows[i]["Order_Number"]),
                            Order_Date = Convert.ToString(ds.Tables[0].Rows[i]["Order_Date"]) != "" ? Convert.ToDateTime(ds.Tables[0].Rows[i]["Order_Date"]).ToString("dd/MM/yyyy") : "",
                            Customer_Id = Convert.ToString(ds.Tables[0].Rows[i]["Customer_Id"]),
                            CUSTOMER_NAME = Convert.ToString(ds.Tables[0].Rows[i]["CUSTOMER_NAME"]),
                            ProductList = ordemodelprod
                        });
                    }
                    orddetl.ResponseCode = "200";
                    orddetl.Responsedetails = "success";
                    orddetl.OrderDetailsList = ordemodel;
                    return Json(orddetl);
                }
                else
                {
                    oerror.ResponseCode = "201";
                    oerror.Responsedetails = "No data found";
                    return Json(oerror);
                }
            }
            else
            {
                oerror.ResponseCode = "103";
                oerror.Responsedetails = "Token Id not match";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult CancelOrderCart(CustomerOrderCancelInput model)
        {
            CartDetailsOutPut omodel = new CartDetailsOutPut();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    string output = string.Empty;

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Prc_API_CancelOrder", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@Document_Id", model.OrderNumber);
                    sqlcmd.Parameters.Add("@Reason", model.Remarks);
                    sqlcmd.Parameters.Add("@ReturnValue", SqlDbType.Char, 500);
                    sqlcmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                    // sqlcmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    output = (string)sqlcmd.Parameters["@ReturnValue"].Value;
                    sqlcon.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Responsedetails"].ToString() == "Success")
                        {
                            oerror.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            oerror.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(oerror);
                        }
                        else
                        {
                            oerror.ResponseCode = dt.Rows[0]["ResponseCode"].ToString();
                            oerror.Responsedetails = dt.Rows[0]["Responsedetails"].ToString();
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult OrderDeliveryStatus(CustomerOrderCancelInput model)
        {
            List<OrderDeliveryDetails> omodel = new List<OrderDeliveryDetails>();

            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == model.Token)
                {
                    string output = string.Empty;
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PROC_API_DeliveryStatus", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@Order_Number", model.OrderNumber);
                    sqlcmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["MSG"].ToString() == "sucess")
                        {
                            omodel = APIHelperMethods.ToModelList<OrderDeliveryDetails>(dt);
                            return Json(omodel);
                        }
                        else
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "No Data Found";
                            return Json(oerror);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult AddEmployee(employeeAddInput model)
        {
            EmployeeAddOutPut omodel = new EmployeeAddOutPut();
            Commonclass oerror = new Commonclass();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                String JobDUrl = System.Configuration.ConfigurationSettings.AppSettings["JobDUrl"];
                if (token == model.Token)
                {
                    DateTime? Dateofbirth = null;
                    if (model.Dob != "" && model.Dob != null)
                    {
                        try
                        {
                            Dateofbirth = DateTime.ParseExact(model.Dob, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Invalid Date of Birth";
                            return Json(oerror);
                        }

                    }

                    DateTime? Dateofjoining = null;
                    if (model.doj != "" && model.doj != null)
                    {
                        try
                        {
                            Dateofjoining = DateTime.ParseExact(model.doj, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Invalid Date of Joining";
                            return Json(oerror);
                        }
                    }

                    DateTime? ValidUp = null;
                    if (model.ValidUpTo != "" && model.ValidUpTo != null)
                    {
                        try
                        {
                            ValidUp = DateTime.ParseExact(model.ValidUpTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            oerror.ResponseCode = "201";
                            oerror.Responsedetails = "Invalid Valid Up To";
                            return Json(oerror);
                        }
                    }


                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionEvac"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("prc_EmployeesImportFromExcel", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@Action", "InsertEmployeeDataFromExcel");
                    sqlcmd.Parameters.AddWithValue("@UserId", null);
                    sqlcmd.Parameters.AddWithValue("@ContactType", "EM");
                    sqlcmd.Parameters.AddWithValue("@EmployeeCode", model.EmployeeCode);
                    sqlcmd.Parameters.AddWithValue("@Salutation", model.Salutation);
                    sqlcmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                    sqlcmd.Parameters.AddWithValue("@MiddileName", model.MiddileName);
                    sqlcmd.Parameters.AddWithValue("@LastName", model.LastName);
                    sqlcmd.Parameters.AddWithValue("@Dob", Dateofbirth);
                    sqlcmd.Parameters.AddWithValue("@Gender", model.Gender);
                    sqlcmd.Parameters.AddWithValue("@doj", Dateofjoining);
                    sqlcmd.Parameters.AddWithValue("@Grade", model.Grade);
                    sqlcmd.Parameters.AddWithValue("@BloodGroup", model.BloodGroup);
                    sqlcmd.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
                    sqlcmd.Parameters.AddWithValue("@Organization", model.Organization);
                    sqlcmd.Parameters.AddWithValue("@JobResposibility", model.JobResposibility);
                    sqlcmd.Parameters.AddWithValue("@Branch", model.Branch);
                    sqlcmd.Parameters.AddWithValue("@Designation", model.Designation);
                    sqlcmd.Parameters.AddWithValue("@EmployeeType", model.EmployeeType);
                    sqlcmd.Parameters.AddWithValue("@ReportTo", model.ReportTo);
                    sqlcmd.Parameters.AddWithValue("@AddressTypeResidence", model.AddressTypeResidence);
                    sqlcmd.Parameters.AddWithValue("@Address1_Res", model.Address1_Res);

                    sqlcmd.Parameters.AddWithValue("@Address2_Res", model.Address2_Res);
                    sqlcmd.Parameters.AddWithValue("@Address3_Res", model.Address3_Res);
                    sqlcmd.Parameters.AddWithValue("@Country_Res", model.Country_Res);
                    sqlcmd.Parameters.AddWithValue("@State_res", model.State_res);
                    sqlcmd.Parameters.AddWithValue("@City_District_Res", model.City_District_Res);
                    sqlcmd.Parameters.AddWithValue("@Pin_Zip_Res", model.Pin_Zip_Res);
                    sqlcmd.Parameters.AddWithValue("@Phone_type_res", model.Phone_type_res);
                    sqlcmd.Parameters.AddWithValue("@Number_Res", model.Number_Res);
                    sqlcmd.Parameters.AddWithValue("@AddressType_off", model.AddressType_off);

                    sqlcmd.Parameters.AddWithValue("@Address1_off", model.Address1_off);

                    sqlcmd.Parameters.AddWithValue("@Address2_off", model.Address2_off);
                    sqlcmd.Parameters.AddWithValue("@Address3_off", model.Address3_off);
                    sqlcmd.Parameters.AddWithValue("@Country_off", model.Country_off);
                    sqlcmd.Parameters.AddWithValue("@State_off", model.State_off);



                    sqlcmd.Parameters.AddWithValue("@City_District_Off", model.City_District_Off);
                    sqlcmd.Parameters.AddWithValue("@Pin_Zip_Off", model.Pin_Zip_Off);
                    sqlcmd.Parameters.AddWithValue("@Phone_type_off", model.Phone_type_off);
                    sqlcmd.Parameters.AddWithValue("@Number_Off", model.Number_Off);
                    sqlcmd.Parameters.AddWithValue("@Email_Type", model.Email_Type);
                    sqlcmd.Parameters.AddWithValue("@Email_Id", model.Email_Id);
                    sqlcmd.Parameters.AddWithValue("@Relationship_1", model.Relationship_1);
                    sqlcmd.Parameters.AddWithValue("@Name_1", model.Name_1);
                    sqlcmd.Parameters.AddWithValue("@RelationShip_2", model.RelationShip_2);
                    sqlcmd.Parameters.AddWithValue("@Name_2", model.Name_2);
                    sqlcmd.Parameters.AddWithValue("@Current_Ctc", model.Current_Ctc);
                    sqlcmd.Parameters.AddWithValue("@Pan", model.Pan);
                    sqlcmd.Parameters.AddWithValue("@Aadhar", model.Aadhar);
                    sqlcmd.Parameters.AddWithValue("@Passport", model.Passport);
                    sqlcmd.Parameters.AddWithValue("@ValidUpTo", ValidUp);
                    sqlcmd.Parameters.AddWithValue("@Epic", model.Epic);
                    sqlcmd.Parameters.AddWithValue("@BankName", model.BankName);
                    sqlcmd.Parameters.AddWithValue("@Account_No", model.Account_No);
                    sqlcmd.Parameters.AddWithValue("@AccountType", model.AccountType);
                    sqlcmd.Parameters.AddWithValue("@Pf_Applicable", model.Pf_Applicable);
                    sqlcmd.Parameters.AddWithValue("@Pf_No", model.Pf_No);
                    sqlcmd.Parameters.AddWithValue("@Uan", model.Uan);
                    sqlcmd.Parameters.AddWithValue("@Esi_Applicable", model.Esi_Applicable);
                    sqlcmd.Parameters.AddWithValue("@Esi_No", model.Esi_No);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["MSG"].ToString() == "SUCCESSFULL")
                        {
                            omodel.Success = dt.Rows[0]["Success"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["MSG"].ToString();
                            omodel.HasLog = dt.Rows[0]["HasLog"].ToString();
                            omodel.EmployeeCode = dt.Rows[0]["internal_id"].ToString();
                            //using (var client = new HttpClient())
                            //{
                            //    client.DefaultRequestHeaders.Accept.Clear();
                            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            //    var response = client.GetAsync(JobDUrl).Result;
                            //    // response.Wait();
                            //    string responseString = response.Content.ReadAsStringAsync().Result;
                            //    var modelObject = response.Content.ReadAsAsync<countyryListRespose>().Result;
                            //}
                            return Json(omodel);
                        }
                        else
                        {
                            omodel.Success = dt.Rows[0]["Success"].ToString();
                            omodel.Responsedetails = dt.Rows[0]["MSG"].ToString();
                            omodel.HasLog = dt.Rows[0]["HasLog"].ToString();
                            omodel.EmployeeCode = dt.Rows[0]["internal_id"].ToString();
                            return Json(omodel);
                        }
                    }
                    else
                    {
                        oerror.ResponseCode = "201";
                        oerror.Responsedetails = "Error";
                        return Json(oerror);
                    }
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }

        [AcceptVerbs("POST")]
        public JsonResult EmployeeMasterDetails(CustomerRegistrationInput model)
        {
            EmployeeDetailsMaster omodel = new EmployeeDetailsMaster();
            List<salutation> salution = new List<salutation>();
            List<Branch> Bnch = new List<Branch>();
            List<Designation> Desi = new List<Designation>();
            List<EmployeeType> EmpType = new List<EmployeeType>();
            List<Organization> Orga = new List<Organization>();
            List<JobResponsibility> JobRespons = new List<JobResponsibility>();
            List<WorkingHour> WorkingHr = new List<WorkingHour>();
            List<LeavePolicy> LvPolicy = new List<LeavePolicy>();
            List<Department> Dept = new List<Department>();
            Commonclass oerror = new Commonclass();

            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];

                if (token == model.Token)
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionEvac"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APIMasterData", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        salution = APIHelperMethods.ToModelList<salutation>(ds.Tables[0]);
                        omodel.ResponseCode = "200";
                        omodel.Responsedetails = "Sucess";
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        Orga = APIHelperMethods.ToModelList<Organization>(ds.Tables[1]);
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        JobRespons = APIHelperMethods.ToModelList<JobResponsibility>(ds.Tables[2]);
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        Bnch = APIHelperMethods.ToModelList<Branch>(ds.Tables[3]);
                    }

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        Desi = APIHelperMethods.ToModelList<Designation>(ds.Tables[4]);
                    }
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        EmpType = APIHelperMethods.ToModelList<EmployeeType>(ds.Tables[5]);
                    }
                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        Dept = APIHelperMethods.ToModelList<Department>(ds.Tables[6]);
                    }
                    if (ds.Tables[7].Rows.Count > 0)
                    {
                        WorkingHr = APIHelperMethods.ToModelList<WorkingHour>(ds.Tables[7]);
                    }
                    if (ds.Tables[8].Rows.Count > 0)
                    {
                        LvPolicy = APIHelperMethods.ToModelList<LeavePolicy>(ds.Tables[8]);
                    }



                    omodel.salutationList = salution;
                    omodel.LeavePolicyList = LvPolicy;
                    omodel.WorkingHourList = WorkingHr;
                    omodel.DepartmentList = Dept;
                    omodel.EmployeeTypeList = EmpType;
                    omodel.DesignationList = Desi;
                    omodel.BranchList = Bnch;
                    omodel.JobResponsibilityList = JobRespons;
                    omodel.OrganizationList = Orga;
                    return Json(omodel);
                }
                else
                {
                    oerror.ResponseCode = "103";
                    oerror.Responsedetails = "Token Id not match";
                    return Json(oerror);
                }
            }
            catch
            {
                oerror.ResponseCode = "201";
                oerror.Responsedetails = "Error";
                return Json(oerror);
            }
        }
        #endregion


        #region #### Start TAB Activity  ####

        [AcceptVerbs("GET")]
        public JsonResult GetAllCustomerList(string user_id, string Token, int pageno, int rowcount)
        {

            CustomerList objomodel = new CustomerList();
            List<AllCustomer> omodel = new List<AllCustomer>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "AllCustomerAndLead");
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            objomodel.totalcount = dt.Rows.Count.ToString();
                            omodel = APIHelperMethods.ToModelList<AllCustomer>(dt);
                            objomodel.customer_list = omodel;
                            return Json(objomodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel, JsonRequestBehavior.AllowGet);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "No data found";
                        return Json(objomodel, JsonRequestBehavior.AllowGet);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel, JsonRequestBehavior.AllowGet);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel, JsonRequestBehavior.AllowGet);
            }

        }


        [AcceptVerbs("POST")]
        public JsonResult AddActivity(string user_id, string cust_id, string Token, string date, string remarks, string next_contact_date)
        {

            Commonclass objomodel = new Commonclass();

            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("sp_TabActivity", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "AddActivity");
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@cust_id", cust_id);
                    sqlcmd.Parameters.Add("@Token", Token);
                    sqlcmd.Parameters.Add("@date", date);
                    sqlcmd.Parameters.Add("@remarks", remarks);
                    sqlcmd.Parameters.Add("@next_contact_date", next_contact_date);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "Activity added successfully.")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Activity added successfully";
                            return Json(objomodel);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = dt.Rows[0][0].ToString();
                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "Invalid User name /password";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }

        [AcceptVerbs("POST")]
        public JsonResult ViewActivity(string user_id, string cust_id, string Token, String member_id)
        {

            ActivityList objomodel = new ActivityList();
            List<Activity> omodel = new List<Activity>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("sp_TabActivity", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "ViewActivity");
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@cust_id", cust_id);
                    sqlcmd.Parameters.Add("@Token", Token);
                    sqlcmd.Parameters.Add("@member_id", member_id);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<Activity>(dt);
                            objomodel.activity_list = omodel;
                            return Json(objomodel);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = dt.Rows[0][0].ToString();
                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "No data found.";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }


        [AcceptVerbs("POST")]
        public JsonResult SendSMSToLead(string user_id, string cust_id, string Token, string isSendSMS)
        {

            Commonclass objomodel = new Commonclass();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    if (isSendSMS == "true")
                    {
                        DataTable dt = new DataTable();
                        String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                        SqlCommand sqlcmd = new SqlCommand();
                        SqlConnection sqlcon = new SqlConnection(con);


                        sqlcon.Open();

                        sqlcmd = new SqlCommand("sp_TabActivity", sqlcon);
                        sqlcmd.Parameters.Add("@Action", "SendSMS");
                        sqlcmd.Parameters.Add("@user_id", user_id);
                        sqlcmd.Parameters.Add("@cust_id", cust_id);
                        sqlcmd.Parameters.Add("@isSendSMS", isSendSMS);
                        sqlcmd.Parameters.Add("@Token", Token);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(dt);
                        sqlcon.Close();


                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {

                                objomodel.ResponseCode = "200";
                                objomodel.Responsedetails = "Successfully updated send sms status.";
                                return Json(objomodel);
                            }
                            else
                            {
                                objomodel.ResponseCode = "201";
                                objomodel.Responsedetails = dt.Rows[0][0].ToString();
                                return Json(objomodel);

                            }

                        }
                        else
                        {

                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "No data found.";
                            return Json(objomodel);

                        }
                    }
                    else
                    {
                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "SMS Not Sent.";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }


        #endregion


        #region Start 21 & 22 Number API


        [AcceptVerbs("POST")]
        public JsonResult GetLeadType(string user_id, string Token)
        {

            LeadTypeList objomodel = new LeadTypeList();
            List<LeadType> omodel = new List<LeadType>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GetLeadType");
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<LeadType>(dt);
                            objomodel.lead_type_list = omodel;
                            return Json(objomodel);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "No data found";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }

        [AcceptVerbs("POST")]
        public JsonResult GetRequirement(string user_id, string Token)
        {

            requirementList objomodel = new requirementList();
            List<requirement> omodel = new List<requirement>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GetRequirement");
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<requirement>(dt);
                            objomodel.req_list = omodel;
                            return Json(objomodel);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "No data found";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }

        #endregion


        #region Start 26 & 27 Number API


        [AcceptVerbs("GET")]
        public JsonResult GetProductGroupList(string user_id, string Token, string pageno, string rowcount, string groupid)
        {

            ProductAndGroupList objomodel = new ProductAndGroupList();
            List<ProductGroupList> omodel = new List<ProductGroupList>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "ProductGroupDetails");
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);
                    sqlcmd.Parameters.Add("@GroupId", groupid);
                    sqlcmd.Parameters.Add("@Weburl", producturl);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<ProductGroupList>(dt);
                            objomodel.prod_grp_list = omodel;
                            return Json(objomodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel, JsonRequestBehavior.AllowGet);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "No data found";
                        return Json(objomodel, JsonRequestBehavior.AllowGet);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel, JsonRequestBehavior.AllowGet);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel, JsonRequestBehavior.AllowGet);
            }

        }

        [AcceptVerbs("POST")]
        public JsonResult GetProductGroupSearch(string user_id, string Token, string item_name, string pageno, string rowcount, string group_id)
        {

            ProductAndGroupList objomodel = new ProductAndGroupList();
            List<ProductGroupList> omodel = new List<ProductGroupList>();
            try
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);


                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiTabDetails", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "ProductGroupDetailsByName");
                    sqlcmd.Parameters.Add("@user_id", user_id);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);
                    sqlcmd.Parameters.Add("@GroupId", group_id);
                    sqlcmd.Parameters.Add("@Weburl", producturl);
                    sqlcmd.Parameters.Add("@ProductGroup_Name", item_name);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();


                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<ProductGroupList>(dt);
                            objomodel.prod_grp_list = omodel;
                            return Json(objomodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User name /password";

                            return Json(objomodel);

                        }

                    }
                    else
                    {

                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "No data found";
                        return Json(objomodel);

                    }

                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel);

                }


            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel);
            }

        }

        #endregion

        #region Start 29 Number API
        [AcceptVerbs("GET")]
        public JsonResult GetUserHierarchyList(string userId, string Token, string pageno, string rowcount)
        {
            UserHierarchyOut objomodel = new UserHierarchyOut();
            List<UserHierarchy> omodel = new List<UserHierarchy>();
            try
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                if (token == Token)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);

                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_ApiUserHierarchy", sqlcon);
                    sqlcmd.Parameters.Add("@USER_ID", userId);
                    sqlcmd.Parameters.Add("@PageNo", pageno);
                    sqlcmd.Parameters.Add("@Pagerows", rowcount);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            objomodel.ResponseCode = "200";
                            objomodel.Responsedetails = "Success";
                            omodel = APIHelperMethods.ToModelList<UserHierarchy>(dt);
                            objomodel.total_count = dt.Rows.Count.ToString();
                            objomodel.team_list = omodel;
                            return Json(objomodel, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objomodel.ResponseCode = "201";
                            objomodel.Responsedetails = "Invalid User";
                            return Json(objomodel, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        objomodel.ResponseCode = "201";
                        objomodel.Responsedetails = "No data found";
                        return Json(objomodel, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    objomodel.ResponseCode = "103";
                    objomodel.Responsedetails = "Token Id not match";
                    return Json(objomodel, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                objomodel.ResponseCode = "103";
                objomodel.Responsedetails = "Error occured";
                return Json(objomodel, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}