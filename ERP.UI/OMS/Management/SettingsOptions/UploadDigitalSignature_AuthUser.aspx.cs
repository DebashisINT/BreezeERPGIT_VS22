using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_UploadDigitalSignature_AuthUser : System.Web.UI.Page
    {
        GlobalSettings global_set = new GlobalSettings();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//


            bindGrid();
            if (!IsPostBack)
            {
                //txtValidUser.Attributes.Add("onkeyup", "CallAjax(this,'SearchEmployeesForDigitalSignatureUser',event)");

            }

        }
        void bindGrid()
        {
            DataTable DT = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand cmd = new SqlCommand("DigitalSignatureAuthUser", con))
            //    {

            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);

            //        cmd.CommandTimeout = 0;
            //        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            //        {

            //            DT.Reset();
            //            da.Fill(DT);
            //        }
            //    }
            //}
            DT = global_set.DigitalSignatureAuthUser(Request.QueryString["id"]);
            gridAuthUser.DataSource = DT;
            gridAuthUser.DataBind();
        }
        protected void gridAuthUser_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string removeID = e.Keys[0].ToString();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand cmd = new SqlCommand("DigitalSignAuthUserDel", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);
            //        cmd.Parameters.AddWithValue("@removeID", removeID);
            //        cmd.CommandTimeout = 0;

            //        con.Open();
            //        int i = cmd.ExecuteNonQuery();
            //    } 

            //}
            int i = global_set.DigitalSignAuthUserDel(Request.QueryString["id"], removeID);
            e.Cancel = true;
            System.GC.Collect();
            bindGrid();
        }
        protected void gridAuthUser_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            HiddenField hidden = (HiddenField)gridAuthUser.FindEditFormTemplateControl("txtValidUser_hidden");
            if (hidden.Value != "")
            {
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{

                //    using (SqlCommand cmd = new SqlCommand("DigitalSignAuthUserAdd", con))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);
                //        cmd.Parameters.AddWithValue("@addID", hidden.Value);

                //        cmd.CommandTimeout = 0;
                //       if(con.State!=ConnectionState.Open)
                //        con.Open();
                //        int i = cmd.ExecuteNonQuery();

                //    }
                //}
                int i = global_set.DigitalSignAuthUserAdd(Request.QueryString["id"], hidden.Value);
                hidden.Value = "";
                gridAuthUser.CancelEdit();
                e.Cancel = true;
                System.GC.Collect();
                bindGrid();

            }
        }
    }
}