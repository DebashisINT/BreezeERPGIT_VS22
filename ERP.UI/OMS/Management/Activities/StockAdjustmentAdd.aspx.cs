using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Web.Services;
using ERP.Models;
using DataAccessLayer;

namespace ERP.OMS.Management.Activities
{
    public partial class StockAdjustmentAdd : System.Web.UI.Page
    {
        StockAdjustmentBL blLayer = new StockAdjustmentBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        MasterSettings objmaster = new MasterSettings();
        CommonBL cbl = new CommonBL();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                }
            }
            string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                }
            }

            //check this WH stk Tranfer from reconcile module or not
            //Start
            if (Request.QueryString["Key"] != "Add")
            {
                Int64 AdjId = Convert.ToInt64(Request.QueryString["Key"]);
                DataTable recondt = oDBEngine.GetDataTable("Select Isnull(ReconcileStockDetails_id,0) ReconValue from tbl_master_ReconcileStockDetails where Reconcile_StkAdjId=" + Convert.ToInt64(AdjId) + "");
                if (recondt.Rows.Count > 0 && recondt != null)
                {
                    if (Convert.ToString(recondt.Rows[0]["ReconValue"]) != "0" && Convert.ToString(recondt.Rows[0]["ReconValue"]) != "")
                    {
                        btn_SaveRecords.ClientVisible = false;
                    }

                }
            }
            //End


            if (!IsPostBack)
            {
                string TechnicianStockAdjustment = objmaster.GetSettings("TechnicianStockAdjustment");
                string StockAdjustmentQtyZero = cbl.GetSystemSettingsResult("StockAdjustmentQtyZero");

                if (!String.IsNullOrEmpty(StockAdjustmentQtyZero))
                {
                    if (StockAdjustmentQtyZero == "Yes")
                    {
                        hdnAllowQty.Value = "1";                      
                    }
                    else if (StockAdjustmentQtyZero.ToUpper().Trim() == "NO")
                    {
                        hdnAllowQty.Value = "0";                       
                    }
                }
                ddlHierarchy.Enabled = false;
                bindHierarchy();
                //txtTotalStockInHand.ClientEnabled = false;
                //txtStockInHand.ClientEnabled = false;
                if (TechnicianStockAdjustment != null)
                {
                    if (TechnicianStockAdjustment == "0")
                    {
                        DivTechnician.Style.Add("display", "none");                       
                    }
                    else
                    {
                        DivTechnician.Style.Add("display", "!inline-block");                       
                    }
                }             


                if (Request.QueryString["Key"] != "Add")
                {
                    string AdjId = Request.QueryString["Key"];
                    EditModeExecute(AdjId);
                    hdAddEdit.Value = "Edit";
                    hdAdjustmentId.Value = AdjId;
                    lblHeading.Text = "Modify Stock Adjustment";
                }
                else
                {
                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=47");

                    if (dtposTime != null && dtposTime.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                    }
                    hdAddEdit.Value = "Add";
                    
                    AddmodeExecuted();

                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Stock Adjustment";
                    btn_SaveRecords.Visible = false;
                    
                }

                //Surojit 30-04-2019

               
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");

                //Surojit 30-04-2019

            }
        }
        private void AddmodeExecuted()
        {

            DataSet allDetails = blLayer.PopulateStockAdjustmentDetails();
            CmbScheme.DataSource = allDetails.Tables[0];
            CmbScheme.ValueField = "Id";
            CmbScheme.TextField = "SchemaName";
            CmbScheme.DataBind();

            ddlBranch.DataSource = allDetails.Tables[1];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

            ccmTechnician.DataSource = allDetails.Tables[3];
            ccmTechnician.ValueField = "ID";
            ccmTechnician.TextField = "Technician_Name";
            ccmTechnician.DataBind();
            

            DateTime startDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_StartDate"]);
            DateTime LastDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_EndDate"]);
            dtTDate.MaxDate = LastDate;
            dtTDate.MinDate = startDate;

            if (DateTime.Now > LastDate)
                dtTDate.Date = LastDate;
            else
                dtTDate.Date = DateTime.Now;
        }
        public void EditModeExecute(string id)
        {
            DataSet EditedDataDetails = blLayer.GetEditedData(id);
            CmbScheme.DataSource = EditedDataDetails.Tables[0];
            CmbScheme.ValueField = "Id";
            CmbScheme.TextField = "SchemaName";
            CmbScheme.DataBind();

            ddlBranch.DataSource = EditedDataDetails.Tables[1];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

            cmbWarehouse.DataSource = EditedDataDetails.Tables[4];
            cmbWarehouse.ValueField = "WarehouseID";
            cmbWarehouse.TextField = "WarehouseName";
            cmbWarehouse.DataBind();

            ccmTechnician.DataSource = EditedDataDetails.Tables[5];
            ccmTechnician.ValueField = "ID";
            ccmTechnician.TextField = "Technician_Name";
            ccmTechnician.DataBind();


            DateTime startDate = Convert.ToDateTime(EditedDataDetails.Tables[2].Rows[0]["FinYear_StartDate"]);
            DateTime LastDate = Convert.ToDateTime(EditedDataDetails.Tables[2].Rows[0]["FinYear_EndDate"]);
            dtTDate.MaxDate = LastDate;
            dtTDate.MinDate = startDate;

            DataRow HeaderRow = EditedDataDetails.Tables[3].Rows[0];
            CmbScheme.Text = Convert.ToString(HeaderRow["SchemaName"]);
           // CmbScheme.Value = Convert.ToString(HeaderRow["SchemeId"]);
            CmbScheme.ClientEnabled = false;
            txtVoucherNo.Text = Convert.ToString(HeaderRow["Stock_No"]);
            dtTDate.Date = Convert.ToDateTime(HeaderRow["Stock_Date"]);
            dtTDate.ClientEnabled = false;
            ddlBranch.SelectedValue = Convert.ToString(HeaderRow["Branch_ID"]);
            hdnBranch.Value = Convert.ToString(HeaderRow["Branch_ID"]);
            ddlBranch.Enabled = false;

            cmbWarehouse.Value = Convert.ToString(HeaderRow["Stock_WarehouseId"]);
           // cmbWarehouse.Text = Convert.ToString(HeaderRow["WarehouseName"]);
            cmbWarehouse.ClientEnabled = false;

            ccmTechnician.Value = Convert.ToString(HeaderRow["Stock_TechnicianId"]);

            txtProdName.Text = Convert.ToString(HeaderRow["Product_Name"]);
            hdnProductId.Value = Convert.ToString(HeaderRow["Stock_ProductId"]);

            txtStockInHand.Value = Convert.ToString(HeaderRow["CurrenctStockInHand"]);
            hdnstockInHand.Value = Convert.ToString(HeaderRow["StockInHand"]);
            txtEnterAdjustQty.Text=Convert.ToString(HeaderRow["EnterAdjustmentQty"]);
            txtEnterAdjustQty.ClientEnabled = false;
            //check this WH stk Tranfer from reconcile module or not
            //Start
            DataTable recondt = oDBEngine.GetDataTable("Select Isnull(ReconcileStockDetails_id,0) ReconValue from tbl_master_ReconcileStockDetails where Reconcile_StkAdjId=" + Convert.ToInt64(id) + "");
            if (recondt.Rows.Count > 0 && recondt !=null)
            {
                if (Convert.ToString(recondt.Rows[0]["ReconValue"]) != "0" && Convert.ToString(recondt.Rows[0]["ReconValue"]) != "")
                {
                    txtProdName.ClientEnabled = false;
                }

            }

            //End
            txtTotalStockInHand.Text=Convert.ToString(HeaderRow["TotalStockInHand"]);
            hdnTotalStockInHand.Value = Convert.ToString(HeaderRow["TotalStockInHand"]);
            txtReason.InnerText = Convert.ToString(HeaderRow["Reason"]);
            txtEnterRate.Text = Convert.ToString(HeaderRow["EnterRate"]);

            lblStockHand.Text = Convert.ToString(HeaderRow["UOM_Name"]);
            ASPxLabel1.Text = Convert.ToString(HeaderRow["UOM_Name"]);
            ASPxLabel2.Text = Convert.ToString(HeaderRow["UOM_Name"]);
            hdnUOMpacking.Value = Convert.ToString(HeaderRow["PackingQty"]);
            lblAvailableStk.Text = Convert.ToString(HeaderRow["avlStock"]) + ' ' + Convert.ToString(HeaderRow["UOM_Name"]);

            lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(HeaderRow["Proj_Id"]));

            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(HeaderRow["Proj_Id"]) + "'");
            if (dt2.Rows.Count > 0)
            {
                ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
            }  
        }

        [WebMethod]
        public static string GetStockInHand(string ProductId, string WarehouseID, string BranchId)
        {
            StockAdjustmentBL blLayer = new StockAdjustmentBL();
            string StockIN = "0.00", StockUOM = "", strRate = "";
            DataTable AllStock = blLayer.PopulateStock(ProductId,WarehouseID,BranchId);
            if (AllStock.Rows.Count > 0)
            {
                StockIN = Convert.ToString(AllStock.Rows[0]["StockInHand"]);
                StockUOM = Convert.ToString(AllStock.Rows[0]["StockUOMName"]);
                strRate = Convert.ToString(AllStock.Rows[0]["Rate"]);
            }
            return StockIN + "~" + StockUOM + "~" + strRate; 
        }


        [WebMethod]
        public static string GetProductUOM(string ProductId)
        {
            StockAdjustmentBL blLayer = new StockAdjustmentBL();
            string ProductUOM = "0.00";
            DataTable AllStock = blLayer.PopulateProductUOM(ProductId);
            if (AllStock.Rows.Count > 0)
            {
                ProductUOM = Convert.ToString(AllStock.Rows[0]["UOM_Name"]);
              
            }
            return ProductUOM ;
        }
       
        public void BindWarehouse()
        {
            try
            {
                //string strBranch = Convert.ToString(ddlBranch.SelectedValue);

                //DataTable dt = new DataTable();

                //if (hdnmultiwarehouse.Value != "1")
                //{
                //    dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
                //}
                //else
                //{
                //    dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");
                //}

                //ddlWarehouse.Items.Clear();
                //ddlWarehouse.DataSource = dt;
                //ddlWarehouse.DataBind();

                //dt = new DataTable();
                //dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");


                //DropDownList ucddlWarehouse = (DropDownList)MultiWarehouceuc.ucddlWarehouse;

                //ucddlWarehouse.Items.Clear();
                //ucddlWarehouse.DataSource = dt;
                //ucddlWarehouse.DataBind();
                
            }
            catch { }
        }

        protected void btn_SaveRecords_Click(object sender, EventArgs e)
        {
            string adjustmentNumber = "";
            int adjustmentId = 0, ErrorCode = 0;

            //blLayer.AddEditStockAdjustment(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
            //   ddlBranch.SelectedValue, Convert.ToString(cmbWarehouse.Value), hdnProductId.Value, Convert.ToString(txtStockInHand.Value), txtEnterAdjustQty.Text,
            //   txtTotalStockInHand.Text, txtReason.InnerText, Convert.ToString(Session["userid"]), txtEnterRate.Text
            //   ,ref adjustmentId,
            //   ref adjustmentNumber, ref ErrorCode, Request.QueryString["Key"]);

            Int64 ProjId = 0;
            if (lookup_Project.Text != "")
            {
                string projectCode = lookup_Project.Text;
                DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
            }
            else if (lookup_Project.Text == "")
            {
                ProjId = 0;
            }

            if (hdnShowUOMConversionInEntry.Value!="1")
            {
                hdnUOMPackingSelectUom.Value = "0";
                hdnUOMpacking.Value = "0";
            }
           

            blLayer.AddEditStockAdjustment(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date,
              hdnBranch.Value, Convert.ToString(cmbWarehouse.Value), hdnProductId.Value, Convert.ToString(hdnstockInHand.Value), txtEnterAdjustQty.Text,
              Convert.ToString(hdnTotalStockInHand.Value), txtReason.InnerText, Convert.ToString(Session["userid"]), txtEnterRate.Text
              , Convert.ToString(ccmTechnician.Value), ProjId
              , ref adjustmentId,
              ref adjustmentNumber, ref ErrorCode, Request.QueryString["Key"], hdnUOMPackingUom.Value, hdnUOMPackingSelectUom.Value, hdnUOMpacking.Value);


            if (ErrorCode==0)
            {
                hdnMsg.Value = adjustmentNumber;
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>MSGShow();</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('" + adjustmentNumber + "')</script>");
                //Response.Redirect("StockAdjustmentList.aspx");
            }
            else if (ErrorCode == -12)
            {               
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('" + adjustmentNumber + "')</script>");
            }
            else if (ErrorCode == -9)
            {
                DataTable dts = new DataTable();
                dts = GetAddLockStatus();
                //grid.JSProperties["cpSaveSuccessOrFail"] = "AddLock";
                string AddLockStatus = (Convert.ToString(dts.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dts.Rows[0]["Lock_Todate"]));
                hDNlOCKMSG.Value = "DATA is Freezed between '" + AddLockStatus + "' for Add.";
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('DATA is Freezed between '" + AddLockStatus + "' for Add.')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>lOCKMSGShow();</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('" + adjustmentNumber + "')</script>");
            }

           

        }

        public DataTable GetAddLockStatus()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_StockAdjustment_details");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForStockAdjustment");

            dt = proc.GetTable();
            return dt;

        }
        protected void ProjectServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = Convert.ToString(ddlBranch.SelectedValue);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;
        }
        //End Rev


        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }
        //Tanmoy Hierarchy End

        //Tanmoy Hierarchy
        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();
            }
        }
    }
}