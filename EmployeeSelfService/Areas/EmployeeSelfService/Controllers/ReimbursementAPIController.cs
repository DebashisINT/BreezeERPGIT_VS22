using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class ReimbursementAPIController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage ApplyRM(RMInputs model)
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
                    sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                    sqlcmd.Parameters.Add("@ACTION", "APPLY_RM");
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@RIMDATE", model.RIMDATE);
                    sqlcmd.Parameters.Add("@RIMTODATE", model.RIMTODATE);
                    sqlcmd.Parameters.Add("@RIMTYPE", model.RIMTYPE);
                    sqlcmd.Parameters.Add("@RIMCAT", model.RIMCAT);
                    sqlcmd.Parameters.Add("@FARE_AMT", model.FARE_AMT);
                    sqlcmd.Parameters.Add("@REFRESH_AMT", model.REFRESH_AMT);
                    sqlcmd.Parameters.Add("@TOTAL_AMT", model.TOTAL_AMT);
                    sqlcmd.Parameters.Add("@REMARKS", model.REMARKS);

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
        public HttpResponseMessage GetRMLIST(BMInputs model)
        {
            List<RMOutput> _msg = new List<RMOutput>();
            try
            {
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GET_RMLIST");
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
                            select new RMOutput()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                RIMDATE = Convert.ToString(dr["RIMDATE"]),
                                RIMTYPE = Convert.ToString(dr["RIMTYPE"]),
                                RIMCAT = Convert.ToString(dr["RIMCAT"]),
                                FARE_AMT = Convert.ToString(dr["FARE_AMT"]),
                                REFRESH_AMT = Convert.ToString(dr["REFRESH_AMT"]),
                                TOTAL_AMT = Convert.ToString(dr["TOTAL_AMT"]),
                                REMARKS = Convert.ToString(dr["REMARKS"]),
                                EMPCODE = Convert.ToString(dr["EMPCODE"]),
                                STATUS = Convert.ToString(dr["STATUS"]),
                                MODIFIED_DATE = Convert.ToString(dr["MODIFIED_DATE"]),
                                CREATED_DATE = Convert.ToString(dr["CREATED_DATE"]),
                                RIMTODATE = Convert.ToString(dr["RIMTODATE"]),
                                //rev Pratik
                                isDelete = Convert.ToString(dr["isDelete"]),
                                isEdit = Convert.ToString(dr["isEdit"]),
                                //End of rev Pratik
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
        public HttpResponseMessage DeleteEmpReimbursement(DeleteReq model)
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
                    sqlcmd.Parameters.Add("@ACTION", "DELETE_ERMB");
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
        public HttpResponseMessage ApplyRMEdit(RMInputs model)
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
                    sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                    sqlcmd.Parameters.Add("@ACTION", "EDIT_RM");
                    sqlcmd.Parameters.Add("@ID", model.ID);
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@RIMDATE", model.RIMDATE);
                    sqlcmd.Parameters.Add("@RIMTODATE", model.RIMTODATE);
                    sqlcmd.Parameters.Add("@RIMTYPE", model.RIMTYPE);
                    sqlcmd.Parameters.Add("@RIMCAT", model.RIMCAT);
                    sqlcmd.Parameters.Add("@FARE_AMT", model.FARE_AMT);
                    sqlcmd.Parameters.Add("@REFRESH_AMT", model.REFRESH_AMT);
                    sqlcmd.Parameters.Add("@TOTAL_AMT", model.TOTAL_AMT);
                    sqlcmd.Parameters.Add("@REMARKS", model.REMARKS);

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
        public HttpResponseMessage GetRMCAT()
        {
            List<RMCATOUTPUT> _msg = new List<RMCATOUTPUT>();
            try
            {
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GET_RMCATEGORY");

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new RMCATOUTPUT()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                CATNAME = Convert.ToString(dr["CATNAME"]),
                                
                            }).ToList();
                }
            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }


        [HttpPost]
        public HttpResponseMessage GetRMTYPES() 
        {
            List<RMTYPEOUTPUT> _msg = new List<RMTYPEOUTPUT>();
            try
            {
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GET_RMTYPES");

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new RMTYPEOUTPUT()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                TYPENAME = Convert.ToString(dr["TYPENAME"]),

                            }).ToList();
                }
            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }

        [HttpPost]
        public HttpResponseMessage ApproveRMReq(appRMReqClass model)
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
                    sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "APPROVE_RM");
                    sqlcmd.Parameters.Add("@RM_IDS", model.RM_IDS);

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
        public HttpResponseMessage RejectRMReq(appRMReqClass model)
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
                    sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "REJECT_RM");
                    sqlcmd.Parameters.Add("@RM_IDS", model.RM_IDS);

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
        public HttpResponseMessage GetRMLISTFORAPP(BMInputs model)
        {
            RMApplyOutput oview = new RMApplyOutput();
            List<RMOutput> _msg = new List<RMOutput>();
            try
            { 
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_ESSREIMBURSEMENT", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GET_RM_APPLIACTIONS");
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
                            select new RMOutput()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                RIMDATE = Convert.ToString(dr["RIMDATE"]),
                                RIMTYPE = Convert.ToString(dr["RIMTYPE"]),
                                RIMCAT = Convert.ToString(dr["RIMCAT"]),
                                FARE_AMT = Convert.ToString(dr["FARE_AMT"]),
                                REFRESH_AMT = Convert.ToString(dr["REFRESH_AMT"]),
                                TOTAL_AMT = Convert.ToString(dr["TOTAL_AMT"]),
                                REMARKS = Convert.ToString(dr["REMARKS"]),
                                EMPCODE = Convert.ToString(dr["EMPCODE"]),
                                STATUS = Convert.ToString(dr["STATUS"]),
                                MODIFIED_DATE = Convert.ToString(dr["MODIFIED_DATE"]),
                                CREATED_DATE = Convert.ToString(dr["CREATED_DATE"]),
                                RIMTODATE = Convert.ToString(dr["RIMTODATE"]),
                                username = Convert.ToString(dr["user_name"])
                            }).ToList();
                }
                oview.Rminfo = _msg;
            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }

        [HttpPost]
        public HttpResponseMessage GetcHART(leaveTypeModel model) 
        {
            List<AttchartOutput> _msg = new List<AttchartOutput>();
            try
            {
                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("proll_SixMonthsAttendance", sqlcon);
                
                sqlcmd.Parameters.Add("@EMPCODE", model.EMPCODE);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new AttchartOutput()
                            {
                                YYMM = Convert.ToString(dr["YYMM"]),
                                Emp_status = Convert.ToString(dr["Emp_status"]),
                                NoOfPresent = Convert.ToString(dr["NoOfPresent"])
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
