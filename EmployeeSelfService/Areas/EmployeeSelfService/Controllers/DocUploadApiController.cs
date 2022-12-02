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
    public class DocUploadApiController : ApiController
    {

        public HttpResponseMessage SaveResume()
        {
            var httpRequest = HttpContext.Current.Request;
            var UserID = Convert.ToString(HttpContext.Current.Request.Form["UserID"]);
            var fullPath = "";
            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    string filename = httpRequest.Files[i].FileName;
                    if (filename != "")
                    {
                        int lastSlash = filename.LastIndexOf("\\");
                        string trailingPath = filename.Substring(lastSlash + 1);
                        fullPath = System.Web.Hosting.HostingEnvironment.MapPath("//CommonFolder//MyResume") + "\\" + trailingPath;
                        httpRequest.Files[i].SaveAs(fullPath);
                    }
                }


            }
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
                    sqlcmd = new SqlCommand("PRC_SAVEDOCUMENTEMP", sqlcon);
                    //sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@USER_ID", UserID);
                    sqlcmd.Parameters.Add("@DOCPATH", fullPath);
                    sqlcmd.Parameters.Add("@DOCTYPE", "Resume");
                    sqlcmd.Parameters.Add("@ACTION", "RESUME");

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully File Uploaded.";

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

            //var message = Request.CreateResponse(HttpStatusCode.OK, "");
            //return message;
        }
        public HttpResponseMessage SaveLeaveDoc()
        {
            var httpRequest = HttpContext.Current.Request;
            var UserID = Convert.ToString(HttpContext.Current.Request.Form["UserID"]);
            var fullPath="";
            if(httpRequest.Files.Count>0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    string filename = httpRequest.Files[i].FileName;
                    if (filename != "")
                    {
                        int lastSlash = filename.LastIndexOf("\\");
                        string trailingPath = filename.Substring(lastSlash + 1);
                        fullPath = System.Web.Hosting.HostingEnvironment.MapPath("//CommonFolder//LeaveDoc") + "\\" + trailingPath;
                        httpRequest.Files[i].SaveAs(fullPath);
                    }
                }
                
                
            }
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
                    String con = Convert.ToString(APIConnction.ApiConnction);;
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_SAVEDOCUMENTEMP", sqlcon);
                    //sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@USER_ID", UserID);
                    sqlcmd.Parameters.Add("@DOCPATH", fullPath);
                    sqlcmd.Parameters.Add("@DOCTYPE", "Leave");
               

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully File Uploaded.";

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

            //var message = Request.CreateResponse(HttpStatusCode.OK, "");
            //return message;
        }
        public HttpResponseMessage SaveHealthDoc()
        {
            var httpRequest = HttpContext.Current.Request;
            var UserID = Convert.ToString(HttpContext.Current.Request.Form["UserID"]);
            var fullPath = "";
            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    string filename = httpRequest.Files[i].FileName;
                    if (filename != "")
                    {
                        int lastSlash = filename.LastIndexOf("\\");
                        string trailingPath = filename.Substring(lastSlash + 1);
                        fullPath = System.Web.Hosting.HostingEnvironment.MapPath("//CommonFolder//LeaveDoc") + "\\" + trailingPath;
                        httpRequest.Files[i].SaveAs(fullPath);
                    }
                }


            }
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
                    sqlcmd = new SqlCommand("PRC_SAVEDOCUMENTEMP", sqlcon);
                    //sqlcmd.Parameters.Add("@Action", "EMP_LEAVE");
                    sqlcmd.Parameters.Add("@USER_ID", UserID);
                    sqlcmd.Parameters.Add("@DOCPATH", fullPath);
                    sqlcmd.Parameters.Add("@DOCTYPE", "Health");


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    //DataTable dts = dt.Tables[0];

                    //oview.leaveinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully File Uploaded.";

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
        public HttpResponseMessage SaveTravelDoc()
        {
            var httpRequest = HttpContext.Current.Request;


            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    string filename = httpRequest.Files[i].FileName;
                    if (filename != "")
                    {
                        int lastSlash = filename.LastIndexOf("\\");
                        string trailingPath = filename.Substring(lastSlash + 1);
                        string fullPath = System.Web.Hosting.HostingEnvironment.MapPath("//CommonFolder//TravelDoc") + "\\" + trailingPath;
                        httpRequest.Files[i].SaveAs(fullPath);
                    }
                }

            }


            var message = Request.CreateResponse(HttpStatusCode.OK, "");
            return message;
        }
        public HttpResponseMessage SaveCompanyDoc() 
        {
            var httpRequest = HttpContext.Current.Request;


            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    string filename = httpRequest.Files[i].FileName;
                    if (filename != "")
                    {
                        int lastSlash = filename.LastIndexOf("\\");
                        string trailingPath = filename.Substring(lastSlash + 1);
                        string fullPath = System.Web.Hosting.HostingEnvironment.MapPath("//CommonFolder//CompanyDoc") + "\\" + trailingPath;
                        httpRequest.Files[i].SaveAs(fullPath);
                    }
                }

            }


            var message = Request.CreateResponse(HttpStatusCode.OK, "");
            return message;
        }
    }

    public class Inputform
    {
        public HttpPostedFileBase Leavedoc { get; set; }
        public HttpPostedFileBase Healthdoc  { get; set; }
        public HttpPostedFileBase Traveldoc  { get; set; }
        public HttpPostedFileBase Companydoc  { get; set; }

    }
}
