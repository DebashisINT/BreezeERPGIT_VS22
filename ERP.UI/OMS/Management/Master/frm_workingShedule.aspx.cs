using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Data;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_workingShedule : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        int NoOfRow = 0;
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            WorkingHourDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_workingShedule.aspx");
            
            if (HttpContext.Current.Session["userid"] == null)
            {
              
            }
            FillGrid();
            Session["KeyVal"] = null;          
            
        }
        public void FillGrid()
        {
            WorkingHourDataSource.SelectCommand = " select Id,Name from tbl_EmpWorkingHours";
            WorkingHourGrid.DataBind();
        }
        protected void EmployeeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            WorkingHourGrid.ClearSort();
            WorkingHourGrid.DataBind();
            int deletecnt = 0;
            string WhichType = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            
               
            if (WhichCall == "Delete")
            {
                if (Convert.ToString(e.Parameters).Contains("~"))
                {
                    if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    {
                        WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                    }
                }
               
                deletecnt = DeleteWorkingHour(WhichType);
                if (deletecnt == 1)
                {
                    FillGrid();
                    WorkingHourGrid.JSProperties["cpDelete"] = "Deleted successfully";                    
                }
                else
                {
                    WorkingHourGrid.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
        }
        public int DeleteWorkingHour(string Id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_WorkingHourAddEdit");
            proc.AddVarcharPara("@action", 100, "Delete");
            proc.AddIntegerPara("@Id", Convert.ToInt16(Id));            
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
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

       
        MasterSettings masterbl = new MasterSettings();
        public bool STBBranchMap { get; set; }

        [WebMethod]
        public static List<ModuleList> GetModuleList(string RosterId)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            List<ModuleList> omodel = new List<ModuleList>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_ModuleRosterMAP");
            proc.AddPara("@RosterId", RosterId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                omodel = UtilityLayer.APIHelperMethods.ToModelList<ModuleList>(dt);
            }
            return omodel;
        }

        [WebMethod]
        public static bool GetModuleListSubmit(string RosterId, List<string> Modulelist)
        {
            Employee_BL objEmploye = new Employee_BL();
            string ModuleId = "";
            int i = 1;

            if (Modulelist != null && Modulelist.Count > 0)
            {
                foreach (string item in Modulelist)
                {
                    if (item == "0")
                    {
                        ModuleId = "0";
                        break;
                    }
                    else
                    {
                        if (i > 1)
                            ModuleId = ModuleId + "," + item;
                        else
                            ModuleId = item;
                        i++;
                    }
                }

            }

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBmoduleRosterMapInsertUpdate");
            proc.AddPara("@RosterId", RosterId);
            proc.AddPara("@ModuleId", ModuleId);
            proc.AddPara("@User_id", Convert.ToString(HttpContext.Current.Session["userid"]));
            dt = proc.GetTable();

            return true;
        }

        public class ModuleList
        {
            public long ModuleId { get; set; }
            public String ModuleName { get; set; }
            public bool IsChecked { get; set; }
            public string status { get; set; }
        }

        /*-------------------Tanmoy------------------*/
    }
}