using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Data;
using System.Resources;
using System.Collections;
using System.IO;

namespace ERP.OMS.Management.DailyTask
{
    public partial class Management_DailyTask_InvTransactions : System.Web.UI.UserControl
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DailyTask_Inventory oDailyTask_Inventory = new DailyTask_Inventory();
        string PositionId = string.Empty;
        //dsdsdsd
        protected void Page_Load(object sender, EventArgs e)
        {

            #region Stock section

            DrpLocStock.Items.Clear();
            DataTable oDataTable = (DataTable)HttpContext.Current.Session["StockDescFilter"];
            DrpLocStock.DataSource = oDataTable;
            DrpLocStock.DataTextField = "unit_StockInHand";
            DrpLocStock.DataValueField = "Stock_ID";
            DrpLocStock.DataBind();
            #endregion

            if (Session["ret"] != null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Key102", "jq132(document).ready(function() { Trans_Popup.Hide(); });", true);
                Session["ret"] = null;
            }
            if (!IsPostBack)
            {
                DeliveryType();
                SizeBind();
                ColorBind();
                Warehouse_DeliveryFromBind();
                Warehouse_DeliveryToBind();
                Customer_DeliveryFromBind();
                Customer_DeliveryToBind();
                BestBeforeYearBind();
                SetDateValue();
            }



            //new code block for showing key from resource page start

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/InventoryControlCentre.resx")))
            {
                ResourceReader resReader = new ResourceReader(Server.MapPath("~/Management/DailyTask/ResourceFiles/InventoryControlCentre.resx"));

                foreach (DictionaryEntry d in resReader)
                {
                    Label currLBL = new Label();
                    currLBL = (Label)Trans_Popup.FindControl(d.Key.ToString());

                    if (currLBL == null) { currLBL = (Label)Parent.FindControl(d.Key.ToString()); }

                    currLBL.Text = d.Value.ToString();
                }

                resReader.Close();
            }
            //new code block for showing key from resource page end
        }



