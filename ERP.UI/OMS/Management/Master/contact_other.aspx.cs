using ClsDropDownlistNameSpace;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_contact_other : ERP.OMS.ViewState_class.VSPage
    {
        #region Global Variable
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        string RetVal = "";
        //Converter Oconverter = new Converter();
        //GenericMethod objGenericMethod;

        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.GenericMethod objGenericMethod;

        AspxHelper objAspxHelper;
        //DBEngine objDbEngine;
        //GenericStoreProcedure objGenericStoredProcedure;

        BusinessLogicLayer.DBEngine objDbEngine;
        BusinessLogicLayer.GenericStoreProcedure objGenericStoredProcedure;
        #endregion
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // txtVerify.Attributes.Add("onkeyup","SearchByEmployee(this,SearchEmployeesForDigitalSignature,event)");
            txtVerify.Attributes.Add("onkeyup", "CallList(this,'SearchByEmployee',event)");

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            else
            {
                ViewState["UserID"] = HttpContext.Current.Session["userid"];
            }
            if (Session["Name"] != null)
            {
                lblName.Text = Session["Name"].ToString();

            }
            if (!Page.IsPostBack)
            {
                StDate.UseMaskBehavior = true;
                StDate.EditFormatString = Oconverter.GetDateFormat("Date");

                BindFamilyRelationShip();
                Bindpep();
                ShowOthers();
                ViewState["mode"] = 1;
                BindRegAgency();
                BindCompany();
                LoadEditableData();

                ScriptManager.RegisterStartupScript(this, GetType(), "closeKraDetailPopUp", "CloseKraDeatailPopUp('1');", true);
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                string CtrlID = string.Empty;
                if (hdnISSavable.Value == "t" && Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"] == string.Empty)
                {
                    // CtrlID = Request.Form["__EVENTTARGET"];
                    hdnISSavable.Value = "f";
                    SaveData();

                }
                if (rbtnlstRegInter.SelectedValue == "2")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideDiv", "HideDiv();", true);
                }
            }
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "CloseKraDeatailPopUp", "CloseKraDeatailPopUp('1');", true);
            }
        }
        protected void compInsert_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string TMCode = "";
            string FieldValue = "";
            if (ddlClientType.SelectedItem.Value != "Trading Member")
                TMCode = "";
            else
                TMCode = txtTMCode.Text;

            if (ddlpep.SelectedValue == "")
                FieldValue = "cnt_clienttype='" + ddlClientType.SelectedItem.Value + "',cnt_Custodian='" + txtCustodian_hidden.Value + "',cnt_SettlementMode='" + ddlSettlementMode.SelectedItem.Value + "',cnt_ContractDeliveryMode='" + ddlcontract.SelectedItem.Value + "',cnt_DirectTMClient='" + ddlDirClirnt.SelectedItem.Value + "',cnt_RelationshipWithDirector='" + ddlRelationshipTM.SelectedItem.Value + "',cnt_DirectorID='" + txtdirector_hidden.Value + "',cnt_DirectorClientRelation='" + ddlRelationship.SelectedItem.Value + "',cnt_HasOtherAccount='" + ddlAssociates.SelectedItem.Value + "',cnt_FamilyGroupCode='" + txtFamilygrpCode_hidden.Value + "',cnt_GroupSettlementMode='" + ddlSettmode.SelectedItem.Value + "',cnt_STPProvider=case when '" + ddlSTP.SelectedItem.Value.ToString() + "'='O' then null else '" + ddlSTP.SelectedItem.Value.ToString() + "' end ,cnt_FundManagerID='" + txtFundManager_hidden.Value + "',cnt_tradingCode='" + TMCode + "',cnt_SpecialCategory='" + cmbCategory.SelectedItem.Value + "',cnt_RiskCategory='" + cmbRisk.SelectedItem.Value + "',cnt_InPersonVerificationDone='" + cmbVerify.SelectedItem.Value + "',cnt_VerifcationRemarks='" + txtRemarks.Text + "'";
            else

                FieldValue = "cnt_clienttype='" + ddlClientType.SelectedItem.Value + "',cnt_Custodian='" + txtCustodian_hidden.Value + "',cnt_SettlementMode='" + ddlSettlementMode.SelectedItem.Value + "',cnt_ContractDeliveryMode='" + ddlcontract.SelectedItem.Value + "',cnt_DirectTMClient='" + ddlDirClirnt.SelectedItem.Value + "',cnt_RelationshipWithDirector='" + ddlRelationshipTM.SelectedItem.Value + "',cnt_DirectorID='" + txtdirector_hidden.Value + "',cnt_DirectorClientRelation='" + ddlRelationship.SelectedItem.Value + "',cnt_HasOtherAccount='" + ddlAssociates.SelectedItem.Value + "',cnt_FamilyGroupCode='" + txtFamilygrpCode_hidden.Value + "',cnt_GroupSettlementMode='" + ddlSettmode.SelectedItem.Value + "',cnt_STPProvider=case when '" + ddlSTP.SelectedItem.Value.ToString() + "'='O' then null else '" + ddlSTP.SelectedItem.Value.ToString() + "' end ,cnt_FundManagerID='" + txtFundManager_hidden.Value + "',cnt_tradingCode='" + TMCode + "',cnt_SpecialCategory='" + cmbCategory.SelectedItem.Value + "',cnt_RiskCategory='" + cmbRisk.SelectedItem.Value + "',cnt_InPersonVerificationDone='" + cmbVerify.SelectedItem.Value + "',cnt_VerifcationRemarks='" + txtRemarks.Text + "',cnt_pep='" + ddlpep.SelectedValue + "'";
            if (cmbVerify.SelectedItem.Value.ToString() == "Y")
            {
                FieldValue = FieldValue + ",cnt_InPersonVerificationDate='" + StDate.Value + "',cnt_InPersonVerificationBy='" + txtVerify_hidden.Value + "'";
            }

            Int32 NoofRowsAffect = oDBEngine.SetFieldValue("tbl_master_contact", FieldValue, " cnt_internalId='" + Session["KeyVal_InternalID"].ToString() + "'");
            if (NoofRowsAffect > 0)
                RetVal = "1";
        }
        protected void compInsert_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpUpdate"] = RetVal;
        }
        public void ShowOthers()
        {
            DataTable dtOthers = oDBEngine.GetDataTable("tbl_master_contact", "cnt_clienttype,cnt_Custodian,cnt_SettlementMode,cnt_ContractDeliveryMode,cnt_DirectTMClient,cnt_RelationshipWithDirector,cnt_DirectorID,cnt_DirectorClientRelation,cnt_HasOtherAccount,cnt_FamilyGroupCode,cnt_GroupSettlementMode,cnt_STPProvider,cnt_FundManagerID,(select Custodian_name+' ['+isnull(custodian_shortname,'')+']' from master_custodians where custodian_internalid=tbl_master_contact.cnt_Custodian) as Custodian,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact c where c.cnt_internalid=tbl_master_contact.cnt_DirectorID) as Director,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact cn where cn.cnt_internalid=tbl_master_contact.cnt_FamilyGroupCode) as FamilyGrpCode,(select fundmanager_firstname+' '+isnull(fundmanager_middlename,'')+' '+isnull(fundmanager_lastname,'') from master_fundmanagers where fundmanager_internalid=tbl_master_contact.cnt_FundManagerID) as FundManager,cnt_tradingCode,cnt_SpecialCategory,cnt_RiskCategory,cnt_InPersonVerificationDone,cnt_InPersonVerificationDate,cnt_InPersonVerificationBy,cnt_VerifcationRemarks,cnt_pep,(select isnull(PoliticalConnection_Description,'') from Master_PoliticalConnection where cnt_pep=PoliticalConnection_ID) as pep", " cnt_internalId='" + Session["KeyVal_InternalID"].ToString() + "'");
            if (dtOthers.Rows.Count > 0)
            {

                ddlClientType.SelectedValue = dtOthers.Rows[0][0].ToString();
                txtCustodian_hidden.Value = dtOthers.Rows[0][1].ToString();
                ddlSettlementMode.SelectedValue = dtOthers.Rows[0][2].ToString();
                ddlcontract.SelectedValue = dtOthers.Rows[0][3].ToString();
                ddlDirClirnt.SelectedValue = dtOthers.Rows[0][4].ToString();
                ddlRelationshipTM.SelectedValue = dtOthers.Rows[0][5].ToString();
                txtdirector_hidden.Value = dtOthers.Rows[0][6].ToString();
                ddlRelationship.SelectedValue = dtOthers.Rows[0][7].ToString();
                ddlAssociates.SelectedValue = dtOthers.Rows[0][8].ToString();
                txtFamilygrpCode_hidden.Value = dtOthers.Rows[0][9].ToString();
                ddlSettmode.SelectedValue = dtOthers.Rows[0][10].ToString();
                ddlSTP.SelectedValue = dtOthers.Rows[0][11].ToString();
                txtFundManager_hidden.Value = dtOthers.Rows[0][12].ToString();
                txtCustodian.Text = dtOthers.Rows[0][13].ToString();
                txtdirector.Text = dtOthers.Rows[0][14].ToString();
                txtFamilygrpCode.Text = dtOthers.Rows[0][15].ToString();
                txtFundManager.Text = dtOthers.Rows[0][16].ToString();
                txtTMCode.Text = dtOthers.Rows[0][17].ToString();
                cmbCategory.SelectedValue = dtOthers.Rows[0]["cnt_SpecialCategory"].ToString();
                cmbRisk.SelectedValue = dtOthers.Rows[0]["cnt_RiskCategory"].ToString();

                cmbVerify.SelectedValue = dtOthers.Rows[0]["cnt_InPersonVerificationDone"].ToString();
                ddlpep.SelectedValue = dtOthers.Rows[0]["cnt_pep"].ToString();
                if (dtOthers.Rows[0]["cnt_InPersonVerificationDone"].ToString() == "Y")
                {
                    if (dtOthers.Rows[0]["cnt_InPersonVerificationDate"].ToString() != "")
                        StDate.Value = Convert.ToDateTime(dtOthers.Rows[0]["cnt_InPersonVerificationDate"].ToString());
                    else
                        //StDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
                        StDate.Value = oDBEngine.GetDate();
                    if (dtOthers.Rows[0]["cnt_InPersonVerificationBy"].ToString() != "")
                    {
                        DataTable dtName = oDBEngine.GetDataTable("tbl_master_contact ", " ltrim(rtrim(isnull(cnt_firstname,'')))+' ' +ltrim(rtrim(isnull(cnt_middlename,' ')))+' '+ltrim(rtrim(isnull(cnt_lastname,' '))) +'['+ ltrim(rtrim(isnull(cnt_shortname,cnt_ucc)))+']'   ", "cnt_internalid='" + dtOthers.Rows[0]["cnt_InPersonVerificationBy"].ToString() + "'");
                        if (dtName.Rows.Count > 0)
                        {
                            txtVerify.Text = dtName.Rows[0][0].ToString();
                        }
                        else
                        {
                            txtVerify.Text = "";
                        }
                    }
                    txtVerify_hidden.Value = dtOthers.Rows[0]["cnt_InPersonVerificationBy"].ToString();
                    txtRemarks.Text = dtOthers.Rows[0]["cnt_VerifcationRemarks"].ToString();


                    Page.ClientScript.RegisterStartupScript(GetType(), "JScri4", "<script language='javascript'>GetDetails('" + dtOthers.Rows[0]["cnt_InPersonVerificationDone"].ToString() + "')</script>");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScri4", "<script language='javascript'>GetDetails('N')</script>");
                }



                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>relationshipTM('" + dtOthers.Rows[0][5].ToString() + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1", "<script language='javascript'>associates('" + dtOthers.Rows[0][8].ToString() + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript2", "<script language='javascript'>TmCodeShow('" + dtOthers.Rows[0][0].ToString() + "')</script>");
            }
        }
        public void BindFamilyRelationShip()
        {
            string[,] Relation = oDBEngine.GetFieldValue("tbl_master_familyRelationship", "fam_id,fam_familyRelationship", null, 2);
            if (Relation[0, 0] != "n")
            {
                //oDBEngine.AddDataToDropDownList(Relation, ddlRelationship);
                oclsDropDownList.AddDataToDropDownList(Relation, ddlRelationship);
            }
        }
        public void Bindpep()
        {
            string[,] pep = oDBEngine.GetFieldValue("Master_PoliticalConnection", "PoliticalConnection_ID,PoliticalConnection_Description", null, 2);
            if (pep[0, 0] != "n")
            {
                //oDBEngine.AddDataToDropDownList(pep, ddlpep);
                oclsDropDownList.AddDataToDropDownList(pep, ddlpep);
                ddlpep.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        #region Control Events
        protected void lbtnKraDetailPopUp_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowKraDetailPopUp", "ShowKraDetailPopUp('1');", true);
            ClearAllValues();
            LoadEditableData();

        }
        protected void btnSaveData_Click(object sender, EventArgs e)
        {
            // SaveData(); 
        }
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            if (divKraDetail.Visible == false)
            {
                divKraDetail.Visible = true;
                EnableDisableControls(true);

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowKraDetailPopUp", "ShowKraDetailPopUp();", true);
            ClearAllValues();
            SetDefaultDates();
        }
        protected void lbtnModify_Click(object sender, EventArgs e)
        {

            EnableDisableControls(true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowKraDetailPopUp", "ShowKraDetailPopUp();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "enableDisable", "EnableDisableControls('1');", true);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }
        public void btnCancels_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Methods
        private void DeleteData()
        {
            //objGenericStoredProcedure = new GenericStoreProcedure();
            //objGenericMethod = new GenericMethod();

            BusinessLogicLayer.GenericStoreProcedure objGenericStoredProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            BusinessLogicLayer.GenericMethod objGenericMethod = new BusinessLogicLayer.GenericMethod();

            string[] strSpParam = new string[2];
            strSpParam[0] = "Mode|" + GenericStoreProcedure.ParamDBType.Int + "|20|" + 3 + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "KRADetail_CntID|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "|" + GenericStoreProcedure.ParamType.ExParam;

            DataTable dtKRADetail = new DataTable();
            dtKRADetail = objGenericStoredProcedure.Procedure_DataTable(strSpParam, "InsertDelete_KraDetail");
            if (dtKRADetail != null && dtKRADetail.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerts", "alert('Data Deleted Successfully');", true);

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowKraDetailPopUp", "CloseKraDeatailPopUp('1');", true);
            }
            LoadEditableData();
        }
        private void EnableDisableControls(bool b)
        {
            txtRegistrationNumber.Enabled = b;
            ddlRegistrationAgency.Enabled = b;
            txtRegistrationDate.Enabled = b;
            rbtnlstRegInter.Enabled = b;
            ddlCompany.Enabled = b;
            txtOtherEntity.Enabled = b;
            txtNewKYCDate.Enabled = b;
            txtKYCModDate.Enabled = b;
            ddlStatus.Enabled = b;
            txtStatusDate.Enabled = b;
            ddlDocumentSource.Enabled = b;
            btnSaveDatas.Visible = b;
            btnCancels.Visible = b;

        }
        private void SetDefaultDates()
        {
            txtRegistrationDate.Value = oDBEngine.GetDate();
            txtStatusDate.Value = oDBEngine.GetDate();
            txtNewKYCDate.Value = oDBEngine.GetDate();
            txtKYCModDate.Value = oDBEngine.GetDate();
        }
        private void LoadEditableData()
        {
            //objGenericStoredProcedure = new GenericStoreProcedure();
            //objGenericMethod = new GenericMethod();

            BusinessLogicLayer.GenericStoreProcedure objGenericStoredProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            BusinessLogicLayer.GenericMethod objGenericMethod = new BusinessLogicLayer.GenericMethod();

            string[] strSpParam = new string[2];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 4 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "KRADetail_CntID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            DataTable dtKRADetail = new DataTable();
            dtKRADetail = objGenericStoredProcedure.Procedure_DataTable(strSpParam, "InsertDelete_KraDetail");
            if (dtKRADetail != null && dtKRADetail.Rows.Count > 0)
            {
                ViewState["mode"] = 2;
                txtRegistrationNumber.Text = Convert.ToString(dtKRADetail.Rows[0]["KRADetail_Number"]);
                txtRegistrationDate.Text = Convert.ToString(dtKRADetail.Rows[0]["KRADetail_RgnDate"]);
                // ddlRegistrationAgency.Value = dtKRADetail.Rows[0]["KRADetail_KycRA"].ToString()+"-RA";
                ddlRegistrationAgency.SelectedValue = dtKRADetail.Rows[0]["KRADetail_KycRA"].ToString() + "-RA";
                if (dtKRADetail.Rows[0]["KRADetail_Source"] != null && Convert.ToString(dtKRADetail.Rows[0]["KRADetail_Source"]) != string.Empty)
                {
                    rbtnlstRegInter.SelectedValue = "1";
                    ddlCompany.SelectedValue = Convert.ToString(dtKRADetail.Rows[0]["KRADetail_Source"]);
                    // ShowHidePanel(true);
                }
                else
                {
                    rbtnlstRegInter.SelectedValue = "2";
                    txtOtherEntity.Text = Convert.ToString(dtKRADetail.Rows[0]["KRADetail_OtherSource"]);
                    // ShowHidePanel(false);
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideDiv", "HideDiv();", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "close", "HideDiv();", true);
                }
                txtNewKYCDate.Text = Convert.ToString(dtKRADetail.Rows[0][7]);
                txtKYCModDate.Text = Convert.ToString(dtKRADetail.Rows[0][8]);
                if (Convert.ToString(dtKRADetail.Rows[0]["KRADetail_Status"]).ToLower() == "verified")
                {
                    ddlStatus.SelectedIndex = 1;
                }
                else if (Convert.ToString(dtKRADetail.Rows[0]["KRADetail_Status"]).ToLower() == "under process")
                {
                    ddlStatus.SelectedIndex = 2;
                }
                else if (Convert.ToString(dtKRADetail.Rows[0]["KRADetail_Status"]).ToLower() == "on hold")
                {
                    ddlStatus.SelectedIndex = 3;
                }
                else if (Convert.ToString(dtKRADetail.Rows[0]["KRADetail_Status"]).ToLower() == "rejected")
                {
                    ddlStatus.SelectedIndex = 4;
                }
                txtStatusDate.Text = Convert.ToString(dtKRADetail.Rows[0]["KRADetail_StatusDate"]);

                if (Convert.ToString(dtKRADetail.Rows[0]["KRADetail_ImageSource"]).ToLower() == "fetched")
                {
                    ddlDocumentSource.SelectedIndex = 1;
                }
                else if (Convert.ToString(dtKRADetail.Rows[0]["KRADetail_ImageSource"]).ToLower() == "scanned")
                {
                    ddlDocumentSource.SelectedIndex = 2;
                }
                EnableDisableControls(false);
                divKraDetail.Visible = true;
                EnableDisableLink(true);
            }
            else
            {
                ViewState["mode"] = 1;
                divKraDetail.Visible = false;
                EnableDisableLink(false);
                SetDefaultDates();
            }

        }
        private void ClearAllValues()
        {
            txtRegistrationNumber.Text = string.Empty;
            ddlRegistrationAgency.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlDocumentSource.SelectedIndex = 0;
            ddlCompany.SelectedIndex = 0;
            txtRegistrationDate.Text = string.Empty;
            txtOtherEntity.Text = string.Empty;
            txtStatusDate.Text = string.Empty;
            txtKYCModDate.Text = string.Empty;
            rbtnlstRegInter.SelectedValue = "1";
            ShowHidePanel(true);

        }
        private void EnableDisableLink(bool IsEnabled)
        {
            lbtnDelete.Enabled = IsEnabled;
            lbtnModify.Enabled = IsEnabled;
            lbtnAdd.Enabled = !IsEnabled;
        }
        private void SaveData()
        {

            //objGenericStoredProcedure = new GenericStoreProcedure();
            //objGenericMethod = new GenericMethod();

            BusinessLogicLayer.GenericStoreProcedure objGenericStoredProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            BusinessLogicLayer.GenericMethod objGenericMethod = new BusinessLogicLayer.GenericMethod();

            string[] strSpParam = new string[13];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + Convert.ToInt32(ViewState["mode"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "KRADetail_CntID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "RegNumber|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + txtRegistrationNumber.Text + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            /*   int originalLength = Convert.ToString(ddlRegistrationAgency.Value).LastIndexOf("-RT")+1;
               string strRegistrationAgencyParameter = Convert.ToString(ddlRegistrationAgency.Value).Substring(0, originalLength);
               strSpParam[3] = "RegAgencyId|" + GenericStoreProcedure.ParamDBType.Varchar + "|150|" + ddlRegistrationAgency.Value.ToString().Split('-')[0] + "|" + GenericStoreProcedure.ParamType.ExParam;*/
            int originalLength = Convert.ToString(ddlRegistrationAgency.SelectedValue).LastIndexOf("-RT") + 1;
            string strRegistrationAgencyParameter = Convert.ToString(ddlRegistrationAgency.SelectedValue).Substring(0, originalLength);
            strSpParam[3] = "RegAgencyId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + ddlRegistrationAgency.SelectedValue.ToString().Split('-')[0] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            if (txtRegistrationDate.Value != null)
            {
                strSpParam[4] = "RegDate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|15|" + (txtRegistrationDate.Value) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            if (rbtnlstRegInter.SelectedValue.ToString() == "1")
            {
                if (ddlCompany.SelectedIndex != 0)
                {
                    //strSpParam[5] = "CompanyIdEntityName|" + GenericStoreProcedure.ParamDBType.Varchar + "|150|" + ddlCompany.Value + "|" + GenericStoreProcedure.ParamType.ExParam;
                    strSpParam[5] = "CompanyIdEntityName|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + ddlCompany.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
            }
            else
            {
                if (txtOtherEntity.Text != string.Empty)
                {
                    strSpParam[5] = "CompanyIdEntityName|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + txtOtherEntity.Text.Trim() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
            }
            if (txtNewKYCDate.Value != null)
            {
                strSpParam[6] = "NewKycDate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|15|" + Convert.ToDateTime(txtNewKYCDate.Value) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }

            if (txtKYCModDate.Value != null)
            {
                strSpParam[7] = "KYCModDate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|15|" + Convert.ToDateTime(txtKYCModDate.Value) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }


            if (ddlStatus.SelectedValue != "0")
            {

                strSpParam[8] = "Status|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + ddlStatus.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }


            if (txtStatusDate.Value != null)
            {
                strSpParam[9] = "StatusDate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|20|" + Convert.ToDateTime(txtStatusDate.Value) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }


            if (ddlDocumentSource.SelectedValue != "0")
            {

                strSpParam[10] = "DocumentSource|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + ddlDocumentSource.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }



            strSpParam[11] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + Convert.ToString(ViewState["UserID"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            if (rbtnlstRegInter.SelectedValue.ToString() == "1")
            {
                strSpParam[12] = "IsOwnCompany|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|2|" + 1 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[12] = "IsOwnCompany|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|2|" + 2 + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            DataTable dtKRADetail = new DataTable();
            dtKRADetail = objGenericStoredProcedure.Procedure_DataTable(strSpParam, "InsertDelete_KraDetail");
            if (dtKRADetail != null && dtKRADetail.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerts", "alert('Data Saved Successfully');", true);

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowKraDetailPopUp", "CloseKraDeatailPopUp('1');", true);
                if (Convert.ToUInt32(ViewState["mode"]) == 1)
                {
                    ViewState["mode"] = 2;
                }
                LoadEditableData();
            }

        }
        private bool HasKraDetails()
        {
            string strKraDetailId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            string strWhereCondition = "KRADetail_CntID='" + strKraDetailId + "'";
            BusinessLogicLayer.GenericMethod objGenericMethod = new BusinessLogicLayer.GenericMethod();
            string[,] strKRAId = objGenericMethod.GetFieldValue("Trans_KRADetail", "KRADetail_ID", strWhereCondition, 1);
            if (strKRAId[0, 0] != "n")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void BindRegAgency()
        {
            //string strQuery="Select"+"'"+ "Select"+"'"+"as cnt_internalId,0 as cnt_id union Select cnt_internalId, cnt_id from tbl_master_contact Order by cnt_id";
            // string strQuery = "Select KycRA_Name, kycRa_Id from master_KycRA Order by kycRa_Id";
            string strQuery = "Select KycRA_Name, CAST( kycRa_Id as varchar(10)) +'-RA' as RAID from master_KycRA Order by kycRa_Id";
            objAspxHelper = new AspxHelper();
            // GenericMethod objGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod objGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtRegAgency = new DataTable();
            dtRegAgency = objGenericMethod.GetDataTable(strQuery);
            // objAspxHelper.Bind_Combo(ddlRegistrationAgency, dtRegAgency, "KycRA_Name", "RAID", 0);
            ddlRegistrationAgency.DataSource = dtRegAgency;
            ddlRegistrationAgency.DataTextField = "KycRA_Name";
            ddlRegistrationAgency.DataValueField = "RAID";
            ddlRegistrationAgency.DataBind();
            //   ddlRegistrationAgency.Items.Insert(0, new DevExpress.Web.ListEditItem("Select", 0));
            ddlRegistrationAgency.Items.Insert(0, new ListItem("Select", "0"));
            ddlRegistrationAgency.SelectedIndex = 0;

        }
        private void BindCompany()
        {
            string strQuery = "Select cmp_Name,cmp_internalid from tbl_master_company order by cmp_Name";
            objAspxHelper = new AspxHelper();
            //objGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod objGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtCompany = new DataTable();
            dtCompany = objGenericMethod.GetDataTable(strQuery);
            //  ddlCompany.Items.Insert(0, new DevExpress.Web.ListEditItem("Select", 0));
            //  objAspxHelper.Bind_Combo(ddlCompany, dtCompany, "cmp_Name", "cmp_internalid", 0);
            ddlCompany.DataSource = dtCompany;
            ddlCompany.DataTextField = "cmp_Name";
            ddlCompany.DataValueField = "cmp_internalid";
            ddlCompany.DataBind();
            //ddlCompany.Items.Insert(0, new DevExpress.Web.ListEditItem("Select", 0));
            ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
            ddlCompany.SelectedIndex = 0;
        }
        private void ShowHidePanel(bool visibilty)
        {
            //  pnlCompany.Visible = visibilty;
            //  pnlOther.Visible = !visibilty;
        }
        #endregion


    }
}
