using System;
using System.Data;
using System.Configuration;
using BusinessLogicLayer;
using System.Web;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_MyTeam : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckUserSession(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            DataTable DT_treeview = new DataTable();

            DataColumn DC_id = new DataColumn("ID");
            DataColumn DC_parentID = new DataColumn("ParentID");
            DataColumn DC_Name = new DataColumn("Name");
            DT_treeview.Columns.Add(DC_id);
            DT_treeview.Columns.Add(DC_parentID);
            DT_treeview.Columns.Add(DC_Name);
            string[,] ParentID = oDBEngine.GetFieldValue("tbl_master_employee inner join tbl_master_user  on tbl_master_employee.emp_contactid=tbl_master_user.user_contactid INNER JOIN tbl_trans_employeeCTC ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId INNER JOIN   tbl_master_contact  ON tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId INNER JOIN  tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id INNER JOIN  tbl_master_company ON tbl_trans_employeeCTC.emp_Organization = tbl_master_company.cmp_id INNER JOIN  tbl_master_designation ON tbl_trans_employeeCTC.emp_Designation = tbl_master_designation.deg_id ", "tbl_master_employee.emp_id,isnull(tbl_master_contact.cnt_firstName,'') + ' ' + isnull(tbl_master_contact.cnt_middlename,'') + isnull(tbl_master_contact.cnt_lastName,'') + '  [' + isnull(tbl_master_company.cmp_Name,'')+ ' : ' + isnull(tbl_master_branch.branch_description,'') + ' : ' + isnull(tbl_master_designation.deg_designation,'') +' : '+ isnull((select top 1 tbl_master_phonefax.phf_phonenumber from tbl_master_phonefax inner join tbl_trans_employeeCTC  on tbl_trans_employeeCTC.emp_cntid=tbl_master_phonefax.phf_cntid inner join tbl_master_employee on tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId inner join tbl_master_user on tbl_master_employee.emp_contactid=tbl_master_user.user_contactid and tbl_master_phonefax.phf_type='Mobile' and  tbl_master_user.user_id='" + Session["userid"].ToString() + "'),'')+ ' : ' + isnull((select top 1 tbl_master_email.eml_email from tbl_master_email  inner join tbl_trans_employeeCTC on tbl_trans_employeeCTC.emp_cntid=tbl_master_email.eml_cntid inner join tbl_master_employee on tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId inner join tbl_master_user on tbl_master_employee.emp_contactid=tbl_master_user.user_contactid  and tbl_master_user.user_id='372'),'') + ']' AS Name   ", "tbl_master_user.user_id='" + Session["userid"].ToString() + "'", 2);
            DataRow DR = DT_treeview.NewRow();
            DR["ID"] = Convert.ToString(ParentID[0, 0]);

            DR["Name"] = (ParentID != null && ParentID.Length>1) ? Convert.ToString(ParentID[0, 1]) : "";
            DR["ParentID"] = "0";
            DT_treeview.Rows.Add(DR);
            CreateDataTable(ParentID[0, 0].ToString(), DT_treeview);
            TVorgHir.DataSource = DT_treeview;
            TVorgHir.DataBind();


        }

        private void CreateDataTable(string ParentID, DataTable DT_treeview)
        {
            DataTable ChildEmployees = oDBEngine.GetDataTable("tbl_trans_employeeCTC INNER JOIN   tbl_master_employee ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId INNER JOIN   tbl_master_contact ON tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId INNER JOIN  tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id INNER JOIN  tbl_master_company ON tbl_trans_employeeCTC.emp_Organization = tbl_master_company.cmp_id INNER JOIN  tbl_master_designation ON tbl_trans_employeeCTC.emp_Designation = tbl_master_designation.deg_id ", " tbl_master_employee.emp_id, isnull(tbl_master_contact.cnt_firstName,1) + ' ' + isnull(tbl_master_contact.cnt_middlename,1) + isnull(tbl_master_contact.cnt_lastName,'') + '  [' + isnull(tbl_master_company.cmp_Name,'') + ' : ' + isnull(tbl_master_branch.branch_description,'') + ' : ' + isnull(tbl_master_designation.deg_designation,'')+' : '+isnull((select top 1 tbl_master_phonefax.phf_phonenumber from tbl_master_phonefax inner join tbl_trans_employeeCTC  on tbl_trans_employeeCTC.emp_cntid=tbl_master_phonefax.phf_cntid and tbl_master_phonefax.phf_type='Mobile' and CAST(tbl_trans_employeeCTC.emp_reportTo as nvarchar(max))='" + ParentID + "'),'')+ ' : ' +isnull((select top 1 tbl_master_email.eml_email from tbl_master_email inner join tbl_trans_employeeCTC on tbl_trans_employeeCTC.emp_cntid=tbl_master_email.eml_cntid and cast(tbl_trans_employeeCTC.emp_reportTo as nvarchar(max))='" + ParentID + "'),'') + ']' AS Name  ", " CAST(tbl_trans_employeeCTC.emp_reportTo as nvarchar(max))='" + ParentID + "' and tbl_trans_employeeCTC.emp_effectiveuntil is null AND ((tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' OR tbl_master_employee.emp_dateofLeaving = '01/01/1900' OR tbl_master_employee.emp_dateofLeaving is null)) ", " tbl_master_contact.cnt_firstName ");
            if (ChildEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < ChildEmployees.Rows.Count; i++)
                {
                    DataRow dr = DT_treeview.NewRow();
                    dr["ID"] = ChildEmployees.Rows[i][0].ToString();
                    dr["Name"] = ChildEmployees.Rows[i][1].ToString();
                    dr["ParentID"] = ParentID;
                    DT_treeview.Rows.Add(dr);

                    CreateDataTable(ChildEmployees.Rows[i][0].ToString(), DT_treeview);
                }
            }
        }


        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExport.Value.ToString() != "")
            {
                if (cmbExport.Value.ToString() == "Pdf")
                    ASPxTreeListExporter1.WritePdfToResponse();
                if (cmbExport.Value.ToString() == "Xls")
                    ASPxTreeListExporter1.WriteXlsToResponse();
                if (cmbExport.Value.ToString() == "Rtf")
                    ASPxTreeListExporter1.WriteRtfToResponse();

                cmbExport.SelectedIndex = 0;
            }
        }
    }
}