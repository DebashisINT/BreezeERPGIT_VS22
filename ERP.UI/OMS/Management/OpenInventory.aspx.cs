using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_OpenInventory : System.Web.UI.Page
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DataTable dtActivelist = new DataTable();
        StockDetails stock = new StockDetails();
        DBEngine oDBEngine = new DBEngine();
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
            //ClientScript.RegisterStartupScript(typeof(Page), "SomeError", "<script type='text/javascript'>alert('Error!');</script>"); 

            if (Session["msg"] != null)
            {
                ClientScript.RegisterStartupScript(typeof(Page), "SomeError", "<script type='text/javascript'>alert('Record Inserted Unsuccessful!');</script>");
                Session["msg"] = null;
            }
            if (!IsPostBack)
            {
                BindSize();
                BindColor();
                BindYear();
                BindGrid();
                BindLocation();
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "sasa", "alert('Record Inserted Unsuccessfull');", true);
                //try
                //{
                //    int b = 0;
                //    int a = 1 / b;
                //}
                //catch (Exception ex)
                //{
                //    ClientScript.RegisterStartupScript(typeof(Page), "SomeError", "<script type='text/javascript'>alert('Error!');</script>");
                //}
            }
        }

        protected void BindLocation()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select * from tbl_master_building order By bui_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(ddlLocation, dtCmb, "bui_Name", "bui_id", 0);
            ddlLocation.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
            ddlLocation.SelectedIndex = 0;


            if (Session["ddlLocation"] != null)
            {
                int indx = Convert.ToInt32(Session["ddlLocation"]);
                ddlLocation.SelectedIndex = indx;
                BindGrid();
            }

        }

        protected void BindSize()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select * from Master_Size order By Size_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(ddlSize, dtCmb, "Size_Name", "Size_ID", 0);
            ddlSize.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
            ddlSize.SelectedIndex = 0;

        }

        protected void BindColor()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select * from Master_Color order By Color_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(ddlColor, dtCmb, "Color_Name", "Color_ID", 0);
            ddlColor.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
            ddlColor.SelectedIndex = 0;

        }

        protected void BindYear()
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
                oAspxHelper.Bind_Combo(ddlYear, dtCmb, "name", "id", 0);
        }

        public void BindGrid()
        {
            //string[] monthNames = ["", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

            string[] monthNames = new string[] { "", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };



            dtActivelist = stock.GetSp_Fetch_StockDetails(0, Convert.ToInt32(ddlLocation.Value));

            if (dtActivelist.Rows.Count > 0)
            {
                ViewState["dtActivelist"] = dtActivelist;// dtActivelist_New;
                grdActive.DataSource = dtActivelist;// dtActivelist_New;
                grdActive.DataBind();

                foreach (GridViewRow gr in grdActive.Rows)
                {
                    HiddenField hddStock_BestBeforeMonth = gr.FindControl("hddStock_BestBeforeMonth") as HiddenField;

                    Label lblStock_BestBeforeMonth = gr.FindControl("lblStock_BestBeforeMonth") as Label;

                    lblStock_BestBeforeMonth.Text = monthNames[Convert.ToInt16(hddStock_BestBeforeMonth.Value)];


                    //HiddenField hddStock_vBestBeforeYear = gr.FindControl("hddStock_vBestBeforeYear") as HiddenField;

                    //Label lblStock_BestBeforeYearNew = gr.FindControl("lblStock_BestBeforeYearNew") as Label;

                    //lblStock_BestBeforeYearNew.Text =  hddStock_vBestBeforeYear.Value;



                }
            }
            else
            {
                dtActivelist.Rows.Add(dtActivelist.NewRow());
                grdActive.DataSource = dtActivelist;
                grdActive.DataBind();
                int totalcolums = grdActive.Rows[0].Cells.Count;
                grdActive.Rows[0].Cells.Clear();
                grdActive.Rows[0].Cells.Add(new TableCell());
                grdActive.Rows[0].Cells[0].ColumnSpan = totalcolums;
                grdActive.Rows[0].Cells[0].Text = "No Data Found";
            }


        }

        protected void BtnBindGridCall_OnClick(object sender, EventArgs e)
        { BindGrid(); }
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            int insertcount = 0;

            System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-GB");
            try
            {
                if (HddInventoryID.Value != "" && HddInventoryID.Value != null)
                {



                    string vExpDate, vcurrency, vPrice, vPriceLot, vPriceUnit, ddlExpYear = "";


                    if (txtExpiryDate.Text.Trim() == "")
                    {
                        vExpDate = "01/01/2010";
                    }
                    else
                    {
                        //vExpDate = txtExpiryDate.Text.Trim();
                        string[] dateParts = txtExpiryDate.Text.Trim().Split('-');
                        vExpDate = dateParts[0] + "/" + dateParts[1] + "/" + dateParts[2];
                    }

                    if (txtCurrency_hidden.Text.Trim() == "")
                    {
                        vcurrency = "0";
                    }
                    else
                    {
                        vcurrency = txtCurrency_hidden.Text.Trim();
                    }


                    if (txtPrice.Text.Trim() == "")
                    {
                        vPrice = "0";
                    }
                    else
                    {
                        vPrice = txtPrice.Text.Trim();
                    }


                    if (txtpricelot.Text.Trim() == "")
                    {
                        vPriceLot = "0";
                    }
                    else
                    {
                        vPriceLot = txtpricelot.Text.Trim();
                    }

                    if (txtpriceUnit_hidden.Text.Trim() == "")
                    {
                        vPriceUnit = "0";
                    }
                    else
                    {

                        if ((txtpricelot.Text.Trim() == "") && (txtpricelot.Text.Trim() == ""))
                        {
                            vPriceUnit = "0";
                        }
                        else
                        {
                            vPriceUnit = txtpriceUnit_hidden.Text.Trim();
                        }
                    }

                    if (ddlYear.SelectedItem != null)
                    {
                        ddlExpYear = ddlYear.SelectedItem.Value.ToString();
                    }
                    else
                    {
                        ddlExpYear = "0";
                    }



                    int Stockinsertdtl = stock.Update_StockDetails(Convert.ToInt32(Convert.ToString(HddInventoryID.Value).Trim()), Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim(),
                        Convert.ToString(HttpContext.Current.Session["LastFinYear"]).Trim(),
                        Convert.ToInt64(txtProduct_hidden.Text.Trim()),
                        txtBrand.Text.Trim(), Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()), Convert.ToInt32(Convert.ToString(ddlExpYear).Trim()),
                        Convert.ToInt32(Convert.ToString(ddlSize.SelectedItem.Value).Trim()), Convert.ToInt32(Convert.ToString(ddlColor.SelectedItem.Value).Trim()),
                        Convert.ToDateTime(vExpDate.Trim().Replace("-", "/"), provider), Convert.ToInt32(vcurrency.Trim()),
                        Convert.ToDecimal(vPrice.Trim()), Convert.ToInt32(vPriceLot.Trim()), Convert.ToInt32(vPriceUnit.Trim()),
                        Convert.ToInt32(txtUnit_hidden.Text.Trim()), Convert.ToDecimal(txtQuantity.Text.Trim()), Convert.ToInt32(Convert.ToString(ddlLocation.SelectedItem.Value).Trim()),
                        txtBatchNo.Text.Trim(), Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()), Convert.ToDateTime(txtAquireDate.Text.Trim().Replace("-", "/"), provider));
                    if (Stockinsertdtl == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "sasa", "alert('Record Inserted Unsuccessfull');", true);
                    }
                }
                else
                {


                    string vExpDate, vcurrency, vPrice, vPriceLot, vPriceUnit, ddlExpYear = "";


                    if (txtExpiryDate.Text.Trim() == "")
                    {
                        vExpDate = "01/01/2010";
                    }
                    else
                    {
                        //vExpDate = txtExpiryDate.Text.Trim();
                        string[] dateParts = txtExpiryDate.Text.Trim().Split('-');
                        vExpDate = dateParts[0] + "/" + dateParts[1] + "/" + dateParts[2];
                    }

                    if (txtCurrency_hidden.Text.Trim() == "")
                    {
                        vcurrency = "0";
                    }
                    else
                    {
                        vcurrency = txtCurrency_hidden.Text.Trim();
                    }


                    if (txtPrice.Text.Trim() == "")
                    {
                        vPrice = "0";
                    }
                    else
                    {
                        vPrice = txtPrice.Text.Trim();
                    }


                    if (txtpricelot.Text.Trim() == "")
                    {
                        vPriceLot = "0";
                    }
                    else
                    {
                        vPriceLot = txtpricelot.Text.Trim();
                    }


                    if (txtpriceUnit_hidden.Text.Trim() == "")
                    {
                        vPriceUnit = "0";
                    }
                    else
                    {
                        if ((txtpricelot.Text.Trim() == "") && (txtpricelot.Text.Trim() == ""))
                        {
                            vPriceUnit = "0";
                        }
                        else
                        {
                            vPriceUnit = txtpriceUnit_hidden.Text.Trim();
                        }
                    }



                    string Stockinsertdtl = stock.Insert_StockDetails(Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim(),
                          Convert.ToString(HttpContext.Current.Session["LastFinYear"]).Trim(),
                          Convert.ToInt64(txtProduct_hidden.Text.Trim()),
                          txtBrand.Text.Trim(), Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()), Convert.ToInt32(Convert.ToString(ddlYear.SelectedItem.Value).Trim()),
                          Convert.ToInt32(Convert.ToString(ddlSize.SelectedItem.Value).Trim()), Convert.ToInt32(Convert.ToString(ddlColor.SelectedItem.Value).Trim()),
                          Convert.ToDateTime(vExpDate.Replace("-", "/"), provider), Convert.ToInt32(vcurrency),
                          Convert.ToDecimal(vPrice.Trim()), Convert.ToInt32(vPriceLot.Trim()), Convert.ToInt32(vPriceUnit.Trim()),
                          Convert.ToInt32(txtUnit_hidden.Text.Trim()), Convert.ToDecimal(txtQuantity.Text.Trim()), Convert.ToInt32(Convert.ToString(ddlLocation.SelectedItem.Value).Trim()),
                          txtBatchNo.Text.Trim(), Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()), Convert.ToDateTime(txtAquireDate.Text.Trim().Replace("-", "/"), provider));
                    if (Stockinsertdtl.Equals("-1"))
                    {
                        Session["msg"] = "err";
                    }


                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(typeof(Page), "SomeError", "<script type='text/javascript'>alert('Error!');</script>");
                Session["msg"] = "err";
            }

            Clearfield();
            Session["ddlLocation"] = ddlLocation.SelectedIndex;
            Response.Redirect("OpenInventory.aspx");

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


        protected void linkbtn_OnClick(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;
            dtActivelist = new DataTable();
            dtActivelist = stock.GetSp_Fetch_StockDetails(Convert.ToInt32(id), Convert.ToInt32(ddlLocation.Value));

            BindSize();
            BindColor();
            BindYear();
            BindLocation();
            System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-GB");

            if (dtActivelist.Rows.Count > 0)
            {
                txtProduct.Text = dtActivelist.Rows[0]["sProducts_Name"].ToString();
                txtProduct.Enabled = false;
                txtProduct_hidden.Text = dtActivelist.Rows[0]["Inventory_ProductID"].ToString();
                //txtBrand.Text=
                ddlSize.Value = dtActivelist.Rows[0]["Inventory_Size"].ToString();
                ddlSize.Enabled = false;
                ddlColor.Value = dtActivelist.Rows[0]["Inventory_Color"].ToString();
                ddlColor.Enabled = false;
                txtBatchNo.Text = dtActivelist.Rows[0]["Inventory_BatchNumber"].ToString();

                txtBatchNo.Enabled = false;

                ddlMonth.Value = dtActivelist.Rows[0]["Inventory_BestBeforeMonth"].ToString();
                ddlMonth.Enabled = false;
                ddlYear.Value = dtActivelist.Rows[0]["Inventory_BestBeforeYear"].ToString();
                txtExpiryDate.Date = Convert.ToDateTime(dtActivelist.Rows[0]["Inventory_ExpiryDate"].ToString(), provider).Date;

                txtExpiryDate.Enabled = false;

                ddlYear.Enabled = false;
                ddlLocation.Value = dtActivelist.Rows[0]["Inventory_OwnLocationT"].ToString();
                txtUnit.Text = dtActivelist.Rows[0]["Quantity_UOM_Name"].ToString();
                txtUnit_hidden.Text = dtActivelist.Rows[0]["Inventory_QuantityUnit"].ToString();
                txtUnit.Enabled = false;
                HddInventoryID.Value = dtActivelist.Rows[0]["Inventory_ID"].ToString();

                txtQuantity.Text = quntituchkin(dtActivelist.Rows[0]["Inventory_QuantityIn"].ToString());

                txtPrice.Text = dtActivelist.Rows[0]["Inventory_UnitPrice"].ToString();
                txtpricelot.Text = dtActivelist.Rows[0]["Inventory_PriceLot"].ToString();
                txtpriceUnit.Text = dtActivelist.Rows[0]["PriceLotUnit_UOM_Name"].ToString();
                txtpriceUnit_hidden.Text = dtActivelist.Rows[0]["Inventory_PriceLotUnit"].ToString();
                txtCurrency.Text = dtActivelist.Rows[0]["Currency_Name"].ToString();
                txtCurrency_hidden.Text = dtActivelist.Rows[0]["Inventory_QuoteCurrency"].ToString();
                txtAquireDate.Date = Convert.ToDateTime(dtActivelist.Rows[0]["Inventory_Date"].ToString(), provider).Date;
                txtBrand.Text = dtActivelist.Rows[0]["Inventory_Brand"].ToString();
            }

        }

        protected string quntituchkin(string value)
        {

            string qty = Convert.ToString(value);
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
            return qty;
        }

        protected void linkbtn_Delete_OnClick(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            string DltStck = stock.Delete_StockDetails(Convert.ToInt32(id));
            BindGrid();
        }
        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            txtProduct.Enabled = true;
            ddlSize.Enabled = true;
            ddlColor.Enabled = true;
            ddlMonth.Enabled = true;
            ddlYear.Enabled = true;
            txtUnit.Enabled = true;
            Clearfield();
            BindGrid();
        }

        protected void ddlLocation_OnSelectedIndexChanged(object sender, EventArgs e)
        { BindGrid(); }
        protected void Clearfield()
        {
            txtProduct.Text = "";
            txtProduct_hidden.Text = "";
            txtBrand.Text = "";
            ddlSize.Value = "0";
            ddlColor.Value = "0";
            txtBatchNo.Text = "";
            ddlMonth.Value = "0";
            ddlYear.Value = "0";
            txtExpiryDate.Value = "";
            txtQuantity.Text = "";
            txtUnit.Text = "";
            txtUnit_hidden.Text = "";
            txtPrice.Text = "";
            txtpricelot.Text = "";
            txtpriceUnit.Text = "";
            txtpriceUnit_hidden.Text = "";
            txtCurrency.Text = "";
            txtCurrency_hidden.Text = "";
            txtAquireDate.Value = "";
            HddInventoryID.Value = "";

        }

    }
}