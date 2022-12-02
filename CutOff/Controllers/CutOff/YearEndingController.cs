using CutOff.Models;
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
using BusinessLogicLayer.YearEnding;
using UtilityLayer;
using System.Web.Script.Serialization;

using DevExpress.Web;
using DevExpress.Web.Mvc;



namespace CutOff.Controllers.CutOff
{

    [SessionTimeout]
    public class YearEndingController : Controller
    {


        YearEndingBL YearEndingBL = new YearEndingBL();

        AccountsBL AccntcBL = new AccountsBL();

        //
        // GET: /YearEnding/
        public ActionResult CutOffStepOne()
        {
           
            string Con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
          
            DBEngine objDb = new DBEngine();
            DateTime CutOffDate = YearEndingBL.GetCutOffDate();
            string CutOffDbname = YearEndingBL.GetCutOffDBName();

            DataTable dtCashBank = new DataTable();
            CashBankModel CBModel = new CashBankModel();

            JournalModel JNModel = new JournalModel();
          
            string ConDate = CutOffDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            ViewBag.CutoffDate = ConDate;
            TempData["CutOffDbname"] = CutOffDbname;
            TempData["Cutoffdate"] = CutOffDate;
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];

            DataTable UpdateStatus = objDb.GetDataTable("select Cutoff_Start,Cutoff_Status,Custoff_StartPage from " + MasterDbname + ".dbo.Master_YearEnding");

            DataTable UpdateStat = objDb.GetDataTable("select Custoff_StartPage from " + MasterDbname + ".dbo.Master_YearEnding");

            if ( Convert.ToInt32(UpdateStatus.Rows[0]["Cutoff_Status"])!=0  || Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"])!="")
            {

                if(Convert.ToInt32(UpdateStatus.Rows[0]["Cutoff_Status"])==1 )
                {
                    ViewBag.Message = "Cut off process started. Please visit after 30 minutes to see the status.";
                    ViewBag.CutOfStatus = "1";
                }
                else if(Convert.ToInt32(UpdateStatus.Rows[0]["Cutoff_Status"])==2 )
                {
                    ViewBag.Message = "Data Backup & Restoration process is completed.";
                    ViewBag.CutOfStatus = "2";
                }
                else if (Convert.ToInt32(UpdateStatus.Rows[0]["Cutoff_Status"]) == -1)
                {
                    ViewBag.Message = "Cut off process failed.";
                    ViewBag.CutOfStatus = "-1";
                }
                else
                {
                    ViewBag.Message = "Please complete previous cut off process first.";
                }

            }
            else
            {
                ViewBag.Message = "";
                ViewBag.CutOfStatus = "0";
            }

            if (Convert.ToString(UpdateStat.Rows[0]["Custoff_StartPage"]) == "Cashbank")
            {

                return View("~/Views/CutOff/Accounts/CashBankImport.cshtml", CBModel.GetCustomers(CutOffDate));
            }

            else if (Convert.ToString(UpdateStat.Rows[0]["Custoff_StartPage"]) == "Journal")
            {


                return View("~/Views/CutOff/Accounts/JournalImport.cshtml", JNModel.GetJournals(CutOffDate));
            }
            else if (Convert.ToString(UpdateStat.Rows[0]["Custoff_StartPage"]) == "Customer")
            {
                YearEndingBL yrBl = new YearEndingBL();
                DateTime dt = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = dt;
                return View("~/Views/CutOff/CustomerSales/CutoffCustomerSales.cshtml");
            }
            else if (Convert.ToString(UpdateStat.Rows[0]["Custoff_StartPage"]) == "Vendor")
            {
                YearEndingBL yrBl = new YearEndingBL();
                DateTime dt = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = dt;
                return View("~/Views/CutOff/VendorPurchase/CutoffVendorPurchase.cshtml");
            }
            else if (Convert.ToString(UpdateStat.Rows[0]["Custoff_StartPage"]) == "Stock")
            {
                YearEndingBL yrBl = new YearEndingBL();
                DateTime dt = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = dt;
                return View("~/Views/CutOff/StockValue/CutOffStockValue.cshtml");
            }
            else if (Convert.ToString(UpdateStat.Rows[0]["Custoff_StartPage"]) == "Ledger")
            {
                YearEndingBL yrBl = new YearEndingBL();
                DateTime dt = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = dt;
                return View("~/Views/CutOff/Accounts/CutoffLedger.cshtml");
            }
            //else if (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"]) == "SubLedger")
            //{
            //    YearEndingBL yrBl = new YearEndingBL();
            //    DateTime Con = yrBl.GetCutOffDate();
            //    ViewBag.CutoffDate = Con;
            //    return View("~/Views/CutOff/Accounts/SubLedgerCutoff.cshtml");
            //}

            else
            {
                return View("~/Views/CutOff/YearEnding/CutOffStepOne.cshtml");
            }
            //return View("~/Views/CutOff/YearEnding/CutOffStepOne.cshtml");

            }

