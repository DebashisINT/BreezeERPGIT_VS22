using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace ERP.OMS.Reports
{
    public partial class GSTReturn : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            if (!IsPostBack)
            {
                bindDropDown();
            }
        }

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            DataSet GSTDS = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_FETCH_GSTRETURN");
            proc.AddVarcharPara("@GSTIN", 15, Convert.ToString(cmbGstinlist.Value));
            //proc.AddVarcharPara("@Month", 15, Convert.ToString(ccmbMonth.Value).Trim());
            proc.AddVarcharPara("@FromDate", 10, FormDate.Date.ToString("yyyy-MM-dd"));
            proc.AddVarcharPara("@ToDate", 10, toDate.Date.ToString("yyyy-MM-dd"));
            proc.AddBooleanPara("@byPartyInvoiceDate", !chkByPartyinvdate.Checked);

            GSTDS = proc.GetDataSet();
            Session["GSTGrid3_1"] = GSTDS.Tables[0];
            Session["GSTGrid3_2"] = GSTDS.Tables[1]; 
            Session["DetailITC"]= GSTDS.Tables[2];
            GSTGrid3_1.DataBind();
            GSTGrid3_2.DataBind();
            DetailITC.DataBind();

        }
        protected void cGSTGrid3_1_DataBinding(object sender, EventArgs e)
        {
            DataTable GSTGrid3_1DT = (DataTable)Session["GSTGrid3_1"];
            if (GSTGrid3_1DT != null)
            {
                GSTGrid3_1.DataSource = GSTGrid3_1DT;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            bindexport(Filter);
        }

        public void bindexport(int Filter)
        { 
            string filename = "GST Return 3.1";
            exporter3_1.FileName = filename;
            exporter3_1.GridViewID = "GSTGrid3_1";
            exporter3_1.PageHeader.Left = "GST Return 3.1";
            exporter3_1.PageFooter.Center = "[Page # of Pages #]";
            exporter3_1.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter3_1.WritePdfToResponse();
                    break;
                case 2:
                    exporter3_1.WriteXlsToResponse();
                    break;
                case 3:
                    exporter3_1.WriteRtfToResponse();
                    break;
                case 4:
                    exporter3_1.WriteCsvToResponse();
                    break;
            }
        }


        protected void cmbExport_SelectedIndexChanged3_2(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(DropDownList1.SelectedItem.Value));
            bindexport3_2(Filter);
        }

        public void bindexport3_2(int Filter)
        {
            string filename = "GST Return 3.2";
            exporter3_2.FileName = filename;
            exporter3_2.GridViewID = "GSTGrid3_2";
            exporter3_2.PageHeader.Left = "GST Return 3.2";
            exporter3_2.PageFooter.Center = "[Page # of Pages #]";
            exporter3_2.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter3_2.WritePdfToResponse();
                    break;
                case 2:
                    exporter3_2.WriteXlsToResponse();
                    break;
                case 3:
                    exporter3_2.WriteRtfToResponse();
                    break;
                case 4:
                    exporter3_2.WriteCsvToResponse();
                    break;
            }
        }


        protected void cGSTGrid3_2_DataBinding(object sender, EventArgs e)
        {
            DataTable GSTGrid3_2DT = (DataTable)Session["GSTGrid3_2"];
            if (GSTGrid3_2DT != null)
            {
                GSTGrid3_2.DataSource = GSTGrid3_2DT;
            }
        }



        protected void DetailITC_SelectedIndexChanged3_2(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(cmbDetailITC.SelectedItem.Value));
            bindexportDetailITC(Filter);
        }

        public void bindexportDetailITC(int Filter)
        {
            string filename = "Input Tax Credit";
            exporter3_2.FileName = filename;
            exporter3_2.GridViewID = "DetailITC";
            exporter3_2.PageHeader.Left = "GST Return 3.2";
            exporter3_2.PageFooter.Center = "[Page # of Pages #]";
            exporter3_2.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter3_2.WritePdfToResponse();
                    break;
                case 2:
                    exporter3_2.WriteXlsToResponse();
                    break;
                case 3:
                    exporter3_2.WriteRtfToResponse();
                    break;
                case 4:
                    exporter3_2.WriteCsvToResponse();
                    break;
            }
        }


        protected void DetailITC_DataBinding(object sender, EventArgs e)
        {
            DataTable DetailITC_2DT = (DataTable)Session["DetailITC"];
            if (DetailITC_2DT != null)
            {
                DetailITC.DataSource = DetailITC_2DT;
            }
        }
        public void bindDropDown()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_gst_return_details");
            proc.AddVarcharPara("@Action", 50, "GSTINLIST");
            DataTable gstinTable = proc.GetTable();

            cmbGstinlist.DataSource = gstinTable;
            cmbGstinlist.ValueField = "GSTIN";
            cmbGstinlist.TextField = "GSTIN";
            cmbGstinlist.DataBind();

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;
            
            //ccmbMonth.Items.Add("Apr", 4);
            //ccmbMonth.Items.Add("May", 5);
            //ccmbMonth.Items.Add("Jun", 6);
            //ccmbMonth.Items.Add("Jul", 7);
            //ccmbMonth.Items.Add("Aug", 8);
            //ccmbMonth.Items.Add("Spt", 9);
            //ccmbMonth.Items.Add("Oct", 10);
            //ccmbMonth.Items.Add("Nov", 11);
            //ccmbMonth.Items.Add("Dec", 12);
            //ccmbMonth.Items.Add("Jan", 1);
            //ccmbMonth.Items.Add("Feb", 2);
            //ccmbMonth.Items.Add("Mar", 3);
            //ccmbMonth.SelectedIndex = 0;


        }
    }
}