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
using System.Web.Services;
////using DevExpress.Web.ASPxClasses;
using DevExpress.Web;
using System.Text;
using System.Globalization;
using System.IO;
//using DevExpress.Web;
using BusinessLogicLayer;
using System.Collections.Generic;
//using DevExpress.Web.ASPxPopupControl;

public partial class Management_Store_Master_ucpOder : System.Web.UI.UserControl
{
    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //foreach (string key in Session.Keys)
            //{
            //    Response.Write(key + " - " + Session[key] + "<br />");
            //}

            MoreInfoGridBind();
            Bindbranch_code();
            SizeBind();
            ColorBind();
            YearBind();
            bind_Order_Type();

            BindCmbpOrder_DeliveryAddress();
            BindCmbpOrder_DeliveryWareHouse();
            BindCmbpOrder_DeliveryBranch();
            bind_Delivery_At("P");
            BindGrid();
            //SetValueFromXMLFile("6");
            if (Request.QueryString["ID"] != null)
            {
                SetValueFromXMLFile(Convert.ToString(Request.QueryString["ID"]));
            }
            if (Session["msg"] != null)
            {
                string msgs = Convert.ToString(Session["msg"]);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", "<script>alert('" + msgs + "')</script>", false);
                Session["msg"] = null;
            }
        }
        //if (Page.IsCallback)
        //{
        //    var x = txthid.Text;
        //   // DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl+"?ID=6");
        //}
    }

    protected void pOrderDetails_OnAfterPerformCallback(object sender, EventArgs e)
    {

        BindGrid();
    }

    protected void BindGrid()
    {

        // oGenericMethod = new GenericMethod();
        oGenericMethod = new BusinessLogicLayer.GenericMethod();
        StringBuilder str = new StringBuilder();
        str.Append("WHERE ");
        if (Convert.ToString(CmbpOrder_Branch.SelectedItem.Value).Trim() != "" && Convert.ToString(CmbpOrder_Branch.SelectedItem.Value).Trim() != "0")
        { str.Append("pOrder_Branch = '" + Convert.ToString(CmbpOrder_Branch.SelectedItem.Value).Trim() + "' and "); }


        if (txtpOrder_Date.Text.Trim() != "")
        { str.Append("pOrder_Date = '" + DateTime.ParseExact(txtpOrder_Date.Text.Trim(), "dd/mm/yyyy", CultureInfo.CurrentCulture) + "' and "); }

        if (Convert.ToString(ddlOrderType.SelectedItem.Value).Trim() != "" && Convert.ToString(ddlOrderType.SelectedItem.Value).Trim() != "0")
        { str.Append("pOrder_Type = '" + Convert.ToString(ddlOrderType.SelectedItem.Value).Trim() + "' and "); }

        if (txtTaxRates_user.Text.Trim() != "" && txtTaxRates_user_hidden.Text.Trim() != "")
        { str.Append("pOrder_ContactID = '" + txtTaxRates_user_hidden.Text.Trim() + "' and "); }

        if (txtOrder_RefNumber.Text.Trim() != "")
        { str.Append("pOrder_RefNumber = '" + txtOrder_RefNumber.Text.Trim() + "' and "); }

        if (txtpOrder_User_AgentID.Text.Trim() != "" && txtpOrder_User_AgentID_hidden.Text.Trim() != "")
        { str.Append("pOrder_AgentID = '" + txtpOrder_User_AgentID_hidden.Text.Trim() + "' and "); }

        //if (ASPxMemo1.Text.Trim() != "" )
        //{ str.Append("pOrder_Instructions = '" + ASPxMemo1.Text.Trim() + "' and "); }

        //if (Convert.ToString(CmbpOrder_PaymentTerm.SelectedItem).Trim() != "")
        //{ str.Append("pOrder_PaymentTerm = '" + Convert.ToString(CmbpOrder_PaymentTerm.SelectedItem.Value).Trim() + "' and "); }

        //if (Convert.ToString(CmbpOrder_PaymentTerm.SelectedItem).Trim() != "")
        //{ str.Append("pOrder_PaymentTerm = '" + Convert.ToString(CmbpOrder_PaymentTerm.SelectedItem.Value).Trim() + "' and "); }


        //if (txtpOrder_DeliveryDate.Text.Trim() != "")
        //{ str.Append("pOrder_DeliveryDate = '" + Convert.ToDateTime(txtpOrder_DeliveryDate.Text.Trim()) + "' and "); }

        //if (txtpOrder_PaymentDate.Text.Trim() != "")
        //{ str.Append("pOrder_PaymentDate = '" + Convert.ToDateTime(txtpOrder_PaymentDate.Text.Trim()) + "' and "); }

        //if (Convert.ToString(ddlDeliveryAt.SelectedItem).Trim() != "")
        //{ str.Append("pOrder_DeliveryAt = '" + Convert.ToString(ddlDeliveryAt.SelectedItem.Value).Trim() + "' and "); }

        if (Convert.ToString(CmbpOrder_DeliveryBranch.SelectedItem.Value).Trim() != "" && Convert.ToString(CmbpOrder_DeliveryBranch.SelectedItem.Value).Trim() != "0")
        { str.Append("pOrder_DeliveryBranch = '" + Convert.ToString(CmbpOrder_DeliveryBranch.SelectedItem.Value).Trim() + "' and "); }

        //if (Convert.ToString(CmbpOrder_DeliveryWareHouse.SelectedItem).Trim() != "")
        //{ str.Append("pOrder_DeliveryWareHouse = '" + Convert.ToString(CmbpOrder_DeliveryWareHouse.SelectedItem.Value).Trim() + "' and "); }

        //if (Convert.ToString(CmbpOrder_DeliveryAddress.SelectedItem).Trim() != "")
        //{ str.Append("pOrder_DeliveryAddress = '" + Convert.ToString(CmbpOrder_DeliveryAddress.SelectedItem.Value).Trim() + "' and "); }

        //if (Convert.ToString(CmbpOrder_DeliveryAddress.SelectedItem).Trim() != "")
        //{ str.Append("pOrder_DeliveryAddress = '" + Convert.ToString(CmbpOrder_DeliveryAddress.SelectedItem.Value).Trim() + "' and "); }

        //if (txtpOrder_DeliveryOther.Text.Trim() != "")
        //{ str.Append("pOrder_DeliveryOther = '" + txtpOrder_DeliveryOther.Text.Trim() + "' and "); }

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


        DataTable dtFillGrid = new DataTable();

        string squery=string.Format(@" SELECT x.[pOrder_ID],x.[pOrder_Company]                                                     
                                                     ,(SELECT a.branch_internalId FROM  tbl_master_branch a WHERE a.branch_id=x.pOrder_Branch) AS pOrder_Branch 
                                                      ,Convert(varchar,x.[pOrder_Date],106) as pOrder_Date
                                                      ,x.[pOrder_FinYear]
                                                      ,x.[pOrder_Type] AS pOrder_Type_Type
                                                      ,CASE WHEN x.[pOrder_Type] = 'P' THEN 'Purchase Order' WHEN x.[pOrder_Type] = 'S' THEN 'Purchase Order Of Customer'
                                                       WHEN x.[pOrder_Type] = 'R' THEN 'Requisition for Stock [Inter-Location]' WHEN x.[pOrder_Type] = 'J' THEN 'Job work to Vendors'
                                                       WHEN x.[pOrder_Type] = 'O' THEN 'Samples to Customers as Business Promotion' END AS pOrder_Type 
                                                      ,(SELECT TOP 1 c.cnt_internalId FROM tbl_master_contact c WHERE c.cnt_internalId = x.pOrder_ContactID) AS pOrder_ContactID
                                                      ,x.[pOrder_RefNumber]
                                                      ,x.[pOrder_Number]
                                                      ,(SELECT cnt_firstName + ' '+ ' '+cnt_middleName+ ' ' +cnt_lastName FROM tbl_master_contact WHERE x.pOrder_ContactID = cnt_internalId) AS pOrder_Vendor
                                                      
                                                      ,(SELECT LEFT(Master_sProducts, LEN(Master_sProducts) - 1) AS pOrder_Product
													   FROM (
													   SELECT sProducts_Name + ', '
													   FROM Master_sProducts WHERE  sProducts_ID in (SELECT pOrderDetail_ProductID FROM Trans_pOrderDetail WHERE pOrderDetail_OrderID = (SELECT pOrder_ID FROM Trans_pOrder WHERE pOrder_ID = x.pOrder_ID))
													   FOR XML PATH ('')
													   ) c (Master_sProducts) ) AS pOrder_Product
                                                      
													  ,(SELECT LEFT(Master_Color, LEN(Master_Color) - 1) AS pOrder_Color
													   FROM (
													   SELECT Color_Name + ', '
													   FROM Master_Color WHERE  Color_ID in (SELECT pOrderDetail_Color FROM Trans_pOrderDetail WHERE pOrderDetail_OrderID = (SELECT pOrder_ID FROM Trans_pOrder WHERE pOrder_ID = x.pOrder_ID))
													   FOR XML PATH ('')
													   ) c (Master_Color) ) AS pOrder_Color 
                                                      ,(SELECT LEFT(Master_Size, LEN(Master_Size) - 1) AS pOrder_Color
													   FROM (
													   SELECT Size_Name + ', '
													   FROM Master_Size WHERE  Size_ID in (SELECT pOrderDetail_Size FROM Trans_pOrderDetail WHERE pOrderDetail_OrderID = (SELECT pOrder_ID FROM Trans_pOrder WHERE pOrder_ID = x.pOrder_ID))
													   FOR XML PATH ('')
													   ) c (Master_Size) ) AS pOrder_Size
                                                      
                                                      ,CASE WHEN x.[pOrder_DeliveryAt] = 'B' THEN 'Branch' WHEN x.[pOrder_DeliveryAt] = 'W' THEN 'WareHouse'
                                                       WHEN x.[pOrder_DeliveryAt] = 'O' THEN 'Other Location' WHEN x.[pOrder_DeliveryAt] = 'A' THEN 'Customers Address'
                                                       END AS pOrder_DeliveryAt 
                                                      
                                                      ,x.[pOrder_DeliveryBranch]
                                                      ,x.[pOrder_DeliveryWareHouse]
                                                      ,x.[pOrder_DeliveryAddress],
                                                      TpordDtl.pOrderDetail_Quantity 
                                                  FROM [dbo].[Trans_pOrder] x 
                                                  left join Trans_pOrderDetail TpordDtl on x.pOrder_ID=TpordDtl.pOrderDetail_OrderID
	                                                left join tbl_master_company tmc on x.[pOrder_Company]=tmc.cmp_id  " + queary );
        if(Session["LastCompanys"]!=null)
        {
            squery=squery+ " and x.[pOrder_Company]='" + Session["LastCompanys"].ToString() + "'";
        }
        dtFillGrid = oGenericMethod.GetDataTable(squery);


        AspxHelper oAspxHelper = new AspxHelper();
        if (dtFillGrid.Rows.Count > 0)
        {
            cityGrid.DataSource = dtFillGrid;
            cityGrid.DataBind();
        }
        else
        {
            cityGrid.DataSource = null;
            cityGrid.DataBind();
        }

    }

    protected void cityGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        Store_MasterBL oStoreMasterBL = new Store_MasterBL();
        string WhichCall = e.Parameters.ToString().Split('~')[0];
        string WhichType = null;
        if (e.Parameters.ToString().Contains("~"))
        {
            if (e.Parameters.ToString().Split('~')[1] != "")
            {
                WhichType = e.Parameters.ToString().Split('~')[1];

            }
        }
        if (WhichCall == "Edit")
        {
            DataTable dtCmb = oGenericMethod.GetDataTable("select * from  [dbo].[Trans_StockPosition] where StockPosition_OrderDetailID=" + WhichType);
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyss", "<script>alert('Sorry you can not delete this order.')</script>", false);
            }
            else
            {


                DataSet ds = new DataSet();
                DataTable dtpOrder = new DataTable();
                DataTable dtpOrderDetails = new DataTable();
                dtpOrder = oStoreMasterBL.GetOrderListById(Convert.ToInt32(WhichType));
                dtpOrder = dtpOrder.Copy();
                dtpOrderDetails = oStoreMasterBL.GetOrderDetailsListById(Convert.ToInt32(WhichType));
                dtpOrderDetails = dtpOrderDetails.Copy();
                dtpOrderDetails.TableName = "Table1";
                ds.Tables.Add(dtpOrder);
                ds.Tables.Add(dtpOrderDetails);
                DatabaseToXMLWrite(ds, WhichType);
                //BasicFunctions();
                //object senders = new object();
                //EventArgs es = new EventArgs();
                //Page_Load(senders, es);
                //SetValueFromXMLFile(WhichType);
                //Response.Redirect(Request.RawUrl,false);

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/management/store/Master/pOrder.aspx?ID=" + WhichType);
            }


        }
        if (WhichCall == "Delete")
        {

            DataTable dtCmb = oGenericMethod.GetDataTable("select * from  [dbo].[Trans_StockPosition] where StockPosition_OrderDetailID=" + WhichType);
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyss", "alert('Sorry you can not delete this order.');", true);
            }
            else
            {

                oGenericMethod.Delete_Table("Trans_pOrder", "pOrder_ID = " + WhichType);
                oGenericMethod.Delete_Table("Trans_pOrderDetail", "pOrderDetail_OrderID = " + WhichType);
                BindGrid();
            }



            

        }
    }
    protected void BasicFunctions()
    {
        Bindbranch_code();
        bind_Order_Type();
        BindCmbpOrder_DeliveryAddress();
        BindCmbpOrder_DeliveryWareHouse();
        BindCmbpOrder_DeliveryBranch();
        bind_Delivery_At("P");
        BindGrid();
    }
    protected void cityGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            int commandColumnIndex = -1;
            for (int i = 0; i < cityGrid.Columns.Count; i++)
                if (cityGrid.Columns[i] is GridViewCommandColumn)
                {
                    commandColumnIndex = i;
                    break;
                }
            if (commandColumnIndex == -1)
                return;
            //____One colum has been hided so index of command column will be leass by 1 
            commandColumnIndex = commandColumnIndex - 2;
            DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
            for (int i = 0; i < cell.Controls.Count; i++)
            {
                DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                if (button == null) return;
                DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                if (hyperlink.Text == "Delete")
                {
                    if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
                    {
                        hyperlink.Enabled = true;
                        continue;
                    }
                    else
                    {
                        hyperlink.Enabled = false;
                        continue;
                    }
                }
            }
        }
    }

    protected void cityGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
    {
        if (!cityGrid.IsNewRowEditing)
        {
            ASPxGridViewTemplateReplacement RT = cityGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
            if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                RT.Visible = true;
            else
                RT.Visible = false;
        }

    }

    ///////////////////////////////////////
    protected void Bindbranch_code()
    {
        //  / oGenericMethod = new GenericMethod();
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select LTRIM(RTRIM(branch_id)) branch_id, LTRIM(RTRIM(branch_code)) branch_code from tbl_master_branch");
        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(CmbpOrder_Branch, dtCmb, "branch_code", "branch_id", 0);

        CmbpOrder_Branch.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        CmbpOrder_Branch.SelectedIndex = 0;

    }

    protected void BindCmbpOrder_DeliveryAddress()
    {
        //  / oGenericMethod = new GenericMethod();
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select add_id,add_entity from tbl_master_address where add_entity<>''");
        AspxHelper oAspxHelper = new AspxHelper();
        //if (dtCmb.Rows.Count > 0)
        //    oAspxHelper.Bind_Combo(CmbpOrder_DeliveryAddress, dtCmb, "add_entity", "add_id", 0);

        //CmbpOrder_DeliveryAddress.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        //CmbpOrder_DeliveryAddress.SelectedIndex = 0;

    }

    protected void BindCmbpOrder_DeliveryWareHouse()
    {
        //  / oGenericMethod = new GenericMethod();
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select WareHouse_ID,WareHouse_Name from Master_WareHouse");
        AspxHelper oAspxHelper = new AspxHelper();
        //if (dtCmb.Rows.Count > 0)
        //    oAspxHelper.Bind_Combo(CmbpOrder_DeliveryWareHouse, dtCmb, "WareHouse_Name", "WareHouse_ID", 0);

        //CmbpOrder_DeliveryWareHouse.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        //CmbpOrder_DeliveryWareHouse.SelectedIndex = 0;

    }
    protected void BindCmbpOrder_DeliveryBranch()
    {
        //  / oGenericMethod = new GenericMethod();
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select BankBranch_ID,BankBranch_Name from Master_BankBranches");
        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(CmbpOrder_DeliveryBranch, dtCmb, "BankBranch_Name", "BankBranch_ID", 0);

        CmbpOrder_DeliveryBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        CmbpOrder_DeliveryBranch.SelectedIndex = 0;

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

    protected void bind_Delivery_At(string type)
    {

        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DataTable dtCmb = new DataTable();

        dtCmb.Columns.Add("id");
        dtCmb.Columns.Add("name");
        DataRow drsession = dtCmb.NewRow();

        if (type.Trim() == "P" || type.Trim() == "R")
        {
            drsession["id"] = "B";
            drsession["name"] = "Branch";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "W";
            drsession["name"] = "WareHouse";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "O";
            drsession["name"] = "Other Location";
            dtCmb.Rows.Add(drsession);
        }
        else if (type.Trim() == "S" || type.Trim() == "O")
        {
            drsession = dtCmb.NewRow();
            drsession["id"] = "A";
            drsession["name"] = "Customer's Address";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "O";
            drsession["name"] = "Other Location";
            dtCmb.Rows.Add(drsession);
        }
        else if (type.Trim() == "J")
        {
            drsession = dtCmb.NewRow();
            drsession["id"] = "A";
            drsession["name"] = "Vendor's Address";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "O";
            drsession["name"] = "Other Location";
            dtCmb.Rows.Add(drsession);

        }

        AspxHelper oAspxHelper = new AspxHelper();
        //if (dtCmb.Rows.Count > 0)
        //    oAspxHelper.Bind_Combo(ddlDeliveryAt, dtCmb, "name", "id", 0);

        //ddlDeliveryAt.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        //ddlDeliveryAt.SelectedIndex = 0;
    }

    protected void ddlDeliveryAt_Callback(object source, CallbackEventArgsBase e)
    {
        string WhichCall = e.Parameter.Split('~')[0];
        if (WhichCall == "bind_Delivery_At")
        {
            string countryID = e.Parameter.Split('~')[1].ToString();
            bind_Delivery_At(countryID);
        }
    }

    protected void btnsubmit_OnClick(object source, EventArgs e)
    { BindGrid(); } 
    private void DatabaseToXMLWrite(DataSet dbDataSet, string EditId)
    {
        DataSet dsToAddXML = new DataSet();
        DataTable dtXML = new DataTable();
        string SaveXMLPath = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditId + "_E");
        PXMLPATH = "../../Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditId + "_E";
        //string sessionUserId = string.Empty;
        //If edit xml already exist then read/write exist file 

        #region check isCheck in
        string sessionUserId = string.Empty;
        sessionUserId = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
        string[] arr = PXMLPATH.Split('_');
        bool isNotEditable = false;

        //get files for edit 

        DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Management/Documents/"));//Assuming Test is your Folder
        FileInfo[] Files = d.GetFiles();


        foreach (var item in Files)
        {
            try
            {
                string[] Fnames = item.ToString().Split('_');
                string Uid = Fnames[1];
                string Pid = Fnames[2];
                string Mode = Fnames[3];
                //check with condition
                if (Mode.Equals("E") && Pid.Equals(EditId) && !Uid.Equals(sessionUserId))
                {
                    isNotEditable = true;
                }
            }
            catch { }

        }



        //if it is already check in then it should not check in again
        if (isNotEditable)
        {
            Session["msg"] = "Sorry this product is already in use";
            // Response.Redirect(Request.RawUrl,false);
            DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl);
            return;
        }
        #endregion
        else
        {
            if (File.Exists(SaveXMLPath))
            {
                //sessionUserId = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
                //string [] arr = PXMLPATH.Split('_');
                ////if it is already check in then it should not check in again
                //if (arr[2].Equals(EditId.Trim()))
                //{
                //    Session["msg"] = "Sorry this product is already check out by another user";
                //   // Response.Redirect(Request.RawUrl,false);
                //    DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl);
                //}

            }

            //If edit xml not exist then create xml file 
            else
            {
                dtXML = dsToAddXML.Tables.Add();
                dtXML.Columns.Add(new DataColumn("pOrderRecordID", typeof(int))); //0
                dtXML.Columns.Add(new DataColumn("pOrderCompany", typeof(string)));//1
                dtXML.Columns.Add(new DataColumn("pOrderBranch", typeof(string)));//2
                dtXML.Columns.Add(new DataColumn("pOrderDate", typeof(string)));//3
                dtXML.Columns.Add(new DataColumn("pOrderFinYear", typeof(string)));//4
                dtXML.Columns.Add(new DataColumn("pOrderType", typeof(string)));//5
                dtXML.Columns.Add(new DataColumn("pOrderContactID", typeof(string)));//6
                dtXML.Columns.Add(new DataColumn("pOrderRefNumber", typeof(string)));//7
                dtXML.Columns.Add(new DataColumn("pOrderNumber", typeof(string)));//8
                dtXML.Columns.Add(new DataColumn("pOrderAgentID", typeof(string)));//9
                dtXML.Columns.Add(new DataColumn("pOrderInstructions", typeof(string)));//10
                dtXML.Columns.Add(new DataColumn("pOrderPaymentTerm", typeof(string)));//11
                dtXML.Columns.Add(new DataColumn("pOrderPaymentDate", typeof(string)));//12
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryDate", typeof(string)));//13
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryAt", typeof(string)));//14
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryBranch", typeof(string)));//15
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryWareHouse", typeof(string)));//16
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryAddress", typeof(string)));//17
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryOther", typeof(string)));//18
                dtXML.Columns.Add(new DataColumn("pOrderVerified", typeof(string)));//19
                dtXML.Columns.Add(new DataColumn("pOrderVerifyRemarks", typeof(string)));//20
                dtXML.Columns.Add(new DataColumn("pOrderVerifyUser", typeof(string)));//21
                dtXML.Columns.Add(new DataColumn("pOrderVerifyTime", typeof(string)));//22
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser1", typeof(string)));//23
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser1Time", typeof(string)));//24 
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser2", typeof(string)));//25 
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser2Time", typeof(string)));//26
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser3", typeof(string)));//27 
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser3Time", typeof(string)));//28
                dtXML.Columns.Add(new DataColumn("contactname", typeof(string)));//29 
                dtXML.Columns.Add(new DataColumn("agentname", typeof(string)));//30
                dtXML.Columns.Add(new DataColumn("DBEditId", typeof(int))); //31
                dtXML.Columns.Add(new DataColumn("pOrder_PaymentDays", typeof(int))); //32
                dtXML.Columns.Add(new DataColumn("pOrderParentRefNumber", typeof(string)));//33
                DataRow drXML = dtXML.NewRow();

                string OrderDate = null;
                string PaymentDate = null;
                string DeliveryDate = null;
                if (Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Date"]) != "")
                {
                    string[] orderDateArry = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Date"]).Split(' ');
                    OrderDate = orderDateArry[0];
                }

                if (Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_PaymentDate"]) != "")
                {
                    string[] paymentDateArry = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_PaymentDate"]).Split(' ');
                    PaymentDate = paymentDateArry[0];
                }

                if (Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryDate"]) != "")
                {
                    string[] delivaryDateArry = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryDate"]).Split(' ');
                    DeliveryDate = delivaryDateArry[0];
                }

                drXML[0] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ID"]);
                drXML[1] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Company"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Company"]);
                drXML[2] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Branch"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Branch"]);
                drXML[3] = Convert.ToString(OrderDate) == "" ? "NULL" : Convert.ToString(OrderDate);
                drXML[4] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_FinYear"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_FinYear"]);
                drXML[5] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Type"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Type"]);
                drXML[6] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ContactID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ContactID"]);
                drXML[7] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_RefNumber"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_RefNumber"]);
                drXML[8] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Number"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Number"]);
                drXML[9] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_AgentID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_AgentID"]);
                drXML[10] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Instructions"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Instructions"]);
                drXML[11] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_PaymentTerm"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_PaymentTerm"]);
                drXML[12] = Convert.ToString(PaymentDate) == "" ? "NULL" : Convert.ToString(PaymentDate);
                drXML[13] = Convert.ToString(DeliveryDate) == "" ? "NULL" : Convert.ToString(DeliveryDate);
                drXML[14] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryAt"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryAt"]);
                drXML[15] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryBranch"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryBranch"]);
                drXML[16] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryWareHouse"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryWareHouse"]);
                drXML[17] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryAddress"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryAddress"]);
                drXML[18] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryOther"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_DeliveryOther"]);
                drXML[19] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Verified"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_Verified"]);
                drXML[20] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[21] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[22] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[23] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[24] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[25] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[26] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[27] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[28] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_VerifyRemarks"]);
                drXML[29] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ContactName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ContactName"]);
                drXML[30] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_AgentName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_AgentName"]);
                drXML[31] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ID"]);
                drXML[32] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_PaymentDays"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_PaymentDays"]);
                drXML[33] = Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ParentOrderNo"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[0].Rows[0]["pOrder_ParentOrderNo"]);

                dtXML.Rows.Add(drXML);
                dtXML.AcceptChanges();
                dsToAddXML.Tables[0].TableName = "DtPerchesOrderVoucher";
                dsToAddXML.WriteXml(Server.MapPath(PXMLPATH));
                HiddenField pOderId_hidden = (HiddenField)Parent.FindControl("pOderId_hidden");
                HiddenField DBEditId = (HiddenField)Parent.FindControl("DBEditId");

                pOderId_hidden.Value = Convert.ToString(drXML[0]);
                DBEditId.Value = Convert.ToString(drXML[31]);
                if (dbDataSet.Tables.Count == 2)
                {
                    PXMLPATH = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditId + "_E");
                    dsToAddXML = new DataSet();
                    dsToAddXML.ReadXml(PXMLPATH);
                    dtXML = dsToAddXML.Tables.Add();
                    for (int i = 0; i <= dbDataSet.Tables[1].Rows.Count - 1; i++)
                    {
                        int rowsCount = dbDataSet.Tables[1].Rows.Count - 1;
                        // For first grid data
                        if (i == 0)
                        {
                            dtXML.Columns.Add(new DataColumn("RecordID", typeof(int))); //0
                            dtXML.Columns.Add(new DataColumn("ProductDetailsName", typeof(string)));//1
                            dtXML.Columns.Add(new DataColumn("ProductDetailsID", typeof(string)));//2
                            dtXML.Columns.Add(new DataColumn("DetailBrandName", typeof(string)));//3
                            dtXML.Columns.Add(new DataColumn("Detail_BrandID", typeof(string)));//4
                            dtXML.Columns.Add(new DataColumn("SizeName", typeof(string)));//5
                            dtXML.Columns.Add(new DataColumn("SizeID", typeof(string)));//6
                            dtXML.Columns.Add(new DataColumn("ColorName", typeof(string)));//7
                            dtXML.Columns.Add(new DataColumn("ColorID", typeof(string)));//8
                            dtXML.Columns.Add(new DataColumn("MonthName", typeof(string)));//9
                            dtXML.Columns.Add(new DataColumn("MonthID", typeof(string)));//10
                            dtXML.Columns.Add(new DataColumn("YearName", typeof(string)));//11
                            dtXML.Columns.Add(new DataColumn("YearID", typeof(string)));//12
                            dtXML.Columns.Add(new DataColumn("QuoteCurrencyName", typeof(string)));//13
                            dtXML.Columns.Add(new DataColumn("QuoteCurrencyID", typeof(string)));//14
                            dtXML.Columns.Add(new DataColumn("PerchesUnit", typeof(string)));//15
                            dtXML.Columns.Add(new DataColumn("LotUnit", typeof(string)));//16
                            dtXML.Columns.Add(new DataColumn("UnitName", typeof(string)));//17(modified for new add logic)
                            dtXML.Columns.Add(new DataColumn("UnitID", typeof(string)));//18
                            dtXML.Columns.Add(new DataColumn("Quantity", typeof(string)));//19
                            dtXML.Columns.Add(new DataColumn("QntityunitName", typeof(string)));//20
                            dtXML.Columns.Add(new DataColumn("QntityunitID", typeof(string)));//21
                            dtXML.Columns.Add(new DataColumn("UnitName1", typeof(string)));//22
                            dtXML.Columns.Add(new DataColumn("UnitID1", typeof(string)));//23
                            dtXML.Columns.Add(new DataColumn("Remarks", typeof(string)));//24 
                            dtXML.Columns.Add(new DataColumn("pOrder_ProductId", typeof(string)));//25 
                            dtXML.Columns.Add(new DataColumn("ProductDescription", typeof(string)));//26

                            drXML = dtXML.NewRow();

                            drXML[0] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_ID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_ID"]);
                            drXML[1] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["ProductName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["ProductName"]);
                            drXML[2] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_ProductID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_ProductID"]);
                            drXML[3] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Brand"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Brand"]);
                            drXML[4] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["Detail_BrandID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["Detail_BrandID"]);
                            drXML[5] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["Size_Name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["Size_Name"]);
                            drXML[6] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Size"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Size"]);
                            drXML[7] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["Color_Name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["Color_Name"]);
                            drXML[8] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Color"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Color"]);
                            drXML[9] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["Month_Name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["Month_Name"]);
                            drXML[10] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_BestBeforeMonth"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_BestBeforeMonth"]);
                            drXML[11] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["year_name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["year_name"]);
                            drXML[12] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_BestBeforeYear"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_BestBeforeYear"]);
                            drXML[13] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["CurrencyName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["CurrencyName"]);
                            drXML[14] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuoteCurrency"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuoteCurrency"]);
                            drXML[15] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_UnitPrice"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_UnitPrice"]);
                            drXML[16] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_PriceLot"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_PriceLot"]);
                            drXML[17] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_PriceLotUnit_name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_PriceLotUnit_name"]);
                            drXML[18] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_PriceLotUnit"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_PriceLotUnit"]);
                            drXML[19] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Quantity"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Quantity"]);
                            drXML[20] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuantityUnitName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuantityUnitName"]);
                            drXML[21] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuantityUnit"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuantityUnit"]);
                            drXML[22] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Remarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Remarks"]);
                            drXML[23] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuantityUnit"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_QuantityUnit"]);
                            drXML[24] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Remarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_Remarks"]);
                            drXML[25] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_UnitPrice"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_UnitPrice"]);
                            drXML[26] = Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_ProductDescription"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[0]["pOrderDetail_ProductDescription"]);
                            dtXML.Rows.Add(drXML);
                            dtXML.AcceptChanges();
                            dsToAddXML.Tables[1].TableName = "DtPerchesOrderDetails";
                            string SavePXMLPATH = "../../Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditId + "_E";
                            dsToAddXML.WriteXml(Server.MapPath(SavePXMLPATH));
                        }
                        // For more then one grid data
                        else
                        {
                            int m = i;
                            drXML = dsToAddXML.Tables[1].NewRow();

                            drXML[0] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_ID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_ID"]);
                            drXML[1] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["ProductName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["ProductName"]);
                            drXML[2] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_ProductID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_ProductID"]);
                            drXML[3] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Brand"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Brand"]);
                            drXML[4] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["Detail_BrandID"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["Detail_BrandID"]);
                            drXML[5] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["Size_Name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["Size_Name"]);
                            drXML[6] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Size"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Size"]);
                            drXML[7] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["Color_Name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["Color_Name"]);
                            drXML[8] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Color"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Color"]);
                            drXML[9] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["Month_Name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["Month_Name"]);
                            drXML[10] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_BestBeforeMonth"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_BestBeforeMonth"]);
                            drXML[11] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["year_name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["year_name"]);
                            drXML[12] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_BestBeforeYear"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_BestBeforeYear"]);
                            drXML[13] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["CurrencyName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["CurrencyName"]);
                            drXML[14] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuoteCurrency"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuoteCurrency"]);
                            drXML[15] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_UnitPrice"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_UnitPrice"]);
                            drXML[16] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_PriceLot"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_PriceLot"]);
                            drXML[17] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_PriceLotUnit_name"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_PriceLotUnit_name"]);
                            drXML[18] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_PriceLotUnit"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_PriceLotUnit"]);
                            drXML[19] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Quantity"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Quantity"]);
                            drXML[20] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuantityUnitName"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuantityUnitName"]);
                            drXML[21] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuantityUnit"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuantityUnit"]);
                            drXML[22] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Remarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Remarks"]);
                            drXML[23] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuantityUnit"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_QuantityUnit"]);
                            drXML[24] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Remarks"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_Remarks"]);
                            drXML[25] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_UnitPrice"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_UnitPrice"]);
                            drXML[26] = Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_ProductDescription"]) == "" ? "NULL" : Convert.ToString(dbDataSet.Tables[1].Rows[m]["pOrderDetail_ProductDescription"]);
                            dtXML.Rows.Add(drXML);
                            string SavePXMLPATH = "../../Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditId + "_E";
                            dsToAddXML.WriteXml(Server.MapPath(SavePXMLPATH));
                            dsToAddXML.Dispose();
                        }
                    }
                }
                dsToAddXML.Dispose();
            }
        }
    }
    private void SetValueFromXMLFile(string EditId)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtpOrderDetails = new DataTable();
        PXMLPATH = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditId + "_E");
        if (File.Exists(PXMLPATH))
        {
            ds.ReadXml(PXMLPATH);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                ASPxComboBox CmbpOrder_Branch = (ASPxComboBox)Parent.FindControl("CmbpOrder_Branch");
                ASPxDateEdit txtTaxRates_DateFrom = (ASPxDateEdit)Parent.FindControl("txtTaxRates_DateFrom");
                ASPxComboBox CmbddlOrderType = (ASPxComboBox)Parent.FindControl("CmbddlOrderType");
                TextBox txttype_UserAccount = (TextBox)Parent.FindControl("txttype_UserAccount");
                TextBox txttype_UserAccount_hidden = (TextBox)Parent.FindControl("txttype_UserAccount_hidden");
                ASPxTextBox txtOrder_RefNumber = (ASPxTextBox)Parent.FindControl("txtOrder_RefNumber");
                TextBox txt_pOrder_AgentID = (TextBox)Parent.FindControl("txt_pOrder_AgentID");
                TextBox txt_pOrder_AgentID_hidden = (TextBox)Parent.FindControl("txt_pOrder_AgentID_hidden");
                ASPxMemo ASPxMemo1 = (ASPxMemo)Parent.FindControl("ASPxMemo1");
                ASPxComboBox CmbpOrder_PaymentTerm = (ASPxComboBox)Parent.FindControl("CmbpOrder_PaymentTerm");
                ASPxDateEdit ASPxDateEdit1 = (ASPxDateEdit)Parent.FindControl("ASPxDateEdit1");
                ASPxDateEdit txtpOrder_PaymentDate = (ASPxDateEdit)Parent.FindControl("txtpOrder_PaymentDate");
                ASPxComboBox ddlDeliveryAt = (ASPxComboBox)Parent.FindControl("ddlDeliveryAt_pOrderType");
                ASPxComboBox CmbpOrder_DeliveryBranch = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryBranch");
                ASPxComboBox CmbpOrder_DeliveryWareHouse = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryWareHouse");
                ASPxComboBox CmbpOrder_DeliveryAddress = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryAddress");
                ASPxMemo txtpOrder_DeliveryOther = (ASPxMemo)Parent.FindControl("txtpOrder_DeliveryOther");
                HiddenField pOderId_hidden = (HiddenField)Parent.FindControl("pOderId_hidden");
                ASPxTextBox txtpOrder_PaymentDays = (ASPxTextBox)Parent.FindControl("txtpOrder_PaymentDays"); 
                if (dt.Rows.Count >= 1)
                {
                    CmbpOrder_Branch.Value = Convert.ToString(dt.Rows[0]["pOrderBranch"]);
                    txtTaxRates_DateFrom.Date = Convert.ToDateTime(dt.Rows[0]["pOrderDate"]);
                    CmbddlOrderType.Value = Convert.ToString(dt.Rows[0]["pOrderType"]);
                    txttype_UserAccount.Text = Convert.ToString(dt.Rows[0]["contactname"]);
                    txttype_UserAccount_hidden.Text = Convert.ToString(dt.Rows[0]["pOrderContactID"]);
                    txtOrder_RefNumber.Value = Convert.ToString(dt.Rows[0]["pOrderRefNumber"]); 
                    //for show reference no in order details popup label
                    Session["_RefNumber"] = Convert.ToString(dt.Rows[0]["pOrderRefNumber"]); 
                    txt_pOrder_AgentID.Text = Convert.ToString(dt.Rows[0]["agentname"]);
                    txt_pOrder_AgentID_hidden.Text = Convert.ToString(dt.Rows[0]["pOrderAgentID"]);
                    ASPxMemo1.Value = Convert.ToString(dt.Rows[0]["pOrderInstructions"]);
                    CmbpOrder_PaymentTerm.Value = Convert.ToString(dt.Rows[0]["pOrderPaymentTerm"]);
                    ASPxDateEdit1.Date = Convert.ToDateTime(dt.Rows[0]["pOrderPaymentDate"]);
                    txtpOrder_PaymentDate.Date = Convert.ToDateTime(dt.Rows[0]["pOrderDeliveryDate"]);
                    ddlDeliveryAt.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryAt"]);
                    CmbpOrder_DeliveryBranch.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryBranch"]);
                    CmbpOrder_DeliveryWareHouse.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryWareHouse"]);
                    CmbpOrder_DeliveryAddress.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryAddress"]);
                    txtpOrder_DeliveryOther.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryOther"]);
                    pOderId_hidden.Value = Convert.ToString(dt.Rows[0]["pOrderBranch"]);
                    txtpOrder_PaymentDays.Value = Convert.ToString(dt.Rows[0]["pOrder_PaymentDays"]);
                }
            }
            if (ds.Tables.Count > 1)
            {
                dtpOrderDetails = ds.Tables[1];
                if (dtpOrderDetails.Rows.Count >= 1)
                {
                    UserControl userControl = (UserControl)Page.FindControl("Spinner1");
                    ASPxGridView gridView = (ASPxGridView)userControl.FindControl("cityGrid");
                    gridView.DataSource = dtpOrderDetails;
                    gridView.DataBind();
                }
            }


        }
    }

    #region MoreInfo
    #region PrivateMethod
    private void SizeBind()
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select Size_ID,Size_Name from Master_Size");
        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(ddlSize, dtCmb, "Size_Name", "Size_ID", 0);

        ddlSize.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
        ddlSize.SelectedIndex = 0;
    }
    private void ColorBind()
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select Color_ID,Color_Name from Master_Color");
        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(ddlColor, dtCmb, "Color_Name", "Color_ID", 0);

        ddlColor.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
        ddlColor.SelectedIndex = 0;
    }
    private void YearBind()
    {

        DataTable dtCmb = new DataTable();

        dtCmb.Columns.Add("id");
        dtCmb.Columns.Add("name");

        DataRow dr = dtCmb.NewRow();
        dr["id"] = "0";
        dr["name"] = "None";
        dtCmb.Rows.Add(dr);

        for (int i = 2000; i <= 2050; i++)
        {
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = Convert.ToString(i);
            drsession["name"] = Convert.ToString(i);
            dtCmb.Rows.Add(drsession);
        }


        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(ASPxYear, dtCmb, "name", "id", 0);
        ASPxYear.SelectedIndex = 0;

    }
    private void MoreInfoGridBind()
    {
        grdMoreInfo.DataSource = null;
        grdMoreInfo.DataBind();
    }
    private void MoreInfoInsert()
    {
        Store_MasterBL oStore_MasterBL = new Store_MasterBL();
        GenericMethod oGeneric = new GenericMethod();

        int mOderId = Convert.ToInt32(hdnMoreInfoOrderId.Value.Trim());
        string mNumber = "";
        string mType = GetMType();
        int mProductId = 0;
        if (!string.IsNullOrEmpty(txtProductDetails_hidden.Text.Trim()))
        {
            mProductId = Convert.ToInt32(txtProductDetails_hidden.Text.Trim());
        }
        string mBrand = Convert.ToString(txtProduct_Order_Detail_Brand.Text.Trim());
        int mSize = 0;
        if (!string.IsNullOrEmpty(Convert.ToString(ddlSize.SelectedItem.Value)))
        {
            mSize = Convert.ToInt32(ddlSize.SelectedItem.Value);
        }
        int mColor = 0;
        if (!string.IsNullOrEmpty(Convert.ToString(ddlColor.SelectedItem.Value)))
        {
            mColor = Convert.ToInt32(ddlColor.SelectedItem.Value);
        }
        int mMonth = 0;
        if (!string.IsNullOrEmpty(Convert.ToString(ASPxMonth.SelectedItem.Value)))
        {
            mMonth = Convert.ToInt32(ASPxMonth.SelectedItem.Value);
        }
        int mYear = 0;
        if (!string.IsNullOrEmpty(Convert.ToString(ASPxYear.SelectedItem.Value)))
        {
            mYear = Convert.ToInt32(ASPxYear.SelectedItem.Value);
        }
        string mPdescription = txtProductDescription.Text.Trim();
        decimal mQuantity = 0;
        if (!string.IsNullOrEmpty(Convert.ToString(txtQuantity.Text.Trim())))
        {
            mQuantity = Convert.ToDecimal(txtQuantity.Text.Trim());
        }
        int mQuantityUnit = 0;
        if (!string.IsNullOrEmpty(txtQntityunit_hidden.Text.Trim()))
        {
            mQuantityUnit = Convert.ToInt32(txtQntityunit_hidden.Text.Trim());
        }
        string mRemarks = txtAreaRemarks.Text.Trim();
        string mCreateUser = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
//        oGeneric.Insert_Table("Trans_JWorkStock", @"[JWorkStock_OrderID],[JWorkStock_Number],[JWorkStock_Type],[JWorkStock_ProductID],[JWorkStock_Brand]
//                                ,[JWorkStock_Size],[JWorkStock_Color],[JWorkStock_BestBeforeMonth],[JWorkStock_BestBeforeYear],[JWorkStock_ProductDescription]
//                                ,[JWorkStock_Quantity],[JWorkStock_QuantityUnit],[JWorkStock_Remarks],[JWorkStock_CreateUser],[JWorkStock_CreateTime]",
//                                @"'" + mOderId + "','" + mNumber + "','" + mType + "','" + mProductId + "','" + mBrand + "','" + mSize + "','" + mColor
//                                + "','" + mMonth + "','" + mYear + "','" + mPdescription + "','" + mQuantity + "','" + mQuantityUnit + "','" + mRemarks
//                                + "','" + mCreateUser + "',GETDATE()");

        oStore_MasterBL.InsertJobWorkStock(mOderId, mNumber, mType, mProductId, mBrand, mSize, mColor, mMonth, mYear, mPdescription, Convert.ToString(mQuantity), mQuantityUnit, mRemarks, Convert.ToInt32(mCreateUser));

        ClearInputs(Page.Controls);
        InfoGridBind(mOderId);
    }
    private string GetMType()
    {
        string type = string.Empty;
        DataTable dt = new DataTable();
        GenericMethod oGeneric = new GenericMethod();
        dt = oGeneric.GetDataTable("SELECT * FROM Trans_pOrder WHERE pOrder_ID ='" + Convert.ToString(hdnMoreInfoOrderId.Value.Trim()) + "'");
        if (dt.Rows.Count > 0)
        {
            type = Convert.ToString(dt.Rows[0]["pOrder_Type"]);
        }
        return type;
    }
    private void InfoGridBind(int id)
    {
        Store_MasterBL oStore_MasterBL = new Store_MasterBL();
        DataTable oDataTable = new DataTable();
        GenericMethod oGeneric = new GenericMethod();
        oDataTable = oStore_MasterBL.GetJobWOrkStockListById(Convert.ToInt32(id)); 
        grdMoreInfo.DataSource = oDataTable;
        grdMoreInfo.DataBind();
    }
    private void ClearInputs(ControlCollection ctrls)
    {
        foreach (Control ctrl in ctrls)
        {
            if (ctrl is TextBox)
                ((TextBox)ctrl).Text = string.Empty;
            else if (ctrl is DropDownList)
                ((DropDownList)ctrl).ClearSelection();

            ClearInputs(ctrl.Controls);
        }
    }
    #endregion

    #region Events

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        MoreInfoInsert();
    }
    protected void btnSavenClose_OnClick(object sender, EventArgs e)
    {
        MoreInfoInsert();
        Popup_MoreInfo.ShowOnPageLoad = false;
    }
    #endregion
    #endregion



}
