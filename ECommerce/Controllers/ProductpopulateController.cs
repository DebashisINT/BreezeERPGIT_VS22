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
    public class ProductpopulateController : ApiController
    {
        public HttpResponseMessage Products(Productlistinput model)
        {
            Productlistoutput omodel = new Productlistoutput();
            List<Productlist> omodelproduct=new List<Productlist>();
            try
            {

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                String siteURL = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
           
                sqlcmd = new SqlCommand("Ecom_API_Productlist", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@PageNo", model.pageno);
                sqlcmd.Parameters.Add("@Pagerows", model.rowcount);
                sqlcmd.Parameters.Add("@WebURL", siteURL);
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);

                da.Fill(dt);
                sqlcon.Close();

                if (dt.Rows.Count > 0)
                {
                    omodel.success = "200";
                    omodel.message = "Total products listed";
                    omodel.product_details = APIHelperMethods.ToModelList<Productlist>(dt);
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
