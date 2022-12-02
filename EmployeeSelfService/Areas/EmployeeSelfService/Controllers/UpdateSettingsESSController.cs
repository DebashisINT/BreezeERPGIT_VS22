using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Data;
using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System.Data.SqlClient;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class UpdateSettingsESSController : ApiController
    {
        //
        // GET: /EmployeeSelfService/UpdateSettingsESS/

        //[HttpPost]
        public HttpResponseMessage AddUpdateSettingsEss(ESSInputs model)
        {

            SettingEssOutput oview = new SettingEssOutput();

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
                    sqlcmd = new SqlCommand("PRC_EditDeleteDays", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "INSERT_EDIT_DELETE_DAYS");
                    sqlcmd.Parameters.Add("@ID", model.ID);
                    sqlcmd.Parameters.Add("@delete_days", model.delete_days);
                    sqlcmd.Parameters.Add("@edit_days", model.edit_days);

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

    }
}
