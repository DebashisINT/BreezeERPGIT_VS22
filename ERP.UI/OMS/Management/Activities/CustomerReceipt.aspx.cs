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
using System.Web.Script.Services;
using System.Threading.Tasks;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerReceipt : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CustRecPayBL blLayer = new CustRecPayBL();
        DataTable BatchGridData = new DataTable();
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        DataTable dtPartyTotal = null;
        string PartyTotalBalDesc = "";
        string PartyTotalBalAmt = "";
        CommonBL cbl = new CommonBL();
        MasterSettings objmaster = new MasterSettings();

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
            PaymentDetails.doc_type = "CRP";
            if (Request.QueryString["key"] != "Add")
            {
                PaymentDetails.StorePaymentDetailsToHiddenfield(Convert.ToInt32(Request.QueryString["id"]));
            }


           
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");

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
                hdnDocumentSegmentSettings.Value = objmaster.GetSettings("DocumentSegment");

                if (hdnDocumentSegmentSettings.Value == "0")
                {
                    DivSegment1.Attributes.Add("style", "display:none");
                    DivSegment2.Attributes.Add("style", "display:none");
                    DivSegment3.Attributes.Add("style", "display:none");
                    DivSegment4.Attributes.Add("style", "display:none");
                    DivSegment5.Attributes.Add("style", "display:none");
                }
                Session["VendorPayRecProjectCodefromDoc"] = null;
                Session["QuotationData"] = null;
                //20-05-2019 Surojit
                hidIsLigherContactPage.Value = "0";
                //IsLighterCustomePage = "";
                //CommonBL cbl = new CommonBL();
                string ISLigherpage = cbl.GetSystemSettingsResult("LighterCustomerEntryPage");
                if (!String.IsNullOrEmpty(ISLigherpage))
                {
                    if (ISLigherpage == "Yes")
                    {
                        hidIsLigherContactPage.Value = "1";
                        //IsLighterCustomePage = "1";
                    }
                }
                //20-05-2019 Surojit
               
                hdnLocalCurrency.InnerText = Convert.ToString(Session["LastCompany"]);
                hdnLocalCurrency.InnerText = Convert.ToString(Session["LocalCurrency"]);

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End


                /*Rev Work Date:-21.03.2022 -Copy Function add*/
               // if (Request.QueryString["key"] == "Add")
                if (Request.QueryString["key"] == "Add" && Request.QueryString["key"] != "Copy")
                /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
                {
                    hdAddEdit.Value = "Add";
                    Keyval_internalId.Value = "Add";
                    //custBal.Style.Add("display", "block");
                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=4");

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
                    DataSet dsEdit = blLayer.GetEditDetails(Id, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userid"]));

                    hdAddEdit.Value = "Copy";
                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=4");

                    if (dtposTime != null && dtposTime.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                    }
                   
                    DataTable dtPI = blLayer.GetEditProformaInvoice(Id);

                    if (dtPI.Rows.Count > 0)
                    {
                        lookup_ProformaInvoice.DataSource = dtPI;
                        lookup_ProformaInvoice.DataBind();
                        Session["QuotationData"] = dtPI;

                        lookup_ProformaInvoice.GridView.Selection.SelectRowByKey(Convert.ToInt32(dsEdit.Tables[0].Rows[0]["ProfomaInvoiceID"]));
                    }


                    ReceiptPaymentId.InnerText = Id;

                    ddlCashBank.TextField = "IntegrateMainAccount";
                    ddlCashBank.ValueField = "MainAccount_ReferenceID";
                    ddlCashBank.DataSource = dsEdit.Tables[12];
                    ddlCashBank.DataBind();

                    ddlBranch.TextField = "Branch_Description";
                    ddlBranch.ValueField = "ID";
                    ddlBranch.DataSource = dsEdit.Tables[8];
                    ddlBranch.DataBind();


                    ddlContactPerson.TextField = "contactperson";
                    ddlContactPerson.ValueField = "id";
                    ddlContactPerson.DataSource = dsEdit.Tables[11];
                    ddlContactPerson.DataBind();

                    ddlCurrency.TextField = "Currency_AlphaCode";
                    ddlCurrency.ValueField = "ID";
                    ddlCurrency.DataSource = dsEdit.Tables[7];
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

                    txtInstNobth.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentNumber"]);

                    txtVoucherAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    hdnEnterBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]);
                    txtNarration.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_Narration"]);
                    txtRate.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_rate"]);
                    txtDrawnOn.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["DrawnOn"]);
                    CB_GSTApplicable.Checked = Convert.ToBoolean(dsEdit.Tables[0].Rows[0]["GSTApplicable"]);
                    rdl_Contact.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["contacttype"]);

                    //////////////////  TCS section  /////////////////////////
                    string strTCScode = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSSection"]);
                    string strTCSappl = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSApplAmount"]);
                    string strTCSpercentage = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSPercentage"]);
                    string strTCSamout = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSAmount"]);

                    txtTCSSection.Text = Convert.ToString(strTCScode);
                    txtTCSapplAmount.Text = Convert.ToString(strTCSappl);
                    txtTCSpercentage.Text = Convert.ToString(strTCSpercentage);
                    txtTCSAmount.Text = Convert.ToString(strTCSamout);
                    //////////////////////////////////////////////////////////

                    string Segment1 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment1"]);
                    string Segment2 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment2"]);
                    string Segment3 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment3"]);
                    string Segment4 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment4"]);
                    string Segment5 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment5"]);

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


                    string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]));
                    
                    //Tanmoy  Hierarchy
                    BusinessLogicLayer.DBEngine oDBEngine1 = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine1.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                    //Tanmoy  Hierarchy End

                    if (!string.IsNullOrEmpty(Convert.ToString(dsEdit.Tables[5].Rows[0]["productIds"])))
                    {
                        btnProduct.Text = Convert.ToString(dsEdit.Tables[5].Rows[0]["productsname"]);
                        hdnProductId.Value = Convert.ToString(dsEdit.Tables[5].Rows[0]["productIds"]);
                        hdnHSNId.Value = Convert.ToString(dsEdit.Tables[5].Rows[0]["HsnCode"]);
                        hdtHsnCode.Value = Convert.ToString(dsEdit.Tables[5].Rows[0]["HsnCode"]);
                        btnProduct.ClientEnabled = true;
                    }

                    IsUdfpresent.Value = Convert.ToString(dsEdit.Tables[14].Rows[0]["cnt"]);
                    Keyval_internalId.Value = "CustrRecePay" + Convert.ToString(Id);

                    if (!string.IsNullOrEmpty(Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"])))
                    {
                        InstDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"]);
                    }


                    PaymentDetails.Setbranchvalue(Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]));
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

                    if (dsEdit.Tables[10] != null && dsEdit.Tables[10].Rows.Count > 0)
                    {
                        dtTDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearEndDate"]);
                        dtTDate.MinDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearStartDate"]);
                        InstDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearEndDate"]);
                        InstDate.MinDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearStartDate"]);
                    }
                    else
                    {
                        dtTDate.MaxDate = DateTime.Today;
                        dtTDate.MinDate = DateTime.Today;
                        InstDate.MaxDate = DateTime.Today;
                        InstDate.MinDate = DateTime.Today;
                    }



                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dsEdit.Tables[13].Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dsEdit.Tables[13].Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsonProducts.InnerText = serializer.Serialize(rows);

                    Sales_BillingShipping.SetBillingShippingTable(dsEdit.Tables[2]);

                    BatchGridData = dsEdit.Tables[1];
                    grid.DataBind();

                    hrCopy.Value = Request.QueryString["key"];
                }
                /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
                else if (Request.QueryString["key"] == "Edit")
                {
                    //lookup_Project.ClientEnabled = false;
                    //lookup_Project.ClearButton.Visibility = AutoBoolean.False;
                    string Id=Convert.ToString(Request.QueryString["id"]);
                    DataSet dsEdit = blLayer.GetEditDetails(Id, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["userid"]));

                    hdAddEdit.Value = "Edit";
                    //string postingDate=dtTDate.Date.ToString("yyyy-MM-dd");

                    DataTable dtPI = blLayer.GetEditProformaInvoice(Id);

                    //ddlProformaInvoice.TextField = "Quote_Number";
                    //ddlProformaInvoice.ValueField = "Id";
                    //ddlProformaInvoice.DataSource = dtPI;
                    //ddlProformaInvoice.DataBind();

                    if (dtPI.Rows.Count > 0)
                    {
                        lookup_ProformaInvoice.DataSource = dtPI;
                        lookup_ProformaInvoice.DataBind();
                        Session["QuotationData"] = dtPI;

                        lookup_ProformaInvoice.GridView.Selection.SelectRowByKey(Convert.ToInt32(dsEdit.Tables[0].Rows[0]["ProfomaInvoiceID"]));
                    }
                    

                    ReceiptPaymentId.InnerText = Id;

                    ddlCashBank.TextField = "IntegrateMainAccount";
                    ddlCashBank.ValueField = "MainAccount_ReferenceID";
                    ddlCashBank.DataSource = dsEdit.Tables[12];
                    ddlCashBank.DataBind();

                    ddlBranch.TextField = "Branch_Description";
                    ddlBranch.ValueField = "ID";
                    ddlBranch.DataSource = dsEdit.Tables[8];
                    ddlBranch.DataBind();


                    ddlContactPerson.TextField = "contactperson";
                    ddlContactPerson.ValueField = "id";
                    ddlContactPerson.DataSource = dsEdit.Tables[11];
                    ddlContactPerson.DataBind();

                    ddlCurrency.TextField = "Currency_AlphaCode";
                    ddlCurrency.ValueField = "ID";
                    ddlCurrency.DataSource = dsEdit.Tables[7];
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

                    txtInstNobth.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentNumber"]);

                    txtVoucherAmount.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    rdl_MultipleType.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_VoucherAmount"]);
                    hdnEnterBranch.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]);
                    txtNarration.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_Narration"]);
                    txtRate.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_rate"]);
                    txtDrawnOn.Text = Convert.ToString(dsEdit.Tables[0].Rows[0]["DrawnOn"]);
                    CB_GSTApplicable.Checked = Convert.ToBoolean(dsEdit.Tables[0].Rows[0]["GSTApplicable"]);
                    rdl_Contact.SelectedValue = Convert.ToString(dsEdit.Tables[0].Rows[0]["contacttype"]);

                    //////////////////  TCS section  /////////////////////////
                    string strTCScode = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSSection"]);
                    string strTCSappl = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSApplAmount"]);
                    string strTCSpercentage = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSPercentage"]);
                    string strTCSamout = Convert.ToString(dsEdit.Tables[0].Rows[0]["TCSAmount"]);

                    txtTCSSection.Text = Convert.ToString(strTCScode);
                    txtTCSapplAmount.Text = Convert.ToString(strTCSappl);
                    txtTCSpercentage.Text = Convert.ToString(strTCSpercentage);
                    txtTCSAmount.Text = Convert.ToString(strTCSamout);
                    //////////////////////////////////////////////////////////


                    //string Segment1Name = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment1Name"]);
                    //string Segment2Name = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment2Name"]);
                    //string Segment3Name = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment3Name"]);
                    //string Segment4Name = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment4Name"]);
                    //string Segment5Name = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment5Name"]);

                    string Segment1 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment1"]);
                    string Segment2 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment2"]);
                    string Segment3 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment3"]);
                    string Segment4 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment4"]);
                    string Segment5 = Convert.ToString(dsEdit.Tables[0].Rows[0]["Segment5"]);

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


                    string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]));

                   // ddlProformaInvoice.Value = Convert.ToString(dsEdit.Tables[0].Rows[0]["ProfomaInvoiceID"]);

                    //Tanmoy  Hierarchy
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dsEdit.Tables[0].Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                    //Tanmoy  Hierarchy End

                    if (!string.IsNullOrEmpty(Convert.ToString(dsEdit.Tables[5].Rows[0]["productIds"])))
                    {
                        btnProduct.Text = Convert.ToString(dsEdit.Tables[5].Rows[0]["productsname"]);
                        hdnProductId.Value = Convert.ToString(dsEdit.Tables[5].Rows[0]["productIds"]);
                        hdnHSNId.Value = Convert.ToString(dsEdit.Tables[5].Rows[0]["HsnCode"]);
                        hdtHsnCode.Value = Convert.ToString(dsEdit.Tables[5].Rows[0]["HsnCode"]);
                        btnProduct.ClientEnabled = true;
                    }

                    IsUdfpresent.Value = Convert.ToString(dsEdit.Tables[14].Rows[0]["cnt"]);
                    Keyval_internalId.Value = "CustrRecePay" + Convert.ToString(Id);

                    if (!string.IsNullOrEmpty(Convert.ToString(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"])))
                    {
                        InstDate.Date = Convert.ToDateTime(dsEdit.Tables[0].Rows[0]["ReceiptPayment_InstrumentDate"]);
                    }


                    PaymentDetails.Setbranchvalue(Convert.ToString(dsEdit.Tables[0].Rows[0]["EnteredBranchID"]));
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


                    if (dsEdit.Tables[10] != null && dsEdit.Tables[10].Rows.Count > 0)
                    {
                        dtTDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearEndDate"]);
                        dtTDate.MinDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearStartDate"]);
                        InstDate.MaxDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearEndDate"]);
                        InstDate.MinDate = Convert.ToDateTime(dsEdit.Tables[10].Rows[0]["finYearStartDate"]);
                    }
                    else
                    {
                        dtTDate.MaxDate = DateTime.Today;
                        dtTDate.MinDate = DateTime.Today;
                        InstDate.MaxDate = DateTime.Today;
                        InstDate.MinDate = DateTime.Today;
                    }



                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dsEdit.Tables[13].Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dsEdit.Tables[13].Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsonProducts.InnerText= serializer.Serialize(rows);

                    Sales_BillingShipping.SetBillingShippingTable(dsEdit.Tables[2]);

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
            else if (e.Column.FieldName == "Receipt" || e.Column.FieldName == "Remarks")
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
            if(hdnProjectSelectInEntryModule.Value=="1")
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

        //chinmoy added for inline project code start 10-12-2019
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

        //chinmoy added for projecvt code start 10-12-2019
        protected void lookup_ProjectCode_DataBinding(object sender, EventArgs e)
        {
            DataTable dsdata = (DataTable)Session["VendorPayRecProjectCodefromDoc"];
            lookupPopup_ProjectCode.DataSource = dsdata;
        }

        //End



        protected void grid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            DataTable AdjustmentTable = new DataTable();
            AdjustmentTable.Columns.Add("Type", typeof(string));
            AdjustmentTable.Columns.Add("DocumentNo", typeof(string));
            AdjustmentTable.Columns.Add("Receipt", typeof(string));
            AdjustmentTable.Columns.Add("Remarks", typeof(string));
            AdjustmentTable.Columns.Add("ReceiptDetail_ID", typeof(string));
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
                    NewRow["Receipt"] = Convert.ToString(args.NewValues["Receipt"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["ReceiptDetail_ID"] = Convert.ToString(args.NewValues["ReceiptDetail_ID"]);
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
                    NewRow["Receipt"] = Convert.ToString(args.NewValues["Receipt"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["ReceiptDetail_ID"] = Convert.ToString(args.NewValues["ReceiptDetail_ID"]);
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
                    DataRow[] AdjTableRow = AdjTable.Select("ReceiptDetail_ID='" + delId + "'");
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
            string CustomerReceiptId = ReceiptPaymentId.InnerText;
            string CashBankBranchID = Convert.ToString(ddlBranch.Value);
            string TransactionDate = dtTDate.Text;
            string CashBankID = Convert.ToString(ddlCashBank.Value);
            string ExchangeSegmentID = "1";
            string TransactionType = "R";
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
            string userid=Convert.ToString(Session["userid"]);

            string SCHEMEID = Convert.ToString(CmbScheme.Value).Split('~')[0];
            string Doc_No = txtVoucherNo.Text;
            string Product_IDS = Convert.ToString(hdnProductId.Value);

            //////////////////  TCS section  /////////////////////////
            string strTCScode = Convert.ToString(txtTCSSection.Text);
            string strTCSappl = Convert.ToString(txtTCSapplAmount.Text);
            string strTCSpercentage = Convert.ToString(txtTCSpercentage.Text);
            string strTCSamout = Convert.ToString(txtTCSAmount.Text);
            //////////////////////////////////////////////////////////



           // string ProformaInvoiceID = Convert.ToString(ddlProformaInvoice.Value);

            string ProformaInvoiceID = Convert.ToString(lookup_ProformaInvoice.Value);


            DataTable BillAddress = Sales_BillingShipping.GetBillingShippingTable();
            Boolean GSTApplicable = CB_GSTApplicable.Checked;


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

            Int64 ProjId=0;
            //String OrderComponent = "";

            //List<object> QuoList = lookup_Project.GridView.GetSelectedFieldValues("Proj_Id");

            //foreach (object Quo in QuoList)
            //{
            //    OrderComponent += "," + Quo;
            //}
            //OrderComponent = OrderComponent.TrimStart(',');

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

            string Segment1 = Convert.ToString(hdnSegment1.Value);
            string Segment2 = Convert.ToString(hdnSegment2.Value);
            string Segment3 = Convert.ToString(hdnSegment3.Value);
            string Segment4 = Convert.ToString(hdnSegment4.Value);
            string Segment5 = Convert.ToString(hdnSegment5.Value);

            string OutputId = "";

         string OutputText=  blLayer.AddEditReceipt(ref OutputId, ActionType, CustomerReceiptId, CashBankBranchID, TransactionDate, CashBankID, ExchangeSegmentID, TransactionType,
                        EntryUserProfile, VoucherAmount, CustomerId, ContactName,
                         Narration,ProjId, Currency, InstrumentType, InstrumentNumber, InstrumentDate, rate, Product_IDS, BatchGridData, BillAddress, GSTApplicable, EnterBranchID, DrawnOn, CompanyId
                         , LastFinYear, userid, paymenttype, dtMultiType, SCHEMEID, Doc_No, ProformaInvoiceID, strTCScode, strTCSappl
                                    , strTCSpercentage, strTCSamout, Segment1, Segment2, Segment3,Segment4, Segment5);


            grid.JSProperties["cpInsert"] = OutputText + "~" + OutputId+"~"+hdnRefreshType.Value;


            DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
            if (udfTable != null)
            {
                Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("CRP", "CustrRecePay" + Convert.ToString(OutputId), udfTable, Convert.ToString(Session["userid"]));
            }


            e.Handled = true;

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
        }
        public DataTable GetAddLockStatus(string type)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForReceiptNote");

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


        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            grid.JSProperties["cpGridBlank"] = "";
            if (strSplitCommand == "GridBlank")
            {
                BatchGridData.Rows.Clear();
                BatchGridData.AcceptChanges();
                //RefetchSrlNo();
                grid.JSProperties["cpGridBlank"] = "GridBlank";
            }
            grid.DataBind();

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

        protected void LinqServerModeDataSourceProject_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = "0";
            if (ddlBranch.Value!=null)
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

        protected void CBPProformaInvoice_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["QuotationData"] = null;
            string CustomerId = e.Parameter.Split('~')[0];
            string AsOnDate = e.Parameter.Split('~')[1];
            CustRecPayBL objCustRecPayBL = new CustRecPayBL();
            DataTable dtCB = new DataTable();
            string CompanyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            dtCB = objCustRecPayBL.PopulateProformaInvoice(CustomerId, AsOnDate);
            if (dtCB.Rows.Count > 0)
            {
                lookup_ProformaInvoice.DataSource = dtCB;
                lookup_ProformaInvoice.DataBind();
                Session["QuotationData"] = dtCB;
            }
        }
        protected void lookup_ProformaInvoice_DataBinding(object sender, EventArgs e)
        {
            if (Session["QuotationData"] != null)
            {
                lookup_ProformaInvoice.DataSource = (DataTable)Session["QuotationData"];
            }
        }

        //End Tanmoy Hierarchy


        [WebMethod(EnableSession = true)]
        public static object getTCSDetails(string CustomerId, string doc_d, string date, string totalAmount, string branch_id)
        {
            string Mode = Convert.ToString(HttpContext.Current.Session["key_QutId"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TCSDetails");
            proc.AddVarcharPara("@CustomerID", 500, CustomerId);
            proc.AddVarcharPara("@invoice_id", 500, doc_d);
            proc.AddVarcharPara("@Action", 500, "ShowTDSCRPDetails");
            proc.AddVarcharPara("@date", 500, date);
            proc.AddVarcharPara("@totalAmount", 500, totalAmount);
            proc.AddVarcharPara("@branch_id", 500, branch_id);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                return new { tds_amount = Convert.ToString(dt.Rows[0]["tds_amount"]), Rate = Convert.ToString(dt.Rows[0]["Rate"]), Code = Convert.ToString(dt.Rows[0]["Code"]), Amount = Convert.ToString(dt.Rows[0]["Amount"]) };
            }
            else
            {
                return new { tds_amount = 0, Rate = 0, Code = 0, Amount = 0 };
            }


        }
        public class SegmentDetails
        {
            public string Segment1 { get; set; }
            public string Segment2 { get; set; }
            public string Segment3 { get; set; }
            public string Segment4 { get; set; }
            public string Segment5 { get; set; }
            public string SegmentName1 { get; set; }
            public string SegmentName2 { get; set; }
            public string SegmentName3 { get; set; }
            public string SegmentName4 { get; set; }
            public string SegmentName5 { get; set; }

            public string Module_Id { get; set; }
            public string SegmentNo { get; set; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetSegmentDetails(string CustomerId)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
                proc.AddVarcharPara("@Action", 50, "GetSegmentDetails");
                proc.AddVarcharPara("@InternalId", 50, CustomerId);
                proc.AddVarcharPara("@Module_Id", 50, "1");//Sales Invoice
                DataTable Address = proc.GetTable();

                if (Address!=null && Address.Rows.Count > 0)
                {
                    SegmentDetails details = new SegmentDetails();
                    details.Segment1 = Convert.ToString(Address.Rows[0]["Segment1"]);
                    details.Segment2 = Convert.ToString(Address.Rows[0]["Segment2"]);
                    details.Segment3 = Convert.ToString(Address.Rows[0]["Segment3"]);
                    details.Segment4 = Convert.ToString(Address.Rows[0]["Segment4"]);
                    details.Segment5 = Convert.ToString(Address.Rows[0]["Segment5"]);

                    details.SegmentName1 = Convert.ToString(Address.Rows[0]["UsedFor1"]);
                    details.SegmentName2 = Convert.ToString(Address.Rows[0]["UsedFor2"]);
                    details.SegmentName3 = Convert.ToString(Address.Rows[0]["UsedFor3"]);
                    details.SegmentName4 = Convert.ToString(Address.Rows[0]["UsedFor4"]);
                    details.SegmentName5 = Convert.ToString(Address.Rows[0]["UsedFor5"]);

                    details.Module_Id = Convert.ToString(Address.Rows[0]["Module_Id"]);
                    details.SegmentNo = Convert.ToString(Address.Rows[0]["SegmentNo"]);

                    return details;

                }

            }
            return null;
        }

    }
}