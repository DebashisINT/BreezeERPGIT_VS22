using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
namespace ERP.OMS.Management.DailyTask
{

    public partial class management_DailyTask_frm_Attendence_Import : ERP.OMS.ViewState_class.VSPage
    {
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        string UploadPath = "";
        FileInfo FFIXCSV = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
            lblStatus.Text = "";
            lblList.Text = "";
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (FLSelectFile.PostedFile.FileName == "")
            {
                lblStatus.Text = "Please select a file";
            }
            else
            {

                String FilePath = Path.GetFullPath(FLSelectFile.PostedFile.FileName);
                String FileName = Path.GetFileName(FLSelectFile.PostedFile.FileName);
                UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                FLSelectFile.PostedFile.SaveAs(UploadPath);
                FileInfo FICSV = new FileInfo(UploadPath);
                string path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                FFIXCSV = new FileInfo(UploadPath);
                File.Copy(UploadPath, path, true);

                //Execute Procedure with parameter list
                InputName[0] = "FilePath";
                InputType[0] = "V";
                InputValue[0] = path.ToString().Trim();
                InputName[1] = "CreateUser";
                InputType[1] = "V";
                InputValue[1] = Session["UserId"].ToString();
                DataTable Emp_upload = new DataTable();
                //Emp_upload = SQLProcedures.SelectProcedureArr("SP_Attendence_Import", InputName, InputType, InputValue);
                Emp_upload = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("SP_Attendence_Import", InputName, InputType, InputValue);
                if (Emp_upload != null)
                {
                    //if (SQLProcedures.errmessage != null)
                    //{
                    //    lblStatus.Text = SQLProcedures.errmessage;
                    //}
                    if (Emp_upload.Rows.Count > 0)
                    {
                        string employee_list = Emp_upload.Rows[0][0].ToString();
                        lblList.Text = employee_list + " Not Found into the system";
                        lblStatus.Text = "";
                    }
                    if (Emp_upload.Rows.Count == 0)
                    {
                        lblList.Text = "";
                        string importstatus = "File " + FileName + " Successfully Uploaded";
                        lblStatus.Text = importstatus;
                    }

                }
                else
                {

                }


            }

        }
        private bool IsValidFile(FileInfo FICSV)
        {
            if (FICSV.FullName.Length <= 0)
            {
                string a = "No File Selected!";
                return false;
            }

            //if (FICSV.Name.Substring(FICSV.Name.Length - 3, 3).ToLower() != "csv" && FICSV.Name.Substring(FICSV.Name.Length - 3, 3).ToLower() != "txt")
            //{
            //    importstatus.Text = "InValid File Selection!";
            //    return false;
            //}

            return true;
        }
    }

}