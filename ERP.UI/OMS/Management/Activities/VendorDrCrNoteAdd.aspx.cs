using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ERP.Models;
using ERP.OMS.Tax_Details.ClassFile;
using System.Threading.Tasks;
using System.Globalization;

namespace ERP.OMS.Management.Activities
{
    public partial class VendorDrCrNoteAdd : System.Web.UI.Page
    {
        DebitCreditNoteBL objDebitCreditBL = new DebitCreditNoteBL();

      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");

            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    dvProject.Style.Add("Display", "block");
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    dvProject.Style.Add("Display", "none");
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
                   // lookup_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                  //  lookup_Project.Columns[3].Visible = false;
                }
            }
            //For Hierarchy End Tanmoy


            if (!IsPostBack)
            {
                #region NewTaxblock
                string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "P");
                HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                HDBranchWiseStateTax.Value = BranchWiseStateTax;
                HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;
                #endregion
                Session.Remove("SESS_VendorNoteLedgerDT");
                Session.Remove("VDCN_FinalTaxRecord");

                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>Vendorledger(" + Request.QueryString["key"] + ");</script>");

                }

                string[] FinYEnd = Session["FinYearEnd"].ToString().Split(' ');
                string strFinYEnd = FinYEnd[0];
                string LastCompany = HttpContext.Current.Session["LastCompany"].ToString();
                string userid = HttpContext.Current.Session["userid"].ToString();
                string LastFinYear = HttpContext.Current.Session["LastFinYear"].ToString();
                string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);

                Task PopulateStockTrialDataTask = new Task(() => BindAllControlDataOnPageLoad(strFinYEnd, LastCompany, userid, LastFinYear, LocalCurrency));
                PopulateStockTrialDataTask.RunSynchronously();

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End

                if (Request.QueryString["key"] == "ADD")
                {
                    DataTable dtposTimeDebit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=6");

                    if (dtposTimeDebit != null && dtposTimeDebit.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTimeDebit.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTimeDebit.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateFreeze.Value = Convert.ToString(dtposTimeDebit.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateFreeze.Value = Convert.ToString(dtposTimeDebit.Rows[0]["DataFreeze_Todate"]);
                    }
                    DataTable dtposTimeCredit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=7");

                    if (dtposTimeCredit != null && dtposTimeCredit.Rows.Count > 0)
                    {
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTimeCredit.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTimeCredit.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateConFreeze.Value = Convert.ToString(dtposTimeCredit.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateConFreeze.Value = Convert.ToString(dtposTimeCredit.Rows[0]["DataFreeze_Todate"]);
                    }

                    hdnPageStatus.Value = "first";
                    ddlNoteType.SelectedValue = "Cr";
                    //hdnNoteType.Value = "Cr";
                    hdnMode.Value = "Entry";
                    Keyval_internalId.Value = "Add";

                    DataTable Transactiondt = CreateTempTable("Transaction");
                    Transactiondt.Rows.Add("1", "1", "", "", "0.00", "", "I", "0.00", "0.00", "", "", "", "");
                    Session["SESS_VendorNoteLedgerDT"] = Transactiondt;
                    grid.DataSource = GetVoucher(Transactiondt);
                    grid.DataBind();


                    #region To Show By Default Cursor after SAVE AND NEW
                    if (Session["SaveModeVDC"] != null)
                    {
                        if (Session["SaveNewValues"] != null)
                        {
                            List<string> SaveNewValues = (Session["SaveNewValues"]) as List<string>;
                            ddlBranch.SelectedValue = SaveNewValues[1];

                            //txtCustName.Text = SaveNewValues[2];
                           // hdfLookupCustomer.Value = SaveNewValues[0];

                            string NoteType = SaveNewValues[3];
                            if (NoteType == "Cr")
                            {
                               // ddlNoteType.Text = "Credit Note";
                                ddlNoteType.SelectedValue = "Cr";
                            }
                            else
                            {
                               // ddlNoteType.Text = "Debit Note";
                                ddlNoteType.SelectedValue = "Dr";
                            }
                        }

                        if (Session["schemaText"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            string StrschemaText = Convert.ToString(Session["schemaText"]);
                            string schemaID = Convert.ToString(Session["schemaID"]);
                            CmbScheme.Text = StrschemaText;
                            // hdnSchemaID.Value = schemaID;
                            CmbScheme.Value = schemaID;
                        }

                        if (Convert.ToString(Session["SaveModeVDC"]) == "A")
                        {
                            tDate.Focus();
                            txtBillNo.Enabled = false;
                            txtBillNo.Text = "Auto";
                        }
                        else if (Convert.ToString(Session["SaveModeVDC"]) == "M")
                        {
                            txtBillNo.Enabled = true;
                            txtBillNo.Text = "";
                            txtBillNo.Focus();
                        }
                    }
                    else
                    {
                        ddlNoteType.Focus();
                    }
                    #endregion To Show By Default Cursor after SAVE AND NEW
                }
                else
                {
                    hdnMode.Value = "Edit";
                    lblHeading.Text = "Modify Vendor Credit/Debit Note";
                    div_Edit.Style.Add("display", "none");
                    string NoteID = Request.QueryString["key"];
                    Keyval_internalId.Value = "VendorNote" + NoteID;
                    hdnNotelNo.Value = NoteID;
                    txtCustName.ClientEnabled = false;
                    FillGrid(NoteID);
                    DataTable Voucherdt = objDebitCreditBL.GetNoteDetailsInEditMode(hdnNotelNo.Value);
                    string TotalAmount = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));//GetNoteDetailsAddressInEditMode

                    BillingShippingControl.SetBillingShippingTable(objDebitCreditBL.GetNoteDetailsAddressInEditMode(hdnNotelNo.Value));
                    txt_Debit.Text = TotalAmount;
                    Session["VDCN_FinalTaxRecord"] = GetVendorDrCrNoteEditedTaxData(NoteID);
                    hdnPageStatus.Value = "update";
                    hdfIsDelete.Value = "C";
                    grid.DataSource = GetVoucher();
                    grid.DataBind();
                }




                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Vendor Credit/Debit Note";
                    btn_SaveRecords.Visible = false;
                    btnSaveRecords.Visible = false;
                }

            }
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='VNOTE'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        public DataTable GetVendorDrCrNoteEditedTaxData(string NoteID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
            proc.AddVarcharPara("@Action", 500, "VendorNoteEditedTaxDetails");
            proc.AddVarcharPara("@NoteID", 500, Convert.ToString(NoteID));
            ds = proc.GetDataSet();
            return ds.Tables[0];
        }
        public DataTable GetNoteDetails(string Action, string NotelNo)
        {
            DataTable dt = objDebitCreditBL.GetNoteDetails(Action, NotelNo);
            return dt;
        }
        //Rev Rajdip
        public DataTable GetDebitNotevalid(string Action, string NotelNo)
        {
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            //Chinmoy edited below code start
            //DataTable dt = objEngine.GetDataTable("select ISNULL(SUM(ISNULL(Adjusted_Amount,0)),0) as ADJAMT from tbl_trans_CrNoteVendorAdvanceAdjustment " +
            //                                      " where Adjusted_doc_id='"+NotelNo+"'");

        
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 100, "CheckUsedOrNot");
            proc.AddVarcharPara("@NoteID", 200, NotelNo);
            dt = proc.GetTable();
           
        
            //End
            return dt;
        }
        //End Rev Rajdip
        public void FillGrid(string NoteID)
        {
            DataTable Dtdebitnotevalidation  = GetDebitNotevalid("VendorNoteHeader", NoteID);
            DataTable Detailsdt = GetNoteDetails("VendorNoteHeader", NoteID);
            DataTable dtProj = objDebitCreditBL.GetProjectDetails(NoteID);
            if (Detailsdt != null && Detailsdt.Rows.Count > 0)
            {
                string NoteType = Convert.ToString(Detailsdt.Rows[0]["NoteType"]);
                string BillNumber = Convert.ToString(Detailsdt.Rows[0]["DocumentNumber"]);
                string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["DocumentDate"]);
                string BranchId = Convert.ToString(Detailsdt.Rows[0]["BranchID"]);

                string Narration = Convert.ToString(Detailsdt.Rows[0]["Narration"]);

                string CustomerID = Convert.ToString(Detailsdt.Rows[0]["VendorID"]);
                string InvoiceID = Convert.ToString(Detailsdt.Rows[0]["InvoiceID"]);//////////////////////////////
                string CurrencyId = Convert.ToString(Detailsdt.Rows[0]["CurrencyId"]);
                string Rate = Convert.ToString(Detailsdt.Rows[0]["Rate"]);

                string PartyInvoiceNo = Convert.ToString(Detailsdt.Rows[0]["PartyInvoiceNo"]);
                string PartyInvoiceDate = Convert.ToString(Detailsdt.Rows[0]["PartyInvoiceDate"]);

                string TaggedDocNumber = Convert.ToString(Detailsdt.Rows[0]["TaggedDocNumber"]);
                string VendorName = Convert.ToString(Detailsdt.Rows[0]["name"]);

                string Invoice_Number = Convert.ToString(Detailsdt.Rows[0]["Invoice_Number"]);
                string Invoice_ID = Convert.ToString(Detailsdt.Rows[0]["Invoice_ID"]);
                Decimal DCNote_TotalAmount = Convert.ToDecimal(Detailsdt.Rows[0]["DCNote_TotalAmount"]);
                Decimal DCNote_UnPaidAmount = Convert.ToDecimal(Detailsdt.Rows[0]["DCNote_UnPaidAmount"]); 
                ddlNoteType.SelectedValue = NoteType;
                if (NoteType == "Cr")
                {
                    //ddlNoteType.Text = "Credit Note";
                    //hdnNoteType.Value = "Cr";
                    div_InvoiceNo.Style.Add("display", "none");
                    div_InvoiceDate.Style.Add("display", "none");

                }
                else
                {
                    //ddlNoteType.Text = "Debit Note";
                    //hdnNoteType.Value = "Dr";
                    div_InvoiceNo.Style.Add("display", "Block");
                    div_InvoiceDate.Style.Add("display", "Block");
                }
                txtBillNo.Text = BillNumber.Trim();
                tDate.Date = Convert.ToDateTime(TransactionDate);
                ddlBranch.SelectedValue = BranchId;
                txtCustName.Text = VendorName;
                hdfLookupCustomer.Value = CustomerID;
                txtPartyInvoice.Text = PartyInvoiceNo;
                

                if (PartyInvoiceDate != "")
                {
                    dt_PartyDate.Date = Convert.ToDateTime(PartyInvoiceDate);
                }

                ddl_Currency.SelectedValue = CurrencyId;
                txt_Rate.Text = Rate;
                txtPurchaseInvoiceNo.Text = Invoice_Number;
                hdnPurchaseInvoiceID.Value = InvoiceID;
                //BinDInvoiceDetails("VendorCredit", CustomerID, BranchId);
                //if (InvoiceID == "-0")
                //{
                //    ddlInvoice.Value = null;
                //}
                //else
                //{
                //    ddlInvoice.Value = InvoiceID;
                //}

                txtNarration.Text = Narration;
                //chinmoy commented ref. mantisId:20658 start
                //if (TaggedDocNumber != "")
                //{
                //    btnSaveRecords.Visible = false;
                //    btn_SaveRecords.Visible = false;
                //    tagged.Style.Add("display", "Block");
                //    spanTaggedDocNo.InnerHtml = TaggedDocNumber;
                //}
                ////chinmoy commented ref. mantisId:20658 End
                //Rev Rajdip
                decimal Adjamt=Convert.ToDecimal(Dtdebitnotevalidation.Rows[0]["ADJAMT"].ToString());
                if (Adjamt > 0)
                {
                    btnSaveRecords.Visible = false;
                    btn_SaveRecords.Visible = false;
                    tagged.Style.Add("display", "Block");
                    spanTaggedDocNo.InnerHtml = "";
                }
                //End Rev Rajdip
                if (DCNote_TotalAmount != DCNote_UnPaidAmount)
                {
                    btnSaveRecords.Visible = false;
                    btn_SaveRecords.Visible = false;
                    tagged.Style.Add("display", "Block");
                    spanTaggedDocNo.InnerHtml = "";
                }


                if (dtProj != null && dtProj.Rows.Count>0)
                {
                   // lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtProj.Rows[0]["Proj_Id"]));
                    txtProject.Text = Convert.ToString(dtProj.Rows[0]["ProjectCode"]);
                    hdnProjectId.Value = Convert.ToString(dtProj.Rows[0]["Proj_Id"]);
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
        }

        public void BindAllControlDataOnPageLoad(string strFinYEnd, string LastCompany, string userid, string LastFinYear, string strLocalCurrency)
        {
            GetAllDropDownDetailForCashBank();

            #region Note Date

            tDate.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;

            string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
            string FinYearEnd = FinYEnd[0];
            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            string ForJournalDate = Convert.ToString(date3);
            int month = oDBEngine.GetDate().Month;
            int date = oDBEngine.GetDate().Day;
            int Year = oDBEngine.GetDate().Year;

            if (date3 < oDBEngine.GetDate().Date)
            {
                fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);
            }
            else
            {
                fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
            }

            tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            #endregion
        }
        public void GetAllDropDownDetailForCashBank()
        {
            DataSet dst = new DataSet();
            //  string NoteType= hdnNoteType.Value;
          //  dst = objDebitCreditBL.AllDropDownDetailForCashBank("Cr");
            if (Session["SaveNewValues"] != null)
            {
                List<string> SaveNewValues = (Session["SaveNewValues"]) as List<string>;
                string NoteType = SaveNewValues[3];
                if (NoteType == "Cr")
                {
                    dst = objDebitCreditBL.AllDropDownDetailForCashBank("Cr");
                }
                else
                {
                    dst = objDebitCreditBL.AllDropDownDetailForCashBank("Dr");
                }
            }
            else
            {
                dst = objDebitCreditBL.AllDropDownDetailForCashBank("Cr");
            }

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataTextField = "BANKBRANCH_NAME";
                ddlBranch.DataValueField = "BANKBRANCH_ID";
                ddlBranch.DataSource = dst.Tables[0];
                ddlBranch.DataBind();
            }
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddl_Currency.DataTextField = "Currency_AlphaCode";
                ddl_Currency.DataValueField = "Currency_ID";
                ddl_Currency.DataSource = dst.Tables[1];
                ddl_Currency.DataBind();
            }
            if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "ID";
                CmbScheme.DataSource = dst.Tables[2];
                CmbScheme.DataBind();
            }
        }



        #region WebMethod

        [WebMethod]
        public static List<ListItem> GetScheme(string sel_type_id)
        {
            string LastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            string LastFinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            string query = " Select '0' as ID,'Select' as  SchemaName union all select Convert(varchar(15),Id)+'~'+Convert(varchar(15),schema_type)+'~'+Convert(varchar(15),length) +'~'+ Convert(varchar(15),Branch) +'~'+CONVERT(NVARCHAR(10),CAST(Valid_From AS DATE),110)+'~'+ CONVERT(NVARCHAR(10),CAST(Valid_Upto AS DATE),110) as ID,SchemaName +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) as SchemaName From tbl_master_Idschema  Where TYPE_ID=(Case When '" + sel_type_id + "'='Dr' Then '27' Else '28' End) AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "')) AND Isnull(comapanyInt,'')='" + LastCompany + "' AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + LastFinYear + "')";
           
             //string constr = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;  MULTI
            string constr = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    List<ListItem> customers = new List<ListItem>();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(new ListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["SchemaName"].ToString()
                            });
                        }
                    }
                    con.Close();
                    return customers;
                }
            }
        }

        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            string strschematype = "", strschemalength = "", strschemavalue = "", strbranchID = "";

          //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,IsNull(Branch,0) as Branch ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strbranchID = Convert.ToString(DT.Rows[i]["Branch"]);

                strschemavalue = strschematype + "~" + strschemalength + "~" + strbranchID;
            }
            return Convert.ToString(strschemavalue);
        }
        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo, string Type)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), Type, "VendorNote_Check");
            }
            return status;
        }
        #endregion

        #region InvoiceDetail
        //public void BinDInvoiceDetails(string Action, string VendorID, string Branchid)
        //{
        //    string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();

        //    DataTable dt = objDebitCreditBL.GetInvoiceDetails(Action, VendorID, FinYear, Branchid);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        ddlInvoice.DataSource = dt;
        //        ddlInvoice.DataBind();
        //    }
        //}
        //protected void ddlInvoice_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string NoteType = e.Parameter.Split('~')[0];
        //    string VendorID = e.Parameter.Split('~')[1];
        //    string Action = "";

        //    Action = "VendorCredit";
        //    string branchid = ddlBranch.SelectedValue;
        //    BinDInvoiceDetails(Action, VendorID, branchid); //Mantis Issue 0015220 by Suman branch  filter required for populating Purchase Invoice no

        //    PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
        //    DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(VendorID);

        //    if (GSTNTable != null && GSTNTable.Rows.Count > 0)
        //    {
        //        string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
        //        if (strGSTN != "")
        //        {
        //            ddlInvoice.JSProperties["cpGSTN"] = "Yes";
        //        }
        //        else
        //        {
        //            ddlInvoice.JSProperties["cpGSTN"] = "No";
        //        }
        //    }
        //}

        #endregion

        #region Grid Events
        public DataTable CreateTempTable(string Type)
        {
            DataTable VenDrCrdt = new DataTable();

            if (Type == "Transaction")
            {
                VenDrCrdt.Columns.Add("SrlNo", typeof(string));
                VenDrCrdt.Columns.Add("CashReportID", typeof(string));
                VenDrCrdt.Columns.Add("MainAccount", typeof(string));
                VenDrCrdt.Columns.Add("SubAccount", typeof(string));
                VenDrCrdt.Columns.Add("WithDrawl", typeof(string));
                VenDrCrdt.Columns.Add("Narration", typeof(string));

                VenDrCrdt.Columns.Add("Status", typeof(string));

                VenDrCrdt.Columns.Add("TaxAmount", typeof(string));
                VenDrCrdt.Columns.Add("NetAmount", typeof(string));
                VenDrCrdt.Columns.Add("MainAccountID", typeof(string));
                VenDrCrdt.Columns.Add("SubAccountID", typeof(string));
                VenDrCrdt.Columns.Add("IsSubledger", typeof(string));
                VenDrCrdt.Columns.Add("TAXable", typeof(string));

            }
            return VenDrCrdt;
        }
        public class VOUCHERLIST
        {
            public string SrlNo { get; set; }
            public string CashReportID { get; set; }
            public string MainAccount { get; set; }
            public string bthSubAccount { get; set; }
            public string WithDrawl { get; set; }
            public string TaxAmount { get; set; }
            public string NetAmount { get; set; }
            public string Narration { get; set; }
            public string gvColMainAccount { get; set; }
            public string gvColSubAccount { get; set; }
            public string IsSubledger { get; set; }
            public string TAXable { get; set; }
        }
        public IEnumerable GetVoucher(DataTable voucherDT)
        {
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
            if (voucherDT != null)
            {
                if (voucherDT.Rows.Count > 0 && voucherDT != null)
                {
                    for (int i = 0; i < voucherDT.Rows.Count; i++)
                    {
                        VOUCHERLIST Vouchers = new VOUCHERLIST();
                        Vouchers.SrlNo = Convert.ToString(voucherDT.Rows[i]["SrlNo"]);
                        Vouchers.CashReportID = Convert.ToString(voucherDT.Rows[i]["CashReportID"]);
                        Vouchers.MainAccount = Convert.ToString(voucherDT.Rows[i]["MainAccountID"]).Trim();
                        Vouchers.bthSubAccount = Convert.ToString(voucherDT.Rows[i]["SubAccountID"]).Trim();
                        Vouchers.WithDrawl = Convert.ToString(voucherDT.Rows[i]["WithDrawl"]);
                        Vouchers.Narration = Convert.ToString(voucherDT.Rows[i]["Narration"]);
                        Vouchers.TaxAmount = Convert.ToString(voucherDT.Rows[i]["TaxAmount"]);
                        Vouchers.NetAmount = Convert.ToString(voucherDT.Rows[i]["NetAmount"]);
                        Vouchers.gvColMainAccount = Convert.ToString(voucherDT.Rows[i]["MainAccount"]).Trim();
                        Vouchers.gvColSubAccount = Convert.ToString(voucherDT.Rows[i]["SubAccount"]).Trim();
                        Vouchers.IsSubledger = Convert.ToString(voucherDT.Rows[i]["IsSubledger"]);
                        Vouchers.TAXable = Convert.ToString(voucherDT.Rows[i]["TAXable"]);
                        VoucherList.Add(Vouchers);
                    }
                }
            }
            return VoucherList;
        }

        public IEnumerable GetVoucher()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            string VoucherNumber = Convert.ToString(hdnNotelNo.Value);
            DataTable Voucherdt = objDebitCreditBL.GetNoteDetailsInEditMode(hdnNotelNo.Value);

            Session["SESS_VendorNoteLedgerDT"] = Voucherdt;

            for (int i = 0; i < Voucherdt.Rows.Count; i++)
            {
                VOUCHERLIST Vouchers = new VOUCHERLIST();
                Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i][0]);
                Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i][1]);
                Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i][2]);

                Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i][3]);
                Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i][5]);

                Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["SubAccount"]).Trim();
                Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]);
                Vouchers.TAXable = Convert.ToString(Voucherdt.Rows[i]["TAXable"]);
                VoucherList.Add(Vouchers);
            }

            return VoucherList;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grid.DataBind();
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["SESS_VendorNoteLedgerDT"] != null && ((DataTable)Session["SESS_VendorNoteLedgerDT"]).Rows.Count > 0)
            {
                grid.DataSource = GetVoucher((DataTable)Session["SESS_VendorNoteLedgerDT"]);
            }
            else
            {
                grid.DataSource = GetVoucher();
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

        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            ASPxGridView gridx = sender as ASPxGridView;
            if (e.Column.FieldName == "MainAccount")
            {
                e.Editor.ReadOnly = true;

            }
            else if (e.Column.FieldName == "bthSubAccount")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "TaxAmount")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "NetAmount")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string type = Convert.ToString(e.Parameters.Split('~')[0]);
            if (type == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    if (Session["SESS_VendorNoteLedgerDT"] != null)
                    {
                        DataTable VendorNotedt = (DataTable)Session["SESS_VendorNoteLedgerDT"];
                        grid.DataBind();
                    }
                }
            }
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpVouvherNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = null;

            string Action = Convert.ToString(hdnMode.Value);
            DataTable VenDrCrdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            if (Session["SESS_VendorNoteLedgerDT"] != null)
            {
                VenDrCrdt = (DataTable)Session["SESS_VendorNoteLedgerDT"];
            }
            else
            {
                VenDrCrdt.Columns.Add("SrlNo", typeof(string));
                VenDrCrdt.Columns.Add("CashReportID", typeof(string));
                VenDrCrdt.Columns.Add("MainAccount", typeof(string));
                VenDrCrdt.Columns.Add("SubAccount", typeof(string));
                VenDrCrdt.Columns.Add("WithDrawl", typeof(string));
                VenDrCrdt.Columns.Add("Narration", typeof(string));
                VenDrCrdt.Columns.Add("Status", typeof(string));

                VenDrCrdt.Columns.Add("TaxAmount", typeof(string));
                VenDrCrdt.Columns.Add("NetAmount", typeof(string));

                VenDrCrdt.Columns.Add("MainAccountID", typeof(string));
                VenDrCrdt.Columns.Add("SubAccountID", typeof(string));
                VenDrCrdt.Columns.Add("IsSubledger", typeof(string));
                VenDrCrdt.Columns.Add("TAXable", typeof(string));
            }

            foreach (var args in e.InsertValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Narration = Convert.ToString(args.NewValues["Narration"]);
                string TaxAmount = Convert.ToString(args.NewValues["TaxAmount"]);
                string NetAmount = Convert.ToString(args.NewValues["NetAmount"]);
                string MainAccountID = Convert.ToString(args.NewValues["MainAccount"]);
                string SubAccountID = Convert.ToString(args.NewValues["bthSubAccount"]);
                string IsSubledger = Convert.ToString(args.NewValues["IsSubledger"]);
                string TAXable = Convert.ToString(args.NewValues["TAXable"]);
                if (MainAccount != "" && MainAccount != "0" && MainAccount != null)
                {
                    if (Convert.ToDecimal(WithDrawl) > 0)
                    {
                        DataRow dr = VenDrCrdt.NewRow();
                        dr["SrlNo"] = SrlNo;
                        dr["CashReportID"] = 0;
                        dr["MainAccount"] = MainAccount;
                        dr["SubAccount"] = SubAccount;
                        dr["WithDrawl"] = WithDrawl;
                        dr["Narration"] = Narration;
                        dr["Status"] = "I";
                        dr["TaxAmount"] = TaxAmount;
                        dr["NetAmount"] = NetAmount;
                        dr["MainAccountID"] = MainAccountID;
                        dr["SubAccountID"] = SubAccountID;
                        dr["IsSubledger"] = IsSubledger;
                        dr["TAXable"] = TAXable;
                        VenDrCrdt.Rows.Add(dr);
                    }
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.Keys["SrlNo"]);
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Narration = Convert.ToString(args.NewValues["Narration"]);
                string TaxAmount = Convert.ToString(args.NewValues["TaxAmount"]);
                string NetAmount = Convert.ToString(args.NewValues["NetAmount"]);
                string MainAccountID = Convert.ToString(args.NewValues["MainAccount"]);
                string SubAccountID = Convert.ToString(args.NewValues["bthSubAccount"]);
                string IsSubledger = Convert.ToString(args.NewValues["IsSubledger"]);
                string TAXable = Convert.ToString(args.NewValues["TAXable"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["CashReportID"]);
                    if (DeleteID == CashReportID)
                    {
                        isDeleted = true;
                        break;
                    }
                }

                if (isDeleted == false)
                {
                    if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null))
                    {
                        bool Isexists = false;
                        foreach (DataRow dr in VenDrCrdt.Rows)
                        {
                            string OldCashReportID = Convert.ToString(dr["CashReportID"]);
                            if (OldCashReportID == CashReportID)
                            {
                                Isexists = true;
                                dr["MainAccount"] = MainAccount;
                                dr["SubAccount"] = SubAccount;
                                dr["WithDrawl"] = WithDrawl;
                                dr["Narration"] = Narration;
                                dr["Status"] = "U";
                                dr["TaxAmount"] = TaxAmount;
                                dr["NetAmount"] = NetAmount;
                                dr["MainAccountID"] = MainAccountID;
                                dr["SubAccountID"] = SubAccountID;
                                dr["IsSubledger"] = IsSubledger;
                                dr["TAXable"] = TAXable;
                                break;
                            }
                        }
                        if (Isexists == false)
                        {
                            VenDrCrdt.Rows.Add(SrlNo, CashReportID, MainAccount, SubAccount, WithDrawl, Narration, "U", TaxAmount, NetAmount, MainAccountID, SubAccountID, IsSubledger, TAXable);
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string SrlNo = "";

                for (int i = VenDrCrdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = VenDrCrdt.Rows[i];
                    string delCashReportID = Convert.ToString(dr["CashReportID"]);

                    if (delCashReportID == CashReportID)
                    {
                        SrlNo = Convert.ToString(dr["CashReportID"]);
                        dr.Delete();
                    }
                }
                VenDrCrdt.AcceptChanges();

                if (CashReportID.Contains("~") != true)
                {
                    VenDrCrdt.Rows.Add("0", CashReportID, "", "", "0.00", "", "D", "0.00", "0.00", "", "", "", "");
                }
            }
            string strDeleteSrlNo = Convert.ToString(hdnDeleteSrlNo.Value);
            if (strDeleteSrlNo != "" && strDeleteSrlNo != "0")
            {
                DeleteTaxDetails(strDeleteSrlNo);

                hdnDeleteSrlNo.Value = "";
            }

            int j = 1;
            foreach (DataRow dr in VenDrCrdt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                string oldSrlNo = Convert.ToString(dr["SrlNo"]);
                string newSrlNo = j.ToString();

                dr["SrlNo"] = j.ToString();
                UpdateTaxDetails(oldSrlNo, newSrlNo);
                if (Status != "D")
                {
                    if (Status == "I" || Status == "")
                    {
                        string strID = Convert.ToString("Q~" + j);
                        dr["CashReportID"] = strID;
                    }
                    j++;
                }
            }
            VenDrCrdt.AcceptChanges();

            Session["SESS_VendorNoteLedgerDT"] = VenDrCrdt;
            if (hdfIsDelete.Value != "D")
            {
                DataTable tempVenDrCrdt = VenDrCrdt.Copy();
                foreach (DataRow dr in tempVenDrCrdt.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);
                    if (Status == "I")
                    {
                        dr["CashReportID"] = 0;
                    }
                    else if (Status == "D")
                    {
                        dr.Delete();
                    }
                }
                tempVenDrCrdt.AcceptChanges();

                DataTable TaxRecord = new DataTable();
                if (Session["VDCN_FinalTaxRecord"] == null)
                {
                    CreateDataTaxTable();
                }

                TaxRecord = (DataTable)Session["VDCN_FinalTaxRecord"];

                TaxRecord = gstTaxDetails.SetTaxTableDataMainAccount(ref tempVenDrCrdt, "SrlNo", "Taxable", "WithDrawl", "TaxAmount", "NetAmount", TaxRecord, "P", DateTime.ParseExact(tDate.Text, "dd-MM-yyyy", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd"), Convert.ToString(ddlBranch.SelectedValue), "", "E", Convert.ToString(hdfLookupCustomer.Value));



                string SchemaID = "", validate = "";
                if (Action == "Entry")
                {
                    //SchemaID = Convert.ToString(hdnSchemaID.Value);
                    SchemaID = Convert.ToString(CmbScheme.Value).Split('~')[0];
                }
                else
                {
                    SchemaID = "0";
                }
               

                string VoucharNo = Convert.ToString(txtBillNo.Text);
                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(ddlBranch.SelectedValue);
                string strCurrency = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                string strUserID = Convert.ToString(Session["userid"]);

                //string NoteType = Convert.ToString(hdnNoteType.Value);
                string NoteType = Convert.ToString(ddlNoteType.SelectedValue);
                string NoteDate = Convert.ToString(tDate.Value);
                string MainNarration = Convert.ToString(txtNarration.Text);
                string CustomerName = Convert.ToString(hdfLookupCustomer.Value);
                string InvoiceNo = Convert.ToString(hdnPurchaseInvoiceID.Value);
                //string InvoiceNo = (Convert.ToString(ddlInvoice.Value) != "") ? Convert.ToString(ddlInvoice.Value) : "0";
                string Currency = Convert.ToString(ddl_Currency.SelectedValue);
                decimal Rate = Convert.ToDecimal(txt_Rate.Value);
                string strPartyInvoice = "";
                if (NoteType == "Dr")
                {
                    strPartyInvoice = Convert.ToString(txtPartyInvoice.Text);
                }
                else if (NoteType == "Cr")
                {
                    strPartyInvoice = txtPurchaseInvoiceNo.Text;
                }

                string strPartyDate = "", NoteID = "";
                if (dt_PartyDate.Date.ToString("yyyy-MM-dd") != "0001-01-01") strPartyDate = dt_PartyDate.Date.ToString("yyyy-MM-dd");


                if (Action == "Entry")
                {
                    Action = "VendorNoteADD";

                }
                else
                {
                    Action = "VendorNoteEdit";

                }

                DataTable CutOffVenDrCrDetailsdt = new DataTable();
                CutOffVenDrCrDetailsdt = tempVenDrCrdt.DefaultView.ToTable(false, "SrlNo", "CashReportID", "MainAccount", "SubAccount", "WithDrawl", "Narration", "Status", "TaxAmount", "NetAmount");
                CutOffVenDrCrDetailsdt.AcceptChanges();

                Boolean _addressFlag = false;

                if (CutOffVenDrCrDetailsdt != null && CutOffVenDrCrDetailsdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in CutOffVenDrCrDetailsdt.Rows)
                    {
                        if (Convert.ToDecimal(dr["WithDrawl"]) == 0)
                        {
                            validate = "zeroAmount";
                            break;
                        }
                         
                        DataTable dt = new DataTable();
                        ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
                        proc.AddVarcharPara("@Action", 500, "CheckLedgerHSN");
                        proc.AddBigIntegerPara("@MainAccID", Convert.ToInt32(dr["MainAccount"]));
                        proc.AddVarcharPara("@ApplicableFor", 1, Convert.ToString("P"));
                        dt = proc.GetTable();
                        if (dt.Rows.Count > 0)
                        {                            

                            //if (Convert.ToDecimal(dr["WithDrawl"]) != 0 && Convert.ToDecimal(dr["TaxAmount"]) == 0)
                            //{
                            //    validate = "taxREquired";
                            //    break;
                            //}
                            _addressFlag = true;
                            if (_addressFlag && (BillingShippingControl.GeteShippingStateCode() == "" || BillingShippingControl.GeteShippingStateCode() == "0"))
                            {
                                validate = "addressrequired";
                                break;
                            }

                        }
                    }
                }


                if (validate == "addressrequired" || validate == "taxREquired" || validate == "zeroAmount" || validate == "mixedvalue" || validate == "SubAccountMandatory")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    #region  BillingShipping user control data
                    DataTable tempBillAddress = new DataTable();
                    tempBillAddress = BillingShippingControl.GetBillingShippingTable();
                    //tempBillAddress = BillingShippingControl.SaveBillingShippingControlData();

                    #endregion
                    
                    foreach (DataRow drDrCr in CutOffVenDrCrDetailsdt.Rows)
                    {
                        int sl = Convert.ToInt16(drDrCr["SrlNo"]);
                        if (TaxRecord != null)
                        {
                            //    Code commented Start according to mantis Id 0019430
                            //if (Convert.ToDecimal(drDrCr["TaxAmount"]) == 0)
                            //{
                                //DataTable POdt = TaxRecord;
                                //DataView dvData = new DataView(POdt);
                                //dvData.RowFilter = "SLNo <>" + sl;

                                //TaxRecord = dvData.ToTable();
                            //}
                            //    Code commented Start according to mantis Id 0019430
                        }
                    }
                    //Project code Tanmoy
                    CommonBL cbl = new CommonBL();
                    string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    Int64 ProjId = 0;
                    if (txtProject.Text != "")
                    {
                        ProjId = Convert.ToInt64(hdnProjectId.Value);
                    }
                    else
                    {
                        ProjId = 0;
                    }
                    //Project code Tanmoy

                    int IsComplete = 0, OutNotelNo = 0;
                    string strVoucharNo = "";
                    objDebitCreditBL.VendorModifyDrCrNote(Action, SchemaID, VoucharNo, strFinYear, strCompanyID, strBranchID, NoteDate, strCurrency, MainNarration, NoteType, CustomerName, InvoiceNo, Currency, Rate, strUserID, CutOffVenDrCrDetailsdt, strPartyInvoice, strPartyDate, tempBillAddress, TaxRecord,ProjId, ref IsComplete, ref strVoucharNo, ref OutNotelNo, hdnNotelNo.Value);
                    if (IsComplete == 1)
                    {
                        grid.JSProperties["cpVouvherNo"] = Convert.ToString(strVoucharNo);
                        SessionClear();
                        hdnNotelNo.Value = "";
                        DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                        if (udfTable != null)
                            Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("VNOTE", "VendorNote" + Convert.ToString(OutNotelNo), udfTable, Convert.ToString(Session["userid"]));


                        grid.JSProperties["cpSaveSuccessOrFail"] = "successInsert";

                        #region To Show By Default Cursor after SAVE AND NEW
                        if (Action == "VendorNoteADD")
                        {
                            if(hdnRefreshType.Value=="S")
                            {
                                string schemaText = Convert.ToString(CmbScheme.Text);
                                string schemaValue = Convert.ToString(CmbScheme.Value);
                                Session["schemaText"] = schemaText;
                                Session["schemaID"] = schemaValue;

                                string VendorName = txtCustName.Text;
                                List<string> myList = new List<string> { CustomerName, strBranchID, VendorName, NoteType };
                                Session["SaveNewValues"] = myList;

                                string schematype = Convert.ToString(txtBillNo.Text);
                                if (schematype == "Auto")
                                {
                                    Session["SaveModeVDC"] = "A";
                                    hdnSchemaType.Value = Convert.ToString("1");
                                }
                                else
                                {
                                    Session["SaveModeVDC"] = "M";
                                }
                            }
                            else if (hdnRefreshType.Value == "E")
                            {
                                Session["SaveModeVDC"] = null;
                            }
                            
                        }

                        #endregion

                    }
                    else if (IsComplete == -9)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "AddLock";
                        DataTable dt = new DataTable();
                        dt = GetAddLockStatus(NoteType);
                        grid.JSProperties["cpAddLockStatus"] = (Convert.ToString(dt.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dt.Rows[0]["Lock_Todate"]));

                    }
                    else if (IsComplete == -12)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                    }
                    else
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                }
            }
        }
        public DataTable GetAddLockStatus(string type)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForDrCrNote");
            proc.AddVarcharPara("@Type", 100, type);
            dt = proc.GetTable();
            return dt;

        }
        private void SessionClear()
        {
            Session.Remove("SESS_VendorNoteLedgerDT");
            Session.Remove("VDCN_FinalTaxRecord");
        }

        public void DeleteTaxDetails(string SrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["VDCN_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["VDCN_FinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["VDCN_FinalTaxRecord"] = TaxDetailTable;
            }
        }

        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["VDCN_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["VDCN_FinalTaxRecord"];

                for (int i = 0; i < TaxDetailTable.Rows.Count; i++)
                {
                    DataRow dr = TaxDetailTable.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["SlNo"]);
                    if (oldSrlNo == Product_SrlNo)
                    {
                        dr["SlNo"] = newSrlNo;
                    }
                }
                TaxDetailTable.AcceptChanges();

                Session["VDCN_FinalTaxRecord"] = TaxDetailTable;
            }
        }

        #endregion

        #region  Tax
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
            Session["VDCN_FinalTaxRecord"] = TaxRecord;
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

        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["VDCN_FinalTaxRecord"];
                int slNo = Convert.ToInt32(HdSerialNo.Value);
                if (cmbGstCstVat.Value != null)
                {
                    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
                    if (finalRow.Length > 0)
                    {
                        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        finalRow[0]["Amount"] = txtGstCstVat.Text;
                        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                    }
                    else
                    {
                        DataRow newRowGST = TaxRecord.NewRow();
                        newRowGST["slNo"] = slNo;
                        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        newRowGST["TaxCode"] = "0";
                        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                        newRowGST["Amount"] = txtGstCstVat.Text;
                        TaxRecord.Rows.Add(newRowGST);
                    }
                }
                //End Here
                aspxGridTax.JSProperties["cpUpdated"] = "";
                Session["SI_FinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                if (Session["VDCN_FinalTaxRecord"] == null)
                {
                    CreateDataTaxTable();
                }
                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["VDCN_FinalTaxRecord"];
                DataTable databaseReturnTable = (DataTable)Session["SI_QuotationTaxDetails"];
                string type = "P";

                ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@MainAccountID", 100, Convert.ToString(setCurrentProdCode.Value));
                proc.AddVarcharPara("@dtTDate", 10, tDate.Date.ToString("yyyy-MM-dd"));
                proc.AddVarcharPara("@Type", 5, type);
                taxDetail = proc.GetTable();
                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                //Get BranchStateCode
                string BranchStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddlBranch.SelectedValue));
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

                    DataTable VendTable = oDBEngine.GetDataTable("Select CNT_GSTIN from tbl_master_contact where cnt_ContactType='DV' AND ISNULL(CNT_GSTIN,'')<>'' and cnt_internalid='" + Convert.ToString(hdfLookupCustomer.Value) + "'");

                    if (VendTable != null && VendTable.Rows.Count > 0)
                    {
                        string str = Convert.ToString(VendTable.Rows[0]["CNT_GSTIN"]);
                        ShippingState = str.Substring(0, 2);

                    }
                    else
                    {
                        DataTable dtState = oDBEngine.GetDataTable("Select top 1 ISNULL(StateCode,'') StateCode from tbl_master_address inner join tbl_master_state on id=add_state where Isdefault =1 and add_addressType='Shipping' and  add_cntId='" + Convert.ToString(hdfLookupCustomer.Value) + "'");
                        string str = Convert.ToString(dtState.Rows[0]["StateCode"]);
                        ShippingState = str;
                    }

                   // ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                }

                #endregion

                if (ShippingState.Trim() != "" && BranchStateCode != "")
                {
                    if (BranchStateCode == ShippingState)
                    {
                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
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
                if (e.Parameters.Split('~')[0] == "New")
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
                    string keyValue = e.Parameters.Split('~')[0];
                    DataTable TaxRecord = (DataTable)Session["SI_FinalTaxRecord"];
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
                    Session["SI_FinalTaxRecord"] = TaxRecord;

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
            DataTable TaxRecord = (DataTable)Session["VDCN_FinalTaxRecord"];
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
            //For GST/CST/VAT
            //if (cmbGstCstVat.Value != null)
            //{
            //    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
            //    if (finalRow.Length > 0)
            //    {
            //        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        finalRow[0]["Amount"] = txtGstCstVat.Text;
            //        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
            //    }
            //    else
            //    {
            //        DataRow newRowGST = TaxRecord.NewRow();
            //        newRowGST["slNo"] = slNo;
            //        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        newRowGST["TaxCode"] = "0";
            //        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
            //        newRowGST["Amount"] = txtGstCstVat.Text;
            //        TaxRecord.Rows.Add(newRowGST);
            //    }
            //}
            //End Here
            Session["VDCN_FinalTaxRecord"] = TaxRecord;

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
        public List<TaxDetails> setCalculatedOn(List<TaxDetails> gridSource, DataTable taxDt)
        {
            foreach (TaxDetails taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.Taxes_Name.Replace("(+)", "").Replace("(-)", "") + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }
        protected void cmbGstCstVat_Callback(object sender, CallbackEventArgsBase e)
        {
            DateTime quoteDate = Convert.ToDateTime(tDate.Date.ToString("yyyy-MM-dd"));

        }
        #endregion

        protected void deleteTax_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.ToString() == "DeleteAllTax")
            {
                if (Session["VDCN_FinalTaxRecord"] != null)
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

        protected void EntityServerModeDataCustDbCr_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddlBranch.SelectedValue)// && d.CustId == hdfLookupCustomer.Value
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }

        //End Tanmoy Hierarchy
    }
}