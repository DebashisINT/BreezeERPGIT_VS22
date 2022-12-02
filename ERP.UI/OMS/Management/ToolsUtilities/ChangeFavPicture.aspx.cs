using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Web;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_ChangeFavPicture : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine("");
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckUserSession(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPicture();
            }
        }
        public void BindPicture()
        {
            DataTable dtFavourite = oDBEngine.GetDataTable("tbl_trans_menu,Config_FavouriteMenu", "mnu_menuname,mnu_menuLink,FavouriteMenu_ID,FavouriteMenu_Image", " FavouriteMenu_MenuID=mnu_id and FavouriteMenu_Segment=mnu_segmentID and FavouriteMenu_UserID=" + Session["userid"].ToString() + " and FavouriteMenu_Segment=" + Session["userlastsegment"].ToString() + "", "FavouriteMenu_Order");
            grdUpdateFavMenu.DataSource = dtFavourite;
            grdUpdateFavMenu.DataBind();
            Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>height()</script>");
        }
        protected void grdUpdateFavMenu_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string FavID = grdUpdateFavMenu.DataKeys[e.RowIndex].Value.ToString();
            oDBEngine.DeleteValue("Config_FavouriteMenu", " FavouriteMenu_ID=" + FavID + "");
            BindPicture();
        }
        protected void btnChangePic_Click(object sender, EventArgs e)
        {
            BindPicture();
        }
    }
}
