using System;
using System.Data;
using System.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Collections.Generic;
using System.Configuration;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System.Linq;

namespace ERP.OMS.Management.Activities
{
    public partial class PosMassBranch : System.Web.UI.Page
    {

       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
              rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PosSalesInvoiceList.aspx");

              if (!IsPostBack)
              {
                  if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                  {
                      string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                      string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                      PopulateBranch(userbranchHierachy, Convert.ToString(Session["userbranchID"]));
                      //massBranch.DataBind();
                  }
              }
        }
        protected void massBranch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //   string JsonString = e.Parameters;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //List<MassBranchAssign> collection = serializer.Deserialize<List<MassBranchAssign>>(JsonString);
            List<object> receiptList = massBranch.GetSelectedFieldValues("Invoice_Id");
            foreach (var invId in receiptList)
            {
                posSale.SetMassAssignBranch(Convert.ToInt32(invId), Convert.ToInt32(MassBranchId.Value));
            }
            //massBranch.DataBind();
            massBranch.JSProperties["cpMsg"] = "Updated Successfully.";
        }
        protected void massBranch_DataBinding(object sender, EventArgs e)
        {
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

            massBranch.DataSource = posSale.GetMassbranchPosDetails(userbranchHierachy, lastCompany);
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

           // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
            branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
            var q = from d in dc.V_PosMassBranchLists
                    where d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                          branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId))
                    orderby d.Invoice_DateTime descending
                    select d;
            e.QueryableSource = q;
        }
        private void PopulateBranch(string userbranchhierchy, string UserBranch)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();

            BranchAssignmentBranch.DataSource = posSale.getBranchListByBranchList(userbranchhierchy, UserBranch);
            BranchAssignmentBranch.ValueField = "branch_id";
            BranchAssignmentBranch.TextField = "branch_description";
            BranchAssignmentBranch.DataBind();
            BranchAssignmentBranch.Value = "0";
           

            MassBranchId.DataSource = LoadBranch();
            MassBranchId.ValueField = "branch_id";
            MassBranchId.TextField = "branch_description";
            MassBranchId.DataBind();
            MassBranchId.Value = Convert.ToString(Session["userbranchID"]);
        }
        public DataTable LoadBranch()
        {
            string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

            return posSale.getBranchListByBranchListForMassBranch(userbranchHierachy, Convert.ToString(Session["userbranchID"]));
        }

        protected void BranchAssigncmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(DropDownList1.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchAssignbindexport(Filter);
               
            }
        }
        public void BranchAssignbindexport(int Filter)
        {
            //GrdQuotation.Columns[6].Visible = false;
            string filename = "Branch Assignment";
            exporter.FileName = filename;
            exporter.FileName = "BranchAssignment";

            exporter.PageHeader.Left = "Branch Assignment";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.GridViewID = "massBranch";
            exporter.Landscape = true;
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
       
        
        #region Assignment
        protected void AssignmentGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            if (e.Parameters.Split('~')[0] == "AssignBranch")
            {
                int assignBranch = Convert.ToInt32(e.Parameters.Split('~')[2]);
                int warehouse = Convert.ToInt32(e.Parameters.Split('~')[3]);
                int invoiceid = Convert.ToInt32(e.Parameters.Split('~')[1]);
                posSale.UpdateAssignBranch(assignBranch, warehouse, invoiceid);
                AssignmentGrid.JSProperties["cpMsg"] = "Updated Successfully.";
            }
            else
            {
                string invoiceId = e.Parameters.Split('~')[0];
                string BranchId = e.Parameters.Split('~')[1];
                DataTable availableStock = posSale.GetBranchAssignmentDetails(Convert.ToInt32(invoiceId), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt32(BranchId));
                Session["BranchAssignmentTableForGrid"] = availableStock;
                AssignmentGrid.DataBind();
            }
        }
        protected void AssignmentGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable availableStock = (DataTable)Session["BranchAssignmentTableForGrid"];
            AssignmentGrid.DataSource = availableStock;
        }

        
        
        #endregion
    }
}