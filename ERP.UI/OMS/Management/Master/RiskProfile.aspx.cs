using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_RiskProfile : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine odbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine odbEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlRiskProfile.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    SqlRiskProfile.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlRiskProfile.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    SqlRiskProfile.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            gridRiskProfile.JSProperties["cpDelmsg"] = null;
        }
        protected void gridRiskProfile_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxComboBox comboProfile = (ASPxComboBox)gridRiskProfile.FindEditFormTemplateControl("comboProfile");
            ASPxTextBox txtCode = (ASPxTextBox)gridRiskProfile.FindEditFormTemplateControl("txtCode");
            ASPxTextBox txtName = (ASPxTextBox)gridRiskProfile.FindEditFormTemplateControl("txtName");
            e.NewValues["TradingProfile_Type"] = comboProfile.SelectedItem.Value;
            e.NewValues["TradingProfile_Code"] = txtCode.Text.ToString();
            e.NewValues["TradingProfile_Name"] = txtName.Text.ToString();
        }
        protected void gridRiskProfile_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            if (e.IsNewRow)
            {
                string Code = e.NewValues["TradingProfile_Code"].ToString();
                string Type = e.NewValues["TradingProfile_Type"].ToString();
                DataTable DT = odbEngine.GetDataTable("Master_TradingProfile", "TradingProfile_ID", " TradingProfile_Code='" + Code + "' and TradingProfile_Type='" + Type + "'");
                if (DT.Rows.Count > 0)
                {
                    e.RowError = "This Code Already Exists !";
                    return;
                }
            }
        }
        protected void gridRiskProfile_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string keyID = e.EditingKeyValue.ToString();
            ASPxComboBox comboProfile = (ASPxComboBox)gridRiskProfile.FindEditFormTemplateControl("comboProfile");
            ASPxTextBox txtCode = (ASPxTextBox)gridRiskProfile.FindEditFormTemplateControl("txtCode");
            comboProfile.Enabled = false;
            txtCode.Enabled = false;
            DataTable dtProf = odbEngine.GetDataTable("Master_TradingProfile", "TradingProfile_Type", " TradingProfile_ID='" + keyID + "'");
            if (dtProf.Rows.Count > 0)
            {
                comboProfile.Value = dtProf.Rows[0][0].ToString();
            }
        }
        protected void gridRiskProfile_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxTextBox txtName = (ASPxTextBox)gridRiskProfile.FindEditFormTemplateControl("txtName");
            e.NewValues["TradingProfile_Name"] = txtName.Text.ToString();
        }
        protected void gridRiskProfile_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (Convert.ToString(e.Values["TradingProfile_Code"]) != "")
            {
                string tradeProfileId = e.Values["TradingProfile_Code"].ToString().Trim();
                SqlDataReader objReader = odbEngine.GetReader("select RiskProfile_Code from Config_RiskProfile where RiskProfile_Code='" + tradeProfileId + "' union select DeliveryProfile_Code from Config_DeliveryProfile where DeliveryProfile_Code='" + tradeProfileId + "' union select FundProfile_Code from Config_FundProfile where FundProfile_Code='" + tradeProfileId + "'");
                if (objReader.HasRows)
                {
                    gridRiskProfile.JSProperties["cpDelmsg"] = "Cannot Delete. This ProfileTrade Code Is In Use";
                    e.Cancel = true;
                }
                else
                {
                    gridRiskProfile.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                }
                odbEngine.CloseConnection();
                objReader.Close();
            }
        }

    }
}