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
    public  class SalesComparisonController : Controller
    {
        //http://localhost:1748/SalesComparison/SalecomparaisonBrand
        [AcceptVerbs("POST")]

        public JsonResult SalecomparaisonBrand(string token)
        {
         //   this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            List<SaleBrands> lmodel = new List<SaleBrands>();
            SalecomparisonBrandoutput omodel = new SalecomparisonBrandoutput();

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

                    sqlcmd = new SqlCommand("SP_Reports_BrandcodeSales", sqlcon);
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
                            lmodel = APIHelperMethods.ToModelList<SaleBrands>(dt);
                            omodel.slecombrand = lmodel;
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
            //Lmodel.Add(new SaleBrands()
            //{
            //    title = "Samsung",
            //    amountcm=173.23,
            //    quantitycm=395,
            //    amountlm=92.28,
            //    quantitylm=205
            //});


            //Lmodel.Add(new SaleBrands()
            //{
            //    title = "Sony",
            //    amountcm = 264.69,
            //    quantitycm = 538,
            //    amountlm = 164.57,
            //    quantitylm = 321
            //});


            //Lmodel.Add(new SaleBrands()
            //{
            //    title = "LG",
            //    amountcm = 111.95,
            //    quantitycm = 266,
            //    amountlm = 66.33,
            //    quantitylm = 146
            //});


            //Lmodel.Add(new SaleBrands()
            //{
            //    title = "PANASONIC",
            //    amountcm = 112.10,
            //    quantitycm = 286,
            //    amountlm = 71.57,
            //    quantitylm = 204
            //});


            //Lmodel.Add(new SaleBrands()
            //{
            //    title = "PHILIPS",
            //    amountcm = 20.13,
            //    quantitycm = 58,
            //    amountlm = 10.13,
            //    quantitylm = 37
            //});


            //Lmodel.Add(new SaleBrands()
            //{
            //    title = "PHILIPS",
            //    amountcm = 20.13,
            //    quantitycm = 58,
            //    amountlm = 10.13,
            //    quantitylm = 37
            //});


            //Lmodel.Add(new SaleBrands()
            //{
            //    title = "TOTAL",
            //    amountcm = 685.10,
            //    quantitycm = 1303,
            //    amountlm = 404.88,
            //    quantitylm = 913
            //});


            //omodel.unit = "Rs.(IN LAKHS)";
            //omodel.data = Lmodel;


          //return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
            return Json(lmodel);
        }

    }

    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            base.OnActionExecuting(filterContext);
        }
    }
}
