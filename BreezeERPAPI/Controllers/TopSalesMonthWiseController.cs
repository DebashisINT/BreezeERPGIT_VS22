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
    public class TopSalesMonthWiseController : Controller
    {
        //http://localhost:1748/TopSalesMonthWise/Monthwise

        [AcceptVerbs("POST")]
        public JsonResult Monthwise(SalesTopMonthwiseInput model)
        {
            List<Topsalesmonthbranch> lomodel = new List<Topsalesmonthbranch>();
            topbranchbonthsalesoutput omodel = new topbranchbonthsalesoutput();

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

                    sqlcmd = new SqlCommand("SP_Reports_MonthwiseSales", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@branchid", model.branch);
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
                            lomodel = APIHelperMethods.ToModelList<Topsalesmonthbranch>(dt);
                            omodel.data = lomodel;

                            return Json(omodel);
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


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "April",
            //    amount = 1642.85
            //});

            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "May",
            //    amount = 1132.49
            //});


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "June",
            //    amount = 974.55
            //});


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "July",
            //    amount = 948.28
            //});


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "August",
            //    amount = 854.28
            //});


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "September",
            //    amount = 819.08
            //});


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "October",
            //    amount = 760.65
            //});


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "November",
            //    amount = 751.32
            //});

            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "December",
            //    amount = 691.63
            //});

            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "January",
            //    amount = 670.27
            //});

            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "February",
            //    amount = 649.21
            //});


            //lomodel.Add(new Topsalesmonthbranch()
            //{
            //    month = "March",
            //    amount = 607.13
            //});
            ////   return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
            //var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
            return Json(lomodel);
            //return message;
        }
    }
}