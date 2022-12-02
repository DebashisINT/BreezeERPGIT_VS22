using System;
using System.Web.UI;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_ShowPicture : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine("");
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {


            string FavID = Request.QueryString["id"].ToString();
            string ImageID = hdnID.Value.Substring(3);
            string ID = "../images/" + ImageID + ".png";
            int NoofRowsAffect = oDBEngine.SetFieldValue("Config_FavouriteMenu", " FavouriteMenu_Image='" + ID + "'", " FavouriteMenu_ID=" + FavID + "");
            if (NoofRowsAffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JS", "alert('Picture Change Successfully');", true);
            }
        }
    }
}