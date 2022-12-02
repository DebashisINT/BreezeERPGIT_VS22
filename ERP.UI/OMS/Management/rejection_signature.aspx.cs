using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_rejection_signature : System.Web.UI.Page
    {
        //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnreject_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                SqlCommand com;
                //SqlCommand com = new SqlCommand("update Trans_CdslOffline set CdslOffline_Rejected=1,CdslOffline_RejectUser='" + Session["userid"].ToString() + "',CdslOffline_RejectDateTime=getdate(),CdslOffline_RejectReason='" + txtreject.Text + "',CdslOffline_VerifyUser ='" + Session["userid"].ToString() + "',CdslOffline_VerifiyDateTime =getdate() where CdslOffline_ID='" + Request.QueryString["id"].ToString() + "'", con);
                if (Request.QueryString["id"].ToString().Split('~')[1] == "CDSL")
                {
                    com = new SqlCommand("update Trans_CdslOffline set CdslOffline_VerifyUser=null,CdslOffline_VerifiyDateTime=null, CdslOffline_Rejected=1,CdslOffline_RejectUser='" + Session["userid"].ToString() + "',CdslOffline_RejectDateTime=getdate(),CdslOffline_RejectReason='" + txtreject.Text + "' where CdslOffline_ID='" + Request.QueryString["id"].ToString().Split('~')[0] + "'", con);
                }
                else
                {
                    com = new SqlCommand("update Trans_nsdlOffline set NsdlOffline_VerifiedBy=null,NsdlOffline_VerifyDateTime=null, NsdlOffline_Rejected=1,NsdlOffline_RejectUser='" + Session["userid"].ToString() + "',NsdlOffline_RejectDateTime=getdate(),NsdlOffline_RejectReason='" + txtreject.Text + "' where NSDLOffline_ID='" + Request.QueryString["id"].ToString().Split('~')[0] + "'", con);

                }
                com.CommandType = CommandType.Text;
                com.ExecuteNonQuery();
                // Mantis Issue 24802
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                // End of Mantis Issue 24802
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Updated successfully');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "pageclose", "pageclose()", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Can not update');", true);

            }

        }
    }
}