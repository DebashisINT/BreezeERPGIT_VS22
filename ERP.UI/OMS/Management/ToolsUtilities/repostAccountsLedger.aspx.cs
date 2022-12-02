using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_toolsutilities_repostAccountsLedger : System.Web.UI.Page
    {
        string Error = "a";
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        Utilities obj = new Utilities();
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
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            }
        }
        protected void ASPxComboBox1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            int NoOfRowEffectedRow = 0;
            DataTable DtCurrentSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            string CompanyID = Session["LastCompany"].ToString();
            string SegmentID = DtCurrentSegment.Rows[0][0].ToString();
            //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand lcmd = new SqlCommand("RecalculateCashBankJournalToAccountsLedger", lcon))
            //    {
            //        lcmd.CommandTimeout = 0;
            //        lcmd.CommandType = CommandType.StoredProcedure;
            //        lcmd.Parameters.Add("@MainCompanyId", SqlDbType.VarChar, 20).Value = CompanyID;
            //        lcmd.Parameters.Add("@MainSegmentId", SqlDbType.Int).Value = Convert.ToInt32(SegmentID);
            //        lcmd.Parameters.Add("@FinYear1", SqlDbType.VarChar).Value = Session["LastFinYear"].ToString();
            //        lcon.Open();
            //        NoOfRowEffectedRow = lcmd.ExecuteNonQuery();
            //    }
            //}
            NoOfRowEffectedRow = obj.RecalculateCashBankJournalToAccountsLedger(CompanyID, SegmentID, Session["LastFinYear"].ToString());
            if (NoOfRowEffectedRow > 0)
            {
                Error = "b";
            }
        }
        protected void ASPxComboBox1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpInsertError"] = Error;
        }
    }
}