using BusinessLogicLayer;
using DevExpress.Web.Data;
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
    public partial class InfluencerPayment : System.Web.UI.Page
    {
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        InfluencerBL blLayer = new InfluencerBL();
        DataTable BatchGridData = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CommonBL cbl = new CommonBL();
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //PaymentDetails.doc_type = "CRP";
            //if (Request.QueryString["key"] != "Add")
            //{
            //    PaymentDetails.StorePaymentDetailsToHiddenfield(Convert.ToInt32(Request.QueryString["id"]));
            //}



        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //InfluencerRights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/InfluencerMasterList.aspx");
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
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
            if (!IsPostBack)
            {

                hdnLocalCurrency.InnerText = Convert.ToString(Session["LastCompany"]);
                hdnLocalCurrency.InnerText = Convert.ToString(Session["LocalCurrency"]);

                bindHierarchy();
                ddlHierarchy.Enabled = false;

                if (Request.QueryString["key"] == "Add")
                {
                    hdAddEdit.Value = "Add";
                    Keyval_internalId.Value = "Add";
                }
                else if (Request.QueryString["key"] == "Edit")
                {
                    string Id = Convert.ToString(Request.QueryString["id"]);
                    DataSet dsEdit = blLayer.GetEditDetails(Id, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userid"]));

                    hdAddEdit.Value = "Edit";
                    if (Request.QueryString["req"] == "View")
                    {
                        btnSaveRecords.ClientVisible = false;
                        btnSaveRecords.ClientVisible = false;
                    }               

                    PaymentId.InnerText = Id;

                    ddlCashBank.TextField = "IntegrateMainAccount";
                    ddlCashBank.ValueField = "MainAccount_ReferenceID";
                    ddlCashBank.DataSource = dsEdit.Tables[8];
                    ddlCashBank.DataBind();

                    ddlBranch.TextField = "Branch_Description";
                    ddlBranch.ValueField = "ID";
                    ddlBranch.DataSource = dsEdit.Tables[4];
                    ddlBranch.DataBind();


                    ddlContactPerson.TextField = "contactperson";
                    ddlContactPerson.ValueField = "id";
                    ddlContactPerson.DataSource = dsEdit.Tables[7];
                    ddlContactPerson.DataBind();

                    ddlCurrency.TextField = "Currency_AlphaCode";
                    ddlCurrency.ValueField = "ID";
                    ddlCurrency.DataSource = dsEdit.Tables[3];
                    ddlCurrency.DataBind();

                    ddl_tdsSection.TextField = "tdsdescription";
                    ddl_tdsSection.ValueField = "tdscode";
                    ddl_tdsSection.DataSource = dsEdit.Tables[9];
                    ddl_tdsSection.DataBind();
                    	




                    #region Header Load

                    hdnInfluencerId.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_InfluencerID"]);
                    ddlBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_BranchID"]);
                    dtTDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["Payment_TransactionDate"]);
                    txtVoucherNo.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_VoucherNumber"]);
                    txtCustName.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["cntName"]);
                    ddlCashBank.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["MainAccount_ReferenceID"]);
                    ddlCurrency.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_Currency"]);
                    ddlContactPerson.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ContactPersonID"]);
                    hdnInstrumentType.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_InstrumentType"]);

                    txtInstNobth.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_InstrumentNumber"]);

                    txtVoucherAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_PayableAmount"]);
                    txtActualVoucherAmount.Text=Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_VoucherAmount"]);

                    //rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_VoucherAmount"]);

                    hdnEnterBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]);
                    txtNarration.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_Narration"]);
                    txtRate.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_rate"]);
                    txtDrawnOn.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_rate"]);

                    hdnMainAccountId.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_MainAccount"]);
                    btnMARoundOff.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["MAName"]);
                    txtMainAccountAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_MainAccountAmount"]);
                    
                   ddl_tdsSection.Value=Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_TdsSection"]);
                   //Add Nil Rate TDS Tanmoy 01-12-2020
                   Boolean NILRateTDS = string.IsNullOrEmpty(dsEdit.Tables[0].Rows[0]["IsNilRated"].ToString()) ? false : Convert.ToBoolean(dsEdit.Tables[0].Rows[0]["IsNilRated"]);
                   
                   if (NILRateTDS==false)
                   {
                       txtTdsAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_TDSamount"]);
                   }
                   //txtTdsAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_TDSamount"]);
                   chkNILRateTDS.Checked = NILRateTDS;
                   //Add Nil Rate TDS Tanmoy 01-12-2020
                    if (!string.IsNullOrEmpty(Convert.ToString(dsEdit.Tables[0].Rows[0]["Payment_InstrumentDate"])))
                    {
                        InstDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["Payment_InstrumentDate"]);
                    }
                    hdnTDSRate.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["TDSPercentage"]);

                    //PaymentDetails.Setbranchvalue(Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]));
                    rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["PaymentType"]);

                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]));
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }                 

                    DoEdit.Value = "1";
                    if (dsEdit.Tables[0].Rows.Count > 0 && Convert.ToString(dsEdit.Tables[0].Rows[0]["isexist"]) != "0")
                    {
                        tdSaveButtonNew.Style.Add("display", "none");
                        tdSaveButton.Style.Add("display", "none");
                        tagged.Style.Add("display", "block");
                        DoEdit.Value = "0";
                    }



                    #endregion





                    //Sales_BillingShipping.SetBillingShippingTable(dsEdit.Tables[2]);

                    BatchGridData = dsEdit.Tables[1];
                    grid.DataBind();

                }

            }
        }

        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Type" || e.Column.FieldName == "DocumentNo")
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
            string InfluencerPaymentId = PaymentId.InnerText;
            string CashBankBranchID = Convert.ToString(ddlBranch.Value);
            string TransactionDate = dtTDate.Text;
            string CashBankID = Convert.ToString(ddlCashBank.Value);
            string ExchangeSegmentID = "1";
            string TransactionType = "P";
            string EntryUserProfile = "1";
            string VoucherAmount = txtVoucherAmount.Text;
            string InfluencerId = hdnInfluencerId.Value;
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

            string MainAccountId = hdnMainAccountId.Value;
            string MainAccountAmount = txtMainAccountAmount.Text;

            string tdsSection = Convert.ToString(ddl_tdsSection.Value);
            string tdsAmount = txtTdsAmount.Text;
            string ActualVoucherAmount = txtActualVoucherAmount.Text;

            //Nil Rate TDS add Tanmoy 01-12-2020
            Boolean NILRateTDS = chkNILRateTDS.Checked;
            //Nil Rate TDS add Tanmoy 01-12-2020

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


            string OutputId = "";

            string OutputText = blLayer.AddEditPayment(ref OutputId, ActionType, InfluencerPaymentId, CashBankBranchID, TransactionDate, CashBankID, ExchangeSegmentID, TransactionType,
                           EntryUserProfile, VoucherAmount, InfluencerId, ContactName,
                            Narration, Currency, InstrumentType, InstrumentNumber, InstrumentDate, rate,  BatchGridData,  EnterBranchID, DrawnOn, CompanyId
                            , LastFinYear, userid, SCHEMEID, Doc_No, MainAccountId, MainAccountAmount, tdsSection, tdsAmount, ActualVoucherAmount, ProjId, NILRateTDS);


            grid.JSProperties["cpInsert"] = OutputText + "~" + OutputId + "~" + hdnRefreshType.Value;


            DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
            if (udfTable != null)
            {
                Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("CRP", "CustrRecePay" + Convert.ToString(OutputId), udfTable, Convert.ToString(Session["userid"]));
            }


            e.Handled = true;

            if (string.IsNullOrEmpty(OutputId) || Convert.ToInt32(OutputId) > 0)
            {
                BatchGridData.Rows.Clear();
                BatchGridData.AcceptChanges();
            }
            grid.DataBind();
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

        protected void ProjectServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = Convert.ToString(ddlBranch.Value);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;
        }
        //End Rev


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


    }
}
