using ERP.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder(); 
            DataSet PosProductData = new DataSet();

            ReadWriteMasterDatabaseBL bbl = new ReadWriteMasterDatabaseBL();

            //  SqlConnection ConDbconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString);

            SqlConnection ConDbconnection = new SqlConnection(bbl.GetDefaultConnectionStringWithoutSession());


            SqlCommand CommandOnApp = new SqlCommand("prc_taxschemeMaxdate", ConDbconnection);
            CommandOnApp.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter ApplicationAdapter = new SqlDataAdapter();
            ApplicationAdapter.SelectCommand = CommandOnApp;
            ApplicationAdapter.Fill(PosProductData);
            System.Web.HttpContext.Current.Cache["SchemeMaxDate"] = PosProductData.Tables[0].Rows[0][0];
            //  Application.Add("POSPRODUCTLISTDATA", PosProductData.Tables[0]);
            CommandOnApp.Dispose();
            ConDbconnection.Dispose();
            ApplicationAdapter.Dispose();
            

        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomActionFilterAttribute());
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //newCulture.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            //newCulture.DateTimeFormat.LongDatePattern = "dd-MMM-yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "-";
            //Thread.CurrentThread.CurrentCulture = newCulture;
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            HttpContext con = HttpContext.Current;
            var v = Server.GetLastError();
            var httpex = v as HttpException;
            if(httpex!=null && httpex.GetHttpCode()==404)
            {

            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Page :    " + Convert.ToString(con.Request.Url));
                sb.AppendLine("Error Message : " + v.Message);
                sb.AppendLine("Inner Message : " + Convert.ToString(v.InnerException)); 
                string filename = Path.Combine(Server.MapPath("~/Errors"), DateTime.Now.ToString("ddMMyyyy")+".txt");
                File.WriteAllText(filename, sb.ToString());
            }
        }


        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            string result = String.Empty;
            if (arg == "userid")
            {
                object userid = Session["userid"];
                if (userid != null) { result = userid.ToString(); }
            }
            else { result = base.GetVaryByCustomString(context, arg); }
            return result;
        }


    }
}
