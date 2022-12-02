using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class root_UserGroup_POPUP : System.Web.UI.Page
    {
        string data;
        int rowcount = 0;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            string menuId = Request.QueryString["ID"];
            DataTable Dt_SubMenu = oDBEngine.GetDataTable(" tbl_trans_submenu ", " smu_id,smu_name,(select sac_accessType from tbl_trans_subaccess where sac_submenuid=smu_id) as Mode,'0' as menuParentID ", " smu_parentId=" + menuId);

            TLgrid.DataSource = Dt_SubMenu;
            TLgrid.DataBind();


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int NoOfRowsEffected = oDBEngine.DeleteValue(" tbl_trans_subaccess ", " sac_menuId=" + Request.QueryString["ID"]);
            List<TreeListNode> nodes = TLgrid.GetSelectedNodes();
            try
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    string nodekey = nodes[i]["smu_id"].ToString();
                    ASPxTextBox txttype = (ASPxTextBox)TLgrid.FindDataCellTemplateControl(nodekey, null, "ASPxTextBox1");
                    string val = txttype.Text;

                    NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_subaccess ", " sac_userGroupId,sac_menuId,sac_submenuId,sac_accessType,Createdate,CreateUser ", Session["KeyVal"].ToString() + "," + Request.QueryString["ID"] + "," + nodekey + ",'" + val + "',getdate()," + HttpContext.Current.Session["userid"]);
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "close", "<script language='javascript'> { window.close();}</script>");
            }
            catch (Exception exc)
            {

            }
        }

        protected void TLgrid_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            if (e.RowKind == TreeListRowKind.Data)
            {
                TreeListNode node = TLgrid.FindNodeByKeyValue(e.NodeKey.ToString());
                node.Selected = true;

            }
        }
    }
}