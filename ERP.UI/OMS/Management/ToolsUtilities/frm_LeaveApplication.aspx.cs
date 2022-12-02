using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_LeaveApplication : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        Utilities obj = new Utilities();
        public string pageAccess = "";
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
                    //DataSourceLeaveApplication.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    DataSourceLeaveApplication.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //DataSourceLeaveApplication.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    DataSourceLeaveApplication.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
            string whereCondition = "e.emp_contactId=contact.cnt_internalid and e.emp_contactId=ectc.emp_cntId and ectc.emp_effectiveuntil is null and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")";
            AllEmployee.SelectCommand = "Select (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid as ID from tbl_master_contact contact,tbl_master_employee e, tbl_trans_employeeCTC ectc where " + whereCondition;
            DataSourceLeaveApplication.SelectCommand = "SELECT (select branch_description from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where cnt_internalId=la_cntId)) as branch,[la_id], [la_cntId], [la_appType], [la_Consideration], [la_ReceivedPhysical], [la_appDate], [la_startDateAppl], [la_endDateAppl], [la_appStatus], [la_apprRejBy], [la_apprRejOn], [la_startDateApr], [la_endDateApr],convert(varchar(11),la_startDateApr,113) as stdate,convert(varchar(11),la_endDateApr,113) as endate,[la_joinDateTime],[la_Remarks] FROM [tbl_trans_LeaveApplication]";
        }
        protected void GridApplicationLeave_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            if (!GridApplicationLeave.IsNewRowEditing)
            {
                object keyFieldValue = GridApplicationLeave.GetRowValues(GridApplicationLeave.FocusedRowIndex, new string[] { GridApplicationLeave.KeyFieldName });

                //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //lcon.Open();
                //SqlCommand lcmd = new SqlCommand("LeaveApplicationUpdate", lcon);
                //lcmd.CommandType = CommandType.StoredProcedure;
                //lcmd.Parameters.Add("@la_cntId", SqlDbType.VarChar).Value = e.NewValues["la_cntId"].ToString();
                //lcmd.Parameters.Add("@la_appType", SqlDbType.VarChar).Value = e.NewValues["la_appType"].ToString();
                //lcmd.Parameters.Add("@la_Consideration", SqlDbType.VarChar).Value = e.NewValues["la_Consideration"].ToString();
                //lcmd.Parameters.Add("@la_ReceivedPhysical", SqlDbType.VarChar).Value = e.NewValues["la_ReceivedPhysical"].ToString();
                //lcmd.Parameters.Add("@la_appDate", SqlDbType.DateTime).Value = Convert.ToDateTime(e.NewValues["la_appDate"].ToString());
                //lcmd.Parameters.Add("@la_startDateAppl", SqlDbType.DateTime).Value = Convert.ToDateTime(e.NewValues["la_startDateAppl"].ToString());
                //lcmd.Parameters.Add("@la_endDateAppl", SqlDbType.DateTime).Value = Convert.ToDateTime(e.NewValues["la_endDateAppl"].ToString());
                //lcmd.Parameters.Add("@la_appStatus", SqlDbType.VarChar).Value = e.NewValues["la_appStatus"].ToString();
                //lcmd.Parameters.Add("@la_apprRejBy", SqlDbType.VarChar).Value = e.NewValues["la_apprRejBy"].ToString();
                //lcmd.Parameters.Add("@la_apprRejOn", SqlDbType.DateTime).Value = Convert.ToDateTime(e.NewValues["la_apprRejOn"].ToString());
                //if (e.NewValues["la_startDateApr"] != null)
                //    lcmd.Parameters.Add("@la_startDateApr", SqlDbType.DateTime).Value = Convert.ToDateTime(e.NewValues["la_startDateApr"].ToString());
                //else
                //    lcmd.Parameters.Add("@la_startDateApr", SqlDbType.DateTime).Value = "1/1/1900";
                //if (e.NewValues["la_endDateApr"] != null)
                //    lcmd.Parameters.Add("@la_endDateApr", SqlDbType.DateTime).Value = Convert.ToDateTime(e.NewValues["la_endDateApr"].ToString());
                //else
                //    lcmd.Parameters.Add("@la_endDateApr", SqlDbType.DateTime).Value = "1/1/1900";
                //lcmd.Parameters.Add("@userId", SqlDbType.Int).Value = int.Parse(HttpContext.Current.Session["userid"].ToString());
                //lcmd.Parameters.Add("@la_id", SqlDbType.Int).Value = int.Parse(keyFieldValue.ToString());
                //lcmd.Parameters.Add("@la_joinDateTime", SqlDbType.DateTime).Value = Convert.ToDateTime(e.NewValues["la_joinDateTime"].ToString());
                //if (e.NewValues["la_Remarks"] != null)
                //    lcmd.Parameters.Add("@la_Remarks", SqlDbType.VarChar).Value = e.NewValues["la_Remarks"].ToString();
                //else
                //    lcmd.Parameters.Add("@la_Remarks", SqlDbType.VarChar).Value = string.Empty;
                //int NoOfRowEffected = lcmd.ExecuteNonQuery();
                //lcmd.Dispose();
                //lcon.Close();
                //lcon.Dispose();
                ////}  
                string la_startDateApr = "";
                string la_endDateApr = "";
                string la_Remarks = "";

                if (e.NewValues["la_startDateApr"] != null)
                    la_startDateApr = e.NewValues["la_startDateApr"].ToString();
                else
                    la_startDateApr = "1/1/1900";
                if (e.NewValues["la_endDateApr"] != null)
                    la_endDateApr = e.NewValues["la_endDateApr"].ToString();
                else
                    la_endDateApr = "1/1/1900";
                if (e.NewValues["la_Remarks"] != null)
                    la_Remarks = e.NewValues["la_Remarks"].ToString();
                else
                    la_Remarks = string.Empty;

                obj.LeaveApplicationUpdate(e.NewValues["la_cntId"].ToString(), e.NewValues["la_appType"].ToString(), e.NewValues["la_Consideration"].ToString(),
                     e.NewValues["la_ReceivedPhysical"].ToString(), Convert.ToDateTime(e.NewValues["la_appDate"].ToString()), Convert.ToDateTime(e.NewValues["la_startDateAppl"].ToString()),
                      Convert.ToDateTime(e.NewValues["la_endDateAppl"].ToString()), e.NewValues["la_appStatus"].ToString(), e.NewValues["la_apprRejBy"].ToString(),
                      Convert.ToDateTime(e.NewValues["la_apprRejOn"].ToString()), Convert.ToDateTime(la_startDateApr), Convert.ToDateTime(la_endDateApr),
                      int.Parse(HttpContext.Current.Session["userid"].ToString()), int.Parse(keyFieldValue.ToString()), Convert.ToDateTime(e.NewValues["la_joinDateTime"].ToString()), la_Remarks);
            }
        }
        protected void GridApplicationLeave_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            //if (!GridApplicationLeave.IsNewRowEditing)
            //{
            //    if (e.Column.FieldName == "la_cntId")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_appStatus")
            //    {
            //        if (e.Editor.Value.ToString() != "DU")
            //            e.Editor.ReadOnly = true;
            //    }
            //    if (e.Column.FieldName == "la_Consideration")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_appDate")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_appType")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_apprRejBy")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_apprRejOn")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_startDateAppl")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_endDateAppl")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_startDateApr")
            //        e.Editor.ReadOnly = true;
            //    if (e.Column.FieldName == "la_endDateApr")
            //        e.Editor.ReadOnly = true;
            //}

        }
        protected void GridApplicationLeave_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                GridApplicationLeave.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridApplicationLeave.FilterExpression = string.Empty;
            }
        }
        protected void GridApplicationLeave_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
    }
}