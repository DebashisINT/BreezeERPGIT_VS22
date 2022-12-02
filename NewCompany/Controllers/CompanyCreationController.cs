using BusinessLogicLayer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NewCompany.DataAccessLayer;
using NewCompany.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace NewCompany.Controllers
{
    public class CompanyCreationController : Controller
    {
        //
        // GET: /CompanyCreation/

        BusinessLogicLayer.CompanyCreation.NewCompany combl = new BusinessLogicLayer.CompanyCreation.NewCompany();

        public JsonResult SaveNewCompany(string Company_Name, string DbName, string Level, string parentid,DateTime start_dt,DateTime end_dt)
        {

            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);

            string url = Convert.ToString(ConfigurationSettings.AppSettings["Url"]);

            DataTable NewcomDt = combl.SaveNewCompanyData("SAVECOMPANY", Level, GetConnectionString(masterdbname), DbName, Company_Name, parentid,start_dt,end_dt);

            string sqlConnectionString = GetConnectionString(masterdbname);
            DataTable dtNewDb = combl.CreateDb(DbName, sqlConnectionString);

            string output_para = "New Company Created Successfully";

            //string output = CreateDatabsse(sqlConnectionString, DbName);

            string Version = "New Company";


            sqlConnectionString = GetConnectionString(DbName);




            //String Stucture = "Stucture";
            //String Data = "Data";
            //String Procedure = "Procedure";
            //String Function = "Function";
            //String View = "View";



            //foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Stucture, "*.sql"))
            //{
            //    try
            //    {
            //        string script = System.IO.File.ReadAllText(file);
            //        SqlConnection conn = new SqlConnection(sqlConnectionString);
            //        Server server = new Server(new ServerConnection(conn));
            //        server.ConnectionContext.ExecuteNonQuery(script);
            //    }
            //    catch (Exception ex)
            //    {
            //        output_para = "Table creation error. " + ex.Message.ToString();
            //    }
            //}

            //foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Data, "*.sql"))
            //{
            //    try
            //    {
            //        string script = System.IO.File.ReadAllText(file);
            //        SqlConnection conn = new SqlConnection(sqlConnectionString);
            //        Server server = new Server(new ServerConnection(conn));
            //        server.ConnectionContext.ExecuteNonQuery(script);
            //    }
            //    catch (Exception ex)
            //    {
            //        output_para = "Data migration error. " + ex.Message.ToString();
            //    }
            //}

            //foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + View, "*.sql"))
            //{
            //    try
            //    {
            //        string script = System.IO.File.ReadAllText(file);
            //        SqlConnection conn = new SqlConnection(sqlConnectionString);
            //        Server server = new Server(new ServerConnection(conn));
            //        server.ConnectionContext.ExecuteNonQuery(script);
            //    }

            //    catch (Exception ex)
            //    {
            //        output_para = "View creation error. " + ex.Message.ToString();
            //    }
            //}
            //foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Function, "*.sql"))
            //{
            //    try
            //    {
            //        string script = System.IO.File.ReadAllText(file);
            //        SqlConnection conn = new SqlConnection(sqlConnectionString);
            //        Server server = new Server(new ServerConnection(conn));
            //        server.ConnectionContext.ExecuteNonQuery(script);
            //    }
            //    catch (Exception ex)
            //    {
            //        output_para = "Function creation error. " + ex.Message.ToString();
            //    }
            //}
            //foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Procedure, "*.sql"))
            //{
            //    try
            //    {
            //        string script = System.IO.File.ReadAllText(file);
            //        SqlConnection conn = new SqlConnection(sqlConnectionString);
            //        Server server = new Server(new ServerConnection(conn));
            //        server.ConnectionContext.ExecuteNonQuery(script);
            //    }
            //    catch (Exception ex)
            //    {
            //        output_para = "Procedure creation error. " + ex.Message.ToString();
            //    }
            //}
            //SqlConnection updatecon = new SqlConnection(sqlConnectionString);
            //Server serverupdatecon = new Server(new ServerConnection(updatecon));
            //try
            //{
            //    serverupdatecon.ConnectionContext.ExecuteNonQuery("update tbl_master_company set cmp_internalid='" + Convert.ToString(NewcomDt.Rows[0][0]) + "',cmp_Name='" + Company_Name + "'");
            //}
            //catch (Exception e)
            //{
            //    output_para = e.Message.ToString();
            //}
            //try
            //{
            //    serverupdatecon.ConnectionContext.ExecuteNonQuery("update tbl_trans_LastSegment set ls_lastCompany='" + Convert.ToString(NewcomDt.Rows[0][0]) + "'");
            //}

            //catch (Exception e)
            //{
            //    output_para = e.Message.ToString();
            //}

            //try
            //{
            //    serverupdatecon.ConnectionContext.ExecuteNonQuery("update Tbl_Master_CompanyExchange set exch_compId='" + Convert.ToString(NewcomDt.Rows[0][0]) + "'");

            //}
            //catch (Exception e)
            //{
            //    output_para = e.Message.ToString();
            //}


            List<KeyObj> param = new List<KeyObj>();
            ExecProcedure exep = new ExecProcedure();
            param.Add(new KeyObj("@compId", Convert.ToString(NewcomDt.Rows[0][0])));
            param.Add(new KeyObj("@Company_Name", Convert.ToString(Company_Name)));
            param.Add(new KeyObj("@start_dt", start_dt));
            param.Add(new KeyObj("@end_dt", end_dt));
            param.Add(new KeyObj("@DbName", DbName));
            exep.ProcedureName = "Prc_NewCompanyUpdate";
            exep.param = new List<KeyObj>();
            exep.param = param;
            exep.ExecuteProcedureNonQuery(GetConnectionString(DbName));
            param.Clear();
            string encodestring = BusinessLogicLayer.CompanyCreation.Encryption.EncryptString(Convert.ToString(NewcomDt.Rows[0][0]), System.Text.Encoding.Unicode);

            combl.UpdateCompanyEncodeCode("UPDATEENCODESTRING", Convert.ToString(NewcomDt.Rows[0][0]), encodestring, GetConnectionString(masterdbname));

            return Json(url + encodestring, JsonRequestBehavior.AllowGet);

           // return Json(output_para, JsonRequestBehavior.AllowGet);
        }

        private string CreateDatabsse(string constr, string dbname)
        {
            String str = "Please specify a database name.";
            if (!string.IsNullOrEmpty(dbname))
            {

                SqlConnection myConn = new SqlConnection
                    (constr);
                //   str = "CREATE DATABASE " + dbname;


                str = "CREATE DATABASE " + dbname;


                SqlCommand myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    str = "Database Created Successfully";
                }
                catch (System.Exception ex)
                {
                    str = ex.ToString();
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }

            }
            return str;
        }


        private string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;

            }
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;



            return str;
        }




        public ActionResult CreateCompany()
        {
            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);

            CompanyCreationClass model = new CompanyCreationClass();
            DBEngine odbengine = new DBEngine();
            model.Company_List = new List<Company_List>();
            model.Company_type = APIHelperMethods.ToModelList<Company_type>(combl.GetNewCompanyData("GETTYPELIST", "", GetConnectionString(masterdbname)));
            model.grd = new List<Griddata>();

            return View("~/Views/NewCompany/CreateCompany.cshtml", model);
        }
        public JsonResult GetparentCompany(string level)
        {
            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
            List<Company_List> comp = new List<Company_List>();
            comp = APIHelperMethods.ToModelList<Company_List>(combl.GetNewCompanyData("GETCOMPANYLIST", level, GetConnectionString(masterdbname)));
            return Json(comp);
        }

        public PartialViewResult CompanyGrid()
        {
            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
            DataTable DBdt = combl.GetCompanyList("GETCOMPANYDETAILS", GetConnectionString(masterdbname));

            List<Griddata> grd = APIHelperMethods.ToModelList<Griddata>(DBdt);

            return PartialView("~/Views/NewCompany/CompanyGrid.cshtml", grd);
        }


        public JsonResult CompanyIfExists(string DbName)
        {
            bool output = true;
            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
            DataTable DBdt = combl.CompanyIfExists("COMPANYIFEXISTS", GetConnectionString(masterdbname), DbName);

            if (DBdt != null && DBdt.Rows.Count > 0)
            {
                output = false;
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }



    }
}
