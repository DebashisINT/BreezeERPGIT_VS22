using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using ERP.Models;
using ERP.OMS.Tax_Details.ClassFile;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerDebitCreditNote : System.Web.UI.Page
    {
        MasterSettings objmaster = new MasterSettings();
        public DataTable BatchGridData { get; set; }
        CustomerNoteBL blLayer = new CustomerNoteBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        DBEngine oDBEngine = new DBEngine();
        CommonBL ComBL = new CommonBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");

            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                }
            }

            string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");

            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hdnProjectMandatory.Value = "1";



                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hdnProjectMandatory.Value = "0";


                }
            }

            //For Hierarchy Start Tanmoy
            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                }
            }
            //For Hierarchy End Tanmoy

            if (!IsPostBack)
            {
                string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "P");
                HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                HDBranchWiseStateTax.Value = BranchWiseStateTax;
                HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End

                hdnDocumentSegmentSettings.Value = objmaster.GetSettings("DocumentSegment");

                if (hdnDocumentSegmentSettings.Value == "0")
                {
                    DivSegment1.Attributes.Add("style", "display:none");
                    DivSegment2.Attributes.Add("style", "display:none");
                    DivSegment3.Attributes.Add("style", "display:none");
                    DivSegment4.Attributes.Add("style", "display:none");
                    DivSegment5.Attributes.Add("style", "display:none");
                }

                if (Request.QueryString["key"] == "Add")
                {
                    btnSaveRecords.ClientVisible = true;
                    hdAddEdit.Value = "Add";

                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=2");

                    if (dtposTime != null && dtposTime.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                    }
                }
                else if (Request.QueryString["key"] == "Edit" || Request.QueryString["key"] == "View")
                {

                    btnSaveRecords.ClientVisible = false;
                    string Id = Convert.ToString(Request.QueryString["id"]);
                    DataSet dsEdit = blLayer.GetEditDetails(Id, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userid"]));
                    DataTable dtProj = blLayer.GetProjectDetails(Id);
                    hdAddEdit.Value = Convert.ToString(Request.QueryString["key"]);

                    Session["RecpayId"] = Id;
                    ReceiptPaymentId.InnerText = Id;


                    ddlBranch.TextField = "Branch_Description";
                    ddlBranch.ValueField = "ID";
                    ddlBranch.DataSource = dsEdit.Tables[4];
                    ddlBranch.DataBind();



                    ddlCurrency.TextField = "Currency_AlphaCode";
                    ddlCurrency.ValueField = "ID";
                    ddlCurrency.DataSource = dsEdit.Tables[3];
                    ddlCurrency.DataBind();


                    ddlInvoice.TextField = "Invoice_Number";
                    ddlInvoice.ValueField = "Invoice_Id";
                    ddlInvoice.DataSource = dsEdit.Tables[8];
                    ddlInvoice.DataBind();


                    ddl_Reason.TextField = "Reason";
                    ddl_Reason.ValueField = "ReasonID";
                    ddl_Reason.DataSource = dsEdit.Tables[7];
                    ddl_Reason.DataBind();


                    #region Header Load

                    hdnCustomerId.Value = Convert.ToString(dsEdit.Tables[1].Rows[0]["CustomerID"]);
                    ddlBranch.Value = Convert.ToString(dsEdit.Tables[1].Rows[0]["BranchID"]);
                    dtTDate.Date = Convert.ToDateTime(dsEdit.Tables[1].Rows[0]["Documentdate"]);
                    txtVoucherNo.Text = Convert.ToString(dsEdit.Tables[1].Rows[0]["documentNumber"]);
                    txtCustName.Text = Convert.ToString(dsEdit.Tables[1].Rows[0]["Name"]);
                    ddlCurrency.Value = Convert.ToString(dsEdit.Tables[1].Rows[0]["CurrencyId"]);
                    txtNarration.Text = Convert.ToString(dsEdit.Tables[1].Rows[0]["Narration"]);
                    txtRate.Text = Convert.ToString(dsEdit.Tables[1].Rows[0]["Rate"]);
                    ddlNoteType.Value = Convert.ToString(dsEdit.Tables[1].Rows[0]["NoteType"]);
                    ddl_Reason.Value = Convert.ToString(dsEdit.Tables[1].Rows[0]["ReasonID"]);
                    ddlInvoice.Value = Convert.ToString(dsEdit.Tables[1].Rows[0]["InvoiceID"]);
                    hdnTagCount.Value = Convert.ToString(dsEdit.Tables[1].Rows[0]["TagCount"]);
                    #endregion
                    Decimal TotAmt = Convert.ToDecimal(dsEdit.Tables[10].Rows[0]["TotalAmount"]);
                    Decimal UnpaidAmt = Convert.ToDecimal(dsEdit.Tables[10].Rows[0]["UnpaidAmt"]);
                    if (dsEdit.Tables[5] != null && dsEdit.Tables[5].Rows.Count > 0)
                    {
                        dtTDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[5].Rows[0]["finYearEndDate"]);
                        dtTDate.MinDate = Convert.ToDateTime(dsEdit.Tables[5].Rows[0]["finYearStartDate"]);
                    }
                    else
                    {
                        dtTDate.MaxDate = DateTime.Today;
                        dtTDate.MinDate = DateTime.Today;
                    }

                    string Segment1 = Convert.ToString(dsEdit.Tables[1].Rows[0]["Segment1"]);
                    string Segment2 = Convert.ToString(dsEdit.Tables[1].Rows[0]["Segment2"]);
                    string Segment3 = Convert.ToString(dsEdit.Tables[1].Rows[0]["Segment3"]);
                    string Segment4 = Convert.ToString(dsEdit.Tables[1].Rows[0]["Segment4"]);
                    string Segment5 = Convert.ToString(dsEdit.Tables[1].Rows[0]["Segment5"]);

                    txtSegment1.Text = Segment1;
                    txtSegment2.Text = Segment2;
                    txtSegment3.Text = Segment3;
                    txtSegment4.Text = Segment4;
                    txtSegment5.Text = Segment5;


                    hdnSegment1.Value = Segment1;
                    hdnSegment2.Value = Segment2;
                    hdnSegment3.Value = Segment3;
                    hdnSegment4.Value = Segment4;
                    hdnSegment5.Value = Segment5;

                    Session["CDCN_FinalTaxRecord"] = dsEdit.Tables[2];

                    BillingShippingControl.SetBillingShippingTable(dsEdit.Tables[9]);

                    BatchGridData = dsEdit.Tables[0];
                    grid.DataBind();
                    if (Request.QueryString["key"] == "Edit")
                    {
                        if (Convert.ToInt32(hdnTagCount.Value) > 0)
                        {
                            tagged.Style.Add("display", "block");
                            btnSaveRecords.ClientVisible = false;
                            btn_SaveRecords.ClientVisible = false;

                        }
                        else
                        {
                            tagged.Style.Add("display", "none");
                            btnSaveRecords.ClientVisible = true;
                            btn_SaveRecords.ClientVisible = true;
                        }
                        if (TotAmt!=UnpaidAmt)
                        {
                            tagged.Style.Add("display", "block");
                            btnSaveRecords.ClientVisible = false;
                            btn_SaveRecords.ClientVisible = false;
                        }
                    }

                  
                    if (dtProj != null)
                    {
                        lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtProj.Rows[0]["Proj_Id"]));

                        //Tanmoy  Hierarchy
                        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                        DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtProj.Rows[0]["Proj_Id"]) + "'");
                        if (dt2.Rows.Count > 0)
                        {
                            ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                        }
                        //Tanmoy  Hierarchy End
                    }

                }
                //Rev v1.0.101  subhra  04-01-2019  0019425 
                BindSystemSettings();
                //End of Subhra
            }




        }
        //Rev v1.0.101  subhra  04-01-2019  0019425
        #region System setup
        public DataSet GetSystemSettings()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetSystemSettingValue");
            dt = proc.GetDataSet();
            return dt;
        }
        public void BindSystemSettings()
        {
            DataSet dtSystemSettings = new DataSet();
            dtSystemSettings = GetSystemSettings();
            if (dtSystemSettings.Tables[0] != null && dtSystemSettings.Tables[0].Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Tables[0].Rows[0]["Variable_Value"]);
                hdnAutoPrint.Value = Variable_Value;
            }
        }
        #endregion
        //End of Subhra

        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "MainAccount" || e.Column.FieldName == "bthSubAccount" || e.Column.FieldName == "TaxAmount" || e.Column.FieldName == "NetAmount")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "btnRecieve" || e.Column.FieldName == "btnLineNarration")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "SrlNo")
            {

                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
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

        protected void grid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            DataTable AdjustmentTable = new DataTable();
            // AdjustmentTable.Columns.Add("SrlNo", typeof(string));
            AdjustmentTable.Columns.Add("MainAccount", typeof(string));
            AdjustmentTable.Columns.Add("bthSubAccount", typeof(string));
            AdjustmentTable.Columns.Add("btnRecieve", typeof(string));
            AdjustmentTable.Columns.Add("TaxAmount", typeof(string));
            AdjustmentTable.Columns.Add("NetAmount", typeof(string));
            AdjustmentTable.Columns.Add("btnLineNarration", typeof(string));
            AdjustmentTable.Columns.Add("HSNCODE", typeof(string));
            AdjustmentTable.Columns.Add("Note_Id", typeof(string));
            AdjustmentTable.Columns.Add("gvColMainAccount", typeof(string));
            AdjustmentTable.Columns.Add("gvColSubAccount", typeof(string));
            AdjustmentTable.Columns.Add("IsSubledger", typeof(string));
            AdjustmentTable.Columns.Add("UpdateEdit", typeof(string));



            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["MainAccount"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    // NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["MainAccount"] = Convert.ToString(args.NewValues["MainAccount"]);
                    NewRow["bthSubAccount"] = Convert.ToString(args.NewValues["bthSubAccount"]);
                    NewRow["btnRecieve"] = Convert.ToString(args.NewValues["btnRecieve"]);
                    NewRow["TaxAmount"] = Convert.ToString(args.NewValues["TaxAmount"]);
                    NewRow["NetAmount"] = Convert.ToString(args.NewValues["NetAmount"]);
                    NewRow["btnLineNarration"] = Convert.ToString(args.NewValues["btnLineNarration"]);
                    NewRow["HSNCODE"] = Convert.ToString(args.NewValues["HSNCODE"]);
                    NewRow["Note_Id"] = Convert.ToString(args.NewValues["Note_Id"]);
                    NewRow["gvColMainAccount"] = Convert.ToString(args.NewValues["gvColMainAccount"]);
                    NewRow["gvColSubAccount"] = Convert.ToString(args.NewValues["gvColSubAccount"]);
                    NewRow["IsSubledger"] = Convert.ToString(args.NewValues["IsSubledger"]);
                    NewRow["UpdateEdit"] = Convert.ToString(args.NewValues["UpdateEdit"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["MainAccount"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    // NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["MainAccount"] = Convert.ToString(args.NewValues["MainAccount"]);
                    NewRow["bthSubAccount"] = Convert.ToString(args.NewValues["bthSubAccount"]);
                    NewRow["btnRecieve"] = Convert.ToString(args.NewValues["btnRecieve"]);
                    NewRow["TaxAmount"] = Convert.ToString(args.NewValues["TaxAmount"]);
                    NewRow["NetAmount"] = Convert.ToString(args.NewValues["NetAmount"]);
                    NewRow["btnLineNarration"] = Convert.ToString(args.NewValues["btnLineNarration"]);
                    NewRow["HSNCODE"] = Convert.ToString(args.NewValues["HSNCODE"]);
                    NewRow["Note_Id"] = Convert.ToString(args.NewValues["Note_Id"]);
                    NewRow["gvColMainAccount"] = Convert.ToString(args.NewValues["gvColMainAccount"]);
                    NewRow["gvColSubAccount"] = Convert.ToString(args.NewValues["gvColSubAccount"]);
                    NewRow["IsSubledger"] = Convert.ToString(args.NewValues["IsSubledger"]);
                    NewRow["UpdateEdit"] = Convert.ToString(args.NewValues["UpdateEdit"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }



            foreach (var args in e.DeleteValues)
            {
                DataTable AdjTable = AdjustmentTable;
                if (AdjTable != null)
                {
                    string delId = Convert.ToString(args.Keys[0]);
                    DataRow[] AdjTableRow = AdjTable.Select("Note_Id='" + delId + "'");
                    foreach (DataRow dr in AdjTableRow)
                    {
                        dr.Delete();
                    }

                    AdjustmentTable.AcceptChanges();
                }
            }
            BatchGridData = AdjustmentTable.Copy();
            RefetchSrlNo();

            DataTable TaxRecord = new DataTable();
            if (Session["CDCN_FinalTaxRecord"] == null)
            {
                CreateDataTaxTable();
            }

            TaxRecord = (DataTable)Session["CDCN_FinalTaxRecord"];

            TaxRecord = gstTaxDetails.SetTaxTableDataMainAccount(ref AdjustmentTable, "Note_Id", "HSNCODE", "btnRecieve", "TaxAmount", "NetAmount", TaxRecord, "S", DateTime.ParseExact(dtTDate.Text, "dd-MM-yyyy", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd"), Convert.ToString(ddlBranch.Value), BillingShippingControl.GetBillingStateCode(), "E", Convert.ToString(hdnCustomerId.Value));

            string validate = "";
            string sstateCode = "";
            sstateCode = BillingShippingControl.GetBillingStateCode();


            DataTable NoteTable = new DataTable();
            NoteTable = GetReceiptDataSource();
            foreach (DataRow dr in BatchGridData.Rows)
            {

                NoteTable.Rows.Add(Convert.ToString(dr["Note_Id"]), Convert.ToString(dr["gvColMainAccount"]), Convert.ToString(dr["gvColSubAccount"]),
                   Convert.ToDecimal(dr["btnRecieve"]), Convert.ToDecimal(dr["TaxAmount"]), Convert.ToDecimal(dr["NetAmount"]), Convert.ToString(dr["btnLineNarration"]));
            }


            string GSTRateTaxMasterMandatory = ComBL.GetSystemSettingsResult("GSTRateTaxMasterMandatory");
            if (!String.IsNullOrEmpty(GSTRateTaxMasterMandatory))
            {
                if (GSTRateTaxMasterMandatory == "Yes")
                {

                    DataTable dtTaxDetails = new DataTable();
                    ProcedureExecute procT = new ProcedureExecute("prdn_CustomerNoteDetails");
                    procT.AddVarcharPara("@Action", 500, "GetTaxDetailsByProductID");
                    procT.AddPara("@ProductDetails", NoteTable);
                    procT.AddVarcharPara("@SupplyState", 15, Convert.ToString(sstateCode));
                    procT.AddVarcharPara("@BRANCHID", 10, Convert.ToString(ddlBranch.Value));
                    procT.AddVarcharPara("@COMPANYID", 500, Convert.ToString(Session["LastCompany"]));
                    procT.AddVarcharPara("@ENTITY_ID", 100, Convert.ToString(hdnCustomerId.Value));

                    dtTaxDetails = procT.GetTable();

                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtTaxDetails.Rows)
                        {
                            string SerialID = Convert.ToString(dr["SrlNo"]);
                            string TaxID = Convert.ToString(dr["TaxCode"]);
                            decimal _TaxAmount = Math.Round(Convert.ToDecimal(dr["TaxAmount"]), 2);
                            string ProductName = Convert.ToString(dr["AccountCode"]);

                            DataRow[] rows = TaxRecord.Select("SlNo = '" + SerialID + "' and TaxCode='" + TaxID + "'");

                            if (rows != null && rows.Length > 0)
                            {
                                //decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);
                                decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);

                                if (EntryTaxAmount != _TaxAmount)
                                {
                                    validate = "checkAcurateTaxAmount";
                                    grid.JSProperties["cpSerialNo"] = SerialID;
                                    grid.JSProperties["cpProductName"] = ProductName;
                                    break;
                                }


                            }

                        }

                    }
                }
            }


            string ActionType = hdAddEdit.Value;
            string NoteId = ReceiptPaymentId.InnerText;
            string NoteBranchID = Convert.ToString(ddlBranch.Value);
            DateTime TransactionDate = dtTDate.Date;
            string TransactionType = ddlNoteType.Value.ToString();
            string CustomerId = hdnCustomerId.Value;
            string Currency = Convert.ToString(ddlCurrency.Value);
            string rate = txtRate.Text;
            string CompanyId = Convert.ToString(Session["LastCompany"]);
            string LastFinYear = Convert.ToString(Session["LastFinYear"]);
            string userid = Convert.ToString(Session["userid"]);
            string SCHEMEID = Convert.ToString(CmbScheme.Value).Split('~')[0];
            string Doc_No = txtVoucherNo.Text;
            string InvoiceId = ddlInvoice.Value.ToString();


            string Narration = txtNarration.Text;
            string PartyInvoiceNo = txtPartyInvoice.Text;
            string ReasonID = ddl_Reason.Value.ToString();
            DateTime? PartyInvoiceDate = null;
            if (dt_PartyDate.Text != "")
            {
                PartyInvoiceDate = dt_PartyDate.Date;
            }

            DataTable BillAddress = BillingShippingControl.GetBillingShippingTable();

            CommonBL cbl = new CommonBL();
            string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            Int64 ProjId = 0;
            if (lookup_Project.Text != "")
            {
                string projectCode = lookup_Project.Text;
                DataTable dtSlOrd = blLayer.GetProjectCode(projectCode);
                //oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                ProjId = Convert.ToInt64(dtSlOrd.Rows[0]["Proj_Id"]);
            }
            else if (lookup_Project.Text == "")
            {
                ProjId = 0;
            }

            else
            {
                ProjId = 0;
            }

            string Segment1 = Convert.ToString(hdnSegment1.Value);
            string Segment2 = Convert.ToString(hdnSegment2.Value);
            string Segment3 = Convert.ToString(hdnSegment3.Value);
            string Segment4 = Convert.ToString(hdnSegment4.Value);
            string Segment5 = Convert.ToString(hdnSegment5.Value);

            string paymenttype = "";
            DataTable dtMultiType;
            string OutputId = "";
            //Rev v1.0.101  subhra  07-01-2019  0019425 
            string OutputNoteId = "";
            //string OutputText = blLayer.AddEditPayment(ref OutputId, ActionType, NoteId, NoteBranchID, TransactionDate, TransactionType, CustomerId, Currency, rate, BatchGridData, BillAddress, CompanyId, LastFinYear, userid, SCHEMEID, Doc_No, TaxRecord, InvoiceId, Narration, PartyInvoiceNo, ReasonID, PartyInvoiceDate);
            if (validate == "checkAcurateTaxAmount")
            {
                OutputId = "0";
                grid.JSProperties["cpInsert"] = OutputId + "~" + "" + "~" + hdnRefreshType.Value;
                grid.JSProperties["cpSaveSuccessOrFail"] = validate;
            }
            else
            {
                string OutputText = blLayer.AddEditPayment(ref OutputId, ref OutputNoteId, ActionType, NoteId, NoteBranchID, TransactionDate, TransactionType, CustomerId, Currency, rate, BatchGridData, BillAddress, CompanyId, LastFinYear, userid, SCHEMEID, Doc_No, TaxRecord, InvoiceId, Narration, PartyInvoiceNo, ReasonID, PartyInvoiceDate, ProjId
                 , Segment1, Segment2, Segment3, Segment4, Segment5);
                grid.JSProperties["cpCrDrNoteId"] = OutputNoteId;
                grid.JSProperties["cpTransactionType"] = TransactionType;
                //End of Rev

                grid.JSProperties["cpInsert"] = OutputId + "~" + OutputText + "~" + hdnRefreshType.Value;


            }

           // if (string.IsNullOrEmpty(OutputText) || Convert.ToInt32(OutputText) > 0)
            if (Convert.ToInt32(OutputId) > 0)
            {
                BatchGridData.Rows.Clear();
                BatchGridData.AcceptChanges();
            }
            else if (Convert.ToInt32(OutputId) == -9)
            //else if (Convert.ToInt32(OutputId) <= 0 && Convert.ToInt32(OutputId) == -9)
            {
                DataTable dt = new DataTable();
                dt = GetAddLockStatus(TransactionType);
                grid.JSProperties["cpAddLockStatus"] = (Convert.ToString(dt.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dt.Rows[0]["Lock_Todate"]));
            }
            grid.DataBind();

            e.Handled = true;
        }
        private DataTable GetReceiptDataSource()
        {
            DataTable NoteTable = new DataTable();
            NoteTable.Columns.Add("Note_Id", typeof(System.String));
            NoteTable.Columns.Add("MainAccountId", typeof(System.String));
            NoteTable.Columns.Add("SubAccountId", typeof(System.String));
            NoteTable.Columns.Add("Amount", typeof(System.Decimal));
            NoteTable.Columns.Add("Charges", typeof(System.Decimal));
            NoteTable.Columns.Add("NetAmount", typeof(System.Decimal));
            NoteTable.Columns.Add("Remarks", typeof(System.String));
            return NoteTable;
        }
        public DataTable GetAddLockStatus(string type)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForDrCrNote");
            proc.AddVarcharPara("@Type", 100, type);
            dt = proc.GetTable();
            return dt;

        }
        protected void EntityServerModeDataCustDbCr_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();



            // Mantis Issue 24976
            //var q = from d in dc.V_ProjectLists
            //        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddlBranch.Value) && d.CustId == hdnCustomerId.Value
            //        orderby d.Proj_Id descending
            //        select d;

            //e.QueryableSource = q;

            CommonBL cbl = new CommonBL();
            string ISProjectIndependentOfBranch = cbl.GetSystemSettingsResult("AllowProjectIndependentOfBranch");

            if (ISProjectIndependentOfBranch == "No")
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddlBranch.Value) && d.CustId == hdnCustomerId.Value
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.CustId == hdnCustomerId.Value
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            // End of Mantis Issue 24976

        }

        private void RefetchSrlNo()
        {
            BatchGridData.Columns.Add("SrlNo", typeof(string));
            int conut = 1;
            foreach (DataRow dr in BatchGridData.Rows)
            {
                dr["SrlNo"] = conut;
                conut++;
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = BatchGridData;
        }

        public class TaxSetailsJson
        {
            public string SchemeName { get; set; }
            public int vissibleIndex { get; set; }
            public string applicableOn { get; set; }
            public string applicableBy { get; set; }
        }
        public class TaxDetails
        {
            public int Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }
            public double Amount { get; set; }
            public string TaxField { get; set; }
            public string taxCodeName { get; set; }
            public decimal calCulatedOn { get; set; }
        }
        public void CreateDataTaxTable()
        {
            DataTable TaxRecord = new DataTable();
            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            Session["CDCN_FinalTaxRecord"] = TaxRecord;
        }
        public double GetTotalTaxAmount(List<TaxDetails> tax)
        {
            double sum = 0;
            foreach (TaxDetails td in tax)
            {
                if (td.Taxes_Name.Substring(td.Taxes_Name.Length - 3, 3) == "(+)")
                    sum += td.Amount;
                else
                    sum -= td.Amount;

            }
            return sum;
        }

        public List<TaxDetails> setCalculatedOn(List<TaxDetails> gridSource, DataTable taxDt)
        {
            foreach (TaxDetails taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.Taxes_Name.Replace("(+)", "").Replace("(-)", "") + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {

                        foreach (var setCalObj in gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }

        public string createJsonForDetails(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }
        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string stat = "";
            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["CDCN_FinalTaxRecord"];
                int slNo = Convert.ToInt32(HdSerialNo.Value);


                //End Here
                aspxGridTax.JSProperties["cpUpdated"] = "";
                Session["SI_FinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                if (Session["CDCN_FinalTaxRecord"] == null)
                {
                    CreateDataTaxTable();
                }
                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["CDCN_FinalTaxRecord"];

                DataRow[] drr = MainTaxDataTable.Select("SLNo='" + HdSerialNo1.Value + "'");

                if (MainTaxDataTable != null && drr.Length == 0)
                {
                    stat = "New";
                    string type = "S";
                    ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@MainAccountID", 100, Convert.ToString(setCurrentProdCode.Value));
                    proc.AddVarcharPara("@dtTDate", 10, dtTDate.Date.ToString("yyyy-MM-dd"));
                    proc.AddVarcharPara("@Type", 5, type);
                    taxDetail = proc.GetTable();
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    //Get BranchStateCode
                    string BranchStateCode = "", BranchGSTIN = "";
                    DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddlBranch.Value));
                    if (BranchTable != null)
                    {
                        BranchStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                        BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                    }
                    string ShippingState = "";

                    #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######

                    // string sstateCode = BillingShippingControl.GetShippingStateCode("ADD");

                    string sstateCode = BillingShippingControl.GeteShippingStateCode();
                    ShippingState = sstateCode;
                    if (ShippingState.Trim() != "")
                    {

                        DataTable VendTable = oDBEngine.GetDataTable("Select CNT_GSTIN from tbl_master_contact where cnt_ContactType='DV' AND ISNULL(CNT_GSTIN,'')<>'' and cnt_internalid='" + Convert.ToString(hdnCustomerId.Value) + "'");

                        if (VendTable != null && VendTable.Rows.Count > 0)
                        {
                            string str = Convert.ToString(VendTable.Rows[0]["CNT_GSTIN"]);
                            ShippingState = str.Substring(0, 2);

                        }
                        else
                        {
                            DataTable dtState = oDBEngine.GetDataTable("Select top 1 ISNULL(StateCode,'') StateCode from tbl_master_address inner join tbl_master_state on id=add_state where Isdefault =1 and add_addressType='Shipping' and  add_cntId='" + Convert.ToString(hdnCustomerId.Value) + "'");
                            if (dtState != null && dtState.Rows.Count > 0)
                            {
                                string str = Convert.ToString(dtState.Rows[0]["StateCode"]);
                                ShippingState = str;
                            }
                        }

                        // ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }

                    #endregion

                    if (ShippingState.Trim() != "" && BranchStateCode != "")
                    {
                        if (BranchStateCode == ShippingState)
                        {
                            if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            taxDetail.AcceptChanges();
                        }
                        else
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                {
                                    dr.Delete();
                                }
                            }
                            taxDetail.AcceptChanges();
                        }
                    }
                    if (compGstin[0].Trim() == "" && BranchGSTIN == "")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                            {
                                dr.Delete();
                            }
                        }
                        taxDetail.AcceptChanges();
                    }
                }



                //Get Company Gstin 09032017

                int slNo = Convert.ToInt32(HdSerialNo1.Value);
                //Get Gross Amount and Net Amount 
                decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
                decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);
                List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();
                //Debjyoti 09032017
                decimal totalParcentage = 0;
                foreach (DataRow dr in taxDetail.Rows)
                {
                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                    {
                        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    }
                }
                if (stat == "New")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);
                        obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]);
                        obj.Amount = 0.0;

                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion
                        //Debjyoti 09032017

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        }
                        else
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        }
                        obj.Amount = Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100));
                        DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        if (filtr.Length > 0)
                        {
                            obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
                            if (obj.Taxes_ID == 0)
                            {
                                //   obj.TaxField = GetTaxName(Convert.ToInt32(Convert.ToString(filtr[0]["AltTaxCode"])));
                                aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtr[0]["AltTaxCode"]);
                            }
                            else
                                obj.TaxField = Convert.ToString(filtr[0]["Percentage"]);
                        }

                        TaxDetailsDetails.Add(obj);
                    }
                }
                else
                {


                    DataView dvView = new DataView(MainTaxDataTable);
                    dvView.RowFilter = "SLNO='" + Convert.ToInt32(HdSerialNo1.Value) + "'";

                    taxDetail = blLayer.GetExactTable(dvView.ToTable());
                    string keyValue = e.Parameters.Split('~')[0];
                    DataTable TaxRecord = (DataTable)Session["CDCN_FinalTaxRecord"];
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);
                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        obj.TaxField = "";
                        obj.Amount = 0.0;
                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion
                        //Debjyoti 09032017

                        DataRow[] filtronexsisting1 = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                        if (filtronexsisting1.Length > 0)
                        {
                            if (obj.Taxes_ID == 0)
                            {
                                obj.TaxField = "0";
                            }
                            else
                            {
                                obj.TaxField = Convert.ToString(filtronexsisting1[0]["Percentage"]);
                            }
                            obj.Amount = Convert.ToDouble(filtronexsisting1[0]["Amount"]);
                        }
                        else
                        {
                            DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            if (filtronexsisting.Length > 0)
                            {
                                DataRow taxRecordNewRow = TaxRecord.NewRow();
                                taxRecordNewRow["SlNo"] = slNo;
                                taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                                taxRecordNewRow["AltTaxCode"] = "0";
                                taxRecordNewRow["Percentage"] = 0.0;
                                taxRecordNewRow["Amount"] = "0";
                                TaxRecord.Rows.Add(taxRecordNewRow);
                            }
                        }
                        TaxDetailsDetails.Add(obj);
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["CDCN_FinalTaxRecord"] = TaxRecord;

                }
                //New Changes 170217
                //GstCode should fetch everytime
                DataRow[] finalFiltrIndex = MainTaxDataTable.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                if (finalFiltrIndex.Length > 0)
                {
                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(finalFiltrIndex[0]["AltTaxCode"]);
                }
                aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);
                retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
                aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;

                if (Request.QueryString["key"] != "ADD")
                {
                    if ((Convert.ToDecimal(TaxAmountOngrid.Value) == 0) && TaxDetailsDetails != null && Convert.ToInt64(VisibleIndexForTax.Value) >= 0)
                    {
                        foreach (var DrTax in TaxDetailsDetails)
                        {

                            DrTax.Amount = 0;
                        }

                    }

                }
                // taxDetail.AcceptChanges();
                TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);
                aspxGridTax.DataSource = TaxDetailsDetails;
                aspxGridTax.DataBind();
                #endregion
            }
        }

        protected void aspxGridTax_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "Taxes_Name")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "taxCodeName")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "calCulatedOn")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }
        }

        protected void taxgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            int slNo = Convert.ToInt32(HdSerialNo1.Value);
            DataTable TaxRecord = (DataTable)Session["CDCN_FinalTaxRecord"];
            foreach (var args in e.UpdateValues)
            {
                string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
                decimal Percentage = 0;
                Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
                decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
                string TaxCode = "0";
                if (!Convert.ToString(args.Keys[0]).Contains('~'))
                {
                    TaxCode = Convert.ToString(args.Keys[0]);
                }
                DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='" + TaxCode + "'");
                if (finalRow.Length > 0)
                {
                    finalRow[0]["Percentage"] = Percentage;
                    finalRow[0]["Amount"] = Amount;
                    finalRow[0]["TaxCode"] = args.Keys[0];
                    finalRow[0]["AltTaxCode"] = "0";
                }
                else
                {
                    DataRow newRow = TaxRecord.NewRow();
                    newRow["slNo"] = slNo;
                    newRow["Percentage"] = Percentage;
                    newRow["TaxCode"] = TaxCode;
                    newRow["AltTaxCode"] = "0";
                    newRow["Amount"] = Amount;
                    TaxRecord.Rows.Add(newRow);
                }
            }

            Session["CDCN_FinalTaxRecord"] = TaxRecord;

        }

        protected void deleteTax_Callback(object sender, CallbackEventArgsBase e)
        {

            string[] parameter = e.Parameter.Split('~');

            if (parameter[0].ToString() == "DeleteAllTax")
            {


                if (Session["CDCN_FinalTaxRecord"] != null)
                {
                    DataTable dtTax = (DataTable)Session["VDCN_FinalTaxRecord"];
                    DataRow[] drTax = dtTax.Select("1=1");
                    foreach (var drr in drTax)
                    {
                        drr.Delete();
                    }
                    dtTax.AcceptChanges();
                    Session["VDCN_FinalTaxRecord"] = dtTax;
                }







            }
            else if (parameter[0].ToString() == "DeleteInlineTax")
            {


                if (Session["CDCN_FinalTaxRecord"] != null)
                {
                    DataTable dtTax = (DataTable)Session["CDCN_FinalTaxRecord"];
                    DataRow[] drTax = dtTax.Select("slNo='" + Convert.ToString(parameter[1]) + "'");
                    foreach (var drr in drTax)
                    {
                        drr.Delete();
                    }
                    dtTax.AcceptChanges();
                    Session["CDCN_FinalTaxRecord"] = dtTax;
                }







            }
        }

        //Tanmoy Hierarchy
        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }
        //Tanmoy Hierarchy End

        //Tanmoy Hierarchy
        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();
            }
        }
        //End Tanmoy Hierarchy
    }
}