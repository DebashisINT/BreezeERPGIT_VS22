using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;

namespace ERP.OMS.Management
{
    public partial class management_frmImportBank : System.Web.UI.Page
    {
        private static String path, path1, FileName, s, time, cannotParse;
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        DataTable dt1 = new DataTable();
        ExcelFile ex = new ExcelFile();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            lblMsgAccCode.Visible = false;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            lblMsgAccCode.Visible = false;
            try
            {

                if (NCDEXSelectFile.FileContent.Length != 0)
                {

                    path = String.Empty;
                    path1 = String.Empty;
                    FileName = String.Empty;
                    s = String.Empty;
                    time = String.Empty;
                    cannotParse = String.Empty;

                    String FilePath = Path.GetFullPath(NCDEXSelectFile.PostedFile.FileName);
                    FileName = Path.GetFileName(NCDEXSelectFile.PostedFile.FileName);
                    String UploadPath = Server.MapPath(ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName);
                    NCDEXSelectFile.PostedFile.SaveAs(UploadPath);
                    FileInfo FICSV = new FileInfo(UploadPath);
                    path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);

                    File.Copy(UploadPath, path, true);

                    ClearArray();

                    InputName[0] = "Module";
                    InputName[1] = "FilePath";
                    InputName[2] = "CreateUser";

                    InputType[0] = "V";
                    InputType[1] = "V";
                    InputType[2] = "I";

                    InputValue[0] = "InsertNCDEXMARGIN";
                    InputValue[1] = path.ToString().Trim();
                    //InputValue[1] = UploadPath.ToString().Trim();
                    InputValue[2] = Session["userid"].ToString();

                    DataTable dt1 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("SP_INSUP_BANKIMPORT", InputName, InputType, InputValue);
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
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Selected File Cannot Be Blank!');</script>");
                }

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