using OpeningBusinessLogic;
using BusinessLogicLayer.Replacement;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpeningBusinessLogic.Vendorconsolidate;

namespace OpeningEntry.ERP
{
    public partial class ConsolidatedVendorList : System.Web.UI.Page
    {
        DataTable dst = new DataTable();
        string strBranchID = "";
        VendorConsolidate obj = new VendorConsolidate();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
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

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/ConsolidatedVendorList.aspx");
            if (!IsPostBack)
            {
                Session["SI_ComponentDataTagged"] = null;
                Branchpopulate();
                Grdconsolidatecustomer.DataSource = GetConsolidatedCustomerListGridData();
                Grdconsolidatecustomer.DataBind();
            }
        }



        #region ########  Branch Populate  #######
        protected void Branchpopulate()
        {
            string userbranchID = Convert.ToString(Session["userbranchID"]);
            dst = obj.GetBranch(Convert.ToInt32(HttpContext.Current.Session["userbranchID"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (dst.Rows.Count > 0)
            {

                ddl_Branch.DataSource = dst;
                ddl_Branch.DataTextField = "branch_code";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataBind();
                // ddl_Branch.SelectedValue = strBranchID;

                if (Cache["name_vendor"] != null)
                {
                    ddl_Branch.SelectedValue = Cache["name_vendor"].ToString();
                }
                else if (Session["userbranchID"] != null)
                {
              
                    ddl_Branch.SelectedValue = userbranchID;
                }
            }
        }

        #endregion
        public DataTable GetConsolidatedCustomerListGridData()
        {
            try
            {

                DataTable dt = obj.GetCustomesconsolidate("ListWiseCustomer", Int32.Parse(ddl_Branch.SelectedValue));
                return dt;
            }
            catch
            {
                return null;
            }

        }
        protected void GrdConsolidatedCustomer_DataBinding(object sender, EventArgs e)
        {
            Grdconsolidatecustomer.DataSource = GetConsolidatedCustomerListGridData();
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
               //     Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                //    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            //GrdReplacement.Columns[6].Visible = false;
            string filename = "ConsolidatedVendor";
            exporter.FileName = filename;
            exporter.FileName = "ConsolidatedVendor";

            exporter.PageHeader.Left = "Consolidated Vendor";
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


        protected void OpeningGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Cache["name_vendor"] != null)
            {
                Cache.Remove("name_vendor");
            }
            string returnPara = Convert.ToString(e.Parameters);
            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "TemporaryData")
            {
                Grdconsolidatecustomer.DataSource = GetConsolidatedCustomerListGridData();
                Grdconsolidatecustomer.DataBind();

            }
        }


        #region Tagged documents
        protected void OpeningGrid_CustomCallbacktaggeddoc(object sourc, ASPxGridViewCustomCallbackEventArgs e)
        {

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameters.Split('~')[0] == "BindComponentGrid")
            {

                string CustomerId = e.Parameters.Split('~')[1];



                DataTable dt = obj.GetCustomesconsolidateTagged("TaggedDocument", CustomerId);

                if (dt.Rows.Count > 0)
                {

                    Session["SI_ComponentDataTagged"] = dt;

                    grid_taggeddocuments.DataSource = dt;
                    grid_taggeddocuments.DataBind();

                }
                else
                {
                    Session["SI_ComponentDataTagged"] = null;
                    grid_taggeddocuments.DataSource = null;
                    grid_taggeddocuments.DataBind();

                }
            }
        }

        protected void GrdConsolidatedtagged_DataBinding(object sender, EventArgs e)
        {
            //   DataTable ComponentTable = new DataTable();

            if (Session["SI_ComponentDataTagged"] != null)
            {
                grid_taggeddocuments.DataSource = (DataTable)Session["SI_ComponentDataTagged"];
            }
        }
        #endregion
    }
}