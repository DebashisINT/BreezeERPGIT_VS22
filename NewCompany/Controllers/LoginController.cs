using NewCompany.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace NewCompany.Controllers
{
    public class LogInController : Controller
    {
        BusinessLogicLayer.CompanyCreation.NewCompany combl = new BusinessLogicLayer.CompanyCreation.NewCompany();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        //
        // GET: /Login/
        [HttpPost]
        public ActionResult LogInToERP(LoginClass logincls)
        {

            string constring = Convert.ToString(Session["ErpConnection"]);


          //  Session["ErpConnection"] = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(logincls.PassWord);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            string Validuser = oDBEngine.AuthenticateUser(logincls.UserId, Encryptpass).ToString();




            string msg = "0";
            try
            {
                if (Validuser == "Y")
                {
                    Session["CurConString"] = Session["ErpConnection"];
                    Session["date"] = DateTime.Today.ToString("dd/MM/yyy");
                    Session["date1"] = DateTime.Today.AddDays(10).ToString("dd/MM/yyy");
                    msg = "1";
                    Session["CurConString"] = Session["CurConString"];
                    return RedirectToAction("UserDashBoard",logincls); 
                }
                else
                {
                    ViewBag.Validate = false;
                    return View("~/Views/NewCompany/Login.cshtml");
                }
            }
            catch (Exception ex)
            {
                msg = "0";
                return RedirectToAction("/Login/" + Convert.ToString(Session["Company_Code"]));
            }
           
        }

        public ActionResult login(string Company_Code)
        {

            

            if (Company_Code != "undefined")
            {

                Session["Company_Code"] = Company_Code;

                if (Company_Code != null)
                {
                    int mod4 = Company_Code.Length % 4;
                    //if (mod4 > 0)
                    //{
                    //    Company_Code += new string('=', 4 - mod4);
                    //    Company_Code += Company_Code;
                    //    Company_Code += Company_Code;
                    //    Company_Code += Company_Code;
                    //    Company_Code += Company_Code;
                    //    Company_Code += Company_Code;
                    //    Company_Code += Company_Code;
                    //    Company_Code += Company_Code;

                    //}
                    Company_Code = BusinessLogicLayer.CompanyCreation.Encryption.DecryptString(Convert.ToString(Company_Code), System.Text.Encoding.Unicode);


                    if (Company_Code == "Decryption Failed")
                    {
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        string constr = GetConnectionStringByCompanyCode(Company_Code);
                        if (constr == "Not Found")
                        {

                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else
                        {
                            Session["ErpConnection"] = constr;
                            Session["LastVersion"] = getApplicationVersion();
                            return View("~/Views/NewCompany/Login.cshtml");
                        }
                    }
                }
                else
                {
                    string constr = GetConnectionStringByCompanyCode();
                    //string constr = "Data Source=3.7.30.86,1480\\SQLEXPRESS;Initial Catalog=AMITH02122020;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=5dc57YITWh;Pooling=True;Max Pool Size=200";
                    if (constr == "Not Found")
                    {

                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        Session["Company_Code"] = "";
                        Session["ErpConnection"] = constr;
                        Session["LastVersion"] = getApplicationVersion();
                        return View("~/Views/NewCompany/Login.cshtml");
                    }
                }
            }
            else
            {
                return View("~/Views/NewCompany/Login.cshtml");
            }
        }
        public string getApplicationVersion()
        {
            string[,] getData;
            getData = oDBEngine.GetFieldValue("Master_CurrentDBVersion", "CurrentDBVersion_Number", null, 1);

            return getData[0, 0];
        }
        private string GetConnectionStringByCompanyCode(string Company_Code)
        {
            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
            DataTable DBdt = combl.GetDatabasFromCompanyCode("GETDBNAME", Company_Code, GetConnectionString(masterdbname));


            if (DBdt != null && DBdt.Rows.Count > 0)
            {
                return GetConnectionString(Convert.ToString(DBdt.Rows[0][0]));
            }
            else
            {
                return "Not Found";
            }
        }


        private string GetConnectionStringByCompanyCode()
        {
            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
            DataTable DBdt = combl.GetDatabasFromCompanyCode("GETDEFAULTDBNAME", "", GetConnectionString(masterdbname));


            if (DBdt != null && DBdt.Rows.Count > 0)
            {
                return GetConnectionString(Convert.ToString(DBdt.Rows[0][0]));
            }
            else
            {
                return "Not Found";
            }
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
        public ActionResult UserDashBoard(LoginClass logincls)
        {

            if (Session["userid"] != null)
            {
                //return Redirect("/OMS/management/ProjectMainPage.aspx");
                if (logincls.ismobile == "0")
                    return Redirect("/OMS/management/ProjectMainPage.aspx");

                else
                  return  Redirect("~/DashBoard/MoView/index.html");
            }
            else
            {
                return RedirectToAction("/Login/" + Convert.ToString(Session["Company_Code"]));
            }  
            
        }

    }
}
