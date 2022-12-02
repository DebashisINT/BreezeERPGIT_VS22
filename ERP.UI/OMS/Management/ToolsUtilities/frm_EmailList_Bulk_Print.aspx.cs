using System;
using System.Data;
using System.Configuration;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_toolsutilities_frm_EmailList_Bulk_Print : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            bulkEmailList();
        }
        protected void bulkEmailList()
        {
            DataTable dt = oDBEngine.GetDataTable("(select cnt_internalId,(ISnull(cnt_firstName,'')+''+Isnull(cnt_middleName,'')+''+Isnull(cnt_lastName,'')) UserName,case when cnt_shortName is null then cnt_UCC else cnt_shortName end as shortName from tbl_master_contact union all select cnt_internalId,(ISnull(cnt_firstName,'')+''+Isnull(cnt_middleName,'')+''+Isnull(cnt_lastName,'')) UserName,cnt_shortName as shortName from tbl_master_contact) mt,tbl_master_email emain", "distinct cnt_internalid as Id,UserName,shortName,(select top 1 eml_email from tbl_master_email EP where eml_type='Personal' and EP.eml_type=emain.eml_type) as Per,(select top 1 eml_email from tbl_master_email EO where eml_type='official' and EO.eml_type=emain.eml_type) as Office ", " emain.eml_cntId=mt.cnt_internalId ");
            DataTable dtEmailList = new DataTable();
            dtEmailList.Columns.Add("Email", String.Empty.GetType());
            int count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string EmailD = "Name:" + dt.Rows[i]["UserName"].ToString() + "[" + dt.Rows[i]["shortName"].ToString() + "]" + "</br>" +
                                "EmailId(O):" + dt.Rows[i]["Office"].ToString() + "</br>" +
                                "EmailId(P):" + dt.Rows[i]["Per"].ToString();
                dtEmailList.Rows.Add(EmailD);
            }
            ASPxDataView1.DataSource = dtEmailList;
            ASPxDataView1.DataBind();
        }
    }
}