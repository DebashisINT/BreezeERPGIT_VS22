using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRrecruitmentagent_BankDetails : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string bankName = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService");
            SqlBank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
             string cnttype = Convert.ToString(Session["ContactType"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //BankDetails.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    BankDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //BankDetails.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    BankDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!Page.IsPostBack)
            {
                
                string[,] EmployeeNameID = objEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
                BindInvestment();

            }
        }
        protected void btn_Finance_Save_Click(object sender, EventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            string BankDetailsID = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);// "EME0000001";//session
            //int CreateUser = 123;//Session UserID
            DateTime CreateDate = Convert.ToDateTime(objEngine.GetDate().ToShortDateString());
            string investmentamount = "", horizon = "", portamount = "";
            int hasfund = 0, portfoilio = 0, house = 0, vehicle = 0;
            if (chkHasFundInvestment.Checked == true)
            {
                hasfund = 1;
                investmentamount = txtAvailableFund.Text;
                horizon = txtInvestmentHorizon.Text;
            }
            else
            {
                hasfund = 0;
                investmentamount = "";
                horizon = "";
            }
            if (chkPortFoilio.Checked == true)
            {
                portfoilio = 1;
                portamount = TxtPortFoilioAmount.Text;
            }
            else
            {
                portfoilio = 0;
                portamount = "";
            }
            if (chkhouse.Checked == true)
            {
                house = 1;
            }
            else
            {
                house = 0;
            }
            if (chkVehicle.Checked == true)
            {
                vehicle = 1;
            }
            else
            {
                vehicle = 0;
            }
            string[,] InternalId = objEngine.GetFieldValue("tbl_master_contactFinance", "cfc_cntId", " cfc_cntId='" + BankDetailsID + "'", 1);
            string IId = InternalId[0, 0];
            if (IId == BankDetailsID)
            {
                string UpdateField = "cfc_grossAnnualSalary='" + txtgrossannualsalary.Text + "',cfc_annualTrunover='" + txtannualTrunover.Text + "',cfc_grossProfit='" + txtGrossProfit.Text + "',cfc_PMExpenses='" + txtPMExpenses.Text + "',cfc_PMSaving='" + txtPMSaving.Text + "',cfc_equity='" + txtequity.Text + "',cfc_mutalFund='" + txtMutalFund.Text + "',cfc_bankFD='" + txtBankFD.Text + "',cfc_debtsInstument='" + txtDebtsInstruments.Text + "',cfc_nss='" + txtNSS.Text + "',cfc_lifeInsurance='" + txtLifeInsurance.Text + "',cfc_healthInsurance='" + txtHealthInsurance.Text + "',cfc_realEstate='" + txtRealEstate.Text + "',cfc_preciousMetals='" + txtPreciousMetals.Text + "',cfc_others='" + txtOthers.Text + "',cfc_hasFundForInvestment=" + hasfund + ",cfc_availableFund='" + investmentamount + "',cfc_investmentHorizen='" + horizon + "',cfc_readyToTransferPortFoilo=" + portfoilio + ",cfc_portFoilioAmount='" + portamount + "',cfc_House=" + house + ",cfc_Vehicle=" + vehicle + ",LastModifyDate='" + CreateDate + "',LastModifyUser='" + HttpContext.Current.Session["userid"] + "'";
                objEngine.SetFieldValue("tbl_master_contactFinance", UpdateField, " cfc_cntId='" + BankDetailsID + "' and cfc_contactType='contact'");
            }
            else
            {
                objEngine.InsurtFieldValue("tbl_master_contactFinance", "cfc_grossAnnualSalary,cfc_annualTrunover,cfc_grossProfit,cfc_PMExpenses,cfc_PMSaving,cfc_equity,cfc_mutalFund,cfc_bankFD,cfc_debtsInstument,cfc_nss,cfc_lifeInsurance,cfc_healthInsurance,cfc_realEstate,cfc_preciousMetals,cfc_others,cfc_hasFundForInvestment,cfc_availableFund,cfc_investmentHorizen,cfc_readyToTransferPortFoilo,cfc_portFoilioAmount,cfc_House,cfc_Vehicle,cfc_cntId,cfc_contactType,CreateDate,CreateUser", "'" + txtgrossannualsalary.Text + "','" + txtannualTrunover.Text + "','" + txtGrossProfit.Text + "','" + txtPMExpenses.Text + "','" + txtPMSaving.Text + "','" + txtequity.Text + "','" + txtMutalFund.Text + "','" + txtBankFD.Text + "','" + txtDebtsInstruments.Text + "','" + txtNSS.Text + "','" + txtLifeInsurance.Text + "','" + txtHealthInsurance.Text + "','" + txtRealEstate.Text + "','" + txtPreciousMetals.Text + "','" + txtOthers.Text + "'," + hasfund + ",'" + investmentamount + "','" + horizon + "'," + portfoilio + ",'" + portamount + "'," + house + "," + vehicle + ",'" + BankDetailsID + "','contact','" + CreateDate + "','" + HttpContext.Current.Session["userid"] + "'");
            }
        }
        public void BindInvestment()
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            string[,] InvestMentData;
            InvestMentData = objEngine.GetFieldValue("tbl_master_contactFinance", "cfc_grossAnnualSalary,cfc_annualTrunover,cfc_grossProfit,cfc_PMExpenses,cfc_PMSaving,cfc_equity,cfc_mutalFund,cfc_bankFD,cfc_debtsInstument,cfc_nss,cfc_lifeInsurance,cfc_healthInsurance,cfc_realEstate,cfc_preciousMetals,cfc_others,cfc_hasFundForInvestment,cfc_availableFund,cfc_investmentHorizen,cfc_readyToTransferPortFoilo,cfc_portFoilioAmount,cfc_House,cfc_Vehicle", "cfc_cntId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and cfc_contactType='contact'", 22);
            if (InvestMentData[0, 0] != "n")
            {
                txtgrossannualsalary.Text = InvestMentData[0, 0];
                txtannualTrunover.Text = InvestMentData[0, 1];
                txtGrossProfit.Text = InvestMentData[0, 2];
                txtPMExpenses.Text = InvestMentData[0, 3];
                txtPMSaving.Text = InvestMentData[0, 4];
                txtequity.Text = InvestMentData[0, 5];
                txtMutalFund.Text = InvestMentData[0, 6];
                txtBankFD.Text = InvestMentData[0, 7];
                txtDebtsInstruments.Text = InvestMentData[0, 8];
                txtNSS.Text = InvestMentData[0, 9];
                txtLifeInsurance.Text = InvestMentData[0, 10];
                txtHealthInsurance.Text = InvestMentData[0, 11];
                txtRealEstate.Text = InvestMentData[0, 12];
                txtPreciousMetals.Text = InvestMentData[0, 13];
                txtOthers.Text = InvestMentData[0, 14];
                chkHasFundInvestment.Checked = Convert.ToBoolean(InvestMentData[0, 15]);
                txtAvailableFund.Text = InvestMentData[0, 16];
                txtInvestmentHorizon.Text = InvestMentData[0, 17];
                chkPortFoilio.Checked = Convert.ToBoolean(InvestMentData[0, 18]);
                TxtPortFoilioAmount.Text = InvestMentData[0, 19];
                chkhouse.Checked = Convert.ToBoolean(InvestMentData[0, 20]);
                chkVehicle.Checked = Convert.ToBoolean(InvestMentData[0, 21]);
            }
        }
        protected void BankDetailsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            //ASPxTextBox txtBox = (sender as ASPxTextBox);
            //ASPxTextBox bankname = (ASPxTextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname");

            //bankname.Attributes.Add("onkeyup", "CallList(this,'bankdetails',event)");

            //TextBox bankname = (TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname");
            //bankname.Attributes.Add("onkeyup", "CallList(this,'bankdetails',event)");

            int rowindex = BankDetailsGrid.EditingRowVisibleIndex;
            if (Convert.ToString(BankDetailsGrid.GetRowValues(rowindex, "BankName1")) != null)
            {
                bankName = Convert.ToString(BankDetailsGrid.GetRowValues(rowindex, "BankName1"));
            }
        }        
        //protected void txtbankname_Init(object sender, EventArgs e)
        //{
        //    (sender as ASPxTextBox).Attributes.Add("onkeyup", "CallList(this,'bankdetails',event)");
        //}
        protected void BankDetailsGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            string[] BankDetail = Convert.ToString(e.NewValues["BankName1"]).Split('~');
            string condition = "";
            if (Convert.ToString(BankDetail[0]) != "")
            {
                if (Convert.ToString(e.NewValues["BankName1"]).Split('~').Length > 3)
                {
                    if (Convert.ToString(BankDetail[5]) == "0")
                    {
                        if (BankDetail[0] != "")
                            condition = " bnk_bankname='" + Convert.ToString(BankDetail[0]) + "'  ";
                        if (BankDetail[1] != "")
                            condition += " and bnk_branchname='" + Convert.ToString(BankDetail[1]) + "'  ";
                        if (BankDetail[2] != "")
                            condition += " and bnk_micrno='" + Convert.ToString(BankDetail[2]) + "'";
                    }
                    if (Convert.ToString(BankDetail[5]) == "1")
                    {
                        if (BankDetail[0] != "")
                            condition = " bnk_micrno='" + Convert.ToString(BankDetail[0]) + "'  ";
                        if (BankDetail[1] != "")
                            condition += " and bnk_bankname='" + Convert.ToString(BankDetail[1]) + "'  ";
                        if (BankDetail[2] != "")
                            condition += " and bnk_branchname='" + Convert.ToString(BankDetail[2]) + "'";
                    }
                    if (Convert.ToString(BankDetail[5]) == "2")
                    {
                        if (BankDetail[0] != "")
                            condition = " bnk_branchname='" + Convert.ToString(BankDetail[0]) + "'  ";
                        if (BankDetail[1] != "")
                            condition += " and bnk_bankname='" + Convert.ToString(BankDetail[1]) + "'  ";
                        if (BankDetail[2] != "")
                            condition += " and bnk_micrno='" + Convert.ToString(BankDetail[2]) + "'";
                    }
                }
                if (condition != "")
                {
                    string[,] DT = objEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                    if (DT[0, 0] != "n")
                    {
                        e.NewValues["BankName1"] = Convert.ToString(DT[0, 0]);
                    }
                    else
                    {
                        lblmessage.Text = "Bank Name is not available in the database!";
                        return;
                    }
                }
                else
                {
                    lblmessage.Text = "Bank Name is not available in the database!";
                    BankDetailsGrid.CancelEdit();
                }
            }
            else
            {
                lblmessage.Text = "Please enter a valid Bank Name!";
                BankDetailsGrid.CancelEdit();
            }
        }
        protected void BankDetailsGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            //if (e.NewValues["Category"] == null)
            //{
            //    e.RowError = "Please select Category.";
            //    return;
            //}
            //if (e.NewValues["AccountType"] == null)
            //{
            //    e.RowError = "Please select Account Type.";
            //    return;
            //}
            if (Convert.ToString(e.NewValues["BankName1"]) == "")
            {
                e.RowError = "Please select Bank Name.";
                return;
            }

            Session["EmpBankSearchBy"] = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpSearchBank")).SelectedIndex;
            if (e.Keys.Count != 0)
                Session["EmployeeBanID"] = Convert.ToString(e.Keys[0]);
            Session["EmpFullBankName"] = Convert.ToString(e.NewValues["BankName1"]);

            try
            {
                if (BankDetailsGrid.IsNewRowEditing)
                {
                    string Category = Convert.ToString(e.NewValues["Category"]);
                    string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_accountCategory", " cbd_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "' and cbd_accountCategory='" + Category + "'", 1);
                    if (Category1[0, 0] != "n")
                    {
                        if (Category1[0, 0] == "Default")
                        {
                            e.RowError = "Default Category Already Exists!";
                            return;
                        }
                    }
                }
                else
                {
                    string KeyVal = Convert.ToString(e.Keys["Id"]);
                    string Category = Convert.ToString(e.NewValues["Category"]);
                    string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_id", " cbd_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "' and cbd_accountCategory='" + Category + "'", 1);
                    if (Category1[0, 0] != "n")
                    {
                        if (KeyVal != Category1[0, 0])
                        {
                            e.RowError = "Default Category Already Exists!";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void BankDetailsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016
            string[] BankDetail = Convert.ToString(e.NewValues["BankName1"]).Split('~');
            string condition = "";
            if (Convert.ToString(BankDetail[0]) != "")
            {
                if (Convert.ToString(BankDetail[5]) == "0")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_bankname='" + Convert.ToString(BankDetail[0]) + "'  ";
                    if (BankDetail[1] != "")
                        condition += " and bnk_branchname='" + Convert.ToString(BankDetail[1]) + "'  ";
                    if (BankDetail[2] != "")
                        condition += " and bnk_micrno='" + Convert.ToString(BankDetail[2]) + "'";
                }
                if (Convert.ToString(BankDetail[5]) == "1")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_micrno='" + Convert.ToString(BankDetail[0]) + "'  ";
                    if (BankDetail[1] != "")
                        condition += " and bnk_bankname='" + Convert.ToString(BankDetail[1]) + "'  ";
                    if (BankDetail[2] != "")
                        condition += " and bnk_branchname='" + Convert.ToString(BankDetail[2]) + "'";
                }
                if (Convert.ToString(BankDetail[5]) == "2")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_branchname='" + Convert.ToString(BankDetail[0]) + "'  ";
                    if (BankDetail[1] != "")
                        condition += " and bnk_bankname='" + Convert.ToString(BankDetail[1]) + "'  ";
                    if (BankDetail[2] != "")
                        condition += " and bnk_micrno='" + Convert.ToString(BankDetail[2]) + "'";
                }
                if (condition != "")
                {
                    string[,] DT = objEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                    if (DT[0, 0] != "n")
                    {
                        e.NewValues["BankName1"] = Convert.ToString(DT[0, 0]);
                    }
                    else
                    {
                        BankDetailsGrid.CancelEdit();
                    }
                }
                else
                {
                    BankDetailsGrid.CancelEdit();
                }
            }
            else
            {
                BankDetailsGrid.CancelEdit();
            }
        }
        protected void chkPortFoilio_CheckedChanged(object sender, EventArgs e)
        {
            TxtPortFoilioAmount.Enabled = true;
        }
        //purpose : User rights in bank details tab 
        //Name : Debjyoti 17-11-2016
        protected void BankDetailsGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }
            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }
        // end debjyoti 17-11-2016//

        //Purpose : Add Compbo Box in Bank Name
        //Name : Sudip 
        // Dated : 23-12-2016
        protected void BankDetailsGrid_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            ASPxComboBox combo = (ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpSearchBank");
        }
        protected void AspxBankCombo_CustomCallback(object source, CallbackEventArgsBase e)
        {
            string searchBy = e.Parameter;

            ASPxComboBox combo = (ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("AspxBankCombo");
            LoadbankNameCombo(combo, Convert.ToInt32(searchBy));
        }
        protected void BankDetailsGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            BankDetailsGrid.SettingsText.PopupEditFormCaption = "Modify Bank Details";
            Session["EmployeeBanID"] = Convert.ToString(e.EditingKeyValue);
        }
        protected void LoadbankNameCombo(ASPxComboBox cmb, int selecttedIndex)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 23-12-2016

            if (Session["EmpBankSearchBy"] != null)
            {
                selecttedIndex = Convert.ToInt32(Convert.ToString(Session["EmpBankSearchBy"]));
                Session["EmpBankSearchBy"] = null;
            }


            if (selecttedIndex == 0)
            {
               // SqlBank.SelectCommand = "select isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~' + isnull(bnk_micrno,'') + '~0' as id,isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~' + isnull(bnk_micrno,'') + '~0' as name from tbl_master_bank";
                SqlBank.SelectCommand = "select isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~' + isnull(bnk_micrno,'') + '~' + isnull(bnk_AcNo,'')+ '~' + isnull(bnk_IFSCCode,'') + '~0' as id,isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~' + isnull(bnk_micrno,'') as name from tbl_master_bank";
            }
            else if (selecttedIndex == 1)
            {
               // SqlBank.SelectCommand = "select isnull(bnk_micrno,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~1' as id, isnull(bnk_micrno,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~1' as name from tbl_master_bank";
                SqlBank.SelectCommand = "select isnull(bnk_micrno,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'')  + '~' + isnull(bnk_AcNo,'')+ '~' + isnull(bnk_IFSCCode,'') + '~1' as id, isnull(bnk_micrno,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'')  as name from tbl_master_bank";
            }
            else
            {
                //SqlBank.SelectCommand = "select isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'') + '~2' as id,isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'') + '~2' as name from tbl_master_bank";
                SqlBank.SelectCommand = "select isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'') + '~' + isnull(bnk_AcNo,'')+ '~' + isnull(bnk_IFSCCode,'') + '~2' as id,isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'')  as name from tbl_master_bank";
            }
            cmb.DataSource = SqlBank;
            cmb.DataBind();

            if (Session["EmployeeBanID"] != null)
            {
               // string[,] BankName = objEngine.GetFieldValue("tbl_trans_contactBankDetails,tbl_master_Bank", "(tbl_master_Bank.bnk_bankName + '~' + tbl_master_Bank.bnk_branchName + '~' + tbl_master_Bank.bnk_micrno + '~' + isnull(tbl_master_Bank.bnk_AcNo,'')+ '~' + isnull(tbl_master_Bank.bnk_IFSCCode,'') + '~0')", "cbd_id='" + Convert.ToString(Session["EmployeeBanID"]) + "' and tbl_master_Bank.bnk_id=tbl_trans_contactBankDetails.cbd_bankCode  ", 1);
                //string[,] BankName = objEngine.GetFieldValue("tbl_trans_contactBankDetails,tbl_master_Bank", "(tbl_master_Bank.bnk_bankName)", "cbd_id='" + Convert.ToString(Session["EmployeeBanID"]) + "' and tbl_master_Bank.bnk_id=tbl_trans_contactBankDetails.cbd_bankCode  ", 1);
                string[,] BankName = objEngine.GetFieldValue("tbl_trans_contactBankDetails,tbl_master_Bank", "(tbl_master_Bank.bnk_bankName + '~' + tbl_master_Bank.bnk_branchName + '~' + tbl_master_Bank.bnk_micrno )", "cbd_id='" + Convert.ToString(Session["EmployeeBanID"]) + "' and tbl_master_Bank.bnk_id=tbl_trans_contactBankDetails.cbd_bankCode  ", 1);

                setComboItem(cmb, Convert.ToString(BankName[0, 0]));
                Session["EmployeeBanID"] = null;
            }
            else if (Session["EmpFullBankName"] != null)
            {
                setComboItem(cmb, Convert.ToString(Session["EmpFullBankName"]));
                Session["EmpFullBankName"] = null;
            }
        }
        protected void setComboItem(ASPxComboBox combo, String bankfullName)
        {
            int cindex = -1;
            foreach (var item in combo.Items)
            {
                cindex = cindex + 1;
                if (Convert.ToString(item) == bankfullName)
                {
                    break;
                }
            }
            combo.SelectedIndex = cindex;
        }
        // end Sudip 23-12-2016//

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
            BankDetailsGrid.Columns[7].Visible = false;
            string filename = "Employee Bank ";
            exporter.FileName = filename;
            exporter.PageHeader.Left = filename;
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