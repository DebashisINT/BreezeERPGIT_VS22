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
    public class STBServiceController : Controller
    {
        [AcceptVerbs("POST")]
        public JsonResult ServiceSummary(ServiceDetailsInput model)
        {
            IDictionary<string, int> number = new Dictionary<string, int>();

            STBServiceSumOutPut omodel = new STBServiceSumOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["SRVConnectionString"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_API_STBSERVICE", sqlcon);
            sqlcmd.Parameters.Add("@Action", "SRVServiceSummary");
            sqlcmd.Parameters.Add("@fromdate", model.from_date);
            sqlcmd.Parameters.Add("@todate", model.to_date);
            sqlcmd.Parameters.Add("@EntityCode", model.entity_code);
            sqlcmd.Parameters.Add("@username", model.login_id);
            sqlcmd.Parameters.Add("@password", model.password);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["status"].ToString() == "401")
                {
                    omodel.error = "401";
                    omodel.message = "Invalid User Credentials";
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        number.Add(Convert.ToString(item["MONTH_NAME"]), Convert.ToInt32(item["COU"]));
                        // omodel.data.Add(Convert.ToString(item[""]), Convert.ToInt32(item[""]));
                    }
                    omodel.error = "200";
                    omodel.message = "Success";
                    omodel.data = number;
                }
            }
            else
            {
                omodel.error = "204";
                omodel.message = "no data found.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult ServiceDetails(ServiceDetailsInput model)
        {
            ServiceDetailsOutPut omodel = new ServiceDetailsOutPut();
            List<ServiceDetails> list = new List<ServiceDetails>();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["SRVConnectionString"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_API_STBSERVICE", sqlcon);
            sqlcmd.Parameters.Add("@Action", "SRVServiceDetails");
            sqlcmd.Parameters.Add("@fromdate", model.from_date);
            sqlcmd.Parameters.Add("@todate", model.to_date);
            sqlcmd.Parameters.Add("@EntityCode", model.entity_code);
            sqlcmd.Parameters.Add("@username", model.login_id);
            sqlcmd.Parameters.Add("@password", model.password);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["status"].ToString() == "401")
                {
                    omodel.error = "401";
                    omodel.message = "Invalid User Credentials";
                }
                else
                {
                    omodel.error = "200";
                    omodel.message = "Success";
                    list = APIHelperMethods.ToModelList<ServiceDetails>(dt);
                    omodel.data = list;
                }
            }
            else
            {
                omodel.error = "204";
                omodel.message = "no data found.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult ProcurementSummary(ServiceDetailsInput model)
        {
            IDictionary<string, int> number = new Dictionary<string, int>();
            STBServiceSumOutPut omodel = new STBServiceSumOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["STBConnectionString"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_API_STBSERVICE", sqlcon);
            sqlcmd.Parameters.Add("@Action", "STBProcurementSummary");
            sqlcmd.Parameters.Add("@fromdate", model.from_date);
            sqlcmd.Parameters.Add("@todate", model.to_date);
            sqlcmd.Parameters.Add("@EntityCode", model.entity_code);
            sqlcmd.Parameters.Add("@username", model.login_id);
            sqlcmd.Parameters.Add("@password", model.password);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["status"].ToString() == "401")
                {
                    omodel.error = "401";
                    omodel.message = "Invalid User Credentials";
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        number.Add(Convert.ToString(item["MONTH_NAME"]), Convert.ToInt32(item["COU"]));
                        // omodel.data.Add(Convert.ToString(item[""]), Convert.ToInt32(item[""]));
                    }
                    omodel.error = "200";
                    omodel.message = "Success";
                    omodel.data = number;
                }
            }
            else
            {
                omodel.error = "204";
                omodel.message = "no data found.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult ProcurementDetails(ServiceDetailsInput model)
        {
            STBProcurementDetailsOutPut omodel = new STBProcurementDetailsOutPut();
            List<ProcurementDetails> list = new List<ProcurementDetails>();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["STBConnectionString"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_API_STBSERVICE", sqlcon);
            sqlcmd.Parameters.Add("@Action", "STBProcurementDetails");
            sqlcmd.Parameters.Add("@fromdate", model.from_date);
            sqlcmd.Parameters.Add("@todate", model.to_date);
            sqlcmd.Parameters.Add("@EntityCode", model.entity_code);
            sqlcmd.Parameters.Add("@username", model.login_id);
            sqlcmd.Parameters.Add("@password", model.password);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["status"].ToString() == "401")
                {
                    omodel.error = "401";
                    omodel.message = "Invalid User Credentials";
                }
                else
                {
                    omodel.error = "200";
                    omodel.message = "Success";
                    list = APIHelperMethods.ToModelList<ProcurementDetails>(dt);
                    omodel.data = list;
                }
            }
            else
            {
                omodel.error = "204";
                omodel.message = "no data found.";
            }
            var message = Json(omodel);
            return message;
        }
    }
}