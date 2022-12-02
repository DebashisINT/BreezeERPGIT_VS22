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
    public class TopNVendorsController : Controller
    {

        ///http://localhost:1748/TopNVendors/SalesItem
        ///
        [AcceptVerbs("POST")]
        public JsonResult SalesItem(NvendorsInput model)
        {

            List<Nsalesmonthbranch> lomodel = new List<Nsalesmonthbranch>();
            Nvendorsoutput omodel = new Nvendorsoutput();

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

                    sqlcmd = new SqlCommand("Sp_Report_TopNVendors", sqlcon);
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

                            lomodel = APIHelperMethods.ToModelList<Nsalesmonthbranch>(dt);
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            omodel.unit = "RS.(In Lakh)";
                            omodel.month = model.month;
                            omodel.year = model.year;
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


            //lomodel.Add(new Nsalesmonthbranch()
            //{
            //    vendor = "Samsung India Electronics Pvt Ltd.",
            //     amount=1642.45
            //});

            //lomodel.Add(new Nsalesmonthbranch()
            //{
            //    vendor = "Carrier Media India Pvt Ltd.",
            //    amount = 814.00
            //});

            //lomodel.Add(new Nsalesmonthbranch()
            //{
            //    vendor = "Sony India Pvt Ltd.",
            //    amount = 750.42
            //});

            //lomodel.Add(new Nsalesmonthbranch()
            //{
            //    vendor = "LG Electronics India Pvt Ltd.",
            //    amount = 735.31
            //});
            //lomodel.Add(new Nsalesmonthbranch()
            //{
            //    vendor = "Daikin. Airconditioning India Pvt Ltd.",
            //    amount = 230.86
            //});


            //omodel.unit = "RS.(In Lakh)";
            //omodel.month = "February";
            //omodel.year = "2017";
            //omodel.data = lomodel;


            //   return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
            return Json(omodel);
        }


    }
}
