using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using ERP.Models;

namespace ERP.OMS.Management.Activities
{
    public partial class ReceiveFromServiceCenterList : ERP.OMS.ViewState_class.VSPage
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ReceiveFromServiceCenterList.aspx");
            if (!IsPostBack)
            {
                Session["exportval"] = null;

            }
            //FillGrid();
        }

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Service_Id";


            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
            branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

            var q = from d in dc.V_ReceiveFromServiceCenterLists
                    where d.Service_Company == lastCompany &&
                          branchidlist.Contains(Convert.ToInt32(d.Service_TransferFromBranch))
                    orderby d.Service_Date descending
                    select d;
            e.QueryableSource = q;
        }
        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = new DataTable();

            if (HttpContext.Current.Session["LastCompany"] != null)
            {

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                if (HttpContext.Current.Session["userbranchHierarchy"] != null)
                {
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    dtdata = objCRMSalesOrderDtlBL.GetReceiveFromServiceCentreListGridData(userbranch, lastCompany);
                }
            }


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                GrdOrder.DataSource = dtdata;
                GrdOrder.DataBind();
            }
            else
            {
                GrdOrder.DataSource = null;
                GrdOrder.DataBind();
            }
        }
        public DataTable GetGridData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "SalesOrder");
            dt = proc.GetTable();
            return dt;
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
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
            GrdOrder.Columns[5].Visible = false;
            string filename = "Sales Challan";
            exporter.FileName = filename;
            exporter.FileName = "GrdChallan";

            exporter.PageHeader.Left = "Sales Challan";
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

        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.SalesOrderEditablePermission(Convert.ToInt32(ActiveUser));

            //}
            return Convert.ToString(ispermission);

        }

        protected void GrdOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdOrder.JSProperties["cpinsert"] = null;
            GrdOrder.JSProperties["cpEdit"] = null;
            GrdOrder.JSProperties["cpUpdate"] = null;
            GrdOrder.JSProperties["cpDelete"] = null;
            GrdOrder.JSProperties["cpExists"] = null;
            GrdOrder.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (WhichCall == "Edit")
            {

                DataTable dtOrderStatus = objCRMSalesOrderDtlBL.GetSalesOrderStatusByOrderID(WhichType);


                if (dtOrderStatus.Rows.Count > 0 && dtOrderStatus != null)
                {
                    string orderid = Convert.ToString(dtOrderStatus.Rows[0]["orderid"]);
                    string orderNumber = Convert.ToString(dtOrderStatus.Rows[0]["orderNumber"]);
                    string Status = Convert.ToString(dtOrderStatus.Rows[0]["Status"]);
                    string Remarks = Convert.ToString(dtOrderStatus.Rows[0]["Remarks"]);
                    string CustomerName = Convert.ToString(dtOrderStatus.Rows[0]["CustomerName"]);
                    GrdOrder.JSProperties["cpEdit"] = orderid + "~"
                                                    + orderNumber + "~"
                                                    + Status + "~"
                                                    + Remarks + "~"
                                                    + CustomerName;

                }
            }
            if (WhichCall == "Delete")
            {
                string Company = Convert.ToString(Session["LastCompany"]);
                string Finyear = Convert.ToString(Session["LastFinYear"]);
                deletecnt = objCRMSalesOrderDtlBL.DeleteReceivedFromService(WhichType,Company,Finyear);
                if (deletecnt == 1)
                {
                    //FillGrid();
                    GrdOrder.JSProperties["cpDelete"] = "Deleted successfully";
                    //GetGridData();
                }
                else
                {
                    GrdOrder.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
        }
        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }
    }
}