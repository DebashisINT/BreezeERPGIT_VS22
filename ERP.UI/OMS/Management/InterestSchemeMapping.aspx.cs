using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management
{
    public partial class Management_InterestSchemeMapping : System.Web.UI.Page
    {
        BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure;
        protected void Page_Load(object sender, EventArgs e)
        {
            //  
            ScriptManager.RegisterStartupScript(this, GetType(), "Height", "height();", true);
            if (!IsPostBack)
            {
                GetCompany();
            }
            /* ViewState["SchemeCodeName"] = txtIntSchemeCode.Text;
             if (ViewState["SchemeCodeName"] != null && ViewState["SchemeCodeName"].ToString() != string.Empty)
             {
                 txtIntSchemeCode.Text = ViewState["SchemeCodeName"].ToString();
             }
             if (txtIntSchemeCode_hidden.Value != string.Empty)
             {
                 ViewState["SchemeCode"] = txtIntSchemeCode_hidden.Value;
             }
             if (ViewState["SchemeCode"] != null && ViewState["SchemeCode"].ToString() != string.Empty)
             {
                 txtIntSchemeCode_hidden.Value = ViewState["SchemeCode"].ToString();
             }  */
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
        protected void btnAdds_Click(object sender, EventArgs e)
        {

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        #region Methods
        private void GetCompany()
        {
            DataTable dt = new DataTable();
            BusinessLogicLayer.GenericMethod obj = new BusinessLogicLayer.GenericMethod();
            dt = obj.GetCompanyDetail(Convert.ToString(Session["LastCompany"]));
            if (dt.Rows.Count > 0)
            {
                lblCompany.Text = Convert.ToString(dt.Rows[0][1]);
                // lblCompany.Text = lblCompany.Text;
            }
        }
        public string GetDateFormat(object dates)
        {
            //string format = "M d h:mm yy";
            string format = "dd-MMM-yyyy";
            DateTime now = DateTime.Now;
            DateTime dt = Convert.ToDateTime(dates);
            string s4 = dt.ToString(format);
            return s4;
        }
        private void ClearAllFields()
        {
            txtPostMainAc.Text = "";
            txtPostMainAc_hidden.Value = "";
            txtPostSubAc.Text = "";
            txtPostSubAc_hidden.Value = "";
            //   txtIntSchemeCode.Text = "";
            //  txtIntSchemeCode_hidden.Value = "";
            txtMainAc.Text = "";
            txtMainAc_hidden.Value = "";
            txtSubAc.Text = "";
            txtSubAc_hidden.Value = "";
            rbtnPostInAc.SelectedValue = "0";
            rbtnPostInSameHead.SelectedValue = "1";
            txtPostMainAc.Enabled = false;
            txtPostSubAc.Enabled = false;
        }
        public void Save()
        {
            objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            DataTable dtInterestSchemeMapping = new DataTable();
            string[] strSpParam = new string[11];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 1 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "IntMembers_Company|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + Session["LastCompany"] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            strSpParam[2] = "IntMembers_MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + txtMainAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            if (txtSubAc_hidden.Value != string.Empty && txtSubAc_hidden.Value.Contains("*") == true)
            {
                strSpParam[3] = "IntMembers_SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + txtSubAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[3] = "IntMembers_SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            strSpParam[4] = "IntMembers_IntSchemeCode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + txtIntSchemeCode_hidden.Value + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            if (rbtnPostInAc.SelectedValue == "1")
            {
                strSpParam[5] = "IntMembers_PostInAccounts|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|10|" + "Y" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                if (rbtnPostInSameHead.SelectedValue == "0")
                {
                    strSpParam[6] = "IntMembers_PostMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + txtPostMainAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    if (txtPostSubAc_hidden.Value != string.Empty && txtPostSubAc_hidden.Value.Contains("*") == true)
                    {
                        strSpParam[7] = "IntMembers_PostSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + txtPostSubAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                    else
                    {
                        strSpParam[7] = "IntMembers_PostSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                }
                else
                {
                    ////  code for post in ac and post in same head condition
                    strSpParam[6] = "IntMembers_PostMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + txtMainAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

                    if (txtSubAc_hidden.Value != string.Empty && txtSubAc_hidden.Value.Contains("*") == true)
                    {
                        strSpParam[7] = "IntMembers_PostSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + txtSubAc_hidden.Value.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                    else
                    {
                        strSpParam[7] = "IntMembers_PostSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                    }
                    ////
                }
            }
            else
            {
                strSpParam[5] = "IntMembers_PostInAccounts|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|10|" + "N" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[6] = "IntMembers_PostMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[7] = "IntMembers_PostSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }

            strSpParam[8] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + HttpContext.Current.Session["userid"] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[9] = "IntMembers_DateFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|30|" + Convert.ToDateTime(txtDateFrom.Value) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            //if (rbtnPostInAc.SelectedValue == "1" && rbtnPostInSameHead.SelectedValue == "1")
            //{
            //    strSpParam[10] = "IsPostInSameAc|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Bit + "|1|" + Convert.ToBoolean(false) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            //}
            if (ViewState["Id"] != null)
            {
                strSpParam[10] = "Id|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|30|" + Convert.ToString(ViewState["Id"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[10] = "Id|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|30|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            dtInterestSchemeMapping = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteUpdateFetch_InterestSchemeMapping");
            // alert messages
            if (dtInterestSchemeMapping != null && dtInterestSchemeMapping.Rows.Count > 0 && dtInterestSchemeMapping.Rows[0][0].ToString().ToLower() == "saved")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "psm", "PostSaveMessage('0')", true);
                BindGrid();
            }
            else if (dtInterestSchemeMapping != null && dtInterestSchemeMapping.Rows.Count > 0 && dtInterestSchemeMapping.Rows[0][0].ToString().ToLower() == "exists")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "psm", "PostSaveMessage('1')", true);

            }
            else if (dtInterestSchemeMapping != null && dtInterestSchemeMapping.Rows.Count > 0 && dtInterestSchemeMapping.Rows[0][0].ToString() == "Exists on the date")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "psm", "PostSaveMessage('2')", true);
            }
            else if (dtInterestSchemeMapping != null && dtInterestSchemeMapping.Rows.Count > 0 && dtInterestSchemeMapping.Rows[0][0].ToString() == "Already scheme mapping exists on the date")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "psm", "PostSaveMessage('3')", true);
            }
            // ClearAllFields();
        }
        private void BindGrid()
        {
            objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            DataTable dtInterestSchemeMapping = new DataTable();
            string strDT = System.DateTime.Now.ToString();
            string[] strSpParam = new string[11];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 2 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "IntMembers_IntSchemeCode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|20|" + txtIntSchemeCode_hidden.Value + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for alternative of default parameter- start
            strSpParam[2] = "IntMembers_Company|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "IntMembers_MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "IntMembers_SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "IntMembers_PostInAccounts|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[6] = "IntMembers_PostMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[7] = "IntMembers_PostSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[8] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[9] = "IntMembers_DateFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|10|" + strDT + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[10] = "Id|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for alternative of default parameter- End

            dtInterestSchemeMapping = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteUpdateFetch_InterestSchemeMapping");
            gvInterestSchemeMapping.DataSource = dtInterestSchemeMapping;
            gvInterestSchemeMapping.DataBind();
            ViewState["DtMapping"] = dtInterestSchemeMapping;
        }
        private void LoadEditableData(DataView dv)
        {
            txtMainAc.Text = Convert.ToString(dv[0]["MainAcName"]);
            txtMainAc_hidden.Value = "5~" + "" + Convert.ToString(dv[0]["MainAcType"]) + "~*" + Convert.ToString(dv[0]["IntMembers_MainAccountID"]);
            string[] arr = txtMainAc_hidden.Value.Split('*');
            txtSubAc.Text = Convert.ToString(dv[0]["SubAcName"]);
            txtSubAc_hidden.Value = "*" + Convert.ToString(dv[0]["IntMembers_SubAccountID"]);
            txtPostMainAc.Text = Convert.ToString(dv[0]["PostMainAcName"]);
            txtPostMainAc_hidden.Value = "aa~" + "" + Convert.ToString(dv[0]["PostMainAcType"]) + "~*" + Convert.ToString(dv[0]["IntMembers_PostMainAccountID"]);
            txtPostSubAc.Text = Convert.ToString(dv[0]["PostSubAcName"]);
            if (txtPostMainAc.Text != string.Empty)
            {
                txtPostMainAc.Enabled = true;
                txtPostSubAc.Enabled = true;
                // rbtnPostInSameHead.Enabled = true;
                rbtnPostInAc.SelectedValue = "1";
                rbtnPostInSameHead.SelectedValue = "0";
            }
            else
            {
                txtPostMainAc.Enabled = false;
                txtPostSubAc.Enabled = false;
                //rbtnPostInSameHead.Enabled = false;
                rbtnPostInAc.SelectedValue = "0";
                rbtnPostInSameHead.SelectedValue = "1";
            }

            txtPostSubAc_hidden.Value = "*" + Convert.ToString(dv[0]["IntMembers_PostSubAccountID"]);
            txtDateFrom.Value = Convert.ToDateTime(dv[0]["IntMembers_DateFrom"]);
            txtIntSchemeCode.Text = Convert.ToString(dv[0]["IntMembers_IntSchemeCode"]);
            txtIntSchemeCode_hidden.Value = Convert.ToString(dv[0]["IntMembers_IntSchemeCode"]);
            rbtnPostInAc.SelectedValue = Convert.ToString(dv[0]["IntMembers_PostInAccounts"]).ToUpper() == "Y" ? "1" : "0";
            if (Convert.ToString(dv[0]["MainAcType"]).Trim() == string.Empty || Convert.ToString(dv[0]["MainAcType"]).Trim().ToLower() == "none")
            {
                hdnMainAcType.Value = "N";
            }
            else
            {
                hdnMainAcType.Value = "Y";
            }
            if ((Convert.ToString(dv[0]["PostMainAcType"])).Trim() == string.Empty || (Convert.ToString(dv[0]["PostMainAcType"])).Trim().ToLower() == "none")
            {
                hdnPostMainAcType.Value = "N";
            }
            else
            {
                hdnPostMainAcType.Value = "Y";
            }
            if (Convert.ToString(dv[0]["DateTos"]) != string.Empty)
            {
                EnableDisable(false);
            }
            else
            {
                EnableDisable(true);
            }
            if (Convert.ToString(dv[0]["IntMembers_PostInAccounts"]) == "Y" && Convert.ToString(dv[0]["PostMainAcName"]) != string.Empty)
            {
                rbtnPostInAc.SelectedValue = "1";
                txtPostSubAc.Enabled = true;
                txtPostMainAc.Enabled = true;
                rbtnPostInSameHead.Enabled = true;
                rbtnPostInSameHead.SelectedValue = "0";
                ScriptManager.RegisterStartupScript(this, GetType(), "makeen", "MakeEnable();", true);

            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alerts122", "Check();", true);
        }
        private void EnableDisable(bool b)
        {
            txtDateFrom.Enabled = b;
            txtMainAc.Enabled = b;
            txtSubAc.Enabled = b;
            txtPostMainAc.Enabled = b;
            txtPostSubAc.Enabled = b;
            btnSave.Visible = b;
        }
        private void Delete(string id)
        {
            string strDT = System.DateTime.Now.ToString();
            if (!(id != string.Empty && id.Split('*')[1].Contains("1900") == false && id.Split('*')[1] != string.Empty))
            {
                DataTable dtInterestSchemeMapping = new DataTable();
                objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
                // DataTable dtInterestSchemeMapping = new DataTable();
                string[] strSpParam = new string[11];
                strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + 3 + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[1] = "Id|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + id.Split('*')[0] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                // code for managing default parameters - start
                strSpParam[2] = "IntMembers_Company|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[3] = "IntMembers_MainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[4] = "IntMembers_SubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "IntMembers_PostInAccounts|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[6] = "IntMembers_PostMainAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[7] = "IntMembers_PostSubAccountID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[8] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[9] = "IntMembers_DateFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.DateTime + "|10|" + strDT + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[10] = "IntMembers_IntSchemeCode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                // code for managing default parameters - End
                dtInterestSchemeMapping = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteUpdateFetch_InterestSchemeMapping");
                if (dtInterestSchemeMapping != null && dtInterestSchemeMapping.Rows.Count > 0 && dtInterestSchemeMapping.Rows[0][0].ToString() == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "pdm1", "PostDeleteMessage()", true);
                    BindGrid();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerts", "alert('This record cannot be deleted')", true);
            }
        }
        #endregion
        #region Grid Events
        protected void gvInterestSchemeMapping_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvInterestSchemeMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            int i = e.NewPageIndex;
            gvInterestSchemeMapping.PageIndex = i;
            BindGrid();


        }

        protected void gvInterestSchemeMapping_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "info")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp();", true);
                DataView dv = new DataView();
                dv.Table = (ViewState["DtMapping"] as DataTable);
                dv.RowFilter = string.Format("IntMembers_ID={0}", Convert.ToString(e.CommandArgument));
                LoadEditableData(dv);
                ViewState["Id"] = Convert.ToString(e.CommandArgument);
            }
            else if (e.CommandName.ToLower() == "addnew")
            {
                ViewState["Id"] = null;
                ClearAllFields();
                ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp();", true);
                txtDateFrom.Value = System.DateTime.Now.ToLocalTime();
                EnableDisable(true);
                txtPostMainAc.Enabled = false;
                txtPostSubAc.Enabled = false;
            }
            else if (e.CommandName.ToLower() == "dele")
            {
                Delete(e.CommandArgument.ToString());
            }
        }
        #endregion
    }
}