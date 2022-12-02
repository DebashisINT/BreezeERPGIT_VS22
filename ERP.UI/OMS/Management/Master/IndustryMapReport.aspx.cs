using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Data;
using DevExpress.Web;
using System.Data;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.Master
{
    public partial class IndustryMapReport : ERP.OMS.ViewState_class.VSPage //SSystem.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Master/IndustryMapReport.aspx");
            
            if (!IsPostBack)
            {
                //IndustryDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                EntityDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                BindIndustryMap();
                //GridIndustryMap.DataBind();
                //GridIndustryMap.ExpandRow(1);

            }
        }
        protected void  BindIndustryMap()
        {
            DTIndustry = objReport.IndustryMapReport();
            GridIndustryMap.DataSource = DTIndustry;
            GridIndustryMap.DataBind();
        }
        protected void GridIndustryMap_DataBinding(object sender, EventArgs e)
        {
            DTIndustry = objReport.IndustryMapReport();
            GridIndustryMap.DataSource = DTIndustry;
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
            //GridSalesReport.Columns[7].Visible = false;
            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = Convert.ToString((Session["Contactrequesttype"] ?? "Industry Map"));
            exporter.FileName = filename;

            exporter.PageHeader.Left = Convert.ToString((Session["Contactrequesttype"] ?? "Industry Map Report"));
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


        //protected void Grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        //{
        //    if (e.MenuType == GridViewContextMenuType.Rows)
        //    {
        //        var item = e.CreateItem("Export", "Export");
        //        item.BeginGroup = true;
        //        e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item);

        //        AddMenuSubItem(item, "PDF", "ExportToPDF", @"Images/ExportToPdf.png", true);
        //        AddMenuSubItem(item, "XLS", "ExportToXLS", @"Images/ExportToXls.png", true);
        //    }
        //}
        //private static void AddMenuSubItem(GridViewContextMenuItem parentItem, string text, string name, string imageUrl, bool isPostBack)
        //{
        //    var exportToXlsItem = parentItem.Items.Add(text, name);
        //    exportToXlsItem.Image.Url = imageUrl;
        //}
        //protected void Grid_ContextMenuItemClick(object sender, ASPxGridViewContextMenuItemClickEventArgs e)
        //{
        //    //switch (e.Item.Name)
        //    //{
        //    //    case "ExportToPDF":
        //    //        GridExporter.WritePdfToResponse();
        //    //        break;
        //    //    case "ExportToXLS":
        //    //        GridExporter.WriteXlsToResponse();
        //    //        break;
        //    //}
        //}
        //protected void Grid_AddSummaryItemViaContextMenu(object sender, ASPxGridViewAddSummaryItemViaContextMenuEventArgs e)
        //{
        //    if (e.SummaryItem.FieldName == "UnitsInStock" && e.SummaryItem.SummaryType == SummaryItemType.Average)
        //        e.SummaryItem.ValueDisplayFormat = "{0:0.00}";
        //}

        //int totalSum;
        //protected void Grid_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        //{
        //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        //        totalSum = 0;
        //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
        //        if (GridIndustryMap.Selection.IsRowSelectedByKey(e.GetValue(GridIndustryMap.KeyFieldName)))
        //            totalSum += Convert.ToInt32(e.FieldValue);
        //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
        //        e.TotalValue = totalSum;
        //}

    }
}