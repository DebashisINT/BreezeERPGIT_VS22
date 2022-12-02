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
    public class TopSalesBranchController : Controller
    {
        ///http://localhost:1748/TopSalesBranch/TopsalesBrnchwise
        [AcceptVerbs("POST")]
        public JsonResult TopsalesBrnchwise(SalesTopbranchwiseInput model)
        {
            List<Topsalesbranch> lomodel = new List<Topsalesbranch>();
            Topsalesbranchoutput omodel = new Topsalesbranchoutput();

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

                    sqlcmd = new SqlCommand("SP_Reports_BranchwiseSales", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@month", model.month);
                    sqlcmd.Parameters.Add("@year", model.year);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);

                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            lomodel = APIHelperMethods.ToModelList<Topsalesbranch>(dt);
                            omodel.data = lomodel;
                            return Json(lomodel);
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


            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Dalhousie",
            //    amount = 1642.85

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Behala",
            //    amount = 1132.49

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Beckbagan",
            //    amount = 974.55

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Rajdanga",
            //    amount = 948.28

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Technocity",
            //    amount = 819.28

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Tonk Road",
            //    amount = 760.65

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Santragachi",
            //    amount = 751.32

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Kankurgachi",
            //    amount = 69163

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Bhubeneswer",
            //    amount = 670.27

            //});
            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Bagnan",
            //    amount = 649.21

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Uttarpara",
            //    amount = 607.13

            //});

            //lomodel.Add(new Topsalesbranch()
            //{
            //    title = "Barasat",
            //    amount = 580.49

            //});
            //omodel.unit = "Rs.(In Lakh)";
            //omodel.month = "February";
            //omodel.year="2017";
            //omodel.data = lomodel;
            ////   return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
            //var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
            return Json(lomodel);
            //return message;
        }

    }
}
