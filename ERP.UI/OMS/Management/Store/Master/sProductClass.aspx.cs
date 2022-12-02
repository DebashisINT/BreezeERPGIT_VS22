using System;
using System.Web;
using DevExpress.Web;
using System.Data;
using System.Web.Services;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Store.Master
{
    public partial class management_sProductClass : System.Web.UI.Page
    {
        public string pageAccess = "";
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSourceProductClass_ParentID.ConnectionString= Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            markets.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            HsnDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            servicetaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sProductClass.aspx");
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        public void bindexport(int Filter)
        {
            marketsGrid.Columns[9].Visible = false;
            marketsGrid.Columns[10].Visible = false;
            //SchemaGrid.Columns[11].Visible = false;
            //SchemaGrid.Columns[12].Visible = false;
            string filename = "Product Class/Group";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Product Class/Group";
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
        protected void marketsGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.Data)
            //{
            //    int commandColumnIndex = -1;
            //    for (int i = 0; i < marketsGrid.Columns.Count; i++)
            //        if (marketsGrid.Columns[i] is GridViewCommandColumn)
            //        {
            //            commandColumnIndex = i;
            //            break;
            //        }
            //    if (commandColumnIndex == -1)
            //        return;
            //    //____One colum has been hided so index of command column will be leass by 1 
            //    commandColumnIndex = commandColumnIndex - 4;
            //    DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
            //    for (int i = 0; i < cell.Controls.Count; i++)
            //    {
            //        DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
            //        if (button == null) return;
            //        DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

            //        if (hyperlink.Text == "Delete")
            //        {
            //            if (Convert.ToString(Session["PageAccess"]) == "DelAdd" || Convert.ToString(Session["PageAccess"]) == "Delete" || Convert.ToString(Session["PageAccess"]) == "All")
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
        protected void marketsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!marketsGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = marketsGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"] != null)
                {
                    if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                        RT.Visible = true;
                    else
                        RT.Visible = false;
                }
            }

        }
        protected void marketsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                marketsGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                marketsGrid.FilterExpression = string.Empty;
            }
        }
        [WebMethod]
        public static bool CheckUniqueCode(string ProductClassCode, string proclassid)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                DataTable dt = new DataTable();
                if (proclassid == "0")
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_ProductClass] WHERE [ProductClass_Code] = " + "'" + ProductClassCode + "'");
                }
                else
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_ProductClass] WHERE [ProductClass_Code] = " + "'" + ProductClassCode + "' and ProductClass_ID<>'" + proclassid + "'");
                }

                int cnt = dt.Rows.Count;
                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

        protected void marketsGrid_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {
            var x = e;
            string msg = Convert.ToString(ConfigurationManager.AppSettings["DeleteErrorMessage"]);
            if (msg.Equals(""))
            {
                msg = "Delete Unsuccessful!";
            }
            e.ErrorText = msg;
        }

        protected void marketsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

        }

        protected void marketsGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            int id = Convert.ToInt32(Convert.ToString(e.EditingKeyValue));
            Session["id"] = id;
            SqlSourceProductClass_ParentID.SelectCommand = "select ProductClass_ID,ProductClass_Name from Master_ProductClass where ProductClass_ID <> " + id + "   order by ProductClass_Name ";

        }

        protected void marketsGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {


        }


        protected void marketsGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }

        protected void marketsGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            BusinessLogicLayer.MasterDataCheckingBL delobj = new BusinessLogicLayer.MasterDataCheckingBL();

            if (e.Keys[0] != null)
            {
                if (delobj.DeleteProductClass(Convert.ToInt32(e.Keys[0])) < 0)
                {
                    marketsGrid.JSProperties["cpErrorMsg"] = "Product Class is in use. Cannot delete.";
                    e.Cancel = true;
                }
            }
        }

    }
}