using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;
using System.Data;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_language : ERP.OMS.ViewState_class.VSPage
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
        protected void Page_Load(object sender, EventArgs e)
        {
             rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/language.aspx");
           
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");



            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //language.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    language.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //language.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    language.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------
            LanguageGrid.JSProperties["cpDelmsg"] = null;
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
            string filename = "Languages";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Languages";           
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
            //...end...
        }

        protected void LanguageGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < LanguageGrid.Columns.Count; i++)
                    if (LanguageGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 1;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        //if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
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
        protected void LanguageGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (!LanguageGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = LanguageGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                //if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                {
                    RT.Visible = true; 
                }
                else { 
                    RT.Visible = false;
                }
            }

        }
        protected void LanguageGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                LanguageGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                LanguageGrid.FilterExpression = string.Empty;
            }
        }

        //Purpose: Add Edit and delete rights to Language
        //Name: Debjyoti Dhar.
        protected void LanguageGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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

        protected void LanguageGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (Convert.ToString(e.Values["lng_id"]) != "")
            {
                string lng_id = string.Empty;
                lng_id = Convert.ToString(e.Values["lng_id"]);

                //DataTable dt = oDBEngine.GetDataTable("select a.AccountGroup_ReferenceID as 'AccountGroup' from Master_AccountGroup a inner join Master_AccountGroup b on a.AccountGroup_ReferenceID=b.AccountGroup_ParentGroupID where a.AccountGroup_Code='" + strAccGroupCode + "' union all select MainAccount_AccountGroup as 'AccountGroup' from Master_MainAccount where MainAccount_AccountGroup=(select AccountGroup_ReferenceID from Master_AccountGroup where AccountGroup_Code='" + strAccGroupCode + "')");

                DataTable dt = oDBEngine.GetDataTable("select cnt_id,cnt_speakLanguage,cnt_writeLanguage from (SELECT cnt_id,LTRIM(RTRIM(m.n.value('.[1]','varchar(8000)'))) AS cnt_speakLanguage,LTRIM(RTRIM(u.v.value('.[1]','varchar(8000)'))) AS cnt_writeLanguage FROM (SELECT cnt_id,CAST('<XMLRoot><RowData>' + REPLACE(cnt_speakLanguage,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x,CAST('<XMLRoot><RowData>' + REPLACE(cnt_writeLanguage,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS y FROM tbl_master_contact)t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n) CROSS APPLY y.nodes('/XMLRoot/RowData')u(v))w where w.cnt_speakLanguage='" + lng_id + "' or w.cnt_writeLanguage='" + lng_id + "'");
                if (dt.Rows.Count > 0)
                {
                    LanguageGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                    //(sender as ASPxGridView).JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                    e.Cancel = true;

                }
                else
                {
                    //AccountGroup.JSProperties["cpDelmsg"] = "Cannot Delete. This AccountGroup Code Is In Use";
                    LanguageGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";

                }

            }
        }


    }
}