using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EntityLayer.CommonELS;
using System.IO;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Financer : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Financer.aspx");
            if (!IsPostBack)
            {

            }
            fillGrid();

        }
        public void fillGrid()
        {
            //     gridFinancerDataSource.SelectCommand = "select h.cnt_id,h.cnt_ucc,h.cnt_firstName,(select branch_description from tbl_master_branch d where d.branch_id=h.cnt_branchId) as branch,* from tbl_master_contact h where cnt_contactType='FI'";
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("select h.cnt_id,h.cnt_ucc,h.cnt_firstName,(select branch_description from tbl_master_branch d where d.branch_id=h.cnt_branchId) as branch,* from tbl_master_contact h where cnt_contactType='FI'");
            if (DT != null && DT.Rows.Count > 0)
            {
                gridFinancer.DataSource = DT;
                gridFinancer.DataBind();
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

        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
                gridFinancer.JSProperties["cpDelmsg"] = null;
                if (CallVal[0] == "Delete")
                {
                    string[,] ContactData;
                    ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstname", "cnt_id=" + CallVal[1], 1);
                    if (ContactData[0, 0] != "n")
                    {
                        int i = delMasterData.DeleteFinancer(ContactData[0, 0]);
                        gridFinancer.JSProperties["cpDelmsg"] = "Deleted Successfully.";
                        fillGrid();
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void AspxExecutiveGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            //DataTable dt = oDBEngine.GetDataTable("select ExecutiveName,ExecutiveuserId,ExecutivePassword from tbl_master_FinancerExecutive where Fin_InternalId=( select cnt_internalId from tbl_master_contact  where cnt_id=" + Convert.ToString(e.Parameters) + ")");
            //AspxExecutiveGrid.DataSource = dt;
            //AspxExecutiveGrid.DataBind();
        }


        public void bindexport(int Filter)
        {
            //gridFinancer.Columns[3].Visible = false;
            string filename = "Financer";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Financer";
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

    }
}