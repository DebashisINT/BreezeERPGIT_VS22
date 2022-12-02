using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_Master_Lead : ERP.OMS.ViewState_class.VSPage
    {

        public string pageAccess = "";
        DBEngine oDBEngine = new DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LeadGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            showgrid();
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void showgrid()
        {
            string AllUserList = "";
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //AllUserList = oDBEngine.getChildUser_for_report(HttpContext.Current.Session["userid"].ToString(), "");
            //AllUserList += HttpContext.Current.Session["userid"].ToString();
            //Session["userlist"] = AllUserList;
            DataTable DT = oDBEngine.GetDataTable(" tbl_master_lead, tbl_master_branch ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name, tbl_master_branch.branch_description AS BranchName, (select top 1 isnull(case mp.phf_phonenumber when null then '' else (Select top 1  '(M)'+ m.phf_phonenumber from tbl_master_phonefax m where m.phf_cntid=cnt_internalId and m.phf_type = 'Mobile') end,'')  + isnull(case mp.phf_phonenumber  when null then '' else (Select top 1  '(R)'+ r.phf_phonenumber from tbl_master_phonefax r where r.phf_cntid=cnt_internalId and r.phf_type = 'Residence') end,'')  + isnull(case mp.phf_phonenumber  when null then '' else (Select top 1  '(R)'+ o.phf_phonenumber from tbl_master_phonefax o where o.phf_cntid=cnt_internalId and o.phf_type = 'Office') end ,'') from tbl_master_phonefax mp where mp.phf_cntid=cnt_internalId)as phone, case tbl_master_lead.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status, tbl_master_lead.cnt_internalId AS Id,tbl_master_lead.createdate, tbl_master_lead.cnt_InternalId as cnt_UCC,tbl_master_lead.createUser, tbl_master_lead.cnt_id ", " tbl_master_lead.CreateUser in (" + HttpContext.Current.Session["userchildHierarchy"].ToString() + ") and tbl_master_lead.cnt_branchid = tbl_master_branch.branch_id order by tbl_master_lead.CreateDate desc ");
            LeadGrid.DataSource = DT.DefaultView;
            LeadGrid.DataBind();
        }
        protected void btnSearch(object sender, EventArgs e)
        {
            LeadGrid.Settings.ShowFilterRow = true;
        }

        protected void LeadGrid_CustomCallback1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LeadGrid.ClearSort();
            LeadGridDataSource.SelectCommand = "select ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name, tbl_master_branch.branch_description AS BranchName, (select top 1 isnull(case mp.phf_phonenumber when null then '' else (Select top 1  '(M)'+ m.phf_phonenumber from tbl_master_phonefax m where m.phf_cntid=cnt_internalId and m.phf_type = 'Mobile') end,'')  + isnull(case mp.phf_phonenumber  when null then '' else (Select top 1  '(R)'+ r.phf_phonenumber from tbl_master_phonefax r where r.phf_cntid=cnt_internalId and r.phf_type = 'Residence') end,'')  + isnull(case mp.phf_phonenumber  when null then '' else (Select top 1  '(R)'+ o.phf_phonenumber from tbl_master_phonefax o where o.phf_cntid=cnt_internalId and o.phf_type = 'Office') end ,'') from tbl_master_phonefax mp where mp.phf_cntid=cnt_internalId)as phone, case tbl_master_lead.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status, tbl_master_lead.cnt_internalId AS Id,tbl_master_lead.createdate, tbl_master_lead.cnt_InternalId as cnt_UCC,tbl_master_lead.createUser, tbl_master_lead.cnt_id from tbl_master_lead, tbl_master_branch Where tbl_master_lead.CreateUser in (" + HttpContext.Current.Session["userchildHierarchy"].ToString() + ") and tbl_master_lead.cnt_branchid = tbl_master_branch.branch_id order by tbl_master_lead.CreateDate desc";
            LeadGrid.DataBind();
            if (e.Parameters == "s")
                LeadGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                LeadGrid.FilterExpression = string.Empty;
            }
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
        protected void LeadGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
    }
}