using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.Inventory
{
    public partial class InventoryDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                PopulateBranchByHierchy(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));


                ProcedureExecute proc = new ProcedureExecute("prc_DashboardSettingModelWise");
                proc.AddPara("@SETTINGS_NAME", "Inventory Analytics");
                proc.AddPara("@user_id", Convert.ToString(Session["userid"]));
                DataTable dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    divStockStatus.Visible = Convert.ToBoolean(dt.Rows[0]["IAStockStatus"]);
                    divTopNProduct.Visible = Convert.ToBoolean(dt.Rows[0]["IATopNProduct"]);
                    divActualvsDemand.Visible = Convert.ToBoolean(dt.Rows[0]["IAActualVsDemand"]);
                    divStockAlert.Visible = Convert.ToBoolean(dt.Rows[0]["IAStockAlert"]);
                    divStockRequisition.Visible = Convert.ToBoolean(dt.Rows[0]["IAStockRequisition"]);

                    divProcurementRequisition.Visible = Convert.ToBoolean(dt.Rows[0]["IAProcurementRequisition"]);

                    fTab.Visible = Convert.ToBoolean(dt.Rows[0]["IAStockStatus"]);
                    STab.Visible = Convert.ToBoolean(dt.Rows[0]["IATopNProduct"]);
                    tTab.Visible = Convert.ToBoolean(dt.Rows[0]["IAActualVsDemand"]);
                    frTab.Visible = Convert.ToBoolean(dt.Rows[0]["IAStockAlert"]);
                    fvTab.Visible = Convert.ToBoolean(dt.Rows[0]["IAStockRequisition"]);

                    fsTab.Visible = Convert.ToBoolean(dt.Rows[0]["IAProcurementRequisition"]);
                   
                }
                else
                {
                    divStockStatus.Visible = Convert.ToBoolean(0);
                    divTopNProduct.Visible = Convert.ToBoolean(0);
                    divActualvsDemand.Visible = Convert.ToBoolean(0);
                    divStockAlert.Visible = Convert.ToBoolean(0);
                    divStockRequisition.Visible = Convert.ToBoolean(0);
                    divProcurementRequisition.Visible = Convert.ToBoolean(0);
                    
                    fTab.Visible = Convert.ToBoolean(0);
                    STab.Visible = Convert.ToBoolean(0);
                    tTab.Visible = Convert.ToBoolean(0);
                    frTab.Visible = Convert.ToBoolean(0);
                    fvTab.Visible = Convert.ToBoolean(0);
                    fsTab.Visible = Convert.ToBoolean(0);
                }
            }
        }

        [WebMethod]
        //for Activities
        public static object GetStockRequisition(string asondate)
        {


            List<StockRequisition> lEfficency = new List<StockRequisition>();
            ProcedureExecute proc = new ProcedureExecute("PRC_STOCKREQDB_REPORT");
            proc.AddVarcharPara("@ASONDATE", 100, asondate);
            //proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new StockRequisition()
                          {
                              BRANCHREQ = Convert.ToString(dr["BRANCHREQ"]),
                              APPRPENDING = Convert.ToString(dr["APPRPENDING"]),
                              OPENREQ = Convert.ToString(dr["OPENREQ"]),
                              CLOSEREQ = Convert.ToString(dr["CLOSEREQ"]),
                              APPRREQ = Convert.ToString(dr["APPRREQ"]),
                          }).ToList();
            return lEfficency;
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            ddlBranch.DataSource = branchtable;
            ddlBranch.ValueField = "branch_id";
            ddlBranch.TextField = "branch_description";
            ddlBranch.DataBind();
            ddlBranch.SelectedIndex = 0;
        }
        public class StockRequisition
        {
            public string BRANCHREQ	{ get; set; }
            public string APPRPENDING { get; set; }
            public string OPENREQ { get; set; }
            public string CLOSEREQ { get; set; }
            public string APPRREQ { get; set; }
        }
    }
}