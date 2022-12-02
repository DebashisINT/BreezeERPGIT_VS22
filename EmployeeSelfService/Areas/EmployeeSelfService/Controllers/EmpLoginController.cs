using BusinessLogicLayer;
using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class EmpLoginController : Controller
    {
        //
        // GET: /EmployeeSelfService/EmpLogin/
        BusinessLogicLayer.CompanyCreation.NewCompany combl = new BusinessLogicLayer.CompanyCreation.NewCompany();
        public ActionResult Login(string id)
        {
            string Company_Code = id;
            if (Company_Code != "undefined")
            {

                Session["Company_Code_ESS"] = Company_Code;

                if (Company_Code != null)
                {
                    string constr = GetConnectionStringByCompanyCode(Company_Code);
                    if (constr == "Not Found")
                    {

                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        Session["ErpConnection"] = constr;
                        APIConnction.ApiConnction = constr;
                        //return View("~/Views/NewCompany/Login.cshtml");
                    }

                }
                else
                {
                    string constr = GetConnectionStringByCompanyCode();
                    if (constr == "Not Found")
                    {

                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        Session["Company_Code_ESS"] = "";
                        Session["ErpConnection"] = constr;
                    }
                }
            }
            
            return View();
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
        public JsonResult DoLogin(LoginClass obj)
        {
            string output = "";
            string url = Url.Action("EmpDashboard", "EmpDashboard");
            //Session["ErpConnection"] = @"Data Source=3.7.30.86,1480\SQLEXPRESS;Initial Catalog=PK02122020;User ID=sa; Password=5dc57YITWh";

            DBEngine objDB = new DBEngine();
            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(obj.PassWord);
            string valid = objDB.AuthenticateUser(obj.username, Encryptpass);
            if (valid == "Y")
            {
                //Session["userid"] = "1";
                string user_id = Session["userid"].ToString();
                //rev Pratik
                GetSessions(user_id);
                GetCompanySession(user_id);
                //Session["UserName"] = obj.username;
                //End of rev Pratik
                output = "success";
                RedirectToAction("EmpDashboard", "EmpDashboard");

            }
            else
            {
                Session["userid"] = null;
                output = valid;
            }

            return Json(new { rtnText = output, rntUrl = url }, JsonRequestBehavior.AllowGet);
        }
        //rev Pratik
        private string GetSessions(string userid)
        {
            string msg = "";
            DataSet dt = new DataSet();
            String con = Convert.ToString(APIConnction.ApiConnction); ;
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
            sqlcmd.Parameters.Add("@Action", "EMP_DETAILS");
            sqlcmd.Parameters.Add("@USER_ID", userid);
            //sqlcmd.Parameters.Add("@user_id", model.user_id);
            //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
            //sqlcmd.Parameters.Add("@TASK", model.task);
            //sqlcmd.Parameters.Add("@DETAILS", model.details);
            //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
            //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();

            DataTable dts = dt.Tables[0];

            userInformation obj = new userInformation();
            obj.Name = Convert.ToString(dts.Rows[0]["Name"]);
            obj.deg_designation = Convert.ToString(dts.Rows[0]["deg_designation"]);
            obj.Date_Of_Confirmation = Convert.ToString(dts.Rows[0]["Date_Of_Confirmation"]);
            obj.emp_dateofJoining = Convert.ToString(dts.Rows[0]["emp_dateofJoining"]);

            obj.empCode = Convert.ToString(dts.Rows[0]["empCode"]);
            obj.phoneNo = Convert.ToString(dts.Rows[0]["phoneNo"]);
            obj.reportingManager = Convert.ToString(dts.Rows[0]["reportingManager"]);
            obj.dateOfBirth = Convert.ToString(dts.Rows[0]["dateOfBirth"]);
            obj.profileImage = Convert.ToString(dts.Rows[0]["profileImage"]);
            obj.AddressInfo = Convert.ToString(dts.Rows[0]["AddressInfo"]);
            obj.PanNo = Convert.ToString(dts.Rows[0]["PanNo"]);
            obj.emailId = Convert.ToString(dts.Rows[0]["emailId"]);
            obj.brachName = Convert.ToString(dts.Rows[0]["brachName"]);
            obj.fathersName = Convert.ToString(dts.Rows[0]["fathersName"]);

            Session["Name"] = obj.Name;
            Session["empCode"] = obj.empCode;
            Session["emailId"] = obj.emailId;
            Session["deg_designation"] = obj.deg_designation;
            Session["brachName"] = obj.brachName;
            //Mantis Issue 24411
            Session["USER_ID"] = userid;
            //End of Mantis Issue 24411
            //oview.userinfo = obj;
            //oview.status = "200";
            //oview.message = "Successfully add task.";
            return msg;
        }

        private string GetCompanySession(string userid)
        {
            string msg = "";
            DBEngine obj = new DBEngine();
            MasterPageBL objMasterPageBL = new MasterPageBL();
            DataTable dt = obj.GetDataTable("select user_id,grp_name from tbl_master_user inner join tbl_master_userGroup on user_group=grp_id WHERE user_id='" + userid + "'");
            string group_name = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                group_name = Convert.ToString(dt.Rows[0]["grp_name"]);
            }
            DataSet dst = new DataSet();
            dst = objMasterPageBL.CompanyMasterDetail(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["ExchangeSegmentID"]), Convert.ToString(Session["userid"]), Convert.ToString(Session["userlastsegment"]), Convert.ToString(Session["usergoup"]));
            string urlName = Convert.ToString(Request.UrlReferrer);

            if (dst.Tables[8] != null && dst.Tables[8].Rows.Count > 0)
            {
                Session["lablelCompanyName"] = Convert.ToString(dst.Tables[8].Rows[0]["comp"]) + " (" + Convert.ToString(dst.Tables[8].Rows[0]["Branch"]) + ")";
                Session["LastFinYear"] = Convert.ToString(dst.Tables[8].Rows[0]["ls_lastFinYear"]);
                Session["FinYearStart"] = Convert.ToString(dst.Tables[8].Rows[0]["FinYearStart"]);
                Session["FinYearEnd"] = Convert.ToString(dst.Tables[8].Rows[0]["FinYearEnd"]);
            }
            else
            {


            }
            return msg;
        }

        public ActionResult SignOff()
        {
            Session.Abandon();
            return RedirectToAction("Login", new { Area = "EmployeeSelfService", id = "COR0000005" });
        }
        //End of rev Pratik
    }

    

    public class LoginClass
    {
        public string username { get; set; }
        public string PassWord { get; set; }
    }
}
