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

    //http://localhost:1748/SaleComparisonClass/Classwise
    public class SaleComparisonClassController : Controller
    {
        [AcceptVerbs("POST")]
        public JsonResult Classwise(string token)
        {
            Salecomparisonclassoutput omodel = new Salecomparisonclassoutput();
            List<Salecomparisonclass> lomodel = new List<Salecomparisonclass>();


            try
            {
                String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];

                if (token == tokenmatch)
                {

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();

                    sqlcmd = new SqlCommand("SP_Reports_ClasswisecodeeSales", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);

                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            omodel.ResponseCode = "200";
                            omodel.Responsedetails = "Success";
                            lomodel = APIHelperMethods.ToModelList<Salecomparisonclass>(dt);
                            omodel.slecomclass = lomodel;
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


            //lomodel.Add(new Salecomparisonclass()
            //{
            //    title = "LED-39 to 46",
            //    amountcm = 685106.07,
            //    quantitycm = 1543,
            //    amountlm = 404.88,
            //    quantitylm = 913
            //});

            //lomodel.Add(new Salecomparisonclass()
            //{
            //    title = "REF-DD-125L to 200L",
            //    amountcm = 5.69,
            //    quantitycm = 38,
            //    amountlm = 11.89,
            //    quantitylm = 99
            //});

            //lomodel.Add(new Salecomparisonclass()
            //{
            //    title = "REF-DD-201L to 260L",
            //    amountcm = 269.05,
            //    quantitycm = 1140,
            //    amountlm = 194.35,
            //    quantitylm = 796
            //});

            //lomodel.Add(new Salecomparisonclass()
            //{
            //    title = "A C Split Inddor(2 ton an Above)",
            //    amountcm = 11.30,
            //    quantitycm = 94,
            //    amountlm = 9.06,
            //    quantitylm = 72
            //});

            //lomodel.Add(new Salecomparisonclass()
            //{
            //    title = "A C Split Inddor(Below 2 ton)",
            //    amountcm = 123.11,
            //    quantitycm = 1350,
            //    amountlm = 145.81,
            //    quantitylm = 1602
            //});

            //lomodel.Add(new Salecomparisonclass()
            //{
            //    title = "Total",
            //    amountcm = 685515.22,
            //    quantitycm = 4165,
            //    amountlm = 765.99,
            //    quantitylm = 3482
            //});

            //omodel.unit = "Rs.(In Lakh)";
            //omodel.data = lomodel;


            //return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
            return Json(lomodel);


        }

    }




}
