using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class BusinessMeetingAPIController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage ApplyBM(BMInputs model)
        {

            BMApplyOutput oview = new BMApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ESSBUSINESS_TRAVEL", sqlcon);
                    sqlcmd.Parameters.Add("@ACTION", "APPLY_BM");
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@FROMDATE", model.FROMDATE);
                    sqlcmd.Parameters.Add("@TODATE", model.TODATE);
                    sqlcmd.Parameters.Add("@VISITIN", model.VISITIN);
                    sqlcmd.Parameters.Add("@CLIENT_DETAILS", model.CLIENT_DETAILS);
                    sqlcmd.Parameters.Add("@AGENDA", model.AGENDA);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage GetBMLIST(BMInputs model)
        {
            List<BMOutput> _msg = new List<BMOutput>();
            try
            {
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_ESSBUSINESS_TRAVEL", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GET_BMLIST");
                sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                sqlcmd.Parameters.Add("@STATUS", model.STATUS);
                sqlcmd.Parameters.Add("@MM", model.MM);
                sqlcmd.Parameters.Add("@YYYY", model.YYYY);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new BMOutput()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                FROMDATE = Convert.ToString(dr["FROMDATE"]),
                                TODATE = Convert.ToString(dr["TODATE"]),
                                VISITIN = Convert.ToString(dr["VISIT_IN"]),
                                CLIENT_DETAILS = Convert.ToString(dr["CLIENT_DETAILS"]),
                                AGENDA = Convert.ToString(dr["AGENDA"]),
                                EMPCODE = Convert.ToString(dr["EMPCODE"]),
                                STATUS = Convert.ToString(dr["STATUS"]),
                                CREATED_DATE = Convert.ToString(dr["CREATED_DATE"]),
                                MODIFIED_DATE = Convert.ToString(dr["MODIFIED_DATE"]),
                                //rev Pratik
                                isDelete = Convert.ToString(dr["isDelete"]),
                                isEdit = Convert.ToString(dr["isEdit"])
                                //End of Rev Pratik
                            }).ToList();
                }
            }
									
            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }

        //rev Pratik

        [HttpPost]
        public HttpResponseMessage DeleteBMeeting(DeleteReq model)
        {

            RMApplyOutput oview = new RMApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ESSDeleteReq", sqlcon);
                    sqlcmd.Parameters.Add("@ACTION", "DELETE_BM");
                    sqlcmd.Parameters.Add("@ID", model.ID);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully deleted.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage ApplyBMEdit(BMInputs model)
        {

            BMApplyOutput oview = new BMApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ESSBUSINESS_TRAVEL", sqlcon);
                    sqlcmd.Parameters.Add("@ACTION", "EDIT_BM");
                    sqlcmd.Parameters.Add("@ID", model.ID);
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@FROMDATE", model.FROMDATE);
                    sqlcmd.Parameters.Add("@TODATE", model.TODATE);
                    sqlcmd.Parameters.Add("@VISITIN", model.VISITIN);
                    sqlcmd.Parameters.Add("@CLIENT_DETAILS", model.CLIENT_DETAILS);
                    sqlcmd.Parameters.Add("@AGENDA", model.AGENDA);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        //End of Rev Pratik

        [HttpPost]
        public HttpResponseMessage ApproveBMReq(appBMReqClass model)
        {

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ESSBUSINESS_TRAVEL", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "APPROVE_BM");
                    sqlcmd.Parameters.Add("@BM_IDS", model.BM_IDS);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Approved";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage RejectBMReq(appBMReqClass model)
        { 

            leaveApplyOutput oview = new leaveApplyOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ESSBUSINESS_TRAVEL", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "REJECT_BM");
                    sqlcmd.Parameters.Add("@BM_IDS", model.BM_IDS);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Rejected";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }
        }


        [HttpPost]
        public HttpResponseMessage GetBMLISTFORAPP(BMInputs model) 
        {
            List<BMOutput> _msg = new List<BMOutput>();
            try
            {
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_ESSBUSINESS_TRAVEL", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GET_BMLISTFORAPPROVAL");
                //Mantis Issue 24411
                sqlcmd.Parameters.Add("@USER_ID", model.ID);
                //End of Mantis Issue 24411
                sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                sqlcmd.Parameters.Add("@STATUS", model.STATUS);
      

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new BMOutput()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                FROMDATE = Convert.ToString(dr["FROMDATE"]),
                                TODATE = Convert.ToString(dr["TODATE"]),
                                VISITIN = Convert.ToString(dr["VISIT_IN"]),
                                CLIENT_DETAILS = Convert.ToString(dr["CLIENT_DETAILS"]),
                                AGENDA = Convert.ToString(dr["AGENDA"]),
                                EMPCODE = Convert.ToString(dr["EMPCODE"]),
                                STATUS = Convert.ToString(dr["STATUS"]),
                                CREATED_DATE = Convert.ToString(dr["CREATED_DATE"]),
                                MODIFIED_DATE = Convert.ToString(dr["MODIFIED_DATE"]),
                                USERNAME = Convert.ToString(dr["USERNAME"])
                            }).ToList();
                }
            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }


        [HttpPost]
        public HttpResponseMessage GetALLUSERS(UserInfoModelInput model)
        {
            List<userOutput> _msg = new List<userOutput>();
            //rev Pratik
           // string USER_ID = Convert.ToString(HttpContext.Current.Session["USER_ID"]).Trim();
            //string USER_ID = Session["userid"].ToString();
            //End of rev Pratik
            try
            {
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_ESSEMPLYEES", sqlcon);
                //Mantis Issue 24411
                sqlcmd.Parameters.Add("@ACTION", "GET_EMP_HIERARCHY");
                sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                //End of Mantis Issue 24411
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new userOutput()
                            {
                                user_id = Convert.ToString(dr["user_id"]),
                                user_contactId = Convert.ToString(dr["user_contactId"]),
                                user_name = Convert.ToString(dr["user_name"])
                            }).ToList();
                }
            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }
    }


}
