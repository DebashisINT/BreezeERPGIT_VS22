using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;

namespace ERP.OMS.Management.Activities
{
    public partial class StockTransfer : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();

        public string pageAccess = "";
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesQuotation.aspx");
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            //if (!IsPostBack)
            //{
            //    dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            //    dt_PlQuoteExpiry.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            //    string finyear = Convert.ToString(Session["LastFinYear"]);
            //    GetAllDropDownDetailForSalesQuotation();
            //    Session["CustomerDetail"] = null;
            //}
            //PopulateCustomerDetail();

        }
        public void PopulateCustomerDetail()
        {
            //if (Session["CustomerDetail"] == null)
            //{
            //    DataTable dtCustomer = new DataTable();
            //    dtCustomer = objSlaesActivitiesBL.PopulateCustomerDetail();

            //    if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            //    {
            //        lookup_Customer.DataSource = dtCustomer;
            //        lookup_Customer.DataBind();
            //        Session["CustomerDetail"] = dtCustomer;
            //    }
            //}
            //else
            //{
            //    lookup_Customer.DataSource = (DataTable)Session["CustomerDetail"];
            //    lookup_Customer.DataBind();
            //}

        }
        public void GetAllDropDownDetailForSalesQuotation()
        {
            //DataSet dst = new DataSet();
            //dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesQuotation();
            //if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            //{
            //    lookup_Customer.DataSource = dst.Tables[0];
            //    lookup_Customer.DataBind();  
            //    //ddl_Customer.DataTextField = "Name";
            //    //ddl_Customer.DataValueField = "cnt_internalid";
            //    //ddl_Customer.DataSource = dst.Tables[0];
            //    //ddl_Customer.DataBind();

            //}
            //if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            //{
            //    ddl_Branch.DataTextField = "branch_code";
            //    ddl_Branch.DataValueField = "branch_internalId";
            //    ddl_Branch.DataSource = dst.Tables[0];
            //    ddl_Branch.DataBind();
            //}
        }

         

        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                BindContactPerson(InternalId);
            }
        }

        protected void BindContactPerson(string InternalId)
        {
            string ContactPerson = "";
            DataSet dstContactPerson = new DataSet();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            //dtContactPerson = objSlaesActivitiesBL.PopulateContactPersonForCustomerOrLead(InternalId);
            //oGenericMethod = new BusinessLogicLayer.GenericMethod();

            //DataTable dtCmb = new DataTable();
            //dtCmb = oGenericMethod.GetDataTable("Select city_id,city_name From tbl_master_city Where state_id=" + stateID + " Order By city_name");//+ " Order By state "
            //AspxHelper oAspxHelper = new AspxHelper();
            //if (dtCmb.Rows.Count > 0)
            //{
            //    //CmbState.Enabled = true;
            //    // oAspxHelper.Bind_Combo(CmbCity, dtCmb, "city_name", "city_id", 0);
            //}
            //else
            //{
            //    //CmbCity.Enabled = false;
            //}
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //grid.DataSource = GetVoucher();
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {


        }

    }
}