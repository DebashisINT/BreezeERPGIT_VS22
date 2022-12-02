using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
//using ExportCSV.Classes;
using BusinessLogicLayer;
using ExportCSV.Classes;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_frmCategory : ERP.OMS.ViewState_class.VSPage
    {
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        public string[] InputName2 = new string[46];
        public string[] InputType2 = new string[46];
        public string[] InputValue2 = new string[46];
        string trade_date, TableName, t1, path, UploadPath1, bb, fileDate = "";
        string[] arrobj = new string[2];
        string[] farr = new string[2];
        string[] srrd = new string[4];
        string[] yy1 = new string[4];
        FileInfo FFIXCSV = null;
        FileInfo securitytxt = null;
        DataTable dt1 = new DataTable();
        FileInfo IXCSV = null;
        string[] arr;
        DataTable dt2 = new DataTable();
        String UploadPath;
        DataTable dt = new DataTable();
        int k = 0;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        string[,] strdate = null;
        string stringdate = "";
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "height();", true);
            }
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                String FilePath = Path.GetFullPath(brSelectFile.PostedFile.FileName);


                String FileName = Path.GetFileName(brSelectFile.PostedFile.FileName);
                UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                brSelectFile.PostedFile.SaveAs(UploadPath);

                FileInfo FICSV = new FileInfo(UploadPath);
                string path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                FFIXCSV = new FileInfo(UploadPath);


                File.Copy(UploadPath, path, true);


                ClsSQLCSV objSQL = new ClsSQLCSV();
               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                //if (ddlFileList.SelectedValue == "5")
                //{
                DataTable dtbl = null;

                if (Session["userid"] != null)
                {
                    InputName[0] = "Module";
                    InputName[1] = "ModifyUser";
                    InputName[2] = "ExcSegmt";
                    InputName[3] = "FilePath";
                    InputName[4] = "SecurityCategory_CompanyID";

                    InputType[0] = "V";
                    InputType[1] = "V";
                    InputType[2] = "I";
                    InputType[3] = "V";
                    InputType[4] = "C";

                    InputValue[0] = "InsertTradeData";
                    InputValue[1] = Session["userid"].ToString();
                    InputValue[2] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                    InputValue[3] = path.ToString().Trim();
                    InputValue[4] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                    dtbl = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("[SP_INSUP_TRANSSECURITYCATEGORY]", InputName, InputType, InputValue);
                    if (dtbl.Rows[0][0].ToString() != "0")
                    {
                        importstatus.Text = "File " + FileName + " Imported Successfully";
                    }
                }

                if (File.Exists(UploadPath))
                {
                    File.Delete(UploadPath);
                }
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                importstatus.Text = ex.Message.ToString() + "<br>";
                importstatus.Text += "Error importing. Please try again";
            }

        }
    }

}