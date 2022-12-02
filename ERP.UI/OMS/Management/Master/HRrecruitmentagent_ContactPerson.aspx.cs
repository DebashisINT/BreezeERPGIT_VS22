using System;
using System.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRrecruitmentagent_ContactPerson : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();


        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDesignation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlFamRelationShip.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["requesttype"] != null)
            //{
            //    lblHeadTitle.Text = Session["requesttype"].ToString() + " Contact Person";
            //}
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
           
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }
           // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Master/HRrecruitmentagent_ContactPerson.aspx");
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService");
            if (!IsPostBack)
            {
                Session["exportval"] = null;
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
            if (EmployeeNameID[0, 0] != "n")
            {
                lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
            }
        }
        protected void GridContactPerson_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "status")
            {
                if (e.CellValue.Equals("Suspended"))
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
            }
        }
        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
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
            string filename = "Recruitment Agents Details (Contact Person)";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Recruitment Agents Details (Contact Person)";
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

        protected void GridContactPerson_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (GridContactPerson.IsNewRowEditing)
            {
                if (e.Column.FieldName == "cp_status")
                {
                    ASPxComboBox cmb = e.Editor as ASPxComboBox;
                    cmb.SelectedIndex = 0;  //or another code that allows to set selected index/value according to your requirements
                }
            }
        }
    }
}