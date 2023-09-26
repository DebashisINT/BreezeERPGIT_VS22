//================================================== Revision History ==========================================================================
//1.0  15-05-2023    V2.0.38    Priti  25893 : Import Module Required for Importing Ledger/Subledger Opening
//2.0  25-09-2023    V2.0.39    Priti   0026836 :Opening Account module required the following columns (Log). Entered By, Entered On, Updated By, Updated On.

//====================================================== Revision History ======================================================================

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
//Rev 1.0
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.Web.Services.Description;
using System.IO;
using DocumentFormat.OpenXml.Office.Word;
//Rev 1.0 End
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
        //Rev 1.0
        private static String path, path1, FileName, s, time, cannotParse;
        string FilePath = "";
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        //Rev 1.0 End
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
            //REv 2.0 
            OpeningDt.Columns.Add("CreateUser", typeof(System.String));
            OpeningDt.Columns.Add("CreateDate", typeof(System.String));
            OpeningDt.Columns.Add("ModifyUser", typeof(System.String));
            OpeningDt.Columns.Add("ModifyDate", typeof(System.String));
            //REv 2.0  End
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
                    //Rev 2.0
                    newRow["CreateUser"] = string.Empty;
                    newRow["CreateDate"] = string.Empty;
                    newRow["ModifyUser"] = string.Empty;
                    newRow["ModifyDate"] = string.Empty;
                    //Rev 2.0 end


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
            //Rev 1.0
            if (finalDt.Columns.Contains("CreateUser"))
            {
                finalDt.Columns.Remove("CreateUser");
            }
            if (finalDt.Columns.Contains("CreateDate"))
            {
                finalDt.Columns.Remove("CreateDate");
            }
            if (finalDt.Columns.Contains("ModifyUser"))
            {
                finalDt.Columns.Remove("ModifyUser");
            }
            if (finalDt.Columns.Contains("ModifyDate"))
            {
                finalDt.Columns.Remove("ModifyDate");
            }
            //Rev 1.0 End
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

        //Rev 1.0
        #region Import
        protected void BtnSaveexcel_Click(object sender, EventArgs e)
        {
            string fName = string.Empty;
            Boolean HasLog = false;
            if (OFDBankSelect.FileContent.Length != 0)
            {
                path = String.Empty;
                path1 = String.Empty;
                FileName = String.Empty;
                s = String.Empty;
                time = String.Empty;
                cannotParse = String.Empty;

                BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                FilePath = Path.GetFullPath(OFDBankSelect.PostedFile.FileName);
                FileName = Path.GetFileName(FilePath);
                string fileExtension = Path.GetExtension(FileName);

                if (fileExtension.ToUpper() != ".XLS" && fileExtension.ToUpper() != ".XLSX")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Uploaded file format not supported by the system');</script>");
                    return;
                }

                if (fileExtension.Equals(".xlsx"))
                {
                    fName = FileName.Replace(".xlsx", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                }

                else if (fileExtension.Equals(".xls"))
                {
                    fName = FileName.Replace(".xls", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
                }

                else if (fileExtension.Equals(".csv"))
                {
                    fName = FileName.Replace(".csv", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv");
                }
                Session["FileName"] = fName;

                string subPath = "Import"; // Your code goes here

                bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                String UploadPath = Server.MapPath((Convert.ToString("Import/") + Session["FileName"].ToString()));
                OFDBankSelect.PostedFile.SaveAs(UploadPath);

                ClearArray();

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                try
                {
                    HttpPostedFile file = OFDBankSelect.PostedFile;
                    String extension = Path.GetExtension(FileName);
                    HasLog = Import_To_Grid(UploadPath, extension, file);
                }
                catch (Exception ex)
                {
                    HasLog = false;
                }
                if (!HasLog)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Please flow the log file.!'); ShowLogData('" + HasLog + "');</script>");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Import Process Successfully Completed!'); ShowLogData('" + HasLog + "');</script>");

                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Selected File Cannot Be Blank');</script>");
            }
        }
        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }
        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }
        public static int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }
        public Boolean Import_To_Grid(string FilePath, string Extension, HttpPostedFile file)
        {

            Boolean Success = false;
            Boolean HasLog = false;
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();

                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    {

                        Sheet sheet = spreadSheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                        Worksheet worksheet = (spreadSheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                        foreach (Row row in rows)
                        {
                            if (row.RowIndex.Value == 1)
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    if (cell.CellValue != null)
                                    {
                                        dt.Columns.Add(GetValue(spreadSheetDocument, cell));
                                    }
                                }
                            }
                            else
                            {
                                DataRow tempRow = dt.NewRow();
                                int columnIndex = 0;
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    // Gets the column index of the cell with data
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--; //zero based index
                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            tempRow[columnIndex] = ""; //Insert blank data here;
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }
                                    try
                                    {
                                        tempRow[columnIndex] = GetValue(spreadSheetDocument, cell);
                                    }
                                    catch
                                    {
                                        tempRow[columnIndex] = "";
                                    }

                                    columnIndex++;
                                }
                                dt.Rows.Add(tempRow);
                            }
                        }
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        string Account = string.Empty;
                        string Unit_Name = string.Empty;
                        string SubAccount = string.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            loopcounter++;
                            try
                            {
                                 Unit_Name = Convert.ToString(row["Unit*"]);
                                 Account = Convert.ToString(row["Account*"]);
                                 SubAccount = Convert.ToString(row["Sub Account"]);
                                string BalanceAmount = Convert.ToString(row["Balance Amount*"]);
                                string DR_CR = Convert.ToString(row["DR_CR*"]);                                
                                string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                                string CompanyID = Convert.ToString(Session["LastCompany"]);
                                string FinYear = Convert.ToString(Session["LastFinYear"]);


                                DataSet dt2 = InsertDataFromExcel(Unit_Name, Account, SubAccount, BalanceAmount, DR_CR, CompanyID, FinYear);

                                if (dt2 != null && dt2.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in dt2.Tables[0].Rows)
                                    {
                                        Success = Convert.ToBoolean(row2["Success"]);
                                        HasLog = Convert.ToBoolean(row2["HasLog"]);
                                    }
                                }
                                if (!HasLog)
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    string CustInternal_Id = Convert.ToString(dt2.Tables[0].Rows[0]["CustInternal_Id"]);

                                    int loginsert = InsertImportLog(Account, SubAccount, Unit_Name, loopcounter, UserId, Session["FileName"].ToString(), description, "Failed", CustInternal_Id);
                                }
                                else
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    string CustInternal_Id = Convert.ToString(dt2.Tables[0].Rows[0]["CustInternal_Id"]);
                                    int loginsert = InsertImportLog(Account, SubAccount, Unit_Name, loopcounter, UserId, Session["FileName"].ToString(), description, "Success", CustInternal_Id);
                                }

                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                                // string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                int loginsert = InsertImportLog(Account, SubAccount, Unit_Name, loopcounter, "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed", "");
                            }

                        }
                    }

                }
                else
                {

                }
            }
            return HasLog;
        }
        public DataSet InsertDataFromExcel(string Unit_Name, string Account, string SubAccount, string BalanceAmount, string DR_CR, string CompanyID, string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_OPENINGENTRIESIMPORTFROMEXCEL");
            proc.AddVarcharPara("@Action", 100, "InsertOpeningBalanceDataFromExcel");
            proc.AddVarcharPara("@Unit_Name", 100, Unit_Name);
            proc.AddVarcharPara("@Account", 200, Account);
            proc.AddVarcharPara("@SubAccount", 200, SubAccount);          
            proc.AddPara("@Balance_Amount", Convert.ToDecimal(BalanceAmount));
            proc.AddVarcharPara("@DR_CR", 20, DR_CR);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            //Rev 2.0
            proc.AddPara("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            //Rev 2.0 End
            ds = proc.GetDataSet();
            return ds;
        }
        public int InsertImportLog(string Account, string SubAccount,string Unit_Name , int loopnumber, string userid, string filename, string description, string status, string CUSTOMERID)
        {
            int i;
            ProcedureExecute proc = new ProcedureExecute("PRC_OPENINGENTRIESIMPORTLOG");
            proc.AddVarcharPara("@action", 150, "InsertOpeningBalanceLog");
            proc.AddVarcharPara("@Account", 200, Account);
            proc.AddVarcharPara("@SubAccount", 200, SubAccount);
            proc.AddVarcharPara("@Unit_Name", 200, Unit_Name);
            proc.AddVarcharPara("@Doc_TYpe", 100, "OpeningBalance");
            proc.AddIntegerPara("@LoopNumber", loopnumber);
            proc.AddVarcharPara("@UserId", 150, userid);
            proc.AddVarcharPara("@FileName", 150, filename);
            proc.AddVarcharPara("@decription", 150, description);
            proc.AddVarcharPara("@status", 150, status);
            proc.AddVarcharPara("@CUSTOMERID", 150, CUSTOMERID);
            i = proc.RunActionQuery();

            return i;
        }
        public DataSet GetCustomerLog(string Filename)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_OPENINGENTRIESIMPORTLOG");
            proc.AddVarcharPara("@action", 150, "GeOpeningBalanceLog");
            proc.AddVarcharPara("@FileName", 150, Filename);
            ds = proc.GetDataSet();
            return ds;
        }
        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {

            string strFileName = "ConsolidatedAccountOS.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=ConsolidatedAccountOS.xlsx");
            Response.TransmitFile(strPath);
            Response.End();
        }

        protected void GvLogSearch_DataBinding(object sender, EventArgs e)
        {
            string fileName = Convert.ToString(Session["FileName"]);
            DataSet dt2 = GetCustomerLog(fileName);
            GvLogSearch.DataSource = dt2.Tables[0];
        }
        #endregion
        //Rev 1.0 End

    }
}