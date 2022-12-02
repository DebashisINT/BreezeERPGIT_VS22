using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmSalesCommodity1 : System.Web.UI.Page
    {
        clsDropDownList cls = new clsDropDownList();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DateTime new_startdate = new DateTime();
        DateTime new_starttime1 = new DateTime();
        string new_starttime;
        string new_enddate;
        string new_endtime;
        protected void Page_Load(object sender, EventArgs e)
        {
            drpSalesStage.Attributes.Add("onchange", "functionSalesStage('" + drpSalesStage.ID + "')");
            rdrCall.Attributes.Add("onclick", "funChangeNext(this)");
            rdrVisit.Attributes.Add("onclick", "funChangeNext(this)");
            if (!IsPostBack)
            {
                string[,] conid = oDBEngine.GetFieldValue("tbl_trans_sales", "sls_contactlead_id", " sls_id='" + Request.QueryString["id"].ToString() + "'", 1);
                if (conid[0, 0] != "n")
                {
                    Session["InternalId"] = conid[0, 0];
                }

                ASPxChqDate1.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxChqDate2.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxNextVisitDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxvisitDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxNextVisitDate.EditFormatString = objConverter.GetDateFormat("DateTime");
                ASPxvisitDate.EditFormatString = objConverter.GetDateFormat("DateTime");
                ASPxChqDate1.EditFormatString = objConverter.GetDateFormat("DateTime");
                ASPxChqDate2.EditFormatString = objConverter.GetDateFormat("DateTime");
                fillBankDropdownlist();
                fillSalesStage();
                fillValues();
                showCallsDetails();
            }
            Session["SalesID"] = Request.QueryString["id"].ToString();
            ViewState["ActivityId"] = "1";
            Session["SalesID"] = Request.QueryString["id"].ToString();
            string[,] Id1 = oDBEngine.GetFieldValue("tbl_trans_SExpenditure", "expnd_empId", " expnd_empId='" + Session["SalesID"].ToString() + "'", 1);
            if (Id1[0, 0] == "n")
            {
                oDBEngine.InsurtFieldValue("tbl_trans_SExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Session["SalesID"].ToString() + "','1','1','1','1'");
                oDBEngine.InsurtFieldValue("tbl_trans_SExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Trav','" + Session["SalesID"].ToString() + "','1','1','1','1'");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JavaScript", "<script language='JavaScript'>height();</script>");

        }
        public void fillBankDropdownlist()
        {
            string[,] bank = oDBEngine.GetFieldValue("tbl_trans_Sales INNER JOIN tbl_trans_contactBankDetails ON tbl_trans_Sales.sls_contactlead_id = tbl_trans_contactBankDetails.cbd_cntId INNER JOIN tbl_master_Bank ON tbl_trans_contactBankDetails.cbd_bankCode = tbl_master_Bank.bnk_id", "tbl_master_Bank.bnk_id AS ID, tbl_master_Bank.bnk_bankName AS BankName", " tbl_trans_Sales.sls_id='" + Request.QueryString["id"].ToString() + "'", 2);
            if (bank[0, 0] != "n")
            {
                cls.AddDataToDropDownList(bank, drpBanks);
                cls.AddDataToDropDownList(bank, drpBanksMargin);
            }
            string[,] NextVisitPlace = oDBEngine.GetFieldValue("tbl_master_address", "add_id,ISNULL(add_address1, '') + ' ' + ISNULL(add_address2, '') + ' ' + ISNULL(add_address3, '') AS ADDRESS", " add_cntId ='" + Session["InternalId"].ToString() + "'", 2);
            if (NextVisitPlace[0, 0] != "n")
            {
                cls.AddDataToDropDownList(NextVisitPlace, DrpNextVisitPlace);
                cls.AddDataToDropDownList(NextVisitPlace, DrpVisitPlace);
            }
        }
        public void fillSalesStage()
        {
            string[,] sales = oDBEngine.GetFieldValue("tbl_master_SalesStatus", "sls_id,sls_status", " sls_id<>4", 2);
            if (sales[0, 0] != "n")
            {
                cls.AddDataToDropDownList(sales, drpSalesStage);
            }
        }
        public void fillValues()
        {
            string ID = Session["InternalId"].ToString();
            string ID2 = "";
            ID2 = ID.Substring(0, 2);
            DataTable dt = new DataTable();
            if (ID2 == "LD")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_salesDetails", "*,(SELECT cnt_firstName FROM tbl_master_Contact WHERE cnt_internalId = sad_cntId) AS Name", " sad_salesId='" + Request.QueryString["id"].ToString() + "' order by sad_id desc");
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_trans_salesDetails", "*,(SELECT cnt_firstName FROM tbl_master_contact WHERE cnt_internalId = sad_cntId) AS Name", " sad_salesId='" + Request.QueryString["id"].ToString() + "' order by sad_id desc");
            }
            if (dt.Rows.Count != 0)
            {
                txtName.Text = dt.Rows[0]["Name"].ToString();
                drpSalesStage.SelectedValue = Convert.ToString(drpSalesStage.Items.FindByValue(dt.Rows[0]["sad_SaleStage"].ToString()).Value);
                txtNote.Text = dt.Rows[0]["sad_Notes"].ToString();
                if (Convert.ToBoolean(dt.Rows[0]["sad_NSEEquity"].ToString()) != false)
                {
                    chkNSEEquity.Checked = true;
                }
                else
                {
                    chkNSEEquity.Checked = false;
                }
                if (Convert.ToBoolean(dt.Rows[0]["sad_RegNSEDerivative"].ToString()) != false)
                {
                    chkNSEDerivatives.Checked = true;
                }
                else
                {
                    chkNSEDerivatives.Checked = false;
                }
                if (Convert.ToBoolean(dt.Rows[0]["sad_RegBSEEquity"].ToString()) != false)
                {
                    chkBSEEquity.Checked = true;
                }
                else
                {
                    chkBSEEquity.Checked = false;
                }
                if (Convert.ToBoolean(dt.Rows[0]["sad_RegBSEDerivative"].ToString()) != false)
                {
                    chkBSEDerivatives.Checked = true;
                }
                else
                {
                    chkBSEDerivatives.Checked = false;
                }
                if (Convert.ToBoolean(dt.Rows[0]["sad_RegNCDEX"].ToString()) != false)
                {
                    chkNCDEX.Checked = true;
                }
                else
                {
                    chkNCDEX.Checked = false;
                }
                if (Convert.ToBoolean(dt.Rows[0]["sad_RegMCX"].ToString()) != false)
                {
                    chkMCX.Checked = true;
                }
                else
                {
                    chkMCX.Checked = false;
                }
                if (Convert.ToBoolean(dt.Rows[0]["sad_RegCDSL"].ToString()) != false)
                {
                    chkCDSL.Checked = true;
                }
                else
                {
                    chkCDSL.Checked = false;
                }
                if (Convert.ToBoolean(dt.Rows[0]["sad_RegNSDL"].ToString()) != false)
                {
                    chkNSDL.Checked = true;
                }
                else
                {
                    chkNSDL.Checked = false;
                }
                ASPxNextVisitDate.Value = Convert.ToDateTime(dt.Rows[0]["sad_nextvisitdate"].ToString());
                ASPxvisitDate.Value = Convert.ToDateTime(dt.Rows[0]["sad_visitdate"].ToString());
                txtChequeNumber.Text = dt.Rows[0]["sad_ChequeNumber"].ToString();
                ASPxChqDate1.Value = Convert.ToDateTime(dt.Rows[0]["sad_ChequeDate"].ToString());
                txtChequeAmount.Text = dt.Rows[0]["sad_ChequeAmount"].ToString();
                try
                {
                    drpBanks.SelectedValue = Convert.ToString(drpBanks.Items.FindByValue(dt.Rows[0]["sad_Bank"].ToString()).Value);
                }
                catch
                {
                }
                txtChequeNumberMargin.Text = dt.Rows[0]["sad_MarginCheckNumber"].ToString();
                ASPxChqDate2.Value = Convert.ToDateTime(dt.Rows[0]["sad_MarginChequeDate"].ToString());
                txtChequeAmountMargin.Text = dt.Rows[0]["sad_MarginChequeAmount"].ToString();
                try
                {
                    drpBanksMargin.SelectedValue = Convert.ToString(drpBanksMargin.Items.FindByValue(dt.Rows[0]["sad_MarginBank"].ToString()).Value);
                }
                catch
                {
                }
                try
                {
                    drpProductApplicationForm.SelectedValue = Convert.ToString(drpProductApplicationForm.Items.FindByText(dt.Rows[0]["sad_ProductApplicationform"].ToString()).Value);
                    drpPhotoIdProof.SelectedValue = Convert.ToString(drpPhotoIdProof.Items.FindByText(dt.Rows[0]["sad_PhotoIDProof"].ToString()).Value);
                    drpPhotoIdProofDocument.SelectedValue = Convert.ToString(drpPhotoIdProofDocument.Items.FindByText(dt.Rows[0]["sad_PhotoIDProofDocument"].ToString()).Value);
                    drpAddressProof.SelectedValue = Convert.ToString(drpAddressProof.Items.FindByText(dt.Rows[0]["sad_AddressProof"].ToString()).Value);
                    drpAddressProofDocument.SelectedValue = Convert.ToString(drpAddressProofDocument.Items.FindByText(dt.Rows[0]["sad_AddressProofDocument"].ToString()).Value);
                    drpAgeProof.SelectedValue = Convert.ToString(drpAgeProof.Items.FindByText(dt.Rows[0]["sad_AgeProof"].ToString()).Value);
                    drpSignatureProof.SelectedValue = Convert.ToString(drpSignatureProof.Items.FindByText(dt.Rows[0]["sad_SignatureProof"].ToString()).Value);
                    drpKYCDocument.SelectedValue = Convert.ToString(drpKYCDocument.Items.FindByText(dt.Rows[0]["sad_KYCDocument"].ToString()).Value);
                    drpTripartiteAgreement.SelectedValue = Convert.ToString(drpTripartiteAgreement.Items.FindByText(dt.Rows[0]["sad_TripartiteAgreement"].ToString()).Value);
                    drpPOAAgreement.SelectedValue = Convert.ToString(drpPOAAgreement.Items.FindByText(dt.Rows[0]["sad_POAAgreement"].ToString()).Value);
                    drpMedicalReports.SelectedValue = Convert.ToString(drpMedicalReports.Items.FindByText(dt.Rows[0]["sad_MedicalReports"].ToString()).Value);
                    drpBankAccountProof.SelectedValue = Convert.ToString(drpBankAccountProof.Items.FindByText(dt.Rows[0]["sad_BankAccountProof"].ToString()).Value);
                    drpBankAccountProofDocument.SelectedValue = Convert.ToString(drpBankAccountProofDocument.Items.FindByText(dt.Rows[0]["sad_BankAccountProofDocument"].ToString()).Value);
                    drpBankVerificationLetter.SelectedValue = Convert.ToString(drpBankVerificationLetter.Items.FindByText(dt.Rows[0]["sad_BankVerificationLetter"].ToString()).Value);
                    drpLastYearIT.SelectedValue = Convert.ToString(drpLastYearIT.Items.FindByText(dt.Rows[0]["sad_LastYearIt"].ToString()).Value);
                    drpVisitPurpose.SelectedValue = Convert.ToString(drpVisitPurpose.Items.FindByText(dt.Rows[0]["sad_visitPurpose"].ToString()).Value);
                    drpNextVisitPurpose.SelectedValue = Convert.ToString(drpNextVisitPurpose.Items.FindByText(dt.Rows[0]["sad_nextvisitpurpose"].ToString()).Value);
                    if (dt.Rows[0]["sad_nextactivitytype"].ToString() == "1")
                    {
                        rdrCall.Checked = true;
                        rdrVisit.Checked = false;
                        lblNextVisitDate.Text = "Next Call Date ";
                        lblNextVisitPurpose.Text = "Next Call Purpose";
                        Page.ClientScript.RegisterStartupScript(GetType(), "Visit/Call", "<script languge='JavaScript'>setvisibleforcall();</script>");
                    }
                    else
                    {
                        rdrVisit.Checked = true;
                        rdrCall.Checked = false;
                        lblNextVisitDate.Text = "Next Visit Date";
                        lblNextVisitPurpose.Text = "Next Visit Purpose";
                        Page.ClientScript.RegisterStartupScript(GetType(), "Visit/Call", "<script languge='JavaScript'>setvisibleforvisit();</script>");
                    }
                }
                catch
                {
                }
            }
            DataTable dt_info = new DataTable();
            if (ID2 == "LD")
            {
                dt_info = oDBEngine.GetDataTable("tbl_trans_Sales INNER JOIN tbl_master_Contact ON tbl_trans_Sales.sls_contactlead_id = tbl_master_Contact.cnt_internalId  INNER JOIN tbl_master_branch ON tbl_master_Contact.cnt_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_legalStatus ON tbl_master_contact.cnt_legalStatus = tbl_master_legalStatus.lgl_id", "tbl_master_contact.cnt_firstName + ' ' + isnull(tbl_master_contact.cnt_middlename,' ') + ' ' + isnull(tbl_master_contact.cnt_lastname,' ') AS Name, tbl_master_branch.branch_description, tbl_master_legalStatus.lgl_legalStatus, case isnull(cnt_referedby,'0') when '0' then 'N/A' else (select tbl_master_contact.cnt_firstname from tbl_master_contact where tbl_master_contact.cnt_internalid = tbl_master_contact.cnt_referedby) end as Introducer", " tbl_trans_Sales.sls_id ='" + Request.QueryString["id"].ToString() + "'");
            }
            else
            {
                dt_info = oDBEngine.GetDataTable("tbl_trans_Sales INNER JOIN tbl_master_contact ON tbl_trans_Sales.sls_contactlead_id = tbl_master_contact.cnt_internalId  INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_legalStatus ON tbl_master_contact.cnt_legalStatus = tbl_master_legalStatus.lgl_id", "tbl_master_contact.cnt_firstName + ' ' + isnull(tbl_master_contact.cnt_middlename,' ') + ' ' + isnull(tbl_master_contact.cnt_lastname,' ') AS Name, tbl_master_branch.branch_description, tbl_master_legalStatus.lgl_legalStatus, case isnull(cnt_referedby,'0') when '0' then 'N/A' else (select c.cnt_firstname from tbl_master_contact c where c.cnt_internalid = tbl_master_contact.cnt_referedby) end as Introducer", " tbl_trans_Sales.sls_id ='" + Request.QueryString["id"].ToString() + "'");
            }
            if (dt_info.Rows.Count != 0)
            {
                txtName.Text = dt_info.Rows[0]["Name"].ToString();
                lblBranch.Text = dt_info.Rows[0]["branch_description"].ToString();
                lblLegalStatus.Text = dt_info.Rows[0]["lgl_legalStatus"].ToString();
                lblIntroducer.Text = dt_info.Rows[0]["Introducer"].ToString();
            }

        }
        public void showCallsDetails()
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_trans_sales INNER JOIN tbl_trans_Activies ON tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id ", "tbl_trans_Activies.act_id as act_id,tbl_trans_Activies.act_assignedBy as act_assignedBy,(convert(varchar(11),tbl_trans_Activies.act_createDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_createDate, 22), 3))) as act_createDate,(convert(varchar(11),tbl_trans_Activies.act_actualStartDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_Activies.act_actualStartDate, 22), 3))) as act_actualStartDate,tbl_trans_Activies.act_priority as act_priority,convert(varchar(11),tbl_trans_Activies.act_scheduledDate ,113)as act_scheduledDate,tbl_trans_Activies.act_scheduledTime as act_scheduledTime,convert(varchar(11),tbl_trans_Activies.act_expectedDate ,113)as act_expectedDate,tbl_trans_Activies.act_expectedTime as act_expectedTime,tbl_trans_Activies.act_instruction as act_instruction", " tbl_trans_Sales.sls_id ='" + Request.QueryString["id"].ToString() + "'");
            if (dt.Rows.Count != 0)
            {
                Session["newactivityid"] = dt.Rows[0]["act_id"].ToString();
                txtAlloatedBy.Text = getName(dt.Rows[0]["act_assignedBy"].ToString());
                txtDateOfAllottment.Text = dt.Rows[0]["act_createDate"].ToString();
                txtAcutalStart.Text = dt.Rows[0]["act_actualStartDate"].ToString();
                string s = dt.Rows[0]["act_priority"].ToString();
                switch (s)
                {
                    case "0":
                        txtPriority.Text = "Low";
                        break;
                    case "1":
                        txtPriority.Text = "Normal";
                        break;
                    case "2":
                        txtPriority.Text = "High";
                        break;
                    case "3":
                        txtPriority.Text = "Urgent";
                        break;
                    case "4":
                        txtPriority.Text = "Immediate";
                        break;
                }
                txtSeheduleStart.Text = dt.Rows[0]["act_scheduledDate"].ToString() + " " + dt.Rows[0]["act_scheduledTime"].ToString();
                txtTotalNumberofCalls.Text = getTotalNumberofCalls(Convert.ToInt32(dt.Rows[0]["act_id"].ToString())).ToString();
                txtSeheduleEnd.Text = dt.Rows[0]["act_expectedDate"].ToString() + " " + dt.Rows[0]["act_expectedTime"].ToString();
                lblShortNote.Text = dt.Rows[0]["act_instruction"].ToString();
                ShowCallInformation.Visible = true;
            }
            else
            {
                ShowCallInformation.Visible = false;
            }
        }
        public int getTotalNumberofCalls(int actid)
        {
            int str = 0;
            DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "count(phc_id)", " phc_activityId='" + actid + "'");
            if (dt.Rows.Count != 0)
            {
                str = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            return str;
        }
        public string getName(string id)
        {
            string name = "";
            DataTable dt = oDBEngine.GetDataTable("tbl_master_user", "user_name", " user_id='" + id + "'");
            if (dt.Rows.Count != 0)
            {
                name = dt.Rows[0][0].ToString();
            }
            else
            {
                name = "";
            }
            return name;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(checkconditionflag()))
            //{
            bool editflag = false;
            bool flag = false;
            string contactid = "";
            string branchid = "";
            string[,] conid = oDBEngine.GetFieldValue("tbl_trans_sales", "sls_contactlead_id", " sls_id='" + Request.QueryString["id"].ToString() + "'", 1);
            if (conid[0, 0] != "n")
            {
                contactid = conid[0, 0];
            }
            string[,] branid = oDBEngine.GetFieldValue("tbl_trans_sales", "sls_branch_id", " sls_id='" + Request.QueryString["id"].ToString() + "'", 1);
            if (branid[0, 0] != "n")
            {
                branchid = branid[0, 0];
            }
            string bseequity = "";
            string nseequity = "";
            string bsederivatives = "";
            string nsederivatives = "";
            string ncdex = "";
            string mcx = "";
            string cdsl = "";
            string nsdl = "";

            int acttype = 0;
            int nextacttype = 0;
            if (lblVisitDate.InnerText == "Call Date")
            {
                acttype = 1;
            }
            else
            {
                acttype = 2;
            }
            string nextvisit = ASPxNextVisitDate.Value.ToString();
            if (drpSalesStage.SelectedValue == "2")
            {
                nextvisit = "";
            }
            if (rdrCall.Checked == true)
            {
                nextacttype = 1;
            }
            else
            {
                nextacttype = 2;
            }
            string bankval = "0";
            if (drpBanks.SelectedValue == "")
            {
                bankval = "0";
            }
            else
            {
                bankval = drpBanks.SelectedValue.ToString();
            }
            string marbankvalue = "0";
            if (drpBanksMargin.SelectedValue == "")
            {
                marbankvalue = "0";
            }
            else
            {
                marbankvalue = drpBanksMargin.SelectedValue.ToString();
            }
            //string fields = "";
            //string val = "";
            //fields = "sad_cntId,sad_Branch,sad_FinancialYear,sad_SalesId,sad_NSEEquity,sad_RegNSEDate,sad_RegNSEDerivative,sad_RegNSEDerivativeDate,sad_RegBSEEquity,sad_RegBSEEquityDate,sad_RegBSEDerivative,sad_RegBSEDerivativeDate,sad_RegNCDEX,sad_RegNCDEXDate,sad_RegMCX,sad_RegMCXDate,sad_RegCDSL,sad_RegCDSLDate,sad_RegNSDL,sad_RegNSDLDate,sad_LoginDate,sad_SaleStage,sad_Notes,";
            //val = "'" + contactid + "'," + branchid + ",'" + oDBEngine.GetDate().Year.ToString() + "'," + Request.QueryString["id"].ToString() + ",'" + chkNSEEquity.Checked + "','" + nseequity + "','" + chkNSEDerivatives.Checked + "','" + nsederivatives + "','" + chkNSEEquity.Checked + "','" + bseequity + "','" + chkBSEDerivatives.Checked + "','" + bsederivatives + "','" + chkNCDEX.Checked + "','" + ncdex + "','" + chkMCX.Checked + "','" + mcx + "','" + chkCDSL.Checked + "','" + cdsl + "','" + chkNSDL.Checked + "','" + nsdl + "','" + ASPxvisitDate.Value + "'," + drpSalesStage.SelectedValue + ",'" + txtNote.Text + "','";
            //fields += "sad_ProductApplicationform, sad_PhotoIDProof, sad_PhotoIdProofDocument, sad_BankAccountProof, sad_BankAccountProofDocument, sad_AddressProof, sad_AgeProof, sad_SignatureProof, sad_KYCDocument, sad_TripartiteAgreement, sad_POAAgreement, sad_MedicalReports, sad_BankVerificationLetter, sad_LastYearIT ,sad_dateTime,sad_visitdate,sad_visitpurpose,sad_nextvisitdate,sad_nextvisitpurpose,sad_activityType,sad_nextactivityType,sad_ChequeNumber,sad_ChequeDate, sad_ChequeAmount, sad_Bank, sad_MarginCheckNumber, sad_MarginChequeDate, sad_MarginChequeAmount, sad_MarginBank,CreateDate,CreateUser";
            //val +=  drpProductApplicationForm.SelectedItem.Text.ToString() + "','" + drpPhotoIdProof.SelectedItem.Text.ToString() + "','" + drpPhotoIdProofDocument.SelectedItem.Text.ToString() + "','" + drpBankAccountProof.SelectedItem.Text.ToString() + "','" + drpBankAccountProofDocument.SelectedItem.Text.ToString() + "','" + drpAddressProof.SelectedItem.Text.ToString() + "','" + drpAddressProofDocument.SelectedItem.Text.ToString() + "','" + drpAgeProof.SelectedItem.Text.ToString() + "','" + drpSignatureProof.SelectedItem.Text.ToString() + "','" + drpKYCDocument.SelectedItem.Text.ToString() + "','" + drpTripartiteAgreement.SelectedItem.Text.ToString() + "','" + drpPOAAgreement.SelectedItem.Text.ToString() + "','" + drpMedicalReports.SelectedItem.Text.ToString() + "','" + drpBankVerificationLetter.SelectedItem.Text.ToString() + "','" + drpLastYearIT.SelectedItem.Text.ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + oDBEngine.getDateTimeFormat(ASPxvisitDate.Value.ToString()) + "','" + drpVisitPurpose.SelectedValue.ToString() + "','" + oDBEngine.getDateTimeFormat(ASPxNextVisitDate.Value.ToString()) + "','" + drpNextVisitPurpose.SelectedValue.ToString() + "'," + acttype + "," + nextacttype + ",'" + txtChequeNumber.Text + "','" + ASPxChqDate1.Value + "','" + txtChequeAmount.Text + "','" + bankval + "','" + txtChequeNumberMargin.Text + "','" + ASPxChqDate2.Value + "','" + txtChequeAmountMargin.Text + "','" + marbankvalue + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
            string chequedate = ASPxChqDate1.Value.ToString();
            string visitdate = ASPxvisitDate.Value.ToString();
            string nextvisitdate = ASPxNextVisitDate.Value.ToString();
            string chequedate1 = ASPxChqDate2.Value.ToString();
            int NoOfEffected = oDBEngine.InsurtFieldValue("tbl_trans_salesDetails", "sad_cntId,sad_Branch,sad_FinancialYear,sad_SalesId,sad_NSEEquity,sad_RegNSEDate,sad_RegNSEDerivative,sad_RegNSEDerivativeDate,sad_RegBSEEquity,sad_RegBSEEquityDate,sad_RegBSEDerivative,sad_RegBSEDerivativeDate,sad_RegNCDEX,sad_RegNCDEXDate,sad_RegMCX,sad_RegMCXDate,sad_RegCDSL,sad_RegCDSLDate,sad_RegNSDL,sad_RegNSDLDate,sad_LoginDate,sad_SaleStage,sad_Notes,sad_ProductApplicationform, sad_PhotoIDProof, sad_PhotoIdProofDocument, sad_BankAccountProof, sad_BankAccountProofDocument, sad_AddressProof, sad_AddressProofDocument, sad_AgeProof, sad_SignatureProof, sad_KYCDocument, sad_TripartiteAgreement, sad_POAAgreement, sad_MedicalReports, sad_BankVerificationLetter, sad_LastYearIT ,sad_dateTime,sad_visitdate,sad_visitpurpose,sad_nextvisitdate,sad_nextvisitpurpose,sad_nextactivityType,sad_ChequeNumber,sad_ChequeDate, sad_ChequeAmount, sad_Bank, sad_MarginCheckNumber, sad_MarginChequeDate, sad_MarginChequeAmount, sad_MarginBank,sad_nextvisitplace,sad_ActivityType,sad_visitplace,CreateDate,CreateUser", "'" + contactid + "'," + branchid + ",'" + oDBEngine.GetDate().Year.ToString() + "'," + Request.QueryString["id"].ToString() + ",'" + chkNSEEquity.Checked + "','" + nseequity + "','" + chkNSEDerivatives.Checked + "','" + nsederivatives + "','" + chkNSEEquity.Checked + "','" + bseequity + "','" + chkBSEDerivatives.Checked + "','" + bsederivatives + "','" + chkNCDEX.Checked + "','" + ncdex + "','" + chkMCX.Checked + "','" + mcx + "','" + chkCDSL.Checked + "','" + cdsl + "','" + chkNSDL.Checked + "','" + nsdl + "','" + chequedate + "'," + drpSalesStage.SelectedValue.ToString() + ",'" + txtNote.Text + "','" + drpProductApplicationForm.SelectedItem.Text.ToString() + "','" + drpPhotoIdProof.SelectedItem.Text.ToString() + "','" + drpPhotoIdProofDocument.SelectedItem.Text.ToString() + "','" + drpBankAccountProof.SelectedItem.Text.ToString() + "','" + drpBankAccountProofDocument.SelectedItem.Text.ToString() + "','" + drpAddressProof.SelectedItem.Text.ToString() + "','" + drpAddressProofDocument.SelectedItem.Text.ToString() + "','" + drpAgeProof.SelectedItem.Text.ToString() + "','" + drpSignatureProof.SelectedItem.Text.ToString() + "','" + drpKYCDocument.SelectedItem.Text.ToString() + "','" + drpTripartiteAgreement.SelectedItem.Text.ToString() + "','" + drpPOAAgreement.SelectedItem.Text.ToString() + "','" + drpMedicalReports.SelectedItem.Text.ToString() + "','" + drpBankVerificationLetter.SelectedItem.Text.ToString() + "','" + drpLastYearIT.SelectedItem.Text.ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + visitdate + "','" + drpVisitPurpose.SelectedValue.ToString() + "','" + nextvisitdate + "','" + drpNextVisitPurpose.SelectedValue.ToString() + "'," + nextacttype + ",'" + txtChequeNumber.Text + "','" + chequedate + "','" + txtChequeAmount.Text + "','" + bankval + "','" + txtChequeNumberMargin.Text + "','" + chequedate1 + "','" + txtChequeAmountMargin.Text + "','" + marbankvalue + "','" + DrpNextVisitPlace.SelectedItem.Value + "','" + ViewState["ActivityId"].ToString() + "','" + DrpVisitPlace.SelectedItem.Value + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
            DataTable dt1 = oDBEngine.GetDataTable("tbl_trans_Activies", "act_actualStartDate", " act_id='" + Session["newactivityid"].ToString() + "'");
            if (dt1.Rows.Count != 0)
            {
                oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate().ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " act_id='" + Session["newactivityid"].ToString() + "'");

            }
            if (NoOfEffected != 0)
            {
                oDBEngine.SetFieldValue("tbl_trans_sales", "sls_nextvisitdate='" + ASPxNextVisitDate.Value.ToString() + "',sls_sales_status='" + drpSalesStage.SelectedValue.ToString() + "',sls_dateTime='" + oDBEngine.GetDate().ToString() + "',sls_nextvisitplace='" + DrpNextVisitPlace.SelectedItem.Value + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " sls_id='" + Request.QueryString["id"].ToString() + "'");
            }
            if (drpSalesStage.SelectedValue.ToString() == "1" || drpSalesStage.SelectedValue.ToString() == "3")
            {
                string str_startnote = "";
                //if (drpSalesStage.SelectedValue.ToString() == "1")
                if (rdrCall.Checked == true)
                {
                    str_startnote = "Phone Call to  ";
                }
                else
                {
                    str_startnote = "Metting With  ";
                }
                try
                {
                    //new_startdate = Convert.ToDateTime(nextvisit).AddDays(-1);
                    new_startdate = Convert.ToDateTime(nextvisit);
                    new_starttime = Convert.ToDateTime(nextvisit).ToString();
                }
                catch
                {
                    new_startdate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString()).AddDays(-1);
                    new_starttime = Convert.ToDateTime(oDBEngine.GetDate()).ToString();
                }
                string rem = "";
                string[,] rem1;
                string ID = Session["InternalId"].ToString();
                string ID1 = ID.Substring(0, 2);
                if (ID1 == "LD")
                {
                    rem1 = oDBEngine.GetFieldValue("tbl_master_Contact", "ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'')", " cnt_internalid='" + Session["InternalId"].ToString() + "'", 1);
                }
                else
                {
                    rem1 = oDBEngine.GetFieldValue("tbl_master_contact", "ISNULL(cnt_firstName,'')+' '+IsNull(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'')", " cnt_internalid='" + Session["InternalId"].ToString() + "'", 1);
                }
                if (rem1[0, 0] != "n")
                {
                    rem = rem1[0, 0];
                }
                string Phno = "";
                string[,] phno1 = oDBEngine.GetFieldValue("tbl_master_phonefax", "phf_phoneNumber", " (phf_entity = 'Lead') AND (phf_type = 'mobile') AND (phf_cntId = '" + Session["InternalId"].ToString() + "')", 1);
                if (phno1[0, 0] != "n")
                {
                    Phno = phno1[0, 0];
                }

                setReminder(str_startnote + rem + " [ Phone Number :-" + Phno + " ]  AT " + new_starttime + " [" + txtNote.Text + "]");

            }
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnDiscard.Enabled = false;
            pnl.Enabled = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "JavaScript2", "<script language='JavaScript'>ActionAfterSave();</script>");
            //showData.Visible = false;
            //}
        }
        public bool checkconditionflag()
        {
            bool flag = true;
            if (drpSalesStage.SelectedValue == "2")
            {
                DataTable dt = new DataTable();
                string pancard = "";
                string[,] pancard1 = oDBEngine.GetFieldValue("tbl_master_contactRegistration", "crg_PanNumber", " crg_cntId='" + Session["SalesActivityId"].ToString() + "'", 1);
                if (pancard1[0, 0] != "n")
                {
                    pancard = pancard1[0, 0];
                }
                if (pancard == "")
                {
                    flag = false;
                    lblError.Text += "Pancard is must for open the account || ";
                }

                string introducer = "";
                string[,] introducer1 = oDBEngine.GetFieldValue("tbl_master_contact INNER JOIN tbl_master_Contact ON tbl_master_contact.cnt_internalId = tbl_master_Contact.cnt_referedBy", "tbl_master_contact.cnt_firstName", " tbl_master_Contact.cnt_internalId='" + Session["SalesActivityId"].ToString() + "'", 1);
                if (introducer1[0, 0] != "n")
                {
                    introducer = introducer1[0, 0];
                }
                if (introducer == "")
                {
                    lblError.Text += "Without Introducer account cannot open ||";
                }

                string emailid = "";
                string[,] emailid1 = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", " eml_cntid='" + Session["SalesActivityId"].ToString() + "'", 1);
                if (emailid1[0, 0] != "n")
                {
                    emailid = emailid1[0, 0];
                }
                if (emailid == "")
                {
                    flag = false;
                    lblError.Text += "Email address is must for open the account || ";
                }

                string phoneno = "";
                string[,] phoneno1 = oDBEngine.GetFieldValue("tbl_master_phonefax", "phf_phoneNumber", " phf_cntid='" + Session["SalesActivityId"].ToString() + "'", 1);
                if (phoneno1[0, 0] != "n")
                {
                    phoneno = phoneno1[0, 0];
                }
                if (phoneno == "")
                {
                    flag = false;
                    lblError.Text += "Phone number is must for open the account || ";
                }

                if (drpKYCDocument.SelectedValue != "Recevied")
                {
                    flag = false;
                    lblError.Text += "KYC Document not recevied ||";
                }
                if (drpBankAccountProof.SelectedValue != "Recevied")
                {
                    lblError.Text += "Back Account Proof not recevied";
                }
                if (drpPhotoIdProof.SelectedValue != "Recevied")
                {
                    flag = false;
                    lblError.Text += "Photo Id Proof not recevied ||";
                }
                if (drpAddressProof.SelectedValue != "Recevied")
                {
                    flag = false;
                    lblError.Text += "Address Proof not recevied ||";
                }
                if (flag == false)
                {
                    return false;
                }
            }
            lblError.Text = "";
            return true;
        }
        protected void btnDiscard_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnDiscard.Enabled = false;
            pnl.Enabled = false;
            fillValues();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {

            btnEdit.Enabled = false;
            btnPhoneFollowup.Enabled = true;
            btnSave.Enabled = true;
            btnDiscard.Enabled = true;
            pnl.Enabled = true;
            lblVisitDate.InnerText = "Visit Date";
            lblVisitPurpose.Text = "Visit Purpose";
            TdPlace.Visible = true;
            TdPlace1.Visible = true;
            //fillValues();
        }
        protected void btnPhoneFollowup_Click(object sender, EventArgs e)
        {
            ViewState["ActivityId"] = "2";
            Session["SalesID"] = Request.QueryString["id"].ToString();
            string[,] Id1 = oDBEngine.GetFieldValue("tbl_trans_SExpenditure", "expnd_empId", " expnd_empId='" + Session["SalesID"].ToString() + "'", 1);
            if (Id1[0, 0] != "n")
            {
            }
            else
            {
                oDBEngine.InsurtFieldValue("tbl_trans_SExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Convenyence','" + Session["SalesID"].ToString() + "','1','1','1','1'");
                oDBEngine.InsurtFieldValue("tbl_trans_SExpenditure", "expnd_expenceType,expnd_empId,expnd_TTravCity1,expnd_TCity1,expnd_TTravCity2,expnd_TCity2", "'Trav','" + Session["SalesID"].ToString() + "','1','1','1','1'");
            }
            btnEdit.Enabled = true;
            btnPhoneFollowup.Enabled = false;
            btnSave.Enabled = true;
            btnDiscard.Enabled = true;
            pnl.Enabled = true;
            lblVisitDate.InnerText = "Call Date";
            lblVisitPurpose.Text = "Call Purpose";
            TdPlace.Visible = false;
            TdPlace1.Visible = false;
            fillValues();
        }
        public void setReminder(string note)
        {
            string remId = "";
            if (Request.QueryString["id"] != null)
            {
                string[,] remid1 = oDBEngine.GetFieldValue("tbl_trans_reminder", "rem_id", " rem_sourceid='" + Request.QueryString["id"].ToString() + "'", 1);
                if (remid1[0, 0] != "n")
                {
                    remId = remid1[0, 0];
                }
            }
            string enddate = Convert.ToDateTime(new_startdate).AddDays(1).ToString();
            string endtime = Convert.ToDateTime(ASPxNextVisitDate.Value).TimeOfDay.ToString();
            //string new_endtime = Convert.ToDateTime(txtnextvisitdate.Text.ToString()).ToShortDateString();
            string new_endtime1 = Convert.ToDateTime(ASPxNextVisitDate.Value).AddMinutes(-30).ToString();

            //string StartDate = ViewState["SDate"].ToString() + " " + ViewState["STime"].ToString();
            string[] aa = new_endtime1.Split(' ');
            new_endtime1 = aa[1].ToString();
            string[] hh1 = aa[1].Split(':');
            if (aa[2].ToString() == "PM" && hh1[0].ToString() != "12")
            {
                string[] hh_mm = new_endtime1.Split(':');
                int hh = 12 + int.Parse(hh_mm.GetValue(0).ToString());
                new_endtime1 = hh + ":" + hh_mm[1];
            }
            new_endtime1 = aa[0].ToString() + " " + new_endtime1;
            string msg = "";
            //msg = txtNotes.Text;
            if (remId == "")
            {
                oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken,rem_sourceid,CreateDate,CreateUser", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().Date + "','" + Session["userid"].ToString() + "','" + Convert.ToDateTime(new_endtime1) + "','" + Convert.ToDateTime(enddate) + "','" + note + "',1,0,'" + Request.QueryString["id"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
            }
            else
            {
                oDBEngine.SetFieldValue("tbl_trans_reminder", "rem_createUser='" + Session["userid"].ToString() + "',rem_createDate='" + oDBEngine.GetDate().ToString() + "',rem_targetUser='" + Session["userid"].ToString() + "',rem_startDate='" + Convert.ToDateTime(new_endtime1) + "',rem_endDate='" + Convert.ToDateTime(enddate) + "',rem_reminderContent='" + note + "',rem_displayTricker=1,rem_actionTaken=0,rem_sourceid='" + Request.QueryString["id"].ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rem_id='" + remId + "'");
            }
        }
    }
}