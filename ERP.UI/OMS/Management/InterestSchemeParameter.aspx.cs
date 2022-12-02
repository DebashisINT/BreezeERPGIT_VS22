using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class Management_InterestSchemeParameter : System.Web.UI.Page
    {
        BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure;
        Management_BL oManagement_BL = new Management_BL();
        public DataTable dtInterestSchemeConfig;
        DataTable dtDetails = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            /* if (!IsPostBack)
             {
                 pnlEmpty.Visible = true;
             }*/
            pnlEmpty.Visible = false;
            // txtDateFrom.Value = System.DateTime.Now.ToLocalTime().ToLongDateString();
            string ss = txtInterestScheme_hidden.Value;
            ScriptManager.RegisterStartupScript(this, GetType(), "Height", "height();", true);
            GetCompany();
            if (IsPostBack)
            {
                BindGrid();
            }
            lblInterestSchemeCode.Text = txtInterestScheme_hidden.Value;
            //  ClearAllFields();
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
        #region Methods
        private void ClearAllFields()
        {
            txtDateFrom.Value = System.DateTime.Now.ToLocalTime().ToString();
            txtExAmount.Text = string.Empty;
            txtTDSRate.Text = string.Empty;
            txtNarration.Text = string.Empty;
            txtMainAc.Text = string.Empty;
            txtMainAc_hidden.Value = string.Empty;
            txtSubAc.Text = string.Empty;
            txtSubAc_hidden.Value = string.Empty;
            txtTDSMainAC.Text = string.Empty;
            txtTDSMainAC_hidden.Value = string.Empty;
            txtTDSSubAC.Text = string.Empty;
            txtTDSSubAC_hidden.Value = string.Empty;
            txtSTMainAc.Text = string.Empty;
            txtSTMainAc_hidden.Value = string.Empty;
            txtSTSubAc.Text = string.Empty;
            txtSTSubAc_hidden.Value = string.Empty;
            rlstBalanceType.SelectedValue = "D";
            rlstCycle.SelectedValue = "M";
            rlstServTax.SelectedValue = "N";
            rlstDays.SelectedValue = "365";
            rbtnCalculateOn.SelectedValue = "C";

        }
        private void GetCompany()
        {
            DataTable dt = new DataTable();
            BusinessLogicLayer.GenericMethod obj = new BusinessLogicLayer.GenericMethod();
            dt = obj.GetCompanyDetail(Convert.ToString(Session["LastCompany"]));
            if (dt.Rows.Count > 0)
            {
                lblCompany.Text = Convert.ToString(dt.Rows[0][1]);
                lblCompanyIn.Text = lblCompany.Text;
            }
        }
        private void Save()
        {
            try
            {
                objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
                new DataTable();
                string[] strSpParam = new string[20];
                strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + hdnMode.Value + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                if (hdnMode.Value == "3")
                {
                    strSpParam[19] = "IntRates_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + Convert.ToString(ViewState["id"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[19] = "IntRates_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                strSpParam[1] = "Company|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + Convert.ToString(Session["LastCompany"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                if (txtMainAc_hidden.Value != string.Empty && txtMainAc_hidden.Value.Contains("*") == true && txtMainAc_hidden.Value.Split('*').Length > 0)
                {
                    strSpParam[2] = "MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtMainAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[2] = "MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }

                if (txtSubAc_hidden.Value != string.Empty && txtSubAc_hidden.Value.Contains("*") == true && txtSubAc_hidden.Value.Split('*').Length > 0)
                {
                    strSpParam[3] = "SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtSubAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[3] = "SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                strSpParam[4] = "SchemeCode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtInterestScheme_hidden.Value + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "CalculateOn|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + rbtnCalculateOn.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                if (txtSTMainAc_hidden.Value != string.Empty && txtSTMainAc_hidden.Value.Contains("*") == true && txtSTMainAc_hidden.Value.Split('*').Length > 0)
                {
                    strSpParam[6] = "STMainAccountID|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtSTMainAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[6] = "STMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                if (txtSTSubAc_hidden.Value != string.Empty && txtSTSubAc_hidden.Value.Contains("*") == true && txtSTSubAc_hidden.Value.Split('*').Length > 0)
                {
                    strSpParam[7] = "STSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtSTSubAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[7] = "STSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                if (txtTDSRate.Text != string.Empty && Convert.ToDecimal(txtTDSRate.Text.Replace(",", "")) > 0)
                {
                    if (txtTDSMainAC_hidden.Value != string.Empty && txtTDSMainAC_hidden.Value.Contains("*") == true && txtTDSMainAC_hidden.Value.Split('*').Length > 0)
                    {
                        strSpParam[8] = "TDSMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtTDSMainAC_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                    else
                    {
                        strSpParam[8] = "TDSMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                    if (txtTDSSubAC_hidden.Value != string.Empty && txtTDSSubAC_hidden.Value.Contains("*") == true && txtTDSSubAC_hidden.Value.Split('*').Length > 0)
                    {
                        strSpParam[9] = "TDSSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtTDSSubAC_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                    else
                    {
                        strSpParam[9] = "TDSSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                }
                else
                {
                    strSpParam[8] = "TDSMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    strSpParam[9] = "TDSSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                strSpParam[10] = "BalanceType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + rlstBalanceType.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[11] = "ServiceTax|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + rlstServTax.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[12] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + HttpContext.Current.Session["userid"] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[13] = "ExemptAmount|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtExAmount.Text.Replace(",", "") + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[14] = "DaysInYear|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + rlstDays.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[15] = "Narration|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + txtNarration.Text + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                if (txtTDSRate.Text != string.Empty)
                {
                    strSpParam[16] = "TDSRate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtTDSRate.Text.Replace(",", "") + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[16] = "TDSRate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                if (txtDateFrom.Value != string.Empty)
                {
                    strSpParam[17] = "DateFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|20|" + txtDateFrom.Value + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[17] = "DateFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|20|" + System.DateTime.Now.ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                }
                strSpParam[18] = "CalculationCycle|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + rlstCycle.SelectedValue + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                dtInterestSchemeConfig = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteUpdateFetch_InterestSchemeParameter");
                if (dtInterestSchemeConfig != null && dtInterestSchemeConfig.Rows.Count > 0 && Convert.ToString(dtInterestSchemeConfig.Rows[0][0]) == "Saved")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "psm0", "PostSaveMessage('0');", true);
                    ClearAllFields();
                    BindGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "psm1", "PostSaveMessage('1');", true);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {

            }
        }
        public string GetDateFormat(object dates)
        {
            if (dates.ToString().Contains("1900") == false && Convert.ToString(dates) != string.Empty)
            {
                //string format = "M d h:mm yy";
                string format = "dd-MMM-yyyy";
                DateTime now = DateTime.Now;
                DateTime dt = Convert.ToDateTime(dates);
                string s4 = dt.ToString(format);
                return s4;
            }
            else
            {
                return "";
            }
        }
        private void DeleteData(string id)
        {
            if (id.ToString().Split('*')[1] == string.Empty)
            {
                objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
                new DataTable();
                string[] strSpParam = new string[20];
                strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 4 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[1] = "IntRates_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + id.ToString().Split('*')[0] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                // code for managing default parameters - Start

                strSpParam[2] = "MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[3] = "SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[4] = "Company|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "CalculateOn|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[6] = "STMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[7] = "STSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[8] = "TDSMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[9] = "TDSSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[10] = "BalanceType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[11] = "ServiceTax|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[12] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[13] = "ExemptAmount|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[14] = "DaysInYear|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[15] = "Narration|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[16] = "TDSRate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[17] = "DateFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|20|" + System.DateTime.Now.ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[18] = "CalculationCycle|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[19] = "SchemeCode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

                // code for managing default parameters - End

                dtInterestSchemeConfig = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteUpdateFetch_InterestSchemeParameter");
                if (dtInterestSchemeConfig == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertdelete", "alert('Data Deleted Successfully');", true);
                    BindGrid();
                }
                dtInterestSchemeConfig = null;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertdelete", "alert('This record you cannot delete');", true);
            }
        }
        public void LoadEditableData(DataView dr)
        {
            txtDateFrom.Value = Convert.ToDateTime(dr[0]["IntRates_DateFrom"]);
            rbtnCalculateOn.SelectedValue = Convert.ToString(dr[0]["IntRates_CalculateOn"]);
            rlstBalanceType.SelectedValue = Convert.ToString(dr[0]["IntRates_BalanceType"]);
            rlstCycle.SelectedValue = Convert.ToString(dr[0]["IntRates_CalculationCycle"]);
            rlstDays.SelectedValue = Convert.ToString(dr[0]["IntRates_DaysInYear"]);
            rlstServTax.SelectedValue = Convert.ToString(dr[0]["IntRates_ServiceTax"]);
            txtExAmount.Text = Convert.ToString(dr[0]["IntRates_ExemptAmount"]);
            txtNarration.Text = Convert.ToString(dr[0]["IntRates_Narration"]);
            if (Convert.ToInt64((dr[0]["IntRates_TDSRate"])) > 0)
            {
                txtTDSRate.Text = Convert.ToString(dr[0]["IntRates_TDSRate"]);
            }
            else
            {
                txtTDSRate.Text = string.Empty;
            }
            txtSubAc.Text = Convert.ToString(dr[0]["SubAcName"]);
            txtTDSSubAC.Text = Convert.ToString(dr[0]["TDSSubAcName"]);
            txtSTSubAc.Text = Convert.ToString(dr[0]["STSubAcName"]);

            txtMainAc.Text = Convert.ToString(dr[0]["MainAcName"]);
            txtSTMainAc.Text = Convert.ToString(dr[0]["StxMainAcName"]);
            txtTDSMainAC.Text = Convert.ToString(dr[0]["TDSMainAcName"]);

            txtTDSMainAC_hidden.Value = "a*" + Convert.ToString(dr[0]["TDSMainAcId"]);
            txtSTMainAc_hidden.Value = "a*" + Convert.ToString(dr[0]["STMainAcId"]);
            txtMainAc_hidden.Value = "a*" + Convert.ToString(dr[0]["MainAcId"]);
            txtSubAc_hidden.Value = "a*" + Convert.ToString(dr[0]["SubAcId"]);
            txtSTSubAc_hidden.Value = "a*" + Convert.ToString(dr[0]["STSubAcId"]);
            txtTDSSubAC_hidden.Value = "a*" + Convert.ToString(dr[0]["TDSSubAcId"]);
            ViewState["CalculationCycle"] = Convert.ToString(dr[0]["IntRates_CalculationCycle"]);

            if (Convert.ToString(dr[0]["MainAcType"]).ToLower().Trim() == string.Empty || Convert.ToString(dr[0]["MainAcType"]).ToLower().Trim() == "none")
            {
                hdnMainAcType.Value = "N";
            }
            else
            {
                hdnMainAcType.Value = "Y";
            }
            if (Convert.ToString(dr[0]["STMainAcType"]).ToLower().Trim() == string.Empty || Convert.ToString(dr[0]["STMainAcType"]).ToLower().Trim() == "none")
            {
                hdnSTMainAcType.Value = "N";
            }
            else
            {
                hdnSTMainAcType.Value = "Y";
            }
            if (Convert.ToString(dr[0]["TDSMainAcType"]).ToLower().Trim() == string.Empty || Convert.ToString(dr[0]["TDSMainAcType"]).ToLower().Trim() == "none")
            {
                hdnTDSMainAcType.Value = "N";
            }
            else
            {
                hdnTDSMainAcType.Value = "Y";
            }
            int id = Convert.ToInt32(dr[0]["Id"]);

            LoadDetailsEditableData(id);
            if (Convert.ToString(dr[0]["DateToMain"]) != "")
            {
                EnableDisableControls(false);
                btnDeleteDetails.Visible = false;
                btnSave.Visible = false;
            }
            else
            {
                EnableDisableControls(true);
                btnDeleteDetails.Visible = true;
                btnSave.Visible = true;
            }
            lblInterestSchemeCode.Text = txtInterestScheme_hidden.Value;
            ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp('3','M');", true);
        }
        private void EnableDisableControls(bool b)
        {
            txtDateFrom.Enabled = b;
            txtExAmount.Enabled = b;
            txtMainAc.Enabled = b;
            txtSubAc.Enabled = b;
            txtTDSMainAC.Enabled = b;
            txtTDSSubAC.Enabled = b;
            txtSTMainAc.Enabled = b;
            txtSTSubAc.Enabled = b;
            rbtnCalculateOn.Enabled = b;
            rlstBalanceType.Enabled = b;
            rlstCycle.Enabled = b;
            rlstServTax.Enabled = b;
            txtTDSRate.Enabled = b;
            txtNarration.Enabled = b;
            pnlSlab.Visible = b;
            btnRemove.Visible = b;
        }
        private void LoadDetailsEditableData(int i)
        {
            DataTable dt = new DataTable();
            objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            string[] strSpParam = new string[4];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 2 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "MainId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + i + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for managing default parameters
            strSpParam[2] = "XmlDetails|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Text + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "User|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            dt = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertUpdateDeleteFetch_InterestParameterDetails");
            if (dt != null && dt.Rows.Count > 0)
            {
                // lblBlankDetailMessage.Text = "";
                btnDeleteDetails.Visible = true;
                // rptDetails.DataSource = dt;
                //  rptDetails.DataBind();
                rptDetails1.DataSource = dt;
                rptDetails1.DataBind();

                // btnRemove.Visible = true;
                int totalRowCount = dt.Rows.Count - 1;
                if (Convert.ToString(dt.Rows[totalRowCount]["DateToMain"]).Trim() != string.Empty)
                {
                    EnableDisableControls(false);
                    btnSaveDetails.Visible = false;
                    txtDateToDetails.Enabled = false;
                    txtAmountTo.Enabled = false;
                }
                else
                {
                    EnableDisableControls(true);
                    btnSaveDetails.Visible = true;
                    txtDateToDetails.Enabled = true;
                    txtAmountTo.Enabled = true;

                }
                pnlSlab.Visible = false;
                // lblBlankDetailMessage.Text = "";
                lblBlank.Visible = false;
            }
            else
            {
                lblBlank.Visible = true;
                //  lblFooter.Visible = true;
                btnDeleteDetails.Visible = false;
                pnlSlab.Visible = true;
                btnRemove.Visible = false;
                btnSaveDetails.Visible = false;
                // EnableDisableControls(false);
            }

        }
        private void BindGrid()
        {

            objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            // DataTable dtInterestSchemeConfig = new DataTable();
            string[] strSpParam = new string[20];

            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 2 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "SchemeCode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtInterestScheme_hidden.Value.Trim() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for managing default parameters - Start
            strSpParam[2] = "MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "Company|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "CalculateOn|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[6] = "STMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[7] = "STSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[8] = "TDSMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[9] = "TDSSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[10] = "BalanceType|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[11] = "ServiceTax|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[12] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[13] = "ExemptAmount|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[14] = "DaysInYear|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[15] = "Narration|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[16] = "TDSRate|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[17] = "DateFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|20|" + System.DateTime.Now.ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[18] = "CalculationCycle|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|1|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[19] = "IntRates_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for managing default parameters - End
            dtInterestSchemeConfig = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteUpdateFetch_InterestSchemeParameter");
            if (dtInterestSchemeConfig != null && dtInterestSchemeConfig.Rows.Count > 0)
            {
                gvInterestSchemeParameter.DataSource = dtInterestSchemeConfig;
                pnlEmpty.Visible = false;
            }
            else
            {
                pnlEmpty.Visible = true;
                gvInterestSchemeParameter.DataSource = null;
            }
            gvInterestSchemeParameter.ShowHeader = true;
            gvInterestSchemeParameter.DataBind();
        }
        private void SaveDetails()
        {
            DataSet ds = new DataSet();
            if (ViewState["Details"] != null)
            {
                dtDetails = (ViewState["Details"] as DataTable);
            }
            else
            {
                dtDetails.Columns.Add("AmountFrom", typeof(decimal));
                dtDetails.Columns.Add("AmountTo", typeof(decimal));
                dtDetails.Columns.Add("DayFrom", typeof(int));
                dtDetails.Columns.Add("DayTo", typeof(int));
                dtDetails.Columns.Add("Rate", typeof(decimal));
                foreach (RepeaterItem ri in rptDetails1.Items)
                {
                    if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                    {
                        int dayFrom = Convert.ToInt32((ri.FindControl("lblDayFrom") as Label).Text);
                        int dayTo = Convert.ToInt32((ri.FindControl("lblDayTo") as Label).Text);
                        decimal amtFrom = Convert.ToDecimal((ri.FindControl("lblAmountFrom") as Label).Text.Replace(",", ""));
                        decimal amtTo = Convert.ToDecimal((ri.FindControl("lblAmountTo") as Label).Text.Replace(",", ""));
                        decimal rate = Convert.ToDecimal((ri.FindControl("lblRate") as Label).Text);
                        dtDetails.Rows.Add(amtFrom, amtTo, dayFrom, dayTo, rate);
                    }

                }
            }
            ds.Tables.Add(dtDetails);
            MemoryStream ms = new MemoryStream();
            dtDetails.WriteXml(ms, true);

            string s = ds.GetXml();
            objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            new DataTable();
            string[] strSpParam = new string[3];
            strSpParam[0] = "XmlDetails|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + s + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "User|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + Convert.ToString(HttpContext.Current.Session["userid"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "MainId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 10 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;


            //SqlConnection con=new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"].ToString());
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection=con;
            //cmd.CommandText = "InsertUpdateDeleteFetch_InterestParameterDetails";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Mode", 1);
            //cmd.Parameters.AddWithValue("@XmlDetails",s);
            //cmd.Parameters.AddWithValue("@User", Convert.ToString(HttpContext.Current.Session["userid"]));
            //cmd.Parameters.AddWithValue("@MainId",Convert.ToString(ViewState["MainId"]));
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.Open();
            //}
            //cmd.ExecuteNonQuery();
            //if (con.State != ConnectionState.Closed)
            //{
            //    con.Close();
            //}

            oManagement_BL.InsertUpdateDeleteFetch_InterestParameterDetails(1, s, Convert.ToString(HttpContext.Current.Session["userid"]), Convert.ToInt32(ViewState["MainId"]));

        }
        private void ClearDetailsField(char exhausted)
        {
            if (exhausted == 'D')
            {
                txtAmountFrom.Text = "";
                txtAmountTo.Text = "";
                //  txtDateFromDetails.Enabled = false;
                txtDateToDetails.Enabled = false;
            }
            else if (exhausted == 'A')
            {
                txtDateFromDetails.Text = "";
                txtDateToDetails.Text = "";
                // txtAmountFrom.Enabled = false;
                txtAmountTo.Enabled = false;
            }
            else if (exhausted == 'N')
            {
                txtAmountFrom.Text = "";
                txtAmountTo.Text = "";
                txtDateFromDetails.Text = "";
                txtDateToDetails.Text = "";
            }
            txtRate.Text = "";
        }
        private void AddDetails()
        {
            btnRemove.Visible = true;
            if (ViewState["Details"] == null)
            {
                //  dtDetails.Columns.Add("MainId", typeof(int));
                dtDetails.Columns.Add("AmountFrom", typeof(decimal));
                dtDetails.Columns.Add("AmountTo", typeof(decimal));
                dtDetails.Columns.Add("DayFrom", typeof(int));
                dtDetails.Columns.Add("DayTo", typeof(int));
                dtDetails.Columns.Add("Rate", typeof(decimal));
                //  dsDetails.Rows.Add(10, Convert.ToInt32(txtAmountFrom.Text), Convert.ToInt32(txtAmountTo.Text), Convert.ToDateTime(txtDateFromDetails.Value), Convert.ToDateTime(txtDateToDetails.Value), Convert.ToDecimal(txtRate.Text));
                //  ViewState["Details"] = dsDetails;

            }
            else
            {
                dtDetails = (ViewState["Details"] as DataTable);

            }
            lblBlank.Visible = false;

            decimal maxAmount = Convert.ToDecimal(txtAmountTo.Text.Replace(",", "")) > 999999999999 ? 999999999999 : Convert.ToDecimal(txtAmountTo.Text.Replace(",", ""));
            dtDetails.Rows.Add(Convert.ToDecimal(txtAmountFrom.Text.Replace(",", "")), maxAmount, Convert.ToInt32(txtDateFromDetails.Text), Convert.ToInt32(txtDateToDetails.Text.Replace(",", "").Replace(".", "")), Convert.ToDecimal(txtRate.Text.Replace(",", "")));

            ViewState["Details"] = dtDetails;
            rptDetails1.DataSource = dtDetails;
            rptDetails1.DataBind();
            if (Convert.ToDecimal(txtAmountTo.Text.Replace(",", "")) >= 999999999999 && ((Convert.ToInt32(txtDateToDetails.Text) < 31 && ViewState["CalculationCycle"].ToString() == "M") || (Convert.ToInt32(txtDateToDetails.Text) < 7 && ViewState["CalculationCycle"].ToString() == "W")))
            {
                int i = (Convert.ToInt32(txtDateToDetails.Text) + 1);
                //  ClearDetailsField('A');
                if ((i - 1) < 7 || (((i - 1) < 31) && ViewState["CalculationCycle"].ToString() == "M"))
                {
                    txtDateFromDetails.Text = i.ToString();
                }
                // txtAmountFrom.Enabled = false;
                txtAmountTo.Enabled = false;
                if (ViewState["CalculationCycle"].ToString() == "M")
                {
                    txtDateToDetails.Text = "31";
                }
                else
                {
                    txtDateToDetails.Text = "7";
                }
                btnSaveDetails.Visible = false;
            }
            else if (Convert.ToDecimal(txtAmountTo.Text.Replace(",", "")) < 999999999999 && ((Convert.ToInt32(txtDateToDetails.Text.Replace(",", "")) >= 31 && ViewState["CalculationCycle"].ToString() == "M") || (Convert.ToInt32(txtDateToDetails.Text.Replace(",", "")) >= 7 && ViewState["CalculationCycle"].ToString() == "W")))
            {
                decimal j = (Convert.ToDecimal(txtAmountTo.Text.Replace(",", "")) + 1);
                // ClearDetailsField('D');
                txtAmountFrom.Text = j.ToString();
                //  txtDateFromDetails.Enabled = false;
                txtDateToDetails.Enabled = false;
                txtAmountTo.Text = "999,999,999,999";
                btnSaveDetails.Visible = false;
                // txtAmountFrom.Text = "1";
            }
            else if (Convert.ToDecimal(txtAmountTo.Text.Replace(",", "")) >= 999999999999 && ((Convert.ToInt32(txtDateToDetails.Text) >= 31 && ViewState["CalculationCycle"].ToString() == "M") || (Convert.ToInt32(txtDateToDetails.Text) >= 7 && ViewState["CalculationCycle"].ToString() == "W")))
            {
                pnlSlab.Visible = false;
                btnSaveDetails.Visible = true;
                btnRemove.Visible = true;
                btnSaveDetails.Visible = true;

            }
            else
            {
                int i = (Convert.ToInt32(txtDateToDetails.Text) + 1);
                txtDateFromDetails.Text = i.ToString();
                decimal j = (Convert.ToDecimal(txtAmountTo.Text.Replace(",", "")) + 1);
                txtAmountFrom.Text = j.ToString();
                txtAmountTo.Text = "999,999,999,999";
                txtDateToDetails.Text = "";
                if (ViewState["CalculationCycle"].ToString() == "M")
                {
                    txtDateToDetails.Text = "31";
                }
                else
                {
                    txtDateToDetails.Text = "7";
                }
                btnSaveDetails.Visible = false;
            }
            txtRate.Text = "";

            //  btnSaveDetails.Visible = true;
            btnRemove.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp('3','D');", true);

        }
        private void ClearDetailsFields()
        {
            txtAmountFrom.Text = string.Empty;
            txtAmountTo.Text = string.Empty;
            txtDateFromDetails.Text = string.Empty;
            txtDateToDetails.Text = string.Empty;
            txtRate.Text = string.Empty;
            //  txtAmountFrom.Enabled = true;
            //  txtDateFromDetails.Enabled = true;
        }
        private void DeleteDetails(int id)
        {

            DataTable dt = new DataTable();
            objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            string[] strSpParam = new string[4];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 3 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "MainId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + id + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for managing default parametrs
            strSpParam[2] = "XmlDetails|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Text + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "User|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + "" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            dt = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertUpdateDeleteFetch_InterestParameterDetails");
            if (dt != null && dt.Rows.Count > 0)
            {
                // ScriptManager.RegisterStartupScript(this, GetType(), "Heisdeehhhght", "alert('Data Deleted Successfully');", true);
                DetailsShowingProcess(id.ToString(), true);
                btnDeleteDetails.Visible = false;


            }
        }
        #endregion
        #region  Control Events
        protected void btnDeleteDetails_Click(object sender, EventArgs e)
        {
            int mainId = Convert.ToInt32(ViewState["MainId"]);
            DeleteDetails(mainId);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddDetails();
        }
        protected void PreventEnteringDetails(string s)
        {
            // lblBlankDetailMessage.Text = "";
            if (s == "0")
            {
                pnlSlab.Visible = false;
                //   lblBlankDetailMessage.Text = "No Record Found";
                btnDeleteDetails.Visible = false;
            }
            else
            {
                //  pnlSlab.Visible = true;
                //lblBlankDetailMessage.Text = "";
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (txtInterestScheme_hidden.Value.Trim() != string.Empty && txtInterestScheme_hidden.Value != null && txtInterestScheme_hidden.Value.Trim() != "No Record Found")
            {
                lblBlankMessage.Text = "";
                BindGrid();
            }
            else
            {
                lblBlankMessage.Text = "Enter a scheme code first";
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            ViewState["Details"] = null;
            rptDetails1.DataSource = null;
            rptDetails1.DataBind();
            pnlSlab.Visible = true;
            btnSaveDetails.Visible = false;
            btnRemove.Visible = false;
            txtDateFromDetails.Text = "1";
            txtDateToDetails.Text = "";
            txtAmountFrom.Text = "1";
            txtAmountTo.Text = "999,999,999,999";
            //  txtDateFromDetails.Enabled = true;
            txtDateToDetails.Enabled = true;
            if (Convert.ToString(ViewState["CalculationCycle"]) == "M")
            {
                txtDateToDetails.Text = "31";
            }
            else if (Convert.ToString(ViewState["CalculationCycle"]) == "W")
            {
                txtDateToDetails.Text = "7";
            }
            //  txtAmountFrom.Enabled = true;
            txtAmountTo.Enabled = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp('3','D');", true);
        }
        protected void btnSaveDetails_Click(object sender, EventArgs e)
        {

            SaveDetails();
        }
        protected void DetailsShowingProcess(string details, bool IsAfterDetails)
        {
            lblBlankMessage.Text = "";
            ClearDetailsFields();
            ViewState["Details"] = null;
            txtAmountFrom.Text = "1";
            txtAmountTo.Text = "999,999,999,999";
            txtDateFromDetails.Text = "1";

            rptDetails1.DataSource = ViewState["Details"] as DataTable;
            rptDetails1.DataBind();
            if (IsAfterDetails == false)
            {
                ViewState["MainId"] = details.ToString().Split('*')[0];

                ViewState["CalculationCycle"] = details.ToString().Split('*')[1];
                if (Convert.ToString(ViewState["CalculationCycle"]) == "M")
                {
                    txtDateToDetails.Text = "31";
                }
                else if (Convert.ToString(ViewState["CalculationCycle"]) == "W")
                {
                    txtDateToDetails.Text = "7";
                }
                string s2 = details.ToString().Split('*')[2].ToString();
                LoadDetailsEditableData(Convert.ToInt32(details.ToString().Split('*')[0]));
                if (s2.Contains("1900"))
                {
                    PreventEnteringDetails("1");
                }
                else
                {
                    PreventEnteringDetails("0");
                }
            }
            else
            {
                ViewState["MainId"] = details;
                LoadDetailsEditableData(Convert.ToInt32(details));
                ScriptManager.RegisterStartupScript(this, GetType(), "showdsft67u8d", "alert('Details Prameters Deleted Successfully');", true);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "show3rd4d4", "ShowPopUp('3','D');", true);
        }
        protected void Add_empty(object sender, EventArgs e)
        {
            ClearAllFields();
            //  rptDetails.DataSource = null;
            //  rptDetails.DataBind();
            rptDetails1.DataSource = null;
            rptDetails1.DataBind();
            if (txtInterestScheme_hidden.Value.Trim() != string.Empty && txtInterestScheme_hidden.Value != null && txtInterestScheme_hidden.Value.Trim() != "No Record Found")
            {
                lblBlankMessage.Text = "";
                txtDateFrom.Value = System.DateTime.Now.ToString();
                EnableDisableControls(true);
                ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp('1','M');", true);
            }
            else
            {
                lblBlankMessage.Text = "Enter a scheme code first";
            }
        }
        #endregion
        #region Grid Events
        protected void gvInterestSchemeParameter_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvInterestSchemeParameter_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInterestSchemeParameter.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        public void gvInterestSchemeParameter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "info")
            {
                ViewState["id"] = Convert.ToString(e.CommandArgument);
                // DataRow[] dr = new DataRow();
                //  dr = dtInterestSchemeConfig.Select("IntRates_ID=" + id);
                DataView dv = new DataView(dtInterestSchemeConfig);
                dv.RowFilter = string.Format("IntRates_ID={0}", Convert.ToString(e.CommandArgument));

                LoadEditableData(dv);
            }
            else if (e.CommandName.ToLower() == "details")
            {
                DetailsShowingProcess(e.CommandArgument.ToString(), false);
                //  txtAmountFrom.Enabled = true;
                txtAmountTo.Enabled = true;
                txtDateToDetails.Enabled = true;
                //  txtDateFromDetails.Enabled = true;

            }
            else if (e.CommandName.ToLower() == "newadd")
            {

                ClearAllFields();
                //  rptDetails.DataSource = null;
                //  rptDetails.DataBind();
                rptDetails1.DataSource = null;
                rptDetails1.DataBind();
                if (txtInterestScheme_hidden.Value.Trim() != string.Empty && txtInterestScheme_hidden.Value != null && txtInterestScheme_hidden.Value.Trim() != "No Record Found")
                {
                    txtDateFrom.Value = System.DateTime.Now.ToLocalTime();
                    lblBlankMessage.Text = "";
                    btnSave.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp('1','M');", true);
                }
                else
                {
                    lblBlankMessage.Text = "Enter a scheme code first";
                }
                EnableDisableControls(true);
                txtDateFrom.Value = System.DateTime.Now.ToString();
            }
            else if (e.CommandName.ToLower() == "del")
            {
                DeleteData(Convert.ToString(e.CommandArgument));
            }
        }
        #endregion
        protected void rptDetails1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int j = 1;
            if (e.CommandName == "rem")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp('3','D');", true);
                DataTable dt = new DataTable();
                dt.Columns.Add("AmountFrom", typeof(decimal));
                dt.Columns.Add("AmountTo", typeof(decimal));
                dt.Columns.Add("DayFrom", typeof(int));
                dt.Columns.Add("DayTo", typeof(int));
                dt.Columns.Add("Rate", typeof(int));

                int i = Convert.ToInt32(e.CommandArgument.ToString());
                if (rptDetails1.Items.Count > 0)
                {
                    foreach (RepeaterItem ri in rptDetails1.Items)
                    {
                        if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                        {
                            int dayFrom = Convert.ToInt32((ri.FindControl("lblDayFrom") as Label).Text);
                            int dayTo = Convert.ToInt32((ri.FindControl("lblDayTo") as Label).Text);
                            decimal amtFrom = Convert.ToDecimal((ri.FindControl("lblAmountFrom") as Label).Text);
                            decimal amtTo = Convert.ToDecimal((ri.FindControl("lblAmountTo") as Label).Text);
                            decimal rate = Convert.ToDecimal((ri.FindControl("lblRate") as Label).Text);
                            if (i != j)
                            {
                                dt.Rows.Add(amtFrom, amtTo, dayFrom, dayTo, rate);
                            }
                            j++;
                        }

                    }
                    lblBlankMessage.Text = "";
                    rptDetails1.DataSource = dt;
                    rptDetails1.DataBind();
                    ViewState["Details"] = dt;
                }
            }
        }
    }
}