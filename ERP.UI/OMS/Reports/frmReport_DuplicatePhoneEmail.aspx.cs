using System;
using System.Data;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_DuplicatePhoneEmail : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Init(object sender, EventArgs e)
        {
            LeadGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PhoneGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //} 
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (DropDownList1.SelectedValue.ToString() == "Email")
            {
                ShowEmailGrid();
                Panel1.Visible = true;
                Panel2.Visible = false;
                Label1.Text = "Email";
                Label1.Visible = true;
            }
            else
            {
                ShowPhoneGrid();
                Panel1.Visible = false;
                Panel2.Visible = true;
                Label1.Text = "Phone";
                Label1.Visible = true;
            }
        }
        private void ShowEmailGrid()
        {
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("tbl_master_email  a, (select eml_email, count(*) cnt from tbl_master_email group by eml_email having count(*) > 1) b ", "a.eml_id,a.eml_entity, a.eml_email, a.eml_cntid ,case when a.eml_entity='contact' Then (Select IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'')+'['+isnull(cnt_UCC,'')+']'  as name from tbl_master_contact where cnt_internalId=a.eml_cntid) when a.eml_entity='CDSL Client' Then (Select   IsNull(CdslClients_FirstHolderName,'')  as name from   Master_CdslClients  where  CdslClients_BOID=a.eml_cntid ) when a.eml_entity='NSDL Client' Then (select NsdlClients_BenFirstHolderName from Master_NsdlClients where cast(NsdlClients_DPID as nvarchar)+''+cast(NsdlClients_BenAccountID as nvarchar)=a.eml_cntid) when a.eml_entity='Lead' Then (Select  IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'')   as name   from   tbl_master_lead where cnt_internalId= a.eml_cntid ) else (Select IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'')  as name from tbl_master_contact where cnt_internalId=a.eml_cntid) end as Name ", "a.eml_email = b.eml_email and a.eml_email<>'' and b.eml_email<>''");
            LeadGrid.DataSource = dt.DefaultView;
            LeadGrid.DataBind();
        }

        protected void LeadGrid_CustomCallback1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LeadGrid.ClearSort();
            // showgrid();
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
        //---------------Phone Section-----------------------------------------
        private void ShowPhoneGrid()
        {
            DataTable dtPhone = new DataTable();
            dtPhone = oDBEngine.GetDataTable("tbl_master_phonefax  a, (select phf_phoneNumber, count(*) cnt from tbl_master_phonefax group by phf_phoneNumber  having count(*) > 1) b ", "a.phf_id,a.phf_entity, a.phf_phoneNumber, a.phf_cntId , case when a.phf_entity='contact' Then (Select IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'')+'['+isnull(cnt_UCC,'')+']'  as name from tbl_master_contact where cnt_internalId=a.phf_cntId) when a.phf_entity='CDSL Client' Then (Select   IsNull(CdslClients_FirstHolderName,'')  as name from   Master_CdslClients  where  CdslClients_BOID=a.phf_cntId ) when a.phf_entity='NSDL Client' Then  (select NsdlClients_BenFirstHolderName from Master_NsdlClients where cast(NsdlClients_DPID as nvarchar)+''+cast(NsdlClients_BenAccountID as nvarchar)=a.phf_cntId) when a.phf_entity='Lead' Then (Select  IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'')   as name   from   tbl_master_lead where cnt_internalId= a.phf_cntId ) else (Select IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'')  as name from tbl_master_contact where cnt_internalId=a.phf_cntId) end as Name ", "a.phf_phoneNumber = b.phf_phoneNumber and a.phf_phoneNumber<>'' and b.phf_phoneNumber<>'' and b.phf_phoneNumber<>',,' and b.phf_phoneNumber<>'                    ,                    ,                    '");
            PhoneGrid.DataSource = dtPhone.DefaultView;
            PhoneGrid.DataBind();
        }

        protected void PhoneGrid_CustomCallback1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            PhoneGrid.ClearSort();
            // showgrid();
            if (e.Parameters == "Sp")
                PhoneGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "Al")
            {
                PhoneGrid.FilterExpression = string.Empty;
            }
        }


        protected void PhoneGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }

        protected void cmbExportPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter1 = int.Parse(cmbExportPhone.SelectedItem.Value.ToString());
            switch (Filter1)
            {
                case 1:
                    Exporter1.WritePdfToResponse();
                    break;
                case 2:
                    Exporter1.WriteXlsToResponse();
                    break;
                case 3:
                    Exporter1.WriteRtfToResponse();
                    break;
                case 4:
                    Exporter1.WriteCsvToResponse();
                    break;
            }
        }
    }
}