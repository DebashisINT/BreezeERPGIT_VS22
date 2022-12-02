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
    public class BioMetricApplicationController : ApiController
    {
        [HttpPost] 
        public HttpResponseMessage getAttReport(attModelInput model)
        {

            attModelOutput oview = new attModelOutput();
            List<attReportData> obj = new List<attReportData>();
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
                    DataTable dt = new DataTable();
                    String con = Convert.ToString(APIConnction.ApiConnction);
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_BIOMETRIC", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_ATTREPORT");
                    sqlcmd.Parameters.Add("@YYMM", model.YYMM);
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null)
                    {
                        obj = (from DataRow dr in dt.Rows
                               select new attReportData()
                               {
                                   EMP_CODE = Convert.ToString(dr["EMP_CODE"]),
                                   DATE = Convert.ToString(dr["DATE"]),
                                   IN_TIME = Convert.ToString(dr["IN_TIME"]),
                                   OUT_TIME = Convert.ToString(dr["OUT_TIME"]),
                                   DIFF = Convert.ToString(dr["DIFF"]),
                                   TIMESHEET_HOUR = Convert.ToString(dr["TIMESHEET_HOUR"]),
                                   SHOW_ACTION = Convert.ToString(dr["SHOW_ACTION"]),
                               }).ToList();
                    }
						
                    oview.attinfo = obj;
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
        public HttpResponseMessage ApplyBiometricIssue(biometricApplyInputs model)
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
                    sqlcmd = new SqlCommand("PRC_BIOMETRIC", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "INSERT_BIOMETRIC_ISSUE");
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@REQ_INTIME", model.REQ_INTIME);
                    sqlcmd.Parameters.Add("@REQ_OUTTIME", model.REQ_OUTTIME);
                    sqlcmd.Parameters.Add("@APPLIED_FOR_DATE", model.APPLIED_FOR_DATE);
                    sqlcmd.Parameters.Add("@BioReason", model.BioReason);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                     
                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully Biometric Applied.";

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
        public HttpResponseMessage getBiometricIssueList(BMInputs model)
        {

            bioOutput oview = new bioOutput();
            List<bioOutputData> obj = new List<bioOutputData>();
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
                    DataTable dt = new DataTable();
                    String con = Convert.ToString(APIConnction.ApiConnction);
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_BIOMETRIC", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_BIOMETRIC_ISSUE_LIST");
                    //Mantis Issue 24411
                    sqlcmd.Parameters.Add("@USER_ID", model.ID);
                    //End Mantis Issue rev 24411
                    sqlcmd.Parameters.Add("@EMP_CODE", model.EMP_CODE);
                    sqlcmd.Parameters.Add("@ISAPPROVED", model.STATUS);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null)
                    {
                        obj = (from DataRow dr in dt.Rows
                               select new bioOutputData()
                               {
                                   user_name = Convert.ToString(dr["user_name"]),
                                   INTIME = Convert.ToString(dr["INTIME"]),
                                   OUTTIME = Convert.ToString(dr["OUTTIME"]),
                                   ID = Convert.ToString(dr["ID"]),
                                   EMPCODE = Convert.ToString(dr["EMPCODE"]),
                                   APPLIED_DATE = Convert.ToString(dr["APPLIED_DATE"]),
                                   ISAPPROVED = Convert.ToString(dr["ISAPPROVED"]),
                                   BioReason = Convert.ToString(dr["BioReason"]),
                                   APPLIED_FOR_DATE = Convert.ToString(dr["APPLIED_FOR_DATE"]),
                               }).ToList();
                    }
          
                    oview.attinfo = obj;
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
        public HttpResponseMessage ApproveBiometricReq(appBiometricReqClass model)
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
                    sqlcmd = new SqlCommand("PRC_BIOMETRIC", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "APPROVE_BIOMETRIC_REQ");
                    sqlcmd.Parameters.Add("@BIOMETRIC_IDS", model.BIOMETRIC_IDS);

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


    
    }
}
