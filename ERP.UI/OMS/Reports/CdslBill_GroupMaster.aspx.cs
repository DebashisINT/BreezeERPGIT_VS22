using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_CdslBill_GroupMaster : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowList();
        }

        public void ShowList()
        {
            DataTable dt = oDBEngine.GetDataTable(" tbl_master_groupmaster", " gpm_Description,gpm_id", " gpm_Type='" + Request.QueryString["type"].ToString() + "'");

            chkGroupBox.DataTextField = "gpm_Description";
            chkGroupBox.DataValueField = "gpm_id";
            chkGroupBox.DataSource = dt;
            chkGroupBox.DataBind();
            foreach (ListItem li in chkGroupBox.Items)
                li.Attributes.Add("mainValue", li.Value);


        }

    }
}
