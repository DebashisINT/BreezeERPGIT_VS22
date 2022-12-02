using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BusinessLogicLayer;
using DevExpress.Web.Data;
using DevExpress.Web;
using System.Data.SqlClient;
using System.Collections;

namespace ERP.OMS.Management.Master
{
    public partial class SystemControl : System.Web.UI.Page
    {
        MasterSettings masterBl = new MasterSettings();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Init(object sender, EventArgs e)
        {
           // ((GridViewDataComboBoxColumn)BackEntries.Columns["DaysId"]).PropertiesComboBox.DataSource = CmbDays();
            //GridViewDataComboBoxColumn combo = ((GridViewDataComboBoxColumn)BackEntries.Columns["DaysId"]);
            //combo.PropertiesComboBox.DataSource = CmbDays();
            if (!IsPostBack)
            {
                BackEntries.DataBind();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string ActiveEInvoice = Convert.ToString(masterBl.GetSettings("ActiveEInvoice"));
            if (!String.IsNullOrEmpty(ActiveEInvoice))
            {
                if (ActiveEInvoice == "1")
                {
                    hdnActiveEInvoice.Value = "1";

                  }
                else if (ActiveEInvoice.ToUpper().Trim() == "0")
                {
                    hdnActiveEInvoice.Value = "0";
                }
            }

            if(!IsPostBack)
            {
                getSystemControl();

                #region Image Compression
                Imageresolutionpopulate();
                #endregion

                #region Print Of Copies
                PrintOfCopies();
                #endregion
                #region Influencer Limit
                InfluencerLimit();
                #endregion

                #region Auto Invoice - Holiday
                HolidayBind();
                #endregion

                string mastersettings = masterBl.GetSettings("isServiceManagementRequred");
                if (mastersettings == "0")
                {
                    DivActualWarehouse.Style.Add("display", "none");
                    DivReplaceablehouse.Style.Add("display", "none");
                    DivAutoAdjustmentNumbering.Style.Add("display", "none");
                    DivDefectWarehouse.Style.Add("display", "none");
                    DivAutoNumberingforDefect.Style.Add("display", "none");

                }
                else
                {
                    DivActualWarehouse.Style.Add("display", "!inline-block");
                    DivReplaceablehouse.Style.Add("display", "!inline-block");
                    DivAutoAdjustmentNumbering.Style.Add("display", "!inline-block");
                    DivDefectWarehouse.Style.Add("display", "!inline-block");
                    DivAutoNumberingforDefect.Style.Add("display", "!inline-block");
                }

                BindWarehouse();
                BindAutoAdjustmentNumbering();
                BindDefectNumbering();
                BindMainAccountPartyJournal();
                BindReturnTransferNumbering();

            }
        }

        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void BackEntries_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            
           // ((GridViewDataComboBoxColumn)BackEntries.Columns["DaysId"]).PropertiesComboBox.DataSource = CmbDays();
             BackEntries.DataSource = GetGriddata();
             BackEntries.DataBind();
          

        }

       
        protected void BackEntries_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //if (e.Column.FieldName == "DaysId")
            //{
            //    //((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(BindWithdrawalForm);
            //    ((GridViewDataComboBoxColumn)BackEntries.Columns["DaysId"]).PropertiesComboBox.DataSource = CmbDays();
            //}
        }
        protected void BackEntries_DataBinding(object sender, EventArgs e)
        {
            BackEntries.DataSource = GetGriddata();
        }

