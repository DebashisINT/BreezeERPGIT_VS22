using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_frm_NSECDXClosingRates : ERP.OMS.ViewState_class.VSPage
    {
        FileInfo FIICXCSV = null;
        //string TableName, t1;
        //FileInfo securitytxt = null;
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        DataTable dt1 = new DataTable();
        string FilePath = "";

        //string[] lineSplit;
        //string BenAccountNumber, dpId, date, isin, settlementId;
        private static String path, path1, FileName, s, time, cannotParse;
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
            trYesNo.Visible = false;

            if (!IsPostBack)
            {
                if ((HttpContext.Current.Session["usersegid"] == null) || (HttpContext.Current.Session["LastCompany"].ToString().Trim() == ""))
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>parent.URL();</script>");

                }

            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (MarketStatsSelectFile.FileContent.Length != 0)
                {

                    path = String.Empty;
                    path1 = String.Empty;
                    FileName = String.Empty;
                    s = String.Empty;
                    time = String.Empty;
                    cannotParse = String.Empty;

                    BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                    FilePath = Path.GetFullPath(MarketStatsSelectFile.PostedFile.FileName);
                    FileName = Path.GetFileName(FilePath);
                    hdfname.Value = FileName;

                    String UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                    MarketStatsSelectFile.PostedFile.SaveAs(UploadPath);

                    FileInfo FICSV = new FileInfo(UploadPath);
                    path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                    FIICXCSV = new FileInfo(UploadPath);
                    File.Copy(UploadPath, path, true);

                    ClearArray();


                    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                    InputName[0] = "Module";
                    InputName[2] = "ModifyUser";
                    InputName[1] = "FilePath";

                    InputType[0] = "V";
                    InputType[2] = "I";
                    InputType[1] = "V";


                    InputValue[0] = "InsertMarketStats";
                    InputValue[2] = Session["userid"].ToString();
                    InputValue[1] = path.ToString().Trim();

                    //DataTable dt2 = SQLProcedures.SelectProcedureArr("[SP_INSUP_MCXClosingRatesCHECK]", InputName, InputType, InputValue);
                    //if (dt2.Rows[0][0].ToString() != "0")
                    //{
                    //    DataTable dt1 = SQLProcedures.SelectProcedureArr("[SP_INSUP_MCXClosingRatesModified]", InputName, InputType, InputValue);
                    //}
                    //else
                    //{
                    DataTable dt1 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("[SP_INSUP_NSECDXClosingRates]", InputName, InputType, InputValue);
                    //}

                    if (File.Exists(UploadPath))
                    {
                        File.Delete(UploadPath);
                    }
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Import Successfully!');</script>");

                }
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Selected File Cannot Be Blank!');</script>");



            }
            catch (Exception Ex)
            {
                //throw (Ex.Message);
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


        protected void btnYes_Click(object sender, EventArgs e)
        {
            FileName = hdfname.Value;
            if (File.Exists(Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName))))
            {
                File.Delete(Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName)));

                path = String.Empty;
                path1 = String.Empty;
                FileName = String.Empty;
                s = String.Empty;
                time = String.Empty;
                cannotParse = String.Empty;

                BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                FileName = hdfname.Value;

                String UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                MarketStatsSelectFile.PostedFile.SaveAs(UploadPath);

                FileInfo FICSV = new FileInfo(UploadPath);
                path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                FIICXCSV = new FileInfo(UploadPath);
                File.Copy(UploadPath, path, true);

                ClearArray();


              //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


                InputName[0] = "Module";
                InputName[2] = "ModifyUser";
                InputName[1] = "FilePath";

                InputType[0] = "V";
                InputType[2] = "I";
                InputType[1] = "V";

                InputValue[0] = "InsertMarketStats";
                InputValue[2] = Session["userid"].ToString();
                InputValue[1] = path.ToString().Trim();

                DataTable dt1 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("SP_INSUP_NSECDXClosingRates", InputName, InputType, InputValue);

                trYesNo.Visible = false;

                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert(Import Successfully!');</script>");
            }
        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            trYesNo.Visible = false;
            return;
        }
    }
}
