using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_frm_NSECDXMargin : ERP.OMS.ViewState_class.VSPage
    {
        FileInfo FIICXCSV = null;

        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        DataTable dt1 = new DataTable();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();

        private static String path, path1, FileName, s, time, cannotParse;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>Page_Load();</script>");
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");

        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MCXMarginSelectFile.FileContent.Length != 0)
                {
                    path = String.Empty;
                    path1 = String.Empty;
                    FileName = String.Empty;
                    s = String.Empty;
                    time = String.Empty;
                    cannotParse = String.Empty;
                    BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();
                    String FilePath = Path.GetFullPath(MCXMarginSelectFile.PostedFile.FileName);
                    FileName = Path.GetFileName(MCXMarginSelectFile.PostedFile.FileName);
                    String UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                    MCXMarginSelectFile.PostedFile.SaveAs(UploadPath);
                    FileInfo FICSV = new FileInfo(UploadPath);
                    path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);

                    FIICXCSV = new FileInfo(UploadPath);
                    File.Copy(UploadPath, path, true);

                    ClearArray();


                    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                    if (cmbFileType.SelectedValue == "0")
                    {
                        InputName[0] = "Module";
                        InputName[1] = "FilePath";
                        InputName[3] = "CompanyID";
                        InputName[2] = "SegmentID";
                        InputName[4] = "ModifyUser";

                        InputType[0] = "V";
                        InputType[1] = "V";
                        InputType[3] = "V";
                        InputType[2] = "I";
                        InputType[4] = "I";


                        InputValue[0] = "InsertNSECDXMARGIN";
                        InputValue[1] = path.ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["usersegid"].ToString();
                        InputValue[4] = Session["userid"].ToString();


                        DataTable dt1 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("SP_INSUP_NSECDXMARGIN", InputName, InputType, InputValue);
                    }
                    if (cmbFileType.SelectedValue == "1")
                    {
                        String strDate = dtfor.Value.ToString();

                        InputName[0] = "FilePath";
                        InputName[1] = "CompanyID";
                        InputName[2] = "SegmentID";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "Adhoc";
                        InputName[5] = "Date";

                        InputType[0] = "V";
                        InputType[1] = "V";
                        InputType[2] = "I";
                        InputType[3] = "I";
                        InputType[4] = "V";
                        InputType[5] = "V";

                        InputValue[0] = path.ToString().Trim();
                        InputValue[1] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["usersegid"].ToString();
                        InputValue[3] = Session["userid"].ToString();
                        if (txtAdhoc.Text != "")
                        {
                            InputValue[4] = txtAdhoc.Text;
                        }
                        else
                        {
                            InputValue[4] = "0";
                        }
                        InputValue[5] = Convert.ToDateTime(strDate).ToString("yyyy-MM-dd");
                        DataTable dt1 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("[Import_NSECDXMARGIN_FT]", InputName, InputType, InputValue);
                    }
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