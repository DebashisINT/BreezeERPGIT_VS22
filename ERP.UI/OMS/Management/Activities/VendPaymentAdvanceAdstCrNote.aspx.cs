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
using System.Data.SqlClient;

namespace ERP.OMS.Management.Activities
{
    public partial class VendPaymentAdvanceAdstCrNote : System.Web.UI.Page
    {
        VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();
        DataTable TempTable = new DataTable();
        string adjustmentNumber = "";
        int adjustmentId = 0, ErrorCode = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/activities/VendPaymentAdvanceAdstCrNoteList.aspx");
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
                        //Divproject.Visible = false;
                        Divproject.Style.Add("Display", "None");
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
                        DivHierarchy.Style.Add("Display", "None");
                    }
                }
                if (Request.QueryString["Key"] != "Add")
                {
                    string AdjId = Request.QueryString["Key"];
                    EditModeExecute(AdjId);
                    hdAddEdit.Value = "Edit";
                    lblHeading.Text = "Modify Adj. of Doc - Payment/Dr. Note with Rec./Cr Note";
                    hdAdjustmentId.Value = AdjId;
                }
                else
                {
                    hdAddEdit.Value = "Add";
                    AddmodeExecuted();
                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Adj. of Doc - Payment/Dr. Note with Rec./Cr Note";
                    btn_SaveRecords.Visible = false;
                    btnSaveRecords.Visible = false;
                }
            }
        }
        private void AddmodeExecuted()
        {

            DataSet allDetails = blLayer.PopulateVendorAdvanceAdjustmentCrNoteDetails();
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
        }
        public void EditModeExecute(string id)
        {
            DataSet EditedDataDetails = blLayer.GetEditedCrNoteData(id);
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
            hdnVendorId.Value = Convert.ToString(HeaderRow["Vendor_id"]);
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
            grid.DataBind();

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

            //blLayer.AddEditCrNoteAdvanceAdjustment(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
            //    ddlBranch.SelectedValue, hdnVendorId.Value, hdAdvanceDocNo.Value, btntxtDocNo.Text, DocAmt.Text,
            //    ExchRate.Text, BaseAmt.Text, Remarks.Text, OsAmt.Text, AdjAmt.Text, Convert.ToString(Session["userid"]), ref adjustmentId,
            //    ref adjustmentNumber, AdjustmentTable, ref ErrorCode, Request.QueryString["Key"], hdCrNoteType.Value);
            Int64 ProjId = 0;

            if (hddnProjectId.Value != "")
            {
                ProjId = Convert.ToInt64(hddnProjectId.Value);
            }
            else
            {
                ProjId = Convert.ToInt64("0");
            }

            AddEditCrNoteAdvanceAdjustment(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
              ddlBranch.SelectedValue, hdnVendorId.Value, hdAdvanceDocNo.Value, btntxtDocNo.Text, DocAmt.Text,
              ExchRate.Text, BaseAmt.Text, Remarks.Text, OsAmt.Text, AdjAmt.Text, Convert.ToString(Session["userid"]),ProjId, ref adjustmentId,
              ref adjustmentNumber, AdjustmentTable, ref ErrorCode, Request.QueryString["Key"], hdCrNoteType.Value);

            
            
            grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
            grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
            e.Handled = true;
        }
        public void AddEditCrNoteAdvanceAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Vendor_Id,
          string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
          string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, Int64 ProjId, ref int AdjustedId, ref string ReturnNumber,
          DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string CrNoteType)
        {
            try
            {
                DataTable dsInst = new DataTable();               


                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_VendorAdvanceAdstCrNote_AddEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", Mode);
                cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
                cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
                cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
                cmd.Parameters.AddWithValue("@Branch", Branch);
                cmd.Parameters.AddWithValue("@Vendor_id", Vendor_Id);
                cmd.Parameters.AddWithValue("@Adjusted_doc_id", Adjusted_doc_id);
                cmd.Parameters.AddWithValue("@Adjusted_Doc_no", Adjusted_Doc_no);
                cmd.Parameters.AddWithValue("@Adjusted_DocAmt", Adjusted_DocAmt);
                cmd.Parameters.AddWithValue("@ExchangeRate", ExchangeRate);
                cmd.Parameters.AddWithValue("@Adjusted_DocAmt_inBaseCur", Adjusted_DocAmt_inBaseCur);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@Adjusted_DocOSAmt", Adjusted_DocOSAmt);
                cmd.Parameters.AddWithValue("@Adjusted_Amount", Adjusted_Amount);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
                cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
                cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
                cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@CrNoteType", CrNoteType);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
                cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
                ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
                ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
            }
            catch (Exception ex)
            {

            }

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