        public ActionResult CutOffStepTwo(string cutoffDate, string backup)
        {
            string MasterDbname=ConfigurationSettings.AppSettings["MasterDBName"];

            TempData["cutoffDate"] = cutoffDate;
            TempData["backup"] = backup;


            if (TempData["cutoffDate"] != null)
            {
                ViewData["cutoffDate"] = TempData["cutoffDate"];
            }
            if (TempData["backup"] != null)
            {
                ViewData["backup"] = TempData["backup"];
            }
            TempData.Keep();

            if (backup == "Yes")
            {


                DBEngine objDb = new DBEngine();

                DataTable dtDoc = objDb.GetDataTable("select YearEnding_DbName,YearEnding_BackUpPath,YearEnding_AuditDb from " + MasterDbname + ".dbo.master_YearEnding");
                ViewData["backupPath"] = Convert.ToString(dtDoc.Rows[0]["YearEnding_BackUpPath"]);
                ViewData["DbName"] = Convert.ToString(dtDoc.Rows[0]["YearEnding_DbName"]);
                ViewData["AuditDbName"] = Convert.ToString(dtDoc.Rows[0]["YearEnding_AuditDb"]);

                return View("~/Views/CutOff/YearEnding/CutOffStepTwo.cshtml");
            }
            else
            {
                return RedirectToAction("CutOffStepThree");
            }
        }

        public ActionResult CutOffStepThree(string cutoffDate, string backup, string Dbname, string AuditDBName)
        {

            ViewData["CutoffDate"] = cutoffDate;
            ViewData["BackupDone"] = backup;

            ViewData["Dbname"] = Dbname;
            ViewData["AuditDBName"] = AuditDBName;

            return View("~/Views/CutOff/YearEnding/CutOffStepThree.cshtml");
        }

