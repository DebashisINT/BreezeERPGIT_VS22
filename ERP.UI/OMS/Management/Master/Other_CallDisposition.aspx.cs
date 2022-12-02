using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Other_CallDisposition : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                //string sPath = HttpContext.Current.Request.Url.ToString();
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            
            CallDisposition.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DispositionSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
         
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Other_CallDisposition.aspx");
           
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //CallDisposition.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                }
                else
                {
                  //  CallDisposition.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                //code Added By Priti on 21122016 to use Export Header,date
                Session["exportval"] = null;
                //....end...
            }
        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 21122016 to use Export Header,date
            string filename = "Call Dispositions";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Call Dispositions";           
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
            //Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //Code  Added and Commented By Priti on 21122016 to use Export Header,date
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
        protected void btnSearch(object sender, EventArgs e)
        {
            CallDispositionGrid.Settings.ShowFilterRow = true;
        }
        protected void CallDispositionGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.Data)
            //{
            //    int commandColumnIndex = -1;
            //    for (int i = 0; i < CallDispositionGrid.Columns.Count; i++)
            //        if (CallDispositionGrid.Columns[i] is GridViewCommandColumn)
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
            //            if (Session["PageAccess"] == "DelAdd" || Session["PageAccess"] == "Delete" || Session["PageAccess"] == "All")
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

           // }

        }
        protected void CallDispositionGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (!CallDispositionGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = CallDispositionGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                //if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Edit" || Session["PageAccess"].ToString().Trim() == "All")
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Edit" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void CallDispositionGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                CallDispositionGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                CallDispositionGrid.FilterExpression = string.Empty;
            }
        }

        //Purpose: Add Edit and delete rights to call disposition
        //Name: Debjyoti Dhar.
        protected void CallDispositionGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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
        public bool GetChecked(string Access)
        {
            switch (Access)
            {
                case "True":
                    return true;
                case "":
                    return false;
                case "Null":
                    return false;
                default:
                    return false;
            }

        }

        protected void CallDispositionGrid_CustomCallback1(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string status = e.Parameters;
            int i = 0;
          var s = status.Split('~');
          string id = s[0];
          string active = s[1];
          BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
          i = obl.EditCallDispositionIsActiveStatus(id, active);
            if(i>0)
            { 
          DispositionSelect.SelectCommand = "SELECT cd.call_id AS ID, cd.IsActive AS IsActive,cd.call_dispositions AS Dispositions, d.Description as Description, cd.Call_Category FROM tbl_master_calldispositions AS cd INNER JOIN tbl_Master_DispositionCategory AS d ON cd.Call_Category = d.Int_id";
            }
        }
        protected void chkActivateBatch_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            string itemindex = Convert.ToString((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            //Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {   { grid.PerformCallback('{0}');  }  }", itemindex);
            Dcheckbox.ClientSideEvents.CheckedChanged = "function(s, e) {  ActiveCall(s,e," + itemindex + ");  }";
        }

      //  protected void chkActive_Init(object sender, EventArgs e)
      //  {
      //      ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
      //      int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
      //Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetCheckStockBlock(s, e, {0}) }}", itemindex);
      //  }

      

    }
}