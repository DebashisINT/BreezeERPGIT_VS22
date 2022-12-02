using System;
using System.Configuration;
using System.IO;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_toolsutilities_Frm_UploadCommonFolder : System.Web.UI.Page
    {
        //Converter objConverter = new Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();



        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnAddForm_Click(object sender, EventArgs e)
        {
            String FileName = Path.GetFileName(OFDSelectFile.PostedFile.FileName);

            if (FileName != "")
            {

                String UploadPath = ConfigurationManager.AppSettings["SaveCSVsql"].ToString() + FileName;
                OFDSelectFile.PostedFile.SaveAs(UploadPath);
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>alert('" + FileName + " file Upload successfully in   " + ConfigurationManager.AppSettings["SaveCSVsql"].ToString() + ".');</script>");

            }
            else
            {
                string popupScript = "";
                popupScript = "<script language='javascript'> dhtmlclose();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>alert('Error on upload please try again !..');</script>");
            }

        }

    }
}