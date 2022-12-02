using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class ApproverMaster : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            dsModuleList.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsGvDS.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grid.JSProperties["cpSave"] = null;
                
            if (e.Parameters.Split('~')[0] == "Save")
            {
                try
                {
                    string id = e.Parameters.Split('~')[1];
                    var selevctedids = grid.GetSelectedFieldValues("id");

                    string APPROVERS = string.Join(",", selevctedids).TrimStart(',');

                    DataTable ds = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("PRC_APPROVER");
                    proc.AddVarcharPara("@Action", 100, "SAVE");
                    proc.AddVarcharPara("@APPROVERS", -1, APPROVERS);
                    proc.AddVarcharPara("@ID", 10, id);
                    ds = proc.GetTable();
                    grid.JSProperties["cpSave"] = "Saved Successfully.";
                }
                catch (Exception ex)
                {
                    grid.JSProperties["cpSave"] = ex.InnerException.Message;
                }
            }
            else if (e.Parameters.Split('~')[0] == "ShowSelected")
            {
                string id = e.Parameters.Split('~')[1];
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_APPROVER");
                proc.AddVarcharPara("@Action", 100, "ShowSelected");
                proc.AddVarcharPara("@ID", 10, id);
                ds = proc.GetTable();
                grid.Selection.UnselectAll();
                foreach (DataRow dr in ds.Rows)
                {
                    grid.Selection.SetSelectionByKey(Convert.ToString(dr["APPROVER_ID"]),true);
                }
            }
        }
    }
}