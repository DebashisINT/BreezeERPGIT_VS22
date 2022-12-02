using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DataAccessLayer;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using BusinessLogicLayer;
using System.Globalization;
using UtilityLayer;
using System.Web.Script.Serialization;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System.IO.Compression;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using CutOff.Models;
using System.Text;
namespace CutOff.Controllers.CutOff
{
    public class CutOffDBBackUpController : Controller
    {
        //
        // GET: /CutOffDBBackUp/
        SqlCommand cmd;
        SqlDataReader dr;
        SqlConnection _conn;
        string response_msg = "";
        CutOffDBCreate obj = null;

        public CutOffDBBackUpController()
        {
            obj = new CutOffDBCreate();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        class DataBaseClass
        {
            SqlConnection conn;
            public SqlConnection openconn()
            {
                #region
                string CS = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                //string CS = "DSN=raideit; UID=sa; Pwd=1234";
                conn = new SqlConnection(CS);
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        return conn;
                    }
                }
                catch (Exception ex)
                {

                    return null;
                }
                return conn;
                #endregion
            }
        }
        
        [HttpPost]
        //public ActionResult CutOffBackUp()
        public JsonResult CutOffBackUp()
        { 
            DataBaseClass dbc = new DataBaseClass();
            _conn = dbc.openconn();

            string path = "C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup";            
            //string _dbname = "YEAREND_TEST";

            string _dbname = _conn.Database;
            //string _dbname = "YEAREND_TEST_3_6_2022";
            string fileName = _dbname + DateTime.Now.Year.ToString() + "-" +
                DateTime.Now.Month.ToString() + "-" +
                DateTime.Now.Day.ToString() + "-" +
                DateTime.Now.Millisecond.ToString() + ".bak";
            string _sql;

            //if (!System.IO.Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}
            try
            {
                //Rev work start 02.06.2022
                //cmd = new SqlCommand("backup database YEAREND_TEST to disk='" + path + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", _conn);
                cmd = new SqlCommand("BACKUP DATABASE YEAREND_TEST TO DISK='" + path + "\\" + _dbname + ".bak'", _conn);
                //Rev work close 02.06.2022
                cmd.ExecuteNonQuery();
                response_msg = "Backup of Database " + _dbname + ".bak Created Successfully in " + path;
            }
            catch (Exception ex)
            {
                //throw;
                response_msg = "Error Occured During DB backup process !<br>" + ex.ToString();
            }
            //response_msg = "Backup of Database " + _dbname + ".bak Created Successfully in " + path;
            return Json(response_msg, JsonRequestBehavior.AllowGet);
        }     
        [HttpGet]
        public ActionResult NewDBCreate()
        {
            return View();
        }
        public ActionResult CutOffDBCreate()
        {

           /* ViewData["CutoffDate"] = cutoffDate;
            ViewData["BackupDone"] = backup;

            ViewData["Dbname"] = Dbname;
            ViewData["AuditDBName"] = AuditDBName;*/

            return View("~/Views/CutOffDBBackUp/CutOffDBCreate.cshtml");
        }
        [HttpGet]
        public  JsonResult Filldatabase()
        {
            string data = string.Empty;
            string CS = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);            
            SqlConnection con = new SqlConnection(CS);
            string dbname = con.Database;
            SqlDataAdapter da = new SqlDataAdapter("Select '"+dbname+"' as DBNAME",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id=\"ddlDataBase\" style='width:137px;height:30px;'>");
            sb.Append("<option value='Select' selected='"+"selected"+"'>Select</option>");
            if(dt.Rows.Count>0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    sb.Append("<option value=\""+dr["DBNAME"]+"\">"+dr["DBNAME"]+"</option>");
                }
                sb.Append("</select>");
            }
            data = sb.ToString();
            var ReturnData = new
            {
                retdata = data
            };

            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult RestoreDB()
        {
            string data = string.Empty;
            string CS = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(CS);
            string dbname = con.Database;
            string _NewDBName = dbname + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
            //SqlDataAdapter da = new SqlDataAdapter("Select name as DBNAME from sys.databases where name='YEAREND_TEST_3_6_2022'", con);
            SqlDataAdapter da = new SqlDataAdapter("Select name as DBNAME from sys.databases where name='" + _NewDBName + "'", con);
            
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id=\"ddlDataBase\" style='width:250px;height:30px;'>");
            sb.Append("<option value='Select' selected='" + "selected" + "'>Select</option>");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("<option value=\"" + dr["DBNAME"] + "\">" + dr["DBNAME"] + "</option>");
                }
                sb.Append("</select>");
            }
            data = sb.ToString();
            var ReturnData = new
            {
                retdata = data
            };

            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //public JsonResult StartDbCreate(string StrData)
        public JsonResult StartDbCreate()
        {
            string StrData=string.Empty;
            string CS = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(CS);
            StrData = con.Database;
            string _DatabaseName = StrData;

            string _NewDBName = _DatabaseName + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
            //string CS = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]); 
            //SqlConnection con = new SqlConnection(CS);            
            try
            {
                con.Open();
                string sqlquery = "CREATE DATABASE " + _NewDBName;
                SqlCommand cmd = new SqlCommand(sqlquery, con);
                cmd.CommandType = CommandType.Text;
                int iRows = cmd.ExecuteNonQuery();
                con.Close();
                response_msg = "1";
            }
            catch (Exception ex)
            {
                //throw;
                response_msg = ex.ToString();
            }    
            //response_msg = response_msg = "1";          
            return Json(response_msg, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ResoteDB()
        {
            //return RedirectToAction("CreateRestoreBackUp", "CutOffDBBackUp");
            return RedirectToAction("CreateRestoreBackUp", "CutOffDBBackUp");
        } 
      
        
        [HttpGet]
        public JsonResult ReadBackupFiles()
        {

            //if (!Directory.Exists(@"C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup"))
            //{
            //    Directory.CreateDirectory(@"C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup");
            //}
           
            //if (!System.IO.Directory.Exists(@"C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\Backup"))
            //{
            //    System.IO.Directory.CreateDirectory(@"C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\Backup");

            //}

            if (!System.IO.Directory.Exists(@"D:\D\Backup"))
          {
              System.IO.Directory.CreateDirectory(@"D:\D\Backup");

          }
            //string[] files = Directory.GetFiles(@"C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup", "*.bak");
           
            //string[] files = Directory.GetFiles(@"C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\Backup", "*.bak");

            string[] files = Directory.GetFiles(@"D:\D\Backup", "*.bak");

            DataTable tbl = new DataTable();

            for (int i = 0; i < files.GetLength(0); i++)
            {
                tbl.Columns.Add("Column" + (i + 1));
            }
            for (var i = 0; i < files.GetLength(0); ++i)
            {
                DataRow row = tbl.NewRow();
                for (var j = 0; j < files.GetLength(0); ++j)
                {
                    row[j] = files[i];
                }
                tbl.Rows.Add(row);
            }
            StringBuilder sb2 = new StringBuilder();
            string Data1 = string.Empty;

            sb2.Append("<select class=\"form-control\" id=\"ddlbackUp\" style='width:250px;height:30px;'>");
            sb2.Append("<option value='Select' selected='" + "selected" + "'>Select</option>");
            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow dr in tbl.Rows)
                {
                    sb2.Append("<option value=\"" + dr["Column1"] + "\">" + dr["Column1"] + "</option>");
                }
                sb2.Append("</select>");
            }
            Data1 = sb2.ToString();

            var ReturnData = new
            {
                retData2 = Data1
            };
            return Json(ReturnData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        //public JsonResult CreateRestoreBackUp(string FromStrData, string ToStrData)
        public ActionResult CreateRestoreBackUp()
        {
            //string _DatabaseName = FromStrData;
            string CS = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(CS);

            string dbname = con.Database;
            string _NewDBName = dbname + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
            string ToStrData = _NewDBName;
            string _BackupName = ToStrData;

            string filepath = _BackupName;
            //string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filepath);

            //string filepathFrom = _DatabaseName;
            //string fileNameWithoutExtFrom = Path.GetFileNameWithoutExtension(filepathFrom);

            
            string fileNameWithoutExtFrom = con.Database;

            string ROWS = string.Empty;
            string LOG = string.Empty;

            DataTable dt = obj.GetDBType(fileNameWithoutExtFrom);
            if (dt.Rows.Count > 0)
            {
                ROWS = dt.Rows[0]["LOGICALNAME_ROWS"].ToString();
                LOG = dt.Rows[0]["LOGICALNAME_LOG"].ToString();
            }

            string path = "C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup\\" + fileNameWithoutExtFrom + ".bak";

            con.Open();
            string sqlQuery = "RESTORE DATABASE " + _BackupName + " FROM DISK = '" + path + "' " +
                                      "WITH REPLACE, RECOVERY, " +
                                      "MOVE N'" + ROWS + "' TO 'C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup\\" + _BackupName + ".mdf', " +
                                      "MOVE N'" + LOG + "' TO 'C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup\\" + _BackupName + ".ldf';  ";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, con);
            sqlCommand.CommandType = CommandType.Text;
            int iRows = sqlCommand.ExecuteNonQuery();
            con.Close();
            
            Boolean Success = false;
            DataSet dt1 = new DataSet();
            dt1 = obj.InsertCompany(_NewDBName);

            return RedirectToAction("DropTableSchema", "CutOffDBBackUp");
            //return RedirectToAction("Action2", "ControllerName");
            /*New addition*/
           
            /* con.Open();
             string sqlQuery = "RESTORE DATABASE " + _BackupName + " FROM DISK = '" + filepathFrom + "' " +
                                       "WITH REPLACE, RECOVERY, " +
                                       "MOVE N'" + fileNameWithoutExtFrom + "' TO 'D:\\BackupDB\\" + _BackupName + ".mdf', " +
                                       "MOVE N'" + fileNameWithoutExtFrom + "_log' TO 'D:\\BackupDB\\" + _BackupName + ".ldf';  ";
             SqlCommand sqlCommand = new SqlCommand(sqlQuery, con);
             sqlCommand.CommandType = CommandType.Text;
             int iRows = sqlCommand.ExecuteNonQuery();
             con.Close();*/

            //response_msg = "The  database restore successfully...";
            //return Json(response_msg, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult StartDbCreate()
        //{
        //    try
        //    {
        //        DataBaseClass dbc = new DataBaseClass();
        //        _conn = dbc.openconn();
        //        string path = "C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup\\YEAREND_TEST.BAK";
        //        //string destdir = "C:\\backupdb\\11082014_121403.Bak";

        //        cmd = new SqlCommand("Restore database YEAREND_TEST from disk='C:\\Program Files\\Microsoft SQL Server\\MSSQL14.SQLEXPRESS\\MSSQL\\Backup\\YEAREND_TEST_NEW_04052022.BAK' ", _conn);
        //        cmd.ExecuteNonQuery();
        //        response_msg = "Restore database successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("Error During backup database!");
        //    }
        //    return Json(response_msg, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult FetchDB()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreateCompany(string dbnm)
        {
            string DatabaseNM = dbnm;
            Boolean Success = false;
            DataSet dt = new DataSet();
            try
            {
                dt = obj.InsertCompany(DatabaseNM);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                    }
                }
                if (Success == true)
                {
                    response_msg = "Company Create Successfully";
                }
                else
                {
                    response_msg = "Company Creation Failed";
                }
            }
            catch { }             
            return Json(response_msg,JsonRequestBehavior.AllowGet);
        }
       [HttpGet]
        public ActionResult DropCreateTable()
        {
            return View();
        }
        //[HttpPost]
        //public JsonResult DropTableSchema()
       [HttpGet]
       public ActionResult DropTableSchema()
        {
            Boolean Success = false;
            DataSet dt = new DataSet();
            try
            {
                dt = obj.DropTBLSchema();
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                    }
                }
                if (Success == true)
                {
                    response_msg = "CutOff Process completed Successfully";
                }
                else
                {
                    response_msg = "CutOff Process Failed";
                }
            }               
            catch { }
            TempData["msg"] = response_msg;
           // return Json(response_msg, JsonRequestBehavior.AllowGet);
            return RedirectToAction("SuccessReturn", "CutOffDBBackUp");
        }
        [HttpGet]
        public ActionResult SuccessReturn()
       {
           return View();
       }
        [HttpPost]
        public JsonResult CreateTableSchema()
        {
            Boolean Success = false;
            DataSet dt = new DataSet();
            try
            {
                dt = obj.CreateTBLSchema();
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                    }
                }
                if (Success == true)
                {
                    response_msg = "Table Drop Successfully";
                }
                else
                {
                    response_msg = "Company Creation Failed";
                }
            }
            catch { }
            return Json(response_msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertFinYear()
        {
            Boolean Success = false;
            DataSet dt = new DataSet();
            try
            {
                dt = obj.InsertFinancialYear();
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                    }
                }
                if (Success == true)
                {
                    response_msg = "Financial Year set Successfully done and Cutoff process done successfully";
                }
                else
                {
                    response_msg = "Financial Year set Failed";
                }
            }
            catch { }
            return Json(response_msg, JsonRequestBehavior.AllowGet);
        }
	}
}