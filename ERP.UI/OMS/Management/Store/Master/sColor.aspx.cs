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
    public partial class management_master_Store_sProductClass : System.Web.UI.Page
    {
        public string pageAccess = "";
        string[] lengthIndex;
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sColor.aspx");
            SqlSourceProductClass_ParentID.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            markets.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        public void bindexport(int Filter)
        {
            marketsGrid.Columns[4].Visible = false;

            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = "Colors";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Colors";
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
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < marketsGrid.Columns.Count; i++)
                    if (marketsGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 5;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if ( Convert.ToString(Session["PageAccess"]) == "DelAdd" ||  Convert.ToString(Session["PageAccess"]) == "Delete" ||  Convert.ToString(Session["PageAccess"]) == "All")
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
        protected void marketsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!marketsGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = marketsGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if ( Convert.ToString(Session["PageAccess"]).Trim() == "Add" ||  Convert.ToString(Session["PageAccess"]).Trim() == "Modify" ||  Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
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
            string[] CallVal = Convert.ToString(e.Parameters).Split('~');
            lengthIndex = e.Parameters.Split('~');
            if (Convert.ToString(lengthIndex[0]) == "Delete")
            {
                string PinId = Convert.ToString(Convert.ToString(CallVal[1]));
                int retValue = masterChecking.DeleteMastercolor(Convert.ToInt32(PinId));
                if (retValue > 0)
                {
                    Session["KeyVal"] = "Succesfully Deleted";
                    marketsGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                    marketsGrid.DataBind();
                }
                else
                {
                    Session["KeyVal"] = "Used in other modules. Cannot Delete.";
                    marketsGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";

                }

            }
        }
        //[WebMethod]
        //public static bool CheckUniqueName(string ColorName)
        //{
        //    bool flag = false;
        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Color] WHERE [Color_Name] = " + "'" + ColorName + "'");
        //        int cnt = dt.Rows.Count;
        //        if (cnt > 0)
        //        {
        //            flag = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //    }
        //    return flag;
        //}



        // [WebMethod]
        //public static bool CheckUniqueCode(string ColorCode)
        // {
        //    bool flag = false;
        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Color] WHERE [Color_Code] = " + "'" + ColorCode + "'");
        //        int cnt = dt.Rows.Count;
        //        if (cnt > 0)
        //        {
        //            flag = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //    }
        //    return flag;
        //}

        
        protected void marketsGrid_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {
            var vErrorMsg = e.ErrorText;

            if (vErrorMsg.Contains("Violation of PRIMARY KEY constraint 'PK_Master_Color"))
            {
               e.ErrorText = "Duplicate Color Name Not Allowed";
                //comment by sanjib due to alert to jalert()23122016;
               // Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Duplicate Color Name Not Allowed')</script>");
               // return;
            }
            if (vErrorMsg.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                //comment by sanjib due to alert to jalert()23122016;
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Used in other modules. Can not Delete.')</script>");
                
               
                e.ErrorText = "Used in other modules. Can not Delete.";
            }

        }

        protected void marketsGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.ButtonType == ColumnCommandButtonType.New)
            {
                if (rights.CanAdd)
                {
                    e.Visible = true;
                }
                else
                {
                    e.Visible = false;
                }
            }
            //if (e.ButtonType == ColumnCommandButtonType.Delete)
            //{
            //    if (rights.CanDelete)
            //    {
            //        e.Visible = true;
            //    }
            //    else
            //    {
            //        e.Visible = false;
            //    }
            //}
            //if (e.ButtonType == ColumnCommandButtonType.Edit)
            //{
            //    if (rights.CanEdit)
            //    {
            //        e.Visible = true;
            //    }
            //    else
            //    {
            //        e.Visible = false;
            //    }
            //}
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
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Color] WHERE [Color_Code] = " + "'" + ProductClassCode + "'");
                }
                else
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Color] WHERE [Color_Code] = " + "'" + ProductClassCode + "' and Color_ID<>'" + proclassid + "'");
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
         

        protected void marketsGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            
            marketsGrid.SettingsText.PopupEditFormCaption="Modify Color";
            int id = Convert.ToInt32( Convert.ToString(e.EditingKeyValue));
            Session["id"] = id;
        }

        protected void marketsGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            marketsGrid.SettingsText.PopupEditFormCaption = "Add Color";
        }
    }
}