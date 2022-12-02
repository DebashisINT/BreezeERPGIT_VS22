using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class erpTeamList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/erpTeamList.aspx");
            if (!IsPostBack)
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int NoOfRowEffected = 0;
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_TEAMLIST");
                    proc.AddIntegerPara("@USERID", user_id);
                    NoOfRowEffected = proc.RunActionQuery();
                }
                else
                {

                }
            }
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "TEAM_ID";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string cust_type = "";// ddlType.SelectedValue;
            int userid = Convert.ToInt32(Session["UserID"]);
            List<int> branchidlist;

            //if (cust_type.ToUpper() != "SELECT")
            //{
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.ERP_TEAMVIEWs
                    //where d.ContactType == cust_type
                    orderby d.SEQ
                    select d;
            e.QueryableSource = q;
            //}
            //else
            //{
            //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //    var q = from d in dc.ERP_TEAMVIEWs
            //            //where d.ContactType == cust_type
            //            orderby d.SEQ
            //            select d;
            //    e.QueryableSource = q;
            //}
        }

        [WebMethod]
        public static string DeleteTeam(int TEAM_ID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_TEAMDELETE");
                    proc.AddIntegerPara("@TEAM_ID", TEAM_ID);
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
    }
}