﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Data;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using DataAccessLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class CustomerCrNoteAdjustDrNote : System.Web.UI.Page
    {

        CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();
        DataTable TempTable = new DataTable();
        string adjustmentNumber = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        int adjustmentId = 0, ErrorCode = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/CustomerCrNoteAdjustDrNoteList.aspx");
                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
                {
                    if (ProjectSelectInEntryModule == "Yes")
                    {
                        //Divproject.Visible = true;
                        Divproject.Style.Add("Display", "block");
                        hdnProjectSelectInEntryModule.Value = "1";
                    }
                    else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                       // Divproject.Visible = false;
                        Divproject.Style.Add("Display", "none");
                        hdnProjectSelectInEntryModule.Value = "0";
                    }
                }
                string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
                if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
                {
                    if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                    {
                        //DivHierarchy.Visible = true;
                        DivHierarchy.Style.Add("Display", "Block");

                    }
                    else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                        //DivHierarchy.Visible = false;
                        DivHierarchy.Style.Add("Display", "none");
                    }
                }
                if (Request.QueryString["Key"] != "Add")
                {
                    string AdjId = Request.QueryString["Key"];
                    EditModeExecute(AdjId);
                    hdAddEdit.Value = "Edit";
                    lblHeading.Text = "Modify Adjustment of Documents - Credit Note With Debit Note";
                    hdAdjustmentId.Value = AdjId;
                    //btnSaveRecords.Visible = false;
                }                   
                else
                {
                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=58");

                    if (dtposTime != null && dtposTime.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                    }
                    hdAddEdit.Value = "Add";
                    AddmodeExecuted();

                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Adjustment of Documents - Credit Note With Invoice";
                    btn_SaveRecords.Visible = false;
                    btnSaveRecords.Visible = false;
                }  
            } 
        }
        private void AddmodeExecuted()
        {

            DataSet allDetails = blLayer.PopulateCrNoteAdjustmentDrNoteDetails();
            CmbScheme.DataSource = allDetails.Tables[0];
            CmbScheme.ValueField = "Id";
            CmbScheme.TextField = "SchemaName";
            CmbScheme.DataBind();

            ddlBranch.DataSource = allDetails.Tables[1];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

            DateTime startDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_StartDate"]);
            DateTime LastDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_EndDate"]);
            dtTDate.MaxDate = LastDate;
            dtTDate.MinDate = startDate;

            if (DateTime.Now > LastDate)
                dtTDate.Date = LastDate;
            else
                dtTDate.Date = DateTime.Now;

            if (Session["schemavalCrDrNoteAdj"] != null)
            {
                List<string> SaveNewValues = (Session["schemavalCrDrNoteAdj"]) as List<string>;
                CmbScheme.Value = SaveNewValues[0];
                CmbScheme.Text = SaveNewValues[1];
                ddlBranch.SelectedValue = SaveNewValues[2];
                ddlBranch.SelectedItem.Text = SaveNewValues[3];
                ddlBranch.Enabled = false;
                hdnCustomerId.Value = SaveNewValues[4];
                txtCustName.Text = SaveNewValues[5];

            }
            if (Convert.ToString(Session["SaveModeCrDrNoteAdj"]) == "A")
            {
                dtTDate.Focus();
                txtVoucherNo.ClientEnabled = false;
                txtVoucherNo.Text = "Auto";
            }
            else if (Convert.ToString(Session["SaveModeCrDrNoteAdj"]) == "M")
            {
                txtVoucherNo.ClientEnabled = true;
                txtVoucherNo.Text = "";
                txtVoucherNo.Focus();

            }
           
        }


        public void EditModeExecute(string id)
        {
            DataSet EditedDataDetails = blLayer.GetEditedDataCrNOteWithDrNote(id);
            CmbScheme.DataSource = EditedDataDetails.Tables[0];
            CmbScheme.ValueField = "Id";
            CmbScheme.TextField = "SchemaName";
            CmbScheme.DataBind();

            ddlBranch.DataSource = EditedDataDetails.Tables[1];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

            DateTime startDate = Convert.ToDateTime(EditedDataDetails.Tables[2].Rows[0]["FinYear_StartDate"]);
            DateTime LastDate = Convert.ToDateTime(EditedDataDetails.Tables[2].Rows[0]["FinYear_EndDate"]);
            dtTDate.MaxDate = LastDate;
            dtTDate.MinDate = startDate;

            DataRow HeaderRow = EditedDataDetails.Tables[3].Rows[0];
            CmbScheme.Text = Convert.ToString(HeaderRow["SchemaName"]);
            CmbScheme.ClientEnabled = false;
            txtVoucherNo.Text = Convert.ToString(HeaderRow["Adjustment_No"]);
            dtTDate.Date = Convert.ToDateTime(HeaderRow["Adjustment_Date"]);
            dtTDate.ClientEnabled = false;
            ddlBranch.SelectedValue = Convert.ToString(HeaderRow["Branch"]);
            ddlBranch.Enabled = false;
            txtCustName.Text = Convert.ToString(HeaderRow["CustName"]);
            txtCustName.ClientEnabled = false;
            hdnCustomerId.Value = Convert.ToString(HeaderRow["Customer_id"]);
            btntxtDocNo.Text = Convert.ToString(HeaderRow["Adjusted_Doc_no"]);
            btntxtDocNo.ClientEnabled = false;
            hdAdvanceDocNo.Value = Convert.ToString(HeaderRow["Adjusted_doc_id"]);
            DocAmt.Text = Convert.ToString(HeaderRow["Adjusted_DocAmt"]);
            ExchRate.Text = Convert.ToString(HeaderRow["ExchangeRate"]);
            BaseAmt.Text = Convert.ToString(HeaderRow["Adjusted_DocAmt_inBaseCur"]);
            Remarks.Text = Convert.ToString(HeaderRow["Remarks"]);
            OsAmt.Text = Convert.ToString(HeaderRow["Adjusted_DocOSAmt"]);
            AdjAmt.Text = Convert.ToString(HeaderRow["Adjusted_Amount"]);
            hdCrNoteType.Value = Convert.ToString(HeaderRow["CrNoteType"]);
            TempTable = EditedDataDetails.Tables[4];

            txtProject.Text = Convert.ToString(HeaderRow["Proj_Code"]);
            hddnProjectId.Value = Convert.ToString(HeaderRow["Proj_Id"]);
            txtHierarchy.Text = Convert.ToString(HeaderRow["HIERARCHY_NAME"]);

            // grid.DataSource = EditedDataDetails.Tables[4];
            grid.DataBind();
        }


        #region Grid Event
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName != "AdjAmt")
            {
                e.Editor.Enabled = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }
        }


        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable AdjustmentTable = new DataTable();
            AdjustmentTable.Columns.Add("DocNo", typeof(string));
            AdjustmentTable.Columns.Add("DocAmt", typeof(decimal));
            AdjustmentTable.Columns.Add("Currency", typeof(string));
            AdjustmentTable.Columns.Add("ExchangeRate", typeof(string));
            AdjustmentTable.Columns.Add("LocalAmt", typeof(decimal));
            AdjustmentTable.Columns.Add("OsAmt", typeof(decimal));
            AdjustmentTable.Columns.Add("AdjAmt", typeof(decimal));
            AdjustmentTable.Columns.Add("RemainingBalance", typeof(decimal));
            AdjustmentTable.Columns.Add("DocumentId", typeof(string));
            AdjustmentTable.Columns.Add("DocumentType", typeof(string));

            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["DocNo"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    NewRow["DocNo"] = Convert.ToString(args.NewValues["DocNo"]);
                    NewRow["DocAmt"] = Convert.ToDecimal(args.NewValues["DocAmt"]);
                    NewRow["Currency"] = Convert.ToString(args.NewValues["Currency"]);
                    NewRow["ExchangeRate"] = Convert.ToString(args.NewValues["ExchangeRate"]);
                    NewRow["LocalAmt"] = Convert.ToDecimal(args.NewValues["LocalAmt"]);
                    NewRow["OsAmt"] = Convert.ToDecimal(args.NewValues["OsAmt"]);
                    NewRow["AdjAmt"] = Convert.ToDecimal(args.NewValues["AdjAmt"]);
                    NewRow["RemainingBalance"] = Convert.ToDecimal(args.NewValues["RemainingBalance"]);
                    NewRow["DocumentId"] = Convert.ToString(args.NewValues["DocumentId"]);
                    NewRow["DocumentType"] = Convert.ToString(args.NewValues["DocumentType"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["DocNo"])))
                {
                     string SrlNo = Convert.ToString(args.Keys["SrlNo"]);
                    bool isDeleted = false;
                    foreach (var arg in e.DeleteValues)
                    {
                        string DeleteID = Convert.ToString(arg.Keys["SrlNo"]);
                        if (DeleteID == SrlNo)
                        {
                            isDeleted = true;
                            break;
                        }
                    }
                    if (isDeleted == false)
                    {
                        NewRow = AdjustmentTable.NewRow();
                        NewRow["DocNo"] = Convert.ToString(args.NewValues["DocNo"]);
                        NewRow["DocAmt"] = Convert.ToDecimal(args.NewValues["DocAmt"]);
                        NewRow["Currency"] = Convert.ToString(args.NewValues["Currency"]);
                        NewRow["ExchangeRate"] = Convert.ToString(args.NewValues["ExchangeRate"]);
                        NewRow["LocalAmt"] = Convert.ToDecimal(args.NewValues["LocalAmt"]);
                        NewRow["OsAmt"] = Convert.ToDecimal(args.NewValues["OsAmt"]);
                        NewRow["AdjAmt"] = Convert.ToDecimal(args.NewValues["AdjAmt"]);
                        NewRow["RemainingBalance"] = Convert.ToDecimal(args.NewValues["RemainingBalance"]);
                        NewRow["DocumentId"] = Convert.ToString(args.NewValues["DocumentId"]);
                        NewRow["DocumentType"] = Convert.ToString(args.NewValues["DocumentType"]);
                        AdjustmentTable.Rows.Add(NewRow);
                    }
                }
            }


            TempTable = AdjustmentTable.Copy();
            RefetchSrlNo();

            blLayer.AddEditCrNoteAdjustmentDrNote(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
                ddlBranch.SelectedValue, hdnCustomerId.Value, hdAdvanceDocNo.Value, btntxtDocNo.Text, DocAmt.Text,
                ExchRate.Text, BaseAmt.Text, Remarks.Text, OsAmt.Text, AdjAmt.Text, Convert.ToString(Session["userid"]), ref adjustmentId,
                ref adjustmentNumber, AdjustmentTable, ref ErrorCode, Request.QueryString["Key"], hdCrNoteType.Value, hddnProjectId.Value);

            if (adjustmentId > 0)
            {
                grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
                grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
                grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
                e.Handled = true;

                #region To Show By Default Cursor after SAVE AND NEW
                if (hdAddEdit.Value == "Add")
                {
                    if (HiddenSaveButton.Value != "E")
                    {
                        string schemavalue = CmbScheme.Value.ToString();
                        string NumberingScheme = CmbScheme.Text;
                        string BranchID = ddlBranch.SelectedValue;
                        string BranchName = ddlBranch.SelectedItem.Text;
                        string CustomerId = hdnCustomerId.Value;
                        string CustomerName = txtCustName.Text;
                        List<string> AfterAdd = new List<string> { schemavalue, NumberingScheme, BranchID, BranchName, CustomerId, CustomerName };

                        Session["schemavalCrDrNoteAdj"] = AfterAdd;

                        string schematype = txtVoucherNo.Text;
                        if (schematype == "Auto")
                        {
                            Session["SaveModeCrDrNoteAdj"] = "A";
                        }
                        else
                        {
                            Session["SaveModeCrDrNoteAdj"] = "M";
                        }
                    }
                }
            }
            else if(adjustmentId==-9)
            {
                DataTable dts = new DataTable();
                dts = GetAddLockStatus();
                grid.JSProperties["cpErrorCode"] = "AddLock";
                grid.JSProperties["cpAddLockStatus"] = (Convert.ToString(dts.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dts.Rows[0]["Lock_Todate"]));
            }
            #endregion
        }
        public DataTable GetAddLockStatus()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustmentDrNote_details");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForCrNoteWithDrNote");

            dt = proc.GetTable();
            return dt;
       }
        private void RefetchSrlNo()
        {
            TempTable.Columns.Add("SrlNo", typeof(string));
            int conut = 1;
            foreach (DataRow dr in TempTable.Rows)
            {
                dr["SrlNo"] = conut;
                conut++;
            }
        }



        protected void grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
            grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = TempTable;
        }
        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion
    }
}