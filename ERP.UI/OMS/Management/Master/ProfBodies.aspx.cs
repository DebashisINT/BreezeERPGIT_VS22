using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_ProfBodies : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'

                //Purpose : Replace .ToString() with Convert.ToString(..)
                //Name : Sudip 
                // Dated : 21-12-2016

                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/ProfBodies.aspx");
            Sqlprof.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
             if (HttpContext.Current.Session["userid"]!= null)
             {
                 if (!IsPostBack)
                 {
                     Session["exportval"] = null;
                 }
             }
              else
             {
                 Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
             }
        }
        protected void gridProf_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            
            gridProf.DataBind();
            if (e.Parameters == "s")
                gridProf.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                gridProf.FilterExpression = string.Empty;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
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
            gridProf.Columns[4].Visible = false;
            string filename = "Professional/Technical Bodies";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Professional/Technical Bodies";
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
        protected void gridProf_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
    }
}