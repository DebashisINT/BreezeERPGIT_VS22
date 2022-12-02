using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

namespace ERP.OMS.Reports.Master
{
    public partial class RptProductMasterReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void ProductGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            string command = e.Parameters.ToString();

            if (command == "SelectAll")
            {
                for (int i = 0; i < gv.VisibleRowCount; i++)
                {
                    gv.Selection.SelectRow(i);
                }
            }
            if (command == "UnSelectAll")
            {
                for (int i = 0; i < gv.VisibleRowCount; i++)
                {
                    gv.Selection.UnselectRow(i);
                }
            }
            if (command == "Revart")
            {
                for (int i = 0; i < gv.VisibleRowCount; i++)
                {
                    if (gv.Selection.IsRowSelected(i))
                        gv.Selection.UnselectRow(i);
                    else
                        gv.Selection.SelectRow(i);
                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string SelectedProdList = "";
            
            //List<object> prodList = ProductGrid.GetSelectedFieldValues("sProducts_ID");
            List<object> prodList = GridLookup.GridView.GetSelectedFieldValues("sProducts_ID");
            foreach (object Pobj in prodList)
            {
                SelectedProdList += "," + Pobj;
            }
            SelectedProdList = SelectedProdList.TrimStart(',');
            if (SelectedProdList.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('Please Select Some Product(s)')", true);
            }
            else
            {
                Session["SelectedProductList"] = SelectedProdList;
                Response.Redirect("~/OMS/Reports/XtraReports/ProductReportViewer.aspx");
            }
        }


        protected void ProductCallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string componet = Convert.ToString(e.Parameter);
            ASPxCallbackPanel cbPanl = source as ASPxCallbackPanel;
            ASPxGridLookup LookUp = (ASPxGridLookup)cbPanl.FindControl("GridLookup");


            if (componet == "SelectAll")
            {
                for (int i = 0; i < LookUp.GridView.VisibleRowCount; i++)
                {
                    LookUp.GridView.Selection.SelectRow(i);
                }
            }
            if (componet == "UnSelectAll")
            {
                for (int i = 0; i < LookUp.GridView.VisibleRowCount; i++)
                {
                    LookUp.GridView.Selection.UnselectRow(i);
                }
            }
            if (componet == "Revart")
            {
                for (int i = 0; i < LookUp.GridView.VisibleRowCount; i++)
                {
                    if (LookUp.GridView.Selection.IsRowSelected(i))
                        LookUp.GridView.Selection.UnselectRow(i);
                    else
                        LookUp.GridView.Selection.SelectRow(i);
                }
            }
        }
    }
}