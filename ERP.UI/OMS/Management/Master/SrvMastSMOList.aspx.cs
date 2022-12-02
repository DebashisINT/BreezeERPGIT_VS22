using BusinessLogicLayer;
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
    public partial class SrvMastSMOList : System.Web.UI.Page
    {
        CommonBL Entitycb = new CommonBL();
        public bool IsImport = false;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.MasterDataCheckingBL delMasterData = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        Srv_MastSMOBL srvBL = new Srv_MastSMOBL();

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
                    HttpContext.Current.Session["UserRightSession/management/Master/SrvMastSMOList.aspx"] = null;
                    Session["ViewMode"] = "";
                }
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/SrvMastSMOList.aspx");
            if (!IsPostBack)
            {
                string EntityImportInEntityMaster = Entitycb.GetSystemSettingsResult("EntityImportInEntityMaster");
                if (EntityImportInEntityMaster.ToUpper() == "YES")
                {
                    IsImport = true;
                }

                Session["ContactType"] = "MSO";

              
            }
            fillGrid();
        }


        public void fillGrid()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_FetchMSO");
            proc.AddVarcharPara("@Action", 500, "MSOReport");
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
                gridFinancer.JSProperties["cpImportModel"] = null;
                if (CallVal[0] == "Delete")
                {
                    string[,] ContactData;
                    ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstname", "cnt_id=" + CallVal[1], 1);
                    if (ContactData[0, 0] != "n")
                    {
                        int i = srvBL.DeleteMSOMaster(ContactData[0, 0]);
                        gridFinancer.JSProperties["cpDelmsg"] = "Deleted Successfully.";
                        fillGrid();
                    }
                }
                else if (CallVal[0] == "ImportModel")
                {
                    string ComanyDbName = Convert.ToString(CallVal[1]);
                   
                    int i = srvBL.ImportEntitylMaster(Convert.ToString(ComanyDbName));
                    gridFinancer.JSProperties["cpImportModel"] = "Success";
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
            string filename = "MSO";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "MSO";
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

        protected void MSOServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.MSO_Reports
                    where d.LOGIN_ID == userid
                    orderby d.SEQ
                    select d;
            e.QueryableSource = q;
        }
    }
}