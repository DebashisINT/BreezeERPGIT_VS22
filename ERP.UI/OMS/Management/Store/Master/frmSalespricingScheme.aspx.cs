using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using System.Data.OleDb;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.Store.Master
{
    public partial class management_master_frmSalesPricingScheme : System.Web.UI.Page
    {

        BusinessLogicLayer.ProductSalesPriceImportBL pBl = new ProductSalesPriceImportBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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
            ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (!IsPostBack) 
            {
                BindProClassCode();
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Store/Master/frmSalesPricingScheme.aspx");
        }

        protected void BindProClassCode()
        { 
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("SELECT ProductClass_ID,ProductClass_Name FROM Master_ProductClass order by ProductClass_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(cmbProductClass, dtCmb, "ProductClass_Name", "ProductClass_ID", "");
            }

        }

        protected void btnUploadRecord_Click(object sender, EventArgs e)
        {
            try
            {
                string fieldName = getValueFor();
                string ProdList = GetProductList();
                decimal newPrice = Convert.ToDecimal(txtNewValue.Text);

                pBl.UpdateProductFieldValue(ProdList, newPrice, fieldName, Convert.ToInt32(Session["userid"]));

                grid.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('Updated Successfully.')", true);
            }
            catch (Exception ex) { }
        }

        public string getValueFor()
        {
            string fieldName = "";
            if (CmbValuefor.SelectedItem.Value == "MRP")
            {
                fieldName = "MRP";
            }
            if (CmbValuefor.SelectedItem.Value == "MARKUPMIN")
            {
                fieldName = "MARKUPMIN";
            }
            if (CmbValuefor.SelectedItem.Value == "MARKPLUS")
            {
                fieldName = "MARKPLUS";
            }
            if (CmbValuefor.SelectedItem.Value == "SALEP")
            {
                fieldName = "SALEP";
            }
            if (CmbValuefor.SelectedItem.Value == "MSALEP")
            {
                fieldName = "MSALEP";
            }
            if (CmbValuefor.SelectedItem.Value == "SaleDisc")
            {
                fieldName = "SaleDisc";
            }
            return fieldName;
        }

        public string GetProductList()
        { 
            string ProdList="";
        if (cmbUpdatefor.SelectedItem.Value == "A")
            ProdList = "All_PRODUCT";
        else if (cmbUpdatefor.SelectedItem.Value == "C")
        {
            DataTable dt = oDBEngine.GetDataTable("select sProducts_ID  from Master_sProducts where ProductClass_Code=" + cmbProductClass.SelectedItem.Value);
            foreach (DataRow dr in dt.Rows)
            {
                ProdList += "," + dr["sProducts_ID"];
            }
           ProdList= ProdList.TrimStart(',');
        }
        else if (cmbUpdatefor.SelectedItem.Value == "S")
        {
            List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("sProducts_ID");
            foreach (object Pobj in ComponentList)
            {
                ProdList += "," + Pobj;
            }
            ProdList = ProdList.TrimStart(',');
        }
        return ProdList;
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            if (Session["userid"] != null)
            {
                foreach (var args in e.UpdateValues)
                {
                    string ProdCode = Convert.ToString(args.NewValues["sProducts_Code"]);
                    decimal Mrp = Convert.ToDecimal(args.NewValues["sProduct_MRP"]);
                    decimal markupmin = Convert.ToDecimal(args.NewValues["markupmin"]);
                    decimal markupPlus = Convert.ToDecimal(args.NewValues["markupPlus"]);
                    decimal sProduct_SalePrice = Convert.ToDecimal(args.NewValues["sProduct_SalePrice"]);
                    decimal sProduct_MinSalePrice = Convert.ToDecimal(args.NewValues["sProduct_MinSalePrice"]);
                    decimal DiscountUpTo = Convert.ToDecimal(args.NewValues["DiscountUpTo"]);
                    pBl.UpdateSalesPrice(ProdCode, Mrp, markupmin, markupPlus, sProduct_SalePrice, sProduct_MinSalePrice, Convert.ToInt32(Session["userid"]), DiscountUpTo);
                }
                grid.JSProperties["cpMsg"] = "Updated Successfully";
            }
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
            grid.Columns["sProduct_MRP"].Visible = true;
            string filename = "Sales Pricing Scheme";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Sales Pricing Scheme";
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
    }
}