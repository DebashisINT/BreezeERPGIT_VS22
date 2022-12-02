using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_verify_documentremarks : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        string id = "";

        DataTable DT = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            id = Request.QueryString["id"].ToString();
            Session["docid"] = "";

            if (!IsPostBack)
            {
                string previousPageUrl = string.Empty;
                if (Request.UrlReferrer != null)
                    previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                else
                    previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

                ViewState["previousPageUrl"] = previousPageUrl;
            }

        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            string p1 = "";

            int NoOfRecordsEffected = oDBEngine.SetFieldValue(" tbl_master_document ", "doc_verifyremarks='" + txtReportTo.Text.ToString().Trim() + "',doc_verifydatetime='" + oDBEngine.GetDate() + "',doc_verifyuser='" + HttpContext.Current.Session["userid"] + "' ", " doc_id in   ( " + id + ")");

            if (NoOfRecordsEffected == 1)
            {

                btnYes.OnClientClick = "removeRowAfterDeletion('" + p1 + "')";
                p1 = id;// +"/" + "0" + "/" + "1";
                string popUpscript = "";
                popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + p1 + "');window.close();</script>";
                ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
                string popUpscript1 = "";
                //popUpscript1 = "alert('Successfully Saved'); parent.editwin.close();";
                popUpscript1 = "jAlert('Successfully Saved'); ";
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);
                if (!string.IsNullOrEmpty(Convert.ToString(Session["PageRedirect"])))
                {
                    string url = Convert.ToString(Session["PageRedirect"]);
                    Response.Redirect(url);
                }
                else
                {
                    string popupScript = string.Empty;
                    string previousPageUrl = ViewState["previousPageUrl"].ToString();
                    popupScript = "<script language='javascript'>" + "window.parent.location.href='" + previousPageUrl + "';</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript34", "<script language='javascript'>alert('Some problem occurred during updation!!');</script>");
            }
            string final = p1 + "$";
            Session["docid"] = final.ToString();

        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>parent.editwin.close();</script>");
            if (!string.IsNullOrEmpty(Convert.ToString(Session["PageRedirect"])))
            {
                string url = Convert.ToString(Session["PageRedirect"]);
                Response.Redirect(url);
            }
            else
            {
                string popupScript = string.Empty;
                string previousPageUrl = ViewState["previousPageUrl"].ToString();
                //popupScript = "<script language='javascript'>" + "alert('Saved Successfully');window.parent.location.href='" + previousPageUrl + "';</script>";
                popupScript = "<script language='javascript'>" + "window.parent.location.href='" + previousPageUrl + "';</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }
        }
    }
}