using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
namespace ERP.OMS.Management.DailyTask
{

    public partial class management_DailyTask_CarryForwardStocks : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

            }

        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            SpCall();
        }
        void Procedure_MrgnHldbk()
        {
            ds = oDailyTaskOther.YearEndMarginCF(
             Convert.ToString(Session["LastCompany"]),
             Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
               Convert.ToString(HttpContext.Current.Session["userid"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('1');", true);

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "YearEndMarginCF";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@CompanyId", Session["LastCompany"].ToString());
            //    cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());
            //    cmd.Parameters.AddWithValue("@CreatUser", HttpContext.Current.Session["userid"].ToString().Trim());
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    da.Fill(ds);
            //    da.Dispose();
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('1');", true);

            //}

        }
        void Procedure_Stocks()
        {
            ds = oDailyTaskOther.YearEndStcks(
            Convert.ToString(Session["LastCompany"]),
            Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
              Convert.ToString(HttpContext.Current.Session["userid"]),
                Convert.ToString(txtClient_hidden.Text),
                  Convert.ToString(Session["usersegid"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('1');", true);
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "YearEndStcks";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@CompanyId", Session["LastCompany"].ToString());
            //    cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());
            //    cmd.Parameters.AddWithValue("@CreatUser", HttpContext.Current.Session["userid"].ToString().Trim());
            //    cmd.Parameters.AddWithValue("@ClientId", txtClient_hidden.Text.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@SegmentId", Convert.ToInt32(Session["usersegid"].ToString()));
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    da.Fill(ds);
            //    da.Dispose();
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('1');", true);

            //}

        }
        void Procedure_Obligation()
        {
            ds = oDailyTaskOther.YearEndObligationCF(
              Convert.ToString(Session["LastCompany"]),
              Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
                Convert.ToString(HttpContext.Current.Session["userid"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('1');", true);
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "YearEndObligationCF";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@CompanyId", Session["LastCompany"].ToString());
            //    cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());
            //    cmd.Parameters.AddWithValue("@CreatUser", HttpContext.Current.Session["userid"].ToString().Trim());
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    da.Fill(ds);
            //    da.Dispose();
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('1');", true);

            //}

        }
        void SpCall()
        {
            if (ddlType.SelectedItem.Value.ToString().Trim() == "1")
            {
                Procedure_MrgnHldbk();
            }
            if (ddlType.SelectedItem.Value.ToString().Trim() == "2")
            {
                if (txtClient_hidden.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('2');", true);

                }
                else
                {
                    Procedure_Stocks();
                }
            }
            if (ddlType.SelectedItem.Value.ToString().Trim() == "3")
            {
                Procedure_Obligation();
            }
        }
    }

}