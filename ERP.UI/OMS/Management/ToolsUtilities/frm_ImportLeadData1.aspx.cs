using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;


namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_ToolsUtilities_frm_ImportLeadData1 : System.Web.UI.Page
    {
        public string pageAccess = "";
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
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string FName = Path.GetFileName(FileUpload.PostedFile.FileName);
            string Extention = Path.GetExtension(FileUpload.PostedFile.FileName);
            if (FName != "")
            {
                if (Extention == ".xls")
                {
                    string FileName = Session.SessionID + FName;
                    string FLocation = Server.MapPath("../Documents/") + FileName;
                    FileUpload.PostedFile.SaveAs(FLocation);
                    Response.Redirect("~/OMS/Management/frm_ImportLeadData.aspx?filestr=" + FLocation);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('File Must Be Excel File')</script>");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please Select File')</script>");
            }
        }
    }
}