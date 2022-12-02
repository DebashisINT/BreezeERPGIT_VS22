using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using DevExpress.Web;
//////using DevExpress.Web.ASPxTabControl;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
//using DevExpress.Web.ASPxCallbackPanel;
//////using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
//using System.Data.SqlClient;
////using DevExpress.Web.ASPxPopupControl;
using System.Collections.Generic;
using DevExpress.Web;
using System.Data.SqlClient;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{

    public partial class management_Contact_BankDetails : System.Web.UI.Page
    {
        static int iCount = 0;

        string id = "";
        object sender = new object();
        string refreshTime = "";
        bool isopen = false;
        //GenericStoreProcedure objGenericStoreProcedure;
        BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure;
        int existingId = 0;
        string BankDetailsID = "";
        // DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
       // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        clsDropDownList OclsDropDownList = new clsDropDownList();
        public string pageAccess = "";
        string pageVisibility = "";
        String bankid = "B";
        string DPID = "Y";

        //DBEngine oDBEngine = new DBEngine(string.Empty);
        //Converter Oconverter = new Converter();

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();

        String Stat = "N";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);

            }
            if (Request.QueryString["id"] != null)
            {
                string id = Request.QueryString["ID"].ToString();
                Session["KeyVal_InternalID"] = id;
                Session["KeyVal"] = "N";
                pageVisibility = "N";

            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //BankDetailsGrid.DataBind();
            //if (isopen == true)
            // {
            string str = Request.Form["__EVENTTARGET"];
            // BankDetailsGrid.CancelEdit();
            if (str != null && str == "")
            {
                //Response.Redirect(Request.RawUrl);

            }



            // }
        }



        protected void Page_Init(object sender, EventArgs e)
        {
            BankDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            Session["Status"] = "n";


            if (!IsPostBack)
            {
                isopen = false;
            }

            string s = Request.RawUrl;
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (Session["Name"] != null)
            {
                lblName.Text = Session["Name"].ToString();
            }
            if (Request.QueryString["formtype"] != null)
            {
                BankDetailsID = Session["InternalId"].ToString();
                DisabledTabPage();
            }
            else

                BankDetailsID = HttpContext.Current.Session["KeyVal_InternalID"].ToString();
            Session["KeyVal_InternalID_New"] = BankDetailsID;
            if (!Page.IsPostBack)
            {
                StDate.UseMaskBehavior = true;
                StDate.EditFormatString = Oconverter.GetDateFormat("Date");
                dtEffect.UseMaskBehavior = true;
                dtEffect.EditFormatString = Oconverter.GetDateFormat("Date");
                BindIncomeRange();
                BindFinYear();
                BindInvestment();


            }
            if (Session["requesttype"] != null)
            {
                if (Session["requesttype"].ToString() == "Recruitment Agent")
                {
                    //BankDetailsPanel.Visible = false;
                }
            }

            if (pageVisibility == "N")
            {
                TabPage page = ASPxPageControl1.TabPages.FindByName("General");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("DP Details");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Documents");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Registration");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Group Member");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Deposit");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Remarks");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Education");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Other");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Subscription");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Family Members");
                page.Visible = false;
            }

            txtgrossannualsalary.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtgrossannualsalary2.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtannualTrunover.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtannualTrunover2.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtGrossProfit.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtGrossProfit2.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtPMExpenses.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtPMExpenses2.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtPMSaving.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtPMSaving2.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtequity.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtMutalFund.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtBankFD.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtDebtsInstruments.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtNSS.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtLifeInsurance.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtHealthInsurance.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtRealEstate.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtPreciousMetals.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtOthers.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtAvailableFund.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtInvestmentHorizon.Attributes.Add("onKeypress", "return MaskMoney(event)");
            TxtPortFoilioAmount.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtbank1.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtbank2.Attributes.Add("onKeypress", "return MaskMoney(event)");


        }
        protected void BindGrid()
        {
            BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            string insuId = Convert.ToString(Session["KeyVal_InternalID_New"]);
            string[] strSpParam = new string[1];

            strSpParam[0] = "insuId|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + insuId + "|" + GenericStoreProcedure.ParamType.ExParam;
            DataTable dtBank = new DataTable();
            dtBank = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "BankDetailsSelect");
            BankDetailsGrid.DataSource = null;
            BankDetailsGrid.DataSource = dtBank;
            BankDetailsGrid.DataBind();


        }

        protected void BindIncomeRange()
        {
            string[,] Relation = oDBEngine.GetFieldValue("Master_annualIncome", "AnnualIncome_ID,AnnualIncome_Description", null, 2);
            if (Relation[0, 0] != "n")
            {

                //oDBEngine.AddDataToDropDownList(Relation, ddlIncomeRange);

                OclsDropDownList.AddDataToDropDownList(Relation, ddlIncomeRange);
                ddlIncomeRange.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }

        protected void btn_Finance_Save_Click(object sender, EventArgs e)
        {
            if (Session["requesttype"] != null)
            {
                int ContType = 0;
                switch (Session["requesttype"].ToString())
                {
                    case "customer":
                        ContType = 1;
                        break;
                    case "subbroker":
                        ContType = 2;
                        break;
                    case "franchisee":
                        ContType = 3;
                        break;
                    case "referalagent":
                        ContType = 8;
                        break;
                    case "broker":
                        ContType = 5;
                        break;
                    case "agent":
                        ContType = 6;
                        break;
                    case "datavendor":
                        ContType = 7;
                        break;
                    case "vendor":
                        ContType = 10;
                        break;
                    case "partner":
                        ContType = 11;
                        break;
                    case "consultant":
                        ContType = 12;
                        break;
                    case "shareholder":
                        ContType = 13;
                        break;
                    case "creditor":
                        ContType = 14;
                        break;
                    case "debtor":
                        ContType = 15;
                        break;

                }
            }
            //DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            string investmentamount = "", horizon = "", portamount = "";
            string GrossAnualSal = "", annualTrunover = "", grossProfit = "";
            string GrossExpense = "";
            string GrossSavings = "";
            string bankstatement1 = "";
            string bankstatement2 = "";

            if (txtGrossProfit2.Text.ToString() == "")
            {
                grossProfit = "0";
            }
            else
            {
                grossProfit = txtGrossProfit2.Text;
            }

            if (txtannualTrunover2.Text.ToString() == "")
            {
                annualTrunover = "0";
            }
            else
            {
                annualTrunover = txtannualTrunover2.Text;
            }

            if (txtgrossannualsalary2.Text.ToString() == "")
            {
                GrossAnualSal = "0";
            }
            else
            {
                GrossAnualSal = txtgrossannualsalary2.Text;
            }

            if (txtPMExpenses2.Text.ToString() == "")
            {
                GrossExpense = "0";
            }
            else
            {
                GrossExpense = txtPMExpenses2.Text;
            }

            if (txtPMSaving2.Text.ToString() == "")
            {
                GrossSavings = "0";
            }
            else
            {
                GrossSavings = txtPMSaving2.Text;
            }

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
            if (txtbank1.Text.ToString() == "")
            {
                bankstatement1 = "0";
            }
            else
            {
                bankstatement1 = txtbank1.Text.ToString().Trim();
            }
            if (txtbank2.Text.ToString() == "")
            {
                bankstatement2 = "0";
            }
            else
            {
                bankstatement2 = txtbank2.Text.ToString().Trim();
            }
            if (txtNetworth.Text == "")
                txtNetworth.Text = "0";
            if (cmbFinYear.SelectedItem.Value.ToString() == "0")
            {
                DataTable dtEx = oDBEngine.GetDataTable("tbl_master_contactFinance ", "cfc_id", "cfc_FinYear ='" + drpFinyear.SelectedItem.Value + "' and (cfc_EffectDate='" + dtEffect.Value + "' Or cfc_EffectDate > '" + dtEffect.Value + "') and cfc_cntid='" + BankDetailsID + "'");
                if (dtEx.Rows.Count == 0)
                {

                    objEngine.InsurtFieldValue("tbl_master_contactFinance", "cfc_grossAnnualSalary,cfc_annualTrunover,cfc_grossProfit,cfc_PMExpenses,cfc_PMSaving,cfc_equity,cfc_mutalFund,cfc_bankFD,cfc_debtsInstument,cfc_nss,cfc_lifeInsurance,cfc_healthInsurance,cfc_realEstate,cfc_preciousMetals,cfc_others,cfc_hasFundForInvestment,cfc_availableFund,cfc_investmentHorizen,cfc_readyToTransferPortFoilo,cfc_portFoilioAmount,cfc_House,cfc_Vehicle,cfc_cntId,cfc_contactType,CreateDate,CreateUser,cfc_grossAnnualSalary2,cfc_annualTrunover2,cfc_grossProfit2,cfc_PMExpenses2,cfc_PMSaving2,cfc_FinYear,cfc_EffectDate,cfc_bankstatementhigh,cfc_bankstatementlow,cfc_AnnualIncomeRange,cfc_Networth", "'" + txtgrossannualsalary.Text + "','" + txtannualTrunover.Text + "','" + txtGrossProfit.Text + "','" + txtPMExpenses.Text + "','" + txtPMSaving.Text + "','" + txtequity.Text + "','" + txtMutalFund.Text + "','" + txtBankFD.Text + "','" + txtDebtsInstruments.Text + "','" + txtNSS.Text + "','" + txtLifeInsurance.Text + "','" + txtHealthInsurance.Text + "','" + txtRealEstate.Text + "','" + txtPreciousMetals.Text + "','" + txtOthers.Text + "'," + hasfund + ",'" + investmentamount + "','" + horizon + "'," + portfoilio + ",'" + portamount + "'," + house + "," + vehicle + ",'" + BankDetailsID + "','contact','" + CreateDate + "','" + HttpContext.Current.Session["userid"] + "','" + GrossAnualSal + "','" + annualTrunover + "','" + grossProfit + "','" + GrossExpense + "','" + GrossSavings + "','" + drpFinyear.SelectedItem.Value + "','" + dtEffect.Value + "','" + bankstatement1 + "','" + bankstatement2 + "','" + ddlIncomeRange.SelectedItem.Value + "'," + txtNetworth.Text + "");
                    BindFinYear();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alt1", "alert('Successfully Added!..')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alt", "alert('Already Exists!..')", true);
                }
            }
            else
            {
                string UpdateField = "cfc_grossAnnualSalary='" + txtgrossannualsalary.Text + "',cfc_annualTrunover='" + txtannualTrunover.Text + "',cfc_grossProfit='" + txtGrossProfit.Text + "',cfc_PMExpenses='" + txtPMExpenses.Text + "',cfc_PMSaving='" + txtPMSaving.Text + "',cfc_equity='" + txtequity.Text + "',cfc_mutalFund='" + txtMutalFund.Text + "',cfc_bankFD='" + txtBankFD.Text + "',cfc_debtsInstument='" + txtDebtsInstruments.Text + "',cfc_nss='" + txtNSS.Text + "',cfc_lifeInsurance='" + txtLifeInsurance.Text + "',cfc_healthInsurance='" + txtHealthInsurance.Text + "',cfc_realEstate='" + txtRealEstate.Text + "',cfc_preciousMetals='" + txtPreciousMetals.Text + "',cfc_others='" + txtOthers.Text + "',cfc_hasFundForInvestment=" + hasfund + ",cfc_availableFund='" + investmentamount + "',cfc_investmentHorizen='" + horizon + "',cfc_readyToTransferPortFoilo=" + portfoilio + ",cfc_portFoilioAmount='" + portamount + "',cfc_House=" + house + ",cfc_Vehicle=" + vehicle + ",LastModifyDate='" + CreateDate + "',LastModifyUser='" + HttpContext.Current.Session["userid"] + "',cfc_grossAnnualSalary2='" + GrossAnualSal + "',cfc_annualTrunover2='" + annualTrunover + "',cfc_grossProfit2='" + grossProfit + "',cfc_PMExpenses2='" + GrossExpense + "',cfc_PMSaving2='" + GrossSavings + "',cfc_FinYear='" + drpFinyear.SelectedItem.Value + "',cfc_EffectDate='" + dtEffect.Value + "',cfc_bankstatementhigh='" + bankstatement1 + "',cfc_bankstatementlow='" + bankstatement2 + "',cfc_AnnualIncomeRange='" + ddlIncomeRange.SelectedItem.Value + "',cfc_Networth='" + txtNetworth.Text + "'";
                objEngine.SetFieldValue("tbl_master_contactFinance", UpdateField, " cfc_id='" + cmbFinYear.SelectedItem.Value + "' and cfc_contactType='contact'");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alt3", "alert('Successfully Updated!..')", true);
                //string popupScript = "";
                //popupScript = "<script language='javascript'>" + "window.close();</script>";
                //ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);

            }
            //string[,] InternalId = objEngine.GetFieldValue("tbl_master_contactFinance", "cfc_cntId", " cfc_cntId='" + BankDetailsID + "'", 1);
            //string IId = InternalId[0, 0];
            //if (IId == BankDetailsID)
            //{
            //    string UpdateField = "cfc_grossAnnualSalary='" + txtgrossannualsalary.Text + "',cfc_annualTrunover='" + txtannualTrunover.Text + "',cfc_grossProfit='" + txtGrossProfit.Text + "',cfc_PMExpenses='" + txtPMExpenses.Text + "',cfc_PMSaving='" + txtPMSaving.Text + "',cfc_equity='" + txtequity.Text + "',cfc_mutalFund='" + txtMutalFund.Text + "',cfc_bankFD='" + txtBankFD.Text + "',cfc_debtsInstument='" + txtDebtsInstruments.Text + "',cfc_nss='" + txtNSS.Text + "',cfc_lifeInsurance='" + txtLifeInsurance.Text + "',cfc_healthInsurance='" + txtHealthInsurance.Text + "',cfc_realEstate='" + txtRealEstate.Text + "',cfc_preciousMetals='" + txtPreciousMetals.Text + "',cfc_others='" + txtOthers.Text + "',cfc_hasFundForInvestment=" + hasfund + ",cfc_availableFund='" + investmentamount + "',cfc_investmentHorizen='" + horizon + "',cfc_readyToTransferPortFoilo=" + portfoilio + ",cfc_portFoilioAmount='" + portamount + "',cfc_House=" + house + ",cfc_Vehicle=" + vehicle + ",LastModifyDate='" + CreateDate + "',LastModifyUser='" + HttpContext.Current.Session["userid"] + "',cfc_grossAnnualSalary2='" + GrossAnualSal + "',cfc_annualTrunover2='" + annualTrunover + "',cfc_grossProfit2='" + grossProfit + "',cfc_PMExpenses2='" + GrossExpense + "',cfc_PMSaving2='" + GrossSavings + "',cfc_FinYear='" + drpFinyear.SelectedItem.Value + "',cfc_EffectDate='" + dtEffect.Value + "'";
            //    objEngine.SetFieldValue("tbl_master_contactFinance", UpdateField, " cfc_cntId='" + BankDetailsID + "' and cfc_contactType='contact'");
            //    string popupScript = "";
            //    popupScript = "<script language='javascript'>" + "window.close();</script>";
            //    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            //}
            //else
            //{
            //    objEngine.InsurtFieldValue("tbl_master_contactFinance", "cfc_grossAnnualSalary,cfc_annualTrunover,cfc_grossProfit,cfc_PMExpenses,cfc_PMSaving,cfc_equity,cfc_mutalFund,cfc_bankFD,cfc_debtsInstument,cfc_nss,cfc_lifeInsurance,cfc_healthInsurance,cfc_realEstate,cfc_preciousMetals,cfc_others,cfc_hasFundForInvestment,cfc_availableFund,cfc_investmentHorizen,cfc_readyToTransferPortFoilo,cfc_portFoilioAmount,cfc_House,cfc_Vehicle,cfc_cntId,cfc_contactType,CreateDate,CreateUser,cfc_grossAnnualSalary2,cfc_annualTrunover2,cfc_grossProfit2,cfc_PMExpenses2,cfc_PMSaving2,cfc_FinYear,cfc_EffectDate", "'" + txtgrossannualsalary.Text + "','" + txtannualTrunover.Text + "','" + txtGrossProfit.Text + "','" + txtPMExpenses.Text + "','" + txtPMSaving.Text + "','" + txtequity.Text + "','" + txtMutalFund.Text + "','" + txtBankFD.Text + "','" + txtDebtsInstruments.Text + "','" + txtNSS.Text + "','" + txtLifeInsurance.Text + "','" + txtHealthInsurance.Text + "','" + txtRealEstate.Text + "','" + txtPreciousMetals.Text + "','" + txtOthers.Text + "'," + hasfund + ",'" + investmentamount + "','" + horizon + "'," + portfoilio + ",'" + portamount + "'," + house + "," + vehicle + ",'" + BankDetailsID + "','contact','" + CreateDate + "','" + HttpContext.Current.Session["userid"] + "','" + GrossAnualSal + "','" + annualTrunover + "','" + grossProfit + "','" + GrossExpense + "','" + GrossSavings + "','" + drpFinyear.SelectedItem.Value + "','" + dtEffect.Value + "'");

            //}

        }
        protected void chkHasFundInvestment_CheckedChanged(object sender, EventArgs e)
        {
            txtAvailableFund.Enabled = true;
            txtInvestmentHorizon.Enabled = true;
        }
        protected void chkPortFoilio_CheckedChanged(object sender, EventArgs e)
        {
            TxtPortFoilioAmount.Enabled = true;
        }

        public void BindInvestment()
        {

            drpFinyear.Items.Clear();
            DataTable DtFinYear = oDBEngine.GetDataTable("master_finyear ", "finyear_code ,finyear_startdate ", null, " finyear_startdate desc");
            if (DtFinYear.Rows.Count > 0)
            {
                for (int i = 0; i < DtFinYear.Rows.Count; i++)
                {
                    drpFinyear.Items.Add(new ListItem(DtFinYear.Rows[i][0].ToString(), DtFinYear.Rows[i][0].ToString()));
                }

            }

            if (cmbFinYear.SelectedItem.Value.ToString() != "0")
            {

                string[,] InvestMentData = objEngine.GetFieldValue("tbl_master_contactFinance", "cfc_grossAnnualSalary,cfc_annualTrunover,cfc_grossProfit,cfc_PMExpenses,cfc_PMSaving,cfc_equity,cfc_mutalFund,cfc_bankFD,cfc_debtsInstument,cfc_nss,cfc_lifeInsurance,cfc_healthInsurance,cfc_realEstate,cfc_preciousMetals,cfc_others,cfc_hasFundForInvestment,cfc_availableFund,cfc_investmentHorizen,cfc_readyToTransferPortFoilo,cfc_portFoilioAmount,cfc_House,cfc_Vehicle,cfc_grossAnnualSalary2,cfc_annualTrunover2,cfc_grossProfit2,cfc_PMExpenses2,cfc_PMSaving2,cfc_FinYear,cfc_EffectDate,cfc_bankstatementhigh,cfc_bankstatementlow,cfc_AnnualIncomeRange,cfc_Networth", "cfc_Id='" + cmbFinYear.SelectedItem.Value + "' and cfc_contactType='contact'", 33);
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
                    if (InvestMentData[0, 15] != "")
                        chkHasFundInvestment.Checked = Convert.ToBoolean(InvestMentData[0, 15]);
                    txtAvailableFund.Text = InvestMentData[0, 16];
                    txtInvestmentHorizon.Text = InvestMentData[0, 17];
                    if (InvestMentData[0, 18] != "")
                        chkPortFoilio.Checked = Convert.ToBoolean(InvestMentData[0, 18]);
                    TxtPortFoilioAmount.Text = InvestMentData[0, 19];
                    if (InvestMentData[0, 20] != "")
                        chkhouse.Checked = Convert.ToBoolean(InvestMentData[0, 20]);
                    if (InvestMentData[0, 21] != "")
                        chkVehicle.Checked = Convert.ToBoolean(InvestMentData[0, 21]);

                    txtgrossannualsalary2.Text = InvestMentData[0, 22];
                    txtannualTrunover2.Text = InvestMentData[0, 23];
                    txtGrossProfit2.Text = InvestMentData[0, 24];
                    txtPMExpenses2.Text = InvestMentData[0, 25];
                    txtPMSaving2.Text = InvestMentData[0, 26];
                    if (InvestMentData[0, 27] != "")
                        drpFinyear.SelectedValue = InvestMentData[0, 27];
                    txtbank1.Text = InvestMentData[0, 29];
                    txtbank2.Text = InvestMentData[0, 30];
                    if (InvestMentData[0, 31] != "")
                        ddlIncomeRange.SelectedValue = InvestMentData[0, 31];
                    txtNetworth.Text = InvestMentData[0, 32];
                    if (InvestMentData[0, 28].ToString() != "")
                    {
                        dtEffect.Value = Convert.ToDateTime(InvestMentData[0, 28]);
                    }
                    else
                    {
                        //dtEffect.Value = Convert.ToDateTime(oDBEngine.GetDate());
                        dtEffect.Value = oDBEngine.GetDate();
                    }
                }
            }
            else
            {
                ddlIncomeRange.SelectedIndex = 0;
                txtNetworth.Text = "";
                txtgrossannualsalary.Text = "";
                txtannualTrunover.Text = "";
                txtGrossProfit.Text = "";
                txtPMExpenses.Text = "";
                txtPMSaving.Text = "";
                txtequity.Text = "";
                txtMutalFund.Text = "";
                txtBankFD.Text = "";
                txtDebtsInstruments.Text = "";
                txtNSS.Text = "";
                txtLifeInsurance.Text = "";
                txtHealthInsurance.Text = "";
                txtRealEstate.Text = "";
                txtPreciousMetals.Text = "";
                txtOthers.Text = "";
                chkHasFundInvestment.Checked = false;
                txtAvailableFund.Text = "";
                txtInvestmentHorizon.Text = "";
                chkPortFoilio.Checked = false;
                TxtPortFoilioAmount.Text = "";
                chkhouse.Checked = false;
                chkVehicle.Checked = false;

                txtgrossannualsalary2.Text = "";
                txtannualTrunover2.Text = "";
                txtGrossProfit2.Text = "";
                txtPMExpenses2.Text = "";
                txtPMSaving2.Text = "";
                drpFinyear.SelectedIndex = 0;
                //dtEffect.Value = Convert.ToDateTime(oDBEngine.GetDate());
                dtEffect.Value = oDBEngine.GetDate();
                txtbank1.Text = "";
                txtbank2.Text = "";


            }
        }

        public void DisabledTabPage()
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("General");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("DP Details");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Family Members");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Registration");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Group Member");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Deposit");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Remarks");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Education");
            page.Enabled = false;
        }



        protected void BankDetailsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {

            TextBox bankname = (TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname");
            string bankid = ((HiddenField)BankDetailsGrid.FindEditFormTemplateControl("txtbankname_hidden")).Value;

            //ASPxComboBox SerchBank = (ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpSearchBank");
            //string searchVal = SerchBank.SelectedItem.Value.ToString();
            bankname.Attributes.Add("onkeyup", "CallList(this,'bankdetails',event)");
            if (refreshTime != string.Empty)
            {
                BankDetailsGrid.DataBind();
                refreshTime = "";
                //  ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage01", "RefreshPage();", true);
                /// Response.Redirect(Request.RawUrl);
                //  UpdateBankDetails(id);

            }

        }

        protected void BankDetailsGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //   string sortby = e.NewValues["drpSearchBank"].ToString();
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
                    if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                    {
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                }
                if (BankDetail[3].ToString() == "1")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_micrno='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                    {
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                    if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                    {
                        condition += " bnk_branchname='" + BankDetail[2].ToString() + "'";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                }
                if (BankDetail[3].ToString() == "2")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_branchname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                    {
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                }
                if (condition != "")
                {
                    // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                  //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                    string[,] DT = oDBEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
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
            // bool b = BankDetailsGrid.IsEditing;
            //foreach (GridViewColumn column in BankDetailsGrid.Columns)
            //{
            //    GridViewDataColumn dataColumn = column as GridViewDataColumn;
            //    if (dataColumn == null) continue;
            //    if (e.NewValues[dataColumn.FieldName] == null)
            //    {
            //        e.Errors[dataColumn] = "Value can't be null.";
            //    }

            //}
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
                string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_accountCategory", " cbd_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);



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
                existingId = Convert.ToInt32(KeyVal);
                string Category = e.NewValues["Category"].ToString();
                if (Category != "Secondary")
                {
                    string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_id", " cbd_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);
                    if (Category1[0, 0] != "n")
                    {
                        if (KeyVal != Category1[0, 0])
                        {
                            ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblCategoryErrorMsg")).Visible = true;
                            e.RowError = "Default Category Already Exists!";
                            return;
                        }
                        else
                        {
                            ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblCategoryErrorMsg")).Visible = false;
                        }
                    }
                }
            }


            string BankCode = "";
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
                    if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                    {
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                }
                if (BankDetail[3].ToString() == "1")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_micrno='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                    {
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                    if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                    {
                        condition += " bnk_branchname='" + BankDetail[2].ToString() + "'";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                }
                if (BankDetail[3].ToString() == "2")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_branchname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                    {
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                    }
                    else
                    {
                        condition += " (bnk_micrno is null or bnk_micrno='')";
                    }
                }
                if (condition != "")
                {
                    // oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    string[,] DT = oDBEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                    if (DT[0, 0] != "n")
                    {
                        BankCode = DT[0, 0].ToString();
                    }

                }

            }
            string AccNo = e.NewValues["AccountNumber"].ToString();
            DataTable dtCnt = oDBEngine.GetDataTable("tbl_trans_contactBankDetails", "(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+ isnull(ltrim(rtrim(cnt_middlename)),'') +' '+isnull(ltrim(rtrim(cnt_lastname)),'') ++'['+ rtrim(ltrim(cnt_ucc)) +']'   from tbl_master_contact where cnt_internalid=cbd_cntid) as [ClientName]  ", "  cbd_bankCode='" + BankCode + "' AND  cbd_accountNumber= '" + AccNo + "' and cbd_cntid !='" + Session["KeyVal_InternalID_New"].ToString() + "' ");
            if (dtCnt.Rows.Count > 0)
            {
                DPID = "";
                for (int i = 0; i < dtCnt.Rows.Count; i++)
                {
                    DPID += "[" + dtCnt.Rows[i]["ClientName"].ToString() + "]";
                }

            }
            string err = e.RowError;
            bool isNew = e.IsNewRow;
            /* if (isNew == false && err == string.Empty)
             {
                 string s = Convert.ToString(e.Keys["Id"]);
                 // InsertBankDetails();
                 id = s;
               // UpdateBankDetails(s);
             }
             else if (isNew == true && err == string.Empty)
             {
               //  InsertBankDetails();
             } */

        }
        private bool validationValues()
        {
            try
            {
                string ids = ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblId")).Text;
                bool b = BankDetailsGrid.IsEditing;
                //  BankDetailsGrid.JSProperties["ed"] = null;

                // GenericStoreProcedure objGenericStoreProcedure = new GenericStoreProcedure();
                BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
                string acNo = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAccountNo")).Text;
                string acName = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAnccountName")).Text;
                string acType = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpAccountType")).Value.ToString();
                string category = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpCategory")).Value.ToString();
                string IsPOA = ((DropDownList)BankDetailsGrid.FindEditFormTemplateControl("ddlPOA")).SelectedValue;
                if (((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("comboPOA")).Value != null)
                {
                    IsPOA = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("comboPOA")).Value.ToString();
                }
                else
                {
                    IsPOA = "0";
                }
                string POA = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtPOA")).Text;
                string Bank = "";
                if (((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Contains("~") == true && ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Split('~').Length > 2)
                {
                    Bank = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Split('~')[2];
                }
                string insuId = Convert.ToString(Session["KeyVal_InternalID_New"]);

                if (category == null)
                {
                    // ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblCategoryErrorMsg")).Text = "Please select Category.";
                    // e.RowError = "Please select Category.";
                    return false;
                }
                if (acType == null)
                {
                    // ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblAcTypeErrorMsg")).Text = "Please select Account Type.";
                    //  e.RowError = "Please select Account Type.";
                    return false;
                }
                if (((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text == "")
                {
                    // e.RowError = "Please select Bank Name.";
                    // ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblCategoryErrorMsg")).Text = "Please select Account Type.";
                    return false;
                }
                if (BankDetailsGrid.IsNewRowEditing)
                {


                    string Category = category;
                    string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_accountCategory", " cbd_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);



                    if (Category1[0, 0] != "n")
                    {
                        if (Category1[0, 0] == "Default")
                        {
                            ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblCategoryErrorMsg")).Visible = true;
                            // e.RowError = "Default Category Already Exists!";
                            return false;
                        }
                        else
                        {
                            ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblCategoryErrorMsg")).Visible = false;
                        }
                    }



                }
                else
                {
                    string KeyVal = ids;
                    existingId = Convert.ToInt32(KeyVal);
                    string Category = category;
                    if (Category != "Secondary")
                    {
                        string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_id", " cbd_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);
                        if (Category1[0, 0] != "n")
                        {
                            if (KeyVal != Category1[0, 0])
                            {
                                //  e.RowError = "Default Category Already Exists!";
                                return false;
                            }
                        }
                    }
                }


                string BankCode = "";
                //  string[] BankDetail = e.NewValues["BankName1"].ToString().Split('~');
                string[] BankDetail = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Split('~');
                string condition = "";
                if (BankDetail[0].ToString() != "")
                {
                    if (BankDetail[3].ToString() == "0")
                    {
                        if (BankDetail[0] != "")
                            condition = " bnk_bankname='" + BankDetail[0].ToString() + "' and ";
                        if (BankDetail[1] != "")
                            condition += " bnk_branchname='" + BankDetail[1].ToString() + "' and ";
                        if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                        {
                            condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                        }
                        else
                        {
                            condition += " (bnk_micrno is null or bnk_micrno='')";
                        }
                    }
                    if (BankDetail[3].ToString() == "1")
                    {
                        if (BankDetail[0] != "")
                            condition = " bnk_micrno='" + BankDetail[0].ToString() + "' and ";
                        if (BankDetail[1] != "")
                        {
                            condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                        }
                        else
                        {
                            condition += " (bnk_micrno is null or bnk_micrno='')";
                        }
                        if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                        {
                            condition += " bnk_branchname='" + BankDetail[2].ToString() + "'";
                        }
                        else
                        {
                            condition += " (bnk_micrno is null or bnk_micrno='')";
                        }
                    }
                    if (BankDetail[3].ToString() == "2")
                    {
                        if (BankDetail[0] != "")
                            condition = " bnk_branchname='" + BankDetail[0].ToString() + "' and ";
                        if (BankDetail[1] != "")
                            condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                        if (BankDetail[2].ToString().Trim() != "" && BankDetail[2].ToString().Trim().Length > 0)
                        {
                            condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                        }
                        else
                        {
                            condition += " (bnk_micrno is null or bnk_micrno='')";
                        }
                    }
                    if (condition != "")
                    {
                        // oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        string[,] DT = oDBEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                        if (DT[0, 0] != "n")
                        {
                            BankCode = DT[0, 0].ToString();
                        }

                    }

                }
                string AccNo = acNo;
                DataTable dtCnt = oDBEngine.GetDataTable("tbl_trans_contactBankDetails", "(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+ isnull(ltrim(rtrim(cnt_middlename)),'') +' '+isnull(ltrim(rtrim(cnt_lastname)),'') ++'['+ rtrim(ltrim(cnt_ucc)) +']'   from tbl_master_contact where cnt_internalid=cbd_cntid) as [ClientName]  ", "  cbd_bankCode='" + BankCode + "' AND  cbd_accountNumber= '" + AccNo + "' and cbd_cntid !='" + Session["KeyVal_InternalID_New"].ToString() + "' ");
                if (dtCnt.Rows.Count > 0)
                {
                    DPID = "";
                    for (int i = 0; i < dtCnt.Rows.Count; i++)
                    {
                        DPID += "[" + dtCnt.Rows[i]["ClientName"].ToString() + "]";
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
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
                    //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                    string[,] DT = oDBEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
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


        protected void BankDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string tranid = e.Parameters.ToString();
            if (tranid == "updateExtra")
            {
                // UpdateExtraFields();
            }
            if (tranid.Length != 0)
            {

                string[] mainid = tranid.Split('~');
                if (mainid[0].ToString() == "Delete")
                {
                    DataTable dtCB = oDBEngine.GetDataTable("Trans_CashBankDetail", " * ", " cashbankdetail_clientbankid='" + mainid[1].ToString() + "'");
                    if (dtCB.Rows.Count > 0)
                    {
                        bankid = "N";
                        BankDetailsGrid.DataBind();
                    }
                    else
                    {
                        oDBEngine.DeleteValue("tbl_trans_contactbankdetails", "cbd_id ='" + mainid[1].ToString() + "'");
                        BankDetailsGrid.DataBind();

                        string strWhrereClause = "cbd_cntId=" + "'" + Session["KeyVal_InternalID"].ToString() + "' AND LogModifyUser=-1";
                        string strSetClause = "LogModifyUser=" + Convert.ToString(HttpContext.Current.Session["userid"]);
                        //  GenericMethod objGM = new GenericMethod();
                        BusinessLogicLayer.GenericMethod objGM = new BusinessLogicLayer.GenericMethod();
                        // int i = objGM.Update_Table("tbl_trans_contactbankdetails_Log", strSetClause, strWhrereClause);
                    }


                }
                else if (mainid[0].ToString() == "GridBind")
                {
                    BankDetailsGrid.DataBind();

                }


            }
            if (e.Parameters == "DeleteCurrentID")
            {
                oDBEngine.DeleteValue("tbl_trans_contactBankDetails", "cbd_id in (select top 1 cbd_id from tbl_trans_contactBankDetails where cbd_cntID='" + Session["KeyVal_InternalID"].ToString() + "' order by CreateDate desc)");
                BankDetailsGrid.DataBind();

            }



        }

        protected void BankDetailsGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = bankid;
            e.Properties["cpWidth"] = DPID;

        }

        protected void BankDetailsGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            string strWhrereClause = "cbd_cntId=" + "'" + Session["KeyVal_InternalID"].ToString() + "' AND LogModifyUser=-1";
            string strSetClause = "LogModifyUser=" + Convert.ToString(HttpContext.Current.Session["userid"]);
            //  GenericMethod objGM = new GenericMethod();
            BusinessLogicLayer.GenericMethod objGM = new BusinessLogicLayer.GenericMethod();
            int i = objGM.Update_Table("tbl_trans_contactBankDetails", strSetClause, strWhrereClause);
        }




        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_trans_contactBankDetails", " cbd_Status,cbd_StatucChangeDate,cbd_StatusChangeReason", " cbd_id ='" + data[1].ToString() + "'");
                if (dtAdd.Rows[0]["cbd_Status"].ToString() != "")
                {
                    cmbStatus.SelectedValue = dtAdd.Rows[0]["cbd_Status"].ToString();
                }
                else
                {
                    cmbStatus.SelectedValue = "N";
                }
                if (dtAdd.Rows[0]["cbd_StatucChangeDate"].ToString() != "")
                {
                    StDate.Value = Convert.ToDateTime(dtAdd.Rows[0]["cbd_StatucChangeDate"].ToString());
                }
                else
                {
                    //StDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
                    StDate.Value = oDBEngine.GetDate();
                }
                txtReason.Text = dtAdd.Rows[0]["cbd_StatusChangeReason"].ToString();

            }
            else if (data[0] == "SaveOld")
            {
                int i = oDBEngine.SetFieldValue("tbl_trans_contactBankDetails", " cbd_Status='" + cmbStatus.SelectedItem.Value + "'  ,cbd_StatucChangeDate='" + StDate.Value + "',  cbd_StatusChangeReason='" + txtReason.Text + "'  ", " cbd_id ='" + data[1].ToString() + "'");
                if (i == 1)
                {
                    Stat = "Y";
                }
            }


        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }

        protected void cmbFinYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvestment();
        }

        protected void drpFinyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFin = oDBEngine.GetDataTable("master_finyear ", "finyear_startdate ", " finyear_code= '" + drpFinyear.SelectedItem.Value + "'");
            dtEffect.Value = Convert.ToDateTime(dtFin.Rows[0][0].ToString());
        }

        protected void BindFinYear()
        {
            cmbFinYear.Items.Clear();
            DataTable DtMain = oDBEngine.GetDataTable("tbl_master_contactFinance ", "cfc_id,isnull(cfc_finyear,'')+' '+ isnull(convert(varchar,cfc_effectDate ,106),'')  as FinYear  ", " cfc_cntid ='" + BankDetailsID + "'", " cfc_effectdate desc ");
            if (DtMain.Rows.Count > 0)
            {
                for (int j = 0; j < DtMain.Rows.Count; j++)
                {
                    cmbFinYear.Items.Add(new ListItem(DtMain.Rows[j][1].ToString(), DtMain.Rows[j][0].ToString()));
                }
                cmbFinYear.Items.Add(new ListItem("New", "0"));
                //cmbFinYear.Items.FindByValue("0").Selected = true;


            }
            else
            {

                cmbFinYear.Items.Add(new ListItem("New", "0"));
            }


        }
        protected void BankDetailsGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }
        protected void BankDetailsGrid_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblCategoryErrorMsg")).Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "OldState", "OldState();", true);
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (validationValues())
            {
                string ids = ((Label)BankDetailsGrid.FindEditFormTemplateControl("lblId")).Text;
                if (ids == "")
                {
                    InsertBankDetails();
                }
                else
                {
                    UpdateBankDetails(ids);
                }
                //  ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage11", "RefreshPage();", true);
            }

        }
        public void UpdateExtraFields(int i)
        {
            if (i == 1)
            {

                // if (hdnPOA.Value != string.Empty)
                //  {
             //   SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"].ToString());

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                try
                {

                    string strpoaName = hdnPOA.Value;
                    string strpoa = hdnIsPOA.Value;

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "UPDATE tbl_trans_contactBankDetails SET cbd_POA=@ISPOA, cbd_POAName=@POAName WHERE cbd_id=(SELECT MAX(cbd_id) as id from tbl_trans_contactBankDetails)";
                    cmd.Parameters.AddWithValue("@POAName", strpoaName);
                    cmd.Parameters.AddWithValue("@ISPOA", strpoa);
                    cmd.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();


                }
                catch (Exception ex)
                { }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }

                // }
            }
            else
            {
                // if (hdnPOA.Value != string.Empty)
                // {

             //   SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"].ToString());

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                try
                {
                    string strpoaName = hdnPOA.Value;
                    string strpoa = hdnIsPOA.Value;

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "UPDATE tbl_trans_contactBankDetails SET cbd_POA=@ISPOA, cbd_POAName=@POAName WHERE cbd_id=" + existingId;
                    cmd.Parameters.AddWithValue("@POAName", strpoaName);
                    cmd.Parameters.AddWithValue("@ISPOA", strpoa);
                    cmd.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();


                }
                catch (Exception ex)
                { }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }

                //  }
            }
            //  ScriptManager.RegisterStartupScript(this, GetType(), "setInitialValues122", "setInitialValues();", true);
        }
        protected void BankDetails_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {

            // string acNo = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAccountNo")).Text;
            // int i =Convert.ToInt32(e.Command.Parameters["@LastId"].Value);
            UpdateExtraFields(1);
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateEdits11", "UpdateEdits();", true);
            // object  s = e.Command.Parameters["@LastId"].Value;

        }
        protected void BankDetails_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {

            // string acNo = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAccountNo")).Text;


            // e.Command.Parameters["@LastId"].Direction = ParameterDirection.Output; 

        }
        private void InsertBankDetails()
        {
            bool b = BankDetailsGrid.IsEditing;

            // BankDetailsGrid.KeyFieldName["Id"]
            //objGenericStoreProcedure = new GenericStoreProcedure();
            BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            string acNo = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAccountNo")).Text;
            string acName = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAnccountName")).Text;
            string acType = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpAccountType")).Value.ToString();
            string category = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpCategory")).Value.ToString();
            string IsPOA = ((DropDownList)BankDetailsGrid.FindEditFormTemplateControl("ddlPOA")).SelectedValue;
            if (((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("comboPOA")).Value != null)
            {
                IsPOA = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("comboPOA")).Value.ToString();
            }
            else
            {
                IsPOA = "0";
            }
            string POA = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtPOA")).Text;
            string Bank = "";
            if (((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpSearchBank")).Value.ToString() == "bnk_Micrno")
            {
                Bank = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Split('~')[0];
            }
            else
            {
                Bank = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Split('~')[2];
            }
            string insuId = Convert.ToString(Session["KeyVal_InternalID_New"]);
            string CreateUser = Convert.ToString(Session["userid"]);
            string[] strSpParam = new string[9];
            strSpParam[0] = "Category|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + category + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "insuId|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + insuId + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "BankName1|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Bank + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "IsPOA|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + IsPOA + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "AccountNumber|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + acNo + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "AccountType|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + acType + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[6] = "AccountName|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + acName + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[7] = "CreateUser|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + CreateUser + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[8] = "POA|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + POA + "|" + GenericStoreProcedure.ParamType.ExParam;
            DataTable dt = new DataTable();
            dt = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "BankDetailsInsert");
            ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage11", "RefreshPage();", true);
            isopen = true;

            iCount = 1;

        }
        private void UpdateBankDetails(string id)
        {

            bool b = BankDetailsGrid.IsEditing;
            //  BankDetailsGrid.JSProperties["ed"] = null;

            //objGenericStoreProcedure = new GenericStoreProcedure();
            BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();

            string acNo = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAccountNo")).Text;
            string acName = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtAnccountName")).Text;
            string acType = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpAccountType")).Value.ToString();
            string category = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpCategory")).Value.ToString();
            string IsPOA = ((DropDownList)BankDetailsGrid.FindEditFormTemplateControl("ddlPOA")).SelectedValue;
            if (((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("comboPOA")).Value != null)
            {
                IsPOA = ((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("comboPOA")).Value.ToString();
            }
            else
            {
                IsPOA = "0";
            }
            string POA = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtPOA")).Text;
            string Bank = "";
            if (((ASPxComboBox)BankDetailsGrid.FindEditFormTemplateControl("drpSearchBank")).Value.ToString() == "bnk_Micrno")
            {
                Bank = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Split('~')[0];
            }
            else
            {
                Bank = ((TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname")).Text.Split('~')[2];
            }
            string insuId = Convert.ToString(Session["KeyVal_InternalID_New"]);
            string CreateUser = Convert.ToString(Session["userid"]);
            string[] strSpParam = new string[9];
            strSpParam[0] = "Category|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + category + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "Id|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + id + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "BankName1|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Bank + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "IsPOA|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + IsPOA + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "AccountNumber|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + acNo + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "AccountType|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + acType + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[6] = "AccountName|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + acName + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[7] = "CreateUser|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + CreateUser + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[8] = "POA|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + POA + "|" + GenericStoreProcedure.ParamType.ExParam;
            DataTable dt = new DataTable();
            dt = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "BankDetailsUpdate");
            ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage11", "RefreshPage();", true);
            //  BankDetailsGrid.DataBind();


            // BankDetailsGrid.JSProperties["ed"] = "y";

            isopen = true;
            // BindGrid();
            //  btnUpdate_Click(this, EventArgs.Empty);
            // refreshTime = "s";


            //  btnForce_Click(sender, new EventArgs());

        }
        protected void BankDetailsGrid_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            //  InsertBankDetails();
        }
        protected void BankDetails_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            // UpdateExtraFields(2);
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateEdits11", "UpdateEdits();", true);
        }
        protected void BankDetailsGrid_RowUpdated1(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            //BankDetailsGrid.DataSource = BankDetails;
            //BankDetailsGrid.DataBind();
            // BankDetailsGrid.CancelEdit();

            string s = Convert.ToString(e.Keys["Id"]);
            //// InsertBankDetails();
            //  UpdateBankDetails(s);
            //  ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPagedo01", "__doPostBack();", true);
        }
        protected void BankDetailsGrid_PreRender(object sender, EventArgs e)
        {

        }
        protected void BankDetailsGrid_Unload(object sender, EventArgs e)
        {
            //  ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage01", "RefreshPage();", true);
            // BankDetailsGrid.DataSource = BankDetails();
            //BankDetailsGrid.DataBind();

        }
        protected void btnForce_Click(object sender, EventArgs e)
        {

            //BankDetailsGrid_RowValidating(this, new DevExpress.Web.Data.ASPxDataValidationEventArgs(true));
            BankDetailsGrid.DataBind();
            //  ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage01", "RefreshPage();", true);
        }
    }
}