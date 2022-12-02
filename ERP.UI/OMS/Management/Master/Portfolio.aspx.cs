using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web;
using DevExpress.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Portfolio : ERP.OMS.ViewState_class.VSPage
    {
        //Converter oconverter = new Converter();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        string CheckID = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlPortFolio.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                BindStock();
                //Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>height();</script>");
            }
            BindGridView_forPortfolio();
        }
        public void BindStock()
        {
            ArrayList list = new ArrayList();
            if (Request.QueryString["type"].ToString() == "T")
            {
                list.Insert(0, new ListItem("Pro-Trading", "T"));
                list.Insert(1, new ListItem("Pro-Investment", "I"));
            }
            else if (Request.QueryString["type"].ToString() == "S")
            {
                list.Insert(0, new ListItem("Client", "C"));
            }
            ddlStockFor.DataSource = list;
            ddlStockFor.DataBind();

        }
        public void BindGridView_forPortfolio()
        {
            string StockFor = null;
            if (ddlStockFor.SelectedItem.Text == "Pro-Trading")
                StockFor = "T";
            else if (ddlStockFor.SelectedItem.Text == "Pro-Investment")
                StockFor = "I";
            else if (ddlStockFor.SelectedItem.Text == "Client")
                StockFor = "C";
            SqlPortFolio.SelectCommand = "select PortfolioDetail_ID,(select isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' from tbl_master_contact where cnt_internalID=PortfolioDetail_CustomerID) as PortfolioDetail_CustomerID,(select isnull(rtrim(Equity_TickerSymbol),'')+'['+isnull(rtrim(Equity_Series),'')+']' from Master_Equity where Equity_SeriesID=PortfolioDetail_SeriesID) as PortfolioDetail_ProductID,PortfolioDetail_BuyQuantity,PortfolioDetail_NetAverageUnitCost,PortfolioDetail_NetValue,PortfolioDetail_HostoricalCost,convert(varchar(12),cast(PortfolioDetail_TradeDate as datetime),113) as PortfolioDetail_TradeDate from Trans_PortfolioDetail where PortfolioDetail_CustomerID='" + txtCustomerID_hidden.Value + "' and PortfolioDetail_For='" + StockFor + "' and PortfolioDetail_FinYear='" + Session["LastFinYear"].ToString().Trim() + "' and PortfolioDetail_CompanyID='" + Session["LastCompany"].ToString() + "'";
            gridPortFolio.DataBind();

        }
        protected void gridPortFolio_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGridView_forPortfolio();
        }

        protected void gridPortFolio_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string Mode = null;
            string id = e.EditingKeyValue.ToString();
            Session["PortID"] = id;

            HiddenField txtSeriesID_hidden = (HiddenField)gridPortFolio.FindEditFormTemplateControl("txtSeriesID_hidden");
            HiddenField txtISIN_hidden = (HiddenField)gridPortFolio.FindEditFormTemplateControl("txtISIN_hidden");
            TextBox txtSeriesID = (TextBox)gridPortFolio.FindEditFormTemplateControl("txtSeriesID");
            ASPxTextBox txtBuyQty = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtBuyQty");
            ASPxTextBox txtNetAvgCost = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtNetAvgCost");
            ASPxTextBox txtHistoricPrice = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtHistoricPrice");
            ASPxTextBox txtSecTax = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtSecTax");
            TextBox txtISIN = (TextBox)gridPortFolio.FindEditFormTemplateControl("txtISIN");
            DropDownList ddlHoldingMode = (DropDownList)gridPortFolio.FindEditFormTemplateControl("ddlHoldingMode");
            ASPxDateEdit dtAcquiredDate = (ASPxDateEdit)gridPortFolio.FindEditFormTemplateControl("dtAcquiredDate");

            DataTable dtPortfolio = objEngine.GetDataTable("Trans_PortfolioDetail", " PortfolioDetail_ID,(select isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' from tbl_master_contact where cnt_internalID=PortfolioDetail_CustomerID) as PortfolioDetail_CustomerID,(select isnull(rtrim(Equity_TickerSymbol),'')+'['+isnull(rtrim(Equity_Series),'')+']' from Master_Equity where Equity_SeriesID=PortfolioDetail_SeriesID) as PortfolioDetail_ProductID,PortfolioDetail_BuyQuantity,PortfolioDetail_NetAverageUnitCost,PortfolioDetail_NetValue,PortfolioDetail_HostoricalCost,PortfolioDetail_HoldingMode,PortfolioDetail_ISIN,PortfolioDetail_STT,PortfolioDetail_CustomerID,PortfolioDetail_ProductID as ProductID,PortfolioDetail_SeriesID,PortfolioDetail_TradeDate ", " PortfolioDetail_ID=" + id + "");
            if (dtPortfolio.Rows.Count > 0)
            {
                txtSeriesID.Text = dtPortfolio.Rows[0]["PortfolioDetail_ProductID"].ToString();
                txtBuyQty.Value = dtPortfolio.Rows[0]["PortfolioDetail_BuyQuantity"].ToString();
                txtNetAvgCost.Value = dtPortfolio.Rows[0]["PortfolioDetail_NetAverageUnitCost"].ToString();
                txtHistoricPrice.Value = dtPortfolio.Rows[0]["PortfolioDetail_HostoricalCost"].ToString();
                txtSecTax.Value = dtPortfolio.Rows[0]["PortfolioDetail_STT"].ToString();
                txtISIN.Text = dtPortfolio.Rows[0]["PortfolioDetail_ISIN"].ToString();
                ddlHoldingMode.SelectedValue = dtPortfolio.Rows[0]["PortfolioDetail_HoldingMode"].ToString();
                Mode = dtPortfolio.Rows[0]["PortfolioDetail_HoldingMode"].ToString();
                txtISIN_hidden.Value = dtPortfolio.Rows[0]["PortfolioDetail_ISIN"].ToString();
                txtSeriesID_hidden.Value = dtPortfolio.Rows[0]["ProductID"].ToString() + "~" + dtPortfolio.Rows[0]["PortfolioDetail_SeriesID"].ToString();
                dtAcquiredDate.Value = Convert.ToDateTime(dtPortfolio.Rows[0]["PortfolioDetail_TradeDate"].ToString());
            }
            txtSeriesID.Enabled = false;
            ddlHoldingMode.Enabled = false;
            txtISIN.Enabled = false;
            CheckID = Mode;
        }
        protected void gridPortFolio_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpExist"] = CheckID;
        }
        protected void gridPortFolio_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            Session["PortID"] = null;
        }
        protected void gridPortFolio_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string Id = e.Keys[0].ToString();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd3 = new SqlCommand("DeletePortFolio", con))
                {
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@ID", Convert.ToInt32(Id));
                    cmd3.CommandTimeout = 0;
                    cmd3.ExecuteNonQuery();
                }
                GC.Collect();
                // Mantis Issue 24802
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                // End of Mantis Issue 24802
            }
            e.Cancel = true;
            CheckID = "Delete";
        }
        protected void gridPortFolio_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            HiddenField txtSeriesID_hidden = (HiddenField)gridPortFolio.FindEditFormTemplateControl("txtSeriesID_hidden");
            ASPxTextBox txtBuyQty = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtBuyQty");
            ASPxTextBox txtNetAvgCost = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtNetAvgCost");
            ASPxTextBox txtHistoricPrice = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtHistoricPrice");
            ASPxTextBox txtSecTax = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtSecTax");
            HiddenField txtISIN_hidden = (HiddenField)gridPortFolio.FindEditFormTemplateControl("txtISIN_hidden");
            DropDownList ddlHoldingMode = (DropDownList)gridPortFolio.FindEditFormTemplateControl("ddlHoldingMode");
            ASPxDateEdit dtAcquiredDate = (ASPxDateEdit)gridPortFolio.FindEditFormTemplateControl("dtAcquiredDate");

            string StockFor = null;
            string ISIN = null;
            string InUpdateType = null;
            int ID = 0;
            if (ddlStockFor.SelectedItem.Text == "Pro-Trading")
                StockFor = "T";
            else if (ddlStockFor.SelectedItem.Text == "Pro-Investment")
                StockFor = "I";
            else if (ddlStockFor.SelectedItem.Text == "Client")
                StockFor = "C";
            string[] openBal = Session["LastFinYear"].ToString().Split('-');
            DateTime OpeningBalance = Convert.ToDateTime("04/01/" + openBal[0].ToString());
            string[] ProductSeries = txtSeriesID_hidden.Value.Split('~');
            string ProductID = ProductSeries[0].ToString();
            string SeriesID = ProductSeries[1].ToString();
            if (ddlHoldingMode.SelectedValue == "P")
                ISIN = "NA";
            else
                ISIN = txtISIN_hidden.Value;
            if (Session["PortID"] == null)
            {
                ID = 0;
                InUpdateType = "Insert";
            }
            else
            {
                ID = Convert.ToInt32(Session["PortID"].ToString());
                InUpdateType = "Update";
            }
            string AccquiredDate = Convert.ToDateTime(dtAcquiredDate.Value).ToShortDateString();

            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd3 = new SqlCommand("InsertUpdatePortfolio", con))
                {
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@For", StockFor);
                    cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                    cmd3.Parameters.AddWithValue("@CustomerID", txtCustomerID_hidden.Value);
                    cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                    cmd3.Parameters.AddWithValue("@Segment", Convert.ToInt32(Session["usersegid"].ToString()));
                    cmd3.Parameters.AddWithValue("@Exchange", Convert.ToInt32(Session["ExchangeSegmentID"].ToString()));
                    cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ProductID));
                    cmd3.Parameters.AddWithValue("@SeriesID", Convert.ToInt32(SeriesID));
                    cmd3.Parameters.AddWithValue("@openingDate", OpeningBalance);
                    cmd3.Parameters.AddWithValue("@TradeDate", Convert.ToDateTime(AccquiredDate));
                    cmd3.Parameters.AddWithValue("@BuyQty", txtBuyQty.Value);
                    cmd3.Parameters.AddWithValue("@NetAvgUnitCost", txtNetAvgCost.Value);
                    cmd3.Parameters.AddWithValue("@HostoricalCost", txtHistoricPrice.Value);
                    cmd3.Parameters.AddWithValue("@SecTranTax", txtSecTax.Value);
                    cmd3.Parameters.AddWithValue("@HoldingMode", ddlHoldingMode.SelectedValue);
                    cmd3.Parameters.AddWithValue("@ISIN", ISIN);
                    cmd3.Parameters.AddWithValue("@CreateUser", Convert.ToInt32(Session["userid"].ToString()));
                    cmd3.Parameters.AddWithValue("@InUpType", InUpdateType);
                    cmd3.Parameters.AddWithValue("@ID", ID);
                    cmd3.CommandTimeout = 0;
                    cmd3.ExecuteNonQuery();
                    GC.Collect();
                }
                // Mantis Issue 24802
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                // End of Mantis Issue 24802
            }

            gridPortFolio.CancelEdit();
            e.Cancel = true;
            CheckID = "Insert";
            //ASPxComboBox1.JSProperties["cpDataExists"] = "insert";
        }
        protected void gridPortFolio_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            HiddenField txtSeriesID_hidden = (HiddenField)gridPortFolio.FindEditFormTemplateControl("txtSeriesID_hidden");
            ASPxTextBox txtBuyQty = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtBuyQty");
            ASPxTextBox txtNetAvgCost = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtNetAvgCost");
            ASPxTextBox txtHistoricPrice = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtHistoricPrice");
            ASPxTextBox txtSecTax = (ASPxTextBox)gridPortFolio.FindEditFormTemplateControl("txtSecTax");
            HiddenField txtISIN_hidden = (HiddenField)gridPortFolio.FindEditFormTemplateControl("txtISIN_hidden");
            DropDownList ddlHoldingMode = (DropDownList)gridPortFolio.FindEditFormTemplateControl("ddlHoldingMode");
            ASPxDateEdit dtAcquiredDate = (ASPxDateEdit)gridPortFolio.FindEditFormTemplateControl("dtAcquiredDate");

            string StockFor = null;
            string ISIN = null;
            string InUpdateType = null;
            int ID = 0;
            if (ddlStockFor.SelectedItem.Text == "Pro-Trading")
                StockFor = "T";
            else if (ddlStockFor.SelectedItem.Text == "Pro-Investment")
                StockFor = "I";
            else if (ddlStockFor.SelectedItem.Text == "Client")
                StockFor = "C";
            string[] openBal = Session["LastFinYear"].ToString().Split('-');
            DateTime OpeningBalance = Convert.ToDateTime("04/01/" + openBal[0].ToString());
            string[] ProductSeries = txtSeriesID_hidden.Value.Split('~');
            string ProductID = ProductSeries[0].ToString();
            string SeriesID = ProductSeries[1].ToString();
            if (ddlHoldingMode.SelectedValue == "P")
                ISIN = "NA";
            else
                ISIN = txtISIN_hidden.Value;
            if (Session["PortID"] == null)
            {
                ID = 0;
                InUpdateType = "Insert";
            }
            else
            {
                ID = Convert.ToInt32(Session["PortID"].ToString());
                InUpdateType = "Update";
            }
            string AccquiredDate = Convert.ToDateTime(dtAcquiredDate.Value).ToShortDateString();

            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd3 = new SqlCommand("InsertUpdatePortfolio", con))
                {
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@For", StockFor);
                    cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                    cmd3.Parameters.AddWithValue("@CustomerID", txtCustomerID_hidden.Value);
                    cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                    cmd3.Parameters.AddWithValue("@Segment", Convert.ToInt32(Session["usersegid"].ToString()));
                    cmd3.Parameters.AddWithValue("@Exchange", Convert.ToInt32(Session["ExchangeSegmentID"].ToString()));
                    cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ProductID));
                    cmd3.Parameters.AddWithValue("@SeriesID", Convert.ToInt32(SeriesID));
                    cmd3.Parameters.AddWithValue("@openingDate", OpeningBalance);
                    cmd3.Parameters.AddWithValue("@TradeDate", Convert.ToDateTime(AccquiredDate));
                    cmd3.Parameters.AddWithValue("@BuyQty", txtBuyQty.Value);
                    cmd3.Parameters.AddWithValue("@NetAvgUnitCost", txtNetAvgCost.Value);
                    cmd3.Parameters.AddWithValue("@HostoricalCost", txtHistoricPrice.Value);
                    cmd3.Parameters.AddWithValue("@SecTranTax", txtSecTax.Value);
                    cmd3.Parameters.AddWithValue("@HoldingMode", ddlHoldingMode.SelectedValue);
                    cmd3.Parameters.AddWithValue("@ISIN", ISIN);
                    cmd3.Parameters.AddWithValue("@CreateUser", Convert.ToInt32(Session["userid"].ToString()));
                    cmd3.Parameters.AddWithValue("@InUpType", InUpdateType);
                    cmd3.Parameters.AddWithValue("@ID", ID);
                    cmd3.CommandTimeout = 0;
                    cmd3.ExecuteNonQuery();

                    GC.Collect();
                }
            }
            gridPortFolio.CancelEdit();
            e.Cancel = true;
            CheckID = "Update";
            // ASPxComboBox1.JSProperties["cpDataExists"] = "insert";
        }
        protected void gridPortFolio_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            ASPxDateEdit dtAcquiredDate = (ASPxDateEdit)gridPortFolio.FindEditFormTemplateControl("dtAcquiredDate");
            dtAcquiredDate.EditFormatString = oconverter.GetDateFormat("Date");
            // dtAcquiredDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
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
    }
}