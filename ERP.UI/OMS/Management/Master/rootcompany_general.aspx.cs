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
using System.Data.SqlClient;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{

    public partial class management_master_rootcompany_general : ERP.OMS.ViewState_class.VSPage
    {
        Int32 ID;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        BusinessLogicLayer.Company ORootCompaniesGeneralBL = new BusinessLogicLayer.Company();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        clsDropDownList OclsDropDownList = new clsDropDownList();

        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        protected void Page_Init(object sender, EventArgs e)
        {

            drCategory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            Session["requesttype"] = "Companies";
            Session["ContactType"] = "Companies";
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                //Debjyoti 23-12-2016
                //Reason: UDF count
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                //End Debjyoti 23-12-2016
                

                txtCINVdate.EditFormatString = OConvert.GetDateFormat("Date");
                txtLocalVdate.EditFormatString = OConvert.GetDateFormat("Data");
                txtincorporateDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtcstVdate.EditFormatString = OConvert.GetDateFormat("Date");

                //txtErpValidFrom.EditFormatString = OConvert.GetDateFormat("Date");
                //txtErpValidUpto.EditFormatString = OConvert.GetDateFormat("");
                //txtESICValidFrom.EditFormatString = OConvert.GetDateFormat("Date");
                //txtESICValidUpto.EditFormatString = OConvert.GetDateFormat("Date");
               

                ShowForm();
            }

            //btnSave.Attributes.Add("Onclick", "Javascript:return ValidateGeneral();");
        }
        private void ShowForm()
        {
            string[,] schemaOnRoll = oDBEngine.GetFieldValue("tbl_master_idschema", "Id, SchemaName", "type_id = 5 and IsActive = 1", 2);
            if (schemaOnRoll[0, 0] != "n")
            {
                OclsDropDownList.AddDataToDropDownList(schemaOnRoll, drpdwn_schema_on);
                drpdwn_schema_on.Items.Insert(0, new ListItem("----Select----", ""));
            }

            //DataTable dtdeduct = oDBEngine.GetDataTable("select isnull(deductcategory_description,'') deductcategory_description from Tbl_master_deductorcategory");
            
            //drdCategory.Items.Add(dtdeduct);
         
            string[,] schemaOffRoll = oDBEngine.GetFieldValue("tbl_master_idschema", "Id, SchemaName", "type_id = 4 and IsActive = 1", 2);
            if (schemaOnRoll[0, 0] != "n")
            {
                OclsDropDownList.AddDataToDropDownList(schemaOffRoll, drpdwn_schema_off);
                drpdwn_schema_off.Items.Insert(0, new ListItem("----Select----", ""));
            }

            if (Request.QueryString["id"] != "ADD")
            {
                Label1.Text = "Update";
                if (Request.QueryString["id"] != null)
                {
                    ID = Int32.Parse(Request.QueryString["id"]);
                    HttpContext.Current.Session["KeyVal"] = ID;
                }


                //DataTable deductshow = oDBEngine.GetDataTable("select dedcmp.deductcategory_description  Description from tbl_master_company cmpny left join Tbl_master_deductorcategory dedcmp on dedcmp.deductcategory_id=cmpny.deductcategory_id where cmpny.cmp_id=" + Int32.Parse(Request.QueryString["id"]) + "");
                string[,] InternalId;

                if (ID != 0)
                {
                    InternalId = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalId", "cmp_id=" + ID, 1);
                }
                else
                {
                    InternalId = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalId", "cmp_id=" + HttpContext.Current.Session["KeyVal"], 1);
                }
                HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                Keyval_internalId.Value = InternalId[0, 0];
                string[,] ContactData;
                if (ID != 0)
                {
                    // Code  Added and Commented By Priti on 15122016 to for edit

                    //ContactData = oDBEngine.GetFieldValue("tbl_master_company",
                    //    //"cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, convert(varchar(10),cnt_DOB,103) as cnt_DOB, convert(varchar(10),cnt_anniversaryDate,103) as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus",
                    //                        "distinct cmp_id,cmp_internalid,cmp_Name, cmp_parentid,cmp_natureOfBusiness,cmp_directors,cmp_authorizedSignatories,cmp_exchange,cmp_registrationNo, cmp_sebiRegnNo, cmp_panNo,cmp_serviceTaxNo,cmp_salesTaxNo, cmp_CIN,case cmp_CINdt when '1/1/1900 12:00:00 AM' then null else cmp_CINdt end as cmp_CINdt,case cmp_VregisNo when '1/1/1900 12:00:00 AM' then null else cmp_VregisNo end as cmp_VregisNo,case cmp_VPanNo when '1/1/1900 12:00:00 AM' then null else cmp_VPanNo end as cmp_VPanNo,case cmp_DateIncorporation when '1/1/1900 12:00:00 AM' then null else cmp_DateIncorporation end as cmp_DateIncorporation,cmp_OffRoleShortName,cmp_OnRoleShortName,cmp_currencyid,cmp_KYCPrefix,cmp_KRAIntermediaryID,cmp_vat_no",
                    //                        " cmp_id=" + ID, 24);
                    ContactData = oDBEngine.GetFieldValue("tbl_master_company",
                    "distinct cmp_id,cmp_internalid,cmp_Name, cmp_parentid,cmp_natureOfBusiness,cmp_directors,cmp_authorizedSignatories,cmp_exchange,cmp_registrationNo,"+ 
                    " cmp_sebiRegnNo, cmp_panNo,cmp_serviceTaxNo,cmp_salesTaxNo, cmp_CIN,case cmp_CINdt when '1/1/1900 12:00:00 AM' then null else cmp_CINdt end as cmp_CINdt,"+
                    " case cmp_VregisNo when '1/1/1900 12:00:00 AM' then null else cmp_VregisNo end as cmp_VregisNo,case cmp_VPanNo when '1/1/1900 12:00:00 AM' then null else"+
                    " cmp_VPanNo end as cmp_VPanNo,case cmp_DateIncorporation when '1/1/1900 12:00:00 AM' then null else cmp_DateIncorporation end as cmp_DateIncorporation," +
                    " cmp_OffRoleShortName,cmp_OnRoleShortName,cmp_currencyid,cmp_KYCPrefix,cmp_KRAIntermediaryID,cmp_vat_no,cmp_EPFRegistrationNo,"+
                    " case cmp_EPFRegistrationNoValidfrom when '1/1/1900 12:00:00 AM' then null else cmp_EPFRegistrationNoValidfrom end as cmp_EPFRegistrationNoValidfrom,"+
                    " case cmp_EPFRegistrationNoValidupto when '1/1/1900 12:00:00 AM' then null else cmp_EPFRegistrationNoValidupto end as cmp_EPFRegistrationNoValidupto,"+
                    " cmp_ESICRegistrationNo,case cmp_ESICRegistrationNoValidfrom when '1/1/1900 12:00:00 AM' then null else cmp_ESICRegistrationNoValidfrom end as cmp_ESICRegistrationNoValidfrom,"+
                    " case cmp_ESICRegistrationNoValidupto when '1/1/1900 12:00:00 AM' then null else cmp_ESICRegistrationNoValidupto end as cmp_ESICRegistrationNoValidupto,"+
                    " onrole_schema_id, offrole_schema_id,deductcat_value,cmp_gstin,MSME_UdyamRCNo", " cmp_id=" + ID, 35);
                }
                else
                {
                    ContactData = oDBEngine.GetFieldValue("tbl_master_company",
                        //"cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, convert(varchar(10),cnt_DOB,103) as cnt_DOB, convert(varchar(10),cnt_anniversaryDate,103) as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus",
                        "distinct cmp_id,cmp_internalid,cmp_Name, cmp_parentid,cmp_natureOfBusiness,cmp_directors,cmp_authorizedSignatories,cmp_exchange,cmp_registrationNo,"+
                        " cmp_sebiRegnNo, cmp_panNo,cmp_serviceTaxNo,cmp_salesTaxNo, cmp_CIN,case cmp_CINdt when '1/1/1900 12:00:00 AM' then null else cmp_CINdt end as cmp_CINdt," +
                        " case cmp_VregisNo when '1/1/1900 12:00:00 AM' then null else cmp_VregisNo end as cmp_VregisNo,case cmp_VPanNo when '1/1/1900 12:00:00 AM' " +
                        " then null else cmp_VPanNo end as cmp_VPanNo,case cmp_DateIncorporation when '1/1/1900 12:00:00 AM' then null else cmp_DateIncorporation end " +
                        " as cmp_DateIncorporation,cmp_OffRoleShortName,cmp_OnRoleShortName,cmp_currencyid,cmp_KYCPrefix,cmp_KRAIntermediaryID,cmp_vat_no,cmp_EPFRegistrationNo," +
                        " case cmp_EPFRegistrationNoValidfrom when '1/1/1900 12:00:00 AM' then null else cmp_EPFRegistrationNoValidfrom end as cmp_EPFRegistrationNoValidfrom," +
                        " case cmp_EPFRegistrationNoValidupto when '1/1/1900 12:00:00 AM' then null else cmp_EPFRegistrationNoValidupto end as cmp_EPFRegistrationNoValidupto," +
                        " cmp_ESICRegistrationNo,case cmp_ESICRegistrationNoValidfrom when '1/1/1900 12:00:00 AM' then null else cmp_ESICRegistrationNoValidfrom end as " +
                        " cmp_ESICRegistrationNoValidfrom, case cmp_ESICRegistrationNoValidupto when '1/1/1900 12:00:00 AM' then null else cmp_ESICRegistrationNoValidupto " +
                        " end as cmp_ESICRegistrationNoValidupto, onrole_schema_id, offrole_schema_id,deductcat_value,cmp_gstin,MSME_UdyamRCNo",
                        " cmp_id=" + HttpContext.Current.Session["KeyVal"], 35);
                }

                //____________ Value BINDING and Allocation _______________//
                DataSet dsCrystal = new DataSet();
                string[] strSpParam = new string[1];
                if (ID != 0)
                {
                    strSpParam[0] = "cmpid|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|10|" + Request.QueryString["id"] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[0] = "cmpid|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|10|" + HttpContext.Current.Session["KeyVal"] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }

                BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
                dsCrystal = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Hr_GetCompanySubTree");

                string[,] parent = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalid,cmp_Name", "cmp_internalid not in (" + (dsCrystal.Tables[0].Rows[0][0].ToString()) + ")", 2);
                if (parent[0, 0] != "n")
                {
                    try
                    {
                        OclsDropDownList.AddDataToDropDownList(parent, drpParentComp, true);
                        drpParentComp.SelectedValue = ContactData[0, 3];
                    }
                    catch { }
                }
                string[,] currency = oDBEngine.GetFieldValue("Master_Currency", "Currency_id,Currency_Name + '  ['+ ltrim(rtrim(Currency_AlphaCode)) + '   '+Currency_Symbol +']'", null, 2);
                if (currency[0, 0] != "n")
                {
                    try
                    {
                        OclsDropDownList.AddDataToDropDownList(currency, ddlcurrency);
                        ddlcurrency.SelectedValue = ContactData[0, 20];
                    }
                    catch { }
                }
                if (currency[0, 0] != "n")
                {
                    try 
                    {
                        drpdwn_schema_on.SelectedValue = ContactData[0, 30];
                        drpdwn_schema_off.SelectedValue = ContactData[0, 31];
                    }
                    catch { }
                }
                //if (Convert.ToString(dtdeduct)!="")
                //{
                    try
                    {
                        //drdCategory.Text = ContactData[0, 32];
                        drdCategory.Value = ContactData[0, 32];
                    }
                    catch { }

               // }
                try
                {
                    txtCompname.Text = ContactData[0, 2];
                    Session["CompanyName"] = ContactData[0, 2];
                    Session["Name"] = ContactData[0, 2];
                    txtNameofBusiness.Text = ContactData[0, 4];
                    txtDirectors.Text = ContactData[0, 5];
                    txtAuthorised.Text = ContactData[0, 6];
                    txtRegnNo.Text = ContactData[0, 8];
                    if (ContactData[0, 15] != "")
                    {
                        txtcstVdate.Value = Convert.ToDateTime(ContactData[0, 15]);
                    }
                    txtPanNo.Text = ContactData[0, 10];
                    if (ContactData[0, 16] != "")
                    {
                        txtLocalVdate.Value = Convert.ToDateTime(ContactData[0, 16]);
                    }
                    txtservicetaxNo.Text = ContactData[0, 11];
                    txtlocalSalesTax.Text = ContactData[0, 12];
                    txtCIN.Text = ContactData[0, 13];
                    txtOffRole.Text = ContactData[0, 18];
                    txtOnRole.Text = ContactData[0, 19];
                    if (ContactData[0, 14] != "")
                    {
                        txtCINVdate.Value = Convert.ToDateTime(ContactData[0, 14]);
                    }
                    if (ContactData[0, 17] != "")
                    {
                        txtincorporateDate.Value = Convert.ToDateTime(ContactData[0, 17]);
                    }
                    txtKraPrefix.Text = ContactData[0, 21];
                    txtKraintermideatoryid.Text = ContactData[0, 22];

                    txtVatRegNo.Text = ContactData[0, 23];
                   // txtTinNo.Text = ContactData[0, 24];
                    txtErpRegistration.Text = ContactData[0, 24];
                    if (ContactData[0, 25] != "")
                    {
                        txtErpValidFrom.Value = Convert.ToDateTime(ContactData[0, 25]);
                    }
                    if (ContactData[0, 26] != "")
                    {
                        txtErpValidUpto.Value = Convert.ToDateTime(ContactData[0, 26]);
                    }
                    txtESIC.Text = ContactData[0, 27];
                    if (ContactData[0, 28] != "")
                    {
                        txtESICValidFrom.Value = Convert.ToDateTime(ContactData[0, 28]);
                    }
                    if (ContactData[0, 29] != "")
                    {
                        txtESICValidUpto.Value = Convert.ToDateTime(ContactData[0, 29]);
                    }

                    // Debjyoti Gstin changes
                    string GSTIN = "";
                    if (ContactData[0, 33] != "")
                    {
                        GSTIN = ContactData[0, 33];
                        txtGSTIN1.Text= GSTIN.Substring(0, 2);
                        txtGSTIN2.Text = GSTIN.Substring(2, 10);
                        txtGSTIN3.Text = GSTIN.Substring(12,3);
                    }

                    if(ContactData[0, 32] !="")
                    {
                        drdCategory.Value = ContactData[0, 32];
                    }

                    txtMSMEUdyamRCNo.Text = ContactData[0, 34];
                }

                catch { }
            }
            else
            {
                Label1.Text = "Add";
                Session["requesttype"] = "";
                Session["ContactType"] = "";
                string[,] parent = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalid,cmp_Name", null, 2);

                if (parent[0, 0] != "n")
                {
                    OclsDropDownList.AddDataToDropDownList(parent, drpParentComp, true);
                }

                string[,] currency = oDBEngine.GetFieldValue("Master_Currency", "Currency_id,Currency_Name + '  ['+ ltrim(rtrim(Currency_AlphaCode)) + '   '+Currency_Symbol +']'", null, 2);
                if (currency[0, 0] != "n")
                {
                    OclsDropDownList.AddDataToDropDownList(currency, ddlcurrency);
                }

                HttpContext.Current.Session["KeyVal"] = 0;
                TabVisibility(false);
                Keyval_internalId.Value = "Add";
            }
        }

        private void TabVisibility(bool visible)
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Enabled = visible;
            page = ASPxPageControl1.TabPages.FindByName("Exchange Segment");
            page.Enabled = visible;
            page = ASPxPageControl1.TabPages.FindByName("DP Memberships");
            page.Enabled = visible;
            page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Enabled = visible;
            page = ASPxPageControl1.TabPages.FindByName("UDF");
            page.Enabled = visible;
            page = ASPxPageControl1.TabPages.FindByName("Logo");
            page.Enabled = visible;
          //  btnUdf.Enabled = visible;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime dtInc, dtCin, dtVreg, dtVPan, dtErpValidFrom, dtErpValidUpto, dtESICValidFrom, dtESICValidUpto;

            //DataTable deduct = oDBEngine.GetDataTable("select deductcategory_id  from Tbl_master_deductorcategory where deductcategory_value='" +drdCategory.Value+"'");
            if (ddlcurrency.SelectedItem.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>jAlert('Please Select Currency !');</script>");
            }
            else
            {
                if (int.Parse(Convert.ToString(HttpContext.Current.Session["KeyVal"])) != 0)  
                {
                    // Code  Added and Commented By Priti on 19122016 to add 6 field and validation for edit
                    if (txtErpValidFrom.Value != null)
                    {
                        if (txtErpRegistration.Text == "")
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript20", "<script language='javascript'>jAlert('Please Insert EPF Registration No !');</script>");
                            return;
                        }
                        dtErpValidFrom = Convert.ToDateTime(txtErpValidFrom.Value);
                    }
                    else
                    {
                        dtErpValidFrom = Convert.ToDateTime("01-01-1900");
                    }
                    if (txtErpValidUpto.Value != null)
                    {
                        if (txtErpRegistration.Text == "")
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript21", "<script language='javascript'>jAlert('Please Insert EPF Registration No !');</script>");
                            return;
                        }
                        dtErpValidUpto = Convert.ToDateTime(txtErpValidUpto.Value);
                    }
                    else
                    {
                        dtErpValidUpto = Convert.ToDateTime("01-01-1900");
                    }
                    //..code for compare two dates...
                    string messageErp = string.Empty;

                    if (dtErpValidUpto < dtErpValidFrom)
                    {
                        messageErp = "EPF Registration No. Valid Upto should be greater than Valid From";
                    }
                    if (dtErpValidUpto == dtErpValidFrom)
                    {
                        if (txtErpValidFrom.Text==string.Empty)
                        {
                            lblVF_erp.Visible = false;
                        }
                        else
                        {
                            messageErp = "EPF Registration No. Valid Upto should be greater than Valid From";
                        }
                      
                    }
                    if (messageErp != "")
                    {
                        //ClientScript.RegisterClientScriptBlock(typeof(string), "message", "alert('" + messageErp + "')", true);
                        lblVF_erp.Visible = true;
                        return;
                    }
                    else
                    {
                        lblVF_erp.Visible = false; 
                    }
                    
                    //..end...
                    if (txtESICValidFrom.Value != null)
                    {
                        if (txtESIC.Text == "")
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScriptesicvf", "<script language='javascript'>jAlert('Please Insert ESIC Registration No. !');</script>");
                            return;
                        }
                        dtESICValidFrom = Convert.ToDateTime(txtESICValidFrom.Value);
                    }
                    else
                    {
                        dtESICValidFrom = Convert.ToDateTime("01-01-1900");
                    }
                    if (txtESICValidUpto.Value != null)
                    {
                        if (txtESIC.Text == "")
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScriptesicvu", "<script language='javascript'>jAlert('Please Insert ESIC Registration No. !');</script>");
                            return;
                        }
                        dtESICValidUpto = Convert.ToDateTime(txtESICValidUpto.Value);
                    }
                    else
                    {
                        dtESICValidUpto = Convert.ToDateTime("01-01-1900");
                    }
                    //..code for compare two dates...
                    string message = string.Empty;
                    if (dtESICValidUpto < dtESICValidFrom)
                    {
                        
                            message = "ESIC Registration No. Valid Upto should be greater than Valid From";
                       
                      
                    }
                    if (dtESICValidUpto == dtESICValidFrom)
                    {
                        if (txtESICValidFrom.Text==string.Empty)
                        {
                            lblVUESIC.Visible = false;
                        }
                        else
                        {
                            message = "ESIC Registration No. Valid Upto should be greater than Valid From";
                        }
                    }
                    if (message != "")
                    {
                       // ClientScript.RegisterClientScriptBlock(typeof(string), "message", "alert('" + message + "')", true);
                        lblVUESIC.Visible = true;
                        return;
                    }
                    else
                    {
                        lblVUESIC.Visible = false;
                    }

                    //Debjyoti GSTIN Changes
                    string UPDATEDGSTIN = "";
                    UPDATEDGSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();

                    string MSMEUdyamRCNo = txtMSMEUdyamRCNo.Text;

                    string[,] originalparent = oDBEngine.GetFieldValue("tbl_master_company", "isnull(cmp_internalid,'')", "cmp_id=" + HttpContext.Current.Session["KeyVal"] + "", 1);
                    DateTime today = oDBEngine.GetDate();
                    //String value = "cmp_parentid='" + drpParentComp.SelectedValue + "',cmp_currencyid='" + ddlcurrency.SelectedItem.Value + 
                    //    "' ,cmp_Name='" + txtCompname.Text + "',  cmp_natureOfBusiness='" + txtNameofBusiness.Text + 
                    //    "', cmp_directors='" + txtDirectors.Text + "', cmp_authorizedSignatories='" + txtAuthorised.Text + 
                    //    "', cmp_registrationNo='" + txtRegnNo.Text + "', cmp_VregisNo='" + txtcstVdate.Value + 
                    //    "', cmp_panNo='" + txtPanNo.Text + "', cmp_serviceTaxNo='" + txtservicetaxNo.Text + 
                    //    "', cmp_salesTaxNo='" + txtlocalSalesTax.Text + "', cmp_CIN='" + txtCIN.Text + "', cmp_CINdt='" + 
                    //    txtCINVdate.Value + "', cmp_DateIncorporation='" + txtincorporateDate.Value + "',  cmp_VPanNo='" + 
                    //    txtLocalVdate.Value + "', lastModifyDate ='" + today + "',cmp_OffRoleShortName='" + txtOffRole.Text + 
                    //    "',cmp_OnRoleShortName='" + txtOnRole.Text + "', lastModifyUser='" + HttpContext.Current.Session["userid"].ToString() + 
                    //    "',cmp_KYCPrefix='" + txtKraPrefix.Text + "',cmp_KRAIntermediaryID='" + txtKraintermideatoryid.Text + "',cmp_vat_no='" + 
                    //    txtVatRegNo.Text.Trim() + "',cmp_EPFRegistrationNo='" + txtErpRegistration.Text.Trim() + "',cmp_EPFRegistrationNoValidfrom='" + 
                    //    txtErpValidFrom.Value + "',cmp_EPFRegistrationNoValidupto='" + txtErpValidUpto.Value + "',cmp_ESICRegistrationNo='" + txtESIC.Text.Trim() + 
                    //    "',cmp_ESICRegistrationNoValidfrom='" + txtESICValidFrom.Value + "',cmp_ESICRegistrationNoValidupto='"+txtESICValidUpto.Value+"'" +
                    //    ", onrole_schema_id=" + Convert.ToInt32(drpdwn_schema_on.SelectedValue) + ", offrole_schema_id=" + Convert.ToInt32(drpdwn_schema_off.SelectedValue); 

                    String value = "cmp_parentid='" + drpParentComp.SelectedValue + "',cmp_currencyid='" + ddlcurrency.SelectedItem.Value +
                       "' ,cmp_Name='" + txtCompname.Text + "',  cmp_natureOfBusiness='" + txtNameofBusiness.Text +
                       "', cmp_directors='" + txtDirectors.Text + "', cmp_authorizedSignatories='" + txtAuthorised.Text +
                       "', cmp_registrationNo='" + txtRegnNo.Text + "', cmp_VregisNo='" + txtcstVdate.Value +
                       "', cmp_panNo='" + txtPanNo.Text + "', cmp_serviceTaxNo='" + txtservicetaxNo.Text +
                       "', cmp_salesTaxNo='" + txtlocalSalesTax.Text + "', cmp_CIN='" + txtCIN.Text + "', cmp_CINdt='" +
                       txtCINVdate.Value + "', cmp_DateIncorporation='" + txtincorporateDate.Value + "',  cmp_VPanNo='" +
                       txtLocalVdate.Value + "', lastModifyDate ='" + today + "',cmp_OffRoleShortName='" + txtOffRole.Text +
                       "',cmp_OnRoleShortName='" + txtOnRole.Text + "', lastModifyUser='" + HttpContext.Current.Session["userid"].ToString() +
                       "',cmp_KYCPrefix='" + txtKraPrefix.Text + "',cmp_KRAIntermediaryID='" + txtKraintermideatoryid.Text + "',cmp_vat_no='" +
                       txtVatRegNo.Text.Trim() + "',cmp_EPFRegistrationNo='" + txtErpRegistration.Text.Trim() + "',cmp_EPFRegistrationNoValidfrom='" +
                      dtErpValidFrom + "',cmp_EPFRegistrationNoValidupto='" + dtErpValidUpto + "',cmp_ESICRegistrationNo='" + txtESIC.Text.Trim() +
                       "',cmp_ESICRegistrationNoValidfrom='" + dtESICValidFrom + "',cmp_ESICRegistrationNoValidupto='" + dtESICValidUpto + "'" +
                       ", onrole_schema_id=" + Convert.ToInt32(drpdwn_schema_on.SelectedValue) + ", offrole_schema_id=" + Convert.ToInt32(drpdwn_schema_off.SelectedValue) + ", deductcat_value='" + drdCategory.Value + "',cmp_gstin='" + UPDATEDGSTIN + "',MSME_UdyamRCNo='" + MSMEUdyamRCNo + "'";
                   //.......end..........
                    if (drpParentComp.SelectedValue != originalparent[0, 0])
                    {
                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_company", value, " cmp_id=" + HttpContext.Current.Session["KeyVal"]);
                        Response.Redirect("rootcompany_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecallal", "<script>alert('Parent Company Can not be Same Company');</script>");
                        drpParentComp.Focus();
                    }
                    //Response.Redirect("rootcompany_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);
                }
                else               //_________For Insurt
                {
                    try
                    {
                        //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                        //using (SqlConnection lcon = new SqlConnection(con))
                        //{
                        //    lcon.Open();
                        //    using (SqlCommand lcmdEmplInsert = new SqlCommand("CompanyInsert", lcon))
                        //    {
                        /*

                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_Name", txtCompname.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_natureOfBusiness", txtNameofBusiness.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_directors", txtDirectors.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_authorizedSignatories", txtAuthorised.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_registrationNo", txtRegnNo.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_panNo", txtPanNo.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_serviceTaxNo", txtservicetaxNo.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_salesTaxNo", txtlocalSalesTax.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_CIN", txtCIN.Text);
                        //if (txtDOB.Text != null)
                        if (txtincorporateDate.Value != null)
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_DateIncorporation", txtincorporateDate.Value);
                        }
                        else
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_DateIncorporation", "");
                        }
                        if (txtCINVdate.Value != null)
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_CINdt", txtCINVdate.Value);
                        }
                        else
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_CINdt", "");
                        }
                        if (txtcstVdate.Value != null)
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_VregisNo", txtcstVdate.Value);
                        }
                        else
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_VregisNo", "");
                        } if (txtLocalVdate.Value != null)
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_VPanNo", txtLocalVdate.Value);
                        }
                        else
                        {
                            lcmdEmplInsert.Parameters.AddWithValue("@cmp_VPanNo", "");
                        }
                        //___________For Returning InternalID_________//
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
                        parameter.Direction = ParameterDirection.Output;
                        ///_______________________________________________//

                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_parentid", drpParentComp.SelectedValue);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_currencyid", ddlcurrency.SelectedItem.Value);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_OffRoleShortName", txtOffRole.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_OnRoleShortName", txtOnRole.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_Kra_Prefix", txtKraPrefix.Text);
                        lcmdEmplInsert.Parameters.AddWithValue("@cmp_Kra_imid", txtKraintermideatoryid.Text);
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();

                        

                        //---- Tier Structure End------------------------------------

                          */

                        //----------------------------

                        //DateTime dtInc, dtCin, dtVreg, dtVPan, dtErpValidFrom, dtErpValidUpto, dtESICValidFrom, dtESICValidUpto;
                        if (txtincorporateDate.Value != null)
                        {
                            dtInc = Convert.ToDateTime(txtincorporateDate.Value);
                        }
                        else
                        {
                            dtInc = Convert.ToDateTime("01-01-1900");
                        }
                        if (txtCINVdate.Value != null)
                        {
                            dtCin = Convert.ToDateTime(txtCINVdate.Value);
                        }
                        else
                        {
                            dtCin = Convert.ToDateTime("01-01-1900");
                        }
                        if (txtcstVdate.Value != null)
                        {
                            dtVreg = Convert.ToDateTime(txtcstVdate.Value);
                        }
                        else
                        {
                            dtVreg = Convert.ToDateTime("01-01-1900");
                        }
                        if (txtLocalVdate.Value != null)
                        {
                            dtVPan = Convert.ToDateTime(txtLocalVdate.Value);
                        }
                        else
                        {
                            dtVPan = Convert.ToDateTime("01-01-1900");
                        }


                        // Code  Added and Commented By Priti on 15122016 to add 6 field
                       
                        if (txtErpValidFrom.Value != null)
                        {
                            if (txtErpRegistration.Text == "")
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript19", "<script language='javascript'>jAlert('Please Insert EPF Registration No !');</script>");
                                return;
                            }
                            dtErpValidFrom = Convert.ToDateTime(txtErpValidFrom.Value);
                        }
                        else
                        {
                            dtErpValidFrom = Convert.ToDateTime("01-01-1900");
                        }
                        if (txtErpValidUpto.Value != null)
                        {
                            if (txtErpRegistration.Text == "")
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript19", "<script language='javascript'>jAlert('Please Insert EPF Registration No !');</script>");
                                return;
                            }
                            dtErpValidUpto = Convert.ToDateTime(txtErpValidUpto.Value);
                        }
                        else
                        {
                            dtErpValidUpto = Convert.ToDateTime("01-01-1900");
                        }
                        //..code for compare two dates...
                        string messageErp = string.Empty;

                        if (dtErpValidUpto < dtErpValidFrom)
                        {
                            messageErp = "EPF Registration No. Valid Upto should be greater than Valid From";
                        }
                        if (dtErpValidUpto == dtErpValidFrom)
                        {
                            if (txtErpValidFrom.Text == string.Empty)
                            {

                                lblVF_erp.Visible = false;
                            }
                            else
                            {
                                messageErp = "EPF Registration No. Valid Upto should be greater than Valid From";  
                            }
                                                         
                        }
                       
                        if (messageErp != "")
                        {
                            //ClientScript.RegisterClientScriptBlock(typeof(string), "message", "alert('" + messageErp + "')", true);
                            lblVF_erp.Visible = true;
                            return;
                        }
                        else
                        {
                            lblVF_erp.Visible = false;
                        }
                        //..end...
                        if (txtESICValidFrom.Value != null)
                        {
                            if (txtESIC.Text == "")
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript19", "<script language='javascript'>jAlert('Please Insert ESIC Registration No. !');</script>");
                                return;
                            }
                           
                            dtESICValidFrom = Convert.ToDateTime(txtESICValidFrom.Value);
                        }
                        else
                        {
                            dtESICValidFrom = Convert.ToDateTime("01-01-1900");
                        }
                        if (txtESICValidUpto.Value != null)
                        {
                            if (txtESIC.Text == "")
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript19", "<script language='javascript'>jAlert('Please Insert ESIC Registration No. !');</script>");
                                return;
                            }
                            dtESICValidUpto = Convert.ToDateTime(txtESICValidUpto.Value);
                        }
                        else
                        {
                            dtESICValidUpto = Convert.ToDateTime("01-01-1900");
                        }
                        //..code for compare two dates...
                        string message = string.Empty;
                        if (dtESICValidUpto < dtESICValidFrom)
                        {
                            message = "ESIC Registration No. Valid Upto should be greater than Valid From";
                        }
                        if (dtESICValidUpto == dtESICValidFrom)
                        {
                            if (txtESICValidFrom.Text ==string.Empty)
                            {
                                lblVUESIC.Visible = false;
                            }
                            else
                            {
                                message = "ESIC Registration No. Valid Upto should be greater than Valid From";
                            }
                        }
                       
                        if(message!="")
                        {
                            //ClientScript.RegisterClientScriptBlock(typeof(string), "message", "alert('" + message + "')", true);
                            lblVUESIC.Visible = true;
                            return;
                        }
                        else
                        {
                            lblVUESIC.Visible = false;
                        }
                        string GSTIN = "";
                        GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();

                        //...end....
                        //string InternalID = ORootCompaniesGeneralBL.Insert_Company(txtCompname.Text.Trim(), txtNameofBusiness.Text.Trim(), txtDirectors.Text.Trim()
                        //                                                           , txtAuthorised.Text.Trim(), txtRegnNo.Text.Trim(), txtPanNo.Text.Trim(),
                        //                                                           txtservicetaxNo.Text.Trim(), txtlocalSalesTax.Text.Trim(), Session["userid"].ToString(),
                        //                                                           txtCIN.Text.Trim(), dtInc, dtCin, dtVreg, dtVPan,
                        //                                                           drpParentComp.SelectedValue, ddlcurrency.SelectedItem.Value,
                        //                                                           txtOffRole.Text.Trim(), txtOnRole.Text.Trim(), txtKraPrefix.Text.Trim(), txtKraintermideatoryid.Text.Trim(),
                        //                                                            txtVatRegNo.Text.Trim() 
                        //                                                            );
                        string InternalID = ORootCompaniesGeneralBL.Insert_Company(txtCompname.Text.Trim(), txtNameofBusiness.Text.Trim(), txtDirectors.Text.Trim()
                                                                                  , txtAuthorised.Text.Trim(), txtRegnNo.Text.Trim(), txtPanNo.Text.Trim(),
                                                                                  txtservicetaxNo.Text.Trim(), txtlocalSalesTax.Text.Trim(), Convert.ToString(Session["userid"]),
                                                                                  txtCIN.Text.Trim(), dtInc, dtCin, dtVreg, dtVPan,
                                                                                  drpParentComp.SelectedValue, ddlcurrency.SelectedItem.Value,
                                                                                  txtOffRole.Text.Trim(), txtOnRole.Text.Trim(), txtKraPrefix.Text.Trim(), txtKraintermideatoryid.Text.Trim(),
                                                                                   txtVatRegNo.Text.Trim(), txtErpRegistration.Text.Trim(), dtErpValidFrom, dtErpValidUpto,
                                                                                   txtESIC.Text.Trim(), dtESICValidFrom, dtESICValidUpto, GSTIN,txtMSMEUdyamRCNo.Text.Trim());
                        //............End................
                      //Udf on Add mode
                        DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                        if (udfTable != null)
                            Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("Cmp",Convert.ToString(InternalID), udfTable, Convert.ToString(Session["userid"]));


                        string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_company", " cmp_id", " cmp_internalId='" + InternalID + "'", 1);
                        
                        if (Convert.ToString(cnt_id[0, 0]) != "n")
                        {
                            // update company schema id
                            oDBEngine.SetFieldValue("tbl_master_company", "onrole_schema_id=" + Convert.ToInt32(drpdwn_schema_on.SelectedValue) + ",offrole_schema_id=" + Convert.ToInt32(drpdwn_schema_off.SelectedValue), "cmp_id=" + cnt_id[0, 0]);


                            // Code  Added and Commented By Priti to use Convert.ToString instead ToString()
                            //Response.Redirect("rootcompany_general.aspx?id=" + cnt_id[0, 0].ToString(), false);
                            Response.Redirect("rootcompany_general.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
                        }
                    }
                    //    }

                    //}
                    catch
                    {

                    }
                }
            }
        }

        //Debjyoti 23-12-2016
        //Reason: For Udf Count
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='Cmp' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
    }
}