        public JsonResult StartBackup(string path, string Dbname, string Cutoffdate, string backup, string AuditDBName)
        {


            string response_msg = "";


            //Backup sqlBackup = new Backup();

            //sqlBackup.Action = BackupActionType.Database;
            //sqlBackup.BackupSetDescription = "ArchiveDataBase:" +
            //                                 DateTime.Now.ToShortDateString();
            //sqlBackup.BackupSetName = "Archive";

            //sqlBackup.Database = Dbname;


            //string _directoryName = @path + @"\" +DateTime.Now.ToString("ddMMyyyy");


            //if (!System.IO.Directory.Exists(_directoryName))
            //{
            //    System.IO.Directory.CreateDirectory(_directoryName);

            //}


            //BackupDeviceItem deviceItem = new BackupDeviceItem("C:\\18092018", DeviceType.File);
            //ServerConnection connection = new ServerConnection(@"10.0.8.251\MSSQLSERVERR2", "sa", "sql@123");
            //Server sqlServer = new Server(connection);

            //Database db = sqlServer.Databases[Dbname];

            //sqlBackup.Initialize = true;
            //sqlBackup.Checksum = true;
            //sqlBackup.ContinueAfterError = true;

            //sqlBackup.Devices.Add(deviceItem);
            //sqlBackup.Incremental = false;

            //sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
            //sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

            //sqlBackup.FormatMedia = false;

            //sqlBackup.SqlBackup(sqlServer);



            //ServerConnection srvConn = new ServerConnection();
            //srvConn.ConnectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            //srvr = new Server(srvConn);

            //string DBpath = @path + "\\" + DateTime.Now.ToString("ddMMyyyy");


            //if (!System.IO.Directory.Exists(DBpath))
            //{
            //    System.IO.Directory.CreateDirectory(DBpath);

            //}
            //DBEngine objDb = new DBEngine();
            //string BackUpFile = Dbname + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak";


            //try
            //{

            //    Backup bkpDatabase = new Backup();
            //    // Set the backup type to a database backup
            //    bkpDatabase.Action = BackupActionType.Database;
            //    // Set the database that we want to perform a backup on
            //    bkpDatabase.Database = Dbname;
            //    // Set the backup device to a file
            //    BackupDeviceItem bkpDevice = new BackupDeviceItem(DBpath + "\\" + BackUpFile, DeviceType.File);
            //    // Add the backup device to the backup
            //    bkpDatabase.Devices.Add(bkpDevice);
            //    // Perform the backup
            //    bkpDatabase.SqlBackup(srvr);




            //    int a = objDb.ExeInteger("update Master_YearEnding set YearEnding_BackUpDataBaseName='" + BackUpFile + "',YearEnding_BackUpDataPath='" + DBpath + "\\" + BackUpFile + "'");




            //    response_msg = "Backup Success";




            //}
            //catch(Exception e)
            //{
            //    response_msg = "Problem Occurs"+e.ToString();
            //}
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            DBEngine objDb = new DBEngine();
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            string _directoryName = Server.MapPath("/") + "\\" + DateTime.Now.ToString("ddMMyyyy");


            if (!System.IO.Directory.Exists(_directoryName))
            {
                System.IO.Directory.CreateDirectory(_directoryName);

            }

            con.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            AuditDBName = AuditDBName;

            try
            {
                string BackUpFile = Dbname + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak";
                con.Open();
                sqlcmd = new SqlCommand("backup database " + Dbname + " to disk='" + _directoryName + "\\" + BackUpFile + "'", con);
                sqlcmd.ExecuteNonQuery();
                con.Close();

                string BackUpFileLog = AuditDBName + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak";
                con.Open();
                sqlcmd = new SqlCommand("backup database " + AuditDBName + " to disk='" + _directoryName + "\\" + BackUpFileLog + "'", con);
                sqlcmd.ExecuteNonQuery();
                con.Close();



                int a = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding set YearEnding_BackUpLogDataBaseName='" + BackUpFileLog + "', YearEnding_BackUpDataBaseName='" + BackUpFile + "',YearEnding_BackUpDataPath='" + "" + DateTime.Now.ToString("ddMMyyyy") + "'");


                //Audit
                //sqlcmd = new SqlCommand("backup database " + AuditDBName + " to disk='" + _directoryName + "\\" + AuditDBName + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", con);
                //sqlcmd.ExecuteNonQuery();
                response_msg = "Backup database successfully";
            }
            catch (Exception ex)
            {
                response_msg = "Error Occured During DB backup process !<br>" + ex.ToString();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return Json(response_msg,JsonRequestBehavior.AllowGet);
        }

        public JsonResult StartDbCreate(string Cutoffdate, string Dbname, string AuditDbname)
        {
            String msg = "";
            //string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            //String str;
            //AuditDbname = AuditDbname;
            //SqlConnection myConn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //DBEngine objDb = new DBEngine();


            //DataTable dt = objDb.GetDataTable("Select YearEnding_LogFilePath,YearEnding_BackUpPath,YearEnding_BackUpDataBaseName,YearEnding_BackUpDataPath,YearEnding_BackUpPath,YearEnding_BackUpLogDataBaseName from " + MasterDbname + ".dbo.Master_YearEnding");

            //string BackUpIP = Convert.ToString(dt.Rows[0]["YearEnding_BackUpPath"]);
            //string LogPath = BackUpIP+"\\"+Server.MapPath("/") + Convert.ToString(dt.Rows[0]["YearEnding_LogFilePath"]);


            //string ActualLogPath =  Server.MapPath("/") + Convert.ToString(dt.Rows[0]["YearEnding_LogFilePath"]);

            //string BackUpFilename = Convert.ToString(dt.Rows[0]["YearEnding_BackUpDataBaseName"]);
            //string BackUpLogFilename = Convert.ToString(dt.Rows[0]["YearEnding_BackUpLogDataBaseName"]);

            //string BackUpDataPath = BackUpIP + "\\" + Server.MapPath("/") + Convert.ToString(dt.Rows[0]["YearEnding_BackUpDataPath"]) + "\\" + BackUpFilename;

            //string BackUpLogDataPath = BackUpIP + "\\" + Server.MapPath("/") + Convert.ToString(dt.Rows[0]["YearEnding_BackUpDataPath"]) + "\\" + BackUpLogFilename;


            //BackUpDataPath    = BackUpDataPath.Replace(":", "$");
            //BackUpLogDataPath = BackUpLogDataPath.Replace(":", "$");


            //LogPath = LogPath.Replace(":", "$");

            //if (!System.IO.Directory.Exists(ActualLogPath))
            //{
            //    System.IO.Directory.CreateDirectory(ActualLogPath);

            //}

            //SqlCommand myCommand;
            //try
            //{
            //    myConn.Open();
            //    str = "CREATE DATABASE " + Dbname + "_" + Cutoffdate.Replace("-", "").Trim();

            //    myCommand = new SqlCommand(str, myConn);               
            //    myCommand.ExecuteNonQuery();

            //    var a = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding set YearEnding_CutOffDBName='" + Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + "'");


            //    DateTime dtCutoff = DateTime.ParseExact(Cutoffdate,"dd-MM-yyyy",CultureInfo.InvariantCulture);

            //    var c = objDb.ExeInteger("update "+MasterDbname+".dbo.Master_YearEnding set YearEnding_Date='" + dtCutoff + "'");


            //}
            //catch (System.Exception ex)
            //{
                
            //}
            //finally
            //{
            //    if (myConn.State == ConnectionState.Open)
            //    {
            //        myConn.Close();
            //    }
            //}



            //try
            //{
            //    myConn.Open();
            //    str = "CREATE DATABASE " + AuditDbname + "_" + Cutoffdate.Replace("-", "").Trim();

            //    myCommand = new SqlCommand(str, myConn);
            //    myCommand.ExecuteNonQuery();

                
            //}
            //catch (System.Exception ex)
            //{

            //}
            //finally
            //{
            //    if (myConn.State == ConnectionState.Open)
            //    {
            //        myConn.Close();
            //    }
            //}



           
            //try
            //{



            //    ServerConnection srvConn = new ServerConnection();
            //    srvConn.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //    srvr = new Server(srvConn);

            //    string DBpath = @BackUpDataPath;
            //    Restore rstDatabase = new Restore();
            //    // Set the restore type to a database restore
            //    rstDatabase.Action = RestoreActionType.Database;
            //    // Set the database that we want to perform the restore on
            //    rstDatabase.Database = Dbname + "_" + Cutoffdate.Replace("-", "").Trim();
            //    // Set the backup device from which we want to restore, to a file
            //    BackupDeviceItem bkpDevice = new BackupDeviceItem(DBpath , DeviceType.File);
            //    // Add the backup device to the restore type
            //    rstDatabase.Devices.Add(bkpDevice);
            //    // If the database already exists, replace it
            //    rstDatabase.ReplaceDatabase = true;

            //    DataTable dtFileList = rstDatabase.ReadFileList(srvr);
            //    string dbLogicalName = dtFileList.Rows[0][0].ToString();
            //    string dbPhysicalName = dtFileList.Rows[0][1].ToString();
            //    string logLogicalName = dtFileList.Rows[1][0].ToString();
            //    string logPhysicalName = dtFileList.Rows[1][1].ToString();


            //    int pos = dbPhysicalName.LastIndexOf(@"\") + 1;
            //    dbPhysicalName = dbPhysicalName.Substring(0, pos);



            //    rstDatabase.RelocateFiles.Add(new RelocateFile(dbLogicalName, dbPhysicalName+Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + ".mdf"));
            //    rstDatabase.RelocateFiles.Add(new RelocateFile(logLogicalName, dbPhysicalName+Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + ".ldf"));


            //    // Perform the restore
            //    rstDatabase.SqlRestore(srvr);


                
            //     DateTime dtYearEndDate = DateTime.ParseExact(Cutoffdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            //    ReadWriteMasterDatabaseBL RWDB = new ReadWriteMasterDatabaseBL();

            //    DataTable companyname = objDb.GetDataTable("select top 1 cmp_name from " + Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + ".dbo.tbl_master_company");
            //    string company = "";

            //    if (companyname != null && companyname.Rows.Count > 0)
            //    {
            //        company = Convert.ToString(companyname.Rows[0]["cmp_name"]);
            //    }


            //    RWDB.WriteNewCompanyToMasterDatabase(company+" " + Cutoffdate.Trim(), Dbname + "_" + Cutoffdate.Replace("-", "").Trim(), Convert.ToDateTime(System.Web.HttpContext.Current.Session["FinYearStart"].ToString()), dtYearEndDate);

            //    msg = "Data Restore success.";



            //}
            //catch (Exception)
            //{
            //    throw;
            //    msg = "Data Restore probelm occurs.";
            //}
            //finally
            //{
            //    myConn.Dispose();
            //    myConn.Close();
            //}

            //try
            //{



            //    ServerConnection srvConn = new ServerConnection();
            //    srvConn.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //    srvr = new Server(srvConn);

            //    string DBpath = @BackUpLogDataPath;
            //    Restore rstDatabase = new Restore();
            //    // Set the restore type to a database restore
            //    rstDatabase.Action = RestoreActionType.Database;
            //    // Set the database that we want to perform the restore on
            //    rstDatabase.Database = AuditDbname + "_" + Cutoffdate.Replace("-", "").Trim();
            //    // Set the backup device from which we want to restore, to a file
            //    BackupDeviceItem bkpDevice = new BackupDeviceItem(DBpath, DeviceType.File);
            //    // Add the backup device to the restore type
            //    rstDatabase.Devices.Add(bkpDevice);
            //    // If the database already exists, replace it
            //    rstDatabase.ReplaceDatabase = true;

            //    DataTable dtFileList = rstDatabase.ReadFileList(srvr);
            //    string dbLogicalName = dtFileList.Rows[0][0].ToString();
            //    string dbPhysicalName = dtFileList.Rows[0][1].ToString();
            //    string logLogicalName = dtFileList.Rows[1][0].ToString();
            //    string logPhysicalName = dtFileList.Rows[1][1].ToString();


            //    int pos = dbPhysicalName.LastIndexOf(@"\") + 1;
            //    dbPhysicalName = dbPhysicalName.Substring(0, pos);



            //    rstDatabase.RelocateFiles.Add(new RelocateFile(dbLogicalName, dbPhysicalName + Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + "_Log.mdf"));
            //    rstDatabase.RelocateFiles.Add(new RelocateFile(logLogicalName, dbPhysicalName + Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + "_Log_Log.ldf"));


            //    // Perform the restore
            //    rstDatabase.SqlRestore(srvr);

            //    XmlReadWriteBL xmlbl = new XmlReadWriteBL();
            //    xmlbl.WriteNewCompanyToXml("", "");


            //    msg = "Data Restore success.";



            //}
            //catch (Exception)
            //{
            //    throw;
            //    msg = "Data Restore probelm occurs.";
            //}
            //finally
            //{
            //    myConn.Dispose();
            //    myConn.Close();
            //}



            //DateTime dtNewYearStartDate = DateTime.ParseExact(Cutoffdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //dtNewYearStartDate = dtNewYearStartDate.AddDays(1);

            //int updateStartdate = objDb.ExeInteger("update " + Dbname + ".dbo.Master_FinYear set FinYear_StartDate='" + dtNewYearStartDate + "'");

            //int b = objDb.ExeInteger("update " + Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + ".dbo.AUDITDB set Audit_Database='" + AuditDbname + "_" + Cutoffdate.Replace("-", "").Trim() + "'");

            //int cutoff = objDb.ExeInteger("update " + Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + ".dbo.master_finyear set FinYear_EndDate='" + dtNewYearStartDate + "'");

            //int CutoffMigration = objDb.ExeInteger("EXEC " + MasterDbname + ".dbo.PRC_USER_MIGRATION");

            //int Cutoff_frmwip = objDb.ExeInteger("update " + Dbname + "_" + Cutoffdate.Replace("-", "").Trim() + ".dbo.tbl_trans_menu set mnu_menuLink='frmwip.aspx' where mnu_menuName='Year Ending'");
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CutOffStepFour()
        {
            
            return View("~/Views/CutOff/YearEnding/CutOffStepFour.cshtml");
        }


      
        public JsonResult StartCutoffProcess(DateTime Cutoffdate)
        {
            String msg = "Cut off process started. Please visit after 30 minutes to see the status.";
            DBEngine objDb = new DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];

            int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Cutoff_Start=1,YearEnding_CutOffDate='" + Cutoffdate.ToString("yyy-MM-dd") + "',YearEnding_Date='" + Cutoffdate.ToString("yyy-MM-dd") + "',Cutoff_Status=1,Custoff_StartPage='Restore'");

            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EndCutoffProcess()
        {
            //String msg = "Cut off process started. Please visit after 30 minutes to see the status.";
            String msg = "Successfully Updated.";
            DBEngine objDb = new DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Cutoff_Status=0,Custoff_StartPage='Ledger'");
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NegativeStockProcess(DateTime Cutoffdate)
        {
            NegativeStocksms NegativeStocksms = new NegativeStocksms();
            string returnValue = string.Empty;
            string returntext = string.Empty;
            YearEndingBL YrdBL = new YearEndingBL();
            YrdBL.NegStockProcess(ref returnValue, ref returntext, Cutoffdate);
            NegativeStocksms.ReturnValue = returnValue;
            NegativeStocksms.ReturnText = returntext;
            TempData["StockValue"] = returnValue;
            TempData.Keep("StockValue");
            return Json(NegativeStocksms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PendingCutoffProcess(DateTime Cutoffdate)
        {
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            Pendingsms sms = new Pendingsms();
            string returnValue = string.Empty;
            string returntext = string.Empty;
            YearEndingBL yrendBL = new YearEndingBL();
            yrendBL.PendingCutOffProcess(ref returnValue, ref returntext, Cutoffdate);
            sms.ReturnValue = returnValue;
            sms.ReturnText = returntext;
            TempData["ReturnValue"] = returnValue;
            TempData.Keep("ReturnValue");
            return Json(sms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataBackUpCheck()
        {

            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            DBEngine objDb = new DBEngine();
            DataTable UpdateStatus = objDb.GetDataTable("select Cutoff_Start,Cutoff_Status,Custoff_StartPage from " + MasterDbname + ".dbo.Master_YearEnding");
            BackUpSms BackUpSms = new BackUpSms();
           
            if ((Convert.ToInt32(UpdateStatus.Rows[0]["Cutoff_Status"]) == 2) && (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"]) == "Restore"))
            {
                ViewBag.Message = "Data Backup & Restoration process is completed.";
                BackUpSms.ReturnSMS = ViewBag.Message;
            }

            else
            {
                ViewBag.Message = "";
                BackUpSms.ReturnSMS = ViewBag.Message;
            }

            return Json(BackUpSms.ReturnSMS, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPendingDocument(DateTime? Cutoffdate)
        
        {

            
            
            YearEndingBL YearEndingBL = new YearEndingBL();

            DataTable dt = new DataTable();
            PendingDocView PendingDocView = new PendingDocView();
            List<pendingDocument> Doclist = new List<pendingDocument>();
            int no=Convert.ToInt32(TempData["ReturnValue"]);
            TempData.Keep("ReturnValue");
            if (no == 2)
            {
                dt = YearEndingBL.PendingSalesDocumentList(Cutoffdate);
            }
            else if (no == 3)
            {
                dt = YearEndingBL.PendingPurchaseDocumentList(Cutoffdate);
            }

            if (dt != null)
            {
                Doclist = APIHelperMethods.ToModelList<pendingDocument>(dt);
                //TempData["ExportResourceMaster"] = omel;
                //TempData.Keep();
            }
            PendingDocView.pendingDocument = Doclist;
            TempData["data"] = Doclist;
            TempData.Keep("data");
           // return PartialView(PendingDocView);
            return PartialView("~/Views/CutOff/YearEnding/_PendingDocRender.cshtml", PendingDocView);
        }

        public ActionResult BindPendingDocument(string Cutoffdate)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            DateTime dtDate = jsonSerialiser.Deserialize<DateTime>(Cutoffdate);

            YearEndingBL YearEndingBL = new YearEndingBL();

            DataTable dt = new DataTable();
            PendingDocView PendingDocView = new PendingDocView();
            List<pendingDocument> Doclist = new List<pendingDocument>();

            int no = Convert.ToInt32(TempData["ReturnValue"]);
            TempData.Keep("ReturnValue");
            if (no == 2)
            {
                dt = YearEndingBL.PendingSalesDocumentList(dtDate);
            }
            else if (no == 3)
            {
                dt = YearEndingBL.PendingPurchaseDocumentList(dtDate);
            }
            if (dt != null)
            {
                Doclist = APIHelperMethods.ToModelList<pendingDocument>(dt);
                //TempData["ExportResourceMaster"] = omel;
                //TempData.Keep();
            }
            PendingDocView.pendingDocument = Doclist;
            TempData["data"] = Doclist;
            TempData.Keep("data");
            return PartialView("~/Views/CutOff/YearEnding/_PendingDocumentPartial.cshtml", PendingDocView);
        }

        public ActionResult NegativeProductList(DateTime? Cutoffdate)
        {



            YearEndingBL YearEndingBL = new YearEndingBL();

            DataTable dt = new DataTable();
            PendingDocView PendingDocView = new PendingDocView();
            List<NegativeProduct> Doclist = new List<NegativeProduct>();
            int no = Convert.ToInt32(TempData["StockValue"]);
            TempData.Keep("StockValue");
            if (no == 1)
            {
                dt = YearEndingBL.PendingNegativeProductList(Cutoffdate);
            }
          

            if (dt != null)
            {
                Doclist = APIHelperMethods.ToModelList<NegativeProduct>(dt);
                //TempData["ExportResourceMaster"] = omel;
                //TempData.Keep();
            }
            PendingDocView.NegativeProduct = Doclist;
            TempData["NegativeProduct"] = Doclist;
            TempData.Keep("NegativeProduct");
            // return PartialView(PendingDocView);
            return PartialView("~/Views/CutOff/YearEnding/_NegativeProductRender.cshtml", PendingDocView);
        }

        public ActionResult BindNegativeProduct(string Cutoffdate)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            DateTime dtDate = jsonSerialiser.Deserialize<DateTime>(Cutoffdate);

            YearEndingBL YearEndingBL = new YearEndingBL();

            DataTable dt = new DataTable();
            PendingDocView PendingDocView = new PendingDocView();
            List<NegativeProduct> Doclist = new List<NegativeProduct>();

            int no = Convert.ToInt32(TempData["StockValue"]);
            TempData.Keep("StockValue");
            if (no == 1)
            {
                dt = YearEndingBL.PendingNegativeProductList(dtDate);
            }
           
            if (dt != null)
            {
                Doclist = APIHelperMethods.ToModelList<NegativeProduct>(dt);
                //TempData["ExportResourceMaster"] = omel;
                //TempData.Keep();
            }
            PendingDocView.NegativeProduct = Doclist;
            TempData["NegativeProduct"] = Doclist;
            TempData.Keep("NegativeProduct");
            return PartialView("~/Views/CutOff/YearEnding/_NegativeProductPartial.cshtml", PendingDocView);
        }
        public ActionResult ExportPendingDoclist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["data"] = TempData["data"];
            TempData.Keep();

            if (ViewData["data"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetRoleGridViewSettings(), ViewData["data"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetRoleGridViewSettings(), ViewData["data"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetRoleGridViewSettings(), ViewData["data"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetRoleGridViewSettings(), ViewData["data"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetRoleGridViewSettings(), ViewData["data"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetRoleGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Pending Document List";
        
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Pending Document List";

            settings.Columns.Add(column =>
            {
                column.Caption = "Document Number";
                column.FieldName = "DocumentNo";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Branch";
                column.FieldName = "Branch";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Posting Date";
                column.FieldName = "Date";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Name";
                column.FieldName = "Name";
            });


            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }


        public ActionResult ExportNegativeProduct(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["NegativeProduct"] = TempData["NegativeProduct"];
            TempData.Keep();

            if (ViewData["NegativeProduct"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetNegativeProductList(), ViewData["NegativeProduct"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetNegativeProductList(), ViewData["NegativeProduct"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetNegativeProductList(), ViewData["NegativeProduct"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetNegativeProductList(), ViewData["NegativeProduct"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetNegativeProductList(), ViewData["NegativeProduct"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetNegativeProductList()
        {
            var settings = new GridViewSettings();
            settings.Name = "Pending Negative Product List";

            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Pending Negative Product List";

            settings.Columns.Add(column =>
            {
                column.Caption = "Product Name";
                column.FieldName = "Product";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Branch";
                column.FieldName = "Branch";

            });

           
            settings.Columns.Add(column =>
            {
                column.Caption = "Quantity";
                column.FieldName = "Quantity";
            });


            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }


        public class Pendingsms
        {
            public string ReturnValue { get; set; }
            public string ReturnText { get; set; }
        }
        public class NegativeStocksms
        {
            public string ReturnValue { get; set; }
            public string ReturnText { get; set; }
        }
        public class BackUpSms
        {
            public string ReturnSMS { get; set; }
        }
        //public class PendingDocView
        //{
        //    public string DocumentNo { get; set; }
        //    public string Branch { get; set; }
        //    public DateTime? Date { get; set; }
        //    public String Name { get; set; }
           
        //}
	}

    
}