using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ServiceManagement.OMS.MasterPage.Services
{
    /// <summary>
    /// Summary description for Master
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
     [System.Web.Script.Services.ScriptService]
    public class Master : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetHelpHtmlByModName(string ModName)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable Dt = oDBEngine.GetDataTable("select HelpText from tbl_HelpMenu where ModName='" + ModName + "'");

            return Convert.ToString(Dt.Rows[0][0]);
        }
    }
}
