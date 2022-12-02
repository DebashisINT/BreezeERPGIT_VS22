using DataAccessLayer; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
 

namespace ERP.OMS.Management.Master.UserControls
{
    public partial class GSTINSettings : System.Web.UI.UserControl
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        clsDropDownList oclsDropDownList = new clsDropDownList();

        #region property
        public string _contact_type;

        public string contact_type
        {
            get
            {
                return _contact_type;
            }
            set
            {
                _contact_type = value;
            }
        }
 


        #endregion




        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                bindDropDown();
            }
            loadGstinGrid();
        }

       public void bindDropDown()
       {
           DataTable CountryTable = oDBEngine.GetDataTable(" select cou_id,cou_country from tbl_master_country");
           cmbCountryGstin.DataSource = CountryTable;
           cmbCountryGstin.TextField = "cou_country";
           cmbCountryGstin.ValueField = "cou_id";
           cmbCountryGstin.DataBind();


           string[,] Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");

           oclsDropDownList.AddDataToDropDownList(Data, cmbgstinBranch);
        
        }

        #region GstinVendor
        public void loadGstinGrid()
        {
            if (Request.QueryString["id"] != "ADD")
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_vendorGstin");
                proc.AddVarcharPara("@action", 10, "select");
                proc.AddIntegerPara("@vendorId", Convert.ToInt32(Request.QueryString["id"]));
                dt = proc.GetTable();
                GstinGrid.DataSource = dt;
                GstinGrid.DataBind();
            }
        }

        protected void GstinGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string mode = e.Parameters.Split('~')[0];
            if (mode == "Add")
            {
                #region add
                int branch_id = Convert.ToInt32(cmbgstinBranch.SelectedValue);
                int country_id = Convert.ToInt32(cmbCountryGstin.Value);
                int state_id = Convert.ToInt32(cmbStateGstin.Value);
                string gstin = gstinpopup1.Text + gstinpopup2.Text + gstinpopup3.Text;
                ProcedureExecute proc = new ProcedureExecute("prc_vendorGstin");
                proc.AddVarcharPara("@action", 10, "Add");
                proc.AddIntegerPara("@vendorId", Convert.ToInt32(Request.QueryString["Id"]));
                proc.AddIntegerPara("@branch_id", branch_id);
                proc.AddIntegerPara("@country_id", country_id);
                proc.AddIntegerPara("@state_id", state_id);
                proc.AddVarcharPara("@GSTIN", 15, gstin);
                proc.AddVarcharPara("@contact_type", 10, _contact_type);
                DataTable dt = proc.GetTable();

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) == "Exists")
                    {
                        GstinGrid.JSProperties["cpBranchExists"] = "True";
                    }
                    else
                    {
                        loadGstinGrid();
                        GstinGrid.JSProperties["cpInsert"] = "True";
                    }

                }

                #endregion
            }
            else if (mode == "Delete")
            {
                #region delete
                int vendor_gstinId = Convert.ToInt32(e.Parameters.Split('~')[1]);
                ProcedureExecute proc = new ProcedureExecute("prc_vendorGstin");
                proc.AddVarcharPara("@action", 10, "Delete");
                proc.AddIntegerPara("@vendor_gstinId", vendor_gstinId);

                proc.GetTable();
                loadGstinGrid();
                GstinGrid.JSProperties["cpDelete"] = "True";
                #endregion
            }
            else if (mode == "Edit")
            {
                #region update
                int vendor_gstinId = Convert.ToInt32(e.Parameters.Split('~')[1]);
                int branch_id = Convert.ToInt32(cmbgstinBranch.SelectedValue);
                int country_id = Convert.ToInt32(cmbCountryGstin.Value);
                int state_id = Convert.ToInt32(cmbStateGstin.Value);
                string gstin = gstinpopup1.Text + gstinpopup2.Text + gstinpopup3.Text;
                ProcedureExecute proc = new ProcedureExecute("prc_vendorGstin");
                proc.AddVarcharPara("@action", 10, "Edit");
                proc.AddIntegerPara("@branch_id", branch_id);
                proc.AddIntegerPara("@country_id", country_id);
                proc.AddIntegerPara("@state_id", state_id);
                proc.AddVarcharPara("@GSTIN", 15, gstin);
                proc.AddIntegerPara("@vendor_gstinId", vendor_gstinId);
                proc.GetTable();
                loadGstinGrid();

                GstinGrid.JSProperties["cpUpdate"] = "True";

                #endregion
            }
        }


        protected void GstinPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            int EditGstinId = Convert.ToInt32(e.Parameter);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_vendorGstin");
            proc.AddVarcharPara("@action", 10, "GetData");
            proc.AddIntegerPara("@vendor_gstinId", EditGstinId);

            dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {

                cmbgstinBranch.SelectedValue = Convert.ToString(dt.Rows[0]["branch_id"]);
                cmbCountryGstin.Value = Convert.ToString(dt.Rows[0]["country_id"]).Trim();
                bindState(Convert.ToString(dt.Rows[0]["country_id"]).Trim());
                cmbStateGstin.Value = Convert.ToString(dt.Rows[0]["state_id"]).Trim();
                string GSTIN = Convert.ToString(dt.Rows[0]["GSTIN"]).Trim();

                gstinpopup1.Text = GSTIN.Substring(0, 2);
                gstinpopup2.Text = GSTIN.Substring(2, 10);
                gstinpopup3.Text = GSTIN.Substring(12, 3);



            }

        }

        public void bindState(string countryId)
        {
            DataTable stateTable = oDBEngine.GetDataTable("select id,state from tbl_master_state where countryId=" + countryId);
            cmbStateGstin.DataSource = stateTable;
            cmbStateGstin.TextField = "state";
            cmbStateGstin.ValueField = "id";
            cmbStateGstin.DataBind();

        }

        #endregion

        protected void cmbStateGstin_Callback(object sender, CallbackEventArgsBase e)
        {
            string countryId = e.Parameter;
            bindState(countryId);
             
        }

    }
}