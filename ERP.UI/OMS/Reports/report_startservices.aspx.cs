using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace ERP.OMS.Reports
{
    public partial class Reports_report_startservices : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        string provider = "RSAProtectedConfigurationProvider";
        string section = "appSettings";
        string appnameshow = "";
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {


            //Page.ClientScript.RegisterStartupScript(GetType(), "autoStartTimer", "<script>autoStartTimer();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            // Page.ClientScript.RegisterStartupScript(GetType(), "CreateFile", "<script>CreateFile();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Process[] processes = Process.GetProcessesByName("influxEmail");

                if (processes.Length != 0)
                {
                    string connection = "";
                    string CurrentMachineName = Environment.MachineName;
                    CurrentMachineName = @"\\" + CurrentMachineName + @"\emailconnection\web.txt";
                    string Path = CurrentMachineName;
                    string lines = System.IO.File.ReadAllText(Path);
                    lines = Encoding.Unicode.GetString(Convert.FromBase64String(lines));
                    connection = lines.ToString().Trim();
                    string[] testpath = connection.Split(';');
                    string[] serverpath = testpath[1].Split('=');
                    string show = serverpath[1].Trim();

                    string current = ConfigurationManager.AppSettings["DBConnectionDefault"];
                    string[] currentserver = current.Split(';');
                    string[] two = currentserver[1].Split('=');
                    string final = two[1].Trim();
                    td_yes.Visible = false;
                    tr_time.Visible = false;
                    //DataTable dtshow = new DataTable();
                    //dtshow = oDBEngine.GetDataTable("select top 1 appprocdetail_appname from trans_appprocdetail where appprocdetail_appname is not null ");
                    //appnameshow = dtshow.Rows[0]["appprocdetail_appname"].ToString().Trim();
                    if (final.ToString().Trim().ToUpper() == show.ToString().Trim().ToUpper())
                    {
                        td_no.Visible = true;
                    }
                    else
                    {
                        td_no.Visible = false;
                        td_service.Visible = false;
                    }
                    //litTimerLabels.Visible = true;
                    // Page.ClientScript.RegisterStartupScript(GetType(), "playTimer", "<script>playTimer(this);</script>");


                    Page.ClientScript.RegisterStartupScript(GetType(), "ShowOnLoad", "<script language='javascript'>alert('Process is Activated from " + show + "');</script>");
                    //                 Page.ClientScript.RegisterStartupScript(Type.GetType
                    //("System.String"), "addScript", "PassValues('" + appnameshow + "')", true);
                    //}

                }
                else
                {
                    td_no.Visible = false;
                    td_yes.Visible = true;
                    tr_time.Visible = true;
                    //litTimerLabels.Visible = false;
                }

            }
        }


        //private static readonly Mutex 
        protected void btnNo_Click(object sender, EventArgs e)
        {
            //Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            //ConfigurationSection confStrSect = confg.GetSection(section);
            //if (confStrSect != null && confStrSect.SectionInformation.IsProtected)
            //{
            //    confStrSect.SectionInformation.UnprotectSection();
            //    confg.Save();
            //}
            Process[] processes = Process.GetProcessesByName("influxEmail");

            foreach (Process process in processes)
            {
                process.Kill();
            }
            Process[] processesread = Process.GetProcessesByName("ReadEmail");

            foreach (Process process in processesread)
            {
                process.Kill();
            }
            int noofrows = oDBEngine.SetFieldValue("trans_appprocdetail", "appprocdetail_enddatetime='" + oDBEngine.GetDate().ToString() + "'", "appprocdetail_enddatetime is null");

            Page.ClientScript.RegisterStartupScript(GetType(), "JScript321", "<script language='javascript'>Page_Load();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript341", "<script language='javascript'>alert('Service Stopped Succesfully');</script>");
            td_no.Visible = false;
            td_yes.Visible = true;
            tr_time.Visible = true;
            //litTimerLabels.Visible = false;


        }
        //private int GetSecondsLeft()
        //{
        //    int formSecondsLeft = Convert.ToInt32(Session["time"]);

        //    if (Request.Form["timerval"] != null &&
        //        int.TryParse(Request.Form["timerval"], 
        //        //out Convert.ToInt32( Session["time"])))
        //        out formSecondsLeft))

        //    {
        //        // Keep in mind, that this can be a negative number if more than 3600 seconds elapsed!
        //        return formSecondsLeft;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        protected void btnYes_Click(object sender, EventArgs e)
        {
            //Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            //ConfigurationSection confStrSect = confg.GetSection(section);
            //if (confStrSect != null && confStrSect.SectionInformation.IsProtected)
            //{
            //    confStrSect.SectionInformation.UnprotectSection();
            //    confg.Save();
            //}
            //Page.ClientScript.RegisterStartupScript(GetType(), "autoStartTimer", "<script>autoStartTimer();</script>");
            Process[] processes = Process.GetProcessesByName("influxEmail");

            if (processes.Length == 0)
            {
                string connection = "";
                string AttachPath = "";
                string path = HttpContext.Current.Server.MapPath("..\\influxEmail.exe");
                string pathforread = HttpContext.Current.Server.MapPath("..\\ReadEmail.exe");
                string path9 = HttpContext.Current.Server.MapPath("..\\web.txt");
                //string test = path.Substring(0, 3);
                string testDir = "c:\\emailconnection";
                string testFile = "web.txt";

                //test = test + @"web.txt";
                //string test1 = "c:\\emailconnection\\web1.txt" + @"web1.txt";
                string test1Dir = "c:\\emailconnection";
                string test1File = "web1.txt";

                //String UploadPath=HttpContext.Current.Server.MapPath("..\\emailconnection\\web.txt");
                //String UploadPath1 = HttpContext.Current.Server.MapPath("..\\emailconnection\\web1.txt");
                //String UploadPath = ((ConfigurationManager.AppSettings["SaveCSVsql"].ToString() + "web.txt"));
                //String UploadPath1 = ((ConfigurationManager.AppSettings["SaveCSVsql"].ToString() + "web1.txt"));
                String UploadPath = testDir.ToString().Trim() + "\\" + testFile.ToString().Trim();
                String UploadPath1 = test1Dir.ToString().Trim() + "\\" + test1File.ToString().Trim();
                //string lines = System.IO.File.ReadAllText(path9);
                string[] lines = System.IO.File.ReadAllLines(path9);
                //StreamReader fp;
                //string sData = null;

                //fp = File.OpenText(path9);
                //sData = fp.ReadToEnd();
                //fp.Close();
                //Response.Write(sData);


                //Display the file contents by using a foreach loop.
                foreach (string line in lines)
                {
                    if (line.Contains("DBConnectionDefault"))
                    {
                        connection = line.Substring(line.IndexOf("value="), line.Length - line.IndexOf("value="));

                        connection = connection.Replace("/>", "");
                        connection = connection.Replace("value=", "");//.Replace(@"\", @"\\");
                        connection = connection.Replace("\"", "");
                    }
                    if (line.Contains("CRMfolderNameWithPath"))
                    {
                        AttachPath = line.Substring(line.IndexOf("value="), line.Length - line.IndexOf("value="));
                        //ConnectionString = "value=Data Source=Subhadeep\\SQL2008R2;Initial Catalog=inaka_subhadeep;User ID=sa; Password=sap@123 />";
                        AttachPath = AttachPath.Replace("/>", "").Replace("value=", "").Replace("\"", "");
                    }
                    //System.IO.File.WriteAllText(test, connection);
                    //string[] cnn = connection.Split('=');
                    //string[] cnn2 = cnn[2].Split(';');
                    //string[]  cnn3 = cnn[3].Split(';');
                    //string[] cnn4 = cnn[4].Split(';');
                    //string cnn5 = cnn[5];
                    //string content = cnn[1] +"="+cnn2[0] + "," + cnn3[0] + "," + cnn4[0] + "," + cnn5;
                    //////////////string content = connection.ToString().Trim() + AttachPath.ToString().Trim();;
                    //////////////content = Convert.ToBase64String(Encoding.Unicode.GetBytes(content));
                    //////////////System.IO.File.WriteAllText(test, content);
                    //string linesss = System.IO.File.ReadAllText(test);
                    //linesss = Encoding.Unicode.GetString(Convert.FromBase64String(linesss));
                    //content = Encoding.Unicode.GetString(Convert.FromBase64String(content));
                    //for (int i = 0; i < connection.Length; i++)
                    //{


                    //}

                }
                //string content = connection.ToString().Trim() + System.Environment.NewLine + AttachPath.ToString().Trim();
                string content = connection.ToString().Trim();
                string attachment = AttachPath.ToString().Trim();
                attachment = Convert.ToBase64String(Encoding.Unicode.GetBytes(attachment));
                content = Convert.ToBase64String(Encoding.Unicode.GetBytes(content));

                if (!System.IO.Directory.Exists(testDir))
                    System.IO.Directory.CreateDirectory(testDir);
                if (!System.IO.Directory.Exists(test1Dir))
                    System.IO.Directory.CreateDirectory(test1Dir);

                System.IO.File.WriteAllText(UploadPath, content);
                System.IO.File.WriteAllText(UploadPath1, attachment);
                //Session["deletepath"] = "//subhadeep/influx/";
                //hdnLocationPath.Value = "//subhadeep/influx/web.txt";
                // }
                //string pwed = base64Encode(lines);
                //StreamReader fp;
                //fp = File.OpenText(pwed);
                //sData = fp.ReadToEnd();
                //fp.Close();
                //Response.Write(test);
                //string path2 = @"E:\\web.config";
                //File.Copy(path9, test,true);
                //string path = (@"\\Subhadeep\\CommonFolderInfluxCRM\\LogFiles\\influxEmail.exe");
                //string path = ("C:\\InfluxEmail\\influxEmail.exe");
                //string path = (@"\\LocalServer\CommonFolderInfluxCRM\LogFiles\influxEmail.exe");
                string[] testpath = connection.Split(';');
                string[] serverpath = testpath[1].Split('=');
                string show = serverpath[1];
                try
                {
                    Process process = Process.Start(path);
                    int id = process.Id;
                    int noofrows = 0;
                    //if (ddlservices.SelectedItem.Value != "12345678")
                    //{
                    noofrows = oDBEngine.SetFieldValue("trans_appprocdetail", "appprocdetail_enddatetime='" + oDBEngine.GetDate().ToString() + "'", "appprocdetail_enddatetime is null");
                    noofrows = oDBEngine.InsurtFieldValue("trans_appprocdetail", "appprocdetail_mainid,appprocdetail_processid,appprocdetail_startdatetime,appprocdetail_userid,appprocdetail_timeinterval", "'" + ddlservices.SelectedItem.Value + "','" + id + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString().Trim() + "','" + DdlRptType.SelectedItem.Value.ToString().Trim() + "'");
                    Process.Start(pathforread);
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript32", "<script language='javascript'>Page_Load();</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript34", "<script language='javascript'>alert('Service Started Succesfully');</script>");
                    td_no.Visible = true;
                    td_yes.Visible = false;
                    tr_time.Visible = false;

                }
                catch (Exception)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript11234", "<script language='javascript'>alert('Your exe is Corrupt.Please change the exe !');</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript5532", "<script language='javascript'>Page_Load();</script>");
                    // throw;
                }
                //Process process = Process.Start(path);
                //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //int id = process.Id;
                //Process id1 = Process.GetProcessById(id);
                //int noofrows = 0;
                ////if (ddlservices.SelectedItem.Value != "12345678")
                ////{
                //    noofrows = oDBEngine.SetFieldValue("trans_appprocdetail", "appprocdetail_enddatetime='" + oDBEngine.GetDate().ToString() + "'", "appprocdetail_enddatetime is null");
                //    noofrows = oDBEngine.InsurtFieldValue("trans_appprocdetail", "appprocdetail_mainid,appprocdetail_processid,appprocdetail_startdatetime,appprocdetail_userid,appprocdetail_timeinterval", "'" + ddlservices.SelectedItem.Value + "','" + id + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString().Trim() + "','" + DdlRptType.SelectedItem.Value.ToString().Trim() + "'");

                //}
                //else
                //{
                //    noofrows = oDBEngine.InsurtFieldValue("trans_appprocdetail", "appprocdetail_mainid,appprocdetail_processid,appprocdetail_startdatetime,appprocdetail_userid,appprocdetail_timeinterval", "'" + ddlservices.SelectedItem.Value + "','" + id + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString().Trim() + "','" + DdlRptType.SelectedItem.Value.ToString().Trim() + "'");
                //}

                /////////////////ENCRYPTION//////////////////////

                //Configuration confg = WebConfigurationManager.OpenWebConfiguration(path);
                //Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                //ConfigurationSection confStrSect = confg.GetSection(section);


                //if (confStrSect != null)
                //{
                //    confStrSect.SectionInformation.ProtectSection(provider);
                //    confg.SaveAs("web.txt");
                //}
                //// the encrypted section is automatically decrypted!!
                //    Response.Write("Configuration Section " + "<b>" +
                //    WebConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString + "</b>" + " is automatically decrypted");




                ////////////////////////////////////////////////

                //    if (DdlRptType.SelectedItem.Value == "300000")
                //    {
                //        Session["time"] = "300";
                //    }
                //    if (DdlRptType.SelectedItem.Value == "600000")
                //    {
                //        Session["time"] = "600";
                //    }
                //    if (DdlRptType.SelectedItem.Value == "1200000")
                //    {
                //        Session["time"] = "1200";
                //    }
                //    if (DdlRptType.SelectedItem.Value == "1800000")
                //    {
                //        Session["time"] = "1800";
                //    }
                //    if (DdlRptType.SelectedItem.Value == "3600000")
                //    {
                //        Session["time"] = "3600";
                //    }
                //    if (DdlRptType.SelectedItem.Value == "7200000")
                //    {
                //        Session["time"] = "7200";
                //    }
                //    if (DdlRptType.SelectedItem.Value == "18000000")
                //    {
                //        Session["time"] = "18000";
                //    }
                //string[,] date1=oDBEngine.GetFieldValue("trans_appprocdetail","appprocdetail_startdatetime","appprocdetail_enddatetime is null",1);
                //string[,] date2 = oDBEngine.GetFieldValue("trans_appprocdetail", "appprocdetail_timeinterval", "appprocdetail_enddatetime is null", 1);

                //    //TimeSpan t1 = date1[0, 0]; 
                //    //TimeSpan t2 = date2[0, 0];
                //    int secondsLeft = GetSecondsLeft();
                //Process process = Process.Start("E:\\EmaiserviceFullVersion23022011\\NewEmailService\\influxEmail\\influxEmail\\bin\\Debug\\influxEmail.exe");
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript32", "<script language='javascript'>Page_Load();</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript34", "<script language='javascript'>alert('Service Started Succesfully');</script>");
                //td_no.Visible = true;
                //td_yes.Visible = false;
                //tr_time.Visible = false;
                //litTimerLabels.Visible = true;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript232", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript234", "<script language='javascript'>alert('Service is Already Running');</script>");
            }
        }

    }

}