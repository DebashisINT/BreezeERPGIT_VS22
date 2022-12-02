using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using BusinessLogicLayer.IMEI;
using System.Web.Services;
using BusinessLogicLayer.WebMacAccess;


namespace ERP.OMS.Management.Master
{
    public partial class WebmacAccess : ERP.OMS.ViewState_class.VSPage
    {

        Webmacaccess objimei = new Webmacaccess();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public  EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();


        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {

                Session["exportval"] = null;
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/WebMacAccess.aspx");
        
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //if (!IsPostBack)
            //{
            BindGrid();
            //}
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        protected void BindGrid()
        {
            DataTable dt = objimei.GetListofImei();
            gridtabimei.DataSource = dt;
            gridtabimei.DataBind();

        }
       
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                 //   Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }

        public void bindexport(int Filter)
        {
          //  gridtabimei.Columns[2].Visible = false;

            string filename = "Web Mac";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "User Mac";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }


        [WebMethod]
        public static bool DeleteWebmac(int mac)
        {
         
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                int i = Webmacaccess.Deletemei(mac, "Delete");
                if(i>0)
                {
                    return true;

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }
    }


}
