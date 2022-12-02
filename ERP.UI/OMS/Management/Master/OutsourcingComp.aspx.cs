using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_OutsourcingComp : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        DataSet Ds_Global;
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/OutsourcingComp.aspx");

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            EmployeeDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] == null)
            {
              ////Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //if (!IsPostBack)
             //   //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
                Ds_Global = new DataSet();
                EmployeeDataSource.SelectCommand = "select tbl_master_contact.cnt_id AS cnt_Id,tbl_master_contact.cnt_internalId AS Id, tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name, isnull(('(O)'+(select top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId and phf_type='Office')),'')+isnull(('(R)'+(select top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId and phf_type='Residence')),'')+isnull(('(M)'+(select top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId and phf_type='Mobile')),'')+isnull(('(F)'+(select top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId and phf_type='Fax')),'')+isnull(('(C)'+(select top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId and phf_type='Correspondence')),'') as phone from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id where cnt_contactType='OC'";
                EmployeeDataSource.DataBind();
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
        protected void EmployeeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            EmployeeGrid.ClearSort();
            EmployeeGrid.DataBind();
            if (e.Parameters == "s")
                EmployeeGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                EmployeeGrid.FilterExpression = string.Empty;
            }
        }
        protected void EmployeeGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
    }
}