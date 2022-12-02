using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerPayment : System.Web.UI.Page
    {
        //public EntityLayer.CommonELS.UserRightsForPage CustomerRights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CustRecPayBL blLayer = new CustRecPayBL();
        DataTable BatchGridData = new DataTable();
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        DataTable dtPartyTotal = null;
        string PartyTotalBalDesc = "";
        string PartyTotalBalAmt = "";
        CommonBL cbl = new CommonBL();

        protected void Page_Init(object sender, EventArgs e)
        {
            PaymentDetails.doc_type = "CRP";
            if (Request.QueryString["key"] != "Add")
            {
                PaymentDetails.StorePaymentDetailsToHiddenfield(Convert.ToInt32(Request.QueryString["id"]));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // CustomerRights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");

            //Rev Tanmoy
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string AllowProjectInDetailsLevel = cbl.GetSystemSettingsResult("AllowProjectInDetailsLevel");
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
                    grid.Columns[5].Width = 0;
                }
            }


            string ProjectMandatoryInEntry = cbl.GetSystemSettingsResult("ProjectMandatoryInEntry");

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

            if (!String.IsNullOrEmpty(AllowProjectInDetailsLevel))
            {
                if (AllowProjectInDetailsLevel.ToUpper().Trim() == "NO")
                {
                    hdnAllowProjectInDetailsLevel.Value = "0";
                    grid.Columns[5].Width = 0;
                }
            }
            //End Rev Tanmoy

            //For Hierarchy Start Tanmoy
            string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
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
                Session["VendorPayRecProjectCodefromDoc"] = null;
                hdnLocalCurrency.InnerText = Convert.ToString(Session["LastCompany"]);
                hdnLocalCurrency.InnerText = Convert.ToString(Session["LocalCurrency"]);

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End
                /*Rev Work Date:-21.03.2022 -Copy Function add*/
                //if (Request.QueryString["key"] == "Add")
                if (Request.QueryString["key"] == "Add" && Request.QueryString["key"] != "Copy")
                /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
                {
                    hdAddEdit.Value = "Add";
                    Keyval_internalId.Value = "Add";
                    //custBal.Style.Add("display", "block");
                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=5");

                    if (dtposTime != null && dtposTime.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                    }
                }
                    /*Rev Work Date:-21.03.2022 -Copy Function add*/
                else if (Request.QueryString["key"] != "Add" && Request.QueryString["key"] == "Copy")
                {
                    string Id = Convert.ToString(Request.QueryString["id"]);
                    DataSet dsEdit = blLayer.GetEditDetailsPayment(Id, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userid"]));

                    hdAddEdit.Value = "Copy";
                    DataTable dtPI = blLayer.GetEditProformaInvoice(Id);

                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=5");

                    if (dtposTime != null && dtposTime.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                    }

                    ddlProformaInvoice.TextField = "Quote_Number";
                    ddlProformaInvoice.ValueField = "Id";
                    ddlProformaInvoice.DataSource = dtPI;
                    ddlProformaInvoice.DataBind();

                    ddlProformaInvoice.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ProfomaInvoiceID"]);


                    ReceiptPaymentId.InnerText = Id;

                    ddlCashBank.TextField = "IntegrateMainAccount";
                    ddlCashBank.ValueField = "MainAccount_ReferenceID";
                    ddlCashBank.DataSource = dsEdit.Tables[11];
                    ddlCashBank.DataBind();

                    ddlBranch.TextField = "Branch_Description";
                    ddlBranch.ValueField = "ID";
                    ddlBranch.DataSource = dsEdit.Tables[7];
                    ddlBranch.DataBind();


                    ddlContactPerson.TextField = "contactperson";
                    ddlContactPerson.ValueField = "id";
                    ddlContactPerson.DataSource = dsEdit.Tables[10];
                    ddlContactPerson.DataBind();

                    ddlCurrency.TextField = "Currency_AlphaCode";
                    ddlCurrency.ValueField = "ID";
                    ddlCurrency.DataSource = dsEdit.Tables[6];
                    ddlCurrency.DataBind();


                    #region Header Load

                    hdnCustomerId.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_CustomerID"]);
                    ddlBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_BranchID"]);
                    dtTDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["ReceiptPayment_TransactionDate"]);
                    txtVoucherNo.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherNumber"]);
                    txtCustName.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["cntName"]);
                    ddlCashBank.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["MainAccount_ReferenceID"]);
                    ddlCurrency.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_Currency"]);
                    ddlContactPerson.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ContactPersonID"]);
                    hdnInstrumentType.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentType"]);
                    txtVoucherAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    hdnEnterBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]);
                    txtNarration.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_Narration"]);
                    txtRate.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_rate"]);
                    txtDrawnOn.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["DrawnOn"]);
                    PaymentDetails.Setbranchvalue(Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]));
                    rdl_Contact.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["contacttype"]);
                    txtInstNobth.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentNumber"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"])))
                    {
                        InstDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"]);
                    }
                    //Rev Tanmoy Project
                    string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]));
                    //End Rev Tanmoy Project

                    //Tanmoy  Hierarchy
                    BusinessLogicLayer.DBEngine oDBEngine1 = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine1.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                    //Tanmoy  Hierarchy End

                    IsUdfpresent.Value = Convert.ToString(dsEdit.Tables[12].Rows[0]["cnt"]);
                    Keyval_internalId.Value = "CustrRecePay" + Convert.ToString(Id);

                    rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["PaymentType"]);

                    DoEdit.Value = "1";
                    if (dsEdit.Tables[0].Rows.Count > 0 && Convert.ToString(dsEdit.Tables[0].Rows[0]["isexist"]) != "0")
                    {
                        tdSaveButtonNew.Style.Add("display", "none");
                        tdSaveButton.Style.Add("display", "none");
                        tagged.Style.Add("display", "block");
                        DoEdit.Value = "0";
                    }

                    #endregion


                    if (dsEdit.Tables[9] != null && dsEdit.Tables[9].Rows.Count > 0)
                    {
                        dtTDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearEndDate"]);
                        dtTDate.MinDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearStartDate"]);
                        InstDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearEndDate"]);
                        InstDate.MinDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearStartDate"]);
                    }
                    else
                    {
                        dtTDate.MaxDate = DateTime.Today;
                        dtTDate.MinDate = DateTime.Today;
                        InstDate.MaxDate = DateTime.Today;
                        InstDate.MinDate = DateTime.Today;
                    }

                    Sales_BillingShipping.SetBillingShippingTable(dsEdit.Tables[2]);

                    BatchGridData = dsEdit.Tables[1];
                    grid.DataBind();
                    hrCopy.Value = Request.QueryString["key"];
                }
                /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
                else if (Request.QueryString["key"] == "Edit")
                {
                    // lookup_Project.ClientEnabled = false;
                    //lookup_Project.ClearButton.Visibility = AutoBoolean.False;
                    string Id = Convert.ToString(Request.QueryString["id"]);
                    DataSet dsEdit = blLayer.GetEditDetailsPayment(Id, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userid"]));

                    hdAddEdit.Value = "Edit";

                    //custBal.Style.Add("display", "none");
                    DataTable dtPI = blLayer.GetEditProformaInvoice(Id);

                    ddlProformaInvoice.TextField = "Quote_Number";
                    ddlProformaInvoice.ValueField = "Id";
                    ddlProformaInvoice.DataSource = dtPI;
                    ddlProformaInvoice.DataBind();

                    ddlProformaInvoice.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ProfomaInvoiceID"]);


                    ReceiptPaymentId.InnerText = Id;

                    ddlCashBank.TextField = "IntegrateMainAccount";
                    ddlCashBank.ValueField = "MainAccount_ReferenceID";
                    ddlCashBank.DataSource = dsEdit.Tables[11];
                    ddlCashBank.DataBind();

                    ddlBranch.TextField = "Branch_Description";
                    ddlBranch.ValueField = "ID";
                    ddlBranch.DataSource = dsEdit.Tables[7];
                    ddlBranch.DataBind();


                    ddlContactPerson.TextField = "contactperson";
                    ddlContactPerson.ValueField = "id";
                    ddlContactPerson.DataSource = dsEdit.Tables[10];
                    ddlContactPerson.DataBind();

                    ddlCurrency.TextField = "Currency_AlphaCode";
                    ddlCurrency.ValueField = "ID";
                    ddlCurrency.DataSource = dsEdit.Tables[6];
                    ddlCurrency.DataBind();


                    #region Header Load

                    hdnCustomerId.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_CustomerID"]);
                    ddlBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_BranchID"]);
                    dtTDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["ReceiptPayment_TransactionDate"]);
                    txtVoucherNo.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherNumber"]);
                    txtCustName.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["cntName"]);
                    ddlCashBank.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["MainAccount_ReferenceID"]);
                    ddlCurrency.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_Currency"]);
                    ddlContactPerson.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ContactPersonID"]);
                    hdnInstrumentType.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentType"]);
                    txtVoucherAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    hdnEnterBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]);
                    txtNarration.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_Narration"]);
                    txtRate.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_rate"]);
                    txtDrawnOn.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["DrawnOn"]);
                    PaymentDetails.Setbranchvalue(Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]));
                    rdl_Contact.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["contacttype"]);
                    txtInstNobth.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentNumber"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"])))
                    {
                        InstDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"]);
                    }
                    //Rev Tanmoy Project
                    string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]));
                    //End Rev Tanmoy Project

                    //Tanmoy  Hierarchy
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                    //Tanmoy  Hierarchy End

                    IsUdfpresent.Value = Convert.ToString(dsEdit.Tables[12].Rows[0]["cnt"]);
                    Keyval_internalId.Value = "CustrRecePay" + Convert.ToString(Id);

                    rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["PaymentType"]);

                    DoEdit.Value = "1";
                    if (dsEdit.Tables[0].Rows.Count > 0 && Convert.ToString(dsEdit.Tables[0].Rows[0]["isexist"]) != "0")
                    {
                        tdSaveButtonNew.Style.Add("display", "none");
                        tdSaveButton.Style.Add("display", "none");
                        tagged.Style.Add("display", "block");
                        DoEdit.Value = "0";
                    }

                    #endregion


                    if (dsEdit.Tables[9] != null && dsEdit.Tables[9].Rows.Count > 0)
                    {
                        dtTDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearEndDate"]);
                        dtTDate.MinDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearStartDate"]);
                        InstDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearEndDate"]);
                        InstDate.MinDate = Convert.ToDateTime(dsEdit.Tables[9].Rows[0]["finYearStartDate"]);
                    }
                    else
                    {
                        dtTDate.MaxDate = DateTime.Today;
                        dtTDate.MinDate = DateTime.Today;
                        InstDate.MaxDate = DateTime.Today;
                        InstDate.MinDate = DateTime.Today;
                    }

                    Sales_BillingShipping.SetBillingShippingTable(dsEdit.Tables[2]);

                    BatchGridData = dsEdit.Tables[1];
                    grid.DataBind();

                }

            }
        }

        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Type")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
            else if(e.Column.FieldName == "DocumentNo")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "Payment" || e.Column.FieldName == "Remarks")
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
            if (hdnProjectSelectInEntryModule.Value == "1")
            {
                if (e.Column.FieldName == "Project_Code")
                {
                    e.Editor.ReadOnly = true;
                }
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
            AdjustmentTable.Columns.Add("Type", typeof(string));
            AdjustmentTable.Columns.Add("DocumentNo", typeof(string));
            AdjustmentTable.Columns.Add("Payment", typeof(string));
            AdjustmentTable.Columns.Add("Remarks", typeof(string));
            AdjustmentTable.Columns.Add("PaymentDetail_ID", typeof(string));
            AdjustmentTable.Columns.Add("IsOpening", typeof(string));
            AdjustmentTable.Columns.Add("ActualAmount", typeof(string));
            AdjustmentTable.Columns.Add("TypeId", typeof(string));
            AdjustmentTable.Columns.Add("DocId", typeof(string));
            AdjustmentTable.Columns.Add("UpdateEdit", typeof(string));
            AdjustmentTable.Columns.Add("ProjectId", typeof(Int64));
            AdjustmentTable.Columns.Add("Project_Code", typeof(string));

            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["Type"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    NewRow["Type"] = Convert.ToString(args.NewValues["Type"]);
                    NewRow["DocumentNo"] = Convert.ToString(args.NewValues["DocumentNo"]);
                    NewRow["Payment"] = Convert.ToString(args.NewValues["Payment"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["PaymentDetail_ID"] = Convert.ToString(args.NewValues["PaymentDetail_ID"]);
                    NewRow["IsOpening"] = Convert.ToString(args.NewValues["IsOpening"]);
                    NewRow["ActualAmount"] = Convert.ToString(args.NewValues["ActualAmount"]);
                    NewRow["TypeId"] = Convert.ToString(args.NewValues["TypeId"]);
                    NewRow["DocId"] = Convert.ToString(args.NewValues["DocId"]);
                    NewRow["UpdateEdit"] = Convert.ToString(args.NewValues["UpdateEdit"]);
                    NewRow["ProjectId"] = Convert.ToInt64(args.NewValues["ProjectId"]);
                    NewRow["Project_Code"] = Convert.ToString(args.NewValues["Project_Code"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["Type"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    NewRow["Type"] = Convert.ToString(args.NewValues["Type"]);
                    NewRow["DocumentNo"] = Convert.ToString(args.NewValues["DocumentNo"]);
                    NewRow["Payment"] = Convert.ToString(args.NewValues["Payment"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["PaymentDetail_ID"] = Convert.ToString(args.NewValues["PaymentDetail_ID"]);
                    NewRow["IsOpening"] = Convert.ToString(args.NewValues["IsOpening"]);
                    NewRow["ActualAmount"] = Convert.ToString(args.NewValues["ActualAmount"]);
                    NewRow["TypeId"] = Convert.ToString(args.NewValues["TypeId"]);
                    NewRow["DocId"] = Convert.ToString(args.NewValues["DocId"]);
                    NewRow["UpdateEdit"] = Convert.ToString(args.NewValues["UpdateEdit"]);
                    NewRow["ProjectId"] = Convert.ToInt64(args.NewValues["ProjectId"]);
                    NewRow["Project_Code"] = Convert.ToString(args.NewValues["Project_Code"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }



            foreach (var args in e.DeleteValues)
            {
                DataTable AdjTable = AdjustmentTable;
                if (AdjTable != null)
                {
                    string delId = Convert.ToString(args.Keys[0]);
                    DataRow[] AdjTableRow = AdjTable.Select("PaymentDetail_ID='" + delId + "'");
                    foreach (DataRow dr in AdjTableRow)
                    {
                        dr.Delete();
                    }

                    AdjustmentTable.AcceptChanges();
                }
            }
            BatchGridData = AdjustmentTable.Copy();
            RefetchSrlNo();

            string ActionType = hdAddEdit.Value;
            string EnterBranchID = hdnEnterBranch.Value;
            string CustomerPaymentId = ReceiptPaymentId.InnerText;
            string CashBankBranchID = Convert.ToString(ddlBranch.Value);
            string TransactionDate = dtTDate.Text;
            string CashBankID = Convert.ToString(ddlCashBank.Value);
            string ExchangeSegmentID = "1";
            string TransactionType = "P";
            string EntryUserProfile = "1";
            string VoucherAmount = txtVoucherAmount.Text;
            string CustomerId = hdnCustomerId.Value;
            string ContactName = Convert.ToString(ddlContactPerson.Value);
            string Narration = txtNarration.Text;
            string Currency = Convert.ToString(ddlCurrency.Value);
            string InstrumentType = Convert.ToString(cmbInstrumentType.Value);
            string InstrumentNumber = txtInstNobth.Text;
            string InstrumentDate = InstDate.Text;
            string rate = txtRate.Text;
            string DrawnOn = txtDrawnOn.Text;
            string CompanyId = Convert.ToString(Session["LastCompany"]);
            string LastFinYear = Convert.ToString(Session["LastFinYear"]);
            string userid = Convert.ToString(Session["userid"]);

            string SCHEMEID = Convert.ToString(CmbScheme.Value).Split('~')[0];
            string Doc_No = txtVoucherNo.Text;
            string ProformaInvoiceID = Convert.ToString(ddlProformaInvoice.Value);



            DataTable BillAddress = Sales_BillingShipping.GetBillingShippingTable();

            string paymenttype = rdl_MultipleType.SelectedValue;
            DataTable dtMultiType;
            if (paymenttype != "S")
            {
                dtMultiType = PaymentDetails.GetPaymentTable();

            }
            else
            {
                dtMultiType = blLayer.CreatePaymentDataTable();
            }

            //Rev Tanmoy Project
            CommonBL cbl = new CommonBL();
            string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

            Int64 ProjId = 0;
            if (lookup_Project.Text != "")
            {
                string projectCode = lookup_Project.Text;
                DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
            }
            else if (lookup_Project.Text == "")
            {
                ProjId = 0;
            }
            //End Rev Tanmoy Project

            string OutputId = "";

            string OutputText = blLayer.AddEditPayment(ref OutputId, ActionType, CustomerPaymentId, CashBankBranchID, TransactionDate, CashBankID, ExchangeSegmentID, TransactionType,
                           EntryUserProfile, VoucherAmount, CustomerId, ContactName,
                            Narration, ProjId, Currency, InstrumentType, InstrumentNumber, InstrumentDate, rate, "", BatchGridData, BillAddress, false, EnterBranchID, DrawnOn, CompanyId
                            , LastFinYear, userid, paymenttype, dtMultiType, SCHEMEID, Doc_No, ProformaInvoiceID);


            grid.JSProperties["cpInsert"] = OutputText + "~" + OutputId + "~" + hdnRefreshType.Value;


            DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
            if (udfTable != null)
            {
                Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("CRP", "CustrRecePay" + Convert.ToString(OutputId), udfTable, Convert.ToString(Session["userid"]));
            }

            //if (string.IsNullOrEmpty(OutputId) || Convert.ToInt32(OutputId) > 0)
            if (Convert.ToInt32(OutputId) > 0)
            {
                BatchGridData.Rows.Clear();
                BatchGridData.AcceptChanges();
            }
            else if (Convert.ToInt32(OutputId) <= 0 && Convert.ToInt32(OutputId) == -9)
            {
                DataTable dt = new DataTable();
                dt = GetAddLockStatus(TransactionType);
                grid.JSProperties["cpAddLockStatus"] = (Convert.ToString(dt.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dt.Rows[0]["Lock_Todate"]));
            }
            grid.DataBind();

            e.Handled = true;
        }
        public DataTable GetAddLockStatus(string type)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForPaymentNote");

            dt = proc.GetTable();
            return dt;

        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            grid.JSProperties["cpGridBlank"] = "";
            if(strSplitCommand=="GridBlank")
            {
                BatchGridData.Rows.Clear();
                BatchGridData.AcceptChanges();
                //RefetchSrlNo();
                grid.JSProperties["cpGridBlank"] = "GridBlank";
            }
            grid.DataBind();

        }
        public DataTable GetProjectCodeDetailsOnDocument(string Type, String DocNo)
        {
            //string ReceiptPayment_ID = Convert.ToString(Session["VendorReceiptPayment_ID"]);

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@DocType", 100, Type);
            proc.AddVarcharPara("@DocNo", 20, DocNo);
            proc.AddVarcharPara("@BranchId", 20, ddlBranch.Value.ToString());
            proc.AddVarcharPara("@InternalId", 100, hdnCustomerId.Value);
            //proc.AddVarcharPara("@Receiptdate", 50, Convert.ToString(receiptdate));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectEditData(string ReceiptPayID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPaymentReciptProjectID");
            proc.AddIntegerPara("@Receipt_ID", Convert.ToInt32(ReceiptPayID));
            proc.AddVarcharPara("@Action", 100, "CustomerReceiptPayment");
            dt = proc.GetTable();
            return dt;
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

        [WebMethod]
        public static string GetTotalBalanceByCashBankID(string CashBankID, string CustomerPaymentID, String PostingDate)
        {
            string VoucherAmount = "0.00", BalanceLimit = "0.00", BalanceExceed = "", ClosingAmt = "0.00";

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "GetTotalBalanceByCashBankID");
            proc.AddVarcharPara("@CashBankID", 200, CashBankID);
            proc.AddVarcharPara("@Payment_ID", 200, CustomerPaymentID);
            proc.AddVarcharPara("@PostingDate", 10, PostingDate);
            proc.AddVarcharPara("@CompanyID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddIntegerPara("@UserId", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@userbranchHierarchy", 50, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            DT = proc.GetTable();


            if (DT.Rows.Count > 0)
            {
                VoucherAmount = Convert.ToString(DT.Rows[0]["VoucherAmount"]);

                BalanceLimit = Convert.ToString(DT.Rows[1]["Cash_Bank_BalanceLimit"]);
                BalanceExceed = Convert.ToString(DT.Rows[1]["NegativeStock"]);
                ClosingAmt = Convert.ToString(DT.Rows[0]["ClosingAmt"]);

            }

            return Convert.ToString(VoucherAmount + "~" + BalanceLimit + "~" + BalanceExceed + "~" + ClosingAmt);
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            #region Block By Sudip
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            string strToDate = Convert.ToString(hddnAsOnDate.Value);
            e.KeyExpression = "SLNO";

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SLNO) != "999999999" && Convert.ToString(d.PARTYTYPE) == "C"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }


            #endregion
            CustomerOutstanding.ExpandAll();

        }


        protected void ProjectCodeCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            DataTable dtDocFroProject = new DataTable();
            string strType = e.Parameter.Split('~')[1];
            string DocNo = e.Parameter.Split('~')[2];
            dtDocFroProject = GetProjectCodeDetailsOnDocument(strType, DocNo);
            Session["VendorPayRecProjectCodefromDoc"] = dtDocFroProject;
            if (dtDocFroProject != null && dtDocFroProject.Rows.Count > 0)
            {
                lookupPopup_ProjectCode.DataSource = dtDocFroProject;
                lookupPopup_ProjectCode.DataBind();
            }
            else
            {
                lookupPopup_ProjectCode.DataSource = null;
                lookupPopup_ProjectCode.DataBind();
            }

        }
        protected void lookup_ProjectCode_DataBinding(object sender, EventArgs e)
        {
            DataTable dsdata = (DataTable)Session["VendorPayRecProjectCodefromDoc"];
            lookupPopup_ProjectCode.DataSource = dsdata;
        }


        protected void ShowGridCustOut_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                dtPartyTotal = oDBEngine.GetDataTable("Select DOC_TYPE,BAL_AMOUNT from PARTYOUTSTANDINGDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999999 AND DOC_TYPE='Gross Outstanding:' AND PARTYTYPE='C'");
                PartyTotalBalDesc = dtPartyTotal.Rows[0][0].ToString();
                PartyTotalBalAmt = dtPartyTotal.Rows[0][1].ToString();

            }
            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_BalAmt":
                        e.Text = PartyTotalBalAmt;
                        break;
                    case "Item_DocType":
                        e.Text = PartyTotalBalDesc;
                        break;
                }
            }

        }
        protected void cCustomerOutstanding_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strAsOnDate = Convert.ToString(e.Parameters.Split('~')[3]);
            string strCustomerId = Convert.ToString(e.Parameters.Split('~')[1]);
            string BranchId = e.Parameters.Split('~')[2];
            string strCommand = Convert.ToString(e.Parameters.Split('~')[0]);
            DataTable dtOutStanding = new DataTable();
            if (strCommand == "BindOutStanding")
            {
                dtOutStanding = objSlaesActivitiesBL.GetCustomerOutstandingRecords(strAsOnDate, strCustomerId, BranchId);

                //CustomerOutstanding.DataSource = dtOutStanding;
                //CustomerOutstanding.DataBind();
                CustomerOutstanding.JSProperties["cpOutStanding"] = "OutStanding";


            }
        }

        protected void ShowGridCustOut_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Doc. Type")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }
        protected void ShowGridCustOut_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (Convert.ToString(e.CellValue) == "Party Outstanding:" || Convert.ToString(e.CellValue) == "Total:")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = System.Drawing.Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "BAL_AMOUNT")
            {
                Session["chk_presenttotal"] = 0;
            }
        }
        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval1"] == null)
                {
                    Session["exportval1"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval1"]) != Filter)
                {
                    Session["exportval1"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            //CustomerOutstanding.Columns[5].Visible = false;
            string filename = "CustomerOutStanding";
            exporter1.FileName = filename;
            exporter1.FileName = "GrdCustomerOutstanding";

            exporter1.PageHeader.Left = "CustomerOutStanding";
            exporter1.PageFooter.Center = "[Page # of Pages #]";
            exporter1.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter1.WritePdfToResponse();
                    break;
                case 2:
                    exporter1.WriteXlsToResponse();
                    break;
                case 3:
                    exporter1.WriteRtfToResponse();
                    break;
                case 4:
                    exporter1.WriteCsvToResponse();
                    break;
            }
        }

        protected void ProjectServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = "1";
            if (ddlBranch.Value != null)
            {
                branch = ddlBranch.Value.ToString();
            }
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            // Mantis Issue 24976
            //var q = from d in dc.V_ProjectLists
            //        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch) && d.CustId == Convert.ToString(hdnCustomerId.Value)
            //        orderby d.Proj_Id descending
            //        select d;

            //e.QueryableSource = q;

            CommonBL cbl = new CommonBL();
            string ISProjectIndependentOfBranch = cbl.GetSystemSettingsResult("AllowProjectIndependentOfBranch");

            if (ISProjectIndependentOfBranch == "No")
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch) && d.CustId == Convert.ToString(hdnCustomerId.Value)
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.CustId == Convert.ToString(hdnCustomerId.Value)
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            // End of Mantis Issue 24976
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