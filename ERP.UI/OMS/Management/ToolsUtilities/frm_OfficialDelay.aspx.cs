using System;
using System.Web;
////using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;


namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_OfficialDelay : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckUserSession(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            AllEmployee.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //DataOD.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    DataOD.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //DataOD.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    DataOD.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                EmployeeIds.Value = oDBEngine.getAllEmployeeInHierarchy(HttpContext.Current.Session["userid"].ToString(), "");
                //string whereCondition = "e.emp_contactId=contact.cnt_internalid and e.emp_contactId=ectc.emp_cntId and ectc.emp_effectiveuntil is null and e.emp_id in (" + EmployeeIds.Value + ")";
                //AllEmployee.SelectCommand = "Select (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid as ID from tbl_master_contact contact,tbl_master_employee e, tbl_trans_employeeCTC ectc where " + whereCondition;

            }
            string whereCondition = "e.emp_contactId=contact.cnt_internalid and e.emp_contactId=ectc.emp_cntId and ectc.emp_effectiveuntil is null and e.emp_id in (" + EmployeeIds.Value + ")";
            AllEmployee.SelectCommand = "Select (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid as ID from tbl_master_contact contact,tbl_master_employee e, tbl_trans_employeeCTC ectc where " + whereCondition;
            whereCondition = " emp_id in (" + EmployeeIds.Value + ")";
            DataOD.SelectCommand = "SELECT [od_id], [od_cntId], [od_type],convert(varchar(11),od_From,113) as od_from1, [od_From],convert(varchar(11),od_To,113) as od_to1, [od_To], [od_reportTime], [od_reason] FROM [tbl_trans_OfficialDelay],[tbl_master_employee] where emp_contactId=od_cntId and" + whereCondition + " ORDER by tbl_trans_OfficialDelay.CreateDate DESC";
        }
        protected void GridOD_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (!GridOD.IsNewRowEditing)
            {

                if (e.Column.FieldName == "od_cntId")
                    e.Editor.ReadOnly = true;
                if (e.Column.FieldName == "od_type")
                    e.Editor.ReadOnly = true;

            }
            if (e.Column.FieldName == "od_From")
            {
                ASPxDateEdit fromDate = (ASPxDateEdit)e.Editor;
                fromDate.MinDate = oDBEngine.GetDate().AddDays(-1);
            }
            if (e.Column.FieldName == "od_To")
            {
                ASPxDateEdit ToDate = (ASPxDateEdit)e.Editor;
                ToDate.MinDate = oDBEngine.GetDate().AddDays(-1);
            }
        }
    }
}