using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_cdslBill_GroupSelection : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowList();
        }

        public void ShowList()
        {
            DataTable dt = oDBEngine.GetDataTable(" Master_ChargeGroup", "  ChargeGroup_Name as name,ChargeGroup_Code as code", " ChargeGroup_Type=3");

            chkGroupBox.DataTextField = "name";
            chkGroupBox.DataValueField = "code";
            chkGroupBox.DataSource = dt;
            chkGroupBox.DataBind();

            foreach (ListItem li in chkGroupBox.Items)
                li.Attributes.Add("mainValue", li.Value);

        }

        protected void GridGroup_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }
    }
}
