using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using System.IO;
using EntityLayer.CommonELS;
using BusinessLogicLayer;
using DataAccessLayer;
using System.Web.Services;
using System.Collections.Generic;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_RootBuilding : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL ComBL = new CommonBL();
        // Mantis Issue 24996
        public bool UserSelInWHMast { get; set; }
        // End of Mantis Issue 24996
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
            
          rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/RootBuilding.aspx");

          // Mantis Issue 24996
          CommonBL config = new CommonBL();
          string mastersettings = config.GetSystemSettingsResult("Allow_UserSelInWHMast");

          if (mastersettings == "No")
          {
              UserSelInWHMast = false;
          }
          else
          {
              UserSelInWHMast = true;
          }
          // End of Mantis Issue 24996

          Session["requesttype"] = "Building/Warehouses";
          Session["ContactType"] = "Building/Warehouses";
            Session["KeyVal_InternalID"] = "";
            Session["Name"] = "";
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //RootSource.SelectCommand = "select c.cnt_firstName + ' ' + c.cnt_middleName + ' ' + c.cnt_lastName AS CareTaker, b.bui_id AS Id, b.bui_address1 + ' , ' + b.bui_address2 + ' , ' + b.bui_address3 + ' ' + b.bui_landmark AS Address, b.bui_name as Building,'Document' AS document from tbl_master_building b,tbl_master_contact c where b.bui_contactId = c.cnt_internalId order by c.cnt_firstName";
            bindgrid();

            string WarehouseCodeRequireWHMaster = ComBL.GetSystemSettingsResult("WarehouseCodeRequireWHMaster");
            if (!String.IsNullOrEmpty(WarehouseCodeRequireWHMaster))
            {
                if (WarehouseCodeRequireWHMaster.ToUpper().Trim() == "YES")
                {
                    RootGrid.Columns[0].Visible = true;
                }
                else if (WarehouseCodeRequireWHMaster.ToUpper().Trim() == "NO")
                {
                    RootGrid.Columns[0].Visible = false;
                }
            }    

            if (!IsPostBack)
            {
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                if (multiwarehouse != "1")
                {
                    RootGrid.Columns[4].Visible = false;
                    RootGrid.Columns[3].Visible = false;
                }
            }
        }
        void bindgrid()
        {
            //DataTable dtbind = oDBEngine.GetDataTable("select c.cnt_firstName + ' ' + c.cnt_middleName + ' ' + c.cnt_lastName AS CareTaker, b.bui_id AS Id, b.bui_address1 + ' , ' + b.bui_address2 + ' , ' + b.bui_address3 + ' ' + b.bui_landmark AS Address, b.bui_name as Building,'Document' AS document from tbl_master_building b,tbl_master_contact c where b.bui_contactId = c.cnt_internalId order by c.cnt_firstName");
            //04-11-2016----changes for when address is coming blank
            //DataTable dtbind = oDBEngine.GetDataTable("select c.cnt_firstName + ' ' + c.cnt_middleName + ' ' + c.cnt_lastName AS CareTaker, b.bui_id AS Id, case when b.bui_address1 ='' then b.bui_address1  else b.bui_address1 +',' end  + case when b.bui_address2 ='' then b.bui_address2  else b.bui_address2 +',' end  +  b.bui_address3 + ' ' + b.bui_landmark AS Address, b.bui_name as Building,'Document' AS document from tbl_master_building b,tbl_master_contact c where b.bui_contactId = c.cnt_internalId order by c.cnt_firstName");

            DataTable dtbind = oDBEngine.GetDataTable("select c.cnt_firstName + ' ' + c.cnt_middleName + ' ' + c.cnt_lastName AS CareTaker, b.bui_id AS Id, case when b.bui_address1 =''  then b.bui_address1  else b.bui_address1 +',' end  +   case when b.bui_address2 ='' then b.bui_address2  else b.bui_address2 +',' end    +  b.bui_address3 + ' ' + b.bui_landmark AS Address, b.bui_name as Building,   'Document' AS document ,ISNULL(d.bui_WarehouseName,'')    ParentWarehouse,e.Level_Name Level ,isnull(bui_code,'')bui_code from tbl_master_building b    left join tbl_master_contact c on b.bui_contactId =    c.cnt_internalId left join V_MultiWareHouse d    on d.bui_WarehouseID=b.bui_ParentId left join Master_WarehouseLayout e    on e.Level_id=b.bui_LevelId order by c.cnt_firstName");


            RootGrid.DataSource = dtbind.DefaultView;
            RootGrid.DataBind();

        }
        protected void btnSearch(object sender, EventArgs e)
        {
            RootGrid.Settings.ShowFilterRow = true;
        }
        protected void RootGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string ContactID = e.GetValue("Id").ToString();

            //e.Row.Cells[5].Style.Add("cursor", "hand");
            // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
            //e.Row.Cells[4].Attributes.Add("onclick", "javascript:showhistory('" + ContactID + "');");
            //e.Row.Cells[4].ToolTip = "ADD/VIEW";
            //e.Row.Cells[4].Style.Add("color", "Blue");
        }
        protected void RootGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                RootGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                RootGrid.FilterExpression = string.Empty;
                bindgrid();
            }
        }
        protected void RootGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //string ID = e.Keys[0].ToString();
            //oDBEngine.DeleteValue("tbl_master_building", "bui_id='" + ID + "'");

            //e.Cancel = true;
            //bindgrid();
            //RootGrid.CancelEdit();

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bindUserGroups();
            RootGrid.Columns[4].Visible = false;
            //Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    //exporter.WritePdfToResponse();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        exporter.WritePdf(stream);
                        WriteToResponse("ExportEmployee", true, "pdf", stream);
                    }
                    //Page.Response.End();
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
            RootGrid.Columns[4].Visible = true;

        }
        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
            //Page.Response.End();
        }
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;
        //    }
        //}

        protected void RootGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            string grp_id = Convert.ToString(e.KeyValue);
            string commanName = e.CommandArgs.CommandName;
            if (commanName == "delete")
            {
                string ID = grp_id;
               int i= oDBEngine.DeleteValue("tbl_master_building", "bui_id='" + ID + "'");
               int j = oDBEngine.DeleteValue("Master_Warehouse_Branchmap", "Bui_id='" + ID + "'");
                if(i==0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Used in other modules. Can not Delete.');", true);
                    return;
                }

                bindgrid();
                RootGrid.CancelEdit();
            }
        }

        // Mantis Issue 24996
        [WebMethod]
        public static List<UserList> GetUserList(string warehouseId)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            List<UserList> omodel = new List<UserList>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_User_WarehouseMAP");
            proc.AddPara("@ACTION", "GetUserList");
            proc.AddPara("@WarehouseID", warehouseId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                omodel = UtilityLayer.APIHelperMethods.ToModelList<UserList>(dt);
            }
            return omodel;
        }

        public class UserList
        {
            public String user_id { get; set; }
            public String user_name { get; set; }
            public bool IsChecked { get; set; }
           
        }

        [WebMethod]
        public static bool UserListSubmit(string WarehouseID, List<string> Userlist)
        {
            //Employee_BL objEmploye = new Employee_BL();
            string UserlistId = "";
            int i = 1;

            if (Userlist != null && Userlist.Count > 0)
            {
                foreach (string item in Userlist)
                {
                    if (item == "0")
                    {
                        UserlistId = "0";
                        break;
                    }
                    else
                    {
                        if (i > 1)
                            UserlistId = UserlistId + "," + item;
                        else
                            UserlistId = item;
                        i++;
                    }
                }

            }

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_User_WarehouseMAP");
             proc.AddPara("@ACTION", "SaveUserWarehouseMaping");
            proc.AddPara("@UserlistId", UserlistId);
            proc.AddPara("@WarehouseID", WarehouseID);
            proc.AddPara("@User_id", Convert.ToString(HttpContext.Current.Session["userid"]));
            dt = proc.GetTable();
            
            return true;
        }
        // End of Mantis Issue 24996
    }
}