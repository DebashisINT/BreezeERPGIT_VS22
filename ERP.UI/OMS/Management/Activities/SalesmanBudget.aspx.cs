using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTabControl;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using ERP.OMS.CustomFunctions;
using System.Text;
using System.Reflection;
using DevExpress.Web;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Activities
{
    public partial class SalesmanBudget : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BudgetDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            BindGrid();
        }

        void BindGrid()
        {
            try
            {
                string typeid = Request.QueryString["tid"];
                string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID
                string FinYear = String.Empty;

                if (Session["LastFinYear"] != null)
                {
                    FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
                }
                string AssignedID = Convert.ToString(HttpContext.Current.Session["cntId"]);

                BudgetDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
                BudgetDataSource.SelectCommand = "sp_Sales";
                BudgetDataSource.SelectParameters.Clear();

                if (typeid == "1")
                { BudgetDataSource.SelectParameters.Add("Mode", "GetSalesmanBudgetInfo"); }
                else if (typeid == "2") { BudgetDataSource.SelectParameters.Add("Mode", "GetAllSalesmanBudgetInfo"); }
                else if (typeid == "3")
                {
                    string CId = Convert.ToString(Request.QueryString["CId"]);
                    string type = Convert.ToString(Request.QueryString["type"]);
                    string SalId = Convert.ToString(Request.QueryString["SalId"]);
                    BudgetDataSource.SelectParameters.Add("Mode", "GetSalesmanBudgetCallRelatedInfo");
                    BudgetDataSource.SelectParameters.Add("cntid", CId);
                    BudgetDataSource.SelectParameters.Add("Tid", type);
                    BudgetDataSource.SelectParameters.Add("sls_ID", SalId);
                }

                BudgetDataSource.SelectParameters.Add("Finyear", FinYear);
                BudgetDataSource.SelectParameters.Add("AssignedID", AssignedID);
                BudgetDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
                BudgetGrid.DataBind();

                if (typeid == "1" || (typeid == "3"))
                {
                    BudgetGrid.Columns["act_activityNo"].Visible = false;
                }
                else
                { BudgetGrid.Columns["act_activityNo"].Visible = true; }
            }
            catch { }
        }

     
    
    }
}