using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class OrderDeliveryScheduleAdd : System.Web.UI.Page
    {
        
        DataTable dst = new DataTable();
        string strBranchID = "";
        // Consolidatecustomer obj = new Consolidatecustomer();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        MasterSettings objmaster = new MasterSettings();
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/ConsolidatedCustomerList.aspx");

            CommonBL ComBL = new CommonBL();

            if (!IsPostBack)
            {
                string MultiUOMSelection = ComBL.GetSystemSettingsResult("MultiUOMSelection");
                if (!String.IsNullOrEmpty(MultiUOMSelection))
                {
                    if (MultiUOMSelection.ToUpper().Trim() == "YES")
                    {
                        hddnMultiUOMSelection.Value = "1";
                        multiuom.Style.Add("display", "block");
                    }
                    else if (MultiUOMSelection.ToUpper().Trim() == "NO")
                    {
                        hddnMultiUOMSelection.Value = "0";
                        multiuom.Style.Add("display", "none");
                    }
                }
                Session["exportval2"] = null;
                Session["MultiUOMData"] = null;
                string OrderId = Request.QueryString["OrderId"];
                hdOrderId.Value = OrderId;
                hdOrderdetailsID.Value = Request.QueryString["DetailsId"];

                if (Request.QueryString["DetailsId"] != null)
                {
                    grid.Visible = true;

                    hiddnmodid.Value = Request.QueryString["DetailsId"];
                    DataSet dt = new DataSet();
                    dt = GetDetailsForHeader(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        txt_OrderNumber.Text = Convert.ToString(dt.Tables[0].Rows[0]["Order_Number"]);
                        dt_date.Date = Convert.ToDateTime(Convert.ToString(dt.Tables[0].Rows[0]["Order_Date"]));
                        txt_ProductCode.Text = Convert.ToString(dt.Tables[0].Rows[0]["sProducts_Code"]);
                        txt_ProductName.Text = Convert.ToString(dt.Tables[0].Rows[0]["sProducts_Name"]);
                        txtProQty.Text = Convert.ToString(dt.Tables[0].Rows[0]["OrderDetails_Quantity"]);
                        txtUOM.Text = Convert.ToString(dt.Tables[0].Rows[0]["UOM_Name"]);
                        hdnProductId.Value = Convert.ToString(dt.Tables[0].Rows[0]["OrderDetails_ProductId"]);
                        hdnUOMId.Value = Convert.ToString(dt.Tables[0].Rows[0]["UOM_ID"]);
                    }
                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        hdnTotalDeliveryQty.Value = Convert.ToString(dt.Tables[1].Rows[0]["TotalDeliveryQty"]);
                    }

                    if (dt.Tables[2].Rows.Count > 0)
                    {
                        txtSerialNumber.Text = Convert.ToString(dt.Tables[2].Rows[0]["SerialNumber"]);
                    }


                    grid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                    grid.DataBind();
                }
                else
                {
                    hiddnmodid.Value = "0";
                }

                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");
            }
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string returnPara = Convert.ToString(e.Parameters);
            string FinYear = String.Empty;
            string User = String.Empty;
            string Company = String.Empty;

            string WhichCall = returnPara.Split('~')[0];

            DataTable MultiUOMDetails = new DataTable();
            


            if (WhichCall == "TemporaryData")
            {
                string WarrantyDay = txtWarranty.Text;
                string DeliveryDate = dt_DeliveryDate.Date.ToString("yyyy-MM-dd");
                string DeliveryQuantity = txtDeliveryQuantity.Text;
                string SerialNumber = txtSerialNumber.Text;
                if (Session["MultiUOMData"] != null)
                {
                    DataTable MultiUOM = (DataTable)Session["MultiUOMData"];
                    MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId", "BaseRate", "AltRate", "UpdateRow");
                }
                else
                {
                    MultiUOMDetails.Columns.Add("SrlNo", typeof(string));
                    MultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltUOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("ProductId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("DetailsId", typeof(string));
                    MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                }

                int SeduleID = InsertSeheduleDetails("InsertSeheduleDetails", WarrantyDay, DeliveryDate, DeliveryQuantity, Request.QueryString["DetailsId"], Request.QueryString["OrderId"], "0", MultiUOMDetails, SerialNumber);

                if (SeduleID > 0)
                {


                    //Rev 24428

                    DataSet dt = new DataSet();
                    dt = GetDetailsForHeader(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        hdnTotalDeliveryQty.Value = Convert.ToString(dt.Tables[1].Rows[0]["TotalDeliveryQty"]);
                    }
                    //End Rev 24428

                    grid.JSProperties["cpSaveSuccessOrFail"] = "Success";
                    //Rev 24428
                    grid.JSProperties["cphdnTotalDeliveryQty"] = hdnTotalDeliveryQty.Value;
                    Session["MultiUOMData"] = null;
                    //End Rev 24428
                    grid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                    grid.DataBind();
                }
            }
            else if (WhichCall == "ModifyData")
            {

                string WarrantyDay = txtWarranty.Text;
                string DeliveryDate = dt_DeliveryDate.Date.ToString("yyyy-MM-dd");
                string DeliveryQuantity = txtDeliveryQuantity.Text;
                string SerialNumber = txtSerialNumber.Text;

                if (Session["MultiUOMData"] != null)
                {
                    DataTable MultiUOM = (DataTable)Session["MultiUOMData"];
                    MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId", "BaseRate", "AltRate", "UpdateRow");
                }
                else
                {
                    MultiUOMDetails.Columns.Add("SrlNo", typeof(string));
                    MultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltUOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("ProductId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("DetailsId", typeof(string));
                    MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                }


                int SeduleID = InsertSeheduleDetails("ModifySeheduleDetails", WarrantyDay, DeliveryDate, DeliveryQuantity, Request.QueryString["DetailsId"], Request.QueryString["OrderId"], Convert.ToString(hdnDeliveryScheduleDetailsId.Value), MultiUOMDetails, SerialNumber);

                if (SeduleID > 0)
                {

                    //Rev 24428

                    DataSet dt = new DataSet();
                    dt = GetDetailsForHeader(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        hdnTotalDeliveryQty.Value = Convert.ToString(dt.Tables[1].Rows[0]["TotalDeliveryQty"]);
                    }
                    //End Rev 24428

                    grid.JSProperties["cpSaveSuccessOrFail"] = "Success";
                    //Rev 24428
                    grid.JSProperties["cphdnTotalDeliveryQty"] = hdnTotalDeliveryQty.Value;
                    //End Rev 24428
                    grid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                    grid.DataBind();

                    Session["MultiUOMData"] = null;
                }

               
            }
            else if (WhichCall == "Delete")
            {
                int ModId = Int32.Parse(returnPara.Split('~')[1]);

                int i2 = IDeleteReplacementDetails(ModId, "Delete");

                //Rev 24428

                DataSet dt = new DataSet();
                dt = GetDetailsForHeader(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    hdnTotalDeliveryQty.Value = Convert.ToString(dt.Tables[1].Rows[0]["TotalDeliveryQty"]);
                }
                //End Rev 24428






                if (i2 > 0)
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = "Delete";
                    //Rev 24428
                    grid.JSProperties["cphdnTotalDeliveryQty"] = hdnTotalDeliveryQty.Value;
                    //End Rev 24428
                }
            }
            else if (WhichCall == "Display")
            {
                grid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);
                grid.DataBind();
            }

            else if (WhichCall == "ClearData")
            {
                txtWarranty.Text = String.Empty;
                dt_DeliveryDate.Text = String.Empty;
                txtDeliveryQuantity.Text = String.Empty;
            }
            else if (WhichCall == "MultiUOMData")
            {
                Session["MultiUOMData"] = GetMultiUOMData(Convert.ToString(hdnProductId.Value));
            }
        }
        public int InsertSeheduleDetails(string Action, string WarrantyDay, string DeliveryDate, string DeliveryQuantity, string OrderDetailsId, string OrderId,
            string DeliveryScheduleDetails_Id, DataTable MultiUOMDetails, string SerialNumber)
        {
            int i;
            int rtrnvalue = 0;
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddPara("@Action", Action);
            proc.AddPara("@WarrantyDay", WarrantyDay);
            proc.AddPara("@DeliveryDate", DeliveryDate);
            proc.AddPara("@DeliveryQuantity", DeliveryQuantity);
            proc.AddPara("@DocID", OrderId);
            proc.AddPara("@DocDetailsID", OrderDetailsId);
            proc.AddPara("@DeliveryScheduleDetails_Id", DeliveryScheduleDetails_Id);
            proc.AddPara("@DeliverySchedule_CompanyID", Convert.ToString(Session["LastCompany"]));
            proc.AddPara("@DeliverySchedule_FinYear", Convert.ToString(Session["LastFinYear"]));
            proc.AddPara("@CreatedBy", Convert.ToString(Session["userid"]));
            proc.AddPara("@PackingUOM_ID", hdnUOMPackingSelectUom.Value);
            proc.AddPara("@PackingQty", hdnUOMpacking.Value);
            proc.AddPara("@MultiUOMDetails", MultiUOMDetails);
            proc.AddPara("@SerialNumber", SerialNumber);

            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int IDeleteReplacementDetails(int Mod, string Action)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddPara("@DeliveryScheduleDetails_Id", Convert.ToString(Mod));
            proc.AddPara("@Action", Action);
            return proc.RunActionQuery();
        }




        #region  ###### Insert  Update  DATA  through Grid Perform Call Back  ##############


        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //if (Session["OpeningDatatable"] != null)
            //{
            //    grid.DataSource = (DataTable)Session["OpeningDatatable"];
            //}
            grid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["DetailsId"], Request.QueryString["OrderId"]);

        }

        //For Project Code Tanmoy
        //public DataTable GetProjectCode(string Proj_Code)
        //{
        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
        //    proc.AddVarcharPara("@Action", 500, "GetProjectCode");
        //    proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
        //    dt = proc.GetTable();
        //    return dt;
        //}
        //For Project Code Tanmoy
        #endregion


        #region ########## Bind Data Customer wise   #############
        public DataTable GetConsolidatedCustomerGridData(string DetailsId, string OrderId)
        {
            try
            {

                DataTable dt = GetCustomesconsolidate("DeliveryScheduleBind", DetailsId, OrderId);
                return dt;
            }
            catch
            {
                return null;
            }

        }
        #endregion
        public DataTable GetCustomesconsolidate(string Action, string DetailsId, string OrderId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddPara("@Action", Action);
            proc.AddPara("@DocDetailsID", DetailsId);
            proc.AddPara("@DocID", OrderId);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetDetailsForHeader(string DetailsId, string OrderId)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddPara("@Action", "GetProductBySalesOrderDetailsID");
            proc.AddPara("@DocDetailsID", DetailsId);
            proc.AddPara("@DocID", OrderId);
            dt = proc.GetDataSet();
            return dt;
        }

        public void cmbExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval2"] == null)
                {
                    Session["exportval2"] = Filter;
                    // BindDropDownList();
                    bindexport2(Filter);
                }
                else if (Convert.ToInt32(Session["exportval2"]) != Filter)
                {
                    Session["exportval2"] = Filter;
                    // BindDropDownList();
                    bindexport2(Filter);
                }
            }

        }
        public void bindexport2(int Filter)
        {
            //GrdReplacement.Columns[6].Visible = false;
            string filename = "DeliveryScheduleDetails";
            exporter.FileName = filename;
            //    exporter.FileName = "SalesRegiserDetailsReport";

            exporter.PageHeader.Left = "Delivery Schedule";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.GridViewID = "grid";
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
        protected void MultiUOM_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string SpltCmmd = e.Parameters.Split('~')[0];
            // Rev Sanchita
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Rev Sanchita

            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "";

            if (SpltCmmd == "MultiUOMDisPlay")
            {
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMData"];
                }
                else
                {
                    MultiUOMData.Columns.Add("SrlNo", typeof(string));
                    MultiUOMData.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UOM", typeof(string));
                    MultiUOMData.Columns.Add("AltUOM", typeof(string));
                    MultiUOMData.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UomId", typeof(Int64));
                    MultiUOMData.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMData.Columns.Add("ProductId", typeof(Int64));
                   MultiUOMData.Columns.Add("DetailsId", typeof(string));
                    MultiUOMData.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("UpdateRow", typeof(string));

                }
                if (MultiUOMData != null && MultiUOMData.Rows.Count > 0)
                {
                    string SrlNo = e.Parameters.Split('~')[1];
                    DataView dvData = new DataView(MultiUOMData);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                     dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = MultiUOMData.DefaultView;
                    grid_MultiUOM.DataBind();
                }
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "OpenFocus";
            }

            else if (SpltCmmd == "SaveDisplay")
            {

                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();
                //Mantis 24428
                int MultiUOMSR = 1;
                //End Mantis 24428
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string DetailID = Convert.ToString(e.Parameters.Split('~')[9]);
                // Rev 24428
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);
                // End of Rev 24428


                DataTable allMultidataDetails = (DataTable)Session["MultiUOMData"];
                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");

                    foreach (DataRow item in MultiUoMresult)
                    {
                        if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                        {
                            if (AltQuantity == item["AltQuantity"].ToString())
                            {
                                Validcheck = "DuplicateUOM";
                                grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                                break;
                            }
                        }
                        // Rev Sanchita [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Rev Sanchita
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMData"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                    }
                    else
                    {
                        MultiUOMSaveData.Columns.Add("SrlNo", typeof(string));
                        MultiUOMSaveData.Columns.Add("Quantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltUOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltQuantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("AltUomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("ProductId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("DetailsId", typeof(string));
                        // Rev Sanchita
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));
                        // End of Rev Sanchita
                        //Mantis 24428
                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.AllowDBNull = false;
                        myDataColumn.AutoIncrement = true;
                        myDataColumn.AutoIncrementSeed = 1;
                        myDataColumn.AutoIncrementStep = 1;
                        myDataColumn.ColumnName = "MultiUOMSR";
                        myDataColumn.DataType = System.Type.GetType("System.Int32");
                        myDataColumn.Unique = true;
                        MultiUOMSaveData.Columns.Add(myDataColumn);
                        //End Mantis 24428
                    }
                     DataRow thisRow;
                     if (MultiUOMSaveData.Rows.Count > 0)
                     {
                         // Rev Sanchita
                         //thisRow = (DataRow)MultiUOMSaveData.Rows[MultiUOMSaveData.Rows.Count - 1];
                         MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                         MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, Convert.ToString(hdnDeliveryScheduleDetailsId.Value), BaseRate, AltRate, UpdateRow, MultiUOMSR);
                         // End of Rev Sanchita
                     }
                     // Rev Sanchita
                     //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                     else
                     {
                         MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, Convert.ToString(hdnDeliveryScheduleDetailsId.Value), BaseRate, AltRate, UpdateRow,MultiUOMSR);

                     }
                    // End of Rev Sanchita
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMData"] = MultiUOMSaveData;

                    if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData);
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                        grid_MultiUOM.DataSource = dvData;
                        grid_MultiUOM.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMData"] = MultiUOMSaveData;
                        grid_MultiUOM.DataSource = MultiUOMSaveData.DefaultView;
                        grid_MultiUOM.DataBind();
                    }
                }
            }

            else if (SpltCmmd == "MultiUomDelete")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        if (AltUOMKeyValue.ToString() == item["AltUomId"].ToString())
                        {
                            //dt.Rows.Remove(item);
                            if (AltUOMKeyqnty.ToString() == item["AltQuantity"].ToString())
                            {
                                item.Table.Rows.Remove(item);
                                break;
                            }
                        }
                    }
                }
                Session["MultiUOMData"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = null;
                    grid_MultiUOM.DataBind();
                }
            }

            else if (SpltCmmd == "CheckMultiUOmDetailsQuantity")
            {
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMData"] = dt;
            }
            // Mantis Issue 24428
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + AltUOMKeyValue + "'");

                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    Decimal AltRate = Convert.ToDecimal(MultiUoMresult[0]["AltRate"]);
                    Decimal AltUom = Convert.ToDecimal(MultiUoMresult[0]["AltUomId"]);
                    bool UpdateRow = Convert.ToBoolean(MultiUoMresult[0]["UpdateRow"]);

                    grid_MultiUOM.JSProperties["cpAllDetails"] = "EditData";

                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;
                    grid_MultiUOM.JSProperties["cpAltRate"] = AltRate;
                    grid_MultiUOM.JSProperties["cpUpdatedrow"] = UpdateRow;
                    grid_MultiUOM.JSProperties["cpuomid"] = AltUOMKeyValue;
                }
                Session["MultiUOMData"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {
                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[13];
                // Rev Sanchita
                string SrlNo = "0";
                string Validcheck = "";
                // End of Rev Sanchita

                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        // Rev SAnchita
                        SrlNo = Convert.ToString(item["SrlNo"]);
                        // End of Rev Sanchita


                    }


                }


                // Rev Sanchita
                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                // End of Rev Sanchita
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string DetailID = Convert.ToString(e.Parameters.Split('~')[9]);
                // Mantis Issue 24428
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);
                // End of Mantis Issue 24428
                //Rev Sanchita
                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and  MultiUOMSR <>'" + muid + "'");

                foreach (DataRow item in MultiUoMresultResult)
                {
                    if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                    {
                        if (AltQuantity == item["AltQuantity"].ToString())
                        {
                            Validcheck = "DuplicateUOM";
                            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                            break;
                        }
                    }
                    // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                    // End of Mantis Issue 24428 
                }


                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                            // Rev SAnchita
                            SrlNo = Convert.ToString(item["SrlNo"]);
                            // End of Rev Sanchita
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailID, BaseRate, AltRate, UpdateRow, muid);
                }
                //End Rev Sanchita

                Session["MultiUOMData"] = dt;
                MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMData"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // Rev Sanchita
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Rev Sanchita

                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }

            }


            // Rev Sanchita
            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                    DataRow[] MultiUoMresult = dt.Select("UpdateRow ='True'");

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);
                    Decimal AltQuantity = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    string AltUOM = Convert.ToString(MultiUoMresult[0]["AltUOM"]);
                    string AltUomId = Convert.ToString(MultiUoMresult[0]["AltUomId"]);
                    grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;
                    grid_MultiUOM.JSProperties["cpAltQuantity"] = AltQuantity;
                    grid_MultiUOM.JSProperties["cpAltUOM"] = AltUOM;                    
                    grid_MultiUOM.JSProperties["cpAltUomId"] = AltUomId;

                }
            }
            // End of Rev Sanchita
        }
        public DataTable GetMultiUOMData(string ProductID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails");
            proc.AddPara("@DeliverySchedule_ProductID", Convert.ToString(ProductID));
            proc.AddPara("@DeliveryScheduleDetails_Id", Convert.ToString(hdnDeliveryScheduleDetailsId.Value));
            ds = proc.GetTable();
            return ds;
        }
        [WebMethod]
        public static object AutoPopulateAltQuantity(Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();            
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddVarcharPara("@Action", 500, "AutoPopulateAltQuantityDetails");
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();           
            if (dt != null && dt.Rows.Count > 0)
            {
                RateLists.Add(new MultiUOMPacking
                       {
                           packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]),
                           sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]),
                           AltUOMId = Convert.ToInt32(dt.Rows[0]["AltUOMId"])
                       });
            }         
            return RateLists;
        }
        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {

        }
        public class MultiUOMPacking
        {
            public decimal packing_quantity { get; set; }
            public decimal sProduct_quantity { get; set; }
            public Int32 AltUOMId { get; set; }
        }
        [WebMethod]
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_DeliveryScheduleDetails");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetails");
            proc.AddIntegerPara("@UomId", UomId);
            proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
            }           
            return RateLists;
        }


        [WebMethod]
        public static Int32 GetUOMID(string  Uom)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            int id=0;

             
                id = oDBEngine.ExeInteger("select UOM_ID from Master_UOM where UOM_Name='"+Uom+"'");


                  return id;
            
        }


        [WebMethod]
        public static Int32 GetQuantityfromSL(string SLNo)
        {
            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMData"] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];               
                DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");         

                SLVal = MultiUoMresult.Length;
            }
            return SLVal;
        }


    }
}