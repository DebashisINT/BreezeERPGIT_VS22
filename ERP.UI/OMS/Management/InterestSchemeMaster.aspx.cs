using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxClasses;
using DevExpress.Web;

namespace ERP.OMS.Management
{
    public partial class Management_InterestSchemeMaster : System.Web.UI.Page
    {
        BusinessLogicLayer.GenericMethod objGenericMethod;
        BusinessLogicLayer.GenericStoreProcedure objGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();

        protected void Page_Load(object sender, EventArgs e)
        {
            //   BindGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "Height", "height();", true);
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

            BindGrid();
        }
        #region Methods
        public void CallBack(object sender, CallbackEventArgsBase e)
        {
            int id = 0;
            if (e.Parameter.ToLower() == "save")
            {
                Save(1);
            }
            else if (e.Parameter.ToLower() == "update")
            {
                Save(4);
            }
            else if (int.TryParse(e.Parameter, out id))
            {
                LoadEditableData(e.Parameter);
            }
            else if (e.Parameter.Split('*')[1].ToLower() == "delete")
            {
                Delete(e.Parameter.Split('*')[0]);
            }

        }
        private void Delete(string id)
        {
            if (id.Split('*')[0] == "1")
            {
                objGenericMethod = new BusinessLogicLayer.GenericMethod();
                string[] strSpParam = new string[6];
                strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + Convert.ToInt32("3") + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[1] = "IntScheme_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + id.Split('*')[1] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

                // code for managing defailt parameter  START
                strSpParam[2] = "IntScheme_Code|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[3] = "IntScheme_Name|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|100|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[4] = "IsDefault|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

                // code for managing defailt parameter END

                DataTable dtInsertedInterestScheme = new DataTable();
                dtInsertedInterestScheme.Clear();
                dtInsertedInterestScheme = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteFetch_InterestScheme");
                if (dtInsertedInterestScheme != null && dtInsertedInterestScheme.Rows.Count > 0 && dtInsertedInterestScheme.Rows[0][0].ToString() == "0")
                {
                    cbpnlTest.JSProperties["cpStatus"] = "Deleted";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "postdelete", "PostDeleteMessage();", true);
                //  string url = Request.Url.ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "predelete", "alert('This record cannot be deleted as this scheme code exist in other tables');", true);
            }
        }
        public void Save(int mode)
        {
            // string s = e.Parameter;
            objGenericMethod = new BusinessLogicLayer.GenericMethod();
            string[] strSpParam = new string[6];
            if (ViewState["ID"] == null)
            {
                strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + "1" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "IntScheme_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + "4" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
                strSpParam[5] = "IntScheme_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|150|" + Convert.ToString(ViewState["ID"]) + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            strSpParam[1] = "IntScheme_Code|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + txtIntSchemeCode.Text.Trim() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "IntScheme_Name|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|100|" + txtIntSchemeName.Text.Trim() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            if (rbtnlstIsDefault.SelectedValue == "0")
            {
                strSpParam[3] = "IsDefault|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|150|" + "N" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[3] = "IsDefault|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|150|" + "Y" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            }
            strSpParam[4] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + HttpContext.Current.Session["userid"] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            DataTable dtInsertedInterestScheme = new DataTable();
            dtInsertedInterestScheme = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteFetch_InterestScheme");

            if (dtInsertedInterestScheme != null && dtInsertedInterestScheme.Rows.Count > 0 && dtInsertedInterestScheme.Rows[0]["messages"].ToString() == "Saved")
            {
                ViewState["ID"] = null;
                ClearAllFields();
                dtInsertedInterestScheme.Clear();
                cbpnlTest.JSProperties["cpStatus"] = "Saved";
                ScriptManager.RegisterStartupScript(this, GetType(), "savemessage", "alert('Data Saved Successfully');", true);
            }
            else
            {
                if (dtInsertedInterestScheme != null && dtInsertedInterestScheme.Rows.Count > 0 && dtInsertedInterestScheme.Rows[0]["messages"] != null && dtInsertedInterestScheme.Rows[0]["messages"].ToString() == "DuplicateCode")
                {
                    cbpnlTest.JSProperties["cpStatus"] = "DuplicateCode";
                    ScriptManager.RegisterStartupScript(this, GetType(), "savemessage", "alert('Scheme code already exists');", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "show1", "ShowPopUp();", true);
                }
                else if (dtInsertedInterestScheme != null && dtInsertedInterestScheme.Rows.Count > 0 && dtInsertedInterestScheme.Rows[0]["messages"] != null && dtInsertedInterestScheme.Rows[0]["messages"].ToString() == "DuplicateDefault")
                {
                    cbpnlTest.JSProperties["cpStatus"] = "DefaultAlreadyExists";
                    ScriptManager.RegisterStartupScript(this, GetType(), "savemessage", "alert('Default scheme already exists');", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "show2", "ShowPopUp();", true);
                }
            }

        }


        private void BindGrid()
        {
            objGenericMethod = new BusinessLogicLayer.GenericMethod();
            string[] strSpParam = new string[6];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + Convert.ToInt32("2") + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for managing defailt parameter  START
            strSpParam[1] = "IntScheme_Code|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "IntScheme_Name|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|100|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "IsDefault|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "IntScheme_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            // code for managing defailt parameter END
            DataTable dtInsertedInterestScheme = new DataTable();
            dtInsertedInterestScheme = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteFetch_InterestScheme");
            gvInresetMaster.DataSource = dtInsertedInterestScheme;
            gvInresetMaster.DataBind();
        }

        private void ClearAllFields()
        {
            txtIntSchemeCode.Text = string.Empty;
            txtIntSchemeName.Text = string.Empty;
        }
        private void LoadEditableData(string id)
        {
            ViewState["ID"] = id;
            objGenericMethod = new BusinessLogicLayer.GenericMethod();
            string[] strSpParam = new string[6];
            strSpParam[0] = "Mode|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + Convert.ToInt32("2") + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "IntScheme_ID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|20|" + id + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            // code for managing defailt parameter  START
            strSpParam[2] = "IntScheme_Code|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|50|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "IntScheme_Name|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|100|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "IsDefault|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Char + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "UserId|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|150|" + "0" + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

            // code for managing defailt parameter END

            DataTable dtInsertedInterestScheme = new DataTable();
            dtInsertedInterestScheme.Clear();
            dtInsertedInterestScheme = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "InsertDeleteFetch_InterestScheme");
            if (dtInsertedInterestScheme != null && dtInsertedInterestScheme.Rows.Count > 0)
            {
                txtIntSchemeCode.Enabled = false;
                txtIntSchemeCode.Text = Convert.ToString(dtInsertedInterestScheme.Rows[0]["IntScheme_Code"]);
                txtIntSchemeName.Text = Convert.ToString(dtInsertedInterestScheme.Rows[0]["IntScheme_Name"]);
                rbtnlstIsDefault.SelectedValue = Convert.ToString(dtInsertedInterestScheme.Rows[0]["IntScheme_Default"]).ToLower().Trim() == "y" ? "1" : "0";
                hdnSaveId.Value = id;
                ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp();", true);
                cbpnlTest.JSProperties["cpStatus"] = "Show*" + Convert.ToString(dtInsertedInterestScheme.Rows[0]["IntScheme_Code"]) + "*" + Convert.ToString(dtInsertedInterestScheme.Rows[0]["IntScheme_Name"]);
            }
        }
        #endregion
        #region Button Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save(1);
        }
        protected void ibtn_Close(object sender, EventArgs e)
        {
            ViewState["ID"] = null;
        }
        #endregion
        #region Grid Events
        protected void gvInresetMaster_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvInresetMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInresetMaster.PageIndex = e.NewPageIndex;
        }
        protected void gvInresetMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "info")
            {
                //   LoadEditableData(Convert.ToInt32(e.CommandArgument));
            }
            else if (e.CommandName.ToLower() == "add")
            {
                txtIntSchemeCode.Enabled = true;
                txtIntSchemeCode.Text = "";
                txtIntSchemeName.Text = "";
                rbtnlstIsDefault.SelectedValue = "0";
                //LoadEditableData(Convert.ToInt32(e.CommandArgument));
                ScriptManager.RegisterStartupScript(this, GetType(), "showing", "ShowPopUp();", true);
            }
            else if (e.CommandName.ToLower() == "ed")
            {
                LoadEditableData(e.CommandArgument.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showing", "ShowPopUp();", true);
            }
            else if (e.CommandName.ToLower() == "del")
            {
                Delete(e.CommandArgument.ToString());
                //  ScriptManager.RegisterStartupScript(this, GetType(), "pdeletemsg", "PostDeleteMessage();", true);
            }
        }
        #endregion
    }
}