        protected void BackEntries_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                string issuccess = string.Empty;
                foreach (var args in e.UpdateValues)
                {
                    string moduleids = Convert.ToString(args.Keys["moduleids"]);

                    string DaysId = Convert.ToString(args.NewValues["DaysId"]);
                   
                   

                    DataSet dsEmail = new DataSet();
                    //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection con = new SqlConnection(conn);
                    SqlCommand cmd3 = new SqlCommand("Prc_BackDatedEntries_Insert", con);
                    cmd3.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(moduleids))
                    {
                        cmd3.Parameters.AddWithValue("@Module_Id", Convert.ToInt64(moduleids));
                        cmd3.Parameters.AddWithValue("@DaysId", Convert.ToInt64(DaysId));
                    }
                   

                  
                    if (!string.IsNullOrEmpty(moduleids))
                    {
                        cmd3.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd3;
                        Adap.Fill(dsEmail);
                        dsEmail.Clear();
                        cmd3.Dispose();
                        con.Dispose();
                        GC.Collect();
                        issuccess = "true";
                    }

                }
                if (issuccess == "true")
                {
                    BackEntries.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Successfully.')</script>");
                }

            }
            catch (Exception ex)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Error exception.Please try again.')</script>");
            }


        }
        public IEnumerable CmbDays()
        {


            List<Days> LevelList = new List<Days>();
            DataTable DT = new DataTable();

            DT = oDBEngine.GetDataTable("select BackDated_Number_Id DaysId,Dated_Id Value from Tbl_BackDated_Number");
          
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Days Levels = new Days();
                Levels.DaysId = Convert.ToInt64(DT.Rows[i]["DaysId"]);
                Levels.Value = Convert.ToInt64(DT.Rows[i]["Value"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
        private IEnumerable GetGriddata()
        {

            List<BackDatedModuleList> approvallist = new List<BackDatedModuleList>();


            DataTable dt = new DataTable();


            dt = oDBEngine.GetDataTable("select ListedModule_Id moduleids,Module_Name modulenames,isnull(Days_Number,0) DaysId  from tbl_BackDated_ListedModule");



            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BackDatedModuleList Vouchers = new BackDatedModuleList();
                    Vouchers.moduleids = Convert.ToInt64(dt.Rows[i]["moduleids"]);

                    Vouchers.modulenames = Convert.ToString(dt.Rows[i]["modulenames"]);
                    Vouchers.DaysId = Convert.ToInt64(dt.Rows[i]["DaysId"]);

                   // if (Convert.ToInt64(dt.Rows[i]["DaysId"]) != null) { Vouchers.DaysId = Convert.ToInt64(dt.Rows[i]["DaysId"]); }

                    approvallist.Add(Vouchers);
                }
            }

            return approvallist;

        }

        void BindAutoAdjustmentNumbering()
        {
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
            proc.AddVarcharPara("@Action", 50, "BinddjustmentNumbering");           
            DataTable warehouse = proc.GetTable();
            if (warehouse.Rows.Count > 0)
            {
                cmbAdjustmentNumbering.DataSource = warehouse;
                cmbAdjustmentNumbering.ValueField = "Id";
                cmbAdjustmentNumbering.TextField = "SchemaName";
                cmbAdjustmentNumbering.DataBind();
            }
        }

        void BindMainAccountPartyJournal()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
            proc.AddVarcharPara("@Action", 50, "BindMainAccountPartyjournal");
            DataTable dtMainAccount = proc.GetTable();
            if (dtMainAccount.Rows.Count > 0)
            {
                cmbMainAccount.DataSource = dtMainAccount;
                cmbMainAccount.ValueField = "MainAccount_ReferenceID";
                cmbMainAccount.TextField = "MainAccount_Name";
                cmbMainAccount.DataBind();
            }
        }

        void BindDefectNumbering()
        {           
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
            proc.AddVarcharPara("@Action", 50, "BindDefectNumbering");
            DataTable warehouse = proc.GetTable();
            if (warehouse.Rows.Count > 0)
            {
                cmbAutoNumberingforDefect.DataSource = warehouse;
                cmbAutoNumberingforDefect.ValueField = "Id";
                cmbAutoNumberingforDefect.TextField = "SchemaName";
                cmbAutoNumberingforDefect.DataBind();
            }
        }

        void BindReturnTransferNumbering()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
            proc.AddVarcharPara("@Action", 50, "BindReturnTransferNumbering");
            DataTable warehouse = proc.GetTable();
            if (warehouse.Rows.Count > 0)
            {
                cmbReturnTNumbering.DataSource = warehouse;
                cmbReturnTNumbering.ValueField = "Id";
                cmbReturnTNumbering.TextField = "SchemaName";
                cmbReturnTNumbering.DataBind();
            }
        }


        void BindWarehouse()
        {
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
            proc.AddVarcharPara("@Action", 50, "BindWarehouse");
            proc.AddVarcharPara("@Multiwarehouse", 10, multiwarehouse);
            DataTable warehouse = proc.GetTable();
            if (warehouse.Rows.Count > 0)
            {
                cmbSourceWarehouse.DataSource = warehouse;
                cmbSourceWarehouse.ValueField = "WarehouseID";
                cmbSourceWarehouse.TextField = "WarehouseName";
                cmbSourceWarehouse.DataBind();

                cmbDestWarehouse.DataSource = warehouse;
                cmbDestWarehouse.ValueField = "WarehouseID";
                cmbDestWarehouse.TextField = "WarehouseName";
                cmbDestWarehouse.DataBind();

                cmbDefectWarehouse.DataSource = warehouse;
                cmbDefectWarehouse.ValueField = "WarehouseID";
                cmbDefectWarehouse.TextField = "WarehouseName";
                cmbDefectWarehouse.DataBind();
            }
        }

         [WebMethod(EnableSession = true)]
        public static object GetSalesPromotion(string SearchKey)
        {
            List<SalesPromotion> listMainAccount = new List<SalesPromotion>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
                proc.AddVarcharPara("@CompanyID", 10, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                proc.AddVarcharPara("@seacrhkey", 4000, SearchKey);
                proc.AddVarcharPara("@Action", 4000, "GetSalesPromotion");
                DataTable SalesPromotiontbl = proc.GetTable();


                listMainAccount = (from DataRow dr in SalesPromotiontbl.Rows
                                   select new SalesPromotion()
                                   {
                                       MainAccount_AccountCode = dr["MainAccount_AccountCode"].ToString(),
                                       MainAccount_Name = dr["MainAccount_Name"].ToString(),
                                       ShortName = dr["MainAccount_AccountCode"].ToString()
                                       
                                   }).ToList();
            }

            return listMainAccount;
        }

         // Mantis Issue 24818 [OpprSupprDay  added] 
         [WebMethod]
         [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
         public static string addSystemControl(string SalesPromotion, string DeliveryCharges, string BarcodeGeneration, string action, string followDaylmt, string LastDBName, string ActualWarehouseID, string ReplaceableWarehouseID, string SehemaID, string DefectWarehouseID, string DefectSehemaID, string mainAccountPartyjournal, string TurnOver, string RtnTransferNum, string OpprSupprDay)
         {
             try
             {
                 bool i;
                 if(BarcodeGeneration == "1")
                 {
                     i = true;
                 }
                 else
                 {
                     i = false;
                 }
                 //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                 BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                 ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
                 proc.AddVarcharPara("@CompanyID", 10, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                 proc.AddVarcharPara("@SalesPromotion", 50,SalesPromotion);
                 proc.AddVarcharPara("@DeliveryCharges", 50, DeliveryCharges);
                 proc.AddBooleanPara("@BarcodeGeneration",i );
                 proc.AddVarcharPara("@lastdbname", 200,LastDBName);
                 proc.AddVarcharPara("@Action", 4000, action);
                 proc.AddVarcharPara("@ActualWarehouseID", 200, ActualWarehouseID);
                 proc.AddVarcharPara("@ReplaceableWarehouseID", 200, ReplaceableWarehouseID);
                 proc.AddVarcharPara("@SehemaID", 200, SehemaID);
                 proc.AddVarcharPara("@DefectWarehouseID", 200, DefectWarehouseID);
                 proc.AddVarcharPara("@DefectSehemaID", 200, DefectSehemaID);
                 proc.AddVarcharPara("@mainAccountPartyjournal", 200, mainAccountPartyjournal);
                 // Mantis Issue 24818
                 proc.AddVarcharPara("@OpprSupprDay", 200, OpprSupprDay);
                 // End of Mantis Issue 24818
                 proc.AddDecimalPara("@TurnOver", 4, 20, Convert.ToDecimal(TurnOver));
                 proc.AddPara("@nextFollowDayLimit", followDaylmt);
                 proc.AddVarcharPara("@WHSTSehemaID", 200, RtnTransferNum);
                 DataTable dtSystemControl = proc.GetTable();
                 if (dtSystemControl.Rows.Count > 0)
                 {
                     return "1";
                 }
                 else
                 {
                     return "0";
                 }



             }
             catch (Exception ex)
             {
                 return "Error occured";
             }
         }
        
        public void getSystemControl()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc = new ProcedureExecute("PRC_SystemControl");
            proc.AddVarcharPara("@CompanyID", 10, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@Action", 4000, "GetSystemControl");
            DataTable dtSystemControl = proc.GetTable();
            if(dtSystemControl.Rows.Count > 0)
            {
                txtSalesPromotion.Text = dtSystemControl.Rows[0]["SalesPromotionName"].ToString();
                hdnSalesPromotionId.Value = dtSystemControl.Rows[0]["SalesPromotion"].ToString();
                txtDeliveryCharges.Text = dtSystemControl.Rows[0]["DeliveryChargesName"].ToString();
                hdnDeliveryChargesId.Value = dtSystemControl.Rows[0]["DeliveryCharges"].ToString();
                txtFollowupDb.Text = dtSystemControl.Rows[0]["nextFollowDayLimit"].ToString();
                lastdbname.Text = dtSystemControl.Rows[0]["Last_YearDb"].ToString();

                cmbSourceWarehouse.Value = dtSystemControl.Rows[0]["ActualWarehouseID"].ToString();
                cmbDestWarehouse.Value = dtSystemControl.Rows[0]["ReplaceableWarehouseID"].ToString();
                cmbDefectWarehouse.Value = dtSystemControl.Rows[0]["DefectWarehouseID"].ToString();
                cmbAdjustmentNumbering.Value = dtSystemControl.Rows[0]["SchemaID"].ToString();
                cmbAutoNumberingforDefect.Value = dtSystemControl.Rows[0]["DefectSehemaID"].ToString();
                cmbMainAccount.Value = dtSystemControl.Rows[0]["INT_LEDGER_PARTYJOURNAL"].ToString();
                txtTurnover.Value = Convert.ToString(dtSystemControl.Rows[0]["EInvoice_TurnOver"]);
                cmbReturnTNumbering.Value = dtSystemControl.Rows[0]["ReturnTransferSehemaID"].ToString();
                // Mantis Issue 24818
                cmbOpprSupprDay.Value = dtSystemControl.Rows[0]["OpprSupprDay"].ToString();
                // End of Mantis Issue 24818
                if ((bool)(dtSystemControl.Rows[0]["BarcodeGeneration"]) == true)
                {
                    chkBarcodeGen.Checked = true;
                }
                else
                {
                    chkBarcodeGen.Checked = false;
                }
            }
        }
        public class SalesPromotion
        {
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_Name { get; set; }
            public string ShortName { get; set; }


        }

        public class Days
        {
            public Int64 DaysId { get; set; }
            public Int64 Value { get; set; }
        }

        public class BackDatedModuleList
        {
            public Int64 moduleids { get; set; }

            public string modulenames { get; set; }
            public Int64 DaysId { get; set; }
            
        }


        #region Image Compression

        public void Imageresolutionpopulate()
        {
            ProcedureExecute proc = new ProcedureExecute("Insert_ImageforOptimization");
            proc.AddPara("@Action", "Imagecompress");
            DataTable dtimage = proc.GetTable();

            if(dtimage.Rows.Count>0)
            {
                txtwidth.Text=Convert.ToString(dtimage.Rows[0]["width"]);
                txtheight.Text = Convert.ToString(dtimage.Rows[0]["height"]);
            }

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string addSystemProductControl(string width, string height)
        {
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("Insert_ImageforOptimization");
                    proc.AddPara("@width", width);
                    proc.AddPara("@height", height);
                    proc.AddPara("@user_id", HttpContext.Current.Session["userid"]);
                    proc.AddPara("@CompanyID", Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                    DataTable dtimage = proc.GetTable();
                    if (dtimage.Rows.Count > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";

                }


            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }
       #endregion

        #region Print of Copies

        public void PrintOfCopies()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDBEngine.GetDataTable("select No_Of_Copies from tbl_Master_SystemControl ");
            if (dt.Rows.Count > 0)
            { 
                ddlnoofcopies.SelectedValue = Convert.ToString(dt.Rows[0]["No_Of_Copies"]);
            }
            //if (dt.Rows.Count > 0)
            //{
            //    txtwidth.Text = Convert.ToString(dt.Rows[0]["width"]);
            //    txtheight.Text = Convert.ToString(dt.Rows[0]["height"]);
            //}

        }
        public void InfluencerLimit()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDBEngine.GetDataTable("select TOP 1 ISNULL(Influencer_Limit,0) Influencer_Limit,ISNULL(Influencer_Msg_Type,'i') Influencer_Msg_Type from tbl_Master_SystemControl");
            if (dt.Rows.Count > 0)
            {
                DropDownList1.SelectedValue = Convert.ToString(dt.Rows[0]["Influencer_Msg_Type"]);
                txtInfLimt.Text = Convert.ToString(dt.Rows[0]["Influencer_Limit"]);
            }
            //if (dt.Rows.Count > 0)
            //{
            //    txtwidth.Text = Convert.ToString(dt.Rows[0]["width"]);
            //    txtheight.Text = Convert.ToString(dt.Rows[0]["height"]);
            //}

        }

        


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string updateSystemControlCopies(int No_Of_Copies)
        {
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt = oDBEngine.GetDataTable("update tbl_Master_SystemControl set No_Of_Copies='" + No_Of_Copies + "'");
                    if (dt.Rows.Count > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";

                }


            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string updateSystemControlInfluencer(string amount,string type)
        {
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt = oDBEngine.GetDataTable("update tbl_Master_SystemControl set Influencer_Limit='" + amount + "',Influencer_Msg_Type='" + type + "'");
                    if (dt.Rows.Count > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";

                }


            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }
        #endregion


       #region Auto Invoice - Holiday
        public void HolidayBind()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDBEngine.GetDataTable("select HOLIDAYID,HOLIDAY_DESC from ERP_HOLIDAYMAIN");

            DataTable dt2 = oDBEngine.GetDataTable("select ISNULL(Holiday_id,0)Holiday_id from  tbl_Master_SystemControl");

            if (dt.Rows.Count > 0)
            {
                ddlHoliday.DataSource = dt;
                ddlHoliday.DataTextField = "HOLIDAY_DESC";
                ddlHoliday.DataValueField = "HOLIDAYID";
                ddlHoliday.DataBind();
                if (dt2!=null && dt2.Rows.Count>0)
                {
                    ddlHoliday.SelectedValue = dt2.Rows[0]["Holiday_id"].ToString();
                }
                else
                {
                    ddlHoliday.SelectedValue = "0";
                }               
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string updateSystemControlHoliday(string holiday_id)
        {
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt = oDBEngine.GetDataTable("update tbl_Master_SystemControl set Holiday_id='" + holiday_id + "'");
                    if (dt.Rows.Count > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }
       #endregion
    }
}