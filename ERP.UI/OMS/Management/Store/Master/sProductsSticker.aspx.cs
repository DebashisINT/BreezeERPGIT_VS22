using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Text;
using System.Collections.Generic;
using System.Resources;
using System.Collections;
using System.IO;
using EntityLayer.CommonELS;
using System.Drawing;
using System.Linq;
using System.Configuration;
using DataAccessLayer;
using System.Web.Script.Services;

namespace ERP.OMS.Management.Store.Master
{
    public partial class sProductsSticker : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sProductsSticker.aspx");
        }
    }
}