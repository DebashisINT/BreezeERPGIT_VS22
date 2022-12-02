using System;
using System.Web;
using System.Web.UI.WebControls;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_ConsumerComp_BankDetails : ERP.OMS.ViewState_class.VSPage
    {
        // DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

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

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            if (!Page.IsPostBack)
            {
                BindInvestment();

            }
        }
        protected void btn_Finance_Save_Click(object sender, EventArgs e)
        {
            string BankDetailsID = HttpContext.Current.Session["KeyVal_InternalID"].ToString();// "EME0000001";//session
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
            string[,] InvestMentData;
            InvestMentData = objEngine.GetFieldValue("tbl_master_contactFinance", "cfc_grossAnnualSalary,cfc_annualTrunover,cfc_grossProfit,cfc_PMExpenses,cfc_PMSaving,cfc_equity,cfc_mutalFund,cfc_bankFD,cfc_debtsInstument,cfc_nss,cfc_lifeInsurance,cfc_healthInsurance,cfc_realEstate,cfc_preciousMetals,cfc_others,cfc_hasFundForInvestment,cfc_availableFund,cfc_investmentHorizen,cfc_readyToTransferPortFoilo,cfc_portFoilioAmount,cfc_House,cfc_Vehicle", "cfc_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and cfc_contactType='contact'", 22);
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
            TextBox bankname = (TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname");
            //ASPxComboBox SerchBank = (ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpSearchBank");
            //string searchVal = SerchBank.SelectedItem.Value.ToString();
            bankname.Attributes.Add("onkeyup", "CallList(this,'bankdetails',event)");
        }
        protected void BankDetailsGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string[] BankDetail = e.NewValues["BankName1"].ToString().Split('~');
            string condition = "";
            if (BankDetail[0].ToString() != "")
            {
                if (BankDetail[3].ToString() == "0")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_bankname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_branchname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "1")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_micrno='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_branchname='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "2")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_branchname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (condition != "")
                {
                    string[,] DT = objEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                    if (DT[0, 0] != "n")
                    {
                        e.NewValues["BankName1"] = DT[0, 0].ToString();
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
            if (e.NewValues["Category"] == null)
            {
                e.RowError = "Please select Category.";
                return;
            }
            if (e.NewValues["AccountType"] == null)
            {
                e.RowError = "Please select Account Type.";
                return;
            }
            if (e.NewValues["BankName1"].ToString() == "")
            {
                e.RowError = "Please select Bank Name.";
                return;
            }
            if (BankDetailsGrid.IsNewRowEditing)
            {
                string Category = e.NewValues["Category"].ToString();
                string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_accountCategory", " cbd_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);
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
                string KeyVal = e.Keys["Id"].ToString();
                string Category = e.NewValues["Category"].ToString();
                string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_id", " cbd_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);
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
        protected void BankDetailsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] BankDetail = e.NewValues["BankName1"].ToString().Split('~');
            string condition = "";
            if (BankDetail[0].ToString() != "")
            {
                if (BankDetail[3].ToString() == "0")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_bankname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_branchname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "1")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_micrno='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_branchname='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "2")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_branchname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (condition != "")
                {
                    string[,] DT = objEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                    if (DT[0, 0] != "n")
                    {
                        e.NewValues["BankName1"] = DT[0, 0].ToString();
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
    }
}