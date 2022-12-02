using System;
using System.Data;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_RegionwiseBranch : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        DataTable DT_treeview = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
           // //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            this.Page.Title = "Influx Regionwise Branch Distribution Report";
            PopulateTreeList();
        }
        public void PopulateTreeList()
        {
            //___________DataTable preparation________//
            DataColumn DC_Acid = new DataColumn("ACTID");
            DataColumn DC_id = new DataColumn("ID");
            DataColumn DC_parentID = new DataColumn("ParentID");
            DataColumn DC_Name = new DataColumn("Name");
            DT_treeview.Columns.Add(DC_Acid);
            DT_treeview.Columns.Add(DC_id);
            DT_treeview.Columns.Add(DC_parentID);
            DT_treeview.Columns.Add(DC_Name);
            //____________

            DataTable DT = new DataTable();
            DT = oDBEngine.GetDatatable_StoredProcedure("SP_GETRegionWiseBranchDetails", null);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DataRow DR1 = DT_treeview.NewRow();
                DR1["ACTID"] = DT.Rows[i]["ActId"].ToString();
                DR1["ID"] = DT.Rows[i]["ID"].ToString();
                DR1["ParentID"] = DT.Rows[i]["ParentId"].ToString();
                if (DT.Rows[i]["CN"].ToString() == "0")
                {
                    DR1["Name"] = DT.Rows[i]["Description"].ToString();
                }
                else
                {
                    DR1["Name"] = DT.Rows[i]["Description"].ToString() + '[' + DT.Rows[i]["CN"].ToString() + ']';
                }
                DT_treeview.Rows.Add(DR1);
            }
            TVRegionBranchHir.DataSource = DT_treeview.DefaultView;
            TVRegionBranchHir.DataBind();
        }
    }
}