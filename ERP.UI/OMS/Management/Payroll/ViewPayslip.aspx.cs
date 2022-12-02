using BusinessLogicLayer;
using BusinessLogicLayer.EmailDetails;
using DataAccessLayer;
using EntityLayer.CommonELS;
using EntityLayer.MailingSystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ERP.OMS.Management.Payroll
{
    public partial class ViewPayslip : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Payroll/ViewPayslip.aspx");
            if (!IsPostBack)
            {
                // Rev Sanchita
                //string[] filePaths = new string[] { };
                //string DesignPath = "";
                //if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                //{
                //    DesignPath = @"Reports\Reports\RepxReportDesign\PaySlip\DocDesign\Designes";
                //}
                //else
                //{
                //    DesignPath = @"Reports\RepxReportDesign\PaySlip\DocDesign\Designes";
                //}
                //string fullpath = Server.MapPath("~");
                //fullpath = fullpath.Replace("ERP.UI\\", "");
                //string DesignFullPath = fullpath + DesignPath;
                //filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                //foreach (string filename in filePaths)
                //{
                //    string reportname = Path.GetFileNameWithoutExtension(filename);
                //    string name = "";
                //    if (reportname.Split('~').Length > 1)
                //    {
                //        name = reportname.Split('~')[0];
                //    }
                //    else
                //    {
                //        name = reportname;
                //    }
                //    string reportValue = reportname;
                //    //if (reportValue != SavereportValue)
                //    //{
                //    CmbDesignName.Items.Add(name, reportValue);
                //    //}
                //}
                //CmbDesignName.SelectedIndex = 0;

                Session["EmployeeDetails"] = null;
                Session["dtSelectedDesign"] = null;
                Session["employeeid_Payslip"] = null;


                getAllCompanyInfo();
                // End of Rev Sanchita
            }
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        // Rev Sanchita
        protected void EmployeeGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["EmployeeDetails"] != null)
            {
                DataTable EmpDT = (DataTable)Session["EmployeeDetails"];
                DataView dvData = new DataView(EmpDT);
                //dvData.RowFilter = "Status <> 'D'";
                EmployeeGrid.DataSource = dvData.ToTable();
            }
        }
        protected void EmployeeGrid_DataBound(object sender, EventArgs e)
        {
            //
        }

        // Rev Sanchita
        public class listPayslipConfig
        {
            public string liID { get; set; }
            public string liEmpList { get; set; }
            public string liPayStructureCode { get; set; }
            public string liDefaultDesign { get; set; }
        }


        [WebMethod]
        public static object GetPayslipConfig()
        {
            List<listPayslipConfig> list = new List<listPayslipConfig>();
            DataTable dtPayslipConfig = (DataTable)HttpContext.Current.Session["dtPayslipConfig"];

            list = (from DataRow dr in dtPayslipConfig.Rows
                    select new listPayslipConfig()
                    {
                        liID = Convert.ToString(dr["ID"]),
                        liEmpList = Convert.ToString(dr["EmpList"]),
                        liPayStructureCode = Convert.ToString(dr["PayStructureCode"]),
                        liDefaultDesign = Convert.ToString(dr["DefaultDesign"]),
                    }).ToList();

            return list;
        }

        //[WebMethod]
        //public static string GetEmployeesForSlip(string liID,string  structureid)
        //{
        //    DataTable dtPayslipConfig = (DataTable)HttpContext.Current.Session["dtPayslipConfig"];
        //    DataRow[] drEmpList = dtPayslipConfig.Select("ID='" + liID + "'");
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("StructureId", typeof(String));
        //    dt.Columns.Add("EmployeeId", typeof(String));
        //    dt.Rows.Add(structureid, drEmpList[0]["EmpList"]);

        //    if (HttpContext.Current.Session["employeeid_Payslip"]==null)
        //    {
        //        HttpContext.Current.Session["employeeid_Payslip"] = dt;  // column EmpList
        //    }
        //    else
        //    {
        //        //
        //    }


        //    return "success";
        //}
        // End of Rev Sanchita

        [WebMethod]
        public static void InitSessionVariables()
        {
            HttpContext.Current.Session["employeeid_Payslip"] = null;
            HttpContext.Current.Session["dtPayslipConfig"] = null;
        }



        protected void EmployeeGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            // Rev Sanchita
            var Action = Convert.ToString(e.Parameters.Split('|')[0]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            if (Action == "ViewPayslip_SelectAll" || Action == "GeneratePDF_SelectAll")
            {
                var Employees = string.Join(",", EmployeeGrid.GetSelectedFieldValues("Employee_Code"));
                var Period = Convert.ToString(e.Parameters.Split('|')[1]);

                if (Employees == "")
                {
                    EmployeeGrid.JSProperties["cpPayslip"] = "NoEmployeeSelected";
                }
                else if (Period == "")
                {
                    EmployeeGrid.JSProperties["cpPayslip"] = "BlankPeriod";
                }
                else
                {
                    DataTable ds = new DataTable();
                    DataAccessLayer.ProcedureExecute proc = new DataAccessLayer.ProcedureExecute("prc_GetPayslipData");
                    proc.AddVarcharPara("@Action", 100, "GetPaySlipData");
                    proc.AddVarcharPara("@EmployeeList", -1, Employees);
                    ds = proc.GetTable();

                    Session["dtPayslipConfig"] = ds;
                    Session["employeeid_Payslip"] = ds;

                    if (Action == "ViewPayslip_SelectAll")
                    {
                        EmployeeGrid.JSProperties["cpPayslip"] = "SelectAllGenerate";
                    }
                    else
                    {


                        foreach (DataRow drEmpDet in ds.Rows)
                        {
                            String EmployeeCode = Convert.ToString(drEmpDet["EmpList"]);
                            String StructureID = Convert.ToString(drEmpDet["PayStructureCode"]);
                            String DesignName = Convert.ToString(drEmpDet["DefaultDesign"]);


                            var arrEmployeeCode = EmployeeCode.Split(',');

                            for (var i = 0; i < arrEmployeeCode.Length; i++)
                            {
                                GeneratePDF(arrEmployeeCode[i].ToString().Trim(), StructureID, DesignName, Period);

                                // SEND EMAIL
                                MailPaySlip(arrEmployeeCode[i].ToString().Trim(), Period);
                                // END of SEND MAIL

                            }
                        }
                        EmployeeGrid.JSProperties["cpPayslip"] = "PDFGenerateSuccess";
                    }

                }


            }
            else if (Action == "ViewPayslip" || Action == "GeneratePDF")
            {
                var PayStructureID = Convert.ToString(e.Parameters.Split('|')[1]);
                var Period = Convert.ToString(e.Parameters.Split('|')[2]);
                var DesignName = Convert.ToString(e.Parameters.Split('|')[3]);

                var Employees = string.Join(",", EmployeeGrid.GetSelectedFieldValues("Employee_Code"));

                if (Employees == "")
                {
                    EmployeeGrid.JSProperties["cpPayslip"] = "NoEmployeeSelected";
                }
                else if (Period == "")
                {
                    EmployeeGrid.JSProperties["cpPayslip"] = "BlankPeriod";
                }
                else if (PayStructureID == "")
                {
                    EmployeeGrid.JSProperties["cpPayslip"] = "NoPayStructureSelected";
                }
                else if (DesignName == "")
                {
                    EmployeeGrid.JSProperties["cpPayslip"] = "NoDesignNameSelected";
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PayStructureCode", typeof(String));
                    dt.Columns.Add("EmpList", typeof(String));
                    dt.Rows.Add(hdnPayStructureID.Value, Employees);
                    HttpContext.Current.Session["employeeid_Payslip"] = dt;  // column EmpList

                    if (Action == "ViewPayslip")
                    {
                        EmployeeGrid.JSProperties["cpPayslip"] = "Generate";
                    }
                    else
                    {
                        var arrEmployeeCode = Employees.Split(',');

                        for (var i = 0; i < arrEmployeeCode.Length; i++)
                        {
                            GeneratePDF(arrEmployeeCode[i].ToString().Trim(), PayStructureID, DesignName, Period);

                            // SEND EMAIL
                            MailPaySlip(arrEmployeeCode[i].ToString().Trim(), Period);
                            // END of SEND MAIL
                        }
                        EmployeeGrid.JSProperties["cpPayslip"] = "PDFGenerateSuccess";
                    }
                }
                

            }

        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked == true)
            {
                DataTable dtNone = new DataTable();
                Session["EmployeeDetails"] = dtNone;
                EmployeeGrid.DataBind();

                getAllCompanyInfo();
            }
            else
            {
                txtPayStructure.Enabled = true;
                CmbDesignName.Enabled = true;
                CmbDesignName.Text = "";

                DataTable dtNone = new DataTable();
                Session["EmployeeDetails"] = dtNone;
                EmployeeGrid.DataBind();

            }
        }

        protected void CmbDesignName_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            //CmbDesignName.Enabled = true; 

            if (Session["dtSelectedDesign"] != null)
            {
                DataTable dtSelectDesign = (DataTable)Session["dtSelectedDesign"];
                CmbDesignName.Items.Clear();

                CmbDesignName.DataSource = dtSelectDesign;
                CmbDesignName.DataBind();
            }

            CmbDesignName.SelectedIndex = 0;


        }
        // End of Rev Sanchita

        private void getAllCompanyInfo()
        {
            DataTable dtEmp = oDBEngine.GetDataTable("Select Employee_Code,EmployeeUniqueCode,Employee_Name From proll_EmployeeAttactchment inner Join v_proll_EmployeeList On proll_EmployeeAttactchment.EmployeeCode=v_proll_EmployeeList.Employee_Code");

            Session["EmployeeDetails"] = dtEmp;
            EmployeeGrid.DataBind();

            txtPayStructure.Text = "";
            txtPayStructure.Enabled = false;
            hdnPayStructureID.Value = "";

            CmbDesignName.Items.Clear();
            //lblSelectDesign.Visible = false;
            CmbDesignName.Text = "";
            CmbDesignName.Enabled = false;

            //CmbDesignName.Enabled = false;
            chkSelectAll.Checked = true;
        }

        private void GeneratePDF(string EmployeeCode, string StructureID, string DesignName, string YYMM)
        {
            string Physical_Path = "";
            // string YYMM = "2012";
            string YY = YYMM.Substring(0, 2);
            string MM = YYMM.Substring(2, 2);

            if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
            {
                Physical_Path = Server.MapPath("~/CommonFolder/Payslip/" + YY + "/" + MM + "/");
            }
            else
            {
                Physical_Path = Server.MapPath("~/CommonFolder/Payslip/" + YY + "/" + MM + "/");
            }

            String mapPath = Server.MapPath("~/");
            String PDFFileName = EmployeeCode + ".pdf";

            if (!Directory.Exists(Physical_Path))
            {
                Directory.CreateDirectory(Physical_Path);
            }
            else
            {
                if (File.Exists(Physical_Path + PDFFileName))
                {
                    File.Delete(Physical_Path + PDFFileName);
                }
            }

            Physical_Path = Physical_Path + PDFFileName;

            Export.PayslipExport exportToPDF = new Export.PayslipExport();
            exportToPDF.ExportToPdfforEmail(DesignName, "PAYSLIP", mapPath, EmployeeCode, StructureID, YYMM, Physical_Path);

        }


        private void MailPaySlip(string EmployeeCode,string YYMM)
        {
            String baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            String Physical_Path = "";

            String YY = YYMM.Substring(0, 2);
            String MM = YYMM.Substring(2, 2);
            String PDFFileName = EmployeeCode + ".pdf";

            if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
            {
                Physical_Path = Server.MapPath("~/CommonFolder/Payslip/" + YY + "/" + MM + "/" + PDFFileName);
            }
            else
            {
                Physical_Path = Server.MapPath("~/CommonFolder/Payslip/" + YY + "/" + MM + "/" + PDFFileName);
            }

            CommonBL ComBL = new CommonBL();
            string AttachPayslipInMail = ComBL.GetSystemSettingsResult("AttachPayslipInMail");


            // Get Email Body
            DataTable dtmsgBody = new DataTable();
            DataAccessLayer.ProcedureExecute proc1 = new DataAccessLayer.ProcedureExecute("prc_GetPayslipMailBody");
            proc1.AddVarcharPara("@Action", 100, "GetMailBody");
            proc1.AddVarcharPara("@EmployeeCode", 500, EmployeeCode);
            proc1.AddVarcharPara("@YYMM", 50, YYMM);
            proc1.AddVarcharPara("@baseUrl", 500, baseUrl + "/CommonFolder/Payslip/" + YY + "/" + MM + "/" + PDFFileName);
            proc1.AddVarcharPara("AttachPayslipInMail", 20, AttachPayslipInMail);
            //

            dtmsgBody = proc1.GetTable();

            if (dtmsgBody != null && dtmsgBody.Rows.Count > 0)
            {
                string msgBody = Convert.ToString(dtmsgBody.Rows[0]["msgBody"]);
                string emailID = Convert.ToString(dtmsgBody.Rows[0]["emailID"]);

                if (emailID != "")
                {
                    SendMail(EmployeeCode, emailID, msgBody, Physical_Path, AttachPayslipInMail);
                }

            }
                                
        }


        private int SendMail(string EmployeeCode, string emailID, string msgBody, string Physical_Path, string AttachPayslipInMail)
        {
            int stat = 0;

            Employee_BL objemployeebal = new Employee_BL();
            ExceptionLogging mailobj = new ExceptionLogging();
            EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
            DataTable dt_EmailConfig = new DataTable();
            DataTable dt_EmailConfigpurchase = new DataTable();

            DataTable dt_Emailbodysubject = new DataTable();
            SalesOrderEmailTags fetchModel = new SalesOrderEmailTags();
            string Subject = "";
            string Body = "";
            string emailTo = "";
            var customerid = Convert.ToString(EmployeeCode);
            dt_EmailConfig = objemployeebal.Getemailids(customerid);
            // string FilePath = string.Empty;
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            string path1 = string.Empty;
            
            if (dt_EmailConfig.Rows.Count > 0)
            {
                emailTo = emailID;

                Body = msgBody;
                Subject = "Template";

                if (AttachPayslipInMail.ToUpper() == "YES")
                {
                    emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", Physical_Path, Body, Subject);
                }
                else
                {
                    emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", null, Body, Subject);
                }

                if (emailSenderSettings.IsSuccess)
                {
                    string Message = "";
                    EmailSenderEL obj2 = new EmailSenderEL();
                    stat = SendEmailUL.sendMailInHtmlFormat(emailSenderSettings.ModelCast<EmailSenderEL>(), out Message);
                }
            }

            return stat;
        }
    }

}