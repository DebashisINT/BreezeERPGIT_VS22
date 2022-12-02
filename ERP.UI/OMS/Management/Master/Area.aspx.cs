using System;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_Area : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        string[] lengthIndex;
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
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
            insertupdate.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectState.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Area.aspx");
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //insertupdate.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                }
                else
                {
                   // insertupdate.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
               ////Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if(!IsPostBack)
            { }
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
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bindUserGroups();

            //Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            string filename = "Area";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Area";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            switch (Filter)
            {
                case 1:
                    //exporter.WritePdfToResponse();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        exporter.WritePdf(stream);
                        WriteToResponse("Area", true, "pdf", stream);
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
        protected void AreaGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < AreaGrid.Columns.Count; i++)
                    if (AreaGrid.Columns[i] is GridViewCommandColumn)
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
        protected void AreaGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (!AreaGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = AreaGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void AreaGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //if (e.Parameters == "s")
            //    AreaGrid.Settings.ShowFilterRow = true;

            //if (e.Parameters == "All")
            //{
            //    AreaGrid.FilterExpression = string.Empty;
            //}

            if (e.Parameters == "All")
            {
                AreaGrid.FilterExpression = string.Empty;
            }
            string[] CallVal = Convert.ToString(e.Parameters).Split('~');
            lengthIndex = e.Parameters.Split('~');
            if (Convert.ToString(lengthIndex[0]) == "Delete")
            {
                string PinId = Convert.ToString(Convert.ToString(CallVal[1]));
                int retValue = masterChecking.DeleteMasterArea(Convert.ToInt32(PinId));
                if (retValue > 0)
                {
                    Session["KeyVal"] = "Succesfully Deleted";
                    AreaGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                    AreaGrid.DataBind();
                }
                else
                {
                    Session["KeyVal"] = "Used in other modules. Cannot Delete.";
                    AreaGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";

                }

            }
        }

        protected void AreaGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            AreaGrid.SettingsText.PopupEditFormCaption = "Add Area";
        }

        protected void AreaGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            AreaGrid.SettingsText.PopupEditFormCaption = "Modify Area";
        }

        protected void AreaGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            int cityid = Convert.ToInt32(e.NewValues["SId"]);
            string area =Convert.ToString(e.NewValues["name"]);

            if (e.IsNewRow)
            {
                DataTable dtadd = oDBEngine.GetDataTable("select area_name from tbl_master_area where city_id=" + cityid + " and area_name='" + area + "'");

                if(dtadd.Rows.Count>0)
                {
                    e.RowError = "Area for the selected city already exists. Cannot Proceed.";
                    return;
                } 
            }
            else
            { 
                string oldarea = Convert.ToString(e.OldValues["name"]); 
                if (oldarea != area)
                {
                    DataTable dtadd = oDBEngine.GetDataTable("select area_name from tbl_master_area where city_id=" + cityid + " and area_name='" + area + "'");
                    if (dtadd.Rows.Count > 0)
                    {
                        e.RowError = "Area for the selected city already exists. Cannot Proceed.";
                        return;
                    } 
                }

            }
        }
        protected void AreaGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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