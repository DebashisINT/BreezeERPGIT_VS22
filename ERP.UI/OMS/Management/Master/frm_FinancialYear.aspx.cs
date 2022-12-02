using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_FinancialYear : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        ProcedureExecute objProcedureExecute = new ProcedureExecute();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frm_FinancialYear.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                ////Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            // //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            gridStatusDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if(!IsPostBack)
            {
                Session["exportval"] = null;
            }

            fillGrid();
        }
        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string tranid = e.Parameters.ToString();

            if (e.Parameters.ToString().Split('~')[0] == "Delete")
            {
                gridStatus.Settings.ShowFilterRow = true;
            }
            //else if (e.Parameters == "All")
            //{
            //    gridStatus.FilterExpression = string.Empty;
            //}

            if (e.Parameters == "All")
            {
                gridStatus.FilterExpression = string.Empty;
            }

            fillGrid();

        }
        public void fillGrid()
        {
            gridStatusDataSource.SelectCommand = "select FinYear_ID,FinYear_Code,convert(varchar,FinYear_StartDate,106) as FinYear_StartDate,convert(varchar,FinYear_EndDate,106) as FinYear_EndDate,FinYear_Remarks from master_finyear";
            gridStatus.DataBind();

        }

        public void DeleteFinancialYear()
        {

        }

        #region Export event

        public void bindexport(int Filter)
        {
            gridStatus.Columns[5].Visible = false;
            //SchemaGrid.Columns[11].Visible = false;
            //SchemaGrid.Columns[12].Visible = false;
            string filename = "Financial Year";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Financial Year";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

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
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        #endregion

        
    }
}
