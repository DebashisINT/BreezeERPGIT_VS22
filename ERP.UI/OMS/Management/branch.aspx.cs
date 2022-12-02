using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;

namespace ERP.OMS.Management
{

    public partial class management_Branch : System.Web.UI.Page
    {   /* For 3 Tier
    DBEngine oDBEngine = new DBEngine(string.Empty);
      */
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Bank objBankStatementBL = new BusinessLogicLayer.Bank();
       // string MyGlobalVariable = "ConnectionStrings:crmConnectionString";

        protected void Page_Init(object sender, EventArgs e)
        {
            gridStatusDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);  //Chinmoy added below code
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Response.Write(); 
                // gridStatusDataSource.
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'


                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            fillGrid();
        }
        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] CallVal = e.Parameters.ToString().Split('~');
            // string tranid = e.Parameters.ToString();

            if (CallVal[0].ToString() == "s")
            {
                gridStatus.Settings.ShowFilterRow = true;
            }
            else if (CallVal[0].ToString() == "All")
            {
                gridStatus.FilterExpression = string.Empty;
            }
            else if (CallVal[0].ToString() == "Delete")
            {

                oDBEngine.DeleteValue("tbl_master_branch ", "branch_ID ='" + CallVal[1].ToString() + "' and branch_id not in (select distinct cnt_branchid from tbl_master_contact)");
                this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
                fillGrid();

            }


        }
        public void fillGrid()
        {
            gridStatusDataSource.SelectCommand = "select tbl_master_branch.branch_id, tbl_master_branch.branch_internalId, tbl_master_branch.branch_code, tbl_master_branch.branch_type, tbl_master_branch.branch_parentId, tbl_master_branch.branch_description, tbl_master_branch.branch_regionid, tbl_master_branch.branch_address1, tbl_master_branch.branch_address2,tbl_master_branch.branch_address3, (select cou_country from tbl_master_country where cou_id=tbl_master_branch.branch_country) as Country, (select state from tbl_master_State where id=tbl_master_branch.branch_state) as State, tbl_master_branch.branch_pin,(select city_name from tbl_master_city where city_id=tbl_master_branch.branch_city)as City, tbl_master_branch.branch_phone, tbl_master_branch.branch_head, tbl_master_branch.branch_contactPerson, tbl_master_branch.branch_cpPhone,tbl_master_branch.branch_cpEmail, tbl_master_branch.CreateDate, tbl_master_branch.CreateUser, tbl_master_branch.LastModifyDate, tbl_master_branch.LastModifyUser, tbl_master_branch.branch_Fax, (case when tbl_master_branch.branch_parentId = 0 then 'None' else (select A.branch_description from tbl_master_branch A where A.branch_id=tbl_master_branch.branch_parentId) end) as ParentBranch  from  tbl_master_branch";
            gridStatus.DataBind();

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