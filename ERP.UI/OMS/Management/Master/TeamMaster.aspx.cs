using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class TeamMaster : System.Web.UI.Page
    {
        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/DashBoardSetting/DashBoardSettingList.aspx");
            try
            {
                if (!IsPostBack)
                {
                    EmployeeGrid.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void EmployeeGrid_DataBinding(object sender, EventArgs e)
        {
            string output = string.Empty;
            try
            {

                DataTable dt = new DataTable();
                //int NoOfRowEffected = 0;
                // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                dt = oDBEngine.GetDataTable(@"SELECT TEAM_NAME,TEAM_DESCRIPTION,isnull(usr1.user_name,usr2.user_name) cr_by,ISNULL(MODIFIED_ON,CREATED_ON) mod_by  FROM Master_Team team LEFT JOIN tbl_master_user usr1 on team.CREATED_BY=usr1.user_id LEFT JOIN tbl_master_user usr2 on team.MODIFIED_BY=usr2.user_id");
             
                EmployeeGrid.DataSource = dt;
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }


        }


        #region webmethod

        [WebMethod]
        public static string DeleteDashBoard(string id)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_dashboardsetting");
                proc.AddVarcharPara("@Action", 20, "delete");
                proc.AddBigIntegerPara("@ID", Convert.ToInt64(id));
                proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                NoOfRowEffected = proc.RunActionQuery();
                output = Convert.ToString(proc.GetParaValue("@is_success"));
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }


            return output;

        }
        #endregion
    }
}