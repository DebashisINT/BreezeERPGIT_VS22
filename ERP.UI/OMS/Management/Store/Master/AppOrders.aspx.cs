using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using DevExpress.Web;
using System.Web.Services;
using System.Text;
using System.IO;
using System.Globalization;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Store.Master
{
    public partial class Management_Store_Master_AppOrders : System.Web.UI.Page
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DBEngine oDBEngine = new DBEngine();
        public string PXMLPATH
        {
            get { return (string)Session["CashBankVoucherFile_XMLPATH"]; }
            set { Session["CashBankVoucherFile_XMLPATH"] = value; }
        }
        public int PCounter
        {
            get { return (int)ViewState["Counter"]; }
            set { ViewState["Counter"] = value; }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oDbConverter = new BusinessLogicLayer.Converter();
        Utilities oUtilities = new Utilities();
        DataTable dtActivelist = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                bind_Order_Type();
                bind_Order_Status();
                //BindCmbpOrder_DeliveryAddress();
                //BindCmbpOrder_DeliveryWareHouse();

                //bind_Delivery_At("P");
                ShowActiveList();
                //BindGrid();
                //SetValueFromXMLFile("6");
                if (Request.QueryString["ID"] != null)
                {
                    // SetValueFromXMLFile(Convert.ToString(Request.QueryString["ID"]));
                }
                if (Session["msg"] != null)
                {
                    string msgs = Convert.ToString(Session["msg"]);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", "<script>alert('" + msgs + "')</script>", false);
                    Session["msg"] = null;
                }

            }

        }

        public void ShowActiveList()
        {

            DataTable dtActivelist_New = new DataTable();
            System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-GB");

            DataTable dtApprovecount = oGenericMethod.GetDataTable(@"SELECT [GlobalSettings_Value]
                                                          
                                                      FROM [dbo].[config_GlobalSettings] ctr
 
                                                      Where GlobalSettings_ID ='11' ");

            string GlobalSettings_Value = dtApprovecount.Rows[0]["GlobalSettings_Value"].ToString();

            StringBuilder str = new StringBuilder();
            str.Append("WHERE ");

            //if (txtpOrder_Date.Text.Trim() != "" && txttoOrderDate.Text.Trim() != "")
            //{ str.Append("pOrder_Date between '" + DateTime.ParseExact(txtpOrder_Date.Text.Trim(), "dd/mm/yyyy", CultureInfo.CurrentCulture) + "' and '" + DateTime.ParseExact(txttoOrderDate.Text.Trim(), "dd/mm/yyyy", CultureInfo.CurrentCulture) + "' and"); }

            if (txtpOrder_Date.Text.Trim() != "" && txttoOrderDate.Text.Trim() != "")
            { str.Append("pOrder_Date between '" + Convert.ToDateTime(txtpOrder_Date.Text.Trim().Replace("-", "/"), provider).Date + "' and '" + Convert.ToDateTime(txttoOrderDate.Text.Trim().Replace("-", "/"), provider).Date + "' and"); }

            if (Convert.ToString(ddlOrderType.SelectedItem.Value).Trim() != "" && Convert.ToString(ddlOrderType.SelectedItem.Value).Trim() != "0")
            { str.Append(" pOrder_Type = '" + Convert.ToString(ddlOrderType.SelectedItem.Value).Trim() + "' and "); }

            if (Convert.ToString(cmbOrder_Status.SelectedItem.Value).Trim() != "" && Convert.ToString(cmbOrder_Status.SelectedItem.Value).Trim() != "0")
            {
                if (Convert.ToString(cmbOrder_Status.SelectedItem.Value).Trim() == "A")
                {
                    str.Append("( pOrder_ApprovUser1 <> '' or pOrder_ApprovUser2 <> '' or pOrder_ApprovUser1 <> '' ) and ");
                }
            }

            string queary = string.Empty;

            if (str.ToString().Trim().EndsWith(" and"))
            {
                int aa = str.ToString().LastIndexOf("and");
                queary = str.ToString().Substring(0, aa); ;
            }

            else if (str.ToString().Trim().EndsWith("WHERE"))
            {
                queary = "";
            }
            else if (str.ToString().Trim().EndsWith("WHERE )"))
            {
                queary = "";
            }
            else
            {
                queary = str.ToString();
            }


            dtActivelist = oGenericMethod.GetDataTable(@" SELECT [pOrder_ID]
                                                            ,UOM_Name
	                                                        ,[pOrder_RefNumber]
		                                                    ,Convert(varchar,[pOrder_Date],103) as [pOrder_Date]     
		                                                    ,[pOrder_ContactID]      
		                                                    ,tmc.cnt_firstname+' '+tmc.cnt_middlename+' '+tmc.cnt_lastname  as Contactname
		                                                    ,msProd.sProducts_Name
		                                                    ,TpordDtl.[pOrderDetail_UnitPrice]
		                                                    ,TpordDtl.pOrderDetail_Quantity
		                                                    ,[pOrder_Company]
		                                                    ,CASE WHEN [pOrder_Branch] = '1' THEN 'True' WHEN [pOrder_Branch] = '0' THEN 'False' END AS [pOrder_Branch]        
		                                                    ,TpordDtl.pOrderDetail_ID
		                                                    ,[pOrder_Type]
		                                                    ,[pOrder_PaymentTerm]
		                                                    ,[pOrder_PaymentDate]
		                                                    ,[pOrder_DeliveryDate]                                                         
                                                            ,pOrderDetail_ApprovPrice
                                                            ,pOrderDetail_ApprovQuantity
		                                                    ,[pOrder_ApprovUser1] 
		                                                    ,[pOrder_ApprovUser2] 
		                                                    ,[pOrder_ApprovUser3] ,
		                                                    mc.Color_Name as Color,
		                                                    ms.Size_Name as Size
                                                        
	                                                   FROM [dbo].Trans_pOrderDetail TpordDtl 
	                                                        
                                                            left join [Trans_pOrder] trnOrd on trnOrd.pOrder_ID=TpordDtl.pOrderDetail_OrderID                                                	
	                                                        left join tbl_master_contact tmc on trnOrd.[pOrder_ContactID]=tmc.cnt_internalId     
                                                            left join dbo.Master_UOM on UOM_ID=pOrderDetail_QuantityUnit                                           	
	                                                        left join Master_sProducts msProd on TpordDtl.pOrderDetail_ProductID=msProd.sProducts_ID  
                                                            left join Master_Color mc on TpordDtl.pOrderDetail_Color=mc.Color_ID 
	                                                        left join Master_Size ms on TpordDtl.pOrderDetail_Size=ms.Size_ID " + queary); //+' ('+tmc.cnt_internalId+')'

            ViewState["dtActivelist"] = dtActivelist;// dtActivelist_New;
            grdActive.DataSource = dtActivelist;// dtActivelist_New;
            grdActive.DataBind();

            /*OrderApproval Ordapp = new OrderApproval();

            foreach (GridViewRow gr in grdActive.Rows)
            {
                CheckBox ChkApprove = gr.FindControl("ChkApprove") as CheckBox;
                CheckBox ChkReject = gr.FindControl("ChkReject") as CheckBox;


                HiddenField hddpOrder_ApprovUser1 = gr.FindControl("hddpOrder_ApprovUser1") as HiddenField;
                HiddenField hddpOrder_ApprovUser2 = gr.FindControl("hddpOrder_ApprovUser2") as HiddenField;
                HiddenField hddpOrder_ApprovUser3 = gr.FindControl("hddpOrder_ApprovUser3") as HiddenField;

                HiddenField hddOrdTlId = gr.FindControl("hddOrdTlId") as HiddenField;

                ChkApprove.Enabled = true;
                ChkReject.Enabled = false;

                DataTable maxApprovalCount = Ordapp.GetSp_GetApprovalCount(Convert.ToInt32(hddOrdTlId.Value));
                string maxAppcnt = maxApprovalCount.Rows[0]["ApproveCount"].ToString();

                DataTable Trans_StockPosition = oGenericMethod.GetDataTable(@"SELECT [StockPosition_OrderDetailID] 
                                                          FROM [dbo].[Trans_StockPosition] ctr 
                                                          Where [StockPosition_OrderDetailID] ='" + Convert.ToString(hddOrdTlId.Value).Trim() + "' ");
                string StockPosition_OrderDetailID = "";
                if (Trans_StockPosition.Rows.Count > 0)
                {
                    StockPosition_OrderDetailID = Trans_StockPosition.Rows[0]["StockPosition_OrderDetailID"].ToString();
                }
                //if (GlobalSettings_Value == maxAppcnt)
                //{
                //    ChkApprove.Checked = true;
                //    ChkApprove.Enabled = false;
                //    ChkReject.Enabled = false;
                //    ChkReject.Checked = false;
                //}
                if (StockPosition_OrderDetailID.Trim() != "")
                {
                    ChkApprove.Checked = true;
                    ChkApprove.Enabled = false;
                    ChkReject.Enabled = false;
                    ChkReject.Checked = false;
                }
                else
                {
                    DataTable dtEdit = oGenericMethod.GetDataTable(@"SELECT [OAppLog_Status] 
                                                          FROM [dbo].[Trans_OAppLog] ctr 
                                                          Where OAppLog_OrderDetailID ='" + Convert.ToString(hddOrdTlId.Value).Trim() + "' and OAppLog_User='" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "'  order by OAppLog_Time desc");
                    if (dtEdit.Rows.Count > 0)
                    {
                        string OAppLog_Status = dtEdit.Rows[0]["OAppLog_Status"].ToString();
                        if (OAppLog_Status.Trim() == "R")
                        {
                            ChkApprove.Checked = false;
                            ChkApprove.Enabled = true;
                            ChkReject.Enabled = false;
                            ChkReject.Checked = true;
                        }
                        else
                        {
                            ChkApprove.Checked = true;
                            ChkApprove.Enabled = false;
                            ChkReject.Enabled = true;
                            ChkReject.Checked = false;
                        }
                    }

                } 
            }*/

        }

        protected void grdActive_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, " ASC");
            }
            //ShowActiveList();
        }

        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }

        }

        private void SortGridView(string sortExpression, string direction)
        {
            DataTable dtSorting = (DataTable)ViewState["dtActivelist"];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            grdActive.DataSource = dv;
            grdActive.DataBind();
        }

        protected void grdActive_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DataTable dtCashBankBook = (DataTable)ViewState["dtActivelist"];
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtCashBankBook.Rows.Count + "'" + ")");

            }


        }

        protected void grdActive_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            OrderApproval Ordapp = new OrderApproval();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ChkApprove = e.Row.FindControl("ChkApprove") as CheckBox;
                CheckBox ChkReject = e.Row.FindControl("ChkReject") as CheckBox;

                HiddenField hddpOrder_ApprovUser1 = e.Row.FindControl("hddpOrder_ApprovUser1") as HiddenField;
                HiddenField hddpOrder_ApprovUser2 = e.Row.FindControl("hddpOrder_ApprovUser2") as HiddenField;
                HiddenField hddpOrder_ApprovUser3 = e.Row.FindControl("hddpOrder_ApprovUser3") as HiddenField;

                HiddenField hddOrdTlId = e.Row.FindControl("hddOrdTlId") as HiddenField;

                ChkApprove.Enabled = true;
                ChkReject.Enabled = false;

                DataTable maxApprovalCount = Ordapp.GetSp_GetApprovalCount(Convert.ToInt32(hddOrdTlId.Value));
                string maxAppcnt = maxApprovalCount.Rows[0]["ApproveCount"].ToString();

                DataTable Trans_StockPosition = oGenericMethod.GetDataTable(@"SELECT [StockPosition_OrderDetailID] 
                                                      FROM [dbo].[Trans_StockPosition] ctr 
                                                      Where [StockPosition_OrderDetailID] ='" + Convert.ToString(hddOrdTlId.Value).Trim() + "' ");
                string StockPosition_OrderDetailID = "";
                if (Trans_StockPosition.Rows.Count > 0)
                {
                    StockPosition_OrderDetailID = Trans_StockPosition.Rows[0]["StockPosition_OrderDetailID"].ToString();
                }
                if (StockPosition_OrderDetailID.Trim() != "")
                {
                    ChkApprove.Checked = true;
                    ChkApprove.Enabled = false;
                    ChkReject.Enabled = false;
                    ChkReject.Checked = false;
                }
                else
                {
                    DataTable dtEdit = oGenericMethod.GetDataTable(@"SELECT [OAppLog_Status] 
                                                      FROM [dbo].[Trans_OAppLog] ctr 
                                                      Where OAppLog_OrderDetailID ='" + Convert.ToString(hddOrdTlId.Value).Trim() + "' and OAppLog_User='" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "'  order by OAppLog_Time desc");
                    if (dtEdit.Rows.Count > 0)
                    {
                        string OAppLog_Status = dtEdit.Rows[0]["OAppLog_Status"].ToString();
                        if (OAppLog_Status.Trim() == "R")
                        {
                            ChkApprove.Checked = false;
                            ChkApprove.Enabled = true;
                            ChkReject.Enabled = false;
                            ChkReject.Checked = true;
                        }
                        else
                        {
                            ChkApprove.Checked = true;
                            ChkApprove.Enabled = false;
                            ChkReject.Enabled = true;
                            ChkReject.Checked = false;
                        }
                    }

                }
            }

        }

        protected void grdActive_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {

            // Use the Cancel property to cancel the paging operation.

            grdActive.PageIndex = e.NewPageIndex;
            ShowActiveList();

        }
        protected void grdActive_PageIndexChanged(Object sender, EventArgs e)
        {
            //ShowActiveList();

        }

        protected void bind_Order_Type()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();

            drsession["id"] = "J";
            drsession["name"] = "Job work to Vendors";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "P";
            drsession["name"] = "Purchase Order To Vendors";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "S";
            drsession["name"] = "Purchase Order Of Customer";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "R";
            drsession["name"] = "Requisition for Stock [Inter-Location]";
            dtCmb.Rows.Add(drsession);



            drsession = dtCmb.NewRow();
            drsession["id"] = "O";
            drsession["name"] = "Samples to Customers as Business Promotion";
            dtCmb.Rows.Add(drsession);

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(ddlOrderType, dtCmb, "name", "id", 0);
            //ddlOrderType.SelectedIndex = 0;
            ddlOrderType.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
            ddlOrderType.SelectedIndex = 0;
        }

        protected void bind_Order_Status()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "0";
            drsession["name"] = "";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "A";
            drsession["name"] = "Approved";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "R";
            drsession["name"] = "Rejected";
            dtCmb.Rows.Add(drsession);



            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbOrder_Status, dtCmb, "name", "id", 0);
            //ddlOrderType.SelectedIndex = 0;

            ddlOrderType.SelectedIndex = 0;
        }


        protected void btnsubmit_OnClick(object source, EventArgs e)
        {
            ShowActiveList();
        }


        public void btnSave_OrderApprove_OnClick(object source, EventArgs e)
        {

            OrderApproval objOrderApproval = new OrderApproval();
            int orderDetailsId = Convert.ToInt32(txtOAppLog_OrderDetailID.Text.Trim());
            int userId = Convert.ToInt32(Convert.ToString(HttpContext.Current.Session["userid"]).Trim());
            string appPrice = txtMarkets_Name.Text.Trim();
            decimal appQuants = Convert.ToDecimal(txtMarkets_Code.Text.Trim());
            string appRemarks = txtMarkets_Description.Text.Trim();

            string retVal = objOrderApproval.Insert_OrderApproval(orderDetailsId, userId, appPrice,
                appQuants, appRemarks);

            ShowActiveList();


            Response.Redirect("AppOrders.aspx");

        }

        public void btnSave_Orderreject_OnClick(object source, EventArgs e)
        {

            OrderApproval objOrderApproval = new OrderApproval();
            string retVal = objOrderApproval.InsertRejection(Convert.ToInt32(txtOAppLog_Reject_OrderDetailID.Text.Trim()), Convert.ToInt32(Convert.ToString(HttpContext.Current.Session["userid"]).Trim()), txtrejectRemarks.Text.Trim());

            ShowActiveList();


            Response.Redirect("AppOrders.aspx");

        }



        [WebMethod]
        public static string GetAutofillValue(string id)
        {

            string AutofillValue = id;
            DataTable dt = new DataTable();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            dt = oGenericMethod.GetDataTable(@"select top 1[OAppLog_ID]
                                                  ,[OAppLog_OrderNumber]
                                                  ,[OAppLog_OrderDetailID]
                                                  ,[OAppLog_User]
                                                  ,[OAppLog_Status]
                                                  ,CAST(ROUND(OAppLog_AppQuantity,2) AS NUMERIC(12,2)) AS OAppLog_AppQuantity  
                                                  ,CAST(ROUND([OAppLog_AppPrice],2) AS NUMERIC(12,2)) AS OAppLog_AppPrice
                                                  ,[OAppLog_Remarks]
                                                  ,[OAppLog_Time]
                                            from Trans_OAppLog
                                            where OAppLog_Status='A' and OAppLog_OrderDetailID=" + id + " Order by OAppLog_Time desc");


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    var value = item.ItemArray;
                    string qty = Convert.ToString(value[5]);
                    try
                    {
                        string[] vals = qty.Split('.');
                        int ints = Convert.ToInt32(vals[0]);
                        int flts = Convert.ToInt32(vals[1]);

                        if (flts > 0)
                        {
                            qty = Convert.ToString(ints) + "." + Convert.ToString(flts);
                            qty = qty.Trim();
                        }
                        else
                        {
                            qty = Convert.ToString(ints);
                        }
                    }
                    catch { }

                    AutofillValue = id + "," + Convert.ToString(value[0]) + "," + Convert.ToString(value[1]) + "," + Convert.ToString(value[2]) + "," + Convert.ToString(value[3]) + "," + Convert.ToString(value[4]) + "," + qty + "," + Convert.ToString(value[6]) + "," + Convert.ToString(value[7]) + "," + Convert.ToString(value[8]);
                }
            }
            else
            {
                dt = oGenericMethod.GetDataTable(@" SELECT  [pOrderDetail_ID] ,[pOrderDetail_UnitPrice],[pOrderDetail_Quantity] FROM [dbo].[Trans_pOrderDetail] where [pOrderDetail_ID]=" + id);

                AutofillValue = id + "," + Convert.ToString("") + "," + Convert.ToString("") + "," + Convert.ToString("") + "," + Convert.ToString("") + "," + Convert.ToString("") + "," + Convert.ToString(dt.Rows[0]["pOrderDetail_Quantity"]) + "," + Convert.ToString(dt.Rows[0]["pOrderDetail_UnitPrice"]) + "," + Convert.ToString("") + "," + Convert.ToString("");

            }
            return AutofillValue;
        }

        //private static string QuantityCheck(string Value)
        //{
        //    string[] valArr;
        //    string FormatedString = string.Empty;
        //    if (Value.Contains('.'))
        //    {
        //        valArr = Value.Split('.');
        //    }
        //    return "";
        //}

        [WebMethod]
        public static string GetDetailsofProduct(string id)
        {

            string AutofillValue = id;
            DataTable dt = new DataTable();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            dt = oGenericMethod.GetDataTable(@"select msp.sProducts_Name
		                                            ,pOrderDetail_Brand
		                                            ,msize.Size_Name
		                                            ,mcolor.Color_Name
		                                            ,pOrderDetail_BestBeforeYear
		                                            ,pOrderDetail_BestBeforeMonth
		                                            ,mcurnc.Currency_Name
		                                            ,pOrderDetail_UnitPrice
		                                            ,pOrderDetail_PriceLot
		                                            ,mUOM.UOM_Name
		                                            ,pOrderDetail_Quantity
		                                            ,mUOM2.UOM_Name as pOrderDetail_QuantityName
		                                            ,pOrderDetail_Remarks
                                             from Trans_pOrderDetail TpOd
	                                            inner join master_sproducts msp on TpOd.pOrderDetail_ProductID=msp.sProducts_ID
	                                            inner join Master_Size msize on TpOd.pOrderDetail_Size=msize.Size_ID
	                                            inner join Master_Color mcolor on TpOd.pOrderDetail_Color=mcolor.Color_ID
	                                            inner join master_currency mcurnc on TpOd.pOrderDetail_QuoteCurrency=mcurnc.Currency_ID
	                                            inner join Master_UOM mUOM on TpOd.pOrderDetail_PriceLotUnit=mUOM.UOM_ID
	                                            inner join Master_UOM mUOM2 on TpOd.pOrderDetail_QuantityUnit=mUOM2.UOM_ID

                                             where pOrderDetail_ID = " + id);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    var value = item.ItemArray;
                    AutofillValue = id + "," + Convert.ToString(value[0]) + "," + Convert.ToString(value[1]) + "," + Convert.ToString(value[2]) + "," + Convert.ToString(value[3]) + "," + Convert.ToString(value[4]) + "," + Convert.ToString(value[5]) + "," + Convert.ToString(value[6]) + "," + Convert.ToString(value[7]) + "," + Convert.ToString(value[8]) + "," + Convert.ToString(value[9]) + "," + Convert.ToString(value[10]) + "," + Convert.ToString(value[11]) + "," + Convert.ToString(value[12]);
                }
            }
            return AutofillValue;
        }
    }
}