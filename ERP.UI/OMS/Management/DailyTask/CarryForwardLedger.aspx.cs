using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BusinessLogicLayer;
using System.Web;
namespace ERP.OMS.Management.DailyTask
{

    public partial class management_DailyTask_CarryForwardLedger : ERP.OMS.ViewState_class.VSPage
    {
        DailyTask_CarryForwardLedgerBL ODailyTask_CarryForwardLedgerBL = new DailyTask_CarryForwardLedgerBL();

       // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DBEngine oDBEngine = new DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {

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
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int NoofRowsEffect = 0;

            /*
            String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd3 = new SqlCommand("CarryForwardValue", con);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@companyID", Session["LastCompany"].ToString().Trim());
            cmd3.Parameters.AddWithValue("@segmentID", Session["usersegid"].ToString().Trim());
            cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());
            cmd3.Parameters.AddWithValue("@UserID", Session["UserID"].ToString().Trim());
            cmd3.CommandTimeout = 0;
            NoofRowsEffect = cmd3.ExecuteNonQuery();
            con.Close();
            cmd3.Dispose();

            */

            NoofRowsEffect = ODailyTask_CarryForwardLedgerBL.CarryForwardValue(Session["LastCompany"].ToString().Trim(), Session["usersegid"].ToString().Trim(),
                                                                 Session["LastFinYear"].ToString().Trim(), Session["UserID"].ToString().Trim());

            if (NoofRowsEffect > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "alert('Carry Forward Of Balance Successfully !!');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "alert('Next Financial Year Not Opened In the System!!');", true);
        }
    }

}