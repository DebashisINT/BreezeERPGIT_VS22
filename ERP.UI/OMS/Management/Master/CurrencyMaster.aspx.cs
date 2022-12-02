using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EntityLayer.CommonELS;
using System.IO;
using BusinessLogicLayer;


namespace ERP.OMS.Management.Master
{

    public partial class CurrencyMaster : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.MasterDataCheckingBL delMasterData = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Init(object sender, EventArgs e)
        {

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/CurrencyMaster.aspx");
            gridFinancerDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (!IsPostBack)
            {
                fillGrid();
            }
        }
        public void fillGrid()
        {
            //     gridFinancerDataSource.SelectCommand = "select h.cnt_id,h.cnt_ucc,h.cnt_firstName,(select branch_description from tbl_master_branch d where d.branch_id=h.cnt_branchId) as branch,* from tbl_master_contact h where cnt_contactType='FI'";
            gridFinancer.DataBind();

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

        public void bindexport(int Filter)
        {
            gridFinancer.Columns[7].Visible = false;
            string filename = "Currency Master";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Currency Master";
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
        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
                gridFinancer.JSProperties["cpDelmsg"] = null;
                if (CallVal[0] == "Delete")
                {
                    int i = 0;
                    MasterDataCheckingBL obMasterDataCheckingBL = new MasterDataCheckingBL();
                    i = obMasterDataCheckingBL.DeleteCurrency(Convert.ToString(CallVal[1]));
                    if (i == 1)
                    {
                        gridFinancer.JSProperties["cpDelmsg"] = "Succesfully Deleted.";
                        fillGrid();
                    }
                    else
                    {
                        gridFinancer.JSProperties["cpDelmsg"] = "Used in other module. Can not delete.";
                    }
                //oDBEngine.DeleteValue("tbl_Master_CurrencyRateDateWise ", "CRID ='" + CallVal[1].ToString() + "' ");
                //gridFinancer.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                //fillGrid();
                }

            }
            catch (Exception ex)
            {

            }

        }

    }
}