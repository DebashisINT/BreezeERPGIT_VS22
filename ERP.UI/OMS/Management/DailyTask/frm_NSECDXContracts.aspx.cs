using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.DailyTask
{
    public partial class Management_DailyTask_frm_NSECDXContracts : ERP.OMS.ViewState_class.VSPage
    {
        FileInfo FIICXCSV = null;
        string TableName, t1;
        FileInfo securitytxt = null;
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        DataTable dt1 = new DataTable();


        string[] lineSplit;
        string BenAccountNumber, dpId, date, isin, settlementId;
        private static String path, path1, FileName, s, pathHolding, pathRemat, time, cannotParse;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MCXSelectFile.FileContent.Length != 0)
                {
                    path = String.Empty;
                    path1 = String.Empty;
                    FileName = String.Empty;
                    s = String.Empty;
                    time = String.Empty;
                    cannotParse = String.Empty;

                    TransctionDescription td = new TransctionDescription();

                    String FilePath = Path.GetFullPath(MCXSelectFile.PostedFile.FileName);
                    FileName = Path.GetFileName(MCXSelectFile.PostedFile.FileName);
                    String UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                    MCXSelectFile.PostedFile.SaveAs(UploadPath);

                    FileInfo FICSV = new FileInfo(UploadPath);
                    path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                    FIICXCSV = new FileInfo(UploadPath);
                    File.Copy(UploadPath, path, true);

                    ClearArray();
                 //   DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                    DBEngine oDBEngine = new DBEngine();


                    InputName[0] = "Module";
                    InputName[2] = "ModifyUser";
                    InputName[1] = "FilePath";

                    InputType[0] = "V";
                    InputType[2] = "I";
                    InputType[1] = "V";

                    InputValue[0] = "InsertNSECDXCSV";
                    InputValue[2] = Session["userid"].ToString();
                    InputValue[1] = path.ToString().Trim();
                    //DataTable dt2 = SQLProcedures.SelectProcedureArr("[SP_INSUP_MCXCDXContractsCSVCheck]", InputName, InputType, InputValue);

                    //if (dt2.Rows[0][0].ToString() != "0")
                    //{
                    //    DataTable dt1 = SQLProcedures.SelectProcedureArr("[SP_INSUP_MCXCDXContractsCSVModified]", InputName, InputType, InputValue);
                    //}
                    //else
                    //{
                    DataTable dt1 = SQLProcedures.SelectProcedureArr("[SP_INSUP_NSECDXContractsCSV]", InputName, InputType, InputValue);
                    //}


                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Import Successfully!');</script>");

                    if (File.Exists(UploadPath))
                    {
                        File.Delete(UploadPath);
                    }
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                }
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Selected File Cannot Be Blank!');</script>");
            }


            catch (Exception ex)
            {

            }
            finally
            {

            }

        }
        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }


    }
}
