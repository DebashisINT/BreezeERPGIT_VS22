using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_frm_Attendance_Import_Monthly : ERP.OMS.ViewState_class.VSPage
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
            string EmployeeCode1 = "";
            string Intime1 = "";
            string Outtime1 = "";
            string Status1 = "";
            string AttDate1 = "";
            if (FLSelectFile.PostedFile.FileName == "")
            {
                lblStatus.Text = "Please select a file";
            }
            else
            {
                string UploadPath1 = "";
                String FilePath = Path.GetFullPath(FLSelectFile.PostedFile.FileName);
                String FileName = Path.GetFileName(FLSelectFile.PostedFile.FileName);
                UploadPath1 = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                FLSelectFile.PostedFile.SaveAs(UploadPath1);
                FileInfo FICSV = new FileInfo(UploadPath1);
                string path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                if (File.Exists(path) == true)
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (IOException ex)
                    {
                        Response.Write(ex.Message.ToString());
                    }
                }

                File.Copy(UploadPath1, path, true);
                TextReader tr = new StreamReader(path);
                string PathCopy = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], "attendance.txt");
                if (File.Exists(PathCopy) == true)
                {

                    File.Delete(PathCopy);
                }

                TextWriter tw1 = new StreamWriter(PathCopy);
                int i = 0;
                while (tr.Peek() > 1)
                {
                    string line = tr.ReadLine();
                    if (line != "")
                    {
                        string[] EmpCodeName1 = line.Split(' ');
                        string[] EmpCode1 = EmpCodeName1[0].ToString().Split(',');
                        int index = EmpCode1[0].ToString().IndexOf(@"/");
                        if (index < 0)
                        {
                            EmployeeCode1 = EmpCode1[1].ToString();
                        }
                        else
                        {
                            AttDate1 = EmpCode1[0].ToString();
                            if (EmpCode1[2] == "WO" || EmpCode1[2] == "AB" || EmpCode1[2] == "PH" || EmpCode1[2] == "PL" || EmpCode1[2] == "OD" || EmpCode1[2] == "SL" || EmpCode1[2] == "CO" || EmpCode1[2] == "CL")
                            {
                                Intime1 = "";
                                Outtime1 = "";
                                Status1 = EmpCode1[2].ToString();

                            }
                            else
                            {
                                Intime1 = EmpCode1[2].ToString();
                                Outtime1 = EmpCode1[3].ToString();
                                if (EmpCode1.Length > 6)
                                {
                                    if (EmpCode1[EmpCode1.Length - 1] == "E4")
                                    {
                                        Status1 = EmpCode1[EmpCode1.Length - 2].ToString();
                                    }
                                    else
                                    {
                                        Status1 = EmpCode1[EmpCode1.Length - 1].ToString();
                                    }
                                }
                                else
                                {

                                    Status1 = EmpCode1[5].ToString();
                                    if (Status1 == "WP")
                                    {
                                        Status1 = "P";
                                    }
                                }
                            }

                            i = i + 1;
                            string FileContent = "01";
                            FileContent = FileContent + "," + AttDate1;
                            FileContent = FileContent + "," + Intime1;
                            FileContent = FileContent + "," + Outtime1;
                            FileContent = FileContent + "," + Status1;
                            FileContent = FileContent + "," + EmployeeCode1.ToString();
                            FileContent = FileContent + "," + i;
                            tw1.Write(FileContent);
                            tw1.WriteLine();
                        }

                    }

                }




                tw1.Close();

                InputName[0] = "FilePath";
                InputType[0] = "V";
                InputValue[0] = PathCopy.ToString().Trim();
                InputName[1] = "CreateUser";
                InputType[1] = "V";
                InputValue[1] = Session["UserId"].ToString();
                DataTable Emp_upload = new DataTable();

                //Emp_upload = SQLProcedures.SelectProcedureArr("[SP_Attendence_Import_monthly]", InputName, InputType, InputValue);
                Emp_upload = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("[SP_Attendence_Import_monthly]", InputName, InputType, InputValue);

                if (Emp_upload != null)
                {
                    if (Emp_upload.Rows.Count > 0)
                    {
                        string employee_list = Emp_upload.Rows[0][0].ToString();
                        lblList.Text = employee_list + " Not Found into the system";
                    }
                    lblStatus.Text = FileName;
                    if (Emp_upload.Rows.Count == 0)
                    {
                        lblList.Text = "";
                        string importstatus = "File " + FileName + " Successfully Uploaded";
                        lblStatus.Text = importstatus;
                    }


                    else
                    {

                    }

                    File.Delete(UploadPath1);
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

            return true;
        }
    }


}