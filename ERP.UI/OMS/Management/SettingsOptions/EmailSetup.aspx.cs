using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_EmailSetup : System.Web.UI.Page
    {
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            gridStatusDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/SettingsOptions/EmailSetup.aspx");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            fillGrid();
        }
        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            string[] CallVal = e.Parameters.ToString().Split('~');
            // string tranid = e.Parameters.ToString();

            if (CallVal[0].ToString() == "s")
            {
                gridStatus.Settings.ShowFilterRow = true;
            }
            else if (CallVal[0].ToString() == "All")
            {
                gridStatus.FilterExpression = string.Empty;
            }
            else if (CallVal[0].ToString() == "Delete")
            {

                oDBEngine.DeleteValue("Config_EmailAccounts ", "EmailAccounts_ID ='" + CallVal[1].ToString() + "'");
                this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
                fillGrid();

            }
            else if (CallVal[0].ToString() == "Access")
            {
                DataTable dtStat = new DataTable();
                dtStat = oDBEngine.GetDataTable("Config_EmailAccounts ", " EmailAccounts_InUse  ", "EmailAccounts_ID ='" + CallVal[1].ToString().Trim() + "'");
                if (dtStat.Rows[0][0].ToString() == "N")
                {
                    oDBEngine.SetFieldValue("Config_EmailAccounts ", "EmailAccounts_InUse='N' ", " EmailAccounts_CompanyID=(SELECT EmailAccounts_CompanyID FROM  config_emailAccounts WHERE  EmailAccounts_ID='" + CallVal[1].ToString() + "') AND EmailAccounts_SegmentID=(SELECT EmailAccounts_SegmentID FROM  config_emailAccounts WHERE  EmailAccounts_ID='" + CallVal[1].ToString() + "')  AND EmailAccounts_UsedFor=(SELECT EmailAccounts_UsedFor FROM  config_emailAccounts WHERE  EmailAccounts_ID='" + CallVal[1].ToString() + "') and EmailAccounts_ID !='" + CallVal[1].ToString() + "' ");


                    oDBEngine.SetFieldValue("Config_EmailAccounts ", "EmailAccounts_InUse='Y'", "EmailAccounts_ID ='" + CallVal[1].ToString() + "'");



                    fillGrid();

                }
                else
                {
                    oDBEngine.SetFieldValue("Config_EmailAccounts ", "EmailAccounts_InUse='N'", "EmailAccounts_ID ='" + CallVal[1].ToString() + "'");
                    fillGrid();
                }
            }

        }
        public void fillGrid()
        {
            gridStatusDataSource.SelectCommand = "select EmailAccounts_ID,(select cmp_name from tbl_master_company where cmp_internalid=EmailAccounts_CompanyID) as Company,(select seg_name from tbl_master_segment where seg_id= EmailAccounts_SegmentID) as Segment,EmailAccounts_EmailID,Case when EmailAccounts_UsedFor='N' then 'Normal' when EmailAccounts_UsedFor='S' then 'Self Service' when EmailAccounts_UsedFor='E' then 'ECN Email' when EmailAccounts_UsedFor='B' then 'Bulk Email'  end  as EmailType ,EmailAccounts_Password,EmailAccounts_SMTP,EmailAccounts_SMTPPort,EmailAccounts_POP,EmailAccounts_POPPort,EmailAccounts_ReplyToAccount,EmailAccounts_Disclaimer,case when EmailAccounts_InUse='Y' then 'Active' else 'Deactive' end as ActiveInd,EmailAccounts_CreateUser,EmailAccounts_CreateDateTime,EmailAccounts_ModifyUser,EmailAccounts_ModifyDateTime,EmailAccounts_SSLMode  from config_emailAccounts";
            gridStatus.DataBind();

        }
    }
}