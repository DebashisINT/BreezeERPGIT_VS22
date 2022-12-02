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
    public class TopsalesitemwiseController : Controller
    {

        //http://localhost:1748/Topsalesitemwise/SalesItem
         [AcceptVerbs("POST")]
        public JsonResult SalesItem(SalesItemInput model)
        {
            List<Items> lomodel = new List<Items>();
            TopSalesItemsoutput omodel = new TopSalesItemsoutput();

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

                     sqlcmd = new SqlCommand("SP_Reports_ItemwiseSales", sqlcon);
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
                             lomodel = APIHelperMethods.ToModelList<Items>(dt);
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


            //List<Items> lomodel = new List<Items>();
            //TopSalesItemsoutput omodel = new TopSalesItemsoutput();

            //lomodel.Add(new Items()
            //{
            //    title = "SAMSUNG LED 42",
            //    amount = 1643.97,
            //    quantity=1500

            //});

            //lomodel.Add(new Items()
            //{
            //    title = "SONY LED 40",
            //    amount = 1132.59,
            //    quantity=1100

            //});

            //lomodel.Add(new Items()
            //{
            //    title = "SAMSUNG 32",
            //    amount = 974.27,
            //    quantity = 900

            //});

            //lomodel.Add(new Items()
            //{
            //    title = "SONY LED 50",
            //    amount = 974.27,
            //    quantity = 900

            //});

            //lomodel.Add(new Items()
            //{
            //    title = "Panasonic 40",
            //    amount = 974.27,
            //    quantity = 900

            //});



            //lomodel.Add(new Items()
            //{
            //    title = "LG LED 40",
            //    amount = 948.64,
            //    quantity = 840

            //});

            //lomodel.Add(new Items()
            //{
            //    title = "LG LED 52",
            //    amount = 948.64,
            //    quantity = 840

            //});


            //lomodel.Add(new Items()
            //{
            //    title = "SAMSUNG LED 32",
            //    amount = 760.64,
            //    quantity = 760

            //});



            //lomodel.Add(new Items()
            //{
            //    title = "IPHONE 6",
            //    amount = 670.27,
            //    quantity = 900

            //});

            //lomodel.Add(new Items()
            //{
            //    title = "SAMSUNG J1",
            //    amount = 607.13,
            //    quantity = 1700

            //});

            //lomodel.Add(new Items()
            //{
            //    title = "IFB Microwave",
            //    amount = 580.49,
            //    quantity = 1490

            //});

            //omodel.unit = "Rs.(In Lakh)";
            //omodel.month = "February";
            //omodel.year = "2017";
            //omodel.data = lomodel;


            return Json(lomodel);
        }

    }
}