        protected void BTNSave_clicked(object sender, EventArgs e)
        {
            string[] key = Convert.ToString(KeyField.Text).Split(',');
            string[] value = Convert.ToString(ValueField.Text).Split(',');
            string RexName = Convert.ToString(RexPageName.Text).Trim();

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx")))
            {
                File.Delete(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            }

            ResourceWriter resourceWriter = new ResourceWriter(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            for (int i = 0; i < key.Length; i++)
            {
                resourceWriter.AddResource(key[i].Trim(), value[i].Trim());
            }
            resourceWriter.Generate();
            resourceWriter.Close();

            Response.Redirect("");
        }

        protected bool ChekStock()
        {
            bool flag = false;
            DataTable dt = (DataTable)Session["StockDesc"];
            string OffcsId = Convert.ToString(cmbWHDeliveryFrom.SelectedItem.Value);
            decimal Quantity = Convert.ToDecimal(txtQuantity.Text);
            decimal quantsDt = 0;
            foreach (DataRow item in dt.Rows)
            {
                if (Convert.ToString(item["Location_Id"]).Equals(OffcsId))
                {
                    if (!Convert.ToString(item["Stock_In_Hand"]).Equals(""))
                    {
                        quantsDt = Convert.ToDecimal(item["Stock_In_Hand"]);
                        if (quantsDt >= Quantity)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }

                }

            }
            return flag;
        }
        #region Events
        protected void btnSave_Onclick(object sender, EventArgs e)
        {

            bool flag = false;

            if (cmbType.SelectedItem.Value.Equals("S") || cmbType.SelectedItem.Value.Equals("I") || cmbType.SelectedItem.Value.Equals("O"))
            {
                flag = ChekStock();
            }
            else
            {
                flag = true;
            }

            if (flag)
            {
                string PieceNo = txtPieceNo.Text;

                Session["ret"] = "True";
                ScriptManager.RegisterStartupScript(this, GetType(), "Key102", "jq132(document).ready(function() { Trans_Popup.Hide(); });", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "Key102", "alert('tst');", true);
                int NoofRowEffected = 0;
                HiddenField hdnTransactionEdit = (HiddenField)Parent.FindControl("hdnTransactionEdit");
                string Company = Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim();
                string Inventory_FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]).Trim();
                HiddenField hdnEditInventoryId = (HiddenField)Parent.FindControl("hdnEditInventoryId");
                int InventoryId = 0;
                if (!string.IsNullOrEmpty(hdnEditInventoryId.Value))
                {
                    InventoryId = Convert.ToInt32(hdnEditInventoryId.Value);
                }
                string OrderType = Convert.ToString(DBNull.Value);
                if (cmbType.SelectedItem != null)
                {
                    OrderType = Convert.ToString(cmbType.SelectedItem.Value);
                }
                string Date = null;
                if (!string.IsNullOrEmpty(Convert.ToString(dtDate.Date).Trim()))
                {
                    Date = Convert.ToDateTime(dtDate.Date).ToString("yyyy-MM-dd");
                }
                string OrderNo = null;
                if (!string.IsNullOrEmpty(txtOrderNo.Text.Trim()))
                {
                    OrderNo = Convert.ToString(txtOrderNo.Text.Trim());
                }
                string OrderDate = null;
                if (!string.IsNullOrEmpty(dtOrderDate.Text.Trim()))
                {
                    OrderDate = Convert.ToDateTime(dtOrderDate.Date).ToString("yyyy-MM-dd");
                }
                string CustomerorVendor = null;
                if (!string.IsNullOrEmpty(txtCustomer_hidden.Text.Trim()))
                {
                    CustomerorVendor = Convert.ToString(txtCustomer_hidden.Text.Trim());
                }

                long ProductID = 0;
                if (!string.IsNullOrEmpty(txtProduct_hidden.Text.Trim()))
                {
                    ProductID = Convert.ToInt64(txtProduct_hidden.Text.Trim());
                }
                string Brand = txtBrand.Text.Trim();
                int BestBeforeMonth = 0;
                if (cmbBestBeforeMonth.SelectedItem != null)
                {
                    BestBeforeMonth = Convert.ToInt32(cmbBestBeforeMonth.SelectedItem.Value);
                }
                int BestBeforeYear = 0;
                if (cmbBestBeforeYear.SelectedItem != null)
                {
                    BestBeforeYear = Convert.ToInt32(cmbBestBeforeYear.SelectedItem.Value);
                }
                int Size = 0;
                if (cmbSize.SelectedItem != null)
                {
                    Size = Convert.ToInt32(cmbSize.SelectedItem.Value);
                }
                int Color = 0;
                if (cmbColor.SelectedItem != null)
                {
                    Color = Convert.ToInt32(cmbColor.SelectedItem.Value);
                }
                string ExpiryDate = null; //need some information to add value into this parameter.
                int Currency = 0;
                if (!string.IsNullOrEmpty(txtCurrency_hidden.Text.Trim()))
                {
                    Currency = Convert.ToInt32(txtCurrency_hidden.Text.Trim());
                }
                decimal UnitPrice = 0;
                if (!string.IsNullOrEmpty(txtPrice.Text.Trim()))
                {
                    UnitPrice = Convert.ToDecimal(txtPrice.Text.Trim());
                }

                int PriceLot = 0;
                if (!string.IsNullOrEmpty(txtPerPrice.Text.Trim()))
                {
                    PriceLot = Convert.ToInt32(txtPerPrice.Text.Trim());
                }
                int PriceLotUnit = 0;
                if (!string.IsNullOrEmpty(txtPriceUnit_hidden.Text.Trim()))
                {
                    PriceLotUnit = Convert.ToInt32(txtPriceUnit_hidden.Text.Trim());
                }
                decimal QuantityIn = 0;
                decimal QuantityOut = 0;
                if (!string.IsNullOrEmpty(txtQuantity.Text.Trim()))
                {
                    if (OrderType == "P" || OrderType == "J")
                    {
                        QuantityIn = Convert.ToDecimal(txtQuantity.Text.Trim());
                    }
                    else if (OrderType == "S" || OrderType == "I")
                    {
                        QuantityOut = Convert.ToDecimal(txtQuantity.Text.Trim());
                    }
                    else if (OrderType == "R")
                    {
                        QuantityIn = Convert.ToDecimal(txtQuantity.Text.Trim());
                        QuantityOut = Convert.ToDecimal(txtQuantity.Text.Trim());

                    }
                }
                int QuantityUnit = 0;
                if (!string.IsNullOrEmpty(txtQuantityUnit_hidden.Text.Trim()))
                {
                    QuantityUnit = Convert.ToInt32(txtQuantityUnit_hidden.Text.Trim());
                }
                string BatchNo = null;
                if (!string.IsNullOrEmpty(txtBatchNo.Text.Trim()))
                {
                    BatchNo = Convert.ToString(txtBatchNo.Text.Trim());
                }
                string ManufactureDate = null;
                if (!string.IsNullOrEmpty(dtManufactureDate.Text.Trim()))
                {
                    ManufactureDate = Convert.ToDateTime(dtManufactureDate.Text.Trim()).ToString("yyyy-MM-dd");
                }
                string ProductDescription = null;
                if (!string.IsNullOrEmpty(memoDescription.Text.Trim()))
                {
                    ProductDescription = Convert.ToString(memoDescription.Text.Trim());
                }
                string DeliveryReferance = null;
                if (!string.IsNullOrEmpty(memoDeliveryRef.Text.Trim()))
                {
                    DeliveryReferance = Convert.ToString(memoDeliveryRef.Text.Trim());
                }
                string Remarks = null;
                if (!string.IsNullOrEmpty(memoRemarks.Text.Trim()))
                {
                    Remarks = Convert.ToString(memoRemarks.Text.Trim());
                }
                int CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());

                //string RecvDate = txtRecvDate.Text.Trim();

                string RecvDate = null;
                if (!string.IsNullOrEmpty(dtOrderDate.Text.Trim()))
                {
                    RecvDate = Convert.ToDateTime(txtRecvDate.Date).ToString("yyyy-MM-dd");
                }

                //condition for delivery address//

                string DeliveryAt = Convert.ToString(hdnDeliveryAt.Value);

                int Inventory_OwnLocationS = 0;
                int Inventory_OwnLocationT = 0;
                int Inventory_ContactLocationS = 0;
                int Inventory_ContactLocationT = 0;
                string Inventory_ContacOthertLocationS = string.Empty;
                string Inventory_ContactOtherLocationT = string.Empty;
                // A= Client Address
                if (DeliveryAt == "A")
                {
                    if (OrderType == "P" || OrderType == "J")
                    {
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                        Inventory_ContactLocationS = Convert.ToInt32(cmbClDeliveryFrom.SelectedItem.Value);
                    }
                    else if (OrderType == "S" || OrderType == "I" || OrderType == "O")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_ContactLocationT = Convert.ToInt32(cmbClDeliveryTo.SelectedItem.Value);
                    }
                    else if (OrderType == "R")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                    }
                }
                // B= Branch
                else if (DeliveryAt == "B")
                {
                    if (OrderType == "P" || OrderType == "J")
                    {
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                        Inventory_ContacOthertLocationS = txtOlDeliveryFrom.Text.Trim();
                    }
                    else if (OrderType == "S" || OrderType == "I" || OrderType == "O")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_ContactOtherLocationT = txtOlDeliveryTo.Text.Trim();
                    }
                    else if (OrderType == "R")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                    }
                }
                // O= Others
                else if (DeliveryAt == "O")
                {
                    if (OrderType == "P" || OrderType == "J")
                    {
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                        Inventory_ContacOthertLocationS = txtOlDeliveryFrom.Text.Trim();
                    }
                    else if (OrderType == "S" || OrderType == "I" || OrderType == "O")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_ContactOtherLocationT = txtOlDeliveryTo.Text.Trim();
                    }
                    else if (OrderType == "R")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                    }
                }
                // w= warehouse
                else if (DeliveryAt == "W")
                {
                    if (OrderType == "P" || OrderType == "J")
                    {
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);

                    }
                    else if (OrderType == "S" || OrderType == "I" || OrderType == "O")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                    }
                    else if (OrderType == "R")
                    {
                        Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
                        Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
                    }
                }

                if (Convert.ToString(hdnTransactionEdit.Value) == "edit")
                {
                    string mode = "Update";
                    NoofRowEffected = oDailyTask_Inventory.InsertTransaction(mode, InventoryId, Company, Inventory_FinYear, OrderType, Date, OrderNo, OrderDate, CustomerorVendor,
                    ProductID, Brand, BestBeforeMonth, BestBeforeYear, Size, Color, ExpiryDate, Currency, UnitPrice, PriceLot, PriceLotUnit, QuantityIn,
                    QuantityOut, QuantityUnit, BatchNo, ManufactureDate, ProductDescription, DeliveryReferance, Remarks, Inventory_OwnLocationS, Inventory_OwnLocationT,
                    Inventory_ContactLocationS, Inventory_ContactLocationT, Inventory_ContacOthertLocationS, Inventory_ContactOtherLocationT, CreateUser, RecvDate, PieceNo);
                }
                else
                {
                    string mode = "Insert";
                    NoofRowEffected = oDailyTask_Inventory.InsertTransaction(mode, InventoryId, Company, Inventory_FinYear, OrderType, Date, OrderNo, OrderDate, CustomerorVendor,
                    ProductID, Brand, BestBeforeMonth, BestBeforeYear, Size, Color, ExpiryDate, Currency, UnitPrice, PriceLot, PriceLotUnit, QuantityIn,
                    QuantityOut, QuantityUnit, BatchNo, ManufactureDate, ProductDescription, DeliveryReferance, Remarks, Inventory_OwnLocationS, Inventory_OwnLocationT,
                    Inventory_ContactLocationS, Inventory_ContactLocationT, Inventory_ContacOthertLocationS, Inventory_ContactOtherLocationT, CreateUser, RecvDate, PieceNo);
                }
                if (NoofRowEffected > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "sasa", "alert('Record Inserted Successfully');document.getElementById(Parea_Trans).style.display='none';document.getElementById(Parea).style.display='none';", true);
                    HiddenField hdnPopupsCheck = (HiddenField)Parent.FindControl("hdnPopupsCheck");
                    hdnPopupsCheck.Value = Convert.ToString(NoofRowEffected);
                    Session["TrnsSave"] = "true";
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "sasa", "alert('Please check your product quantity');", true);
            }


        }
        protected void btnConversionSubmit_OnClick(object sender, EventArgs e)
        {
            int NoofRowEffected = 0;
            HiddenField hdnTransactionEdit = (HiddenField)Parent.FindControl("hdnTransactionEdit");
            string Company = Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim();
            string Inventory_FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]).Trim();
            HiddenField hdnEditInventoryId = (HiddenField)Parent.FindControl("hdnEditInventoryId");
            int InventoryId = 0;
            string PieceNo = "0";
            if (!string.IsNullOrEmpty(txtPieceNo.Text))
            {
                PieceNo = txtPieceNo.Text;
            }
            if (!string.IsNullOrEmpty(hdnEditInventoryId.Value))
            {
                InventoryId = Convert.ToInt32(hdnEditInventoryId.Value);
            }
            string OrderType = "P";
            string Date = null;
            if (!string.IsNullOrEmpty(Convert.ToString(dtDate.Date).Trim()))
            {
                Date = Convert.ToDateTime(dtDate.Date).ToString("yyyy-MM-dd");
            }
            string OrderNo = null;
            if (!string.IsNullOrEmpty(txtOrderNo.Text.Trim()))
            {
                OrderNo = Convert.ToString(txtOrderNo.Text.Trim());
            }
            string OrderDate = null;
            if (!string.IsNullOrEmpty(dtOrderDate.Text.Trim()))
            {
                OrderDate = Convert.ToDateTime(dtOrderDate.Date).ToString("yyyy-MM-dd");
            }
            string CustomerorVendor = null;
            if (!string.IsNullOrEmpty(txtCustomer_hidden.Text.Trim()))
            {
                CustomerorVendor = Convert.ToString(txtCustomer_hidden.Text.Trim());
            }

            long ProductID = 0;
            if (!string.IsNullOrEmpty(txtProduct_hidden.Text.Trim()))
            {
                ProductID = Convert.ToInt64(txtProduct_hidden.Text.Trim());
            }
            string Brand = txtBrand.Text.Trim();
            int BestBeforeMonth = 0;
            if (cmbBestBeforeMonth.SelectedItem != null)
            {
                BestBeforeMonth = Convert.ToInt32(cmbBestBeforeMonth.SelectedItem.Value);
            }
            int BestBeforeYear = 0;
            if (cmbBestBeforeYear.SelectedItem != null)
            {
                BestBeforeYear = Convert.ToInt32(cmbBestBeforeYear.SelectedItem.Value);
            }
            int Size = 0;
            if (cmbSize.SelectedItem != null)
            {
                Size = Convert.ToInt32(cmbSize.SelectedItem.Value);
            }
            int Color = 0;
            if (cmbColor.SelectedItem != null)
            {
                Color = Convert.ToInt32(cmbColor.SelectedItem.Value);
            }
            string ExpiryDate = null; //need some information to add value into this parameter.
            int Currency = 0;
            if (!string.IsNullOrEmpty(txtCurrency_hidden.Text.Trim()))
            {
                Currency = Convert.ToInt32(txtCurrency_hidden.Text.Trim());
            }
            decimal UnitPrice = 0;
            if (!string.IsNullOrEmpty(txtPrice.Text.Trim()))
            {
                UnitPrice = Convert.ToDecimal(txtPrice.Text.Trim());
            }

            int PriceLot = 0;
            if (!string.IsNullOrEmpty(txtPerPrice.Text.Trim()))
            {
                PriceLot = Convert.ToInt32(txtPerPrice.Text.Trim());
            }
            int PriceLotUnit = 0;
            if (!string.IsNullOrEmpty(txtPriceUnit_hidden.Text.Trim()))
            {
                PriceLotUnit = Convert.ToInt32(txtPriceUnit_hidden.Text.Trim());
            }
            decimal Quantity = 0;
            int QuantityUnit = 0;
            if (!string.IsNullOrEmpty(hdnQuantity_toconvertUnit.Value.Trim()))
            {
                QuantityUnit = Convert.ToInt32(hdnQuantity_toconvertUnit.Value.Trim());
            }
            string BatchNo = string.Empty;
            if (!string.IsNullOrEmpty(txtBatchNo.Text.Trim()))
            {
                BatchNo = Convert.ToString(txtBatchNo.Text.Trim());
            }
            string ManufactureDate = null;
            if (!string.IsNullOrEmpty(dtManufactureDate.Text.Trim()))
            {
                ManufactureDate = Convert.ToDateTime(dtManufactureDate.Text.Trim()).ToString("yyyy-MM-dd");
            }
            string ProductDescription = null;
            if (!string.IsNullOrEmpty(memoDescription.Text.Trim()))
            {
                ProductDescription = Convert.ToString(memoDescription.Text.Trim());
            }
            string DeliveryReferance = null;
            if (!string.IsNullOrEmpty(memoDeliveryRef.Text.Trim()))
            {
                DeliveryReferance = Convert.ToString(memoDeliveryRef.Text.Trim());
            }
            string Remarks = null;
            if (!string.IsNullOrEmpty(memoRemarks.Text.Trim()))
            {
                Remarks = Convert.ToString(memoRemarks.Text.Trim());
            }
            int CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());

            //condition for delivery address//

            string DeliveryAt = Convert.ToString(hdnDeliveryAt.Value);

            int Inventory_OwnLocationS = 0;
            int Inventory_OwnLocationT = 0;
            int Inventory_ContactLocationS = 0;
            int Inventory_ContactLocationT = 0;
            string Inventory_ContacOthertLocationS = string.Empty;
            string Inventory_ContactOtherLocationT = string.Empty;
            Inventory_OwnLocationS = Convert.ToInt32(cmbWHDeliveryFrom.SelectedItem.Value);
            Inventory_OwnLocationT = Convert.ToInt32(cmbWHDeliveryTo.SelectedItem.Value);
            string mode = "Insert";
            try
            {
                //unique order reference for pair 
                //Guid id = Guid.NewGuid();
                //string OrderIds = "Z" + id;

                decimal QuantityIn = Convert.ToDecimal(txtQuantity_toconvert.Text.Trim());
                QuantityUnit = Convert.ToInt32(hdnQuantity_toconvertUnit.Value.Trim());
                decimal QuantityOut = 0;
                Inventory_OwnLocationS = 0;
                string RecvDate = txtRecvDate.Text.Trim();
                // stock in new UOM
                NoofRowEffected = oDailyTask_Inventory.InsertTransaction(mode, InventoryId, Company, Inventory_FinYear, OrderType, Date, OrderNo, OrderDate, CustomerorVendor,
                ProductID, Brand, BestBeforeMonth, BestBeforeYear, Size, Color, ExpiryDate, Currency, UnitPrice, PriceLot, PriceLotUnit, QuantityIn,
                QuantityOut, QuantityUnit, BatchNo, ManufactureDate, ProductDescription, DeliveryReferance, Remarks, Inventory_OwnLocationS, Inventory_OwnLocationT,
                Inventory_ContactLocationS, Inventory_ContactLocationT, Inventory_ContacOthertLocationS, Inventory_ContactOtherLocationT,
                CreateUser, RecvDate, PieceNo);


                if (NoofRowEffected > 1)
                {
                    // stock out exesting UOM
                    decimal QuantityIn1 = 0;
                    decimal QuantityOut1 = Convert.ToDecimal(txtWarehouseQuantity.Text.Trim());
                    QuantityUnit = Convert.ToInt32(hdnWarehouseQuantityUnit.Value.Trim());


                    Inventory_OwnLocationT = 0;

                    NoofRowEffected = oDailyTask_Inventory.InsertTransaction(mode, InventoryId, Company, Inventory_FinYear, OrderType, Date, OrderNo, OrderDate, CustomerorVendor,
                    ProductID, Brand, BestBeforeMonth, BestBeforeYear, Size, Color, ExpiryDate, Currency, UnitPrice, PriceLot, PriceLotUnit, QuantityIn1,
                    QuantityOut1, QuantityUnit, BatchNo, ManufactureDate, ProductDescription, DeliveryReferance, Remarks, Inventory_OwnLocationS, Inventory_OwnLocationT,
                    Inventory_ContactLocationS, Inventory_ContactLocationT, Inventory_ContacOthertLocationS, Inventory_ContactOtherLocationT, CreateUser,
                    RecvDate, PieceNo);
                }
                DataTable oTable = oGenericMethod.GetDataTable("SELECT TOP 1 Inventory_ID FROM Trans_Inventory ORDER BY Inventory_ID DESC");
                /*int PieceNo = 0;
                if (oTable.Rows.Count > 0) 
                {
                    PieceNo = Convert.ToInt32(oTable.Rows[0]["Inventory_ID"]);
                }*/
                Page.ClientScript.RegisterStartupScript(this.GetType(), "sasb", "alert('Piece No:'" + PieceNo + "'');", true);
            }
            catch { }

        }
        #endregion

        #region Methods
        private void DeliveryType()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("id");
            dt.Columns.Add("name");

            DataRow drsession = dt.NewRow();
            drsession["id"] = "P";
            drsession["name"] = "Purchase Order To Vendors";
            dt.Rows.Add(drsession);

            drsession = dt.NewRow();
            drsession["id"] = "S";
            drsession["name"] = "Purchase Order Of Customer";
            dt.Rows.Add(drsession);

            drsession = dt.NewRow();
            drsession["id"] = "R";
            drsession["name"] = "Requisition for Stock [Inter-Location]";
            dt.Rows.Add(drsession);

            drsession = dt.NewRow();
            drsession["id"] = "J";
            drsession["name"] = "Job work to Vendors";
            dt.Rows.Add(drsession);

            drsession = dt.NewRow();
            drsession["id"] = "I";
            drsession["name"] = "Job work from customer";
            dt.Rows.Add(drsession);

            drsession = dt.NewRow();
            drsession["id"] = "O";
            drsession["name"] = "Samples to Customers as Business Promotion";
            dt.Rows.Add(drsession);

            AspxHelper oAspxHelper = new AspxHelper();
            if (dt.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbType, dt, "name", "id", 0);
        }

        private void SizeBind()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGenericMethod.GetDataTable("SELECT Size_ID,Size_Name FROM Master_Size");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dt.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbSize, dt, "Size_Name", "Size_ID", 0);
            cmbType.SelectedIndex = 0;
        }

        private void ColorBind()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGenericMethod.GetDataTable("SELECT Color_ID,Color_Name FROM Master_Color");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dt.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbColor, dt, "Color_Name", "Color_ID", 0);
            cmbType.SelectedIndex = 0;
        }

        private void Warehouse_DeliveryFromBind()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGenericMethod.GetDataTable("select bui_id as WareHouse_ID,bui_Name as WareHouse_Name from tbl_master_building");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dt.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbWHDeliveryFrom, dt, "WareHouse_Name", "WareHouse_ID", 0);
            cmbWHDeliveryFrom.SelectedIndex = 0;
        }

        private void Warehouse_DeliveryToBind()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGenericMethod.GetDataTable("select bui_id as WareHouse_ID,bui_Name as WareHouse_Name from tbl_master_building");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dt.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbWHDeliveryTo, dt, "WareHouse_Name", "WareHouse_ID", 0);
            cmbWHDeliveryTo.SelectedIndex = 0;
        }

        private void Customer_DeliveryFromBind()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGenericMethod.GetDataTable("select add_id,add_addressType + ' ('+ SUBSTRING (add_address1,1,5) +'..)' as add_addressType from tbl_master_address where add_address1<>''");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dt.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbClDeliveryFrom, dt, "add_addressType", "add_id", 0);
            cmbClDeliveryFrom.SelectedIndex = 0;

        }

        private void Customer_DeliveryToBind()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGenericMethod.GetDataTable("select add_id,add_addressType + ' ('+ SUBSTRING (add_address1,1,5) +'..)' as add_addressType from tbl_master_address where add_address1<>''");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dt.Rows.Count > 0)
                oAspxHelper.Bind_Combo(cmbClDeliveryTo, dt, "add_addressType", "add_id", 0);
            cmbClDeliveryTo.SelectedIndex = 0;
        }

        private void BestBeforeYearBind()
        {
            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");

            DataRow dr = dtCmb.NewRow();
            dr["id"] = "0";
            dr["name"] = "";
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
                oAspxHelper.Bind_Combo(cmbBestBeforeYear, dtCmb, "name", "id", 0);
        }

        private void SetDateValue()
        {
            dtDate.Date = System.DateTime.Today;
            //dtManufactureDate.Date = System.DateTime.Today;
            dtOrderDate.Date = System.DateTime.Today;
        }
        #endregion

    }
}