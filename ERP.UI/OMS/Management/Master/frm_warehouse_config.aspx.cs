using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EntityLayer.CommonELS;
using System.IO;
using DevExpress.Web;
using BusinessLogicLayer;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{
    public partial class frm_warehouse_config : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);multi
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        MasterDbEngine modb = new MasterDbEngine();
        BusinessLogicLayer.MasterDataCheckingBL delMasterData = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        WarehouseConfigMasterBL wcmbl = new WarehouseConfigMasterBL();
        DataTable dt = new DataTable();

        public int ShowBatch { get; set; }
        public int ShowSerial { get; set; }
        public int ShowBarcode { get; set; }


        bool warehousechange = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            gridWarehouseConfigDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlwarehouse.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_warehouse_config.aspx");
            
            //chinmoy added below line 17-07-2019
            //start
            //MasterSettings masterBl = new MasterSettings();
            //string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
            //string Query = "select -1 as bui_id,'none' as bui_name union all select bui_id,bui_name from tbl_master_building";
            //DataTable ware = new DataTable();
            //if (multiwarehouse == "1")
            //{
            //    ware = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '0','0'");
            //    Query = "select -1 as bui_id,'none' as bui_name union all select bui_id,bui_name from MultiWareHouse";
            //}

            //sqlwarehouse.SelectCommand = Query;
            //End

            if (!IsPostBack)
            {
                //chinmoy commented below line 17-07-2019
                //start
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                string Query = "select -1 as bui_id,'none' as bui_name union all select bui_id,bui_name from tbl_master_building";
                DataTable ware = new DataTable();
                if (multiwarehouse == "1")
                {
                    ware = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '0','0'");
                    Query = "select -1 as bui_id,'none' as bui_name union all select bui_id,bui_name from MultiWareHouse";
                }

                sqlwarehouse.SelectCommand = Query;
                //end
                dt = oDBEngine.GetDataTable("select Is_active_warehouse,Is_active_serialno from Master_sProducts where sProduct_IsInventory=1");
                
                if (dt.Rows.Count > 0)
                {
                    checkIsactive.Visible = true;
                    Button1.Visible = true;
                    Button2.Visible = true;

                    if(Convert.ToBoolean(dt.Rows[0][0].ToString())==true)
                    {
                        checkIsactive.Checked = true;
                        ViewState["chkwarehouse"] = checkIsactive.Checked;
                    }
                   
                }
                else
                {
                    checkIsactive.Visible = false;
                    Button1.Visible = false;
                    Button2.Visible = false;
                }
            }
           
        }

        public void ShowHideColumn()
        {
            MasterSettings masterBl = new MasterSettings();
            ShowBatch = Convert.ToInt32(masterBl.GetSettings("IsShowBatch"));
            ShowSerial = Convert.ToInt32(masterBl.GetSettings("IsShowSerial"));
            ShowBarcode = Convert.ToInt32(masterBl.GetSettings("IsShowbarcode"));

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

        protected void gridWarehouseConfig_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

        }

        protected void gridWarehouseConfig_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            ShowHideColumn();
            if (e.Column.FieldName == "Is_active_Batch")
            {
                if (ShowBatch==1)
                {
                    e.Column.Visible = true;
                }
                else
                {
                    e.Column.Visible = false;

                }
            }
            else if (e.Column.FieldName == "Is_active_serialno")
            {
                if (ShowSerial==1)
                {
                    e.Column.Visible = true;
                }
                else
                {
                    e.Column.Visible = false;

                }
            }
            else if (e.Column.FieldName == "Is_BarCode_Active")
            {
                if (ShowBarcode==1)
                {
                    e.Column.Visible = true;
                }
                else
                {
                    e.Column.Visible = false;

                }
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
           
                int Id = wcmbl.UpdateProductWarehouseConfig(hdnprodrowid.Value, hdnwarehouseid.Value, Convert.ToBoolean(checkIsactive.Checked), hdnActiveBranch.Value, hdnSerialNo.Value, hdnStockBlock.Value,hdnBarCode.Value);
                if (Id > 0)
                {
                    string popUpscript1 = "";
                    popUpscript1 = "jAlert('Record Updated Successfully');";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);

                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "<script>SavedSuccessfully('" + popUpscript1 + "');</script> ", true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "SavedSuccessfully('Record Updated Successfully'); ", true);
                    RefereshApplicationProductData();
                }
                //Response.Redirect("frm_warehouse_config.aspx");
           
           
        }
        public void RefereshApplicationProductData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_POS_PRODUCTLIST");
            dt = proc.GetTable();
            Application.Remove("POSPRODUCTLISTDATA");
            Application.Add("POSPRODUCTLISTDATA", dt);
        }

        public bool GetChecked(string Access)
        {
            switch (Access)
            {
                case "True":
                    return true;
              
                default:
                    return false;
            }
           
        }
        //public bool GetAllChecked(string Access)
        //{

        //    bool check;
        //    DataTable dtbatch = oDBEngine.GetDataTable("select count(*) from  (select Is_active_serialno from Master_sProducts where sProduct_IsInventory=1) as tab where tab.Is_active_serialno<>1");
        //    if (dtbatch.Rows.Count>1)
        //    {
        //        check = false;
        //    }
        //    else
        //    {
        //        check = true;
        //    }
        //    return check;
        //}
        
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] param = e.Parameters.Split('^');

           
                if (param[1] == "batch")
                {
                    //int prodid, string is_type,bool is_checked
                    //frm_warehouse_config.GetTransactionExistscheck
                    hdnActiveBranch.Value = "";
                    ASPxGridView grid = sender as ASPxGridView;
                    var state = Convert.ToBoolean(param[0]);

                    //for (int i = 0; i < grid.VisibleRowCount; i++)
                    //{
                    //    grid.Selection.SelectRow(i);
                    //}
                    var headerCb = grid.FindHeaderTemplateControl(grid.Columns[3], "SelectAllCheckBox") as ASPxCheckBox;
                    headerCb.Checked = state;
                    hdnstoreheaderbatch.Value = Convert.ToString(headerCb.Checked);



                    //for (int i = 0; i < gridWarehouseConfig.VisibleRowCount; i++)
                    //{
                    for (int i = 0; i < gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id").Count; i++)
                    {
                        //object oCommId = gridWarehouseConfig.GetRowValues(i, new string[] { "sproducts_id" });
                        object oCommId = gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id")[i];
                        string msg= frm_warehouse_config.GetTransactionExistscheck(Convert.ToInt32(oCommId), "batch", state);
                       if (msg != "Exists")
                       {
                           //ASPxCheckBox chk = gridWarehouseConfig.FindRowCellTemplateControl(i, gridWarehouseConfig.DataColumns["Is_active_Batch"], "chkActivateBatch") as ASPxCheckBox;
                           ASPxCheckBox chk = gridWarehouseConfig.FindRowCellTemplateControlByKey(oCommId, gridWarehouseConfig.DataColumns["Is_active_Batch"], "chkActivateBatch") as ASPxCheckBox;
                           chk.Checked = state;
                           if (hdnActiveBranch.Value == "")
                           {
                               hdnActiveBranch.Value = state + "~" + oCommId;

                           }
                           else
                           {
                               hdnActiveBranch.Value = hdnActiveBranch.Value + "," + state + "~" + oCommId;
                           }
                           grid.JSProperties["cpBatch"] = hdnActiveBranch.Value;
                       }

                        



                    }
                }
                else if (param[1] == "serial")
                {
                    hdnSerialNo.Value = "";
                    ASPxGridView grid = sender as ASPxGridView;
                    var state = Convert.ToBoolean(param[0]);

                    //for (int i = 0; i < grid.VisibleRowCount; i++)
                    //{
                    //    grid.Selection.SelectRow(i);
                    //}
                    var headerCb = grid.FindHeaderTemplateControl(grid.Columns[4], "SelectAllSerial") as ASPxCheckBox;
                    headerCb.Checked = state;
                    hdnstoreheaderbatch.Value = Convert.ToString(headerCb.Checked);
                    //for (int i = 0; i < gridWarehouseConfig.VisibleRowCount; i++)
                    //{
                    for (int i = 0; i < gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id").Count; i++)
                    {
                        //object oCommId = gridWarehouseConfig.GetRowValues(i, new string[] { "sproducts_id" });
                        object oCommId = gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id")[i];
                        string msg= frm_warehouse_config.GetTransactionExistscheck(Convert.ToInt32(oCommId), "serial", state);
                         if (msg != "Exists")
                         {
                       
                        DataTable dtCheck;
                        dtCheck = oDBEngine.GetDataTable("select * from Master_sProducts where sProduct_StockUOM=sProducts_TradingLotUnit and sProducts_TradingLotUnit=sProducts_QuoteLotUnit and sProducts_ID='" + oCommId + "'");
                        if (dtCheck.Rows.Count > 0)
                        {
                            ASPxCheckBox chk = gridWarehouseConfig.FindRowCellTemplateControlByKey(oCommId, gridWarehouseConfig.DataColumns["Is_active_serialno"], "chkActivateSerialno") as ASPxCheckBox;
                            chk.Checked = state;
                            //hdnActiveBranch


                            if (hdnSerialNo.Value == "")
                            {
                                hdnSerialNo.Value = state + "~" + oCommId;

                            }
                            else
                            {
                                hdnSerialNo.Value = hdnSerialNo.Value + "," + state + "~" + oCommId;
                            }
                            grid.JSProperties["cpSerial"] = hdnSerialNo.Value;
                        }
                        else
                        {
                            ASPxCheckBox chk = gridWarehouseConfig.FindRowCellTemplateControlByKey(oCommId, gridWarehouseConfig.DataColumns["Is_active_serialno"], "chkActivateSerialno") as ASPxCheckBox;
                            chk.Checked = false;
                        }
                      }




                    }
                }
                else if (param[1] == "stock")
                {
                    hdnActiveBranch.Value = "";
                    ASPxGridView grid = sender as ASPxGridView;
                    var state = Convert.ToBoolean(param[0]);

                    //for (int i = 0; i < grid.VisibleRowCount; i++)
                    //{
                    //    grid.Selection.SelectRow(i);
                    //}
                    var headerCb = grid.FindHeaderTemplateControl(grid.Columns[5], "SelectAllstock") as ASPxCheckBox;
                    headerCb.Checked = state;
                    hdnstoreheaderbatch.Value = Convert.ToString(headerCb.Checked);
                    //for (int i = 0; i < gridWarehouseConfig.VisibleRowCount; i++)
                    //{
                    for (int i = 0; i < gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id").Count; i++)
                    {
                       
                        object oCommId = gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id")[i];
                        //string msg = frm_warehouse_config.GetTransactionExistscheck(Convert.ToInt32(oCommId), "stockblock", state);
                        //if (msg != "Exists")
                        //{
                        ASPxCheckBox chk = gridWarehouseConfig.FindRowCellTemplateControlByKey(oCommId, gridWarehouseConfig.DataColumns["Is_stock_Block"], "chkStkBlck") as ASPxCheckBox;
                            chk.Checked = state;

                        if (hdnStockBlock.Value == "")
                        {
                            hdnStockBlock.Value = state + "~" + oCommId;

                        }
                        else
                        {
                            hdnStockBlock.Value = hdnStockBlock.Value + "," + state + "~" + oCommId;
                        }
                        grid.JSProperties["cpStock"] = hdnStockBlock.Value;

                        //}

                    }
                }
                else if (param[1] == "warehouse")
                {
                     bool checkstate = checkIsactive.Checked; 
                     int itemIndex = Convert.ToInt32(param[0]);
                     //for (int i = 0; i < gridWarehouseConfig.VisibleRowCount; i++)
                     //{
                     for (int i = 0; i < gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id").Count; i++)
                     {
                       
                         //object oCommId = gridWarehouseConfig.GetRowValues(i, new string[] { "sproducts_id" });
                         object oCommId = gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id")[i];
                         string msg = frm_warehouse_config.GetTransactionExistscheck(Convert.ToInt32(oCommId), "warehouse", checkstate);
                         if (msg != "Exists")
                         {
                             checkIsactive.Checked = Convert.ToBoolean(ViewState["chkwarehouse"]);
                         }
                     }
                }

                else if (param[1] == "BarCode")
                {
                    hdnBarCode.Value = "";
                    ASPxGridView grid = sender as ASPxGridView;
                    var state = Convert.ToBoolean(param[0]);
                    var headerCb = grid.FindHeaderTemplateControl(grid.Columns[6], "SelectAllBarCode") as ASPxCheckBox;
                    headerCb.Checked = state;
                    hdnstoreheaderbatch.Value = Convert.ToString(headerCb.Checked);
                    
                    for (int i = 0; i < gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id").Count; i++)
                    {
                      
                        object oCommId = gridWarehouseConfig.GetCurrentPageRowValues("sproducts_id")[i];
                        //string msg = frm_warehouse_config.GetTransactionExistscheck(Convert.ToInt32(oCommId), "BarCode", state);
                        //if (msg != "Exists")
                        //{

                            //DataTable dtCheck;
                            //dtCheck = oDBEngine.GetDataTable("select * from Master_sProducts where sProduct_StockUOM=sProducts_TradingLotUnit and sProducts_TradingLotUnit=sProducts_QuoteLotUnit and sProducts_ID='" + oCommId + "'");
                            //if (dtCheck.Rows.Count > 0)
                            //{
                                ASPxCheckBox chk = gridWarehouseConfig.FindRowCellTemplateControlByKey(oCommId, gridWarehouseConfig.DataColumns["Is_BarCode_Active"], "chkActivateBarCode") as ASPxCheckBox;
                                chk.Checked = state;



                                if (hdnBarCode.Value == "")
                                {
                                    hdnBarCode.Value = state + "~" + oCommId;

                                }
                                else
                                {
                                    hdnBarCode.Value = hdnBarCode.Value + "," + state + "~" + oCommId;
                                }
                                grid.JSProperties["cpBarCode"] = hdnBarCode.Value;
                            //}
                            //else
                            //{
                            //    ASPxCheckBox chk = gridWarehouseConfig.FindRowCellTemplateControlByKey(oCommId, gridWarehouseConfig.DataColumns["Is_BarCode_Active"], "chkActivateBarCode") as ASPxCheckBox;
                            //    chk.Checked = false;
                            //}
                       // }

                    }
                }
          
        
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //gridWarehouseConfigDataSource.SelectCommand = "select sproducts_id,sProducts_Name,sProducts_Code,warehouse_id,warehouse_name=(select bui_name from tbl_master_building where bui_id=warehouse_id) from Master_sProducts where sProduct_IsInventory=1";
            gridWarehouseConfigDataSource.SelectCommand = " select sproducts_id,sProducts_Name,sProducts_Code,warehouse_id,warehouse_name=(select bui_name from tbl_master_building where bui_id=warehouse_id),Is_active_Batch,Is_active_serialno,Is_stock_Block,Is_BarCode_Active from Master_sProducts where sProduct_IsInventory=1";
            dt = oDBEngine.GetDataTable("select Is_active_warehouse from Master_sProducts where sProduct_IsInventory=1 and Is_active_warehouse<>0");
            if (dt.Rows.Count > 1)
            {
                checkIsactive.Checked = true;
                //checkIsActivateSerialno.Checked = true;
            }
            else
            {
                checkIsactive.Checked = false;
                //checkIsActivateSerialno.Checked = false;
            }
        }
        protected void cb_Init(object sender, EventArgs e)
        {
            ASPxComboBox DBAcombobox = (ASPxComboBox)sender;
            int itemindex = (((ASPxComboBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            DBAcombobox.ClientSideEvents.SelectedIndexChanged = String.Format("function(s, e) {{ OnSelectedIndexChanged(s, e, {0}) }}", itemindex);


        }
        protected void chkActivateBatch_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetCheckActivateBatch(s, e, {0}) }}", itemindex);

        }

        protected void chkActivateSerialno_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetCheckSerialNo(s, e, {0}) }}", itemindex);
        }
        protected void chkActivateBarCode_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetCheckBarCode(s, e, {0}) }}", itemindex);
        }
        protected void chkStkBlck_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetCheckStockBlock(s, e, {0}) }}", itemindex);
        }

        [System.Web.Services.WebMethod]
        public static string GetUOMcheck(string prodid)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string msg;
            DataTable dtCheck;
            dtCheck = oDBEngine.GetDataTable("select * from Master_sProducts where sProduct_StockUOM=sProducts_TradingLotUnit and sProducts_TradingLotUnit=sProducts_DeliveryLotUnit and sProducts_ID='" + prodid + "'");
            if (dtCheck.Rows.Count>0)
            {
                msg = "1";
            }
            else
            {
                msg = "0";
            }
            //return "Hello " + name + Environment.NewLine + "The Current Time is: "
            //    + DateTime.Now.ToString();
            return msg;
        }

        [System.Web.Services.WebMethod]
        public static string GetTransactionExistscheck(int prodid, string is_type,bool is_checked)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            ProcedureExecute proc;


            string msg="";
            try
            {
                using (proc = new ProcedureExecute("sp_chkTranExtAgstInvConfig"))
                {
                    proc.AddIntegerPara("@p_prodid", prodid);
                    proc.AddVarcharPara("@p_is_type", 20, is_type);
                    proc.AddVarcharPara("@p_Return_msg", 50, "", QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    msg = Convert.ToString(proc.GetParaValue("@p_Return_msg"));
                }
            }
            catch (Exception ex)
            {
                //retval = false;
            }
            finally
            {
                proc = null;
            }
            return msg;







            //string msg;
            //DataTable dtCheck;
            //dtCheck = oDBEngine.GetDataTable("select * from Master_sProducts where sProduct_StockUOM=sProducts_TradingLotUnit and sProducts_TradingLotUnit=sProducts_DeliveryLotUnit and sProducts_ID='" + prodid + "'");
            //if (dtCheck.Rows.Count > 0)
            //{
            //    msg = "1";
            //}
            //else
            //{
            //    msg = "0";
            //}
         
            //return msg;
        }
        protected void checkIsactive_CheckedChanged(object sender, EventArgs e)
        {
            warehousechange = true;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Check('Record Updated Successfully'); ", true);

        }

        protected void gridWarehouseConfig_PageIndexChanged(object sender, EventArgs e)
        {
            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
            string Query = "select -1 as bui_id,'none' as bui_name union all select bui_id,bui_name from tbl_master_building";
            DataTable ware = new DataTable();
            if (multiwarehouse == "1")
            {
                ware = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '0','0'");
                Query = "select -1 as bui_id,'none' as bui_name union all select bui_id,bui_name from MultiWareHouse";
            }

            sqlwarehouse.SelectCommand = Query;
        }

    }
}