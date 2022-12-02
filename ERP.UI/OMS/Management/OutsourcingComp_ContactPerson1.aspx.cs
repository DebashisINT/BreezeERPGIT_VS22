using System;
using System.Configuration;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Web;

namespace ERP.OMS.Management
{
    public partial class management_OutsourcingComp_ContactPerson1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
            {
                string ID = Request.QueryString["id"];
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                DBEngine oDBEngine = new DBEngine();
                string[,] internalID = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalId ", " cnt_id=" + ID, 1);
                Session["KeyVal_InternalID"] = internalID[0, 0];
            }
            SqlDataSource1.SelectCommand = "select A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as phone,(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, cp_status from tbl_master_contactperson A  where cp_agentInternalId='" + Session["KeyVal_InternalID"] + "' ORDER BY cp_status desc";
            //GridContactPerson.DataBind();
        }
        protected void GridContactPerson_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "status")
            {
                if (e.CellValue.Equals("Suspended"))
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
            }
        }
        //__________select command
        //__________select A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as phone,(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, cp_status from tbl_master_contactperson A  where cp_agentInternalId=@insuId ORDER BY cp_status
    }
}