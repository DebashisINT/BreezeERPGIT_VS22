using BusinessLogicLayer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NewCompany.Controllers
{
    public class UpdateDbversionController : Controller
    {
        //
        // GET: /UpdateDbversion/
        MasterDbEngine modb = new MasterDbEngine();
        public JsonResult UpdateDb()
        {
            string output = "";
            string output_para = "Database updated successfully without error. Enjoy!";
            string sqlConnectionString = "";
            DataTable dtCompany = modb.GetDataTable("select DbName DbName,Name Company_Name from ERP_Company_List where IsActive=1");


            foreach (DataRow dr in dtCompany.Rows)
            {

                string dbname = Convert.ToString(dr["DbName"]);

                DataTable dtFOLDER = modb.GetDataTable("SELECT VERSION_FOLDER FROM ERP_VERSIONS WHERE ID>(select ID from ERP_VERSIONS EV INNER JOIN (select VersionHistory_Number from " + dbname + ".dbo.Master_VersionHistory where VersionHistory_UpdateDate=(select MAX(VersionHistory_UpdateDate) from " + dbname + ".dbo.Master_VersionHistory)) TBL ON TBL.VersionHistory_Number=EV.VIRSION_NAME)");

                if (dtFOLDER != null && dtFOLDER.Rows.Count > 0)
                {
                    foreach (DataRow DDR in dtFOLDER.Rows)
                    {

                        sqlConnectionString = GetConnectionString(dbname);

                        String Stucture = "Stucture";
                        String Data = "Data";
                        String Procedure = "Procedure";
                        String Function = "Function";
                        String View = "View";
                        string Version = Convert.ToString(DDR["VERSION_FOLDER"]);


                        foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Stucture, "*.sql"))
                        {
                            try
                            {
                                string script = System.IO.File.ReadAllText(file);
                                SqlConnection conn = new SqlConnection(sqlConnectionString);
                                Server server = new Server(new ServerConnection(conn));
                                server.ConnectionContext.ExecuteNonQuery(script);
                            }
                            catch (Exception ex)
                            {
                                output_para = "Table creation error. " + ex.Message.ToString();
                            }
                        }

                        foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Data, "*.sql"))
                        {
                            try
                            {
                                string script = System.IO.File.ReadAllText(file);
                                SqlConnection conn = new SqlConnection(sqlConnectionString);
                                Server server = new Server(new ServerConnection(conn));
                                server.ConnectionContext.ExecuteNonQuery(script);
                            }
                            catch (Exception ex)
                            {
                                output_para = "Data migration error. " + ex.Message.ToString();
                            }
                        }

                        foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + View, "*.sql"))
                        {
                            try
                            {
                                string script = System.IO.File.ReadAllText(file);
                                SqlConnection conn = new SqlConnection(sqlConnectionString);
                                Server server = new Server(new ServerConnection(conn));
                                server.ConnectionContext.ExecuteNonQuery(script);
                            }

                            catch (Exception ex)
                            {
                                output_para = "View creation error. " + ex.Message.ToString();
                            }
                        }
                        foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Function, "*.sql"))
                        {
                            try
                            {
                                string script = System.IO.File.ReadAllText(file);
                                SqlConnection conn = new SqlConnection(sqlConnectionString);
                                Server server = new Server(new ServerConnection(conn));
                                server.ConnectionContext.ExecuteNonQuery(script);
                            }
                            catch (Exception ex)
                            {
                                output_para = "Function creation error. " + ex.Message.ToString();
                            }
                        }
                        foreach (string file in Directory.EnumerateFiles(Server.MapPath("/") + "/Migrations/" + Version + "/" + Procedure, "*.sql"))
                        {
                            try
                            {
                                string script = System.IO.File.ReadAllText(file);
                                SqlConnection conn = new SqlConnection(sqlConnectionString);
                                Server server = new Server(new ServerConnection(conn));
                                server.ConnectionContext.ExecuteNonQuery(script);
                            }
                            catch (Exception ex)
                            {
                                output_para = "Procedure creation error. " + ex.Message.ToString();
                            }
                        }

                        DataTable dt = modb.GetDataTable("INSERT INTO " + dbname + ".dbo.Master_VersionHistory(VersionHistory_UpdateDate,VersionHistory_Number) VALUES(GETDATE(),(select VIRSION_NAME FROM ERP_VERSIONS where ID=(select MAX(ID) from ERP_VERSIONS)))");
                    }

                   

                }
            }

            return Json(output_para,JsonRequestBehavior.AllowGet);
        }

        public ActionResult updateDBView()
        {
            return View(@"~\Views\NewCompany\DbAutoUpdate\UpdateDb.cshtml");
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

    }
}
