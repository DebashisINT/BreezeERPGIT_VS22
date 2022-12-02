using System;
using System.Data;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using System.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_IframeTopics : ERP.OMS.ViewState_class.VSPage //SSystem.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string RVal = "I~0";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), "HeightL", "<script language='JavaScript'>height();</script>");

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            SqlTopics.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlTopics.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    SqlTopics.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlTopics.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    SqlTopics.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

        }
        protected void gridTopics_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string keyVal = e.EditingKeyValue.ToString();
            RVal = "I~1";
            DataTable dtTopic = oDBEngine.GetDataTable("Master_Topics", " Topics_Status", " Topics_Status='B' and Topics_Code='" + keyVal + "'");
            if (dtTopic.Rows.Count > 0)
            {
                RVal = "I~2";
            }
            //gridTopics.Columns.RemoveAt(3);
        }
        protected void gridTopics_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpFetch"] = RVal;
        }
        protected void gridTopics_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (!gridTopics.IsNewRowEditing)
            {
                if (RVal == "I~2")
                {
                    ASPxGridViewTemplateReplacement RT = gridTopics.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                    RT.Visible = false;
                }
            }
        }
    }
}