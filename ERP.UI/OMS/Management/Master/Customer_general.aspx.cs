using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using DataAccessLayer;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;

namespace ERP.OMS.Management.Master
{
    public partial class Customer_general : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page
    {
        Int32 ID;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
                if (Request.QueryString["id"] == "ADD")
                {
                    //   DisabledTabPage();
                    base.OnPreInit(e);
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (txtClentUcc.Text.ToString() != "")
            {
                //string prefx = txtFirstNmae.Text.ToString().Substring(0, 1).ToUpper();
                string prefx = txtClentUcc.Text.ToString();


                /* Tier Structure
                String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlConnection lcon = new SqlConnection(con);
                lcon.Open();
                SqlCommand lcmdEmplInsert = new SqlCommand("sp_GenerateContactUCC", lcon);
                lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                lcmdEmplInsert.Parameters.AddWithValue("@UCC", prefx);
                SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 10);
                parameter.Direction = ParameterDirection.Output;
                lcmdEmplInsert.Parameters.Add(parameter);
                lcmdEmplInsert.ExecuteNonQuery();
                   string InternalID = parameter.Value.ToString();
                */

                string InternalID = oContactGeneralBL.Get_UCCCode(prefx);



                if (InternalID != "")
                {
                    txtClentUcc.Text = InternalID;
                }
                else
                {
                    lblErr.Text = "</br>No UCC found..Type another UCC.";
                    lblErr.Visible = true;
                }
            }
            else
            {
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "<script>alert('Please Insert first name');</script>", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "popup();", true);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtbranch = new DataTable();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Customer_general.aspx");

            CommonBL ComBL = new CommonBL();
            string UniqueAutoNumberCustomerMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberCustomerMaster");

            string SyncUsertoWhileCreating = ComBL.GetSystemSettingsResult("SyncCustomertoFSMWhileCreating");
            hdnSyncCustomertoFSMWhileCreating.Value = SyncUsertoWhileCreating;

            cmbContactStatusclient.Visible = false;
            txtContactStatusclient.Visible = false;


            this.Title = "Customer/Client";
            pnlCredit.Style.Add("display", "block");
            lblCreditcard.Visible = true;
            ChkCreditcard.Enabled = true;
            lblcreditDays.Visible = true;
            txtcreditDays.Enabled = true;
            lblCreditLimit.Visible = true;
            txtCreditLimit.Enabled = true;
            cmbContactStatusclient.Visible = true;
            txtContactStatusclient.Visible = true;



            if (!IsPostBack)
            {
                table_others.Visible = true;
                cmbContactStatus.Attributes.Add("onchange", "javascript:ContactStatus()");
                DDLBind();

                if (!String.IsNullOrEmpty(UniqueAutoNumberCustomerMaster))
                {
                    if (UniqueAutoNumberCustomerMaster == "Yes")
                    {
                        hdnAutoNumStg.Value = "1";
                        hdnTransactionType.Value = "CL";
                        //dvIdType.Visible = false;
                        //ASPxLabel12.Visible = false;
                        //ASPxLabelS12.Visible = false;
                        //dvClentUcc.Visible = false;
                        dvIdType.Style.Add("display", "none");
                        dvUniqueId.Style.Add("display", "none");
                        ddl_Num.Style.Add("display", "block");
                        dvCustDocNo.Style.Add("display", "block");
                        NumberingSchemeBind();
                    }
                    else if (UniqueAutoNumberCustomerMaster.ToUpper().Trim() == "NO")
                    {
                        hdnAutoNumStg.Value = "0";
                        hdnTransactionType.Value = "";
                        dvIdType.Style.Add("display", "block");
                        dvUniqueId.Style.Add("display", "block");

                    }
                }


                //For Udf data
                if (Request.QueryString["InternalId"] != null)
                {
                    //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                    InsertMode.Value = "Edit";
                    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                    DataTable DT = objEngine.GetDataTable("tbl_master_contact", "*,isnull(cnt_IdType,0) Cust_IdType", "cnt_internalId = '" + Convert.ToString(Request.QueryString["InternalId"]) + "'");
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        ddl_Num.Style.Add("display", "none");
                        txt_CustDocNo.ClientEnabled = false;
                        txt_CustDocNo.Text = Convert.ToString(DT.Rows[0]["cnt_UCC"]);
                        KeyVal_InternalID.Value = Convert.ToString(DT.Rows[0]["cnt_internalId"]);
                        hdCustomerName.Value = Convert.ToString(DT.Rows[0]["cnt_firstName"]);
                        cmbLegalStatus.SelectedValue = Convert.ToString(DT.Rows[0]["cnt_legalStatus"]);
                        txtClentUcc.Text = Convert.ToString(DT.Rows[0]["cnt_UCC"]);
                        HdCustUniqueName.Value = Convert.ToString(DT.Rows[0]["cnt_UCC"]);
                        txtClentUcc.ClientEnabled = false;
                        txtFirstNmae.Text = Convert.ToString(DT.Rows[0]["cnt_firstName"]);
                        txtDOB.Value = Convert.ToDateTime(DT.Rows[0]["cnt_dOB"]);
                        ddlnational.SelectedValue = Convert.ToString(DT.Rows[0]["cnt_branchId"]);
                        txtAnniversary.Value = Convert.ToDateTime(DT.Rows[0]["cnt_anniversaryDate"]);
                        cmbGender.SelectedValue = Convert.ToString(DT.Rows[0]["cnt_sex"]);
                        ChkCreditcard.Value = Convert.ToBoolean(DT.Rows[0]["cnt_IsCreditHold"]);
                        txtcreditDays.Text = Convert.ToString(DT.Rows[0]["cnt_CreditDays"]);
                        txtCreditLimit.Text = Convert.ToString(DT.Rows[0]["cnt_CreditLimit"]);
                        cmbMaritalStatus.SelectedValue = Convert.ToString(DT.Rows[0]["cnt_maritalStatus"]);
                        cmbContactStatusclient.Value = Convert.ToString(DT.Rows[0]["Statustype"]);
                        lstTaxRates_MainAccount.SelectedValue = Convert.ToString(DT.Rows[0]["cnt_mainAccount"]);
                        hdnNumberingId.Value = Convert.ToString(DT.Rows[0]["cnt_Numberscheme"]);
                        string TcsApplyValue = Convert.ToString(DT.Rows[0]["cnt_Numberscheme"]);
                        string GSTIN = Convert.ToString(DT.Rows[0]["Is_TCSApplicable"]);
                        if (TcsApplyValue != "" && TcsApplyValue != null)
                        {
                            TCSApplicable.Value = Convert.ToBoolean(TcsApplyValue);
                        }
                        if (GSTIN != "")
                        {
                            txtGSTIN1.Text = GSTIN.Substring(0, 2);
                            txtGSTIN2.Text = GSTIN.Substring(2, 10);
                            txtGSTIN3.Text = GSTIN.Substring(12, 3);
                        }
                        else
                        {
                            txtGSTIN1.Text = "";
                            txtGSTIN2.Text = "";
                            txtGSTIN3.Text = "";
                        }
                        ddlIdType.SelectedValue = Convert.ToString(DT.Rows[0]["Cust_IdType"]);

                        TabPage page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                        page.Visible = true;
                    }
                    ddlIdType.Enabled = false;
                }
                else
                {
                    InsertMode.Value = "Add";
                    KeyVal_InternalID.Value = "";
                    cmbLegalStatus.SelectedIndex.Equals(0);
                    txtClentUcc.ClientEnabled = true;
                    txtClentUcc.Text = "";
                    txtFirstNmae.Text = "";
                    txtMiddleName.Text = "";
                    txtDOB.Value = "";
                    ddlnational.ClearSelection();
                    ddlnational.Items.FindByValue("78").Selected = true;
                    txtAnniversary.Value = "";
                    cmbGender.SelectedIndex.Equals(0);
                    ChkCreditcard.Value = false;
                    txtcreditDays.Text = "";
                    txtCreditLimit.Text = "";
                    cmbMaritalStatus.SelectedIndex.Equals(0);
                    cmbContactStatusclient.SelectedIndex.Equals(0);
                    lstTaxRates_MainAccount.SelectedIndex.Equals(0);
                    txtGSTIN1.Text = "";
                    txtGSTIN2.Text = "";
                    txtGSTIN3.Text = "";
                    TabPage page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                    page.Visible = false;
                }
            }
            //  SetUdfApplicableValue();

        }
        protected void SetUdfApplicableValue()
        {
            hdKeyVal.Value = "Cus";
            //Debjyoti 30-12-2016
            //Reason: UDF count
            IsUdfpresent.Value = Convert.ToString(getUdfCount());
            //End Debjyoti 30-12-2016
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='" + hdKeyVal.Value + "'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        public void NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForCustomerMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

        }

