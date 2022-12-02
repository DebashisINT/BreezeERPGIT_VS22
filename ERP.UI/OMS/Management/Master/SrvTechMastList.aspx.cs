using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class SrvTechMastList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.MasterDataCheckingBL delMasterData = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        SrvTechMastBL srvBL = new SrvTechMastBL();

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
          
            if (Session["ViewMode"] != null)
            {
                if (Session["ViewMode"].ToString() == "view")
                {
                    HttpContext.Current.Session["UserRightSession/management/Master/SrvTechMastList.aspx"] = null;
                    Session["ViewMode"] = "";
                }
            }
            

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/SrvTechMastList.aspx");
            if (!IsPostBack)
            {
                Session["ContactType"] = "TM";
            }
            fillGrid();
        }

        public void fillGrid()
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            //DataTable DT = objEngine.GetDataTable("select h.cnt_id,h.cnt_ucc,h.cnt_firstName,(select branch_description from tbl_master_branch d where d.branch_id=h.cnt_branchId) as branch,* from tbl_master_contact h where cnt_contactType='FI'");
            //if (DT != null && DT.Rows.Count > 0)
            //{
            //    gridFinancer.DataSource = DT;
            //    gridFinancer.DataBind();
            //}

            ProcedureExecute proc = new ProcedureExecute("PRC_FetchTechnician");
            proc.AddVarcharPara("@Action", 500, "TechnicianReport");
            proc.AddIntegerPara("@USER_ID", Convert.ToInt32(Session["userid"]));
            DataTable dt = proc.GetTable();
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
                        int i = srvBL.DeleteTechnicianMaster(ContactData[0, 0]);
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
            string filename = "Technician";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Technician";
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

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.Technician_Reports
                    where d.LOGIN_ID == userid
                    orderby d.SEQ
                    select d;
            e.QueryableSource = q;
        }
    }
}