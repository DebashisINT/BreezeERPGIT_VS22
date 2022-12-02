using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

using System.Web.Services;
using BusinessLogicLayer.SalesmanAddress;



namespace ERP.OMS.Management.Master
{

    public partial class Salesman_AddressList : ERP.OMS.ViewState_class.VSPage
    {
        SalesmanAddress objusraddress = new SalesmanAddress();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        public  EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);

                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Salesman-AddressList.aspx");
           
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
     
            BindGrid();
         
        }

        protected void BindGrid()
        {
            DataTable dt = objusraddress.GetListofUserAddress();
            gridtabusraddress.DataSource = dt;
            gridtabusraddress.DataBind();

        }
       
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                   /// Session["exportval"] = Filter;
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

            string filename = "User Home Address";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "User Home Address";
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
        public static bool DeleteImei(int Addid)
        {
         
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                int i = SalesmanAddress.DeletAddress(Addid, "Delete");
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
