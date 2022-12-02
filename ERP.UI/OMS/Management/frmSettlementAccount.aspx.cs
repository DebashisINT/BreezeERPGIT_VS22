using System;
using System.Web;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_frmSettlementAccount : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {

            String setno = HttpContext.Current.Session["LastSettNo"].ToString();
            oDBEngine.GetReader("sp_DeliveryPosition '" + setno + "','N','" + HttpContext.Current.Session["LastFinYear"] + "','" + HttpContext.Current.Session["userlastsegment"] + "','" + HttpContext.Current.Session["userid"] + "'");
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            // oDBEngine.GetReader("sp_DeliveryPosition '" + HttpContext.Current.Session["LastSettNo"] + "','N','" + HttpContext.Current.Session["LastFinYear"] + "','"+ HttpContext.Current.Session["userlastsegment"] +"','1'");
            // oDBEngine.GetReader("sp_DeliveryPosition '2009121','N','2009-2010','14','1'");
            //oDBEngine.GetReader("sp_DeliveryPosition '2009121','N','" + HttpContext.Current.Session["LastFinYear"] + "','14','1'");
        }
    }
}