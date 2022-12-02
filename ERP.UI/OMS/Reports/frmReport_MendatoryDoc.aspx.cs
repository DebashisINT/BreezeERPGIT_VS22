using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_MendatoryDoc : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataTable dt = new DataTable();
        DataTable dttemp = new DataTable();
        String CName = null;
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
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                HttpContext.Current.Session["KeyVal"] = "n";
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                //______________________________End Script____________________________//
                FillContactType();
                FillDocumentType("All");
                cmbDocType.SelectedIndex = 0;
                cmbContactType.SelectedIndex = 0;

            }
            if (IsCallback)
            {
                if (HttpContext.Current.Session["KeyVal"] == "y")
                    BindResultGrid();
            }

        }
        public void FillContactType()
        {
            dt = null;
            dt = oDBEngine.GetDataTable("tbl_master_contactType order by cnttpy_contactType", "cnttpy_id,cnttpy_contactType,cnt_prefix", null);
            cmbContactType.Items.Add("All", 0);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbContactType.Items.Add(dt.Rows[i]["cnttpy_contactType"].ToString(), dt.Rows[i]["cnttpy_contactType"].ToString());
            }
            cmbContactType.SelectedIndex = 0;
        }

        public void FillDocumentType(string value)
        {
            cmbDocType.Items.Clear();
            string whereC = "dty_mandatory =1";
            if (value != "All" && value != "0")
            {
                whereC = whereC + "and RTRIM(dty_applicableFor)='" + value + "'";
            }
            dt = null;
            dt = oDBEngine.GetDataTable("tbl_master_documentType", "distinct dty_documentType", whereC);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbDocType.Items.Add(dt.Rows[i]["dty_documentType"].ToString(), dt.Rows[i]["dty_documentType"].ToString());
                }
            }
            cmbDocType.SelectedIndex = 0;
        }
        protected void cmbDocType_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            FillDocumentType(e.Parameter);
        }
        public void BindResultGrid()
        {
            dt = null;
            string documentId = "0";
            if (rbCategory.SelectedItem.Value.ToString() == "S")
            {
                if (cmbDocType.SelectedItem != null)
                    documentId = cmbDocType.Value.ToString();
            }
            string[,] Data = { { "@contacttype", SqlDbType.VarChar.ToString(), cmbContactType.SelectedItem.Text }, { "@documentTyp", SqlDbType.VarChar.ToString(), documentId } };
            dt = oDBEngine.GetDatatable_StoredProcedure("SP_MendetoryDocumentsStatus", Data);
            AspXMendatoryDocGrid.DataSource = dt.DefaultView;
            AspXMendatoryDocGrid.DataBind();
        }
        protected void AspXMendatoryDocGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            HttpContext.Current.Session["KeyVal"] = "y";
            if (e.Parameters == "s")
                AspXMendatoryDocGrid.Settings.ShowFilterRow = true;
            else if (e.Parameters == "All")
            {
                AspXMendatoryDocGrid.FilterExpression = string.Empty;
                AspXMendatoryDocGrid.ClearSort();
            }
            BindResultGrid();
        }
        protected void AspXMendatoryDocGrid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            CName = e.GetValue("Name").ToString();
            if (CName == "")
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F7F3E5");

            }
            else
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4D4FF");
            }
        }

        protected void AspXMendatoryDocGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "4";
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