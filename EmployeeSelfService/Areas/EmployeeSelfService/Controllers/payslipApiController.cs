using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class payslipApiController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetFile(string year, string month, string user)
        {
            if (String.IsNullOrEmpty(user))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            string fileName;
            string localFilePath;
            int fileSize;
            string BaseUrl = "http://3.7.30.86:85/CommonFolder/PaySlip";
            localFilePath = BaseUrl + "/" + year + "/" + month + "/" + user + ".pdf";
            return Request.CreateResponse(HttpStatusCode.OK,localFilePath);
        }


        [HttpPost]

        public HttpResponseMessage PayslipPass(PayslipPassInput model)
        {

            PayslipPassOutput oview = new PayslipPassOutput();

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
                    sqlcmd = new SqlCommand("prc_GetPayslipData", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GetPassword");
                    sqlcmd.Parameters.Add("@EmployeeCode", model.EmployeeCode);
                    sqlcmd.Parameters.Add("@YYMM", model.YYMM);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    userInformation obj = new userInformation();
                    oview.Palslip_Password = Convert.ToString(dts.Rows[0]["Palslip_Password"]);
                    oview.status = "200";
                    oview.message = "Successfull Retrival of Pass.";

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
