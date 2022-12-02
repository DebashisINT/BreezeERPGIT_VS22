using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_ConvertedLead : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";

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
            if (HttpContext.Current.Session["userid"] == null)
            {
               // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                TxtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                TxtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                TxtFromDate.Value = oDBEngine.GetDate();
                TxtToDate.Value = oDBEngine.GetDate();
                TdExport.Visible = false;
            }
           // //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (RBReportType.SelectedItem.Value.ToString() == "Screen")
            {
                ShowTreeList();
            }
            else
            {
                showCrystalReport();
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = Int32.Parse(cmbExport.SelectedItem.Value.ToString());
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
        public void ShowTreeList()
        {
            DataTable DtConverted = oDBEngine.GetDataTable("tbl_master_contact", "(cnt_firstname+' '+cnt_middlename+' '+cnt_lastname +'['+cnt_shortname+']') as name,cnt_internalid,(select count(cnt_referedBy) from tbl_master_lead where cnt_referedBy=tbl_master_contact.cnt_internalid and createdate between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as Counmt, (select count(cnt_Lead_Stage) from tbl_master_lead where cnt_referedBy=tbl_master_contact.cnt_internalid and cnt_Lead_Stage in(2,3) and createdate between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as Used,(select count(cnt_Lead_Stage) from tbl_master_lead where cnt_referedBy=tbl_master_contact.cnt_internalid and cnt_Lead_Stage='4' and createdate between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as Converted,(select count(cnt_Lead_Stage) from tbl_master_lead where cnt_referedBy=tbl_master_contact.cnt_internalid and cnt_status='due' and createdate between '" + TxtFromDate.Value.ToString() + "' and '" + TxtToDate.Value.ToString() + "') as NonUsable", "cnt_internalid like 'dv%' ");
            GridConvertedLead.DataSource = DtConverted.DefaultView;
            GridConvertedLead.DataBind();
            TdExport.Visible = true;
        }
        public void showCrystalReport()
        {

        }

    }
}