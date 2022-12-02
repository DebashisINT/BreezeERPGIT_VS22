using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_Frm_RiskProfileMember : System.Web.UI.Page
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (cmbProfile.Items.Count == 0)
            {
                cmbProfile.Items.Add(new ListItem("---ALL---", "0"));
            }
            BindGrid();
        }
        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbProfile.Items.Clear();
            cmbProfile.Items.Add(new ListItem("---ALL---", "0"));
            DataTable dtTrade = oDBEngine.GetDataTable("master_tradingprofile", "TradingProfile_Name,TradingProfile_Code", "TradingProfile_Type='" + cmbType.SelectedItem.Value + "'");
            for (int i = 0; i < dtTrade.Rows.Count; i++)
            {
                cmbProfile.Items.Add(new ListItem(dtTrade.Rows[i]["TradingProfile_Name"].ToString(), dtTrade.Rows[i]["TradingProfile_Code"].ToString()));

            }
        }

        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            //string[] CallVal = e.Parameters.ToString().Split('~');
            //// string tranid = e.Parameters.ToString();

            if (e.Parameters.ToString() == "s")
            {
                gridStatus.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                gridStatus.FilterExpression = string.Empty;
            }
            //else if (CallVal[0].ToString() == "Delete")
            //{

            //    oDBEngine.DeleteValue("Config_EmailAccounts ", "EmailAccounts_ID ='" + CallVal[1].ToString() + "'");
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
            //    fillGrid();

            //}
            //else if (CallVal[0].ToString() == "Access")
            //{
            //    DataTable dtStat = new DataTable();
            //    dtStat = oDBEngine.GetDataTable("Config_EmailAccounts ", " EmailAccounts_InUse  ", "EmailAccounts_ID ='" + CallVal[1].ToString().Trim() + "'");
            //    if (dtStat.Rows[0][0].ToString() == "N")
            //    {
            //        oDBEngine.SetFieldValue("Config_EmailAccounts ", "EmailAccounts_InUse='N' ", " EmailAccounts_CompanyID=(SELECT EmailAccounts_CompanyID FROM  config_emailAccounts WHERE  EmailAccounts_ID='" + CallVal[1].ToString() + "') AND EmailAccounts_SegmentID=(SELECT EmailAccounts_SegmentID FROM  config_emailAccounts WHERE  EmailAccounts_ID='" + CallVal[1].ToString() + "')  AND EmailAccounts_UsedFor=(SELECT EmailAccounts_UsedFor FROM  config_emailAccounts WHERE  EmailAccounts_ID='" + CallVal[1].ToString() + "') and EmailAccounts_ID !='" + CallVal[1].ToString() + "' ");


            //        oDBEngine.SetFieldValue("Config_EmailAccounts ", "EmailAccounts_InUse='Y'", "EmailAccounts_ID ='" + CallVal[1].ToString() + "'");



            //        fillGrid();

            //    }
            //    else
            //    {
            //        oDBEngine.SetFieldValue("Config_EmailAccounts ", "EmailAccounts_InUse='N'", "EmailAccounts_ID ='" + CallVal[1].ToString() + "'");
            //        fillGrid();
            //    }
            //}

        }
        protected void Procedure()
        {

            string WhereCond = " WHERE PROFILEMEMBER_CUSTOMERID IN (SELECT CNT_INTERNALID FROM TBL_MASTER_CONTACT WHERE CNT_BRANCHID IN (" + Session["userbranchHierarchy"].ToString() + "))";
            if (cmbType.SelectedItem.Value.ToString() != "0")
            {
                if (cmbProfile.SelectedItem.Value.ToString() != "0")
                {
                    WhereCond = WhereCond + "  AND  ProfileMember_Type='" + cmbType.SelectedItem.Value + "' and  ProfileMember_Code='" + cmbProfile.SelectedItem.Value.ToString().Trim() + "' ";
                }
                else
                {
                    WhereCond = WhereCond + "  AND  ProfileMember_Type='" + cmbType.SelectedItem.Value + "'";
                }
            }


            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT ProfileMember_ID,(SELECT TOP 1 ISNULL(CNT_FIRSTNAME,'')+ ''+ ISNULL(CNT_MIDDLENAME,'') +''+ISNULL(CNT_LASTNAME,'') +'['+RTRIM(LTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' FROM TBL_MASTER_CONTACT WHERE CNT_INTERNALID=ProfileMember_CustomerID) AS MEMBER_NAME, CASE WHEN ProfileMember_Type='1' THEN 'RISK' WHEN ProfileMember_Type='2' THEN 'DELIVERY' WHEN ProfileMember_Type='3' THEN 'FUND' ELSE '' END AS PROFILE_TYPE,ProfileMember_Code,(SELECT TRADINGPROFILE_NAME FROM master_tradingprofile WHERE TradingProfile_Code=ProfileMember_Code) AS  PROFILE_NAME,CONVERT(VARCHAR,ProfileMember_DateFrom,106) AS ProfileMember_DateFrom,CONVERT(VARCHAR,ProfileMember_DateTo,106) AS ProfileMember_DateTo FROM Trans_ProfileMember  " + WhereCond, con))
                {


                    da.SelectCommand.CommandType = CommandType.Text;
                    da.SelectCommand.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    // Mantis Issue 24802
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    // End of Mantis Issue 24802
                    ViewState["DatasetMain"] = ds;
                    BindGrid();
                }
            }

        }
        protected void BindGrid()
        {
            if (ViewState["DatasetMain"] != null)
            {
                DataSet dsNew = (DataSet)ViewState["DatasetMain"];
                gridStatus.DataSource = dsNew.Tables[0];
                gridStatus.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Height", "height();", true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Procedure();
            exporter.WriteXlsToResponse();
        }

    }
}