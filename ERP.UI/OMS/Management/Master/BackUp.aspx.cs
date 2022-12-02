using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using System.IO;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class BackUp : System.Web.UI.Page
    {
        SqlCommand cmd;
        SqlDataReader dr;
        SqlConnection _conn;
        protected void Page_Load(object sender, EventArgs e)
        {

            string userInput = ConfigurationManager.AppSettings["BackupTime"];
            var time = TimeSpan.Parse(userInput);
            var dateTime = DateTime.Today.Add(time);

            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, dateTime.TimeOfDay) != -1)
            {
                if (Convert.ToString(Session["UserName"]).ToUpper() == "ADMIN")
                {
                    if (!Page.IsPostBack)
                    {

                        System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Backup"));

                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }

                        try
                        {
                            DataBaseClass dbc = new DataBaseClass();
                            // select *  from sys.servers getting server names that exist
                            cmd = new SqlCommand("select *  from sys.servers", dbc.openconn());
                            dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                ListItem lst = new ListItem(dr[1].ToString());
                                cbservername.Items.Add(lst);
                            }
                            dr.Close();
                        }
                        catch (Exception ex)
                        {
                            //
                        }
                        Createconnection();
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "onpageredirect('You have to be admin to access this page')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "onpageredirect('Can not take back up right now . Please try after " + userInput + ".')", true);
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass dbc = new DataBaseClass();
                _conn = dbc.openconn();
                if (_conn == null)
                    return;

                string path = Server.MapPath("~/Backup");


                bool exist = System.IO.Directory.Exists(Server.MapPath("~/Backup"));

                if (!exist)
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Backup"));


                SqlCommand _command = new SqlCommand();
                _command.Connection = _conn;
                string _dbname = cbdatabasename.SelectedItem.Text;

                // nice filename on local side, so we know when backup was done
                string fileName = _dbname + DateTime.Now.Year.ToString() + "-" +
                    DateTime.Now.Month.ToString() + "-" +
                    DateTime.Now.Day.ToString() + "-" +
                    DateTime.Now.Millisecond.ToString() + ".bak";
                // we invoke this method to ensure we didnt mess up with other programs


                string _sql;

                _sql = String.Format("BACKUP DATABASE {0} TO DISK = N'{1}\\{0}.bak' WITH FORMAT, COPY_ONLY, INIT, NAME = N'{0} - Full Database Backup', SKIP ", _dbname, path, fileName);
                _command.CommandText = _sql;
                _command.CommandTimeout = 9000000;
                _command.ExecuteNonQuery();

                SqlConnection con = _conn;
                // con.ConnectionString = _conn;

                string backupDIR = Server.MapPath("");

                //try
                //{
                // con.Open();
                //cmd = new SqlCommand("backup database " + _dbname + " to disk='" + backupDIR + "\\" + _dbname + ".Bak' WITH COMPRESSION", con);
                //cmd.CommandTimeout = 0;
                //cmd.ExecuteNonQuery();
                //con.Close();

                //}
                //catch (Exception ex)
                //{
                //}


                string DateTimes = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss tt");

                bool exists = System.IO.Directory.Exists(Path.Combine(path, DateTimes));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Backup"), DateTimes));



                string remoteUri = "http://3.7.30.86:85/";
                string myStringWebResource = null;
                // Create a new WebClient instance.
                //WebClient myWebClient = new WebClient();
                // Concatenate the domain with the Web resource filename.
                myStringWebResource = remoteUri + _dbname;

                System.IO.File.Move(Path.Combine(path, _dbname + ".BAK"), Path.Combine(path, DateTimes, _dbname + ".BAK"));




                string startPath = Path.Combine(path, DateTimes);
                string zipPath = Path.Combine(path, _dbname + DateTimes + ".ZIP");


                ZipFile.CreateFromDirectory(startPath, zipPath);

                var dir = new DirectoryInfo(Path.Combine(Server.MapPath("~/Backup"), DateTimes));
                dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                dir.Delete(true);

                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + _dbname + ".zip");
                Response.TransmitFile(Path.Combine(path, _dbname + DateTimes + ".ZIP")); //backup must be located in folder in your application folder, that folder named *backups*
                Response.End();




            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Createconnection()
        {
            DataBaseClass dbc = new DataBaseClass();

            cbdatabasename.Items.Clear();
            // select * from sys.databases getting all database name from sql server 

            cmd = new SqlCommand("select * from AuDitDB", dbc.openconn());
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListItem lst = new ListItem(dr[0].ToString());
                cbdatabasename.Items.Add(lst);

                string CS = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                //string CS = "DSN=raideit; UID=sa; Pwd=1234";
                SqlConnection con = new SqlConnection(CS);

                ListItem lst1 = new ListItem(con.Database);
                cbdatabasename.Items.Add(lst1);

            }
            dr.Close();


        }


        public void query(string que)
        {


            DataBaseClass dbc = new DataBaseClass();
            cmd = new SqlCommand(que, dbc.openconn());

            cmd.ExecuteNonQuery();



        }

        public void newquery(string que)
        {


            DataBaseClass dbc = new DataBaseClass();
            cmd = new SqlCommand(que, dbc.openconn());
            cmd.CommandTimeout = 50;
            cmd.ExecuteNonQuery();
        }
        // Backup Database


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
}