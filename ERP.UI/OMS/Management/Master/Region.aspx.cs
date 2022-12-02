using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using System.IO;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_Region : ERP.OMS.ViewState_class.VSPage
    {

        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        string[] lengthIndex;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //((GridViewDataComboBoxColumn)grid.Columns["CountryID"]).PropertiesComboBox.DataSource = GetAllMainAccount();
            //((GridViewDataComboBoxColumn)grid.Columns["CityID"]).PropertiesComboBox.DataSource = GetSubAccount("", Convert.ToString(Session["userbranchID"]), "ALL");

            
            SelectRegion.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCountry.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            insertupdatedelete.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }


        protected void Page_Load(object sender, EventArgs e)
        {


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Region.aspx");


            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                   // insertupdatedelete.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];

                }
                else
                {
                   // insertupdatedelete.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];

                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");



        }
       
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
             Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
             if (Filter != 0)
             {
                 if (Session["exportval"] == null)
                 {
                     Session["exportval"] = Filter;
                     bindexport(Filter);
                 }
                 else if (Convert.ToInt32(Session["exportval"]) != Filter)
                 {
                     Session["exportval"] = Filter;
                     bindexport(Filter);
                 }
             }
        }
        public void bindexport(int Filter)
        {
            RegionGrid.Columns[3].Visible = false;

            string filename = "Regions";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Regions";
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
        
        protected void btnSearch(object sender, EventArgs e)
        {
            RegionGrid.Settings.ShowFilterRow = true;
        }
        protected void RegionGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.Data)
            //{
            //    int commandColumnIndex = -1;
            //    for (int i = 0; i < RegionGrid.Columns.Count; i++)
            //        if (RegionGrid.Columns[i] is GridViewCommandColumn)
            //        {
            //            commandColumnIndex = i;
            //            break;
            //        }
            //    if (commandColumnIndex == -1)
            //        return;
            //    //____One colum has been hided so index of command column will be leass by 1 
            //    commandColumnIndex = commandColumnIndex - 2;
            //    DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
            //    for (int i = 0; i < cell.Controls.Count; i++)
            //    {
            //        DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
            //        if (button == null) return;
            //        DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

            //        if (hyperlink.Text == "Delete")
            //        {
            //            if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
            //            {
            //                hyperlink.Enabled = true;
            //                continue;
            //            }
            //            else
            //            {
            //                hyperlink.Enabled = false;
            //                continue;
            //            }
            //        }


            //    }

            //}

        }
        protected void RegionGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!RegionGrid.IsNewRowEditing)
            {

                ASPxGridViewTemplateReplacement RT = RegionGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void RegionGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //if (e.Parameters == "s")
            //    RegionGrid.Settings.ShowFilterRow = true;
            string[] CallVal = Convert.ToString(e.Parameters).Split('~');
                lengthIndex = e.Parameters.Split('~');
                if (Convert.ToString(lengthIndex[0]) == "Delete")
                {
                    string PinId = Convert.ToString(Convert.ToString(CallVal[1]));
                    int retValue = masterChecking.DeleteMasterregion(Convert.ToInt32(PinId));
                    if (retValue > 0)
                    {
                        Session["KeyVal"] = "Succesfully Deleted";
                        RegionGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                        RegionGrid.DataBind();
                    }
                    else
                    {
                        Session["KeyVal"] = "Used in other modules. Cannot Delete.";
                        RegionGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";

                    }
                
                }
            if (e.Parameters == "All")
            {
                RegionGrid.FilterExpression = string.Empty;
            }
        }

        protected void RegionGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            //if (!rights.CanDelete)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Delete)
            //    {
            //        e.Visible = false;
            //    }
            //}


            //if (!rights.CanEdit)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Edit)
            //    {
            //        e.Visible = false;
            //    }
            //}
        }
    }
}