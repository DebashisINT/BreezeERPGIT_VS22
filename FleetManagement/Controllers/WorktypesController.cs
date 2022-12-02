using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FleetManagement.Controllers
{
    public class WorktypesController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Types(WorktypesInput model)
        {
            Worktypesoutput odata = new Worktypesoutput();
            try
            {
                List<worktypes> oview = new List<worktypes>();


                string token = string.Empty;
                string versionname = string.Empty;
                System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
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
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 
                   // String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();


                    sqlcmd = new SqlCommand("proc_fleet_MasterLists", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "WorkTypes");
                    sqlcmd.Parameters.Add("@User_id", model.user_id);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        oview = APIHelperMethods.ToModelList<worktypes>(dt);
                        odata.status = "200";
                        odata.message = "Success";
                        odata.worktype_list = oview;
                    }

                    var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                    return message;
                }
                else
                {
                    odata.status = "205";
                    odata.message = "Token Id does not matched.";
                    var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                    return message;

                }
            }

            catch (Exception ex)
            {


                odata.status = "209";

                odata.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                return message;
            }
        }
    }
}
