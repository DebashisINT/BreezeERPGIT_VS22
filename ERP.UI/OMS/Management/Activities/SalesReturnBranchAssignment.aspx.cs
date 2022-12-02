using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;

namespace ERP.OMS.Management.Activities
{
    public partial class SalesReturnBranchAssignment : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        SalesReturnBranchAssignmentBl SalesRetBranchAssign = new SalesReturnBranchAssignmentBl();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateBranch();
                BindSalesReturnGrid();
            }
        }

        protected void BindSalesReturnGrid() 
        {
            DataTable SalesReyurnListforAssignment = SalesRetBranchAssign.GetSalesReturnForBranchAssignment(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userbranchID"]));
          
            Session["grid_salesReturnAssignment"] = SalesReyurnListforAssignment;
            grid_salesReturnAssignment.DataBind();
        }
        protected void grid_salesReturnAssignment_OnDataBinding(object sender, EventArgs e)
        {
            DataTable SalesReyurnListforAssignment = (DataTable)Session["grid_salesReturnAssignment"];
            grid_salesReturnAssignment.DataSource = SalesReyurnListforAssignment;
        }

        protected void BranchTransferCallBackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string receivedString = e.Parameter;
            BranchTransferCallBackPanel.JSProperties["cpReceivedString"] = receivedString.Split('~')[0];

            if (receivedString.Split('~')[0] == "PopulateAssignBranch") 
            {
                int Id = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataTable dt = SalesRetBranchAssign.GetAssignBranchdata(Id);
                if(dt != null)
                {
                    txtNarration.Text =Convert.ToString(dt.Rows[0][0]);
                    ddlBranch.Value = Convert.ToString(dt.Rows[0][1]);
                }
            }

            else if (receivedString.Split('~')[0] == "SaveData")
            {
                int Id = Convert.ToInt32(e.Parameter.Split('~')[1]);
                SalesRetBranchAssign.UpdateAssignBranch(Convert.ToInt32(ddlBranch.Value), txtNarration.Text, Id);
                BranchTransferCallBackPanel.JSProperties["cpSave"] = "yes";
            }

        }

        protected void PopulateBranch() 
        {
            ddlBranch.DataSource = SalesRetBranchAssign.getBranchListByBranchList(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(Session["userbranchID"]));
            ddlBranch.ValueField = "branch_id";
            ddlBranch.TextField = "branch_description";
            ddlBranch.DataBind();
            ddlBranch.Value = "0";
        
        }
    }
}