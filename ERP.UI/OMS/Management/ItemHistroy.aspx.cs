using System;
using System.Web;
//using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using DevExpress.Web;

namespace ERP.OMS.Management
{

    public partial class management_ItemHistroy : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        string FinYear = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            sqlItemHistory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            string str = Request.QueryString["id"].ToString();
            string[] strname = str.Split('[');
            string[] ItemName = oDBEngine.GetFieldValue1("Master_SubAccount", "SubAccount_Code", "SubAccount_Name='" + strname[0] + "'", 1);

            sqlItemHistory.SelectCommand = "Select  M.AssetDetail_ID,M.AssetDetail_CompanyID,M.AssetDetail_FinYear,M.AssetDetail_MainAccountCode,M.AssetDetail_SubAccountCode,M.AssetDetail_Category,M.AssetDetail_PurchaseDate,M.AssetDetail_Vendor,M.AssetDetail_CostPrice,M.AssetDetail_Additions,M.AssetDetail_Disposals,M.AssetDetail_Depreciation,M.Assetdetail_DepreciationIT,M.AssetDetail_Location,M.AssetDetail_User,M.AssetDetail_Insurer,M.AssetDetail_BroughtForward,(M.AssetDetail_Broughtforward+M.AssetDetail_CostPrice+M.AssetDetail_Additions-M.AssetDetail_Disposals-M.AssetDetail_Depreciation) as NetValue from Master_AssetDetail M where M.AssetDetail_SubAccountcode='" + ItemName[0] + "' And M.AssetDetail_FinYear<'" + FinYear + "'Order By M.[AssetDetail_FinYear]";
            gridHistory.DataBind();

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
        protected void gridHistory_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
            {
                gridHistory.Settings.ShowFilterRow = true;

            }

            if (e.Parameters == "All")
            {
                gridHistory.FilterExpression = string.Empty;
            }
        }
        protected void gridHistory_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

        }
        protected void gridHistory_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                if (e.GetValue("AssetDetail_BroughtForward") != DBNull.Value)
                {
                    e.Row.Cells[0].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_BroughtForward")));

                }

                if (e.GetValue("AssetDetail_CostPrice") != DBNull.Value)
                {
                    e.Row.Cells[3].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_CostPrice")));

                }
                if (e.GetValue("AssetDetail_Additions") != DBNull.Value)
                {
                    e.Row.Cells[4].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_Additions")));
                }
                if (e.GetValue("AssetDetail_Disposals") != DBNull.Value)
                {
                    e.Row.Cells[5].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_Disposals")));

                }
                if (e.GetValue("AssetDetail_Depreciation") != DBNull.Value)
                {
                    e.Row.Cells[6].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_Depreciation")));

                }
                if (e.GetValue("Assetdetail_DepreciationIT") != DBNull.Value)
                {
                    e.Row.Cells[7].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("Assetdetail_DepreciationIT")));

                }
                if (e.GetValue("NetValue") != DBNull.Value)
                {
                    e.Row.Cells[8].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("NetValue")));

                }

            }
        }
    }
}