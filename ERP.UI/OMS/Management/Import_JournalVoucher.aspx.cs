using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_Import_JournalVoucher : System.Web.UI.Page
    {
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        String FileName = String.Empty;
        BusinessLogicLayer.DBEngine oDBEngine1 = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataTable dt1 = new DataTable();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        protected void Page_Load(object sender, EventArgs e)
        {

            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            if (Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (BK01File.PostedFile.ContentLength > 0)
                {

                    String FilePath = Path.GetFullPath(BK01File.PostedFile.FileName);

                    FileName = Path.GetFileName(BK01File.PostedFile.FileName);
                    String UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                    BK01File.PostedFile.SaveAs(UploadPath);
                    FileInfo FICSV = new FileInfo(UploadPath);
                    string path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                    File.Copy(UploadPath, path, true);

                    ClearArray();

                    InputName[0] = "ExcSegmt";
                    InputName[1] = "ExchangeSegmentID";
                    InputName[2] = "FilePath";
                    InputName[3] = "CompanyID";
                    InputName[4] = "SettlementNo";
                    InputName[5] = "AutoGenNo";
                    InputName[6] = "CreateUser";
                    InputName[7] = "BatchNo";

                    InputType[0] = "I";
                    InputType[1] = "I";
                    InputType[2] = "V";
                    InputType[3] = "V";
                    InputType[4] = "V";
                    InputType[5] = "V";
                    InputType[6] = "V";
                    InputType[7] = "V";


                    InputValue[0] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                    InputValue[1] = HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim();
                    InputValue[2] = path.ToString().Trim();
                    InputValue[3] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                    InputValue[4] = HttpContext.Current.Session["LastSettNo"].ToString().Trim();
                    InputValue[5] = oconverter.GetAutoGenerateNo();
                    InputValue[6] = Session["userid"].ToString();
                    InputValue[7] = FileName.Substring(11, 2).ToString();

                    DataTable dt1 = SQLProcedures.SelectProcedureArr("[Import_JornalVoucher]", InputName, InputType, InputValue);
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
            }
            catch (Exception ex)
            {

                importstatus.Text = ex.Message.ToString() + "<br>";
                importstatus.Text += "Error importing. Please try again";
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