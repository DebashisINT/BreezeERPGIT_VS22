using BreezeERPAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BreezeERPAPI.Controllers
{
    public class MopAnalysisController : Controller
    {
        [AcceptVerbs("POST")]
        public JsonResult Analysis(MopAnalysisInput model)
        {
            List<MobAnalysis> lomodel = new List<MobAnalysis>();
            MopAnalysisoutput omodel = new MopAnalysisoutput();
            try
            {
                String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];

                if (model.token == tokenmatch)
                {
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();

                    sqlcmd = new SqlCommand("Sp_API_MOPAnalysis", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@month", model.month);
                    sqlcmd.Parameters.Add("@year", model.year);
                    sqlcmd.Parameters.Add("@branch", model.branch);

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {

                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            lomodel = APIHelperMethods.ToModelList<MobAnalysis>(dt);
                            omodel.data = lomodel;

                            return Json(omodel);

                        }
                        else
                        {

                        }

                    }

                    else
                    {
                        omodel.ResponseCode = "201";
                        omodel.Responsedetails = "No Data Found";
                        return Json(omodel);
                    }
                }
                else
                {
                    omodel.ResponseCode = "203";
                    omodel.Responsedetails = "Token Id Mismatch";
                    return Json(omodel);
                }
            }
            catch
            {

            }


            //lomodel.Add(new MobAnalysis()
            //{
            //    title = "Sale",
            //    cash = 2181951.00,
            //    finance = 0.00,
            //    credit = 46746.00,
            //    total = 2228697.00
            //});


            //lomodel.Add(new MobAnalysis()
            //{
            //    title = "Sale Order",
            //    cash = 669.00,
            //    finance = 0.00,
            //    credit = 0.00,
            //    total = 669.00
            //});


            //lomodel.Add(new MobAnalysis()
            //{
            //    title = "Sale Return",
            //    cash = -158.00,
            //    finance = -3698.00,
            //    credit = 0.00,
            //    total = -3856.00
            //});


            //lomodel.Add(new MobAnalysis()
            //{
            //    title = "Grand Total",
            //    cash = 2182462.00,
            //    finance = -3698.00,
            //    credit = 46746.00,
            //    total = 22222100.00
            //});


            //omodel.unit="Rs.(In Lakh)";
            //omodel.branch = "Dalhousie";
            //omodel.month = "February";
            //omodel.year = "2017";
            //omodel.data = lomodel;

            //return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);

            return Json(lomodel);

        }
    }

}
