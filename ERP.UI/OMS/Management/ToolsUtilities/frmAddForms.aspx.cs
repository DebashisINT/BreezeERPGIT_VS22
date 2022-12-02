using System;
using System.Configuration;
using System.IO;
using System.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmAddForms : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            string id1 = Request.QueryString["id"].ToString();
            if (id1 == "yes")
            {
                lblP.Text = "Personal";
                lblP.Visible = true;
            }
            else
            {
                lblP.Text = "Common";
                lblP.Visible = true;
            }
        }
        protected void btnAddForm_Click(object sender, EventArgs e)
        {
            Int32 CreateUser = Int32.Parse(HttpContext.Current.Session["userid"].ToString());//Session UserID
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            string FName = Path.GetFileName(txtfilename.PostedFile.FileName);
            string id = Request.QueryString["id"].ToString();
            string flagy = "";
            if (id == "yes")
            {
                flagy = "1";
                lblP.Text = "Personal";
                lblP.Visible = true;
            }
            else
            {
                flagy = "0";
                lblP.Text = "Common";
                lblP.Visible = true;
            }
            if (FName != "")
            {

                string sd = objConverter.GetAutoGenerateNo();
                //  string filename = Session.SessionID + FName;
                string filename = HttpContext.Current.Session["userid"].ToString() + sd + FName;
                string DirLocation = Server.MapPath("../Documents/FormsNotices/");
                string FLocation = DirLocation + filename;
                if (!System.IO.Directory.Exists(DirLocation))
                    System.IO.Directory.CreateDirectory(DirLocation);
                
                txtfilename.PostedFile.SaveAs(FLocation);
                oDBEngine.InsurtFieldValue("tbl_master_forms", "frm_FormName,frm_source,frm_private,CreateDate,CreateUser,frm_desc", "'" + txtName.Text + "','" + "Documents/FormsNotices/" + filename + "','" + flagy + "','" + CreateDate.ToString() + "','" + CreateUser + "','" + txtFile.Text + "'");
                txtName.Text = "";
                txtFile.Text = "";
                string popupScript = "";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>dhtmlclose();</script>");
            }
            else
            {
                string popupScript = "";
                popupScript = "<script language='javascript'> dhtmlclose();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }
            //string popupScript = "";
            //string query = Request.QueryString["id"].ToString();
            //if (Session["KeyVal"] != null)
            //{
            //    popupScript = "<script language='javascript'>" + "alert('Successfully Uploaded');window.parent.Getvalue();window.parent.popup.Hide();</script>";
            //    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            //}
            //else
            //{
            //    popupScript = "<script language='javascript'>" + "alert('Successfully Uploaded');</script>";
            //    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            //} 
        }


        protected void btnDocumentDiscard_Click(object sender, EventArgs e)
        {
            this.Page.ClientScript.RegisterStartupScript(GetType(), "Back", "<script>GoBack();</script>");
        }
    }
}