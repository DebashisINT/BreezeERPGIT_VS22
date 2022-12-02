using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxClasses;
//using DevExpress.Web.ASPxEditors;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using DevExpress.Utils;
using System.Web.Services;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_Contact_Registration : ERP.OMS.ViewState_class.VSPage
    {
        #region  Global Variable
        //GenericMethod oGenericMethod= new GenericMethod();
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        string strBannedBySEBIMsg = "";
        //oGenericMethod = new GenericMethod();
        AspxHelper oAspxHelper = new AspxHelper();
        bool isPanExists = false;
        string strDuplicatePanMsg = "";
        #endregion

        string DocumentID = null;
        // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        string valueData = "";
        string emailid = "b";
        string bannedid = "N";
        public string[] valuesw = new string[] { "Membership Expiry Date", "Membership Expiry Date:", "Membership Expiry Date :" };
        BusinessLogicLayer.Contact OContract_RegistrationBL = new Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                //string sPath = HttpContext.Current.Request.Url.ToString();
                // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
                oDBEngine.Call_CheckPageaccessebility(sPath);


            }
        }
        //below added for connection purpose
        protected void Page_Init(object sender,EventArgs e)
        {
            Sqlmembership.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlExchange.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlComp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlExchangeSegment.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlProfessional.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSegment.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
          
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            if (Session["requesttype"] != null)
            {
                lblHeadTitle.Text = Convert.ToString(Session["requesttype"]) + " Registration List";
                //lblHeadTitle.Text = Session["requesttype"].ToString() + " Registration List";
            }
            CommonBL ComBL = new CommonBL();
            string duplicate_PAN_Number = ComBL.GetSystemSettingsResult("duplicate_PAN_Number");
            // bellow code added by debjyoti 
            //Reason: Rights of user set from parents 15-11-2016


            if (Session["requesttype"] != null)
            {
                if (Convert.ToString(Session["requesttype"]).Trim() == "Lead")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=Lead");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Customer/Client")
                {
                    //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=customer");
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Franchisee")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=franchisee");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Partner")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=partner");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Relationship Partners")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=referalagent");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Consultant")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=consultant");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Share Holder")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=shareholder");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Debtor")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=debtor");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Creditors")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=creditor");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Relationship Manager")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=agent");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Transporter")
                {
                    //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=Transporter");
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/TransporterMasterList.aspx?requesttype=Transporter");
                }
                else
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_UserGroups.aspx");
                }

            }
            //End Here Debjyoti 15-11-2016


            if (Session["requesttype"] == "Relationship Partners")
            {
                if (!String.IsNullOrEmpty(duplicate_PAN_Number))
                {
                    if (duplicate_PAN_Number == "Yes")
                    {
                        hdnduplicate_PAN_Number.Value = "1";
                    }
                    else if (duplicate_PAN_Number.ToUpper().Trim() == "NO")
                    {
                         hdnduplicate_PAN_Number.Value="0"; 
                    }
                }
             }
            if (Session["requesttype"] == "Customer/Client")
            {
                if (!String.IsNullOrEmpty(duplicate_PAN_Number))
                {
                    if (duplicate_PAN_Number == "Yes")
                    {
                        hdnduplicate_PAN_Number.Value = "1";
                    }
                    else if (duplicate_PAN_Number.ToUpper().Trim() == "NO")
                    {
                        hdnduplicate_PAN_Number.Value = "0";
                    }
                }
            }


            // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_UserGroups.aspx"); 

            string cnttype = Convert.ToString(Session["ContactType"]);
            //string cnttype = Session["ContactType"].ToString();

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (Convert.ToString(Session["requesttype"]) == "Lead")
            {
                TabPage pageFM = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                pageFM.Visible = true;
            }

            if (Convert.ToString(Session["requesttype"]) == "Transporter")
            {
                TabPage page = ASPxPageControl1.TabPages.FindByName("Group Member");
                page.Visible = false;
            }

            if (Convert.ToString(Session["requesttype"]) == "Lead")
            {
                TabPage pageFM = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                pageFM.Visible = true;
            }
            if (Session["Name"] != null)
            {
                lblName.Text = Convert.ToString(Session["Name"]);
                //lblName.Text = Session["Name"].ToString();
            }
            if (Request.QueryString["formtype"] != null)
            {
                //DocumentID = Session["InternalId"].ToString();
                DocumentID = Convert.ToString(Session["InternalId"]);
                DisabledTabPage();
            }
            else
                //DocumentID = HttpContext.Current.Session["KeyVal_InternalID"].ToString();
                DocumentID = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            Session["KeyVal_InternalID"] = DocumentID;
            DataTable Gridbind = oGenericMethod.GetDataTable("SELECT [crg_id], crg_PanExmptProofType,crg_PanExmptProofNumber,[crg_cntId], [crg_type], [crg_Number], [crg_registrationAuthority], [crg_place], [crg_Date],convert(varchar(11),crg_Date,113) as crg_Date1, [crg_validDate],convert(varchar(11),crg_validDate,113) as crg_validDate1,[crg_verify], [CreateDate], [CreateUser], [LastModifyDate], [LastModifyUser] FROM [tbl_master_contactRegistration] where crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "'");
            //DataTable Gridbind = oGenericMethod.GetDataTable("SELECT [crg_id], crg_PanExmptProofType,crg_PanExmptProofNumber,[crg_cntId], [crg_type], [crg_Number], [crg_registrationAuthority], [crg_place], [crg_Date],convert(varchar(11),crg_Date,113) as crg_Date1, [crg_validDate],convert(varchar(11),crg_validDate,113) as crg_validDate1,[crg_verify], [CreateDate], [CreateUser], [LastModifyDate], [LastModifyUser] FROM [tbl_master_contactRegistration] where crg_cntId='" + Session["KeyVal_InternalID"].ToString() + "'");
            oAspxHelper.BindGrid(Gridbind, gridRegisStatutory);

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            hdnBannedPAN.Value = "n";
            hdnDuplicatePAN.Value = "n";
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
            page = ASPxPageControl1.TabPages.FindByName("Bank Details");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Group Member");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Deposit");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Remarks");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Education");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Other");
            page.Enabled = false;
        }

        protected bool subProgram(string str)
        {
            if (str == "Y")
                return true;
            else
                return false;
        }

        protected string MemoQuery_Bind(string QueryId)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string QueryShortName = null;
            //oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtMemoBindResult = oGenericMethod.GetDataTable("Select * From tbl_master_contactRegistration where crg_id ='" + QueryId + "'");
            if (dtMemoBindResult.Rows.Count > 0)
            {
                string type = Convert.ToString(dtMemoBindResult.Rows[0]["crg_type"]);
                //string type = dtMemoBindResult.Rows[0]["crg_type"].ToString();
                //if (type == "Ration Card")
                //{
                //    txtNumber.Text = dtMemoBindResult.Rows[0]["crg_Number"].ToString();
                //    txtIssuePlace.Text = dtMemoBindResult.Rows[0]["crg_place"].ToString();
                //    cmbSR.SelectedItem.Text = dtMemoBindResult.Rows[0]["crg_type"].ToString();


                //}
                QueryShortName = dtMemoBindResult.Rows[0]["crg_type"] + "," + dtMemoBindResult.Rows[0]["crg_Number"] + "," + dtMemoBindResult.Rows[0]["crg_registrationAuthority"] + "," + Convert.ToString(dtMemoBindResult.Rows[0]["crg_place"]) + "," + Convert.ToString(dtMemoBindResult.Rows[0]["crg_Date"]) + "," + Convert.ToString(dtMemoBindResult.Rows[0]["crg_validDate"]) + "," + Convert.ToString(dtMemoBindResult.Rows[0]["crg_verify"]) + "," + Convert.ToString(dtMemoBindResult.Rows[0]["crg_PanExmptProofType"]) + "," + Convert.ToString(dtMemoBindResult.Rows[0]["crg_PanExmptProofNumber"]);
                //QueryShortName = dtMemoBindResult.Rows[0]["crg_type"] + "," + dtMemoBindResult.Rows[0]["crg_Number"] + "," + dtMemoBindResult.Rows[0]["crg_registrationAuthority"] + "," + dtMemoBindResult.Rows[0]["crg_place"].ToString() + "," + dtMemoBindResult.Rows[0]["crg_Date"].ToString() + "," + dtMemoBindResult.Rows[0]["crg_validDate"].ToString() + "," + dtMemoBindResult.Rows[0]["crg_verify"].ToString() + "," + dtMemoBindResult.Rows[0]["crg_PanExmptProofType"].ToString() + "," + dtMemoBindResult.Rows[0]["crg_PanExmptProofNumber"].ToString();

            }
            return QueryShortName;

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        #region gridMembership Section

        protected void gridMembership_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            //string Membershipid = e.Keys[0].ToString();
            // string validitytype = "";
            // validitytype=Convert.ToString(e.NewValues["crg_validityType"]);
            //  if (validitytype == "Lifetime")
            //  {
            //      int i = oDBEngine.SetFieldValue("tbl_master_contactMembership", " crg_memExpDate=" + null + "  ", " crg_internalid ='" + Membershipid + "'");
            //      if (i == 1)
            //      {

            //      }
            //  }

        }

        protected void gridMembership_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string validitytype = "";
            DateTime valdt = new DateTime();
            validitytype = Convert.ToString(e.NewValues["crg_validityType"]);
            if (validitytype == "Limited")
            {
                if (Convert.ToString(e.NewValues["crg_memExpDate"]) != "")
                {
                    valdt = Convert.ToDateTime(e.NewValues["crg_memExpDate"]);
                    if (valdt == DateTime.MinValue)
                    {
                        e.RowError = "Enter a valid date";
                        return;
                    }
                }
                else if (e.NewValues["crg_memExpDate"] == null)
                {
                    e.RowError = "Enter a valid date";
                    return;
                }
            }


            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 


        }
        protected void gridMembership_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string strWhrereClause = "crg_cntId=" + "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "' AND LogModifyUser=-1";
            //string strWhrereClause = "crg_cntId=" + "'" + Session["KeyVal_InternalID"].ToString() + "' AND LogModifyUser=-1";
            string strSetClause = "LogModifyUser=" + Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.GenericMethod objGM = new BusinessLogicLayer.GenericMethod();
            int i = objGM.Update_Table("tbl_master_contactMembership_Log", strSetClause, strWhrereClause);

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        #endregion

        #region PAN Card Checking Section

        private void checkPANExistance(string type, string Id)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string pann = Convert.ToString(CmbProofNumber.Value);
            pann = txtNumber.Text.Trim();
            bool isPanRequired = chkPan.Checked;
            DataTable dtPann = new DataTable();
            //string[] arr = e.Parameters.ToString().Split('~');
            //dtPann = oDBEngine.GetDataTable("tbl_master_contactRegistration", "(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+ltrim(rtrim(ISNULL(con.cnt_UCC,con.cnt_UCC)))+']' from tbl_master_contact as  con where con.cnt_internalid=crg_cntId) as ContactName", "rtrim(ltrim(crg_Number)) ='" + PanNo.ToString().Trim() + "'  and crg_Type='Pancard'   and crg_cntId <> '" + companyID + "' ");
            if (isPanRequired == false && type == "new")
            {
                dtPann = oGenericMethod.GetDataTable("tbl_master_contactRegistration", "crg_cntId", "crg_number='" + pann + "'");
            }
            else if (isPanRequired == false && type != "new")
            {
                dtPann = oGenericMethod.GetDataTable("tbl_master_contactRegistration", "crg_cntId", "crg_number='" + pann + "'  and crg_id !=" + Id);
            }
            if (dtPann != null && dtPann.Rows.Count > 0 && hdnDuplicatePAN.Value.ToLower() == "n")
            {
                isPanExists = true;
                DataTable dtExistingPan = new DataTable();
                dtExistingPan = oGenericMethod.GetDataTable("tbl_master_contact", "ISNULL(cnt_firstName,'') +ISNULL(cnt_middleName,'')+ISNULL(cnt_lastName,'') +' ['+ISNULL(cnt_UCC,'')+']' as NAME", "cnt_internalId='" + Convert.ToString(dtPann.Rows[0][0]) + "'");
                //dtExistingPan = oGenericMethod.GetDataTable("tbl_master_contact", "ISNULL(cnt_firstName,'') +ISNULL(cnt_middleName,'')+ISNULL(cnt_lastName,'') +' ['+ISNULL(cnt_UCC,'')+']' as NAME", "cnt_internalId='" + dtPann.Rows[0][0].ToString() + "'");
                if (dtExistingPan != null && dtExistingPan.Rows.Count > 0)
                {
                    strDuplicatePanMsg = Convert.ToString(dtExistingPan.Rows[0][0]) + " has same PAN. \n Do you want to discard ???";
                }
            }
            else
            {
                isPanExists = false;
            }

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 

        }
        private bool IsBannedPAN(string pan)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            DataTable dtBannedPanCard = new DataTable();
            //   dtBannedPanCard = oDBEngine.GetDataTable("master_bannedentity", " convert(varchar(10), bannedentity_OrderDate,105) as bannedentity_OrderDate,bannedentity_BanPeriod,bannedentity_NSECircularNumber ", "rtrim(ltrim(bannedentity_PAN)) ='" + pan.ToString().Trim() + "'");
            dtBannedPanCard = oDBEngine.GetDataTable("master_bannedentity", "'Circular No : '+BannedEntity_NSECircularNumber +'Order Date : '+ Convert(varchar(20),BannedEntity_OrderDate,105)+ ', Order Period : '+BannedEntity_BanPeriod as Msg", "rtrim(ltrim(bannedentity_PAN)) ='" + Convert.ToString(pan.Trim()) + "'");
            if (dtBannedPanCard != null && dtBannedPanCard.Rows.Count > 0 && hdnBannedPAN.Value.ToLower() == "n")
            {
                strBannedBySEBIMsg = Convert.ToString(dtBannedPanCard.Rows[0][0]);
                return true;
            }
            else
            {
                strBannedBySEBIMsg = string.Empty;
                return false;
            }

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        #endregion

        #region gridRegisStatutory grid Section

        protected void gridRegisStatutory_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = emailid;
            e.Properties["cpWidth"] = bannedid;
        }
        protected void gridRegisStatutory_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string companyID = Convert.ToString(Session["KeyVal_InternalID"]);
            //string companyID = Session["KeyVal_InternalID"].ToString();
            string exchangeId = "";
            string PanNo = "";
            string eid = "";
            string eid1 = "";
            try
            {
                exchangeId = Convert.ToString(e.NewValues["crg_type"]);
                //exchangeId = e.NewValues["crg_type"].ToString();
            }
            catch
            {
            }
            if (Session["check"] != null)
            {

                Session["check"] = null;
                string[,] id = oDBEngine.GetFieldValue("tbl_master_contactRegistration", "crg_cntId", "crg_cntId='" + companyID + "' and crg_type='" + exchangeId + "' ", 1);

                if ((exchangeId == "Pancard") || (exchangeId == "CIN"))
                {
                    if (e.NewValues["crg_verify"] == null)
                    {
                        e.NewValues["crg_verify"] = "";
                    }
                    if (e.NewValues["crg_Number"] == null)
                    {
                        e.NewValues["crg_Number"] = "";
                    }
                    //PanNo = e.NewValues["crg_Number"].ToString();
                    //eid1 = e.NewValues["crg_verify"].ToString();

                    PanNo = Convert.ToString(e.NewValues["crg_Number"]);
                    eid1 = Convert.ToString(e.NewValues["crg_verify"]);

                    //DataTable dtBanned = oDBEngine.GetDataTable("master_bannedentity", " convert(varchar(10), bannedentity_OrderDate,105) as bannedentity_OrderDate,bannedentity_BanPeriod,bannedentity_NSECircularNumber ", "rtrim(ltrim(bannedentity_PAN)) ='" + PanNo.ToString().Trim() + "'");
                    DataTable dtBanned = oDBEngine.GetDataTable("master_bannedentity", " convert(varchar(10), bannedentity_OrderDate,105) as bannedentity_OrderDate,bannedentity_BanPeriod,bannedentity_NSECircularNumber ", "rtrim(ltrim(bannedentity_PAN)) ='" + Convert.ToString(PanNo.Trim()) + "'");
                    if (dtBanned.Rows.Count > 0)
                    {
                        bannedid = "";
                        for (int j = 0; j < dtBanned.Rows.Count; j++)
                        {
                            //bannedid += "Circular No.:" + dtBanned.Rows[j]["bannedentity_NSECircularNumber"].ToString().Trim() + " Order Date: " + dtBanned.Rows[j]["bannedentity_OrderDate"].ToString().Trim() + " Order Period:" + dtBanned.Rows[j]["bannedentity_BanPeriod"].ToString().Trim() + "";
                            bannedid += "Circular No.:" + Convert.ToString(dtBanned.Rows[j]["bannedentity_NSECircularNumber"]).Trim() + " Order Date: " + Convert.ToString(dtBanned.Rows[j]["bannedentity_OrderDate"]).Trim() + " Order Period:" + Convert.ToString(dtBanned.Rows[j]["bannedentity_BanPeriod"]).Trim() + "";
                        }

                    }
                    DataTable dtPan = oDBEngine.GetDataTable("tbl_master_contactRegistration", "(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+ltrim(rtrim(ISNULL(con.cnt_UCC,con.cnt_UCC)))+']' from tbl_master_contact as  con where con.cnt_internalid=crg_cntId) as ContactName", "rtrim(ltrim(crg_Number)) ='" + Convert.ToString(PanNo).Trim() + "'  and crg_Type='Pancard'   and crg_cntId <> '" + companyID + "' ");
                    //DataTable dtPan = oDBEngine.GetDataTable("tbl_master_contactRegistration", "(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+ltrim(rtrim(ISNULL(con.cnt_UCC,con.cnt_UCC)))+']' from tbl_master_contact as  con where con.cnt_internalid=crg_cntId) as ContactName", "rtrim(ltrim(crg_Number)) ='" + PanNo.ToString().Trim() + "'  and crg_Type='Pancard'   and crg_cntId <> '" + companyID + "' ");
                    if (dtPan.Rows.Count > 0)
                    {
                        emailid = "";
                        for (int i = 0; i < dtPan.Rows.Count; i++)
                        {
                            emailid += "[" + Convert.ToString(dtPan.Rows[i]["ContactName"]) + "]";
                        }

                    }

                    if (eid1 == "")
                    {
                        e.RowError = "Please Select Verify Type";
                        return;
                    }
                    if (PanNo == "")
                    {

                        e.RowError = "Please Insert Number";
                        return;
                    }
                    if (PanNo.ToUpper() == "PAN_EXEMPT")
                    {
                        // e.NewValues["crg_Number"] = e.NewValues["crg_PanExmptProofNumber"].ToString();
                        if ((e.NewValues["crg_PanExmptProofNumber"] == null) || (e.NewValues["crg_PanExmptProofType"] == null))
                        {
                            emailid = "NN";
                            bannedid = "BB";
                            e.RowError = "Below Fields are mandatory for PAN_EXEMPT";
                            return;
                        }
                        emailid = "NN";
                        bannedid = "BB";

                    }

                }



                if (id[0, 0] != "n")
                {
                    eid = id[0, 0];
                }

                if (eid == "")
                {
                }
                else
                {
                    e.RowError = "This Type Already Exists";
                    return;
                }

            }
            else
            {
                string depositoryId1 = "";
                try
                {
                    depositoryId1 = Convert.ToString(e.OldValues["crg_type"]);
                    //depositoryId1 = e.OldValues["crg_type"].ToString();
                }
                catch
                {

                }
                if (exchangeId == depositoryId1)
                {
                    if ((exchangeId == "Pancard") || (exchangeId == "CIN"))
                    {
                        PanNo = Convert.ToString(e.NewValues["crg_Number"]);
                        DataTable dtPan = oDBEngine.GetDataTable("tbl_master_contactRegistration", "(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+ltrim(rtrim(ISNULL(con.cnt_UCC,con.cnt_UCC)))+']' from tbl_master_contact as  con where con.cnt_internalid=crg_cntId) as ContactName", "rtrim(ltrim(crg_Number)) ='" + Convert.ToString(PanNo).Trim() + "'  and crg_Type='Pancard'   and crg_cntId <> '" + companyID + "' ");
                        if (dtPan.Rows.Count > 0)
                        {
                            emailid = "";
                            for (int i = 0; i < dtPan.Rows.Count; i++)
                            {
                                //emailid += "[" + dtPan.Rows[i]["ContactName"].ToString() + "]";
                                emailid += "[" + Convert.ToString(dtPan.Rows[i]["ContactName"]) + "]";
                            }

                        }

                        DataTable dtBanned = oDBEngine.GetDataTable("master_bannedentity", " convert(varchar(10), bannedentity_OrderDate,105) as bannedentity_OrderDate,bannedentity_BanPeriod,bannedentity_NSECircularNumber ", "rtrim(ltrim(bannedentity_PAN)) ='" + PanNo.ToString().Trim() + "'");
                        if (dtBanned.Rows.Count > 0)
                        {
                            bannedid = "";
                            for (int j = 0; j < dtBanned.Rows.Count; j++)
                            {
                                //bannedid += "Circular No.:" + dtBanned.Rows[j]["bannedentity_NSECircularNumber"].ToString().Trim() + " Order Date: " + dtBanned.Rows[j]["bannedentity_OrderDate"].ToString().Trim() + " Order Period:" + dtBanned.Rows[j]["bannedentity_BanPeriod"].ToString().Trim() + "";
                                bannedid += "Circular No.:" + Convert.ToString(dtBanned.Rows[j]["bannedentity_NSECircularNumber"]).Trim() + " Order Date: " + Convert.ToString(dtBanned.Rows[j]["bannedentity_OrderDate"]).Trim() + " Order Period:" + Convert.ToString(dtBanned.Rows[j]["bannedentity_BanPeriod"]).Trim() + "";
                            }

                        }

                    }
                    if (exchangeId == "Pancard")
                    {
                        if (PanNo.ToUpper() == "PAN_EXEMPT")
                        {
                            // e.NewValues["crg_Number"] = e.NewValues["crg_PanExmptProofNumber"].ToString();
                            if ((e.NewValues["crg_PanExmptProofNumber"] == null) || (e.NewValues["crg_PanExmptProofType"] == null))
                            {
                                emailid = "NN";
                                bannedid = "BB";
                                e.RowError = "Below Fields are mandatory for PAN_EXEMPT";
                                return;
                            }
                            emailid = "NN";
                            bannedid = "BB";

                        }
                    }
                }
                else
                {
                    string[,] id = oDBEngine.GetFieldValue("tbl_master_contactRegistration", "crg_cntId", "crg_cntId='" + companyID + "' and crg_type='" + exchangeId + "' ", 1);
                    if (id[0, 0] != "n")
                    {
                        eid = id[0, 0];
                    }
                    if (eid == "")
                    {
                    }
                    else
                    {
                        e.RowError = "This Type Already Exists";
                        return;
                    }
                }
            }
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        protected void gridRegisStatutory_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            Session["check"] = "a";
        }

        protected void gridRegisStatutory_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            DateTime issdt = new DateTime();
            DateTime valdt = new DateTime();

            string strPanValiddate = "";
            string strPanIssueDate = "";
            string keyId = string.Empty;
            string cnt = "0";
            int insertcount = 0;
            int deletecount = 0;
            int updatecount = 0;
            gridRegisStatutory.JSProperties["cpPopupfillup"] = null;
            gridRegisStatutory.JSProperties["cpEditPopup"] = null;
            gridRegisStatutory.JSProperties["cpupdate"] = null;
            gridRegisStatutory.JSProperties["cpedit"] = null;
            gridRegisStatutory.JSProperties["cpalreadyexist"] = null;
            gridRegisStatutory.JSProperties["cpinsert"] = null;
            gridRegisStatutory.JSProperties["cpdelete"] = null;
            gridRegisStatutory.JSProperties["cppanMsg"] = null;
            gridRegisStatutory.JSProperties["cppanDuplicate"] = null;
            gridRegisStatutory.JSProperties["cppanBanned"] = null;
            // gridRegisStatutory.JSProperties["cpBannedPan"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            //string WhichCall = e.Parameters.ToString().Split('~')[0];
            //string WhichType = e.Parameters.ToString().Split('~')[1];
            #region saveold
            if (WhichCall == "saveold")
            {
                    BusinessLogicLayer.Contract_RegistrationBL CRB = new BusinessLogicLayer.Contract_RegistrationBL();
                    Boolean IsPresent = false;


                //oGenericMethod = new GenericMethod();
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                #region checked
                if (WhichType == "Pan Card")
                {


                    string chkunchk = Convert.ToString(e.Parameters).Split('~')[2];
                    if (chkunchk == "true")
                    {
                        //................CODE UPDATED BY sAM ON 18102016.................................................

                        if (Convert.ToString(e.Parameters).Split('~')[9] == "Not Verified")
                        {
                            cnt = "0";
                        }
                        if (Convert.ToString(e.Parameters).Split('~')[9] == "Verified")
                        {
                            cnt = "1";
                        }
                        if (Convert.ToString(dtValid.Date) != "1/1/0001 12:00:00 AM")
                        {
                            valdt = Convert.ToDateTime(dtValid.Date);
                            //strPanValiddate = Convert.ToString(dtValid.Date);
                        }
                        else
                        {
                            valdt = DateTime.Now;
                        }
                        if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                        {
                            issdt = Convert.ToDateTime(dtIssue.Date);
                            //strPanIssueDate = Convert.ToString(dtIssue.Date);
                        }
                        else
                        {
                            issdt = DateTime.Now;
                        }
                        //................CODE  UPDATED BY sAM ON 18102016.................................................
                        //string strPanValiddate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[7]);
                        //string strPanIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[8]);
                        // checkPANExistance("update", e.Parameters.ToString().Split('~')[10]);
                        //................CODE UPDATED BY sAM ON 18102016.................................................
                        //updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Session["KeyVal_InternalID"].ToString() + "',crg_contactType='contact',crg_type='" + WhichType.ToString().Trim() + "',crg_Number='PAN_EXEMPT',crg_place='" + e.Parameters.ToString().Split('~')[6] + "',crg_validDate='" + strPanValiddate + "',crg_Date='" + strPanIssueDate + "',crg_verify='" + e.Parameters.ToString().Split('~')[9] + "',crg_PanExmptProofType='" + e.Parameters.ToString().Split('~')[3] + "',crg_PanExmptProofNumber='" + e.Parameters.ToString().Split('~')[4] + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + e.Parameters.ToString().Split('~')[10] + "'");

                        updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='PAN_EXEMPT',crg_place='" + Convert.ToString(e.Parameters).Split('~')[6] + "',crg_validDate='" + valdt + "',crg_Date='" + issdt + "',crg_verify='" + cnt + "',crg_PanExmptProofType='" + Convert.ToString(e.Parameters).Split('~')[3] + "',crg_PanExmptProofNumber='" + Convert.ToString(e.Parameters).Split('~')[4] + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToString(e.Parameters).Split('~')[10] + "'");
                        //................CODE Above UPDATED BY sAM ON 18102016.................................................
                        if (updatecount > 0)
                            gridRegisStatutory.JSProperties["cpupdate"] = "success";
                        else
                            gridRegisStatutory.JSProperties["cpupdate"] = "fail";



                    }
                    else
                    {
                        if (hdnduplicate_PAN_Number.Value != "1")
                        {
                            checkPANExistance("update", Convert.ToString(e.Parameters).Split('~')[5]);
                        }
                        if (isPanExists == false)
                        {
                            if (IsBannedPAN(txtNumber.Text) == false)
                            {
                                if (Convert.ToString(e.Parameters).Split('~')[4] == "Not Verified")
                                {
                                    cnt = "0";
                                }
                                if (Convert.ToString(e.Parameters).Split('~')[4] == "Verified")
                                {
                                    cnt = "1";
                                }
                                if (Convert.ToString(dtValid.Date) != "1/1/0001 12:00:00 AM")
                                {
                                    valdt = Convert.ToDateTime(dtValid.Date);
                                    //strPanValiddate = Convert.ToString(dtValid.Date);
                                }
                                else
                                {
                                    valdt = DateTime.Now;
                                }
                                //................CODE UPDATED BY sAM ON 18102016.................................................
                                //updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Session["KeyVal_InternalID"].ToString() + "',crg_contactType='contact',crg_type='" + WhichType.ToString().Trim() + "',crg_Number='" + e.Parameters.ToString().Split('~')[3] + "',crg_verify='" + e.Parameters.ToString().Split('~')[4] + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + e.Parameters.ToString().Split('~')[5] + "'");
                                if (hdnduplicate_PAN_Number.Value == "1" && Session["requesttype"] == "Relationship Partners")
                                {
                                    IsPresent = CRB.CheckUniqueReginfluencerDocNumber(Convert.ToString(e.Parameters).Split('~')[5], Convert.ToString(e.Parameters).Split('~')[3], 1);
                                }
                                else if (hdnduplicate_PAN_Number.Value == "1" && Session["requesttype"] == "Customer/Client")
                                {
                                    IsPresent = CRB.CheckUniqueRegCustomerDocNumber(Convert.ToString(e.Parameters).Split('~')[5], Convert.ToString(e.Parameters).Split('~')[3], 1);
                                }
                                else
                                {
                                    IsPresent = CRB.CheckUniqueRegDocNumber(Convert.ToString(e.Parameters).Split('~')[5], Convert.ToString(e.Parameters).Split('~')[3], 1);
                                }
                                if (IsPresent == true)
                                {
                                   // Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Number already exists. Please add onother.')</script>", false);
                                    gridRegisStatutory.JSProperties["cpupdate"] = "duplicate"; 
                                    return;
                                }
                                
                                updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='" + Convert.ToString(e.Parameters).Split('~')[3] + "',crg_verify='" + cnt + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToString(e.Parameters).Split('~')[5] + "'");

                                //................CODE Above UPDATED BY sAM ON 18102016.................................................
                                if (updatecount > 0)
                                {
                                    hdnBannedPAN.Value = "N";
                                    hdnDuplicatePAN.Value = "N";
                                    gridRegisStatutory.JSProperties["cpupdate"] = "success";

                                }
                                else
                                    gridRegisStatutory.JSProperties["cpupdate"] = "fail";
                            }
                            else
                            {
                                gridRegisStatutory.JSProperties["cpupdate"] = "fail";
                                gridRegisStatutory.JSProperties["cppanMsg"] = "This PAN is banned by SEBI. See details \n  " + strBannedBySEBIMsg + " \n \n Do you want to discard ???";
                                gridRegisStatutory.JSProperties["cppanBanned"] = "non";
                            }
                        }
                        else
                        {
                            gridRegisStatutory.JSProperties["cpupdate"] = "fail";
                            gridRegisStatutory.JSProperties["cppanMsg"] = strDuplicatePanMsg;
                            gridRegisStatutory.JSProperties["cppanDuplicate"] = "non";
                        }
                    }
                #endregion checked
                }

                else if (WhichType == "VoterId" || WhichType == "SEBI Registration" || WhichType == "MAPIN CODE")
                {
                    //string strIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[4]);
                    if (Convert.ToString(cmbVerify.SelectedItem.Text) == "Not Verified")
                    {
                        cnt = "0";
                    }
                    if (Convert.ToString(cmbVerify.SelectedItem.Text) == "Verified")
                    {
                        cnt = "1";
                    }
                    if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                    {
                        issdt = Convert.ToDateTime(dtIssue.Date);
                        //strPanIssueDate = Convert.ToString(dtIssue.Date);
                    }
                    else
                    {
                        issdt = DateTime.Now;
                    }
                    IsPresent = CRB.CheckUniqueRegDocNumber(Convert.ToString(e.Parameters).Split('~')[6], Convert.ToString(e.Parameters).Split('~')[2], 1);
                    if (IsPresent == true)
                    {
                        gridRegisStatutory.JSProperties["cpupdate"] = "duplicate";
                        //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Number already exists. Please add onother.')</script>", false);

                        return;
                    }
                    
                    updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='" + Convert.ToString(e.Parameters).Split('~')[2] + "',crg_place='" + Convert.ToString(e.Parameters).Split('~')[3] + "',crg_Date='" + issdt + "',crg_verify='" + cnt + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToString(e.Parameters).Split('~')[6] + "'");

                    if (updatecount > 0)
                        gridRegisStatutory.JSProperties["cpupdate"] = "success";
                    else
                        gridRegisStatutory.JSProperties["cpupdate"] = "fail";
                }

                else if ((WhichType == "Other") || (WhichType == "MOA"))
                {
                    //string strOMIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[5]);
                    if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                    {
                        issdt = Convert.ToDateTime(dtIssue.Date);
                        //strPanIssueDate = Convert.ToString(dtIssue.Date);
                    }
                    else
                    {
                        issdt = DateTime.Now;
                    }

                    IsPresent = CRB.CheckUniqueRegDocNumber(Convert.ToString(e.Parameters).Split('~')[7], Convert.ToString(e.Parameters).Split('~')[2], 1);
                    if (IsPresent == true)
                    {
                        gridRegisStatutory.JSProperties["cpupdate"] = "duplicate";
                       // Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Number already exists. Please add onother.')</script>", false);

                        return;
                    }
                    updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='" + Convert.ToString(e.Parameters).Split('~')[2] + "',crg_place='" + Convert.ToString(e.Parameters).Split('~')[3] + "',crg_Date='" + issdt + "',crg_registrationAuthority='" + Convert.ToString(e.Parameters).Split('~')[6] + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToString(e.Parameters).Split('~')[7] + "'");

                    if (updatecount > 0)
                        gridRegisStatutory.JSProperties["cpupdate"] = "success";
                    else
                        gridRegisStatutory.JSProperties["cpupdate"] = "fail";
                }
                else if (WhichType == "CIN")
                {
                    //string strCinIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[5]);
                    if (Convert.ToString(e.Parameters).Split('~')[7] == "Not Verified")
                    {
                        cnt = "0";
                    }
                    if (Convert.ToString(e.Parameters).Split('~')[7] == "Verified")
                    {
                        cnt = "1";
                    }
                    if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                    {
                        issdt = Convert.ToDateTime(dtIssue.Date);
                        //strPanIssueDate = Convert.ToString(dtIssue.Date);
                    }
                    else
                    {
                        issdt = DateTime.Now;
                    }
                    //updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Session["KeyVal_InternalID"].ToString() + "',crg_contactType='contact',crg_type='" + WhichType.ToString().Trim() + "',crg_Number='" + e.Parameters.ToString().Split('~')[2] + "',crg_place='" + e.Parameters.ToString().Split('~')[3] + "',crg_Date='" + issdt + "',crg_registrationAuthority='" + e.Parameters.ToString().Split('~')[6] + "',crg_verify='" + e.Parameters.ToString().Split('~')[7] + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + e.Parameters.ToString().Split('~')[8] + "'");

                    IsPresent = CRB.CheckUniqueRegDocNumber(Convert.ToString(e.Parameters).Split('~')[8], Convert.ToString(e.Parameters).Split('~')[2], 1);
                    if (IsPresent == true)
                    {
                        gridRegisStatutory.JSProperties["cpupdate"] = "duplicate";
                        //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Number already exists. Please add onother.')</script>", false);

                        return;
                    }
                    
                    updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='" + Convert.ToString(e.Parameters).Split('~')[2] + "',crg_place='" + Convert.ToString(e.Parameters).Split('~')[3] + "',crg_Date='" + issdt + "',crg_registrationAuthority='" + Convert.ToString(e.Parameters).Split('~')[6] + "',crg_verify='" + cnt + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToString(e.Parameters).Split('~')[8] + "'");

                    if (updatecount > 0)
                        gridRegisStatutory.JSProperties["cpupdate"] = "success";
                    else
                        gridRegisStatutory.JSProperties["cpupdate"] = "fail";
                }
                else if (WhichType == "AdharCard" || WhichType == "Aadhar Card")
                {
                    if (Convert.ToString(e.Parameters).Split('~')[3] == "Not Verified")
                    {
                        cnt = "0";
                    }
                    if (Convert.ToString(e.Parameters).Split('~')[3] == "Verified")
                    {
                        cnt = "1";
                    }
                    //updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Session["KeyVal_InternalID"].ToString() + "',crg_contactType='contact',crg_type='" + WhichType.ToString().Trim() + "',crg_Number='" + e.Parameters.ToString().Split('~')[2] + "',crg_verify='" + e.Parameters.ToString().Split('~')[3] + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + e.Parameters.ToString().Split('~')[4] + "'");

                    IsPresent = CRB.CheckUniqueRegDocNumber(Convert.ToString(e.Parameters).Split('~')[4], Convert.ToString(e.Parameters).Split('~')[2], 1);
                    if (IsPresent == true)
                    {
                        gridRegisStatutory.JSProperties["cpupdate"] = "duplicate";
                        //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Number already exists. Please add onother.')</script>", false);

                        return;
                    }
                    
                    updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='" + Convert.ToString(e.Parameters).Split('~')[2] + "',crg_verify='" + cnt + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToString(e.Parameters).Split('~')[4] + "'");

                    if (updatecount > 0)
                        gridRegisStatutory.JSProperties["cpupdate"] = "success";
                    else
                        gridRegisStatutory.JSProperties["cpupdate"] = "fail";

                }
                else if (WhichType == "VAT Regn No" || WhichType == "CST Regn No" || WhichType == "ECC Regn No" || WhichType == "Service Tax Regn No" || WhichType == "SGST" || WhichType == "IGST" || WhichType == "CGST")
                {

                    //................CODE UPDATED BY sAM ON 18102016.................................................

                    if (Convert.ToString(e.Parameters).Split('~')[9] == "Not Verified")
                    {
                        cnt = "0";
                    }
                    if (Convert.ToString(e.Parameters).Split('~')[9] == "Verified")
                    {
                        cnt = "1";
                    }


                    IsPresent = CRB.CheckUniqueRegDocNumber(Convert.ToString(Session["EditId"]), Convert.ToString(e.Parameters).Split('~')[2], 1);
                    if (IsPresent == true)
                    {
                        gridRegisStatutory.JSProperties["cpupdate"] = "duplicate";
                       // Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Number already exists. Please add onother.')</script>", false);

                        return;
                    }
                        updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='" + Convert.ToString(e.Parameters).Split('~')[2] + "',crg_verify='" + cnt + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToSingle(Session["EditId"]) + "'");
                  
                    
                    //................CODE Above UPDATED BY sAM ON 18102016.................................................
                    if (updatecount > 0)
                        gridRegisStatutory.JSProperties["cpupdate"] = "success";
                    else
                        gridRegisStatutory.JSProperties["cpupdate"] = "fail";

                }
                else
                {
                    //string strOthValiddate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[4]);
                    //string strOthIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[5]);
                    if (Convert.ToString(e.Parameters).Split('~')[6] == "Not Verified")
                    {
                        cnt = "0";
                    }
                    if (Convert.ToString(e.Parameters).Split('~')[6] == "Verified")
                    {
                        cnt = "1";
                    }
                    if (Convert.ToString(dtValid.Date) != "1/1/0001 12:00:00 AM")
                    {
                        valdt = Convert.ToDateTime(dtValid.Date);
                        //strPanValiddate = Convert.ToString(dtValid.Date);
                    }
                    else
                    {
                        valdt = DateTime.Now;
                    }
                    if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                    {
                        issdt = Convert.ToDateTime(dtIssue.Date);
                        //strPanIssueDate = Convert.ToString(dtIssue.Date);
                    }
                    else
                    {
                        issdt = DateTime.Now;
                    }
                    //updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Session["KeyVal_InternalID"].ToString() + "',crg_contactType='contact',crg_type='" + WhichType.ToString().Trim() + "',crg_Number='" + e.Parameters.ToString().Split('~')[2] + "',crg_place='" + e.Parameters.ToString().Split('~')[3] + "',crg_validDate='" + strOthValiddate + "',crg_Date='" + strOthIssueDate + "',crg_verify='" + e.Parameters.ToString().Split('~')[6] + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + e.Parameters.ToString().Split('~')[7] + "'");

                   
                  
                        updatecount = oGenericMethod.Update_Table("tbl_master_contactRegistration", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "',crg_contactType='contact',crg_type='" + Convert.ToString(WhichType).Trim() + "',crg_Number='" + Convert.ToString(e.Parameters).Split('~')[2] + "',crg_place='" + Convert.ToString(e.Parameters).Split('~')[3] + "',crg_validDate='" + valdt + "',crg_Date='" + issdt + "',crg_verify='" + cnt + "',LastModifyDate='" + oGenericMethod.GetDate(110) + "',LastModifyUser=" + Session["userid"] + "", "crg_id='" + Convert.ToString(e.Parameters).Split('~')[7] + "'");
                   
                    if (updatecount > 0)
                        gridRegisStatutory.JSProperties["cpupdate"] = "success";
                    else
                        gridRegisStatutory.JSProperties["cpupdate"] = "fail";
                }


            }
            #endregion saveold
            #region edit
            if (WhichCall == "Edit")
            {
                //oGenericMethod = new GenericMethod();
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                DataTable DtEdit = oGenericMethod.GetDataTable("select isnull(crg_type,'') as crg_type,isnull(crg_PanExmptProofType,'') as crg_PanExmptProofType,isnull(crg_PanExmptProofNumber,'') as crg_PanExmptProofNumber,isnull(crg_Number,'') as crg_Number,isnull(crg_place,'') as crg_place,isnull(crg_validDate,'') as crg_validDate,isnull(crg_Date,'') as crg_Date,isnull(crg_verify,'') as crg_verify,isnull(crg_registrationAuthority,'') as crg_registrationAuthority from tbl_master_contactRegistration where crg_id=" + WhichType + "");
                string CmbType = Convert.ToString(DtEdit.Rows[0]["crg_type"]);
                string CmbProofType = Convert.ToString(DtEdit.Rows[0]["crg_PanExmptProofType"]);
                string strCmbProofNumber = Convert.ToString(DtEdit.Rows[0]["crg_PanExmptProofNumber"]);
                string CmbNumber = Convert.ToString(DtEdit.Rows[0]["crg_Number"]);
                string CmbPlace = Convert.ToString(DtEdit.Rows[0]["crg_place"]);
                string CmbValidUntil = Convert.ToString(DtEdit.Rows[0]["crg_validDate"]);
                string CmbIssueDt = Convert.ToString(DtEdit.Rows[0]["crg_Date"]);
                string CmbRegAuth = Convert.ToString(DtEdit.Rows[0]["crg_registrationAuthority"]);
                string CmbVerify = Convert.ToString(DtEdit.Rows[0]["crg_verify"]);
                gridRegisStatutory.JSProperties["cpedit"] = CmbType + "~" + CmbProofType + "~" + strCmbProofNumber + "~" + CmbNumber + "~" + CmbPlace + "~" + CmbValidUntil + "~" + CmbIssueDt + "~" + CmbRegAuth + "~" + CmbVerify + "~" + WhichType;
                Session["EditId"] = WhichType;
            }
            #endregion
            #region Delete
            if (WhichCall == "DeleteItem")
            {
                //oGenericMethod = new GenericMethod();
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

                deletecount = oGenericMethod.Delete_Table("tbl_master_contactRegistration", "crg_id=" + WhichType + "");
                if (deletecount > 0)
                {
                    gridRegisStatutory.JSProperties["cpdelete"] = "deleteok";
                    //    string strWhrereClause = "crg_cntId=" + "'" + Session["KeyVal_InternalID"].ToString() + "' AND LogModifyUser=-1";
                    //    string strSetClause = "LogModifyUser=" + Convert.ToString(HttpContext.Current.Session["userid"]);
                    //    GenericMethod objGM = new GenericMethod();
                    //    int i = objGM.Update_Table("tbl_master_contactRegistration_log", strSetClause, strWhrereClause);
                }
                else
                    gridRegisStatutory.JSProperties["cpdelete"] = "deletefail";

                hdnDuplicatePAN.Value = "n";
                hdnBannedPAN.Value = "n";
                // ScriptManager.RegisterStartupScript(this, GetType(), "ForcePostBack", "ForcePostBack();", true);

            }
            #endregion Delete
            #region onlybind
            if (WhichCall == "onlybind")
            {
                //oGenericMethod = new GenericMethod();
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

                DataTable Gridbind = oGenericMethod.GetDataTable("SELECT [crg_id], crg_PanExmptProofType,crg_PanExmptProofNumber,[crg_cntId], [crg_type], [crg_Number], [crg_registrationAuthority], [crg_place], [crg_Date],convert(varchar(11),crg_Date,113) as crg_Date1, [crg_validDate],convert(varchar(11),crg_validDate,113) as crg_validDate1,[crg_verify], [CreateDate], [CreateUser], [LastModifyDate], [LastModifyUser] FROM [tbl_master_contactRegistration] where crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "'");
                if (Gridbind.Rows.Count > 0)
                    oAspxHelper.BindGrid(Gridbind, gridRegisStatutory);
                else
                    oAspxHelper.BindGrid(gridRegisStatutory);


            }
            #endregion onlybind
            #region savenew
            if (WhichCall == "savenew")
            {


                BusinessLogicLayer.Contract_RegistrationBL CRB = new BusinessLogicLayer.Contract_RegistrationBL();
                Boolean IsPresent = CRB.CheckUniqueRegDocNumber("", Convert.ToString(e.Parameters).Split('~')[2], 0);
                if (IsPresent == true)
                {
                 //   ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Number already exists. Please add onother');", true);
                 //   ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Number already exists. Please add onother');", true);             
                    gridRegisStatutory.JSProperties["cpinsert"] = "duplicate";                         
             return;
                }
                                     




                DateTime dt = new DateTime();
                //oGenericMethod = new GenericMethod();
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

                string[,] countrecord = oGenericMethod.GetFieldValue("tbl_master_contactRegistration", "crg_type", "crg_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "' and crg_type='" + WhichType + "'", 1);
                if (countrecord[0, 0] == "n")
                {
                    if (WhichType == "Pan Card")
                    {
                        //................CODE UPDATED BY sAM ON 18102016.................................................



                        string chkunchk = Convert.ToString(e.Parameters).Split('~')[2];

                        if (chkunchk == "true")
                        {
                            if (Convert.ToString(e.Parameters).Split('~')[9] == "Not Verified")
                            {
                                cnt = "0";
                            }
                            if (Convert.ToString(e.Parameters).Split('~')[9] == "Verified")
                            {
                                cnt = "1";
                            }

                            if (Convert.ToString(dtValid.Date) != "1/1/0001 12:00:00 AM")
                            {
                                valdt = Convert.ToDateTime(dtValid.Date);
                                //strPanValiddate = Convert.ToString(dtValid.Date);
                            }
                            else
                            {
                                valdt = DateTime.Now;
                            }
                            if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                            {
                                issdt = Convert.ToDateTime(dtIssue.Date);
                                //strPanIssueDate = Convert.ToString(dtIssue.Date);
                            }
                            else
                            {
                                issdt = DateTime.Now;
                            }
                            //if(Convert.ToString(dtValid.Date)!="")
                            //{

                            //    strPanValiddate = Convert.ToString(dtValid.Date);
                            //}
                            //else
                            //{
                            //     strPanValiddate = oGenericMethod.GetDate(110);
                            //}
                            //if (Convert.ToString(dtIssue.Date) != "")
                            //{
                            //    strPanIssueDate = Convert.ToString(dtIssue.Date);
                            //}
                            //else
                            //{
                            //     strPanIssueDate = oGenericMethod.GetDate(110);
                            //}

                            //string strPanValiddate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[7]);
                            //string strPanIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[8]);
                            if (hdnduplicate_PAN_Number.Value != "1")
                            {
                                checkPANExistance("new", "0");
                            }
                            //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_validDate,crg_Date,crg_verify,crg_PanExmptProofType,crg_PanExmptProofNumber,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','PAN_EXEMPT','" + e.Parameters.ToString().Split('~')[6] + "','" + strPanValiddate + "','" + strPanIssueDate + "','" + cmbVerify.SelectedItem.Text + "','" + e.Parameters.ToString().Split('~')[3] + "','" + e.Parameters.ToString().Split('~')[4] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                            insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_validDate,crg_Date,crg_verify,crg_PanExmptProofType,crg_PanExmptProofNumber,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','PAN_EXEMPT','" + Convert.ToString(e.Parameters).Split('~')[6] + "','" + valdt + "','" + issdt + "','" + cnt + "','" + Convert.ToString(e.Parameters).Split('~')[3] + "','" + Convert.ToString(e.Parameters).Split('~')[4] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                            //................CODE Above UPDATED BY sAM ON 18102016.................................................

                            if (insertcount > 0)
                                gridRegisStatutory.JSProperties["cpinsert"] = "success";
                            else
                                gridRegisStatutory.JSProperties["cpinsert"] = "fail";

                        }
                        else
                        {
                            if (hdnduplicate_PAN_Number.Value != "1")
                            {
                                checkPANExistance("new", "0");
                            }
                            if (isPanExists == false)
                            {
                                if (IsBannedPAN(txtNumber.Text) == false)
                                {
                                    if (Convert.ToString(e.Parameters).Split('~')[4] == "Not Verified")
                                    {
                                        cnt = "0";
                                    }
                                    if (Convert.ToString(e.Parameters).Split('~')[4] == "Verified")
                                    {
                                        cnt = "1";
                                    }
                                    //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_verify,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','" + e.Parameters.ToString().Split('~')[3] + "','" + e.Parameters.ToString().Split('~')[4] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                                   
                                      
                                   
                                          insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_verify,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','" + Convert.ToString(e.Parameters).Split('~')[3] + "','" + cnt + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                                     
                                    if (insertcount > 0)
                                    {
                                        gridRegisStatutory.JSProperties["cpinsert"] = "success";
                                        hdnBannedPAN.Value = "N";
                                        hdnDuplicatePAN.Value = "N";
                                    }
                                    else
                                        gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                                }
                                else
                                {
                                    gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                                    gridRegisStatutory.JSProperties["cppanMsg"] = "This PAN is banned by SEBI. See details \n  " + strBannedBySEBIMsg + " \n \n Do you want to discard ???";
                                    gridRegisStatutory.JSProperties["cppanBanned"] = "non";
                                }
                            }
                            else
                            {
                                gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                                gridRegisStatutory.JSProperties["cppanMsg"] = strDuplicatePanMsg;
                                gridRegisStatutory.JSProperties["cppanDuplicate"] = "non";
                            }
                        }
                    }

                    else if ((WhichType == "Other") || (WhichType == "MOA"))
                    {
                        if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                        {
                            issdt = Convert.ToDateTime(dtIssue.Date);
                            //strPanIssueDate = Convert.ToString(dtIssue.Date);
                        }
                        else
                        {
                            issdt = DateTime.Now;
                        }
                        //string strOMIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[4]);

                        //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_Date,crg_registrationAuthority,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','" + e.Parameters.ToString().Split('~')[2] + "','" + e.Parameters.ToString().Split('~')[3] + "','" + strOMIssueDate + "','" + e.Parameters.ToString().Split('~')[6] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                        insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_Date,crg_registrationAuthority,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','" + Convert.ToString(e.Parameters).Split('~')[2] + "','" + Convert.ToString(e.Parameters).Split('~')[3] + "','" + issdt + "','" + Convert.ToString(e.Parameters).Split('~')[6] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");

                        if (insertcount > 0)
                            gridRegisStatutory.JSProperties["cpinsert"] = "success";
                        else
                            gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                    }

                    else if (WhichType == "VoterId" || WhichType == "SEBI Registration" || WhichType == "MAPIN CODE")
                    {
                        if (cmbVerify.SelectedItem.Text == "Not Verified")
                        {
                            cnt = "0";
                        }
                        if (cmbVerify.SelectedItem.Text == "Verified")
                        {
                            cnt = "1";
                        }
                        if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                        {
                            issdt = Convert.ToDateTime(dtIssue.Date);
                            //strPanIssueDate = Convert.ToString(dtIssue.Date);
                        }
                        else
                        {
                            issdt = DateTime.Now;
                        }
                        //string strIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[4]);

                        //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_Date,crg_verify,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','" + e.Parameters.ToString().Split('~')[2] + "','" + e.Parameters.ToString().Split('~')[3] + "','" + strIssueDate + "','" + cmbVerify.SelectedItem.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                        insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_Date,crg_verify,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','" + Convert.ToString(e.Parameters).Split('~')[2] + "','" + Convert.ToString(e.Parameters).Split('~')[3] + "','" + issdt + "','" + cnt + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");

                        if (insertcount > 0)
                            gridRegisStatutory.JSProperties["cpinsert"] = "success";
                        else
                            gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                    }



                    else if (WhichType == "CIN")
                    {

                        //string strCinIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[4]);
                        if (cmbVerify.SelectedItem.Text == "Not Verified")
                        {
                            cnt = "0";
                        }
                        if (cmbVerify.SelectedItem.Text == "Verified")
                        {
                            cnt = "1";
                        }
                        //if (Convert.ToString(e.Parameters).Split('~')[3] == "Not Verified")
                        //{
                        //    cnt = "0";
                        //}
                        //if (Convert.ToString(e.Parameters).Split('~')[3] == "Verified")
                        //{
                        //    cnt = "1";
                        //}
                        if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                        {
                            issdt = Convert.ToDateTime(dtIssue.Date);
                            //strPanIssueDate = Convert.ToString(dtIssue.Date);
                        }
                        else
                        {
                            issdt = DateTime.Now;
                        }
                        //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_Date,crg_registrationAuthority,crg_verify,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','" + e.Parameters.ToString().Split('~')[2] + "','" + e.Parameters.ToString().Split('~')[3] + "','" + strCinIssueDate + "','" + e.Parameters.ToString().Split('~')[6] + "','" + cmbVerify.SelectedItem.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                        insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_Date,crg_registrationAuthority,crg_verify,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','" + Convert.ToString(e.Parameters).Split('~')[2] + "','" + Convert.ToString(e.Parameters).Split('~')[3] + "','" + issdt + "','" + Convert.ToString(e.Parameters).Split('~')[6] + "','" + cnt + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");

                        if (insertcount > 0)
                            gridRegisStatutory.JSProperties["cpinsert"] = "success";
                        else
                            gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                    }
                    else if (WhichType == "AdharCard")
                    {
                        if (Convert.ToString(e.Parameters).Split('~')[3] == "Not Verified")
                        {
                            cnt = "0";
                        }
                        if (Convert.ToString(e.Parameters).Split('~')[3] == "Verified")
                        {
                            cnt = "1";
                        }
                        //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_verify,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','" + e.Parameters.ToString().Split('~')[2] + "','" + e.Parameters.ToString().Split('~')[3] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                        insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_verify,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','" + Convert.ToString(e.Parameters).Split('~')[2] + "','" + cnt + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");

                        if (insertcount > 0)
                            gridRegisStatutory.JSProperties["cpinsert"] = "success";
                        else
                            gridRegisStatutory.JSProperties["cpinsert"] = "fail";


                    }
                    else if (WhichType == "VAT Regn No" || WhichType == "CST Regn No" || WhichType == "ECC Regn No" || WhichType == "Service Tax Regn No" || WhichType == "SGST" || WhichType == "IGST" || WhichType == "CGST")
                    {
                        if (Convert.ToString(e.Parameters).Split('~')[6] == "Not Verified")
                        {
                            cnt = "0";
                        }
                        if (Convert.ToString(e.Parameters).Split('~')[6] == "Verified")
                        {
                            cnt = "1";
                        }
                        //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_verify,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','" + e.Parameters.ToString().Split('~')[2] + "','" + e.Parameters.ToString().Split('~')[3] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                        insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_verify,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','" + Convert.ToString(e.Parameters).Split('~')[2] + "','" + cnt + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");

                        if (insertcount > 0)
                            gridRegisStatutory.JSProperties["cpinsert"] = "success";
                        else
                            gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                    }
                    else
                    {
                        //......................................... Code  Modified By Sam on 17102016..................................................

                        //string strOthValiddate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[4]);
                        //string strOthIssueDate = oGenericMethod.GetDate(BusinessLogicLayer.GenericMethod.DateConvertFrom.UTCToDateTime, e.Parameters.ToString().Split('~')[5]);

                        //string strOthValiddate = Convert.ToString( e.Parameters).Split('~')[4];
                        //string strOthIssueDate = Convert.ToString(e.Parameters).Split('~')[5];
                        if (Convert.ToString(e.Parameters).Split('~')[6] == "Not Verified")
                        {
                            cnt = "0";
                        }
                        if (Convert.ToString(e.Parameters).Split('~')[6] == "Verified")
                        {
                            cnt = "1";
                        }

                        if (Convert.ToString(dtValid.Date) != "1/1/0001 12:00:00 AM")
                        {
                            valdt = Convert.ToDateTime(dtValid.Date);
                            //strPanValiddate = Convert.ToString(dtValid.Date);
                        }
                        else
                        {
                            valdt = DateTime.Now;
                        }
                        if (Convert.ToString(dtIssue.Date) != "1/1/0001 12:00:00 AM")
                        {
                            issdt = Convert.ToDateTime(dtIssue.Date);
                            //strPanIssueDate = Convert.ToString(dtIssue.Date);
                        }
                        else
                        {
                            issdt = DateTime.Now;
                        }
                        //......................................... Code Above Modified By Sam on 17102016..................................................

                        //string strOthValiddate = System.DateTime.Today.ToShortDateString();
                        //string strOthIssueDate = System.DateTime.Today.ToShortDateString();
                        //insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_validDate,crg_Date,crg_verify,CreateDate,CreateUser", "'" + Session["KeyVal_InternalID"].ToString() + "','contact','" + WhichType.ToString().Trim() + "','" + e.Parameters.ToString().Split('~')[2] + "','" + e.Parameters.ToString().Split('~')[3] + "','" + valdt + "','" + issdt + "','" + e.Parameters.ToString().Split('~')[6] + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
                        insertcount = oGenericMethod.Insert_Table("tbl_master_contactRegistration", "crg_cntId,crg_contactType,crg_type,crg_Number,crg_place,crg_validDate,crg_Date,crg_verify,CreateDate,CreateUser", "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "','contact','" + Convert.ToString(WhichType).Trim() + "','" + Convert.ToString(e.Parameters).Split('~')[2] + "','" + Convert.ToString(e.Parameters).Split('~')[3] + "','" + valdt + "','" + issdt + "','" + cnt + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");


                        if (insertcount > 0)
                            gridRegisStatutory.JSProperties["cpinsert"] = "success";
                        else
                            gridRegisStatutory.JSProperties["cpinsert"] = "fail";
                    }


                }
                else
                {
                    gridRegisStatutory.JSProperties["cpalreadyexist"] = "exist~" + WhichType;
                }
            }
            #endregion savenew

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }


        #endregion

        #region gridExchange grid Section

        protected void gridExchange_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string exchangesegmentid = null;
            ASPxComboBox comp = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compCompany");
            ASPxComboBox segment = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compSegment");
            TextBox txtClientUcc = (TextBox)gridExchange.FindEditFormTemplateControl("txtClientUcc");
            ASPxDateEdit dtRegistration = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtRegistration");
            ASPxDateEdit dtBusienssCommDate = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtBusienssCommDate");
            ASPxDateEdit dtSuspension = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtSuspension");
            HiddenField txtSubBroker_hidden = (HiddenField)gridExchange.FindEditFormTemplateControl("txtSubBroker_hidden");
            HiddenField txtDealer_hidden = (HiddenField)gridExchange.FindEditFormTemplateControl("txtDealer_hidden");
            ASPxDateEdit dtCloser = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtCloser");
            TextBox txtFOfficeBranch = (TextBox)gridExchange.FindEditFormTemplateControl("txtFOfficeBranch");
            TextBox txtFOfficeGroup = (TextBox)gridExchange.FindEditFormTemplateControl("txtFOfficeGroup");
            TextBox txtParticipantCode = (TextBox)gridExchange.FindEditFormTemplateControl("txtParticipantCode");
            TextBox txtBankCode = (TextBox)gridExchange.FindEditFormTemplateControl("txtBankCode");
            TextBox txtSchemeCode = (TextBox)gridExchange.FindEditFormTemplateControl("txtSchemeCode");
            DropDownList ddlPatern = (DropDownList)gridExchange.FindEditFormTemplateControl("ddlPatern");
            TextBox txtSuspension = (TextBox)gridExchange.FindEditFormTemplateControl("txtSuspension");
            DropDownList drpSttWap = (DropDownList)gridExchange.FindEditFormTemplateControl("drpSttWap");
            ASPxComboBox ComboSwapMapinSebi = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("ComboSwapMapinSebi");

            SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + ' - ' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + comp.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
            segment.DataBind();
            if (dtRegistration.Value == null)
                //dtRegistration.Value = "1/1/1900 12:00:00 AM";
                dtRegistration.Value = DBNull.Value;
            if (dtBusienssCommDate.Value == null)
                dtBusienssCommDate.Value = DBNull.Value;
            if (dtSuspension.Value == null)
                dtSuspension.Value = DBNull.Value;
            if (dtCloser.Value == null)
                dtCloser.Value = DBNull.Value;

            /* Tier Structure

            SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("insertContactExchange", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@crg_cntID", SqlDbType.VarChar).Value = Session["KeyVal_InternalID"].ToString();
            lcmd.Parameters.Add("@crg_company1", SqlDbType.VarChar).Value = comp.SelectedItem.Value;
            lcmd.Parameters.Add("@crg_exchange1", SqlDbType.VarChar).Value = segment.Text;

            if (dtRegistration.Value != null)
            {
                lcmd.Parameters.Add("@crg_regisDate", SqlDbType.DateTime).Value = dtRegistration.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_regisDate", DBNull.Value);
            }

            if (dtBusienssCommDate.Value != null)
            {
                lcmd.Parameters.Add("@crg_businessCmmDate", SqlDbType.DateTime).Value = dtBusienssCommDate.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_businessCmmDate", DBNull.Value);
            }
            // lcmd.Parameters.Add("@crg_businessCmmDate", SqlDbType.DateTime).Value = dtBusienssCommDate.Value;


            if (dtSuspension.Value != null)
            {
                lcmd.Parameters.Add("@crg_suspensionDate", SqlDbType.DateTime).Value = dtSuspension.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_suspensionDate", DBNull.Value);
            }

        


            //    lcmd.Parameters.Add("@crg_suspensionDate", SqlDbType.DateTime).Value = dtSuspension.Value;
            lcmd.Parameters.Add("@crg_reasonforSuspension", SqlDbType.VarChar).Value = txtSuspension.Text;
            lcmd.Parameters.Add("@crg_tcode", SqlDbType.VarChar).Value = txtClientUcc.Text;
            lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(Session["userid"].ToString());
            lcmd.Parameters.Add("@crg_SubBrokerFranchiseeID", SqlDbType.VarChar).Value = txtSubBroker_hidden.Value;
            lcmd.Parameters.Add("@crg_Dealer", SqlDbType.VarChar).Value = txtDealer_hidden.Value;
            if (dtCloser.Value != null)
            {
                lcmd.Parameters.Add("@crg_AccountClosureDate", SqlDbType.DateTime).Value = dtCloser.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_AccountClosureDate", DBNull.Value);
            }

        

            //lcmd.Parameters.Add("@crg_AccountClosureDate", SqlDbType.DateTime).Value = dtCloser.Value;
            lcmd.Parameters.Add("@crg_FrontOfficeBranchCode", SqlDbType.VarChar).Value = txtFOfficeBranch.Text;
            lcmd.Parameters.Add("@crg_FrontOfficeGroupCode", SqlDbType.VarChar).Value = txtFOfficeGroup.Text;
            lcmd.Parameters.Add("@crg_ParticipantSchemeCode", SqlDbType.VarChar).Value = txtParticipantCode.Text;
            lcmd.Parameters.Add("@crg_ClearingBankCode", SqlDbType.VarChar).Value = txtBankCode.Text;
            lcmd.Parameters.Add("@crg_SchemeCode", SqlDbType.VarChar).Value = txtSchemeCode.Text;
            lcmd.Parameters.Add("@crg_STTPattern", SqlDbType.VarChar).Value = ddlPatern.SelectedItem.Value;
            lcmd.Parameters.Add("@SttWap", SqlDbType.Char, 1).Value = drpSttWap.SelectedItem.Value;
            lcmd.Parameters.Add("@MapinSwapSebi", SqlDbType.Char, 1).Value = ComboSwapMapinSebi.SelectedItem.Value.ToString();
       
            string exchange = segment.Text.Split('-')[0].ToString().Trim();
            string segmentid = null;
            DataTable dtexchange = new DataTable();
            if (segment.Text.Contains("-"))
                segmentid = segment.Text.Split('-')[1].ToString().Trim();
            if (segmentid != null)
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + segment.Text.Split('-')[0].ToString().Trim() + "%' and exch_segmentId like '%" + segment.Text.Split('-')[1].ToString().Trim() + "%' and Exch_CompID='" + comp.SelectedItem.Value.ToString().Trim() + "'");
            else
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + segment.Text.Split('-')[0].ToString().Trim() + "%'  and Exch_CompID='" + comp.SelectedItem.Value.ToString().Trim() + "'");
            exchangesegmentid = dtexchange.Rows[0]["Exch_InternalID"].ToString();
            lcmd.Parameters.AddWithValue("@crg_exchangesegment", exchangesegmentid);
            lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();
              */

            //--------------------- Tier Start---------------------------
            DateTime dtRegDate, dtBusDate, dtSupDate, dtClosorDate;

            if (dtRegistration.Value != null)
            {
                dtRegDate = Convert.ToDateTime(dtRegistration.Value);
            }
            else
            {
                dtRegDate = Convert.ToDateTime("01-01-1900");
            }

            if (dtBusienssCommDate.Value != null)
            {
                dtBusDate = Convert.ToDateTime(dtBusienssCommDate.Value);
            }
            else
            {
                dtBusDate = Convert.ToDateTime("01-01-1900");
            }

            if (dtSuspension.Value != null)
            {
                dtSupDate = Convert.ToDateTime(dtSuspension.Value);
            }
            else
            {
                dtSupDate = Convert.ToDateTime("01-01-1900");
            }

            if (dtCloser.Value != null)
            {
                dtClosorDate = Convert.ToDateTime(dtCloser.Value);
            }
            else
            {
                dtClosorDate = Convert.ToDateTime("01-01-1900");
            }

            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string exchange = Convert.ToString(segment.Text.Split('-')[0]).Trim();
            string segmentid = null;
            DataTable dtexchange = new DataTable();
            if (segment.Text.Contains("-"))
                segmentid = Convert.ToString(segment.Text.Split('-')[1]).Trim();
            if (segmentid != null)
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + Convert.ToString(segment.Text.Split('-')[0]).Trim() + "%' and exch_segmentId like '%" + Convert.ToString(segment.Text.Split('-')[1]).Trim() + "%' and Exch_CompID='" + Convert.ToString(comp.SelectedItem.Value).Trim() + "'");
            else
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + Convert.ToString(segment.Text.Split('-')[0]).Trim() + "%'  and Exch_CompID='" + Convert.ToString(comp.SelectedItem.Value).Trim() + "'");
            exchangesegmentid = Convert.ToString(dtexchange.Rows[0]["Exch_InternalID"]);


            OContract_RegistrationBL.Insert_ContactRegistration(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToString(comp.SelectedItem.Value), segment.Text.Trim(),
                                            dtRegDate, dtBusDate, dtSupDate, txtSuspension.Text.Trim(), txtClientUcc.Text.Trim(),
                                            Convert.ToString(Session["userid"]), txtSubBroker_hidden.Value, txtDealer_hidden.Value,
                                            dtClosorDate, txtFOfficeBranch.Text.Trim(), txtFOfficeGroup.Text.Trim(), txtParticipantCode.Text.Trim(),
                                            txtBankCode.Text.Trim(), txtSchemeCode.Text.Trim(), ddlPatern.SelectedItem.Value, drpSttWap.SelectedItem.Value,
                                            Convert.ToString(ComboSwapMapinSebi.SelectedItem.Value), exchangesegmentid
                                                                                              );


            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 

            //------------------ Tier End----------------------------------



            gridExchange.CancelEdit();
            e.Cancel = true;


        }
        protected void gridExchange_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            Session["check"] = null;
            string InternalId = Convert.ToString(e.EditingKeyValue);
            DataTable dtEdit = oDBEngine.GetDataTable("tbl_master_contactExchange", "crg_company,crg_exchange,crg_STTPattern,crg_STTWap,isnull(crg_swapUCC,'N')", " crg_internalid=" + InternalId + "");
            if (dtEdit.Rows.Count > 0)
            {
                ASPxComboBox segment = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compSegment");
                DropDownList ddlPatern = (DropDownList)gridExchange.FindEditFormTemplateControl("ddlPatern");
                DropDownList drpSttWap = (DropDownList)gridExchange.FindEditFormTemplateControl("drpSttWap");
                ASPxComboBox ComboSwapMapinSebi = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("ComboSwapMapinSebi");
                SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + ' - ' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Convert.ToString(dtEdit.Rows[0][0]) + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                segment.DataBind();
                valueData = Convert.ToString(dtEdit.Rows[0][1]) + "~" + ddlPatern.ClientID + "~" + Convert.ToString(dtEdit.Rows[0][2]) + "~" + drpSttWap.ClientID + "~" + Convert.ToString(dtEdit.Rows[0][3]) + "~" + Convert.ToString(dtEdit.Rows[0][4]);
            }
            if (Convert.ToString(Session["userlastsegment"]) != "1")
            {
                ASPxComboBox company = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compCompany");
                ASPxComboBox segment = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compSegment");
                segment.ClientEnabled = false;
                company.ClientEnabled = false;
            }

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        protected void gridExchange_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string strWhrereClause = "crg_cntID=" + "'" + Convert.ToString(Session["KeyVal_InternalID"]) + "' AND LogModifyUser=-1";
            string strSetClause = "LogModifyUser=" + Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.GenericMethod objGM = new BusinessLogicLayer.GenericMethod();
            int i = objGM.Update_Table("tbl_master_contactExchange_Log", strSetClause, strWhrereClause);

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        protected void gridExchange_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }
        protected void gridExchange_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string InternalId = Convert.ToString(e.Keys[0]);
            string exchangesegmentid = null;
            ASPxComboBox comp = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compCompany");
            ASPxComboBox segment = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compSegment");
            TextBox txtClientUcc = (TextBox)gridExchange.FindEditFormTemplateControl("txtClientUcc");
            ASPxDateEdit dtRegistration = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtRegistration");
            ASPxDateEdit dtBusienssCommDate = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtBusienssCommDate");
            ASPxDateEdit dtSuspension = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtSuspension");
            HiddenField txtSubBroker_hidden = (HiddenField)gridExchange.FindEditFormTemplateControl("txtSubBroker_hidden");
            HiddenField txtDealer_hidden = (HiddenField)gridExchange.FindEditFormTemplateControl("txtDealer_hidden");
            ASPxDateEdit dtCloser = (ASPxDateEdit)gridExchange.FindEditFormTemplateControl("dtCloser");
            TextBox txtFOfficeBranch = (TextBox)gridExchange.FindEditFormTemplateControl("txtFOfficeBranch");
            TextBox txtFOfficeGroup = (TextBox)gridExchange.FindEditFormTemplateControl("txtFOfficeGroup");
            TextBox txtParticipantCode = (TextBox)gridExchange.FindEditFormTemplateControl("txtParticipantCode");
            TextBox txtBankCode = (TextBox)gridExchange.FindEditFormTemplateControl("txtBankCode");
            TextBox txtSchemeCode = (TextBox)gridExchange.FindEditFormTemplateControl("txtSchemeCode");
            DropDownList ddlPatern = (DropDownList)gridExchange.FindEditFormTemplateControl("ddlPatern");
            TextBox txtSuspension = (TextBox)gridExchange.FindEditFormTemplateControl("txtSuspension");
            DropDownList drpSttWap = (DropDownList)gridExchange.FindEditFormTemplateControl("drpSttWap");
            ASPxComboBox ComboSwapMapinSebi = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("ComboSwapMapinSebi");
            SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + ' - ' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + comp.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
            segment.DataBind();
            if (dtRegistration.Value == null)
                dtRegistration.Value = DBNull.Value;
            if (dtBusienssCommDate.Value == null)
                dtBusienssCommDate.Value = DBNull.Value;
            if (dtSuspension.Value == null)
                dtSuspension.Value = DBNull.Value;
            if (dtCloser.Value == null)
                dtCloser.Value = DBNull.Value;

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
            /* For Tier Structure 

            SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("updateContactExchange", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@crg_internalId", SqlDbType.VarChar).Value = InternalId;
            lcmd.Parameters.Add("@crg_cntID", SqlDbType.VarChar).Value = Session["KeyVal_InternalID"].ToString();
            lcmd.Parameters.Add("@crg_company1", SqlDbType.VarChar).Value = comp.SelectedItem.Value;
            lcmd.Parameters.Add("@crg_exchange1", SqlDbType.VarChar).Value = segment.Text;
            //lcmd.Parameters.Add("@crg_regisDate", SqlDbType.DateTime).Value = dtRegistration.Value;

            if (dtRegistration.Value != null)
            {
                lcmd.Parameters.Add("@crg_regisDate", SqlDbType.DateTime).Value = dtRegistration.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_regisDate", DBNull.Value);
            }
            //lcmd.Parameters.Add("@crg_businessCmmDate", SqlDbType.DateTime).Value = dtBusienssCommDate.Value;
            if (dtBusienssCommDate.Value != null)
            {
                lcmd.Parameters.Add("@crg_businessCmmDate", SqlDbType.DateTime).Value = dtBusienssCommDate.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_businessCmmDate", DBNull.Value);
            }


            // lcmd.Parameters.Add("@crg_suspensionDate", SqlDbType.DateTime).Value = dtSuspension.Value;

            if (dtSuspension.Value != null)
            {
                lcmd.Parameters.Add("@crg_suspensionDate", SqlDbType.DateTime).Value = dtSuspension.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_suspensionDate", DBNull.Value);
            }

       


            lcmd.Parameters.Add("@crg_reasonforSuspension", SqlDbType.VarChar).Value = txtSuspension.Text;
            lcmd.Parameters.Add("@crg_tcode", SqlDbType.VarChar).Value = txtClientUcc.Text;
            lcmd.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(Session["userid"].ToString());
            lcmd.Parameters.Add("@crg_SubBrokerFranchiseeID", SqlDbType.VarChar).Value = txtSubBroker_hidden.Value;
            lcmd.Parameters.Add("@crg_Dealer", SqlDbType.VarChar).Value = txtDealer_hidden.Value;
            //lcmd.Parameters.Add("@crg_AccountClosureDate", SqlDbType.DateTime).Value = dtCloser.Value;
            if (dtCloser.Value != null)
            {
                lcmd.Parameters.Add("@crg_AccountClosureDate", SqlDbType.DateTime).Value = dtCloser.Value;
            }
            else
            {
                lcmd.Parameters.AddWithValue("@crg_AccountClosureDate", DBNull.Value);
            }

     

            lcmd.Parameters.Add("@crg_FrontOfficeBranchCode", SqlDbType.VarChar).Value = txtFOfficeBranch.Text;
            lcmd.Parameters.Add("@crg_FrontOfficeGroupCode", SqlDbType.VarChar).Value = txtFOfficeGroup.Text;
            lcmd.Parameters.Add("@crg_ParticipantSchemeCode", SqlDbType.VarChar).Value = txtParticipantCode.Text;
            lcmd.Parameters.Add("@crg_ClearingBankCode", SqlDbType.VarChar).Value = txtBankCode.Text;
            lcmd.Parameters.Add("@crg_SchemeCode", SqlDbType.VarChar).Value = txtSchemeCode.Text;
            lcmd.Parameters.Add("@crg_STTPattern", SqlDbType.VarChar).Value = ddlPatern.SelectedItem.Value;
            lcmd.Parameters.Add("@SttWap", SqlDbType.Char, 1).Value = drpSttWap.SelectedItem.Value;
            lcmd.Parameters.Add("@MapinSwapSebi", SqlDbType.Char, 1).Value = ComboSwapMapinSebi.Text;
            string exchange = segment.Text.Split('-')[0].ToString().Trim();
            string segmentid = null;
            DataTable dtexchange = new DataTable();
            if (segment.Text.Contains("-"))
                segmentid = segment.Text.Split('-')[1].ToString().Trim();
            if (segmentid != null)
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + segment.Text.Split('-')[0].ToString().Trim() + "%' and exch_segmentId like '%" + segment.Text.Split('-')[1].ToString().Trim() + "%' and Exch_CompID='" + comp.SelectedItem.Value.ToString().Trim() + "'");
            else
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + segment.Text.Split('-')[0].ToString().Trim() + "%'  and Exch_CompID='" + comp.SelectedItem.Value.ToString().Trim() + "'");
            exchangesegmentid = dtexchange.Rows[0]["Exch_InternalID"].ToString();
            lcmd.Parameters.AddWithValue("@crg_exchangesegment", exchangesegmentid);
            lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();

             */

            //--------------------- Tier Start---------------------------
            DateTime dtRegDate, dtBusDate, dtSupDate, dtClosorDate;

            if (dtRegistration.Value != null)
            {
                dtRegDate = Convert.ToDateTime(dtRegistration.Value);
            }
            else
            {
                dtRegDate = Convert.ToDateTime("01-01-1900");
            }

            if (dtBusienssCommDate.Value != null)
            {
                dtBusDate = Convert.ToDateTime(dtBusienssCommDate.Value);
            }
            else
            {
                dtBusDate = Convert.ToDateTime("01-01-1900");
            }

            if (dtSuspension.Value != null)
            {
                dtSupDate = Convert.ToDateTime(dtSuspension.Value);
            }
            else
            {
                dtSupDate = Convert.ToDateTime("01-01-1900");
            }

            if (dtCloser.Value != null)
            {
                dtClosorDate = Convert.ToDateTime(dtCloser.Value);
            }
            else
            {
                dtClosorDate = Convert.ToDateTime("01-01-1900");
            }

            string exchange = Convert.ToString(segment.Text.Split('-')[0]).Trim();
            string segmentid = null;
            DataTable dtexchange = new DataTable();
            if (segment.Text.Contains("-"))
                segmentid = Convert.ToString(segment.Text.Split('-')[1]).Trim();
            if (segmentid != null)
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + Convert.ToString(segment.Text.Split('-')[0]).Trim() + "%' and exch_segmentId like '%" + Convert.ToString(segment.Text.Split('-')[1]).Trim() + "%' and Exch_CompID='" + Convert.ToString(comp.SelectedItem.Value).Trim() + "'");
            else
                dtexchange = oDBEngine.GetDataTable("Select Exch_InternalID ,Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName and exh_shortName like '%" + Convert.ToString(segment.Text.Split('-')[0]).Trim() + "%'  and Exch_CompID='" + Convert.ToString(comp.SelectedItem.Value).Trim() + "'");
            exchangesegmentid = Convert.ToString(dtexchange.Rows[0]["Exch_InternalID"]);


            OContract_RegistrationBL.Update_ContactRegistration(InternalId, Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToString(comp.SelectedItem.Value), segment.Text.Trim(),
                                            dtRegDate, dtBusDate, dtSupDate, txtSuspension.Text.Trim(), txtClientUcc.Text.Trim(),
                                            Convert.ToString(Session["userid"]), txtSubBroker_hidden.Value, txtDealer_hidden.Value,
                                            dtClosorDate, txtFOfficeBranch.Text.Trim(), txtFOfficeGroup.Text.Trim(), txtParticipantCode.Text.Trim(),
                                            txtBankCode.Text.Trim(), txtSchemeCode.Text.Trim(), ddlPatern.SelectedItem.Value, drpSttWap.SelectedItem.Value,
                                           ComboSwapMapinSebi.Text, exchangesegmentid);




            //------------------ Tier End----------------------------------

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 

            gridExchange.CancelEdit();
            e.Cancel = true;
        }
        protected void gridExchange_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpValue"] = valueData;
        }
        protected void gridExchange_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            // .............................Code Commented and Added by Sanjib on 21122016 to use Convert.tostring instead of tostring(). ................
            if (gridExchange.IsNewRowEditing)
            {
                if (Convert.ToString(Session["userlastsegment"]) != "1")
                {
                    ASPxComboBox company = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compCompany");
                    ASPxComboBox segment = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compSegment");
                    SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + ' - ' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" +Convert.ToString(Session["LastCompany"]) + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                    segment.DataBind();
                    //company.Value = Session["LastCompany"];
                    string[,] seg = oDBEngine.GetFieldValue("tbl_master_segment", "seg_name", " seg_id='" + Convert.ToString(HttpContext.Current.Session["userlastsegment"]) + "'", 1);
                    if (seg[0, 0] != "n")
                    {
                        string[] Segname = seg[0, 0].Split('-');
                        if (Segname.Length > 1)
                        {
                            string segmentName = Convert.ToString(Segname[0]) + " - " + Convert.ToString(Segname[1]);
                            segment.Text = segmentName;
                        }
                        else
                        {
                            segment.Text = seg[0, 0];
                        }
                    }
                    segment.ClientEnabled = false;
                    company.ClientEnabled = false;
                    valueData = "Show~" + Convert.ToString(Session["LastCompany"]);

                }



            }
            else
            {
                ((ASPxComboBox)gridExchange.FindEditFormTemplateControl("compCompany")).Enabled = false;
            }
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void gridExchange_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string companyID = Convert.ToString(Session["KeyVal_InternalID"]);
            string Ucc = "";
            string eid = "";
            string eid1 = "";
            string company = "";
            string exchange = "";
            ASPxComboBox comp = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compCompany");
            ASPxComboBox segment = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compSegment");
            TextBox txtClientUcc = (TextBox)gridExchange.FindEditFormTemplateControl("txtClientUcc");

            try
            {

                Ucc = Convert.ToString(e.NewValues["crg_tcode"]);
                company = Convert.ToString(e.NewValues["crg_company1"]);
                exchange = Convert.ToString(e.NewValues["crg_exchange"]);



            }
            catch
            {

            }



            if (Ucc == "")
            {
                return;
            }

            if (Session["check"] != null)
            {

                DataTable dtCX = oDBEngine.GetDataTable("tbl_master_contactexchange,tbl_master_contact", "*", "crg_company='" + comp.SelectedItem.Value + "' and crg_exchange='" + segment.Text + "' and  crg_tcode='" + txtClientUcc.Text + "'");
                // DataTable dtCX1 = oDBEngine.GetDataTable("tbl_master_contactexchange", "*", "crg_company='" + comp.SelectedItem.Value + "' and crg_exchange='" + segment.Text + "'");// and  crg_tcode='" + txtClientUcc.Text + "'");
                // if(dtCX.Rows.Count>0)
                //{

                ////  Session["check"] = null;
                string[,] id = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_internalId,crg_cntId", "crg_company='" + comp.SelectedItem.Value + "' and crg_exchange='" + segment.Text + "' and crg_tcode='" + Ucc + "'", 2);
                string[,] exch = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_exchange", " crg_company='" + comp.SelectedItem.Value + "' and crg_exchange='" + segment.Text + "' and crg_cntID='" + companyID + "'", 1);
                if (exch[0, 0] != "n")
                {
                    string[] sname = exch[0, 0].Split('-');
                    string[,] shortName = oDBEngine.GetFieldValue("tbl_master_exchange", "exh_shortName", " exh_shortName='" + sname[0].Trim() + "'", 1);
                    if (shortName[0, 0] != "n")
                    //if(dtCX1.Rows.Count>0)
                    {
                        e.RowError = "This Combination Already Exists";
                        return;
                    }
                }
                DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode,crg_cntid,cnt_internalid", "cnt_ucc='" + Ucc + "' or crg_tcode='" + Ucc + "' and cnt_internalid = crg_cntid");
                if (dt1.Rows.Count > 0)
                {
                    if (id[0, 0] != "n")
                    {
                        if (id[0, 1] != companyID)
                        {
                            eid = id[0, 1];
                            if (eid != "")
                            //if(dtCX.Rows.Count>0)
                            {
                                e.RowError = "This UCC Already Exists";
                                return;
                            }
                        }
                    }
                }


                if (dtCX.Rows.Count > 0)
                {
                    e.RowError = "Already Exists";
                    return;
                }

            }
            else
            {
                string UCC1 = "";
                string company1 = "";
                string exchange1 = "";
                try
                {
                    UCC1 = Convert.ToString(e.OldValues["crg_tcode"]);
                    company1 = Convert.ToString(e.OldValues["crg_company1"]);
                    exchange1 = Convert.ToString(e.OldValues["crg_exchange"]);
                }
                catch
                {

                }

                if (Ucc == UCC1)
                {
                }
                else
                {
                    //DataTable dt2 = oDBEngine.GetDataTable("tbl_master_contactExchange", "crg_cntId", "crg_cntID!='" + companyID + "' and crg_company!='" + comp.SelectedItem.Value + "' and crg_exchange!='" + segment.Text + "'");
                    string[,] id = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_internalId,crg_cntId", "crg_company='" + comp.SelectedItem.Value + "' and crg_exchange='" + segment.Text + "' and crg_tcode='" + Ucc + "'", 2);
                    string[,] exch = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_exchange,crg_cntId", " crg_company='" + comp.SelectedItem.Value + "' and crg_exchange='" + segment.Text + "' and crg_cntID='" + companyID + "'", 2);
                    string[,] id1 = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_internalId,crg_cntId", "crg_company!='" + comp.SelectedItem.Value + "' and crg_exchange!='" + segment.Text + "' and crg_tcode!='" + Ucc + "' and crg_cntID='" + companyID + "'", 2);
                    if (exch[0, 0] != "n")
                    {
                        //if(exch[0, 1] !=compSegment)
                        //{
                        string[] sname = exch[0, 0].Split('-');
                        string[,] shortName = oDBEngine.GetFieldValue("tbl_master_exchange,tbl_master_contactExchange", "exh_shortName,crg_cntID", " exh_shortName='" + sname[0].Trim() + "' and crg_company!='" + comp.SelectedItem.Value + "' and crg_exchange!='" + segment.Text + "' and crg_cntID='" + companyID + "'", 2);
                        if (shortName[0, 0] != "n")
                        {

                            //if (dt2.Rows.Count>0)
                            if (id1[0, 1] != companyID)
                            {
                                e.RowError = "This Combination Already Exists";
                                return;
                                //}
                            }
                        }
                    }
                    // DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode,crg_cntid,cnt_internalid", "cnt_ucc='" + Ucc + "' or crg_tcode='" + Ucc + "' and cnt_internalid = crg_cntid ");
                    DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode,crg_cntid,cnt_internalid", "(cnt_ucc='" + Ucc + "' or crg_tcode='" + Ucc + "') and cnt_internalid = crg_cntid ");
                    if (dt1.Rows.Count > 0)
                    {
                        if (id[0, 0] != "n")
                        {
                            if (id[0, 1] != companyID)
                            {
                                eid = id[0, 1];
                                if (eid != "")
                                //if(dtCX.Rows.Count>0)
                                {
                                    e.RowError = "This UCC Already Exists";
                                    return;
                                }
                            }
                        }
                    }
                }

            }
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        // }
        protected void gridExchange_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            Session["check"] = "a";
            string[,] id = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_UCC", "cnt_internalId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "'", 1);
            if (id[0, 0] != "n")
            {
                e.NewValues["crg_tcode"] = id[0, 0];
            }
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void compSegment_Callback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox comp = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compCompany");
            ASPxComboBox segment = (ASPxComboBox)gridExchange.FindEditFormTemplateControl("compSegment");
            SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + ' - ' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + comp.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
            segment.DataBind();
        }

        #endregion

        protected void gridMembership_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            string Membershipid = Convert.ToString(e.Keys[0]);
            string validitytype = "";
            validitytype = Convert.ToString(e.NewValues["crg_validityType"]);
            if (validitytype == "Lifetime")
            {
                int i = oDBEngine.SetFieldValue("tbl_master_contactMembership", " crg_memExpDate= null   ", " crg_internalid ='" + Membershipid + "'");

            }
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        //Purpose: User rights in Statutory grid and  membership grid 
        //Name : Debjyoti 16-11-2016

        protected void gridMembership_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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

        //End : debjyoti 16-11-2016
        [WebMethod]
        public static bool CheckUniqueNumber(string CrgNumber, string procode)
        {

            BusinessLogicLayer.Contract_RegistrationBL CRB = new BusinessLogicLayer.Contract_RegistrationBL();
          
            bool IsPresent = false;
          

               
            //procode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);

            procode = Convert.ToString(procode);

            if (procode == "0" || procode=="")
            {
                IsPresent = CRB.CheckUniqueRegDocNumber(procode, CrgNumber, 0);
            }
            else
            {
                IsPresent = CRB.CheckUniqueRegDocNumber(procode, CrgNumber, 1);
            }


            return IsPresent;
        }

        [WebMethod]
        public static string CheckUniqueNumberRetCustomername(string CrgNumber, string procode)
        {

            BusinessLogicLayer.Contract_RegistrationBL CRB = new BusinessLogicLayer.Contract_RegistrationBL();

           // bool IsPresent = false;
            string CusName = String.Empty;


            //procode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);

            procode = Convert.ToString(procode);

            if (procode == "0" || procode == "")
            {
                CusName = CRB.CheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 0);
            }
            else
            {
                CusName = CRB.CheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 1);
            }


            return CusName;
        }
        [WebMethod]
        public static string CheckUniqueNumberRetInfluencername(string CrgNumber, string procode)
        {

            BusinessLogicLayer.Contract_RegistrationBL CRB = new BusinessLogicLayer.Contract_RegistrationBL();

            // bool IsPresent = false;
            string CusName = String.Empty;


            //procode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);

            procode = Convert.ToString(procode);

            if (procode == "0" || procode == "")
            {
                CusName = CRB.CheckUniqueRegDocNumberRetInfluencerName(procode, CrgNumber, 0);
            }
            else
            {
                CusName = CRB.CheckUniqueRegDocNumberRetInfluencerName(procode, CrgNumber, 1);
            }


            return CusName;
        }
        [WebMethod]
        public static string SettingsCheckUniqueNumberRetCustomername(string CrgNumber, string procode)
        {

            BusinessLogicLayer.Contract_RegistrationBL CRB = new BusinessLogicLayer.Contract_RegistrationBL();
           
            // bool IsPresent = false;
            string CusName = String.Empty;


            //procode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);

            procode = Convert.ToString(procode);

            if (procode == "0" || procode == "")
            {
                CusName = CRB.SettingsCheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 0);
            }
            else
            {
                CusName = CRB.SettingsCheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 1);
            }


            return CusName;
        }


        [WebMethod]
        public static string SettingsWisecheckCustomerPanNameExists(string CrgNumber, string procode)
        {

            BusinessLogicLayer.Contract_RegistrationBL CRB = new BusinessLogicLayer.Contract_RegistrationBL();
             CommonBL ComBL = new CommonBL();
            string duplicate_PAN_Number = ComBL.GetSystemSettingsResult("duplicate_PAN_Number");
            // bool IsPresent = false;
            string CusName = String.Empty;


            //procode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);

            procode = Convert.ToString(procode);
            // Rev Sanchita
            //if (duplicate_PAN_Number.ToUpper() == "YES")
            //{
            //    if (procode == "0" || procode == "")
            //    {
            //        CusName = CRB.SettingsCheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 0);
            //    }
            //    else
            //    {
            //        CusName = CRB.SettingsCheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 1);
            //    }
            //}
            //else
            //{
            //    if (procode == "0" || procode == "")
            //    {
            //        CusName = CRB.CheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 0);
            //    }
            //    else
            //    {
            //        CusName = CRB.CheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 1);
            //    }
            //}

            if (duplicate_PAN_Number.ToUpper() == "NO")
            {
                if (procode == "0" || procode == "")
                {
                    CusName = CRB.SettingsCheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 0);
                }
                else
                {
                    CusName = CRB.SettingsCheckUniqueRegDocNumberRetCustomerName(procode, CrgNumber, 1);
                }
            }
            // End of Rev Sanchita

            return CusName;
        }

    }
}