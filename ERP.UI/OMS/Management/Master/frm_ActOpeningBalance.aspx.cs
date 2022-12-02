using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO; 
//using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
//////using DevExpress.Web;
//using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using DevExpress.Web;
using EntityLayer.CommonELS;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{

    public partial class management_master_frm_ActOpeningBalance : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //public string strDataStatus;
        public string strID;
        String strOpeningCr;
        String strOpeningDr;
        DataTable dataTable = new DataTable();
        DBEngine odebEngine = new DBEngine();

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender,EventArgs e)
        {
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_actOpeningBalance.aspx");
           
            if (!IsPostBack)
            {
                Session.Remove("OpeningDatatable");
                if (Session["userbranchHierarchy"] != null)
                {
                    dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
                    cmbBranch.DataBind();
                }
                CreateSessionDatatable();
                RefreshGrid();
                loadMainAccount();
            }

        }

        public void CreateSessionDatatable()
        {
            DataTable OpeningDt = new DataTable();
            OpeningDt.Columns.Add("UniqueId", typeof(System.Guid));
            OpeningDt.Columns.Add("FinYear", typeof(System.String));
            OpeningDt.Columns.Add("CompanyID", typeof(System.String));
            OpeningDt.Columns.Add("BranchId", typeof(System.Int32));
            OpeningDt.Columns.Add("Branch", typeof(System.String));
            OpeningDt.Columns.Add("AccountCode", typeof(System.String));
            OpeningDt.Columns.Add("Account", typeof(System.String));
            OpeningDt.Columns.Add("SubAccountCode", typeof(System.String));
            OpeningDt.Columns.Add("SubAccount", typeof(System.String));
            OpeningDt.Columns.Add("DrCr", typeof(System.String));
            OpeningDt.Columns.Add("DebitAmount", typeof(System.Decimal));
            OpeningDt.Columns.Add("CreditAmount", typeof(System.Decimal));
            OpeningDt.Columns.Add("currency", typeof(System.Decimal));
            Session["OpeningDatatable"] = OpeningDt;
        }

        protected void OpeningGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string returnPara = Convert.ToString(e.Parameters);

            DataTable OpeningDt = (DataTable)Session["OpeningDatatable"];
            #region Add new
            if (returnPara.Split('~')[0] == "Add")
            {
                string subAccountCode = "";
                if (hdSubAccountId.Value != null)
                {
                    subAccountCode = Convert.ToString(hdSubAccountId.Value);
                }
                string accName = GetAccountName(Convert.ToString(cmbAccount.Value));
                DataRow[] filterRow = OpeningDt.Select("AccountCode='" + accName + "' and SubAccountCode='" + subAccountCode + "' and BranchId=" + cmbBranch.Value);
                if (filterRow.Length > 0)
                {
                    filterRow[0]["DrCr"] = cmbDrorCr.Value;
                    string strOpeningDr = txtAmmount.Text;
                    if (Convert.ToString(cmbDrorCr.Value) == "D")
                    {
                        filterRow[0]["DebitAmount"] = strOpeningDr;
                        filterRow[0]["CreditAmount"] = 0.0;
                    }
                    else
                    {
                        filterRow[0]["DebitAmount"] = 0.0;
                        filterRow[0]["CreditAmount"] = strOpeningDr;
                    }
                }
                else
                {
                    DataRow newRow = OpeningDt.NewRow();
                    newRow["UniqueId"] = Guid.NewGuid();
                    newRow["FinYear"] = Convert.ToString(Session["LastFinYear"]);
                    newRow["CompanyID"] = Convert.ToString(Session["LastCompany"]);
                    newRow["BranchId"] = Convert.ToInt32(cmbBranch.Value);
                    newRow["Branch"] = cmbBranch.Text;
                    newRow["AccountCode"] = accName;
                    newRow["Account"] = cmbAccount.Text;
                    newRow["currency"] = Convert.ToString(Session["ActiveCurrency"]).Split('~')[0];
                    if (hdSubAccountId.Value != null)
                    {
                        newRow["SubAccountCode"] = hdSubAccountId.Value;
                        newRow["SubAccount"] = txtSubName.Text;
                    }
                    else
                    {
                        newRow["SubAccountCode"] = string.Empty;
                        newRow["SubAccount"] = string.Empty;
                    }
                    newRow["DrCr"] = cmbDrorCr.Value;
                    string strOpeningDr = txtAmmount.Text;
                    if (Convert.ToString(cmbDrorCr.Value) == "D")
                    {
                        newRow["DebitAmount"] = strOpeningDr;
                        newRow["CreditAmount"] = 0.0;
                    }
                    else
                    {
                        newRow["DebitAmount"] = 0.0;
                        newRow["CreditAmount"] = strOpeningDr;
                    }
                    OpeningDt.Rows.Add(newRow);
                }



                Session["OpeningDatatable"] = OpeningDt;
                OpeningGrid.DataSource = OpeningDt;
                OpeningGrid.DataBind();

                OpeningGrid.JSProperties["cpTotalAmount"] = ComputeTotalDrCr(OpeningDt);
            }
            #endregion
            #region Edit
            else if (returnPara.Split('~')[0] == "Edit")
            {
                DataRow[] filterRow = OpeningDt.Select("UniqueId='" + returnPara.Split('~')[1] + "'");
                if (filterRow.Length > 0)
                {
                    string BranchId = Convert.ToString(filterRow[0]["BranchId"]);
                    string AccountCode =GetAccountCode( Convert.ToString(filterRow[0]["AccountCode"]));
                    string SubAccountCode = Convert.ToString(filterRow[0]["SubAccountCode"]);
                    string subAccountName = Convert.ToString(filterRow[0]["SubAccount"]);
                    string DrCr = Convert.ToString(filterRow[0]["DrCr"]);
                    string Amount;
                    if (DrCr == "D")
                        Amount = Convert.ToString(filterRow[0]["DebitAmount"]);
                    else
                        Amount = Convert.ToString(filterRow[0]["CreditAmount"]);


                    OpeningGrid.JSProperties["cpBeforeEdit"] = returnPara.Split('~')[1] +
                                                              "~" + BranchId +
                                                              "~" + AccountCode +
                                                              "~" + SubAccountCode +
                                                              "~" + DrCr +
                                                              "~" + Amount+
                                                              "~" + subAccountName;

                }


            }
            #endregion
            #region AfterEdit
            else if (returnPara.Split('~')[0] == "EditDone")
            {
                string uniqueId = returnPara.Split('~')[1];
                DataRow[] filterRow = OpeningDt.Select("UniqueId='" + returnPara.Split('~')[1] + "'");
                if (filterRow.Length > 0)
                {
                    filterRow[0]["UniqueId"] = returnPara.Split('~')[1];
                    filterRow[0]["BranchId"] = Convert.ToInt32(cmbBranch.Value);
                    filterRow[0]["Branch"] = cmbBranch.Text;
                    filterRow[0]["AccountCode"] =GetAccountName(Convert.ToString( cmbAccount.Value));
                    filterRow[0]["Account"] = cmbAccount.Text;
                    if (hdSubAccountId.Value != null)
                    {
                        filterRow[0]["SubAccountCode"] = hdSubAccountId.Value;
                        filterRow[0]["SubAccount"] = txtSubName.Text;
                    }
                    filterRow[0]["DrCr"] = cmbDrorCr.Value;
                    string strOpeningDr = txtAmmount.Text;
                    if (Convert.ToString(cmbDrorCr.Value) == "D")
                    {
                        filterRow[0]["DebitAmount"] = strOpeningDr;
                        filterRow[0]["CreditAmount"] = 0.0;
                    }
                    else
                    {
                        filterRow[0]["DebitAmount"] = 0.0;
                        filterRow[0]["CreditAmount"] = strOpeningDr;
                    }


                    Session["OpeningDatatable"] = OpeningDt;
                    OpeningGrid.DataSource = OpeningDt;
                    OpeningGrid.DataBind();

                    OpeningGrid.JSProperties["cpTotalAmount"] = ComputeTotalDrCr(OpeningDt);
                }
            }
            #endregion
            #region SaveAllRecord
            else if (returnPara.Split('~')[0] == "SaveAllRecord")
            {
                SaveAllRecord();
            }
            #endregion

        }

        protected void SaveAllRecord()
        {
            DataTable OpeningDt = (DataTable)Session["OpeningDatatable"];
            DataTable finalDt = OpeningDt.Clone();
            finalDt.Merge(OpeningDt);
            //Remove Extra Columns
            finalDt.Columns.Remove("UniqueId");
            finalDt.Columns.Remove("Branch");
            finalDt.Columns.Remove("Account");
            finalDt.Columns.Remove("SubAccount");
            finalDt.Columns.Remove("DrCr");
            OpeningBalanceBl opb = new OpeningBalanceBl();
            opb.UpdateOpeningBalance(finalDt);
            Session["OpeningDatatable"] = OpeningDt;
            OpeningGrid.DataSource = OpeningDt;
            OpeningGrid.DataBind();
            OpeningGrid.JSProperties["cpClientMsg"] = "Updated Successfully.";
        }

        protected string ComputeTotalDrCr(DataTable computeTable)
        {
            string finalString = "";
            String finalDebit = "0.00", finalCredit = "0.00";
            finalDebit = Convert.ToString(computeTable.Compute("SUM(DebitAmount)", string.Empty));
            finalCredit = Convert.ToString(computeTable.Compute("SUM(CreditAmount)", string.Empty));

            finalString = "Debit~" + finalDebit + "~TotalCredit~" + finalCredit;

            return finalString;
        }

        protected void cmbAccount_CustomCallback(object source, CallbackEventArgsBase e)
        {
            //string branchId = Convert.ToString(e.Parameter);
            //DataTable accountDT;
            //if (branchId == "0")
            //{
            //    accountDT = odebEngine.GetDataTable("select MainAccount_Name ,MainAccount_AccountCode from master_mainaccount order by MainAccount_Name");
            //}
            //else
            //{
            //    accountDT = odebEngine.GetDataTable("select MainAccount_Name ,MainAccount_AccountCode from master_mainaccount where MainAccount_branchId=" + branchId + " order by MainAccount_Name");
            //}

            //ASPxComboBox cuurentCmb = source as ASPxComboBox;
            //cuurentCmb.DataSource = accountDT;
            //cuurentCmb.TextField = "MainAccount_Name";
            //cuurentCmb.ValueField = "MainAccount_AccountCode";
            //cuurentCmb.DataBind();
            loadMainAccount();
        }

        protected void cmbSubAccount_CustomCallback(object source, CallbackEventArgsBase e)
        {
            string AccountId = Convert.ToString(e.Parameter);
            DataTable SubaccountDT = odebEngine.GetDataTable("select SubAccount_Code,SubAccount_Name from Master_SubAccount where SubAccount_MainAcReferenceID ='" + AccountId + "'");

            ASPxComboBox cuurentCmb = source as ASPxComboBox;

            string RequestLetter = "%";
            var SegID = "";
            var SegmentName = "Accounts";




            var ProcedureName = "SubAccountSelect_New";
            var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
            var InputType = "V|V|V|V|V";
            var InputValue = AccountId.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
            var SplitChar = "|";
            var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


            string[] paramSub = CombinedSubQuery.Split('$');


            char SplitSubChar = Convert.ToChar(paramSub[4]);
            string ProcedureSubName = Convert.ToString(paramSub[0]);
            string[] InputSubName = paramSub[1].Split(SplitSubChar);
            string[] InputSubType = paramSub[2].Split(SplitSubChar);
            string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
            string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);

            cuurentCmb.DataSource = GetSubAccount(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
            cuurentCmb.TextField = "Contact_Name";
            cuurentCmb.ValueField = "SubAccount_ReferenceID";
            cuurentCmb.DataBind();


        }


        public DataTable GetSubAccount(string ProcedureSubName, string[] InputSubName, string[] InputSubType, string[] InputSubValue)
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
            if (DT.Columns.Count == 2)
            {
                DT.Columns[0].ColumnName = "Contact_Name";
                DT.Columns[1].ColumnName = "SubAccount_ReferenceID";
            }
            foreach (DataRow dr in DT.Rows)
            {
                string[] data = Convert.ToString(dr["SubAccount_ReferenceID"]).Split('~');
                if (data.Length > 0)
                {
                    dr["SubAccount_ReferenceID"] = data[0];
                }
            }
            return DT;
        }
        protected void RefreshGrid()
        {
            OpeningBalanceBl opb = new OpeningBalanceBl();
            DataTable opDetails = opb.GetOpeningBalanceDetails(Convert.ToString(Session["userbranchHierarchy"]));
            OpeningGrid.DataSource = opDetails;
            Session["OpeningDatatable"] = opDetails;
            OpeningGrid.DataBind(); 
            OpeningGrid.JSProperties["cpTotalAmount"] = ComputeTotalDrCr(opDetails);
            
            //Set Total Amount 
            string TotalAmount = ComputeTotalDrCr(opDetails);
            totalDebit.Text = TotalAmount.Split('~')[1];
            lblTotDebit.Text = TotalAmount.Split('~')[1];

            totalCredit.Text = TotalAmount.Split('~')[3];
            lblTotCredit.Text=TotalAmount.Split('~')[3];


            totalDebit.ClientEnabled = false;
            totalCredit.ClientEnabled = false;
        }

        protected void AmountPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string gotparameter = Convert.ToString(e.Parameter);
            
            if (gotparameter.Split('~')[0] == "MainAccount")
            {
                string branchId = gotparameter.Split('~')[2];
                string mainActId = gotparameter.Split('~')[1];
                DataTable OpeningDt = (DataTable)Session["OpeningDatatable"];
                string mainAcccountCode = GetAccountName(mainActId);
                DataRow[] filterRow = OpeningDt.Select("AccountCode='" + mainAcccountCode + "' and SubAccountCode='' and BranchId='" + branchId+"'");

                if (filterRow.Length > 0)
                {
                    string drCr = "D";
                    drCr = Convert.ToString(filterRow[0]["DrCr"]);
                    if (drCr == "D")
                    {
                        AmountPanel.JSProperties["cpAmount"] = drCr + "~" + Convert.ToString(filterRow[0]["DebitAmount"]);
                    }
                    else
                    {
                        AmountPanel.JSProperties["cpAmount"] = drCr + "~" + Convert.ToString(filterRow[0]["CreditAmount"]);
                    }
                }
                else
                {
                    AmountPanel.JSProperties["cpAmount"] = "D~0.0";
                }

            }

            if (gotparameter.Split('~')[0] == "SubMainAccount")
            {
                string mainActId = gotparameter.Split('~')[1];
                string SubmainActId = gotparameter.Split('~')[2];
                string branchId = gotparameter.Split('~')[3];
                DataTable OpeningDt = (DataTable)Session["OpeningDatatable"];
                DataRow[] filterRow = OpeningDt.Select("AccountCode='" + GetAccountName(mainActId) + "' and SubAccountCode='" + SubmainActId + "' and BranchId='" + branchId+"'");
                if (filterRow.Length > 0)
                {
                    string drCr = "D";
                    drCr = Convert.ToString(filterRow[0]["DrCr"]);
                    if (drCr == "D")
                    {
                        AmountPanel.JSProperties["cpAmount"] = drCr + "~" + Convert.ToString(filterRow[0]["DebitAmount"]);
                    }
                    else
                    {
                        AmountPanel.JSProperties["cpAmount"] = drCr + "~" + Convert.ToString(filterRow[0]["CreditAmount"]);
                    }
                }
                else
                {
                    AmountPanel.JSProperties["cpAmount"] = "D~0.0";
                }

            }



        }


        protected void loadMainAccount()
        {

            ProcedureExecute proc = new ProcedureExecute("prc_accountOpeningLedger");
            proc.AddVarcharPara("@action", 30, "GetMainAccount");
            proc.AddVarcharPara("@branchHierchy", -1, Convert.ToString(Session["userbranchHierarchy"]));
            DataTable accountDT = proc.GetTable();


          //  DataTable accountDT = odebEngine.GetDataTable("select MainAccount_Name ,MainAccount_AccountCode,MainAccount_ReferenceID from master_mainaccount where MainAccount_AccountCode not like 'SYSTM%' and ( MainAccount_branchId in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")  or MainAccount_branchId=0 ) order by MainAccount_Name");
            Session["AccountDetailsOpening"] = accountDT;
            cmbAccount.DataSource = accountDT;
            cmbAccount.TextField = "MainAccount_Name";
            cmbAccount.ValueField = "MainAccount_ReferenceID";
            cmbAccount.DataBind();

        }

        public string GetAccountName(string actCode)
        {
            string accountName = "";
            DataTable accountDT = (DataTable)Session["AccountDetailsOpening"];
            DataRow[] name = accountDT.Select("MainAccount_ReferenceID='" + actCode + "'");
            if (name.Length > 0)
            {
                accountName =Convert.ToString( name[0]["MainAccount_AccountCode"]);
            }


            return accountName.Trim();
        }

        public string GetAccountCode(string actname)
        {
            string accountName = "";
            DataTable accountDT = (DataTable)Session["AccountDetailsOpening"];
            DataRow[] name = accountDT.Select("MainAccount_AccountCode='" + actname + "'");
            if (name.Length > 0)
            {
                accountName = Convert.ToString(name[0]["MainAccount_ReferenceID"]);
            }


            return accountName.Trim();
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["OpeningDatatable"] != null)
            {
                OpeningGrid.DataSource = (DataTable)Session["OpeningDatatable"];
            }


        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            //GridPurchaseReturnREquest.Columns[7].Visible = false;
            //string filename = "Purchase Order";
            //exporter.FileName = filename;
            exporter.GridViewID = "OpeningGrid";
            exporter.FileName = "Opening";

            exporter.PageHeader.Left = "Opening Balance";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }


        
    }
}