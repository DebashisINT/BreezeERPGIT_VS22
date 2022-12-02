using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CutOff.CutOff.Master
{
    public partial class FinYearList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        ProcedureExecute objProcedureExecute = new ProcedureExecute();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frm_FinancialYear.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                ////Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            // //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (!IsPostBack)
            {
                Session["exportval"] = null;
            }

            fillGrid();
        }
        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string tranid = e.Parameters.ToString();

            if (e.Parameters.ToString().Split('~')[0] == "Delete")
            {
                gridStatus.Settings.ShowFilterRow = true;
            }
            //else if (e.Parameters == "All")
            //{
            //    gridStatus.FilterExpression = string.Empty;
            //}

            if (e.Parameters == "All")
            {
                gridStatus.FilterExpression = string.Empty;
            }

            fillGrid();

        }
        public void fillGrid()
        {
            gridStatusDataSource.SelectCommand = "select FinYear_ID,FinYear_Code,convert(varchar,FinYear_StartDate,106) as FinYear_StartDate,convert(varchar,FinYear_EndDate,106) as FinYear_EndDate,FinYear_Remarks from master_finyear";
            gridStatus.DataBind();

        }

        public void DeleteFinancialYear()
        {

        }
    }
}