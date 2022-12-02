using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frm_TransactionUpdate : System.Web.UI.Page
    {
        private static String path, path1, FileName, s, time, cannotParse;
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        DataTable dt1 = new DataTable();
        ExcelFile ex = new ExcelFile();
        protected void Page_Load(object sender, EventArgs e)
        {


            txtInsurerCompany.Attributes.Add("onkeyup", "InsurerCompany(this,'getCompanyByLetters',event,'Insurance-Life')");
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
                    InputName[3] = "Company";
                    InputName[4] = "SchemeCompany";

                    InputType[0] = "V";
                    InputType[1] = "V";
                    InputType[2] = "I";
                    InputType[3] = "V";
                    InputType[4] = "V";

                    InputValue[0] = "InsertNCDEXMARGIN";
                    InputValue[1] = path.ToString().Trim();
                    //InputValue[1] = UploadPath.ToString().Trim();
                    InputValue[2] = Session["userid"].ToString();
                    InputValue[3] = Session["LastCompany"].ToString();
                    InputValue[4] = txtInsurerCompany_hidden.Value;

                    if (txtInsurerCompany_hidden.Value == "ICR0000001")
                    {
                        DataTable dt1 = SQLProcedures.SelectProcedureArr("SP_INSUP_InsTransactionUpdate", InputName, InputType, InputValue);
                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Import Successfully!');</script>");
                    }
                    else if (txtInsurerCompany_hidden.Value == "ICB0000002")
                    {
                        DataTable dt1 = SQLProcedures.SelectProcedureArr("SP_INSUP_InsTransactionForBirlaUpdate", InputName, InputType, InputValue);
                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Import Successfully!');</script>");
                    }
                    else if (txtInsurerCompany_hidden.Value == "ICK0000001")
                    {
                        DataTable dt1 = SQLProcedures.SelectProcedureArr("SP_INSUP_InsTransactionForKotakUpdate", InputName, InputType, InputValue);
                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Import Successfully!');</script>");

                    }
                    else
                    {

                        Page.ClientScript.RegisterStartupScript(GetType(), "PageScriptss", "<script language='javascript'>alert('Could not import the file!');</script>");
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