        public DataTable GetAllDropDownDetailForCustomerMaster(string UserBranch, string Qry)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, Qry);
            proc.AddVarcharPara("@userbranchlist", 4000, UserBranch);
            ds = proc.GetTable();
            return ds;
        }
        public void DDLBind()
        {
            DataSet DDDs = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPopup");
            proc.AddVarcharPara("@Action", 500, "GetDropDownValue");
            DDDs = proc.GetDataSet();

            cmbMaritalStatus.DataSource = DDDs.Tables[0];
            cmbMaritalStatus.DataValueField = "mts_id";
            cmbMaritalStatus.DataTextField = "mts_maritalStatus";

            cmbLegalStatus.DataSource = DDDs.Tables[1];
            cmbLegalStatus.DataValueField = "lgl_id";
            cmbLegalStatus.DataTextField = "lgl_legalStatus";

            ddlnational.DataSource = DDDs.Tables[2];
            ddlnational.DataValueField = "Nationality_id";
            ddlnational.DataTextField = "Nationality_Description";

            cmbContactStatus.Items.Add("1");

            cmbMaritalStatus.DataBind();
            cmbLegalStatus.DataBind();
            ddlnational.DataBind();

            cmbLegalStatus.SelectedValue = "1";
            cmbContactStatus.SelectedValue = "0";
            cmbMaritalStatus.Items.Insert(0, new ListItem("--Select--", "0"));



        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string dd = "Customer/Client";
            DateTime dtDob, dtanniversary, dtReg, dtBusiness;
            string country = ddlnational.SelectedValue;
            string GSTIN = "";
            GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();

            if (txtDOB.Value != null)
            {
                dtDob = Convert.ToDateTime(txtDOB.Value);
            }
            else
            {
                dtDob = Convert.ToDateTime("01-01-1900");
            }

            if (txtAnniversary.Value != null)
            {
                dtanniversary = Convert.ToDateTime(txtAnniversary.Value);
            }
            else
            {
                dtanniversary = Convert.ToDateTime("01-01-1900");
            }
            Boolean Creditcard;
            if (ChkCreditcard.Checked)
            {
                Creditcard = true;
            }
            else
            {
                Creditcard = false;
            }
            int creditDays = 0;
            if (txtcreditDays.Text.Trim() != "")
            {
                creditDays = Convert.ToInt32(txtcreditDays.Text.Trim());
            }
            decimal CreditLimit = 0;
            if (txtCreditLimit.Text.Trim() != "")
            {
                CreditLimit = Convert.ToDecimal(txtCreditLimit.Text.Trim());
            }

            // chinmoy edited for Auto Number scheme start
            int numberingId = 0;
            string UccName = "";
            if (hdnAutoNumStg.Value == "1" && hdnTransactionType.Value == "CL")
            {
                numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                UccName = hddnDocNo.Value.Trim();

            }
            else
            {
                UccName = txtClentUcc.Text.Trim();
            }
            //End
            //rev srijeeta
            string AlternativeCode = "";
            if (Alternative_Code.Text.Trim() != "")
            {
                AlternativeCode = Alternative_Code.Text.Trim();
            }

            //end of rev srijeeta
            if (KeyVal_InternalID.Value == "")
            {
                //rev srijeeta
                //string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, UccName, "1",
                //                                      txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), "",
                //                                      UccName, Convert.ToString(Session["userbranchID"]), cmbGender.SelectedItem.Value,
                //                                      cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                //                                      "1", "1", "",
                //                                      "0", "0", "0",
                //                                      "1", "", "", "CL",
                //                                      cmbContactStatus.SelectedItem.Value, DateTime.Now, "1", "",
                //                                      "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                //                                      DateTime.Now, "", country, Creditcard, creditDays,
                //                                      CreditLimit, Convert.ToString(cmbContactStatusclient.SelectedItem.Value),
                //                                      GSTIN, hidAssociatedEmp.Value, "", "", "", numberingId, "", "", "", Convert.ToBoolean(TCSApplicable.Value));
                string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, UccName, AlternativeCode, "1",
                                                      txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), "",
                                                      UccName, Convert.ToString(Session["userbranchID"]), cmbGender.SelectedItem.Value,
                                                      cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                                      "1", "1", "",
                                                      "0", "0", "0",
                                                      "1", "", "", "CL",
                                                      cmbContactStatus.SelectedItem.Value, DateTime.Now, "1", "",
                                                      "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                                                      DateTime.Now, "", country, Creditcard, creditDays,
                                                      CreditLimit, Convert.ToString(cmbContactStatusclient.SelectedItem.Value),
                                                      GSTIN, hidAssociatedEmp.Value, "", "", "", numberingId, "", "", "", Convert.ToBoolean(TCSApplicable.Value));
                //end of rev srijeeta
                KeyVal_InternalID.Value = InternalId;
                hdCustomerName.Value = txtFirstNmae.Text.Trim();
                HdCustUniqueName.Value = txtClentUcc.Text.Trim();
                txtClentUcc.ClientEnabled = false;
                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_IdType=" + ddlIdType.SelectedValue, " cnt_internalId='" + InternalId + "'");
                UpdateUniqueId(InternalId, Convert.ToInt32(ddlIdType.SelectedValue), txtClentUcc.Text.Trim());
                ddlIdType.Enabled = false;
                if (hdnAutoNumStg.Value == "1" && hdnTransactionType.Value == "CL")
                {
                    DataTable dtDoc = new DataTable();
                    ddl_Num.Style.Add("display", "none");
                    txt_CustDocNo.ClientEnabled = false;

                    dtDoc = oDBEngine.GetDataTable("select isnull(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalId.Trim() + "'");
                    txt_CustDocNo.Text = Convert.ToString(dtDoc.Rows[0]["cnt_UCC"]);
                }
                TabPage page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                page.Visible = true;
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Succesfully.')</script>");
            }
            else
            {
                txtClentUcc.ClientEnabled = false;
                //rev srijeeta
                //string value = "Statustype='',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=1,  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='', cnt_shortName='', cnt_branchId=" + Convert.ToInt32(Session["userbranchID"]) + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=0, cnt_profession=0, cnt_organization='0', cnt_jobResponsibility=0, cnt_designation=0, cnt_industry=0, cnt_contactSource=0, cnt_referedBy='0', cnt_contactType='CL', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + DateTime.Now.ToShortDateString() + "',cnt_rating='0',cnt_reason='0',cnt_bloodgroup='0',WebLogIn='No',PassWord='', lastModifyDate ='" + DateTime.Now.ToShortDateString() + "',cnt_PlaceOfIncorporation='0',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='" + DateTime.Now.ToShortDateString() + "',cnt_OtherOccupation='',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + creditDays + "' ,cnt_CreditLimit='" + CreditLimit + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_AssociatedEmp= '" + hidAssociatedEmp.Value + "',cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',Is_TCSApplicable='" + Convert.ToBoolean(TCSApplicable.Value) + "'";
                //oDBEngine.SetFieldValue("tbl_master_contact", value, "cnt_internalId = '" + KeyVal_InternalID.Value + "'");
                string value = "Statustype='',cnt_ucc='" + txtClentUcc.Text + "',Alternative_Code ='" + Alternative_Code.Text + "', cnt_salutation=1,  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='', cnt_shortName='', cnt_branchId=" + Convert.ToInt32(Session["userbranchID"]) + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=0, cnt_profession=0, cnt_organization='0', cnt_jobResponsibility=0, cnt_designation=0, cnt_industry=0, cnt_contactSource=0, cnt_referedBy='0', cnt_contactType='CL', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + DateTime.Now.ToShortDateString() + "',cnt_rating='0',cnt_reason='0',cnt_bloodgroup='0',WebLogIn='No',PassWord='', lastModifyDate ='" + DateTime.Now.ToShortDateString() + "',cnt_PlaceOfIncorporation='0',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='" + DateTime.Now.ToShortDateString() + "',cnt_OtherOccupation='',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + creditDays + "' ,cnt_CreditLimit='" + CreditLimit + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_AssociatedEmp= '" + hidAssociatedEmp.Value + "',cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',Is_TCSApplicable='" + Convert.ToBoolean(TCSApplicable.Value) + "'";
                oDBEngine.SetFieldValue("tbl_master_contact", value, "cnt_internalId = '" + KeyVal_InternalID.Value + "'");
                //end of rev srijeeta
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Updated Succesfully.')</script>");
            }
        }


        protected static void UpdateUniqueId(string internalId, int contactType, string uniqueID)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SetCustomerUniqueIdToCorrespondence");
            proc.AddVarcharPara("@cnt_internalId", 10, internalId);
            proc.AddIntegerPara("@cnt_IdType", contactType);
            proc.AddVarcharPara("@uniqueId", 80, uniqueID);
            proc.RunActionQuery();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["formtype"] != null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>parent.editwin.close();</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript2", "<script>parent.editwin.close();</script>");
            }
        }
        protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
        {
        }


        public int DeleteContactDetails(string InternalId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_DeleteContactInsert");
            proc.AddVarcharPara("@InternalId", 200, InternalId);
            proc.RunActionQuery();
            return 1;
        }

        [WebMethod]
        public static string CheckUniqueName(string clientName, string procode)
        {
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            bool IsPresent = false;
            string ContType = "CL";
            string entityName = "";
            if (procode == "0")
            {
                IsPresent = mshort.CheckUniqueWithtypeContactMaster(clientName, procode, "MasterContactType", ContType, ref entityName);
            }
            else
            {
                IsPresent = mshort.CheckUniqueWithtypeContactMaster(clientName, procode, "Mastercustomerclient", ContType, ref entityName);
            }


            return Convert.ToString(IsPresent) + "~" + entityName;
        }

        [WebMethod]
        public static bool CheckUniqueNumberingCode(string uccName, string Type)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(uccName, "0", Type);
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }


        [WebMethod]
        public static List<string> GetMainAccountList(string reqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            // DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");

            // DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name, MainAccount_AccountCode ", " MainAccount_AccountCode not like 'SYS%'");
            ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            proc.AddVarcharPara("@action", 20, "GetMainAccount");
            DT = proc.GetTable();

            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["MainAccount_Name"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
            }
            return obj;
        }
        [WebMethod]
        public static List<string> GetrefBy(string query)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable(query);
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["cnt_firstName"]) + "|" + Convert.ToString(dr["cnt_internalid"]));
            }
            return obj;

        }



        #region NewCustomer web method



        [WebMethod]
        public static object GetAddressdetails(string pinCode)
        {
            if (pinCode.Trim() != "")
            {

                DataTable fetchTable = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetSetCustomerPopup");
                proc.AddVarcharPara("@action", 500, "GETPINDETAILS");
                proc.AddVarcharPara("@pin", 6, pinCode);
                fetchTable = proc.GetTable();


                if (fetchTable.Rows.Count > 0)
                {
                    string country, state, city;
                    country = Convert.ToString(fetchTable.Rows[0]["cou_country"]);
                    state = Convert.ToString(fetchTable.Rows[0]["state"]);
                    city = Convert.ToString(fetchTable.Rows[0]["city_name"]);
                    string rreturnString = country + "||" + state + "||" + city;

                    var storiesObj = new { status = "Ok", Country = country, state = state, city = city };
                    return storiesObj;

                }
            }
            var storiesObj1 = new { status = "Not Found" };
            return storiesObj1;
        }




        [WebMethod]
        public static object GetAddressdetailsForNewBillShipp(string pinCode)
        {
            if (pinCode.Trim() != "")
            {

                DataTable fetchTable = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetSetCustomerPopup");
                proc.AddVarcharPara("@action", 500, "GetPinDetailsForBillShipp");
                proc.AddVarcharPara("@pin", 6, pinCode);
                fetchTable = proc.GetTable();


                if (fetchTable.Rows.Count > 0)
                {
                    string country, state, city, country_id, state_Id, city_id;
                    country = Convert.ToString(fetchTable.Rows[0]["cou_country"]);
                    state = Convert.ToString(fetchTable.Rows[0]["state"]);
                    city = Convert.ToString(fetchTable.Rows[0]["city_name"]);
                    country_id = Convert.ToString(fetchTable.Rows[0]["cou_id"]);
                    state_Id = Convert.ToString(fetchTable.Rows[0]["id"]);
                    city_id = Convert.ToString(fetchTable.Rows[0]["city_id"]);
                    string rreturnString = country + "||" + state + "||" + city + "||" + country_id + "||" + state_Id + "||" + city_id;

                    var storiesObj = new { status = "Ok", Country = country, state = state, city = city, Cou_id = country_id, state_id = state_Id, City_Id = city_id };
                    return storiesObj;

                }
            }
            var storiesObj1 = new { status = "Not Found" };
            return storiesObj1;
        }

        [WebMethod]
        public static object CheckuniqueId(string uniqueId)
        {
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            bool IsPresent = false;
            string entityName = "";
            IsPresent = mshort.CheckUniqueWithtypeContactMaster(uniqueId, "", "MasterContactType", "CL", ref entityName);
            return new { IsPresent = IsPresent, entityName = entityName };
        }

        [WebMethod]
        public static object VendorCheckuniqueId(string uniqueId)
        {
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            bool IsPresent = false;
            string entityName = "";
            IsPresent = mshort.CheckUniqueWithtypeContactMaster(uniqueId, "", "MasterContactType", "DV", ref entityName);
            return new { IsPresent = IsPresent, entityName = entityName };
        }

        public static bool IsBannedPAN(string pan)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            DataTable dtBannedPanCard = new DataTable();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //   dtBannedPanCard = oDBEngine.GetDataTable("master_bannedentity", " convert(varchar(10), bannedentity_OrderDate,105) as bannedentity_OrderDate,bannedentity_BanPeriod,bannedentity_NSECircularNumber ", "rtrim(ltrim(bannedentity_PAN)) ='" + pan.ToString().Trim() + "'");
            dtBannedPanCard = oDBEngine.GetDataTable("master_bannedentity", "'Circular No : '+BannedEntity_NSECircularNumber +'Order Date : '+ Convert(varchar(20),BannedEntity_OrderDate,105)+ ', Order Period : '+BannedEntity_BanPeriod as Msg", "rtrim(ltrim(bannedentity_PAN)) ='" + Convert.ToString(pan.Trim()) + "'");
            if (dtBannedPanCard != null && dtBannedPanCard.Rows.Count > 0)
            {

                return true;
            }
            else
            {

                return false;
            }

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }


        public static bool checkPANExistance(string Pan)
        {
            bool isPanExists = false;

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string pann = Convert.ToString(Pan);

            DataTable dtPann = new DataTable();

            dtPann = oGenericMethod.GetDataTable("tbl_master_contactRegistration", "crg_cntId", "crg_number='" + pann + "'");

            if (dtPann != null && dtPann.Rows.Count > 0)
            {

                isPanExists = true;

            }
            else
            {
                isPanExists = false;
            }

            return isPanExists;

        }



        [WebMethod]
        //rev srijeeta
        //public static object SaveCustomer(string UniqueID, string Name, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2,
        //    string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson, string IdTypeval, string GrpCust = null
        //    , int NumberingId = 0, bool TCSApplicable = false, string PANValue = null, String TransactionCategory = null, string DocumentSegments = null
        //    , string Segment1 = null, string Segment2 = null, string Segment3 = null, string Segment4 = null, string Segment5 = null
        //    , string SegmentUsedFor1 = null, string SegmentUsedFor2 = null, string SegmentUsedFor3 = null, string SegmentUsedFor4 = null, string SegmentUsedFor5 = null
        //    , string SegmentMandatory1 = null, string SegmentMandatory2 = null, string SegmentMandatory3 = null, string SegmentMandatory4 = null, string SegmentMandatory5 = null
        //   , string ServiceBranch = null
        //    )
        public static object SaveCustomer(string UniqueID,string Alternative_Code, string Name, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2,
            string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson, string IdTypeval, string GrpCust = null
            , int NumberingId = 0, bool TCSApplicable = false, string PANValue = null, String TransactionCategory = null, string DocumentSegments = null
            , string Segment1 = null, string Segment2 = null, string Segment3 = null, string Segment4 = null, string Segment5 = null
            , string SegmentUsedFor1 = null, string SegmentUsedFor2 = null, string SegmentUsedFor3 = null, string SegmentUsedFor4 = null, string SegmentUsedFor5 = null
            , string SegmentMandatory1 = null, string SegmentMandatory2 = null, string SegmentMandatory3 = null, string SegmentMandatory4 = null, string SegmentMandatory5 = null
           , string ServiceBranch = null
            )
            //end of rev srijeeta
        {
            BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            CommonBL ComBL = new CommonBL();
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            string SyncCustomertoFSMWhileCreating = ComBL.GetSystemSettingsResult("SyncCustomertoFSMWhileCreating");
            string duplicate_PAN_Number = ComBL.GetSystemSettingsResult("duplicate_PAN_Number");
            //Rev Tanmoy settings wise lead save
            string CreateLeadfromAddOnFlyCustomer = ComBL.GetSystemSettingsResult("CreateLeadfromAddOnFlyCustomer");
            //Rev Tanmoy settings wise lead save End
            try
            {
                int NumId = 0;
                NumId = Convert.ToInt32(NumberingId);
                string entityName = "";
                if (GSTIN != "")
                {
                    string DuplicateGSTINCustomer = ComBL.GetSystemSettingsResult("DuplicateGSTINCustomer");
                    if (!String.IsNullOrEmpty(DuplicateGSTINCustomer))
                    {
                        if (DuplicateGSTINCustomer == "No")
                        {
                            if (ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "CL"))
                            {
                                return new { status = "DuplicateGSTIN", Msg = "Duplicate GSTIN" };
                            }
                        }
                    }
                }
                if (Convert.ToString(PANValue) != "")
                {
                    if (IsBannedPAN(PANValue) == true)
                    {
                        return new { status = "BannedPAN", Msg = "This PAN is banned by SEBI." };
                    }
                    if (duplicate_PAN_Number.ToUpper().ToString() != "YES")
                    {
                        if (checkPANExistance(PANValue) == true)
                        {
                            return new { status = "DuplicatePAN", Msg = "Duplicate PAN number is not allowed." };
                        }
                    }
                }


                if (!mshort.CheckUniqueWithtypeContactMaster(UniqueID, "", "MasterContactType", "LD", ref entityName) || !string.IsNullOrEmpty(Convert.ToString(NumId)))
                {
                    string InternalId = "";
                    if (CreateLeadfromAddOnFlyCustomer.ToUpper() == "NO")
                    {
                        //rev srijeeta
                        //InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, "1", //3
                        //                                    Name, "", "",//3	
                        //                                    "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3	
                        //                                    "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), //2	
                        //                                    new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//2	
                        //                                    "1", "1", "",//3	
                        //                                    "0", "0", "0",//3	CL
                        //                                    "1", "", "", "CL",//4	
                        //                                    "1", DateTime.Now, "1", "",//4	
                        //                                    "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5	
                        //                                    DateTime.Now, "", "78", false, 0,//5	
                        //                                    0, "A",//2	
                        //                                    GSTIN, "", "", "", "", NumId, "", "", "", TCSApplicable, PANValue, TransactionCategory, ServiceBranch);//
                        InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, Alternative_Code, "1", //3
                                                            Name, "", "",//3	
                                                            "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3	
                                                            "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), //2	
                                                            new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//2	
                                                            "1", "1", "",//3	
                                                            "0", "0", "0",//3	CL
                                                            "1", "", "", "CL",//4	
                                                            "1", DateTime.Now, "1", "",//4	
                                                            "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5	
                                                            DateTime.Now, "", "78", false, 0,//5	
                                                            0, "A",//2	
                                                            GSTIN, "", "", "", "", NumId, "", "", "", TCSApplicable, PANValue, TransactionCategory, ServiceBranch);//
                        //end of rev srijeeta
                        if (SyncCustomertoFSMWhileCreating.ToUpper() == "YES")
                        {
                            CustomerSync(InternalId);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(NumId)) && Convert.ToString(NumId) != "0")
                        {
                            //rev srijeeta
                            //InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, "1", //3
                            //                                Name, "", "",//3	
                            //                                "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3	
                            //                                "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), //2	
                            //                                new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//2	
                            //                                "1", "1", "",//3	
                            //                                "0", "0", "0",//3	CL
                            //                                "1", "", "", "CL",//4	
                            //                                "1", DateTime.Now, "1", "",//4	
                            //                                "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5	
                            //                                DateTime.Now, "", "78", false, 0,//5	
                            //                                0, "A",//2	
                            //                                GSTIN, "", "", "", "", NumId, "", "", "", TCSApplicable, PANValue, TransactionCategory, ServiceBranch);//
                            InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, Alternative_Code, "1", //3
                                                           Name, "", "",//3	
                                                           "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3	
                                                           "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), //2	
                                                           new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//2	
                                                           "1", "1", "",//3	
                                                           "0", "0", "0",//3	CL
                                                           "1", "", "", "CL",//4	
                                                           "1", DateTime.Now, "1", "",//4	
                                                           "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5	
                                                           DateTime.Now, "", "78", false, 0,//5	
                                                           0, "A",//2	
                                                           GSTIN, "", "", "", "", NumId, "", "", "", TCSApplicable, PANValue, TransactionCategory, ServiceBranch);//
                            //end of rev srijeeta
                            if (SyncCustomertoFSMWhileCreating.ToUpper() == "YES")
                            {
                                CustomerSync(InternalId);
                            }
                        }
                        else
                            //rev srijeeta
                            //InternalId = oContactGeneralBL.Insert_ContactGeneral("Lead", UniqueID, "1", //3
                            //                                      Name, "", "",//3
                            //                                      "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3
                            //                                      "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), //2
                            //                                      new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//2
                            //                                      "1", "1", "",//3
                            //                                      "0", "0", "0",//3
                            //                                      "1", "", "", "LD",//4
                            //                                      "1", DateTime.Now, "1", "",//4
                            //                                      "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5
                            //                                      DateTime.Now, "", "78", false, 0,//5
                            //                                      0, "A",//2
                            //                                      GSTIN, "", "", "", "", NumId, "", "", "", TCSApplicable, PANValue, TransactionCategory, ServiceBranch);//3
                            InternalId = oContactGeneralBL.Insert_ContactGeneral("Lead", UniqueID, Alternative_Code, "1", //3
                                                                  Name, "", "",//3
                                                                  "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3
                                                                  "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), //2
                                                                  new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//2
                                                                  "1", "1", "",//3
                                                                  "0", "0", "0",//3
                                                                  "1", "", "", "LD",//4
                                                                  "1", DateTime.Now, "1", "",//4
                                                                  "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5
                                                                  DateTime.Now, "", "78", false, 0,//5
                                                                  0, "A",//2
                                                                  GSTIN, "", "", "", "", NumId, "", "", "", TCSApplicable, PANValue, TransactionCategory, ServiceBranch);//3
                        //end of rev srijeeta
                    }


                    if (InternalId != "")
                    {

                        if (!string.IsNullOrEmpty(DocumentSegments))
                        {
                            string[] _DocumentSegment = DocumentSegments.Split('~');

                            ProcedureExecute proc;
                            string rtrnvalue = "";
                            try
                            {
                                using (proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP"))
                                {
                                    proc.AddVarcharPara("@action", 50, "InsertENTITY_SEGMENT_MAP");
                                    proc.AddVarcharPara("@InetrnalId", 50, InternalId);

                                    //proc.AddVarcharPara("@Segment1", 100, _DocumentSegment[0]);
                                    //proc.AddVarcharPara("@Segment2", 100, _DocumentSegment[1]);
                                    //proc.AddVarcharPara("@Segment3", 100, _DocumentSegment[2]);
                                    //proc.AddVarcharPara("@Segment4", 100, _DocumentSegment[3]);
                                    //proc.AddVarcharPara("@Segment5", 100, _DocumentSegment[4]);

                                    proc.AddVarcharPara("@Segment1", 100, Segment1);
                                    proc.AddVarcharPara("@Segment2", 100, Segment2);
                                    proc.AddVarcharPara("@Segment3", 100, Segment3);
                                    proc.AddVarcharPara("@Segment4", 100, Segment4);
                                    proc.AddVarcharPara("@Segment5", 100, Segment1);


                                    proc.AddVarcharPara("@SegmentUsedFor1", 100, SegmentUsedFor1);
                                    proc.AddVarcharPara("@SegmentUsedFor2", 100, SegmentUsedFor2);
                                    proc.AddVarcharPara("@SegmentUsedFor3", 100, SegmentUsedFor3);
                                    proc.AddVarcharPara("@SegmentUsedFor4", 100, SegmentUsedFor4);
                                    proc.AddVarcharPara("@SegmentUsedFor5", 100, SegmentUsedFor5);

                                    proc.AddVarcharPara("@SegmentMandatory1", 100, SegmentMandatory1);
                                    proc.AddVarcharPara("@SegmentMandatory2", 100, SegmentMandatory2);
                                    proc.AddVarcharPara("@SegmentMandatory3", 100, SegmentMandatory3);
                                    proc.AddVarcharPara("@SegmentMandatory4", 100, SegmentMandatory4);
                                    proc.AddVarcharPara("@SegmentMandatory5", 100, SegmentMandatory5);

                                    int i = proc.RunActionQuery();

                                }
                            }

                            catch (Exception ex)
                            {
                                throw ex;
                            }

                            finally
                            {
                                proc = null;
                            }
                        }
                    }
                    DataTable dts = new DataTable();
                    dts = oDBEngine.GetDataTable("select isnull(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalId + "'");
                    if (dts.Rows.Count == 1)
                    {
                        if (Convert.ToString(dts.Rows[0]["cnt_UCC"]) == "Auto")
                        {
                            ProcedureExecute pr = new ProcedureExecute("Prc_DeleteContactInsert");
                            pr.AddVarcharPara("@InternalId", 200, InternalId);
                            pr.RunActionQuery();


                            //if (hdnAutoNumStg.Value == "LDAutoNum1")
                            //{
                            //    txt_CustDocNo.Text = "Auto";
                            //    txt_CustDocNo.ClientEnabled = false;
                            //}
                            //else
                            //{
                            //    txt_CustDocNo.Text = "Auto";
                            //    txt_CustDocNo.ClientEnabled = false;
                            //}
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Either Document Number Exists OR Document Number Exhausted.')", true);
                        }
                    }
                    if (dts.Rows.Count == 1 && Convert.ToString(dts.Rows[0]["cnt_UCC"]) != "Auto")
                    {
                        //Mantis Issue 24201 changed address fields length from 100 to 500
                        ProcedureExecute proc = new ProcedureExecute("prc_GetSetCustomerPopup");
                        proc.AddVarcharPara("@action", 500, "SaveBillingShippingWithType");
                        proc.AddVarcharPara("@pin", 10, BillingPin);
                        proc.AddVarcharPara("@billingAddress1", 500, BillingAddress1);
                        proc.AddVarcharPara("@billingAddress2", 500, BillingAddress2);
                        proc.AddVarcharPara("@shippingpin", 10, shippingPin);
                        proc.AddVarcharPara("@shippingbillingAddress1", 500, shippingAddress1);
                        proc.AddVarcharPara("@shippingbillingAddress2", 500, shippingAddress2);
                        proc.AddVarcharPara("@customerInternalId", 10, InternalId);
                        proc.AddVarcharPara("@cntUcc", 50, UniqueID);
                        proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                        proc.AddVarcharPara("@BillingPhone", 20, BillingPhone);
                        proc.AddVarcharPara("@ShippingPhone", 20, ShippingPhone);
                        proc.AddVarcharPara("@contactperson", 40, contactperson);
                        proc.AddIntegerPara("@cnt_IdType", Convert.ToInt32(IdTypeval));
                        proc.AddIntegerNullPara("@BillCountry", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@BillState", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@BillCity", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@Billpin", QueryParameterDirection.Output);

                        proc.AddIntegerNullPara("@ShipCountry", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@ShipState", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@ShipCity", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@Shippin", QueryParameterDirection.Output);


                        proc.RunActionQuery();



                        int billCountry = Convert.ToInt32(proc.GetParaValue("@BillCountry"));
                        int billState = Convert.ToInt32(proc.GetParaValue("@BillState"));
                        int billcity = Convert.ToInt32(proc.GetParaValue("@BillCity"));
                        int billpin = Convert.ToInt32(proc.GetParaValue("@Billpin"));


                        int ShipCountry = Convert.ToInt32(proc.GetParaValue("@ShipCountry"));
                        int ShipState = Convert.ToInt32(proc.GetParaValue("@ShipState"));
                        int Shipcity = Convert.ToInt32(proc.GetParaValue("@ShipCity"));
                        int Shippin = Convert.ToInt32(proc.GetParaValue("@Shippin"));

                        //Billig Details
                        DataTable AddressDetaildt = new DataTable();

                        AddressDetaildt.Columns.Add("QuoteAdd_QuoteId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_CompanyID", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_BranchId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_FinYear", typeof(System.String));

                        AddressDetaildt.Columns.Add("QuoteAdd_ContactPerson", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_addressType", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_address1", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_address2", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_address3", typeof(System.String));


                        AddressDetaildt.Columns.Add("QuoteAdd_landMark", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_countryId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_stateId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_cityId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_areaId", typeof(System.Int32));


                        AddressDetaildt.Columns.Add("QuoteAdd_pin", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_CreatedDate", typeof(System.DateTime));
                        AddressDetaildt.Columns.Add("QuoteAdd_CreatedUser", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_LastModifyDate", typeof(System.DateTime));
                        AddressDetaildt.Columns.Add("QuoteAdd_LastModifyUser", typeof(System.Int32));

                        DataRow addressRow = AddressDetaildt.NewRow();

                        addressRow["QuoteAdd_QuoteId"] = 0;
                        addressRow["QuoteAdd_CompanyID"] = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        addressRow["QuoteAdd_BranchId"] = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        addressRow["QuoteAdd_FinYear"] = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                        addressRow["QuoteAdd_ContactPerson"] = "";
                        addressRow["QuoteAdd_addressType"] = "Billing";
                        addressRow["QuoteAdd_address1"] = BillingAddress1;
                        addressRow["QuoteAdd_address2"] = BillingAddress2;
                        addressRow["QuoteAdd_address3"] = "";
                        addressRow["QuoteAdd_landMark"] = "";

                        addressRow["QuoteAdd_countryId"] = billCountry;
                        addressRow["QuoteAdd_stateId"] = billState;
                        addressRow["QuoteAdd_cityId"] = billcity;
                        addressRow["QuoteAdd_areaId"] = 0;
                        addressRow["QuoteAdd_pin"] = billpin;
                        addressRow["QuoteAdd_CreatedDate"] = DateTime.Now;
                        addressRow["QuoteAdd_CreatedUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        addressRow["QuoteAdd_LastModifyDate"] = DateTime.Now;
                        addressRow["QuoteAdd_LastModifyUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        AddressDetaildt.Rows.Add(addressRow);



                        addressRow = AddressDetaildt.NewRow();
                        addressRow["QuoteAdd_QuoteId"] = 0;
                        addressRow["QuoteAdd_CompanyID"] = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        addressRow["QuoteAdd_BranchId"] = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        addressRow["QuoteAdd_FinYear"] = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                        addressRow["QuoteAdd_ContactPerson"] = "";
                        addressRow["QuoteAdd_addressType"] = "Shipping";
                        addressRow["QuoteAdd_address1"] = shippingAddress1;
                        addressRow["QuoteAdd_address2"] = shippingAddress2;
                        addressRow["QuoteAdd_address3"] = "";
                        addressRow["QuoteAdd_landMark"] = "";

                        addressRow["QuoteAdd_countryId"] = ShipCountry;
                        addressRow["QuoteAdd_stateId"] = ShipState;
                        addressRow["QuoteAdd_cityId"] = Shipcity;
                        addressRow["QuoteAdd_areaId"] = 0;
                        addressRow["QuoteAdd_pin"] = Shippin;
                        addressRow["QuoteAdd_CreatedDate"] = DateTime.Now;
                        addressRow["QuoteAdd_CreatedUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        addressRow["QuoteAdd_LastModifyDate"] = DateTime.Now;
                        addressRow["QuoteAdd_LastModifyUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        AddressDetaildt.Rows.Add(addressRow);
                        HttpContext.Current.Session["SI_QuotationAddressDtl"] = AddressDetaildt;

                        DataTable StateDetails = oDBEngine.GetDataTable("select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + billState + " union all select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + ShipState);
                        string BillingStateText = "", BillingStateCode = "", ShippingStateText = "", ShippingStateCode = "";
                        if (StateDetails.Rows.Count > 0)
                        {
                            BillingStateText = Convert.ToString(StateDetails.Rows[0]["stateText"]);
                            BillingStateCode = Convert.ToString(StateDetails.Rows[0]["id"]);

                            ShippingStateText = Convert.ToString(StateDetails.Rows[1]["stateText"]);
                            ShippingStateCode = Convert.ToString(StateDetails.Rows[1]["id"]);
                        }
                        if (GrpCust != "0")
                        {
                            int a = oDBEngine.ExeInteger("insert into tbl_trans_group(grp_contactId,grp_groupMaster,grp_groupType,CreateDate,CreateUser) values('" + InternalId + "','" + GrpCust + "','Customers','" + CreateDate + "','" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "')");
                        }

                        oDBEngine.GetDataTable("update tbl_Master_Contact SET cnt_ContactType='CL' where cnt_InternalId='" + Convert.ToString(InternalId) + "'");




                        return new { status = "Ok", InternalId = InternalId, BillingStateText = BillingStateText, BillingStateCode = BillingStateCode, ShippingStateText = ShippingStateText, ShippingStateCode = ShippingStateCode };
                    }
                    else
                    {
                        return new { status = "AUTOError" };
                    }




                }
                else
                {
                    return new { status = "Error", Msg = "Already Exists" };
                }



            }
            catch (Exception ex)
            {
                return new { status = "Error", Msg = ex.Message };
            }
        }

        [WebMethod]
        public static object SaveCustomerMaster(string UniqueID, string Name, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2, string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson, string IdTypeval, string GrpCust = null)
        {
            BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            try
            {
                string entityName = "";
                if (GSTIN != "")
                {
                    if (ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "CL"))
                    {
                        return new { status = "DuplicateGSTIN", Msg = "Duplicate GSTIN" };
                    }
                }

                if (!mshort.CheckUniqueWithtypeContactMaster(UniqueID, "", "MasterContactType", "CL", ref entityName))
                {
                    //rev srijeeta
                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, "1",
                    //                                Name, "", "",
                    //                                "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",
                    //                                "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",
                    //                                "1", "1", "",
                    //                                "0", "0", "0",
                    //                                "1", "", "", "CL",
                    //                                "1", DateTime.Now, "1", "",
                    //                                "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                    //                                DateTime.Now, "", "78", false, 0,
                    //                                0, "A",
                    //                                "", "");
                    string InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, "1","1",
                                                    Name, "", "",
                                                    "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",
                                                    "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",
                                                    "1", "1", "",
                                                    "0", "0", "0",
                                                    "1", "", "", "CL",
                                                    "1", DateTime.Now, "1", "",
                                                    "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                                                    DateTime.Now, "", "78", false, 0,
                                                    0, "A",
                                                    "", "");
                    //end of rev srijeeta



                    //Mantis Issue 24201 changed address fields length from 100 to 500
                    ProcedureExecute proc = new ProcedureExecute("prc_GetSetCustomerPopup_Lightwet");
                    proc.AddVarcharPara("@action", 500, "SaveBillingShipping");
                    proc.AddVarcharPara("@pin", 10, BillingPin);
                    proc.AddVarcharPara("@billingAddress1", 500, BillingAddress1);
                    proc.AddVarcharPara("@billingAddress2", 500, BillingAddress2);
                    proc.AddVarcharPara("@shippingpin", 10, shippingPin);
                    proc.AddVarcharPara("@shippingbillingAddress1", 500, shippingAddress1);
                    proc.AddVarcharPara("@shippingbillingAddress2", 500, shippingAddress2);
                    proc.AddVarcharPara("@customerInternalId", 10, InternalId);
                    proc.AddVarcharPara("@cntUcc", 50, UniqueID);
                    proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                    proc.AddVarcharPara("@BillingPhone", 20, BillingPhone);
                    proc.AddVarcharPara("@ShippingPhone", 20, ShippingPhone);
                    proc.AddVarcharPara("@contactperson", 40, contactperson);

                    proc.AddIntegerPara("@cnt_IdType", Convert.ToInt32(IdTypeval));

                    proc.AddIntegerNullPara("@BillCountry", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@BillState", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@BillCity", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@Billpin", QueryParameterDirection.Output);

                    proc.AddIntegerNullPara("@ShipCountry", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@ShipState", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@ShipCity", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@Shippin", QueryParameterDirection.Output);


                    proc.RunActionQuery();



                    int billCountry = Convert.ToInt32(proc.GetParaValue("@BillCountry"));
                    int billState = Convert.ToInt32(proc.GetParaValue("@BillState"));
                    int billcity = Convert.ToInt32(proc.GetParaValue("@BillCity"));
                    int billpin = Convert.ToInt32(proc.GetParaValue("@Billpin"));


                    int ShipCountry = Convert.ToInt32(proc.GetParaValue("@ShipCountry"));
                    int ShipState = Convert.ToInt32(proc.GetParaValue("@ShipState"));
                    int Shipcity = Convert.ToInt32(proc.GetParaValue("@ShipCity"));
                    int Shippin = Convert.ToInt32(proc.GetParaValue("@Shippin"));



                    DataTable StateDetails = oDBEngine.GetDataTable("select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + billState + " union all select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + ShipState);
                    string BillingStateText = "", BillingStateCode = "", ShippingStateText = "", ShippingStateCode = "";
                    if (StateDetails.Rows.Count > 0)
                    {
                        BillingStateText = Convert.ToString(StateDetails.Rows[0]["stateText"]);
                        BillingStateCode = Convert.ToString(StateDetails.Rows[0]["id"]);

                        ShippingStateText = Convert.ToString(StateDetails.Rows[1]["stateText"]);
                        ShippingStateCode = Convert.ToString(StateDetails.Rows[1]["id"]);
                    }

                    if (GrpCust != "0")
                    {
                        int a = oDBEngine.ExeInteger("insert into tbl_trans_group(grp_contactId,grp_groupMaster,grp_groupType,CreateDate,CreateUser) values('" + InternalId + "','" + GrpCust + "','Customers','" + CreateDate + "','" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "')");
                    }
                    return new { status = "Ok", InternalId = InternalId, BillingStateText = BillingStateText, BillingStateCode = BillingStateCode, ShippingStateText = ShippingStateText, ShippingStateCode = ShippingStateCode };
                }
                else
                {
                    return new { status = "Error", Msg = "Already Exists" };
                }

            }
            catch (Exception ex)
            {
                return new { status = "Error", Msg = ex.Message };
            }

        }

        [WebMethod]
        public static object SaveVendorMaster(string UniqueID, string Name, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2, string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson, string Vendortype, string addtypeVal, string accountgroupval)
        {
            BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


            try
            {
                string entityName = "";
                if (GSTIN != "")
                {
                    if (ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "DV"))
                    {
                        return new { status = "DuplicateGSTIN", Msg = "Duplicate GSTIN" };
                    }
                }

                if (!mshort.CheckUniqueWithtypeContactMaster(UniqueID, "", "MasterContactType", "DV", ref entityName))
                {
                    //rev srijeeta
                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral("Vendor", UniqueID, "1",
                    //                                    Name, "", "",
                    //                                    UniqueID, "0", "2",
                    //                                    "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",
                    //                                    "1", "1", "",
                    //                                    "0", "0", "0",
                    //                                    "1", "", "", "DV",
                    //                                    "1", DateTime.Now, "1", "",
                    //                                    "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                    //                                    DateTime.Now, "", "78", false, 0,
                    //                                    0, "A",
                    //                                    "", "");
                    string InternalId = oContactGeneralBL.Insert_ContactGeneral("Vendor", UniqueID, "1","1",
                                                       Name, "", "",
                                                       UniqueID, "0", "2",
                                                       "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",
                                                       "1", "1", "",
                                                       "0", "0", "0",
                                                       "1", "", "", "DV",
                                                       "1", DateTime.Now, "1", "",
                                                       "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                                                       DateTime.Now, "", "78", false, 0,
                                                       0, "A",
                                                       "", "");
                    //end rev srijeeta


                    RemarkCategoryBL reCat = new RemarkCategoryBL();
                    int brmap = reCat.insertVendorBranchMap("", InternalId, 0);
                    Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_EntityType='" + Vendortype + "',AccountGroupID='" + accountgroupval + "'", " cnt_internalId='" + InternalId + "'");

                    //Mantis Issue 0024200 changed address fields length from 100 to 500
                    ProcedureExecute proc = new ProcedureExecute("prc_GetSetVendorPopup");
                    proc.AddVarcharPara("@action", 500, "SaveBillingShipping");
                    proc.AddVarcharPara("@pin", 10, BillingPin);
                    proc.AddVarcharPara("@billingAddress1", 500, BillingAddress1);
                    proc.AddVarcharPara("@billingAddress2", 500, BillingAddress2);
                    proc.AddVarcharPara("@shippingpin", 10, shippingPin);
                    proc.AddVarcharPara("@shippingbillingAddress1", 500, shippingAddress1);
                    proc.AddVarcharPara("@shippingbillingAddress2", 500, shippingAddress2);
                    proc.AddVarcharPara("@customerInternalId", 10, InternalId);
                    proc.AddVarcharPara("@cntUcc", 50, UniqueID);
                    proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                    proc.AddVarcharPara("@BillingPhone", 20, BillingPhone);
                    proc.AddVarcharPara("@ShippingPhone", 20, ShippingPhone);
                    proc.AddVarcharPara("@contactperson", 40, contactperson);
                    proc.AddVarcharPara("@AddressType", 40, addtypeVal);
                    proc.AddIntegerNullPara("@BillCountry", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@BillState", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@BillCity", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@Billpin", QueryParameterDirection.Output);
                    proc.RunActionQuery();



                    int billCountry = Convert.ToInt32(proc.GetParaValue("@BillCountry"));
                    int billState = Convert.ToInt32(proc.GetParaValue("@BillState"));
                    int billcity = Convert.ToInt32(proc.GetParaValue("@BillCity"));
                    int billpin = Convert.ToInt32(proc.GetParaValue("@Billpin"));




                    DataTable StateDetails = oDBEngine.GetDataTable("select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + billState);

                    string BillingStateText = "", BillingStateCode = "";
                    if (StateDetails.Rows.Count > 0)
                    {
                        BillingStateText = Convert.ToString(StateDetails.Rows[0]["stateText"]);
                        BillingStateCode = Convert.ToString(StateDetails.Rows[0]["id"]);
                    }


                    return new { status = "Ok", InternalId = InternalId, BillingStateText = BillingStateText, BillingStateCode = BillingStateCode, ShippingStateText = "", ShippingStateCode = "" };
                }
                else
                {
                    return new { status = "Error", Msg = "Already Exists" };
                }

            }
            catch (Exception ex)
            {
                return new { status = "Error", Msg = ex.Message };
            }
        }


        [WebMethod]
        //rev srijeeta
        //public static object SaveVendorLightwet(string UniqueID, string Name, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2, string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson, string IdTypeval, string GrpCust = null, int NumberingId = 0
        //    , String TransactionCategory = "0",string ProductIDs=null)
        public static object SaveVendorLightwet(string UniqueID, string Alternative_Code,string Name, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2, string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson, string IdTypeval, string GrpCust = null, int NumberingId = 0
            , String TransactionCategory = "0", string ProductIDs = null)
            //end of rev srijeeta
        {
            BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            try
            {
                string entityName = "";
                if (GSTIN != "")
                {
                    if (ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "DV"))
                    {
                        return new { status = "DuplicateGSTIN", Msg = "Duplicate GSTIN" };
                    }
                }

                if (!mshort.CheckUniqueWithtypeContactMaster(UniqueID, "", "MasterContactType", "DV", ref entityName) || !string.IsNullOrEmpty(Convert.ToString(NumberingId)))
                {
                    //rev srijeeta
                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral("Data Vendor", UniqueID, "1",   //3
                    //                                    Name, "", "",//3
                    //                                    UniqueID, Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3
                    //                                    "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//4
                    //                                    "1", "1", "",//3
                    //                                    "0", "0", "0",//3
                    //                                    "1", "", "", "DV",///4
                    //                                    "1", DateTime.Now, "1", "",//4
                    //                                    "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5
                    //                                    DateTime.Now, "", "78", false, 0,//5
                    //                                    0, "A",//2
                    //                                    GSTIN, "", "", "", "", NumberingId, "", "", "", false, null, TransactionCategory,null, ProductIDs);//6
                    string InternalId = oContactGeneralBL.Insert_ContactGeneral("Data Vendor", UniqueID, Alternative_Code, "1",   //3
                                                        Name, "", "",//3
                                                        UniqueID, Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3
                                                        "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//4
                                                        "1", "1", "",//3
                                                        "0", "0", "0",//3
                                                        "1", "", "", "DV",///4
                                                        "1", DateTime.Now, "1", "",//4
                                                        "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5
                                                        DateTime.Now, "", "78", false, 0,//5
                                                        0, "A",//2
                                                        GSTIN, "", "", "", "", NumberingId, "", "", "", false, null, TransactionCategory, null, ProductIDs);//6
                    //end of rev srijeeta


                    DataTable dts = new DataTable();
                    dts = oDBEngine.GetDataTable("select isnull(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalId + "'");
                    if (dts.Rows.Count == 1)
                    {
                        if (Convert.ToString(dts.Rows[0]["cnt_UCC"]) == "Auto")
                        {
                            ProcedureExecute pr = new ProcedureExecute("Prc_DeleteVendorContactInsert");
                            pr.AddVarcharPara("@InternalId", 200, InternalId);
                            pr.RunActionQuery();


                            //if (hdnAutoNumStg.Value == "LDAutoNum1")
                            //{
                            //    txt_CustDocNo.Text = "Auto";
                            //    txt_CustDocNo.ClientEnabled = false;
                            //}
                            //else
                            //{
                            //    txt_CustDocNo.Text = "Auto";
                            //    txt_CustDocNo.ClientEnabled = false;
                            //}
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Either Document Number Exists OR Document Number Exhausted.')", true);
                        }
                    }

                    if (dts.Rows.Count == 1 && Convert.ToString(dts.Rows[0]["cnt_UCC"]) != "Auto")
                    {
                       // Mantis Issue 0024200 changed address fields length from 100 to 500
                        ProcedureExecute proc = new ProcedureExecute("prc_GetSetCustomerPopup");
                        proc.AddVarcharPara("@action", 500, "SaveVendorBillingShipping");
                        proc.AddVarcharPara("@pin", 10, BillingPin);
                        proc.AddVarcharPara("@billingAddress1", 500, BillingAddress1);
                        proc.AddVarcharPara("@billingAddress2", 500, BillingAddress2);
                        proc.AddVarcharPara("@shippingpin", 10, shippingPin);
                        proc.AddVarcharPara("@shippingbillingAddress1", 500, shippingAddress1);
                        proc.AddVarcharPara("@shippingbillingAddress2", 500, shippingAddress2);
                        proc.AddVarcharPara("@customerInternalId", 10, InternalId);
                        proc.AddVarcharPara("@cntUcc", 50, UniqueID);
                        //rev srijeeta
                        proc.AddVarcharPara("@Alternative_Code", 100, Alternative_Code);
                        //end of rev srijeeta
                        proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                        proc.AddVarcharPara("@BillingPhone", 20, BillingPhone);
                        proc.AddVarcharPara("@ShippingPhone", 20, ShippingPhone);
                        proc.AddVarcharPara("@contactperson", 40, contactperson);
                        proc.AddIntegerPara("@cnt_IdType", Convert.ToInt32(IdTypeval));
                        proc.AddIntegerPara("@UserId", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                        proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                        proc.AddIntegerNullPara("@BillCountry", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@BillState", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@BillCity", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@Billpin", QueryParameterDirection.Output);

                        proc.AddIntegerNullPara("@ShipCountry", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@ShipState", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@ShipCity", QueryParameterDirection.Output);
                        proc.AddIntegerNullPara("@Shippin", QueryParameterDirection.Output);


                        proc.RunActionQuery();



                        int billCountry = Convert.ToInt32(proc.GetParaValue("@BillCountry"));
                        int billState = Convert.ToInt32(proc.GetParaValue("@BillState"));
                        int billcity = Convert.ToInt32(proc.GetParaValue("@BillCity"));
                        int billpin = Convert.ToInt32(proc.GetParaValue("@Billpin"));


                        int ShipCountry = Convert.ToInt32(proc.GetParaValue("@ShipCountry"));
                        int ShipState = Convert.ToInt32(proc.GetParaValue("@ShipState"));
                        int Shipcity = Convert.ToInt32(proc.GetParaValue("@ShipCity"));
                        int Shippin = Convert.ToInt32(proc.GetParaValue("@Shippin"));

                        //Billig Details
                        DataTable AddressDetaildt = new DataTable();

                        AddressDetaildt.Columns.Add("QuoteAdd_QuoteId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_CompanyID", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_BranchId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_FinYear", typeof(System.String));

                        AddressDetaildt.Columns.Add("QuoteAdd_ContactPerson", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_addressType", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_address1", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_address2", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_address3", typeof(System.String));


                        AddressDetaildt.Columns.Add("QuoteAdd_landMark", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_countryId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_stateId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_cityId", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_areaId", typeof(System.Int32));


                        AddressDetaildt.Columns.Add("QuoteAdd_pin", typeof(System.String));
                        AddressDetaildt.Columns.Add("QuoteAdd_CreatedDate", typeof(System.DateTime));
                        AddressDetaildt.Columns.Add("QuoteAdd_CreatedUser", typeof(System.Int32));
                        AddressDetaildt.Columns.Add("QuoteAdd_LastModifyDate", typeof(System.DateTime));
                        AddressDetaildt.Columns.Add("QuoteAdd_LastModifyUser", typeof(System.Int32));

                        DataRow addressRow = AddressDetaildt.NewRow();

                        addressRow["QuoteAdd_QuoteId"] = 0;
                        addressRow["QuoteAdd_CompanyID"] = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        addressRow["QuoteAdd_BranchId"] = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        addressRow["QuoteAdd_FinYear"] = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                        addressRow["QuoteAdd_ContactPerson"] = "";
                        addressRow["QuoteAdd_addressType"] = "Billing";
                        addressRow["QuoteAdd_address1"] = BillingAddress1;
                        addressRow["QuoteAdd_address2"] = BillingAddress2;
                        addressRow["QuoteAdd_address3"] = "";
                        addressRow["QuoteAdd_landMark"] = "";

                        addressRow["QuoteAdd_countryId"] = billCountry;
                        addressRow["QuoteAdd_stateId"] = billState;
                        addressRow["QuoteAdd_cityId"] = billcity;
                        addressRow["QuoteAdd_areaId"] = 0;
                        addressRow["QuoteAdd_pin"] = billpin;
                        addressRow["QuoteAdd_CreatedDate"] = DateTime.Now;
                        addressRow["QuoteAdd_CreatedUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        addressRow["QuoteAdd_LastModifyDate"] = DateTime.Now;
                        addressRow["QuoteAdd_LastModifyUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        AddressDetaildt.Rows.Add(addressRow);



                        addressRow = AddressDetaildt.NewRow();
                        addressRow["QuoteAdd_QuoteId"] = 0;
                        addressRow["QuoteAdd_CompanyID"] = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        addressRow["QuoteAdd_BranchId"] = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                        addressRow["QuoteAdd_FinYear"] = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                        addressRow["QuoteAdd_ContactPerson"] = "";
                        addressRow["QuoteAdd_addressType"] = "Shipping";
                        addressRow["QuoteAdd_address1"] = shippingAddress1;
                        addressRow["QuoteAdd_address2"] = shippingAddress2;
                        addressRow["QuoteAdd_address3"] = "";
                        addressRow["QuoteAdd_landMark"] = "";

                        addressRow["QuoteAdd_countryId"] = ShipCountry;
                        addressRow["QuoteAdd_stateId"] = ShipState;
                        addressRow["QuoteAdd_cityId"] = Shipcity;
                        addressRow["QuoteAdd_areaId"] = 0;
                        addressRow["QuoteAdd_pin"] = Shippin;
                        addressRow["QuoteAdd_CreatedDate"] = DateTime.Now;
                        addressRow["QuoteAdd_CreatedUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        addressRow["QuoteAdd_LastModifyDate"] = DateTime.Now;
                        addressRow["QuoteAdd_LastModifyUser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        AddressDetaildt.Rows.Add(addressRow);
                        HttpContext.Current.Session["SI_QuotationAddressDtl"] = AddressDetaildt;

                        DataTable StateDetails = oDBEngine.GetDataTable("select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + billState + " union all select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + ShipState);
                        string BillingStateText = "", BillingStateCode = "", ShippingStateText = "", ShippingStateCode = "";
                        if (StateDetails.Rows.Count > 0)
                        {
                            BillingStateText = Convert.ToString(StateDetails.Rows[0]["stateText"]);
                            BillingStateCode = Convert.ToString(StateDetails.Rows[0]["id"]);

                            ShippingStateText = Convert.ToString(StateDetails.Rows[1]["stateText"]);
                            ShippingStateCode = Convert.ToString(StateDetails.Rows[1]["id"]);
                        }
                        if (GrpCust != "0")
                        {
                            int a = oDBEngine.ExeInteger("insert into tbl_trans_group(grp_contactId,grp_groupMaster,grp_groupType,CreateDate,CreateUser) values('" + InternalId + "','" + GrpCust + "','Vendors','" + CreateDate + "','" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "')");
                        }
                        return new { status = "Ok", InternalId = InternalId, BillingStateText = BillingStateText, BillingStateCode = BillingStateCode, ShippingStateText = ShippingStateText, ShippingStateCode = ShippingStateCode };
                    }
                    else
                    {
                        return new { status = "AUTOError" };
                    }
                }
                else
                {
                    return new { status = "Error", Msg = "Already Exists" };
                }

            }
            catch (Exception ex)
            {
                return new { status = "Error", Msg = ex.Message };
            }
        }


        #endregion

        public static object CustomerSync(String InternalId)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dts = new DataTable();
            dts = oDBEngine.GetDataTable("select cnt_Id from tbl_master_contact where cnt_internalId='" + InternalId + "'");
            if (dts.Rows.Count == 1)
            {
                String weburl = System.Configuration.ConfigurationSettings.AppSettings["FSMAPIBaseUrl"];
                string apiUrl = weburl + "ShopRegisterPortal/CustomerSyncinShop";
                RegisterShopOutput oview = new RegisterShopOutput();
                int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                RegisterShopInputPortal empDtls = new RegisterShopInputPortal();
                DataTable dt = new DataTable();

                ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeDetailsForSync");
                proc.AddPara("@ACTION", "CustomerDetails");
                proc.AddPara("@ContactID", dts.Rows[0]["cnt_Id"].ToString());
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {

                    DateTime date1 = DateTime.Parse("1970-01-01");
                    DateTime date2 = System.DateTime.Now;
                    var Difference_In_Time = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    var middle = (Math.Round(Convert.ToDecimal(Difference_In_Time) / 1000) * 1155) + 1;

                    empDtls.session_token = "zksjfhjsdjkskjdh";
                    empDtls.user_id = Convert.ToString(378);
                    empDtls.shop_name = Convert.ToString(dt.Rows[0]["PartyName"]);
                    empDtls.address = Convert.ToString(dt.Rows[0]["ADDRESS1"]);
                    empDtls.pin_code = Convert.ToString(dt.Rows[0]["PinCode"]);
                    empDtls.shop_lat = Convert.ToString(dt.Rows[0]["PartyLocationLat"]);
                    empDtls.shop_long = Convert.ToString(dt.Rows[0]["PartyLocationLong"]);
                    empDtls.owner_name = Convert.ToString(dt.Rows[0]["Owner"]);
                    empDtls.owner_contact_no = Convert.ToString(dt.Rows[0]["Contact"]);
                    empDtls.owner_email = Convert.ToString(dt.Rows[0]["Email"]);
                    empDtls.type = Convert.ToInt32(dt.Rows[0]["Type"]);
                    empDtls.dob = Convert.ToString(dt.Rows[0]["DOB"]);
                    empDtls.date_aniversary = Convert.ToString(dt.Rows[0]["Anniversary"]);
                    empDtls.shop_id = Convert.ToString(dt.Rows[0]["AssignToUser"]) + "_" + Convert.ToString(Difference_In_Time);
                    empDtls.added_date = Convert.ToString(System.DateTime.Now);
                    empDtls.assigned_to_pp_id = "";
                    empDtls.assigned_to_dd_id = "";
                    empDtls.EntityCode = Convert.ToString(dt.Rows[0]["PartyCode"]);
                    empDtls.Entity_Location = Convert.ToString(dt.Rows[0]["Location"]);
                    empDtls.Alt_MobileNo = Convert.ToString(dt.Rows[0]["AlternateContact"]);
                    //empDtls.State_ID = Convert.ToString(dt.Rows[0]["State"]);
                    empDtls.Entity_Status = Convert.ToString(dt.Rows[0]["Status"]);
                    empDtls.Entity_Type = Convert.ToString(dt.Rows[0]["EntityCategory"]);
                    empDtls.ShopOwner_PAN = Convert.ToString(dt.Rows[0]["OwnerPAN"]);
                    empDtls.ShopOwner_Aadhar = Convert.ToString(dt.Rows[0]["OwnerAadhaar"]);
                    empDtls.Remarks = Convert.ToString(dt.Rows[0]["Remarks"]);
                    empDtls.AreaId = Convert.ToString(dt.Rows[0]["Area"]);
                    empDtls.CityId = Convert.ToString(dt.Rows[0]["District"]);
                    empDtls.Entered_by = Convert.ToString(userid);
                    empDtls.retailer_id = Convert.ToString("0");
                    empDtls.dealer_id = Convert.ToString("0");
                    empDtls.entity_id = Convert.ToString("0");
                    empDtls.party_status_id = Convert.ToString(dt.Rows[0]["PartyStatus"]);
                    empDtls.beat_id = Convert.ToString(dt.Rows[0]["GroupBeat"]);
                    empDtls.IsServicePoint = Convert.ToString(dt.Rows[0]["IsServicePoint"]);
                }

                String Status = "Failed";
                String FailedReason = "";
                string data = JsonConvert.SerializeObject(empDtls);

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(data), "data");
                var result = httpClient.PostAsync(apiUrl, form).Result;

                oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);

                if (Convert.ToString(oview.status) == "200")
                {
                    Status = "Success";
                }
                else if (Convert.ToString(oview.status) == "202")
                {
                    FailedReason = "Customer Name Not found";
                }
                else if (Convert.ToString(oview.status) == "203")
                {
                    FailedReason = "Entity Code not found";
                }
                else if (Convert.ToString(oview.status) == "204")
                {
                    FailedReason = "Owner Name Not found";
                }
                else if (Convert.ToString(oview.status) == "205")
                {
                    FailedReason = "Customer Address not found";
                }
                else if (Convert.ToString(oview.status) == "206")
                {
                    FailedReason = "Pin Code not found";
                }
                else if (Convert.ToString(oview.status) == "207")
                {
                    FailedReason = "Customer Contact number not found";
                }
                else if (Convert.ToString(oview.status) == "208")
                {
                    FailedReason = "User or session token not matched";
                }
                else if (Convert.ToString(oview.status) == "209")
                {
                    FailedReason = "Duplicate Customer Id or contact number";
                }
                else if (Convert.ToString(oview.status) == "210")
                {
                    FailedReason = "Duplicate contact number";
                }

                ProcedureExecute proc1 = new ProcedureExecute("PRC_EmployeeDetailsForSync");
                proc1.AddPara("@ACTION", "SyncLog");
                proc1.AddPara("@ContactID", dts.Rows[0]["cnt_Id"].ToString());
                proc1.AddPara("@CustomerName", Convert.ToString(dt.Rows[0]["PartyName"]));
                proc1.AddPara("@CustomerAddress", Convert.ToString(dt.Rows[0]["ADDRESS1"]));
                proc1.AddPara("@CustomerPhone", Convert.ToString(dt.Rows[0]["Contact"]));
                proc1.AddPara("@SyncBy", userid);
                proc1.AddPara("@Status", Status);
                proc1.AddPara("@FailedReason", FailedReason);
                proc1.AddPara("@Shop_Code", empDtls.shop_id);
                int i = proc1.RunActionQuery();
            }
            return new { status = "ok" };
        }

        public class RegisterShopInputPortal
        {
            public string session_token { get; set; }
            //[Required]
            public string user_id { get; set; }
            //[Required]
            public string shop_name { get; set; }
            //[Required]
            public string address { get; set; }
            //[Required]
            public string pin_code { get; set; }
            //[Required]
            public string shop_lat { get; set; }
            //[Required]
            public string shop_long { get; set; }
            //[Required]
            public string owner_name { get; set; }
            //[Required]
            public string owner_contact_no { get; set; }
            //[Required]
            public string owner_email { get; set; }
            public int? type { get; set; }
            public string dob { get; set; }
            public string date_aniversary { get; set; }
            public string shop_id { get; set; }
            public string added_date { get; set; }
            public string assigned_to_pp_id { get; set; }
            public string assigned_to_dd_id { get; set; }
            public string amount { get; set; }
            public Nullable<DateTime> family_member_dob { get; set; }
            public string director_name { get; set; }
            public string key_person_name { get; set; }
            public string phone_no { get; set; }
            public Nullable<DateTime> addtional_dob { get; set; }
            public Nullable<DateTime> addtional_doa { get; set; }
            public Nullable<DateTime> doc_family_member_dob { get; set; }
            public string specialization { get; set; }
            public string average_patient_per_day { get; set; }
            public string category { get; set; }
            public string doc_address { get; set; }
            public string doc_pincode { get; set; }
            public string is_chamber_same_headquarter { get; set; }
            public string is_chamber_same_headquarter_remarks { get; set; }
            public string chemist_name { get; set; }
            public string chemist_address { get; set; }
            public string chemist_pincode { get; set; }
            public string assistant_name { get; set; }
            public string assistant_contact_no { get; set; }
            public Nullable<DateTime> assistant_dob { get; set; }
            public Nullable<DateTime> assistant_doa { get; set; }
            public Nullable<DateTime> assistant_family_dob { get; set; }
            public string EntityCode { get; set; }
            public string Entity_Location { get; set; }
            public string Alt_MobileNo { get; set; }
            public string Entity_Status { get; set; }
            public string Entity_Type { get; set; }
            public string ShopOwner_PAN { get; set; }
            public string ShopOwner_Aadhar { get; set; }
            public string Remarks { get; set; }
            public string AreaId { get; set; }
            public string CityId { get; set; }
            public string Entered_by { get; set; }
            public string entity_id { get; set; }
            public string party_status_id { get; set; }
            public string retailer_id { get; set; }
            public string dealer_id { get; set; }
            public string beat_id { get; set; }
            public string IsServicePoint { get; set; }
        }

        public class RegisterShopOutput
        {
            public string status { get; set; }
            public string message { get; set; }
            public string session_token { get; set; }
        }
    }
}