using System;
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

namespace ERP.OMS.Management.Activities
{
    public partial class JournalCreditorsAdjustPReturn : System.Web.UI.Page
    {
        CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        DataTable TempTable = new DataTable(); 
         string adjustmentNumber="";
         int adjustmentId=0,ErrorCode=0;
         CommonBL cbl = new CommonBL();
        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (!IsPostBack) 
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/JournalCreditorsAdjustPReturnList.aspx");
                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
                {
                    if (ProjectSelectInEntryModule == "Yes")
                    {
                        //Divproject.Visible = true;
                        Divproject.Style.Add("Display", "Block");
                        hdnProjectSelectInEntryModule.Value = "1";
                    }
                    else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                       // Divproject.Visible = false;
                        Divproject.Style.Add("Display", "None");
                        hdnProjectSelectInEntryModule.Value = "0";
                    }
                }
                string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
                if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
                {
                    if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                    {
                       // DivHierarchy.Visible = true;
                        DivHierarchy.Style.Add("Display", "Block");

                    }
                    else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                    {
                       // DivHierarchy.Visible = false;
                        DivHierarchy.Style.Add("Display", "None");
                    }
                }
                if (Request.QueryString["Key"] != "Add")
                {
                    string AdjId = Request.QueryString["Key"];
                    EditModeExecute(AdjId);
                    hdAddEdit.Value = "Edit";
                    hdAdjustmentId.Value = AdjId;
                   // btnSaveRecords.Visible = false;
                   // btnSaveRecords.ClientEnabled = false;
                }
                else
                {
                    hdAddEdit.Value = "Add";
                    AddmodeExecuted();
                    Session["AdjustJournalCreditorDetailTablePR"] = null;
                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Adjustment of Documents - Journal with Purchase Return";
                    btn_SaveRecords.Visible = false;
                    btnSaveRecords.Visible = false;
                }  
            } 

        }

        private void AddmodeExecuted()
        {

            DataSet allDetails = blLayer.PopulateJournalCreditorsAdjustmentPReturnDetails();
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

            if (Session["schemavalJourCreditorAdjPR"] != null)
            {
                List<string> SaveNewValues = (Session["schemavalJourCreditorAdjPR"]) as List<string>;
                CmbScheme.Value = SaveNewValues[0];
                CmbScheme.Text = SaveNewValues[1];
                ddlBranch.SelectedValue = SaveNewValues[2];
                ddlBranch.SelectedItem.Text = SaveNewValues[3];
                ddlBranch.Enabled = false;

            }
            if (Convert.ToString(Session["SaveModeJourCreditorsAdjPR"]) == "A")
            {
                dtTDate.Focus();
                txtVoucherNo.ClientEnabled = false;
                txtVoucherNo.Text = "Auto";
            }
            else if (Convert.ToString(Session["SaveModeJourCreditorsAdjPR"]) == "M")
            {
                txtVoucherNo.ClientEnabled = true;
                txtVoucherNo.Text = "";
                txtVoucherNo.Focus();

            }
        }


        public void EditModeExecute(string id)
        {
            DataSet EditedDataDetails = blLayer.GetEditedJounalCreditorPReturnData(id);
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
            txtVendName.Text = Convert.ToString(HeaderRow["CustName"]);
            txtVendName.ClientEnabled = false;
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
            hdAdjustmentType.Value = Convert.ToString(HeaderRow["AdvType"]);
            TempTable = EditedDataDetails.Tables[4];
            HiddenRowCount.Value = TempTable.Rows.Count.ToString();
            Session["AdjustJournalCreditorDetailTablePR"] = EditedDataDetails.Tables[4];
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
            AdjustmentTable.Columns.Add("ActualSL", typeof(string));
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
                    NewRow["ActualSL"] = Convert.ToString(args.NewValues["ActualSL"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            foreach (var args in e.UpdateValues)
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
                    NewRow["ActualSL"] = Convert.ToString(args.NewValues["ActualSL"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }


           
            foreach (var args in e.DeleteValues)
            {
                DataTable AdjTable = (DataTable)Session["AdjustJournalCreditorDetailTablePR"];
                if (AdjTable != null)
                {
                    string delId = Convert.ToString(args.Keys[0]);
                    DataRow[] AdjTableRow = AdjustmentTable.Select("ActualSL='" + delId + "'");
                    //DataRow[] delRow = AdjustmentTable.Select("DocumentId='" + AdjTableRow[0]["DocumentId"] + "' and DocumentType='" + AdjTableRow[0]["DocumentType"] + "'");
                    foreach (DataRow dr in AdjTableRow)
                        dr.Delete();

                    AdjustmentTable.AcceptChanges();
                }
            }
            
            TempTable = AdjustmentTable.Copy();
            AdjustmentTable.Columns.Remove("ActualSL");
            AdjustmentTable.AcceptChanges();
            RefetchSrlNo();
            Int64 ProjId = 0;

            if (hddnProjectId.Value != "")
            {
                ProjId = Convert.ToInt64(hddnProjectId.Value);
            }
            else
            {
                ProjId = Convert.ToInt64("0");
            }

            blLayer.AddEditJournalAdjustmentPurchaseReturn(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
                ddlBranch.SelectedValue,hdnCustomerId.Value,hdAdvanceDocNo.Value,btntxtDocNo.Text,DocAmt.Text,
                ExchRate.Text,BaseAmt.Text,Remarks.Text,OsAmt.Text,AdjAmt.Text,Convert.ToString(Session["userid"]),ProjId,ref adjustmentId,
                ref adjustmentNumber, AdjustmentTable, ref ErrorCode, Request.QueryString["Key"], hdAdjustmentType.Value);

            grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            grid.JSProperties["cpErrorCode"] =  Convert.ToString(ErrorCode);
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

                    List<string> AfterAdd = new List<string> { schemavalue, NumberingScheme, BranchID, BranchName };

                    Session["schemavalJourCreditorAdjPR"] = AfterAdd;

                    string schematype = txtVoucherNo.Text;
                    if (schematype == "Auto")
                    {
                        Session["SaveModeJourCreditorsAdjPR"] = "A";
                    }
                    else
                    {
                        Session["SaveModeJourCreditorsAdjPR"] = "M";
                    }
                }

            }


            #endregion
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
            grid.JSProperties["cpErrorCode"] =  Convert.ToString(ErrorCode);
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