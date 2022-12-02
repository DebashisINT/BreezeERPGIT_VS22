using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class OrderDeliveryScheduleList : System.Web.UI.Page
    {
        DataTable dst = new DataTable();
        string strBranchID = "";
        // Consolidatecustomer obj = new Consolidatecustomer();
           public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesOrderEntityList.aspx");

            if (!IsPostBack)
            {
                Session["SI_ComponentDataTagged"] = null;
                string OrderId = Request.QueryString["Key"];
                hdOrderId.Value = OrderId;
               // hdOrderdetailsID.Value = Request.QueryString["Key"];
                gridsalesOrderProduct.DataSource = GetSalesorderProductGridData();
                gridsalesOrderProduct.DataBind();
            }
        }
        public DataTable GetSalesorderProductGridData()
        {
            try
            {
                DataTable dt = GetProductDetails("GetProductBySalesOrderID", Convert.ToString(hdOrderId.Value));
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetProductDetails(string Action, string OrderId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddPara("@Action", Action);
            proc.AddPara("@DocID", OrderId);
            dt = proc.GetTable();
            return dt;
        }
        protected void GrdConsolidatedCustomer_DataBinding(object sender, EventArgs e)
        {
            gridsalesOrderProduct.DataSource = GetSalesorderProductGridData();
        }

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }


        public void bindexport(int Filter)
        {
            //GrdReplacement.Columns[6].Visible = false;
            string filename = "Delivery Schedule ";
            exporter.FileName = filename;
            exporter.FileName = "DeliverySchedule ";

            exporter.PageHeader.Left = "Delivery Schedule";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        #region ########  Branch Populate  #######
        //protected void Branchpopulate()
        //{
        //    string userbranchID = Convert.ToString(Session["userbranchID"]);
        //    dst = obj.GetBranch(Convert.ToInt32(HttpContext.Current.Session["userbranchID"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

        //    if (dst.Rows.Count > 0)
        //    {

        //        ddl_Branch.DataSource = dst;
        //        ddl_Branch.DataTextField = "branch_code";
        //        ddl_Branch.DataValueField = "branch_id";
        //        ddl_Branch.DataBind();
        //        // ddl_Branch.SelectedValue = strBranchID;

        //        if (Cache["name"] != null)
        //        {
        //            ddl_Branch.SelectedValue = Cache["name"].ToString();
        //        }
        //        else if (Session["userbranchID"] != null)
        //        {
        //            ddl_Branch.SelectedValue = userbranchID;
        //        }
        //    }
        //}

        #endregion

    }
}