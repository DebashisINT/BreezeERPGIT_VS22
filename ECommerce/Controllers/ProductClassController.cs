using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UtilityLayer;

namespace ECommerce.Controllers
{
    public class ProductClassController : ApiController
    {
        public HttpResponseMessage ProductsClass()
        {
            Productclasslistoutput omodel = new Productclasslistoutput();
            List<Productclasslist> omodelproduct = new List<Productclasslist>();

            try
            {

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("Ecom_API_ProductClasslist", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.Add("@PageNo", model.pageno);   
                //sqlcmd.Parameters.Add("@Pagerows", model.rowcount);
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();


                if (dt.Rows.Count > 0)
                {
                    omodel.success = "200";
                    omodel.message = "Total products class listed";
                    omodel.class_details = APIHelperMethods.ToModelList<Productclasslist>(dt);
                }

            }
            catch
            {

            }

            var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
            return message;

        }

    }
}
