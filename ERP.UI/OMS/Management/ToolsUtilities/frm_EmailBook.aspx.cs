using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
//using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_EmailBook : System.Web.UI.Page
    {
        string whereClause = "";
        string userLeadId = "";
        DataTable dt = new DataTable();
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
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
            DataSourceContactMaster.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSourceEmails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            // Change Session Handling By Sudip on 15122016

            if (Session["userid"] != null)
            {
                if (IsCallback)
                {
                    populateMastergrid();
                }
                else if (!IsPostBack)
                {
                    cmbSearchType.SelectedIndex = 0;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        protected void populateMastergrid()
        {
            whereClause = "";
            whereClause = "TEmail.eml_cntId=tm.cnt_internalId and eml_type <> '' and eml_type <> 'Web Site' and tm.UserName is not null";
            if (drpFilterType.SelectedValue == "1")
            {
                if (txtKeyWord.Text.Trim() != "")
                {
                    if (AspxChkWhole.Checked == true)
                    {
                        whereClause = whereClause + " and tm.UserName = '" + txtKeyWord.Text.Trim().Replace("'", "`") + "'";
                    }
                    else
                    {
                        whereClause = whereClause + " and tm.UserName like '%" + txtKeyWord.Text.Trim().Replace("'", "`") + "%'";
                    }
                }

            }
            else
            {
                if (txtKeyWord.Text.Trim() != "")
                {
                    if (AspxChkWhole.Checked == true)
                    {
                        whereClause = whereClause + " and tm.cnt_shortName ='" + txtKeyWord.Text.Trim().Replace("'", "`") + "'";
                    }
                    else
                    {
                        whereClause = whereClause + " and tm.cnt_shortName like '%" + txtKeyWord.Text.Trim().Replace("'", "`") + "%'";
                    }

                }

            }

            DataSourceContactMaster.SelectCommand = "select distinct cnt_internalId,cnt_shortName,UserName from (select  (ISnull(cnt_firstName,'')+''+Isnull(cnt_middleName,'')+''+Isnull(cnt_lastName,'')) UserName,case when cnt_shortName is null then cnt_UCC else cnt_shortName end as cnt_shortName,cnt_internalId from tbl_master_contact " +
                                                    " union all select  (ISnull(cnt_firstName,'')+''+Isnull(cnt_middleName,'')+''+Isnull(cnt_lastName,'')) UserName,Isnull(cnt_internalId,'') cnt_shortName,cnt_internalId from tbl_master_Contact) tm,tbl_master_email TEmail where " + whereClause + "  order by UserName asc ";
            gridEmailMain.DataBind();
        }
        protected void gridEmailMain_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            populateMastergrid();
        }
        protected void SubGridMails_Load(object sender, EventArgs e)
        {
            ASPxGridView SubGridDetails = (ASPxGridView)sender;
            string userLeadId = ((DevExpress.Web.GridViewDetailRowTemplateContainer)(SubGridDetails.Parent)).KeyValue.ToString();
            SqlDataSourceEmails.SelectCommand = "select  distinct rptv.add_cntId,eml_id,eml_type,eml_email,eml_ccEmail from View_AddressReport rptv left outer join tbl_master_email Temail on  rptv.add_cntId=Temail.eml_cntId " +
                                              "where rptv.add_cntId='" + userLeadId + "' and eml_type <> '' and eml_type <> 'Web Site' order by rptv.add_cntId";
            SubGridDetails.DataBind();
        }
    }
}