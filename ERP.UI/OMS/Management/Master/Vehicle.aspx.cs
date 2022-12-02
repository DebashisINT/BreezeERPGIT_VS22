using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Text;
using System.Collections.Generic;
using System.Resources;
using System.Collections;
using System.IO;
using EntityLayer.CommonELS;
using System.Drawing;
using System.Linq;
using System.Configuration;
using DataAccessLayer;


namespace ERP.OMS.Management.Master
{
    public partial class Vehicle : ERP.OMS.ViewState_class.VSPage
    {

        public string pageAccess = "";
        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        public ProductComponentBL prodComp = new ProductComponentBL();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Vehicle.aspx");
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {

            }
            if (!IsPostBack)
            {
                Session["exportval"] = null;


                string RequestType = Convert.ToString(Session["requesttype"]);

            }
            BindGrid();

            if (Session["BranchListTableForVehicle"] != null)
            {
                Session.Remove("BranchListTableForVehicle");
            }
        }


        protected void BindGrid()
        {
            VehicleBL vehObj = new VehicleBL();
            DataTable dtFillGrid = new DataTable();
            string vehicleLstOrderBy = "ADD";
            if (HttpContext.Current.Session["VehicleListOrderBy"] != "" & HttpContext.Current.Session["VehicleListOrderBy"] != null)
            {
                vehicleLstOrderBy = Convert.ToString(HttpContext.Current.Session["VehicleListOrderBy"]);

                if (vehicleLstOrderBy.Trim().ToUpper() == "ADD") //General Ordering
                {
                    dtFillGrid = vehObj.GetsVehicleList(vehicleLstOrderBy);
                }
                if (vehicleLstOrderBy.Trim().ToUpper() == "EDIT") //Ordering by Last updated Date
                {
                    dtFillGrid = vehObj.GetsVehicleList(vehicleLstOrderBy);
                }
            }
            else //This is General Ordering
            {
                dtFillGrid = vehObj.GetsVehicleList(vehicleLstOrderBy);
            }


            AspxHelper oAspxHelper = new AspxHelper();

            cityGrid.DataSource = dtFillGrid;
            cityGrid.DataBind();

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
                        if (Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd" || Convert.ToString(Session["PageAccess"]).Trim() == "Delete" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
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
            //if (!cityGrid.IsNewRowEditing)
            //{
            //    ASPxGridViewTemplateReplacement RT = cityGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
            //    if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
            //        RT.Visible = true;
            //    else
            //        RT.Visible = false;
            //}

        }

        protected void cityGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;

            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

            if (e.Parameters == "s")
                cityGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
                cityGrid.FilterExpression = string.Empty;

            if (WhichCall == "savecity")
            {

            }
            if (WhichCall == "updatecity")
            {

            }
            if (WhichCall == "Delete")
            {
                int checkinvalue = masterChecking.DeleteVehicle(WhichType);
                if (checkinvalue > 0)
                {
                    cityGrid.JSProperties["cpDelete"] = "Success";
                    BindGrid();
                }
                else if (checkinvalue == -10)
                {
                    cityGrid.JSProperties["cpDelete"] = "refrenceExist";
                    BindGrid();
                }
                else
                {
                    cityGrid.JSProperties["cpDelete"] = "Fail";

                    //if (checkinvalue == -2)
                    //{
                    //    cityGrid.JSProperties["cpDelete"] = "Fail";
                    //    cityGrid.JSProperties["cpErrormsg"] = "Transaction exists. Cannot delete.";
                    //}
                    //else if (checkinvalue == -3)
                    //{
                    //    string UsedProductList = "";
                    //    DataTable dt = oDBEngine.GetDataTable("select (select sproducts_name from Master_sProducts where sProducts_ID=h.Product_id) as Product_idName,Product_id, (select sproducts_name from Master_sProducts where sProducts_ID=h.Component_prodId) as Component_prodIdName,Component_prodId from tbl_master_ProdComponent h  where h.Component_prodId=" + WhichType + " or Product_id=" + WhichType + "");
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow dr in dt.Rows)
                    //        {
                    //            if (Convert.ToString(dr["Component_prodId"]).Trim() == WhichType)
                    //            {
                    //                UsedProductList += ", " + Convert.ToString(dr["Product_idName"]);
                    //            }
                    //            else
                    //            {
                    //                UsedProductList += ", " + Convert.ToString(dr["Component_prodIdName"]);
                    //            }
                    //        }
                    //        cityGrid.JSProperties["cpDelete"] = "Fail";
                    //        cityGrid.JSProperties["cpErrormsg"] = "This Product associated with " + UsedProductList.TrimStart(',') + ". \n Cannot delete.";
                    //    }
                    //}
                    //else
                    //{
                    //    cityGrid.JSProperties["cpDelete"] = "Fail";
                    //    cityGrid.JSProperties["cpErrormsg"] = "Product is in use. Cannot delete.";
                    //}
                }
            }
            if (WhichCall == "Edit")
            {

            }
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

        public void bindexport(int Filter)
        {
            //cityGrid.Columns[7].Visible = false;
            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = "Vehicle";//Convert.ToString((Session["Contactrequesttype"] ?? "Lead")); //need to check this logic implementation
            exporter.FileName = filename;

            exporter.PageHeader.Left = filename; //Convert.ToString((Session["Contactrequesttype"] ?? "Lead"));
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

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
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                //if (Session["exportVehicle"] == null)
                //{
                //    Session["exportVehicle"] = Filter;
                    bindexport(Filter);
                //}
                //else if (Convert.ToInt32(Session["exportVehicle"]) != Filter)
                //{
                //    Session["exportVehicle"] = Filter;
                //    bindexport(Filter);
                //}
            }
        }

    }
}