using System;
using System.Data;
using System.Web;
using System.Web.UI;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_FundManager_general : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        //Converter objConverter = new Converter();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                btnSave.Attributes.Add("onclick", "javascript:return Validate();");
                BindDropDown();
                DOBDate.EditFormatString = objConverter.GetDateFormat("Date");
                DOBDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ShowData();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["KeyVal_InternalID"].ToString() == "0")
            {
                string NameFLetter = txtFirstname.Text.Substring(0, 1).ToUpper();
                string Prefix = "FM" + NameFLetter;
                string InternaliD = oDBEngine.GetInternalId(Prefix, "Master_FundManagers", "FundManager_InternalID", " FundManager_InternalID");
                string FieldName = "FundManager_InternalID,FundManager_Salutation,FundManager_FirstName,FundManager_MiddleName,FundManager_LastName,FundManager_Gender,FundManager_NationalityID,FundManager_LegalStatus,FundManager_DOB,FundManager_CreateUser,FundManager_CreateDateTime";
                string FieldValue = "'" + InternaliD + "','" + drpSalutation.SelectedItem.Value + "','" + txtFirstname.Text + "','" + txtMiddleName.Text + "','" + txtLastName.Text + "','" + drpGender.SelectedItem.Value + "','" + drpNationality.SelectedItem.Value + "','" + drpLegalStatus.SelectedItem.Value + "','" + DOBDate.Value + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'";
                Int32 NoofEffect = oDBEngine.InsurtFieldValue("Master_FundManagers", FieldName, FieldValue);
                if (NoofEffect > 0)
                    Response.Redirect("FundManager_general.aspx?id=" + InternaliD, false);
            }
            else
            {
                string FieldName = "FundManager_Salutation='" + drpSalutation.SelectedItem.Value + "',FundManager_FirstName='" + txtFirstname.Text + "',FundManager_MiddleName='" + txtMiddleName.Text + "',FundManager_LastName='" + txtLastName.Text + "',FundManager_Gender='" + drpGender.SelectedItem.Value + "',FundManager_NationalityID='" + drpNationality.SelectedItem.Value + "',FundManager_LegalStatus='" + drpLegalStatus.SelectedItem.Value + "',FundManager_DOB='" + DOBDate.Value + "',FundManager_ModifyUser='" + Session["userid"].ToString() + "',FundManager_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'";
                Int32 NoofRowsAffect = oDBEngine.SetFieldValue("Master_FundManagers", FieldName, " FundManager_InternalID='" + Request.QueryString["id"].ToString() + "'");
            }
        }
        public void BindDropDown()
        {
            string[,] Salutation = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id,sal_name", null, 2);
            if (Salutation[0, 0] != "n")
                //oDBEngine.AddDataToDropDownList(Salutation, drpSalutation);
                oclsDropDownList.AddDataToDropDownList(Salutation, drpSalutation);
            string[,] nationality = oDBEngine.GetFieldValue("tbl_master_country", " cou_id,cou_country", null, 2);
            if (nationality[0, 0] != "n")
                //oDBEngine.AddDataToDropDownList(nationality, drpNationality);
                oclsDropDownList.AddDataToDropDownList(nationality, drpNationality);
            string[,] legalstatus = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id,lgl_legalstatus", null, 2);
            if (legalstatus[0, 0] != "n")
                //oDBEngine.AddDataToDropDownList(legalstatus, drpLegalStatus);
                oclsDropDownList.AddDataToDropDownList(legalstatus, drpLegalStatus);
        }
        public void ShowData()
        {
            if (Request.QueryString["id"] != "ADD")
            {
                if (Request.QueryString["id"] != null)
                {
                    Session["KeyVal_InternalID"] = Request.QueryString["id"].ToString();
                }
                DataTable dtFundmanager = oDBEngine.GetDataTable("Master_FundManagers", "FundManager_Salutation,FundManager_FirstName,FundManager_MiddleName,FundManager_LastName,FundManager_Gender,FundManager_NationalityID,FundManager_LegalStatus,FundManager_DOB", " FundManager_InternalID='" + Session["KeyVal_InternalID"].ToString() + "'");
                if (dtFundmanager.Rows.Count != 0)
                {
                    drpSalutation.SelectedValue = dtFundmanager.Rows[0][0].ToString();
                    txtFirstname.Text = dtFundmanager.Rows[0][1].ToString();
                    txtMiddleName.Text = dtFundmanager.Rows[0][2].ToString();
                    txtLastName.Text = dtFundmanager.Rows[0][3].ToString();
                    drpGender.SelectedValue = dtFundmanager.Rows[0][4].ToString();
                    drpNationality.SelectedValue = dtFundmanager.Rows[0][5].ToString();
                    drpLegalStatus.SelectedValue = dtFundmanager.Rows[0][6].ToString();
                    DOBDate.Value = Convert.ToDateTime(dtFundmanager.Rows[0][7].ToString());
                    txtFirstname.Enabled = false;
                }

                DisabledTabPages(true);
            }
            else
            {
                Session["KeyVal_InternalID"] = "0";
                DisabledTabPages(false);
            }
        }
        public void DisabledTabPages(bool onOff)
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Visible = onOff;
        }

    }
}